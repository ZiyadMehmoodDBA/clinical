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
using iTextSharp.text.pdf;
using System.Web;
using System.Web.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.html;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Batch
{
    public class Batch_Fax
    {
        //Programmer: Ahsan Nasir


        private BLLAdminProfile BLLAdminProfileObj = null;
        public Batch_Fax()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }

        #region Singleton
        private static Batch_Fax _obj = null;
        public static Batch_Fax Instance()
        {
            if (_obj == null)
                _obj = new Batch_Fax();
            return _obj;
        }

        #endregion

        #region command Handler
        public void CommandHandler(HttpContext context)
        {

            string commandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (commandAction)
            {
                case "PREVIEW_FAX": // Download and return PDF in base64
                    {

                        string fieldsJSON = context.Request["Fax"];

                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                        var json = SearchedfieldsJSON["Status"];
                        BLObject<byte[]> stream = GetFaxStream(json);
                        var response = new
                        {
                            status = true,
                            LabOrderHTML = Convert.ToBase64String(stream.Data),
                        };
                        context.Response.ContentType = "text/plain";
                        var strJSONData = Convert.ToBase64String(stream.Data);
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "GET_COVER":
                    string cdata = context.Request["CoverPageData"];
                    //   b64 = b64.Split(',')[1];
                    //  byte[] bytes = Convert.FromBase64String(b64);

                    byte[] coverpage = createCoverPage(cdata);
                    // var base64 = MergeTwoPDFFiles(coverpage, bytes);
                    var base64 = Convert.ToBase64String(coverpage);
                    context.Response.Write(base64);

                    break;

                case "FAX_ACCESS":  // To read client ID and Client secret for MD Vision from configuration file.
                    string client_id = WebConfigurationManager.AppSettings["client_id"];
                    string client_secret = WebConfigurationManager.AppSettings["client_secret"];
                    var resp = new
                    {
                        client_id = client_id,
                        client_secret = client_secret,
                    };
                    string res = Newtonsoft.Json.JsonConvert.SerializeObject(resp);
                    context.Response.Write(res);
                    break;
                case "LOAD_USER_PROFILES":
                    string userFields = context.Request["UserId"];
                    string strData = LoadUsersFaxesProfiles(userFields);

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(strData);
                    break;
                case "SAVE_CONTACT":
                    string contactData = context.Request["ContactData"];
                    string JSONData = SaveContact(contactData);

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JSONData);
                    break;
                case "DELETE_CONTACT":
                    string contactDelData = context.Request["ContactData"];
                    string JSONDelData = DeleteContact(contactDelData);

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JSONDelData);
                    break;
                case "LOAD_PROVIDER_CONTACTS":
                    string pid = context.Request["ProviderId"];
                    string strProviderContactData = LoadProviderContacts(pid);

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(strProviderContactData);
                    break;
                case "LOAD_FACILITY_CONTACTS":
                    string Fid = context.Request["FacilityId"];
                    string strFacilityContactData = LoadFacilityContacts(Fid);

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(strFacilityContactData);
                    break;
                case "SEARCH_CONTACTS":
                    string data = context.Request["SearchFaxData"];
                    string ContactData = SearchFaxContacts(data);

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(ContactData);
                    break;
                case "SAVE_FAX_DOCUMENT":
                    string docdata = context.Request["FaxDocData"];
                    string faxDocData = saveFaxDocument(docdata);

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(faxDocData);
                    break;

                case "LOAD_FAX_CONFIDENTIALITY":
                    string fax = context.Request["FaxData"];
                    string respfaxDocData = LoadFaxConfidentiality();

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(respfaxDocData);
                    break;
                case "LOAD_FAX_ACCESS":
                    string x = context.Request["FaxData"];
                    string respfaxAccData = GetFaxAccess();

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(respfaxAccData);
                    break;

                default:
                    break;


            }
        }

        #endregion

        #region Fax Functions

        public string MergeTwoPDFFiles(byte[] file1, byte[] file2)
        {
            MemoryStream stream_ = new MemoryStream();
            Document pdfDocument = new Document(PageSize.LETTER, 30, 30, 30, 30);
            PdfCopy copy = new PdfCopy(pdfDocument, stream_);
            pdfDocument.Open();

            // we create a reader for a certain document
            PdfReader readerOne = new PdfReader(file1);
            PdfReader readerTwo = new PdfReader(file2);
            //reader.ConsolidateNamedDestinations();

            copy.SetMergeFields();

            copy.AddDocument(readerOne);
            copy.AddDocument(readerTwo);




            //   writer.Close();
            pdfDocument.Close();
            var base64 = Convert.ToBase64String(stream_.GetBuffer());
            return base64;
        }
        public BLObject<byte[]> GetFaxStream(string url)
        {
            byte[] faxBytes = null;
            try
            {
                WebClient wb = new WebClient();
                wb.Headers["User-Agent"] = "Googlebot/2.1 (+http://www.googlebot.com/bot.html)"; // Use a google agent to get PDF - Fax
                faxBytes = wb.DownloadData(url);

                return new BLObject<byte[]>(faxBytes);
            }
            catch (Exception e)
            {
                return new BLObject<byte[]>(faxBytes);
            }
        }
        private string saveFaxDocument(string FaxData)
        {

            string FaxDocumentPath = "";
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            ser.MaxJsonLength = Int32.MaxValue;
            var data = ser.Deserialize<dynamic>(FaxData);
            if (!string.IsNullOrEmpty(Convert.ToString(data["FileStream"])))
            {

                FaxDocumentPath = ConvertFilestreamintoPdf(data["FileStream"], data["FilePath"]);
            }


            DSDocument dsDocument = new DSDocument();
            DSDocument.FaxDocumentsRow dr = dsDocument.FaxDocuments.NewFaxDocumentsRow();

            if (!string.IsNullOrEmpty(Convert.ToString(data["DocumentId"])))
            {
                dr.DocId = Convert.ToInt64(data["DocumentId"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(data["UserId"])))
            {
                dr.UserId = Convert.ToInt64(data["UserId"]);
            }
            dr.FileType = Convert.ToString(data["FileType"]);
            dr.FilePath = Convert.ToString(data["FilePath"]);

            dr.FileStream = data["FileStream"];
            dr.Pages = Convert.ToInt32(data["Pages"]);
            dr.Comments = Convert.ToString(data["Comments"]);
            dr.IsActive = Convert.ToBoolean(data["IsActive"]);
            dr.FaxId = Convert.ToInt64(data["FaxId"]);
            dr.IsConfidential = Convert.ToBoolean(data["IsConfidential"]);
            dr.AssignedByUserId = MDVSession.Current.AppUserId;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;
            dr.FaxDocumentPath = FaxDocumentPath;
            dsDocument.FaxDocuments.AddFaxDocumentsRow(dr);
            string obj = BLLAdminProfileObj.InsertFaxDocument(ref dsDocument);


            return obj;
        }
        private string ConvertFilestreamintoPdf(string FaxStream, string FaxName)
        {

            try
            {
                string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                string FolderName = WebConfigurationManager.AppSettings["FaxDocumentsFolder"];

                if (Directory.Exists(FilePath))
                {
                    FaxStream = FaxStream.Replace(" ", "+");


                    int mod4 = FaxStream.Length % 4;
                    if (mod4 > 0)
                    {
                        FaxStream += new string('=', 4 - mod4);
                    }
                    byte[] Filestream = null;
                    Filestream = Convert.FromBase64String(FaxStream);
                    File.WriteAllBytes(FilePath + FolderName + FaxName, Filestream);
                    //   File.WriteAllText(FilePath + FaxName, FaxStream);




                    return FolderName + FaxName;
                }
                else
                {
                    // return error message that no path is exist.
                    return "0";


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string GetFaxAccess()
        {
            var viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fax", "View")).ToString();
            bool status = false;
            if (viewPermission == "")
            {
                status = true;
            }
            else
            {
                status = false;
            }
            var resp = new
            {
                status = status

            };
            string res = Newtonsoft.Json.JsonConvert.SerializeObject(resp);
            return res;
        }


        // Default cover page while sending fax
        private byte[] createCoverPage(string covData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var fields = ser.Deserialize<dynamic>(covData);

            var fromName = Convert.ToString(fields["From"]);
            var fromFax = Convert.ToString(fields["FaxNumber"]);
            var subject = Convert.ToString(fields["Subject"]);
            var faxNotes = Convert.ToString(fields["FaxNotes"]);
            faxNotes = faxNotes.Replace(" ", "+");
            int mod4 = faxNotes.Length % 4;
            if (mod4 > 0)
            {
                faxNotes += new string('=', 4 - mod4);
            }
            byte[] data = Convert.FromBase64String(faxNotes);
            string FaxNotes = Encoding.UTF8.GetString(data);

            var CName = Convert.ToString(fields["CompanyName"]);
            var CompanyName = "";
            if (CName != "undefined")
            {
                CompanyName = "Company Name: " + CName + "\n";
            }
            var PNo = Convert.ToString(fields["PhoneNo"]);
            var PhoneNo = "";
            if (PNo != "undefined" || PNo != "")
            {
                PhoneNo = "Phone No: " + PNo + "\n";
            }

            var TimeZone = Convert.ToString(fields["TimeZone"]);





            iTextSharp.text.Font patientNameFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font bodyFontParent = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);



            MemoryStream stream_ = new MemoryStream();


            Document pdfDocument = new Document(PageSize.LETTER, 30, 30, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(pdfDocument, stream_);
            pdfDocument.Open();

            Paragraph Heading = new Paragraph("Fax Transmission: ", patientNameFont);
            pdfDocument.Add(Heading);

            LineSeparator line3 = new LineSeparator(1f, 100f, BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -10);
            pdfDocument.Add(line3);


            PdfPTable detailsTable = new PdfPTable(1);
            float[] notifierHeaderWidths = new float[] { 20f };
            detailsTable.SetWidths(notifierHeaderWidths);
            detailsTable.TotalWidth = 520f;
            detailsTable.LockedWidth = true;
            detailsTable.HorizontalAlignment = Element.ALIGN_CENTER;
            detailsTable.DefaultCell.Border = Rectangle.NO_BORDER;
            detailsTable.SpacingBefore = 15f;
            detailsTable.SpacingAfter = 15f;

            Paragraph cellHeadingOne = new Paragraph("Attention To: ", patientNameFont);
            Paragraph cellHeadingTwo = new Paragraph("From: ", patientNameFont);


            Paragraph fromDetails = new Paragraph("Name: " + fromName + "\n" + "Fax Number: " + fromFax + "\n" + CompanyName + PhoneNo + "TimeZone: " + TimeZone + "\n" + "Date & Time: " + DateTime.Now.ToString(), bodyFont);


            PdfPCell cellOne = new PdfPCell();
            cellOne.HorizontalAlignment = Element.ALIGN_CENTER;
            cellOne.AddElement(cellHeadingOne);
            //cellOne.AddElement(attentionToDetails);
            cellOne.Border = Rectangle.NO_BORDER;


            PdfPCell cellTwo = new PdfPCell();
            cellTwo.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTwo.AddElement(cellHeadingTwo);
            cellTwo.AddElement(fromDetails);
            cellTwo.Border = Rectangle.NO_BORDER;


            //  detailsTable.AddCell(cellOne);
            detailsTable.AddCell(cellTwo);

            pdfDocument.Add(detailsTable);

            pdfDocument.Add(new Paragraph("Subject: " + subject, patientNameFont));
            pdfDocument.Add(line3);


            Paragraph commentHeader = new Paragraph("Comments and Notes:", bodyFontParent);
            commentHeader.SpacingBefore = 10f;
            pdfDocument.Add(commentHeader);

            PdfPTable notesTable = new PdfPTable(1);
            float[] notifierWidths = new float[] { 20f };
            notesTable.SetWidths(notifierHeaderWidths);
            notesTable.TotalWidth = 520f;
            notesTable.LockedWidth = true;
            notesTable.HorizontalAlignment = Element.ALIGN_CENTER;
            notesTable.DefaultCell.Border = Rectangle.NO_BORDER;
            notesTable.SpacingBefore = 15f;
            notesTable.SpacingAfter = 15f;

            PdfPCell cellnotes = new PdfPCell();
            cellnotes.Border = Rectangle.NO_BORDER;
            using (StringReader sr = new StringReader(FaxNotes))
            {
                List<IElement> elements = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(sr, null);
                foreach (IElement e in elements)
                {
                    //Add those elements to the paragraph
                    cellnotes.AddElement(e);
                }
            }

            notesTable.AddCell(cellnotes);
            pdfDocument.Add(notesTable);



            pdfDocument.Close();
            writer.Close();




            return stream_.ToArray();
        }
        private string LoadUsersFaxesProfiles(string UserData)
        {
            DSProfile ds = null;
            long UserId = 0;

            UserId = Convert.ToInt64(UserData);
            BLObject<DSProfile> obj = BLLAdminProfileObj.LoadUsersFaxProfiles(UserId, 1, 1000);

            if (obj.Data != null)
            {
                ds = obj.Data;
                if ((ds.Tables[ds.FacilityFaxSettingsUsers.TableName].Rows.Count > 0) || (ds.Tables[ds.ProviderFaxSettingsUsers.TableName].Rows.Count > 0))
                {

                    var ProviderRows = ds.Tables[ds.FacilityFaxSettingsUsers.TableName];

                    var dataRows = MDVUtility.JSON_DataTable(ProviderRows);

                    var FacilityRows = ds.Tables[ds.ProviderFaxSettingsUsers.TableName];

                    var dataRowsProviders = MDVUtility.JSON_DataTable(FacilityRows);





                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        FaxUsersFacilities = dataRows,
                        FaxUsersProviders = dataRowsProviders

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                return "";
            }
            return "";


        }
        private string LoadFaxConfidentiality()
        {
            var pagenumber = 1;
            var rowsppage = 1000;
            DSDocument ds = null;
            BLObject<DSDocument> obj = BLLAdminProfileObj.LoadFaxConfidentiality(pagenumber, rowsppage);
            if (obj.Data != null)
            {
                ds = obj.Data;
                if ((ds.Tables[ds.FaxDocuments.TableName].Rows.Count > 0))
                {

                    var Rows = ds.Tables[ds.FaxDocuments.TableName];
                    var dataRows = MDVUtility.JSON_DataTable(Rows);

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        FaxDocument = dataRows,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                return "";
            }
            return "";


        }
        private string SaveContact(string ContactData)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var data = ser.Deserialize<dynamic>(ContactData);

                DSProfile dsProfile = new DSProfile();
                string ProviderOrFacility = "";

                if (Convert.ToString(data["Type"]) == "Provider") // Save Contact Against Provider
                {
                    ProviderOrFacility = "Provider";
                    DSProfile.ProviderFaxContactsRow dr = dsProfile.ProviderFaxContacts.NewProviderFaxContactsRow();

                    if (!string.IsNullOrEmpty(data["Id"]))
                        dr.ProviderId = Convert.ToInt64(data["Id"]);

                    dr.ContactName = Convert.ToString(data["ContactName"]);
                    dr.FaxNumber = Convert.ToString(data["FaxNumber"]);
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    dsProfile.ProviderFaxContacts.AddProviderFaxContactsRow(dr);
                }
                else       // Save Against Facility
                {
                    DSProfile.FacilityFaxContactsRow dr = dsProfile.FacilityFaxContacts.NewFacilityFaxContactsRow();
                    ProviderOrFacility = "Facility";
                    if (!string.IsNullOrEmpty(data["Id"]))
                        dr.FacilityId = Convert.ToInt64(data["Id"]);

                    dr.ContactName = Convert.ToString(data["ContactName"]);
                    dr.FaxNumber = Convert.ToString(data["FaxNumber"]);
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsProfile.FacilityFaxContacts.AddFacilityFaxContactsRow(dr);
                }
                BLObject<DSProfile> obj = BLLAdminProfileObj.InsertProviderContact(ref dsProfile, ProviderOrFacility);

                if (obj.Data != null)
                {
                    if (ProviderOrFacility == "Provider")
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ContactId = dsProfile.Tables[dsProfile.ProviderFaxContacts.TableName].Rows[0][dsProfile.ProviderFaxContacts.ContactIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ContactId = dsProfile.Tables[dsProfile.FacilityFaxContacts.TableName].Rows[0][dsProfile.FacilityFaxContacts.ContactIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string DeleteContact(string ContactData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(ContactData);

            DSProfile dsProfile = new DSProfile();


            if (Convert.ToString(data["Type"]) == "Provider") // Save Contact Against Provider
            {
                long ProviderId = Convert.ToInt64(data["ProviderId"]);
                long ContactId = Convert.ToInt64(data["ContactId"]);

                return BLLAdminProfileObj.DeleteProviderContacts(ProviderId, ContactId);

            }
            else       // Save Against Facility
            {

                long FacilityId = Convert.ToInt64(data["FacilityId"]);
                long ContactId = Convert.ToInt64(data["ContactId"]);

                return BLLAdminProfileObj.DeleteFacilityContacts(FacilityId, ContactId);

            }
        }
        private string LoadProviderContacts(string UserData)
        {
            DSProfile ds = null;
            long ProviderId = 0;

            ProviderId = Convert.ToInt64(UserData);

            BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProviderFaxContacts(ProviderId, 1, 1000);
            if (obj.Data != null)
            {
                ds = obj.Data;
                if ((ds.Tables[ds.ProviderFaxContacts.TableName].Rows.Count > 0))
                {

                    var ProviderRows = ds.Tables[ds.ProviderFaxContacts.TableName];
                    var dataRows = MDVUtility.JSON_DataTable(ProviderRows);

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        FaxProviderContacts = dataRows,


                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }


            }
            else
            {
                var response = new
                {
                    status = false,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }



        }
        private string LoadFacilityContacts(string UserData)
        {
            DSProfile ds = null;
            long FacilityId = 0;

            FacilityId = Convert.ToInt64(UserData);

            BLObject<DSProfile> obj = BLLAdminProfileObj.LoadFacilityFaxContacts(FacilityId, 1, 1000);
            if (obj.Data != null)
            {
                ds = obj.Data;
                if ((ds.Tables[ds.FacilityFaxContacts.TableName].Rows.Count > 0))
                {

                    var FacilityRows = ds.Tables[ds.FacilityFaxContacts.TableName];
                    var dataRows = MDVUtility.JSON_DataTable(FacilityRows);

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        FaxFacilityContacts = dataRows,


                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            else
            {
                var response = new
                {
                    status = false,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        private string SearchFaxContacts(string ContactData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(ContactData);

            bool isProvider;
            long FacilityId = 0;
            long ProviderId = 0;
            if (Convert.ToString(data["Type"]) == "Provider")
            {
                isProvider = true;
                ProviderId = Convert.ToInt64(data["ProviderId"]);
            }
            else
            {
                isProvider = false;
                FacilityId = Convert.ToInt64(data["FacilityId"]);
            }

            string contactName = Convert.ToString(data["ContactName"]);

            DSProfile ds = null;

            BLObject<DSProfile> obj = BLLAdminProfileObj.SearchFaxContacts(isProvider, ProviderId, FacilityId, contactName, 1, 10);
            if (obj.Data != null)
            {
                ds = obj.Data;
                if ((ds.Tables[ds.ProviderFaxContacts.TableName].Rows.Count > 0))
                {

                    var ProviderRows = ds.Tables[ds.ProviderFaxContacts.TableName];
                    var dataRows = MDVUtility.JSON_DataTable(ProviderRows);

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        FaxProviderContacts = dataRows,


                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else if ((ds.Tables[ds.FacilityFaxContacts.TableName].Rows.Count > 0))
                {

                    var FacilityRows = ds.Tables[ds.FacilityFaxContacts.TableName];
                    var dataRows = MDVUtility.JSON_DataTable(FacilityRows);

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        FaxFacilityContacts = dataRows,


                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            else
            {
                var response = new
                {
                    status = false,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }



        }
        #endregion
    }
}