using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MDVision.IEHR.Controls.Billing.Payments
{
    public class Bill_PaymentBatch_Detail
    {
        private BLLBilling BLLBillingObj = null;
        public Bill_PaymentBatch_Detail()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Bill_PaymentBatch_Detail _obj = null;
        public static Bill_PaymentBatch_Detail Instance()
        {
            if (_obj == null)
                _obj = new Bill_PaymentBatch_Detail();
            return _obj;
        }
        #endregion

        #region Service Command Handler

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "ADD_PAYMENTBATCH":
                    {
                        string fieldsJSON = context.Request["PaymentBatchData"];
                        string strJSONData = SavePaymentBatch(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "DELETE_PAYMENT_BATCH":
                    {
                        Int64 BatchID = MDVUtility.ToInt32(context.Request["BatchId"]);
                        string strJSONData = DeletePaymentBatch(BatchID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "FILL_PAYMENT_BATCH":
                    {
                        Int64 BatchID = MDVUtility.ToInt64(context.Request["BatchId"]);
                        string strJSONData = FillPaymentBatch(BatchID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "UPDATE_PAYMENT_BATCH":
                    {
                        string fieldsJSON = context.Request["paymentBatchData"];
                        Int64 BatchId = MDVUtility.ToInt(context.Request["BatchId"]);
                        string strJSONData = UpdatePaymentBatch(fieldsJSON, BatchId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "ADD_PAYMENTBATCH_DOCUMENT":
                    {
                        string strJSONData = "";
                        try
                        {
                            if (context.Request.Files.Count > 0)
                            {
                                int BatchID = MDVUtility.ToInt32(context.Request["BatchID"]);
                                string fileName = context.Request["FileName"];
                                string fileType = context.Request["fileType"];
                                fileType = fileType == null ? "application/pdf" : fileType;
                                strJSONData = SavePaymentBatchDocument(BatchID, context.Request.Files, fileName, null, fileType);

                            }
                            else
                            {
                                int BatchID = MDVUtility.ToInt32(context.Request["BatchID"]);
                                string strBase64 = MDVUtility.ToStr(context.Request["base64"]);
                                string fileType = MDVUtility.ToStr(context.Request["fileType"]);
                                string fileName = context.Request["FileName"];
                                strJSONData = SavePaymentBatchDocument(BatchID, context.Request.Files, fileName, strBase64, fileType);
                            }

                        }
                        catch (Exception ex)
                        {
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);

                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "SEARCH_PAYMENT_BATCH_DOCUMENT":
                    {
                        int BatchDocId = MDVUtility.ToInt32(context.Request["BatchDocId"]);
                        int BatchId = MDVUtility.ToInt32(context.Request["BatchId"]);
                        string isFileStream = MDVUtility.ToStr(context.Request["isFileStream"]);
                        string strJSONData = LoadBatchChargeDocument(BatchDocId, BatchId, isFileStream);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "DELETE_PAYMENT_BATCH_DOCUMENT":
                    {
                        Int64 BatchDocId = MDVUtility.ToInt64(context.Request["BatchDocId"]);
                        string strJSONData = DeletePaymentBatchDocument(BatchDocId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "FILL_PAYMENT_BATCH_DOCUMENT":
                    {
                        Int64 BatchDocId = MDVUtility.ToInt64(context.Request["BatchDocId"]);
                        string strJSONData = FillBatchChargeDocument(BatchDocId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "UPDATE_PAYMENT_BATCH_DOCUMENT":
                    {
                        string fieldsJSON = context.Request["batchChargeDocumentData"];
                        Int64 BatchDocId = MDVUtility.ToInt64(context.Request["BatchDocId"]);
                        string strJSONData = UpdatePaymentBatchDocument(fieldsJSON, BatchDocId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "SEARCH_PAYMENT_BY_BATCH":
                    {
                        Int32 BatchId = MDVUtility.ToInt32(context.Request["BatchId"]);
                        string strJSONData = LoadPaymentByBatch(BatchId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }

                case "SEARCH_INSURANCE_PAYMENT_BY_BATCH":
                    {
                        Int32 BatchId = MDVUtility.ToInt32(context.Request["BatchId"]);
                        string strJSONData = LoadInsurancePaymentByBatch(BatchId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
            }
        }
        #endregion


        #region CRUD Private Functions
        private string SavePaymentBatch(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPayment dsPaymentBatch = new DSPayment();
                DSPayment.PaymentBatchRow dr = dsPaymentBatch.PaymentBatch.NewPaymentBatchRow();

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"])))
                    dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtFacility"])))
                    dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtPractice"])))
                    dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpDepositDate"])))
                    dr.DepositDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDepositDate"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpCheckDate"])))
                    dr.CheckDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCheckDate"]);

                dr.CheckNumber = SearchedfieldsJSON["txtCheckNumber"];

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBiller"])))
                    dr.BillerId = MDVUtility.ToLong(SearchedfieldsJSON["ddlBiller"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtPlanAmount"])))
                    dr.PlanAmount = MDVUtility.ToDouble(SearchedfieldsJSON["txtPlanAmount"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtPatientAmount"])))
                    dr.PatientAmount = MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientAmount"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCopayment"])))
                    dr.Copayment = MDVUtility.ToDouble(SearchedfieldsJSON["txtCopayment"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtComments"])))
                    dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBatchStatus"])))
                    dr.BatchStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlBatchStatus"]);
                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsPaymentBatch.PaymentBatch.AddPaymentBatchRow(dr);
                BLObject<DSPayment> obj = BLLBillingObj.InsertPaymentBatch(dsPaymentBatch);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        BatchNumber = dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatch.TableName].Rows[0][dsPaymentBatch.PaymentBatch.PmtBatchNumberColumn.ColumnName],
                        BatchID = dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatch.TableName].Rows[0][dsPaymentBatch.PaymentBatch.PmtBatchIdColumn.ColumnName],
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string DeletePaymentBatch(Int64 BatchID)
        {

            try
            {
                if (BatchID == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.DeletePaymentBatch(BatchID);
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string FillPaymentBatch(Int64 BatchId)
        {
            try
            {

                DSPayment dsPaymentatch = null;
                BLObject<DSPayment> obj = null;
                obj = BLLBillingObj.LoadPaymentBatch(BatchId,"","",0,0,0,0,"");

                dsPaymentatch = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPaymentatch.Tables[dsPaymentatch.PaymentBatch.TableName].Rows.Count > 0)
                    {

                        DataRow dr = dsPaymentatch.Tables[dsPaymentatch.PaymentBatch.TableName].Rows[0];
                        var keyValues = new Dictionary<string, string>
                        {
                            { "hfBatchID", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.PmtBatchIdColumn.ColumnName])},
                            { "txtBatchNumber", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.PmtBatchNumberColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.DescriptionColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.FacilityIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.FacilityNameColumn.ColumnName])},
                            { "txtPractice", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.PracticeNameColumn.ColumnName])},
                            { "hfPractice", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.PracticeIdColumn.ColumnName])},
                            //{ "dtpDepositDate", MDVUtility.ToDateTime(dr[dsPaymentatch.PaymentBatch.DepositDateColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "dtpDepositDate",  dr[dsPaymentatch.PaymentBatch.DepositDateColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsPaymentatch.PaymentBatch.DepositDateColumn.ColumnName]).ToShortDateString()},
                            { "ddlBiller", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.BillerIdColumn.ColumnName])},
                            //{ "dtpCheckDate", MDVUtility.ToDateTime(dr[dsPaymentatch.PaymentBatch.CheckDateColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "dtpCheckDate",  dr[dsPaymentatch.PaymentBatch.CheckDateColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsPaymentatch.PaymentBatch.CheckDateColumn.ColumnName]).ToShortDateString()},
                            { "txtPlanAmount", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.PlanAmountColumn.ColumnName])},
                            { "txtCheckNumber", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.CheckNumberColumn.ColumnName])},
                            { "txtPatientAmount", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.PatientAmountColumn.ColumnName])},
                            { "txtCopayment", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.CopaymentColumn.ColumnName])},
                            { "txtPlanAmountPosted", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.PlanAmountPostedColumn.ColumnName])},
                            { "txtPatientAmountPosted", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.PatientAmountPostedColumn.ColumnName])},
                            { "txtCopayAmountPosted", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.CopayAmountPostedColumn.ColumnName])},
                            { "txtAdjustments", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.AdjustmentAmtColumn.ColumnName])},
                            { "ddlBatchStatus", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.BatchStatusIdColumn.ColumnName])},
                            //{ "dtpStarDate", MDVUtility.ToDateTime(dr[dsPaymentatch.PaymentBatch.StartDateColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "dtpStarDate",  dr[dsPaymentatch.PaymentBatch.StartDateColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsPaymentatch.PaymentBatch.StartDateColumn.ColumnName]).ToShortDateString()},
                            //{ "dtpEndDate", MDVUtility.ToDateTime(dr[dsPaymentatch.PaymentBatch.EndDateColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "dtpEndDate",  dr[dsPaymentatch.PaymentBatch.EndDateColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsPaymentatch.PaymentBatch.EndDateColumn.ColumnName]).ToShortDateString()},
                            { "txtHoursSpent", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.TotalHoursColumn.ColumnName])},
                            { "txtBatchEnteredBy", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.BatchEnteredByColumn.ColumnName])},
                            //{ "txtBatchCreatedon", MDVUtility.ToDateTime(dr[dsPaymentatch.PaymentBatch.CreatedOnColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "txtBatchCreatedon",  dr[dsPaymentatch.PaymentBatch.CreatedOnColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsPaymentatch.PaymentBatch.CreatedOnColumn.ColumnName]).ToShortDateString()},
                            { "txtComments", MDVUtility.ToStr(dr[dsPaymentatch.PaymentBatch.CommentsColumn.ColumnName])},

                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        var response = new
                        {
                            status = true,
                            PaymentBatchCount = dsPaymentatch.Tables[dsPaymentatch.PaymentBatch.TableName].Rows.Count,
                            PaymentBatchLoad_JSON = js.Serialize(keyValues),
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UpdatePaymentBatch(string fieldsJSON, Int64 BatchId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPayment dsPaymentBatch = new DSPayment();
                BLObject<DSPayment> objLoad = BLLBillingObj.LoadPaymentBatch(BatchId, "", "", 0, 0, 0, 0,"");
                dsPaymentBatch = objLoad.Data;
                foreach (DSPayment.PaymentBatchRow dr in dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatch.TableName].Rows)
                {
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"])))
                        dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.DescriptionColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtFacility"])))
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.FacilityIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtPractice"])))
                        dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.PracticeIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpDepositDate"])))
                        dr.DepositDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDepositDate"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.DepositDateColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpCheckDate"])))
                        dr.CheckDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCheckDate"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.CheckDateColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCheckNumber"])))
                        dr.CheckNumber = SearchedfieldsJSON["txtCheckNumber"];
                    else
                        dr[dsPaymentBatch.PaymentBatch.CheckNumberColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBiller"])))
                        dr.BillerId = MDVUtility.ToLong(SearchedfieldsJSON["ddlBiller"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.BillerIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtPlanAmount"])))
                        dr.PlanAmount = MDVUtility.ToDouble(SearchedfieldsJSON["txtPlanAmount"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.PlanAmountColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtPatientAmount"])))
                        dr.PatientAmount = MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientAmount"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.PatientAmountColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCopayment"])))
                        dr.Copayment = MDVUtility.ToDouble(SearchedfieldsJSON["txtCopayment"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.CopaymentColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBatchStatus"])))
                        dr.BatchStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlBatchStatus"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.BatchStatusIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtComments"])))
                        dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                    else
                        dr[dsPaymentBatch.PaymentBatch.CommentsColumn] = DBNull.Value;

                    dr.IsActive = true;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation



                if (dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatch.TableName].Rows.Count > 0)
                {
                    BLObject<DSPayment> obj = BLLBillingObj.UpdatePaymentBatch(dsPaymentBatch);
                    if (obj.Data != null)
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
                        Message = ""
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SavePaymentBatchDocument(Int64 BatchID, HttpFileCollection files, string fileName, string strBase64 = null, string fileType = null)
        {
            try
            {
                int counter = 0;
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                DSPayment dsPaymentBatch = new DSPayment();
                if (strBase64 == null)
                {
                    foreach (string name in files)
                    {
                        HttpPostedFile file = files[name];
                        DSPayment.PaymentBatchDocsRow dr = dsPaymentBatch.PaymentBatchDocs.NewPaymentBatchDocsRow();
                        dr = SavePaymentChargeDocumentFile(dsPaymentBatch, BatchID, file, fileType.Split(',')[counter], fileName.Split(',')[counter], null);
                        dsPaymentBatch.PaymentBatchDocs.AddPaymentBatchDocsRow(dr);
                        counter += 1;
                    }
                }
                else
                {
                    byte[] currentFileStream = Convert.FromBase64String(strBase64);
                        DSPayment.PaymentBatchDocsRow dr = dsPaymentBatch.PaymentBatchDocs.NewPaymentBatchDocsRow();
                        dr = SavePaymentChargeDocumentFile(dsPaymentBatch, BatchID, null, fileType, fileName, currentFileStream);
                        dsPaymentBatch.PaymentBatchDocs.AddPaymentBatchDocsRow(dr);
                }
                #region Database Insertion
                BLObject<DSPayment> obj = BLLBillingObj.InsertPaymentBatchDocument(dsPaymentBatch);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        BatchDocId = dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Rows[0][dsPaymentBatch.PaymentBatchDocs.PmtBthDocIdColumn.ColumnName]
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public DSPayment.PaymentBatchDocsRow SavePaymentChargeScaneDocument(DSPayment dsPaymentBatch, Int64 BatchID, HttpPostedFile file, string ContentType, string FileName)
        {
            DSPayment.PaymentBatchDocsRow dr = dsPaymentBatch.PaymentBatchDocs.NewPaymentBatchDocsRow();

            dr.PmtBatchId = BatchID;
            byte[] currentFileStream = new byte[file.ContentLength];
            int isRead = file.InputStream.Read(currentFileStream, 0, file.ContentLength);
            dr.FileType = ContentType;
            dr.FilePath = FileName.Split('.')[0];
            //MemoryStream ms = new MemoryStream(currentFileStream);
            if (ContentType == "application/pdf")
                dr.Pages = 1;// MDVUtility.getPdfPagesCount(currentFileStream);
            else
                dr.Pages = 1;
            dr.IsActive = true;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;
            dr.Url = CommonFunc.SaveDocumentToFolder(file, "Payment Batch Documents", "Check", BatchID, FileName, null);

            return dr;
        }

        public DSPayment.PaymentBatchDocsRow SavePaymentChargeDocumentFile(DSPayment dsPaymentBatch, Int64 BatchID, HttpPostedFile file, string ContentType, string FileName, byte[] currentFileStream)
        {
            DSPayment.PaymentBatchDocsRow dr = dsPaymentBatch.PaymentBatchDocs.NewPaymentBatchDocsRow();

            dr.PmtBatchId = BatchID;
            if (file != null)
            {
                currentFileStream = new byte[file.ContentLength];
                int isRead = file.InputStream.Read(currentFileStream, 0, file.ContentLength);
                //dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
            }
            dr.FileType = ContentType;
            dr.FilePath = FileName.Split('.')[0];
            if (ContentType == "application/pdf")
                dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
            else
                dr.Pages = 1;
            dr.IsActive = true;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;
            dr.Url = CommonFunc.SaveDocumentToFolder(file, "Payment Batch Documents", "Check", BatchID, FileName, currentFileStream);

            return dr;
        }

        private string LoadBatchChargeDocument(Int32 BatchDocId, Int32 BatchId, string isFileStream = "0")
        {

            try
            {

                DSPayment dsPaymentBatch = null;
                BLObject<DSPayment> obj;
                obj = BLLBillingObj.LoadPaymentBatchDocument(BatchDocId, BatchId ,isFileStream);
                dsPaymentBatch = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Rows)
                        {
                            byte[] byteArr = dr["FileStream"] as byte[];
                            if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                // Add a New Column to Store the Base64 String
                                if (!dr.Table.Columns.Contains("Base64FileStream"))
                                {
                                    dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dr["Base64FileStream"] = strBase64;
                            }
                        }
                        var response = new
                        {
                            status = true,
                            PaymentBatchDocumentCount = dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Rows.Count,
                            PaymentBatchDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName]),
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        private string DeletePaymentBatchDocument(Int64 BatchDocId)
        {

            try
            {
                if (BatchDocId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.DeletePaymentBatchDocument(BatchDocId);
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string FillBatchChargeDocument(Int64 BatchDocId)
        {
            try
            {

                DSPayment dsPaymentBatch = null;
                BLObject<DSPayment> obj = null;
                obj = BLLBillingObj.LoadPaymentBatchDocument(BatchDocId, 0, "1");
                dsPaymentBatch = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Rows.Count > 0)
                    {
                        string LoadPrevious = "0";
                        foreach (DataRow dtr in dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Rows)
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
                                    if (!dtr.Table.Columns.Contains("Base64FileStream"))
                                    {
                                        dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                    }
                                    dtr["Base64FileStream"] = Convert.ToBase64String(file);
                                }
                                LoadPrevious = "1";
                            }
                            else if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                // Add a New Column to Store the Base64 String
                                if (!dtr.Table.Columns.Contains("Base64FileStream"))
                                {
                                    dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dtr["Base64FileStream"] = strBase64;
                                LoadPrevious = "1";
                            }
                        }
                        DataRow dr = dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Rows[0];
                        string fileName = MDVUtility.ToStr(dr[dsPaymentBatch.PaymentBatchDocs.FilePathColumn.ColumnName]);
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
                                strFileExt = "." + MDVUtility.ToStr(dr[dsPaymentBatch.PaymentBatchDocs.FileTypeColumn.ColumnName]).Split('/')[1];
                            }
                        }
                        else
                        {
                            strFileExt = "." + MDVUtility.ToStr(dr[dsPaymentBatch.PaymentBatchDocs.FileTypeColumn.ColumnName]).Split('/')[1];
                        }
                        var keyValues = new Dictionary<string, string>
                        {
                            { "txtLoadPrevious", LoadPrevious},
                            { "txtPaymentCheckNumber", MDVUtility.ToStr(dr[dsPaymentBatch.PaymentBatchDocs.CheckNumberColumn.ColumnName])},
                            { "dpCheckDate", MDVUtility.ToStr(dr[dsPaymentBatch.PaymentBatchDocs.CheckDateColumn.ColumnName])},
                            { "txtPaymentComments", MDVUtility.ToStr(dr[dsPaymentBatch.PaymentBatchDocs.CommentsColumn.ColumnName])},
                            { "Base64FileStream", MDVUtility.ToStr(dr["Base64FileStream"])},
                            { "ddlPaymentAction", MDVUtility.ToStr(dr[dsPaymentBatch.PaymentBatchDocs.ActionIdColumn.ColumnName])},
                            { "ddlPaymentReason", MDVUtility.ToStr(dr[dsPaymentBatch.PaymentBatchDocs.ReasonIdColumn.ColumnName])},
                            { "FileType", MDVUtility.ToStr(dr[dsPaymentBatch.PaymentBatchDocs.FileTypeColumn.ColumnName])},
                            { "txtFileNamePayment",strFileName },
                            { "lnkFileNameExtPayment", strFileExt}

                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        var response = new
                        {
                            status = true,
                            Count = dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Rows.Count,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string UpdatePaymentBatchDocument(string fieldsJSON, Int64 BatchDocId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPayment dsPaymentBatch = new DSPayment();
                BLObject<DSPayment> objLoad = BLLBillingObj.LoadPaymentBatchDocument(BatchDocId, 0,"1");
                dsPaymentBatch = objLoad.Data;
                foreach (DSPayment.PaymentBatchDocsRow dr in dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Rows)
                {
                    dr.CheckNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtPaymentCheckNumber"]);
                    dr.CheckDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dpCheckDate"]);
                    dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtPaymentComments"]);

                    if (MDVUtility.ToInt32(SearchedfieldsJSON["ddlPaymentAction"]) != 0)
                        dr.ActionId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPaymentAction"]);
                    else
                        dr[dsPaymentBatch.PaymentBatchDocs.ActionIdColumn] = DBNull.Value;

                    if (MDVUtility.ToInt32(SearchedfieldsJSON["ddlPaymentReason"]) != 0)
                        dr.ReasonId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPaymentReason"]);
                    else
                        dr[dsPaymentBatch.PaymentBatchDocs.ReasonIdColumn] = DBNull.Value;
                    if (!string.IsNullOrWhiteSpace(SearchedfieldsJSON["txtFileNamePayment"]) && SearchedfieldsJSON.ContainsKey("txtFileNamePayment"))
                    {
                        dr[dsPaymentBatch.PaymentBatchDocs.FilePathColumn] = SearchedfieldsJSON["txtFileNamePayment"] + SearchedfieldsJSON["lnkFileNameExtPayment"];
                    }
                    dr.IsActive = true;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation



                if (dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatchDocs.TableName].Rows.Count > 0)
                {
                    BLObject<DSPayment> obj = BLLBillingObj.UpdatePaymentBatchDocument(dsPaymentBatch);
                    if (obj.Data != null)
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
                        Message = ""
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        private string LoadInsurancePaymentByBatch(Int32 BatchId)
        {
            DSPayment dsPaymentBatch = null;
            BLObject<DSPayment> obj;
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                obj = BLLBillingObj.LoadInsurancePaymentByBatch(BatchId);

                dsPaymentBatch = obj.Data;

                if (obj.Data != null)
                {
                    if (dsPaymentBatch.Tables[dsPaymentBatch.PaymentByBatch.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PaymentByBatchCount = dsPaymentBatch.Tables[dsPaymentBatch.PaymentByBatch.TableName].Rows.Count,
                            PaymentByBatchLoad_JSON = MDVUtility.JSON_DataTable(dsPaymentBatch.Tables[dsPaymentBatch.PaymentByBatch.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PaymentByBatchCount = 0,
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
                        PaymentByBatchCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        private string LoadPaymentByBatch(Int32 BatchId)
        {
            DSPayment dsPaymentBatch = null;
            BLObject<DSPayment> obj;
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                obj = BLLBillingObj.LoadPaymentByBatch(BatchId);

                dsPaymentBatch = obj.Data;

                if (obj.Data != null)
                {
                    if (dsPaymentBatch.Tables[dsPaymentBatch.PaymentByBatch.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PaymentByBatchCount = dsPaymentBatch.Tables[dsPaymentBatch.PaymentByBatch.TableName].Rows.Count,
                            PaymentByBatchLoad_JSON = MDVUtility.JSON_DataTable(dsPaymentBatch.Tables[dsPaymentBatch.PaymentByBatch.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PaymentByBatchCount = 0,
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
                        PaymentByBatchCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        #endregion
    }
}