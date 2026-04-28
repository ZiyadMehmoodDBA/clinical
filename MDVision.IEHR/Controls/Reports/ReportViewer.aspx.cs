using MDVision.Business.BCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MDVision.IEHR.Controls.Reports
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        //private List<Stream> m_streams;

        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    if (!Page.IsPostBack)
        //    {

        //        // Set the processing mode for the ReportViewer to Remote
        //        RptViewer.ProcessingMode = ProcessingMode.Remote;

        //        ServerReport serverReport = RptViewer.ServerReport;

        //        // Set the report server URL and report path
        //        serverReport.ReportServerUrl =
        //            new Uri("http://mdvision-18:80/ReportServer1");
        //        serverReport.ReportPath =
        //            "/Report Project1/test";

        //        // Create the sales order number report parameter
        //        RptViewer.ServerReport.Refresh();
        //    }
        //}
        public string queryString = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Uri theRealURL = new Uri(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.RawUrl);

                string path = HttpUtility.ParseQueryString(theRealURL.Query).Get("reportpath");
                if (Session["Reports_QueryString"] != null)
                {
                    theRealURL = new Uri(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.RawUrl) ;
                    queryString  = Session["Reports_QueryString"].ToString();
                    Session.Remove("Reports_QueryString");
                }
                var IsClaimForm = Request.QueryString["IsClaimForm"];
                if (IsClaimForm != null && IsClaimForm == "True")
                    Load_ClaimForm();
                else
                    ShowReport(path, theRealURL);


            }

        }
        private void ShowReport(string path, Uri theRealURL)
        {
            try
            {
                string urlReportServer = MDVSession.Current.ReportURL;
                RptViewer.ProcessingMode = ProcessingMode.Remote; // ProcessingMode will be Either Remote or Local
                RptViewer.ServerReport.ReportServerUrl = new Uri(urlReportServer); //Set the ReportServer Url
                RptViewer.ServerReport.ReportPath = "/Clients/" + MDVApplication.InitialCatalog + "/" + path; //Passing the Report Path                

                //Creating an ArrayList for combine the Parameters which will be passed into SSRS Report
                ArrayList reportParam = new ArrayList();
                reportParam = ReportDefaultPatam(theRealURL);

                ArrayList reportParam1 = new ArrayList();
                reportParam1 = ReportDefaultParam(queryString);

                var arrQueryString = queryString.Split('&');
                ReportParameter[] param = new ReportParameter[(reportParam.Count + arrQueryString.Length)];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }
                for (int k = 0; k < reportParam1.Count; k++)
                {
                    param[reportParam.Count + k] = (ReportParameter)reportParam1[k];
                }

                //pass parmeters to report
                RptViewer.ServerReport.SetParameters(param); //Set Report Parameters
                RptViewer.ServerReport.Refresh();
                // if report is Financial Analysis at CPt Level than, we are generating pdf for print of the report
                //string ReportPath = path.Trim().ToLower();
                //if (ReportPath.IndexOf("financial") > -1 || ReportPath.IndexOf("revenuebyprovider") > -1 || ReportPath.IndexOf("revenuebyfacility") > -1)
                //{
                //    genertePDF(ReportPath);
                //}
                //else
                //{
                //    Session.Remove("CurrentReportView");
                //}
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                                        "script", "<script>parent.iframeLoadedContent()</script>");
                // throw ex;
            }
        }
        private ArrayList ReportDefaultPatam(Uri theRealURL)
        {
            ArrayList arrLstDefaultParam = new ArrayList();

            foreach (String key in HttpUtility.ParseQueryString(theRealURL.Query).AllKeys)
            {
                if (!key.Equals("reportpath"))
                {
                    string keyValue = string.IsNullOrEmpty(HttpUtility.ParseQueryString(theRealURL.Query).GetValues(key)[0]) ? string.Empty : HttpUtility.UrlDecode(HttpUtility.ParseQueryString(theRealURL.Query).GetValues(key)[0]);// HttpUtility.ParseQueryString(theRealURL.Query).Get(key).ToString();
                    arrLstDefaultParam.Add(CreateReportParameter(key, string.IsNullOrEmpty(keyValue) ? null : keyValue));
                }

            }
            arrLstDefaultParam.Add(CreateReportParameter("EntityId", MDVSession.Current.EntityId.ToString()));


            return arrLstDefaultParam;
        }

        private ArrayList ReportDefaultParam(string theRealURL)
        {
            ArrayList arrLstDefaultParam = new ArrayList();

            foreach (String key in HttpUtility.ParseQueryString(theRealURL).AllKeys)
            {
                if (!key.Equals("reportpath"))
                {
                    string keyValue = string.IsNullOrEmpty(HttpUtility.ParseQueryString(theRealURL).GetValues(key)[0]) ? string.Empty : HttpUtility.UrlDecode(HttpUtility.ParseQueryString(theRealURL).GetValues(key)[0]);// HttpUtility.ParseQueryString(theRealURL.Query).Get(key).ToString();
                    arrLstDefaultParam.Add(CreateReportParameter(key, string.IsNullOrEmpty(keyValue) ? null : keyValue));
                }

            }

            return arrLstDefaultParam;
        }

        private ReportParameter CreateReportParameter(string paramName, string pramValue)
        {
            ReportParameter aParam = new ReportParameter(paramName, pramValue);
            return aParam;
        }

        /*Author : Muhammad Azhar Shahzad
         Date: Dec 30, 2015,
         Purpose:  if report is Financial Analysis at CPt Level than, we are generating pdf for print of the report*/
        public void genertePDF(string ReportPath)
        {
            string width = "11.69in";
            string height = "8.27in";
            if (ReportPath.IndexOf("financial") > -1 )
            {
                width = "32.69in"; height = "8.87in";
            }
            else if(ReportPath.IndexOf("revenuebyprovider") > -1 || ReportPath.IndexOf("revenuebyfacility") > -1)
            {
                
            }
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension = "pdf";
            //The DeviceInfo settings should be changed based on the reportType 
            string deviceInfo = @"<DeviceInfo>             
						 <OutputFormat>PDF</OutputFormat>              
						 <PageWidth>"+width+"</PageWidth> "+             
						" <PageHeight>"+height+"</PageHeight> "+           
						" <MarginTop>0.00in</MarginTop>  "+          
						" <MarginLeft>0.00in</MarginLeft> "+
                        " <MarginRight>0.00in</MarginRight> " +        
						" <MarginBottom>0.00in</MarginBottom></DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = RptViewer.ServerReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out  streams, out  warnings);
            Session["CurrentReportView"] = Convert.ToBase64String(renderedBytes);


        }

        #region " Health Insurance Claim Form "

        protected void Load_ClaimForm()
        {
            try
            {
                InitializeForm(RptViewer);
            }
            catch (Exception)
            {

            }
        }


        private void InitializeForm(Microsoft.Reporting.WebForms.ReportViewer report_viewer)
        {
            //string path = "file:///" + Server.MapPath("~/Content/images/fill_form.png");


            //report_viewer.ProcessingMode = ProcessingMode.Local;
            //report_viewer.AsyncRendering = true;
            //report_viewer.SizeToReportContent = false;
            //report_viewer.Width = Unit.Pixel(850);
            //report_viewer.Height = Unit.Pixel(1100);
            ////report_viewer.Style.Add("overflow","hidden");

            //report_viewer.ShowPrintButton = true;
            //report_viewer.LocalReport.EnableExternalImages = true;
            //report_viewer.LocalReport.ReportPath = Server.MapPath("~/Controls/Reports/Hcfa/ClaimForm.rdlc");

            //List<ReportParameter> param = new List<ReportParameter>();
            //ReportParameter aParam = new ReportParameter("bgImage", path);
            //param.Add(aParam);

            //string Visits = Request.QueryString["Visits"];
            //bool IsSubmit = Convert.ToBoolean(Request.QueryString["isSubmit"]);
            //string ClearningHouseId = Request.QueryString["ClearningHouseId"];

            //List<long> Visits_list = new List<long>();
            //string[] temp = Visits.Split(',');
            //foreach (var item in temp)
            //{
            //    if (!string.IsNullOrEmpty(item))
            //        Visits_list.Add(MDVUtility.ToLong(item));
            //}

            //BLObject<DSHCFA> obj = BusinessWrapper.BillingClaim.BusinessObj.PrintClaim(Visits_list, Convert.ToInt64(ClearningHouseId), IsSubmit);
            //if (obj.Data != null)
            //{
            //    DSHCFA ds = obj.Data;
            //    ReportDataSource claims = new ReportDataSource("HCFAClaims", ds.Tables[ds.HCFAClaims.TableName]);


            //    report_viewer.LocalReport.DataSources.Clear();
            //    report_viewer.LocalReport.DataSources.Add(claims);

            //    DataTable dt = ds.Tables[ds.HCFACharges.TableName];
            //    if (dt.Rows.Count < 6)
            //    {
            //        for (int i = dt.Rows.Count; i < 6; i++)
            //        {
            //            DataRow row = dt.NewRow();
            //            dt.Rows.Add(row);
            //        }
            //    }

            //    ReportDataSource charges = new ReportDataSource("HCFACharges", dt);

            //    report_viewer.LocalReport.DataSources.Add(charges);

            //    report_viewer.LocalReport.SetParameters(param);
            //    report_viewer.LocalReport.Refresh();
            //}
        }



        #endregion

    }
}