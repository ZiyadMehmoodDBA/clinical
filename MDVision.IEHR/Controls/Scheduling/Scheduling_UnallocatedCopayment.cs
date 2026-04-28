using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Controls.Patient.Demographics;
using MDVision.Model.Billing.Payments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_UnallocatedCopayment
    {
        private BLLBilling BLLBillingObj = null;
        private BLLPatient BLLPatientObj = null;
        private BLLDocument BLLDocumentObj = null;

        public Scheduling_UnallocatedCopayment()
        {
            BLLBillingObj = new BLLBilling();
            BLLPatientObj = new BLLPatient();
            BLLDocumentObj = new BLLDocument();
        }

        #region Singleton
        private static Scheduling_UnallocatedCopayment _obj = null;
        public static Scheduling_UnallocatedCopayment Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_UnallocatedCopayment();
            return _obj;
        }
        #endregion

        #region Private functions

        private string PostUnAllocatedCoPayment(long unAllocatedCopayId, long visitId)
        {
            try
            {
                BLObject<List<UnAllocatedCopayModel>> objUnAllocatedCopay = BLLBillingObj.LoadUnAllocatedCopay(unAllocatedCopayId, 0, 0, 0, 0, null, null, null);
                if (objUnAllocatedCopay.Data.Count > 0)
                {
                    UnAllocatedCopayModel model = objUnAllocatedCopay.Data.FirstOrDefault(p => p.Status.ToLower() == "unallocated");
                    if (model != null)
                    {
                        DSPayment dsPayment = new DSPayment();
                        DSPayment.PatientPaymentsRow dr = dsPayment.PatientPayments.NewPatientPaymentsRow();

                        //-----------------------  For Copayment Entry  -------------------//


                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.AppointmentId))
                            dr.AppointmentId = MDVUtility.ToInt64(model.AppointmentId);

                        if (!string.IsNullOrEmpty(model.ProviderId))
                        {
                            dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                        }
                        if (!string.IsNullOrEmpty(model.FacilityId))
                        {
                            dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                        }

                        if (!string.IsNullOrEmpty(model.LedgerAccId))
                        {
                            dr.LedgerAccId = MDVUtility.ToInt32(model.LedgerAccId);
                        }

                        dr.PaidAmountDr = MDVUtility.ToDouble(model.CopayAmount);
                        dr.Comments = MDVUtility.ToStr(model.Comments);

                        dr.IsActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;


                        //string time = DateTime.Now.ToString("HH:mm:ss tt");

                        //if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.ReceiptDate)))
                        //    dr.PaymentDate = MDVUtility.ToDateTime(model.ReceiptDate + " " + time);

                        dr.PaymentDate = DateTime.Now;

                        if (!string.IsNullOrEmpty(model.PmtTypeId))
                        {
                            dr.PmtTypeId = MDVUtility.ToInt32(model.PmtTypeId);
                        }

                        if (MDVUtility.ToInt32(model.PmtTypeId) == 2)
                        {
                            //check
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.CheckNo)))
                                dr.CheckNo = model.CheckNo;

                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.CheckDate)))
                                dr.CheckDate = MDVUtility.ToStr(model.CheckDate);

                        }
                        else if (MDVUtility.ToInt32(model.PmtTypeId) == 3)
                        {
                            //credit card
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.CheckNo)))
                                dr.CheckNo = model.CheckNo;

                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.ExpiryDate)))
                                dr.ExpiryDate = MDVUtility.ToStr(model.ExpiryDate);

                            if (!string.IsNullOrEmpty(model.CardTypeId))
                            {
                                dr.CardTypeId = MDVUtility.ToInt32(model.CardTypeId);
                            }
                        }

                        else if (MDVUtility.ToInt32(model.PmtTypeId) == 4)
                        {
                            //Advance Payment
                            long advancePaymentId = 0;
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.AdvPmtId)))
                            {
                                dr.AdvPmtId = MDVUtility.ToInt64(model.AdvPmtId);
                                advancePaymentId = dr.AdvPmtId;
                            }

                            Patient_AdvancePayment.Instance().validateAdvancePayment(dr.PatientId, dr.PaidAmountDr, advancePaymentId);

                        }

                        if (visitId > 0)
                            dr.VisitId = MDVUtility.ToLong(visitId);

                        dsPayment.PatientPayments.AddPatientPaymentsRow(dr);


                        #region Database Insertion
                        // POST Copayment and then Update Unallocated Copayment Status.
                        BLObject<bool> res = BLLBillingObj.PostUnAllocatedCopay(model, dsPayment);
                        if (res.Data)
                        {
                            var response = new
                            {
                                status = true,
                                ReceiptNumber = model.ReceiptNumber,
                                Message = Common.AppPrivileges.Save_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = res.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        #endregion
                    }
                    else
                    {
                        throw new Exception("Record not found.");
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objUnAllocatedCopay.Message
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

        private string LoadUnallocated_Copayment(string fieldsJSON, int PageNumber, int RowsPerPage)
        {
            try
            {
                UnAllocatedCopayModel model = new UnAllocatedCopayModel();

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (!SearchedfieldsJSON.ContainsKey("txtFacility") || SearchedfieldsJSON["txtFacility"] == "")
                    model.FacilityId = null;
                else
                    model.FacilityId = SearchedfieldsJSON["hfFacility"];

                if (!SearchedfieldsJSON.ContainsKey("txtProvider") || SearchedfieldsJSON["txtProvider"] == "")
                    model.ProviderId = null;
                else
                    model.ProviderId = SearchedfieldsJSON["hfProvider"];

                if (!SearchedfieldsJSON.ContainsKey("txtPatientName") || SearchedfieldsJSON["txtPatientName"] == "")
                    model.PatientId = null;
                else
                    model.PatientId = SearchedfieldsJSON["hfPatientId"];

                if (!SearchedfieldsJSON.ContainsKey("txtReceiptNumber") || SearchedfieldsJSON["txtReceiptNumber"] == "")
                    model.ReceiptNumber = null;
                else
                    model.ReceiptNumber = SearchedfieldsJSON["txtReceiptNumber"];

                if (!SearchedfieldsJSON.ContainsKey("AppointmentId") || SearchedfieldsJSON["AppointmentId"] == "")
                    model.AppointmentId = null;
                else
                    model.AppointmentId = SearchedfieldsJSON["AppointmentId"];

                DateTime? dtfrom = String.IsNullOrEmpty(SearchedfieldsJSON["dpStartDate"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpStartDate"]);
                DateTime? dtto = String.IsNullOrEmpty(SearchedfieldsJSON["dpToDate"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpToDate"]);

                model.ReceiptDateFrom = dtfrom;
                model.ReceiptDateTo = dtto;

                List<UnAllocatedCopayModel> UnAllocatedCopayList = null;
                BLObject<List<UnAllocatedCopayModel>> objUnAllocatedCopay;

                objUnAllocatedCopay = BLLBillingObj.LoadUnAllocatedCopay(0, MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.FacilityId), MDVUtility.ToInt64(model.AppointmentId), model.ReceiptNumber, dtfrom, dtto, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage), false);

                UnAllocatedCopayList = objUnAllocatedCopay.Data;
                if (objUnAllocatedCopay.Data != null)
                {
                    if (UnAllocatedCopayList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            UnAllocatedCopayListRecordCount = UnAllocatedCopayList.Count,
                            iTotalDisplayRecords = UnAllocatedCopayList[0].RecordCount,
                            UnAllocatedCopayInfo_JSON = UnAllocatedCopayList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            UnAllocatedCopayListRecordCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objUnAllocatedCopay.Message
                    };
                    return (JsonConvert.SerializeObject(response));
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

        private string LoadUnallocatedCopaymentScheduling(Int64 AppointmentID)
        {
            try
            {
                List<UnAllocatedCopayModel> UnAllocatedCopayList = null;
                BLObject<List<UnAllocatedCopayModel>> objUnAllocatedCopay;

                objUnAllocatedCopay = BLLBillingObj.LoadUnAllocatedCopay(0, 0, 0, 0, AppointmentID, null, null, null, 1, 1000);

                UnAllocatedCopayList = objUnAllocatedCopay.Data;
                if (objUnAllocatedCopay.Data != null)
                {
                    if (UnAllocatedCopayList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            UnAllocatedCopayListRecordCount = UnAllocatedCopayList.Count,
                            iTotalDisplayRecords = UnAllocatedCopayList[0].RecordCount,
                            UnAllocatedCopayInfo_JSON = UnAllocatedCopayList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            UnAllocatedCopayListRecordCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objUnAllocatedCopay.Message
                    };
                    return (JsonConvert.SerializeObject(response));
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
        private string LoadUnallocatedCopayReceiptInfo(long unAllocatedCopayId)
        {
            try
            {
                BLObject<List<UnAllocatedCopayModel>> objUnAllocatedCopay = BLLBillingObj.LoadUnAllocatedCopay(unAllocatedCopayId, 0, 0, 0, 0, null, null, null);

                if (objUnAllocatedCopay.Data != null && objUnAllocatedCopay.Data.Count > 0)
                {
                    UnAllocatedCopayModel model = objUnAllocatedCopay.Data.FirstOrDefault();
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    if (model != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            ReceiptInfo = js.Serialize(model)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objUnAllocatedCopay.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objUnAllocatedCopay.Message
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
        private string SavePatientCoPayment(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                UnAllocatedCopayModel model = new UnAllocatedCopayModel();

                if (SearchedfieldsJSON["txtPaid"] != "")
                {
                    model.PatientId = SearchedfieldsJSON["hfPatientId"];

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfAppointmentId"]))
                        model.AppointmentId = SearchedfieldsJSON["hfAppointmentId"];

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProviderId"]))
                    {
                        model.ProviderId = SearchedfieldsJSON["hfProviderId"];
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacilityId"]))
                    {
                        model.FacilityId = SearchedfieldsJSON["hfFacilityId"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPaidLedger"]))
                    {
                        model.LedgerAccId = SearchedfieldsJSON["ddlPaidLedger"];
                    }
                    model.CopayAmount = SearchedfieldsJSON["txtPaid"];

                    model.Status = "Unallocated";

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dpReceiptDate"]))
                    {
                        model.ReceiptDate = SearchedfieldsJSON["dpReceiptDate"];
                    }

                    model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    model.CreatedOn = DateTime.Now.ToString();
                    model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    model.ModifiedOn = DateTime.Now.ToString();

                    if (SearchedfieldsJSON.ContainsKey("txtComments") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                    {
                        model.Comments = SearchedfieldsJSON["txtComments"];
                    }


                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPaymentType"]))
                    {
                        model.PmtTypeId = SearchedfieldsJSON["ddlPaymentType"];
                    }

                    if (MDVUtility.ToInt32(model.PmtTypeId) == 2)
                    {
                        //check

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCheckNumber"])))
                            model.CheckNo = SearchedfieldsJSON["txtCheckNumber"];

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dpCheckDate"])))
                            model.CheckDate = SearchedfieldsJSON["dpCheckDate"];

                    }
                    else if (MDVUtility.ToInt32(model.PmtTypeId) == 3)
                    {
                        //credit card

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCardNumber"])))
                            model.CheckNo = SearchedfieldsJSON["txtCardNumber"];

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dpExpiryDate"])))
                            model.ExpiryDate = SearchedfieldsJSON["dpExpiryDate"];

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCardType"]))
                        {
                            model.CardTypeId = SearchedfieldsJSON["ddlCardType"];
                        }
                    }

                    else if (MDVUtility.ToInt32(model.PmtTypeId) == 4)
                    {
                        //Advance Payment
                        long advancePaymentId = 0;
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["hfAdvancePaymentId"])))
                        {
                            model.AdvPmtId = SearchedfieldsJSON["hfAdvancePaymentId"];
                            advancePaymentId = MDVUtility.ToLong(model.AdvPmtId);
                        }

                        Patient_AdvancePayment.Instance().validateAdvancePayment(MDVUtility.ToLong(model.PatientId), MDVUtility.ToDouble(model.CopayAmount), advancePaymentId);

                    }
                }

                #region Database Insertion


                UnAllocatedCopayModel obj = BLLBillingObj.SaveUnAllocatedCopay(model);

                var response = new
                {
                    status = true,
                    Message = Common.AppPrivileges.Save_Message,
                    UnAllocatedCopayId = model.UnAllocatedCopayId,
                    ReceiptNumber = model.ReceiptNumber
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

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

        public Int32 GetDocumentId(string FolderName)
        {
            Int32 Documentid = 0;

            DSDocument dsDoc = new DataAccess.DAL.Document.DALDocument().LoadDocument(0, FolderName, 0, "", "", "");
            DataTable dtDocType = dsDoc.Documents;
            if (dtDocType != null && dtDocType.Rows.Count > 0)
            {
                Documentid = MDVUtility.ToInt32(dtDocType.Rows[0]["DocId"]);
            }

            return Documentid;
        }

        public string SaveUnallocatedCopayReceiptDocument(string strBase64FileStream, Int64 PatientID, string FileName, Int64 VisitId = 0, string dtpDOS = "")
        {

            try
            {
                byte[] currentFileStream = System.Convert.FromBase64String(strBase64FileStream);

                DSPatient dsDocument = new DSPatient();
                if (!string.IsNullOrEmpty(dtpDOS))
                    dtpDOS = MDVUtility.ToStr(dtpDOS);
                else
                    dtpDOS = DateTime.Now.ToString();

                //Delete Unallocated Copay Document from DB and Server.

                DeleteUnallocatedCopayDocument(PatientID, FileName, dtpDOS, VisitId);

                DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                dr.PatientId = PatientID;

                Int32 Documentid = 0;
                Documentid = GetDocumentId("Copay Receipt");
                if (Documentid == 0)
                    throw new Exception("Copay Receipt folder not found.");

                if (VisitId > 0)
                    dr.VisitId = Convert.ToInt32(VisitId);
                if (!string.IsNullOrEmpty(dtpDOS))
                    dr.DOS = MDVUtility.ToDateTime(dtpDOS);
                else
                    dr.DOS = DateTime.Now;
                string fileType = "application/pdf";
                dr.FileType = fileType;
                dr.FilePath = FileName;
                if (fileType == "application/pdf")
                    dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
                else
                    dr.Pages = 1;
                dr.IsActive = true;

                dr.Documentid = Documentid;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                string FilePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Copay Receipt", PatientID, FileName + ".pdf", currentFileStream);
                dr.Url = FilePath;
                dsDocument.PatientDocument.AddPatientDocumentRow(dr);

                BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        MedicalDocumentId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.PatDocIdColumn.ColumnName]
                    };
                    return (JsonConvert.SerializeObject(response));
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
            catch (Exception obj)
            {
                var response = new
                {
                    status = false,
                    Message = obj.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }


        private bool DeleteUnallocatedCopayDocument(Int64 PatientID, string FileName, string dtpDOS, Int64 VisitId = 0)
        {
            bool isok = true;
            try
            {
                BLObject<bool> obj = BLLPatientObj.DeleteUnallocatedCopayDocument(PatientID, FileName, dtpDOS, VisitId);
                if (obj.Data && !string.IsNullOrEmpty(obj.Message))
                {
                    string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatientFilesPath"];
                    if (File.Exists(ServerPath + obj.Message))
                    {
                        File.Delete(ServerPath + obj.Message);
                    }
                }
                else if (obj.Message != "")
                    throw new Exception(obj.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isok;

        }
        private string RefundUnAllocatedCopay(Int64 UnAllocatedCopayId)
        {
            try
            {
                var obj = BLLBillingObj.RefundUnAllocatedCopay(UnAllocatedCopayId);

                if (obj == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        UnAllocatedCopayId = UnAllocatedCopayId
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj
                    };
                    return (JsonConvert.SerializeObject(response));
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

        #region Service Command Handler

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_UNALLOCATED_COPAYMENT":
                    {
                        string fieldsJSON = context.Request["PatientCopaymentData"];
                        string strJSONData = SavePatientCoPayment(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "GET_UNALLOCATED_COPAYMENT":
                    {
                        string fieldsJSON = context.Request["CopayReceiptData"];
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadUnallocated_Copayment(fieldsJSON, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32((RowsPerPage)));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "GET_UNALLOCATED_COPAYMENT_SCHEDULING":
                    {
                        long AppointmentID = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        string strJSONData = LoadUnallocatedCopaymentScheduling(AppointmentID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "POST_UNALLOCATED_COPAYMENT":
                    {
                        long VisitId = MDVUtility.ToLong(context.Request["VisitId"]);
                        long UnAllocatedCopayId = MDVUtility.ToLong(context.Request["UnAllocatedCopayId"]);
                        string strJSONData = PostUnAllocatedCoPayment(UnAllocatedCopayId, VisitId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "LOAD_RECEIPT_INFO":
                    {
                        long UnAllocatedCopayId = MDVUtility.ToLong(context.Request["UnAllocatedCopayId"]);
                        string strJSONData = LoadUnallocatedCopayReceiptInfo(UnAllocatedCopayId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PATIENT_UNALLOCATED_COPAY_RECEIPT_DOCUMENT":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string FileName = context.Request["FileName"];
                        Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitId"]);
                        string dtoS = context.Request["dtoS"];

                        string strBase64FileStream = context.Request["pdfFile"];

                        string strJSONData = SaveUnallocatedCopayReceiptDocument(strBase64FileStream, MDVUtility.ToInt64(PatientID), FileName, VisitID, dtoS);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "REFUND_UNALLOCATED_COPAYMENT":
                    {
                        Int64 UnAllocatedCopayId = MDVUtility.ToInt64(context.Request["UnAllocatedCopayId"]);
                        string strJSONData = RefundUnAllocatedCopay(UnAllocatedCopayId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }


        #endregion
    }
}