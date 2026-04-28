using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Billing
{
    public class Bill_ChargeBatchSearch
    {
        private BLLBilling BLLBillingObj = null;
        public Bill_ChargeBatchSearch()
        {
            BLLBillingObj = new BLLBilling();
        }

        #region Singleton
        private static Bill_ChargeBatchSearch _obj = null;
        public static Bill_ChargeBatchSearch Instance()
        {
            if (_obj == null)
                _obj = new Bill_ChargeBatchSearch();
            return _obj;
        }
        #endregion

        #region Private Functions


        private string LoadBatchChargeSearch(string fieldsJSON, Int32 BatchId, int PageNumber, int RowsPerPage)
        {


            string provider,practice;
            string facility;
            string biller;
            string description = null;


            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DateTime? EntryDateFrom = String.IsNullOrEmpty(SearchedfieldsJSON["dpEntryDatefrm"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpEntryDatefrm"]);
                DateTime? EntryDateTo = String.IsNullOrEmpty(SearchedfieldsJSON["dpEntryDateTo"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpEntryDateTo"]);
                DateTime? DOSFrom = String.IsNullOrEmpty(SearchedfieldsJSON["dpDSOFrm"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpDSOFrm"]);
                DateTime? DOSTo = String.IsNullOrEmpty(SearchedfieldsJSON["dpDOSTo"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpDOSTo"]);

                if (SearchedfieldsJSON["txtFacility"] == "")
                    facility = "0";
                else
                    facility = SearchedfieldsJSON["hfFacility"];

                if (SearchedfieldsJSON["txtProvider"] == "")
                    provider = "0";
                else
                    provider = SearchedfieldsJSON["hfProvider"];

                if (SearchedfieldsJSON["ddlBiller"] == "")
                    biller = "0";
                else
                    biller = SearchedfieldsJSON["ddlBiller"];
                //if (EntryDateTo == null)
                //    EntryDateTo = EntryDateFrom;
                if (EntryDateFrom == null)
                    EntryDateFrom = EntryDateTo;
                if (DOSFrom == null)
                    DOSFrom = DOSTo;

                //bug #PMS-3448
                //if (DOSTo == null)
                //    DOSTo = DOSFrom;
                //bug #PMS-3448
                if(SearchedfieldsJSON["txtdescription"] != "")
                    description = SearchedfieldsJSON["txtdescription"];

                if (SearchedfieldsJSON["txtPractice"] == "")
                    practice = "0";
                else
                    practice = SearchedfieldsJSON["hfPractice"];

                DSBatchCharge dsBatchCharge = null;
                BLObject<DSBatchCharge> obj;
                if (SearchedfieldsJSON == null)
                {

                    obj = BLLBillingObj.BatchChargeSearch(BatchId, "", "", 0, 0, 0, "", 0, "", null, null, null, null);
                }
                else
                
                    //long BatchId, string BatchNumber, long FacilityId, long ProviderId, long BillerId, Int32 BatchStatusId, DateTime? DOSFrom = null, DateTime? DOSTo = null
                    obj = BLLBillingObj.BatchChargeSearch(BatchId, SearchedfieldsJSON["txtbatchno"], description, MDVUtility.ToInt64(facility), MDVUtility.ToInt64(provider), MDVUtility.ToInt32(biller), SearchedfieldsJSON["ddlPaid"], MDVUtility.ToInt64(practice), SearchedfieldsJSON["txtEnteredBy"], EntryDateFrom, EntryDateTo, DOSFrom, DOSTo, PageNumber, RowsPerPage);


                dsBatchCharge = obj.Data;

                if (obj.Data != null)
                {
                    if (dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ChargeBatchCount = dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows.Count,
                            iTotalDisplayRecords = dsBatchCharge.Batches.Rows[0][dsBatchCharge.Batches.RecordCountColumn.ColumnName],
                            ChargeBatchLoad_JSON = MDVUtility.JSON_DataTable(dsBatchCharge.Tables[dsBatchCharge.Batches.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ChargeBatchCount = 0,
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
                        ChargeBatchCount = 0,
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

              
                case "GET_BATCHCHARGE_SEARCH":
                    {
                        Int32 BatchId = MDVUtility.ToInt32(context.Request["BatchId"]);
                        string fieldsJSON = context.Request["BatchChargeData"];
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadBatchChargeSearch(fieldsJSON, BatchId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32((RowsPerPage)));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

               
            }
        }
        #endregion
    }
}