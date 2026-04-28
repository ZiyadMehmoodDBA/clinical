using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.DataAccess.DAL.Admin;
using MDVision.DataAccess.DAL.Patient;
using System.IO;
using iTextSharp.text;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using iTextSharp.text.pdf;
using System.Web;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Model.Clinical.Medical.PatientEducation;

namespace MDVision.Business.BLL
{


    public class BLLPatientEducation
    {

        #region Constructors
        public BLLPatientEducation()
        {
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

        #region CRUD for Patient Education
        public BLObject<DSPatientEducation> LookupClinical_PatientEducation(string DocumentName)
        {
            try
            {
                DSPatientEducation ds = new DSPatientEducation();
                ds = new DALPatientEducation().LookupClinical_PatientEducation(DocumentName);
                return new BLObject<DSPatientEducation>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::LookupClinical_PatientEducation", ex);
                return new BLObject<DSPatientEducation>(null, ex.Message);
            }
        }

        public BLObject<DSPatientEducation> loadClinical_PatientEducation_Obsolete(long PatientID, string DocType, int PageNumber, int RowsPerPage, long NoteId = 0)
        {
            try
            {
                DSPatientEducation ds = new DSPatientEducation();
                ds = new DALPatientEducation().loadClinical_PatientEducation_Obsolete(PatientID, DocType, PageNumber, RowsPerPage, NoteId);
                return new BLObject<DSPatientEducation>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::loadClinical_PatientEducation", ex);
                return new BLObject<DSPatientEducation>(null, ex.Message);
            }
        }

        public List<PatientEducation> loadClinical_PatientEducation(long PatientID, string DocType, int PageNumber, int RowsPerPage, long NoteId = 0)
        {
            try
            {
                List<PatientEducation> patientEducationList = new DALPatientEducation().loadClinical_PatientEducation(PatientID, DocType, PageNumber, RowsPerPage, NoteId);
                return patientEducationList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::loadClinical_PatientEducation", ex);
                throw ex;
            }
        }

        public BLObject<DSPatientEducation> InsertClinical_PatientEducation(DSPatientEducation ds)
        {
            try
            {
                 ds = new DALPatientEducation().InsertClinical_PatientEducation(ds);
                return new BLObject<DSPatientEducation>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::InsertClinical_PatientEducation", ex);
                return new BLObject<DSPatientEducation>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteClinical_PatientEducation(long PatientEducationId, int PatDocID)
        {
            try
            {
                var returnVal = new DALPatientEducation().DeleteClinical_PatientEducation(PatientEducationId, PatDocID);

                return new BLObject<string>(returnVal);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::deletePatientEducation", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        #endregion

        #region 
        public BLObject<DSPatient> loadDataForPdf(long patientId)
        {
            try
            {
                DSPatient dspatient = new DALPatient().FillPatient(patientId, "", "");
                long practiceId = MDVUtility.ToInt64(dspatient.Tables[dspatient.Patients.TableName].Rows[0][dspatient.Patients.PracticeIdColumn.ColumnName]);
                dspatient.Merge(new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

                return new BLObject<DSPatient>(dspatient);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::loadDataForPdf", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<byte[]> previewInfoPdf(byte[] contentInfo , long patientId)
        {
            try
            {
                DSPatient dsPatient = new DALPatient().FillPatient(patientId, "", "");
                long practiceId = MDVUtility.ToInt64(dsPatient.Patients.Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                dsPatient.Merge(new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));
                DSProfile dsProfile = new DSProfile();

                byte[] patientDataInfo = null;
                byte[] fullPdfInfo = null;
                
                using (MemoryStream stream_ = new MemoryStream())
                {
                    Document pdfDocument = new Document(PageSize.LETTER, 20, 20, 20, 20);
                    // Heading Font Style
                    var fontColour = new BaseColor(102, 178, 255);
                    iTextSharp.text.Font patientNameFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, fontColour);
                    iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    iTextSharp.text.Font componentHeadingFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, fontColour);
                    iTextSharp.text.Font componentHeaderFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    iTextSharp.text.Font gridbodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

                    MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Info");
                    pdf.Document.Open();

                    PdfPTable headerTable = new PdfPTable(1);
                    headerTable.TotalWidth = 575f;
                    headerTable.LockedWidth = true;
                    headerTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(@"~\content\images\mdvision-logo.png"));
                    logo.ScalePercent(59f);
                    PdfPCell cell1 = new PdfPCell();
                    cell1.AddElement(logo);
                    cell1.Border = Rectangle.NO_BORDER;
                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    headerTable.AddCell(cell1);

                    pdfDocument.Add(headerTable);

                    #region Patient's Data

                    // Start Append Patient's Data
                    PdfPTable patientTable = new PdfPTable(2);
                    patientTable.TotalWidth = 575f;
                    patientTable.LockedWidth = true;
                    patientTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                      
                    foreach (DSPatient.PatientsRow dr in dsPatient.Patients.Rows)
                    {

                        PdfPTable ageTable = new PdfPTable(1);
                        ageTable.DefaultCell.Border = Rectangle.NO_BORDER;
                       
                        ageTable.AddCell(new Paragraph(dsPatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.ShortNameColumn.ColumnName].ToString(), patientNameFont));
                        ageTable.AddCell(new Paragraph(dsPatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.AddressColumn.ColumnName].ToString() + " " + dsPatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.CityColumn.ColumnName].ToString() + " " + dsPatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.StateColumn.ColumnName].ToString() + " " + dsPatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.ZIPCodeColumn.ColumnName].ToString(), bodyFont));
                        ageTable.AddCell(new Paragraph(dsPatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.PhoneNoColumn.ColumnName].ToString(), bodyFont));
                        ageTable.AddCell(new Paragraph(dsPatient.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.FaxColumn.ColumnName].ToString(), bodyFont));                        

                        PdfPTable emailTable = new PdfPTable(1);
                        emailTable.DefaultCell.Border = Rectangle.NO_BORDER;

                        emailTable.AddCell(new Paragraph("Patient Name: " + MDVUtility.ToStr(dr[dsPatient.Patients.FullNameColumn.ColumnName]), bodyFont));
                        emailTable.AddCell(new Paragraph("DOB: " + MDVUtility.ToStr(dr[dsPatient.Patients.DOBColumn.ColumnName]), bodyFont));
                        emailTable.AddCell(new Paragraph("MRN: " + MDVUtility.ToStr(dr[dsPatient.Patients.MRNumberColumn.ColumnName]), bodyFont));

                        patientTable.AddCell(ageTable);

                        PdfPCell cell = new PdfPCell();
                        cell.AddElement(emailTable);
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.Border = Rectangle.NO_BORDER;
                        patientTable.AddCell(cell);

                        patientTable.DefaultCell.Padding = 4;
                        pdfDocument.Add(patientTable);

                    }
                    // End Append Patient's Data

                    #endregion

                    pdf.Document.Close();
                    pdf.Writer.Close();

                    patientDataInfo = stream_.GetBuffer();
                }
               
                //fullPdfInfo = Combine(patientDataInfo, contentInfo);

                return new BLObject<byte[]>(patientDataInfo);
                
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::previewInfoPdf", ex);
                return new BLObject<byte[]>(null, ex.Message);
            }
        }


        public BLObject<DSPatientEducation> getLatestPatientEducationByPatientId(long PatientId)
        {
            try
            {
                DSPatientEducation ds = new DSPatientEducation();
                ds = new DALPatientEducation().getLatestPatientEducationByPatientId(PatientId);

                return new BLObject<DSPatientEducation>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::getLatestPatientEducationByPatientId", ex);
                return new BLObject<DSPatientEducation>(null, ex.Message);
            }
        }
        public BLObject<DSPatientEducation> getPatientEducationSOAP(Int64 PatientId, string PatEducationId, Int64 NoteId)
        {
            try
            {
                DSPatientEducation ds = new DSPatientEducation();
                ds = new DALPatientEducation().getPatientEducationSOAP(PatientId, PatEducationId, NoteId);

                return new BLObject<DSPatientEducation>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::getLatestPatientEducationByPatientId", ex);
                return new BLObject<DSPatientEducation>(null, ex.Message);
            }
        }
        public BLObject<string> detachPatientEducationFromNotes(string PatientEducationId, long NotesId)
        {
            try
            {
                var msgPatientEducationId = new DALPatientEducation().detachPatientEducationFromNotes(PatientEducationId, NotesId);
                return new BLObject<string>(msgPatientEducationId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::detachPatientEducationFromNotes", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSPatientEducation> attachPatientEducationWithNotes(string PatientEducationId, long NotesId)
        {
            try
            {
                DSPatientEducation ds = new DSPatientEducation();
                ds = new DALPatientEducation().attachPatientEducationWithNotes(PatientEducationId, NotesId);
                return new BLObject<DSPatientEducation>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatientEducation::attachPatientEducationWithNotes", ex);
                return new BLObject<DSPatientEducation>(null, ex.Message);
            }
        }

        private byte[] Combine(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            System.Buffer.BlockCopy(a, 0, c, 0, a.Length);
            System.Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
            return c;
        }

        #endregion

    }
}
