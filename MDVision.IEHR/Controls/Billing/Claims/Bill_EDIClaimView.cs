using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Billing.Claims
{
    public class Bill_EDIClaimView
    {
       private BLLBillingClaim BLLBillingClaimObj = null;
       public Bill_EDIClaimView()
        {
            BLLBillingClaimObj = new BLLBillingClaim();
        }
        #region Singleton
        private static Bill_EDIClaimView _obj = null;
        public static Bill_EDIClaimView Instance()
        {
            if (_obj == null)
                _obj = new Bill_EDIClaimView();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string LoadEDIClaimsViewDetail(Int64 _837BatchId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DS837Batch dsClaim = null;
                BLObject<DS837Batch> obj;
                obj = BLLBillingClaimObj.Load837BatchClaim(_837BatchId);
                dsClaim = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        EDIClaimDetailCount = dsClaim.Tables[dsClaim._837BatchClaim.TableName].Rows.Count > 0 ? dsClaim.Tables[dsClaim._837BatchClaim.TableName].Rows.Count : 0,
                        EDIClaimDetail_JSON = MDVUtility.JSON_DataTable(dsClaim.Tables[dsClaim._837BatchClaim.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        EDIClaimDetailCount = 0,
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
        /*
        private string ReTransmitClaim(string _837BatchId, string claims)
        {
            try
            {

                BLObject<bool> obj;
                if (!string.IsNullOrEmpty(claims))
                {
                    List<long> Visits_list = new List<long>();
                    string[] temp = claims.Split(',');
                    foreach (var item in temp)
                    {
                        if (!string.IsNullOrEmpty(item))
                            Visits_list.Add(MDVUtility.ToLong(item));
                    }

                    obj = BLLBillingClaimObj.UpdateVisitChargeStatus(Convert.ToInt32(_837BatchId), 3, Visits_list);

                    if (obj.Data == true)
                    {
                        //check if all claims of that batch is resubmitted then update batch status to resubmitted.
                        #region " Update Batch Status "

                        BLObject<DS837Batch> objBatchClaim = BLLBillingClaimObj.Load837BatchClaim((Convert.ToInt32(_837BatchId)));
                        if (objBatchClaim.Data != null)
                        {
                            DS837Batch dsBatch = objBatchClaim.Data;
                            DataRow[] drs = dsBatch._837BatchClaim.Select("" + dsBatch._837BatchClaim.ClaimStatusColumn.ColumnName + "<>'ReSubmit'");
                            if (drs.Count() <= 0)
                            {
                                BLObject<DS837Batch> objBatch = BLLBillingClaimObj.Load837Batch((Convert.ToInt32(_837BatchId)),"","",null,0,"","","");
                                if (objBatch.Data != null)
                                {
                                    DS837Batch._837BatchRow dr = (DS837Batch._837BatchRow)objBatch.Data._837Batch.Rows[0];
                                    dr.BatchStatus = "Resubmitted";
                                    BLLBillingClaimObj.Update837Batch(objBatch.Data);
                                }
                            }
                        }


                        #endregion

                        var response = new
                        {
                            status = true,
                            Message = "Claim status are Resubmitted.",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        string message = obj.Message;
                        if (string.IsNullOrEmpty(message))
                            message = "There was an error.";

                        var response = new
                        {
                            status = false,
                            Message = message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

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
        */

        private string ReTransmitClaim(string _837BatchId, string visitsIds)
        {
            try
            {

                BLObject<bool> obj;
                if (!string.IsNullOrEmpty(visitsIds))
                {
                    //StatusID=3=> For=> Status="ReSubmit", VisitStatusId=2=> For =>VisitStatus="Seen", ClaimStatusId=3=> FOr =>ClaimStatus="ReSubmit"
                    obj = BLLBillingClaimObj.UpdateResubmitStatusOfVisitsAndCharges(Convert.ToInt32(_837BatchId), 3, visitsIds, 2, 3);

                    if (obj.Data == true)
                    {
                        //RESUBMIT BATCH STATUS, check if all claims of that batch is resubmitted then update batch status to resubmitted.
                        BLLBillingClaimObj.Update837BatchStatusResubmit(Convert.ToInt32(_837BatchId), 0);

                        var response = new
                        {
                            status = true,
                            Message = "Claim status is Resubmitted.",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        string message = obj.Message;
                        if (string.IsNullOrEmpty(message))
                            message = "There was an error.";

                        var response = new
                        {
                            status = false,
                            Message = message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

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
                case "SEARCH_EDI_CLAIMVIEW_DETAIL":
                    {
                        string _837BatchId = MDVUtility.ToStr(context.Request["_837BatchId"]);
                        string strJSONData = LoadEDIClaimsViewDetail(MDVUtility.ToInt64(_837BatchId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_CLAIM":
                    {
                        string _837BatchId = MDVUtility.ToStr(context.Request["_837BatchId"]);
                        string VisitIDs = context.Request["VisitIDs"];
                        string strJSONData = ReTransmitClaim(_837BatchId, VisitIDs);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
              }
        }
        #endregion
    }
}