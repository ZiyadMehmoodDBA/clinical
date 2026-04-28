using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Web.Configuration;
using System.Data;
using MDVision.IEHR.Common;
using System.IO;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.DataAccess.DAL.Document;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using iTextSharp.text;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.parser;
using HtmlAgilityPack;
using MDVision.IEHR.Model.PDFHelpers;
using MDVision.Model.Patient;
using MDVision.Model.Common;
using MDVision.Model.Document;
using MDVision.Model.Clinical.Notes.Notes;
using Newtonsoft.Json;
using MDVision.Model.Clinical.Notes;

namespace MDVision.IEHR.Controls.Patient.Document
{
    public class Patient_Document
    {

        private BLLClinical BLLClinicalObj = null;
        private BLLPatient BLLPatientObj = null;
        private BLLDocument BLLDocumentObj = null;
        private BLLMessage BLLMessageObj = null;
        public Patient_Document()
        {
            BLLPatientObj = new BLLPatient();
            BLLClinicalObj = new BLLClinical();
            BLLDocumentObj = new BLLDocument();
            BLLMessageObj = new BLLMessage();
        }
        #region Singleton
        private static Patient_Document _obj = null;
        public static Patient_Document Instance()
        {
            if (_obj == null)
                _obj = new Patient_Document();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Load all the LoadPatientDocument for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The Document identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string SearchPatientDocument(string fieldsJSON, Int64 PatientID, Int32 PageNumber, Int32 RowsPerPage, string IsReviewed, long NoteId, string PatDocIds = "", string bFileStream = "0", string AdvancePaymentId = null)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = null;

                DSPatient dsReviewedDoc = null;
                BLObject<DSPatient> objReviewedDoc = null;

                Int64 advancePaymentId = 0;

                if (AdvancePaymentId != null)
                    advancePaymentId = MDVUtility.ToInt64(AdvancePaymentId);


                if (SearchedfieldsJSON == null || SearchedfieldsJSON.Count == 0)
                    obj = BLLPatientObj.searchPatientDocument(PatDocIds, PatientID, "", null, null, null, null, "", 0, "", 0, bFileStream, advancePaymentId, 1, 1000, 0, null, null, NoteId);
                else
                {
                    DateTime? FromDOS = null;
                    DateTime? ToDOS = null;
                    DateTime? FromEntryDate = null;
                    DateTime? ToEntryDate = null;
                    DateTime? FromExpiry = null;
                    DateTime? ToExpiry = null;
                    string AccountNumber = null;
                    string PatientLastName = null;
                    string PatientFirstName = null;
                    Int32 AssignedToReviewedID = 0;
                    string EnteredBy = "";
                    string DocPriority = "";
                    Int64 tagId = 0;
                    int DocumentId = 0;
                    if (SearchedfieldsJSON.ContainsKey("dtpFromDOS"))
                        FromDOS = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpFromDOS"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpFromDOS"]) : null;

                    if (SearchedfieldsJSON.ContainsKey("dtpToDOS"))
                        ToDOS = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpToDOS"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpToDOS"]) : null;

                    if (SearchedfieldsJSON.ContainsKey("dtpFromEntry"))
                        FromEntryDate = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpFromEntry"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpFromEntry"]) : null;

                    if (SearchedfieldsJSON.ContainsKey("dtpToEntry"))
                        ToEntryDate = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpToEntry"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpToEntry"]) : null;

                    //if (SearchedfieldsJSON.ContainsKey("txtAccountNumber"))
                    //    AccountNumber = SearchedfieldsJSON["txtAccountNumber"];

                    //if (SearchedfieldsJSON.ContainsKey("txtPatientLastName"))
                    //    PatientLastName = SearchedfieldsJSON["txtPatientLastName"];

                    //if (SearchedfieldsJSON.ContainsKey("txtPatientFirstName"))
                    //    PatientFirstName = SearchedfieldsJSON["txtPatientFirstName"];

                    if (SearchedfieldsJSON.ContainsKey("ddlAssignedtoReview"))
                        AssignedToReviewedID = MDVUtility.ToInt32(SearchedfieldsJSON["ddlAssignedtoReview"]);

                    if (SearchedfieldsJSON.ContainsKey("ddlEnteredBy_text"))
                    {
                        if (SearchedfieldsJSON["ddlEnteredBy_text"] == "- Select -")
                        {
                            EnteredBy = "";
                        }
                        else
                        {
                            EnteredBy = SearchedfieldsJSON["ddlEnteredBy_text"];
                        }
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlDocumentPriority"))
                    {
                        DocPriority = SearchedfieldsJSON["ddlDocumentPriority"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("hfDocumentId"))
                    {
                        DocumentId = MDVUtility.ToInt32(SearchedfieldsJSON["hfDocumentId"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("dtpFromExpiry"))
                        FromExpiry = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpFromExpiry"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpFromExpiry"]) : null;

                    if (SearchedfieldsJSON.ContainsKey("dtpToExpiry"))
                        ToExpiry = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpToExpiry"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpToExpiry"]) : null;
                    if (SearchedfieldsJSON.ContainsKey("txtTagName") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTagName"]))
                        tagId = MDVUtility.ToInt64(SearchedfieldsJSON["hfDocumentTagId"]);

                    if (IsReviewed == "")
                    {
                        obj = BLLPatientObj.searchPatientDocument("", PatientID, AccountNumber, FromDOS, ToDOS, FromEntryDate, ToEntryDate, EnteredBy, MDVUtility.ToInt64(AssignedToReviewedID), "0", DocumentId, "0", advancePaymentId, PageNumber, RowsPerPage, AssignedToReviewedID, PatientLastName, PatientFirstName, NoteId, DocPriority, FromExpiry, ToExpiry, tagId, SearchedfieldsJSON.ContainsKey("chkRecentDocuments") ? MDVUtility.ToBool(SearchedfieldsJSON["chkRecentDocuments"]) : false);
                        objReviewedDoc = BLLPatientObj.searchPatientDocument("", PatientID, AccountNumber, FromDOS, ToDOS, FromEntryDate, ToEntryDate, EnteredBy, MDVUtility.ToInt64(null), "1", DocumentId, "0", advancePaymentId, PageNumber, RowsPerPage, AssignedToReviewedID, PatientLastName, PatientFirstName, NoteId, DocPriority, FromExpiry, ToExpiry, tagId, SearchedfieldsJSON.ContainsKey("chkRecentDocuments") ? MDVUtility.ToBool(SearchedfieldsJSON["chkRecentDocuments"]) : false);
                    }

                    else
                    {
                        if (IsReviewed == "0")
                            obj = BLLPatientObj.searchPatientDocument("", PatientID, AccountNumber, FromDOS, ToDOS, FromEntryDate, ToEntryDate, EnteredBy, MDVUtility.ToInt64(AssignedToReviewedID), "0", DocumentId, "0", advancePaymentId, PageNumber, RowsPerPage, AssignedToReviewedID, PatientLastName, PatientFirstName, NoteId, DocPriority, FromExpiry, ToExpiry, tagId, SearchedfieldsJSON.ContainsKey("chkRecentDocuments") ? MDVUtility.ToBool(SearchedfieldsJSON["chkRecentDocuments"]) : false);
                        else if (IsReviewed == "1")
                            objReviewedDoc = BLLPatientObj.searchPatientDocument("", PatientID, AccountNumber, FromDOS, ToDOS, FromEntryDate, ToEntryDate, EnteredBy, MDVUtility.ToInt64(AssignedToReviewedID), "1", DocumentId, "0", advancePaymentId, PageNumber, RowsPerPage, AssignedToReviewedID, PatientLastName, PatientFirstName, NoteId, DocPriority, FromExpiry, ToExpiry, tagId, SearchedfieldsJSON.ContainsKey("chkRecentDocuments") ? MDVUtility.ToBool(SearchedfieldsJSON["chkRecentDocuments"]) : false);
                    }

                }
                if (obj != null)
                {
                    dsPatient = obj.Data;
                }
                if (objReviewedDoc != null)
                {
                    dsReviewedDoc = objReviewedDoc.Data;
                }
                if (dsPatient != null && dsReviewedDoc != null)
                {
                    if (dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count > 0 || dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows.Count > 0)
                    {
                        //foreach (DataRow dr in dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows)
                        //{
                        //    byte[] byteArr = dr["FileStream"] as byte[];
                        //    if (byteArr != null)
                        //    {
                        //        string strBase64 = Convert.ToBase64String(byteArr);
                        //        // Add a New Column to Store the Base64 String
                        //        if (!dr.Table.Columns.Contains("Base64FileStream"))
                        //        {

                        //            dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                        //        }
                        //        dr["Base64FileStream"] = strBase64;
                        //    }
                        //}
                        //foreach (DataRow dr in dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows)
                        //{
                        //    byte[] byteArr = dr["FileStream"] as byte[];
                        //    if (byteArr != null)
                        //    {
                        //        string strBase64 = Convert.ToBase64String(byteArr);
                        //        // Add a New Column to Store the Base64 String
                        //        if (!dr.Table.Columns.Contains("Base64FileStream"))
                        //        {

                        //            dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                        //        }
                        //        dr["Base64FileStream"] = strBase64;
                        //    }
                        //}
                        if (dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count > dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows.Count)
                        {
                            if (dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows.Count == 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.PendingColumn.ColumnName],
                                    ReviewedCount = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else if (dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count != 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.PendingColumn.ColumnName],
                                    ReviewedCount = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.PendingColumn.ColumnName],
                                    ReviewedCount = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }

                        }
                        else if (dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count < dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows.Count)
                        {
                            if (dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count == 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.RecordCountColumn.ColumnName],
                                    PendingCount = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.PendingColumn.ColumnName],
                                    ReviewedCount = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else if (dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count != 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.PendingColumn.ColumnName],
                                    ReviewedCount = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocument.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.PendingColumn.ColumnName],
                                    ReviewedCount = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                DocumentCount = dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count,
                                iTotalDisplayRecords = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.RecordCountColumn.ColumnName],
                                PendingCount = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.PendingColumn.ColumnName],
                                ReviewedCount = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.ReviewedColumn.ColumnName],
                                DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName]),
                                ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else if (dsReviewedDoc == null)
                {
                    if (dsPatient != null && dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count > 0)
                    {
                        //foreach (DataRow dr in dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows)
                        //{
                        //    byte[] byteArr = dr["FileStream"] as byte[];
                        //    if (byteArr != null)
                        //    {
                        //        string strBase64 = Convert.ToBase64String(byteArr);
                        //        // Add a New Column to Store the Base64 String
                        //        if (!dr.Table.Columns.Contains("Base64FileStream"))
                        //        {

                        //            dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                        //        }
                        //        dr["Base64FileStream"] = strBase64;
                        //    }
                        //}
                        var response = new
                        {
                            status = true,
                            DocumentCount = dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.RecordCountColumn.ColumnName],
                            PendingCount = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.PendingColumn.ColumnName],
                            ReviewedCount = dsPatient.PatientDocumentSearch.Rows[0][dsPatient.PatientDocumentSearch.ReviewedColumn.ColumnName],
                            DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName]),
                            ReviewedDocumentLoad_JSON = "",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else if (dsPatient == null)
                {
                    if (dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows.Count > 0)
                    {
                        //foreach (DataRow dr in dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows)
                        //{
                        //    byte[] byteArr = dr["FileStream"] as byte[];
                        //    if (byteArr != null)
                        //    {
                        //        string strBase64 = Convert.ToBase64String(byteArr);
                        //        // Add a New Column to Store the Base64 String
                        //        if (!dr.Table.Columns.Contains("Base64FileStream"))
                        //        {

                        //            dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                        //        }
                        //        dr["Base64FileStream"] = strBase64;
                        //    }
                        //}
                        var response = new
                        {
                            status = true,
                            DocumentCount = dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName].Rows.Count,
                            iTotalDisplayRecords = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.RecordCountColumn.ColumnName],
                            PendingCount = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.PendingColumn.ColumnName],
                            ReviewedCount = dsReviewedDoc.PatientDocumentSearch.Rows[0][dsReviewedDoc.PatientDocumentSearch.ReviewedColumn.ColumnName],
                            DocumentLoad_JSON = "",
                            ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocumentSearch.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        DocumentCount = 0,
                        Message = "Record not found."
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }




        private string GetPatientDocument(string fieldsJSON, Int64 PatientId)
        {
            DSDocumentLookup dsDocuemnt = new DSDocumentLookup();
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                DateTime? FromDOS = null;
                DateTime? ToDOS = null;
                DateTime? FromEntryDate = null;
                DateTime? ToEntryDate = null;
                DateTime? FromExpiry = null;
                DateTime? ToExpiry = null;
                string AccountNumber = null;
                string PatientLastName = null;
                string PatientFirstName = null;
                Int32 AssignedToReviewedID = 0;
                string EnteredBy = "";
                string DocPriority = "";
                Int64 tagId = 0;
                bool IsrecentCheck = false;

                if (SearchedfieldsJSON.ContainsKey("dtpFromDOS"))
                    FromDOS = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpFromDOS"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpFromDOS"]) : null;

                if (SearchedfieldsJSON.ContainsKey("dtpToDOS"))
                    ToDOS = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpToDOS"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpToDOS"]) : null;

                if (SearchedfieldsJSON.ContainsKey("dtpFromEntry"))
                    FromEntryDate = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpFromEntry"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpFromEntry"]) : null;

                if (SearchedfieldsJSON.ContainsKey("dtpToEntry"))
                    ToEntryDate = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpToEntry"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpToEntry"]) : null;

                //if (SearchedfieldsJSON.ContainsKey("txtAccountNumber") && SearchedfieldsJSON["txtAccountNumber"] !="")
                //    AccountNumber = SearchedfieldsJSON["txtAccountNumber"];

                //if (SearchedfieldsJSON.ContainsKey("txtPatientLastName"))
                //    PatientLastName = SearchedfieldsJSON["txtPatientLastName"];

                //if (SearchedfieldsJSON.ContainsKey("txtPatientFirstName"))
                //    PatientFirstName = SearchedfieldsJSON["txtPatientFirstName"];

                if (SearchedfieldsJSON.ContainsKey("ddlAssignedtoReview"))
                    AssignedToReviewedID = MDVUtility.ToInt32(SearchedfieldsJSON["ddlAssignedtoReview"]);

                if (SearchedfieldsJSON.ContainsKey("ddlEnteredBy_text"))
                {
                    if (SearchedfieldsJSON["ddlEnteredBy_text"] == "- Select -")
                    {
                        EnteredBy = "";
                    }
                    else
                    {
                        EnteredBy = SearchedfieldsJSON["ddlEnteredBy_text"];
                    }
                }
                if (SearchedfieldsJSON.ContainsKey("ddlDocumentPriority"))
                {
                    DocPriority = SearchedfieldsJSON["ddlDocumentPriority"];
                }
                if (SearchedfieldsJSON.ContainsKey("dtpFromExpiry"))
                    FromExpiry = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpFromExpiry"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpFromExpiry"]) : null;

                if (SearchedfieldsJSON.ContainsKey("dtpToExpiry"))
                    ToExpiry = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpToExpiry"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpToExpiry"]) : null;
                if (SearchedfieldsJSON.ContainsKey("txtTagName") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTagName"]))
                    tagId = MDVUtility.ToInt64(SearchedfieldsJSON["hfDocumentTagId"]);
                IsrecentCheck = SearchedfieldsJSON.ContainsKey("chkRecentDocuments") ? MDVUtility.ToBool(SearchedfieldsJSON["chkRecentDocuments"]) : false;
                BLObject<DSDocumentLookup> objDocument = new BLLDocument().GetPatientDocument(PatientId, FromDOS, ToDOS, FromEntryDate, ToEntryDate, AccountNumber, AssignedToReviewedID,
                    EnteredBy, DocPriority, FromExpiry, ToExpiry, tagId, IsrecentCheck);
                dsDocuemnt = objDocument.Data;
                var response = new
                {
                    status = true,
                    Folders_JSON = MDVUtility.JSON_DataTable(dsDocuemnt.Tables[dsDocuemnt.Documents.TableName]),
                    FolderChild_JSON = MDVUtility.JSON_DataTable(dsDocuemnt.Tables[dsDocuemnt.FolderDocument.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        private string searchPatientLinkedDocument(Int64 MedicalParentDocId)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = null;
                obj = BLLPatientObj.searchPatientLinkedDocument(MedicalParentDocId);

                if (obj != null)
                {
                    dsPatient = obj.Data;
                }
                if (dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        DocumentLoad_JSON = "[]",
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string GetAudioFile(int VisitId)
        {
            try
            {
                string strFilePath = string.Empty;
                string ServerFilePath = string.Empty;
                var modelNote = BLLPatientObj.GetAudioFile(VisitId);
                if (modelNote != null)
                {
                    if (!string.IsNullOrWhiteSpace(modelNote.Url))
                    {
                        byte[] byteArr = null;
                        ServerFilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        strFilePath = ServerFilePath + modelNote.Url;
                        if (File.Exists(strFilePath))
                        {
                            using (var stream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    byteArr = reader.ReadBytes((int)stream.Length);
                                }
                            }
                        }
                        var keyValues = new Dictionary<string, string>
                          {
                              { "FileType", MDVUtility.ToStr(modelNote.FileType)},
                              { "Base64FileStream", byteArr != null ? Convert.ToBase64String(byteArr) : string.Empty },
                          };
                        var response = new
                        {
                            status = true,

                            FileStream = keyValues,
                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        return js.Serialize(response);
                    }
                    else
                    {
                        var keyValues = new Dictionary<string, string>
                          {
                              { "FileType", MDVUtility.ToStr(modelNote.FileType)},
                              { "Base64FileStream", modelNote.FileStream != null ? Convert.ToBase64String(modelNote.FileStream) : string.Empty },
                          };
                        var response = new
                        {
                            status = true,

                            FileStream = keyValues,
                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        return js.Serialize(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        AudioFileCount = 0,
                        Message = Common.AppPrivileges.No_Record_Message,
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Saves the Patient Document.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PatientID">The Patient Identifier.</param>
        /// <param name="files">HttpFileCollection.</param>
        /// <returns></returns>
        public string SavePatientDocument(string fieldsJSON, Int64 PatientID, HttpFileCollection files, string strBase64 = null, string fileType = null, string advancePaymentId = null, string FileName = "", string refModuleName = "", Int64 TransitionId = 0, byte[] docByteArray = null, long OrderSetReferralId = 0, long VisitId = 0, bool isReviewed = false)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                //if (PatientID > -1) //Changed by Arslan for Patient Education Flow
                //{
                DSPatient dsDocument = new DSPatient();
                DSMessage dsMessage = new DSMessage();

                if (docByteArray != null && docByteArray.Length > 0)
                {
                    DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                    dr.PatientId = PatientID;
                    bool isValid = false;
                    if (refModuleName.ToLower().Trim() == "lab order")
                    {
                        refModuleName = "Lab Orders";
                        DSLabOrder dsLabOrder = null, dsLabProblems = null;
                        BLObject<DSLabOrder> objLabOrder = BLLClinicalObj.LoadLabOrder(TransitionId, PatientID, 0, "1", "15", "", "", 0, "", "", "", 0, "0", "");
                        dsLabOrder = objLabOrder.Data;
                        if (dsLabOrder.LabOrder.Rows.Count > 0)
                        {
                            isValid = true;
                            dr.DOS = MDVUtility.ToDateTime(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.OrderDateColumn]);
                            dr.CreatedOn = MDVUtility.ToDateTime(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.OrderDateColumn]);
                            dr.ModifiedOn = MDVUtility.ToDateTime(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.OrderDateColumn]);
                            string test = MDVUtility.ToStr(dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.TestColumn]);
                            if (test.IndexOf("|") > -1)
                            {
                                test = test.Split('|')[0];

                            }
                            
                            string dt = DateTime.Now.ToShortDateString();
                            string tm = DateTime.Now.ToShortTimeString();
                            string fName = dt + " " + tm.Replace(" ", "");
                            fName = fName.Replace("/", "-").Replace(":", "");

                            var mnth = fName.Split('-')[0];
                            var day = fName.Split('-')[1];
                            var year = fName.Split('-')[2];
                            mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                            day = day.Length == 1 ? "0" + day : day;
                            var mnt = year.Split(' ')[1];
                            year = year.Split(' ')[0];
                            mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                            //fName = mnth + "-" + day + "-" + year + " " + mnt + "_" + test + ".pdf";
                            fName = mnth + "." + day + "." + year + "_" + test + ".pdf";

                            dr.FilePath = fName;
                            FileName = fName;
                            FileName = FileName.Replace('/', '_')
                                        .Replace(':', '_')
                                        .Replace('*', '_')
                                        .Replace('?', '_')
                                        .Replace('"', '_')
                                        .Replace('<', '_')
                                        .Replace('>', '_')
                                        .Replace(@"\", "_");

                        }

                    }
                    else if (refModuleName.ToLower().Trim() == "lab result" || refModuleName.ToLower().Trim() == "psa" || refModuleName.ToLower().Trim() == "urine")
                    {
                        if (refModuleName.ToLower().Trim() == "psa")
                        {
                            refModuleName = "PSA";
                        }
                        else if (refModuleName.ToLower().Trim() == "urine")
                        {
                            refModuleName = "Urine Results";
                        }
                        else
                        {
                            refModuleName = "Lab Results";
                        }
                        DSLabResult dsLabResult = new DSLabResult();
                        BLObject<DSLabResult> objResult = BLLClinicalObj.LoadLabResult(TransitionId, 0, "1", "15", "", "", 0, "", "", "", "", 0, PatientID);
                        dsLabResult = objResult.Data;
                        if (dsLabResult.LabOrderResult.Rows.Count > 0)
                        {
                            isValid = true;
                            dr.DOS = MDVUtility.ToDateTime(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.ModifiedOnColumn]);
                            dr.CreatedOn = MDVUtility.ToDateTime(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.ModifiedOnColumn]);
                            dr.ModifiedOn = MDVUtility.ToDateTime(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.ModifiedOnColumn]);
                            string test = MDVUtility.ToStr(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.LabOrderDocNameColumn]);
                            //EMR-7396
                            if (test.IndexOf("||") > -1)
                            {
                                test = test.Split(new String[]{"||"}, StringSplitOptions.None)[0];

                            }
                            string dt = DateTime.Now.ToShortDateString();
                            string tm = DateTime.Now.ToShortTimeString();
                            string fName = dt + " " + tm.Replace(" ", "");
                            fName = fName.Replace("/", "-").Replace(":", "");

                            var mnth = fName.Split('-')[0];
                            var day = fName.Split('-')[1];
                            var year = fName.Split('-')[2];
                            mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                            day = day.Length == 1 ? "0" + day : day;
                            var mnt = year.Split(' ')[1];
                            year = year.Split(' ')[0];
                            mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                            //fName = mnth + "-" + day + "-" + year + " " + mnt + "_" + test + ".pdf";
                            fName = mnth + "." + day + "." + year + "_" + test + ".pdf";

                            dr.FilePath = fName;

                            FileName = fName;

                            FileName = FileName.Replace('/', '_')
                                          .Replace(':', '_')
                                          .Replace('*', '_')
                                          .Replace('?', '_')
                                          .Replace('"', '_')
                                          .Replace('<', '_')
                                          .Replace('>', '_')
                                          .Replace(@"\", "_");
                            if (isReviewed)
                            {
                                dr.ReviewedById = MDVUtility.ToLong(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.ReviewedByIdColumn]);
                                dr.ReviewDate = DateTime.Now;
                            }

                        }
                    }
                    //dr.Documentid = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
                    //{
                    //    dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                    //    dr.ViewBy = MDVUtility.ToStr(SearchedfieldsJSON["ddlAssignUserto_text"]);
                    //}

                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]))
                    //{
                    //    string dtpDOS = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);

                    //    if (!string.IsNullOrEmpty(dtpDOS))
                    //        dr.DOS = MDVUtility.ToDateTime(dtpDOS);
                    //}
                    if (isValid == true)
                    {
                        //dr.FileStream = docByteArray.Length > 0 ? docByteArray : null;
                        dr.Url = CommonFunc.SaveDocumentToFolder(null, "Lab Orders", "Lab Order", PatientID, FileName, docByteArray);
                        dr.FileType = "application/pdf";

                        dr.Pages = MDVUtility.getPdfPagesCount(docByteArray);

                        dr.IsActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.ModifiedOn = DateTime.Now;

                        if (!string.IsNullOrEmpty(refModuleName))
                        {
                            dr.RefModuleName = refModuleName;
                        }
                        else
                        {
                            dr[dsDocument.PatientDocument.RefModuleNameColumn] = DBNull.Value;
                        }

                        if (TransitionId > 0)
                        {
                            dr.TransitionId = TransitionId;
                        }
                        else
                        {
                            dr[dsDocument.PatientDocument.TransitionIdColumn] = DBNull.Value;
                        }

                        if (OrderSetReferralId > 0)
                        {
                            dr.OrderSetReferralId = OrderSetReferralId;
                        }
                        else
                        {
                            dr[dsDocument.PatientDocument.OrderSetReferralIdColumn] = DBNull.Value;
                        }

                        dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                    }

                }
                else if (strBase64 == null)
                {
                    foreach (string name in files)
                    {
                        HttpPostedFile file = files[name];
                        DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                        dr.PatientId = PatientID;
                        dr.Documentid = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);

                        if (VisitId > 0)
                        { // Visit Id directly sent to cover Phone encounter audio file scenerios
                            dr.VisitId = Convert.ToInt32(VisitId);
                        }



                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
                        {
                            dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                            dr.ViewBy = MDVUtility.ToStr(SearchedfieldsJSON["ddlAssignUserto_text"]);
                        }

                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                        //    dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]))
                        {
                            dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseId"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                            dr.VisitId = MDVUtility.ToInt64(SearchedfieldsJSON["hfVisitId"]);


                        string dtpDOS = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);

                        if (!string.IsNullOrEmpty(dtpDOS))
                        {
                            dr.DOS = MDVUtility.ToDateTime(dtpDOS);
                        }

                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                        //    dr.ClaimId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaim"]);
                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCase"]))
                        //    dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlCase"]);

                        byte[] currentFileStream = new byte[file.ContentLength];
                        int isRead = file.InputStream.Read(currentFileStream, 0, file.ContentLength);

                        dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                        dr.FileType = file.ContentType;// MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);
                        dr.FilePath = file.FileName;//Utility.ToStr(SearchedfieldsJSON["txtDescription"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                        {
                            dr.Comments = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
                        }
                        dr.PhoneEncounterFolder = MDVUtility.ToStr(SearchedfieldsJSON["hfPhoneEncounterFolder"]);
                        if (file.ContentType == "application/pdf")
                            dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
                        else
                            dr.Pages = 1;
                        dr.IsActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;


                        if (advancePaymentId != null)
                            dr.AdvPaymentId = MDVUtility.ToInt64(advancePaymentId);
                        if (!string.IsNullOrEmpty(refModuleName))
                        {
                            dr.RefModuleName = refModuleName;
                        }

                        if (TransitionId > 0)
                        {
                            dr.TransitionId = TransitionId;
                        }

                        if (OrderSetReferralId > 0)
                        {
                            dr.OrderSetReferralId = OrderSetReferralId;
                        }

                        dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                    }

                }
                else
                {
                    DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                    dr.PatientId = PatientID;
                    dr.Documentid = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
                    {
                        dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                        dr.ViewBy = MDVUtility.ToStr(SearchedfieldsJSON["ddlAssignUserto_text"]);
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]))
                    {
                        string dtpDOS = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);

                        if (!string.IsNullOrEmpty(dtpDOS))
                            dr.DOS = MDVUtility.ToDateTime(dtpDOS);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                        dr.ClaimId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaim"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCase"]))
                        dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlCase"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                        dr.VisitId = MDVUtility.ToInt64(SearchedfieldsJSON["hfVisitId"]);

                    byte[] currentFileStream = Convert.FromBase64String(strBase64);


                    dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                    dr.FileType = fileType;
                    if (FileName == "")
                        dr.FilePath = Guid.NewGuid().ToString() + "." + fileType.Split('/')[1];//Utility.ToStr(SearchedfieldsJSON["txtDescription"]);
                    else
                        dr.FilePath = FileName + "." + fileType.Split('/')[1];
                    MemoryStream ms = new MemoryStream(currentFileStream);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                    {
                        dr.Comments = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
                    }

                    if (fileType == "application/pdf")
                        dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
                    else
                        dr.Pages = 1;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    if (advancePaymentId != null)
                        dr.AdvPaymentId = MDVUtility.ToInt64(advancePaymentId);

                    if (!string.IsNullOrEmpty(refModuleName))
                    {
                        dr.RefModuleName = refModuleName;
                    }
                    else
                    {
                        dr[dsDocument.PatientDocument.RefModuleNameColumn] = DBNull.Value;
                    }

                    if (TransitionId > 0)
                    {
                        dr.TransitionId = TransitionId;
                    }
                    else
                    {
                        dr[dsDocument.PatientDocument.TransitionIdColumn] = DBNull.Value;
                    }

                    if (OrderSetReferralId > 0)
                    {
                        dr.OrderSetReferralId = OrderSetReferralId;
                    }
                    else
                    {
                        dr[dsDocument.PatientDocument.OrderSetReferralIdColumn] = DBNull.Value;
                    }

                    dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                }

                #region Database Insertion
                BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument, isReviewed);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        MedicalDocumentId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.MedicalDocIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion
                //}
                //else
                //{
                //    throw new Exception("Patient not found.");
                //}

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

        public string SaveOrderSetReferralDocument(string fieldsJSON, HttpFileCollection files, long OrderSetReferralId, string strBase64 = null, string fileType = null, string FileName = "", string refModuleName = "", byte[] docByteArray = null)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (OrderSetReferralId > -1)
                {
                    DSPatient dsDocument = new DSPatient();
                    if (docByteArray != null && docByteArray.Length > 0)
                    {
                        DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                        dr.FileStream = docByteArray.Length > 0 ? docByteArray : null;
                        dr.FileType = "application/pdf";

                        dr.Pages = MDVUtility.getPdfPagesCount(docByteArray);

                        dr.IsActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.ModifiedOn = DateTime.Now;

                        if (!string.IsNullOrEmpty(refModuleName))
                        {
                            dr.RefModuleName = refModuleName;
                        }
                        else
                        {
                            dr[dsDocument.PatientDocument.RefModuleNameColumn] = DBNull.Value;
                        }
                        if (OrderSetReferralId > 0)
                        {
                            dr.OrderSetReferralId = OrderSetReferralId;
                        }
                        else
                        {
                            dr[dsDocument.PatientDocument.OrderSetReferralIdColumn] = DBNull.Value;
                        }

                        dsDocument.PatientDocument.AddPatientDocumentRow(dr);

                    }
                    else if (strBase64 == null)
                    {

                        foreach (string name in files)
                        {
                            HttpPostedFile file = files[name];
                            DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                            dr.Documentid = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
                            {
                                dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                                dr.ViewBy = MDVUtility.ToStr(SearchedfieldsJSON["ddlAssignUserto_text"]);
                            }
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]))
                                dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseId"]);
                            else
                                dr[dsDocument.PatientDocument.CaseIdColumn] = DBNull.Value;

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                                dr.VisitId = MDVUtility.ToInt64(SearchedfieldsJSON["hfVisitId"]);
                            else
                                dr[dsDocument.PatientDocument.VisitIdColumn] = DBNull.Value;


                            string dtpDOS = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);
                            if (!string.IsNullOrEmpty(dtpDOS))
                                dr.DOS = MDVUtility.ToDateTime(dtpDOS);

                            byte[] currentFileStream = new byte[file.ContentLength];
                            int isRead = file.InputStream.Read(currentFileStream, 0, file.ContentLength);

                            dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                            dr.FileType = file.ContentType;
                            dr.FilePath = file.FileName;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                            {
                                dr.Comments = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
                            }

                            if (file.ContentType == "application/pdf")
                                dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
                            else
                                dr.Pages = 1;
                            dr.IsActive = true;
                            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.CreatedOn = DateTime.Now;
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;

                            if (!string.IsNullOrEmpty(refModuleName))
                            {
                                dr.RefModuleName = refModuleName;
                            }

                            if (OrderSetReferralId > 0)
                            {
                                dr.OrderSetReferralId = OrderSetReferralId;
                            }

                            dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                        }

                    }
                    else
                    {
                        DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                        dr.Documentid = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
                        {
                            dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                            dr.ViewBy = MDVUtility.ToStr(SearchedfieldsJSON["ddlAssignUserto_text"]);
                        }

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]))
                        {
                            string dtpDOS = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);

                            if (!string.IsNullOrEmpty(dtpDOS))
                                dr.DOS = MDVUtility.ToDateTime(dtpDOS);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                            dr.ClaimId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaim"]);
                        else
                            dr[dsDocument.PatientDocument.ClaimIdColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCase"]))
                            dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlCase"]);
                        else
                            dr[dsDocument.PatientDocument.CaseIdColumn] = DBNull.Value;


                        byte[] currentFileStream = Convert.FromBase64String(strBase64);


                        dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                        dr.FileType = fileType;
                        if (FileName == "")
                            dr.FilePath = Guid.NewGuid().ToString() + "." + fileType.Split('/')[1];//Utility.ToStr(SearchedfieldsJSON["txtDescription"]);
                        else
                            dr.FilePath = FileName + "." + fileType.Split('/')[1];
                        MemoryStream ms = new MemoryStream(currentFileStream);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                        {
                            dr.Comments = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
                        }

                        if (fileType == "application/pdf")
                            dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
                        else
                            dr.Pages = 1;
                        dr.IsActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        if (!string.IsNullOrEmpty(refModuleName))
                        {
                            dr.RefModuleName = refModuleName;
                        }
                        else
                        {
                            dr[dsDocument.PatientDocument.RefModuleNameColumn] = DBNull.Value;
                        }

                        if (OrderSetReferralId > 0)
                        {
                            dr.OrderSetReferralId = OrderSetReferralId;
                        }
                        else
                        {
                            dr[dsDocument.PatientDocument.OrderSetReferralIdColumn] = DBNull.Value;
                        }

                        dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                    }

                    #region Database Insertion
                    BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            MedicalDocumentId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.MedicalDocIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                    #endregion
                }
                else
                {
                    throw new Exception("OrderSet Referral not found.");
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

        // adnan maqbool, for EMR-1853
        public Int32 GetDocumentId(string FolderName = "")
        {
            Int32 Documentid = 0;
            if (FolderName == "")
            {
                FolderName = "Progress Notes";
            }
            else if (FolderName == "Custom Form")
            {
                FolderName = "Custom Form";
            }
            DSDocument dsDoc = new DALDocument().LoadDocument(0, FolderName, 0, "", "", "");
            DataTable dtDocType = dsDoc.Documents;
            if (dtDocType != null && dtDocType.Rows.Count > 0)
            {
                Documentid = MDVUtility.ToInt32(dtDocType.Rows[0]["DocId"]);
            }

            return Documentid;
        }
        // adnan maqbool, for EMR-1853        
        public string SaveSignedDocument(string fieldsJSON, Int64 PatientID, long TransitionId, string RefModuleName, string strBase64 = null, string fileType = null, string FileName = "", string FolderName = "")
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                Int32 Documentid = 0;
                if (PatientID > -1)
                {
                    DSPatient dsDocument = new DSPatient();


                    DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                    dr.PatientId = PatientID;
                    //Documentid = SearchedfieldsJSON["Folder"] == "Custom Form" ? GetDocumentId(dsDocument, SearchedfieldsJSON["Folder"]) : GetDocumentId(dsDocument);
                    Documentid = GetDocumentId(FolderName);
                    if (Documentid > 0)
                        dr.Documentid = Documentid;
                    else
                        dr.Documentid = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);

                    byte[] currentFileStream = Convert.FromBase64String(strBase64);

                    // Change Images and Documents Storage from Database to FileSystem  IMP-1313
                    //dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                    dr.FileStream = null;
                    dr.FileType = fileType;
                    if (SearchedfieldsJSON != null && SearchedfieldsJSON.ContainsKey("TransitionId"))
                    {
                        dr.TransitionId = MDVUtility.ToInt64(SearchedfieldsJSON["TransitionId"]);
                    }
                    else if (TransitionId > 0)
                    {
                        dr.TransitionId = TransitionId;
                    }
                    else
                    {
                        dr[dsDocument.PatientDocument.TransitionIdColumn.ColumnName] = DBNull.Value;
                    }
                    if (SearchedfieldsJSON != null && SearchedfieldsJSON.ContainsKey("RefModuleName"))
                    {
                        dr.RefModuleName = MDVUtility.ToStr(SearchedfieldsJSON["RefModuleName"]);
                    }
                    else if (!string.IsNullOrEmpty(RefModuleName))
                    {
                        dr.RefModuleName = RefModuleName;
                    }
                    else
                    {
                        dr[dsDocument.PatientDocument.RefModuleNameColumn.ColumnName] = DBNull.Value;
                    }

                    if (FileName == "")
                        dr.FilePath = Guid.NewGuid().ToString() + "." + fileType.Split('/')[1];//Utility.ToStr(SearchedfieldsJSON["txtDescription"]);
                    else
                        dr.FilePath = FileName;

                    if (FileName.Contains("/"))
                    {
                        FileName = FileName.Replace("/", "_");
                    }

                    //var curDate = DateTime.Now;
                    if (SearchedfieldsJSON != null && SearchedfieldsJSON.ContainsKey("Folder"))
                    {
                        if (SearchedfieldsJSON["Folder"] == "Custom Form")
                        {
                            dr.FilePath = DateTime.Now.ToString("MM.dd.yyyy") + "_" + SearchedfieldsJSON["CustomFormName"];
                            dr.DOS = Convert.ToDateTime(SearchedfieldsJSON["DOS"]);
                            FileName = DateTime.Now.ToString("MM.dd.yyyy") + "_" + SearchedfieldsJSON["CustomFormName"];
                        }
                    }
                    if (SearchedfieldsJSON != null && SearchedfieldsJSON.ContainsKey("noteVisitDate"))
                    {
                        dr.DOS = Convert.ToDateTime(SearchedfieldsJSON["noteVisitDate"]);
                    }
                    //AST-50 BY:MAhmad
                    dr.ReviewDate = DateTime.Now;
                    dr.ReviewBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);


                    if (SearchedfieldsJSON != null && SearchedfieldsJSON.ContainsKey("NoteHtml"))
                    {
                        if (SearchedfieldsJSON["NoteHtml"] != null && SearchedfieldsJSON["NoteHtml"] != "")
                        {
                            if (fileType == "application/pdf")
                                dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
                            else
                                dr.Pages = 1;
                        }
                        else
                        {
                            dr.Pages = 1;
                        }
                    }
                    else
                    {
                        if (fileType == "application/pdf")
                            dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
                        else
                            dr.Pages = 1;
                    }
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.DocumentHtml = (SearchedfieldsJSON != null && SearchedfieldsJSON.ContainsKey("NoteHtml")) ? SearchedfieldsJSON["NoteHtml"] : "";
                    dr.Url = CommonFunc.SaveDocumentToFolder(null, "Patient Document", FolderName, PatientID, FileName + ".pdf", currentFileStream);

                    dsDocument.PatientDocument.AddPatientDocumentRow(dr);

                    #region Database Insertion
                    BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            MedicalDocumentId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.MedicalDocIdColumn.ColumnName],
                            CustomFormNameForDoc = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.FilePathColumn.ColumnName],
                            PatDocId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.PatDocIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                    #endregion
                }
                else
                {
                    throw new Exception("Patient not found.");
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


        //string strJSONData = UpdatePatientDocumentFromNotes(CustomFormNameForDoc, fileStream);

        public string UpdatePatientDocumentFromNotes(string FileName, string strBase64)
        {
            try
            {
                DSPatient dsDocument = new DSPatient();
                DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();

                byte[] currentFileStream = Convert.FromBase64String(strBase64);

                dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;

                dr.FilePath = FileName;
                MemoryStream ms = new MemoryStream(currentFileStream);


                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dsDocument.PatientDocument.AddPatientDocumentRow(dr);

                #region Database Insertion
                BLObject<DSPatient> obj = BLLPatientObj.UpdatePatientDocumentFromNotes(dsDocument);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        MedicalDocumentId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.MedicalDocIdColumn.ColumnName],
                        CustomFormNameForDoc = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.FilePathColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion


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
        public string UpdatePatientDocumentFromClaim(Int32 PatDocId, long VisitId)
        {
            try
            {
                DSPatient dsDocument = new DSPatient();
                DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                dr.PatDocId = PatDocId;
                dr.VisitId = VisitId;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                #region Database Insertion
                BLObject<DSPatient> obj = BLLPatientObj.UpdatePatientDocumentFromClaim(dsDocument);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = "Successfully Linked.",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion


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



        // 
        /// <summary>
        /// Deletes the Patient Document.
        /// </summary>
        /// <param name="PatientDocId">The PatientDocument identifier.</param>
        /// <returns></returns>
        /// 
        private string DeletePatientDocument(string PatientDocId, string ChildPatientDocId, string LinkDocumentId)
        {

            try
            {
                if (string.IsNullOrEmpty(PatientDocId))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLPatientObj.DeletePatientDocument(PatientDocId, ChildPatientDocId, LinkDocumentId);

                    var response = new
                    {
                        status = true,
                        Message = obj.Data
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        
       

        private string DeAttachClaimDocument(string PatientDocId)
        {

            try
            {
                if (string.IsNullOrEmpty(PatientDocId))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLPatientObj.DeAttachClaimDocument(PatientDocId);

                    var response = new
                    {
                        status = true,
                        Message = obj.Data
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string InsertPatientDocumentMultipleFolders(string fieldsJSON, Int64 PatientId, string PatientDocId, string IsReviewed, string IsMessage, string IsSigned, List<CommonSearch> FoldersList, string CurrentForlderId, string CurrentForlderName, List<CommonSearch> deletedNewFolderList,List<CommonSearch> AddedNewFoldersList)
        {
            try
            {
                if (PatientDocId != "")
                {

                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = null;
                    if (IsSigned == "1")
                        obj = BLLPatientObj.LoadPatientDocument(PatientDocId, PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "", IsSigned);
                    else
                        obj = BLLPatientObj.LoadPatientDocument(PatientDocId, PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "");
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };

                        var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                        #region DATSET BINDING
                        foreach (DataRow dr in dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows)
                        {
                            if (SearchedfieldsJSON.ContainsKey("reviewed") && MDVUtility.ToBool(SearchedfieldsJSON["reviewed"]))
                            {
                                IsReviewed = "1";
                            }
                            if (SearchedfieldsJSON.ContainsKey("ddlAssignUserto") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
                            {
                                dr[dsPatient.PatientDocument.AssignedToIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                                dr[dsPatient.PatientDocument.ViewByColumn] = SearchedfieldsJSON["ddlAssignUserto_text"];
                            }
                            else
                            {
                                dr[dsPatient.PatientDocument.AssignedToIdColumn] = DBNull.Value;
                            }

                            // currentfolder id 
                            if (MDVUtility.ToInt32(CurrentForlderId) > 0) {
                                dr[dsPatient.PatientDocument.DocumentidColumn] = MDVUtility.ToInt32(CurrentForlderId);
                            }

                            //if (SearchedfieldsJSON.ContainsKey("ddlFolder") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlFolder"]))
                            //    dr[dsPatient.PatientDocument.DocumentidColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);
                            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                            //    dr[dsPatient.PatientDocument.ClaimIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaim"]);
                            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCase"]))
                            //    dr[dsPatient.PatientDocument.CaseIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlCase"]);
                            if (SearchedfieldsJSON.ContainsKey("txtComments") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                            {
                                dr[dsPatient.PatientDocument.CommentsColumn] = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("txtComments") && string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                            {
                                dr[dsPatient.PatientDocument.CommentsColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("hfPatientId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                            {
                                dr[dsPatient.PatientDocument.PatientIdColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["hfPatientId"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("hfPatientId") && string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                            {
                                dr[dsPatient.PatientDocument.PatientIdColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("rdCalendar") == true)
                            { dr[dsPatient.PatientDocument.IsCalendarColumn] = MDVUtility.ToStr(SearchedfieldsJSON["rdCalendar"]) == "True" ? true : false; }
                            if (SearchedfieldsJSON.ContainsKey("dtpDOS") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]) && Convert.ToBoolean(dr[dsPatient.PatientDocument.IsCalendarColumn]) == true)
                                dr[dsPatient.PatientDocument.DOSColumn] = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);
                            else if (SearchedfieldsJSON.ContainsKey("dtpDOS") && string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]) && Convert.ToBoolean(dr[dsPatient.PatientDocument.IsCalendarColumn]) == true)
                                dr[dsPatient.PatientDocument.DOSColumn] = DBNull.Value;
                            if (SearchedfieldsJSON.ContainsKey("ddlDOS") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDOS"]) && Convert.ToBoolean(dr[dsPatient.PatientDocument.IsCalendarColumn]) == false)
                                dr[dsPatient.PatientDocument.DOSColumn] = Convert.ToDateTime(SearchedfieldsJSON["ddlDOS"]);
                            else if (SearchedfieldsJSON.ContainsKey("ddlDOS") && string.IsNullOrEmpty(SearchedfieldsJSON["ddlDOS"]) && Convert.ToBoolean(dr[dsPatient.PatientDocument.IsCalendarColumn]) == false)
                                dr[dsPatient.PatientDocument.DOSColumn] = DBNull.Value;

                            if (SearchedfieldsJSON.ContainsKey("txtCaseNumber") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]))
                                dr[dsPatient.PatientDocument.CaseIdColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["hfCaseId"]);
                            else if (SearchedfieldsJSON.ContainsKey("txtCaseNumber") && string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]))
                                dr[dsPatient.PatientDocument.CaseIdColumn] = DBNull.Value;

                            if (SearchedfieldsJSON.ContainsKey("txtClaimNumber") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                            {
                                dr[dsPatient.PatientDocument.VisitIdColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["hfVisitId"]);
                                dr[dsPatient.PatientDocument.IsAttachedColumn] = Convert.ToBoolean(SearchedfieldsJSON["claimAttach"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("txtClaimNumber") && string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                            {
                                dr[dsPatient.PatientDocument.VisitIdColumn] = DBNull.Value;
                                dr[dsPatient.PatientDocument.IsAttachedColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("txtFileName") && !string.IsNullOrWhiteSpace(SearchedfieldsJSON["txtFileName"]))
                            {
                                dr[dsPatient.PatientDocument.FilePathColumn] = SearchedfieldsJSON["txtFileName"] + SearchedfieldsJSON["lnkFileNameExt"];
                            }

                            if (SearchedfieldsJSON.ContainsKey("dtpReceivedOn") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpReceivedOn"]))
                            {
                                dr[dsPatient.PatientDocument.ReceivedOnColumn] = MDVUtility.ToDateTime(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpReceivedOn"]));
                            }
                            else if (SearchedfieldsJSON.ContainsKey("dtpReceivedOn") && string.IsNullOrEmpty(SearchedfieldsJSON["dtpReceivedOn"]))
                            {
                                dr[dsPatient.PatientDocument.ReceivedOnColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("ddlDocumentSource") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentSource"]))
                            {
                                dr[dsPatient.PatientDocument.DocumentSourceIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentSource"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("ddlDocumentSource") && string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentSource"]))
                            {
                                dr[dsPatient.PatientDocument.DocumentSourceIdColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("txtOtherDocumentSource") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtOtherDocumentSource"]))
                            {
                                dr[dsPatient.PatientDocument.OtherDocumentSourceColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtOtherDocumentSource"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("txtOtherDocumentSource") && string.IsNullOrEmpty(SearchedfieldsJSON["txtOtherDocumentSource"]))
                            {
                                dr[dsPatient.PatientDocument.OtherDocumentSourceColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("ddlDocumentProvider") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentProvider"]))
                            {
                                dr[dsPatient.PatientDocument.DocumentProviderIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentProvider"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("ddlDocumentProvider") && string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentProvider"]))
                            {
                                dr[dsPatient.PatientDocument.DocumentProviderIdColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("txtNarrativeReference") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtNarrativeReference"]))
                            {
                                dr[dsPatient.PatientDocument.NarrativeReferenceColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtNarrativeReference"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("txtNarrativeReference") && string.IsNullOrEmpty(SearchedfieldsJSON["txtNarrativeReference"]))
                            {
                                dr[dsPatient.PatientDocument.NarrativeReferenceColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("txtReferenceLink") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtReferenceLink"]))
                            {
                                dr[dsPatient.PatientDocument.ReferenceLinkColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtReferenceLink"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("txtReferenceLink") && string.IsNullOrEmpty(SearchedfieldsJSON["txtReferenceLink"]))
                            {
                                dr[dsPatient.PatientDocument.ReferenceLinkColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("ddlDocumentPriority") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentPriority"]))
                            {
                                dr[dsPatient.PatientDocument.DocPriorityIDColumn] = SearchedfieldsJSON["ddlDocumentPriority"];
                            }
                            else if (SearchedfieldsJSON.ContainsKey("ddlDocumentPriority") && string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentPriority"]))
                            {
                                dr[dsPatient.PatientDocument.DocPriorityIDColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("dtpExpirtyDate") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpExpirtyDate"]))
                            {
                                string dtpExpirtyDate = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpExpirtyDate"]);
                                dr[dsPatient.PatientDocument.ExpiryDateColumn] = MDVUtility.ToDateTime(dtpExpirtyDate);
                            }
                            else
                            {
                                dr[dsPatient.PatientDocument.ExpiryDateColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("txtTagName") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTagName"]))
                            {
                                dr[dsPatient.PatientDocument.TagIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["hfDocumentTagId"]);
                            }
                            else
                            {
                                dr[dsPatient.PatientDocument.TagIdColumn] = DBNull.Value;
                            }


                        }
                        #endregion
                        BLObject<DSPatient> objPatientDocument = null;

                        objPatientDocument = BLLPatientObj.InsertPatientDocMultipleFolder(dsPatient, PatientId, FoldersList,deletedNewFolderList,AddedNewFoldersList, IsReviewed, IsMessage, IsSigned);

                        if (objPatientDocument.Data != null)
                        {
                            if (IsMessage == "0" && dsPatient.PatientDocument.Rows[0]["AssignedToId"].ToString() == MDVSession.Current.AppUserId.ToString())
                            {
                                MDVSession.Current.MessagesCount = (Convert.ToInt32(MDVSession.Current.MessagesCount) + 1).ToString();
                            }
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                PatDocId =dsPatient.PatientDocument.Rows[0]["PatDocId"].ToString()
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objPatientDocument.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient Document not found."
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



        private string UpdatePatientDocument(string fieldsJSON, Int64 PatientId, string PatientDocId,string CurrentForlderId,string IsReviewed, string IsMessage, string IsSigned)
        {
            try
            {
                if (PatientDocId != "")
                {

                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = null;
                    if (IsSigned == "1")
                        obj = BLLPatientObj.LoadPatientDocument(PatientDocId, PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "", IsSigned);
                    else
                        obj = BLLPatientObj.LoadPatientDocument(PatientDocId, PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "");
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };

                        var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                        #region DATSET BINDING
                        foreach (DataRow dr in dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows)
                        {
                            if (SearchedfieldsJSON.ContainsKey("reviewed") && MDVUtility.ToBool(SearchedfieldsJSON["reviewed"]))
                            {
                                IsReviewed = "1";
                            }
                            if (SearchedfieldsJSON.ContainsKey("ddlAssignUserto") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
                            {
                                dr[dsPatient.PatientDocument.AssignedToIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                                dr[dsPatient.PatientDocument.ViewByColumn] = SearchedfieldsJSON["ddlAssignUserto_text"];
                            }
                            else
                            {
                                dr[dsPatient.PatientDocument.AssignedToIdColumn] = DBNull.Value;
                            }

                            // if Current Folder id 
                            if (MDVUtility.ToInt32(CurrentForlderId) > 0)
                            {
                                dr[dsPatient.PatientDocument.DocumentidColumn] = MDVUtility.ToInt32(CurrentForlderId);

                            }
                            else {
                                if (SearchedfieldsJSON.ContainsKey("ddlFolder") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlFolder"]))
                                   dr[dsPatient.PatientDocument.DocumentidColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);
                            }
                            

                            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                            //    dr[dsPatient.PatientDocument.ClaimIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaim"]);
                            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCase"]))
                            //    dr[dsPatient.PatientDocument.CaseIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlCase"]);
                            if (SearchedfieldsJSON.ContainsKey("txtComments") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                            {
                                dr[dsPatient.PatientDocument.CommentsColumn] = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("txtComments") && string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                            {
                                dr[dsPatient.PatientDocument.CommentsColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("hfPatientId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                            {
                                dr[dsPatient.PatientDocument.PatientIdColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["hfPatientId"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("hfPatientId") && string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                            {
                                dr[dsPatient.PatientDocument.PatientIdColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("rdCalendar") == true)
                            { dr[dsPatient.PatientDocument.IsCalendarColumn] = MDVUtility.ToStr(SearchedfieldsJSON["rdCalendar"]) == "True" ? true : false; }
                            if (SearchedfieldsJSON.ContainsKey("dtpDOS") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]) && Convert.ToBoolean(dr[dsPatient.PatientDocument.IsCalendarColumn]) == true)
                                dr[dsPatient.PatientDocument.DOSColumn] = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);
                            else if (SearchedfieldsJSON.ContainsKey("dtpDOS") && string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]) && Convert.ToBoolean(dr[dsPatient.PatientDocument.IsCalendarColumn]) == true)
                                dr[dsPatient.PatientDocument.DOSColumn] = DBNull.Value;
                            if (SearchedfieldsJSON.ContainsKey("ddlDOS") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDOS"]) && Convert.ToBoolean(dr[dsPatient.PatientDocument.IsCalendarColumn]) == false)
                                dr[dsPatient.PatientDocument.DOSColumn] = Convert.ToDateTime(SearchedfieldsJSON["ddlDOS"]);
                            else if (SearchedfieldsJSON.ContainsKey("ddlDOS") && string.IsNullOrEmpty(SearchedfieldsJSON["ddlDOS"]) && Convert.ToBoolean(dr[dsPatient.PatientDocument.IsCalendarColumn]) == false)
                                dr[dsPatient.PatientDocument.DOSColumn] = DBNull.Value;

                            if (SearchedfieldsJSON.ContainsKey("txtCaseNumber") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]))
                                dr[dsPatient.PatientDocument.CaseIdColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["hfCaseId"]);
                            else if (SearchedfieldsJSON.ContainsKey("txtCaseNumber") && string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]))
                                dr[dsPatient.PatientDocument.CaseIdColumn] = DBNull.Value;

                            if (SearchedfieldsJSON.ContainsKey("txtClaimNumber") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                            {
                                dr[dsPatient.PatientDocument.VisitIdColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["hfVisitId"]);
                                dr[dsPatient.PatientDocument.IsAttachedColumn] = Convert.ToBoolean(SearchedfieldsJSON["claimAttach"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("txtClaimNumber") && string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                            {
                                dr[dsPatient.PatientDocument.VisitIdColumn] = DBNull.Value;
                                dr[dsPatient.PatientDocument.IsAttachedColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("txtFileName") && !string.IsNullOrWhiteSpace(SearchedfieldsJSON["txtFileName"]))
                            {
                                dr[dsPatient.PatientDocument.FilePathColumn] = SearchedfieldsJSON["txtFileName"] + SearchedfieldsJSON["lnkFileNameExt"];
                            }

                            if (SearchedfieldsJSON.ContainsKey("dtpReceivedOn") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpReceivedOn"]))
                            {
                                dr[dsPatient.PatientDocument.ReceivedOnColumn] = MDVUtility.ToDateTime(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpReceivedOn"]));
                            }
                            else if (SearchedfieldsJSON.ContainsKey("dtpReceivedOn") && string.IsNullOrEmpty(SearchedfieldsJSON["dtpReceivedOn"]))
                            {
                                dr[dsPatient.PatientDocument.ReceivedOnColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("ddlDocumentSource") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentSource"]))
                            {
                                dr[dsPatient.PatientDocument.DocumentSourceIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentSource"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("ddlDocumentSource") && string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentSource"]))
                            {
                                dr[dsPatient.PatientDocument.DocumentSourceIdColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("txtOtherDocumentSource") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtOtherDocumentSource"]))
                            {
                                dr[dsPatient.PatientDocument.OtherDocumentSourceColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtOtherDocumentSource"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("txtOtherDocumentSource") && string.IsNullOrEmpty(SearchedfieldsJSON["txtOtherDocumentSource"]))
                            {
                                dr[dsPatient.PatientDocument.OtherDocumentSourceColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("ddlDocumentProvider") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentProvider"]))
                            {
                                dr[dsPatient.PatientDocument.DocumentProviderIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentProvider"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("ddlDocumentProvider") && string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentProvider"]))
                            {
                                dr[dsPatient.PatientDocument.DocumentProviderIdColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("txtNarrativeReference") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtNarrativeReference"]))
                            {
                                dr[dsPatient.PatientDocument.NarrativeReferenceColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtNarrativeReference"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("txtNarrativeReference") && string.IsNullOrEmpty(SearchedfieldsJSON["txtNarrativeReference"]))
                            {
                                dr[dsPatient.PatientDocument.NarrativeReferenceColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("txtReferenceLink") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtReferenceLink"]))
                            {
                                dr[dsPatient.PatientDocument.ReferenceLinkColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtReferenceLink"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("txtReferenceLink") && string.IsNullOrEmpty(SearchedfieldsJSON["txtReferenceLink"]))
                            {
                                dr[dsPatient.PatientDocument.ReferenceLinkColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("ddlDocumentPriority") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentPriority"]))
                            {
                                dr[dsPatient.PatientDocument.DocPriorityIDColumn] = SearchedfieldsJSON["ddlDocumentPriority"];
                            }
                            else if (SearchedfieldsJSON.ContainsKey("ddlDocumentPriority") && string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentPriority"]))
                            {
                                dr[dsPatient.PatientDocument.DocPriorityIDColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("dtpExpirtyDate") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpExpirtyDate"]))
                            {
                                string dtpExpirtyDate = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpExpirtyDate"]);
                                dr[dsPatient.PatientDocument.ExpiryDateColumn] = MDVUtility.ToDateTime(dtpExpirtyDate);
                            }
                            else
                            {
                                dr[dsPatient.PatientDocument.ExpiryDateColumn] = DBNull.Value;
                            }
                            if (SearchedfieldsJSON.ContainsKey("txtTagName") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTagName"]))
                            {
                                dr[dsPatient.PatientDocument.TagIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["hfDocumentTagId"]);
                            }
                            else
                            {
                                dr[dsPatient.PatientDocument.TagIdColumn] = DBNull.Value;
                            }


                        }
                        #endregion
                        BLObject<DSPatient> objPatientDocument = null;

                        objPatientDocument = BLLPatientObj.UpdatePatientDocument(dsPatient, PatientId, SearchedfieldsJSON
                         ["ddlFolder_text"], IsReviewed, IsMessage, IsSigned);

                        if (objPatientDocument.Data != null)
                        {
                            if (IsMessage == "0" && dsPatient.PatientDocument.Rows[0]["AssignedToId"].ToString() == MDVSession.Current.AppUserId.ToString())
                            {
                                MDVSession.Current.MessagesCount = (Convert.ToInt32(MDVSession.Current.MessagesCount) + 1).ToString();
                            }
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objPatientDocument.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient Document not found."
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




        private string UpdatePatientDocumentIsActive(Int64 PatientId, string PatientDocId, Int64 IsActive)
        {
            try
            {
                if (MDVUtility.ToInt64(PatientDocId) > 0)
                {

                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientDocument(PatientDocId, PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "");
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows[0];
                        dr[dsPatient.PatientDocument.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatient> objFamily = BLLPatientObj.UpdatePatientDocument(dsPatient, PatientId);
                        string successMsg;
                        if (objFamily.Data != null)
                        {
                            if (IsActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objFamily.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient Document not found."
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

        private string FillPatientDocument(Int64 PatientID, string PatDocIds = "", string bFileStream = "0", string IsReviewed = "")
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = null;
                obj = BLLPatientObj.LoadPatientDocument(PatDocIds, PatientID, "", "", "", null, null, null, null, "", 0, 0, IsReviewed, 0, "", bFileStream);
                dsPatient = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0)
                    {
                        string LoadPrevious = "0";
                        foreach (DataRow dtr in dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows)
                        {
                            byte[] byteArr = dtr["FileStream"] as byte[];
                            string UrlPath = dtr["Url"].ToString();
                            if (!String.IsNullOrEmpty(UrlPath))
                            {
                                byte[] file;
                                string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                                UrlPath = FilePath + UrlPath;
                                using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                                {
                                    using (var reader = new BinaryReader(stream))
                                    {
                                        file = reader.ReadBytes((int)stream.Length);

                                    }
                                }
                                if (file != null)
                                {
                                    dtr["Base64FileStream"] = Convert.ToBase64String(file);
                                    dtr["FileStream"] = file;
                                    LoadPrevious = "1";
                                }
                            }
                            else if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                // Add a New Column to Store the Base64 String
                                if (!dtr.Table.Columns.Contains("Base64FileStream"))
                                {
                                    dsPatient.Tables[dsPatient.PatientDocument.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dtr["Base64FileStream"] = strBase64;
                                LoadPrevious = "1";
                            }

                        }


                        DataRow dr = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows[0];
                        string fileName = MDVUtility.ToStr(dr[dsPatient.PatientDocument.FilePathColumn.ColumnName]);
                        int index = fileName.LastIndexOf('.');
                        string strFileName = string.Empty;
                        string strFileExt = string.Empty;
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            if (index > 0)
                            {
                                strFileName = fileName.Substring(0, index);
                                strFileExt = "." + fileName.Substring(index + 1);
                            }
                            else
                            {
                                strFileName = fileName;
                                if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.PatientDocument.FileTypeColumn.ColumnName])))
                                    strFileExt = "." + MDVUtility.ToStr(dr[dsPatient.PatientDocument.FileTypeColumn.ColumnName]).Split('/')[1];
                            }
                        }
                        else if (!string.IsNullOrEmpty(dr[dsPatient.PatientDocument.FileTypeColumn.ColumnName].ToString()))
                        {
                            strFileExt = "." + MDVUtility.ToStr(dr[dsPatient.PatientDocument.FileTypeColumn.ColumnName]).Split('/')[1];
                        }
                        var keyValues = new Dictionary<string, string>
                        {
                            { "txtLoadPrevious", LoadPrevious},
                            { "ddlFolder", MDVUtility.ToStr(dr[dsPatient.PatientDocument.DocumentidColumn.ColumnName])},
                            { "txtAccountNumber", MDVUtility.ToStr(dr[dsPatient.PatientDocument.AccountNumberColumn.ColumnName])},

                            // EMR-2211
                            // faizan ameen
                            // txtPatientName value changed from ,- to empty in case of no patient found.
                            
                         
                            { "txtPatientName", MDVUtility.ToStr(dr[dsPatient.PatientDocument.LastNameColumn.ColumnName])!=string.Empty|| MDVUtility.ToStr(dr[dsPatient.PatientDocument.FirstNameColumn.ColumnName])!=string.Empty? MDVUtility.ToStr(dr[dsPatient.PatientDocument.AccountNumberColumn.ColumnName]) +" - "+MDVUtility.ToStr(dr[dsPatient.PatientDocument.LastNameColumn.ColumnName]+","+dr[dsPatient.PatientDocument.FirstNameColumn.ColumnName]):string.Empty},

                            { "txtPatientNameFull", MDVUtility.ToStr(dr[dsPatient.PatientDocument.LastNameColumn.ColumnName])!=string.Empty|| MDVUtility.ToStr(dr[dsPatient.PatientDocument.FirstNameColumn.ColumnName])!=string.Empty? MDVUtility.ToStr(dr[dsPatient.PatientDocument.LastNameColumn.ColumnName]+", "+dr[dsPatient.PatientDocument.FirstNameColumn.ColumnName]):string.Empty},

                            //{ "dtpDOS", MDVUtility.ToStr(dr[dsPatient.PatientDocument.DOSColumn.ColumnName])},
                            //{ "dtpDOS",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.PatientDocument.DOSColumn.ColumnName])) == true?null:MDVUtility.ToDateTime(dr[dsPatient.PatientDocument.DOSColumn.ColumnName]).ToShortDateString()},
                            { "txtCaseNumber", MDVUtility.ToStr(dr[dsPatient.PatientDocument.CaseNumberColumn.ColumnName])},
                            { "txtClaimNumber", MDVUtility.ToStr(dr[dsPatient.PatientDocument.ClaimNumberColumn.ColumnName])},
                            { "claimAttach", MDVUtility.ToStr(dr[dsPatient.PatientDocument.IsAttachedColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsPatient.PatientDocument.CommentsColumn.ColumnName])},
                            { "hfPatientId", MDVUtility.ToStr(dr[dsPatient.PatientDocument.PatientIdColumn.ColumnName])},
                            { "hfCaseId", MDVUtility.ToStr(dr[dsPatient.PatientDocument.CaseIdColumn.ColumnName])},
                            { "hfVisitId", MDVUtility.ToStr(dr[dsPatient.PatientDocument.VisitIdColumn.ColumnName])},
                           
                             //{ "Base64FileStream", MDVUtility.ToStr(dr["Base64FileStream"])},
                            { "Base64FileStream", MDVUtility.ToStr(dr[dsPatient.PatientDocument.Base64FileStreamColumn.ColumnName])},
                            { "FileType", MDVUtility.ToStr(dr[dsPatient.PatientDocument.FileTypeColumn.ColumnName])},
                            { "ReviewDate", MDVUtility.ToStr(dr[dsPatient.PatientDocument.ReviewDateColumn.ColumnName])},
                            { "SignDate", MDVUtility.ToStr(dr[dsPatient.PatientDocument.SignDateColumn.ColumnName])},
                            { "txtFileName",strFileName },
                            { "lnkFileNameExt", strFileExt},
                            { "NoteHtml", MDVUtility.ToStr(dr[dsPatient.PatientDocument.DocumentHtmlColumn.ColumnName])},
                            { "Url", MDVUtility.ToStr(dr[dsPatient.PatientDocument.UrlColumn.ColumnName])},
                            { "dtpReceivedOn", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.PatientDocument.ReceivedOnColumn.ColumnName])) == true?null:MDVUtility.ToDateTime(dr[dsPatient.PatientDocument.ReceivedOnColumn.ColumnName]).ToShortDateString()},
                            { "ddlDocumentSource", MDVUtility.ToStr(dr[dsPatient.PatientDocument.DocumentSourceIdColumn.ColumnName])},
                            { "txtOtherDocumentSource", MDVUtility.ToStr(dr[dsPatient.PatientDocument.OtherDocumentSourceColumn.ColumnName])},
                            { "txtNarrativeReference", MDVUtility.ToStr(dr[dsPatient.PatientDocument.NarrativeReferenceColumn.ColumnName])},
                            { "txtReferenceLink", MDVUtility.ToStr(dr[dsPatient.PatientDocument.ReferenceLinkColumn.ColumnName])},
                            { "ddlDocumentProvider", MDVUtility.ToStr(dr[dsPatient.PatientDocument.DocumentProviderIdColumn.ColumnName])},
                            { "ddlDocumentPriority", MDVUtility.ToStr(dr[dsPatient.PatientDocument.DocPriorityIDColumn.ColumnName])},
                            { "ddlDocumentReview", MDVUtility.ToStr(dr[dsPatient.PatientDocument.AssignedToIdColumn.ColumnName])},
                            { "reviewed", MDVUtility.ToStr(dr[dsPatient.PatientDocument.ReviewedColumn.ColumnName])},
                            { "dtpExpirtyDate",MDVUtility.GetDateMMDDYYY(dr[dsPatient.PatientDocument.ExpiryDateColumn.ColumnName].ToString())},
                            { "txtTagName",dr[dsPatient.PatientDocument.TagNameColumn.ColumnName].ToString()},
                            { "hfDocumentTagId",dr[dsPatient.PatientDocument.TagIdColumn.ColumnName].ToString()},
                            { "IsAttachedWithNote",dr[dsPatient.PatientDocument.IsAttachedWithNoteColumn.ColumnName].ToString()},
                            { "PasswordCreatedBy", dr[dsPatient.PatientDocument.PasswordCreatedByIdColumn.ColumnName].ToString()},
                            { "ddlAssignUserto", dr[dsPatient.PatientDocument.AssignedToIdColumn.ColumnName].ToString()},
                            { "DocPassword", dr[dsPatient.PatientDocument.DocPasswordColumn.ColumnName].ToString()},
                            { "dtpDOB", dr[dsPatient.PatientDocument.DOBColumn.ColumnName].ToString()},
                            { "FolderIds", dr[dsPatient.PatientDocument.FolderIdsColumn.ColumnName].ToString()},
                             { "hfEOBManualPostingId", dr[dsPatient.PatientDocument.EOBManualPostingIdColumn.ColumnName].ToString()}
                        };
                        if (dr[dsPatient.PatientDocument.IsCalendarColumn.ColumnName] != System.DBNull.Value)
                        {
                            if (Convert.ToBoolean(dr[dsPatient.PatientDocument.IsCalendarColumn.ColumnName]) == true)
                            {
                                keyValues.Add("rdCalendar", MDVUtility.ToStr(dr[dsPatient.PatientDocument.IsCalendarColumn.ColumnName]));

                            }
                            else { keyValues.Add("rdDropdown", MDVUtility.ToStr(dr[dsPatient.PatientDocument.IsCalendarColumn.ColumnName])); }
                            if (Convert.ToBoolean(dr[dsPatient.PatientDocument.IsCalendarColumn.ColumnName]))
                                keyValues.Add(
                                      "dtpDOS", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.PatientDocument.DOSColumn.ColumnName])) == true ? string.Empty : MDVUtility.ToDateTime(dr[dsPatient.PatientDocument.DOSColumn.ColumnName]).ToShortDateString()
                                    );
                            else
                                keyValues.Add("ddlDOS", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.PatientDocument.DOSColumn.ColumnName])) == true ? string.Empty : MDVUtility.GetDateMMDDYYY(dr[dsPatient.PatientDocument.DOSColumn.ColumnName].ToString()));
                        }
                        else if (dr[dsPatient.PatientDocument.DOSColumn.ColumnName] == System.DBNull.Value)
                        {
                            keyValues.Add(
                                      "dtpDOS", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.PatientDocument.DOSColumn.ColumnName])) == true ? string.Empty : MDVUtility.ToDateTime(dr[dsPatient.PatientDocument.DOSColumn.ColumnName]).ToShortDateString()
                                    );
                        }
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        var response = new
                        {
                            status = true,
                            DocumentCount = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count,
                            DocumentLoad_JSON = js.Serialize(keyValues),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = obj.Message
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string FillPatientDocuments(Int64 PatientID, string PatDocIds = "", string bFileStream = "0", string IsReviewed = "")
        {
            try
            {
                List<PatientDocumentModel> documentsList = null;
                BLObject<List<PatientDocumentModel>> obj = BLLPatientObj.LoadAttachedPatientDocument(PatDocIds, PatientID, "", "", "", null, null, null, null, "", 0, 0, IsReviewed, 0, "", bFileStream);
                documentsList = obj.Data;

                if (obj.Data != null)
                {
                    if (documentsList.Count > 0)
                    {

                        foreach (var dtr in documentsList)
                        {
                            byte[] byteArr = dtr.FileStream;
                            string UrlPath = dtr.Url;
                            if (!String.IsNullOrEmpty(UrlPath))
                            {
                                byte[] file;
                                string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                                UrlPath = FilePath + UrlPath;
                                using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                                {
                                    using (var reader = new BinaryReader(stream))
                                    {
                                        file = reader.ReadBytes((int)stream.Length);

                                    }
                                }
                                if (file != null)
                                {
                                    dtr.Base64FileStream = Convert.ToBase64String(file);
                                    dtr.FileStream = file;
                                }
                            }
                            else if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                dtr.Base64FileStream = strBase64;
                            }

                        }

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        var response = new
                        {
                            status = true,
                            DocumentCount = documentsList.Count,
                            DocumentLoad_JSON = js.Serialize(documentsList),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AmendmentNoteCount = 0,
                            Message = "Record not found."
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string FillPatientDocumentsMerged(Int64 PatientID, string PatDocIds = "", string bFileStream = "0", string IsReviewed = "")
        {
            try
            {
                if (PatDocIds != "")
                {
                    List<PatientDocumentModel> documentsList = null;
                    BLObject<List<PatientDocumentModel>> obj = BLLPatientObj.LoadAttachedPatientDocument(PatDocIds, PatientID, "", "", "", null, null, null, null, "", 0, 0, IsReviewed, 0, "", bFileStream);
                    documentsList = obj.Data;

                    List<byte[]> docsToPrintArr = new List<byte[]>();
                    List<PatientDocumentModel> patDocs = new List<PatientDocumentModel>();
                    List<PatientDocumentModel> eduDocs = new List<PatientDocumentModel>();

                    if (obj.Data != null)
                    {
                        if (documentsList.Count > 0)
                        {
                            patDocs = documentsList.Where(d => d.DocumentType.Equals("Med")).ToList();

                            if (patDocs.Count > 0)
                            {
                                foreach (var dtr in patDocs)
                                {
                                    byte[] byteArr = dtr.FileStream;
                                    string UrlPath = dtr.Url;
                                    if (!String.IsNullOrEmpty(UrlPath))
                                    {
                                        byte[] file;
                                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                                        UrlPath = FilePath + UrlPath;
                                        using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                                        {
                                            using (var reader = new BinaryReader(stream))
                                            {
                                                file = reader.ReadBytes((int)stream.Length);

                                            }
                                        }
                                        if (file != null)
                                        {
                                            if (MDVUtility.ToStr(dtr.FileType).IndexOf("pdf") >= 0)
                                            {
                                                docsToPrintArr.Add(file);
                                            }
                                            else if (MDVUtility.ToStr(dtr.FileType).IndexOf("html") >= 0)
                                            {
                                                MemoryStream memoryStream = new System.IO.MemoryStream(file);
                                                byte[] eduDoc = memoryStream.ToArray();
                                                string htmlEduDoc = Encoding.ASCII.GetString(eduDoc);
                                                byte[] mergedByteEduDoc = ConvertHtmlToPdf(htmlEduDoc);

                                                docsToPrintArr.Add(mergedByteEduDoc);
                                            }
                                        }
                                    }
                                    else if (byteArr != null)
                                    {
                                        if (MDVUtility.ToStr(dtr.FileType).IndexOf("pdf") >= 0)
                                        {
                                            docsToPrintArr.Add(byteArr);
                                        }
                                        else if (MDVUtility.ToStr(dtr.FileType).IndexOf("html") >= 0)
                                        {
                                            MemoryStream memoryStream = new System.IO.MemoryStream(byteArr);
                                            byte[] eduDoc = memoryStream.ToArray();
                                            string htmlEduDoc = Encoding.ASCII.GetString(eduDoc);
                                            byte[] mergedByteEduDoc = ConvertHtmlToPdf(htmlEduDoc);

                                            docsToPrintArr.Add(mergedByteEduDoc);
                                        }
                                    }
                                }
                            }

                            eduDocs = documentsList.Where(d => d.DocumentType == "Edu").ToList();

                            if (eduDocs.Count > 0)
                            {
                                foreach (var dtr in eduDocs)
                                {
                                    byte[] byteArr = dtr.FileStream;
                                    string UrlPath = dtr.Url;
                                    if (!String.IsNullOrEmpty(UrlPath))
                                    {
                                        byte[] file;
                                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                                        UrlPath = FilePath + UrlPath;
                                        using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                                        {
                                            using (var reader = new BinaryReader(stream))
                                            {
                                                file = reader.ReadBytes((int)stream.Length);

                                            }
                                        }
                                        if (file != null)
                                        {
                                            if (MDVUtility.ToStr(dtr.FileType).IndexOf("image") >= 0)
                                            {
                                                byte[] imgByteArr = imageToPdf(file);
                                                docsToPrintArr.Add(imgByteArr);
                                            }
                                            else if (MDVUtility.ToStr(dtr.FileType).IndexOf("pdf") >= 0)
                                            {
                                                docsToPrintArr.Add(file);
                                            }
                                            else if (MDVUtility.ToStr(dtr.FileType).IndexOf("html") >= 0)
                                            {
                                                MemoryStream memoryStream = new System.IO.MemoryStream(file);
                                                byte[] eduDoc = memoryStream.ToArray();
                                                string htmlEduDoc = Encoding.ASCII.GetString(eduDoc);
                                                byte[] mergedByteEduDoc = ConvertHtmlToPdf(htmlEduDoc);

                                                docsToPrintArr.Add(mergedByteEduDoc);
                                            }
                                        }
                                    }
                                    else if (byteArr != null)
                                    {
                                        if (MDVUtility.ToStr(dtr.FileType).IndexOf("image") >= 0)
                                        {
                                            byte[] imgByteArr = imageToPdf(byteArr);
                                            docsToPrintArr.Add(imgByteArr);
                                        }
                                        else if (MDVUtility.ToStr(dtr.FileType).IndexOf("pdf") >= 0)
                                        {
                                            docsToPrintArr.Add(byteArr);
                                        }
                                        else if (MDVUtility.ToStr(dtr.FileType).IndexOf("html") >= 0)
                                        {
                                            MemoryStream memoryStream = new System.IO.MemoryStream(byteArr);
                                            byte[] eduDoc = memoryStream.ToArray();
                                            string htmlEduDoc = Encoding.ASCII.GetString(eduDoc);
                                            byte[] mergedByteEduDoc = ConvertHtmlToPdf(htmlEduDoc);

                                            docsToPrintArr.Add(mergedByteEduDoc);
                                        }
                                    }
                                }
                            }

                            byte[] MergedByteArr = concatAndAddContentToPrint(docsToPrintArr);

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            js.MaxJsonLength = Int32.MaxValue;
                            var response = new
                            {
                                status = true,
                                MergedContent = Convert.ToBase64String(MergedByteArr.ToArray())
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                AmendmentNoteCount = 0,
                                Message = "Record not found."
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
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "No Document Found."
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public byte[] ConvertHtmlToPdf(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var contentNode = doc.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("serviceContent"));
            if (contentNode != null)
            {
                var tableNode = contentNode.Descendants().FirstOrDefault(n => n.Name.Equals("table"));
                if (tableNode != null)
                {
                    var div = tableNode.NextSibling;
                    if (div != null)
                    {
                        div.Remove();
                    }
                }
            }

            contentNode = doc.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("ptEduFooter"));
            if (contentNode != null)
            {
                contentNode.SetAttributeValue("style", "width:auto;background:#005da9 !important; color:#fff !important;");
            }
            System.IO.StringWriter sw = new StringWriter();
            System.Xml.XmlTextWriter xw = new System.Xml.XmlTextWriter(sw);
            doc.Save(xw);
            string result = sw.ToString();

            using (var stream = new MemoryStream())
            {
                using (var document = new iTextSharp.text.Document())
                {
                    var writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    var tagProcessorFactory = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                    tagProcessorFactory.RemoveProcessor(HTML.Tag.IMG);
                    tagProcessorFactory.AddProcessor(HTML.Tag.IMG, new MDVision.IEHR.Model.PDFHelpers.CustomImageTagProcessor());

                    var htmlPipelineContext = new HtmlPipelineContext(null);
                    htmlPipelineContext.SetTagFactory(tagProcessorFactory);

                    var pdfWriterPipeline = new PdfWriterPipeline(document, writer);
                    var htmlPipeline = new HtmlPipeline(htmlPipelineContext, pdfWriterPipeline);

                    var charset = Encoding.UTF8;
                    var cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                    cssResolver.AddCss(@"code { padding: 2px 4px; }", "utf-8", true);
                    var cssResolverPipeline = new iTextSharp.tool.xml.pipeline.css.CssResolverPipeline(
                        cssResolver, htmlPipeline
                    );

                    var worker = new XMLWorker(cssResolverPipeline, true);
                    var parser = new XMLParser(worker);
                    using (var stringReader = new StringReader(result))
                    {
                        parser.Parse(stringReader);
                    }

                    document.Close();
                    return stream.ToArray();
                }
            }
        }

        public byte[] concatAndAddContentToPrint(List<byte[]> pdfByteContent)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document())
                {
                    using (var copy = new PdfSmartCopy(doc, ms))
                    {
                        doc.Open();
                        foreach (var p in pdfByteContent)
                        {
                            using (var reader = new PdfReader(p))
                            {
                                copy.AddDocument(reader);
                            }
                        }

                        doc.Close();
                    }
                }
                return ms.ToArray();
            }
        }

        public byte[] imageToPdf(byte[] imgByteContent)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imgByteContent);

            byte[] bytes = null;
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document();
                image.Alignment = iTextSharp.text.Image.ALIGN_TOP;
                var lm = document.LeftMargin;
                var rm = document.RightMargin;
                var bm = document.BottomMargin;
                var tm = document.TopMargin;
                var pageWidth = document.PageSize.Width - (lm + rm);
                var pageHeight = document.PageSize.Height - (bm + tm);

                if (image.Width > 500 && image.Width > document.PageSize.Width || image.Height > 842 && image.Height > document.PageSize.Height)
                {
                    image.ScaleToFit(pageWidth, pageHeight);
                }

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                document.Add(image);
                document.Close();
                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }
            return bytes.ToArray();
        }

        private string FillPatientDocumentsForFax(Int64 PatientID, string PatDocIds = "", string bFileStream = "0", string IsReviewed = "", string AttachedFilesStream = "")
        {
            try
            {
                if (PatDocIds != "")
                {
                    List<PatientDocumentModel> documentsList = null;
                    BLObject<List<PatientDocumentModel>> obj = BLLPatientObj.LoadAttachedPatientDocument(PatDocIds, PatientID, "", "", "", null, null, null, null, "", 0, 0, IsReviewed, 0, "", bFileStream);
                    documentsList = obj.Data;

                    List<byte[]> docsToPrintArr = new List<byte[]>();

                    if (!string.IsNullOrEmpty(AttachedFilesStream))
                    {
                        AttachedFilesStream = AttachedFilesStream.Replace(" ", "+");
                        byte[] attachedFilesByteArr = Convert.FromBase64String(AttachedFilesStream);
                        docsToPrintArr.Add(attachedFilesByteArr);
                    }

                    if (obj.Data != null)
                    {
                        if (documentsList.Count > 0)
                        {
                            foreach (var dtr in documentsList)
                            {
                                byte[] byteArr = dtr.FileStream;
                                string UrlPath = dtr.Url;
                                if (!String.IsNullOrEmpty(UrlPath))
                                {
                                    byte[] file;
                                    string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                                    UrlPath = FilePath + UrlPath;
                                    using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                                    {
                                        using (var reader = new BinaryReader(stream))
                                        {
                                            file = reader.ReadBytes((int)stream.Length);

                                        }
                                    }
                                    if (file != null)
                                    {
                                        if (string.IsNullOrEmpty(dtr.FileType) && !string.IsNullOrEmpty(UrlPath))
                                        {
                                            int index = UrlPath.LastIndexOf(".");
                                            string strFileExt = string.Empty;
                                            if (index > -1)
                                            {
                                                strFileExt = "." + UrlPath.Substring(index + 1);
                                                if (strFileExt == ".pdf")
                                                {
                                                    dtr.FileType = "pdf";
                                                }
                                                else if (strFileExt == ".html")
                                                {
                                                    dtr.FileType = "html";
                                                }
                                                else if (strFileExt == ".png" || strFileExt == ".jpg")
                                                {
                                                    dtr.FileType = "image";
                                                }
                                            }
                                        }
                                        if (MDVUtility.ToStr(dtr.FileType).IndexOf("image") >= 0)
                                        {
                                            byte[] imgByteArr = imageToPdf(file);
                                            docsToPrintArr.Add(imgByteArr);
                                        }
                                        else if (MDVUtility.ToStr(dtr.FileType).IndexOf("pdf") >= 0)
                                        {
                                            docsToPrintArr.Add(file);
                                        }
                                        else if (MDVUtility.ToStr(dtr.FileType).IndexOf("html") >= 0)
                                        {
                                            MemoryStream memoryStream = new System.IO.MemoryStream(file);
                                            byte[] eduDoc = memoryStream.ToArray();
                                            string htmlEduDoc = Encoding.ASCII.GetString(eduDoc);
                                            byte[] mergedByteEduDoc = ConvertHtmlToPdf(htmlEduDoc);

                                            docsToPrintArr.Add(mergedByteEduDoc);
                                        }
                                    }
                                }
                                else if (byteArr != null)
                                {

                                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(dtr.FileType)) && MDVUtility.ToStr(dtr.FileType).IndexOf("image") >= 0)
                                    {
                                        byte[] imgByteArr = imageToPdf(byteArr);
                                        docsToPrintArr.Add(imgByteArr);
                                    }
                                    else if (!string.IsNullOrEmpty(MDVUtility.ToStr(dtr.FileType)) && MDVUtility.ToStr(dtr.FileType).IndexOf("pdf") >= 0)
                                    {
                                        docsToPrintArr.Add(byteArr);
                                    }
                                    else if (!string.IsNullOrEmpty(MDVUtility.ToStr(dtr.FileType)) && MDVUtility.ToStr(dtr.FileType).IndexOf("html") >= 0)
                                    {
                                        MemoryStream memoryStream = new System.IO.MemoryStream(byteArr);
                                        byte[] eduDoc = memoryStream.ToArray();
                                        string htmlEduDoc = Encoding.ASCII.GetString(eduDoc);
                                        byte[] mergedByteEduDoc = ConvertHtmlToPdf(htmlEduDoc);

                                        docsToPrintArr.Add(mergedByteEduDoc);
                                    }
                                    // when file type is empty
                                    else if (string.IsNullOrEmpty(MDVUtility.ToStr(dtr.FileType)))
                                    {
                                        int index = dtr.FilePath.LastIndexOf(".");
                                        string strFileExt = string.Empty;
                                        if (index > -1)
                                        {
                                            strFileExt = "." + dtr.FilePath.Substring(index + 1);
                                            if (strFileExt == ".pdf")
                                            {
                                                docsToPrintArr.Add(byteArr);
                                            }
                                            else if (strFileExt == ".html")
                                            {
                                                MemoryStream memoryStream = new System.IO.MemoryStream(byteArr);
                                                byte[] eduDoc = memoryStream.ToArray();
                                                string htmlEduDoc = Encoding.ASCII.GetString(eduDoc);
                                                byte[] mergedByteEduDoc = ConvertHtmlToPdf(htmlEduDoc);

                                                docsToPrintArr.Add(mergedByteEduDoc);

                                            }
                                            else if (strFileExt == "image")
                                            {
                                                byte[] imgByteArr = imageToPdf(byteArr);
                                                docsToPrintArr.Add(imgByteArr);
                                            }
                                        }
                                    }
                                }
                            }

                            byte[] MergedByteArr = concatAndAddContentToPrint(docsToPrintArr);

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            js.MaxJsonLength = Int32.MaxValue;
                            var response = new
                            {
                                status = true,
                                MergedContent = Convert.ToBase64String(MergedByteArr.ToArray())
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                AmendmentNoteCount = 0,
                                Message = "Record not found."
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
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "No Document Found."
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string FillMyComputerDocumentsForFax(string FileName = "", string FileType = "", string AttachedFilesStream = "", HttpFileCollection files = null)
        {
            try
            {
                List<byte[]> docsToPrintArr = new List<byte[]>();

                if (!string.IsNullOrEmpty(AttachedFilesStream))
                {

                    byte[] attachedFilesByteArr = Convert.FromBase64String(AttachedFilesStream);
                    docsToPrintArr.Add(attachedFilesByteArr);
                }

                foreach (string item in files)
                {
                    HttpPostedFile file = files[item];
                    if (file != null)
                    {
                        byte[] currentFileStream = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            currentFileStream = binaryReader.ReadBytes(file.ContentLength);
                        }

                        if (file.ContentType.IndexOf("image") >= 0)
                        {
                            byte[] imgByteArr = imageToPdf(currentFileStream);
                            docsToPrintArr.Add(imgByteArr);
                        }
                        else if (file.ContentType.IndexOf("pdf") >= 0)
                        {
                            docsToPrintArr.Add(currentFileStream);
                        }
                        else if (file.ContentType.IndexOf("html") >= 0)
                        {
                            MemoryStream memoryStream = new System.IO.MemoryStream(currentFileStream);
                            byte[] eduDoc = memoryStream.ToArray();
                            string htmlEduDoc = Encoding.ASCII.GetString(eduDoc);
                            byte[] mergedByteEduDoc = ConvertHtmlToPdf(htmlEduDoc);

                            docsToPrintArr.Add(mergedByteEduDoc);
                        }
                    }
                }
                byte[] MergedByteArr = concatAndAddContentToPrint(docsToPrintArr);

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                js.MaxJsonLength = Int32.MaxValue;
                var response = new
                {
                    status = true,
                    MergedContent = Convert.ToBase64String(MergedByteArr.ToArray())
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        private string SearchPatientInformationSubmission(long PatientID, string Status, int PageNumber, int RowsPerPage)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = null;

                obj = BLLPatientObj.LoadPatientInformationSubmission(0, PatientID, Status, PageNumber, RowsPerPage);


                if (obj != null && obj.Data != null)
                {
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.PatientPortalDocument.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientInformationSubmissionCount = dsPatient.Tables[dsPatient.PatientPortalDocument.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPatient.PatientPortalDocument.Rows[0][dsPatient.PatientPortalDocument.RecordCountColumn.ColumnName],
                            PatientInformationSubmissionLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientPortalDocument.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientInformationSubmissionCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientInformationSubmissionCount = 0,
                        Message = "Record not found."
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UpdatePatientInformationSubmission(string fieldsJSON, long Id)
        {
            try
            {
                if (Id >= 0)
                {
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = null;
                    obj = BLLPatientObj.LoadPatientInformationSubmission(Id, 0);
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.PatientPortalDocument.TableName].Rows.Count > 0)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                        foreach (DSPatient.PatientPortalDocumentRow dr in dsPatient.Tables[dsPatient.PatientPortalDocument.TableName].Rows)
                        {
                            if (SearchedfieldsJSON.ContainsKey("Comments") && !string.IsNullOrEmpty(SearchedfieldsJSON["Comments"]))
                            {
                                dr[dsPatient.PatientPortalDocument.CommentsColumn] = MDVUtility.ToStr(SearchedfieldsJSON["Comments"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("Comments") && string.IsNullOrEmpty(SearchedfieldsJSON["Comments"]))
                            {
                                dr[dsPatient.PatientPortalDocument.CommentsColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("FileName") && !string.IsNullOrEmpty(SearchedfieldsJSON["FileName"]))
                            {
                                dr[dsPatient.PatientPortalDocument.FileNameColumn] = MDVUtility.ToStr(SearchedfieldsJSON["FileName"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("FileName") && string.IsNullOrEmpty(SearchedfieldsJSON["FileName"]))
                            {
                                dr[dsPatient.PatientPortalDocument.FileNameColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("FileType") && !string.IsNullOrEmpty(SearchedfieldsJSON["FileType"]))
                            {
                                dr[dsPatient.PatientPortalDocument.FileTypeColumn] = SearchedfieldsJSON["FileType"];
                            }
                            else if (SearchedfieldsJSON.ContainsKey("FileType") && string.IsNullOrEmpty(SearchedfieldsJSON["FileType"]))
                            {
                                dr[dsPatient.PatientPortalDocument.FileTypeColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("PatientId") && !string.IsNullOrEmpty(SearchedfieldsJSON["PatientId"]))
                            {
                                dr[dsPatient.PatientPortalDocument.PatientIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["PatientId"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("PatientId") && string.IsNullOrEmpty(SearchedfieldsJSON["PatientId"]))
                            {
                                dr[dsPatient.PatientPortalDocument.PatientIdColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("Reason") && !string.IsNullOrEmpty(SearchedfieldsJSON["Reason"]))
                            {
                                dr[dsPatient.PatientPortalDocument.ReasonColumn] = MDVUtility.ToStr(SearchedfieldsJSON["Reason"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("Reason") && string.IsNullOrEmpty(SearchedfieldsJSON["Reason"]))
                            {
                                dr[dsPatient.PatientPortalDocument.ReasonColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("Status") && !string.IsNullOrEmpty(SearchedfieldsJSON["Status"]))
                            {
                                dr[dsPatient.PatientPortalDocument.StatusColumn] = MDVUtility.ToStr(SearchedfieldsJSON["Status"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("Status") && string.IsNullOrEmpty(SearchedfieldsJSON["Status"]))
                            {
                                dr[dsPatient.PatientPortalDocument.StatusColumn] = DBNull.Value;
                            }

                            if (SearchedfieldsJSON.ContainsKey("Url") && !string.IsNullOrEmpty(SearchedfieldsJSON["Url"]))
                            {
                                dr[dsPatient.PatientPortalDocument.UrlColumn] = MDVUtility.ToStr(SearchedfieldsJSON["Url"]);
                            }
                            else if (SearchedfieldsJSON.ContainsKey("Url") && string.IsNullOrEmpty(SearchedfieldsJSON["Url"]))
                            {
                                dr[dsPatient.PatientPortalDocument.UrlColumn] = DBNull.Value;
                            }

                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }

                        BLObject<DSPatient> objPatient = null;

                        objPatient = BLLPatientObj.UpdatePatientInformationSubmission(dsPatient);

                        if (objPatient.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = AppPrivileges.Update_Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objPatient.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient Information not found."
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

        /// <summary>
        /// Fills the emergency contact.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string FillPatientInformationSubmission(long Id)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(Id)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientInformationSubmission(Id, 0);
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.PatientPortalDocument.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.PatientPortalDocument.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                            {
                                { "txtFileName", MDVUtility.ToStr(dr[dsPatient.PatientPortalDocument.FileNameColumn.ColumnName])},
                                { "txtFileType", MDVUtility.ToStr(dr[dsPatient.PatientPortalDocument.FileTypeColumn.ColumnName])},
                                { "txtUrl", MDVUtility.ToStr(dr[dsPatient.PatientPortalDocument.UrlColumn.ColumnName])},
                                { "ddlDocumentProvider", MDVUtility.ToStr(dr[dsPatient.PatientPortalDocument.DocumentProviderIdColumn.ColumnName])}
                            };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientInformationSubmissionFill_JSON = js.Serialize(keyValues)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = AppPrivileges.No_Record_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

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

        private string SavePatientInformationSubmission(string fieldsJSON, long PatientId, long PatPortalDocId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatient dsDocument = new DSPatient();
                DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                dr.PatientId = PatientId;
                dr.Documentid = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
                {
                    dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                    dr.ViewBy = MDVUtility.ToStr(SearchedfieldsJSON["ddlAssignUserto_text"]);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]))
                {
                    dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseId"]);
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                    dr.VisitId = MDVUtility.ToInt64(SearchedfieldsJSON["hfVisitId"]);

                string dtpDOS = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);

                if (!string.IsNullOrEmpty(dtpDOS))
                {
                    dr.DOS = MDVUtility.ToDateTime(dtpDOS);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpReceivedOn"]))
                {
                    dr.ReceivedOn = MDVUtility.ToDateTime(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpReceivedOn"]));
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentSource"]))
                {
                    dr.DocumentSourceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentSource"]);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOtherDocumentSource"]))
                {
                    dr.OtherDocumentSource = MDVUtility.ToStr(SearchedfieldsJSON["txtOtherDocumentSource"]);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtNarrativeReference"]))
                {
                    dr.NarrativeReference = MDVUtility.ToStr(SearchedfieldsJSON["txtNarrativeReference"]);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtReferenceLink"]))
                {
                    dr.ReferenceLink = MDVUtility.ToStr(SearchedfieldsJSON["txtReferenceLink"]);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentProvider"]))
                {
                    dr.DocumentProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentProvider"]);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFileType"]))
                {
                    dr.FileType = MDVUtility.ToStr(SearchedfieldsJSON["txtFileType"]);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFileName"]))
                {
                    dr.FilePath = MDVUtility.ToStr(SearchedfieldsJSON["txtFileName"]);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtUrl"]))
                {
                    dr.Url = MDVUtility.ToStr(SearchedfieldsJSON["txtUrl"]);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                {
                    dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                }

                dr.Pages = 1;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                dsDocument.PatientDocument.AddPatientDocumentRow(dr);

                var keyValues = new Dictionary<string, string>
                {
                    { "Status", "Accepted"}
                };
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var PatientInformationSubmissionFill_JSON = js.Serialize(keyValues);

                #region Database Insertion
                BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument);

                if (obj.Data != null)
                {
                    UpdatePatientInformationSubmission(PatientInformationSubmissionFill_JSON, PatPortalDocId);
                }
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message,
                        MedicalDocumentId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.PatDocIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion
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

        private string FillPatientInformationViewer(string UrlPath, string FileType)
        {
            if (!String.IsNullOrEmpty(UrlPath))
            {
                byte[] file;
                string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                UrlPath = FilePath + UrlPath;
                using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        file = reader.ReadBytes((int)stream.Length);
                    }
                }
                if (file != null)
                {
                    var keyValues = new Dictionary<string, string>
                    {
                        { "Base64FileStream", Convert.ToBase64String(file)},
                        { "FileType", FileType},
                    };
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    js.MaxJsonLength = Int32.MaxValue;
                    var response = new
                    {
                        status = true,
                        DocumentLoad_JSON = js.Serialize(keyValues),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            else
            {
                var response = new
                {
                    status = false
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        /// <summary>
        /// Load DOS on Scan Screen 
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string GetPatientVisitDOS(Int64 PatientId)
        {
            List<PatientVisitDOS> obj = BLLPatientObj.GetPatientVisitDOS(PatientId);
            var response = new
            {
                status = true,

                PatientDOS = obj,
                TotalDOS = obj.Count
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

        private string LoadPatientDocumentExpiryAlert(long PatientID)
        {
            try
            {
                List<PatientDocumentModel> documentsList = null;
                BLObject<List<PatientDocumentModel>> obj = BLLPatientObj.LoadPatientDocumentExpiryAlert(MDVSession.Current.AppUserId, PatientID);
                if (obj.Data != null)
                {
                    documentsList = obj.Data;
                    if (documentsList.Count > 0)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            DocumentCount = documentsList.Count,
                            PatientDocumentExpiry_JSON = js.Serialize(documentsList)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = AppPrivileges.No_Record_Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        DocumentCount = 0,
                        Message = obj.Message,
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string InsertPatientDocumentExpiryAlert(string PatDocIds)
        {
            if (!string.IsNullOrEmpty(PatDocIds))
            {
                try
                {
                    DataTable dtPatDocIds = new DataTable();
                    dtPatDocIds = MDVUtility.ConvertCommaSepatedValuesToDataTable(PatDocIds);
                    DateTime CreatedOn = DateTime.Now;
                    long UserId = MDVSession.Current.AppUserId;
                    BLObject<string> obj = new BLLPatient().InsertPatientDocumentExpiryAlert(UserId, ref dtPatDocIds, "ExpiryAlert", CreatedOn);
                    if (obj != null && obj.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Save_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = AppPrivileges.Save_Error_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                catch (Exception ex)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVCustomException.HumanReadableMessage(ex.Message)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = AppPrivileges.Save_Error_Message
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        private string SearchDocumentTags(Int64 TagId, int PageNumber, int RowsPerPage)
        {
            try
            {
                List<DocumentTag> obj = BLLDocumentObj.GetDocumentsTag(TagId, PageNumber, RowsPerPage);
                if (obj != null)
                {
                    if (obj.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            DocumentTags = obj,
                            TotalTagsCount = obj.Count,
                            iTotalDisplayRecords = obj[0].RecordCount
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            TotalTagsCount = 0,

                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        TotalTagsCount = 0,
                        Message = "Record not found."
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        private string AddNewTagDocument(string fieldsJSON)
        {
            try
            {
                DocumentTag model = new DocumentTag();
                string lResult = string.Empty;
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                string NewTagsValue = string.Empty;
                NewTagsValue = MDVUtility.ToStr(SearchedfieldsJSON["txtTagName"]);
                model.Name = NewTagsValue;
                lResult = BLLDocumentObj.AddNewtagDocument(model);


                if (!lResult.Equals("Tag " + NewTagsValue + " Already Exists"))
                {
                    var response = new
                    {
                        status = true,
                        message = "Successfully Added."
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = false,
                        Message = lResult
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
        public string CheckForPrivacy(Int64 PatientId, Int64 PatDocId)
        {
            try
            {
                List<DocumentPrivacyModel> obj = BLLPatientObj.CheckForPrivacy(PatientId, PatDocId);
                var response = new
                {
                    status = true,

                    DocPasswordInfo = obj,
                    DocPasswordInfoCount = obj.Count
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public string DocPasswordMatch(string Password, Int64 PatDocId)
        {
            try
            {
                List<DocumentPrivacyModel> obj = BLLPatientObj.DocPasswordMatch(Password, PatDocId);
                if (obj.Count > 0)
                {
                    var response = new
                    {
                        status = true,

                        DocPasswordInfo = obj,
                        DocPasswordInfoCount = obj.Count
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Password is incorrect or You donot have Access to the document."
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string ChangeOrRemovePassword(Int64 PatDocID, string OldPassword, string NewPassword, string ConfirmNewPassword)
        {
            try
            {
                List<DocumentPrivacyModel> objMatch = BLLPatientObj.DocPasswordMatch(OldPassword, PatDocID);
                if (objMatch.Count > 0)
                {
                    if (NewPassword == ConfirmNewPassword)
                    {
                        List<DocumentPrivacyModel> obj = BLLPatientObj.ChangeOrRemovePassword(PatDocID, OldPassword, NewPassword);
                        if (obj.Count == 0)
                        {
                            var response = new
                            {
                                status = true,
                                Message = "Password changed/removed successfully.",
                                DocPasswordInfo = obj,
                                DocPasswordInfoCount = obj.Count
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Unable to change password."
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "New Password & Confirm Password donot match."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Password is incorrect or You do not have Access to the document."
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string DOCPasswordSave(string PasswordJSON, string fieldsJSON, Int64 PatDocId, Int64 PatientId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var PasswordfieldsJSON = ser.Deserialize<dynamic>(PasswordJSON);

                string Password = "";
                if (PasswordfieldsJSON.ContainsKey("SetPassword") && !string.IsNullOrEmpty(PasswordfieldsJSON["SetPassword"]))
                {
                    Password = MDVUtility.ToStr(PasswordfieldsJSON["SetPassword"]);
                }

                List<DocumentPrivacyModel> obj = BLLPatientObj.DOCPasswordSave(Password, PatDocId);
                string UserAccess = SaveDocumentUserAccess(PasswordJSON, MDVUtility.ToStr(PatDocId));
                string SendUsersAccessMessage = SendDocumentAccessMessageViewer(PatientId, PasswordJSON, fieldsJSON);

                var response = new
                {
                    status = true,

                    DocPasswordInfo = obj,
                    DocPasswordInfoCount = obj.Count
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        #endregion

        public string DeleteDocumentTag(long TagID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(TagID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLDocumentObj.DeleteTagDocuments(MDVUtility.ToStr(TagID));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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
        private string UpdatePatientDocumentSignReviewed(string PatientDocId, Int64 PatientId, string FolderName)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = null;
                if (PatientDocId != "")
                {

                    obj = BLLPatientObj.LoadPatientDocument(PatientDocId, PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "", "1");
                    dsPatient = obj.Data;
                    BLObject<DSPatient> objPatientDocument = null;

                    objPatientDocument = BLLPatientObj.UpdatePatientDocument(dsPatient, PatientId, FolderName
                    , "1", "1", "1");

                    if (objPatientDocument.Data != null)
                    {

                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = objPatientDocument.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Patient Document not found."
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
        public string ActiveInActiveDocumentTag(long TagID, string TagName, bool IsActive)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(TagID)))
                {

                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {

                    DocumentTag model = new DocumentTag()
                    {
                        TagId = MDVUtility.ToStr(TagID),
                        Name = TagName,
                        IsActive = IsActive
                    };

                    BLObject<string> obj = BLLDocumentObj.ActiveInActiveTagDocuments(model);


                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = "Successfully Updated."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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
        public string GetTagDocumentsByName(string name)
        {

            List<DocumentTag> obj = BLLDocumentObj.GetTagDocumentsByName(name);
            var response = new
            {
                status = true,
                DocumentTags = obj,
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }
        public string GetNotesByDocument(Int64 PatDocId)
        {
            try
            {

                List<Notes> obj;
                obj = BLLClinicalObj.loadPatientDocumentClinical_Notes(0, 0, 0, 1, 15, PatDocId);


                if (obj.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        NotesLoad_JSON = obj,
                        ClinicalNotesCount = obj.Count
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Record not found.",
                        ClinicalNotesCount = obj.Count
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #region Service Command Handler
        /// <summary>
        /// Handle the Basic Free Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "DOWNLOAD_PATIENT_DOCUMENT":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strPatDocIds = context.Request["PatDocIds"];
                        string strConvertType = context.Request["ConvertType"];
                        string strJSONData = DownloadPatientDocument(PatientID, strPatDocIds, strConvertType);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "INTERNAL_EXPORT_PATIENT_DOCUMENT":
                    {
                        string strJSONData = "";
                        try
                        {
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            Int64 CurrentPatientId = MDVUtility.ToInt64(context.Request["CurrentPatientId"]);
                            List<CommonSearch> FoldersList = new List<CommonSearch>();
                            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                            FoldersList = ser.Deserialize<List<CommonSearch>>(MDVUtility.ToStr(context.Request["FolderList"]));
                            string DocumentIds = context.Request["DocumentIds"];
                            if (PatientID <= 0)
                            {
                                throw new Exception("Unable to export documents. Please select patient account number.");
                            }

                            strJSONData = SavePatientDocumentDoc(PatientID, FoldersList, DocumentIds, CurrentPatientId);
                        }
                        catch (Exception ex)
                        {
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SEARCH_PATIENT_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "SEARCH")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["PatientDocumentData"];
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            string strPatDocIds = context.Request["PatDocIds"];
                            string strFileStream = context.Request["bFileStream"];
                            Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                            Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                            string IsReviewed = MDVUtility.ToStr(context.Request["IsReviewed"]);
                            Int64 NoteId = MDVUtility.ToInt64(context.Request["NoteId"]);

                            if (IsReviewed == "undefined")
                                IsReviewed = "";

                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(context.Request["AdvancePaymentId"])))
                                strJSONData = SearchPatientDocument(fieldsJSON, PatientID, PageNumber, RowsPerPage, IsReviewed, NoteId, strPatDocIds, strFileStream, MDVUtility.ToStr(context.Request["AdvancePaymentId"]));
                            else
                                strJSONData = SearchPatientDocument(fieldsJSON, PatientID, PageNumber, RowsPerPage, IsReviewed, NoteId, strPatDocIds, strFileStream);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "INSERT_PATIENT_DOCUMENT_MULTIPLE_FOLDERS":
                    {

                        
                        string fieldsJSON = context.Request["PatientDocumentData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string DocumentID = MDVUtility.ToStr(context.Request["DocumentID"]);
                        string IsMessage = MDVUtility.ToStr(context.Request["IsMessage"]);
                        string IsReviewed = MDVUtility.ToStr(context.Request["IsReviewed"]);
                        string IsSigned = MDVUtility.ToStr(context.Request["IsSigned"]);
                        string CurrentForlderId = MDVUtility.ToStr(context.Request["CurrentFolderID"]);
                        string CurrentForlderName = MDVUtility.ToStr(context.Request["CurrentForlderName"]);
                        List<CommonSearch> FoldersList = new List<CommonSearch>();
                        List<CommonSearch> AddedNewFoldersList = new List<CommonSearch>();
                        List<CommonSearch> deletedNewFolderList = new List<CommonSearch>();
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        FoldersList = ser.Deserialize<List<CommonSearch>>(MDVUtility.ToStr(context.Request["FolderList"]));
                        deletedNewFolderList = ser.Deserialize<List<CommonSearch>>(MDVUtility.ToStr(context.Request["deletedNewFolderList"]));
                        AddedNewFoldersList = ser.Deserialize<List<CommonSearch>>(MDVUtility.ToStr(context.Request["AddedNewFoldersList"]));
                        string strJSONData = InsertPatientDocumentMultipleFolders(fieldsJSON, PatientID, DocumentID, IsReviewed, IsMessage, IsSigned, FoldersList, CurrentForlderId, CurrentForlderName,deletedNewFolderList, AddedNewFoldersList);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["PatientDocumentData"];
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            string DocumentID = MDVUtility.ToStr(context.Request["DocumentID"]);
                            string IsMessage = MDVUtility.ToStr(context.Request["IsMessage"]);
                            string IsReviewed = MDVUtility.ToStr(context.Request["IsReviewed"]);
                            string IsSigned = MDVUtility.ToStr(context.Request["IsSigned"]);
                            string CurrentForlderId = MDVUtility.ToStr(context.Request["CurrentFolderID"]);
                            strJSONData = UpdatePatientDocument(fieldsJSON, PatientID, DocumentID, CurrentForlderId, IsReviewed, IsMessage, IsSigned);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_DOCUMENT_FROM_NOTES":
                    {
                        string fileStream = context.Request["notes"];
                        string CustomFormNameForDoc = MDVUtility.ToStr(context.Request["CustomFormNameForDoc"]);
                        string strJSONData = UpdatePatientDocumentFromNotes(CustomFormNameForDoc, fileStream);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_DOCUMENT_FROM_CLAIM":
                    {
                        long visitId = MDVUtility.ToInt32(context.Request["VisitId"]);
                        Int32  PatDocId= MDVUtility.ToInt32(context.Request["PatDocId"]);
                        string strJSONData = UpdatePatientDocumentFromClaim(PatDocId, visitId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_DOCUMENT_ACTIVE_INACTIVE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            string DocumentID = MDVUtility.ToStr(context.Request["DocumentID"]);
                            Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                            strJSONData = UpdatePatientDocumentIsActive(PatientID, DocumentID, IsActive);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "GET_AUDIOFILE":
                    {
                        int VisitID = MDVUtility.ToInt32(context.Request["VisitID"]);
                        string strJSONData = GetAudioFile(VisitID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PATIENT_DOCUMENT_BULKSIGN":
                    {
                        string strJSONData = string.Empty;
                        try
                        {
                            string fieldsJSON = context.Request["PatientDocumentData"];
                            strJSONData = SavePatientDocumentDocBulkSign(fieldsJSON);
                        }
                        catch (Exception ex)
                        {
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "ANNOTATE_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        try
                        {
                            string Base64 = context.Request["Base64"];
                            string Annotations = context.Request["Annotations"];
                            strJSONData = AnnotateDocument(Base64, Annotations);
                        }
                        catch (Exception ex)
                        {
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_ANNOTATED_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        try
                        {
                            string Base64 = context.Request["Base64"];
                            long PatDocID = MDVUtility.ToLong(context.Request["PatDocID"]);
                            strJSONData = SaveAnnotateDocument(Base64, PatDocID);
                        }
                        catch (Exception ex)
                        {
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PATIENT_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "ADD")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            try
                            {
                                string patientId = "";
                                string refModuleName = string.IsNullOrEmpty(context.Request["RefModuleName"]) == true ? "" : MDVUtility.ToStr(context.Request["RefModuleName"]);
                                Int64 TransitionId = string.IsNullOrEmpty(context.Request["TransitionId"]) == true ? 0 : MDVUtility.ToInt64(context.Request["TransitionId"]);
                                Int64 OrderSetReferralId = string.IsNullOrEmpty(context.Request["OrderSetReferralId"]) == true ? 0 : MDVUtility.ToInt64(context.Request["OrderSetReferralId"]);
                                Int64 VisitId = string.IsNullOrEmpty(context.Request["VisitId"]) == true ? 0 : MDVUtility.ToInt64(context.Request["VisitId"]);
                                if (context.Request.Files.Count > 0)
                                {
                                    string fieldsJSON = context.Request["PatientDocumentData"];
                                    Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                                    List<CommonSearch> FoldersList = new List<CommonSearch>();
                                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    FoldersList = ser.Deserialize<List<CommonSearch>>(MDVUtility.ToStr(context.Request["FolderList"]));
                                    //HttpPostedFile file = context.Request.Files[0];
                                    patientId = context.Request["PatientID"];

                                    if (string.IsNullOrEmpty(patientId))
                                    {
                                        throw new Exception("Unable to save patient image.");
                                    }
                                    string PasswordJSON = context.Request["PasswordJSON"];
                                    string FileName = context.Request["FileName"];
                                    string fileType = context.Request["fileType"];

                                    strJSONData = SavePatientDocumentDoc(fieldsJSON, PatientID, context.Request.Files, FoldersList, null, context.Request["scanFile"], fileType, MDVUtility.ToStr(context.Request["advancePaymentId"]), FileName, refModuleName, TransitionId, null, OrderSetReferralId, VisitId, null, PasswordJSON);

                                    //if (!string.IsNullOrEmpty(MDVUtility.ToStr(context.Request["advancePaymentId"])))
                                    //    strJSONData = SavePatientDocument(fieldsJSON, MDVUtility.ToInt64(patientId), context.Request.Files, null, null, MDVUtility.ToStr(context.Request["advancePaymentId"]), "", "", 0, null, OrderSetReferralId, VisitId);
                                    //else if (OrderSetReferralId > 0)
                                    //{
                                    //    strJSONData = SaveOrderSetReferralDocument(fieldsJSON, context.Request.Files, OrderSetReferralId, null, null, null, refModuleName);
                                    //}
                                    //else
                                    //    strJSONData = SavePatientDocument(fieldsJSON, MDVUtility.ToInt64(patientId), context.Request.Files, null, null, null, null, refModuleName, TransitionId, null, OrderSetReferralId, VisitId);
                                }

                                else if (context.Request["scanFile"] != null)
                                {
                                    string fieldsJSON = context.Request["PatientDocumentData"];
                                    Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                                    string fileType = context.Request["fileType"];
                                    string ext = "jpg";
                                    string FileName = "";
                                    if (!string.IsNullOrWhiteSpace(fileType))
                                    {
                                        ext = fileType.Split('/')[1];
                                    }
                                    if (string.IsNullOrWhiteSpace(context.Request["FileName"]))
                                    {
                                        string dt = DateTime.Now.ToShortDateString();
                                        string tm = DateTime.Now.ToShortTimeString();
                                        string fName = dt + " " + tm.Replace(" ", "");
                                        fName = fName.Replace("/", "-").Replace(":", "");

                                        var mnth = fName.Split('-')[0];
                                        var day = fName.Split('-')[1];
                                        var year = fName.Split('-')[2];
                                        mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                                        day = day.Length == 1 ? "0" + day : day;
                                        var mnt = year.Split(' ')[1];
                                        year = year.Split(' ')[0];
                                        mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                                        //fName = mnth + "-" + day + "-" + year + " " + mnt + '.' + ext;
                                        fName = mnth + "." + day + "." + year + '.' + ext;
                                        FileName = fName;
                                    }
                                    else
                                        FileName = context.Request["FileName"];

                                    string AttachedDocs = context.Request["AttachedDocs"];
                                    List<CommonSearch> FoldersList = new List<CommonSearch>();
                                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    FoldersList = ser.Deserialize<List<CommonSearch>>(MDVUtility.ToStr(context.Request["FolderList"]));
                                    //HttpPostedFile file = context.Request.Files[0];
                                    patientId = context.Request["PatientID"];
                                    string PasswordJSON = context.Request["PasswordJSON"];

                                    if (string.IsNullOrEmpty(patientId))
                                    {
                                        throw new Exception("Unable to save patient image.");
                                    }

                                    strJSONData = SavePatientDocumentDoc(fieldsJSON, PatientID, null, FoldersList, AttachedDocs, context.Request["scanFile"], fileType, MDVUtility.ToStr(context.Request["advancePaymentId"]), FileName, refModuleName, TransitionId, null, OrderSetReferralId, VisitId, null, PasswordJSON);

                                    //if (!string.IsNullOrEmpty(MDVUtility.ToStr(context.Request["advancePaymentId"])))
                                    //    strJSONData = SavePatientDocument(fieldsJSON, MDVUtility.ToInt64(patientId), context.Request.Files, context.Request["scanFile"], context.Request["fileType"], MDVUtility.ToStr(context.Request["advancePaymentId"]));
                                    //else if (OrderSetReferralId > 0)
                                    //{
                                    //    strJSONData = SaveOrderSetReferralDocument(fieldsJSON, context.Request.Files, OrderSetReferralId, context.Request["scanFile"], context.Request["fileType"], FileName, refModuleName);
                                    //}
                                    //else
                                    //    strJSONData = SavePatientDocument(fieldsJSON, MDVUtility.ToInt64(patientId), context.Request.Files, context.Request["scanFile"], context.Request["fileType"], null, FileName, refModuleName, TransitionId);

                                }
                                // adnan maqbool, for EMR-1853
                                else if (context.Request["notes"] != null)
                                {
                                    string fieldsJSON = context.Request["PatientDocumentData"];
                                    Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                                    string FileName = context.Request["FileName"];
                                    //HttpPostedFile file = context.Request.Files[0];
                                    patientId = context.Request["PatientID"];
                                    string FolderName = context.Request["FolderName"];

                                    if (string.IsNullOrEmpty(patientId))
                                    {
                                        throw new Exception("Unable to save patient image.");
                                    }
                                    strJSONData = SaveSignedDocument(fieldsJSON, MDVUtility.ToInt64(patientId), 0, "", context.Request["notes"], context.Request["fileType"], FileName, FolderName);
                                }
                                else if (context.Request["DirectMSG"] != null)
                                {
                                    string fieldsJSON = context.Request["PatientDocumentData"];
                                    Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                                    string FileName = context.Request["FileName"];
                                    string AttachedDocs = context.Request["AttachedDocs"];
                                    List<CommonSearch> FoldersList = new List<CommonSearch>();
                                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    FoldersList = ser.Deserialize<List<CommonSearch>>(MDVUtility.ToStr(context.Request["FolderList"]));
                                    patientId = context.Request["PatientID"];

                                    if (string.IsNullOrEmpty(patientId))
                                    {
                                        throw new Exception("Unable to save document.");
                                    }
                                    string fileType = context.Request["fileType"];
                                    strJSONData = SavePatientDocumentDoc(fieldsJSON, PatientID, null, FoldersList, AttachedDocs, context.Request["DirectMSG"], fileType, MDVUtility.ToStr(context.Request["advancePaymentId"]), FileName, refModuleName, TransitionId, null, OrderSetReferralId, VisitId, "DirectMSG");
                                }
                            }
                            catch (Exception ex)
                            {
                                strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
                            }
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PATIENT_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "DELETE")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string DocumentID = MDVUtility.ToStr(context.Request["DocumentID"]);
                            string ChildPatientDocId = MDVUtility.ToStr(context.Request["ChildPatientDocId"]);
                            string LinkDocumentId = MDVUtility.ToStr(context.Request["LinkDocumentId"]);
                            strJSONData = DeletePatientDocument(DocumentID, ChildPatientDocId, LinkDocumentId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DEATTACH_CLAIM_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "DELETE")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string DocumentID = MDVUtility.ToStr(context.Request["DocumentID"]);
                            
                            strJSONData = DeAttachClaimDocument(DocumentID);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_DOCUMENT":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strPatDocIds = context.Request["PatDocIds"];
                        string strFileStream = context.Request["bFileStream"];
                        string IsReviewed = context.Request["IsReviewed"] != null ? context.Request["IsReviewed"] : "";
                        if (IsReviewed == "undefined")
                        {
                            IsReviewed = "";
                        }
                        string strJSONData = FillPatientDocument(PatientID, strPatDocIds, strFileStream, IsReviewed);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "GET_PATIENT_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "SEARCH")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["PatientDocumentData"];
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            // string strPatDocIds = context.Request["PatDocIds"];
                            // string strFileStream = context.Request["bFileStream"];
                            // Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                            // Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                            //string IsReviewed = MDVUtility.ToStr(context.Request["IsReviewed"]);
                            Int64 NoteId = MDVUtility.ToInt64(context.Request["NoteId"]);
                            strJSONData = GetPatientDocument(fieldsJSON, PatientID);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_DOCUMENTS":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strPatDocIds = context.Request["PatDocIds"];
                        string strFileStream = context.Request["bFileStream"];
                        string IsReviewed = context.Request["IsReviewed"] != null ? context.Request["IsReviewed"] : "";
                        if (IsReviewed == "undefined")
                        {
                            IsReviewed = "";
                        }
                        string strJSONData = FillPatientDocuments(PatientID, strPatDocIds, strFileStream, IsReviewed);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_DOCUMENTS_MERGED":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strPatDocIds = context.Request["PatDocIds"];
                        string strFileStream = context.Request["bFileStream"];
                        string IsReviewed = context.Request["IsReviewed"] != null ? context.Request["IsReviewed"] : "";
                        if (IsReviewed == "undefined")
                        {
                            IsReviewed = "";
                        }
                        string strJSONData = FillPatientDocumentsMerged(PatientID, strPatDocIds, strFileStream, IsReviewed);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_DOCUMENTS_FOR_FAX":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strPatDocIds = context.Request["PatDocIds"];
                        string strFileStream = context.Request["bFileStream"];
                        string IsReviewed = context.Request["IsReviewed"] != null ? context.Request["IsReviewed"] : "";
                        if (IsReviewed == "undefined")
                        {
                            IsReviewed = "";
                        }
                        string AttchedFilesStream = context.Request["AttachedFilesStream"] != null ? context.Request["AttachedFilesStream"] : "";
                        string strJSONData = FillPatientDocumentsForFax(PatientID, strPatDocIds, strFileStream, IsReviewed, AttchedFilesStream);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_MYCOMPUTER_DOCS_FOR_FAX":
                    {
                        if (context.Request.Files.Count > 0)
                        {
                            string FileName = context.Request["FileName"];
                            string FileType = context.Request["FileType"];
                            string AttchedFilesStream = context.Request["AttchedFilesStream"] != null ? context.Request["AttchedFilesStream"] : "";
                            string strJSONData = FillMyComputerDocumentsForFax(FileName, FileType, AttchedFilesStream, context.Request.Files);
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(strJSONData);
                        }
                    }
                    break;
                case "SEARCH_PATIENT_INFORMATION_SUBMISSION":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "VIEW")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            int PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                            int RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                            string Status = MDVUtility.ToStr(context.Request["Status"]);

                            strJSONData = SearchPatientInformationSubmission(PatientID, Status, PageNumber, RowsPerPage);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PATIENT_INFORMATION_SUBMISSION":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "DELETE")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long Id = MDVUtility.ToInt64(context.Request["Id"]);
                            string fieldsJSON = MDVUtility.ToStr(context.Request["Data"]);
                            strJSONData = UpdatePatientInformationSubmission(fieldsJSON, Id);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_INFORMATION_SUBMISSION":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "VIEW")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long Id = MDVUtility.ToInt64(context.Request["Id"]);
                            strJSONData = FillPatientInformationSubmission(Id);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PATIENT_INFORMATION_SUBMISSION":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "ADD")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long PatientId = MDVUtility.ToInt64(context.Request["PatientId"]);
                            long PatPortalDocId = MDVUtility.ToInt64(context.Request["PatPortalDocId"]);
                            string fieldsJSON = context.Request["PatientDocumentData"];
                            strJSONData = SavePatientInformationSubmission(fieldsJSON, PatientId, PatPortalDocId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_INFORMATION_VIEWER":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string Url = MDVUtility.ToStr(context.Request["Url"]);
                            string FileType = MDVUtility.ToStr(context.Request["FileType"]);
                            strJSONData = FillPatientInformationViewer(Url, FileType);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "GET_VISIT_DOS":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = GetPatientVisitDOS(PatientID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_PATIENT_LINKED_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "SEARCH")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            Int64 MedicalParentDocId = MDVUtility.ToInt64(context.Request["MedicalParentDocId"]);
                            strJSONData = searchPatientLinkedDocument(MedicalParentDocId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_TAG_INFORMATION":
                    {
                        long TagId = MDVUtility.ToInt64(context.Request["TagId"]);
                        int PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        int RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        string fieldsJSON = MDVUtility.ToStr(context.Request["Data"]);
                        string strJSONData = SearchDocumentTags(TagId, PageNumber, RowsPerPage);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "ADD_NEW_TAG":
                    {
                        string fieldsJSON = MDVUtility.ToStr(context.Request["Patient_DocumentTag"]);
                        string strJSONData = AddNewTagDocument(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_DOCUMENT_TAG":
                    {
                        string DocumentTagID = context.Request["TagID"];
                        string strJSONData = DeleteDocumentTag(MDVUtility.ToInt64(DocumentTagID));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "ACTIVEINACTIVE_DOCUMENT_TAG":
                    {
                        string DocumentTagID = MDVUtility.ToStr(context.Request["TagID"]);
                        string TagName = MDVUtility.ToStr(context.Request["TagName"]);
                        bool IsActive = MDVUtility.ToBool(context.Request["IsActive"]);
                        string strJSONData = ActiveInActiveDocumentTag(MDVUtility.ToInt64(DocumentTagID), TagName, IsActive);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "TAG_BY_NAME":
                    {
                        string fieldsJSON = MDVUtility.ToStr(context.Request["TagName"]);
                        string strJSONData = GetTagDocumentsByName(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DOCUMENT_NOTES_LIST":
                    {
                        Int64 patDocId = MDVUtility.ToInt64(context.Request["PatDocId"]);
                        string strJSONData = GetNotesByDocument(patDocId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_DOCUMENT_EXPIRY_ALERT":
                    {
                        long PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = LoadPatientDocumentExpiryAlert(PatientID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "INSERT_PATIENT_DOCUMENT_EXPIRY_ALERT":
                    {
                        string PatDocIds = MDVUtility.ToStr(context.Request["PatDocIds"]);
                        string strJSONData = InsertPatientDocumentExpiryAlert(PatDocIds);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CHECK_FOR_PRIVACY":
                    {
                        Int64 PatientId = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 PatDocId = MDVUtility.ToInt64(context.Request["PatDocID"]);
                        string strJSONData = CheckForPrivacy(PatientId, PatDocId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DOC_PASSWORD_MATCH":
                    {
                        string Password = MDVUtility.ToStr(context.Request["Password"]);
                        Int64 PatDocId = MDVUtility.ToInt64(context.Request["PatDocID"]);
                        string strJSONData = DocPasswordMatch(Password, PatDocId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DOCUMENT_ACTIVITY":
                    {
                        Int64 DocID = MDVUtility.ToInt64(context.Request["PatDocId"]);
                        int PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        int RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        string strJSONData = LoadDocumentActivity(DocID, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "REVIEWED_AND_SIGN_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "REVIEW AND SIGN")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string DocID = MDVUtility.ToStr(context.Request["PatDocId"]);
                            Int64 PatientId = MDVUtility.ToInt64(context.Request["PatientID"]);
                            string FolderName = MDVUtility.ToStr(context.Request["FolderName"]);
                            strJSONData = UpdatePatientDocumentSignReviewed(DocID, PatientId, FolderName);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CHANGE_OR_REMOVE_PASSWORD":
                    {
                        string OldPassword = MDVUtility.ToStr(context.Request["OldPassword"]);
                        string NewPassword = MDVUtility.ToStr(context.Request["NewPassword"]);
                        string ConfirmPassword = MDVUtility.ToStr(context.Request["ConfirmPassword"]);
                        Int64 PatDocID = MDVUtility.ToInt64(context.Request["PatDocID"]);
                        string strJSONData = ChangeOrRemovePassword(PatDocID, OldPassword, NewPassword, ConfirmPassword);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "VIEWER_SAVE_PASSWORD":
                    {
                        string PasswordJSON = MDVUtility.ToStr(context.Request["PasswordJSON"]);
                        string filedsJSON = MDVUtility.ToStr(context.Request["FieldsJSON"]);
                        Int64 PatDocID = MDVUtility.ToInt64(context.Request["PatDocID"]);
                        Int64 PatientId = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = DOCPasswordSave(PasswordJSON, filedsJSON, PatDocID, PatientId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion

        #region Documents

        public string SavePatientDocumentDocMultiple(string fieldsJSON, Int64 PatientID, HttpFileCollection files, string strBase64 = null, string fileType = null, string advancePaymentId = null, string FileName = "", string refModuleName = "", Int64 TransitionId = 0, byte[] docByteArray = null, long OrderSetReferralId = 0, long VisitId = 0)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatient dsDocument = new DSPatient();
                DSMessage dsMessage = new DSMessage();
                int counter = 0;

                if (string.IsNullOrWhiteSpace(strBase64))
                {
                    foreach (string name in files)
                    {
                        HttpPostedFile file = files[name];
                        DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                        dr = SaveImportDocumentMultiple(file, dsDocument, PatientID, fieldsJSON, VisitId, fileType.Split(',')[counter], FileName.Split(',')[counter], advancePaymentId, refModuleName, TransitionId, OrderSetReferralId);
                        dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                        counter += 1;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(strBase64))
                {
                    //foreach (string name in files)
                    //{
                    //    HttpPostedFile file = files[name];
                    DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                    dr = SaveSannnedDocumentMultiple(null, dsDocument, PatientID, fieldsJSON, strBase64, fileType, FileName, advancePaymentId, refModuleName, TransitionId, OrderSetReferralId);
                    dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                    //}
                }

                #region Database Insertion
                BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        MedicalDocumentId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.PatDocIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion
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

        public string SaveAnnotateDocument(string Base64, long PatDocID)
        {
            try
            {
                if (PatDocID <= 0)
                    throw new Exception("Document not selected.");

                BLObject<DSPatient> obj = null;
                DSPatient dsDocumentsDB = new DSPatient();
                obj = BLLPatientObj.searchPatientDocument(PatDocID.ToString(), 0, "", null, null, null, null, "", 0, "", 0);
                if (obj.Data != null)
                {
                    dsDocumentsDB = obj.Data;
                    if (dsDocumentsDB.Tables[dsDocumentsDB.PatientDocumentSearch.TableName].Rows.Count > 0)
                    {
                        DSPatient.PatientDocumentSearchRow dr = (DSPatient.PatientDocumentSearchRow)dsDocumentsDB.Tables[dsDocumentsDB.PatientDocumentSearch.TableName].Rows[0];
                        byte[] currentFileStream = Convert.FromBase64String(Base64);

                        string FilePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", dr.DocumentName, dr.PatientId, dr.FilePath, currentFileStream, true, dr.Url);

                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Document not found",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string AnnotateDocument(string Base64, string Annotations)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var objs = ser.Deserialize<dynamic>(Annotations);
                DocumentAnnotation annotation = new DocumentAnnotation();
                foreach (var item in objs)
                {
                    if (item.ContainsKey("area") && item["area"] != null)
                        annotation.area.Add(ser.Deserialize<area>(item["area"]));
                    else if (item.ContainsKey("highlight") && item["highlight"] != null)
                        annotation.highlight.Add(ser.Deserialize<highlight>(item["highlight"]));
                    else if (item.ContainsKey("drawing") && item["drawing"] != null)
                        annotation.drawing.Add(ser.Deserialize<drawing>(item["drawing"]));
                    else if (item.ContainsKey("strikeout") && item["strikeout"] != null)
                        annotation.strikeout.Add(ser.Deserialize<strikeout>(item["strikeout"]));
                    else if (item.ContainsKey("textbox") && item["textbox"] != null)
                        annotation.textbox.Add(ser.Deserialize<textbox>(item["textbox"]));
                }

                string out_file = string.Empty;
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Base64), true))
                {
                    PdfReader reader = new PdfReader(ms);
                    reader.SelectPages("1-" + reader.NumberOfPages + "");
                    float page_height = reader.GetPageSize(1).Height;
                    float page_width = reader.GetPageSize(1).Width;

                    MemoryStream out_ = new MemoryStream();
                    using (PdfStamper stamper = new PdfStamper(reader, out_))
                    {
                        foreach (var item in annotation.area)
                        {
                            PdfContentByte cb = stamper.GetOverContent(item.page);
                            cb.SetColorStroke(BaseColor.RED);
                            cb.Rectangle(item.x, page_height - (item.y + item.height), item.width, item.height);
                            cb.Stroke();
                            cb.SaveState();

                        }
                        foreach (var item in annotation.textbox)
                        {
                            PdfContentByte cb = stamper.GetOverContent(item.page);
                            cb.BeginText();
                            cb.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL);
                            item.color = item.color.Contains("#") ? item.color : "#" + item.color;
                            cb.SetColorStroke(new BaseColor(System.Drawing.ColorTranslator.FromHtml(item.color)));
                            cb.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml(item.color)));
                            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                            cb.SetFontAndSize(bf, item.size);
                            cb.SetTextMatrix((float)item.x, (float)(page_height - (item.y + item.size)));
                            cb.ShowText(item.content);
                            cb.EndText();
                            cb.SaveState();
                        }
                        foreach (var item in annotation.highlight)
                        {
                            PdfContentByte cb = stamper.GetUnderContent(item.page);
                            item.color = item.color.Contains("#") ? item.color : "#" + item.color;
                            cb.SetColorStroke(new BaseColor(System.Drawing.ColorTranslator.FromHtml(item.color)));
                            cb.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml(item.color)));

                            foreach (var rect in item.rectangles)
                            {
                                cb.Rectangle(rect.x, page_height - (rect.y + rect.height), rect.width, rect.height);
                                cb.FillStroke();
                            }
                            cb.SaveState();
                        }
                        foreach (var item in annotation.strikeout)
                        {
                            PdfContentByte cb = stamper.GetOverContent(item.page);
                            item.color = item.color.Contains("#") ? item.color : "#" + item.color;
                            cb.SetColorStroke(new BaseColor(System.Drawing.ColorTranslator.FromHtml(item.color)));
                            cb.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml(item.color)));
                            foreach (var rect in item.rectangles)
                            {
                                cb.Rectangle(rect.x, page_height - (rect.y), rect.width, 0.2f);
                                cb.FillStroke();
                            }
                            cb.SaveState();
                        }
                        foreach (var item in annotation.drawing)
                        {
                            PdfContentByte cb = stamper.GetOverContent(item.page);
                            item.color = item.color.Contains("#") ? item.color : "#" + item.color;
                            cb.SetColorStroke(new BaseColor(System.Drawing.ColorTranslator.FromHtml(item.color)));
                            cb.SetLineWidth((float)item.width);
                            int wd = 4;
                            foreach (var rect in item.lines)
                            {
                                cb.MoveTo(rect.FirstOrDefault(), page_height - (rect.LastOrDefault() + 10));
                                cb.LineTo(rect.FirstOrDefault(), rect.LastOrDefault() + wd);
                                cb.FillStroke();
                            }
                            cb.SaveState();
                        }
                    }

                    out_file = Convert.ToBase64String(out_.ToArray());
                }

                var response = new
                {
                    status = true,
                    AnnotatedPdf = out_file
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        // Save Patient Document for Bulk Sign Note
        public string SavePatientDocumentDocBulkSign(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                List<NoteSignPDF> lstNoteSignPDF = ser.Deserialize<List<NoteSignPDF>>(fieldsJSON);
                DSPatient dsDocument = new DSPatient();
                if (lstNoteSignPDF != null && lstNoteSignPDF.Any())
                {
                    foreach (NoteSignPDF objNote in lstNoteSignPDF)
                    {
                        DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                        dr = SaveSannnedDocumentNoteSign(dsDocument, MDVUtility.ToInt64(objNote.PatientID), objNote.PDFData, objNote.FileType, objNote.FileName, MDVUtility.ToInt32(objNote.FolderId), objNote.FolderName, objNote.NotesId, objNote.DOS);
                        dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                    }
                }

                #region Database Insertion
                BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        MedicalDocumentId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.PatDocIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    #endregion
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


        public string SavePatientDocumentDoc(string fieldsJSON, Int64 PatientID, HttpFileCollection files, List<CommonSearch> FoldersList, string AttachedDocs = null, string strBase64 = null, string fileType = null, string advancePaymentId = null, string FileName = "", string refModuleName = "", Int64 TransitionId = 0, byte[] docByteArray = null, long OrderSetReferralId = 0, long VisitId = 0, string DirectMSG = null, string PasswordJSON = "")
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                DSPatient dsDocument = new DSPatient();
                DSMessage dsMessage = new DSMessage();
                int counter = 0;

                if (!string.IsNullOrWhiteSpace(DirectMSG))
                {
                    foreach (CommonSearch objFolder in FoldersList)
                    {
                        DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                        dr = SaveSannnedDocument(null, dsDocument, PatientID, fieldsJSON, objFolder, strBase64, AttachedDocs, fileType, FileName, advancePaymentId, refModuleName, TransitionId, OrderSetReferralId, DirectMSG, PasswordJSON);
                        dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                    }
                }
                else if (string.IsNullOrWhiteSpace(strBase64))
                {
                    foreach (string name in files)
                    {
                        bool bFirstLoad = true;
                        int pageCount = 0;
                        foreach (CommonSearch objFolder in FoldersList)
                        {
                            HttpPostedFile file = files[name];
                            DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                            dr = SaveImportDocument(file, dsDocument, PatientID, fieldsJSON, objFolder, bFirstLoad, VisitId, fileType.Split(',')[counter], FileName.Split(',')[counter], advancePaymentId, refModuleName, TransitionId, OrderSetReferralId, PasswordJSON);
                            if (!bFirstLoad)
                            {
                                dr.Pages = pageCount;
                            }
                            else
                            {
                                pageCount = dr.Pages;
                            }
                            dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                            bFirstLoad = false;
                        }
                        counter += 1;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(strBase64))
                {
                    //foreach (string name in files)
                    //{
                    //    HttpPostedFile file = files[name];
                    foreach (CommonSearch objFolder in FoldersList)
                    {
                        DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                        dr = SaveSannnedDocument(null, dsDocument, PatientID, fieldsJSON, objFolder, strBase64, AttachedDocs, fileType, FileName, advancePaymentId, refModuleName, TransitionId, OrderSetReferralId, null, PasswordJSON);
                        dsDocument.PatientDocument.AddPatientDocumentRow(dr);
                    }
                }

                #region Database Insertion
                BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument);
                if (obj.Data != null)
                {
                    string PatDocIds = String.Join(",", dsDocument.Tables[dsDocument.PatientDocument.TableName].AsEnumerable().Select(x => x.Field<int>(dsDocument.PatientDocument.PatDocIdColumn.ColumnName).ToString()).ToArray());
                    string UserAccess = SaveDocumentUserAccess(PasswordJSON, PatDocIds);

                    if (string.IsNullOrWhiteSpace(strBase64))
                    {
                        string SendUsersAccessMessage = SendDocumentAccessMessage(PatientID, PasswordJSON, fieldsJSON);
                    }
                    else
                    {
                        string SendUsersAccessMessage = SendDocumentAccessMessageScan(PatientID, PasswordJSON, fieldsJSON);
                    }
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        MedicalDocumentId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.PatDocIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion
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

        public string SaveDocumentUserAccess(string PasswordJSON, string PatDocId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(PasswordJSON);

                DSPatient ds = new DSPatient();
                DSPatient.UserDocumentAlertRow dur = ds.UserDocumentAlert.NewUserDocumentAlertRow();

                dur.Userid = SearchedfieldsJSON["UserId"];
                dur.PatDOcId = PatDocId;
                dur.ShowPasswordAlert = "false";

                ds.UserDocumentAlert.AddUserDocumentAlertRow(dur);

                BLObject<DSPatient> obj = BLLPatientObj.InsertDocumentUserAccess(ds);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        public string SendDocumentAccessMessage(Int64 PatientId, string PasswordJSON, string fieldJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldJSON);
                var PasswordfieldJSON = ser.Deserialize<dynamic>(PasswordJSON);

                DSMessage dsMessage = new DSMessage();
                string PatientName = "";
                string DocName = "";
                string FolderName = "";
                string DOS = "";
                string PasswordVal = "";
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAccountNumber"]))
                {
                    PatientName = SearchedfieldsJSON["txtAccountNumber"];
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["uploadFilePH"]))
                {
                    DocName = SearchedfieldsJSON["uploadFilePH"];
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFolder_text"]))
                {
                    FolderName = SearchedfieldsJSON["ddlFolder_text"];
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]))
                {
                    DOS = MDVUtility.ToStr(SearchedfieldsJSON["dtpDOS"]).Replace("%2F", "/");
                }
                if (!string.IsNullOrEmpty(PasswordfieldJSON["SetPassword"]))
                {
                    PasswordVal = PasswordfieldJSON["SetPassword"];
                }

                string Message = "Document Privacy Check (" + MDVUtility.GetDateMMDDYYY(DateTime.Now.ToString()) + ")\n\nPatient Detail:         " + PatientName + "\n\nDocument:              " + DocName + "\nFolder:                    " + FolderName + "\nDOS:                      " + DOS + "\n\nPassword:              \"" + PasswordVal + "\"";

                foreach (string User in PasswordfieldJSON["UserId"].ToString().Split(','))
                {
                    DSMessage.UserMessagesRow dr = dsMessage.UserMessages.NewUserMessagesRow();

                    dr.AssignedFromId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);

                    if (!string.IsNullOrEmpty(User))
                        dr.AssignedToId = MDVUtility.ToInt64(User);
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                        dr.AttatchedPatientId = MDVUtility.ToInt64(PatientId);

                    dr.Subject = MDVUtility.ToStr("Document Privacy Check");
                    if (!string.IsNullOrEmpty(Message))
                        dr.MessageDetail = MDVUtility.ToStr(Message);

                    dr.PriorityId = 2;
                    dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.IsRead = false;
                    dr.Entityid = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    dr.MessagerType = "Practice";
                    dr.UniqueNumber = Guid.NewGuid().ToString();
                    dr.UserNameWithPractice = MDVSession.Current.AppUserFullName + " - " + MDVSession.Current.DefaultPracticeName;
                    dr.usernamewithpracticefrom = MDVSession.Current.AppUserFullName + " - " + MDVSession.Current.DefaultPracticeName;
                    dr.PatientLetterId = MDVUtility.ToInt64(0);

                    dsMessage.UserMessages.AddUserMessagesRow(dr);
                }
                #region Database Insertion
                BLObject<DSMessage> obj = BLLMessageObj.InsertPracticeMessage(dsMessage);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserMessageId = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.UserMessagesIdColumn.ColumnName],
                        //AccountNumber = dsMessage.Tables[dsMessage.Patients.TableName].Rows[0][dsMessage.Patients.AccountNumberColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion
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

        public string SendDocumentAccessMessageViewer(Int64 PatientId, string PasswordJSON, string fieldJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldJSON);
                var PasswordfieldJSON = ser.Deserialize<dynamic>(PasswordJSON);

                DSMessage dsMessage = new DSMessage();
                string PatientName = "";
                string DocName = "";
                string FolderName = "";
                string DOS = "";
                string PasswordVal = "";
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientName"]))
                {
                    PatientName = SearchedfieldsJSON["txtPatientName"];
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFileName"]))
                {
                    DocName = SearchedfieldsJSON["txtFileName"];
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFolder_text"]))
                {
                    FolderName = SearchedfieldsJSON["ddlFolder_text"];
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]))
                {
                    DOS = MDVUtility.ToStr(SearchedfieldsJSON["dtpDOS"]).Replace("%2F", "/");
                }
                if (!string.IsNullOrEmpty(PasswordfieldJSON["SetPassword"]))
                {
                    PasswordVal = PasswordfieldJSON["SetPassword"];
                }

                string Message = "Document Privacy Check (" + MDVUtility.GetDateMMDDYYY(DateTime.Now.ToString()) + ")\n\nPatient Detail:         " + PatientName + "\n\nDocument:              " + DocName + "\nFolder:                    " + FolderName + "\nDOS:                      " + DOS + "\n\nPassword:              \"" + PasswordVal + "\"";

                foreach (string User in PasswordfieldJSON["UserId"].ToString().Split(','))
                {
                    DSMessage.UserMessagesRow dr = dsMessage.UserMessages.NewUserMessagesRow();

                    dr.AssignedFromId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);

                    if (!string.IsNullOrEmpty(User))
                        dr.AssignedToId = MDVUtility.ToInt64(User);
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                        dr.AttatchedPatientId = MDVUtility.ToInt64(PatientId);

                    dr.Subject = MDVUtility.ToStr("Document Privacy Check");
                    if (!string.IsNullOrEmpty(Message))
                        dr.MessageDetail = MDVUtility.ToStr(Message);

                    dr.PriorityId = 2;
                    dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.IsRead = false;
                    dr.Entityid = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    dr.MessagerType = "Practice";
                    dr.UniqueNumber = Guid.NewGuid().ToString();
                    dr.UserNameWithPractice = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserFullName);
                    dr.usernamewithpracticefrom = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserFullName);
                    dr.PatientLetterId = MDVUtility.ToInt64(0);

                    dsMessage.UserMessages.AddUserMessagesRow(dr);
                }
                #region Database Insertion
                BLObject<DSMessage> obj = BLLMessageObj.InsertPracticeMessage(dsMessage);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserMessageId = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.UserMessagesIdColumn.ColumnName],
                        //AccountNumber = dsMessage.Tables[dsMessage.Patients.TableName].Rows[0][dsMessage.Patients.AccountNumberColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion
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

        public string SendDocumentAccessMessageScan(Int64 PatientId, string PasswordJSON, string fieldJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldJSON);
                var PasswordfieldJSON = ser.Deserialize<dynamic>(PasswordJSON);

                DSMessage dsMessage = new DSMessage();
                string PatientName = "";
                string DocName = "";
                string FolderName = "";
                string DOS = "";
                string PasswordVal = "";
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAccountNumber"]))
                {
                    PatientName = SearchedfieldsJSON["txtAccountNumber"];
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txt_fileNameforSave"]))
                {
                    DocName = SearchedfieldsJSON["txt_fileNameforSave"];
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFolder_text"]))
                {
                    FolderName = SearchedfieldsJSON["ddlFolder_text"];
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]))
                {
                    DOS = MDVUtility.ToStr(SearchedfieldsJSON["dtpDOS"]).Replace("%2F", "/");
                }
                if (!string.IsNullOrEmpty(PasswordfieldJSON["SetPassword"]))
                {
                    PasswordVal = PasswordfieldJSON["SetPassword"];
                }

                string Message = "Document Privacy Check (" + MDVUtility.GetDateMMDDYYY(DateTime.Now.ToString()) + ")\n\nPatient Detail:         " + PatientName + "\n\nDocument:              " + DocName + "\nFolder:                    " + FolderName + "\nDOS:                      " + DOS + "\n\nPassword:              \"" + PasswordVal + "\"";

                foreach (string User in PasswordfieldJSON["UserId"].ToString().Split(','))
                {
                    DSMessage.UserMessagesRow dr = dsMessage.UserMessages.NewUserMessagesRow();

                    dr.AssignedFromId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);

                    if (!string.IsNullOrEmpty(User))
                        dr.AssignedToId = MDVUtility.ToInt64(User);
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                        dr.AttatchedPatientId = MDVUtility.ToInt64(PatientId);

                    dr.Subject = MDVUtility.ToStr("Document Privacy Check");
                    if (!string.IsNullOrEmpty(Message))
                        dr.MessageDetail = MDVUtility.ToStr(Message);

                    dr.PriorityId = 2;
                    dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.IsRead = false;
                    dr.Entityid = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    dr.MessagerType = "Practice";
                    dr.UniqueNumber = Guid.NewGuid().ToString();
                    dr.UserNameWithPractice = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserFullName);
                    dr.usernamewithpracticefrom = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserFullName);
                    dr.PatientLetterId = MDVUtility.ToInt64(0);

                    dsMessage.UserMessages.AddUserMessagesRow(dr);
                }
                #region Database Insertion
                BLObject<DSMessage> obj = BLLMessageObj.InsertPracticeMessage(dsMessage);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserMessageId = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.UserMessagesIdColumn.ColumnName],
                        //AccountNumber = dsMessage.Tables[dsMessage.Patients.TableName].Rows[0][dsMessage.Patients.AccountNumberColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion
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
        public DSPatient.PatientDocumentRow SaveSannnedDocumentMultiple(HttpPostedFile file, DSPatient dsDocument, Int64 PatientID, string fieldsJSON, string strBase64, string fileType = "", string FileName = "", string advancePaymentId = null, string refModuleName = null, Int64 TransitionId = 0, Int64 OrderSetReferralId = 0)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
            DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
            dr.PatientId = PatientID;
            dr.Documentid = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
            {
                dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                dr.ViewBy = MDVUtility.ToStr(SearchedfieldsJSON["ddlAssignUserto_text"]);
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]))
            {
                string dtpDOS = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);

                if (!string.IsNullOrEmpty(dtpDOS))
                    dr.DOS = MDVUtility.ToDateTime(dtpDOS);
            }
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                dr.ClaimId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaim"]);
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCase"]))
                dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlCase"]);

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                dr.VisitId = MDVUtility.ToInt64(SearchedfieldsJSON["hfVisitId"]);

            byte[] currentFileStream = Convert.FromBase64String(strBase64);


            //dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
            dr.FileType = fileType;
            if (FileName == "")
                dr.FilePath = Guid.NewGuid().ToString() + "." + fileType.Split('/')[1];//Utility.ToStr(SearchedfieldsJSON["txtDescription"]);
            else
                dr.FilePath = FileName.Split('.')[0];// +"." + fileType.Split('/')[1];
            MemoryStream ms = new MemoryStream(currentFileStream);

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
            {
                dr.Comments = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
            }

            if (SearchedfieldsJSON.ContainsKey("chkReviewed"))
            {
                dr.ChkReviewed = MDVUtility.ToStr(SearchedfieldsJSON["chkReviewed"]) == "True" ? true : false;

                if (dr.ChkReviewed == true)
                {
                    dr.ReviewBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ReviewDate = DateTime.Now;
                }
            }

            if (fileType == "application/pdf")
                dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
            else
                dr.Pages = 1;
            dr.IsActive = true;
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentPriority"]))
            {
                dr.DocPriorityID = SearchedfieldsJSON["ddlDocumentPriority"];
            }
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(advancePaymentId))
                dr.AdvPaymentId = MDVUtility.ToInt64(advancePaymentId);

            if (!string.IsNullOrEmpty(refModuleName))
            {
                dr.RefModuleName = refModuleName;
            }
            else
            {
                dr[dsDocument.PatientDocument.RefModuleNameColumn] = DBNull.Value;
            }

            if (TransitionId > 0)
            {
                dr.TransitionId = TransitionId;
            }
            else
            {
                dr[dsDocument.PatientDocument.TransitionIdColumn] = DBNull.Value;
            }

            if (OrderSetReferralId > 0)
            {
                dr.OrderSetReferralId = OrderSetReferralId;
            }
            else
            {
                dr[dsDocument.PatientDocument.OrderSetReferralIdColumn] = DBNull.Value;
            }
            string FilePath = CommonFunc.SaveDocumentToFolder(file, "Patient Document", MDVUtility.ToStr(SearchedfieldsJSON["ddlFolder_text"]), PatientID, FileName, currentFileStream);
            dr.Url = FilePath;
            return dr;
        }

        public DSPatient.PatientDocumentRow SaveSannnedDocument(HttpPostedFile file, DSPatient dsDocument, Int64 PatientID, string fieldsJSON, CommonSearch objFolder, string strBase64, string AttachedDocs = null, string fileType = "", string FileName = "", string advancePaymentId = null, string refModuleName = null, Int64 TransitionId = 0, Int64 OrderSetReferralId = 0, string DirectMSG = null, string PasswordJSON = null)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
            var PasswordfieldsJSON = ser.Deserialize<dynamic>(PasswordJSON);
            DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
            dr.PatientId = PatientID;
            dr.Documentid = objFolder.Value;// MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);
            if (SearchedfieldsJSON.ContainsKey("ddlAssignUserto") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
            {
                dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                dr.ViewBy = MDVUtility.ToStr(SearchedfieldsJSON["ddlAssignUserto_text"]);
            }
            if (!string.IsNullOrWhiteSpace(AttachedDocs))
            {
                dr.AttachedDocs = AttachedDocs;
            }
            dr.IsCalendar = MDVUtility.ToStr(SearchedfieldsJSON.ContainsKey("rdCalendar") && SearchedfieldsJSON["rdCalendar"]) == "True" ? true : false;
            if (SearchedfieldsJSON.ContainsKey("dtpDOS") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOS"]) && dr.IsCalendar == true)
            {
                string dtpDOS = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);

                if (!string.IsNullOrEmpty(dtpDOS))
                    dr.DOS = MDVUtility.ToDateTime(dtpDOS);
            }
            if (SearchedfieldsJSON.ContainsKey("ddlDOS") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDOS"]) && dr.IsCalendar == false)
            {
                dr.DOS = MDVUtility.ToDateTime(SearchedfieldsJSON["ddlDOS"]);
            }

            if (string.IsNullOrWhiteSpace(DirectMSG))
            {
                if (SearchedfieldsJSON.ContainsKey("ddlClaim") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                    dr.ClaimId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaim"]);
                if (SearchedfieldsJSON.ContainsKey("ddlCase") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlCase"]))
                    dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlCase"]);

                if (SearchedfieldsJSON.ContainsKey("ddlClaim") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                    dr.VisitId = MDVUtility.ToInt64(SearchedfieldsJSON["hfVisitId"]);
            }

            byte[] currentFileStream = Convert.FromBase64String(strBase64);


            //dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
            dr.FileType = fileType;
            if (FileName == "")
                dr.FilePath = Guid.NewGuid().ToString() + "." + fileType.Split('/')[1];//Utility.ToStr(SearchedfieldsJSON["txtDescription"]);
            else
                dr.FilePath = FileName;//.Split('.')[0];// +"." + fileType.Split('/')[1];
            MemoryStream ms = new MemoryStream(currentFileStream);

            if (SearchedfieldsJSON.ContainsKey("txtComments") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
            {
                dr.Comments = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
            }
            if (SearchedfieldsJSON.ContainsKey("chkReviewed") && SearchedfieldsJSON.ContainsKey("chkReviewed"))
            {
                dr.ChkReviewed = MDVUtility.ToStr(SearchedfieldsJSON["chkReviewed"]) == "True" ? true : false;

                if (dr.ChkReviewed == true)
                {
                    dr.ReviewBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ReviewDate = DateTime.Now;
                }
            }

            if (fileType == "application/pdf")
                dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
            else
                dr.Pages = 1;
            if (SearchedfieldsJSON.ContainsKey("ddlDocumentPriority") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentPriority"]))
            {
                dr.DocPriorityID = MDVUtility.ToLong(SearchedfieldsJSON["ddlDocumentPriority"]);
            }
            if (SearchedfieldsJSON.ContainsKey("dtpExpirtyDate") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpExpirtyDate"]))
            {
                string dtpExpirtyDate = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpExpirtyDate"]);

                if (!string.IsNullOrEmpty(dtpExpirtyDate))
                    dr.ExpiryDate = MDVUtility.ToDateTime(dtpExpirtyDate);
            }
            if (SearchedfieldsJSON.ContainsKey("txtTagName") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTagName"]))
            {
                dr.TagId = MDVUtility.ToInt64(SearchedfieldsJSON["hfDocumentTagId"]);
            }
            dr.IsActive = true;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(advancePaymentId))
                dr.AdvPaymentId = MDVUtility.ToInt64(advancePaymentId);

            if (!string.IsNullOrEmpty(refModuleName))
            {
                dr.RefModuleName = refModuleName;
            }
            else
            {
                dr[dsDocument.PatientDocument.RefModuleNameColumn] = DBNull.Value;
            }

            if (TransitionId > 0)
            {
                dr.TransitionId = TransitionId;
            }
            else
            {
                dr[dsDocument.PatientDocument.TransitionIdColumn] = DBNull.Value;
            }

            if (OrderSetReferralId > 0)
            {
                dr.OrderSetReferralId = OrderSetReferralId;
            }
            else
            {
                dr[dsDocument.PatientDocument.OrderSetReferralIdColumn] = DBNull.Value;
            }
            string FilePath = CommonFunc.SaveDocumentToFolder(file, "Patient Document", objFolder.Name, PatientID, FileName, currentFileStream);
            dr.Url = FilePath;
            dr.DocPassword = PasswordfieldsJSON["SetPassword"];
            dr.PasswordCreatedById = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
            return dr;
        }

        public DSPatient.PatientDocumentRow SaveSannnedDocumentNoteSign(DSPatient dsDocument, Int64 PatientID, string strBase64, string fileType, string FileName, int FolderId, string Folder, string NotesId, string DOS)
        {
            DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
            dr.PatientId = PatientID;
            dr.Documentid = GetDocumentId(Folder);
            byte[] currentFileStream = Convert.FromBase64String(strBase64);
            dr.FileType = fileType;
            if (FileName == "")
                dr.FilePath = Guid.NewGuid().ToString() + "." + fileType.Split('/')[1];
            else
                dr.FilePath = FileName;
            dr.ReviewBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ReviewDate = DateTime.Now;
            if (fileType == "application/pdf")
                dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
            else
                dr.Pages = 1;
            dr.IsActive = true;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;
            string FilePath = CommonFunc.DocumentToFolderSave(null, "Patient Document", Folder, PatientID, FileName, "pdf", currentFileStream);
            dr.Url = FilePath;
            dr.TransitionId = Convert.ToInt64(NotesId == "" ? "0" : NotesId);
            if (!string.IsNullOrEmpty(DOS))
            {
                dr.DOS = MDVUtility.ToDateTime(DOS);
            }
            if (Folder == "" || Folder == "Progress Notes")
            {
                dr.RefModuleName = "Progress Notes";
            }
            else if (Folder == "Custom Form")
            {
                dr.RefModuleName = "Custom Form";
            }
            else if (Folder.ToLower() == "phone encounters")
            {
                dr.RefModuleName = "Phone Encounters";
            }
            return dr;
        }

        public DSPatient.PatientDocumentRow SaveImportDocumentMultiple(HttpPostedFile file, DSPatient dsDocument, Int64 PatientID, string fieldsJSON, Int64 VisitId = 0, string fileType = "", string FileName = "", string advancePaymentId = null, string refModuleName = "", Int64 TransitionId = 0, Int64 OrderSetReferralId = 0)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
            DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
            dr.PatientId = PatientID;


            dr.Documentid = MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolder"]);

            if (VisitId > 0)
            { // Visit Id directly sent to cover Phone encounter audio file scenerios
                dr.VisitId = Convert.ToInt32(VisitId);
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
            {
                dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                dr.ViewBy = MDVUtility.ToStr(SearchedfieldsJSON["ddlAssignUserto_text"]);
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]))
            {
                dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseId"]);
            }
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                dr.VisitId = MDVUtility.ToInt64(SearchedfieldsJSON["hfVisitId"]);


            string dtpDOS = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);

            if (!string.IsNullOrEmpty(dtpDOS))
            {
                dr.DOS = MDVUtility.ToDateTime(dtpDOS);
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpReceivedOn"]))
            {
                dr.ReceivedOn = MDVUtility.ToDateTime(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpReceivedOn"]));
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentSource"]))
            {
                dr.DocumentSourceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentSource"]);
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOtherDocumentSource"]))
            {
                dr.OtherDocumentSource = MDVUtility.ToStr(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtOtherDocumentSource"]));
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtNarrativeReference"]))
            {
                dr.NarrativeReference = MDVUtility.ToStr(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtNarrativeReference"]));
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtReferenceLink"]))
            {
                dr.ReferenceLink = MDVUtility.ToStr(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtReferenceLink"]));
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentProvider"]))
            {
                dr.DocumentProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentProvider"]);
            }
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentPriority"]))
            {
                dr.DocPriorityID = SearchedfieldsJSON["ddlDocumentPriority"];
            }
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpExpirtyDate"]))
            {
                dr.ExpiryDate = SearchedfieldsJSON["dtpExpirtyDate"];
            }
            //else
            //{
            //    dr.DocPriorityID = null;
            //}


            if (SearchedfieldsJSON.ContainsKey("chkReviewed"))
            {

                dr.ChkReviewed = MDVUtility.ToStr(SearchedfieldsJSON["chkReviewed"]) == "True" ? true : false;
                if (dr.ChkReviewed == true)
                {
                    dr.ReviewBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ReviewDate = DateTime.Now;
                }
            }

            byte[] currentFileStream = new byte[file.ContentLength];
            int isRead = file.InputStream.Read(currentFileStream, 0, file.ContentLength);

            //dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
            dr.FileType = fileType.Split(',')[0];
            dr.FilePath = FileName.Split('.')[0];
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
            {
                dr.Comments = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
            }
            dr.PhoneEncounterFolder = MDVUtility.ToStr(SearchedfieldsJSON["hfPhoneEncounterFolder"]);
            if (fileType == "application/pdf")
                dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
            else
                dr.Pages = 1;
            dr.IsActive = true;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;


            if (!string.IsNullOrWhiteSpace(advancePaymentId))
                dr.AdvPaymentId = MDVUtility.ToInt64(advancePaymentId);
            if (!string.IsNullOrEmpty(refModuleName))
            {
                dr.RefModuleName = refModuleName;
            }

            if (TransitionId > 0)
            {
                dr.TransitionId = TransitionId;
            }

            if (OrderSetReferralId > 0)
            {
                dr.OrderSetReferralId = OrderSetReferralId;
            }
            string FilePath = CommonFunc.SaveDocumentToFolder(file, "Patient Document", MDVUtility.ToStr(SearchedfieldsJSON["ddlFolder_text"]), PatientID, FileName, null);
            dr.Url = FilePath;
            return dr;
        }

        public DSPatient.PatientDocumentRow SaveImportDocument(HttpPostedFile file, DSPatient dsDocument, Int64 PatientID, string fieldsJSON, CommonSearch objFolder, bool bFirstLoad, Int64 VisitId = 0, string fileType = "", string FileName = "", string advancePaymentId = null, string refModuleName = "", Int64 TransitionId = 0, Int64 OrderSetReferralId = 0, string PasswordJSON = "")
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
            var PasswordfieldsJSON = ser.Deserialize<dynamic>(PasswordJSON);
            DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
            dr.PatientId = PatientID;


            dr.Documentid = objFolder.Value;

            if (VisitId > 0)
            { // Visit Id directly sent to cover Phone encounter audio file scenerios
                dr.VisitId = Convert.ToInt32(VisitId);
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignUserto"]))
            {
                dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignUserto"]);
                dr.ViewBy = MDVUtility.ToStr(SearchedfieldsJSON["ddlAssignUserto_text"]);
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]))
            {
                dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseId"]);
            }
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                dr.VisitId = MDVUtility.ToInt64(SearchedfieldsJSON["hfVisitId"]);

            dr.IsCalendar = MDVUtility.ToStr(SearchedfieldsJSON["rdCalendar"]) == "True" ? true : false;
            string dtpDOS = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpDOS"]);

            if (!string.IsNullOrEmpty(dtpDOS) && dr.IsCalendar == true)
            {
                dr.DOS = MDVUtility.ToDateTime(dtpDOS);
            }
            else if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDOS"]) && dr.IsCalendar == false)
            {
                dr.DOS = MDVUtility.ToDateTime(SearchedfieldsJSON["ddlDOS"]);
            }
            if (SearchedfieldsJSON.ContainsKey("dtpReceivedOn") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpReceivedOn"]))
            {
                dr.ReceivedOn = MDVUtility.ToDateTime(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpReceivedOn"]));
            }

            if (SearchedfieldsJSON.ContainsKey("ddlDocumentSource") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentSource"]))
            {
                dr.DocumentSourceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentSource"]);
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOtherDocumentSource"]))
            {
                dr.OtherDocumentSource = MDVUtility.ToStr(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtOtherDocumentSource"]));
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtNarrativeReference"]))
            {
                dr.NarrativeReference = MDVUtility.ToStr(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtNarrativeReference"]));
            }

            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtReferenceLink"]))
            {
                dr.ReferenceLink = MDVUtility.ToStr(System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtReferenceLink"]));
            }
            if (SearchedfieldsJSON.ContainsKey("ddlDocumentProvider") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentProvider"]))
            {
                dr.DocumentProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentProvider"]);
            }

            byte[] currentFileStream = new byte[file.ContentLength];
            int isRead = file.InputStream.Read(currentFileStream, 0, file.ContentLength);

            //dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
            dr.FileType = fileType.Split(',')[0];
            dr.FilePath = FileName.Split('.')[0];
            if (!string.IsNullOrWhiteSpace(fileType))
            {
                if (FileName.Split('.')[1] == "doc" || FileName.Split('.')[1] == "docx" || FileName.Split('.')[1] == "xls" || FileName.Split('.')[1] == "xlsx" || FileName.Split('.')[1] == "jpg" || FileName.Split('.')[1] == "png" || FileName.Split('.')[1] == "pdf")
                    dr.FilePath = FileName;
            }
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
            {
                dr.Comments = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
            }
            dr.PhoneEncounterFolder = MDVUtility.ToStr(SearchedfieldsJSON["hfPhoneEncounterFolder"]);
            if (SearchedfieldsJSON.ContainsKey("chkReviewed"))
            {

                dr.ChkReviewed = MDVUtility.ToStr(SearchedfieldsJSON["chkReviewed"]) == "True" ? true : false;
                if (dr.ChkReviewed == true)
                {
                    dr.ReviewBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ReviewDate = DateTime.Now;
                }
            }
            if (fileType == "application/pdf")
            {
                if (bFirstLoad)
                    dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
            }
            else
                dr.Pages = 1;
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentPriority"]))
            {
                dr.DocPriorityID = Convert.ToInt64(SearchedfieldsJSON["ddlDocumentPriority"]);
            }
            string dtpExpirtyDate = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["dtpExpirtyDate"]);
            if (!string.IsNullOrEmpty(dtpExpirtyDate))
            {
                dr.ExpiryDate = MDVUtility.ToDateTime(dtpExpirtyDate);
            }
            if (SearchedfieldsJSON.ContainsKey("txtTagName") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTagName"]))
            {
                dr.TagId = MDVUtility.ToInt64(SearchedfieldsJSON["hfDocumentTagId"]);
            }
            if (SearchedfieldsJSON.ContainsKey("hfEOBManualPostingId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfEOBManualPostingId"]))
                {
                dr.EOBManualPostingId = MDVUtility.ToStr(SearchedfieldsJSON["hfEOBManualPostingId"]);
            }
            dr.IsActive = true;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;


            if (!string.IsNullOrWhiteSpace(advancePaymentId))
                dr.AdvPaymentId = MDVUtility.ToInt64(advancePaymentId);
            if (!string.IsNullOrEmpty(refModuleName))
            {
                dr.RefModuleName = refModuleName;
            }

            if (TransitionId > 0)
            {
                dr.TransitionId = TransitionId;
            }

            if (OrderSetReferralId > 0)
            {
                dr.OrderSetReferralId = OrderSetReferralId;
            }

            string FilePath = CommonFunc.SaveDocumentToFolder(file, "Patient Document", objFolder.Name, PatientID, FileName, null);
            dr.Url = FilePath;
            dr.DocPassword = PasswordfieldsJSON["SetPassword"];
            dr.PasswordCreatedById = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
            return dr;
        }

        public string SavePatientDocumentDoc(Int64 PatientID, List<CommonSearch> FoldersList, string DocumentIds, Int64 currentPatientId = 0)
        {
            try
            {
                DSPatient dsDocumentsDB = new DSPatient();
                BLObject<DSPatient> obj = null;
                obj = BLLPatientObj.searchPatientDocument(DocumentIds, PatientID, "", null, null, null, null, "", 0, "", 0, "1");
                dsDocumentsDB = obj.Data;

                if (dsDocumentsDB.Tables[dsDocumentsDB.PatientDocumentSearch.TableName].Rows.Count <= 0 && currentPatientId > 0) // export to other patient
                {
                    obj = BLLPatientObj.searchPatientDocument(DocumentIds, currentPatientId, "", null, null, null, null, "", 0, "", 0, "1");
                    dsDocumentsDB = obj.Data;

                }
                if ((dsDocumentsDB.Tables[dsDocumentsDB.PatientDocumentSearch.TableName].Rows.Count <= 0 && currentPatientId <= 0) || dsDocumentsDB.Tables[dsDocumentsDB.PatientDocumentSearch.TableName].Rows.Count <= 0) // export to other patient
                {
                    obj = BLLPatientObj.searchPatientDocument(DocumentIds, 0, "", null, null, null, null, "", 0, "", 0, "1");
                    dsDocumentsDB = obj.Data;

                }

                if (obj.Data != null)
                {
                    DSPatient dsDocumnets = new DSPatient();
                    if (dsDocumentsDB.Tables[dsDocumentsDB.PatientDocumentSearch.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dtr in dsDocumentsDB.Tables[dsDocumentsDB.PatientDocumentSearch.TableName].Rows)
                        {
                            string UrlPath = dtr["Url"].ToString();
                            byte[] fileByteArray;
                            if (!String.IsNullOrEmpty(UrlPath))
                            {
                                string PatientFilesPath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                                UrlPath = PatientFilesPath + UrlPath;
                                using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                                {
                                    using (var reader = new BinaryReader(stream))
                                    {
                                        fileByteArray = reader.ReadBytes((int)stream.Length);
                                    }
                                }
                            }
                            else
                            {
                                fileByteArray = dtr["FileStream"] as byte[];
                            }
                            if (fileByteArray != null && fileByteArray.Length > 0)
                            {
                                foreach (CommonSearch objFolder in FoldersList)
                                {
                                    DSPatient.PatientDocumentRow dr = dsDocumnets.PatientDocument.NewPatientDocumentRow();
                                    //SaveImportDocument(fileByteArray, dsPatient, PatientID, null, FoldersList[0], false,0, MDVUtility.ToStr(dtr[dsPatient.PatientDocument.FileTypeColumn.ColumnName]), MDVUtility.ToStr(dtr[dsPatient.PatientDocument.fil.ColumnName]));

                                    dr.Pages = MDVUtility.ToInt32(dtr[dsDocumentsDB.PatientDocumentSearch.PagesColumn.ColumnName]);
                                    dr.PatientId = PatientID;
                                    dr.Documentid = objFolder.Value;
                                    //dr.DocumentProviderId = MDVUtility.ToInt32(dtr[dsPatient.PatientDocument.PagesColumn.ColumnName]);
                                    dr.AssignedToId = MDVUtility.ToInt64(dtr[dsDocumentsDB.PatientDocumentSearch.AssignedToIdColumn.ColumnName]);
                                    if (dr.AssignedToId == 0)
                                    {
                                        dr[dsDocumnets.PatientDocument.AssignedToIdColumn.ColumnName] = DBNull.Value;
                                    }
                                    if (dtr[dsDocumentsDB.PatientDocumentSearch.DOSColumn.ColumnName].ToString() == "")
                                    {
                                        dr[dsDocumnets.PatientDocument.DOSColumn.ColumnName] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr.DOS = MDVUtility.ToDateTime(dtr[dsDocumentsDB.PatientDocumentSearch.DOSColumn.ColumnName]);
                                    }
                                    dr.DocPriorityID = MDVUtility.ToInt64(dtr[dsDocumentsDB.PatientDocumentSearch.DocPriorityIdColumn.ColumnName]);
                                    if (dr.DocPriorityID == 0)
                                    {
                                        dr[dsDocumnets.PatientDocument.DocPriorityIDColumn.ColumnName] = DBNull.Value;
                                    }
                                    dr.Comments = MDVUtility.ToStr(dtr[dsDocumentsDB.PatientDocumentSearch.CommentsColumn.ColumnName]);
                                    dr.FilePath = MDVUtility.ToStr(dtr[dsDocumentsDB.PatientDocumentSearch.FilePathColumn.ColumnName]);
                                    dr.FileType = MDVUtility.ToStr(dtr[dsDocumentsDB.PatientDocumentSearch.FileTypeColumn.ColumnName]);

                                    dr.NarrativeReference = MDVUtility.ToStr(dtr[dsDocumentsDB.PatientDocumentSearch.NarrativeReferenceColumn.ColumnName]);
                                    if (dtr[dsDocumentsDB.PatientDocumentSearch.ReceivedOnColumn.ColumnName].ToString() == "")
                                    {
                                        dr[dsDocumnets.PatientDocument.ReceivedOnColumn.ColumnName] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr.ReceivedOn = MDVUtility.ToDateTime(dtr[dsDocumentsDB.PatientDocumentSearch.ReceivedOnColumn.ColumnName]);
                                    }
                                    dr.ClaimNumber = MDVUtility.ToStr(dtr[dsDocumentsDB.PatientDocumentSearch.ClaimNumberColumn.ColumnName]);
                                    dr.CaseId = MDVUtility.ToInt64(dtr[dsDocumentsDB.PatientDocumentSearch.CaseIdColumn.ColumnName]);
                                    if (dtr[dsDocumentsDB.PatientDocumentSearch.ExpiryDateColumn.ColumnName].ToString() == "")
                                    {
                                        dr[dsDocumnets.PatientDocument.ExpiryDateColumn.ColumnName] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr.ExpiryDate = MDVUtility.ToDateTime(dtr[dsDocumentsDB.PatientDocumentSearch.ExpiryDateColumn.ColumnName]);
                                    }
                                    dr.DocumentSourceId = MDVUtility.ToInt64(dtr[dsDocumentsDB.PatientDocumentSearch.DocumentSourceIdColumn.ColumnName]);
                                    dr.DocumentProviderId = MDVUtility.ToInt64(dtr[dsDocumentsDB.PatientDocumentSearch.DocumentProviderIdColumn.ColumnName]);
                                    dr.ReferenceLink = MDVUtility.ToStr(dtr[dsDocumentsDB.PatientDocumentSearch.ReferenceLinkColumn.ColumnName]);
                                    if (dr.CaseId == 0)
                                    {
                                        dr[dsDocumnets.PatientDocument.CaseIdColumn.ColumnName] = DBNull.Value;
                                    }
                                    if (dr.DocumentSourceId == 0)
                                    {
                                        dr[dsDocumnets.PatientDocument.DocumentSourceIdColumn.ColumnName] = DBNull.Value;
                                    }
                                    //if (dr.DocumentProviderId == 0)
                                    //{
                                    //    dr[dsDocumnets.PatientDocument.DocumentProviderIdColumn.ColumnName] = DBNull.Value;
                                    //}

                                    dr.TagId = MDVUtility.ToInt64(dtr[dsDocumentsDB.PatientDocumentSearch.TagIdColumn.ColumnName]);
                                    dr.TagName = MDVUtility.ToStr(dtr[dsDocumentsDB.PatientDocumentSearch.TagNameColumn.ColumnName]);
                                    if (dr.TagId == 0)
                                    {
                                        dr[dsDocumnets.PatientDocument.TagIdColumn.ColumnName] = DBNull.Value;
                                    }
                                    dr.IsActive = true;
                                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                    dr.CreatedOn = DateTime.Now;
                                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                    dr.ModifiedOn = DateTime.Now;

                                    string fileName = MDVUtility.ToStr(dtr[dsDocumentsDB.PatientDocumentSearch.FilePathColumn.ColumnName]);

                                    string fileExt = Path.GetExtension(fileName);
                                    if (fileExt.Length <= 0)
                                    {
                                        switch (dr.FileType.ToUpper())
                                        {
                                            case "AUDIO/MP3":
                                                fileName = fileName + ".mp3";
                                                break;
                                            case "APPLICATION/PDF":
                                                fileName = fileName + ".pdf";
                                                break;
                                            case "IMAGE/JPEG":
                                            case "IMAGE/JPG":
                                                fileName = fileName + ".jpeg";
                                                break;
                                            case "IMAGE/PNG":
                                                fileName = fileName + ".png";
                                                break;
                                            case "IMAGE/GIF":
                                                fileName = fileName + ".gif";
                                                break;
                                            case "IMAGE/BMP":
                                                fileName = fileName + ".bmp";
                                                break;
                                            case "APPLICATION/VND.OPENXMLFORMATS-OFFICEDOCUMENT.SPREADSHEETML.SHEET":
                                                fileName = fileName + ".xlsx";
                                                break;
                                            case "APPLICATION/VND.MS-EXCEL":
                                                fileName = fileName + ".xls";
                                                break;
                                            case "APPLICATION/VND.OPENXMLFORMATS-OFFICEDOCUMENT.WORDPROCESSINGML.DOCUMENT":
                                                fileName = fileName + ".docx";
                                                break;
                                            case "APPLICATION/MSWORD":
                                                fileName = fileName + ".doc";
                                                break;
                                            case "APPLICATION/HTML":
                                                fileName = fileName + ".html";
                                                break;
                                            case "APPLICATION/VND.OPEN":
                                                if (fileExt == ".docx")
                                                    fileName = fileName + ".docx";
                                                else if (fileExt == ".doc")
                                                    fileName = fileName + ".doc";
                                                else if (fileExt == ".xlsx")
                                                    fileName = fileName + ".xlsx";
                                                else if (fileExt == ".xls")
                                                    fileName = fileName + ".xls";
                                                break;
                                        }
                                    }

                                    string filePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", objFolder.Name, PatientID, fileName, fileByteArray);
                                    dr.Url = filePath;

                                    dsDocumnets.PatientDocument.AddPatientDocumentRow(dr);

                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    message = "Document \"" + MDVUtility.ToStr(dtr[dsDocumentsDB.PatientDocumentSearch.FilePathColumn.ColumnName]) + "not exist."
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }

                        }
                    }
                    #region Database Insertion
                    BLObject<DSPatient> objDSPatient = BLLPatientObj.InsertPatientDocument(dsDocumnets);
                    if (objDSPatient.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objDSPatient.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Documents not exists"
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


        private string DownloadPatientDocument(Int64 PatientID, string PatDocIds, string strConvertType)
        {

            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = null;
                obj = BLLPatientObj.searchPatientDocument(PatDocIds, PatientID, "", null, null, null, null, "", 0, "", 0, "1");
                dsPatient = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dtr in dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows)
                        {
                            byte[] file = null;

                            string UrlPath = dtr["Url"].ToString();
                            string filePath = Convert.ToString(dtr["FilePath"]);
                            string fileType = Convert.ToString(dtr["FileType"]);
                            string fileExt = Path.GetExtension(filePath);
                            if (fileExt.Length <= 0)
                            {
                                switch (fileType.ToUpper())
                                {
                                    case "AUDIO/MP3":
                                        filePath = filePath + ".mp3";
                                        break;
                                    case "APPLICATION/PDF":
                                        filePath = filePath + ".pdf";
                                        break;
                                    case "IMAGE/JPEG":
                                    case "IMAGE/JPG":
                                        filePath = filePath + ".jpeg";
                                        break;
                                    case "IMAGE/PNG":
                                        filePath = filePath + ".png";
                                        break;
                                    case "IMAGE/GIF":
                                        filePath = filePath + ".gif";
                                        break;
                                    case "IMAGE/BMP":
                                        filePath = filePath + ".bmp";
                                        break;
                                    case "APPLICATION/VND.OPENXMLFORMATS-OFFICEDOCUMENT.SPREADSHEETML.SHEET":
                                        filePath = filePath + ".xlsx";
                                        break;
                                    case "APPLICATION/VND.MS-EXCEL":
                                        filePath = filePath + ".xls";
                                        break;
                                    case "APPLICATION/VND.OPENXMLFORMATS-OFFICEDOCUMENT.WORDPROCESSINGML.DOCUMENT":
                                        filePath = filePath + ".docx";
                                        break;
                                    case "APPLICATION/MSWORD":
                                        filePath = filePath + ".doc";
                                        break;
                                    case "APPLICATION/HTML":
                                        filePath = filePath + ".html";
                                        break;
                                }
                                dtr["FilePath"] = filePath;
                            }

                            if (!String.IsNullOrEmpty(UrlPath))
                            {
                                string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                                UrlPath = FilePath + UrlPath;
                                using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                                {
                                    using (var reader = new BinaryReader(stream))
                                    {
                                        file = reader.ReadBytes((int)stream.Length);
                                    }
                                }
                            }
                            else
                            {
                                file = dtr["FileStream"] as byte[];
                            }



                            if (!String.IsNullOrEmpty(strConvertType) && strConvertType.Length > 0)
                            {
                                MemoryStream msImage = new MemoryStream(file);
                                System.Drawing.Image innerImage = System.Drawing.Image.FromStream(msImage, true, true);
                                msImage.Close();
                                msImage.Dispose();
                                msImage = null;
                                System.Drawing.Bitmap img = new System.Drawing.Bitmap(innerImage);

                                innerImage.Dispose();

                                MemoryStream msConvertedImage = new MemoryStream();

                                switch (strConvertType.ToUpper())
                                {
                                    case "BMP":
                                        img.Save(msConvertedImage, System.Drawing.Imaging.ImageFormat.Bmp);
                                        file = msConvertedImage.ToArray();
                                        dtr["FilePath"] = filePath.Substring(0, filePath.LastIndexOf('.') - 1) + ".bmp";
                                        break;
                                    case "JPEG":
                                        img.Save(msConvertedImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        file = msConvertedImage.ToArray();
                                        dtr["FilePath"] = filePath.Substring(0, filePath.LastIndexOf('.') - 1) + ".jpeg";
                                        break;
                                    case "PNG":

                                        img.Save(msConvertedImage, System.Drawing.Imaging.ImageFormat.Png);
                                        file = msConvertedImage.ToArray();
                                        dtr["FilePath"] = filePath.Substring(0, filePath.LastIndexOf('.') - 1) + ".png";
                                        break;
                                    case "TIFF":
                                        img.Save(msConvertedImage, System.Drawing.Imaging.ImageFormat.Tiff);
                                        file = msConvertedImage.ToArray();
                                        dtr["FilePath"] = filePath.Substring(0, filePath.LastIndexOf('.') - 1) + ".tiff";
                                        break;
                                    case "PDF":
                                        var pageSize = new Rectangle(0, 0, img.Size.Width, img.Size.Height);
                                        var document = new iTextSharp.text.Document(pageSize, 10, 10, 10, 10);
                                        PdfWriter.GetInstance(document, msConvertedImage).SetFullCompression();
                                        document.Open();

                                        var image = iTextSharp.text.Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        document.Add(image);
                                        document.Close();

                                        file = msConvertedImage.ToArray();
                                        dtr["FilePath"] = filePath.Substring(0, filePath.LastIndexOf('.') - 1) + ".pdf";
                                        break;
                                }
                                img.Dispose();
                                msConvertedImage.Close();
                            }

                            if (file != null)
                            {
                                string strBase64 = Convert.ToBase64String(file);
                                // Add a New Column to Store the Base64 String
                                if (!dtr.Table.Columns.Contains("Base64FileStream"))
                                {
                                    dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dtr["Base64FileStream"] = strBase64;
                            }

                        }


                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        var response = new
                        {
                            status = true,
                            DocumentCount = dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName].Rows.Count,
                            Documents = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocumentSearch.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = obj.Message
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        private string LoadDocumentActivity(Int64 DocID, int PageNumber, int RowsPerPage)
        {

            List<DocumentActivity> obj = BLLDocumentObj.LoadDocumentsActivity(DocID, PageNumber, RowsPerPage);
            var response = new
            {
                status = true,
                DocumentActivity = obj,

                iTotalDisplayRecords = obj[0].RecordCount
            };
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "MM/dd/yyyy hh:mm:ss";
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response, jsonSettings));
        }
        #endregion Documents

    }
}
