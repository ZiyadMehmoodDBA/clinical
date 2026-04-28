using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.Reports;
using MDVision.Datasets;
using MDVision.Model.Clinical.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Model.CCM.Reports;
using MDVision.Model.Report;

namespace MDVision.Business.BLL
{
    public class BLLReports
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminProfile"/> class.
        /// </summary>
        public BLLReports()
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call

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

        public String GetReports_DetailsHTML(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Report_AdvancePayment.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    // String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: #468AEA;vertical-align: top;text-align: left;direction: ltr;min-width: 28.61mm;width: 30.72mm;'>";
                    String thColumnEnd = "</th>";
                    String HeadingColumnDiv = "<div style='word-wrap: break-word;white-space: pre-wrap;font-style: normal;font-family: Arial;font-size: 10pt;font-weight: 400;text-decoration: none;unicode-bidi: normal;color: #FFF;direction: ltr;vertical-align: top;text-align: left;'>";
                    thColumnStart = thColumnStart + HeadingColumnDiv;
                    thColumnEnd = thColumnEnd + "</div>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String FootercolumnBoldStart = "<td colspan='5' align='center' style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: transparent;vertical-align: top;text-align: left;direction: ltr;'><b>";
                    String FootercolumnBoldStart1 = "<td colspan='4' align='center' style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: transparent;vertical-align: top;text-align: left;direction: ltr;'><b>";
                    String FootercolumnBoldStartDollar = "<td style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: transparent;vertical-align: top;text-align: right;direction: ltr;'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String ColumnStart = "<td style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: transparent;vertical-align: top;text-align: left;direction: ltr;min-width: 28.61mm;width: 30.72mm;'>";
                    String ColumnStartDollar = "<td style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: transparent;vertical-align: top;text-align: right;direction: ltr;'>$";
                    String ColumnEnd = "</td>";
                    string ColumnDiv = "<div style=' word-wrap: break-word;white-space: pre-wrap;font-style: normal;font-family: Arial;font-size: 8pt;font-weight: 400;text-decoration: none;unicode-bidi: normal;color: #000;direction: ltr;vertical-align: top;text-align: left;'>";
                    ColumnStart = ColumnStart + ColumnDiv;
                    ColumnEnd = ColumnEnd + "</div>";
                    String TableHeading = TableHead + "<th class='noWordBreak'>" + "Account" + "</th>" +
                        "<th class='noWordBreak' >" + "Patient Name" + "</th>" +
                        "<th class='noWordBreak'>" + "Facility " + "</th>" +
                        "<th class='noWordBreak'>" + "Practice" + "</th>" +
                        "<th class='noWordBreak'>" + "Date Paid" + "</th>" +
                        "<th class='noWordBreak'>" + "Amt.Paid" + "</th>" +
                        "<th class='noWordBreak'>" + "Payment Type" + "</th>" +
                        "<th class='noWordBreak'>" + "Chk/Cc" + "</th>" +
                        "<th class='noWordBreak'>" + "Apply To" + "</th>" +
                        "<th class='noWordBreak'>" + "Description" + "</th>" +
                        "<th class='noWordBreak'>" + "Advance Bal." + "</th>" + TableHeadEnd;



                    sb.Append(PrintDivStart);
                    // sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double Fee = 0;
                    double AmtPaid = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Report_AdvancePayment.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(
                            "<td>" + dr[dsReports.DT_Report_AdvancePayment.AccountNumberColumn] + "</td>");
                        sb.Append("<td>" + dr[dsReports.DT_Report_AdvancePayment.PatientNameColumn] + "</td>");
                        sb.Append("<td>" + dr[dsReports.DT_Report_AdvancePayment.FacilityNameColumn] + "</td>");
                        sb.Append("<td>" + dr[dsReports.DT_Report_AdvancePayment.PracticeNameColumn] + "</td>");
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Report_AdvancePayment.DatePaidColumn].ToString()))
                        {
                            sb.Append("<td>" + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Report_AdvancePayment.DatePaidColumn].ToString()) + "</td>");//
                        }
                        else
                        {
                            sb.Append("<td>" + dr[dsReports.DT_Report_AdvancePayment.DatePaidColumn] + "</td>");//
                        }
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_AdvancePayment.AmountPaidColumn])) + "</td>");
                        sb.Append("<td>" + dr[dsReports.DT_Report_AdvancePayment.PaymentTypeColumn] + "</td>");
                        sb.Append("<td>" + dr[dsReports.DT_Report_AdvancePayment.CheckNumberColumn] + "</td>");
                        sb.Append("<td>" + dr[dsReports.DT_Report_AdvancePayment.ApplyToColumn] + "</td>");
                        sb.Append("<td>" + dr[dsReports.DT_Report_AdvancePayment.DescriptionColumn] + "</td>");
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_AdvancePayment.AdvanceBalanceColumn])) + "</td>");

                        sb.Append(RowEnd);
                        Fee += MDVUtility.ToDouble(dr[dsReports.DT_Report_AdvancePayment.AdvanceBalanceColumn]);
                        AmtPaid += MDVUtility.ToDouble(dr[dsReports.DT_Report_AdvancePayment.AmountPaidColumn]);
                    }
                    Fee = MDVUtility.ToAmount(Fee);
                    AmtPaid = MDVUtility.ToAmount(AmtPaid);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AmtPaid) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Fee) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }
        }
        #region Patient Reports
        public String LoadAdvancePaymentsReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Report_AdvancePayment.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='9' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + thColumnStart + "Account" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Facility " + thColumnEnd +
                        thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Date Paid" + thColumnEnd +
                        thColumnStart + "Payment " + thColumnEnd +
                        thColumnStart + "Check Number" + thColumnEnd +
                        thColumnStart + "Apply To" + thColumnEnd +
                        thColumnStart + "Description" + thColumnEnd +
                        thColumnStart + "Advance Balance" + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double Fee = 0;

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Report_AdvancePayment.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_AdvancePayment.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_AdvancePayment.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_AdvancePayment.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_AdvancePayment.PracticeNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Report_AdvancePayment.DatePaidColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Report_AdvancePayment.DatePaidColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Report_AdvancePayment.DatePaidColumn] + ColumnEnd);//
                        }

                        sb.Append(ColumnStart + dr[dsReports.DT_Report_AdvancePayment.PaymentTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_AdvancePayment.CheckNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_AdvancePayment.ApplyToColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_AdvancePayment.DescriptionColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + dr[dsReports.DT_Report_AdvancePayment.AdvanceBalanceColumn] + ColumnEnd);

                        sb.Append(RowEnd);

                        Fee += MDVUtility.ToDouble(dr[dsReports.DT_Report_AdvancePayment.AdvanceBalanceColumn]);
                    }
                    Fee = MDVUtility.ToAmount(Fee);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + Fee + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }

        public String GetDiagnosisAnalysisReports(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Report_DiagnosisAnalysis.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + thColumnStart + "Year" + thColumnEnd +
                        thColumnStart + "Account" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "ICD Code " + thColumnEnd +
                        thColumnStart + "ICD Code Description" + thColumnEnd +
                        thColumnStart + "ICD Code 1" + thColumnEnd +
                        thColumnStart + "Q1 " + thColumnEnd +
                        thColumnStart + "Q2" + thColumnEnd +
                        thColumnStart + "Q3" + thColumnEnd +
                        thColumnStart + "Q4" + thColumnEnd +
                        thColumnStart + "ICD Code 2" + thColumnEnd +
                        thColumnStart + "Q1" + thColumnEnd +
                        thColumnStart + "Q2" + thColumnEnd +
                        thColumnStart + "Q3" + thColumnEnd +
                        thColumnStart + "Q4" + thColumnEnd +
                        thColumnStart + "ICD Code 3" + thColumnEnd +
                        thColumnStart + "Q1" + thColumnEnd +
                        thColumnStart + "Q2" + thColumnEnd +
                        thColumnStart + "Q3" + thColumnEnd +
                        thColumnStart + "Q4" + thColumnEnd +
                        thColumnStart + "ICD Code 4" + thColumnEnd +
                        thColumnStart + "Q1" + thColumnEnd +
                        thColumnStart + "Q2" + thColumnEnd +
                        thColumnStart + "Q3" + thColumnEnd +
                        thColumnStart + "Q4" + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Report_DiagnosisAnalysis.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.YearRangeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICDCOdeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICDCodeDescriptionColumn] + ColumnEnd);


                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICDCode1Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD1Quarter1Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD1Quarter2Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD1Quarter3Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD1Quarter4Column] + ColumnEnd);

                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICDCode2Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD2Quarter1Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD2Quarter2Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD2Quarter3Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD2Quarter4Column] + ColumnEnd);

                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICDCode3Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD3Quarter1Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD3Quarter2Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD3Quarter3Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD3Quarter4Column] + ColumnEnd);

                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICDCode4Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD4Quarter1Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD4Quarter2Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD4Quarter3Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_DiagnosisAnalysis.ICD4Quarter4Column] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }

        public String LoadCollectedCopaymentReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_CollectedCopayment.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String FootercolumnBoldStart = "<td colspan='10' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + thColumnStart + "Practice " + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider  " + thColumnEnd +
                        thColumnStart + "Plan" + thColumnEnd +
                        thColumnStart + "Plan Category" + thColumnEnd +
                        thColumnStart + "Account " + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Claim #" + thColumnEnd +
                        thColumnStart + "Visit Date" + thColumnEnd +
                        thColumnStart + "Payment Date" + thColumnEnd +
                        thColumnStart + "Charge Amount" + thColumnEnd +
                        thColumnStart + "Paid Amount" + thColumnEnd +
                        thColumnStart + "Discount Amount" + thColumnEnd +
                        thColumnStart + "Balance" + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double ChrgCopay = 0;
                    double CopayPaidAmt = 0;
                    double CopayDiscuntAmt = 0;
                    double CopayBal = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_CollectedCopayment.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CollectedCopayment.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CollectedCopayment.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CollectedCopayment.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CollectedCopayment.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CollectedCopayment.PlanCategoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CollectedCopayment.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CollectedCopayment.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CollectedCopayment.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_CollectedCopayment.VisitDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_CollectedCopayment.VisitDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_CollectedCopayment.VisitDateColumn] + ColumnEnd);//
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_CollectedCopayment.PaymentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_CollectedCopayment.PaymentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_CollectedCopayment.PaymentDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_CollectedCopayment.CopayChargeAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_CollectedCopayment.CopayPaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_CollectedCopayment.CopayDiscountAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_CollectedCopayment.CopayBalanceColumn])) + ColumnEnd);
                        sb.Append(RowEnd);
                        ChrgCopay += MDVUtility.ToDouble(dr[dsReports.DT_Reports_CollectedCopayment.CopayChargeAmountColumn]);
                        CopayPaidAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_CollectedCopayment.CopayPaidAmountColumn]);
                        CopayDiscuntAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_CollectedCopayment.CopayDiscountAmountColumn]);
                        CopayBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_CollectedCopayment.CopayBalanceColumn]);

                    }

                    ChrgCopay = MDVUtility.ToAmount(ChrgCopay);
                    CopayPaidAmt = MDVUtility.ToAmount(CopayPaidAmt);
                    CopayDiscuntAmt = MDVUtility.ToAmount(CopayDiscuntAmt);
                    CopayBal = MDVUtility.ToAmount(CopayBal);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", ChrgCopay) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", CopayPaidAmt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", CopayDiscuntAmt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", CopayBal) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }

        public String LoadUnallocatedCopaymentReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Unallocated_Copayment.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String FootercolumnBoldStart = "<td colspan='8' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead +
                        thColumnStart + "Practice " + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider  " + thColumnEnd +
                        thColumnStart + "Plan" + thColumnEnd +
                        thColumnStart + "Account " + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Visit Date" + thColumnEnd +
                        thColumnStart + "Payment Date" + thColumnEnd +
                        thColumnStart + "Copay Amount" + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double ChrgCopay = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Unallocated_Copayment.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unallocated_Copayment.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unallocated_Copayment.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unallocated_Copayment.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unallocated_Copayment.InsPLanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unallocated_Copayment.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unallocated_Copayment.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Unallocated_Copayment.AppointmentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Unallocated_Copayment.AppointmentDateColumn].ToString()) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Unallocated_Copayment.AppointmentDateColumn] + ColumnEnd);
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Unallocated_Copayment.CreatedOnColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Unallocated_Copayment.CreatedOnColumn].ToString()) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Unallocated_Copayment.CreatedOnColumn] + ColumnEnd);
                        }
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Unallocated_Copayment.CopayAmountColumn])) + ColumnEnd);
                        sb.Append(RowEnd);
                        ChrgCopay += MDVUtility.ToDouble(dr[dsReports.DT_Unallocated_Copayment.CopayAmountColumn]);
                    }

                    ChrgCopay = MDVUtility.ToAmount(ChrgCopay);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", ChrgCopay) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadUnallocatedCopaymentReport", ex);
                return ex.Message;
            }


        }
        public String LoadARAging_AnalysisReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_ARAging_Analysis.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='8' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</td></b>";
                    String TableHeading = TableHead
                         + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='7' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Insurance" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='5'>" + "Insurance AR" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Patient" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='5'>" + "Patient AR" + thColumnEnd + RowEnd +
                        RowStart
                        + thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider " + thColumnEnd +
                        thColumnStart + "Claim Number " + thColumnEnd +
                        thColumnStart + "Account Number " + thColumnEnd +
                        thColumnStart + "Claim Status " + thColumnEnd +
                        thColumnStart + "DOS" + thColumnEnd +
                        thColumnStart + "Plan" + thColumnEnd +
                        thColumnStart + "Billed Charges" + thColumnEnd +
                        thColumnStart + "Paid Amount " + thColumnEnd +
                        thColumnStart + "Write Off" + thColumnEnd +
                        thColumnStart + "AR" + thColumnEnd +
                        thColumnStart + "Current" + thColumnEnd +
                        thColumnStart + "AR 30+" + thColumnEnd +
                        thColumnStart + "AR 60+ " + thColumnEnd +
                        thColumnStart + "AR 90+" + thColumnEnd +
                        thColumnStart + "AR 120+" + thColumnEnd +
                        //thColumnStart + "Charges" + thColumnEnd +
                        thColumnStart + "Paid Amount" + thColumnEnd +
                        thColumnStart + "Discount" + thColumnEnd +
                        thColumnStart + "Total AR" + thColumnEnd +
                        thColumnStart + "ARCurrent" + thColumnEnd +
                        thColumnStart + "AR 30+" + thColumnEnd +
                         thColumnStart + "AR 60+" + thColumnEnd +
                        thColumnStart + "AR 90+" + thColumnEnd +
                        thColumnStart + "AR 120+" + thColumnEnd +
                        thColumnStart + "Total AR" + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double InsCharges = 0;
                    double InsPaidAmount = 0;
                    double InsWriteOff = 0;
                    double TotalAR = 0;
                    double TotalARCurrent = 0;
                    double AR30 = 0;
                    double AR60 = 0;
                    double AR90 = 0;
                    double AR120 = 0;
                    double PatCharges = 0;
                    double PatPaidAmount = 0;
                    double Discount = 0;
                    double PatTotalAR = 0;
                    double PatTotalARCurrent = 0;
                    double PatAR30 = 0;
                    double PatAR60 = 0;
                    double PatAR90 = 0;
                    double PatAR120 = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_ARAging_Analysis.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.SubmitStatusColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_ARAging_Analysis.DOSColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_ARAging_Analysis.DOSColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.DOSColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.PlanColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.BilledChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.InsurancePaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.InsuranceWriteOffColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARCurrentColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR30_Column])) + ColumnEnd);

                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR60_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR90_Column])) + ColumnEnd);

                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR120_Column])) + ColumnEnd);

                        //sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.InsuranceChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.PatientPaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.PatientDiscountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalPatientARColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.PatientARCurrentColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR30_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR60_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR90_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR120_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalARColumn])) + ColumnEnd);

                        sb.Append(RowEnd);

                        InsCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.BilledChargesColumn]);
                        InsPaidAmount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.InsurancePaidAmountColumn]);
                        InsWriteOff += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.InsuranceWriteOffColumn]);
                        TotalAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARColumn]);
                        TotalARCurrent += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARCurrentColumn]);
                        AR30 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR30_Column]);
                        AR60 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR60_Column]);
                        AR90 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR90_Column]);
                        AR120 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR120_Column]);
                        //PatCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientChargesColumn]);
                        PatPaidAmount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientPaidAmountColumn]);
                        Discount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientDiscountColumn]);
                        PatTotalAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalPatientARColumn]);
                        PatTotalARCurrent += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientARCurrentColumn]);
                        PatAR30 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR30_Column]);
                        PatAR60 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR60_Column]);
                        PatAR90 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR90_Column]);
                        PatAR120 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR120_Column]);
                        PatCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalARColumn]);
                    }
                    InsCharges = MDVUtility.ToAmount(InsCharges);
                    InsPaidAmount = MDVUtility.ToAmount(InsPaidAmount);
                    InsWriteOff = MDVUtility.ToAmount(InsWriteOff);
                    TotalAR = MDVUtility.ToAmount(TotalAR);
                    TotalARCurrent = MDVUtility.ToAmount(TotalARCurrent);
                    AR30 = MDVUtility.ToAmount(AR30);
                    AR60 = MDVUtility.ToAmount(AR60);
                    AR90 = MDVUtility.ToAmount(AR90);
                    AR120 = MDVUtility.ToAmount(AR120);
                    PatCharges = MDVUtility.ToAmount(PatCharges);
                    PatPaidAmount = MDVUtility.ToAmount(PatPaidAmount);
                    Discount = MDVUtility.ToAmount(Discount);
                    PatTotalAR = MDVUtility.ToAmount(PatTotalAR);
                    PatTotalARCurrent = MDVUtility.ToAmount(PatTotalARCurrent);
                    PatAR30 = MDVUtility.ToAmount(PatAR30);
                    PatAR60 = MDVUtility.ToAmount(PatAR60);
                    PatAR90 = MDVUtility.ToAmount(PatAR90);
                    PatAR120 = MDVUtility.ToAmount(PatAR120);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsCharges) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsPaidAmount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsWriteOff) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", TotalAR) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", TotalARCurrent) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR30) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR60) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR90) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR120) + FootercolumnBoldEnd);
                    //sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatCharges) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatPaidAmount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Discount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatTotalAR) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatTotalARCurrent) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR30) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR60) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR90) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR120) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatCharges) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }


        public String LoadInsuranceAnalysisReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_InsuranceAnalysis.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String FootercolumnBoldStart = "<td colspan='10' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Practice " + thColumnEnd +
                        thColumnStart + "Insurance" + thColumnEnd +
                        thColumnStart + "Plan" + thColumnEnd +
                        thColumnStart + "Account " + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Registration Date" + thColumnEnd +
                        thColumnStart + "Home Phone" + thColumnEnd +
                        thColumnStart + "Subscriber ID" + thColumnEnd +
                        thColumnStart + "Ins Balance" + thColumnEnd +
                        thColumnStart + "Patient Balance" + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    double InsBal = 0;
                    double PatBal = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_InsuranceAnalysis.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.InsuranceNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.InsurancePlanNameColumn] + ColumnEnd);

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_InsuranceAnalysis.RegistrationDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_InsuranceAnalysis.RegistrationDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.RegistrationDateColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.RegistrationDateColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.PatientTelColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysis.SubscriberIdColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsuranceAnalysis.InsBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsuranceAnalysis.PatientBalanceColumn])) + ColumnEnd);

                        sb.Append(RowEnd);

                        InsBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsuranceAnalysis.InsBalanceColumn]);
                        PatBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsuranceAnalysis.PatientBalanceColumn]);
                    }
                    InsBal = MDVUtility.ToAmount(InsBal);
                    PatBal = MDVUtility.ToAmount(PatBal);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatBal) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }

        public String LoadAInsuranceAnalysisSummaryReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_InsuranceAnalysisSummary.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='5' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + thColumnStart + "Provider" + thColumnEnd +

                        thColumnStart + "Facility " + thColumnEnd +
                        thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Insurance" + thColumnEnd +
                        thColumnStart + "Plan " + thColumnEnd +
                        thColumnStart + "Plan Balance" + thColumnEnd +
                        thColumnStart + "Patient Balance" + thColumnEnd +
                         TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double InsBal = 0;
                    double PatBal = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_InsuranceAnalysisSummary.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysisSummary.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysisSummary.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysisSummary.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysisSummary.InsuranceNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsuranceAnalysisSummary.InsurancePlanNameColumn] + ColumnEnd);
                        //sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsuranceAnalysisSummary.InsBalanceColumn])) + ColumnEnd);
                        //sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsuranceAnalysisSummary.PatientBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsuranceAnalysisSummary.InsBalanceColumn]) >= 0 ? String.Format("{0:#######0.00}", MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsuranceAnalysisSummary.InsBalanceColumn])) : String.Format("({0:#######0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsuranceAnalysisSummary.InsBalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartDollar + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsuranceAnalysisSummary.PatientBalanceColumn]) >= 0 ? String.Format("{0:#######0.00}", MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsuranceAnalysisSummary.PatientBalanceColumn])) : String.Format("({0:#######0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsuranceAnalysisSummary.PatientBalanceColumn])))) + ColumnEnd);
                        sb.Append(RowEnd);
                        InsBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsuranceAnalysisSummary.InsBalanceColumn]);
                        PatBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsuranceAnalysisSummary.PatientBalanceColumn]);
                    }
                    InsBal = MDVUtility.ToAmount(InsBal);
                    PatBal = MDVUtility.ToAmount(PatBal);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatBal) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }

        public String LoadOutStandingBalancesReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_OutStandingBalances.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStartWithHeader = "<th class='noWordBreak'>";
                    String thColumnStart = "<th class='noWordBreak' style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: #468AEA;vertical-align: top;text-align: left;direction: ltr;min-width: 28.61mm;width: 30.72mm;'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: transparent;vertical-align: top;text-align: left;direction: ltr;min-width: 28.61mm;width: 30.72mm;'>";
                    String ColumnStartDollar = "<td style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: transparent;vertical-align: top;text-align: right;direction: ltr;min-width: 28.61mm;width: 30.72mm;'>$";
                    String FootercolumnBoldStart = "<td colspan='7' align='center' style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: transparent;vertical-align: top;text-align: left;direction: ltr;min-width: 28.61mm;width: 30.72mm;'><b>";
                    String FootercolumnBoldStartDollar = "<td style='padding: 2pt;border: 1pt solid #D3D3D3;background-color: transparent;vertical-align: top;text-align: right;direction: ltr;min-width: 28.61mm;width: 30.72mm;'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String ColumnEnd = "</td>";
                    //String TableHeading = TableHead + RowStart + thColumnStart + "Account Number" + thColumnEnd +
                    //    thColumnStart + "Patient Name" + thColumnEnd +
                    //    thColumnStart + "Home Phone No " + thColumnEnd +
                    //    thColumnStart + "DOS From" + thColumnEnd +
                    //    thColumnStart + "DOS To" + thColumnEnd +
                    //    thColumnStart + "Patient Balance " + thColumnEnd +
                    //    thColumnStart + "Last Payment Date" + thColumnEnd +
                    //    thColumnStart + "Last Paid Amount" + thColumnEnd +

                    //    "<th class='noWordBreak' colspan='5'>" + "Balance (Days)" + thColumnEnd +
                    //     RowEnd+ RowStart +
                    //    thColumnStartWithHeader + "Current" + thColumnEnd +
                    //    thColumnStartWithHeader + "30+" + thColumnEnd +
                    //    thColumnStartWithHeader + "60+" + thColumnEnd +
                    //    thColumnStartWithHeader + "90+" + thColumnEnd +
                    //    thColumnStartWithHeader + "120+" + thColumnEnd + RowEnd + TableHeadEnd;
                    String TableHeading = TableHead + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='11' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th><th class='noWordBreak text-center' style='background: #468cec' colspan='5'>" + "Balance (Days)" + thColumnEnd + RowEnd +
                        RowStart + thColumnStart + "Account Number" + thColumnEnd +
                     thColumnStart + "Patient Name" + thColumnEnd +
                     thColumnStart + "Home Phone No " + thColumnEnd +
                     thColumnStart + "Provider" + thColumnEnd +
                     thColumnStart + "Facility" + thColumnEnd +
                     thColumnStart + "DOS From" + thColumnEnd +
                     thColumnStart + "DOS To" + thColumnEnd +
                     thColumnStart + "Patient Balance " + thColumnEnd +
                     thColumnStart + "Last Payment Date" + thColumnEnd +
                     thColumnStart + "Last Paid Amount" + thColumnEnd +
                     thColumnStart + "Last DOS Days" + thColumnEnd +
                     thColumnStartWithHeader + "Current" + thColumnEnd +
                     thColumnStartWithHeader + "30+" + thColumnEnd +
                     thColumnStartWithHeader + "60+" + thColumnEnd +
                     thColumnStartWithHeader + "90+" + thColumnEnd +
                     thColumnStartWithHeader + "120+" + thColumnEnd + RowEnd + TableHeadEnd;
                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double PatBal = 0;
                    double LstPaidAmt = 0;
                    double CrntBal = 0;
                    double ThirtyBal = 0;
                    double SixtyBal = 0;
                    double NintyBal = 0;
                    double TwntyBal = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_OutStandingBalances.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_OutStandingBalances.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_OutStandingBalances.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_OutStandingBalances.HomePhoneNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_OutStandingBalances.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_OutStandingBalances.FacilityNameColumn] + ColumnEnd);

                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_OutStandingBalances.DOSFromColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_OutStandingBalances.DOSFromColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_OutStandingBalances.DOSFromColumn] + ColumnEnd);//
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_OutStandingBalances.DOSToColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_OutStandingBalances.DOSToColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_OutStandingBalances.DOSToColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_OutStandingBalances.PatientBalanceColumn])) + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_OutStandingBalances.LastPaymentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_OutStandingBalances.LastPaymentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_OutStandingBalances.LastPaymentDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_OutStandingBalances.LastpaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_OutStandingBalances.SubmittedDateColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_OutStandingBalances.CurrentBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_OutStandingBalances._30_DaysBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_OutStandingBalances._60_DaysBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_OutStandingBalances._90_DaysbalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_OutStandingBalances._120_DaysBalanceColumn])) + ColumnEnd);

                        sb.Append(RowEnd);

                        PatBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_OutStandingBalances.PatientBalanceColumn]);
                        LstPaidAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_OutStandingBalances.LastpaidAmountColumn]);
                        CrntBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_OutStandingBalances.CurrentBalanceColumn]);
                        ThirtyBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_OutStandingBalances._30_DaysBalanceColumn]);
                        SixtyBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_OutStandingBalances._60_DaysBalanceColumn]);
                        NintyBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_OutStandingBalances._90_DaysbalanceColumn]);
                        TwntyBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_OutStandingBalances._120_DaysBalanceColumn]);
                    }
                    PatBal = MDVUtility.ToAmount(PatBal);
                    LstPaidAmt = MDVUtility.ToAmount(LstPaidAmt);
                    CrntBal = MDVUtility.ToAmount(CrntBal);
                    ThirtyBal = MDVUtility.ToAmount(ThirtyBal);
                    SixtyBal = MDVUtility.ToAmount(SixtyBal);
                    NintyBal = MDVUtility.ToAmount(NintyBal);
                    TwntyBal = MDVUtility.ToAmount(TwntyBal);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatBal) + FootercolumnBoldEnd);
                    sb.Append(ColumnStart + "" + ColumnEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", LstPaidAmt) + FootercolumnBoldEnd);
                    sb.Append(ColumnStart + "" + ColumnEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", CrntBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", ThirtyBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SixtyBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", NintyBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", TwntyBal) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }

        public String LoadPatientListReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_PatientList.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + thColumnStart + "Practice " + thColumnEnd +

                        thColumnStart + "Facility " + thColumnEnd +
                        thColumnStart + "Provider " + thColumnEnd +
                        thColumnStart + "Account " + thColumnEnd +
                        thColumnStart + "SSN" + thColumnEnd +
                         thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "DOB " + thColumnEnd +
                        thColumnStart + "Sex" + thColumnEnd +
                        thColumnStart + "Marital Status" + thColumnEnd +
                        thColumnStart + "Address" + thColumnEnd +
                        thColumnStart + "Last Visit Date" + thColumnEnd +
                        thColumnStart + "Ref. Provider" + thColumnEnd +
                        thColumnStart + "Home Phone" + thColumnEnd +
                        thColumnStart + "Work Phone" + thColumnEnd +
                        thColumnStart + "Cell Phone" + thColumnEnd +
                        thColumnStart + "Ethnicity" + thColumnEnd +
                        thColumnStart + "Race" + thColumnEnd +
                        thColumnStart + "Pref. Lang" + thColumnEnd +
                        thColumnStart + "Guarantor" + thColumnEnd +
                        thColumnStart + "Email Address" + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_PatientList.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.FacilityNameColumn] + ColumnEnd);

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.SSNColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_PatientList.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_PatientList.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.DOBColumn] + ColumnEnd);//
                        }

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.SexColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.MaritialStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.PatientAddressColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_PatientList.LastVisitDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_PatientList.LastVisitDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.LastVisitDateColumn] + ColumnEnd);//
                        }

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.ReferringProviderNameColumn] + ColumnEnd);

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.HomePhoneNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.WorkPhoneNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.CellNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.EthnicityDescColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.RaceDescColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.CodeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.GuarantorColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientList.EmailAddressColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }

        public String LoadBillingInquiryByProviderReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Biliing_Inquiry_By_Provider.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String FootercolumnBoldStart = "<td colspan='5' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td colspan='2' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + 
                        thColumnStart + "Practice " + thColumnEnd +
                        thColumnStart + "Facility " + thColumnEnd +
                        thColumnStart + "Provider " + thColumnEnd +
                        thColumnStart + "Account #" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "OutStanding Balance " + thColumnEnd +
                        thColumnStart + "Inquiry Sent Date" + thColumnEnd +
                        thColumnStart + "Inquiry Sent By" + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    double OutBal = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Biliing_Inquiry_By_Provider.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Biliing_Inquiry_By_Provider.PracticeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Biliing_Inquiry_By_Provider.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Biliing_Inquiry_By_Provider.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Biliing_Inquiry_By_Provider.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Biliing_Inquiry_By_Provider.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Biliing_Inquiry_By_Provider.OutstandingBalanceColumn])) + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Biliing_Inquiry_By_Provider.InquirySentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Biliing_Inquiry_By_Provider.InquirySentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Biliing_Inquiry_By_Provider.InquirySentDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Biliing_Inquiry_By_Provider.InquirySentByColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                        OutBal += MDVUtility.ToDouble(dr[dsReports.DT_Biliing_Inquiry_By_Provider.OutstandingBalanceColumn]);
                    }

                    OutBal = MDVUtility.ToAmount(OutBal);

                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", OutBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + "" + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadBillingInquiryByProviderReport", ex);
                return ex.Message;
            }


        }
        public String LoadProcedureAnalysisReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_ProcedureAnalysis.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + thColumnStart + "Year" + thColumnEnd +
                        thColumnStart + "Account Number" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "CPT Code " + thColumnEnd +
                        thColumnStart + "Description" + thColumnEnd +
                        thColumnStart + "Practice " + thColumnEnd +

                        thColumnStart + "Facility " + thColumnEnd +

                        thColumnStart + "Provider" + thColumnEnd +
                         thColumnStart + "Q1" + thColumnEnd +
                         thColumnStart + "Q2" + thColumnEnd +
                         thColumnStart + "Q3" + thColumnEnd +
                         thColumnStart + "Q4" + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_ProcedureAnalysis.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.YearRangeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.CPTCodeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.DescriptionColumn] + ColumnEnd);

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.CountCPTCodeQuarter1Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.CountCPTCodeQuarter2Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.CountCPTCodeQuarter3Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProcedureAnalysis.CountCPTCodeQuarter4Column] + ColumnEnd);

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }
        /// <summary>
        /// When No Group Selected From Report
        /// </summary>
        /// <param name="ReportsParamaters"></param>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public String LoadPatientStatementNoneGroup(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName, true);

                if (dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].Rows.Count > 0)
                {

                    StringBuilder sb = new StringBuilder();
                    sb = sb.Append(string.Empty);
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead +
                       RowStart
                       + thColumnStart + " style='text-align:left'>" + "Practice " + thColumnEnd +
                            thColumnStart + ">" + "Facility" + thColumnEnd +
                            thColumnStart + "> Provider" + thColumnEnd +
                            thColumnStart + "> Account Number" + thColumnEnd +
                            thColumnStart + ">" + "Patient Name" + thColumnEnd +
                            thColumnStart + ">" + "Created Date" + thColumnEnd +
                            thColumnStart + ">" + "Patient Statement Status" + thColumnEnd +
                            thColumnStart + ">" + "Patient Balance" + thColumnEnd +
                            thColumnStart + ">" + "Last Statement Date" + thColumnEnd +
                          RowEnd + TableHeadEnd;
                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_PatientStatementPreference.PracticeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_PatientStatementPreference.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_PatientStatementPreference.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_PatientStatementPreference.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_PatientStatementPreference.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_PatientStatementPreference.CreatedDateColumn].ToString()) + ColumnEnd);
                        string patientStatus = dr[dsReports.DT_PatientStatementPreference.PatientStatementStatusColumn].ToString().ToLower() == "true" ? "Checked" : "UnChecked";
                        sb.Append(ColumnStart + ">" + patientStatus + ColumnEnd);
                        sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", dr[dsReports.DT_PatientStatementPreference.PatientBalanceColumn],"</div>") + ColumnEnd);
                        string lastStatement = (dr[dsReports.DT_PatientStatementPreference.LastStatementDateColumn.ColumnName] != System.DBNull.Value ? MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_PatientStatementPreference.LastStatementDateColumn.ColumnName].ToString()) : "N/A");
                        sb.Append(ColumnStart + ">" +
                           lastStatement
                            + ColumnEnd);


                        sb.Append(RowEnd);
                    }
                    Int64 UnCheckCount = 0, CheckCount = 0;
                    Double TotalBalance = 0;
                    TotalBalance = dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].AsEnumerable().Sum(x => x.Field<Double>(7));
                    sb.Append(RowStart);
                    sb.Append(ColumnStart + " colspan='7'><div style='text-align:right;font-weight: bold;'> Total</div>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", TotalBalance, "</div>") + ColumnEnd);
                    sb.Append(ColumnStart + ">" + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    // second table

                    String TableHeading1 = TableHead +
                       RowStart
                       + thColumnStart + " style='text-align:left'>" + "Total Statement UnCheck " + thColumnEnd +
                            thColumnStart + ">" + "Total Statements Check" + thColumnEnd +
                            thColumnStart + ">" + "Patient Balance" + thColumnEnd +
                            RowEnd + TableHeadEnd;
                    sb.Append("<div style='width: 50%;'>");
                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading1);
                   
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].Rows)
                    {
                        if (dr[dsReports.DT_PatientStatementPreference.PatientStatementStatusColumn].ToString().ToLower()=="true".ToLower())    
                            CheckCount = CheckCount + 1;
                        else
                            UnCheckCount = UnCheckCount + 1;
                    }
                    sb.Append(RowStart);
                    sb.Append(ColumnStart + "><div style='text-align:right;'> " + UnCheckCount +"</div>"+ ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'>" + CheckCount + "</div>"+ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("$ {0}", TotalBalance) + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();

                }
                else
                    return string.Empty;



            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientStatementNoneGroup", ex);
                return ex.Message;
            }
        }
        /// <summary>
        /// Single Group Selected From Report
        /// </summary>
        /// <param name="ReportsParamaters"></param>
        /// <param name="ReportName"></param>
        /// <param name="GroupByColumn"></param>
        /// <returns></returns>
        public String LoadPatientStatementSingleGroup(Dictionary<string, object> ReportsParamaters, String ReportName, string GroupByColumn)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                StringBuilder GroupbyHeadingColumns = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                sb = sb.Append(string.Empty);
                string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                string PrintDivEnd = "</div>";
                String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                String TableHead = "<thead class='printHeading'>";
                String thColumnStart = "<th class='noWordBreak'";
                String thColumnEnd = "</th>";
                String TableHeadEnd = "</thead>";
                String TableEnd = "</table>";
                String RowStart = "<tr>";
                String RowEnd = "</tr>";
                String ColumnStart = "<td";
                String ColumnEnd = "</td>";
                String TableHeading = TableHead +
                       RowStart;
                String HeadingColumn =
                            thColumnStart + " style='text-align:left'>" + "Practice " + thColumnEnd +
                            thColumnStart + "> Facility" + thColumnEnd +
                            thColumnStart + ">" + "Provider" + thColumnEnd +
                            thColumnStart + ">" + "Account Number" + thColumnEnd +
                            thColumnStart + ">" + "Patient Name" + thColumnEnd +
                            thColumnStart + ">" + "Patient Balance" + thColumnEnd +
                            thColumnStart + ">" + "Last Statement Date" + thColumnEnd +
                            RowEnd + TableHeadEnd;

                sb.Append(PrintDivStart);
                sb.Append(TableStart);
                sb.Append(TableHeading);
                if (GroupByColumn.ToLower() == "PatientStatementStatus".ToLower())
                {
                    sb.Append(thColumnStart + "> Patient Statement Status " + thColumnEnd);
                    sb.Append(thColumnStart + ">" + "Created Date" + thColumnEnd);
                }
                if (GroupByColumn.ToLower() == "CreatedDate".ToLower())
                {
                    sb.Append(thColumnStart + ">  Created Date" + thColumnEnd);
                    sb.Append(thColumnStart + ">" + " Patient Statement Status" + thColumnEnd);
                }
                sb.Append(HeadingColumn);
                if (dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].Rows.Count > 0)
                {

                    var result = from row in dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].AsEnumerable()
                                 group row by row.Field<string>(GroupByColumn.ToLower()) into grp
                                 select new
                                 {
                                     Columnkey = grp.Key,
                                     PatientStatementDetail = grp,
                                 };

                    foreach (var item in result)
                    {
                        bool appendGroupName = true;
                        foreach (var subitem in item.PatientStatementDetail)
                        {
                            sb.Append(RowStart);
                            if (appendGroupName)
                            {
                                if (GroupByColumn.ToLower() == "CreatedDate".ToLower())
                                    sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(item.Columnkey) + ColumnEnd);
                                else
                                {
                                    string patientStatus = item.Columnkey.ToLower() == "true" ? "Checked" : "UnChecked";
                                    sb.Append(ColumnStart + ">" + patientStatus + ColumnEnd); }
                                appendGroupName = false;

                            }
                            else
                                sb.Append(ColumnStart + ">" + ColumnEnd);
                            if (GroupByColumn.ToLower() == "PatientStatementStatus".ToLower())
                                sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(subitem.Field<string>("CreatedDate")) + ColumnEnd);
                            if (GroupByColumn.ToLower() == "CreatedDate".ToLower())
                            {
                                string patientStatus = subitem.Field<string>("PatientStatementStatus").ToLower() == "true" ? "Checked" : "UnChecked";
                                sb.Append(ColumnStart + ">" + patientStatus + ColumnEnd);
                            }
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Practice") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Facility") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Provider") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("AccountNumber") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("PatientName") + ColumnEnd);
                            sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", subitem.Field<Double>("PatientBalance"), "</div>") + ColumnEnd);
                            string lastStatement = subitem.Field<string>("LastStatementDate") == "" ? MDVUtility.GetDateMMDDYYY(subitem.Field<string>("LastStatementDate")) : "N/A";
                            sb.Append(ColumnStart + ">" + lastStatement + ColumnEnd);
                            sb.Append(RowEnd);
                        }
                    }
                    Int64 UnCheckCount = 0, CheckCount = 0;
                    Double TotalBalance = 0;
                    TotalBalance = dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].AsEnumerable().Sum(x => x.Field<Double>(7));
                    sb.Append(RowStart);
                    sb.Append(ColumnStart + " colspan='7'><div style='text-align:right;font-weight: bold;'> Total</div>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", TotalBalance, "</div>") + ColumnEnd);
                    sb.Append(ColumnStart + ">" + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    // second table

                    String TableHeading1 = TableHead +
                       RowStart
                       + thColumnStart + " style='text-align:left'>" + "Total Statements UnChecked " + thColumnEnd +
                            thColumnStart + ">" + "Total Statements Checked" + thColumnEnd +
                            thColumnStart + ">" + "Patient Balance" + thColumnEnd +
                            RowEnd + TableHeadEnd;
                    sb.Append("<div style='width: 50%;'>");
                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading1);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].Rows)
                    {
                        if (dr[dsReports.DT_PatientStatementPreference.PatientStatementStatusColumn].ToString().ToLower()=="true")
                            CheckCount = CheckCount + 1;
                        else
                            UnCheckCount = UnCheckCount + 1;
                    }
                    sb.Append(RowStart);
                    sb.Append(ColumnStart + "><div style='text-align:right;'> " + UnCheckCount + "</div>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'>" + CheckCount + "</div>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("$ {0}", TotalBalance) + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientStatementSingleGroup", ex);
                return ex.Message;
            }
        }
        public String LoadPatientStatementMutlipleGroup(Dictionary<string, object> ReportsParamaters, String ReportName, string Group1, string Group2)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName, true);
                StringBuilder GroupbyHeadingColumns = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                sb = sb.Append(string.Empty);
                string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                string PrintDivEnd = "</div>";
                String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                String TableHead = "<thead class='printHeading'>";
                String thColumnStart = "<th class='noWordBreak'";
                String thColumnEnd = "</th>";
                String TableHeadEnd = "</thead>";
                String TableEnd = "</table>";
                String RowStart = "<tr>";
                String RowEnd = "</tr>";
                String ColumnStart = "<td";
                String ColumnEnd = "</td>";
                String TableHeading = TableHead +
                       RowStart;
                String HeadingColumn =
                           thColumnStart + " style='text-align:left'>" + "Practice " + thColumnEnd +
                            thColumnStart + "> Facility" + thColumnEnd +
                            thColumnStart + ">" + "Provider" + thColumnEnd +
                            thColumnStart + ">" + "Account Number" + thColumnEnd +
                            thColumnStart + ">" + "Patient Name" + thColumnEnd +
                            thColumnStart + ">" + "Patient Balance" + thColumnEnd +
                            thColumnStart + ">" + "Last Statement Date" + thColumnEnd +
                            RowEnd + TableHeadEnd;

                sb.Append(PrintDivStart);
                sb.Append(TableStart);
                sb.Append(TableHeading);
                if (Group1.ToLower() == "PatientStatementStatus".ToLower())
                {
                    sb.Append(thColumnStart + "> Patient Statement Status " + thColumnEnd);
                    sb.Append(thColumnStart + ">" + "Created Date" + thColumnEnd);
                }
                else
                {
                    sb.Append(thColumnStart + ">  Created Date" + thColumnEnd);
                    sb.Append(thColumnStart + ">" + " Patient Statement Status" + thColumnEnd);
                }
                sb.Append(HeadingColumn);
                if (dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].Rows.Count > 0)
                {

                    var result = from row in dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].AsEnumerable()
                                 group row by row.Field<string>(Group1.ToLower()) into grp1
                                 select new
                                 {
                                     Group1key = grp1.Key,
                                     Group1Detail = grp1,
                                     Group2Detail = from row in grp1
                                                    group row by row.Field<string>(Group2.ToLower()) into grp2
                                                    select new
                                                    {
                                                        Group2key = grp2.Key,
                                                        Group2Details = grp2
                                                    }
                                 };
                    foreach (var item in result)
                    {
                        bool appendGroup1Name = true;
                        foreach (var subitem in item.Group2Detail)
                        {
                            bool appendGroup2Name = true;
                            foreach (var subitem1 in subitem.Group2Details)
                            {
                                sb.Append(RowStart);
                                if (appendGroup1Name)
                                {  if (Group1.ToLower() == "CreatedDate".ToLower())
                                        sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(item.Group1key) + ColumnEnd);
                                    else
                                    {
                                        string patientStatus = item.Group1key.ToLower() == "true" ? "Checked" : "UnChecked";
                                        sb.Append(ColumnStart + ">" + patientStatus + ColumnEnd);

                                    }
                                    appendGroup1Name = false;
                                }
                                else
                                    sb.Append(ColumnStart + ">" + ColumnEnd);
                                if (appendGroup2Name)
                                {
                                    if (Group2.ToLower() == "CreatedDate".ToLower())
                                        sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(subitem.Group2key) + ColumnEnd);
                                    else
                                    {
                                        string patientStatus = subitem.Group2key.ToLower() == "true" ? "Checked" : "UnChecked";
                                        sb.Append(ColumnStart + ">" + patientStatus + ColumnEnd);

                                    }
                                    appendGroup2Name = false;
                                }
                                else
                                    sb.Append(ColumnStart + ">" + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Practice") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Facility") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Provider") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("AccountNumber") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("PatientName") + ColumnEnd);
                                sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", subitem1.Field<Double>("PatientBalance"), "</div>") + ColumnEnd);
                                string lastStatement = subitem1.Field<string>("LastStatementDate") == "" ? MDVUtility.GetDateMMDDYYY(subitem1.Field<string>("LastStatementDate")) : "N/A";
                                sb.Append(ColumnStart + ">" + lastStatement + ColumnEnd);
                                sb.Append(RowEnd);
                            }
                        }
                    }
                    Int64 UnCheckCount = 0, CheckCount = 0;
                    Double TotalBalance = 0;
                    TotalBalance = dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].AsEnumerable().Sum(x => x.Field<Double>(7));
                    sb.Append(RowStart);
                    sb.Append(ColumnStart + " colspan='7'><div style='text-align:right;font-weight: bold;'> Total</div>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", TotalBalance, "</div>") + ColumnEnd);
                    sb.Append(ColumnStart + ">" + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    // second table

                    String TableHeading1 = TableHead +
                       RowStart
                       + thColumnStart + " style='text-align:left'>" + "Total Statements UnChecked " + thColumnEnd +
                            thColumnStart + ">" + "Total Statements Check" + thColumnEnd +
                            thColumnStart + ">" + "Patient Balance" + thColumnEnd +
                            RowEnd + TableHeadEnd;
                    sb.Append("<div style='width: 50%;'>");
                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading1);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_PatientStatementPreference.TableName].Rows)
                    {
                        if (dr[dsReports.DT_PatientStatementPreference.PatientStatementStatusColumn].ToString().ToLower()=="true")
                            CheckCount = CheckCount + 1;
                        else
                            UnCheckCount = UnCheckCount + 1;

                    }
                    sb.Append(RowStart);
                    sb.Append(ColumnStart + "><div style='text-align:right;'> " + UnCheckCount + "</div>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'>" + CheckCount + "</div>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("$ {0}", TotalBalance) + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientStatementMutlipleGroup", ex);
                return ex.Message;
            }
        }
        #endregion

        #region Schedular Reports

        public String LoadCheckInAndCheckOutDurationReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_CheckinCheckoutDuration.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + thColumnStart + "Provider" + thColumnEnd +
                         thColumnStart + "Facility " + thColumnEnd +
                          thColumnStart + "Date " + thColumnEnd +
                           thColumnStart + "Account " + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +

                        thColumnStart + "Reason" + thColumnEnd +
                        thColumnStart + "Status" + thColumnEnd +
                        thColumnStart + "Check In Time " + thColumnEnd +
                        thColumnStart + "Check Out Time" + thColumnEnd +
                        thColumnStart + "Duration" + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_CheckinCheckoutDuration.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CheckinCheckoutDuration.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CheckinCheckoutDuration.FacilityNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_CheckinCheckoutDuration.AppointmentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_CheckinCheckoutDuration.AppointmentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_CheckinCheckoutDuration.AppointmentDateColumn] + ColumnEnd);//
                        }

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CheckinCheckoutDuration.AccountNumberColumn] + ColumnEnd);

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CheckinCheckoutDuration.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CheckinCheckoutDuration.AppointmentReasonColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CheckinCheckoutDuration.AppointmentStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CheckinCheckoutDuration.CheckinTimeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CheckinCheckoutDuration.CheckOutTimeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_CheckinCheckoutDuration.DurationInMinutesColumn] + ColumnEnd);

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }
        public String LoadDailyAppointmentsProviderReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_DailyAppointmentsProvider.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='13' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead +
                        thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Reason " + thColumnEnd +
                        thColumnStart + "Date" + thColumnEnd +
                        thColumnStart + "Time" + thColumnEnd +
                        thColumnStart + "Minutes " + thColumnEnd +
                        thColumnStart + "Account" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Date of Birth" + thColumnEnd +
                        thColumnStart + "Home Tel" + thColumnEnd +
                         thColumnStart + "Cell" + thColumnEnd +
                         thColumnStart + "Insurance Plan" + thColumnEnd +
                         thColumnStart + "Copayment" + thColumnEnd +
                         thColumnStart + "Paid Amount" + thColumnEnd +
                         thColumnStart + "Patient Type" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Created By" + thColumnEnd +
                          thColumnStart + "Modified By" + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double Copay = 0;
                    double CopayPaid = 0;
                   // double CopayDiscount = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_DailyAppointmentsProvider.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.AppointmentReasonColumn] + ColumnEnd);

                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_DailyAppointmentsProvider.AppointmentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_DailyAppointmentsProvider.AppointmentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.AppointmentDateColumn] + ColumnEnd);//
                        }


                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.AppointmentTimeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.AppointmentMinutesColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_DailyAppointmentsProvider.DateofBirthColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_DailyAppointmentsProvider.DateofBirthColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.DateofBirthColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.HomeTelColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.CellColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_DailyAppointmentsProvider.CopaymentColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_DailyAppointmentsProvider.paidamountColumn])) + ColumnEnd);
                        // sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_DailyAppointmentsProvider.DiscountAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.PatientTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.AppointmentStatusColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_DailyAppointmentsProvider.DateofBirthColumn].ToString()))
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.CreatedByColumn] + " " + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_DailyAppointmentsProvider.CreatedOnColumn].ToString()) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.CreatedByColumn] + " " + dr[dsReports.DT_Reports_DailyAppointmentsProvider.CreatedOnColumn] + ColumnEnd);
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_DailyAppointmentsProvider.DateofBirthColumn].ToString()))
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.ModifiedByColumn] + " " + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_DailyAppointmentsProvider.ModifiedOnColumn].ToString()) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsProvider.ModifiedByColumn] + " " + dr[dsReports.DT_Reports_DailyAppointmentsProvider.ModifiedOnColumn] + ColumnEnd);
                        }
                        sb.Append(RowEnd);

                        Copay += MDVUtility.ToDouble(dr[dsReports.DT_Reports_DailyAppointmentsProvider.CopaymentColumn]);
                        CopayPaid += MDVUtility.ToDouble(dr[dsReports.DT_Reports_DailyAppointmentsProvider.paidamountColumn]);
                        //CopayDiscount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_DailyAppointmentsProvider.DiscountAmountColumn]);
                    }

                    Copay = MDVUtility.ToAmount(Copay);
                    CopayPaid = MDVUtility.ToAmount(CopayPaid);
                   // CopayDiscount = MDVUtility.ToAmount(CopayDiscount);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Copay) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", CopayPaid) + FootercolumnBoldEnd);
                  //  sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", CopayDiscount) + FootercolumnBoldEnd);
                    sb.Append(ColumnStart + "" + ColumnEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadDailyAppointmentsProviderReport", ex);
                return ex.Message;
            }


        }
        public String LoadDailyAppointmentsResourcesReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_DailyAppointmentsResource.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Reason " + thColumnEnd +
                        thColumnStart + "Date" + thColumnEnd +
                        thColumnStart + "Time" + thColumnEnd +
                        thColumnStart + "Minutes " + thColumnEnd +
                        thColumnStart + "Account" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Date of Birth" + thColumnEnd +
                        thColumnStart + "Home Tel" + thColumnEnd +
                         thColumnStart + "Cell" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_DailyAppointmentsResource.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.ResourcesNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.AppointmentReasonColumn] + ColumnEnd);

                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_DailyAppointmentsResource.AppointmentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_DailyAppointmentsResource.AppointmentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.AppointmentDateColumn] + ColumnEnd);//
                        }


                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.AppointmentTimeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.AppointmentMinutesColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_DailyAppointmentsResource.DateofBirthColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_DailyAppointmentsResource.DateofBirthColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.DateofBirthColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.HomeTelColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.CellColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_DailyAppointmentsResource.AppointmentStatusColumn] + ColumnEnd);

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadDailyAppointmentsResourcesReport", ex);
                return ex.Message;
            }




        }
        //note reminder for azhar: add header appointment
        public String LoadEnterpriseSchedulingReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Report_EnterpriseScheduling.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnStartWithHeader = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String FootercolumnBoldStart = "<td colspan='11' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='7' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th><th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Appointment" + thColumnEnd + "<th class='noWordBreak th-adjust' colspan='2' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Resource " + thColumnEnd +
                        thColumnStart + "Referring Provider" + thColumnEnd +
                        thColumnStart + "Plan" + thColumnEnd +
                        thColumnStart + "Subscriber  " + thColumnEnd +
                        thColumnStart + "Subscriber ID" + thColumnEnd +
                        thColumnStartWithHeader + "Date" + thColumnEnd +
                        thColumnStartWithHeader + "Time" + thColumnEnd +
                        thColumnStartWithHeader + "Reason" + thColumnEnd +
                        thColumnStart + "Comments" + thColumnEnd +
                        thColumnStart + "Copayment" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double Copay = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Report_EnterpriseScheduling.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.ResourceNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.ReferringProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.SubscriberNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.SubscriberIdColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Report_EnterpriseScheduling.AppointmentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Report_EnterpriseScheduling.AppointmentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.AppointmentDateColumn] + ColumnEnd);//
                        }

                        sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.AppointmentTimeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.AppointmentReasonColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_EnterpriseScheduling.CommentsColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_EnterpriseScheduling.CopaymentColumn])) + ColumnEnd);

                        sb.Append(RowEnd);

                        Copay += MDVUtility.ToDouble(dr[dsReports.DT_Report_EnterpriseScheduling.CopaymentColumn]);
                    }
                    Copay = MDVUtility.ToAmount(Copay);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + " Grand Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Copay) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadEnterpriseSchedulingReport", ex);
                return ex.Message;
            }


        }
        public String LoadTotalProviderAppointmentReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_ProviderAppointment.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Reason " + thColumnEnd +
                        thColumnStart + "Appointment Date" + thColumnEnd +
                        thColumnStart + "Number Of Appointments" + thColumnEnd +
                        thColumnStart + "Total Minutes " + thColumnEnd +
                        thColumnStart + "Patient Type " + thColumnEnd +
                        thColumnStart + "Visit Type " + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_ProviderAppointment.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProviderAppointment.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProviderAppointment.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProviderAppointment.AppointmentReasonColumn] + ColumnEnd);

                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_ProviderAppointment.AppointmentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_ProviderAppointment.AppointmentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProviderAppointment.AppointmentDateColumn] + ColumnEnd);//
                        }

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProviderAppointment.NumberOfAppointmentsColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProviderAppointment.TotalMinutesColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProviderAppointment.PatienttypeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ProviderAppointment.VisitTypeNameColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadTotalProviderAppointmentReport", ex);
                return ex.Message;
            }


        }


        public String LoadTotalResourceAppointmentReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_ResourceAppointment.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + thColumnStart + "Resources" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Appointment Date" + thColumnEnd +
                        thColumnStart + "Reason " + thColumnEnd +
                        thColumnStart + "Number Of Appointments" + thColumnEnd +
                        thColumnStart + "Total Minutes " + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_ResourceAppointment.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ResourceAppointment.ResourceNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ResourceAppointment.FacilityNameColumn] + ColumnEnd);


                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_ResourceAppointment.AppointmentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_ResourceAppointment.AppointmentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_ResourceAppointment.AppointmentDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ResourceAppointment.AppointmentReasonColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ResourceAppointment.NumberOfAppointmentsColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ResourceAppointment.TotalMinutesColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadTotalResourceAppointmentReport", ex);
                throw ex;
                // return ex.Message;
            }


        }

        public String LoadTotalFollowupAppointmentsReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Ttl_Followup_Appointments.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    string ColSpan = "<td colspan=4></td>";
                    String TableHeading = TableHead + thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Appointment Date" + thColumnEnd +
                        thColumnStart + "Time" + thColumnEnd +
                        thColumnStart + " Appt. Status" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    int TotalAppointments = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Ttl_Followup_Appointments.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Ttl_Followup_Appointments.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Ttl_Followup_Appointments.FacilityNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Ttl_Followup_Appointments.AppointmentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Ttl_Followup_Appointments.AppointmentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Ttl_Followup_Appointments.AppointmentDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Ttl_Followup_Appointments.TimeFromColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Ttl_Followup_Appointments.AppointmentStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Ttl_Followup_Appointments.PatientNameColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                        TotalAppointments++;
                    }
                    sb.Append(RowStart);
                    sb.Append(ColumnStart + "<b>Total Appointments :</b>" + ColumnEnd);
                    sb.Append(ColumnStart + "<b>" + TotalAppointments + "</b>" + ColumnEnd);
                    sb.Append(ColSpan);
                   
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadTotalFollowupAppointmentsReport", ex);
                throw ex;
                // return ex.Message;
            }


        }

        public String LoadDailyAppointmentReminderReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Daily_Appointment_Reminder.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead +
                        thColumnStart + "Date" + thColumnEnd +
                        thColumnStart + "Time" + thColumnEnd +
                        thColumnStart + "Minutes " + thColumnEnd +
                        thColumnStart + "Account Number" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Date of Birth " + thColumnEnd +
                        thColumnStart + "Home Tel " + thColumnEnd +
                        thColumnStart + "Cell No" + thColumnEnd +
                        thColumnStart + "Appt. Status" + thColumnEnd +
                        thColumnStart + "Reminder Type" + thColumnEnd +
                        thColumnStart + "Reminder Delivery" + thColumnEnd +
                        thColumnStart + "Reminder Response" + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Daily_Appointment_Reminder.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Daily_Appointment_Reminder.DateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Daily_Appointment_Reminder.DateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.DateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.TimeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.MinutesColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Daily_Appointment_Reminder.DoBColumn].ToString()) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.HomePhoneNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.CellNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.ApptStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.ReminderTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.ReminderDeliveryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Daily_Appointment_Reminder.ReminderResponceColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadDailyProviderAppointmentsReport", ex);
                throw ex;
                // return ex.Message;
            }


        }
        public String LoadDailyCopaySheetReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_DailyCopaySheet.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlignDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FooterRowStart = "<tr>";
                    String FootercolumnBoldStart = "<td colspan='6' align='center'><b>";
                    String FootercolumnBoldStartSpace = "<td colspan='1' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td class='text-right'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead +
                        thColumnStart + "Appointment Time" + thColumnEnd +
                        thColumnStart + "Account #" + thColumnEnd +
                        thColumnStart + "Phone Number " + thColumnEnd +
                        thColumnStart + "Insurance Plan" + thColumnEnd +
                        thColumnStart + "Subscriber ID" + thColumnEnd +
                        thColumnStart + "Visit Type" + thColumnEnd +
                        thColumnStart + "Copay Amount" + thColumnEnd +
                        thColumnStart + "Cash Amount" + thColumnEnd +
                        thColumnStart + "Check Amount" + thColumnEnd +
                        thColumnStart + "Check Number" + thColumnEnd +
                        thColumnStart + "CC Amount " + thColumnEnd +
                        thColumnStart + "CC Type" + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double Copay = 0;
                    double cash = 0;
                    double check = 0;
                    double card = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_DailyCopaySheet.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_DailyCopaySheet.TimeFromColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_DailyCopaySheet.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_DailyCopaySheet.HomePhoneNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_DailyCopaySheet.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_DailyCopaySheet.SubscriberIdColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_DailyCopaySheet.VisittypeColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlignDollar + dr[dsReports.DT_DailyCopaySheet.CopaymentColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlignDollar + dr[dsReports.DT_DailyCopaySheet.CashColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlignDollar + dr[dsReports.DT_DailyCopaySheet.CheckAmountColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_DailyCopaySheet.CheckNoColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlignDollar + dr[dsReports.DT_DailyCopaySheet.CCAmountColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_DailyCopaySheet.CardTypeColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                        if (MDVUtility.ToInt64(dr[dsReports.DT_DailyCopaySheet.RankIdColumn]) == 1)
                        {
                            Copay += MDVUtility.ToDouble(dr[dsReports.DT_DailyCopaySheet.CopaymentColumn]);
                        }
                        cash += MDVUtility.ToDouble(dr[dsReports.DT_DailyCopaySheet.CashColumn]);
                        check += MDVUtility.ToDouble(dr[dsReports.DT_DailyCopaySheet.CheckAmountColumn]);
                        card += MDVUtility.ToDouble(dr[dsReports.DT_DailyCopaySheet.CCAmountColumn]);
                    }
                    Copay = MDVUtility.ToAmount(Copay);
                    cash = MDVUtility.ToAmount(cash);
                    check = MDVUtility.ToAmount(check);
                    card = MDVUtility.ToAmount(card);
                    //Footer
                    sb.Append(FooterRowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);

                    sb.Append(FootercolumnBoldStart1 + (Copay >= 0 ? String.Format("${0:#,###,##0.00}", Copay) : String.Format("(${0:#,###,##0.00})", Math.Abs(Copay))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (cash >= 0 ? String.Format("${0:#,###,##0.00}", cash) : String.Format("(${0:#,###,##0.00})", Math.Abs(cash))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (check >= 0 ? String.Format("${0:#,###,##0.00}", check) : String.Format("(${0:#,###,##0.00})", Math.Abs(check))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartSpace + " " + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (card >= 0 ? String.Format("${0:#,###,##0.00}", card) : String.Format("(${0:#,###,##0.00})", Math.Abs(card))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartSpace + " " + FootercolumnBoldEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadDailyCopaySheetReport", ex);
                throw ex;
            }
        }

        public String LoadUnclaimedAppointmentsReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Unclaimed_Appointment.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead +
                        thColumnStart + "Account #" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Claim Number" + thColumnEnd +
                        thColumnStart + "DOS From" + thColumnEnd +
                        thColumnStart + "DOS To" + thColumnEnd +
                        thColumnStart + "Status" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Insurance Plan" + thColumnEnd +
                        thColumnStart + "Check In" + thColumnEnd +
                        thColumnStart + "Submit Status" + thColumnEnd +
                        thColumnStart + "Claim Status" + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Unclaimed_Appointment.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.PatientAccountNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Unclaimed_Appointment.DOSFromColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Unclaimed_Appointment.DOSFromColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.DOSFromColumn] + ColumnEnd);//
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Unclaimed_Appointment.DOSToColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Unclaimed_Appointment.DOSToColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.DOSToColumn] + ColumnEnd);//
                        }

                        sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.AppointmentStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.AppointmentProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + "Yes" + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.SubmitStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Unclaimed_Appointment.ClaimStatusColumn] + ColumnEnd);

                        sb.Append(RowEnd);
                    }

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadUnclaimedAppointmentsReport", ex);
                return ex.Message;
            }


        }
        #endregion

        #region Charges and Payments Reports
        public String LoadChargesListReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_ChargesList.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='14' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td colspan='6' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility " + thColumnEnd +
                         thColumnStart + "Provider " + thColumnEnd +
                          thColumnStart + "Plan" + thColumnEnd +
                           thColumnStart + "Account" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "DOS" + thColumnEnd +
                        thColumnStart + "Claim Number" + thColumnEnd +
                        thColumnStart + "CPTCode" + thColumnEnd +
                        thColumnStart + "Modifiers" + thColumnEnd +
                        thColumnStart + "ICDCode1" + thColumnEnd +
                        thColumnStart + "ICDCode2" + thColumnEnd +
                        thColumnStart + "ICDCode3" + thColumnEnd +
                         thColumnStart + "ICDCode4" + thColumnEnd +
                          thColumnStart + "Fee" + thColumnEnd +
                           thColumnStart + "Copay" + thColumnEnd +
                            thColumnStart + "Submitted Date" + thColumnEnd +
                            thColumnStart + "Claim Status" + thColumnEnd +
                            thColumnStart + "Submit Status" + thColumnEnd +
                            thColumnStart + "Voided Status" + thColumnEnd +
                             thColumnStart + "Entry Date" + thColumnEnd +
                             thColumnStart + "Entered By" + thColumnEnd +
                             thColumnStart + "Ins Charges" + thColumnEnd +
                             thColumnStart + "Ins Paid" + thColumnEnd +
                             thColumnStart + "Ins Write Off" + thColumnEnd +
                             thColumnStart + "Ins Balance" + thColumnEnd +
                             thColumnStart + "Pat Charges" + thColumnEnd +
                             thColumnStart + "Pat Paid" + thColumnEnd +
                             thColumnStart + "Pat Discount" + thColumnEnd +
                             thColumnStart + "Pat Bal" + thColumnEnd   +
                             thColumnStart + "Ref.Provider" + thColumnEnd + TableHeadEnd;


                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    double Fee = 0;
                    double Copay = 0;
                    double InsChrg = 0;
                    double InsPaid = 0;
                    double InsWrtOff = 0;
                    double InsBal = 0;
                    double PatChrg = 0;
                    double PatPaid = 0;
                    double PatDiscunt = 0;
                    double PatBla = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_ChargesList.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.PatientAccountColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_ChargesList.DOSFromColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_ChargesList.DOSFromColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.DOSFromColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.CPTCodeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.ModifiersColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.ICDCode1Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.ICDCode2Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.ICDCode3Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.ICDCode4Column] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ChargesList.FeeColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ChargesList.CopayColumn])) + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_ChargesList.SubmittedDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_ChargesList.SubmittedDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.SubmittedDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.ClaimStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.SubmitStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.VoidedStatusColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_ChargesList.EntryDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_ChargesList.EntryDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.EntryDateColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.EnteredByColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ChargesList.InsChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ChargesList.InsurancePaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ChargesList.InsuranceWriteOffColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ChargesList.InsuranceBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ChargesList.PatientChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ChargesList.PatientPaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ChargesList.PatientDiscountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ChargesList.PatientBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ChargesList.RefProviderNameColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                        Fee += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ChargesList.FeeColumn]);
                        Copay += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ChargesList.CopayColumn]);
                        InsChrg += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ChargesList.InsChargesColumn]);
                        InsPaid += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ChargesList.InsurancePaidAmountColumn]);
                        InsWrtOff += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ChargesList.InsuranceWriteOffColumn]);
                        InsBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ChargesList.InsuranceBalanceColumn]);
                        PatChrg += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ChargesList.PatientChargesColumn]);
                        PatPaid += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ChargesList.PatientPaidAmountColumn]);
                        PatDiscunt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ChargesList.PatientDiscountColumn]);
                        PatBla += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ChargesList.PatientBalanceColumn]);
                    }
                    Fee = MDVUtility.ToAmount(Fee);
                    Copay = MDVUtility.ToAmount(Copay);
                    InsChrg = MDVUtility.ToAmount(InsChrg);
                    InsPaid = MDVUtility.ToAmount(InsPaid);
                    InsWrtOff = MDVUtility.ToAmount(InsWrtOff);
                    InsBal = MDVUtility.ToAmount(InsBal);
                    PatChrg = MDVUtility.ToAmount(PatChrg);
                    PatPaid = MDVUtility.ToAmount(PatPaid);
                    PatDiscunt = MDVUtility.ToAmount(PatDiscunt);
                    PatBla = MDVUtility.ToAmount(PatBla);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Fee) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Copay) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + "" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsChrg) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsPaid) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsWrtOff) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatChrg) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatPaid) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatDiscunt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatBla) + FootercolumnBoldEnd);
                    sb.Append(ColumnStart + "" + ColumnEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadChargesListReport", ex);
                return ex.Message;
            }


        }

        public String LoadClaimsInCollectionReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_ClaimsInCollection.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='14' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td colspan='3' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility " + thColumnEnd +
                        thColumnStart + "Provider " + thColumnEnd +
                        thColumnStart + "Account " + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "DOB" + thColumnEnd +
                        thColumnStart + "Address" + thColumnEnd +
                        thColumnStart + "ZIP Code" + thColumnEnd +
                        thColumnStart + "Home Phone" + thColumnEnd +
                        thColumnStart + "Cell Phone" + thColumnEnd +
                        thColumnStart + "Service Date" + thColumnEnd +
                        thColumnStart + "Claim Date" + thColumnEnd +
                        thColumnStart + "Claim Number" + thColumnEnd +
                        thColumnStart + "CPT Code" + thColumnEnd +
                        thColumnStart + "Charge Amount" + thColumnEnd +
                        thColumnStart + "Patient Payments" + thColumnEnd +
                        thColumnStart + "Patient Balance" + thColumnEnd + TableHeadEnd;
                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    double totalChargeAmount = 0;
                    double totlaPatientPayments = 0;
                    double totalPatientBalance = 0;
                   
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_ClaimsInCollection.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.PatientColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_ClaimsInCollection.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_ClaimsInCollection.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.DOBColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.HomeAddressColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.ZIPCodeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.HomePhoneNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.CellNoColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_ClaimsInCollection.ServiceDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_ClaimsInCollection.ServiceDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.ServiceDateColumn] + ColumnEnd);//
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_ClaimsInCollection.ClaimDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_ClaimsInCollection.ClaimDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.ClaimDateColumn] + ColumnEnd);//
                        }


                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsInCollection.CPTCodeColumn] + ColumnEnd);


                        
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_ClaimsInCollection.ChargeAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_ClaimsInCollection.PatientPaymentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_ClaimsInCollection.PatientBalanceColumn])) + ColumnEnd);
                        sb.Append(RowEnd);

                        totalChargeAmount += MDVUtility.ToDouble(dr[dsReports.DT_ClaimsInCollection.ChargeAmountColumn]);
                        totlaPatientPayments += MDVUtility.ToDouble(dr[dsReports.DT_ClaimsInCollection.PatientPaymentsColumn]);
                        totalPatientBalance += MDVUtility.ToDouble(dr[dsReports.DT_ClaimsInCollection.PatientBalanceColumn]);
                       
                    }
                    
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", totalChargeAmount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", totlaPatientPayments) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", totalPatientBalance) + FootercolumnBoldEnd);
                    //sb.Append(FootercolumnBoldStart1 + "" + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadClaimsInCollectionReport", ex);
                return ex.Message;
            }


        }
        /// <summary>
        /// Single Group Selected From Report
        /// </summary>
        /// <param name="ReportsParamaters"></param>
        /// <param name="ReportName"></param>
        /// <param name="GroupByColumn"></param>
        /// <returns></returns>
        public String LoadClaimsInCollectionSingleGroup(Dictionary<string, object> ReportsParamaters, String ReportName, string GroupByColumn)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                StringBuilder GroupbyHeadingColumns = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                sb = sb.Append(string.Empty);
                string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                string PrintDivEnd = "</div>";
                String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                String TableHead = "<thead class='printHeading'>";
                String thColumnStart = "<th class='noWordBreak'>";
                String thColumnEnd = "</th>";
                String TableHeadEnd = "</thead>";
                String TableEnd = "</table>";
                String RowStart = "<tr>";
                String RowEnd = "</tr>";
                String ColumnStart = "<td";
                String ColumnEnd = "</td>";
                String TableHeading = TableHead +
                       RowStart;
                String HeadingColumn =
                             thColumnStart  + "Practice " + thColumnEnd +
                             thColumnStart + "Facility " + thColumnEnd +
                             thColumnStart + "Provider " + thColumnEnd +
                             thColumnStart + "Account " + thColumnEnd +
                             thColumnStart + "DOB" + thColumnEnd +
                             thColumnStart + "Address" + thColumnEnd +
                             thColumnStart + "ZIP Code" + thColumnEnd +
                             thColumnStart + "Home Phone" + thColumnEnd +
                             thColumnStart + "Cell Phone" + thColumnEnd +
                             thColumnStart + "Service Date" + thColumnEnd +
                             thColumnStart + "Claim Date" + thColumnEnd +
                             thColumnStart + "Claim Number" + thColumnEnd +
                             thColumnStart + "CPT Code" + thColumnEnd +
                             thColumnStart + "Charge Amount" + thColumnEnd +
                             thColumnStart + "Patient Payments" + thColumnEnd +
                            RowEnd + TableHeadEnd;

                sb.Append(PrintDivStart);
                sb.Append(TableStart);
                sb.Append(TableHeading);
                if (GroupByColumn.ToLower() == "PatientBalance".ToLower())
                {
                    sb.Append(thColumnStart + " Patient Balance " + thColumnEnd);
                    sb.Append(thColumnStart  + "Patient Name" + thColumnEnd);
                }
                if (GroupByColumn.ToLower() == "Patient".ToLower())
                {
                    sb.Append(thColumnStart + "  Patient Name" + thColumnEnd);
                    sb.Append(thColumnStart  + " Patient Balance" + thColumnEnd);
                }
                sb.Append(HeadingColumn);
                if (dsReports.Tables[dsReports.DT_ClaimsInCollection.TableName].Rows.Count > 0)
                {

                    var result = from row in dsReports.Tables[dsReports.DT_ClaimsInCollection.TableName].AsEnumerable()
                                 group row by row.Field<string>(GroupByColumn.ToLower()) into grp
                                 select new
                                 {
                                     Columnkey = grp.Key,
                                     ClaimCollectionDetail = grp,
                                 };

                    foreach (var item in result)
                    {
                        bool appendGroupName = true;
                        foreach (var subitem in item.ClaimCollectionDetail)
                        {
                            sb.Append(RowStart);
                            if (appendGroupName)
                            {
                                if (GroupByColumn.ToLower() == "Patient".ToLower())
                                    sb.Append(ColumnStart + ">" + item.Columnkey + ColumnEnd);
                                else
                                {
                                    sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", item.Columnkey, "</div>") + ColumnEnd);
                                }
                                appendGroupName = false;

                            }
                            else
                                sb.Append(ColumnStart + ">" + ColumnEnd);
                            if (GroupByColumn.ToLower() == "Patient".ToLower())
                               sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", subitem.Field<string>("PatientBalance"), "</div>") + ColumnEnd);
                            if (GroupByColumn.ToLower() == "PatientBalance".ToLower())
                            {
                               sb.Append(ColumnStart + ">" + subitem.Field<string>("Patient") + ColumnEnd);
                            }
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("PracticeName") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Facility") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Provider") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("AccountNumber") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(subitem.Field<string>("DOB")) + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("HomeAddress") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("ZIPCode") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("HomePhoneNo") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("CellNo") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY( subitem.Field<string>("ServiceDate")) + ColumnEnd);
                            sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(subitem.Field<string>("ClaimDate")) + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("ClaimNumber") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("CPTCode") + ColumnEnd);
                            sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", subitem.Field<Double>("ChargeAmount"), "</div>") + ColumnEnd);
                            sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", subitem.Field<Double>("PatientPayments"), "</div>") + ColumnEnd);
                            sb.Append(RowEnd);
                        }
                    }

                    Double TotalChargeAmount = 0, TotalPatientPayment=0;
                    TotalChargeAmount = dsReports.Tables[dsReports.DT_ClaimsInCollection.TableName].AsEnumerable().Sum(x => x.Field<Double>(7));
                    TotalPatientPayment = dsReports.Tables[dsReports.DT_ClaimsInCollection.TableName].AsEnumerable().Sum(x => x.Field<Double>(6));
                    sb.Append(RowStart);
                    sb.Append(ColumnStart + " colspan='15'><div style='text-align:right;font-weight: bold;'> Total</div>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'><b>" + string.Format("${0}{1}", TotalChargeAmount, "</div>") + "</b>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'><b>" + string.Format("${0}{1}", TotalPatientPayment, "</div>") + "</b>" + ColumnEnd);
                    sb.Append(ColumnStart + ">" + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);

                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadClaimsInCollectionSingleGroup", ex);
                return ex.Message;
            }
        }
        /// <summary>
        /// mutli group report for claim in collection
        /// </summary>
        /// <param name="ReportsParamaters"></param>
        /// <param name="ReportName"></param>
        /// <param name="Group1"></param>
        /// <param name="Group2"></param>
        /// <returns></returns>
        public String LoadClaimsInCollectionMutlipleGroup(Dictionary<string, object> ReportsParamaters, String ReportName, string Group1, string Group2)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName, true);
                StringBuilder GroupbyHeadingColumns = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                sb = sb.Append(string.Empty);
                string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                string PrintDivEnd = "</div>";
                String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                String TableHead = "<thead class='printHeading'>";
                String thColumnStart = "<th class='noWordBreak'>";
                String thColumnEnd = "</th>";
                String TableHeadEnd = "</thead>";
                String TableEnd = "</table>";
                String RowStart = "<tr>";
                String RowEnd = "</tr>";
                String ColumnStart = "<td";
                String ColumnEnd = "</td>";
                String TableHeading = TableHead +
                       RowStart;
                String HeadingColumn =
                             thColumnStart + "Practice " + thColumnEnd +
                             thColumnStart + "Facility " + thColumnEnd +
                             thColumnStart + "Provider " + thColumnEnd +
                             thColumnStart + "Account " + thColumnEnd +
                             thColumnStart + "DOB" + thColumnEnd +
                             thColumnStart + "Address" + thColumnEnd +
                             thColumnStart + "ZIP Code" + thColumnEnd +
                             thColumnStart + "Home Phone" + thColumnEnd +
                             thColumnStart + "Cell Phone" + thColumnEnd +
                             thColumnStart + "Service Date" + thColumnEnd +
                             thColumnStart + "Claim Date" + thColumnEnd +
                             thColumnStart + "Claim Number" + thColumnEnd +
                             thColumnStart + "CPT Code" + thColumnEnd +
                             thColumnStart + "Charge Amount" + thColumnEnd +
                             thColumnStart + "Patient Payments" + thColumnEnd +
                            RowEnd + TableHeadEnd;

                sb.Append(PrintDivStart);
                sb.Append(TableStart);
                sb.Append(TableHeading);
                if (Group1.ToLower() == "Patient".ToLower())
                {
                    sb.Append(thColumnStart + "Patient Name " + thColumnEnd);
                    sb.Append(thColumnStart +  "Patient Balance" + thColumnEnd);
                }
                else
                {
                    sb.Append(thColumnStart + " Patient Balance" + thColumnEnd);
                    sb.Append(thColumnStart +  " Patient Name" + thColumnEnd);
                }
                sb.Append(HeadingColumn);
                if (dsReports.Tables[dsReports.DT_ClaimsInCollection.TableName].Rows.Count > 0)
                {

                    var result = from row in dsReports.Tables[dsReports.DT_ClaimsInCollection.TableName].AsEnumerable()
                                 group row by row.Field<string>(Group1.ToLower()) into grp1
                                 select new
                                 {
                                     Group1key = grp1.Key,
                                     Group1Detail = grp1,
                                     Group2Detail = from row in grp1
                                                    group row by row.Field<string>(Group2.ToLower()) into grp2
                                                    select new
                                                    {
                                                        Group2key = grp2.Key,
                                                        Group2Details = grp2
                                                    }
                                 };
                    foreach (var item in result)
                    {
                        bool appendGroup1Name = true;
                        foreach (var subitem in item.Group2Detail)
                        {
                            bool appendGroup2Name = true;
                            foreach (var subitem1 in subitem.Group2Details)
                            {
                                sb.Append(RowStart);
                                if (appendGroup1Name)
                                {
                                    if (Group1.ToLower() == "Patient".ToLower())
                                        sb.Append(ColumnStart + ">" + item.Group1key + ColumnEnd);
                                    else
                                        sb.Append(ColumnStart + "> $" + item.Group1key + ColumnEnd);
                                    appendGroup1Name = false;
                                }
                                else
                                    sb.Append(ColumnStart + ">" + ColumnEnd);
                                if (appendGroup2Name)
                                {
                                    if (Group2.ToLower() == "PatientBalance".ToLower())
                                        sb.Append(ColumnStart + "> $" + subitem.Group2key + ColumnEnd);
                                    else 
                                        sb.Append(ColumnStart + ">" + subitem.Group2key + ColumnEnd);
                                     appendGroup2Name = false;
                                }
                                else
                                    sb.Append(ColumnStart + ">" + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("PracticeName") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Facility") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Provider") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("AccountNumber") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(subitem1.Field<string>("DOB")) + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("HomeAddress") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("ZIPCode") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("HomePhoneNo") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("CellNo") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(subitem1.Field<string>("ServiceDate")) + ColumnEnd);
                                sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(subitem1.Field<string>("ClaimDate")) + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("ClaimNumber") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("CPTCode") + ColumnEnd);
                                sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", subitem1.Field<Double>("ChargeAmount"), "</div>") + ColumnEnd);
                                sb.Append(ColumnStart + "><div style='text-align:right;'>" + string.Format("${0}{1}", subitem1.Field<Double>("PatientPayments"), "</div>") + ColumnEnd);
                                sb.Append(RowEnd);
                            }
                        }
                    }
                    Double TotalChargeAmount = 0, TotalPatientPayment = 0;
                    TotalChargeAmount = dsReports.Tables[dsReports.DT_ClaimsInCollection.TableName].AsEnumerable().Sum(x => x.Field<Double>(7));
                    TotalPatientPayment = dsReports.Tables[dsReports.DT_ClaimsInCollection.TableName].AsEnumerable().Sum(x => x.Field<Double>(6));
                    sb.Append(RowStart);
                    sb.Append(ColumnStart + " colspan='15'><div style='text-align:right;font-weight: bold;'> Total</div>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'><b>" + string.Format("${0}{1}", TotalChargeAmount, "</div>") + "</b>" + ColumnEnd);
                    sb.Append(ColumnStart + "><div style='text-align:right;'><b>" + string.Format("${0}{1}", TotalPatientPayment, "</div>") + "</b>" + ColumnEnd);
                    sb.Append(ColumnStart + ">" + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    // second table


                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadClaimsInCollectionMutlipleGroup", ex);
                return ex.Message;
            }
        }

        //add header insurance
        public String LoadEnterpriseARAnalysisReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_EnterpriseARAnalysis.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    String thColumnStartWithHeader = "<th class='noWordBreak'>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlign = "<td class='text-right'>";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='5' align='center'><b>";
                    String FooterColumnStartTextAlign = "<td class='text-right'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead
                        + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='5' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='4'>" + "Insurance" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='4'>" + "Patient" + thColumnEnd +
                        "<th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart

                        + thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider " + thColumnEnd +
                        thColumnStart + "Plan" + thColumnEnd +
                        thColumnStart + "Month Year" + thColumnEnd +
                        thColumnStartWithHeader + "Charges " + thColumnEnd +
                        thColumnStartWithHeader + "Paid Amount" + thColumnEnd +
                        thColumnStartWithHeader + "Write Off" + thColumnEnd +
                        thColumnStartWithHeader + "Total AR" + thColumnEnd +
                        thColumnStartWithHeader + "Charges" + thColumnEnd +
                        thColumnStartWithHeader + "Paid Amount" + thColumnEnd +
                        thColumnStartWithHeader + "Discount" + thColumnEnd +
                        thColumnStartWithHeader + "Total AR" + thColumnEnd +
                        thColumnStart + "Month AR" + thColumnEnd +
                        thColumnStart + "Total AR" + thColumnEnd +
                        thColumnStart + "Turn Over Ratio" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double InsCharges = 0;
                    double InsPaidAmount = 0;
                    double InsWriteOff = 0;
                    double InsTotalAR = 0;
                    double PatCharges = 0;
                    double PatPaidAmount = 0;
                    double PatDiscount = 0;
                    double PatAR = 0;
                    double MnthAR = 0;
                    double TotalAR = 0;
                    double TurnOvr = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_EnterpriseARAnalysis.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_EnterpriseARAnalysis.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_EnterpriseARAnalysis.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_EnterpriseARAnalysis.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_EnterpriseARAnalysis.YearMonthColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsuranceChargesColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDecimal((dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsuranceChargesColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsuranceChargesColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsurancePaidAmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDecimal((dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsurancePaidAmountColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsurancePaidAmountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsuranceWriteOffColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDecimal((dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsuranceWriteOffColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsuranceWriteOffColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalInsuranceARColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDecimal((dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalInsuranceARColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalInsuranceARColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientChargesColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDecimal((dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientChargesColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientChargesColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientPaidAmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDecimal((dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientPaidAmountColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientPaidAmountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientDiscountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDecimal((dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientDiscountColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientDiscountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalPatientARColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDecimal((dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalPatientARColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalPatientARColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.MonthArColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDecimal((dr[dsReports.DT_Reports_EnterpriseARAnalysis.MonthArColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.MonthArColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalARColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDecimal((dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalARColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalARColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TurnOverRatioColumn]) >= 0 ? String.Format("{0:#######0.###}", MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TurnOverRatioColumn])) : String.Format("({0:#######0.###})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TurnOverRatioColumn])))) + ColumnEnd);

                        sb.Append(RowEnd);

                        InsCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsuranceChargesColumn]);
                        InsPaidAmount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsurancePaidAmountColumn]);
                        InsWriteOff += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.InsuranceWriteOffColumn]);
                        InsTotalAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalInsuranceARColumn]);
                        PatCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientChargesColumn]);
                        PatPaidAmount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientPaidAmountColumn]);
                        PatDiscount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.PatientDiscountColumn]);
                        PatAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalPatientARColumn]);
                        MnthAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.MonthArColumn]);
                        TotalAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TotalARColumn]);
                        TurnOvr += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseARAnalysis.TurnOverRatioColumn]);
                    }
                    InsCharges = MDVUtility.ToAmount(InsCharges);
                    InsPaidAmount = MDVUtility.ToAmount(InsPaidAmount);
                    InsWriteOff = MDVUtility.ToAmount(InsWriteOff);
                    InsTotalAR = MDVUtility.ToAmount(InsTotalAR);
                    PatCharges = MDVUtility.ToAmount(PatCharges);
                    PatPaidAmount = MDVUtility.ToAmount(PatPaidAmount);
                    PatDiscount = MDVUtility.ToAmount(PatDiscount);
                    PatAR = MDVUtility.ToAmount(PatAR);
                    MnthAR = MDVUtility.ToAmount(MnthAR);
                    TotalAR = MDVUtility.ToAmount(TotalAR);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (InsCharges >= 0 ? String.Format("${0:#,###,##0.00}", InsCharges) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsCharges))) + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (InsPaidAmount >= 0 ? String.Format("${0:#,###,##0.00}", InsPaidAmount) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsPaidAmount))) + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (InsWriteOff >= 0 ? String.Format("${0:#,###,##0.00}", InsWriteOff) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsWriteOff))) + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (InsTotalAR >= 0 ? String.Format("${0:#,###,##0.00}", InsTotalAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsTotalAR))) + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatCharges >= 0 ? String.Format("${0:#,###,##0.00}", PatCharges) : String.Format("(${0:#,###,##0.00})", Math.Abs(PatCharges))) + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatPaidAmount >= 0 ? String.Format("${0:#,###,##0.00}", PatPaidAmount) : String.Format("(${0:#,###,##0.00})", Math.Abs(PatPaidAmount))) + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatDiscount >= 0 ? String.Format("${0:#,###,##0.00}", PatDiscount) : String.Format("(${0:#,###,##0.00})", Math.Abs(PatDiscount))) + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatAR >= 0 ? String.Format("${0:#,###,##0.00}", PatAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(PatAR))) + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (MnthAR >= 0 ? String.Format("${0:#,###,##0.00}", MnthAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(MnthAR))) + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (TotalAR >= 0 ? String.Format("${0:#,###,##0.00}", TotalAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(TotalAR))) + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (TurnOvr >= 0 ? String.Format("{0:######0.###}", TurnOvr) : String.Format("({0:######0.###})", Math.Abs(TurnOvr))) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadEnterpriseARAnalysisReport", ex);
                return ex.Message;
            }


        }
        //add header
        public String LoadEnterpriseRevenueReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_EnterpriseRevenue.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnStartWithHeader = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='4' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='6' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='4'>" + "Insurance" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='4'>" + "Patient" + thColumnEnd +
                          "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Copay" + thColumnEnd + RowEnd +
                        RowStart
                        + thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider " + thColumnEnd +
                        thColumnStart + "Plan" + thColumnEnd +
                        thColumnStart + "Fee" + thColumnEnd +
                        thColumnStart + "Expected Fee" + thColumnEnd +
                        thColumnStartWithHeader + "Charges " + thColumnEnd +
                        thColumnStartWithHeader + "Paid Amount" + thColumnEnd +
                        thColumnStartWithHeader + "Write Off" + thColumnEnd +
                        thColumnStartWithHeader + "Balance" + thColumnEnd +
                        thColumnStartWithHeader + "Charges" + thColumnEnd +
                        thColumnStartWithHeader + "Paid Amount" + thColumnEnd +
                        thColumnStartWithHeader + "Discount" + thColumnEnd +
                        thColumnStartWithHeader + "Balance" + thColumnEnd +
                        thColumnStartWithHeader + "Copay" + thColumnEnd +
                        thColumnStartWithHeader + "Paid" + thColumnEnd +
                        thColumnStartWithHeader + "Discount" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double Fee = 0;
                    double ExpctdFee = 0;
                    double InsChrgs = 0;
                    double InsPaidAmt = 0;
                    double InsWriteOff = 0;
                    double InsBal = 0;
                    double PatChrgs = 0;
                    double PatPaidAmt = 0;
                    double PatDiscount = 0;
                    double PatBal = 0;
                    double Copay = 0;
                    double CopayPaid = 0;
                    double CopayDiscount = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_EnterpriseRevenue.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_EnterpriseRevenue.PrcticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_EnterpriseRevenue.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_EnterpriseRevenue.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_EnterpriseRevenue.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.FeeColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.ExpectedFeeColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.InsuranceChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.InsurancePaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.InsuranceWriteOffColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.InsuranceBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.PatientChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.PatientPaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.PatientDiscountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.PatientBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.CopayColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.CopayPaidColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_EnterpriseRevenue.CopayDiscountColumn])) + ColumnEnd);

                        sb.Append(RowEnd);

                        Fee += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.FeeColumn]);
                        ExpctdFee += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.ExpectedFeeColumn]);
                        InsChrgs += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.InsuranceChargesColumn]);
                        InsPaidAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.InsurancePaidAmountColumn]);
                        InsWriteOff += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.InsuranceWriteOffColumn]);
                        InsBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.InsuranceBalanceColumn]);
                        PatChrgs += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.PatientChargesColumn]);
                        PatPaidAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.PatientPaidAmountColumn]);
                        PatDiscount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.PatientDiscountColumn]);
                        PatBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.PatientBalanceColumn]);
                        Copay += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.CopayColumn]);
                        CopayPaid += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.CopayPaidColumn]);
                        CopayDiscount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_EnterpriseRevenue.CopayDiscountColumn]);
                    }
                    Fee = MDVUtility.ToAmount(Fee);
                    ExpctdFee = MDVUtility.ToAmount(ExpctdFee);
                    InsChrgs = MDVUtility.ToAmount(InsChrgs);
                    InsPaidAmt = MDVUtility.ToAmount(InsPaidAmt);
                    InsWriteOff = MDVUtility.ToAmount(InsWriteOff);
                    InsBal = MDVUtility.ToAmount(InsBal);
                    PatChrgs = MDVUtility.ToAmount(PatChrgs);
                    PatPaidAmt = MDVUtility.ToAmount(PatPaidAmt);
                    PatDiscount = MDVUtility.ToAmount(PatDiscount);
                    PatBal = MDVUtility.ToAmount(PatBal);
                    Copay = MDVUtility.ToAmount(Copay);
                    CopayPaid = MDVUtility.ToAmount(CopayPaid);
                    CopayDiscount = MDVUtility.ToAmount(CopayDiscount);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Fee) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", ExpctdFee) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsChrgs) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsPaidAmt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsWriteOff) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatChrgs) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatPaidAmt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatDiscount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Copay) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", CopayPaid) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", CopayDiscount) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadEnterpriseARAnalysisReport", ex);
                return ex.Message;
            }
        }
        public String LoadInsurancePlanARReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_InsurancePlanAR.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlign = "<td class='text-right'>";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='8' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td class='text-right'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Claim Number" + thColumnEnd +
                         thColumnStart + "Practice" + thColumnEnd +
                         thColumnStart + "Facility" + thColumnEnd +
                         thColumnStart + "Provider " + thColumnEnd +
                         thColumnStart + "Plan" + thColumnEnd +
                         thColumnStart + "SubscriberId" + thColumnEnd +
                         thColumnStart + "Month Year" + thColumnEnd +
                         thColumnStart + "Charges " + thColumnEnd +
                         thColumnStart + "Paid Amount" + thColumnEnd +
                         thColumnStart + "Write Off" + thColumnEnd +
                         thColumnStart + "Total Insurance  AR" + thColumnEnd +
                         thColumnStart + "Month AR" + thColumnEnd +
                         thColumnStart + "Total AR" + thColumnEnd +
                         thColumnStart + "Turn Over Ratio" + thColumnEnd +
                         thColumnStart + "Date Of Service" + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double InsChrgs = 0;
                    double InsPaidAmt = 0;
                    double InsWrteOff = 0;
                    double InsAR = 0;
                    double MnthAR = 0;
                    double TotalAR = 0;
                    double TurnOvr = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_InsurancePlanAR.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsurancePlanAR.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsurancePlanAR.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsurancePlanAR.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsurancePlanAR.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsurancePlanAR.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsurancePlanAR.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsurancePlanAR.SubscriberIdColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_InsurancePlanAR.YearMonthColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsurancePlanAR.InsuranceChargesColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_InsurancePlanAR.InsuranceChargesColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsurancePlanAR.InsuranceChargesColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsurancePlanAR.InsurancePaidAmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_InsurancePlanAR.InsurancePaidAmountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsurancePlanAR.InsurancePaidAmountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsurancePlanAR.InsuranceWriteOffColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_InsurancePlanAR.InsuranceWriteOffColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsurancePlanAR.InsuranceWriteOffColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsurancePlanAR.TotalInsuranceARColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_InsurancePlanAR.TotalInsuranceARColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsurancePlanAR.TotalInsuranceARColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsurancePlanAR.MonthARColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_InsurancePlanAR.MonthARColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsurancePlanAR.MonthARColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsurancePlanAR.TotalARColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_InsurancePlanAR.TotalARColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsurancePlanAR.TotalARColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_InsurancePlanAR.TurnOverRatioColumn]) >= 0 ? String.Format("{0:#,###,##0.###}", MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsurancePlanAR.TurnOverRatioColumn])) : String.Format("({0:#,###,##0.###})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_InsurancePlanAR.TurnOverRatioColumn])))) + ColumnEnd);

                        string DOS = MDVUtility.ToStr(dr[dsReports.DT_Reports_InsurancePlanAR.DOSFromColumn]);
                        if (!string.IsNullOrEmpty(DOS))
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(DOS) + ColumnEnd);
                        else
                            sb.Append(ColumnStart + DOS + ColumnEnd);

                        sb.Append(RowEnd);

                        InsChrgs += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsurancePlanAR.InsuranceChargesColumn]);
                        InsPaidAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsurancePlanAR.InsurancePaidAmountColumn]);
                        InsWrteOff += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsurancePlanAR.InsuranceWriteOffColumn]);
                        InsAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsurancePlanAR.TotalInsuranceARColumn]);
                        MnthAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsurancePlanAR.MonthARColumn]);
                        TotalAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsurancePlanAR.TotalARColumn]);
                        TurnOvr += MDVUtility.ToDouble(dr[dsReports.DT_Reports_InsurancePlanAR.TurnOverRatioColumn]);
                    }

                    InsChrgs = MDVUtility.ToAmount(InsChrgs);
                    InsPaidAmt = MDVUtility.ToAmount(InsPaidAmt);
                    InsWrteOff = MDVUtility.ToAmount(InsWrteOff);
                    InsAR = MDVUtility.ToAmount(InsAR);
                    MnthAR = MDVUtility.ToAmount(MnthAR);
                    TotalAR = MDVUtility.ToAmount(TotalAR);
                    TurnOvr = MDVUtility.ToAmount(TurnOvr);

                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (InsChrgs >= 0 ? String.Format("${0:#,###,##0.00}", InsChrgs) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsChrgs))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (InsPaidAmt >= 0 ? String.Format("${0:#,###,##0.00}", InsPaidAmt) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsPaidAmt))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (InsWrteOff >= 0 ? String.Format("${0:#,###,##0.00}", InsWrteOff) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsWrteOff))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (InsAR >= 0 ? String.Format("${0:#,###,##0.00}", InsAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsAR))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (MnthAR >= 0 ? String.Format("${0:#,###,##0.00}", MnthAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(MnthAR))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (TotalAR >= 0 ? String.Format("${0:#,###,##0.00}", TotalAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(TotalAR))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + TurnOvr + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + "" + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadInsurancePlanARReport", ex);
                return ex.Message;
            }


        }
        public String LoadInsuranceARPlanReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Insurance_AR_Plan.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th rowspan='2' class='va-middle noWordBreak'>";
                    String thColumnStartMerge = "<th colspan='2' class='text-center noWordBreak'>";
                    String thColumnStartsplit = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlign = "<td class='text-right'>";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='19' align='center'><b>";
                    String FootercolumnBoldStartEnd = "<td colspan='4' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td class='text-right'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart +
                        thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                         thColumnStart + "Provider" + thColumnEnd +
                         thColumnStart + "Account #" + thColumnEnd +
                         thColumnStart + "Patient" + thColumnEnd +
                         thColumnStart + "DOB" + thColumnEnd +
                         thColumnStart + "Insurance Plan" + thColumnEnd +
                         thColumnStart + "Subscriber ID" + thColumnEnd +
                         thColumnStart + "DOS " + thColumnEnd +
                         thColumnStart + "Claim Number" + thColumnEnd +
                         thColumnStart + "Claim Date" + thColumnEnd +
                         thColumnStart + "Claim Submit Date" + thColumnEnd +
                         thColumnStart + "Status" + thColumnEnd +
                         thColumnStartMerge + "Clinical Note" + thColumnEnd +
                         thColumnStart + "Claim Note" + thColumnEnd +
                         thColumnStart + "Days in AR" + thColumnEnd +
                         thColumnStart + "Insurance Roll Up" + thColumnEnd +
                         thColumnStart + "Insurance Type" + thColumnEnd +
                         thColumnStart + "Billed Amount" + thColumnEnd +
                         thColumnStartMerge + "Payments" + thColumnEnd +
                         thColumnStart + "Adjustment" + thColumnEnd +
                         thColumnStartMerge + "Total AR" + thColumnEnd +
                         thColumnStart + "Status" + thColumnEnd +
                         thColumnStart + "Statement Count" + thColumnEnd +
                         thColumnStart + "Statement Date" + thColumnEnd +
                         thColumnStart + "Collection Agency" + thColumnEnd + RowEnd +
                         
                         RowStart + 
                         thColumnStartsplit + "Modified On" +thColumnEnd +
                         thColumnStartsplit + "Modified By" + thColumnEnd +
                         thColumnStartsplit + "Insurance Payment" + thColumnEnd +
                         thColumnStartsplit + "Patient Payment" + thColumnEnd +
                         thColumnStartsplit + "Insurance AR" + thColumnEnd +
                         thColumnStartsplit + "Patient AR" + thColumnEnd +
                         RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double BilledAmountColumn = 0;
                    double InsurancePaymentsColumn = 0;
                    double PatientPaymentsColumn = 0;
                    double AdjustmentColumn = 0;
                    double InsuranceARColumn = 0;
                    double PatientARColumn = 0;
                    
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Insurance_AR_Plan.TableName].Rows)
                    {
                        sb.Append(RowStart);

                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_Insurance_AR_Plan.DOBColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.SubscriberIdColumn] + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_Insurance_AR_Plan.DOSFromColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_Insurance_AR_Plan.CreatedOnColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_Insurance_AR_Plan.SubmittedDateColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Insurance_AR_Plan.NoteModifiedOnColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.NoteModifiedByColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.ClaimNoteColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.DaysInArColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.InsuranceRollUpColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.InsuranceTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Insurance_AR_Plan.BilledAmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Insurance_AR_Plan.BilledAmountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Insurance_AR_Plan.BilledAmountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Insurance_AR_Plan.InsurancePaymentsColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Insurance_AR_Plan.InsurancePaymentsColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Insurance_AR_Plan.InsurancePaymentsColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Insurance_AR_Plan.PatientPaymentsColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Insurance_AR_Plan.PatientPaymentsColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Insurance_AR_Plan.PatientPaymentsColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Insurance_AR_Plan.AdjustmentColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Insurance_AR_Plan.AdjustmentColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Insurance_AR_Plan.AdjustmentColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Insurance_AR_Plan.InsuranceARColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Insurance_AR_Plan.InsuranceARColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Insurance_AR_Plan.InsuranceARColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Insurance_AR_Plan.PatientARColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Insurance_AR_Plan.PatientARColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Insurance_AR_Plan.PatientARColumn])))) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Insurance_AR_Plan.StatementCountColumn] + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_Insurance_AR_Plan.StatementDateColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_Insurance_AR_Plan.CollectionAgencyColumn])) + ColumnEnd);

                        sb.Append(RowEnd);

                        BilledAmountColumn += MDVUtility.ToDouble(dr[dsReports.DT_Insurance_AR_Plan.BilledAmountColumn]);
                        InsurancePaymentsColumn += MDVUtility.ToDouble(dr[dsReports.DT_Insurance_AR_Plan.InsurancePaymentsColumn]);
                        PatientPaymentsColumn += MDVUtility.ToDouble(dr[dsReports.DT_Insurance_AR_Plan.PatientPaymentsColumn]);
                        AdjustmentColumn += MDVUtility.ToDouble(dr[dsReports.DT_Insurance_AR_Plan.AdjustmentColumn]);
                        InsuranceARColumn += MDVUtility.ToDouble(dr[dsReports.DT_Insurance_AR_Plan.InsuranceARColumn]);
                        PatientARColumn += MDVUtility.ToDouble(dr[dsReports.DT_Insurance_AR_Plan.PatientARColumn]);
                    }

                    BilledAmountColumn = MDVUtility.ToAmount(BilledAmountColumn);
                    InsurancePaymentsColumn = MDVUtility.ToAmount(InsurancePaymentsColumn);
                    PatientPaymentsColumn = MDVUtility.ToAmount(PatientPaymentsColumn);
                    AdjustmentColumn = MDVUtility.ToAmount(AdjustmentColumn);
                    InsuranceARColumn = MDVUtility.ToAmount(InsuranceARColumn);
                    PatientARColumn = MDVUtility.ToAmount(PatientARColumn);
                    

                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (BilledAmountColumn >= 0 ? String.Format("${0:#,###,##0.00}", BilledAmountColumn) : String.Format("(${0:#,###,##0.00})", Math.Abs(BilledAmountColumn))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (InsurancePaymentsColumn >= 0 ? String.Format("${0:#,###,##0.00}", InsurancePaymentsColumn) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsurancePaymentsColumn))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (PatientPaymentsColumn >= 0 ? String.Format("${0:#,###,##0.00}", PatientPaymentsColumn) : String.Format("(${0:#,###,##0.00})", Math.Abs(PatientPaymentsColumn))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (AdjustmentColumn >= 0 ? String.Format("${0:#,###,##0.00}", AdjustmentColumn) : String.Format("(${0:#,###,##0.00})", Math.Abs(AdjustmentColumn))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (InsuranceARColumn >= 0 ? String.Format("${0:#,###,##0.00}", InsuranceARColumn) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsuranceARColumn))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (PatientARColumn >= 0 ? String.Format("${0:#,###,##0.00}", PatientARColumn) : String.Format("(${0:#,###,##0.00})", Math.Abs(PatientARColumn))) + FootercolumnBoldEnd);

                    sb.Append(FootercolumnBoldStartEnd + "" + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadInsurancePlanARReport", ex);
                return ex.Message;
            }


        }
        public String LoadPatientARReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_PatientAR.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlign = "<td class='text-right'>";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='6' align='center'><b>";
                    String FooterColumnStartTextAlign = "<td class='text-right'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='6' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Charges" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Paid" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Discount" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='border-top: #fff solid 1px;border-left: #fff solid 1px;' colspan='2'>" + "" + thColumnEnd + RowEnd +
                        RowStart +
                        thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider " + thColumnEnd +
                        thColumnStart + "Claim Number" + thColumnEnd +
                        thColumnStart + "Plan" + thColumnEnd +
                        thColumnStart + "Month/Year" + thColumnEnd +
                        thColumnStart + "Patient " + thColumnEnd +
                        thColumnStart + "Copay" + thColumnEnd +
                        thColumnStart + "Total" + thColumnEnd +
                        thColumnStart + "Patient" + thColumnEnd +
                         thColumnStart + "Copay" + thColumnEnd +
                         thColumnStart + "Total" + thColumnEnd +
                        thColumnStart + "Patient" + thColumnEnd +
                        thColumnStart + "Copay" + thColumnEnd +
                        thColumnStart + "Total" + thColumnEnd +
                        thColumnStart + "Total Patient AR" + thColumnEnd +
                       // thColumnStart + "Turn Over Ratio" + thColumnEnd + 
                       TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double PatChrgs = 0;
                    double CopayChrgs = 0;
                    double PatPaid = 0;
                    double CopayPaid = 0;
                    double Discount = 0;
                    double CopayDiscount = 0;
                    double TotalPatAR = 0;
                    double TotalCharge = 0;
                    double TotalPaid = 0;
                    double TotalDiscount = 0;
                    //double TurnOverRatio = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_PatientAR.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientAR.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientAR.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientAR.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientAR.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientAR.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PatientAR.YearMonthColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.PatientChargesColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.PatientChargesColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.PatientChargesColumn])))) + ColumnEnd);
                        //sb.Append(ColumnStart + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.PatientChargesColumn])) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.PatientChargesColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.PatientChargesColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.CopayChargesColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.CopayChargesColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.CopayChargesColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.TotalChargesColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.TotalChargesColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.TotalChargesColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.PatientPaidAmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.PatientPaidAmountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.PatientPaidAmountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.CopayPaidColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.CopayPaidColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.CopayPaidColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.TotalPatientPaidColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.TotalPatientPaidColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.TotalPatientPaidColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.PatientDiscountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.PatientDiscountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.PatientDiscountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.CopayDiscountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.CopayDiscountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.CopayDiscountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.TotalDiscountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.TotalDiscountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.TotalDiscountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PatientAR.TotalPatientARColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PatientAR.TotalPatientARColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.TotalPatientARColumn])))) + ColumnEnd);

                       // sb.Append(ColumnStart + String.Format("{0:#,###,##0.00}%", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PatientAR.TurnOverRatioColumn])) * 100) + ColumnEnd);

                        sb.Append(RowEnd);
                        PatChrgs += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.PatientChargesColumn]);
                        CopayChrgs += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.CopayChargesColumn]);
                        PatPaid += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.PatientPaidAmountColumn]);
                        CopayPaid += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.CopayPaidColumn]);
                        Discount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.PatientDiscountColumn]);
                        CopayDiscount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.CopayDiscountColumn]);
                        TotalPatAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.TotalPatientARColumn]);
                        TotalCharge += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.TotalChargesColumn]);
                        TotalPaid += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.TotalPatientPaidColumn]);
                        TotalDiscount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.TotalPatientPaidColumn]);
                      // TurnOverRatio += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PatientAR.TurnOverRatioColumn]);

                    }
                    PatChrgs = MDVUtility.ToAmount(PatChrgs);
                    CopayChrgs = MDVUtility.ToAmount(CopayChrgs);
                    TotalCharge = MDVUtility.ToAmount(TotalCharge);
                    PatPaid = MDVUtility.ToAmount(PatPaid);
                    CopayPaid = MDVUtility.ToAmount(CopayPaid);
                    TotalPaid = MDVUtility.ToAmount(TotalPaid);
                    Discount = MDVUtility.ToAmount(Discount);
                    CopayDiscount = MDVUtility.ToAmount(CopayDiscount);
                    TotalDiscount = MDVUtility.ToAmount(TotalDiscount);
                    TotalPatAR = MDVUtility.ToAmount(TotalPatAR);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatChrgs >= 0 ? String.Format("${0:#,###,##0.00}", PatChrgs) : String.Format("(${0:#,###,##0.00})", Math.Abs(PatChrgs))) + ColumnEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatChrgs >= 0 ? String.Format("${0:#,###,##0.00}", CopayChrgs) : String.Format("(${0:#,###,##0.00})", Math.Abs(CopayChrgs))) + ColumnEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatChrgs >= 0 ? String.Format("${0:#,###,##0.00}", TotalCharge) : String.Format("(${0:#,###,##0.00})", Math.Abs(TotalCharge))) + ColumnEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatChrgs >= 0 ? String.Format("${0:#,###,##0.00}", PatPaid) : String.Format("(${0:#,###,##0.00})", Math.Abs(PatPaid))) + ColumnEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatChrgs >= 0 ? String.Format("${0:#,###,##0.00}", CopayPaid) : String.Format("(${0:#,###,##0.00})", Math.Abs(CopayPaid))) + ColumnEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatChrgs >= 0 ? String.Format("${0:#,###,##0.00}", TotalPaid) : String.Format("(${0:#,###,##0.00})", Math.Abs(TotalPaid))) + ColumnEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatChrgs >= 0 ? String.Format("${0:#,###,##0.00}", Discount) : String.Format("(${0:#,###,##0.00})", Math.Abs(Discount))) + ColumnEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatChrgs >= 0 ? String.Format("${0:#,###,##0.00}", CopayDiscount) : String.Format("(${0:#,###,##0.00})", Math.Abs(CopayDiscount))) + ColumnEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatChrgs >= 0 ? String.Format("${0:#,###,##0.00}", TotalDiscount) : String.Format("(${0:#,###,##0.00})", Math.Abs(TotalDiscount))) + ColumnEnd);
                    sb.Append(FooterColumnStartTextAlign + (PatChrgs >= 0 ? String.Format("${0:#,###,##0.00}", TotalPatAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(TotalPatAR))) + ColumnEnd);
                   // sb.Append(ColumnStart + String.Format("{0:#,###,##0.00}%", Math.Abs(MDVUtility.ToDecimal(TurnOverRatio)) * 100) + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }
        public String LoadPaymentEntriesReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_PaymentEnteries.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlign = "<td class='text-right'>";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='16' align='right'><b>";
                    String FooterColumnBoldStartTextAlign = "<td class='text-right'><b>";
                    String FootercolumnBoldStart1 = "<td colspan='12'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility " + thColumnEnd +
                        thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Insurance Plan" + thColumnEnd +
                        thColumnStart + "Account" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Claim Number" + thColumnEnd +
                        thColumnStart + "CPT Code" + thColumnEnd +
                        thColumnStart + "Visit Date " + thColumnEnd +
                        thColumnStart + "Check Number" + thColumnEnd +
                          thColumnStart + "Payment Type" + thColumnEnd +
                        thColumnStart + "Apply To" + thColumnEnd +
                        thColumnStart + "System Category" + thColumnEnd +
                        thColumnStart + "Ledger Description" + thColumnEnd +
                         thColumnStart + "Payment Ledger Type" + thColumnEnd +
                        thColumnStart + "Date Paid" + thColumnEnd +
                              thColumnStart + "Amount" + thColumnEnd +
                                thColumnStart + "Entry Date" + thColumnEnd +
                                  thColumnStart + "Entered By" + thColumnEnd +
                                     TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double InsChrg = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_PaymentEnteries.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.CptCodeColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_PaymentEnteries.VisitDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_PaymentEnteries.VisitDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.VisitDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.CheckNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.PaymentTypeColumn] + ColumnEnd);

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.ApplyToColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.SystemCategoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.LedgerAccDescriptionColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.LedgerAccTypeColumn] + ColumnEnd);

                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_PaymentEnteries.DatePaidColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_PaymentEnteries.DatePaidColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.DatePaidColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PaymentEnteries.AmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PaymentEnteries.AmountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PaymentEnteries.AmountColumn])))) + ColumnEnd);

                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_PaymentEnteries.EntryDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_PaymentEnteries.EntryDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.EntryDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.EnteredByColumn] + ColumnEnd);

                        sb.Append(RowEnd);

                        InsChrg += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PaymentEnteries.AmountColumn]);
                    }
                    InsChrg = MDVUtility.ToAmount(InsChrg);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FooterColumnBoldStartTextAlign + (InsChrg >= 0 ? String.Format("${0:#,###,##0.00}", InsChrg) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsChrg))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + "" + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPaymentEntriesReport", ex);
                return ex.Message;
            }


        }
        public String LoadProviderAnalysisByPlanReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='7' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider " + thColumnEnd +
                        thColumnStart + "Plan" + thColumnEnd +
                        thColumnStart + "Plan Category" + thColumnEnd +
                        thColumnStart + "# Claims" + thColumnEnd +
                        thColumnStart + "# Patients " + thColumnEnd +
                        thColumnStart + "Ins Charges" + thColumnEnd +
                        thColumnStart + "Paid Amount" + thColumnEnd +
                        thColumnStart + "Write Off" + thColumnEnd +
                        thColumnStart + "Balance" + thColumnEnd +
                        thColumnStart + "Primary Charge ID" + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    double InsChrg = 0;
                    double InsPaidAmt = 0;
                    double InsWriteOff = 0;
                    double InsBal = 0;

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.PlanCategoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.NumberOfDOSColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.NumberOfPatientsColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.InsuranceChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.InsurancePaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.InsuranceWriteOffColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.InsuranceBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.MasterChargeIdColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                        InsChrg += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.InsuranceChargesColumn]);
                        InsPaidAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.InsurancePaidAmountColumn]);
                        InsWriteOff += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.InsuranceWriteOffColumn]);
                        InsBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Provider_Analysis_By_InsurancePlan.InsuranceBalanceColumn]);
                    }
                    InsChrg = MDVUtility.ToAmount(InsChrg);
                    InsPaidAmt = MDVUtility.ToAmount(InsPaidAmt);
                    InsWriteOff = MDVUtility.ToAmount(InsWriteOff);
                    InsBal = MDVUtility.ToAmount(InsBal);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsChrg) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsPaidAmt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsWriteOff) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsBal) + FootercolumnBoldEnd);
                    sb.Append(ColumnStart + "" + ColumnEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadProviderAnalysisByPlanReport", ex);
                return ex.Message;
            }


        }
        public String LoadProviderProcedureUtilizationReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Provider_Procedure_Utilization.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='7' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td colspan='3' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + thColumnStart + "Practice" + thColumnEnd +
                       thColumnStart + "Facility" + thColumnEnd +
                       thColumnStart + "Provider " + thColumnEnd +
                        thColumnStart + "Visit Date From" + thColumnEnd +
                        thColumnStart + "Visit Date To" + thColumnEnd +
                        thColumnStart + "CPT Code " + thColumnEnd +
                        thColumnStart + "CPT Code Description" + thColumnEnd +
                        thColumnStart + "Amount" + thColumnEnd +
                        thColumnStart + "Charges Average" + thColumnEnd +
                        thColumnStart + "Charges Percentage" + thColumnEnd +
                        thColumnStart + "Units " + thColumnEnd +
                        thColumnStart + "Percentage Units " + thColumnEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double ChrgAmt = 0;
                    double ChrgAvg = 0;

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Provider_Procedure_Utilization.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Procedure_Utilization.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Procedure_Utilization.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Procedure_Utilization.ProviderNameColumn] + ColumnEnd);

                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Provider_Procedure_Utilization.VisitDateFromColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Provider_Procedure_Utilization.VisitDateFromColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Procedure_Utilization.VisitDateFromColumn] + ColumnEnd);//
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Provider_Procedure_Utilization.VisitDateToColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Provider_Procedure_Utilization.VisitDateToColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Procedure_Utilization.VisitDateToColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Procedure_Utilization.CPTCodeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Procedure_Utilization.CPTCodeDescriptionColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_Provider_Procedure_Utilization.ChargeAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_Provider_Procedure_Utilization.ChargesAverageColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Procedure_Utilization.ChargesPercentageColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Provider_Procedure_Utilization.UnitsColumn] + ColumnEnd);
                        sb.Append(ColumnStart + String.Format("{0:#,###,##0.00}%", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_Provider_Procedure_Utilization.PercentageUnitsColumn])) * 100) + ColumnEnd);

                        sb.Append(RowEnd);
                        ChrgAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Provider_Procedure_Utilization.ChargeAmountColumn]);
                        ChrgAvg += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Provider_Procedure_Utilization.ChargesAverageColumn]);
                    }
                    ChrgAmt = MDVUtility.ToAmount(ChrgAmt);
                    ChrgAvg = MDVUtility.ToAmount(ChrgAvg);

                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", ChrgAmt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", ChrgAvg) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + "" + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadProviderProcedureUtilizationReport", ex);
                return ex.Message;
            }


        }
        //add header
        public String LoadRevenueByFacilityReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Revenue_By_Facility.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnStartWithHeader = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead
                        + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='6'>" + "Insurance" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='5'>" + "Patient" + thColumnEnd + RowEnd +
                        RowStart
                    + thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider " + thColumnEnd +
                        thColumnStartWithHeader + "Plan" + thColumnEnd +
                        thColumnStartWithHeader + "Charges" + thColumnEnd +
                         thColumnStartWithHeader + "Expected Amount" + thColumnEnd +
                          thColumnStartWithHeader + "Paid Amount" + thColumnEnd +
                           thColumnStartWithHeader + "Write Off" + thColumnEnd +
                            thColumnStartWithHeader + "Balance" + thColumnEnd +
                        thColumnStartWithHeader + "Charges" + thColumnEnd +

                          thColumnStartWithHeader + "Paid Amount" + thColumnEnd +
                           thColumnStartWithHeader + "Discount" + thColumnEnd +
                            thColumnStartWithHeader + "Balance" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Revenue_By_Facility.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_Facility.PracticeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_Facility.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_Facility.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_Facility.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Facility.InsuranceChargesColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Facility.InsuranceExpectedAmountColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Facility.InsurancePaidAmountColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Facility.InsuranceWriteOffColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Facility.InsuranceBalanceColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Facility.PatientChargesColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Facility.PatientPaidAmountColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Facility.PatientDiscountColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Facility.PatientBalanceColumn]) + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByFacilityReport", ex);
                return ex.Message;
            }


        }

        public String LoadRevenueByProviderReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Revenue_By_Provider.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='6'>" + "Insurance" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='4'>" + "Patient" + thColumnEnd + RowEnd +
                        RowStart
                        + thColumnStart + "Practice" + thColumnEnd +
                          thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Provider " + thColumnEnd +
                          thColumnStart + "Plan" + thColumnEnd +
                          thColumnStart + "Charges" + thColumnEnd +
                           thColumnStart + "Expected Amount" + thColumnEnd +
                            thColumnStart + "Paid Amount" + thColumnEnd +
                             thColumnStart + "Write Off" + thColumnEnd +
                              thColumnStart + "Balance" + thColumnEnd +
                            thColumnStart + "Paid Amount" + thColumnEnd +
                             thColumnStart + "Discount" + thColumnEnd +
                              thColumnStart + "Balance" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Revenue_By_Provider.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_Provider.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_Provider.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_Provider.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_Provider.InsurancePlanNameColumn] + ColumnEnd);

                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Provider.InsuranceChargesColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Provider.InsuranceExpectedAmountColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Provider.InsurancePaidAmountColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Provider.InsuranceWriteOffColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Provider.InsuranceBalanceColumn]) + ColumnEnd);

                        //    sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_Provider.PatientChargesColumn] + ColumnEnd);

                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Provider.PatientPaidAmountColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Provider.PatientDiscountColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_Provider.PatientBalanceColumn]) + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }


        }

        public String LoadARAgingAnalysisReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_ARAging_Analysis.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    String thColumnStartWithHeader = "<th class='noWordBreak'>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='4' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead
                         + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='6' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Insurance" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='5'>" + "Insurance AR" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Patient" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='5'>" + "Patient AR" + thColumnEnd + RowEnd +
                        RowStart
                        + thColumnStart + "Practice" + thColumnEnd +
                          thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Provider " + thColumnEnd +
                          thColumnStart + "DOS" + thColumnEnd +
                          thColumnStart + "Plan" + thColumnEnd +
                          thColumnStartWithHeader + "Billed Charges" + thColumnEnd +
                          thColumnStartWithHeader + "Paid Amount" + thColumnEnd +
                          thColumnStartWithHeader + "Write Off" + thColumnEnd +
                          thColumnStartWithHeader + "AR" + thColumnEnd +
                          thColumnStartWithHeader + "Current" + thColumnEnd +
                          thColumnStartWithHeader + "AR 30+" + thColumnEnd +
                          thColumnStartWithHeader + "AR 60+" + thColumnEnd +
                          thColumnStartWithHeader + "AR 90+" + thColumnEnd +
                          thColumnStartWithHeader + "AR 120+" + thColumnEnd +
                          thColumnStartWithHeader + "Paid Amount" + thColumnEnd +
                          thColumnStartWithHeader + "Discount" + thColumnEnd +
                          thColumnStartWithHeader + "AR" + thColumnEnd +
                          thColumnStartWithHeader + "Current" + thColumnEnd +
                          thColumnStartWithHeader + "AR 30+" + thColumnEnd +
                          thColumnStartWithHeader + "AR 60+" + thColumnEnd +
                          thColumnStartWithHeader + "AR 90+" + thColumnEnd +
                          thColumnStartWithHeader + "AR 120+" + thColumnEnd +
                          thColumnStartWithHeader + "Total AR+" + thColumnEnd + RowEnd +
                          TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double InsCharges = 0;
                    double InsPaidAmount = 0;
                    double InsWriteOff = 0;
                    double TotalAR = 0;
                    double TotalARCurrent = 0;
                    double AR30 = 0;
                    double AR60 = 0;
                    double AR90 = 0;
                    double AR120 = 0;
                    double PatCharges = 0;
                    double PatPaidAmount = 0;
                    double Discount = 0;
                    double PatTotalAR = 0;
                    double PatTotalARCurrent = 0;
                    double PatAR30 = 0;
                    double PatAR60 = 0;
                    double PatAR90 = 0;
                    double PatAR120 = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_ARAging_Analysis.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_ARAging_Analysis.DOSColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_ARAging_Analysis.DOSColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.DOSColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.PlanColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.BilledChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.InsurancePaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.InsuranceWriteOffColumn])) + ColumnEnd);

                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARCurrentColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR30_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR60_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR90_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR120_Column])) + ColumnEnd);

                        //sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.PatientChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.PatientPaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.PatientDiscountColumn])) + ColumnEnd);

                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalPatientARColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.PatientARCurrentColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR30_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR60_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR90_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR120_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalARColumn])) + ColumnEnd);
                        sb.Append(RowEnd);
                        InsCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.BilledChargesColumn]);
                        InsPaidAmount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.InsurancePaidAmountColumn]);
                        InsWriteOff += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.InsuranceWriteOffColumn]);
                        TotalAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARColumn]);
                        TotalARCurrent += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARCurrentColumn]);
                        AR30 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR30_Column]);
                        AR60 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR60_Column]);
                        AR90 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR90_Column]);
                        AR120 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR120_Column]);
                        //PatCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientChargesColumn]);
                        PatPaidAmount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientPaidAmountColumn]);
                        Discount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientDiscountColumn]);
                        PatTotalAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalPatientARColumn]);
                        PatTotalARCurrent += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientARCurrentColumn]);
                        PatAR30 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR30_Column]);
                        PatAR60 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR60_Column]);
                        PatAR90 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR90_Column]);
                        PatAR120 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR120_Column]);
                        PatCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalARColumn]);
                    }
                    InsCharges = MDVUtility.ToAmount(InsCharges);
                    InsPaidAmount = MDVUtility.ToAmount(InsPaidAmount);
                    InsWriteOff = MDVUtility.ToAmount(InsWriteOff);
                    TotalAR = MDVUtility.ToAmount(TotalAR);
                    TotalARCurrent = MDVUtility.ToAmount(TotalARCurrent);
                    AR30 = MDVUtility.ToAmount(AR30);
                    AR60 = MDVUtility.ToAmount(AR60);
                    AR90 = MDVUtility.ToAmount(AR90);
                    AR120 = MDVUtility.ToAmount(AR120);
                    PatCharges = MDVUtility.ToAmount(PatCharges);
                    PatPaidAmount = MDVUtility.ToAmount(PatPaidAmount);
                    Discount = MDVUtility.ToAmount(Discount);
                    PatTotalAR = MDVUtility.ToAmount(PatTotalAR);
                    PatTotalARCurrent = MDVUtility.ToAmount(PatTotalARCurrent);
                    PatAR30 = MDVUtility.ToAmount(PatAR30);
                    PatAR60 = MDVUtility.ToAmount(PatAR60);
                    PatAR90 = MDVUtility.ToAmount(PatAR90);
                    PatAR120 = MDVUtility.ToAmount(PatAR120);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsCharges) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsPaidAmount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsWriteOff) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", TotalAR) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", TotalARCurrent) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR30) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR60) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR90) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR120) + FootercolumnBoldEnd);
                    //sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatCharges) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatPaidAmount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Discount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatTotalAR) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatTotalARCurrent) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR30) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR60) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR90) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR120) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatCharges) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadARAgingAnalysisReport", ex);
                return ex.Message;
            }


        }

        public String LoadRevenueByPlanReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Revenue_By_InsurancePlan.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    String thColumnStartWithHeader = "<th class='noWordBreak'>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlign = "<td class='text-right'>";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='4' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td class='text-right'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead
                              + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                             "<th class='noWordBreak text-center' style='background: #468cec' colspan='6'>" + "Insurance" + thColumnEnd +
                             "<th class='noWordBreak text-center' style='background: #468cec' colspan='4'>" + "Patient" + thColumnEnd + RowEnd +
                             RowStart +
                             thColumnStart + "Practice" + thColumnEnd +
                              thColumnStart + "Facility" + thColumnEnd +
                              thColumnStart + "Provider " + thColumnEnd +
                               thColumnStartWithHeader + "Insurance Plan " + thColumnEnd +
                              thColumnStartWithHeader + "Charges" + thColumnEnd +
                              thColumnStartWithHeader + "Expected Amount" + thColumnEnd +
                              thColumnStartWithHeader + "Paid Amount" + thColumnEnd +
                              thColumnStartWithHeader + "Write Off" + thColumnEnd +
                              thColumnStartWithHeader + "Balance" + thColumnEnd +
                              thColumnStartWithHeader + "Charges" + thColumnEnd +
                              thColumnStartWithHeader + "Paid Amount" + thColumnEnd +
                              thColumnStartWithHeader + "Discount" + thColumnEnd +
                              thColumnStartWithHeader + "Balance" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double SumInsChrg = 0;
                    double SumInsExpctAmt = 0;
                    double SumInsPaidAmt = 0;
                    double SumInsWrtOff = 0;
                    double SumInsBal = 0;
                    double SumPatChrg = 0;
                    double SumPatPaidAmt = 0;
                    double SumPatDscnt = 0;
                    double SumPatBal = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Revenue_By_InsurancePlan.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceChargesColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceChargesColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceChargesColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceExpectedAmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceExpectedAmountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceExpectedAmountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsurancePaidAmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsurancePaidAmountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsurancePaidAmountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceWriteOffColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceWriteOffColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceWriteOffColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceBalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientChargesColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientChargesColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientChargesColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientPaidAmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientPaidAmountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientPaidAmountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientDiscountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientDiscountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientDiscountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientBalanceColumn])))) + ColumnEnd);
                        sb.Append(RowEnd);
                        SumInsChrg += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceChargesColumn]);
                        SumInsExpctAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceExpectedAmountColumn]);
                        SumInsPaidAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsurancePaidAmountColumn]);
                        SumInsWrtOff += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceWriteOffColumn]);
                        SumInsBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.InsuranceBalanceColumn]);
                        SumPatChrg += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientChargesColumn]);
                        SumPatPaidAmt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientPaidAmountColumn]);
                        SumPatDscnt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientDiscountColumn]);
                        SumPatBal += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Revenue_By_InsurancePlan.PatientBalanceColumn]);
                    }
                    SumInsChrg = MDVUtility.ToAmount(SumInsChrg);
                    SumInsExpctAmt = MDVUtility.ToAmount(SumInsExpctAmt);
                    SumInsPaidAmt = MDVUtility.ToAmount(SumInsPaidAmt);
                    SumInsWrtOff = MDVUtility.ToAmount(SumInsWrtOff);
                    SumInsBal = MDVUtility.ToAmount(SumInsBal);
                    SumPatChrg = MDVUtility.ToAmount(SumPatChrg);
                    SumPatPaidAmt = MDVUtility.ToAmount(SumPatPaidAmt);
                    SumPatDscnt = MDVUtility.ToAmount(SumPatDscnt);
                    SumPatBal = MDVUtility.ToAmount(SumPatBal);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumInsChrg >= 0 ? String.Format("${0:#,###,##0.00}", SumInsChrg) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumInsChrg))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumInsExpctAmt >= 0 ? String.Format("${0:#,###,##0.00}", SumInsExpctAmt) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumInsExpctAmt))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumInsPaidAmt >= 0 ? String.Format("${0:#,###,##0.00}", SumInsPaidAmt) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumInsPaidAmt))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumInsWrtOff >= 0 ? String.Format("${0:#,###,##0.00}", SumInsWrtOff) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumInsWrtOff))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumInsBal >= 0 ? String.Format("${0:#,###,##0.00}", SumInsBal) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumInsBal))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumPatChrg >= 0 ? String.Format("${0:#,###,##0.00}", SumPatChrg) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumPatChrg))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumPatPaidAmt >= 0 ? String.Format("${0:#,###,##0.00}", SumPatPaidAmt) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumPatPaidAmt))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumPatDscnt >= 0 ? String.Format("${0:#,###,##0.00}", SumPatDscnt) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumPatDscnt))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumPatBal >= 0 ? String.Format("${0:#,###,##0.00}", SumPatBal) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumPatBal))) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByPlanReport", ex);
                return ex.Message;
            }


        }

        public String LoadFinancialAnalysisAtCPTLevel(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Financial_Analysis_At_CPT.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnStartWithHeader = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='6' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                        //    "<th class='noWordBreak text-center' style='background: #468cec' colspan='4'>" + "Insurance" + thColumnEnd +
                        //    "<th class='noWordBreak text-center' style='background: #468cec' colspan='4'>" + "Patient" + thColumnEnd +
                        //      "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Copay" + thColumnEnd + RowEnd +
                    RowStart
                        + thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Insurance" + thColumnEnd +
                        thColumnStart + "Appointment Provider" + thColumnEnd +
                        thColumnStart + "CPT" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Patient Account No" + thColumnEnd +
                        thColumnStartWithHeader + "Service Date" + thColumnEnd +
                        thColumnStartWithHeader + "Claim Date" + thColumnEnd +
                        thColumnStartWithHeader + "Claim No" + thColumnEnd +
                        thColumnStartWithHeader + "ICD 1" + thColumnEnd +
                        thColumnStartWithHeader + "ICD 2" + thColumnEnd +
                        thColumnStartWithHeader + "ICD 3" + thColumnEnd +
                        thColumnStartWithHeader + "Billed Charged" + thColumnEnd +
                        thColumnStartWithHeader + "Allowed Fee" + thColumnEnd +
                        thColumnStartWithHeader + "Payment Paid" + thColumnEnd +
                        thColumnStartWithHeader + "Patient Payments" + thColumnEnd +
                        thColumnStartWithHeader + "Insurance Payment" + thColumnEnd +
                        thColumnStartWithHeader + "Total Adjustments" + thColumnEnd +
                        thColumnStartWithHeader + "Insurance Withheld" + thColumnEnd +
                        thColumnStartWithHeader + "Contractual Adjustments" + thColumnEnd +
                        thColumnStartWithHeader + "Write Off Adjustments" + thColumnEnd +
                        thColumnStartWithHeader + "Refunds" + thColumnEnd +
                        thColumnStartWithHeader + "Billed Units" + thColumnEnd +
                        thColumnStartWithHeader + "Procedure Count" + thColumnEnd +
                        thColumnStartWithHeader + "Visit Count" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Financial_Analysis_At_CPT.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.InsuranceColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.AppProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.CPTCodeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.ChargeDOSColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.ClaimEntryDateColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.ClaimNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.ICDCode1Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.ICDCode2Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.ICDCode3Column] + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.FeeColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.ExpectedFeeColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.PaymentPaidColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.PatientPaymentColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.InsurancePaidamountColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.TotalAdjustmentsColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.InsuranceWithheldColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.ContractualAdjustmentsColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.WriteOffAdjColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.refundColumn]) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.BilledUnitsColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.ProcedureCountColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Financial_Analysis_At_CPT.VisitCountColumn] + ColumnEnd);

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadFinancialAnalysisAtCPTLevel", ex);
                return ex.Message;
            }
        }

        public String LoadBeginningAREndingAR(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Report_Beginning_AR_Ending_AR.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartRightAlign = "<td style=text-align:right>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td style=text-align:right><b>$";
                    String FootercolumnBold = "<td class='text-right'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Beginning AR" + thColumnEnd +
                          thColumnStart + "Billed Charges" + thColumnEnd +
                          thColumnStart + "Payment" + thColumnEnd +
                          thColumnStart + "Insurance Payment" + thColumnEnd +
                          thColumnStart + "Patient Payment" + thColumnEnd +
                           thColumnStart + "Adjustments" + thColumnEnd +
                           thColumnStart + "Ending AR" + thColumnEnd +
                           thColumnStart + "Change in AR" + thColumnEnd +
                            thColumnStart + "Claim Count" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double SumBegingAR = 0;
                    double SumBilledChrg = 0;
                    double Sumpayments = 0;
                    double SumInspayments = 0;
                    double SumPatpayments = 0;
                    double Sumadjstmnt = 0;
                    double SumEndingAR = 0;
                    double SumChrgAR = 0;
                    double ClaimCount = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Report_Beginning_AR_Ending_AR.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Beginning_AR_Ending_AR.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.BeginingArColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.BilledChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.PaymentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.InsPaymentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.PatPaymentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.AdjustmentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.EndingArColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.ChangeArColumn])) + ColumnEnd);
                        sb.Append(ColumnStartRightAlign + dr[dsReports.DT_Report_Beginning_AR_Ending_AR.CountedClaimsColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                        SumBegingAR += MDVUtility.ToDouble(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.BeginingArColumn]);
                        SumBilledChrg += MDVUtility.ToDouble(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.BilledChargesColumn]);
                        Sumpayments += MDVUtility.ToDouble(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.PaymentsColumn]);
                        Sumadjstmnt += MDVUtility.ToDouble(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.AdjustmentsColumn]);
                        SumEndingAR += MDVUtility.ToDouble(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.EndingArColumn]);
                        SumChrgAR += MDVUtility.ToDouble(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.ChangeArColumn]);
                        ClaimCount += MDVUtility.ToDouble(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.CountedClaimsColumn]);
                        SumInspayments += MDVUtility.ToDouble(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.InsPaymentsColumn]);
                        SumPatpayments += MDVUtility.ToDouble(dr[dsReports.DT_Report_Beginning_AR_Ending_AR.PatPaymentsColumn]);
                    }
                    SumBegingAR = MDVUtility.ToAmount(SumBegingAR);
                    SumBilledChrg = MDVUtility.ToAmount(SumBilledChrg);
                    Sumpayments = MDVUtility.ToAmount(Sumpayments);
                    Sumadjstmnt = MDVUtility.ToAmount(Sumadjstmnt);
                    SumEndingAR = MDVUtility.ToAmount(SumEndingAR);
                    SumChrgAR = MDVUtility.ToAmount(SumChrgAR);
                    SumInspayments = MDVUtility.ToAmount(SumInspayments);
                    SumPatpayments = MDVUtility.ToAmount(SumPatpayments);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumBegingAR) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumBilledChrg) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Sumpayments) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumInspayments) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumPatpayments) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Sumadjstmnt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumEndingAR) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumChrgAR) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBold + ClaimCount + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }


        }

        public String LoadPaymentByUsers(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Report_Payments_By_User.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlign = "<td class='text-right'>";
                    String ColumnEnd = "</td>";
                    String FooterRowStart = "<tr>";
                    String FootercolumnBoldStart = "<td colspan='12' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td class='text-right'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "User Name" + thColumnEnd +
                        thColumnStart + "Appointment Provider" + thColumnEnd +
                        thColumnStart + "Rendering Provider" + thColumnEnd +
                        thColumnStart + "Resource Provider" + thColumnEnd +
                          thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Claim Number" + thColumnEnd +
                          thColumnStart + "Payment Type" + thColumnEnd +
                          thColumnStart + "Payment ID" + thColumnEnd +
                          thColumnStart + "Payment Date" + thColumnEnd +
                          thColumnStart + "Payor" + thColumnEnd +
                          thColumnStart + "Check No" + thColumnEnd +
                          thColumnStart + "Payment Type" + thColumnEnd +
                          thColumnStart + "Total" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double sum = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Report_Payments_By_User.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.UserNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.AppointmentProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.RenderingProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.ResourceProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.NameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.PayorTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.PaymentIdColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Report_Payments_By_User.PaymentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Report_Payments_By_User.PaymentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.PaymentDateColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.PaymentDateColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.PayorColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.CheckNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Report_Payments_By_User.PaymentTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Report_Payments_By_User.TotalAmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToAmount((dr[dsReports.DT_Report_Payments_By_User.TotalAmountColumn]))) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Report_Payments_By_User.TotalAmountColumn])))) + ColumnEnd);
                        sb.Append(RowEnd);
                        sum += MDVUtility.ToDouble(dr[dsReports.DT_Report_Payments_By_User.TotalAmountColumn]);
                    }
                    sum = MDVUtility.ToAmount(sum);
                    //make footer here
                    sb.Append(FooterRowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (sum >= 0 ? String.Format("${0:#,###,##0.00}", sum) : String.Format("(${0:#,###,##0.00})", Math.Abs(sum))) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }


        }

        public String LoadAgingSummaryAnalysis(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Aging_Summary_Analysis.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='5' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Insurance Plan" + thColumnEnd +
                          thColumnStart + "Renderring Provider" + thColumnEnd +
                          thColumnStart + "Resource Provider" + thColumnEnd +
                          thColumnStart + "Appointment Provider" + thColumnEnd +
                          thColumnStart + "Claim Amount" + thColumnEnd +
                          thColumnStart + "0-30 Days" + thColumnEnd +
                          thColumnStart + "31-60 Days" + thColumnEnd +
                          thColumnStart + "61-90 Days" + thColumnEnd +
                          thColumnStart + "91-120 Days" + thColumnEnd +
                          thColumnStart + "121-150 Days" + thColumnEnd +
                          thColumnStart + "151-180 Days" + thColumnEnd +
                          thColumnStart + ">180 Days" + thColumnEnd +
                          thColumnStart + "Total Balance" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double SumClaimAmt = 0;
                    double Sum30Bal = 0;
                    double Sum60Bal = 0;
                    double Sum90Bal = 0;
                    double Sum120Bal = 0;
                    double Sum150Bal = 0;
                    double Sum180Bal = 0;
                    double Sum181Bal = 0;
                    double SumTotal = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Aging_Summary_Analysis.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Summary_Analysis.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Summary_Analysis.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Summary_Analysis.RenderingProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Summary_Analysis.ResourceProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Summary_Analysis.AppointmentProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Aging_Summary_Analysis.ClaimAmountColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Aging_Summary_Analysis.thirtyDaysBalanceColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Aging_Summary_Analysis.sixtyDaysBalanceColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Aging_Summary_Analysis.nintyDaysBalanceColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Aging_Summary_Analysis.onetiDaysBalanceColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Aging_Summary_Analysis.onefDaysBalanceColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Aging_Summary_Analysis.oneeDaysBalanceColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Aging_Summary_Analysis.eightyPlusDaysBalanceColumn]) + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Aging_Summary_Analysis.TotalBalanceColumn]) + ColumnEnd);
                        sb.Append(RowEnd);
                        SumClaimAmt += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Summary_Analysis.ClaimAmountColumn]);
                        Sum30Bal += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Summary_Analysis.thirtyDaysBalanceColumn]);
                        Sum60Bal += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Summary_Analysis.sixtyDaysBalanceColumn]);
                        Sum90Bal += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Summary_Analysis.nintyDaysBalanceColumn]);
                        Sum120Bal += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Summary_Analysis.onetiDaysBalanceColumn]);
                        Sum150Bal += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Summary_Analysis.onefDaysBalanceColumn]);
                        Sum180Bal += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Summary_Analysis.oneeDaysBalanceColumn]);
                        Sum181Bal += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Summary_Analysis.eightyPlusDaysBalanceColumn]);
                        SumTotal += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Summary_Analysis.TotalBalanceColumn]);
                    }
                    SumClaimAmt = MDVUtility.ToAmount(SumClaimAmt);
                    Sum30Bal = MDVUtility.ToAmount(Sum30Bal);
                    Sum60Bal = MDVUtility.ToAmount(Sum60Bal);
                    Sum90Bal = MDVUtility.ToAmount(Sum90Bal);
                    Sum120Bal = MDVUtility.ToAmount(Sum120Bal);
                    Sum150Bal = MDVUtility.ToAmount(Sum150Bal);
                    Sum180Bal = MDVUtility.ToAmount(Sum180Bal);
                    Sum181Bal = MDVUtility.ToAmount(Sum181Bal);
                    SumTotal = MDVUtility.ToAmount(SumTotal);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + SumClaimAmt + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + Sum30Bal + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + Sum60Bal + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + Sum90Bal + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + Sum120Bal + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + Sum150Bal + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + Sum180Bal + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + Sum181Bal + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + SumTotal + FootercolumnBoldEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }


        }

        public String LoadEncounterWithoutClaims(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Encounter_Without_Claims.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Appointment Provider" + thColumnEnd +
                          thColumnStart + "Appointment Date" + thColumnEnd +
                          thColumnStart + "Patient Acc No" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "Visit Type" + thColumnEnd +
                          thColumnStart + "Visit Type Descriptin" + thColumnEnd +
                          thColumnStart + "Encounter ID" + thColumnEnd + RowEnd + TableHeadEnd;


                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Encounter_Without_Claims.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Encounter_Without_Claims.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Encounter_Without_Claims.AppointmentProviderColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Encounter_Without_Claims.AppointmentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Encounter_Without_Claims.AppointmentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Encounter_Without_Claims.AppointmentDateColumn] + ColumnEnd);
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Encounter_Without_Claims.PatientAccounrNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Encounter_Without_Claims.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Encounter_Without_Claims.VisitTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Encounter_Without_Claims.VisitTypeDescriptionColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Encounter_Without_Claims.EncounterIdColumn] + ColumnEnd);

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }


        }
        public String LoadChargesbyUsers(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Charges_By_Users.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='4' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Claim Created By" + thColumnEnd +
                          thColumnStart + "Renderring Provider" + thColumnEnd +
                          thColumnStart + "Appointment Provider" + thColumnEnd +
                          thColumnStart + "Amount" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double sum = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Charges_By_Users.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Charges_By_Users.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Charges_By_Users.ClaimCreatedByColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Charges_By_Users.RenderingProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Charges_By_Users.AppointmentProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + MDVUtility.ToAmount(dr[dsReports.DT_Charges_By_Users.AmountColumn]) + ColumnEnd);
                        sb.Append(RowEnd);
                        sum += MDVUtility.ToDouble(dr[dsReports.DT_Charges_By_Users.AmountColumn]);
                    }
                    sum = MDVUtility.ToAmount(sum);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + sum + FootercolumnBoldEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }


        }

        public String LoadBeginningAREndingARFacility(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Beginning_AR_Ending_AR_Facility.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartRightAlign = "<td style=text-align:right;>";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='4' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td style=text-align:right;><b>";
                    String FootercolumnBoldEnd = "</b></td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='4' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Resource Provider(s)" + thColumnEnd +
                        thColumnStart + "Appointment Provider(s)" + thColumnEnd +
                        thColumnStart + "Rendering  Provider(s)" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Beginning AR" + thColumnEnd +
                        thColumnStart + "Billed Charges" + thColumnEnd +
                        thColumnStart + "Payments" + thColumnEnd +
                        thColumnStart + "Insurance Payments" + thColumnEnd +
                        thColumnStart + "Patient Payments" + thColumnEnd +
                        thColumnStart + "Adjustments" + thColumnEnd +
                        thColumnStart + "Ending AR" + thColumnEnd +
                        thColumnStart + "Change AR" + thColumnEnd +
                          thColumnStart + "Claim(s) Count" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double SumBegingAR = 0;
                    double SumBilledChrg = 0;
                    double Sumpayments = 0;
                    double SumInspayments = 0;
                    double SumPatpayments = 0;
                    double Sumadjstmnt = 0;
                    double SumEndingAR = 0;
                    double SumChrgAR = 0;
                    double calimcount = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Beginning_AR_Ending_AR_Facility.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.BillingProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.AppointmentProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.RenderingProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStartRightAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.BeginingArColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.BeginingArColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.BeginingArColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartRightAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.BilledChargesColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.BilledChargesColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.BilledChargesColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartRightAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.PaymentsColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.PaymentsColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.PaymentsColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartRightAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.InsPaymentsColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.InsPaymentsColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.InsPaymentsColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartRightAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.PatPaymentsColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.PatPaymentsColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.PatPaymentsColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartRightAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.AdjustmentsColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.AdjustmentsColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.AdjustmentsColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartRightAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.EndingArColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.EndingArColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.EndingArColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartRightAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.ChangeArColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.ChangeArColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.ChangeArColumn])))) + ColumnEnd);

                        sb.Append(ColumnStartRightAlign + dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.ClaimCountsColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                        SumBegingAR += MDVUtility.ToDouble(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.BeginingArColumn]);
                        SumBilledChrg += MDVUtility.ToDouble(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.BilledChargesColumn]);
                        Sumpayments += MDVUtility.ToDouble(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.PaymentsColumn]);
                        SumInspayments += MDVUtility.ToDouble(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.InsPaymentsColumn]);
                        SumPatpayments += MDVUtility.ToDouble(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.PatPaymentsColumn]);
                        Sumadjstmnt += MDVUtility.ToDouble(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.AdjustmentsColumn]);
                        SumEndingAR += MDVUtility.ToDouble(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.EndingArColumn]);
                        SumChrgAR += MDVUtility.ToDouble(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.ChangeArColumn]);
                        calimcount += MDVUtility.ToDouble(dr[dsReports.DT_Beginning_AR_Ending_AR_Facility.ClaimCountsColumn]);
                    }

                    SumBegingAR = MDVUtility.ToAmount(SumBegingAR);
                    SumBilledChrg = MDVUtility.ToAmount(SumBilledChrg);
                    Sumpayments = MDVUtility.ToAmount(Sumpayments);
                    SumInspayments = MDVUtility.ToAmount(SumInspayments);
                    SumPatpayments = MDVUtility.ToAmount(SumPatpayments);
                    Sumadjstmnt = MDVUtility.ToAmount(Sumadjstmnt);
                    SumEndingAR = MDVUtility.ToAmount(SumEndingAR);
                    SumChrgAR = MDVUtility.ToAmount(SumChrgAR);
                    calimcount = MDVUtility.ToAmount(calimcount);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumBegingAR >= 0 ? String.Format("${0:#,###,##0.00}", SumBegingAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumBegingAR))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumBilledChrg >= 0 ? String.Format("${0:#,###,##0.00}", SumBilledChrg) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumBilledChrg))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (Sumpayments >= 0 ? String.Format("${0:#,###,##0.00}", Sumpayments) : String.Format("(${0:#,###,##0.00})", Math.Abs(Sumpayments))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumInspayments >= 0 ? String.Format("${0:#,###,##0.00}", SumInspayments) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumInspayments))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumPatpayments >= 0 ? String.Format("${0:#,###,##0.00}", SumPatpayments) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumPatpayments))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (Sumadjstmnt >= 0 ? String.Format("${0:#,###,##0.00}", Sumadjstmnt) : String.Format("(${0:#,###,##0.00})", Math.Abs(Sumadjstmnt))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumEndingAR >= 0 ? String.Format("${0:#,###,##0.00}", SumEndingAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumEndingAR))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (SumChrgAR >= 0 ? String.Format("${0:#,###,##0.00}", SumChrgAR) : String.Format("(${0:#,###,##0.00})", Math.Abs(SumChrgAR))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + calimcount + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }


        }

        public String LoadClaimCommentsbyUser(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Claim_Comments_By_Users.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";


                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "User" + thColumnEnd +
                        thColumnStart + "Entry Date" + thColumnEnd +
                        thColumnStart + "Claim Comments" + thColumnEnd +
                        thColumnStart + "Claim Number" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);


                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Claim_Comments_By_Users.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Claim_Comments_By_Users.UserColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Claim_Comments_By_Users.EntryDateColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Claim_Comments_By_Users.ClaimCommentColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Claim_Comments_By_Users.ClaimNumberColumn] + ColumnEnd);

                        sb.Append(RowEnd);

                    }

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }


        }
        public String LoadUserActivityReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_User_Activity_Report.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";


                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "User" + thColumnEnd +
                        thColumnStart + "Role" + thColumnEnd +
                        thColumnStart + "Created Date" + thColumnEnd +
                        thColumnStart + "New Patient" + thColumnEnd +
                        thColumnStart + "Updated Patient" + thColumnEnd +
                        thColumnStart + "New Charges" + thColumnEnd +
                        thColumnStart + "Updated Charges" + thColumnEnd +
                        thColumnStart + "Submitted" + thColumnEnd +
                        thColumnStart + "Payments" + thColumnEnd +
                        thColumnStart + "Transfers" + thColumnEnd +
                        thColumnStart + "Adjustments" + thColumnEnd +
                        thColumnStart + "New Appointments" + thColumnEnd +
                        thColumnStart + "Updated Appointments" + thColumnEnd +
                        thColumnStart + "Total Transactions" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);


                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_User_Activity_Report.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.FullNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.RoleNameColumn] + ColumnEnd);
                        //sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.CreatedDateColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_User_Activity_Report.CreatedDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_User_Activity_Report.CreatedDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.CreatedDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.NewPatientColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.UpdatedPatientColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.NewChargesColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.UpdatedChargesColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.SubmittedColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.PaymentsColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.TransfersColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.AdjustmentsColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.NewAppointmentsColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.UpdatedAppointmentsColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_User_Activity_Report.TotalTransactionsColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                    }

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }
        }
        public String LoadClaimSUbmitStatus(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Claim_Submit_Status.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlign = "<td class='text-right'>";
                    String ColumnEnd = "</td>";
                    String FooterRowStart = "<tr>";
                    String FootercolumnBoldStart = "<td colspan='8' align='center'><b>";
                    String FootercolumnBoldStart2 = "<td colspan='4' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td class='text-right'><b>";
                    String FootercolumnBoldEnd = "</b></td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                    + thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Resource Provider" + thColumnEnd +
                        thColumnStart + "Account Number" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "DOS" + thColumnEnd +
                        thColumnStart + "Claim Number" + thColumnEnd +
                        thColumnStart + "Insurance Plan" + thColumnEnd +
                        thColumnStart + "Billed Amount" + thColumnEnd +
                        thColumnStart + "Balance" + thColumnEnd +
                        thColumnStart + "Claim Status" + thColumnEnd +
                        thColumnStart + "Submit Status" + thColumnEnd +
                        thColumnStart + "Created by" + thColumnEnd +
                        thColumnStart + "Created On" + thColumnEnd + RowEnd + TableHeadEnd;


                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    Double Sum = 0;
                    Double Bal = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Claim_Submit_Status.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.ResourceProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.PaientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claim_Submit_Status.DOSFromColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claim_Submit_Status.DOSFromColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.DOSFromColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.DOSFromColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + String.Format("${0:#,###,##0.00}", MDVUtility.ToDouble(dr[dsReports.DT_Claim_Submit_Status.BilledAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToDouble(dr[dsReports.DT_Claim_Submit_Status.ClaimBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDouble(dr[dsReports.DT_Claim_Submit_Status.ClaimBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDouble(dr[dsReports.DT_Claim_Submit_Status.ClaimBalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.ClaimStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.SubmitStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.CreatedByColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submit_Status.CreatedOnColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                        Sum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Submit_Status.BilledAmountColumn]);
                        Bal += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Submit_Status.ClaimBalanceColumn]);
                    }
                    Sum = MDVUtility.ToAmount(Sum);
                    Bal = MDVUtility.ToAmount(Bal);
                    //make footer here
                    sb.Append(FooterRowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (Sum >= 0 ? String.Format("${0:#,###,##0.00}", Sum) : String.Format("(${0:#,###,##0.00})", Math.Abs(Sum))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (Bal >= 0 ? String.Format("${0:#,###,##0.00}", Bal) : String.Format("(${0:#,###,##0.00})", Math.Abs(Bal))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart2 + " " + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }


        }

        public String LoadAgingDetailAnalysis(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Aging_Detail_Analysis.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String FooterRowStart = "<tr>";
                    String FootercolumnBoldStart = "<td colspan='10' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td><b>";
                    String FootercolumnBoldEnd = "</b></td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Patient Name" + thColumnEnd
                        + thColumnStart + "Account Number" + thColumnEnd
                        + thColumnStart + "DOS From" + thColumnEnd
                        + thColumnStart + "DOS TO" + thColumnEnd
                        + thColumnStart + "Facility" + thColumnEnd
                        + thColumnStart + "Plan" + thColumnEnd
                        + thColumnStart + "Appointment Provider" + thColumnEnd
                        + thColumnStart + "Rendering Provider" + thColumnEnd
                        + thColumnStart + "Resource Provider" + thColumnEnd
                        + thColumnStart + "Claim Number" + thColumnEnd
                        + thColumnStart + "0-30 Days Balance" + thColumnEnd
                        + thColumnStart + "30-60 Days Balance" + thColumnEnd
                        + thColumnStart + "60-90 Days Balance" + thColumnEnd
                        + thColumnStart + "90-120 Days Balance" + thColumnEnd
                        + thColumnStart + "120-150 Days Balance" + thColumnEnd
                        + thColumnStart + "150-180 Days Balance" + thColumnEnd
                        + thColumnStart + "180+ Days Balance" + thColumnEnd +
                          thColumnStart + "Total Balance" + thColumnEnd + RowEnd + TableHeadEnd;


                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    Double thirty = 0;
                    Double sixty = 0;
                    Double ninty = 0;
                    Double oneti = 0;
                    Double onef = 0;
                    Double oneeti = 0;
                    Double morethaneighty = 0;
                    Double total = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Aging_Detail_Analysis.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.AccountNumberColumn] + ColumnEnd);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.DOSFromColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Aging_Detail_Analysis.DOSFromColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Aging_Detail_Analysis.DOSFromColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.DOSFromColumn] + ColumnEnd);
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Aging_Detail_Analysis.DOSToColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Aging_Detail_Analysis.DOSToColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.DOSToColumn] + ColumnEnd);
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.AppointmentProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.RenderingProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.ResourceProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Aging_Detail_Analysis.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.thirtyDaysBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.thirtyDaysBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.thirtyDaysBalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.sixtyDaysBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.sixtyDaysBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.sixtyDaysBalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.nintyDaysBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.nintyDaysBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.nintyDaysBalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.onetiDaysBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.onetiDaysBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.onetiDaysBalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.onefDaysBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.onefDaysBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.onefDaysBalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.oneeDaysBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.oneeDaysBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.oneeDaysBalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.eightyPlusDaysBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.eightyPlusDaysBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.eightyPlusDaysBalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.TotalBalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.TotalBalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.TotalBalanceColumn])))) + ColumnEnd);
                        sb.Append(RowEnd);

                        thirty += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.thirtyDaysBalanceColumn]);
                        sixty += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.sixtyDaysBalanceColumn]);
                        ninty += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.nintyDaysBalanceColumn]);
                        oneti += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.onetiDaysBalanceColumn]);
                        onef += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.onefDaysBalanceColumn]);
                        oneeti += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.oneeDaysBalanceColumn]);
                        morethaneighty += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.eightyPlusDaysBalanceColumn]);
                        total += MDVUtility.ToDouble(dr[dsReports.DT_Aging_Detail_Analysis.TotalBalanceColumn]);
                    }
                    thirty = MDVUtility.ToAmount(thirty);
                    sixty = MDVUtility.ToAmount(sixty);
                    ninty = MDVUtility.ToAmount(ninty);
                    oneti = MDVUtility.ToAmount(oneti);
                    onef = MDVUtility.ToAmount(onef);
                    oneeti = MDVUtility.ToAmount(oneeti);
                    morethaneighty = MDVUtility.ToAmount(morethaneighty);
                    total = MDVUtility.ToAmount(total);
                    //make footer here
                    sb.Append(FooterRowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (thirty >= 0 ? String.Format("${0:#,###,##0.00}", thirty) : String.Format("(${0:#,###,##0.00})", Math.Abs(thirty))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (sixty >= 0 ? String.Format("${0:#,###,##0.00}", sixty) : String.Format("(${0:#,###,##0.00})", Math.Abs(sixty))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (ninty >= 0 ? String.Format("${0:#,###,##0.00}", ninty) : String.Format("(${0:#,###,##0.00})", Math.Abs(ninty))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (oneti >= 0 ? String.Format("${0:#,###,##0.00}", oneti) : String.Format("(${0:#,###,##0.00})", Math.Abs(oneti))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (onef >= 0 ? String.Format("${0:#,###,##0.00}", onef) : String.Format("(${0:#,###,##0.00})", Math.Abs(onef))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (oneeti >= 0 ? String.Format("${0:#,###,##0.00}", oneeti) : String.Format("(${0:#,###,##0.00})", Math.Abs(oneeti))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (morethaneighty >= 0 ? String.Format("${0:#,###,##0.00}", morethaneighty) : String.Format("(${0:#,###,##0.00})", Math.Abs(morethaneighty))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (total >= 0 ? String.Format("${0:#,###,##0.00}", total) : String.Format("(${0:#,###,##0.00})", Math.Abs(total))) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadRevenueByProviderReport", ex);
                return ex.Message;
            }


        }

        public String LoadARReconciliationReportDetail(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_AR_RECONCILIATION_REPORT.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center'colspan='2'><b>";
                    String FootercolumnBoldStart1 = "<td align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Claim Number" + thColumnEnd +
                          thColumnStart + "Resource Provider" + thColumnEnd +
                          thColumnStart + "Charges" + thColumnEnd +
                          thColumnStart + "Payment" + thColumnEnd +
                          thColumnStart + "Adjustments" + thColumnEnd +
                          thColumnStart + "Ending AR" + thColumnEnd +
                          thColumnStart + "Claim Status" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double SumBilledChrg = 0;
                    double Sumpayments = 0;
                    double Sumadjstmnt = 0;
                    double SumEndingAR = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_AR_RECONCILIATION_REPORT.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_AR_RECONCILIATION_REPORT.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AR_RECONCILIATION_REPORT.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_AR_RECONCILIATION_REPORT.BilledChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_AR_RECONCILIATION_REPORT.PaymentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_AR_RECONCILIATION_REPORT.AdjustmentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_AR_RECONCILIATION_REPORT.EndingARColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AR_RECONCILIATION_REPORT.SubmitStatusColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                        SumBilledChrg += MDVUtility.ToDouble(dr[dsReports.DT_AR_RECONCILIATION_REPORT.BilledChargesColumn]);
                        Sumpayments += MDVUtility.ToDouble(dr[dsReports.DT_AR_RECONCILIATION_REPORT.PaymentsColumn]);
                        Sumadjstmnt += MDVUtility.ToDouble(dr[dsReports.DT_AR_RECONCILIATION_REPORT.AdjustmentsColumn]);
                        SumEndingAR += MDVUtility.ToDouble(dr[dsReports.DT_AR_RECONCILIATION_REPORT.EndingARColumn]);
                    }
                    SumBilledChrg = MDVUtility.ToAmount(SumBilledChrg);
                    Sumpayments = MDVUtility.ToAmount(Sumpayments);
                    Sumadjstmnt = MDVUtility.ToAmount(Sumadjstmnt);
                    SumEndingAR = MDVUtility.ToAmount(SumEndingAR);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumBilledChrg) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Sumpayments) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Sumadjstmnt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumEndingAR) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + "" + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadARReconciliationReportDetail", ex);
                return ex.Message;
            }


        }
        public String LoadARReconciliationReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_AR_RECONCILIATION_REPORT.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Claim Status" + thColumnEnd +
                          thColumnStart + "Charges" + thColumnEnd +
                          thColumnStart + "Payment" + thColumnEnd +
                          thColumnStart + "Adjustments" + thColumnEnd +
                          thColumnStart + "Ending AR" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double SumBilledChrg = 0;
                    double Sumpayments = 0;
                    double Sumadjstmnt = 0;
                    double SumEndingAR = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_AR_RECONCILIATION_REPORT.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_AR_RECONCILIATION_REPORT.SubmitStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_AR_RECONCILIATION_REPORT.BilledChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_AR_RECONCILIATION_REPORT.PaymentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_AR_RECONCILIATION_REPORT.AdjustmentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_AR_RECONCILIATION_REPORT.EndingARColumn])) + ColumnEnd);
                        sb.Append(RowEnd);

                        SumBilledChrg += MDVUtility.ToDouble(dr[dsReports.DT_AR_RECONCILIATION_REPORT.BilledChargesColumn]);
                        Sumpayments += MDVUtility.ToDouble(dr[dsReports.DT_AR_RECONCILIATION_REPORT.PaymentsColumn]);
                        Sumadjstmnt += MDVUtility.ToDouble(dr[dsReports.DT_AR_RECONCILIATION_REPORT.AdjustmentsColumn]);
                        SumEndingAR += MDVUtility.ToDouble(dr[dsReports.DT_AR_RECONCILIATION_REPORT.EndingARColumn]);
                    }
                    SumBilledChrg = MDVUtility.ToAmount(SumBilledChrg);
                    Sumpayments = MDVUtility.ToAmount(Sumpayments);
                    Sumadjstmnt = MDVUtility.ToAmount(Sumadjstmnt);
                    SumEndingAR = MDVUtility.ToAmount(SumEndingAR);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumBilledChrg) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Sumpayments) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Sumadjstmnt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumEndingAR) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadARReconciliationReport", ex);
                return ex.Message;
            }
        }

        public String LoadIncorrectBalancebyVoidedClaims(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);

                if (dsReports.Tables[dsReports.DT_IncorrectBalancebyVoidedClaims.TableName].Rows.Count > 0)
                {
                    string RunByValue = IncorrectBalanceParamsCheck(ReportsParamaters);
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center' colspan='4'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + RunByValue + thColumnEnd +
                          thColumnStart + "Original Claim Number" + thColumnEnd +
                          thColumnStart + "Voided Claim Number" + thColumnEnd +
                          thColumnStart + "New Claim Number" + thColumnEnd +
                          thColumnStart + "Original Claim Balance" + thColumnEnd +
                          thColumnStart + "Voided Claim Balance" + thColumnEnd +
                          thColumnStart + "Difference in Claim Balance" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double SumOriBla = 0;
                    double SumNewBal = 0;
                    double SumBalDiff = 0;
                    //double SumEndingAR = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_IncorrectBalancebyVoidedClaims.TableName].Rows)
                    {

                        sb.Append(RowStart);
                        if (RunByValue == "Facility")
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_IncorrectBalancebyVoidedClaims.FacilityNameColumn] + ColumnEnd);
                        }
                        else if (RunByValue == "Rendering Provider")
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_IncorrectBalancebyVoidedClaims.ProviderNameColumn] + ColumnEnd);
                        }
                        else if (RunByValue == "Insurance Plan")
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_IncorrectBalancebyVoidedClaims.InsurancePlanColumn] + ColumnEnd);
                        }
                        else if (RunByValue == "Patient/Self Pay")
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_IncorrectBalancebyVoidedClaims.InsurancePlanColumn] + ColumnEnd);
                        }

                        sb.Append(ColumnStart + dr[dsReports.DT_IncorrectBalancebyVoidedClaims.VoidedClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_IncorrectBalancebyVoidedClaims.NegativeClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_IncorrectBalancebyVoidedClaims.PossitiveClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_IncorrectBalancebyVoidedClaims.VoidedBalColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_IncorrectBalancebyVoidedClaims.totalNegBalColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_IncorrectBalancebyVoidedClaims.TotalDiffColumn])) + ColumnEnd);
                        //sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_IncorrectBalancebyVoidedClaims.EndingARColumn])) + ColumnEnd);
                        sb.Append(RowEnd);

                        SumOriBla += MDVUtility.ToDouble(dr[dsReports.DT_IncorrectBalancebyVoidedClaims.VoidedBalColumn]);
                        SumNewBal += MDVUtility.ToDouble(dr[dsReports.DT_IncorrectBalancebyVoidedClaims.totalNegBalColumn]);
                        SumBalDiff += MDVUtility.ToDouble(dr[dsReports.DT_IncorrectBalancebyVoidedClaims.TotalDiffColumn]);
                        //SumEndingAR += MDVUtility.ToDouble(dr[dsReports.DT_IncorrectBalancebyVoidedClaims.EndingARColumn]);
                    }
                    SumOriBla = MDVUtility.ToAmount(SumOriBla);
                    SumNewBal = MDVUtility.ToAmount(SumNewBal);
                    SumBalDiff = MDVUtility.ToAmount(SumBalDiff);
                    //SumEndingAR = MDVUtility.ToAmount(SumEndingAR);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumOriBla) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumNewBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumBalDiff) + FootercolumnBoldEnd);
                    //sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumEndingAR) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadIncorrectBalancebyVoidedClaims", ex);
                return ex.Message;
            }
        }


        public String LoadPatientOverPayment(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Patient_OverPayment.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center' colspan='4'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account Number" + thColumnEnd +
                          thColumnStart + "Patient Last Name" + thColumnEnd +
                          thColumnStart + "Patient First Name" + thColumnEnd +
                          thColumnStart + "Claim Numbers" + thColumnEnd +
                          thColumnStart + "Patient Charges" + thColumnEnd +
                          thColumnStart + "Patient Paid" + thColumnEnd +
                          thColumnStart + "Patient Adjustments" + thColumnEnd +
                          thColumnStart + "Patient Balance" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double SumBilledChrg = 0;
                    double Sumpayments = 0;
                    double Sumadjstmnt = 0;
                    double SumEndingAR = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Patient_OverPayment.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Patient_OverPayment.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Patient_OverPayment.LastNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Patient_OverPayment.FirstNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Patient_OverPayment.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_Patient_OverPayment.PatChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_Patient_OverPayment.PatPaidColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_Patient_OverPayment.PatAdjustmentColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_Patient_OverPayment.PatBalanceColumn])) + ColumnEnd);
                        sb.Append(RowEnd);

                        SumBilledChrg += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Patient_OverPayment.PatChargesColumn]);
                        Sumpayments += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Patient_OverPayment.PatPaidColumn]);
                        Sumadjstmnt += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Patient_OverPayment.PatAdjustmentColumn]);
                        SumEndingAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_Patient_OverPayment.PatBalanceColumn]);
                    }
                    SumBilledChrg = MDVUtility.ToAmount(SumBilledChrg);
                    Sumpayments = MDVUtility.ToAmount(Sumpayments);
                    Sumadjstmnt = MDVUtility.ToAmount(Sumadjstmnt);
                    SumEndingAR = MDVUtility.ToAmount(SumEndingAR);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumBilledChrg) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Sumpayments) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Sumadjstmnt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumEndingAR) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientOverPayment", ex);
                return ex.Message;
            }


        }
        /// <summary>
        /// load under paid claims whose billed amount- expected fee greater than 0
        /// </summary>
        /// <param name="ReportsParamaters"></param>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public String LoadZeroPaidClaim(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Zero_Paid_Claims.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center' colspan='11'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + "<th class='noWordBreak'>Practice</th>"
                        + "<th class='noWordBreak'>Facility</th>"
                        + "<th class='noWordBreak'>Provider</th>"
                        + "<th class='noWordBreak'>Account Number</th>"
                        + "<th class='noWordBreak'>Patient Name</th>"
                        + "<th class='noWordBreak'>Insurance Plan</th>"
                        + "<th class='noWordBreak'>Subscriber Id</th>"
                        + "<th class='noWordBreak'>DOS</th>"
                        + "<th class='noWordBreak'>Claim Number</th>"
                        + "<th class='noWordBreak'>Claim Date</th>"
                        + "<th class='noWordBreak'>Entered By</th>"
                        + "<th class='noWordBreak'>Charges</th>"
                        + "<th class='noWordBreak'>Insurance Balance</th>"
                        + "<th class='noWordBreak'>Aging Days</th>"
                        + "<th class='noWordBreak'>Group Code</th>"
                        + "<th class='noWordBreak size-min200'>Group Code Desc</th>"
                        + "<th class='noWordBreak'>Reason Code</th>"
                        + "<th class='noWordBreak size-min200'>Reason Code Desc</th>"
                        + "<th class='noWordBreak size-min200'>Comments</th>"
                        + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double SumChrg = 0;
                    double SumBal = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Zero_Paid_Claims.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.InsurancePlanNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.SubscriberIdColumn] + ColumnEnd);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.DOSColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Zero_Paid_Claims.DOSColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Zero_Paid_Claims.DOSColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.DOSColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.ClaimNumberColumn] + ColumnEnd);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.ClaimDateColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Zero_Paid_Claims.ClaimDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Zero_Paid_Claims.ClaimDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.ClaimDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.EnteredByColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Zero_Paid_Claims.InsuranceChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Zero_Paid_Claims.InsuranceBalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.AgingDaysColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.GroupCodeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.GroupCodeDescColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.ReasonCodeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.ReasonCodeDescColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Zero_Paid_Claims.CommentsColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                        SumChrg += MDVUtility.ToDouble(dr[dsReports.DT_Zero_Paid_Claims.InsuranceChargesColumn]);
                        SumBal += MDVUtility.ToDouble(dr[dsReports.DT_Zero_Paid_Claims.InsuranceBalanceColumn]);
                    }
                    SumChrg = MDVUtility.ToAmount(SumChrg);
                    SumBal = MDVUtility.ToAmount(SumBal);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumChrg) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumBal) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadZeroPaidClaim", ex);
                return ex.Message;
            }


        }

        /// <summary>
        /// load under paid claims whose billed amount- expected fee less than 0
        /// </summary>
        /// <param name="ReportsParamaters"></param>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public String LoadClaimUnderPaidByInsurance(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Claim_UnderPaid_Insurance.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    //String thColumnStart = "<th class='noWordBreak'>";
                    //String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center' colspan='10'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + "<th class='noWordBreak'>Practice</th>"
                        + "<th class='noWordBreak'>Provider</th>"
                        + "<th class='noWordBreak'>Facility</th>"
                        + "<th class='noWordBreak'>Insurance Plan</th>"
                        + "<th class='noWordBreak'>Account #</th>"
                        + "<th class='noWordBreak'>Patient Name</th>"
                        + "<th class='noWordBreak'>Claim Number</th>"
                        + "<th class='noWordBreak'>DOS</th>"
                        + "<th class='noWordBreak'>CPT</th>"
                        + "<th class='noWordBreak'>Claim Date</th>"
                        + "<th class='noWordBreak'>Billed Amount</th>"
                        + "<th class='noWordBreak'>Expected Amount</th>"
                        + "<th class='noWordBreak'>Paid Amount</th>"
                        + "<th class='noWordBreak'>Write-Off</th>"
                        + "<th class='noWordBreak'>Under Paid Amount</th>"
                        + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double BilledAmountSum = 0;
                    double ExpectedAmountSum = 0;
                    double PaidAmountSum = 0;
                    double WriteoffAmountSum = 0;
                    double UnderpaidAmountSum = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Claim_UnderPaid_Insurance.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_UnderPaid_Insurance.PracticeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_UnderPaid_Insurance.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_UnderPaid_Insurance.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_UnderPaid_Insurance.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_UnderPaid_Insurance.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_UnderPaid_Insurance.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_UnderPaid_Insurance.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claim_UnderPaid_Insurance.DOSColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claim_UnderPaid_Insurance.DOSColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claim_UnderPaid_Insurance.DOSColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_UnderPaid_Insurance.CPTCodeColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claim_UnderPaid_Insurance.ClaimDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claim_UnderPaid_Insurance.ClaimDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claim_UnderPaid_Insurance.ClaimDateColumn] + ColumnEnd);//
                        }

                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_UnderPaid_Insurance.BilledAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_UnderPaid_Insurance.ExpectedAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_UnderPaid_Insurance.PaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_UnderPaid_Insurance._Write_OffColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_UnderPaid_Insurance.UnderPaidAmountColumn])) + ColumnEnd);

                        sb.Append(RowEnd);

                        BilledAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_UnderPaid_Insurance.BilledAmountColumn]);
                        ExpectedAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_UnderPaid_Insurance.ExpectedAmountColumn]);
                        PaidAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_UnderPaid_Insurance.PaidAmountColumn]);
                        WriteoffAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_UnderPaid_Insurance._Write_OffColumn]);
                        UnderpaidAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_UnderPaid_Insurance.UnderPaidAmountColumn]);
                    }
                    BilledAmountSum = MDVUtility.ToAmount(BilledAmountSum);
                    ExpectedAmountSum = MDVUtility.ToAmount(ExpectedAmountSum);
                    PaidAmountSum = MDVUtility.ToAmount(PaidAmountSum);
                    WriteoffAmountSum = MDVUtility.ToAmount(WriteoffAmountSum);
                    UnderpaidAmountSum = MDVUtility.ToAmount(UnderpaidAmountSum);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", BilledAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", ExpectedAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PaidAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", WriteoffAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", UnderpaidAmountSum) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadClaimUnderPaidByInsurance", ex);
                return ex.Message;
            }

        }


        public String LoadClaimOverPaidByInsurance(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Claim_OverPaid_Insurance.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    //String thColumnStart = "<th class='noWordBreak'>";
                    //String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center' colspan='10'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + "<th class='noWordBreak'>Practice</th>"
                        + "<th class='noWordBreak'>Provider</th>"
                        + "<th class='noWordBreak'>Facility</th>"
                        + "<th class='noWordBreak'>Insurance Plan</th>"
                        + "<th class='noWordBreak'>Account #</th>"
                        + "<th class='noWordBreak'>Patient Name</th>"
                        + "<th class='noWordBreak'>Claim Number</th>"
                        + "<th class='noWordBreak'>DOS</th>"
                        + "<th class='noWordBreak'>CPT</th>"
                        + "<th class='noWordBreak'>Claim Date</th>"
                        + "<th class='noWordBreak'>Billed Amount</th>"
                        + "<th class='noWordBreak'>Expected Amount</th>"
                        + "<th class='noWordBreak'>Paid Amount</th>"
                        + "<th class='noWordBreak'>Write-Off</th>"
                        + "<th class='noWordBreak'>Over Paid Amount</th>"
                        + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double BilledAmountSum = 0;
                    double ExpectedAmountSum = 0;
                    double PaidAmountSum = 0;
                    double WriteoffAmountSum = 0;
                    double OverpaidAmountSum = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Claim_OverPaid_Insurance.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_OverPaid_Insurance.PracticeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_OverPaid_Insurance.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_OverPaid_Insurance.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_OverPaid_Insurance.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_OverPaid_Insurance.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_OverPaid_Insurance.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_OverPaid_Insurance.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claim_OverPaid_Insurance.DOSColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claim_OverPaid_Insurance.DOSColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claim_OverPaid_Insurance.DOSColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_OverPaid_Insurance.CPTCodeColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claim_OverPaid_Insurance.ClaimDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claim_OverPaid_Insurance.ClaimDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claim_OverPaid_Insurance.ClaimDateColumn] + ColumnEnd);//
                        }

                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_OverPaid_Insurance.BilledAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_OverPaid_Insurance.ExpectedAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_OverPaid_Insurance.PaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_OverPaid_Insurance._Write_OffColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_OverPaid_Insurance.OverPaidAmountColumn])) + ColumnEnd);

                        sb.Append(RowEnd);

                        BilledAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_OverPaid_Insurance.BilledAmountColumn]);
                        ExpectedAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_OverPaid_Insurance.ExpectedAmountColumn]);
                        PaidAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_OverPaid_Insurance.PaidAmountColumn]);
                        WriteoffAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_OverPaid_Insurance._Write_OffColumn]);
                        OverpaidAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_OverPaid_Insurance.OverPaidAmountColumn]);
                    }
                    BilledAmountSum = MDVUtility.ToAmount(BilledAmountSum);
                    ExpectedAmountSum = MDVUtility.ToAmount(ExpectedAmountSum);
                    PaidAmountSum = MDVUtility.ToAmount(PaidAmountSum);
                    WriteoffAmountSum = MDVUtility.ToAmount(WriteoffAmountSum);
                    OverpaidAmountSum = MDVUtility.ToAmount(OverpaidAmountSum);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", BilledAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", ExpectedAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PaidAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", WriteoffAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", OverpaidAmountSum) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadClaimOverPaidByInsurance", ex);
                return ex.Message;
            }


        }

        public String LoadCDSAlertReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_CDS_Alerts.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Practice" + thColumnEnd +
                          thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Account Number" + thColumnEnd +
                          thColumnStart + "First Name" + thColumnEnd +
                          thColumnStart + "Last Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Gender" + thColumnEnd +
                          thColumnStart + "Rule Type" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_CDS_Alerts.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_CDS_Alerts.PracticeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_CDS_Alerts.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_CDS_Alerts.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_CDS_Alerts.FirstNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_CDS_Alerts.LastNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_CDS_Alerts.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_CDS_Alerts.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_CDS_Alerts.DOBColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_CDS_Alerts.GenderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_CDS_Alerts.RuleTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_CDS_Alerts.CDSStatusColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadZeroPaidClaim", ex);
                return ex.Message;
            }


        }

        public String LoadAnesthesiaOverlapping(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Anesthesia_Overlapping.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    //String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    //String FootercolumnBoldStart = "<td align='center' colspan='11'><b>";
                    //String FootercolumnBoldStartDollar = "<td><b>$";
                    //String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Anesthesiologist" + thColumnEnd +
                          thColumnStart + "CRNA" + thColumnEnd +
                          thColumnStart + "DOS" + thColumnEnd +
                          thColumnStart + "Claim Number" + thColumnEnd +
                         thColumnStart + "Claim Date" + thColumnEnd +
                          thColumnStart + "Start Time" + thColumnEnd +
                          thColumnStart + "End Time" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Anesthesia_Overlapping.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Anesthesia_Overlapping.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Anesthesia_Overlapping.AnesthesiologistNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Anesthesia_Overlapping.CRNANameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Anesthesia_Overlapping.dosfromColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Anesthesia_Overlapping.dosfromColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Anesthesia_Overlapping.dosfromColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Anesthesia_Overlapping.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Anesthesia_Overlapping.CreatedOnColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Anesthesia_Overlapping.CreatedOnColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Anesthesia_Overlapping.CreatedOnColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Anesthesia_Overlapping.starttimeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Anesthesia_Overlapping.endtimeColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadZeroPaidClaim", ex);
                return ex.Message;
            }


        }

        public String LoadClaimStatusDashboardDetail(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Claim_Status_Dashboard.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnStartNotes = "<th class='noWordBreak size-min300'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartNotes = "<td class='size-min300'>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center'colspan='13'><b>";
                    //String FootercolumnBoldStart1 = "<td align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Resource Provider" + thColumnEnd +
                          thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Insurance Plan" + thColumnEnd +
                          thColumnStart + "Claim Number" + thColumnEnd +
                          thColumnStart + "Claim Date" + thColumnEnd +
                          thColumnStart + "DOS" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Account Number" + thColumnEnd +
                          thColumnStart + "Modified By" + thColumnEnd +
                          thColumnStart + "Modified Date" + thColumnEnd +
                          thColumnStart + "Entered By" + thColumnEnd +
                          thColumnStartNotes + "Notes" + thColumnEnd +
                          thColumnStart + "Claim Amount" + thColumnEnd +
                          thColumnStart + "Insurance Payment" + thColumnEnd +
                          thColumnStart + "Patient Payment" + thColumnEnd +
                          thColumnStart + "Copay" + thColumnEnd +
                          thColumnStart + "Adjustments" + thColumnEnd +
                          thColumnStart + "Insurance Balance" + thColumnEnd +
                          thColumnStart + "Patient Balance" + thColumnEnd +
                          thColumnStart + "Total Balance" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double SumBilledChrg = 0;
                    double SumInsPymt = 0;
                    double SumPatPymt = 0;
                    double SumCopay = 0;
                    double SumAdj = 0;
                    double SumInsBal = 0;
                    double SumPatBal = 0;
                    double SumBal = 0;
                    //double SumEndingAR = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Claim_Status_Dashboard.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.ResProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claim_Status_Dashboard.CreatedOnColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claim_Status_Dashboard.CreatedOnColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.CreatedOnColumn] + ColumnEnd);//
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claim_Status_Dashboard.DOSColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claim_Status_Dashboard.DOSColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.DOSColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.SubmitStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.ModifiedByColumn] + ColumnEnd);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.ModifiedOnColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claim_Status_Dashboard.ModifiedOnColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claim_Status_Dashboard.ModifiedOnColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.ModifiedOnColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Status_Dashboard.CreatedByColumn] + ColumnEnd);
                        sb.Append(ColumnStartNotes + dr[dsReports.DT_Claim_Status_Dashboard.NoteCommentsColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_Status_Dashboard.chargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_Status_Dashboard.InsurancePaymentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_Status_Dashboard.PatientPaymentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_Status_Dashboard.CopayPaidColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_Status_Dashboard.AdjustmentsColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_Status_Dashboard.InsbalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_Status_Dashboard.PatbalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_Status_Dashboard.balanceColumn])) + ColumnEnd);
                        sb.Append(RowEnd);

                        SumBilledChrg += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Status_Dashboard.chargesColumn]);
                        SumInsPymt += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Status_Dashboard.InsurancePaymentsColumn]);
                        SumPatPymt += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Status_Dashboard.PatientPaymentsColumn]);
                        SumCopay += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Status_Dashboard.CopayPaidColumn]);
                        SumAdj += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Status_Dashboard.AdjustmentsColumn]);
                        SumInsBal += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Status_Dashboard.InsbalanceColumn]);
                        SumPatBal += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Status_Dashboard.PatbalanceColumn]);
                        SumBal += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Status_Dashboard.balanceColumn]);

                    }
                    SumBilledChrg = MDVUtility.ToAmount(SumBilledChrg);
                    SumInsPymt = MDVUtility.ToAmount(SumInsPymt);
                    SumPatPymt = MDVUtility.ToAmount(SumPatPymt);
                    SumCopay = MDVUtility.ToAmount(SumCopay);
                    SumAdj = MDVUtility.ToAmount(SumAdj);
                    SumInsBal = MDVUtility.ToAmount(SumInsBal);
                    SumPatBal = MDVUtility.ToAmount(SumPatBal);
                    SumBal = MDVUtility.ToAmount(SumBal);

                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumBilledChrg) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumInsPymt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumPatPymt) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumCopay) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumAdj) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumInsBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumPatBal) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumBal) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadARReconciliationReportDetail", ex);
                return ex.Message;
            }


        }

        public String LoadDrugCodeCostReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Drug_Code_Cost.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center'colspan='11'><b>";
                    String FootercolumnBoldStartDollar = "<td><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart +
                          thColumnStart + "Practice" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "User" + thColumnEnd +
                          thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "Claim Number" + thColumnEnd +
                          thColumnStart + "DOS" + thColumnEnd +
                          thColumnStart + "Payor Type" + thColumnEnd +
                          thColumnStart + "Payor" + thColumnEnd +
                          thColumnStart + "Payment Date" + thColumnEnd +
                          thColumnStart + "Paid Amount" + thColumnEnd +
                          thColumnStart + "Drug Code" + thColumnEnd +
                          thColumnStart + "Drug Code Cost" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    double SumaPaidAmt = 0;
                    double SumDrugCst = 0;
                    //double SumEndingAR = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Drug_Code_Cost.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.UserNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Drug_Code_Cost.DOSFromColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Drug_Code_Cost.DOSFromColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.DOSFromColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.PayorTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.payerColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Drug_Code_Cost.PaymentDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Drug_Code_Cost.PaymentDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.PaymentDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStartDollar + (MDVUtility.ToAmount(dr[dsReports.DT_Drug_Code_Cost.PaymentAmountColumn]) <= 0 ? "0.00" : String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Drug_Code_Cost.PaymentAmountColumn]))) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Drug_Code_Cost.cptcodeColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + (MDVUtility.ToAmount(dr[dsReports.DT_Drug_Code_Cost.DrugCodeCostColumn]) <= 0 ? "0.00" : String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Drug_Code_Cost.DrugCodeCostColumn]))) + ColumnEnd);
                        sb.Append(RowEnd);

                        SumaPaidAmt += MDVUtility.ToDouble(dr[dsReports.DT_Drug_Code_Cost.PaymentAmountColumn]);
                        SumDrugCst += MDVUtility.ToDouble(dr[dsReports.DT_Drug_Code_Cost.DrugCodeCostColumn]);
                    }
                    SumaPaidAmt = MDVUtility.ToAmount(SumaPaidAmt);
                    SumDrugCst = MDVUtility.ToAmount(SumDrugCst);

                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumaPaidAmt) + FootercolumnBoldEnd);
                    sb.Append(ColumnStart + "" + ColumnEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", SumDrugCst) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadDrugCodeCostReport", ex);
                return ex.Message;
            }


        }
        public String LoadUserAuditReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Audit_User.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                       "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Changes" + thColumnEnd +
                       RowEnd +
                       RowStart
                       + thColumnStart + " style='text-align:left'>" + "Users " + thColumnEnd +
                         thColumnStart + ">" + "Security Role" + thColumnEnd +
                         thColumnStart + " style='width:7%;text-align:center;'>" + "Action" + thColumnEnd +
                         thColumnStart + " style='text-align:left'>" + "Field" + thColumnEnd +
                         thColumnStart + ">" + "Original Value" + thColumnEnd +
                         thColumnStart + " style='text-align:center'>" + "Current Value" + thColumnEnd +
                         thColumnStart + ">" + "Date-Time" + thColumnEnd +
                         thColumnStart + ">" + "Entered By" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Audit_User.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_Audit_User.AuditedUserNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_Audit_User.SecurityRoleColumn] + ColumnEnd);
                        sb.Append(ColumnStart + " style='text-align:center'>" + dr[dsReports.DT_Audit_User.DBAuditActionColumn] + ColumnEnd);
                        sb.Append(ColumnStart + " style='text-align:left'>" + dr[dsReports.DT_Audit_User.FiledColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_Audit_User.OriginalValueColumn] + ColumnEnd);
                        sb.Append(ColumnStart + " style='text-align:center'>" + dr[dsReports.DT_Audit_User.CurrentValueColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_Audit_User.CreatedDateColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_Audit_User.UserNameColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadUserAuditReport", ex);
                return ex.Message;
            }
        }
        /// <summary>
        /// Get Appointment Vs Claim Summary for Print Functionality
        /// </summary>
        /// <param name="ReportsParamaters"></param>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public String LoadAppointmentVsClaimSummary(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName, true);
                StringBuilder sb = new StringBuilder();
                sb = sb.Append(string.Empty);
                string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                string PrintDivEnd = "</div>";
                //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                String TableHead = "<thead class='printHeading'>";
                String thColumnStart = "<th class='noWordBreak'";
                String thColumnEnd = "</th>";
                String TableHeadEnd = "</thead>";
                String TableEnd = "</table>";
                String RowStart = "<tr>";
                String RowEnd = "</tr>";
                String ColumnStart = "<td";
                String ColumnEnd = "</td>";
                Int64 totalAppointment = 0, CheckOut = 0, CheckIn = 0, Confirm = 0, Cancel = 0, Scheduled = 0, SelfPay = 0, Pending = 0, ReSchedule = 0, OnCall = 0, QRCheckIn = 0,
                         Meeting = 0, NoAnswer = 0, NoResponse = 0, NoShow = 0, Voice = 0, Waiting = 0, NotesCreated = 0, NotesNotCreated = 0, ChargeCreated = 0, ChargeNotCreated = 0;
                // Get Sum of ALL Column to show in Grand total
                totalAppointment = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(4));
                CheckOut = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(5));
                CheckIn = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(6));
                Confirm = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(7));
                Cancel = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(8));
                Scheduled = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(9));
                SelfPay = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(10));
                Pending = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(11));
                ReSchedule = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(12));
                OnCall = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(13));
                QRCheckIn = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(14));
                Meeting = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(15));
                NoAnswer = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(16));
                NoResponse = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(17));
                NoShow = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(18));
                Voice = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(19));
                Waiting = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(20));
                NotesCreated = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(21));
                NotesNotCreated = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(22));
                ChargeCreated = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(23));
                ChargeNotCreated = dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].AsEnumerable().Sum(x => x.Field<Int32>(24));
                //Number of Column to show
                StringBuilder HeadingColumnToShow = new StringBuilder();
                StringBuilder FooterColumnToShow = new StringBuilder();
                int colspan = 0;
                if (totalAppointment > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Total Appt." + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + totalAppointment + ColumnEnd); }
                if (CheckOut > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Check Out" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + CheckOut + ColumnEnd); }
                if (CheckIn > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Check In" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + CheckIn + ColumnEnd); }
                if (Confirm > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Confirm" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + Confirm + ColumnEnd); }
                if (Cancel > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Cancel" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + Cancel + ColumnEnd); }
                if (Scheduled > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Scheduled" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + Scheduled + ColumnEnd); }
                if (SelfPay > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Self Pay" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + SelfPay + ColumnEnd); }
                if (Pending > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Pending" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + Pending + ColumnEnd); }
                if (ReSchedule > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "ReSchedule" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + ReSchedule + ColumnEnd); }
                if (OnCall > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "On Call" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + OnCall + ColumnEnd); }
                if (QRCheckIn > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "QR Check In" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + QRCheckIn + ColumnEnd); }
                if (Meeting > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Meeting" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + Meeting + ColumnEnd); }
                if (NoAnswer > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "No Answer" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + NoAnswer + ColumnEnd); }
                if (NoResponse > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "No Response" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + NoResponse + ColumnEnd); }
                if (NoShow > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "No Show" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + NoShow + ColumnEnd); }
                if (Voice > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Voice" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + Voice + ColumnEnd); }
                if (Waiting > 0)
                { HeadingColumnToShow.Append(thColumnStart + ">" + "Waiting" + thColumnEnd); colspan++; FooterColumnToShow.Append(ColumnStart + ">" + Waiting + ColumnEnd); }
                string appointStatusHeader = string.Empty;
                String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='4' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>";

                if (totalAppointment > 0)
                    appointStatusHeader = "<th class='noWordBreak text-center' style='background: #468cec' colspan='" + colspan + "'>  Appointment Status";

                TableHeading = TableHeading + appointStatusHeader + thColumnEnd +
                       RowEnd +
                       RowStart
                       + thColumnStart + " style='text-align:left'>" + "Practice " + thColumnEnd +
                            thColumnStart + ">" + "Provider" + thColumnEnd +
                            thColumnStart + "> Facility" + thColumnEnd +
                            thColumnStart + "> Appointment Date" + thColumnEnd +
                            HeadingColumnToShow.ToString() +
                            thColumnStart + ">" + "Notes Created" + thColumnEnd +
                            thColumnStart + ">" + "Notes Not Created Yet" + thColumnEnd +
                            thColumnStart + ">" + "Charges Created " + thColumnEnd +
                            thColumnStart + ">" + "Charges Not Created Yet" + thColumnEnd
                         + RowEnd + TableHeadEnd;

                sb.Append(PrintDivStart);
                sb.Append(TableStart);
                sb.Append(TableHeading);

                if (dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].Rows.Count > 0)
                {



                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryA.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.PracticeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + " style='text-align:center'>" + dr[dsReports.DT_AppointmentVsClaimSummaryA.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + " style='text-align:left'>" + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_AppointmentVsClaimSummaryA.AppointmentDateColumn].ToString()) + ColumnEnd);
                        if (totalAppointment > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.Total_ApptColumn] + ColumnEnd);
                        if (CheckOut > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.Check_OutColumn] + ColumnEnd);
                        if (CheckIn > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.Check_InColumn] + ColumnEnd);
                        if (Confirm > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.ConfirmColumn] + ColumnEnd);
                        if (Cancel > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.CancelColumn] + ColumnEnd);
                        if (Scheduled > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.ScheduledColumn] + ColumnEnd);
                        if (SelfPay > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.Self_PayColumn] + ColumnEnd);
                        if (Pending > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.PendingColumn] + ColumnEnd);
                        if (ReSchedule > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.ReScheduleColumn] + ColumnEnd);
                        if (OnCall > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.On_CallColumn] + ColumnEnd);
                        if (QRCheckIn > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.QR_Check_InColumn] + ColumnEnd);
                        if (Meeting > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.MeetingColumn] + ColumnEnd);
                        if (NoAnswer > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.No_AnswerColumn] + ColumnEnd);
                        if (NoResponse > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.No_ResponseColumn] + ColumnEnd);
                        if (NoShow > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.No_ShowColumn] + ColumnEnd);
                        if (Voice > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.VoiceColumn] + ColumnEnd);
                        if (Waiting > 0)
                            sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.WaitingColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.Notes_CreatedColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.Notes_Not_Created_YetColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.Charges_CreatedColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryA.Charges_Not_Created_YetColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                    }
                    sb.Append(RowStart);
                    sb.Append("<td colspan='4' style='font-weight: bold;'>" + "Total" + ColumnEnd);
                    sb.Append(FooterColumnToShow.ToString());
                    sb.Append(ColumnStart + ">" + NotesCreated + ColumnEnd);
                    sb.Append(ColumnStart + ">" + NotesNotCreated + ColumnEnd);
                    sb.Append(ColumnStart + ">" + ChargeCreated + ColumnEnd);
                    sb.Append(ColumnStart + ">" + ChargeNotCreated + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);

                }
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryB.TableName].Rows.Count > 0)
                {
                    // second table

                    String TableHeading1 = TableHead +
                       RowStart
                       + thColumnStart + " style='text-align:left'>" + "Status " + thColumnEnd +
                            thColumnStart + ">" + "Claim Count" + thColumnEnd +
                            RowEnd + TableHeadEnd;
                    sb.Append("<div style='width: 27%;'>");
                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading1);
                    Int64 ClaimCount = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_AppointmentVsClaimSummaryB.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryB.ClaimStatusesColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimSummaryB.Claim_CountColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                        ClaimCount = ClaimCount + MDVUtility.ToInt64(dr[dsReports.DT_AppointmentVsClaimSummaryB.Claim_CountColumn]);
                    }
                    sb.Append(RowStart);
                    sb.Append(ColumnStart + " style='font-weight: bold;' >" + "Grand Total" + ColumnEnd);
                    sb.Append(ColumnStart + ">" + ClaimCount + ColumnEnd);
                    sb.Append(RowEnd);
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);

                }
                return sb.ToString();


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadAppointmentVsClaimSummary", ex);
                return ex.Message;
            }
        }
        /// <summary>
        /// Get Detail Report For Appointment vs Claim
        /// </summary>
        /// <param name="ReportsParamaters"></param>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public String LoadAppointmentVsClaimDetail(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName, true);

                if (dsReports.Tables[dsReports.DT_AppointmentVsClaimDetail.TableName].Rows.Count > 0)
                {

                    StringBuilder sb = new StringBuilder();
                    sb = sb.Append(string.Empty);
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead +
                       RowStart
                       + thColumnStart + " style='text-align:left'>" + "Practice " + thColumnEnd +
                            thColumnStart + ">" + "Facility" + thColumnEnd +
                            thColumnStart + "> Appt.Provider" + thColumnEnd +
                            thColumnStart + "> Date" + thColumnEnd +
                            thColumnStart + ">" + "Time" + thColumnEnd +
                            thColumnStart + ">" + "Account" + thColumnEnd +
                            thColumnStart + ">" + "Patient Name" + thColumnEnd +
                            thColumnStart + ">" + "Appt.Status" + thColumnEnd +
                            thColumnStart + ">" + "Notes Created" + thColumnEnd +
                            thColumnStart + ">" + "Notes Status" + thColumnEnd +
                            thColumnStart + ">" + "Charges On eSuperBill" + thColumnEnd +
                            thColumnStart + ">" + "Created Claims" + thColumnEnd +
                            thColumnStart + ">" + "No.Of Charges" + thColumnEnd +
                            thColumnStart + ">" + "Claim Status" + thColumnEnd
                         + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_AppointmentVsClaimDetail.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.PracticeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + " style='text-align:center'>" + dr[dsReports.DT_AppointmentVsClaimDetail.Appt_ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + " style='text-align:left'>" + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_AppointmentVsClaimDetail.DateColumn].ToString()) + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.TimeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.AccountColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.NotesCreatedColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.NotesStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.Charges_on_eSuperBillColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.Created_ClaimsColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.No_Of_ChargesColumn] + ColumnEnd);
                        sb.Append(ColumnStart + ">" + dr[dsReports.DT_AppointmentVsClaimDetail.Claim_StatusColumn] + ColumnEnd);

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();

                }
                else
                    return string.Empty;



            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadAppointmentVsClaimDetail", ex);
                return ex.Message;
            }
        }
        /// <summary>
        /// When only one group is selected in report
        /// </summary>
        /// <param name="ReportsParamaters"></param>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public String LoadAppointmentVsClaimDetailSingleGroup(Dictionary<string, object> ReportsParamaters, String ReportName, string GroupByColumn)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName, true);
                StringBuilder GroupbyHeadingColumns = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                sb = sb.Append(string.Empty);
                string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                string PrintDivEnd = "</div>";
                String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                String TableHead = "<thead class='printHeading'>";
                String thColumnStart = "<th class='noWordBreak'";
                String thColumnEnd = "</th>";
                String TableHeadEnd = "</thead>";
                String TableEnd = "</table>";
                String RowStart = "<tr>";
                String RowEnd = "</tr>";
                String ColumnStart = "<td";
                String ColumnEnd = "</td>";
                String TableHeading = TableHead +
                       RowStart;
                String HeadingColumn =
                            thColumnStart + " style='text-align:left'>" + "Practice " + thColumnEnd +
                            thColumnStart + "> Date" + thColumnEnd +
                            thColumnStart + ">" + "Time" + thColumnEnd +
                            thColumnStart + ">" + "Account" + thColumnEnd +
                            thColumnStart + ">" + "Patient Name" + thColumnEnd +
                            thColumnStart + ">" + "Appt.Status" + thColumnEnd +
                            thColumnStart + ">" + "Notes Created" + thColumnEnd +
                            thColumnStart + ">" + "Notes Status" + thColumnEnd +
                            thColumnStart + ">" + "Charges On eSuperBill" + thColumnEnd +
                            thColumnStart + ">" + "Created Claims" + thColumnEnd +
                            thColumnStart + ">" + "No.Of Charges" + thColumnEnd +
                            thColumnStart + ">" + "Claim Status" + thColumnEnd
                         + RowEnd + TableHeadEnd;

                sb.Append(PrintDivStart);
                sb.Append(TableStart);
                sb.Append(TableHeading);
                if (GroupByColumn.ToLower() == "facility")
                {
                    sb.Append(thColumnStart + "> Facility " + thColumnEnd);
                    sb.Append(thColumnStart + ">" + "Appt.Provider" + thColumnEnd);
                }
                if (GroupByColumn.ToLower() == "Appt Provider".ToLower())
                {
                    sb.Append(thColumnStart + ">  Appt.Provider" + thColumnEnd);
                    sb.Append(thColumnStart + ">" + " Facility" + thColumnEnd);
                }
                sb.Append(HeadingColumn);
                if (dsReports.Tables[dsReports.DT_AppointmentVsClaimDetail.TableName].Rows.Count > 0)
                {

                    var result = from row in dsReports.Tables[dsReports.DT_AppointmentVsClaimDetail.TableName].AsEnumerable()
                                 group row by row.Field<string>(GroupByColumn.ToLower()) into grp
                                 select new
                                 {
                                     Columnkey = grp.Key,
                                     AppointmentDetail = grp,
                                 };

                    foreach (var item in result)
                    {
                        bool appendGroupName = true;
                        foreach (var subitem in item.AppointmentDetail)
                        {
                            sb.Append(RowStart);
                            if (appendGroupName)
                            { sb.Append(ColumnStart + ">" + item.Columnkey + ColumnEnd); appendGroupName = false; }
                            else
                                sb.Append(ColumnStart + ">" + ColumnEnd);
                            if (GroupByColumn.ToLower() == "facility")
                                sb.Append(ColumnStart + ">" + subitem.Field<string>("Appt Provider") + ColumnEnd);
                            if (GroupByColumn.ToLower() == "Appt Provider".ToLower())
                                sb.Append(ColumnStart + ">" + subitem.Field<string>("Facility") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Practice") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(subitem.Field<string>("Date")) + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Time") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Account") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("PatientName") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Status") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("NotesCreated") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("NotesStatus") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Charges on eSuperBill") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Created Claims") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("No Of Charges") + ColumnEnd);
                            sb.Append(ColumnStart + ">" + subitem.Field<string>("Claim Status") + ColumnEnd);
                            sb.Append(RowEnd);
                        }
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadAppointmentVsClaimDetailSingleGroup", ex);
                return ex.Message;
            }
        }
        /// <summary>
        /// When more than one group is selected in report
        /// </summary>
        /// <param name="ReportsParamaters"></param>
        /// <param name="ReportName"></param>
        /// <param name="GroupByColumn"></param>
        /// <returns></returns>
        public String LoadAppointmentVsClaimDetailMutlipleGroup(Dictionary<string, object> ReportsParamaters, String ReportName, string Group1, string Group2)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName, true);
                StringBuilder GroupbyHeadingColumns = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                sb = sb.Append(string.Empty);
                string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                string PrintDivEnd = "</div>";
                String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                String TableHead = "<thead class='printHeading'>";
                String thColumnStart = "<th class='noWordBreak'";
                String thColumnEnd = "</th>";
                String TableHeadEnd = "</thead>";
                String TableEnd = "</table>";
                String RowStart = "<tr>";
                String RowEnd = "</tr>";
                String ColumnStart = "<td";
                String ColumnEnd = "</td>";
                String TableHeading = TableHead +
                       RowStart;
                String HeadingColumn =
                            thColumnStart + " style='text-align:left'>" + "Practice " + thColumnEnd +
                            thColumnStart + "> Date" + thColumnEnd +
                            thColumnStart + ">" + "Time" + thColumnEnd +
                            thColumnStart + ">" + "Account" + thColumnEnd +
                            thColumnStart + ">" + "Patient Name" + thColumnEnd +
                            thColumnStart + ">" + "Appt.Status" + thColumnEnd +
                            thColumnStart + ">" + "Notes Created" + thColumnEnd +
                            thColumnStart + ">" + "Notes Status" + thColumnEnd +
                            thColumnStart + ">" + "Charges On eSuperBill" + thColumnEnd +
                            thColumnStart + ">" + "Created Claims" + thColumnEnd +
                            thColumnStart + ">" + "No.Of Charges" + thColumnEnd +
                            thColumnStart + ">" + "Claim Status" + thColumnEnd
                         + RowEnd + TableHeadEnd;

                sb.Append(PrintDivStart);
                sb.Append(TableStart);
                sb.Append(TableHeading);
                if (Group1.ToLower() == "facility")
                {
                    sb.Append(thColumnStart + "> Facility " + thColumnEnd);
                    sb.Append(thColumnStart + ">" + "Appt.Provider" + thColumnEnd);
                }
                else
                {
                    sb.Append(thColumnStart + ">  Appt.Provider" + thColumnEnd);
                    sb.Append(thColumnStart + ">" + " Facility" + thColumnEnd);
                }
                sb.Append(HeadingColumn);
                if (dsReports.Tables[dsReports.DT_AppointmentVsClaimDetail.TableName].Rows.Count > 0)
                {

                    var result = from row in dsReports.Tables[dsReports.DT_AppointmentVsClaimDetail.TableName].AsEnumerable()
                                 group row by row.Field<string>(Group1.ToLower()) into grp1
                                 select new
                                 {
                                     Group1key = grp1.Key,
                                     Group1Detail = grp1,
                                     Group2Detail = from row in grp1
                                                    group row by row.Field<string>(Group2.ToLower()) into grp2
                                                    select new
                                                    {
                                                        Group2key = grp2.Key,
                                                        Group2Details = grp2
                                                    }
                                 };
                    foreach (var item in result)
                    {
                        bool appendGroup1Name = true;
                        foreach (var subitem in item.Group2Detail)
                        {
                            bool appendGroup2Name = true;
                            foreach (var subitem1 in subitem.Group2Details)
                            {
                                sb.Append(RowStart);
                                if (appendGroup1Name)
                                { sb.Append(ColumnStart + ">" + item.Group1key + ColumnEnd); appendGroup1Name = false; }
                                else
                                    sb.Append(ColumnStart + ">" + ColumnEnd);
                                if (appendGroup2Name)
                                { sb.Append(ColumnStart + ">" + subitem.Group2key + ColumnEnd); appendGroup2Name = false; }
                                else
                                    sb.Append(ColumnStart + ">" + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Practice") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + MDVUtility.GetDateMMDDYYY(subitem1.Field<string>("Date")) + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Time") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Account") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("PatientName") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Status") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("NotesCreated") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("NotesStatus") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Charges on eSuperBill") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Created Claims") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("No Of Charges") + ColumnEnd);
                                sb.Append(ColumnStart + ">" + subitem1.Field<string>("Claim Status") + ColumnEnd);
                                sb.Append(RowEnd);
                            }
                        }
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadAppointmentVsClaimDetailMutlipleGroup", ex);
                return ex.Message;
            }
        }
        public String LoadClaimUnderPaidByPrimaryInsurance(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    //String thColumnStart = "<th class='noWordBreak'>";
                    //String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td style='text-align:right;'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='center' colspan='14'><b>";
                    String FootercolumnBoldStartDollar = "<td style='text-align:right;'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + "<th class='noWordBreak'>Practice</th>"
                        + "<th class='noWordBreak'>Provider</th>"
                        + "<th class='noWordBreak'>Provider Specialty</th>"
                        + "<th class='noWordBreak'>Facility</th>"
                        + "<th class='noWordBreak'>Insurance Plan</th>"
                        + "<th class='noWordBreak'>Payer ID</th>"
                        + "<th class='noWordBreak'>Account #</th>"
                        + "<th class='noWordBreak'>Patient Name</th>"
                        + "<th class='noWordBreak'>Subscriber ID</th>"
                        + "<th class='noWordBreak'>Claim Number</th>"
                        + "<th class='noWordBreak'>DOS</th>"
                        + "<th class='noWordBreak'>CPT</th>"
                        + "<th class='noWordBreak'>Claim Date</th>"
                        + "<th class='noWordBreak'>Claim Submission Date</th>"
                        + "<th class='noWordBreak'>Billed Amount</th>"
                        + "<th class='noWordBreak'>Expected Amount</th>"
                        + "<th class='noWordBreak'>Allowed Amount</th>"
                        + "<th class='noWordBreak'>Paid Amount</th>"
                        + "<th class='noWordBreak'>Check Date</th>"
                        + "<th class='noWordBreak'>Posting Method</th>"
                        + "<th class='noWordBreak'>Write-Off</th>"
                        + "<th class='noWordBreak'>Under Paid Amount</th>"
                        + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double BilledAmountSum = 0;
                    double ExpectedAmountSum = 0;
                    double AllowedAmountSum = 0;
                    double PaidAmountSum = 0;
                    double WriteoffAmountSum = 0;
                    double UnderpaidAmountSum = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.PracticeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ProviderSpecialtyColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.PayerIdColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.SubscriberIdColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.DOSColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.DOSColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.DOSColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.CPTCodeColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ClaimDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ClaimDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ClaimDateColumn] + ColumnEnd);//
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ClaimSubmissionDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ClaimSubmissionDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ClaimSubmissionDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.BilledAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ExpectedAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.AllowedAmtColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.PaidAmountColumn])) + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.CheckDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.CheckDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.CheckDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.PostingMethodColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.Write_OffColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.UnderPaidAmountColumn])) + ColumnEnd);

                        sb.Append(RowEnd);

                        BilledAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.BilledAmountColumn]);
                        ExpectedAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.ExpectedAmountColumn]);
                        AllowedAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.AllowedAmtColumn]);
                        PaidAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.PaidAmountColumn]);
                        WriteoffAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.Write_OffColumn]);
                        UnderpaidAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_ClaimsUnderPaidByPrimaryInsurance.UnderPaidAmountColumn]);
                    }
                    BilledAmountSum = MDVUtility.ToAmount(BilledAmountSum);
                    ExpectedAmountSum = MDVUtility.ToAmount(ExpectedAmountSum);
                    AllowedAmountSum = MDVUtility.ToAmount(AllowedAmountSum);
                    PaidAmountSum = MDVUtility.ToAmount(PaidAmountSum);
                    WriteoffAmountSum = MDVUtility.ToAmount(WriteoffAmountSum);
                    UnderpaidAmountSum = MDVUtility.ToAmount(UnderpaidAmountSum);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", BilledAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", ExpectedAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AllowedAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PaidAmountSum) + FootercolumnBoldEnd);
                    sb.Append(ColumnStart + "" + ColumnEnd);
                    sb.Append(ColumnStart + "" + ColumnEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", WriteoffAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", UnderpaidAmountSum) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadClaimUnderPaidByPrimaryInsurance", ex);
                return ex.Message;
            }

        }

        public String LoadSecondaryInsuranceClaim(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Scecondary_Insurance_Claim.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    //String thColumnStart = "<th class='noWordBreak'>";
                    //String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td style='text-align:right;'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='right' colspan='11'><b>";
                    String FootercolumnBoldStartDollar = "<td style='text-align:right;'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + "<th class='noWordBreak'>Practice</th>"
                        + "<th class='noWordBreak'>Provider</th>"
                        + "<th class='noWordBreak'>Facility</th>"
                        + "<th class='noWordBreak'>Primary Insurance</th>"
                        + "<th class='noWordBreak'>Secondary Insurance</th>"
                        + "<th class='noWordBreak'>Account #</th>"
                        + "<th class='noWordBreak'>Patient Name</th>"
                        + "<th class='noWordBreak'>Subscriber ID</th>"
                        + "<th class='noWordBreak'>Claim Number</th>"
                        + "<th class='noWordBreak'>DOS</th>"
                        + "<th class='noWordBreak'>CPT</th>"
                        + "<th class='noWordBreak'>Pri Billed Amount</th>"
                        + "<th class='noWordBreak'>Pri Paid Amount</th>"
                        + "<th class='noWordBreak'>Pri Write-Off</th>"
                        + "<th class='noWordBreak'>Open Balance</th>"
                        + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double BilledAmountSum = 0;
                    double PaidAmountSum = 0;
                    double WriteoffAmountSum = 0;
                    double OpenBalanceAmountSum = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Scecondary_Insurance_Claim.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.PracticeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.FacilityColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.InsurancePlanPrimaryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.SubscriberIdColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Scecondary_Insurance_Claim.DOSColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Scecondary_Insurance_Claim.DOSColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.DOSColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Scecondary_Insurance_Claim.CPTCodeColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Scecondary_Insurance_Claim.BilledAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Scecondary_Insurance_Claim.PaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Scecondary_Insurance_Claim.Write_OffColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Scecondary_Insurance_Claim.OpenBalanceColumn])) + ColumnEnd);
                        sb.Append(RowEnd);
                        BilledAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Scecondary_Insurance_Claim.BilledAmountColumn]);
                        PaidAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Scecondary_Insurance_Claim.PaidAmountColumn]);
                        WriteoffAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Scecondary_Insurance_Claim.Write_OffColumn]);
                        OpenBalanceAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Scecondary_Insurance_Claim.OpenBalanceColumn]);
                    }
                    BilledAmountSum = MDVUtility.ToAmount(BilledAmountSum);
                    PaidAmountSum = MDVUtility.ToAmount(PaidAmountSum);
                    WriteoffAmountSum = MDVUtility.ToAmount(WriteoffAmountSum);
                    OpenBalanceAmountSum = MDVUtility.ToAmount(OpenBalanceAmountSum);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", BilledAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PaidAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", WriteoffAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", OpenBalanceAmountSum) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadSecondaryInsuranceClaim", ex);
                return ex.Message;
            }

        }

        public String LoadClaimFollowup(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Claim_Follow_Up.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    //String thColumnStart = "<th class='noWordBreak'>";
                    //String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td style='text-align:right;'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td align='right' colspan='13'><b>";
                    String FootercolumnBoldStartDollar = "<td style='text-align:right;'><b>$";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + "<th class='noWordBreak'>Provider</th>"
                        + "<th class='noWordBreak'>Facility</th>"
                        + "<th class='noWordBreak'>Ins Plan Category</th>"
                        + "<th class='noWordBreak'>Insurance Plan</th>"
                        + "<th class='noWordBreak'>Account #</th>"
                        + "<th class='noWordBreak'>Patient Name</th>"
                        + "<th class='noWordBreak'>DOS</th>"
                        + "<th class='noWordBreak'>Claim Number</th>"
                        + "<th class='noWordBreak'>First Submitt Date</th>"
                        + "<th class='noWordBreak'>Aging Days</th>"
                        + "<th class='noWordBreak'>Follow Up Group</th>"
                        + "<th class='noWordBreak'>Follow Up Action</th>"
                        + "<th class='noWordBreak'>Follow Up Reason</th>"
                        + "<th class='noWordBreak'>Total Charges</th>"
                        + "<th class='noWordBreak'>Balance</th>"
                        + "<th class='noWordBreak'>Follow Up Comments</th>"
                        + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double BilledAmountSum = 0;
                    double BalanceSum = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Claim_Follow_Up.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.RenderingProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.PlanCategoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claim_Follow_Up.DOSFromColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claim_Follow_Up.DOSFromColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.DOSFromColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claim_Follow_Up.firstsubmitteddateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claim_Follow_Up.firstsubmitteddateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.firstsubmitteddateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.AgingDaysColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.GroupNameColumn] + ColumnEnd);         
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.ActionNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.ReasonNameColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_Follow_Up.ClaimAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Claim_Follow_Up.BalanceColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Follow_Up.CommentsColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                        BilledAmountSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Follow_Up.ClaimAmountColumn]);
                        BalanceSum += MDVUtility.ToDouble(dr[dsReports.DT_Claim_Follow_Up.BalanceColumn]);
                    }
                    BilledAmountSum = MDVUtility.ToAmount(BilledAmountSum);
                    BalanceSum = MDVUtility.ToAmount(BalanceSum);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", BilledAmountSum) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", BalanceSum) + FootercolumnBoldEnd);
                    sb.Append(ColumnStart + "" + ColumnEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadSecondaryInsuranceClaim", ex);
                return ex.Message;
            }
        }

        public String LoadClaimSubmissionHistoryReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Claim_Submission_History_Report.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th rowspan='2' class='va-middle noWordBreak'>";
                    String thColumnStartMerge = "<th colspan='3' class='text-center noWordBreak'>";
                    String thColumnStartsplit = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlign = "<td class='text-right'>";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='8' align='center'><b>";
                    String FootercolumnBoldStartEnd = "<td colspan='4' align='center'><b>";
                    String FootercolumnBoldStart1 = "<td class='text-right'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + RowStart +
                        thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                         thColumnStart + "Current Responsibility" + thColumnEnd +
                         thColumnStart + "Account #" + thColumnEnd +
                         thColumnStart + "Patient Name" + thColumnEnd +
                         thColumnStart + "DOS" + thColumnEnd +
                         thColumnStart + "Claim Number" + thColumnEnd +
                         thColumnStart + "Claim Date" + thColumnEnd +
                         thColumnStart + "Total Charges" + thColumnEnd +
                         thColumnStart + "Total Paid" + thColumnEnd +
                         thColumnStart + "Total Adjustment" + thColumnEnd +
                         thColumnStart + "Balance" + thColumnEnd +
                         thColumnStart + "Insurance Type" + thColumnEnd +
                         thColumnStartMerge + "Submission" + thColumnEnd + RowEnd +

                         RowStart +
                         thColumnStartsplit + "Insurance Plan" + thColumnEnd +
                         thColumnStartsplit + "Submit Date" + thColumnEnd +
                         thColumnStartsplit + "Submitted By" + thColumnEnd +
                         RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double BilledAmountColumn = 0;
                    double PaymentsColumn = 0;
                    double AdjustmentColumn = 0;
                    double BalanceColumn = 0;
                    

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Claim_Submission_History_Report.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submission_History_Report.RenderingProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submission_History_Report.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submission_History_Report.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submission_History_Report.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submission_History_Report.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_Claim_Submission_History_Report.DOSFromColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submission_History_Report.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_Claim_Submission_History_Report.createdonColumn])) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Claim_Submission_History_Report.ClaimAmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Claim_Submission_History_Report.ClaimAmountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Claim_Submission_History_Report.ClaimAmountColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Claim_Submission_History_Report.PaymentsColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Claim_Submission_History_Report.PaymentsColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Claim_Submission_History_Report.PaymentsColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Claim_Submission_History_Report.AdjustmentsColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Claim_Submission_History_Report.AdjustmentsColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Claim_Submission_History_Report.AdjustmentsColumn])))) + ColumnEnd);
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Claim_Submission_History_Report.BalanceColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Claim_Submission_History_Report.BalanceColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Claim_Submission_History_Report.BalanceColumn])))) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToInt(dr[dsReports.DT_Claim_Submission_History_Report.PlanPriorityColumn]) == 1 ? "Primary" : MDVUtility.ToInt(dr[dsReports.DT_Claim_Submission_History_Report.PlanPriorityColumn]) == 2 ? "Secondary" : MDVUtility.ToInt(dr[dsReports.DT_Claim_Submission_History_Report.PlanPriorityColumn]) == 3 ? "Tertiary" : "") + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submission_History_Report.SubmittedInsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_Claim_Submission_History_Report.SubmittedOnColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claim_Submission_History_Report.SubmittedByColumn] + ColumnEnd);
                        sb.Append(RowEnd);

                        BilledAmountColumn += (MDVUtility.ToInt(dr[dsReports.DT_Claim_Submission_History_Report.rColumn]) == 1 ? MDVUtility.ToDouble(dr[dsReports.DT_Claim_Submission_History_Report.ClaimAmountColumn]) : 0);
                        PaymentsColumn += (MDVUtility.ToInt(dr[dsReports.DT_Claim_Submission_History_Report.rColumn]) == 1 ? MDVUtility.ToDouble(dr[dsReports.DT_Claim_Submission_History_Report.PaymentsColumn]) : 0);
                        AdjustmentColumn += (MDVUtility.ToInt(dr[dsReports.DT_Claim_Submission_History_Report.rColumn]) == 1 ? MDVUtility.ToDouble(dr[dsReports.DT_Claim_Submission_History_Report.AdjustmentsColumn]) : 0);
                        BalanceColumn += (MDVUtility.ToInt(dr[dsReports.DT_Claim_Submission_History_Report.rColumn]) == 1 ? MDVUtility.ToDouble(dr[dsReports.DT_Claim_Submission_History_Report.BalanceColumn]) : 0);
                    }

                    BilledAmountColumn = MDVUtility.ToAmount(BilledAmountColumn);
                    PaymentsColumn = MDVUtility.ToAmount(PaymentsColumn);
                    AdjustmentColumn = MDVUtility.ToAmount(AdjustmentColumn);
                    BalanceColumn = MDVUtility.ToAmount(BalanceColumn);


                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (BilledAmountColumn >= 0 ? String.Format("${0:#,###,##0.00}", BilledAmountColumn) : String.Format("(${0:#,###,##0.00})", Math.Abs(BilledAmountColumn))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (PaymentsColumn >= 0 ? String.Format("${0:#,###,##0.00}", PaymentsColumn) : String.Format("(${0:#,###,##0.00})", Math.Abs(PaymentsColumn))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (AdjustmentColumn >= 0 ? String.Format("${0:#,###,##0.00}", AdjustmentColumn) : String.Format("(${0:#,###,##0.00})", Math.Abs(AdjustmentColumn))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + (BalanceColumn >= 0 ? String.Format("${0:#,###,##0.00}", BalanceColumn) : String.Format("(${0:#,###,##0.00})", Math.Abs(BalanceColumn))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartEnd + "" + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadInsurancePlanARReport", ex);
                return ex.Message;
            }


        }
        public String LoadClaimScrubberErrors(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_ClaimScrubberErrors.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th rowspan='2' class='va-middle noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + RowStart +
                        thColumnStart + "Patient" + thColumnEnd +
                        thColumnStart + "Account #" + thColumnEnd +
                        thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "DOS" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Claim Number" + thColumnEnd +
                        thColumnStart + "Claim Date" + thColumnEnd +
                        thColumnStart + "Insurance Plan" + thColumnEnd +
                        thColumnStart + "Error Code" + thColumnEnd +
                        thColumnStart + "Error Message" + thColumnEnd +
                        thColumnStart + "Suggested Action" + thColumnEnd +
                        thColumnStart + "Mark as Clean" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
      
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_ClaimScrubberErrors.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimScrubberErrors.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimScrubberErrors.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimScrubberErrors.RenderingProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_ClaimScrubberErrors.DOSFromColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimScrubberErrors.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimScrubberErrors.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsReports.DT_ClaimScrubberErrors.createdonColumn])) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimScrubberErrors.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimScrubberErrors.ErrorCodeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimScrubberErrors.DescriptionColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ClaimScrubberErrors.ActionColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToBool(dr[dsReports.DT_ClaimScrubberErrors.IsCleanClaimColumn]) == true ? "Yes" : "No") + ColumnEnd);
                        sb.Append(RowEnd);
                    }

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadInsurancePlanARReport", ex);
                return ex.Message;
            }
        }

        #endregion

        #region Clinical Reports
        public String LoadAllergiesReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Allergies.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account Number" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Allergy" + thColumnEnd +
                          thColumnStart + "Reactions" + thColumnEnd +
                          thColumnStart + "Onset Date" + thColumnEnd +
                          thColumnStart + "AllergyStatus" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Allergies.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Allergies.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Allergies.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Allergies.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Allergies.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Allergies.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Allergies.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Allergies.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Allergies.AllergyColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Allergies.ReactionColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Allergies.OnSetDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Allergies.OnSetDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Allergies.OnSetDateColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Allergies.OnSetDateColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToBool(dr[dsReports.DT_Allergies.AllergyStatusColumn]) == true ? "Active" : "Inactive") + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadAllergiesReport", ex);
                return ex.Message;
            }


        }

        public String LoadMonthlyPaymentTrend(Dictionary<string, object> ReportsParamaters, String ReportName) {
            try
            {

                StringBuilder sb = new StringBuilder();
                StringBuilder columnHead = new StringBuilder();
                StringBuilder secondColumnHead = new StringBuilder();
                StringBuilder tableFooterColumns = new StringBuilder();
                StringBuilder completeTableRow = new StringBuilder();
              


                DSReports dsReports = new DSReports();
                double totalPaymentInMonth =0.0;
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);

                String TableHeading;
                String SecondTableHeading;
               
                if (dsReports.Tables[dsReports.DT_Monthly_Payment_Trend_Report.TableName].Rows.Count > 0)
                {
                    // get distinct data
                    DataView view = new DataView(dsReports.Tables[dsReports.DT_Monthly_Payment_Trend_Report.TableName]);
                    view.Sort = "MonthNumber asc";
                    DataTable distinctValuesProviderIds = view.ToTable(true, "ResourceProviderId");
                    string[] monthdetail = new string[2];
                    monthdetail[0] = "YearMonth";
                    monthdetail[1] = "MonthNumber";
                    DataTable YearMonth = view.ToTable(true, monthdetail);
                    


                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";                   
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";

                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnStartGray = "<th class='noWordBreak'  style='background-color:#468aea'>";
                    String RightAlignthColumnStart = "<th class='noWordBreak' style='text-align: right;'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td style='text-align: right;'>";
                    String ColumnEnd = "</td>";
                    String LeftAlignColumnStart = "<td>";
                    

                    TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'>" + RowEnd +
                        RowStart
                      + thColumnStart + "Month-Year" + thColumnEnd;

                    SecondTableHeading = RowStart
                      + thColumnStart + "Providers" ;

                    var totalPaymentRecord = "";
                    totalPaymentRecord = RowStart + LeftAlignColumnStart + "<b>Total payments<b>" + ColumnEnd;
                    foreach (DataRow dr in YearMonth.Rows)
                    {
                      //("Table header")
                      columnHead.Append(thColumnStart + dr["YearMonth"] + thColumnEnd);
                      secondColumnHead.Append(thColumnStartGray + ""+ thColumnEnd);

                      DataRow[] MonthwisePaymentRecord = dsReports.DT_Monthly_Payment_Trend_Report.Select(dsReports.DT_Monthly_Payment_Trend_Report.YearMonthColumn.ColumnName + " = '" + dr.Field<string>("YearMonth") + "'");

                        totalPaymentInMonth = 0.0;
                        // clauclate the Payment's sum by month 
                        foreach (DataRow monthlyproviderPayment in MonthwisePaymentRecord){
                            string providerPaymentforMonth = monthlyproviderPayment["Payments"].ToString();
                            totalPaymentInMonth += MDVUtility.ToDouble(providerPaymentforMonth);
                        }
                        // Append total payment by month (Table's footer)
                       
                        
                        tableFooterColumns.Append(RightAlignthColumnStart + "$"+ String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(totalPaymentInMonth)) + thColumnEnd);
                       
                    }



                    // This dectionary will  contain monthly  provider's payment.
                    Dictionary<string, string> tdDec = new Dictionary<string, string>();
                    StringBuilder tempTableRow = new StringBuilder();
                    foreach (DataRow item in distinctValuesProviderIds.Rows)
                    {
                        // get all month record agaist unique provider id.
                        DataRow[] providerRecords = dsReports.DT_Monthly_Payment_Trend_Report.Select(dsReports.DT_Monthly_Payment_Trend_Report.ResourceProviderIdColumn.ColumnName + " = '" + item.Field<string>("ResourceProviderId") + "'");                       
                        var providernamecolumn = "";                       
                        tempTableRow = tempTableRow.Clear();

                        // Iterate through all Monthly payment record of one provider.
                        foreach (var providerOneRecord in providerRecords)
                        {
                            var providerName = providerOneRecord["ProviderName"];
                            providernamecolumn = RowStart + LeftAlignColumnStart + providerName + ColumnEnd; 
                           
                            // fill  dictionary on the basis of provider's monthly payment.                          
                            foreach (DataRow yearmonth in YearMonth.Rows)
                            {
                                
                                if (providerOneRecord["YearMonth"].ToString() == yearmonth["YearMonth"].ToString())
                                {
                                    tdDec[yearmonth["YearMonth"].ToString()] = ColumnStart + "$"+ String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount( providerOneRecord["Payments"]))+ ColumnEnd;
                                    
                                }
                                else
                                {  // if month does not exist then 
                                    if (!tdDec.ContainsKey(yearmonth["YearMonth"].ToString()))
                                    tdDec[yearmonth["YearMonth"].ToString()] = ColumnStart+ "$" + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount("0.0"))+ ColumnEnd;

                                }
                            }

                           
                          
                        }
                        // make the Provider's record columns 
                        foreach (var ProviderPaymentColumn in  tdDec)
                        {
                            tempTableRow.Append(ProviderPaymentColumn.Value);
                        }
                      // append one specifec provider's record row
                      completeTableRow.Append(providernamecolumn + tempTableRow.ToString() + RowEnd);

                    }

                    sb.Append(PrintDivStart);
                    sb.Append(TableStart);
                    sb.Append(TableHeading + columnHead.ToString());
                    sb.Append(SecondTableHeading + secondColumnHead.ToString() + RowEnd + TableHeadEnd);
                    sb.Append(RowStart);
                    sb.Append(completeTableRow);
                    sb.Append(RowEnd);
                    sb.Append(totalPaymentRecord + tableFooterColumns.ToString() + RowEnd);                   
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadMonthlyPaymentTrend", ex);
                return ex.Message;
            }
        }

        public String LoadProblemsReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Problems_Report.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Problem" + thColumnEnd +
                          thColumnStart + "Problem Status" + thColumnEnd +
                          thColumnStart + "Chronicity Level" + thColumnEnd +
                          thColumnStart + "Severity" + thColumnEnd +
                          thColumnStart + "Start Date" + thColumnEnd +
                          thColumnStart + "End Date" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Problems_Report.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Problems_Report.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Problems_Report.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Problems_Report.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Problems_Report.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Problems_Report.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Problems_Report.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Problems_Report.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Problems_Report.ProblemColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Problems_Report.PrStatusColumn]) == "True" ? "Active" : "Inactive") + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Problems_Report.ChronicityLevelColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Problems_Report.SeverityColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Problems_Report.StartDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Problems_Report.StartDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Problems_Report.StartDateColumn] + ColumnEnd);//
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Problems_Report.EndDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Problems_Report.EndDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Problems_Report.EndDateColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Problems_Report.StartDateColumn] + ColumnEnd);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Problems_Report.EndDateColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadProblemsReport", ex);
                return ex.Message;
            }


        }

        public String LoadProceduresReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Procedures_Report.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Procedure" + thColumnEnd +
                          thColumnStart + "ICD" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Start Date" + thColumnEnd +
                          thColumnStart + "End Date" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Procedures_Report.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Procedures_Report.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Procedures_Report.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Procedures_Report.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Procedures_Report.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Procedures_Report.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Procedures_Report.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Procedures_Report.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        //string PatStatus = string.Empty;
                        //if (Convert.ToInt32(dr[dsReports.DT_Procedures_Report.PatStatusColumn]) == 1)
                        //    PatStatus = "Active";
                        //else
                        //    PatStatus = "InActive";
                        //sb.Append(ColumnStart + PatStatus + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Procedures_Report.ProcedureColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Procedures_Report.ICDColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Procedures_Report.ProviderColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Procedures_Report.StartDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Procedures_Report.StartDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Procedures_Report.StartDateColumn] + ColumnEnd);//
                        }
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Procedures_Report.EndDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Procedures_Report.EndDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Procedures_Report.EndDateColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Procedures_Report.StartDateColumn] + ColumnEnd);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Procedures_Report.EndDateColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadProceduresReport", ex);
                return ex.Message;
            }


        }

        public String LoadProgressNoteReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Progress_Note.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Created Date" + thColumnEnd +
                          thColumnStart + "Account" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Home Phone" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Note Type" + thColumnEnd +
                          thColumnStart + "Practice" + thColumnEnd +
                          thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Ref. Provider" + thColumnEnd +
                           RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Progress_Note.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Progress_Note.CreatedOnColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Progress_Note.CreatedOnColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Progress_Note.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Progress_Note.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.DOBColumn] + ColumnEnd);
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.HomePhoneNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.NoteStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.NotesTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.ReferringProviderNameColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadZeroPaidClaim", ex);
                return ex.Message;
            }


        }

        public String LoadProgressNoteAmendementReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Progress_Note.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Created Date" + thColumnEnd +
                          thColumnStart + "Notes" + thColumnEnd +
                          thColumnStart + "Account" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Note Type" + thColumnEnd +
                          thColumnStart + "Practice" + thColumnEnd +
                          thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Ref. Provider" + thColumnEnd +
                          thColumnStart + "Amendments" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Progress_Note.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Progress_Note.CreatedOnColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Progress_Note.CreatedOnColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + "View Note" + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.NoteStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.NotesTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.ReferringProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + "Detail" + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadZeroPaidClaim", ex);
                return ex.Message;
            }


        }

        public String LoadPOSSurveyReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_POS_Survey.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Appointment Date" + thColumnEnd +
                          thColumnStart + "Encounter Id" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Gender" + thColumnEnd +
                          thColumnStart + "Race" + thColumnEnd +
                          thColumnStart + "Ethnicity" + thColumnEnd +
                          thColumnStart + "Email" + thColumnEnd +
                          thColumnStart + "SSN" + thColumnEnd +
                          thColumnStart + "Deceased" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Statement" + thColumnEnd +
                          thColumnStart + "Address" + thColumnEnd +
                          thColumnStart + "PCP Name" + thColumnEnd +
                          thColumnStart + "PCP NPI" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Provider NPI" + thColumnEnd +
                          thColumnStart + "Referrring Provider" + thColumnEnd +
                          thColumnStart + "Referring Provider NPI" + thColumnEnd +
                          thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Primary Insurance" + thColumnEnd +
                          thColumnStart + "Secondary Insurance" + thColumnEnd +
                          thColumnStart + "Tertiary Insurance" + thColumnEnd +
                          thColumnStart + "Visit type" + thColumnEnd +
                          thColumnStart + "Visit Status" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_POS_Survey.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.AppointmentDateColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.AppointmentIdColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_POS_Survey.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_POS_Survey.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.DOBColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.SexColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.RaceDescColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.EthnicityDescColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.EmailAddressColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.SSNColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_POS_Survey.DODColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_POS_Survey.DODColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.DODColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.PatientstatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.FirstStatementDateColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.PatientAddressColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.PCPNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.PCPNPIColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.ProviderNPIColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.ReferringProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.RefProvNPIColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.FirstPlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.SecondPlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.ThirdPlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.VisitTypeColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_POS_Survey.AppointmentStatusColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPOSSurveyReport", ex);
                return ex.Message;
            }


        }

        public String LoadPhoneEncounterReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_PhoneEncounter.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Created Date" + thColumnEnd +
                          thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Home Phone" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Duration" + thColumnEnd +
                          thColumnStart + "Practice" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Ref. Provider" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_PhoneEncounter.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_PhoneEncounter.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_PhoneEncounter.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_PhoneEncounter.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_PhoneEncounter.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_PhoneEncounter.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_PhoneEncounter.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_PhoneEncounter.HomePhoneNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_PhoneEncounter.NoteStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_PhoneEncounter.DurationColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_PhoneEncounter.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_PhoneEncounter.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_PhoneEncounter.ReferringProviderNameColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPhoneEncounterReport", ex);
                return ex.Message;
            }
        }

        public String LoadImmunizationReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Immunization_Report.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Category" + thColumnEnd +
                          thColumnStart + "Vaccine" + thColumnEnd +
                          thColumnStart + "Alert" + thColumnEnd +
                          thColumnStart + "Route" + thColumnEnd +
                          thColumnStart + "Reaction" + thColumnEnd +
                          thColumnStart + "Administered By" + thColumnEnd +
                          thColumnStart + "Admin Date" + thColumnEnd +
                          thColumnStart + "Due Date" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Immunization_Report.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Immunization_Report.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Immunization_Report.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Immunization_Report.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Immunization_Report.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CategoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.VaccineColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.AlertColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.RouteColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.ReactionColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.AdministeredByColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.AdminDateColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.DueDateColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadImmunizationReport", ex);
                return ex.Message;
            }
        }

        public String LoadMedicationsReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Medication_Report.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Medication" + thColumnEnd +
                          thColumnStart + "Med. Status" + thColumnEnd +
                          thColumnStart + "Start Date" + thColumnEnd +
                          thColumnStart + "End Date" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Medication_Report.TableName].Rows)
                    {
                        string medStatus = string.Empty;
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Medication_Report.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Medication_Report.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Medication_Report.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Medication_Report.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Medication_Report.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Medication_Report.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Medication_Report.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Medication_Report.MedicationColumn] + ColumnEnd);
                        if (Convert.ToInt32(dr[dsReports.DT_Medication_Report.MedStatusColumn]) == 0)
                            medStatus = "Past";
                        else if (Convert.ToInt32(dr[dsReports.DT_Medication_Report.MedStatusColumn]) == 1)
                            medStatus = "Current";
                        sb.Append(ColumnStart + medStatus + ColumnEnd);
                        if(!string.IsNullOrEmpty(dr[dsReports.DT_Medication_Report.StartDateColumn].ToString()))
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Medication_Report.StartDateColumn].ToString()) + ColumnEnd);
                        else sb.Append(ColumnStart + dr[dsReports.DT_Medication_Report.StartDateColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Medication_Report.StartDateColumn].ToString()))
                        { sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Medication_Report.EndDateColumn].ToString()) + ColumnEnd); }
                        else
                        { sb.Append(ColumnStart + dr[dsReports.DT_Medication_Report.EndDateColumn] + ColumnEnd); }
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadMedicationsReport", ex);
                return ex.Message;
            }
        }

        public String LoadVitalsReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Vitals_Report.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Systolic (mmHg)" + thColumnEnd +
                          thColumnStart + "Diastolic (mmHg)" + thColumnEnd +
                          thColumnStart + "Pulse (bpm)" + thColumnEnd +
                          thColumnStart + "Temp. (F)" + thColumnEnd +
                          thColumnStart + "Resp. (rpm)" + thColumnEnd +
                          thColumnStart + "Height (Inches)" + thColumnEnd +
                          thColumnStart + "BSA (m2)" + thColumnEnd +
                          thColumnStart + "Weight (lbs)" + thColumnEnd +
                          thColumnStart + "BMI (kg/m2)" + thColumnEnd +
                          thColumnStart + "SPO2 (%)" + thColumnEnd +
                          thColumnStart + "DOS" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Vitals_Report.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Vitals_Report.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Vitals_Report.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Vitals_Report.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Vitals_Report.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.SystolicColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.DiastolicColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.PulseColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.TempColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.RespColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.HeightColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.BSAColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.WeightColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.BMIColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.SPO2Column] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Vitals_Report.DOSColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Vitals_Report.DOSColumn].ToString()) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Vitals_Report.DOSColumn] + ColumnEnd);
                        }
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadVitalsReport", ex);
                return ex.Message;
            }
        }

        public String LoadOrdersLab(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Orders_Lab.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Test" + thColumnEnd +
                          thColumnStart + "Laboratory" + thColumnEnd +
                          thColumnStart + "Order Number" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Date & Time" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Orders_Lab.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Orders_Lab.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Orders_Lab.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Reports_Orders_Lab.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Reports_Orders_Lab.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.TestColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.LaboratoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.OrderNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.ProviderColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Orders_Lab.DateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + (MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Orders_Lab.DateColumn].ToString()) + " " + dr[dsReports.DT_Reports_Orders_Lab.TimeColumn]) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + (dr[dsReports.DT_Reports_Orders_Lab.DateColumn] + " " + dr[dsReports.DT_Reports_Orders_Lab.TimeColumn]) + ColumnEnd);
                        }
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadOrdersLab", ex);
                return ex.Message;
            }
        }

        public String LoadOrdersRadiology(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Orders_Lab.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Test" + thColumnEnd +
                          thColumnStart + "Radiology Type" + thColumnEnd +
                          thColumnStart + "Order Number" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Facility To" + thColumnEnd +
                          thColumnStart + "Date & Time" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Orders_Lab.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Orders_Lab.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Orders_Lab.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Reports_Orders_Lab.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Reports_Orders_Lab.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.TestColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.LaboratoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.OrderNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.FacilityToShortNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Orders_Lab.DateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + (MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Orders_Lab.DateColumn].ToString()) + " " + dr[dsReports.DT_Reports_Orders_Lab.TimeColumn]) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + (dr[dsReports.DT_Reports_Orders_Lab.DateColumn] + " " + dr[dsReports.DT_Reports_Orders_Lab.TimeColumn]) + ColumnEnd);
                        }
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadOrdersRadiology", ex);
                return ex.Message;
            }
        }

        public String LoadOrdersProcedure(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Orders_Lab.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Procedure" + thColumnEnd +
                          thColumnStart + "Order Number" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Assignee Provider" + thColumnEnd +
                          thColumnStart + "Date & Time" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Orders_Lab.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Orders_Lab.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Orders_Lab.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Reports_Orders_Lab.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Reports_Orders_Lab.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.ProceduresColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.OrderNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.AssigneeProviderColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Orders_Lab.DateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + (MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Orders_Lab.DateColumn].ToString()) + " " + dr[dsReports.DT_Reports_Orders_Lab.TimeColumn]) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + (dr[dsReports.DT_Reports_Orders_Lab.DateColumn] + " " + dr[dsReports.DT_Reports_Orders_Lab.TimeColumn]) + ColumnEnd);
                        }
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadOrdersProcedure", ex);
                return ex.Message;
            }
        }

        public String LoadOrdersConsultation(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Orders_Lab.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Procedure" + thColumnEnd +
                          thColumnStart + "Laboratory" + thColumnEnd +
                          thColumnStart + "Order Number" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Assignee Provider" + thColumnEnd +
                          thColumnStart + "Date & Time" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Orders_Lab.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Orders_Lab.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Orders_Lab.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Reports_Orders_Lab.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Reports_Orders_Lab.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.ProceduresColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.LaboratoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.OrderNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Lab.AssigneeProviderColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Orders_Lab.DateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + (MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Orders_Lab.DateColumn].ToString()) + " " + dr[dsReports.DT_Reports_Orders_Lab.TimeColumn]) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + (dr[dsReports.DT_Reports_Orders_Lab.DateColumn] + " " + dr[dsReports.DT_Reports_Orders_Lab.TimeColumn]) + ColumnEnd);
                        }
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadOrdersConsultation", ex);
                return ex.Message;
            }
        }

        public String LoadOrdersPrescription(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_Orders_Prescription.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Medication" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Pharmacy" + thColumnEnd +
                          thColumnStart + "Refill(s)" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Prescribed On" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_Orders_Prescription.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Prescription.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Prescription.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Orders_Prescription.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Orders_Prescription.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Prescription.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Reports_Orders_Prescription.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Reports_Orders_Prescription.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Prescription.MedicationColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Prescription.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Prescription.PharmacyColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Prescription.Refill_s_Column] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_Orders_Prescription.ProviderColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_Orders_Prescription.PrescribedONColumn].ToString()))
                        {
                            sb.Append(ColumnStart + (MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_Orders_Prescription.PrescribedONColumn].ToString())) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + (dr[dsReports.DT_Reports_Orders_Prescription.PrescribedONColumn]) + ColumnEnd);
                        }

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadOrdersPrescription", ex);
                return ex.Message;
            }
        }

        public String LoadResultsLab(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Results_Lab.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Test" + thColumnEnd +
                          thColumnStart + "Laboratory" + thColumnEnd +
                          thColumnStart + "Result Number" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Date & Time" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Results_Lab.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Results_Lab.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Results_Lab.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Results_Lab.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Results_Lab.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.TestColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.LaboratoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.ResultNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (dr[dsReports.DT_Results_Lab.ObservationDateColumn]) + ColumnEnd);

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadResultsLab", ex);
                return ex.Message;
            }
        }

        public String LoadResultsRadiology(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Results_Lab.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Test" + thColumnEnd +
                          thColumnStart + "Radiology Type" + thColumnEnd +
                          thColumnStart + "Result Number" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Date & Time" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Results_Lab.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Results_Lab.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Results_Lab.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Results_Lab.PatStatusColumn]) == "1" ? "Active" : (MDVUtility.ToStr(dr[dsReports.DT_Results_Lab.PatStatusColumn]) == "0" ? "Inactive" : "Deceased")) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.TestColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.LaboratoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.ResultNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.ProviderColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Results_Lab.DateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + (MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Results_Lab.DateColumn].ToString()) + " " + dr[dsReports.DT_Results_Lab.TimeColumn]) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + (dr[dsReports.DT_Results_Lab.DateColumn] + " " + dr[dsReports.DT_Results_Lab.TimeColumn]) + ColumnEnd);
                        }

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadResultsLab", ex);
                return ex.Message;
            }
        }

        public String LoadResultsConsultation(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Results_Lab.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Account #" + thColumnEnd +
                          thColumnStart + "Patient Name" + thColumnEnd +
                          thColumnStart + "DOB" + thColumnEnd +
                          thColumnStart + "Pt. Status" + thColumnEnd +
                          thColumnStart + "Test" + thColumnEnd +
                          thColumnStart + "Laboratory" + thColumnEnd +
                          thColumnStart + "Result Number" + thColumnEnd +
                          thColumnStart + "Status" + thColumnEnd +
                          thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Date & Time" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Results_Lab.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.PatientNameColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Results_Lab.DOBColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Results_Lab.DOBColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.DOBColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_Results_Lab.PatStatusColumn]) == "1" ? "Active" : "Inactive") + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.TestColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.LaboratoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.ResultNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.StatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Results_Lab.ProviderColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Results_Lab.DateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + (MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Results_Lab.DateColumn].ToString()) + " " + dr[dsReports.DT_Results_Lab.TimeColumn]) + ColumnEnd);
                        }
                        else
                        {
                            sb.Append(ColumnStart + (dr[dsReports.DT_Results_Lab.DateColumn] + " " + dr[dsReports.DT_Results_Lab.TimeColumn]) + ColumnEnd);
                        }

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadResultsLab", ex);
                return ex.Message;
            }
        }

        public String LoadAROReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_ARO_Report.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Event Id" + thColumnEnd +
                          thColumnStart + "Account Number" + thColumnEnd +
                          thColumnStart + "Location" + thColumnEnd +
                          thColumnStart + "Date Specimen Collected" + thColumnEnd +
                          thColumnStart + "Isolate ID" + thColumnEnd +
                          thColumnStart + "Specimen Group" + thColumnEnd +
                          thColumnStart + "Pathogen Description" + thColumnEnd +
                          thColumnStart + "Drug Description" + thColumnEnd +
                          thColumnStart + "Final Interpretation Description" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    Double EventId = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_ARO_Report.TableName].Rows)
                    {
                        EventId = EventId + 1;
                        sb.Append(RowStart);
                        //sb.Append(ColumnStart + dr[dsReports.DT_Immunization_Report.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + EventId + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ARO_Report.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ARO_Report.LocationColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_ARO_Report.DateSpecimenCollectedColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_ARO_Report.DateSpecimenCollectedColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_ARO_Report.DateSpecimenCollectedColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ARO_Report.OrderNoColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ARO_Report.SpecimenGroupColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ARO_Report.PathogenDescriptionColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ARO_Report.DrugDescriptionColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_ARO_Report.FinalInterpretationColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadResultsLab", ex);
                return ex.Message;
            }
        }

        public String LoadAUPReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_AUP_Report.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Facility Org Id" + thColumnEnd +
                          thColumnStart + "Facility Name" + thColumnEnd +
                          thColumnStart + "Summary year/Month" + thColumnEnd +
                          thColumnStart + "Antimicrobial Agent" + thColumnEnd +
                          thColumnStart + "Location" + thColumnEnd +
                          thColumnStart + "Antimicrobial Days" + thColumnEnd +
                          thColumnStart + "Route IM" + thColumnEnd +
                          thColumnStart + "Route IV" + thColumnEnd +
                          thColumnStart + "Route Digestive" + thColumnEnd +
                          thColumnStart + "Route Respiratory" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_AUP_Report.TableName].Rows)
                    {
                        sb.Append(RowStart);

                        sb.Append(ColumnStart + dr[dsReports.DT_AUP_Report.FacilityOrgIdColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AUP_Report.FacilityNameColumn] + ColumnEnd);
                        //sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_AUP_Report.PatStatusColumn]) == "1" ? "Active" : "Inactive") + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AUP_Report.SumYearMonthColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AUP_Report.AgentColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AUP_Report.LocationColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AUP_Report.AntimicrobialDaysColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AUP_Report.RouteIMColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AUP_Report.RouteIVColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AUP_Report.RouteDigestiveColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_AUP_Report.RouteRespiratoryColumn] + ColumnEnd);

                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadResultsLab", ex);
                return ex.Message;
            }
        }
        #endregion
        #region MPS Reports
        public String LoadARAging_AnalysisMPSReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_ARAging_Analysis.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartDollar = "<td class='text-right'>$";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='9' align='center'><b>";
                    String FootercolumnBoldStartDollar = "<td class='text-right'><b>$";
                    String FootercolumnBoldEnd = "</td></b>";
                    String TableHeading = TableHead
                         + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='10' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Insurance" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='5'>" + "Insurance AR" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='3'>" + "Patient" + thColumnEnd +
                        "<th class='noWordBreak text-center' style='background: #468cec' colspan='5'>" + "Patient AR" + thColumnEnd + RowEnd +
                        RowStart
                        + thColumnStart + "Pracitce" + thColumnEnd +
                        thColumnStart + "Facility" + thColumnEnd +
                        thColumnStart + "Provider " + thColumnEnd +
                        thColumnStart + "Claim Number " + thColumnEnd +
                        thColumnStart + "Charge Id" + thColumnEnd +
                        thColumnStart + "Account Number " + thColumnEnd +
                        thColumnStart + "Claim Status " + thColumnEnd +
                        thColumnStart + "DOS" + thColumnEnd +
                        thColumnStart + "Plan" + thColumnEnd +
                        thColumnStart + "Billed Charges" + thColumnEnd +
                        thColumnStart + "Paid Amount " + thColumnEnd +
                        thColumnStart + "Write Off" + thColumnEnd +
                        thColumnStart + "AR" + thColumnEnd +
                        thColumnStart + "Current" + thColumnEnd +
                        thColumnStart + "AR 30+" + thColumnEnd +
                        thColumnStart + "AR 60+ " + thColumnEnd +
                        thColumnStart + "AR 90+" + thColumnEnd +
                        thColumnStart + "AR 120+" + thColumnEnd +
                        //thColumnStart + "Charges" + thColumnEnd +
                        thColumnStart + "Paid Amount" + thColumnEnd +
                        thColumnStart + "Discount" + thColumnEnd +
                        thColumnStart + "Total AR" + thColumnEnd +
                        thColumnStart + "ARCurrent" + thColumnEnd +
                        thColumnStart + "AR 30+" + thColumnEnd +
                         thColumnStart + "AR 60+" + thColumnEnd +
                        thColumnStart + "AR 90+" + thColumnEnd +
                        thColumnStart + "AR 120+" + thColumnEnd +
                        thColumnStart + "Total AR" + thColumnEnd +
                        TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double InsCharges = 0;
                    double InsPaidAmount = 0;
                    double InsWriteOff = 0;
                    double TotalAR = 0;
                    double TotalARCurrent = 0;
                    double AR30 = 0;
                    double AR60 = 0;
                    double AR90 = 0;
                    double AR120 = 0;
                    double PatCharges = 0;
                    double PatPaidAmount = 0;
                    double Discount = 0;
                    double PatTotalAR = 0;
                    double PatTotalARCurrent = 0;
                    double PatAR30 = 0;
                    double PatAR60 = 0;
                    double PatAR90 = 0;
                    double PatAR120 = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_ARAging_Analysis.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.ChargeCapIdColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.SubmitStatusColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_ARAging_Analysis.DOSColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_ARAging_Analysis.DOSColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.DOSColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_ARAging_Analysis.PlanColumn] + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.BilledChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.InsurancePaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.InsuranceWriteOffColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARCurrentColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR30_Column])) + ColumnEnd);

                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR60_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR90_Column])) + ColumnEnd);

                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR120_Column])) + ColumnEnd);

                        //sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.InsuranceChargesColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.PatientPaidAmountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.PatientDiscountColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalPatientARColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.PatientARCurrentColumn])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR30_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR60_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR90_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR120_Column])) + ColumnEnd);
                        sb.Append(ColumnStartDollar + String.Format("{0:#,###,##0.00}", MDVUtility.ToAmount(dr[dsReports.DT_Reports_ARAging_Analysis.TotalARColumn])) + ColumnEnd);

                        sb.Append(RowEnd);

                        InsCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.BilledChargesColumn]);
                        InsPaidAmount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.InsurancePaidAmountColumn]);
                        InsWriteOff += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.InsuranceWriteOffColumn]);
                        TotalAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARColumn]);
                        TotalARCurrent += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalInsuranceARCurrentColumn]);
                        AR30 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR30_Column]);
                        AR60 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR60_Column]);
                        AR90 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR90_Column]);
                        AR120 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._InsuranceAR120_Column]);
                        //PatCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientChargesColumn]);
                        PatPaidAmount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientPaidAmountColumn]);
                        Discount += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientDiscountColumn]);
                        PatTotalAR += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalPatientARColumn]);
                        PatTotalARCurrent += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.PatientARCurrentColumn]);
                        PatAR30 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR30_Column]);
                        PatAR60 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR60_Column]);
                        PatAR90 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR90_Column]);
                        PatAR120 += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis._PatientAR120_Column]);
                        PatCharges += MDVUtility.ToDouble(dr[dsReports.DT_Reports_ARAging_Analysis.TotalARColumn]);
                    }
                    InsCharges = MDVUtility.ToAmount(InsCharges);
                    InsPaidAmount = MDVUtility.ToAmount(InsPaidAmount);
                    InsWriteOff = MDVUtility.ToAmount(InsWriteOff);
                    TotalAR = MDVUtility.ToAmount(TotalAR);
                    TotalARCurrent = MDVUtility.ToAmount(TotalARCurrent);
                    AR30 = MDVUtility.ToAmount(AR30);
                    AR60 = MDVUtility.ToAmount(AR60);
                    AR90 = MDVUtility.ToAmount(AR90);
                    AR120 = MDVUtility.ToAmount(AR120);
                    PatCharges = MDVUtility.ToAmount(PatCharges);
                    PatPaidAmount = MDVUtility.ToAmount(PatPaidAmount);
                    Discount = MDVUtility.ToAmount(Discount);
                    PatTotalAR = MDVUtility.ToAmount(PatTotalAR);
                    PatTotalARCurrent = MDVUtility.ToAmount(PatTotalARCurrent);
                    PatAR30 = MDVUtility.ToAmount(PatAR30);
                    PatAR60 = MDVUtility.ToAmount(PatAR60);
                    PatAR90 = MDVUtility.ToAmount(PatAR90);
                    PatAR120 = MDVUtility.ToAmount(PatAR120);
                    //make footer here
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsCharges) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsPaidAmount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", InsWriteOff) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", TotalAR) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", TotalARCurrent) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR30) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR60) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR90) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", AR120) + FootercolumnBoldEnd);
                    //sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatCharges) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatPaidAmount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", Discount) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatTotalAR) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatTotalARCurrent) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR30) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR60) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR90) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatAR120) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartDollar + String.Format("{0:#,###,##0.00}", PatCharges) + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPatientPayments", ex);
                return ex.Message;
            }


        }

        public String LoadPaymentEntriesReportMPS(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Reports_PaymentEnteries.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnStartTextAlign = "<td class='text-right'>";
                    String ColumnEnd = "</td>";
                    String FootercolumnBoldStart = "<td colspan='15' align='right'><b>";
                    String FootercolumnBoldStartEnd = "<td><b>";
                    String FooterColumnStartTextAlign = "<td class='text-right'><b>";
                    String FootercolumnBoldStart1 = "<td colspan='15'><b>";
                    String FootercolumnBoldEnd = "</b></td>";
                    String TableHeading = TableHead + thColumnStart + "Practice" + thColumnEnd +
                        thColumnStart + "Facility " + thColumnEnd +
                        thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Insurance Plan" + thColumnEnd +
                        thColumnStart + "Account" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Claim" + thColumnEnd +
                        thColumnStart + "Charge Id" + thColumnEnd +
                        thColumnStart + "Visit Date " + thColumnEnd +
                        thColumnStart + "Check Number" + thColumnEnd +
                          thColumnStart + "Payment Type" + thColumnEnd +
                        thColumnStart + "Apply To" + thColumnEnd +
                        thColumnStart + "System Category" + thColumnEnd +
                        thColumnStart + "Ledger Description" + thColumnEnd +
                         thColumnStart + "Payment Ledger Type" + thColumnEnd +
                        thColumnStart + "Date Paid" + thColumnEnd +
                              thColumnStart + "Amount" + thColumnEnd +
                                thColumnStart + "Entry Date" + thColumnEnd +
                                  thColumnStart + "Entered By" + thColumnEnd +
                                     TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);
                    double InsChrg = 0;
                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Reports_PaymentEnteries.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.PracticeNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.InsurancePlanColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.ClaimNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.ChargeIdColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_PaymentEnteries.VisitDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_PaymentEnteries.VisitDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.VisitDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.CheckNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.PaymentTypeColumn] + ColumnEnd);

                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.ApplyToColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.SystemCategoryColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.LedgerAccDescriptionColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.LedgerAccTypeColumn] + ColumnEnd);

                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_PaymentEnteries.DatePaidColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_PaymentEnteries.DatePaidColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.DatePaidColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStartTextAlign + (MDVUtility.ToAmount(dr[dsReports.DT_Reports_PaymentEnteries.AmountColumn]) >= 0 ? String.Format("${0:#,###,##0.00}", (dr[dsReports.DT_Reports_PaymentEnteries.AmountColumn])) : String.Format("(${0:#,###,##0.00})", Math.Abs(MDVUtility.ToDecimal(dr[dsReports.DT_Reports_PaymentEnteries.AmountColumn])))) + ColumnEnd);

                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Reports_PaymentEnteries.EntryDateColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Reports_PaymentEnteries.EntryDateColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.EntryDateColumn] + ColumnEnd);//
                        }
                        sb.Append(ColumnStart + dr[dsReports.DT_Reports_PaymentEnteries.EnteredByColumn] + ColumnEnd);

                        sb.Append(RowEnd);

                        InsChrg += MDVUtility.ToDouble(dr[dsReports.DT_Reports_PaymentEnteries.AmountColumn]);
                    }
                    InsChrg = MDVUtility.ToAmount(InsChrg);
                    sb.Append(RowStart);
                    sb.Append(FootercolumnBoldStart + "Total:" + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStartEnd + "" + FootercolumnBoldEnd);
                    sb.Append(FooterColumnStartTextAlign + (InsChrg >= 0 ? String.Format("${0:#,###,##0.00}", InsChrg) : String.Format("(${0:#,###,##0.00})", Math.Abs(InsChrg))) + FootercolumnBoldEnd);
                    sb.Append(FootercolumnBoldStart1 + "" + FootercolumnBoldEnd);
                    sb.Append(RowEnd);

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadPaymentEntriesReport", ex);
                return ex.Message;
            }


        }
        #endregion
        #region iTrack Reports

        public String LoadMIPSImprovementActivity(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_MIPS_Improvement_Activity.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th rowspan='2' class='va-middle noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";
                    String TableHeading = TableHead + RowStart +
                        thColumnStart + "Account Number" + thColumnEnd +
                        thColumnStart + "Patient Name" + thColumnEnd +
                        thColumnStart + "Performance Period" + thColumnEnd +
                        thColumnStart + "Provider" + thColumnEnd +
                        thColumnStart + "Depression Screeening" + thColumnEnd +
                        thColumnStart + "Tobacco Use" + thColumnEnd +
                        thColumnStart + "Implementation of Fall Screening & Assessment Programs" + thColumnEnd +
                        thColumnStart + "Use of CEHRT to Capture Patient Reported Outcomes" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_MIPS_Improvement_Activity.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_MIPS_Improvement_Activity.AccountNumberColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_MIPS_Improvement_Activity.PatientNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(ReportsParamaters["@QuarterlyReport"]) == "false" ? dr[dsReports.DT_MIPS_Improvement_Activity.PerformanceYearColumn] : ReportsParamaters["@QuarterName"] + " Quarter, " + ReportsParamaters["@Year"]) + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_MIPS_Improvement_Activity.ProviderColumn] + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_MIPS_Improvement_Activity.DepressionScreeningColumn]) == "N/A" ? "N/A" : "<b>PERFORMED</b> " + dr[dsReports.DT_MIPS_Improvement_Activity.DepressionScreeningColumn]) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_MIPS_Improvement_Activity.TobbacoUseColumn]) == "N/A" ? "N/A" : "<b>PERFORMED</b> " + dr[dsReports.DT_MIPS_Improvement_Activity.TobbacoUseColumn]) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_MIPS_Improvement_Activity.ImplementationOfFallScreeningAndAssesmentProgramColumn]) == "N/A" ? "N/A" : "<b>PERFORMED</b> " + dr[dsReports.DT_MIPS_Improvement_Activity.ImplementationOfFallScreeningAndAssesmentProgramColumn]) + ColumnEnd);
                        sb.Append(ColumnStart + (MDVUtility.ToStr(dr[dsReports.DT_MIPS_Improvement_Activity.UseOfCEHRTtoCapturePatientreportedOutcomesColumn]) == "N/A" ? "N/A" : "<b>PERFORMED</b> " + dr[dsReports.DT_MIPS_Improvement_Activity.UseOfCEHRTtoCapturePatientreportedOutcomesColumn]) + ColumnEnd);
                        sb.Append(RowEnd);
                    }

                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadMIPSImprovementActivity", ex);
                return ex.Message;
            }
        }
        #endregion
        #region MU Automated Measure report data.
        /// <summary>
        /// Author :  Khaleel Ur Rehman.
        /// Date : 21 May 2016
        /// Purpose : Fetch MU report data.
        /// </summary>
        /// <param name="ReferralId"></param>
        /// <param name="patientId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        public BLObject<DSReports> LoadMUReportData(long providerId, long PatientId, int reportType, string fromDate, string toDate, string MUID, string rptName)
        {
            try
            {
                DSReports ds = new DSReports();
                ds = new DALReports().LoadMUReportData(providerId, PatientId, reportType, fromDate, toDate, MUID, rptName);
                return new BLObject<DSReports>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadMUReportData", ex);
                return new BLObject<DSReports>(null, ex.Message);
            }
        }
        #endregion


        #region Clinical Reports
        /// <summary>
        /// Author :  Khaleel Ur Rehman.
        /// Date : 21 May 2016
        /// Purpose : Fetch MU report data.
        /// </summary>
        /// <param name="ReferralId"></param>
        /// <param name="patientId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>

        public BLObject<List<CPhoneEncounterModel>> LoadPhoneEncounterReport(string CreateDateFrom, string CreateDateTo, string NoteStatus, string DurationFrom, string DurationTo, string ProviderId, string RefProviderId, string PracticeId)
        {
            try
            {

                var result = new DALReports().LoadPhoneEncounterReport(CreateDateFrom, CreateDateTo, NoteStatus, DurationFrom, DurationTo, ProviderId, RefProviderId, PracticeId);
                return new BLObject<List<CPhoneEncounterModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadAppointmentsVisits", ex);
                return new BLObject<List<CPhoneEncounterModel>>(null, ex.Message);
            }
        }

        public BLObject<List<CProgressNoteReportModel>> LoadProgressNotesReport(string CreateDateFrom, string CreateDateTo, string NoteStatus, long NoteType, string ProviderId, string FacilityId, string RefProviderId, string PracticeId, bool IsAmendedNote)
        {
            try
            {

                var result = new DALReports().LoadProgressNotesReport(CreateDateFrom, CreateDateTo, NoteStatus, NoteType, ProviderId, FacilityId, RefProviderId, PracticeId, IsAmendedNote);
                return new BLObject<List<CProgressNoteReportModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadAppointmentsVisits", ex);
                return new BLObject<List<CProgressNoteReportModel>>(null, ex.Message);
            }
        }

        public BLObject<List<CAllergiesFillModel>> LoadAllergiesReport(CAllergiesModel model)
        {
            try
            {

                var result = new DALReports().LoadAllergiesReport(model);
                return new BLObject<List<CAllergiesFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadAllergiesReport", ex);
                return new BLObject<List<CAllergiesFillModel>>(null, ex.Message);
            }
        }

        public BLObject<List<CMedicationFillModel>> LoadMedicationReport(CMedicationModel model)
        {
            try
            {

                var result = new DALReports().LoadMedicationReport(model);
                return new BLObject<List<CMedicationFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadMedicationReport", ex);
                return new BLObject<List<CMedicationFillModel>>(null, ex.Message);
            }
        }

        public BLObject<List<CVitalsFillModel>> LoadVitalsReport(CVitalsModel model)
        {
            try
            {

                var result = new DALReports().LoadVitalsReport(model);
                return new BLObject<List<CVitalsFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadVitalsReport", ex);
                return new BLObject<List<CVitalsFillModel>>(null, ex.Message);
            }
        }

        public BLObject<List<COrdersFillModel>> LoadOrdersReport(COrdersModel model)
        {
            try
            {

                var result = new DALReports().LoadOrdersReport(model);
                return new BLObject<List<COrdersFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadOrdersReport", ex);
                return new BLObject<List<COrdersFillModel>>(null, ex.Message);
            }
        }

        public BLObject<List<COrdersFillModel>> LoadConsultationOrdersReport(COrdersModel model)
        {
            try
            {

                var result = new DALReports().LoadConsultationOrdersReport(model);
                return new BLObject<List<COrdersFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadConsultationOrdersReport", ex);
                return new BLObject<List<COrdersFillModel>>(null, ex.Message);
            }
        }

        public String LoadClaimsNeverSubmitedReport(Dictionary<string, object> ReportsParamaters, String ReportName)
        {
            try
            {
                DSReports dsReports = new DSReports();
                dsReports = new DALReports().GetReportDetail(ReportsParamaters, ReportName);
                if (dsReports.Tables[dsReports.DT_Claims_Never_Submitted.TableName].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string PrintDivStart = "<div class='table-responsive Of-a' id='reportPrintTable'>";
                    string PrintDivEnd = "</div>";
                    //  String PrintButton = "<button class='btn btn-primary btn-sm mb-md pull-right' id='printMe' onclick='this.hide();window.print()'><i class='fa fa-print'></i> Print Report</button>";
                    String TableStart = "<table  class='table table-bordered table-striped table-condensed table-hover'>";
                    String TableHead = "<thead class='printHeading'>";
                    String thColumnStart = "<th class='noWordBreak'>";
                    String thColumnEnd = "</th>";
                    String TableHeadEnd = "</thead>";
                    String TableEnd = "</table>";
                    String RowStart = "<tr>";
                    String RowEnd = "</tr>";
                    String ColumnStart = "<td>";
                    String ColumnEnd = "</td>";

                    String TableHeading = TableHead + RowStart + "<tr style='background: none;' class='tr-adjust'><th class='noWordBreak th-adjust' colspan='3' style='border-top: #fff solid 1px;border-left: #fff solid 1px;'></th>" + RowEnd +
                        RowStart
                        + thColumnStart + "Provider" + thColumnEnd +
                          thColumnStart + "Facility" + thColumnEnd +
                          thColumnStart + "Resource Provider" + thColumnEnd +
                          thColumnStart + "Insurance Plan" + thColumnEnd +
                          thColumnStart + "Claim Number" + thColumnEnd +
                          thColumnStart + "DOS" + thColumnEnd +
                          thColumnStart + "Claim Entry Date" + thColumnEnd +
                          thColumnStart + "Claim Status" + thColumnEnd +
                          thColumnStart + "Created By" + thColumnEnd + RowEnd + TableHeadEnd;

                    sb.Append(PrintDivStart);
                    //   sb.Append(PrintButton);
                    sb.Append(TableStart);
                    sb.Append(TableHeading);

                    foreach (DataRow dr in dsReports.Tables[dsReports.DT_Claims_Never_Submitted.TableName].Rows)
                    {
                        sb.Append(RowStart);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claims_Never_Submitted.ProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claims_Never_Submitted.FacilityNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claims_Never_Submitted.ResouceProviderNameColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claims_Never_Submitted.InsuranceColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claims_Never_Submitted.ClaimNumberColumn] + ColumnEnd);
                        if (!string.IsNullOrEmpty(dr[dsReports.DT_Claims_Never_Submitted.DOSFromColumn].ToString()))
                        {
                            sb.Append(ColumnStart + MDVUtility.GetDateMMDDYYY(dr[dsReports.DT_Claims_Never_Submitted.DOSFromColumn].ToString()) + ColumnEnd);//
                        }
                        else
                        {
                            sb.Append(ColumnStart + dr[dsReports.DT_Claims_Never_Submitted.DOSFromColumn] + ColumnEnd);//
                        }
                        //sb.Append(ColumnStart + dr[dsReports.DT_Progress_Note.CreatedOnColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claims_Never_Submitted.ClaimEntryDateColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claims_Never_Submitted.ClaimStatusColumn] + ColumnEnd);
                        sb.Append(ColumnStart + dr[dsReports.DT_Claims_Never_Submitted.CreatedByColumn] + ColumnEnd);
                        sb.Append(RowEnd);
                    }
                    sb.Append(TableEnd);
                    sb.Append(PrintDivEnd);
                    return sb.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReports::LoadClaimsNeverSubmitedReport", ex);
                return ex.Message;
            }


        }

        #region Problem

        public BLObject<List<CProblemsFillModel>> LoadProblemsReport(CProblemsModel model)
        {
            try
            {

                var result = new DALReports().LoadProblemsReport(model);
                return new BLObject<List<CProblemsFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadProblemsReport", ex);
                return new BLObject<List<CProblemsFillModel>>(null, ex.Message);
            }
        }

        #endregion


        public BLObject<List<CProceduresFillModelcs>> LoadProceduresReport(CProceduresModelcs model)
        {
            try
            {

                var result = new DALReports().LoadProceduresReport(model);
                return new BLObject<List<CProceduresFillModelcs>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadProceduresReport", ex);
                return new BLObject<List<CProceduresFillModelcs>>(null, ex.Message);
            }
        }

        public BLObject<List<CImmunizationFillModel>> LoadImmunizationReport(CImmunizationModel model)
        {
            try
            {

                var result = new DALReports().LoadImmunizationReport(model);
                return new BLObject<List<CImmunizationFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadAllergiesReport", ex);
                return new BLObject<List<CImmunizationFillModel>>(null, ex.Message);
            }
        }

        public BLObject<List<CResultsFillModel>> LoadResultsReport(CResultsModel model)
        {
            try
            {
                var result = new DALReports().LoadResultsReport(model);
                return new BLObject<List<CResultsFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadResultsReport", ex);
                return new BLObject<List<CResultsFillModel>>(null, ex.Message);
            }
        }
        public BLObject<List<COrdersFillModel>> LoadProcedureOrdersReport(COrdersModel model)
        {
            try
            {
                var result = new DALReports().LoadProcedureOrdersReport(model);
                return new BLObject<List<COrdersFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadProcedureOrdersReport", ex);
                return new BLObject<List<COrdersFillModel>>(null, ex.Message);
            }
        }

        public BLObject<List<CPrescriptionOrderFillModel>> LoadPrescriptionOrdersReport(CPrescriptionOrderModel model)
        {
            try
            {
                var result = new DALReports().LoadPrescriptionOrdersReport(model);
                return new BLObject<List<CPrescriptionOrderFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::LoadPrescriptionOrdersReport", ex);
                return new BLObject<List<CPrescriptionOrderFillModel>>(null, ex.Message);
            }
        }

        public BLObject<DSMedicationLookup> getPharmacyLookup()
        {
            try
            {
                DSMedicationLookup ds = new DSMedicationLookup();
                ds = new DALReports().getPharmacyLookup();

                return new BLObject<DSMedicationLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::getPharmacyLookup", ex);
                return new BLObject<DSMedicationLookup>(null, ex.Message);
            }
        }

        public BLObject<DSRadiologyOrderLookup> LookupAntimicrobials()
        {
            try
            {
                DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
                ds = new DALReports().LookupAntimicrobials();
                return new BLObject<DSRadiologyOrderLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::LookupAntimicrobials", ex);
                return new BLObject<DSRadiologyOrderLookup>(null, ex.Message);
            }

        }

        #endregion



        #region CCM Reports
        public BLObject<List<CCM_ReportFillModel>> Load_CCM_Report(CCM_ReportSearchModel model)
        {
            try
            {
                List<CCM_ReportFillModel> result = new DALReports().Load_CCM_Report(model);
                return new BLObject<List<CCM_ReportFillModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReport::Load_CCM_Report", ex);
                return new BLObject<List<CCM_ReportFillModel>>(null, ex.Message);
            }
        }
        #endregion

        #region Report Checks
        private string IncorrectBalanceParamsCheck(Dictionary<string, object> ReportsParamaters)
        {
            string RunReport;
            foreach (var item in ReportsParamaters)
            {
                if (item.Key == "@RunReportByName")
                {
                    RunReport = item.Value.ToString();
                    return RunReport;
                }
                //   dbManager.AddParameters(counter, item.Key, string.IsNullOrEmpty(item.Value.ToString()) ? DBNull.Value : item.Value);
                // counter++;
                //return "Rendering Provider";
            }
            return null;
        }
        #endregion
        #region Financial Analytics
        /// <summary>
        /// Load Monthly Kpi Detail for specfic provider
        /// </summary>
        /// <param name="ProviderId"></param>
        /// <param name="ProviderName"></param>
        /// <param name="ClaimDateFrom"></param>
        /// <param name="ClaimDateTo"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        public BLObject<List<ProviderMonthlyPayment>> LoadMonthlyPaymentTrendDetail(string ProviderId, string ProviderName, string ClaimDateFrom, string ClaimDateTo, string EntityId)
        {

            try
            {
                List<ProviderMonthlyPayment> MonthlyPaymentTrend = new List<ProviderMonthlyPayment>();
                MonthlyPaymentTrend = new DALReports().LoadMonthlyPaymentTrendDetail( ProviderId,  ProviderName,  ClaimDateFrom,  ClaimDateTo,  EntityId);

                return new BLObject<List<ProviderMonthlyPayment>>(MonthlyPaymentTrend);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadMonthlyPaymentTrendDetail", ex);
                return new BLObject<List<ProviderMonthlyPayment>>(null, ex.Message);
            }

        }
        #endregion
    }
}
