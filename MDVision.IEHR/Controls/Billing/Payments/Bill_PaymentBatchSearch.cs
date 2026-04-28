using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Billing.Payments
{
    public class Bill_PaymentBatchSearch
    {
        private BLLBilling BLLBillingObj = null;
        public Bill_PaymentBatchSearch()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Bill_PaymentBatchSearch _obj = null;
        public static Bill_PaymentBatchSearch Instance()
        {
            if (_obj == null)
                _obj = new Bill_PaymentBatchSearch();
            return _obj;
        }
        #endregion

        #region Private Functions


        private string LoadPaymentBatches(string fieldsJSON, Int32 BatchId, int PageNumber, int RowsPerPage)
        {
            string batchNumber = null, description = null, enteredBy = null,checkNo=null;
            Int64 practiceId = 0, facilityId = 0, billerId = 0;
            int batchStatusId = 0;
            DSPayment dsPaymentBatch = null;
            BLObject<DSPayment> obj;

            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (SearchedfieldsJSON == null)
                {

                    obj = BLLBillingObj.PaymentBatchSearch(BatchId, batchNumber, description, practiceId, facilityId, billerId, batchStatusId, enteredBy, checkNo);
                }
                else
                {
                    DateTime? dtDepositDate = String.IsNullOrEmpty(SearchedfieldsJSON["dtpDepositDate"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dtpDepositDate"]);
                    DateTime? dtEntryDateFrom = String.IsNullOrEmpty(SearchedfieldsJSON["dpEntryDatefrm"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpEntryDatefrm"]);
                    DateTime? dtEntryDateTo = String.IsNullOrEmpty(SearchedfieldsJSON["dpEntryDateTo"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpEntryDateTo"]);

                    if (SearchedfieldsJSON["txtFacility"] == "")
                        facilityId = 0;
                    else
                        facilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);

                    if (SearchedfieldsJSON["txtPractice"] == "")
                        practiceId = 0;
                    else
                        practiceId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);

                    if (SearchedfieldsJSON["ddlBiller"] == "")
                        billerId = 0;
                    else
                        billerId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBiller"]);

                    if (SearchedfieldsJSON["ddlBatchStatus"] == "")
                        batchStatusId = 0;
                    else
                        batchStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlBatchStatus"]);
                    //if (dtEntryDateTo == null)
                    //    dtEntryDateTo = dtEntryDateFrom;
                    //if (dtEntryDateFrom == null)
                    //    dtEntryDateFrom = dtEntryDateTo;
                    batchNumber = SearchedfieldsJSON["txtBatchNumber"];
                    description = SearchedfieldsJSON["txtDescription"];
                    enteredBy = SearchedfieldsJSON["txtEnteredBy"];
                    checkNo = SearchedfieldsJSON["txtCheck"];
                    obj = BLLBillingObj.PaymentBatchSearch(BatchId, batchNumber, description, practiceId, facilityId, billerId, batchStatusId, enteredBy, checkNo, dtDepositDate, dtEntryDateFrom, dtEntryDateTo, PageNumber, RowsPerPage);

                }

                dsPaymentBatch = obj.Data;

                if (obj.Data != null)
                {
                    if (dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatch.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PaymentBatchCount = dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatch.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPaymentBatch.PaymentBatch.Rows[0][dsPaymentBatch.PaymentBatch.RecordCountColumn.ColumnName],
                            PaymentBatchLoad_JSON = MDVUtility.JSON_DataTable(dsPaymentBatch.Tables[dsPaymentBatch.PaymentBatch.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PaymentBatchCount = 0,
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
                        PaymentBatchCount = 0,
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

        #region Service Command Handler

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {


                case "SEARCH_PAYMENT_BATCH":
                    {
                        Int32 BatchId = MDVUtility.ToInt32(context.Request["BatchId"]);
                        string fieldsJSON = context.Request["PaymentBatchData"];
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);

                        string strJSONData = LoadPaymentBatches(fieldsJSON, BatchId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32((RowsPerPage)));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }

                    break;


            }
        }
        #endregion
    }
}