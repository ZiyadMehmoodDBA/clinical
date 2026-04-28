using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Clinical;
using System;
using MDVision.DataAccess.DAL.Schedule;
using MDVision.DataAccess.DAL.Appointment;
using MDVision.DataAccess.DAL.Patient;
using System.IO;
using iTextSharp.text;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using HtmlAgilityPack;
using iTextSharp.text.pdf;
using MDVision.DataAccess.DCommon;
using iTextSharp.text.pdf.draw;
using System.Data;
using System.Web;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf.fonts;
using MDVision.DataAccess.DAL.Admin;
using MDVision.DataAccess.DAL.Clinical.Batch;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MDVision.Model;
using MDVision.Model.FaceSheet;
using MDVision.Model.Lookups;
using MDVision.DataAccess.DAL.ReportHeader;
using iTextSharp.text.html;
using MDVision.Model.Clinical.Reports;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Business.BCommon;
using MDVision.Model.Clinical.History;
using System.Net.Sockets;
using System.Linq;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.Clinical.ROS;
using MDVision.Model.Clinical.Notes;
using System.Reflection;
using MDVision.IEHR.EMR.Model.FavoriteList;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Requisitions
{


    /// <summary>
    // PLEASE DO NOT MAKE ANY CHANGES TO THIS FILE WITHOUT AHSAN NASIR'S/MUHAMMAD KHAWERS ACKNOWLEDGEMENT
    /// </summary>
    public class LabOrderResultRequisitions
    {
        public LabOrderResultRequisitions()
        {

        }
        private static LabOrderResultRequisitions _instance = null;
        public static LabOrderResultRequisitions Instance()
        {
            if (_instance == null)
                _instance = new LabOrderResultRequisitions();
            return _instance;
        }
        string OrderingProviderName = string.Empty;
        string OrderingProviderNPI = string.Empty;
        string OrderingProviderAddress = string.Empty;
        byte[] ProviderSignatures = null;
        public BLObject<byte[]> GenerateLabOrderReq(long labId, long patientId, string BarCodeHtml, string ImagePath)
        {
            try
            {
                byte[] bytes = null;
                DSLabOrder dsLabOrder = new DALLabOrder().LoadLabOrder(labId, patientId, 0, "", "", "", "", 0, "", "", "", 0, "", "1");
                DSLab dsLab = new DALLab().GetLab(MDVUtility.ToLong(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.LabIdColumn.ColumnName]), MDVUtility.ToLong(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]), MDVUtility.ToLong(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.FacilityIdColumn.ColumnName]));
                if (dsLab.Lab.Rows.Count > 0)
                {

                    if (dsLab.Lab.Rows[0][dsLab.Lab.RequisitionTemplateNameColumn.ColumnName].ToString() == "BioReference")
                    {
                        bytes = this.BioRefOrderTemplate(labId, patientId, BarCodeHtml, ImagePath, dsLabOrder, dsLab);
                    }
                    else if (dsLab.Lab.Rows[0][dsLab.Lab.RequisitionTemplateNameColumn.ColumnName].ToString() == "VitalAxis")
                    {
                        bytes = this.VitalAxisOrderTemplate(labId, patientId, BarCodeHtml, ImagePath, dsLabOrder, dsLab);
                    }
                }
                return new BLObject<byte[]>(bytes);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::previewLabOrder", ex);
                return new BLObject<byte[]>(null, ex.Message);
            }
        }
        public BLObject<byte[]> GenerateLabResultReq(long labOrderResultId, long labOrderId, long patientId, string BarCodeHtmlText, string ImagePath)
        {
            try
            {
                byte[] bytes = null;
                DSLabOrder dsLabOrder = new DALLabOrder().LoadLabOrder(labOrderId, patientId, 0, "", "", "", "", 0, "", "", "", 0, "", "1");
                if (dsLabOrder.LabOrder.Rows.Count > 0)
                {
                    DSLab dsLab = new DALLab().GetLab(MDVUtility.ToLong(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.LabIdColumn.ColumnName]), MDVUtility.ToLong(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]), MDVUtility.ToLong(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.FacilityIdColumn.ColumnName]));
                    if (dsLab.Lab.Rows.Count > 0)
                    {

                        if (dsLab.Lab.Rows[0][dsLab.Lab.RequisitionTemplateNameColumn.ColumnName].ToString() == "BioReference")
                        {
                            bytes = this.BioRefResultTemplate(labOrderResultId, labOrderId, patientId, BarCodeHtmlText, ImagePath, dsLabOrder, dsLab);
                        }
                        else if (dsLab.Lab.Rows[0][dsLab.Lab.RequisitionTemplateNameColumn.ColumnName].ToString() == "VitalAxis")
                        {
                            bytes = this.VitalAxisResultTemplate(labOrderResultId, labOrderId, patientId, BarCodeHtmlText, ImagePath, dsLabOrder, dsLab);
                        }
                    }
                }
                return new BLObject<byte[]>(bytes);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::previewLabOrder", ex);
                return new BLObject<byte[]>(null, ex.Message);
            }
        }

        #region Lab based Templates
        #region BioReference Labs
        private byte[] BioRefOrderTemplate(long labId, long patientId, string BarCodeHtml, string ImagePath, DSLabOrder dsLabOrder, DSLab dsLab)
        {
            {
                try
                {
                    string patientName = "";
                    byte[] newByteArr = null;
                    var filePath = string.Empty;
                    var folderPath = string.Empty;
                    var pngfileName = string.Empty;


                    DSInsurance dsInsurance = new DSInsurance();
                    DSPatient dsPatient = new DSPatient();
                    DSProfile dsProfile = new DSProfile();
                    DSLabOrder dsLabOrderExternalBillingInfoPrimary = null;
                    DataRow drLabOrderExternalBillingInfoPrimary = null;

                    #region Fonts

                    var fontColour = new BaseColor(102, 178, 255);
                    Font patientNameFont = FontFactory.GetFont("Arial", 12, Font.BOLD, fontColour);
                    Font bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                    Font bodyFontSmall = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                    Font testDetailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                    Font componentHeadingFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, fontColour);
                    Font componentHeaderFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                    Font gridbodyFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);

                    #endregion

                    #region Fetch Routine

                    if (patientId == 0)
                        if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                            patientId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.PatientIdColumn.ColumnName]);

                    dsLabOrder.Merge(new DALPatient().FillPatient(patientId, "", ""));
                    dsLabOrder.Merge(new DALLabOrder().loadLabOrderProblems(0, labId, patientId));
                    dsLabOrder.Merge(new DALLabOrder().LoadLabOrderTest(labId, 0, patientId, "", ""));

                    long practiceId = MDVUtility.ToInt64(dsLabOrder.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                    long providerId = MDVUtility.ToInt64(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]);
                    string ProviderText = GetProviderText(providerId);
                    //if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                    //    providerId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]);

                    dsLabOrder.Merge(new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

                    #endregion

                    using (MemoryStream stream_ = new MemoryStream())
                    {
                        #region Header Segment

                        PdfPTable ReportHeaderTable = setLabHeaderPDF(practiceId, patientId, dsLab, dsLabOrder, bodyFont, BarCodeHtml, true);
                        float bottomMargin = 22;
                        float topMargin = 20;
                        //if (IsReportHeaderApplied)
                        //{
                        topMargin = ReportHeaderTable.CalculateHeights() + 33;
                        bottomMargin = 52;
                        //}
                        #endregion
                        #region MD Vision Footer

                        PdfPTable footer = setLabOrder_ResultFooterPDF();

                        #endregion
                        #region Document Object
                        Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                        MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Face Sheet");
                        pdf.Writer.PageEvent = new MDVision.Common.Utilities.MDVUtility.AddFooterHeader(ReportHeaderTable, true, null, footer);

                        #endregion

                        pdf.Document.Open();
                        BaseColor myColor1 = WebColors.GetRGBColor("#66B2FF");
                        LineSeparator line2 = new LineSeparator(1f, 100f, myColor1, Element.ALIGN_CENTER, -1);
                        pdfDocument.Add(line2);
                        #region Patient/Provider Segment
                        string AccountNumber = dsLabOrder.Tables[dsPatient.Patients.TableName].Rows[0]["AccountNumber"].ToString();
                        try
                        {
                            PdfPTable ReportPatientProvider = new PdfPTable(2);
                            ReportPatientProvider.TotalWidth = 575f;
                            ReportPatientProvider.SpacingBefore = 3f;
                            ReportPatientProvider.SpacingAfter = 8f;
                            ReportPatientProvider.LockedWidth = true;
                            ReportPatientProvider.DefaultCell.Border = Rectangle.NO_BORDER;

                            ReportPatientProvider.AddCell(SetLabPatient(patientId, patientName, fontColour, bodyFont, componentHeadingFont, patientNameFont, AccountNumber));
                            ReportPatientProvider.AddCell(SetLabProvider(providerId, patientNameFont, bodyFont, ProviderText));
                            pdfDocument.Add(ReportPatientProvider);
                        }
                        catch (Exception ex)
                        {
                            MDVLogger.BLLErrorLog("BLLAdminClinical::LabOrderPreview, Patient or Provider Segment", ex);
                        }

                        BaseColor myColor = WebColors.GetRGBColor("#66B2FF");
                        LineSeparator Borderline = new LineSeparator(1f, 100f, myColor, Element.ALIGN_CENTER, 4);
                        pdfDocument.Add(Borderline);

                        Paragraph requisitionHeading = new Paragraph("Lab Requisition".ToString(), patientNameFont);
                        LineSeparator requisitionHeadingLine = new LineSeparator(1f, 100f, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -1);
                        requisitionHeadingLine.Offset = 1;
                        pdfDocument.Add(requisitionHeading);


                        #endregion
                        #region Lab Requisition

                        // Order Information & Insured Relationship
                        labOrderInformation(pdfDocument, dsLabOrder, componentHeaderFont, componentHeadingFont, bodyFont);
                        // Billing Information
                        labOrderInformationANDInsuredInformation(pdfDocument, dsLabOrder, componentHeaderFont, componentHeadingFont, bodyFont, dsLabOrderExternalBillingInfoPrimary, drLabOrderExternalBillingInfoPrimary, dsLab);

                        // Guarantor
                        labOrderGuarantor(pdfDocument, componentHeadingFont, componentHeaderFont, bodyFont, dsLabOrder);

                        // Test Information
                        labOrderTestInformation(pdfDocument, componentHeadingFont, componentHeaderFont, bodyFont, testDetailFont, dsLabOrder);

                        // Diagnosis Codes
                        labOrderDiagnosisCodes(pdfDocument, componentHeadingFont, bodyFont, dsLabOrder);

                        #endregion
                        #region Comments

                        Paragraph comments_Heading = new Paragraph("Comments\n".ToString(), componentHeadingFont);
                        pdfDocument.Add(comments_Heading);
                        pdfDocument.Add(new Paragraph(dsLabOrder.Tables[dsLabOrder.LabOrderTest.TableName].Rows[0][dsLabOrder.LabOrderTest.FillerInstructionColumn.ColumnName].ToString(), bodyFont));

                        #endregion
                        #region e-Signed

                        pdfDocument.Add(Chunk.NEWLINE);
                        pdfDocument.Add(Chunk.NEWLINE);
                        Paragraph signedBy = new Paragraph("e-Signed By: " + MDVUtility.ToStr(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ModifiedByNameColumn.ColumnName]) + " on " + MDVUtility.ToDateTime(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ModifiedOnColumn.ColumnName]).ToLongDateString() + " at " + MDVUtility.ToDateTime(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ModifiedOnColumn.ColumnName]).ToLongTimeString(), bodyFont);
                        signedBy.Alignment = Element.ALIGN_LEFT;
                        pdfDocument.Add(signedBy);

                        #endregion
                        #region But in the end It doesn't even matter

                        pdf.Document.Close();
                        pdf.Writer.Close();
                        pdfDocument.Close();

                        MemoryStream stream = new MemoryStream(stream_.ToArray());
                        PdfReader npdf = new PdfReader(stream);
                        MemoryStream outstream = new MemoryStream();

                        var color = System.Drawing.ColorTranslator.FromHtml("#fff");
                        var fontcolor = new BaseColor(color);
                        Font componentFooterFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, fontcolor);
                        using (PdfStamper stamper = new PdfStamper(npdf, outstream, '\0', true))
                        {
                            stamper.Writer.CloseStream = false;
                            int PageCount = npdf.NumberOfPages;
                            for (int i = 1; i <= PageCount; i++)
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, new Paragraph(String.Format("Page {0} of {1}", i, PageCount), componentFooterFont), 555, 18, 0);
                        }
                        newByteArr = outstream.GetBuffer();

                        #endregion
                    }
                    return newByteArr;
                }
                catch (Exception ex)
                {
                    MDVLogger.BLLErrorLog("BLLClinical::previewLabOrder", ex);
                    throw ex;

                }
            }
        }
        private byte[] BioRefResultTemplate(long labOrderResultId, long labOrderId, long patientId, string BarCodeHtmlText, string ImagePath, DSLabOrder dsLabOrder, DSLab dsLab)
        {
            try
            {
                DSLabResult dsLabResult = new DSLabResult();
                DSPatient dsPatient = new DSPatient();
                DSProfile dsProfile = new DSProfile();

                string FirstName = "";
                string LastName = "";
                string patientName = "";
                string patientComments = "";
                long specimenId = MDVUtility.ToInt64(null);
                var folderPath = string.Empty;
                var pngfileName = string.Empty;
                //long practiceId = MDVUtility.ToInt64(null);
                //long providerId = MDVUtility.ToInt64(null);

                #region Fonts

                var fontColour = new BaseColor(102, 178, 255);
                Font patientNameFont = FontFactory.GetFont("Arial", 12, Font.BOLD, fontColour);
                Font bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                Font bodyFontSmall = FontFactory.GetFont("Courier", 9, Font.NORMAL, BaseColor.BLACK);
                Font bodyFontSmall1 = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                Font uomFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.RED);
                Font uomFontOrange = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.ORANGE);
                Font componentHeadingFont = FontFactory.GetFont("Arial", 11, Font.BOLD, fontColour);
                Font componentHeaderFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                Font componentGridHeaderFont = FontFactory.GetFont("Arial", 10, Font.BOLD, new BaseColor(102, 178, 255));
                Font gridbodyFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
                Font bodyFontParent = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
                Font gridbodyFontTest = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLUE);
                Font PFont = FontFactory.GetFont("Arial", 12, Font.BOLD, fontColour);

                #endregion

                #region Fetch Routine

                dsLabOrder.Merge(new DALLabOrder().LoadLabOrderTest(labOrderId, 0, patientId, "", ""));
                dsLabResult = new DALLabResult().LoadLabResult(labOrderResultId, labOrderId, "", "", "", "", 0, "", "", "", "", 0, patientId, "", "1");

                if (patientId == 0)
                    if (dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count > 0)
                        patientId = MDVUtility.ToInt64(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.PatientIdColumn.ColumnName]);

                dsLabResult.Merge(new DALPatient().FillPatient(patientId, "", ""));
                long practiceId = MDVUtility.ToInt64(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                long providerId = MDVUtility.ToInt64(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ProviderIdColumn.ColumnName]);
                dsLabResult.Merge(new DALLabResult().loadLabResultDetail(0, MDVUtility.ToInt64(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName])));
                dsLabResult.Merge(new DALLabOrder().LoadLabOrder(labOrderId, patientId, 0, "", "", "", "", 0, "", "", "", 0));
                dsLabResult.Merge(new DALLabOrder().LoadLabOrderTest(labOrderId, 0, patientId, "", ""));
                string ProviderText = GetProviderText(providerId);
                int testCount = 0;
                if (dsLabResult.Tables[dsLabOrder.LabOrderTest.TableName].Rows.Count > 0)
                {
                    foreach (DSLabOrder.LabOrderTestRow r in dsLabResult.Tables[dsLabOrder.LabOrderTest.TableName].Rows)
                    {
                        if (testCount == 0)
                        {
                            dsLabResult.Merge(new DALLabResult().loadLabResultSpecimen(specimenId, MDVUtility.ToInt64(r[dsLabOrder.LabOrderTest.LabOrderTestIdColumn.ColumnName])));
                            testCount++;
                        }
                    }
                }

                if (dsLabResult.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                {
                    //providerId = MDVUtility.ToInt64(dsLabResult.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]);
                    //practiceId = MDVUtility.ToInt64(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                    FirstName = MDVUtility.ToStr(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.FirstNameColumn.ColumnName]);
                    LastName = MDVUtility.ToStr(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.LastNameColumn.ColumnName]);
                    patientComments = MDVUtility.ToStr(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.CommentsColumn.ColumnName]);
                }
                dsLabResult.Merge(new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

                #endregion

                #region PDF Segments


                #region Report Header
                DSReportHeader.ReportHeaderTagsDataTable dtReportHeaderTags;

                bool IsReportHeaderApplied = false;
                var dsReportHeader = new MDVision.DataAccess.DAL.ReportHeader.DALReportHeader().getReportHeaderTagsValue(patientId, 0, -1, "Results");
                IsReportHeaderApplied = dsReportHeader.ReportHeaderTags.Count > 0;
                dtReportHeaderTags = dsReportHeader.ReportHeaderTags;
                if (IsReportHeaderApplied &&
                     (
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["PatientText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["ProviderText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["PracticeText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["HeaderLogo"].ToString())
                            )
                    )
                {
                    IsReportHeaderApplied = true;
                }
                else
                {
                    IsReportHeaderApplied = false;
                }
                // return new BLObject<DSReportHeader>(dsReportHeader);

                byte[] newByteArr = null;
                using (MemoryStream stream_ = new MemoryStream())
                {
                    // Heading Font Style
                    //var fontColour = new BaseColor(102, 178, 255);
                    //iTextSharp.text.Font patientNameFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, fontColour);
                    //iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    //iTextSharp.text.Font componentHeadingFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, fontColour);
                    //iTextSharp.text.Font componentHeaderFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                    bool IsFooterExist = false;
                    string FooterGeneratedBy = "";
                    PdfPTable ReportHeaderTable = new PdfPTable(2);
                    PdfPTable footer = new PdfPTable(1);
                    float bottomMargin = 22;
                    float topMargin = 20;
                    PdfPTable patientTable = new PdfPTable(2);
                    Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                    //if (IsReportHeaderApplied)
                    //{
                    //    DSReportHeader.ReportHeaderTagsRow drReportHeader = (DSReportHeader.ReportHeaderTagsRow)dtReportHeaderTags.Rows[0];
                    //    #region Report Header

                    //    try
                    //    {
                    //        //------------------------------------  DSReportHeader.ReportHeaderTags dtReportHeaderTags =
                    //        if (IsReportHeaderApplied)
                    //        {
                    //            ReportHeaderTable.TotalWidth = 630f;
                    //            ReportHeaderTable.LockedWidth = true;
                    //            ReportHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;


                    //            IsFooterExist = drReportHeader.Field<string>("FooterText") != null;
                    //            if (IsFooterExist)
                    //            {
                    //                FooterGeneratedBy = drReportHeader.FooterText;
                    //                footer = setLabOrder_ResultFooterPDF(FooterGeneratedBy);
                    //            }
                    //            #region Header Logo
                    //            if (drReportHeader.Field<string>("HeaderLogo") != null && drReportHeader.Field<string>("HeaderLogo") != "")
                    //            {
                    //                PdfPTable headerTable = new PdfPTable(1);
                    //                headerTable.TotalWidth = 575f;
                    //                headerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                headerTable.DefaultCell.PaddingLeft = 50;

                    //               // headerTable.LockedWidth = true;

                    //                headerTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    //                Byte[] buffer = Convert.FromBase64String(drReportHeader.HeaderLogo.Split(new string[] { "base64," }, StringSplitOptions.None)[1]);

                    //                var memoryStream = new MemoryStream(buffer); //new MemoryStream(buffer, offset, count);
                    //                System.Drawing.Image newImagej = System.Drawing.Image.FromStream(memoryStream);

                    //                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(buffer, false);
                    //                logo.ScalePercent(59f);
                    //                logo.ScaleAbsoluteHeight(100);
                    //                logo.ScaleAbsoluteWidth(150);

                    //                PdfPCell cell1 = new PdfPCell();
                    //                cell1.AddElement(logo);
                    //                cell1.Border = Rectangle.NO_BORDER;
                    //                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                cell1.PaddingLeft = 50;
                    //                ReportHeaderTable.AddCell(cell1);
                    //            }

                    //            else
                    //            {
                    //                PdfPTable EmptyHeaderTable = new PdfPTable(1);
                    //                EmptyHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    //                EmptyHeaderTable.AddCell(new Paragraph("", bodyFont));
                    //                ReportHeaderTable.AddCell(EmptyHeaderTable);
                    //            }

                    //            #endregion

                    //            #region practice

                    //            PdfPTable PracticeTable = new PdfPTable(1);
                    //            PracticeTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    //            //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                    //            PracticeTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //            PracticeTable.DefaultCell.PaddingRight = 50;
                    //            //END Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                    //            if (drReportHeader.Field<string>("PracticeText") != null)
                    //            {
                    //                int practiceCounter = 0;
                    //                string[] PracticeColumns = drReportHeader.PracticeText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                    //                foreach (string PracticeColumn in PracticeColumns)
                    //                {
                    //                    if (!string.IsNullOrEmpty(PracticeColumn) && !string.IsNullOrWhiteSpace(PracticeColumn))
                    //                    {
                    //                        if (practiceCounter < 7)
                    //                        {
                    //                            PracticeTable.AddCell(new Paragraph(PracticeColumn, bodyFont));
                    //                            practiceCounter++;
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                PracticeTable.AddCell(new Paragraph("", bodyFont));
                    //            }
                    //            ReportHeaderTable.AddCell(PracticeTable);
                    //            #endregion

                    //            #region  Patient

                    //            PdfPTable PatientTable = new PdfPTable(1);
                    //            PatientTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    //            PatientTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //            PatientTable.DefaultCell.PaddingLeft = 50;

                    //            if (drReportHeader.Field<string>("PatientText") != null)
                    //            {
                    //                int patientCounter = 0;
                    //                string[] PatientColumns = drReportHeader.PatientText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                    //                foreach (string PatientColumn in PatientColumns)
                    //                {
                    //                    if (!string.IsNullOrEmpty(PatientColumn) && !string.IsNullOrWhiteSpace(PatientColumn))
                    //                    {
                    //                        if (patientCounter < 7)
                    //                        {
                    //                            PatientTable.AddCell(new Paragraph(PatientColumn, bodyFont));
                    //                            patientCounter++;
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                PatientTable.AddCell(new Paragraph("", bodyFont));
                    //            }
                    //            //PatientTable.DefaultCell.PaddingLeft = 50;
                    //            PatientTable.DefaultCell.UseAscender = true;

                    //            //PatientTable.DefaultCell.PaddingRight = 50;

                    //            ReportHeaderTable.AddCell(PatientTable);
                    //            #endregion

                    //            #region  Provider

                    //            PdfPTable ProviderTable = new PdfPTable(1);
                    //            ProviderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    //            //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                    //            ProviderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //            ProviderTable.DefaultCell.PaddingRight = 50;
                    //            //End Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                    //            if (drReportHeader.Field<string>("ProviderText") != null)
                    //            {
                    //                int providerCounter = 0;
                    //                string[] ProviderColumns = drReportHeader.ProviderText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                    //                foreach (string ProviderColumn in ProviderColumns)
                    //                {
                    //                    if (!string.IsNullOrEmpty(ProviderColumn) && !string.IsNullOrWhiteSpace(ProviderColumn))
                    //                    {
                    //                        if (providerCounter < 7)
                    //                        {
                    //                            ProviderTable.AddCell(new Paragraph(ProviderColumn, bodyFont));
                    //                            providerCounter++;
                    //                        }
                    //                    }
                    //                }

                    //            }
                    //            else
                    //            {
                    //                ProviderTable.AddCell(new Paragraph("", bodyFont));
                    //            }
                    //            ReportHeaderTable.AddCell(ProviderTable);
                    //            #endregion
                    //            //ReportHeaderTable.DefaultCell.Padding = -1;
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MDVLogger.BLLErrorLog("BLLAdminClinical::getReportHeaderTagsValue", ex);
                    //        // return new BLObject<DSReportHeader>(null, ex.Message);
                    //    }


                    //    #endregion
                    //}
                    //else
                    //{
                    //    #region Patient's Data

                    //    #region Header Segment
                    //    ReportHeaderTable = setLabHeaderPDF(practiceId, patientId, dsLab, dsLabOrder, bodyFont, "", true);

                    //    //if (IsReportHeaderApplied)
                    //    //{
                    //    topMargin = ReportHeaderTable.CalculateHeights() + 33;
                    //    bottomMargin = 52;
                    //    //}
                    //    #endregion
                    //    #region MD Vision Footer

                    //    footer = setLabOrder_ResultFooterPDF(FooterGeneratedBy);
                    //    // ReportHeaderTable.AddCell(footer);

                    //    #endregion




                    //    #endregion
                    //}

                    //if (IsReportHeaderApplied)
                    //{
                    //    topMargin = ReportHeaderTable.CalculateHeights() + 33;
                    //    bottomMargin = 52;
                    //}
                    PdfPTable emptyTable = new PdfPTable(1);
                    pdfDocument.SetMargins(20, 20, 20, 52);
                    footer = setLabOrder_ResultFooterPDF(FooterGeneratedBy);
                    //pdfDocument.SetMargins(20, 20, topMargin, bottomMargin);
                    // pdfDocument.SetMargins(20, 20, 20, bottomMargin);
                    //Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                    MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Lab Result");
                    pdf.Writer.PageEvent = new MDVision.Common.Utilities.MDVUtility.AddFooterHeader(null, true, null, footer);


                    pdf.Document.Open();
                    #region Patient's Data

                    #region Header Segment
                    ReportHeaderTable = setLabHeaderPDF(practiceId, patientId, dsLab, dsLabOrder, bodyFont, BarCodeHtmlText, false);

                    //if (IsReportHeaderApplied)
                    //{
                    topMargin = ReportHeaderTable.CalculateHeights() + 33;
                    bottomMargin = 52;
                    //}
                    #endregion
                    #region MD Vision Footer


                    // ReportHeaderTable.AddCell(footer);

                    #endregion
                    #endregion
                    pdfDocument.Add(ReportHeaderTable);

                    DSPatient dsPatientHeader = new DALPatient().FillPatient(patientId, "", "");
                    var x2 = new MyPdfPageEventHelpPageNo(MDVUtility.ToStr(dsPatientHeader.Tables[dsPatientHeader.Patients.TableName].Rows[0][dsPatientHeader.Patients.LastNameColumn.ColumnName].ToString() + ", " + dsPatientHeader.Tables[dsPatientHeader.Patients.TableName].Rows[0][dsPatientHeader.Patients.FirstNameColumn.ColumnName].ToString()), bodyFont, MDVUtility.ToStr(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.OrderNoColumn.ColumnName]), componentHeadingFont);
                    pdf.Writer.PageEvent = x2;
                    BaseColor myColor1 = WebColors.GetRGBColor("#66B2FF");
                    LineSeparator line2 = new LineSeparator(1f, 100f, myColor1, iTextSharp.text.Element.ALIGN_CENTER, 0);
                    pdfDocument.Add(line2);


                    #region Patient/Provider Segment
                    string AccountNumber = dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0]["AccountNumber"].ToString();
                    try
                    {
                        PdfPTable ReportPatientProvider = new PdfPTable(2);
                        ReportPatientProvider.TotalWidth = 575f;
                        ReportPatientProvider.SpacingBefore = 3f;
                        ReportPatientProvider.SpacingAfter = 8f;
                        ReportPatientProvider.LockedWidth = true;
                        ReportPatientProvider.DefaultCell.Border = Rectangle.NO_BORDER;

                        ReportPatientProvider.AddCell(SetLabPatient(patientId, patientName, fontColour, bodyFont, componentHeadingFont, patientNameFont, AccountNumber));
                        ReportPatientProvider.AddCell(SetLabProvider(providerId, patientNameFont, bodyFont, ProviderText));
                        pdfDocument.Add(ReportPatientProvider);

                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLAdminClinical::LabOrderPreview, Patient or Provider Segment", ex);
                    }
                    pdfDocument.Add(line2);


                    #endregion

                    #endregion

                    #region Lab, OrderNumber, Facility and Assigne Information Segment

                    setLabResultOrderInformation(pdfDocument, dsLabResult, dsLabOrder, componentHeaderFont, componentHeadingFont, bodyFont);

                    #endregion

                    #region Sample Segment

                    Paragraph SampleHeading = new Paragraph("Sample", componentHeadingFont);
                    pdfDocument.Add(SampleHeading);

                    if (dsLabResult.Tables[dsLabResult.LabResultSpecimen.TableName].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsLabResult.Tables[dsLabResult.LabResultSpecimen.TableName].Rows[0][dsLabResult.LabResultSpecimen.ExternalSpecimenIdColumn.ColumnName].ToString()))
                        {
                            pdfDocument.Add(setLabResultSample(dsLabOrder, dsLabResult, componentHeaderFont, bodyFont));
                        }
                        else
                        {
                            Paragraph noProblems = new Paragraph("No Sample Found".ToString(), bodyFont);
                            noProblems.Alignment = Element.ALIGN_CENTER;
                            pdfDocument.Add(noProblems);
                        }
                    }

                    #endregion

                    #region Result Information Segment

                    setLabresultInformation(pdfDocument, dsLabOrder, dsLabResult, componentHeadingFont, componentHeaderFont, componentGridHeaderFont, bodyFont, uomFont, uomFontOrange, gridbodyFont, bodyFontSmall);

                    #endregion

                    #region CLIA

                    setCLIA(pdfDocument, dsLab, componentHeaderFont);

                    #endregion

                    #endregion

                    #region Remarks

                    Paragraph remarks_Heading = new Paragraph("Remarks \n".ToString(), componentHeadingFont);
                    remarks_Heading.SpacingBefore = remarks_Heading.SpacingAfter = 5;
                    pdfDocument.Add(remarks_Heading);
                    Paragraph remarks = new Paragraph(MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.RemarksColumn.ColumnName]), bodyFont);
                    remarks.Alignment = Element.ALIGN_LEFT;
                    pdfDocument.Add(remarks);

                    #endregion

                    #region Comments

                    Paragraph comments_Heading = new Paragraph("Comments \n".ToString(), componentHeadingFont);
                    comments_Heading.SpacingBefore = comments_Heading.SpacingAfter = 5;
                    pdfDocument.Add(comments_Heading);

                    //Paragraph comments = new Paragraph(MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.CommentsColumn.ColumnName]), bodyFont);
                    //comments.Alignment = Element.ALIGN_LEFT;
                    //pdfDocument.Add(comments);




                    using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDocument))
                    {

                        //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                        using (var sr = new StringReader(MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.CommentsColumn.ColumnName])))
                        {

                            //Parse the HTML
                            htmlWorker.Parse(sr);
                        }
                    }



                    pdfDocument.Add(Chunk.NEWLINE);
                    pdfDocument.Add(Chunk.NEWLINE);

                    #endregion

                    #region e-Signed

                    string modifiedDate = MDVUtility.ToDateTime(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ModifiedOnColumn.ColumnName]).ToLongDateString();

                    if (!string.IsNullOrEmpty(modifiedDate))
                    {
                        modifiedDate = modifiedDate + " at ";
                    }
                    string modifiedTime = MDVUtility.ToDateTime(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ModifiedOnColumn.ColumnName]).ToLongTimeString();

                    Paragraph signedByPara = new Paragraph("e-Signed By: " + MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ModifiedByNameColumn.ColumnName])
                       + " on " + modifiedDate + modifiedTime, bodyFont);
                    signedByPara.Alignment = Element.ALIGN_LEFT;
                    //if (dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.IsAknowledgedColumn.ColumnName] != System.DBNull.Value)
                    //{
                    //    if (Convert.ToBoolean(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.IsAknowledgedColumn.ColumnName]))
                    //    {
                    // pdfDocument.Add(signedByPara);
                    //    }
                    //}


                    #endregion

                    #region Reviewed By
                    if (MDVUtility.ToBool(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.MarkAsReviewedColumn.ColumnName]))
                    {
                        Paragraph reviewedByPara = new Paragraph("Result reviewed by " + MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ReviewedByColumn.ColumnName])
                           + " on " + modifiedDate + modifiedTime, bodyFont);
                        reviewedByPara.Alignment = Element.ALIGN_LEFT;
                        pdfDocument.Add(reviewedByPara);
                    }

                    #endregion

                    #region Footer

                    #endregion

                    newByteArr = stream_.GetBuffer();
                    pdf.Document.Close();
                    pdf.Writer.Close();
                    pdfDocument.Close();

                    MemoryStream stream = new MemoryStream(stream_.ToArray());
                    PdfReader npdf = new PdfReader(stream);
                    MemoryStream outstream = new MemoryStream();
                    var color = System.Drawing.ColorTranslator.FromHtml("#fff");
                    var fontcolor = new BaseColor(color);
                    Font componentFooterFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, fontcolor);
                    using (PdfStamper stamper = new PdfStamper(npdf, outstream, '\0', true))
                    {
                        stamper.Writer.CloseStream = false;
                        int PageCount = npdf.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, new Paragraph(String.Format("Page {0} of {1}", i, PageCount), componentFooterFont), 555, 18, 0);
                    }
                    newByteArr = outstream.GetBuffer();
                }
                return newByteArr;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::previewLabResult", ex);
                throw ex;
            }
        }
        #endregion

        #region VitalAxis Labs
        private byte[] VitalAxisOrderTemplate(long labId, long patientId, string BarCodeHtml, string ImagePath, DSLabOrder dsLabOrder, DSLab dsLab)
        {
            {
                try
                {
                    string patientName = "";
                    byte[] newByteArr = null;
                    var filePath = string.Empty;
                    var folderPath = string.Empty;
                    var pngfileName = string.Empty;

                    DSInsurance dsInsurance = new DSInsurance();
                    DSPatient dsPatient = new DSPatient();
                    DSProfile dsProfile = new DSProfile();
                    DSLabOrder dsLabOrderExternalBillingInfoPrimary = null;
                    DataRow drLabOrderExternalBillingInfoPrimary = null;

                    #region Fonts

                    var fontColour = new BaseColor(102, 178, 255);
                    Font patientNameFont = FontFactory.GetFont("Arial", 12, Font.BOLD, fontColour);
                    Font bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                    Font bodyFontSmall = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                    Font testDetailFont = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK);
                    Font componentHeadingFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, fontColour);
                    Font componentHeaderFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                    Font gridbodyFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);

                    #endregion

                    #region Fetch Routine


                    if (patientId == 0)
                        if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                            patientId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.PatientIdColumn.ColumnName]);

                    dsLabOrder.Merge(new DALPatient().FillPatient(patientId, "", ""));
                    dsLabOrder.Merge(new DALLabOrder().loadLabOrderProblems(0, labId, patientId));
                    dsLabOrder.Merge(new DALLabOrder().LoadLabOrderTest(labId, 0, patientId, "", ""));

                    string[] FacilityData = GetFacilityText(MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.FacilityIdColumn.ColumnName])).Split(new string[] { "<br/>" }, StringSplitOptions.None);
                    dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.FacilityColumn.ColumnName] = FacilityData[1].Split(':')[1];
                    long practiceId = MDVUtility.ToInt64(dsLabOrder.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                    long providerId = MDVUtility.ToInt64(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]);
                    string ProviderText = GetProviderText(providerId);
                    //if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                    //    providerId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]);

                    //    dsLabOrder.Merge(new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

                    #endregion

                    using (MemoryStream stream_ = new MemoryStream())
                    {
                        #region Header Segment

                        PdfPTable ReportHeaderTable = setLabHeaderPDFVitalAxis(practiceId, patientId, dsLab, dsLabOrder, bodyFont, BarCodeHtml, true);
                        float bottomMargin = 22;
                        float topMargin = 20;
                        //if (IsReportHeaderApplied)
                        //{
                        topMargin = ReportHeaderTable.CalculateHeights() + 33;
                        bottomMargin = 52;
                        //}
                        #endregion
                        #region MD Vision Footer

                        PdfPTable footer = setLabOrder_ResultFooterPDF();

                        #endregion
                        #region Document Object
                        Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                        MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Face Sheet");
                        pdf.Writer.PageEvent = new MDVision.Common.Utilities.MDVUtility.AddFooterHeader(ReportHeaderTable, true, null, footer);

                        #endregion

                        pdf.Document.Open();
                        BaseColor myColor1 = WebColors.GetRGBColor("#66B2FF");
                        LineSeparator line2 = new LineSeparator(1f, 100f, myColor1, Element.ALIGN_CENTER, -1);
                        pdfDocument.Add(line2);
                        #region Patient/Provider Segment
                        string AccountNumber = dsLabOrder.Tables[dsPatient.Patients.TableName].Rows[0]["AccountNumber"].ToString();
                        try
                        {
                            PdfPTable ReportPatientProvider = new PdfPTable(2);
                            ReportPatientProvider.TotalWidth = 575f;
                            ReportPatientProvider.SpacingBefore = 3f;
                            ReportPatientProvider.SpacingAfter = 8f;
                            ReportPatientProvider.LockedWidth = true;
                            ReportPatientProvider.DefaultCell.Border = Rectangle.NO_BORDER;

                            ReportPatientProvider.AddCell(SetLabPatient(patientId, patientName, fontColour, bodyFont, componentHeadingFont, patientNameFont, AccountNumber));
                            PdfPTable ProviderTable = SetLabProvider(providerId, patientNameFont, bodyFont, ProviderText);

                            PdfPCell Providercell = new PdfPCell();
                            string FacilityAddress = string.Empty;
                            foreach (var item in FacilityData)
                            {
                                FacilityAddress += item.Split(':')[0].ToString() == "Description" ? item.Split(':')[1].ToString() + "\n" : string.Empty;
                                FacilityAddress += item.Split(':')[0].ToString() == "Address" ? item.Split(':')[1].ToString() : string.Empty;
                            }
                            Paragraph LocationAddress = new Paragraph(FacilityAddress, bodyFont);
                            LocationAddress.Alignment = Element.ALIGN_RIGHT;
                            Providercell.AddElement(LocationAddress);
                            Providercell.Border = Rectangle.NO_BORDER;
                            Providercell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            ProviderTable.AddCell(Providercell);
                            ReportPatientProvider.AddCell(ProviderTable);
                            pdfDocument.Add(ReportPatientProvider);
                        }
                        catch (Exception ex)
                        {
                            MDVLogger.BLLErrorLog("BLLAdminClinical::LabOrderPreview, Patient or Provider Segment", ex);
                        }

                        BaseColor myColor = WebColors.GetRGBColor("#66B2FF");
                        LineSeparator Borderline = new LineSeparator(1f, 100f, myColor, Element.ALIGN_CENTER, 4);
                        pdfDocument.Add(Borderline);

                        Paragraph requisitionHeading = new Paragraph("Lab Requisition".ToString(), patientNameFont);
                        LineSeparator requisitionHeadingLine = new LineSeparator(1f, 100f, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -1);
                        requisitionHeadingLine.Offset = 1;
                        pdfDocument.Add(requisitionHeading);


                        #endregion
                        #region Lab Requisition

                        // Order Information & Insured Relationship
                        labOrderInformation(pdfDocument, dsLabOrder, componentHeaderFont, componentHeadingFont, bodyFont);
                        // Billing Information
                        labOrderInformationANDInsuredInformation(pdfDocument, dsLabOrder, componentHeaderFont, componentHeadingFont, bodyFont, dsLabOrderExternalBillingInfoPrimary, drLabOrderExternalBillingInfoPrimary, dsLab);

                        // Guarantor
                        labOrderGuarantor(pdfDocument, componentHeadingFont, componentHeaderFont, bodyFont, dsLabOrder);

                        // Test Information
                        labOrderTestInformationVitalAxis(pdfDocument, componentHeadingFont, componentHeaderFont, bodyFont, testDetailFont, dsLabOrder);

                        // Diagnosis Codes
                        labOrderDiagnosisCodes(pdfDocument, componentHeadingFont, bodyFont, dsLabOrder);

                        #endregion
                        #region Comments

                        Paragraph comments_Heading = new Paragraph("Comments\n".ToString(), componentHeadingFont);
                        pdfDocument.Add(comments_Heading);
                        pdfDocument.Add(new Paragraph(dsLabOrder.Tables[dsLabOrder.LabOrderTest.TableName].Rows[0][dsLabOrder.LabOrderTest.FillerInstructionColumn.ColumnName].ToString(), bodyFont));

                        #endregion
                        #region e-Signed

                        pdfDocument.Add(Chunk.NEWLINE);
                        pdfDocument.Add(Chunk.NEWLINE);
                        string modifiedDate = MDVUtility.ToStr(MDVUtility.ToDateTime(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ModifiedOnColumn.ColumnName]).ToLongDateString());

                        if (!string.IsNullOrEmpty(modifiedDate))
                        {
                            modifiedDate = modifiedDate + " at ";
                        }
                        string modifiedTime = MDVUtility.ToStr(MDVUtility.ToDateTime(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ModifiedOnColumn.ColumnName]).ToLongTimeString());

                        Paragraph signedByPara = new Paragraph("e-Signed By: " + OrderingProviderName + ", " + OrderingProviderNPI
                           + " on " + modifiedDate + modifiedTime, bodyFont);
                        signedByPara.Alignment = Element.ALIGN_LEFT;
                        pdfDocument.Add(signedByPara);

                        #endregion
                        #region Medical Director
                        pdfDocument.Add(setVitalAxisOrderFooter());
                        #endregion
                        #region But in the end It doesn't even matter

                        pdf.Document.Close();
                        pdf.Writer.Close();
                        pdfDocument.Close();

                        MemoryStream stream = new MemoryStream(stream_.ToArray());
                        PdfReader npdf = new PdfReader(stream);
                        MemoryStream outstream = new MemoryStream();

                        var color = System.Drawing.ColorTranslator.FromHtml("#fff");
                        var fontcolor = new BaseColor(color);
                        Font componentFooterFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, fontcolor);
                        using (PdfStamper stamper = new PdfStamper(npdf, outstream, '\0', true))
                        {
                            stamper.Writer.CloseStream = false;
                            int PageCount = npdf.NumberOfPages;
                            for (int i = 1; i <= PageCount; i++)
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, new Paragraph(String.Format("Page {0} of {1}", i, PageCount), componentFooterFont), 555, 18, 0);
                        }
                        newByteArr = outstream.GetBuffer();

                        #endregion
                    }
                    return newByteArr;
                }
                catch (Exception ex)
                {
                    MDVLogger.BLLErrorLog("BLLClinical::previewLabOrder", ex);
                    throw ex;

                }
            }
        }
        private byte[] VitalAxisResultTemplate(long labOrderResultId, long labOrderId, long patientId, string BarCodeHtmlText, string ImagePath, DSLabOrder dsLabOrder, DSLab dsLab)
        {
            try
            {

                DSLabResult dsLabResult = new DSLabResult();
                DSPatient dsPatient = new DSPatient();
                DSProfile dsProfile = new DSProfile();

                string FirstName = "";
                string LastName = "";
                string patientName = "";
                string patientComments = "";
                long specimenId = MDVUtility.ToInt64(null);
                var folderPath = string.Empty;
                var pngfileName = string.Empty;
                //long practiceId = MDVUtility.ToInt64(null);
                //long providerId = MDVUtility.ToInt64(null);

                #region Fonts

                var fontColour = new BaseColor(102, 178, 255);
                Font patientNameFont = FontFactory.GetFont("Arial", 12, Font.BOLD, fontColour);
                Font bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                Font bodyFontSmall = FontFactory.GetFont("Courier", 9, Font.NORMAL, BaseColor.BLACK);
                Font bodyFontSmall1 = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                Font uomFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.RED);
                Font uomFontOrange = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.ORANGE);
                Font componentHeadingFont = FontFactory.GetFont("Arial", 11, Font.BOLD, fontColour);
                Font componentHeaderFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                Font componentGridHeaderFont = FontFactory.GetFont("Arial", 10, Font.BOLD, new BaseColor(102, 178, 255));
                Font gridbodyFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
                Font bodyFontParent = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
                Font gridbodyFontTest = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLUE);
                Font PFont = FontFactory.GetFont("Arial", 12, Font.BOLD, fontColour);

                #endregion



                #region Fetch Routine

                dsLabOrder.Merge(new DALLabOrder().LoadLabOrderTest(labOrderId, 0, patientId, "", ""));
                dsLabResult = new DALLabResult().LoadLabResult(labOrderResultId, labOrderId, "", "", "", "", 0, "", "", "", "", 0, patientId, "", "1");

                if (patientId == 0)
                    if (dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count > 0)
                        patientId = MDVUtility.ToInt64(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.PatientIdColumn.ColumnName]);

                dsLabResult.Merge(new DALPatient().FillPatient(patientId, "", ""));
                long practiceId = MDVUtility.ToInt64(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                long providerId = MDVUtility.ToInt64(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ProviderIdColumn.ColumnName]);
                dsLabResult.Merge(new DALLabResult().loadLabResultDetail(0, MDVUtility.ToInt64(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName])));
                dsLabResult.Merge(new DALLabOrder().LoadLabOrder(labOrderId, patientId, 0, "", "", "", "", 0, "", "", "", 0));
                dsLabResult.Merge(new DALLabOrder().LoadLabOrderTest(labOrderId, 0, patientId, "", ""));
                string ProviderText = GetProviderText(providerId);
                string[] FacilityData = GetFacilityText(MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.FacilityIdColumn.ColumnName])).Split(new string[] { "<br/>" }, StringSplitOptions.None);
                dsLabResult.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.FacilityColumn.ColumnName] = FacilityData[1].Split(':')[1];
                int testCount = 0;
                if (dsLabResult.Tables[dsLabOrder.LabOrderTest.TableName].Rows.Count > 0)
                {
                    foreach (DSLabOrder.LabOrderTestRow r in dsLabResult.Tables[dsLabOrder.LabOrderTest.TableName].Rows)
                    {
                        if (testCount == 0)
                        {

                            dsLabResult.Merge(new DALLabResult().loadLabResultSpecimen(specimenId, MDVUtility.ToInt64(r[dsLabOrder.LabOrderTest.LabOrderTestIdColumn.ColumnName])));
                            testCount++;

                        }
                    }
                }

                if (dsLabResult.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                {
                    //providerId = MDVUtility.ToInt64(dsLabResult.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]);
                    //practiceId = MDVUtility.ToInt64(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                    FirstName = MDVUtility.ToStr(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.FirstNameColumn.ColumnName]);
                    LastName = MDVUtility.ToStr(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.LastNameColumn.ColumnName]);
                    patientComments = MDVUtility.ToStr(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.CommentsColumn.ColumnName]);
                }
                dsLabResult.Merge(new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

                #endregion

                #region Document Object

                //byte[] newByteArr = null;
                //MemoryStream stream_ = new MemoryStream();
                //Document pdfDocument = new Document(PageSize.LETTER, 20, 20, 20, 35);
                //MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Requisition");
                //pdf.Document.Open();

                #endregion

                #region PDF Segments

                #region Header Segment

                // setLabHeader(pdfDocument, practiceId, patientId, dsLab, dsLabOrder, bodyFont, BarCodeHtmlText, false);

                #endregion

                //#region Patient/Provider Segment

                //try
                //{
                //    PdfPTable ReportPatientProvider = new PdfPTable(2);
                //    ReportPatientProvider.TotalWidth = 575f;
                //    ReportPatientProvider.SpacingBefore = 3f;
                //    ReportPatientProvider.SpacingAfter = 8f;
                //    ReportPatientProvider.LockedWidth = true;
                //    ReportPatientProvider.DefaultCell.Border = Rectangle.NO_BORDER;

                //    //  ReportPatientProvider.AddCell(SetLabPatient(pdf, patientId, patientName, fontColour, bodyFont, componentHeadingFont, PFont, dsLabOrder));
                //    ReportPatientProvider.AddCell(SetLabProvider(providerId, PFont, bodyFont));
                //    pdfDocument.Add(ReportPatientProvider);
                //}
                //catch (Exception ex)
                //{
                //    MDVLogger.BLLErrorLog("BLLAdminClinical::LabOrderPreview, Patient or Provider Segment", ex);
                //}

                //#region Line Seperator and Heading

                //BaseColor myColor = WebColors.GetRGBColor("#66B2FF");
                //LineSeparator Borderline = new LineSeparator(1f, 100f, myColor, Element.ALIGN_CENTER, 4);
                //pdfDocument.Add(Borderline);

                //Paragraph faceSheetHeading = new Paragraph("Lab Result".ToString(), patientNameFont);
                //pdfDocument.Add(faceSheetHeading);

                //#endregion

                //#endregion





                #region Report Header
                DSReportHeader.ReportHeaderTagsDataTable dtReportHeaderTags;

                bool IsReportHeaderApplied = false;
                var dsReportHeader = new MDVision.DataAccess.DAL.ReportHeader.DALReportHeader().getReportHeaderTagsValue(patientId, 0, -1, "Results");
                IsReportHeaderApplied = dsReportHeader.ReportHeaderTags.Count > 0;
                dtReportHeaderTags = dsReportHeader.ReportHeaderTags;
                IsReportHeaderApplied = false;
                if (IsReportHeaderApplied &&
                     (
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["PatientText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["ProviderText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["PracticeText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["HeaderLogo"].ToString())
                            )
                    )
                {
                    IsReportHeaderApplied = true;
                }
                else
                {
                    IsReportHeaderApplied = false;
                }
                // return new BLObject<DSReportHeader>(dsReportHeader);

                byte[] newByteArr = null;
                using (MemoryStream stream_ = new MemoryStream())
                {
                    bool IsFooterExist = false;
                    string FooterGeneratedBy = "";
                    PdfPTable ReportHeaderTable = new PdfPTable(2);
                    PdfPTable footer = new PdfPTable(1);
                    float bottomMargin = 22;
                    float topMargin = 20;
                    PdfPTable patientTable = new PdfPTable(2);
                    Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                    if (IsReportHeaderApplied)
                    {
                        DSReportHeader.ReportHeaderTagsRow drReportHeader = (DSReportHeader.ReportHeaderTagsRow)dtReportHeaderTags.Rows[0];
                        #region Report Header

                        try
                        {
                            //------------------------------------  DSReportHeader.ReportHeaderTags dtReportHeaderTags =
                            if (IsReportHeaderApplied)
                            {
                                ReportHeaderTable.TotalWidth = 630f;
                                ReportHeaderTable.LockedWidth = true;
                                ReportHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                                IsFooterExist = drReportHeader.Field<string>("FooterText") != null;
                                if (IsFooterExist)
                                {
                                    FooterGeneratedBy = drReportHeader.FooterText;
                                    footer = setLabOrder_ResultFooterPDF(FooterGeneratedBy);
                                }
                                #region Header Logo
                                if (drReportHeader.Field<string>("HeaderLogo") != null)
                                {
                                    PdfPTable headerTable = new PdfPTable(1);
                                    headerTable.TotalWidth = 575f;
                                    headerTable.LockedWidth = true;
                                    headerTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                    Byte[] buffer = Convert.FromBase64String(drReportHeader.HeaderLogo.Split(new string[] { "base64," }, StringSplitOptions.None)[1]);

                                    var memoryStream = new MemoryStream(buffer); //new MemoryStream(buffer, offset, count);
                                    System.Drawing.Image newImagej = System.Drawing.Image.FromStream(memoryStream);

                                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(buffer, false);
                                    logo.ScalePercent(59f);
                                    logo.ScaleAbsoluteHeight(100);
                                    logo.ScaleAbsoluteWidth(150);

                                    PdfPCell cell1 = new PdfPCell();
                                    cell1.AddElement(logo);
                                    cell1.Border = Rectangle.NO_BORDER;
                                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    ReportHeaderTable.AddCell(cell1);
                                }

                                else
                                {
                                    PdfPTable EmptyHeaderTable = new PdfPTable(1);
                                    EmptyHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                    EmptyHeaderTable.AddCell(new Paragraph("", bodyFont));
                                    ReportHeaderTable.AddCell(EmptyHeaderTable);
                                }

                                #endregion

                                #region practice

                                PdfPTable PracticeTable = new PdfPTable(1);
                                PracticeTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                PracticeTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                PracticeTable.DefaultCell.PaddingRight = 50;
                                //END Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                if (drReportHeader.Field<string>("PracticeText") != null)
                                {
                                    int practiceCounter = 0;
                                    string[] PracticeColumns = drReportHeader.PracticeText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                                    foreach (string PracticeColumn in PracticeColumns)
                                    {
                                        if (!string.IsNullOrEmpty(PracticeColumn) && !string.IsNullOrWhiteSpace(PracticeColumn))
                                        {
                                            if (practiceCounter < 7)
                                            {
                                                PracticeTable.AddCell(new Paragraph(PracticeColumn, bodyFont));
                                                practiceCounter++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    PracticeTable.AddCell(new Paragraph("", bodyFont));
                                }
                                ReportHeaderTable.AddCell(PracticeTable);
                                #endregion

                                #region  Patient

                                PdfPTable PatientTable = new PdfPTable(1);
                                PatientTable.DefaultCell.Border = Rectangle.NO_BORDER;

                                if (drReportHeader.Field<string>("PatientText") != null)
                                {
                                    int patientCounter = 0;
                                    string[] PatientColumns = drReportHeader.PatientText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                                    foreach (string PatientColumn in PatientColumns)
                                    {
                                        if (!string.IsNullOrEmpty(PatientColumn) && !string.IsNullOrWhiteSpace(PatientColumn))
                                        {
                                            if (patientCounter < 7)
                                            {
                                                PatientTable.AddCell(new Paragraph(PatientColumn, bodyFont));
                                                patientCounter++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    PatientTable.AddCell(new Paragraph("", bodyFont));
                                }
                                PatientTable.DefaultCell.Padding = 0f;
                                PatientTable.DefaultCell.UseAscender = true;
                                ReportHeaderTable.AddCell(PatientTable);
                                #endregion

                                #region  Provider

                                PdfPTable ProviderTable = new PdfPTable(1);
                                ProviderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                ProviderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                ProviderTable.DefaultCell.PaddingRight = 50;
                                //End Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                if (drReportHeader.Field<string>("ProviderText") != null)
                                {
                                    int providerCounter = 0;
                                    string[] ProviderColumns = drReportHeader.ProviderText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                                    foreach (string ProviderColumn in ProviderColumns)
                                    {
                                        if (!string.IsNullOrEmpty(ProviderColumn) && !string.IsNullOrWhiteSpace(ProviderColumn))
                                        {
                                            if (providerCounter < 7)
                                            {
                                                ProviderTable.AddCell(new Paragraph(ProviderColumn, bodyFont));
                                                providerCounter++;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    ProviderTable.AddCell(new Paragraph("", bodyFont));
                                }
                                ReportHeaderTable.AddCell(ProviderTable);
                                #endregion
                                //ReportHeaderTable.DefaultCell.Padding = -1;
                            }
                        }
                        catch (Exception ex)
                        {
                            MDVLogger.BLLErrorLog("BLLAdminClinical::getReportHeaderTagsValue", ex);
                            // return new BLObject<DSReportHeader>(null, ex.Message);
                        }


                        #endregion
                    }
                    else
                    {
                        #region Patient's Data

                        #region Header Segment
                        ReportHeaderTable = setLabHeaderPDFVitalAxis(practiceId, patientId, dsLab, dsLabOrder, bodyFont, "true", true);

                        //if (IsReportHeaderApplied)
                        //{
                        topMargin = ReportHeaderTable.CalculateHeights() + 33;
                        bottomMargin = 52;
                        //}
                        #endregion
                        #region MD Vision Footer

                        footer = setLabOrder_ResultFooterPDF(FooterGeneratedBy);
                        // ReportHeaderTable.AddCell(footer);

                        #endregion




                        #endregion
                    }

                    if (IsReportHeaderApplied)
                    {
                        topMargin = ReportHeaderTable.CalculateHeights() + 33;
                        bottomMargin = 52;
                    }
                    PdfPTable emptyTable = new PdfPTable(1);
                    //pdfDocument.SetMargins(20, 20, topMargin, bottomMargin);
                    pdfDocument.SetMargins(20, 20, 20, bottomMargin);
                    //Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                    MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Lab Result");
                    pdf.Writer.PageEvent = new MDVision.Common.Utilities.MDVUtility.AddFooterHeader(null, true, null, footer);


                    pdf.Document.Open();
                    pdfDocument.Add(ReportHeaderTable);
                    DSPatient dsPatientHeader = new DALPatient().FillPatient(patientId, "", "");
                    var x2 = new MyPdfPageEventHelpPageNo(MDVUtility.ToStr(dsPatientHeader.Tables[dsPatientHeader.Patients.TableName].Rows[0][dsPatientHeader.Patients.LastNameColumn.ColumnName].ToString() + ", " + dsPatientHeader.Tables[dsPatientHeader.Patients.TableName].Rows[0][dsPatientHeader.Patients.FirstNameColumn.ColumnName].ToString()), bodyFont, MDVUtility.ToStr(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.OrderNoColumn.ColumnName]), componentHeadingFont);
                    pdf.Writer.PageEvent = x2;
                    BaseColor myColor1 = WebColors.GetRGBColor("#66B2FF");
                    LineSeparator line2 = new LineSeparator(1f, 100f, myColor1, iTextSharp.text.Element.ALIGN_CENTER, 0);
                    pdfDocument.Add(line2);

                    if (!IsReportHeaderApplied)
                    {
                        #region Patient/Provider Segment
                        string AccountNumber = dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0]["AccountNumber"].ToString();
                        try
                        {
                            PdfPTable ReportPatientProvider = new PdfPTable(2);
                            ReportPatientProvider.TotalWidth = 575f;
                            ReportPatientProvider.SpacingBefore = 3f;
                            ReportPatientProvider.SpacingAfter = 8f;
                            ReportPatientProvider.LockedWidth = true;
                            ReportPatientProvider.DefaultCell.Border = Rectangle.NO_BORDER;

                            ReportPatientProvider.AddCell(SetLabPatient(patientId, patientName, fontColour, bodyFont, componentHeadingFont, patientNameFont, AccountNumber));
                            PdfPTable ProviderTable = SetLabProvider(providerId, patientNameFont, bodyFont, ProviderText);

                            PdfPCell Providercell = new PdfPCell();
                            string FacilityAddress = string.Empty;
                            foreach (var item in FacilityData)
                            {
                                FacilityAddress += item.Split(':')[0].ToString() == "Description" ? item.Split(':')[1].ToString() + "\n" : string.Empty;
                                FacilityAddress += item.Split(':')[0].ToString() == "Address" ? item.Split(':')[1].ToString() : string.Empty;
                            }

                            Paragraph LocationAddress = new Paragraph(FacilityAddress, bodyFont);
                            LocationAddress.Alignment = Element.ALIGN_RIGHT;
                            Providercell.AddElement(LocationAddress);
                            Providercell.Border = Rectangle.NO_BORDER;
                            Providercell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            ProviderTable.AddCell(Providercell);
                            ReportPatientProvider.AddCell(ProviderTable);
                            pdfDocument.Add(ReportPatientProvider);

                        }
                        catch (Exception ex)
                        {
                            MDVLogger.BLLErrorLog("BLLAdminClinical::LabOrderPreview, Patient or Provider Segment", ex);
                        }
                        pdfDocument.Add(line2);
                    }
                    #endregion
                    #endregion


                    #region Lab, OrderNumber, Facility and Assigne Information Segment

                    setLabResultOrderInformation(pdfDocument, dsLabResult, dsLabOrder, componentHeaderFont, componentHeadingFont, bodyFont);

                    #endregion

                    #region Sample Segment

                    Paragraph SampleHeading = new Paragraph("Sample", componentHeadingFont);
                    pdfDocument.Add(SampleHeading);

                    if (dsLabResult.Tables[dsLabResult.LabResultSpecimen.TableName].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsLabResult.Tables[dsLabResult.LabResultSpecimen.TableName].Rows[0][dsLabResult.LabResultSpecimen.ExternalSpecimenIdColumn.ColumnName].ToString()))
                        {
                            pdfDocument.Add(setLabResultSample(dsLabOrder, dsLabResult, componentHeaderFont, bodyFont));
                        }
                        else
                        {
                            Paragraph noProblems = new Paragraph("No Sample Found".ToString(), bodyFont);
                            noProblems.Alignment = Element.ALIGN_CENTER;
                            pdfDocument.Add(noProblems);
                        }
                    }

                    #endregion

                    #region Result Information Segment

                    setLabresultInformation(pdfDocument, dsLabOrder, dsLabResult, componentHeadingFont, componentHeaderFont, componentGridHeaderFont, bodyFont, uomFont, uomFontOrange, gridbodyFont, bodyFontSmall);

                    #endregion

                    #region CLIA

                    //setCLIA(pdfDocument, dsLab, componentHeaderFont);

                    #endregion

                    #endregion

                    #region Remarks

                    Paragraph remarks_Heading = new Paragraph("Remarks \n".ToString(), componentHeadingFont);
                    remarks_Heading.SpacingBefore = remarks_Heading.SpacingAfter = 5;
                    pdfDocument.Add(remarks_Heading);
                    Paragraph remarks = new Paragraph(MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.RemarksColumn.ColumnName]), bodyFont);
                    remarks.Alignment = Element.ALIGN_LEFT;
                    pdfDocument.Add(remarks);

                    #endregion

                    #region Comments

                    Paragraph comments_Heading = new Paragraph("Comments \n".ToString(), componentHeadingFont);
                    comments_Heading.SpacingBefore = comments_Heading.SpacingAfter = 5;
                    pdfDocument.Add(comments_Heading);

                    //Paragraph comments = new Paragraph(MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.CommentsColumn.ColumnName]), bodyFont);
                    //comments.Alignment = Element.ALIGN_LEFT;
                    //pdfDocument.Add(comments);

                    using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDocument))
                    {

                        //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                        using (var sr = new StringReader(MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.CommentsColumn.ColumnName])))
                        {

                            //Parse the HTML
                            htmlWorker.Parse(sr);
                        }
                    }




                    pdfDocument.Add(Chunk.NEWLINE);
                    pdfDocument.Add(Chunk.NEWLINE);

                    #endregion

                    #region e-Signed

                    string modifiedDate = MDVUtility.ToDateTime(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ModifiedOnColumn.ColumnName]).ToLongDateString();

                    if (!string.IsNullOrEmpty(modifiedDate))
                    {
                        modifiedDate = modifiedDate + " at ";
                    }
                    string modifiedTime = MDVUtility.ToDateTime(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ModifiedOnColumn.ColumnName]).ToLongTimeString();

                    Paragraph signedByPara = new Paragraph("e-Signed By: " + OrderingProviderName + ", " + OrderingProviderNPI
                       + " on " + modifiedDate + modifiedTime, bodyFont);
                    signedByPara.Alignment = Element.ALIGN_LEFT;
                    pdfDocument.Add(signedByPara);

                    #endregion

                    #region Footer
                    pdfDocument.Add(setVitalAxisFooter());
                    #endregion


                    #region But in the end It doesn't even matter

                    newByteArr = stream_.GetBuffer();
                    pdf.Document.Close();
                    pdf.Writer.Close();
                    pdfDocument.Close();

                    MemoryStream stream = new MemoryStream(stream_.ToArray());
                    PdfReader npdf = new PdfReader(stream);
                    MemoryStream outstream = new MemoryStream();
                    var color = System.Drawing.ColorTranslator.FromHtml("#fff");
                    var fontcolor = new BaseColor(color);
                    Font componentFooterFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, fontcolor);
                    using (PdfStamper stamper = new PdfStamper(npdf, outstream, '\0', true))
                    {
                        stamper.Writer.CloseStream = false;
                        int PageCount = npdf.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, new Paragraph(String.Format("Page {0} of {1}", i, PageCount), componentFooterFont), 555, 18, 0);
                    }

                    newByteArr = outstream.GetBuffer();
                }
                return newByteArr;

                #endregion
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::previewLabResult", ex);
                throw ex;
            }
        }
        #endregion

        #endregion
        private PdfPTable setVitalAxisFooter()
        {
            Font bodyFontSmall1 = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
            PdfPTable PreFooter = new PdfPTable(1);
            PreFooter.TotalWidth = 575f;
            PreFooter.SpacingBefore = 10f;
            PreFooter.LockedWidth = true;
            PreFooter.DefaultCell.Border = Rectangle.NO_BORDER;

            // Provider Signatures

            if (ProviderSignatures != null)
            {
                MemoryStream SigStream = new MemoryStream(ProviderSignatures);
                iTextSharp.text.Image OPSignatures = iTextSharp.text.Image.GetInstance(SigStream);

                OPSignatures.ScalePercent(75f);
                OPSignatures.ScaleAbsoluteHeight(100);
                OPSignatures.ScaleAbsoluteWidth(150);

                PdfPCell ProviderSigCell = new PdfPCell();
                ProviderSigCell.AddElement(OPSignatures);
                ProviderSigCell.Border = Rectangle.NO_BORDER;
                ProviderSigCell.HorizontalAlignment = Element.ALIGN_LEFT;
                PreFooter.AddCell(ProviderSigCell);
            }

            // Medical Directors Signatures

            MemoryStream ms = new MemoryStream();
            byte[] bytes;
            using (FileStream file = new FileStream(System.Web.HttpContext.Current.Server.MapPath(@"~\content\images\Dr. Cyril D'Cruz - Signatures.png"), FileMode.Open, System.IO.FileAccess.Read))
            {
                bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);
            }

            iTextSharp.text.Image Signatures = iTextSharp.text.Image.GetInstance(bytes, false);
            Signatures.ScalePercent(75f);
            Signatures.ScaleAbsoluteHeight(120);
            Signatures.ScaleAbsoluteWidth(165);



            PdfPCell MDCell = new PdfPCell();
            Paragraph MDDetails = new Paragraph("Dr. Cyril D'Cruz" + "\n" + "NPI: 1508822388", bodyFontSmall1);
            MDCell.AddElement(MDDetails);
            MDCell.Border = Rectangle.NO_BORDER;
            MDCell.HorizontalAlignment = Element.ALIGN_LEFT;
            PreFooter.AddCell(MDCell);

            PdfPCell logoCell = new PdfPCell();
            logoCell.AddElement(Signatures);
            logoCell.Border = Rectangle.NO_BORDER;
            logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
            PreFooter.AddCell(logoCell);

            PdfPCell CLIA = new PdfPCell();
            Paragraph CLIAText = new Paragraph("Testing performed by Sovereign Laboratory Services, LLC at 15-01 Broadway, Suite 31, Fair Lawn, New Jersey 07410 CLIA number 31D2022328, CLIS 0005087, Cyril D'Cruz, MD Medical Director", bodyFontSmall1);
            CLIA.AddElement(CLIAText);
            CLIA.Border = Rectangle.NO_BORDER;
            CLIA.HorizontalAlignment = Element.ALIGN_CENTER;
            PreFooter.AddCell(CLIA);
            return PreFooter;
        }
        private PdfPTable setVitalAxisOrderFooter()
        {
            Font bodyFontSmall1 = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
            PdfPTable PreFooter = new PdfPTable(1);
            PreFooter.TotalWidth = 575f;
            PreFooter.SpacingBefore = 10f;
            PreFooter.LockedWidth = true;
            PreFooter.DefaultCell.Border = Rectangle.NO_BORDER;

            // Provider Signatures

            if (ProviderSignatures != null)
            {
                MemoryStream SigStream = new MemoryStream(ProviderSignatures);
                iTextSharp.text.Image OPSignatures = iTextSharp.text.Image.GetInstance(SigStream);

                OPSignatures.ScalePercent(75f);
                OPSignatures.ScaleAbsoluteHeight(120);
                OPSignatures.ScaleAbsoluteWidth(165);

                PdfPCell ProviderSigCell = new PdfPCell();
                ProviderSigCell.AddElement(OPSignatures);
                ProviderSigCell.Border = Rectangle.NO_BORDER;
                ProviderSigCell.HorizontalAlignment = Element.ALIGN_LEFT;
                PreFooter.AddCell(ProviderSigCell);
            }

            return PreFooter;
        }
        #region Common Functions
        private PdfPTable setLabOrder_ResultFooter(Document pdfDocument)
        {
            PdfPTable footer = new PdfPTable(1);
            footer.TotalWidth = 575f;
            footer.LockedWidth = true;
            footer.HorizontalAlignment = Element.ALIGN_CENTER;
            footer.DefaultCell.Border = Rectangle.NO_BORDER;
            footer.SpacingBefore = 5f;

            PdfPCell footerCell = new PdfPCell();
            var color = System.Drawing.ColorTranslator.FromHtml("#005da9");
            footerCell.BackgroundColor = new BaseColor(color);
            color = System.Drawing.ColorTranslator.FromHtml("#fff");
            var fontcolor = new BaseColor(color);
            Font componentFooterFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, fontcolor);

            Paragraph footerPara = new Paragraph("Generated by: MDVision PMS EMR", componentFooterFont);
            footerPara.SpacingAfter = 5f;
            footerCell.AddElement(footerPara);
            footer.AddCell(footerCell);
            // pdfDocument.Add(footer);
            return footer;
        }
        private PdfPTable setLabOrder_ResultFooterPDF(string generatedBy = "")
        {
            PdfPTable footer = new PdfPTable(1);
            footer.TotalWidth = 575f;
            footer.LockedWidth = true;
            footer.HorizontalAlignment = Element.ALIGN_CENTER;
            footer.DefaultCell.Border = Rectangle.NO_BORDER;
            footer.SpacingBefore = 5f;

            PdfPCell footerCell = new PdfPCell();
            var color = System.Drawing.ColorTranslator.FromHtml("#005da9");
            footerCell.BackgroundColor = new BaseColor(color);
            color = System.Drawing.ColorTranslator.FromHtml("#fff");
            var fontcolor = new BaseColor(color);
            Font componentFooterFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, fontcolor);

            Paragraph footerPara = new Paragraph(generatedBy == "" ? "Generated by: MDVision PMS EMR" : "Generated by: " + generatedBy, componentFooterFont);
            footerPara.SpacingAfter = 5f;
            footerCell.AddElement(footerPara);
            footer.AddCell(footerCell);
            // pdfDocument.Add(footer);
            return footer;
        }
        private PdfPTable setLabHeaderPDF(long practiceId, long patientId, DSLab dsLab, DSLabOrder dsLabOrder, Font bodyFont, string BarCodeHtmlText, bool isLabOrder = true)
        {
            PdfPTable ReportHeaderTable = new PdfPTable(2);
            #region Practice

            //string practiceAddress = GetPracticeText(practiceId);
            //string[] practiceAddresses = practiceAddress.Split(new string[] { "<br/>" }, StringSplitOptions.None);

            //practiceAddress = "";
            //foreach (var column in practiceAddresses)
            //    if (column != string.Empty)
            //        practiceAddress += column + "\n";
            //if (practiceAddress.Length > 0)
            //{
            //    practiceAddress = practiceAddress.Remove(practiceAddress.Length - 1);

            //}


            var practiceAddress = "Sovereign Medical Group" + "\n" + OrderingProviderAddress;
            if (dsLab.Lab.Rows.Count > 0)
            {
                practiceAddress += "\n Account Number: " + dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName].ToString();
            }
            Paragraph p_practiceAddress = new Paragraph(practiceAddress, bodyFont);

            PdfPCell headerSovreignAddress = new PdfPCell();
            headerSovreignAddress.AddElement(p_practiceAddress);
            headerSovreignAddress.Border = Rectangle.NO_BORDER;
            headerSovreignAddress.HorizontalAlignment = Element.ALIGN_LEFT;

            #endregion

            #region Header

            if (true || BarCodeHtmlText != string.Empty)
            {
                MDVision.Business.BCommon.Barcode39 b = new MDVision.Business.BCommon.Barcode39();
                System.Drawing.Image img;
                b.ShowString = true;

                b.IncludeCheckSumDigit = false;
                b.TextFont = new System.Drawing.Font("Courier New", 9);

                #region BarCode/String/Logo

                string barCodeCondition1 = ""; // PatientAccount
                string barCodeCondition2 = ""; // Order Number
                string barCodeCondition3 = ""; // PatientLastName
                string barCodeCondition4 = ""; // PatientFirstNameInitial

                StringBuilder strBarcode = new StringBuilder();
                if (dsLabOrder != null && dsLabOrder.LabOrder.Rows.Count > 0)
                {
                    DSPatient dsPatient_BarCode = new DALPatient().FillPatient(patientId, "", "");
                    if (dsPatient_BarCode.Patients.Rows.Count > 0)
                    {
                        if (dsLab.Lab.Rows.Count > 0)
                        {
                            barCodeCondition1 = MDVUtility.ToStr(dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName]);
                        }
                        barCodeCondition2 = MDVUtility.ToStr(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.OrderNoColumn.ColumnName]);
                        barCodeCondition3 = MDVUtility.ToStr(dsPatient_BarCode.Patients.Rows[0][dsPatient_BarCode.Patients.LastNameColumn.ColumnName]);
                        barCodeCondition4 = MDVUtility.ToStr(dsPatient_BarCode.Patients.Rows[0][dsPatient_BarCode.Patients.FirstNameColumn.ColumnName]);
                    }

                    barCodeCondition3 = barCodeCondition3[0].ToString();
                    barCodeCondition4 = barCodeCondition4[0].ToString();
                    if (!string.IsNullOrEmpty(barCodeCondition1))
                    {
                        strBarcode.Append(barCodeCondition1);
                        strBarcode.Append(",");
                    }
                    strBarcode.Append(barCodeCondition2);
                    strBarcode.Append(",");
                    strBarcode.Append(barCodeCondition3);
                    strBarcode.Append(" ");
                    strBarcode.Append(barCodeCondition4);
                }

                img = b.GenerateBarcodeImage(240, 64, strBarcode.ToString());
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Png);
                pic.SpacingAfter = 10;

                // -------------------------------------------------------------------------------------------------------- //

                PdfPTable ReportHeaderBarCode = new PdfPTable(1);
                ReportHeaderBarCode.TotalWidth = 190f;
                ReportHeaderBarCode.LockedWidth = true;
                ReportHeaderBarCode.HorizontalAlignment = Element.ALIGN_RIGHT;
                ReportHeaderBarCode.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell lineCell = new PdfPCell();

                if (BarCodeHtmlText != null)
                {
                    if (BarCodeHtmlText != string.Empty && BarCodeHtmlText.ToLower() != "false")
                        lineCell.AddElement(pic);
                    else
                        lineCell.AddElement(new Chunk());
                }


                lineCell.Border = Rectangle.NO_BORDER;
                lineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                ReportHeaderBarCode.AddCell(lineCell);


                ReportHeaderTable.TotalWidth = 575f;
                ReportHeaderTable.SpacingBefore = 10f;
                ReportHeaderTable.LockedWidth = true;
                ReportHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                MemoryStream ms = new MemoryStream();
                byte[] bytes;
                var FileName = "";
                if (isLabOrder)
                {
                    FileName = "SHS-final-logo.png";
                }
                else
                {
                    if (dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.NameColumn.ColumnName].ToString() == "BioReference Laboratories")
                    {
                        FileName = "bio-reference-logo.png";
                    }
                    else
                    {
                        FileName = "SHS-final-logo.png";
                    }
                }
                using (FileStream file = new FileStream(System.Web.HttpContext.Current.Server.MapPath(@"~\content\images\" + FileName + ""), FileMode.Open, System.IO.FileAccess.Read))
                {
                    bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                }

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(bytes, false);
                logo.ScalePercent(75f);
                logo.ScaleAbsoluteHeight(120);
                logo.ScaleAbsoluteWidth(165);

                PdfPCell logoCell = new PdfPCell();
                logoCell.AddElement(logo);
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                ReportHeaderTable.AddCell(logoCell);

                #endregion

                #region Header Table

                ReportHeaderTable.AddCell(ReportHeaderBarCode);
                ReportHeaderTable.AddCell(headerSovreignAddress);

                string clientDetails = "";
                if (dsLab.Lab.Rows.Count > 0)
                {
                    clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.NameColumn.ColumnName] + "\n");
                    clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.AddressColumn.ColumnName], "\n");
                    clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.CityColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.CityColumn.ColumnName] + ", ");
                    clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.StateColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.StateColumn.ColumnName] + ", ");
                    clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName] + "\n");
                    clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.PhoneNoColumn.ColumnName]);
                }

                Paragraph p = new Paragraph(clientDetails, bodyFont);
                PdfPCell clientAccountNumber = new PdfPCell();
                p.Alignment = Element.ALIGN_RIGHT;
                clientAccountNumber.AddElement(p);
                clientAccountNumber.Border = Rectangle.NO_BORDER;
                clientAccountNumber.HorizontalAlignment = Element.ALIGN_RIGHT;

                ReportHeaderTable.AddCell(clientAccountNumber);
                ReportHeaderTable.SpacingAfter = 3f;
                #endregion
            }
            #endregion
            return ReportHeaderTable;
        }
        private PdfPTable setLabHeaderPDFVitalAxis(long practiceId, long patientId, DSLab dsLab, DSLabOrder dsLabOrder, Font bodyFont, string BarCodeHtmlText, bool isLabOrder = true)
        {
            PdfPTable ReportHeaderTable = new PdfPTable(2);
            var fontColour = new BaseColor(102, 178, 255);
            Font linkFont = FontFactory.GetFont("Arial", 10, Font.ITALIC, fontColour);

            #region Practice

            string practiceAddress = "Sovereign Laboratory Services" + "\n" + "15 -01 Broadway Suite 31" + "\n" + "Fair Lawn, NJ 07410";

            Paragraph p_practiceAddress = new Paragraph(practiceAddress, bodyFont);

            PdfPCell headerSovreignAddress = new PdfPCell();
            headerSovreignAddress.AddElement(p_practiceAddress);
            headerSovreignAddress.Border = Rectangle.NO_BORDER;
            headerSovreignAddress.HorizontalAlignment = Element.ALIGN_LEFT;

            #endregion

            #region Header

            if (true || BarCodeHtmlText != string.Empty)
            {
                MDVision.Business.BCommon.Barcode39 b = new MDVision.Business.BCommon.Barcode39();
                System.Drawing.Image img;
                b.ShowString = true;

                b.IncludeCheckSumDigit = false;
                b.TextFont = new System.Drawing.Font("Courier New", 9);

                #region BarCode/String/Logo

                string barCodeCondition1 = ""; // PatientAccount
                string barCodeCondition2 = ""; // Order Number
                string barCodeCondition3 = ""; // PatientLastName
                string barCodeCondition4 = ""; // PatientFirstNameInitial

                StringBuilder strBarcode = new StringBuilder();
                if (dsLabOrder != null && dsLabOrder.LabOrder.Rows.Count > 0)
                {
                    DSPatient dsPatient_BarCode = new DALPatient().FillPatient(patientId, "", "");
                    if (dsPatient_BarCode.Patients.Rows.Count > 0)
                    {
                        if (dsLab.Lab.Rows.Count > 0)
                        {
                            barCodeCondition1 = MDVUtility.ToStr(dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName]);
                        }
                        barCodeCondition2 = MDVUtility.ToStr(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.OrderNoColumn.ColumnName]);
                        barCodeCondition3 = MDVUtility.ToStr(dsPatient_BarCode.Patients.Rows[0][dsPatient_BarCode.Patients.LastNameColumn.ColumnName]);
                        barCodeCondition4 = MDVUtility.ToStr(dsPatient_BarCode.Patients.Rows[0][dsPatient_BarCode.Patients.FirstNameColumn.ColumnName]);
                    }

                    barCodeCondition3 = barCodeCondition3[0].ToString();
                    barCodeCondition4 = barCodeCondition4[0].ToString();
                    if (!string.IsNullOrEmpty(barCodeCondition1))
                    {
                        strBarcode.Append(barCodeCondition1);
                        strBarcode.Append(",");
                    }
                    strBarcode.Append(barCodeCondition2);
                    strBarcode.Append(",");
                    strBarcode.Append(barCodeCondition3);
                    strBarcode.Append(" ");
                    strBarcode.Append(barCodeCondition4);
                }

                img = b.GenerateBarcodeImage(240, 64, strBarcode.ToString());
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Png);
                pic.SpacingAfter = 10;

                // -------------------------------------------------------------------------------------------------------- //

                PdfPTable ReportHeaderBarCode = new PdfPTable(1);
                ReportHeaderBarCode.TotalWidth = 190f;
                ReportHeaderBarCode.LockedWidth = true;
                ReportHeaderBarCode.HorizontalAlignment = Element.ALIGN_RIGHT;
                ReportHeaderBarCode.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell lineCell = new PdfPCell();

                if (BarCodeHtmlText != string.Empty && BarCodeHtmlText.ToLower() != "false" && isLabOrder)
                    lineCell.AddElement(pic);
                else
                    lineCell.AddElement(new Chunk());

                lineCell.Border = Rectangle.NO_BORDER;
                lineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                ReportHeaderBarCode.AddCell(lineCell);


                ReportHeaderTable.TotalWidth = 575f;
                ReportHeaderTable.SpacingBefore = 10f;
                ReportHeaderTable.LockedWidth = true;
                ReportHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                MemoryStream ms = new MemoryStream();
                byte[] bytes;
                using (FileStream file = new FileStream(System.Web.HttpContext.Current.Server.MapPath(@"~\content\images\SHS-nav-logo-Labs.png"), FileMode.Open, System.IO.FileAccess.Read))
                {
                    bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                }

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(bytes, false);
                logo.ScalePercent(75f);
                logo.ScaleAbsoluteHeight(120);
                logo.ScaleAbsoluteWidth(165);

                PdfPCell logoCell = new PdfPCell();
                logoCell.AddElement(logo);
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                ReportHeaderTable.AddCell(logoCell);

                #endregion

                #region Header Table

                ReportHeaderTable.AddCell(ReportHeaderBarCode);
                ReportHeaderTable.AddCell(headerSovreignAddress);

                string clientDetails = "";
                clientDetails = "Phone: (201) 933-3028" + "\n" + "Fax: (201) 703 - 7977\n";



                //if (dsLab.Lab.Rows.Count > 0)
                //{
                //    clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.NameColumn.ColumnName] + "\n");
                //    clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.AddressColumn.ColumnName], "\n");
                //    clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.CityColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.CityColumn.ColumnName] + ", ");
                //    clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.StateColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.StateColumn.ColumnName] + ", ");
                //    clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName] + "\n");
                //    clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.PhoneNoColumn.ColumnName]);
                //}

                Paragraph p = new Paragraph(clientDetails, bodyFont);
                iTextSharp.text.Anchor a = new iTextSharp.text.Anchor("www.SLSResults.com", linkFont);
                a.Reference = "https://slsresults.com";
                p.Add(a);
                PdfPCell clientAccountNumber = new PdfPCell();
                p.Alignment = Element.ALIGN_RIGHT;
                clientAccountNumber.AddElement(p);
                clientAccountNumber.Border = Rectangle.NO_BORDER;
                clientAccountNumber.HorizontalAlignment = Element.ALIGN_RIGHT;

                ReportHeaderTable.AddCell(clientAccountNumber);
                ReportHeaderTable.SpacingAfter = 3f;
                #endregion
            }
            #endregion
            return ReportHeaderTable;
        }
        private PdfPTable SetLabPatient(long patientId, string patientName, BaseColor fontColour, Font bodyFont, Font componentHeadingFont, Font PFont, string AccountNumber = "")
        {
            PdfPTable patientTable = new PdfPTable(1);
            patientTable.DefaultCell.Border = Rectangle.NO_BORDER;
            string PatientText = GetPatientText(patientId);
            string[] PatientColumns = PatientText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
            patientName = PatientColumns[0];


            patientTable.AddCell(new Paragraph("Patient", PFont));
            bool isInserted = false;
            foreach (string PatientColumn in PatientColumns)
            {
                patientTable.AddCell(new Paragraph(PatientColumn, bodyFont));
                if (isInserted == false && AccountNumber != "")
                {
                    patientTable.AddCell(new Paragraph(AccountNumber, bodyFont));
                }
                isInserted = true;
            }

            return patientTable;
        }
        private PdfPTable SetLabProvider(long providerId, Font PFont, Font bodyFont, string ProviderText)
        {
            PdfPTable providerTable = new PdfPTable(1);
            providerTable.DefaultCell.Border = Rectangle.NO_BORDER;
            string[] ProviderColumns = ProviderText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
            string providerData = "";
            for (int i = 0; i < ProviderColumns.Length; i++)
            {
                if (ProviderColumns[i] == "Provider")
                {
                    Paragraph providerParagraph = new Paragraph(ProviderColumns[i], PFont);
                    providerParagraph.Alignment = Element.ALIGN_RIGHT;
                    PdfPCell Providercell = new PdfPCell();
                    Providercell.AddElement(providerParagraph);
                    Providercell.Border = Rectangle.NO_BORDER;
                    Providercell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    providerTable.AddCell(Providercell);
                }
                else
                {
                    providerData += ProviderColumns[i];
                    if (!(i == ProviderColumns.Length - 1))
                    {
                        providerData += "\n";
                    }
                }
            }
            PdfPCell ProviderDataCell = new PdfPCell();
            Paragraph ProviderDetails = new Paragraph(providerData, bodyFont);
            ProviderDetails.Alignment = Element.ALIGN_RIGHT;
            ProviderDataCell.AddElement(ProviderDetails);
            ProviderDataCell.Border = Rectangle.NO_BORDER;
            ProviderDataCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            providerTable.AddCell(ProviderDataCell);
            return providerTable;
        }
        private PdfPTable setLabResultSample(DSLabOrder dsLabOrder, DSLabResult dsLabResult, Font componentHeaderFont, Font bodyFont)
        {
            PdfPTable SampleTable = new PdfPTable(4);
            float[] SampleWidths = new float[] { 4f, 8f, 4f, 4f };
            SampleTable.SetWidths(SampleWidths);
            SampleTable.TotalWidth = 575f;
            SampleTable.LockedWidth = true;
            SampleTable.HorizontalAlignment = Element.ALIGN_CENTER;
            SampleTable.DefaultCell.Border = Rectangle.NO_BORDER;

            Paragraph spec = new Paragraph("Specimen ID:", componentHeaderFont);
            SampleTable.AddCell(spec);
            SampleTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabResultSpecimen.TableName].Rows[0][dsLabResult.LabResultSpecimen.ExternalSpecimenIdColumn.ColumnName])), bodyFont));
            SampleTable.AddCell(new Paragraph("Date of report:", componentHeaderFont));
            var dateTime = MDVUtility.ToDateTime(dsLabResult.Tables[dsLabResult.LabOrderResultDetail.TableName].Rows[0][dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName].ToString());
            var date = dateTime.ToShortDateString();

            SampleTable.AddCell(new Paragraph(date.ToString(), bodyFont));
            SampleTable.AddCell(new Paragraph("Collection Date & Time:\n", componentHeaderFont));
            SampleTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dsLabResult.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.OrderDateColumn.ColumnName]).ToShortDateString() + ' ' + dsLabResult.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.OrderTimeColumn.ColumnName].ToString(), bodyFont));

            SampleTable.AddCell(new Paragraph("Received Date & Time:", componentHeaderFont));
            SampleTable.AddCell(new Paragraph(dsLabResult.Tables[dsLabResult.LabResultSpecimen.TableName].Rows[0][dsLabResult.LabResultSpecimen.CollectionDateTimeColumn.ColumnName].ToString(), bodyFont));

            SampleTable.AddCell(new Paragraph(string.Empty));
            SampleTable.AddCell(new Paragraph(string.Empty));
            return SampleTable;

        }
        private void setLabResultOrderInformation(Document pdfDocument, DSLabResult dsLabResult, DSLabOrder dsLabOrder, Font componentHeaderFont, Font componentHeadingFont, Font bodyFont)
        {
            Paragraph Lab_OrderNumber_Facility_Assigne_Heading = new Paragraph("Order Information \n".ToString(), componentHeadingFont);

            if (dsLabResult.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
            {
                pdfDocument.Add(Lab_OrderNumber_Facility_Assigne_Heading);

                PdfPTable Lab_OrderNumber_Facility_Assigne_Table = new PdfPTable(4);
                float[] orderWidths = new float[] { 4f, 8f, 4f, 4f };
                Lab_OrderNumber_Facility_Assigne_Table.SetWidths(orderWidths);
                Lab_OrderNumber_Facility_Assigne_Table.TotalWidth = 575f;
                Lab_OrderNumber_Facility_Assigne_Table.LockedWidth = true;
                Lab_OrderNumber_Facility_Assigne_Table.HorizontalAlignment = Element.ALIGN_CENTER;
                Lab_OrderNumber_Facility_Assigne_Table.DefaultCell.Border = Rectangle.NO_BORDER;

                foreach (DSLabOrder.LabOrderRow dr in dsLabResult.Tables[dsLabOrder.LabOrder.TableName].Rows)
                {
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph("Laboratory:", componentHeaderFont));
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.LabNameColumn.ColumnName])), bodyFont));

                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph("Order Number:", componentHeaderFont));
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderNoColumn.ColumnName])), bodyFont));

                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph("Facility:", componentHeaderFont));
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityColumn.ColumnName])), bodyFont));
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(string.Empty));
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(string.Empty));

                    if (!string.IsNullOrWhiteSpace(HttpUtility.HtmlDecode(MDVUtility.ToStr(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.AssignedToColumn.ColumnName]))))
                    {
                        Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph("Assignee:", componentHeaderFont));
                        Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.AssignedToColumn.ColumnName])), bodyFont));
                    }
                }
                pdfDocument.Add(Lab_OrderNumber_Facility_Assigne_Table);
            }
            else
            {
                Lab_OrderNumber_Facility_Assigne_Heading.SpacingBefore = Lab_OrderNumber_Facility_Assigne_Heading.SpacingAfter = 5;
                pdfDocument.Add(Lab_OrderNumber_Facility_Assigne_Heading);

                PdfPTable Lab_OrderNumber_Facility_Assigne_Table = new PdfPTable(5);
                float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
                Lab_OrderNumber_Facility_Assigne_Table.SetWidths(widths);
                Lab_OrderNumber_Facility_Assigne_Table.TotalWidth = 575f;
                Lab_OrderNumber_Facility_Assigne_Table.LockedWidth = true;
                Lab_OrderNumber_Facility_Assigne_Table.DefaultCell.Border = Rectangle.NO_BORDER;
                Lab_OrderNumber_Facility_Assigne_Table.AddCell(string.Empty);
                Lab_OrderNumber_Facility_Assigne_Table.AddCell(string.Empty);
                pdfDocument.Add(Lab_OrderNumber_Facility_Assigne_Table);
                Paragraph orderInformation = new Paragraph("No Order Information Found".ToString());
                orderInformation.Alignment = Element.ALIGN_CENTER;
                pdfDocument.Add(orderInformation);
            }
        }
        private void setLabresultInformation(Document pdfDocument, DSLabOrder dsLabOrder, DSLabResult dsLabResult, Font componentHeadingFont, Font componentHeaderFont, Font componentGridHeaderFont, Font bodyFont, Font uomFont, Font uomFontOrange, Font gridbodyFont, Font bodyFontSmall)
        {
            if (dsLabResult.Tables[dsLabResult.LabOrderResultDetail.TableName].Rows.Count > 0)
            {
                PdfPTable resultHeader = new PdfPTable(4);
                float[] resultHeaderwidths = new float[] { 4f, 8f, 4f, 4f };
                resultHeader.SetWidths(resultHeaderwidths);
                resultHeader.TotalWidth = 575f;
                resultHeader.SpacingBefore = 5f;
                resultHeader.LockedWidth = true;
                resultHeader.DefaultCell.Border = Rectangle.NO_BORDER;

                Paragraph test_Heading1 = new Paragraph("Result Information".ToString(), componentHeadingFont);
                test_Heading1.SpacingBefore = 5f;
                test_Heading1.SpacingAfter = 10f;
                resultHeader.AddCell(test_Heading1);
                resultHeader.AddCell(string.Empty);

                bool flagCorrected = false;
                for (int i = 0; i < dsLabResult.LabOrderResultDetail.Rows.Count; i++)
                    if (dsLabResult.LabOrderResultDetail.Rows[i][dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName].ToString() == "Correction")
                        flagCorrected = true;

                if (!flagCorrected)
                {
                    Paragraph test_Heading2 = new Paragraph("Status: " + dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.StatusColumn.ColumnName].ToString(), componentHeaderFont);
                    test_Heading2.Alignment = Element.ALIGN_LEFT;
                    resultHeader.AddCell(test_Heading2);
                }
                else
                {
                    Paragraph test_Heading2 = new Paragraph("Status: Revised", componentHeaderFont);
                    test_Heading2.Alignment = Element.ALIGN_LEFT;
                    resultHeader.AddCell(test_Heading2);
                }

                resultHeader.AddCell(string.Empty);
                pdfDocument.Add(resultHeader);

                PdfPTable testTable = new PdfPTable(6);
                float[] widths = new float[] { 3f, 9f, 2f, 2f, 2f, 2.5f };
                testTable.SetWidths(widths);
                testTable.TotalWidth = 575f;
                testTable.LockedWidth = true;
                testTable.DefaultCell.Border = Rectangle.NO_BORDER;
                testTable.AddCell(new Paragraph("Date & Time", componentGridHeaderFont));
                testTable.AddCell(new Paragraph("Observation", componentGridHeaderFont));
                testTable.AddCell(new Paragraph("Result", componentGridHeaderFont));
                testTable.AddCell(new Paragraph("UoM", componentGridHeaderFont));
                testTable.AddCell(new Paragraph("Flag", componentGridHeaderFont));
                testTable.AddCell(new Paragraph("Range", componentGridHeaderFont));


                foreach (DSLabOrder.LabOrderTestRow dr in dsLabResult.Tables[dsLabOrder.LabOrderTest.TableName].Rows)
                {
                    if (MDVUtility.ToBool(dr[dsLabOrder.LabOrderTest.IsActiveColumn.ColumnName]))
                    {
                        testTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsLabOrder.LabOrderTest.TestDateColumn.ColumnName]).ToShortDateString() + ' ' + dr[dsLabOrder.LabOrderTest.TestTimeColumn.ColumnName].ToString(), gridbodyFont));
                        testTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.CPTCodeColumn.ColumnName] + " " + dr[dsLabOrder.LabOrderTest.CPTCodeDescriptionColumn.ColumnName])), gridbodyFont));

                        testTable.AddCell(string.Empty);
                        testTable.AddCell(string.Empty);
                        testTable.AddCell(string.Empty);
                        testTable.AddCell(string.Empty);

                        DSLabResult.LabOrderResultDetailRow[] labResultDetail = (DSLabResult.LabOrderResultDetailRow[])dsLabResult.LabOrderResultDetail.Select(dsLabResult.LabOrderResultDetail.CPTCodeColumn.ColumnName + '=' + MDVUtility.ToLINQFormatString(dr[dsLabOrder.LabOrderTest.CPTCodeColumn.ColumnName]));
                        foreach (DSLabResult.LabOrderResultDetailRow drDetail in labResultDetail)
                        {
                            Font temp = null;
                            testTable.AddCell(new Paragraph(MDVUtility.ToDateTime(drDetail[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm"), bodyFont));
                            if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]) == "Normal")
                            {
                                if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + "  (Corrected)", bodyFont));
                                    temp = bodyFont;
                                }
                                else
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), bodyFont));
                                    temp = bodyFont;
                                }
                            }

                            else if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]) == "High")
                            {
                                if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + "  (Corrected)", uomFont));
                                    temp = uomFont;
                                }
                                else
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), uomFont));
                                    temp = uomFont;
                                }
                            }
                            else if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]).IndexOf("Abnormal") > -1)
                            {
                                if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + "  (Corrected)", uomFont));
                                    temp = uomFont;
                                }
                                else
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), uomFont));
                                    temp = uomFont;
                                }
                            }
                            else if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]) == "Low")
                            {
                                if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + "  (Corrected)", uomFontOrange));
                                    temp = uomFontOrange;
                                }
                                else
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), uomFontOrange));
                                    temp = uomFontOrange;
                                }
                            }
                            else if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]).IndexOf("Susceptible") > -1)
                            {
                                if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + "  (Corrected)", bodyFont));
                                    temp = bodyFont;
                                }
                                else
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), bodyFont));
                                    temp = bodyFont;
                                }
                            }
                            else if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]) == "Intermediate")
                            {
                                if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + "  (Corrected)", bodyFont));
                                    temp = bodyFont;
                                }
                                else
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), bodyFont));
                                    temp = bodyFont;
                                }
                            }
                            else if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]) == "Resistant")
                            {
                                if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + "  (Corrected)", bodyFont));
                                    temp = bodyFont;
                                }
                                else
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), bodyFont));
                                    temp = bodyFont;
                                }
                            }
                            else if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]) == "Negative")
                            {
                                if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + "  (Corrected)", uomFont));
                                    temp = uomFont;
                                }
                                else
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), uomFont));
                                    temp = uomFont;
                                }

                            }
                            else if (string.IsNullOrEmpty(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName])))
                            {
                                if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + "  (Corrected)", bodyFont));
                                    temp = bodyFont;
                                }
                                else
                                {
                                    testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), bodyFont));
                                    temp = bodyFont;
                                }
                            }
                            else
                                continue;

                            testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.ResultColumn.ColumnName]), temp));
                            testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.UoMColumn.ColumnName]), bodyFont));
                            testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]), temp));
                            testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.RangeColumn.ColumnName]), bodyFont));
                            if (!String.IsNullOrEmpty(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.NTETextColumn.ColumnName])))
                            {
                                string nte = MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.NTETextColumn.ColumnName]);
                                string[] nteColumns = nte.Split(new string[] { "~" }, StringSplitOptions.None);
                                nte = "";
                                string nteTemp = "";
                                foreach (string nteColumn in nteColumns)
                                {
                                    nteTemp = nteColumn.TrimStart();
                                    testTable.AddCell(String.Empty);
                                    testTable.AddCell(new Phrase(nteTemp, bodyFontSmall));
                                    testTable.AddCell(String.Empty);
                                    testTable.AddCell(String.Empty);
                                    testTable.AddCell(String.Empty);
                                    testTable.AddCell(String.Empty);
                                }
                            }
                        }
                    }
                }
                pdfDocument.Add(testTable);
            }
            else
            {
                Paragraph test_Heading = new Paragraph("Result Information \n".ToString(), componentHeadingFont);
                test_Heading.SpacingBefore = test_Heading.SpacingAfter = 5;
                pdfDocument.Add(test_Heading);
                PdfPTable testTable = new PdfPTable(7);
                float[] widths = new float[] { 8f, 10f, 8f, 8f, 8f, 8f, 8f };
                testTable.SetWidths(widths);
                testTable.TotalWidth = 575f;
                testTable.LockedWidth = true;
                testTable.DefaultCell.Border = Rectangle.NO_BORDER;
                testTable.AddCell(string.Empty);
                pdfDocument.Add(testTable);
                Paragraph noTest = new Paragraph("No Result Information Found".ToString());
                noTest.Alignment = Element.ALIGN_CENTER;
                pdfDocument.Add(noTest);
            }
        }
        private void setCLIA(Document pdfDocument, DSLab dsLab, Font componentHeaderFont)
        {
            //Paragraph CLIA = new Paragraph(MDVUtility.ToStr("Lab Test Performed By: "
            //+ dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.CLIANoColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.NameColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.AddressColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.CityColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.StateColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab._FillersField_2Column.ColumnName]), componentHeaderFont);
            //return CLIA;
            if (dsLab.Lab.Rows.Count > 0)
            {
                string CLIANo = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.CLIANoColumn.ColumnName]);
                CLIANo = string.IsNullOrEmpty(CLIANo) ? "" : CLIANo + ", ";

                string labName = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.NameColumn.ColumnName]);
                labName = string.IsNullOrEmpty(labName) ? "" : labName + ", ";

                string labAddress = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.AddressColumn.ColumnName]);
                labAddress = string.IsNullOrEmpty(labAddress) ? "" : labAddress + ", ";

                string labCity = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.CityColumn.ColumnName]);
                labCity = string.IsNullOrEmpty(labCity) ? "" : labCity + ", ";

                string labState = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.StateColumn.ColumnName]);
                labState = string.IsNullOrEmpty(labState) ? "" : labState + ", ";

                string labZIPCode = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName]);
                labZIPCode = string.IsNullOrEmpty(labZIPCode) ? "" : labZIPCode + ", ";

                string labFillersField_2 = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab._FillersField_2Column.ColumnName]);
                labFillersField_2 = string.IsNullOrEmpty(labFillersField_2) ? "" : labFillersField_2 + ", ";

                string commaSeparatedText = CLIANo + labName + labAddress + labCity + labState + labZIPCode + labFillersField_2;

                if (!string.IsNullOrEmpty(commaSeparatedText))
                {
                    string CLIAText = "Lab Test Performed By: " + commaSeparatedText;
                    CLIAText = CLIAText.Trim().Trim(',');

                    Paragraph CLIA = new Paragraph(CLIAText, componentHeaderFont);
                    pdfDocument.Add(CLIA);
                }
            }
        }
        #region LabOrder, OrderInformation
        private void labOrderInformation(Document pdfDocument, DSLabOrder dsLabOrder, Font componentHeaderFont, Font componentHeadingFont, Font bodyFont)
        {
            if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
            {
                Paragraph order_Heading = new Paragraph("Order Information \n".ToString(), componentHeadingFont);
                pdfDocument.Add(order_Heading);

                PdfPTable orderTable = new PdfPTable(4);
                float[] widths = new float[] { 3f, 8f, 4f, 3f };
                orderTable.SetWidths(widths);
                orderTable.TotalWidth = 575f;
                orderTable.LockedWidth = true;
                orderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                orderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                foreach (DSLabOrder.LabOrderRow dr in dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows)
                {
                    orderTable.AddCell(new Paragraph("Laboratory:", componentHeaderFont));
                    orderTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.LabNameColumn.ColumnName])), bodyFont));
                    orderTable.AddCell(new Paragraph("Collection Date & Time:", componentHeaderFont));
                    orderTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName]).ToShortDateString() + ' ' + dr[dsLabOrder.LabOrder.OrderTimeColumn.ColumnName].ToString(), bodyFont));
                    orderTable.AddCell(new Paragraph("Facility:", componentHeaderFont));
                    orderTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityColumn.ColumnName])), bodyFont));
                    orderTable.AddCell(new Paragraph("Order Number:", componentHeaderFont));
                    orderTable.AddCell(new Paragraph(dr[dsLabOrder.LabOrder.OrderNoColumn.ColumnName].ToString(), bodyFont));
                }
                pdfDocument.Add(orderTable);
            }
            else
            {
                Paragraph order_Heading = new Paragraph("Order Information \n".ToString(), componentHeadingFont);
                order_Heading.SpacingBefore = order_Heading.SpacingAfter = 5;
                pdfDocument.Add(order_Heading);

                PdfPTable orderTable = new PdfPTable(5);
                float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
                orderTable.SetWidths(widths);
                orderTable.TotalWidth = 575f;
                orderTable.LockedWidth = true;
                orderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                orderTable.AddCell(string.Empty);
                pdfDocument.Add(orderTable);
                Paragraph noOrder = new Paragraph("No Order Information Found".ToString());
                noOrder.Alignment = Element.ALIGN_CENTER;
                pdfDocument.Add(noOrder);
            }
        }
        private void labOrderInformationANDInsuredInformation(Document pdfDocument, DSLabOrder dsLabOrder, Font componentHeaderFont, Font componentHeadingFont, Font bodyFont, DSLabOrder dsLabOrderExternalBillingInfoPrimary, DataRow drLabOrderExternalBillingInfoPrimary, DSLab dslab)
        {
            bool IsInsuranceAvailable = false;
            if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
            {
                Paragraph order_Heading = new Paragraph("Billing Information \n".ToString(), componentHeadingFont);

                pdfDocument.Add(order_Heading);

                PdfPTable orderTable = new PdfPTable(4);
                float[] widths = new float[] { 3f, 8f, 4f, 3f };
                orderTable.SetWidths(widths);
                orderTable.TotalWidth = 575f;
                orderTable.LockedWidth = true;
                orderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                orderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                foreach (DSLabOrder.LabOrderRow dr in dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows)
                {
                    int billingTypeId = MDVUtility.ToInt32(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.BillingTypeIdColumn.ColumnName]));
                    var primaryInsID = MDVUtility.ToStr(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.PrimaryInsuraceIdColumn.ColumnName]));

                    if (!string.IsNullOrEmpty(primaryInsID))
                    {
                        IsInsuranceAvailable = true;
                        dsLabOrderExternalBillingInfoPrimary = new DALLabOrder().LoadLabOrderExternalBillingInformation(primaryInsID);
                    }

                    if (billingTypeId != 0)
                    {
                        orderTable.AddCell(new Paragraph("Billing Type:", componentHeaderFont));

                        //if (billingTypeId == 1)
                        //    orderTable.AddCell(new Paragraph("Patient", bodyFont));
                        //else if (billingTypeId == 2)
                        //    orderTable.AddCell(new Paragraph("Client", bodyFont));
                        //else if (billingTypeId == 3)
                        //    orderTable.AddCell(new Paragraph("Insurance", bodyFont));


                        if (billingTypeId != 3)
                        {
                            Paragraph noOrder = new Paragraph("No Billing Information Found".ToString(), bodyFont);
                            noOrder.Alignment = Element.ALIGN_CENTER;
                            pdfDocument.Add(noOrder);
                            IsInsuranceAvailable = false;
                        }
                        if (billingTypeId == 3)
                        {
                            orderTable.AddCell(new Paragraph("Insurance", bodyFont));
                            if (dsLabOrderExternalBillingInfoPrimary != null && dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.Rows.Count > 0)
                            {
                                drLabOrderExternalBillingInfoPrimary = dsLabOrderExternalBillingInfoPrimary.Tables[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.TableName].Rows[0];
                                orderTable.AddCell(new Paragraph("Policy No:", componentHeaderFont));
                                orderTable.AddCell(new Paragraph(string.IsNullOrEmpty(drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredSubscriberIdColumn.ColumnName].ToString()) ? "N/A" : drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredSubscriberIdColumn.ColumnName].ToString(), bodyFont));
                            }
                            else
                            {
                                orderTable.AddCell(new Paragraph(string.Empty, componentHeaderFont));
                                orderTable.AddCell(new Paragraph(string.Empty, bodyFont));
                            }
                        }

                        if (billingTypeId == 3)
                            if (dsLabOrderExternalBillingInfoPrimary != null && dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.Rows.Count > 0)
                                drLabOrderExternalBillingInfoPrimary = dsLabOrderExternalBillingInfoPrimary.Tables[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.TableName].Rows[0];

                        if (billingTypeId == 3 && dsLabOrder != null && dsLabOrderExternalBillingInfoPrimary != null && drLabOrderExternalBillingInfoPrimary != null)
                        {
                            if (MDVUtility.ToStr(dr[dsLabOrder.LabOrder.PrimaryInsuraceColumn.ColumnName]) != string.Empty)
                            {

                                if (!String.IsNullOrEmpty(primaryInsID))
                                {
                                    var insuraneName = dr[dsLabOrder.LabOrder.PrimaryInsuraceColumn.ColumnName].ToString();
                                    int index = insuraneName.IndexOf("[");

                                    if (index > 0)
                                    {
                                        insuraneName = insuraneName.Substring(0, index);

                                        if (Convert.ToString(dslab.Tables[dslab.Lab.TableName].Rows[0][dslab.Lab.NameColumn.ColumnName]) != "VitalAxis") // VitalAxis does not receive insurance codes
                                        {
                                            insuraneName += "[" + drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.BRLCodeColumn.ColumnName].ToString() + "]";
                                        }

                                        orderTable.AddCell(new Paragraph("Primary Insurance:", componentHeaderFont));
                                        orderTable.AddCell(new Paragraph(insuraneName, bodyFont));

                                        if (dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.Rows.Count > 0)
                                        {

                                            drLabOrderExternalBillingInfoPrimary = dsLabOrderExternalBillingInfoPrimary.Tables[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.TableName].Rows[0];
                                            orderTable.AddCell(new Paragraph("Group No:", componentHeaderFont));
                                            orderTable.AddCell(new Paragraph(string.IsNullOrEmpty(drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredGroupIdColumn.ColumnName].ToString()) ? "N/A" : drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredGroupIdColumn.ColumnName].ToString(), bodyFont));

                                        }

                                        orderTable.AddCell(new Paragraph("Address: ", componentHeaderFont));
                                        orderTable.AddCell(new Paragraph(string.IsNullOrEmpty(drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsurancePlanAddressColumn.ColumnName].ToString()) ? "N/A" : drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsurancePlanAddressColumn.ColumnName].ToString(), bodyFont));
                                        orderTable.AddCell(new Paragraph(string.Empty));
                                        orderTable.AddCell(new Paragraph(string.Empty));
                                    }
                                }
                            }
                            else
                            {
                                orderTable.AddCell(new Paragraph(string.Empty, componentHeaderFont));
                                orderTable.AddCell(new Paragraph(string.Empty, bodyFont));
                            }
                        }
                    }
                    else
                    {

                        Paragraph noOrder = new Paragraph("No Billing Information Found".ToString(), bodyFont);
                        noOrder.Alignment = Element.ALIGN_CENTER;
                    }
                }
                pdfDocument.Add(orderTable);
            }
            else
            {
                Paragraph order_Heading = new Paragraph("Billing Information \n".ToString(), componentHeadingFont);
                order_Heading.SpacingBefore = order_Heading.SpacingAfter = 5;
                pdfDocument.Add(order_Heading);

                PdfPTable orderTable = new PdfPTable(5);
                float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
                orderTable.SetWidths(widths);
                orderTable.TotalWidth = 575f;
                orderTable.LockedWidth = true;
                orderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                orderTable.AddCell(string.Empty);
                pdfDocument.Add(orderTable);
                Paragraph noOrder = new Paragraph("No Billing Information Found".ToString());
                noOrder.Alignment = Element.ALIGN_CENTER;
                pdfDocument.Add(noOrder);
            }

            if (IsInsuranceAvailable)
            {
                Paragraph Relation_Heading = new Paragraph("Insured Relationship\n".ToString(), componentHeadingFont);
                pdfDocument.Add(Relation_Heading);

                PdfPTable insuredTable = new PdfPTable(4);
                float[] insuredWidths = new float[] { 3f, 8f, 4f, 3f };
                insuredTable.SetWidths(insuredWidths);
                insuredTable.TotalWidth = 575f;
                insuredTable.LockedWidth = true;
                insuredTable.HorizontalAlignment = Element.ALIGN_CENTER;
                insuredTable.DefaultCell.Border = Rectangle.NO_BORDER;

                if (drLabOrderExternalBillingInfoPrimary != null)
                {

                    Paragraph rel = new Paragraph("Insured Name: ", componentHeaderFont);
                    var insuredLastName = string.IsNullOrEmpty(drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredLastNameColumn.ColumnName.ToString()].ToString()) ? "N/A" : drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredLastNameColumn.ColumnName.ToString()].ToString();
                    var insuredFirstName = string.IsNullOrEmpty(drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredFirstNameColumn.ColumnName.ToString()].ToString()) ? "N/A" : drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredFirstNameColumn.ColumnName.ToString()].ToString();

                    Paragraph value = new Paragraph((insuredLastName + ", " + insuredFirstName), bodyFont);
                    Paragraph relName = new Paragraph("Insured Relation: ", componentHeaderFont);

                    Paragraph relValue = new Paragraph(string.IsNullOrEmpty(drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredRelationshipColumn.ColumnName.ToString()].ToString()) ? "N/A" : drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredRelationshipColumn.ColumnName.ToString()].ToString(), bodyFont);
                    Paragraph paraAddress = new Paragraph("Insured Address: ", componentHeaderFont);
                    Paragraph relAddValue = new Paragraph(string.IsNullOrEmpty(drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredAddressColumn.ColumnName.ToString()].ToString()) ? "N/A" : drLabOrderExternalBillingInfoPrimary[dsLabOrderExternalBillingInfoPrimary.External_PatientInsurancePlanAddress.InsuredAddressColumn.ColumnName.ToString()].ToString(), bodyFont);
                    insuredTable.AddCell(rel);
                    insuredTable.AddCell(value);
                    insuredTable.AddCell(relName);
                    insuredTable.AddCell(relValue);
                    insuredTable.AddCell(paraAddress);
                    insuredTable.AddCell(relAddValue);
                    insuredTable.AddCell(new Paragraph(string.Empty));
                    insuredTable.AddCell(new Paragraph(string.Empty));
                    pdfDocument.Add(insuredTable);
                }
                else
                {
                    Paragraph rel = new Paragraph("Insured Name: ", componentHeaderFont);
                    Paragraph value = new Paragraph(("N/A"), bodyFont);
                    Paragraph relName = new Paragraph("Insured Relation: ", componentHeaderFont);
                    Paragraph relValue = new Paragraph("N/A", bodyFont);
                    Paragraph paraAddress = new Paragraph("Insured Address: ", componentHeaderFont);
                    Paragraph relAddValue = new Paragraph("N/A", bodyFont);
                    insuredTable.AddCell(rel);
                    insuredTable.AddCell(value);
                    insuredTable.AddCell(relName);
                    insuredTable.AddCell(relValue);
                    insuredTable.AddCell(paraAddress);
                    insuredTable.AddCell(relAddValue);
                    insuredTable.AddCell(new Paragraph(string.Empty));
                    insuredTable.AddCell(new Paragraph(string.Empty));
                    pdfDocument.Add(insuredTable);
                }
            }
        }
        private void labOrderGuarantor(Document pdfDocument, Font componentHeadingFont, Font componentHeaderFont, Font bodyFont, DSLabOrder dsLabOrder)
        {
            foreach (DSLabOrder.LabOrderRow dr in dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows)
            {
                Paragraph Relation_Heading = new Paragraph("Guarantor\n".ToString(), componentHeadingFont);
                pdfDocument.Add(Relation_Heading);
                if (!string.IsNullOrEmpty(dr[dsLabOrder.LabOrder.GuarantorFirstNameColumn.ColumnName].ToString()))
                {
                    PdfPTable GuarantorTable = new PdfPTable(4);
                    Paragraph value = null;

                    float[] GuarantorWidths = new float[] { 3f, 8f, 4f, 3f };
                    GuarantorTable.SetWidths(GuarantorWidths);
                    GuarantorTable.TotalWidth = 575f;
                    GuarantorTable.LockedWidth = true;
                    GuarantorTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    GuarantorTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    Paragraph rel = new Paragraph("Guarantor Name: ", componentHeaderFont);
                    var guarantorLastName = string.IsNullOrEmpty(dr[dsLabOrder.LabOrder.GuarantorLastNameColumn].ToString()) ? "N/A" : dr[dsLabOrder.LabOrder.GuarantorLastNameColumn.ColumnName].ToString();
                    var guarantorFirstName = string.IsNullOrEmpty(dr[dsLabOrder.LabOrder.GuarantorFirstNameColumn.ColumnName].ToString()) ? "N/A" : dr[dsLabOrder.LabOrder.GuarantorFirstNameColumn.ColumnName].ToString();
                    if (guarantorFirstName == "N/A" && guarantorLastName == "N/A")
                    {
                        value = new Paragraph(("N/A"), bodyFont);
                    }
                    else
                    {
                        value = new Paragraph((guarantorLastName + ", " + guarantorFirstName), bodyFont);
                    }
                    Paragraph relName = new Paragraph("Guarantor Relation: ", componentHeaderFont);
                    Paragraph relValue = new Paragraph(string.IsNullOrEmpty(dr[dsLabOrder.LabOrder.RelationShipColumn.ColumnName].ToString()) ? "N/A" : dr[dsLabOrder.LabOrder.RelationShipColumn.ColumnName].ToString(), bodyFont);
                    Paragraph paraAddress = new Paragraph("Guarantor Address: ", componentHeaderFont);
                    Paragraph relAddValue = new Paragraph(string.IsNullOrEmpty(dr[dsLabOrder.LabOrder.GuarantorAddressColumn.ColumnName].ToString()) ? "N/A" : dr[dsLabOrder.LabOrder.GuarantorAddressColumn.ColumnName].ToString() + ", " + dr[dsLabOrder.LabOrder.GuarantorCityColumn.ColumnName] + ", " + dr[dsLabOrder.LabOrder.GuarantorStateColumn.ColumnName] + ", " + dr[dsLabOrder.LabOrder.GuarantorZipCodeColumn.ColumnName], bodyFont);
                    GuarantorTable.AddCell(rel);
                    GuarantorTable.AddCell(value);
                    GuarantorTable.AddCell(relName);
                    GuarantorTable.AddCell(relValue);
                    GuarantorTable.AddCell(paraAddress);
                    GuarantorTable.AddCell(relAddValue);
                    GuarantorTable.AddCell(new Paragraph(string.Empty));
                    GuarantorTable.AddCell(new Paragraph(string.Empty));
                    pdfDocument.Add(GuarantorTable);

                }
                else
                {
                    Paragraph noProblems = new Paragraph("No Guarantor Found".ToString(), bodyFont);
                    noProblems.Alignment = Element.ALIGN_CENTER;
                    pdfDocument.Add(noProblems);
                }
            }
        }
        private void labOrderTestInformation(Document pdfDocument, Font componentHeadingFont, Font componentHeaderFont, Font bodyFont, Font testDetailFont, DSLabOrder dsLabOrder)
        {
            int Counter = 1;
            if (dsLabOrder.Tables[dsLabOrder.LabOrderTest.TableName].Rows.Count > 0)
            {
                Paragraph test_Heading = new Paragraph("Test Information \n".ToString(), componentHeadingFont);
                pdfDocument.Add(test_Heading);

                PdfPTable testTable = new PdfPTable(3);
                float[] widths = new float[] { 3f, 12f, 3f };
                testTable.SetWidths(widths);
                testTable.TotalWidth = 575f;
                testTable.LockedWidth = true;
                testTable.HorizontalAlignment = Element.ALIGN_CENTER;
                testTable.DefaultCell.Border = Rectangle.NO_BORDER;

                DSLabOrder dsLabOrderTestAOEAnswer = new DSLabOrder();
                var labOrderTestAOEAnswerText = "";
                foreach (DSLabOrder.LabOrderTestRow dr in dsLabOrder.Tables[dsLabOrder.LabOrderTest.TableName].Rows)
                {
                    labOrderTestAOEAnswerText = "";

                    #region OBX, AOEAnswers, //MK

                    var labOrderTestId = dr[dsLabOrder.LabOrderTest.LabOrderTestIdColumn.ColumnName].ToString();
                    if (!String.IsNullOrEmpty(labOrderTestId))
                    {
                        dsLabOrderTestAOEAnswer = new DALLabOrder().LoadLabOrderAOEAnswers("", labOrderTestId);
                        if (dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.Rows.Count > 0)
                        {
                            var count = dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.Rows.Count;
                            foreach (DSLabOrder.LabOrderAOEAnswersRow drLabOrderAoeAnswers in dsLabOrderTestAOEAnswer.Tables[dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.TableName].Rows)
                            {
                                if (--count > 0)
                                    labOrderTestAOEAnswerText += String.Concat(MDVUtility.ToStr(drLabOrderAoeAnswers[dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.QuestionColumn.ColumnName]), ": " + MDVUtility.ToStr(drLabOrderAoeAnswers[dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.AnswerColumn.ColumnName]), ", ");
                                else
                                    labOrderTestAOEAnswerText += String.Concat(MDVUtility.ToStr(drLabOrderAoeAnswers[dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.QuestionColumn.ColumnName]), ": " + MDVUtility.ToStr(drLabOrderAoeAnswers[dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.AnswerColumn.ColumnName]));
                            }
                        }
                    }

                    #endregion

                    testTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsLabOrder.LabOrderTest.TestDateColumn.ColumnName]).ToShortDateString() + ' ' + dr[dsLabOrder.LabOrderTest.TestTimeColumn.ColumnName].ToString(), bodyFont));
                    labOrderTestAOEAnswerText.Replace("::", " ");
                    labOrderTestAOEAnswerText = HttpUtility.HtmlDecode(labOrderTestAOEAnswerText);
                    var diet = MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.DietDescriptionColumn.ColumnName]);
                    var specimenDesc = MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.SpecimenDescriptionColumn.ColumnName]);
                    var sampleStorage = MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.SampleStorageColumn.ColumnName]);
                    var testDesc = MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.CPTCodeColumn.ColumnName].ToString() + " " + dr[dsLabOrder.LabOrderTest.CPTCodeDescriptionColumn.ColumnName].ToString());
                    Paragraph p = new Paragraph(testDesc, bodyFont);
                    p.SetLeading(0.0f, 3.0f);

                    testTable.AddCell(p);
                    if (!string.IsNullOrEmpty(dr[dsLabOrder.LabOrderTest.UrgencyNameColumn.ColumnName].ToString()))
                        testTable.AddCell(new Paragraph("Urgency: " + dr[dsLabOrder.LabOrderTest.UrgencyNameColumn.ColumnName].ToString(), bodyFont));
                    else
                        testTable.AddCell(string.Empty);

                    if (labOrderTestAOEAnswerText != "")
                    {
                        testTable.AddCell(string.Empty);
                        Paragraph p1 = new Paragraph(labOrderTestAOEAnswerText, testDetailFont);
                        p.SetLeading(0.0f, 3.0f);
                        testTable.AddCell(p1);
                        testTable.AddCell(string.Empty);
                    }
                    var details = "";
                    if (diet != "")
                        details += "Diet: " + diet;
                    if (specimenDesc != "")
                        details += "   Specimen: " + specimenDesc;
                    if (sampleStorage != "")
                        details += "   Sample Storage: " + sampleStorage;

                    if (details != "")
                    {
                        testTable.AddCell(string.Empty);
                        Paragraph p2 = new Paragraph(details, testDetailFont);
                        p.SetLeading(0.0f, 3.0f);
                        testTable.AddCell(p2);
                        testTable.AddCell(string.Empty);
                    }
                }
                pdfDocument.Add(testTable);
            }
            else
            {
                Paragraph test_Heading = new Paragraph("Test Information \n".ToString(), componentHeadingFont);
                test_Heading.SpacingBefore = test_Heading.SpacingAfter = 5;
                pdfDocument.Add(test_Heading);
                PdfPTable testTable = new PdfPTable(5);
                float[] hwidths = new float[] { 8f, 8f, 8f, 8f, 8f };
                testTable.SetWidths(hwidths);
                testTable.TotalWidth = 575f;
                testTable.LockedWidth = true;
                testTable.DefaultCell.Border = Rectangle.NO_BORDER;
                testTable.AddCell(string.Empty);
                pdfDocument.Add(testTable);
                Paragraph noTest = new Paragraph("No Test Information Found".ToString());
                noTest.Alignment = Element.ALIGN_CENTER;
                pdfDocument.Add(noTest);
            }
        }

        private void labOrderTestInformationVitalAxis(Document pdfDocument, Font componentHeadingFont, Font componentHeaderFont, Font bodyFont, Font testDetailFont, DSLabOrder dsLabOrder)
        {
            int Counter = 1;
            if (dsLabOrder.Tables[dsLabOrder.LabOrderTest.TableName].Rows.Count > 0)
            {
                Paragraph test_Heading = new Paragraph("Test Information \n".ToString(), componentHeadingFont);
                pdfDocument.Add(test_Heading);

                PdfPTable testTable = new PdfPTable(3);
                float[] widths = new float[] { 3f, 12f, 3f };
                testTable.SetWidths(widths);
                testTable.TotalWidth = 575f;
                testTable.LockedWidth = true;
                testTable.HorizontalAlignment = Element.ALIGN_CENTER;
                testTable.DefaultCell.Border = Rectangle.NO_BORDER;

                DSLabOrder dsLabOrderTestAOEAnswer = new DSLabOrder();
                var labOrderTestAOEAnswerText = "";
                foreach (DSLabOrder.LabOrderTestRow dr in dsLabOrder.Tables[dsLabOrder.LabOrderTest.TableName].Rows)
                {
                    labOrderTestAOEAnswerText = "";

                    #region OBX, AOEAnswers, //MK

                    var labOrderTestId = dr[dsLabOrder.LabOrderTest.LabOrderTestIdColumn.ColumnName].ToString();
                    if (!String.IsNullOrEmpty(labOrderTestId))
                    {
                        dsLabOrderTestAOEAnswer = new DALLabOrder().LoadLabOrderAOEAnswers("", labOrderTestId);
                        if (dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.Rows.Count > 0)
                        {
                            foreach (DSLabOrder.LabOrderAOEAnswersRow drLabOrderAoeAnswers in dsLabOrderTestAOEAnswer.Tables[dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.TableName].Rows)
                            {
                                labOrderTestAOEAnswerText += String.Concat(MDVUtility.ToStr(drLabOrderAoeAnswers[dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.QuestionColumn.ColumnName]), ": "
                                    + MDVUtility.ToStr(drLabOrderAoeAnswers[dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.AnswerColumn.ColumnName]));
                                labOrderTestAOEAnswerText += "\n";
                            }
                        }
                    }

                    #endregion

                    testTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsLabOrder.LabOrderTest.TestDateColumn.ColumnName]).ToShortDateString() + ' ' + dr[dsLabOrder.LabOrderTest.TestTimeColumn.ColumnName].ToString(), bodyFont));
                    var diet = MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.DietDescriptionColumn.ColumnName]);
                    var specimenDesc = MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.SpecimenDescriptionColumn.ColumnName]);
                    var sampleStorage = MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.SampleStorageColumn.ColumnName]);
                    var testDesc = MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.CPTCodeColumn.ColumnName].ToString() + " " + dr[dsLabOrder.LabOrderTest.CPTCodeDescriptionColumn.ColumnName].ToString());
                    Paragraph p = new Paragraph(testDesc, bodyFont);
                    p.SetLeading(0.0f, 3.0f);

                    testTable.AddCell(p);
                    if (!string.IsNullOrEmpty(dr[dsLabOrder.LabOrderTest.UrgencyNameColumn.ColumnName].ToString()))
                        testTable.AddCell(new Paragraph("Urgency: " + dr[dsLabOrder.LabOrderTest.UrgencyNameColumn.ColumnName].ToString(), bodyFont));
                    else
                        testTable.AddCell(string.Empty);

                    if (labOrderTestAOEAnswerText != "")
                    {
                        testTable.AddCell(string.Empty);
                        Paragraph p1 = new Paragraph(labOrderTestAOEAnswerText, testDetailFont);
                        p.SetLeading(0.0f, 3.0f);
                        testTable.AddCell(p1);
                        testTable.AddCell(string.Empty);
                    }
                    var details = "";
                    if (diet != "")
                        details += "Diet: " + diet;
                    if (specimenDesc != "")
                        details += "   Specimen: " + specimenDesc;
                    if (sampleStorage != "")
                        details += "   Sample Storage: " + sampleStorage;

                    if (details != "")
                    {
                        testTable.AddCell(string.Empty);
                        Paragraph p2 = new Paragraph(details, testDetailFont);
                        p.SetLeading(0.0f, 3.0f);
                        testTable.AddCell(p2);
                        testTable.AddCell(string.Empty);
                    }
                }
                pdfDocument.Add(testTable);
            }
            else
            {
                Paragraph test_Heading = new Paragraph("Test Information \n".ToString(), componentHeadingFont);
                test_Heading.SpacingBefore = test_Heading.SpacingAfter = 5;
                pdfDocument.Add(test_Heading);
                PdfPTable testTable = new PdfPTable(5);
                float[] hwidths = new float[] { 8f, 8f, 8f, 8f, 8f };
                testTable.SetWidths(hwidths);
                testTable.TotalWidth = 575f;
                testTable.LockedWidth = true;
                testTable.DefaultCell.Border = Rectangle.NO_BORDER;
                testTable.AddCell(string.Empty);
                pdfDocument.Add(testTable);
                Paragraph noTest = new Paragraph("No Test Information Found".ToString());
                noTest.Alignment = Element.ALIGN_CENTER;
                pdfDocument.Add(noTest);
            }
        }
        private void labOrderDiagnosisCodes(Document pdfDocument, Font componentHeadingFont, Font bodyFont, DSLabOrder dsLabOrder)
        {
            if (dsLabOrder.Tables[dsLabOrder.LabOrderProblem.TableName].Rows.Count > 0)
            {
                Paragraph problems_Heading = new Paragraph("Diagnosis Codes\n".ToString(), componentHeadingFont);
                pdfDocument.Add(problems_Heading);

                PdfPTable problemsTable = new PdfPTable(3);
                float[] widths = new float[] { 3f, 12f, 3f };
                problemsTable.SetWidths(widths);
                problemsTable.TotalWidth = 575f;
                problemsTable.LockedWidth = true;
                problemsTable.HorizontalAlignment = Element.ALIGN_CENTER;
                problemsTable.DefaultCell.Border = Rectangle.NO_BORDER;

                foreach (DSLabOrder.LabOrderProblemRow dr in dsLabOrder.Tables[dsLabOrder.LabOrderProblem.TableName].Rows)
                {
                    var ProblemName = HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrderProblem.ProblemNameColumn.ColumnName]));
                    var ProblemNames1 = ProblemName.Split('-');
                    var ICD10 = string.Empty;

                    int PIndex = GetNthIndex(ProblemName, '-', 1);
                    if (PIndex != -1)
                    {
                        PIndex += 2;
                        var Plen = ProblemName.Length;

                        if (PIndex < Plen)
                            ProblemName = ProblemName.Substring(PIndex);
                        else
                            ProblemName = "";
                    }
                    else
                        ProblemName = "Problem Name not found";

                    if (ProblemNames1.Length > 1)
                        ICD10 = ProblemNames1[0].Trim();

                    problemsTable.AddCell(new Paragraph(ICD10, bodyFont));
                    problemsTable.AddCell(new Paragraph(ProblemName, bodyFont));
                    problemsTable.AddCell(string.Empty);
                }
                pdfDocument.Add(problemsTable);
            }
            else
            {
                Paragraph problems_Heading = new Paragraph("Diagnosis Codes \n".ToString(), componentHeadingFont);
                problems_Heading.SpacingBefore = problems_Heading.SpacingAfter = 5;
                pdfDocument.Add(problems_Heading);
                PdfPTable problemsTable = new PdfPTable(5);
                float[] dwidths = new float[] { 1f, 8f, 8f, 8f, 8f };
                problemsTable.SetWidths(dwidths);
                problemsTable.TotalWidth = 575f;
                problemsTable.LockedWidth = true;
                problemsTable.DefaultCell.Border = Rectangle.NO_BORDER;
                problemsTable.AddCell(string.Empty);
                pdfDocument.Add(problemsTable);
                Paragraph noProblems = new Paragraph("No Diagnosis Codes Found".ToString(), bodyFont);
                noProblems.Alignment = Element.ALIGN_CENTER;
                pdfDocument.Add(noProblems);
            }
        }

        #endregion

        // By Ahsan Nasir
        public int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        private string GetPracticeText(long practiceId)
        {
            DSProfile ds = new DSProfile();
            ds = new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            string PatientHtml = string.Empty;
            if (ds != null)
            {
                if (ds.Tables[ds.Practice.TableName].Rows.Count > 0)
                {
                    DataRow drPractice = ds.Tables[ds.Practice.TableName].Rows[ds.Tables[ds.Practice.TableName].Rows.Count - 1];

                    PatientHtml = string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.DescriptionColumn.ColumnName]), "<br/>");
                    PatientHtml = string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.AddressColumn.ColumnName]), "<br/>");
                    PatientHtml = string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.Address2Column.ColumnName]));
                    PatientHtml = MDVUtility.ToStr(drPractice[ds.Practice.CityColumn.ColumnName]) == string.Empty ? string.Empty : string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.CityColumn.ColumnName]), ", ");
                    PatientHtml = MDVUtility.ToStr(drPractice[ds.Practice.StateColumn.ColumnName]) == string.Empty ? string.Empty : string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.StateColumn.ColumnName]), ", ");
                    PatientHtml = MDVUtility.ToStr(drPractice[ds.Practice.ZIPCodeColumn.ColumnName]) == string.Empty ? string.Empty : string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.ZIPCodeColumn.ColumnName]));
                    PatientHtml = string.Concat(PatientHtml, "<br/>");
                    PatientHtml = string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.PhoneNoColumn.ColumnName]), "<br/>");

                }

            }
            return PatientHtml;
        }
        private string GetFacilityText(long FacilityId)
        {
            DSProfile ds = new DALFacility().LoadFacility(FacilityId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            string FacilityData = string.Empty;
            string Address = string.Empty;
            if (ds != null)
            {
                if (ds.Facility.Rows.Count > 0)
                {
                    FacilityData += "ShortName:" + Convert.ToString(ds.Facility.Rows[0][ds.Facility.ShortNameColumn.ColumnName]) + "<br/>";
                    FacilityData += "Description:" + Convert.ToString(ds.Facility.Rows[0][ds.Facility.DescriptionColumn.ColumnName]) + "<br/>";
                    Address = Convert.ToString(string.IsNullOrEmpty(ds.Facility.Rows[0][ds.Facility.AddressColumn.ColumnName].ToString()) ? "" : ds.Facility.Rows[0][ds.Facility.AddressColumn.ColumnName].ToString() + "\n");
                    Address += Convert.ToString((!string.IsNullOrEmpty(ds.Facility.Rows[0][ds.Facility.CityColumn.ColumnName].ToString()) ? ds.Facility.Rows[0][ds.Facility.CityColumn.ColumnName].ToString() : ""));
                    Address += Convert.ToString((!string.IsNullOrEmpty(ds.Facility.Rows[0][ds.Facility.StateColumn.ColumnName].ToString()) ? ", " + ds.Facility.Rows[0][ds.Facility.StateColumn.ColumnName].ToString() : ""));
                    Address += Convert.ToString((!string.IsNullOrEmpty(ds.Facility.Rows[0][ds.Facility.ZIPCodeColumn.ColumnName].ToString()) ? ", " + ds.Facility.Rows[0][ds.Facility.ZIPCodeColumn.ColumnName].ToString() : ""));
                    FacilityData += "Address:" + Address;
                }
            }
            return FacilityData;
        }
        private string GetProviderText(long providerId)
        {

            DSProfile ds = new DALProvider().LoadProvider(providerId, "", "", "", "", "", "", "", 1, 2000);
            string PatientHtml = string.Empty;
            if (ds != null)
            {
                if (ds.Provider.Rows.Count > 0)
                {
                    DataRow drProvider = ds.Tables[ds.Provider.TableName].Rows[ds.Tables[ds.Provider.TableName].Rows.Count - 1];
                    var providerFullName = MDVUtility.ToStr(drProvider[ds.Provider.LastNameColumn.ColumnName]) + ", "
                        + MDVUtility.ToStr(drProvider[ds.Provider.FirstNameColumn.ColumnName]);
                    var providerNPI = MDVUtility.ToStr(drProvider[ds.Provider.NPIColumn.ColumnName]);
                    var providerAddress = MDVUtility.ToStr(drProvider[ds.Provider.OfficeAddressColumn.ColumnName]) + "\n";
                    providerAddress += drProvider[ds.Provider.CityColumn.ColumnName].ToString() == "" ? "" : drProvider[ds.Provider.CityColumn.ColumnName].ToString() + ", ";
                    providerAddress += drProvider[ds.Provider.StateColumn.ColumnName].ToString() == "" ? "" : drProvider[ds.Provider.StateColumn.ColumnName].ToString() + ", ";
                    providerAddress += drProvider[ds.Provider.ZIPCodeColumn.ColumnName].ToString();
                    OrderingProviderName = providerFullName;
                    OrderingProviderNPI = providerNPI;
                    OrderingProviderAddress = providerAddress;

                    //ms = new MemoryStream(data);
                    // = Image.FromStream(ms);
                    if (Convert.ToBoolean(drProvider[ds.Provider.Is_eSignaturedColumn.ColumnName] == DBNull.Value ? false : drProvider[ds.Provider.Is_eSignaturedColumn.ColumnName]))
                    {
                        ProviderSignatures = (byte[])drProvider[ds.Provider.eSignatureColumn.ColumnName];
                    }

                    var providerOfficeAddress = MDVUtility.ToStr(drProvider[ds.Provider.OfficeAddressColumn.ColumnName]);
                    var providerOfficePhone = MDVUtility.ToStr(drProvider[ds.Provider.PhoneNoColumn.ColumnName]);
                    PatientHtml = string.Concat(PatientHtml, "Provider", "", "<br/>");
                    PatientHtml = string.Concat(PatientHtml, "Provider Name: ", providerFullName, "<br/>");
                    PatientHtml = string.Concat(PatientHtml, "Provider NPI: ", providerNPI, "<br/>");
                    PatientHtml = string.Concat(PatientHtml, "Speciality: ", MDVUtility.ToStr(drProvider["SpecialtyName"]), "<br/>");
                }
            }
            return PatientHtml;
        }
        private string GetPatientText(long patientId)
        {
            DSPatient ds = new DSPatient();
            ds = new DALPatient().FillPatient(patientId, "", "");
            string PatientHtml = string.Empty;
            if (ds != null)
            {
                if (ds.Patients.Rows.Count > 0)
                {
                    PatientHtml = string.Concat("Patient", "<br/>");
                    PatientHtml = string.Concat(MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.LastNameColumn.ColumnName]), ", ", MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.FirstNameColumn.ColumnName]), " ", MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.MIColumn.ColumnName]));
                    PatientHtml = string.Concat(PatientHtml, "<br/>");
                    PatientHtml = string.Concat(PatientHtml, MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.AgeColumn.ColumnName]), " Y, ", MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.GenderColumn.ColumnName]), ", ");
                    PatientHtml = string.Concat(PatientHtml, "DOB: ", Convert.ToDateTime(MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.DOBColumn.ColumnName])).ToString("MM/dd/yyyy"), "<br/>");
                    PatientHtml = string.Concat(PatientHtml, "Patient ID: ", MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.AccountNumberColumn.ColumnName]), "<br/>");
                    if (!string.IsNullOrWhiteSpace(MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.HomePhoneNoColumn.ColumnName])))
                        PatientHtml = string.Concat(PatientHtml, "Ph: ", MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.HomePhoneNoColumn.ColumnName]), "<br/>");
                    PatientHtml = string.Concat(PatientHtml,
                                (ds.Patients.Rows[0][ds.Patients.Address1Column.ColumnName].ToString() == "" ? "" : (MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.Address1Column.ColumnName])).ToString() + "<br/>")
                                + (ds.Patients.Rows[0][ds.Patients.Address2Column.ColumnName].ToString() == "" ? "" : (MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.Address2Column.ColumnName])).ToString() + "<br/>")
                                + (ds.Patients.Rows[0][ds.Patients.CityColumn.ColumnName].ToString() == "" ? "" : (MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.CityColumn.ColumnName])).ToString())
                                + (ds.Patients.Rows[0][ds.Patients.StateColumn.ColumnName].ToString() == "" ? "" : (", " + MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.StateColumn.ColumnName])).ToString())
                                + (ds.Patients.Rows[0][ds.Patients.ZIPCodeColumn.ColumnName].ToString() == "" ? "" : (", " + MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ZIPCodeColumn.ColumnName])).ToString()), "<br/>");

                }
            }
            return PatientHtml;

        }
        #endregion


        #region events
        //Overriden by Ahsan Nasir
        public class MyPdfPageEventHelpPageNo : iTextSharp.text.pdf.PdfPageEventHelper
        {
            Font bodyFont = null;
            Font bodyFontBlue = null;
            string patientName = "";
            string orderNo = "";
            public MyPdfPageEventHelpPageNo(string patientName, Font bodyFont, string orderNo, Font ComponentHeader)
            {
                this.bodyFont = bodyFont;
                this.patientName = patientName;//patientName;
                this.orderNo = orderNo;
                this.bodyFontBlue = ComponentHeader;
            }

            public MyPdfPageEventHelpPageNo()
            {
            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                // base.OnStartPage(writer, document);
                document.Add(new Paragraph("\n" + "     ", bodyFont));

                //PatientName
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Paragraph("Patient Name: ", bodyFontBlue), 20, 750, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Paragraph(patientName, bodyFont), 100, 750, 0);

                //Order No
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Paragraph("Order No: ", bodyFontBlue), 400, 750, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Paragraph(orderNo, bodyFont), 460, 750, 0);
                BaseColor myColor1 = WebColors.GetRGBColor("#66B2FF");
                LineSeparator line2 = new LineSeparator(1f, 100f, myColor1, iTextSharp.text.Element.ALIGN_CENTER, 0);
                document.Add(line2);
                document.Add(new Paragraph("\n" + "     ", bodyFont));
            }
        }
        #endregion
    }
}
