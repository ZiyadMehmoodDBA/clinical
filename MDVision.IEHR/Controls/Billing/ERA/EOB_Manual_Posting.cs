using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.Model.Billing.ERA;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Billing.ERA
{
    public class EOB_Manual_Posting
    {
        private BLLEOBManualPosting BLLEOBManualPostingObj = null;
        public EOB_Manual_Posting()
        {
            BLLEOBManualPostingObj = new BLLEOBManualPosting();

        }
        #region Singleton
        private static EOB_Manual_Posting _objManualPosting = null;
        public static EOB_Manual_Posting Instance()
        {
            if (_objManualPosting == null)
                _objManualPosting = new EOB_Manual_Posting();
            return _objManualPosting;
        }
        #endregion
        #region ChargeModelSearch
        public string SearchEOBManualPosting(string fieldsJSON)
        {

            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                InsurancePaymentDetail modal = new InsurancePaymentDetail();
                modal.PayerName = MDVUtility.ToStr(SearchedfieldsJSON["PayerName"]);
                modal.CheckNo = MDVUtility.ToStr(SearchedfieldsJSON["CheckNumber"]);
                modal.CheckAmount = MDVUtility.ToDecimal(SearchedfieldsJSON["CheckAmount"]);
                modal.PostedStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["Status"]);
                modal.FromEntryDate = MDVUtility.ToStr(SearchedfieldsJSON["FromEntryDate"]);
                modal.ToEntryDate = MDVUtility.ToStr(SearchedfieldsJSON["ToEntryDate"]);
                modal.FromCheckDate = MDVUtility.ToStr(SearchedfieldsJSON["FromCheckDate"]);
                modal.ToCheckDate = MDVUtility.ToStr(SearchedfieldsJSON["ToCheckDate"]);
                List <InsurancePaymentDetail> obj = BLLEOBManualPostingObj.LoadInsurancePaymentSearch(modal);
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    EOBManualPostingData = obj
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
        public string SaveEOBManualPosting(string fieldsJSON)
        {
           
            try
            {
               
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                InsurancePaymentDetail modal = new InsurancePaymentDetail();
                modal.PayerName = MDVUtility.ToStr(SearchedfieldsJSON["txtInsurancePlan"]);
                modal.InsurancePlanId = MDVUtility.ToInt32(SearchedfieldsJSON["hfInsurancePlan"]);
                modal.CheckNo = MDVUtility.ToStr(SearchedfieldsJSON["txtCheckNumber"]);
                modal.CheckAmount = MDVUtility.ToDecimal(SearchedfieldsJSON["txtCheckAmount"]);
                modal.CheckDate = MDVUtility.ToStr(SearchedfieldsJSON["dpCheckDate"]);
                modal.CheckDepositDate = MDVUtility.ToStr(SearchedfieldsJSON["dpCheckDepositDate"]);
                modal.PostedStatusId= MDVUtility.ToInt32(SearchedfieldsJSON["ddlStatus"]);
                modal.PostedAmount =string.IsNullOrEmpty(SearchedfieldsJSON["txtPostedAmount"])?0: MDVUtility.ToDecimal(SearchedfieldsJSON["txtPostedAmount"]);
                modal.Id = string.IsNullOrEmpty(SearchedfieldsJSON["hfEOBManualPostingId"]) ? 0 : MDVUtility.ToInt64(SearchedfieldsJSON["hfEOBManualPostingId"]);
                modal.IsActive = true;
                modal.CreatedOn = DateTime.Now;
                modal.CreatedBy = MDVSession.Current.AppUserFullName;
                modal.ModifiedOn = DateTime.Now;
                modal.ModifiedBy = MDVSession.Current.AppUserFullName;
                InsurancePaymentDetail obj = null;
                string message = Common.AppPrivileges.Save_Message;
                if (modal.Id == 0)
                { obj = BLLEOBManualPostingObj.InsertEOBManualPosting(modal); }
                else
                {
                    obj = BLLEOBManualPostingObj.UpdateEOBManualPosting(modal);
                    message= Common.AppPrivileges.Update_Message;
                }
                if (obj.Id>0)
                {
                    var response = new
                    {
                        status = true,
                        message = message,
                        EOBManualPostingId=modal.Id
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Active_Error_Message,
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
        public string EobManualPostingExistORNot(long VisitId,long InsurancePaymentDetailId)
        {

            try
            {

                BLObject<string> obj = BLLEOBManualPostingObj.EobManualPostingExistORNot(VisitId, InsurancePaymentDetailId);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        IsEobDetailExists = obj.Data,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        IsEobDetailExists = false
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

        public string LoadChargeDetail(Int64 VisitId)
        {
            try
            {
                List<EOBManualChargeLoad> ChargeList = null;
                BLObject<List<EOBManualChargeLoad>> obj = BLLEOBManualPostingObj.LoadPatientCharges(VisitId);
                ChargeList = obj.Data;
                    if (obj.Data != null)
                    {
                        
                        //if (ChargeList.Count > 0)
                        //{
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ChargesCount = ChargeList.Count,
                                ChargeLoad_JSON = js.Serialize(ChargeList),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        //}
                        //else
                        //{
                        //    var response = new
                        //    {
                        //        status = false,
                        //        Message = Common.AppPrivileges.No_Record_Message,
                        //    };
                        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        //}
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
        public string  LoadEOBManualPostingDocument(long EOBId)
        {
            try
            {
                
               long  PatDocId = BLLEOBManualPostingObj.LoadEOBManualPostingDocument(EOBId);
               System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
               var response = new
               {
                   status = true,
                   PatDocId = PatDocId,
                  
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
        public string LoadPaymentDetail(Int64 VisitId,Int64 EOBPostingId, Int64 EOBDetlId)
        {
            try
            {
                List<EOBManualPaymentPost> PaymentList = null;
                BLObject<List<EOBManualPaymentPost>> obj = BLLEOBManualPostingObj.LoadEOBPaymentDetail(VisitId, EOBPostingId, EOBDetlId);
                PaymentList = obj.Data;
                if (obj.Data != null)
                {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ChargesCount = PaymentList.Count,
                            PaymentDetailLoad_JSON = js.Serialize(PaymentList),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        public string SavePaymentsDetail(string fieldsJSON)
        {
            try
            {
                List<EOBManualPaymentPost> PaymentList = JsonConvert.DeserializeObject<List<EOBManualPaymentPost>>(fieldsJSON);
                PaymentList.ForEach(
                    x =>
                    {
                       
                        x.IsActive = "1";
                        x.CreatedOn = DateTime.Now.ToString();
                        x.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        x.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        x.ModifiedOn = DateTime.Now.ToString();
                    }
                );
                DataTable dtPaymentList = MDVUtility.ConvertListToDataTable(PaymentList.ToList());
                string insertDetail = BLLEOBManualPostingObj.InsertEOBManualPostingDetail(dtPaymentList);
                if (insertDetail == "")
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
                        Message = Common.AppPrivileges.Save_Error_Message,
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

        public string UpdatePaymentDetail(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                EOBManualPaymentPost PaymentList = JsonConvert.DeserializeObject<EOBManualPaymentPost>(fieldsJSON);
                if (PaymentList.NextResponsibilityId == "1")
                {
                    PaymentList.NextResponsibilityId = PaymentList.InsurancePlanId;
                }
                PaymentList.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                PaymentList.ModifiedOn = DateTime.Now.ToString();
                string insertDetail = BLLEOBManualPostingObj.UpdateEOBManualPostingDetail(PaymentList);
                if (insertDetail=="")
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Update_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Update_Error_Message,
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
        public string DeleteEOBManualPosting(long EOBDetlId)
        {
            try
            {
                string deleteDetail = BLLEOBManualPostingObj.DeleteEOBManualPosting(EOBDetlId);
                if (deleteDetail == "")
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Delete_Message,
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = deleteDetail,
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
        public string DeleteEOBManualPostingDetail(long EOBDetlId)
        {
            try
            {
                string deleteDetail = BLLEOBManualPostingObj.DeleteEOBManualPostingDetail(EOBDetlId);
                if (deleteDetail == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Delete_Error_Message,
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


       public string PostEOBManualPosting(long EOBPostingId,long EOBDetlId)
          {
            try
            {
               


                string insertDetail = BLLEOBManualPostingObj.PostEOBManualPosting(EOBPostingId, EOBDetlId);
                if (insertDetail == "")
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
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
                        status = true,
                        message = Common.AppPrivileges.Update_Error_Message,
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
        public string FillEOBManualPosting(Int64 EOBManualPostingId)
        {

            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
              
                InsurancePaymentDetail modal = new InsurancePaymentDetail();
                modal.Id = EOBManualPostingId;
                List<InsurancePaymentDetail> obj = BLLEOBManualPostingObj.LoadInsurancePaymentSearch(modal);
                 var keyValues = new Dictionary<string, string>
                        {
                            { "txtInsurancePlan",obj.FirstOrDefault().PayerName},
                            { "ddlStatus", obj.FirstOrDefault().PostedStatusId.ToString()},
                            { "txtCheckNumber", obj.FirstOrDefault().CheckNo}, 
                            { "dpCheckDate", obj.FirstOrDefault().CheckDate},
                            { "dpCheckDepositDate",obj.FirstOrDefault().CheckDepositDate},
                            { "txtPostedAmount", obj.FirstOrDefault().PostedAmount.ToString()},
                            { "dpEntryDate", obj.FirstOrDefault().CreatedOn.ToString()},
                            { "txtUser", obj.FirstOrDefault().ModifiedBy},
                            { "hfInsurancePlan",obj.FirstOrDefault().InsurancePlanId.ToString() },
                            {"txtCheckAmount",obj.FirstOrDefault().CheckAmount.ToString() },
                            {"hfEOBManualPostingId",obj.FirstOrDefault().Id.ToString() },
                            
                        };
                var response = new
                {
                    status = true,
                    EOBManualPostingCount= obj.Count,
                     EOBManualPostingLoad_JSON = ser.Serialize(keyValues),
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
        #region Service Command Handler
        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "EOB_MANUAL_POSTING_SEARCH":
                    {
                        string fieldsJSON = context.Request["EOBPostingData"];
                        string strJSONData = SearchEOBManualPosting(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_EOB_MANUAL_POSTING":
                    {
                        string fieldsJSON = context.Request["insurancePayments"];
                        string strJSONData = SaveEOBManualPosting(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CHECK_EOB_MANUAL_POSTING_EXIST":
                    {
                        long VisitId = MDVUtility.ToLong( context.Request["VisitId"]);
                        long InsurancePaymentDetailId = MDVUtility.ToLong(context.Request["InsurancePaymentDetailId"]);
                        string strJSONData = EobManualPostingExistORNot(VisitId, InsurancePaymentDetailId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                 case "LOAD_CHARGE_DETAIL":
                    {
                        long VisitId = MDVUtility.ToLong(context.Request["VisitId"]);
                        string strJSONData = LoadChargeDetail(VisitId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PAYMENTS_DETAIL":
                    {
                        string Paymentfield= MDVUtility.ToStr(context.Request["Paymentdata"]);
                        string strJSONData = SavePaymentsDetail(Paymentfield);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PAYMENTS_DETAIL":
                    {
                        string Paymentfield = MDVUtility.ToStr(context.Request["Paymentdata"]);
                        string strJSONData = UpdatePaymentDetail(Paymentfield);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_EOB_PAYMENT_DETAIL":
                    {
                        long VisitId = MDVUtility.ToLong(context.Request["VisitId"]);
                        long EOBDtlId = MDVUtility.ToLong(context.Request["EOBDtlId"]);
                        long EOBPostingId = MDVUtility.ToLong(context.Request["EOBPostingId"]);
                        string strJSONData = LoadPaymentDetail(VisitId, EOBPostingId, EOBDtlId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_EOB_MANUAL_POSTING":
                    {
                     
                        long EOBId = MDVUtility.ToLong(context.Request["EOBId"]);
                        string strJSONData = FillEOBManualPosting(EOBId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        
                    }
                    break;
                    
                        case "POST_MANUAL_PAYMENT":
                    {

                        long EOBId = MDVUtility.ToLong(context.Request["EOBId"]);
                        long EOBDetlId = MDVUtility.ToLong(context.Request["EOBDetlId"]);
                        string strJSONData = PostEOBManualPosting(EOBId,EOBDetlId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);

                    }
                    break;
                case "DELETE_MANUAL_PAYMENT":
                    {

                        long EOBId = MDVUtility.ToLong(context.Request["EOBId"]);
                        string strJSONData = DeleteEOBManualPosting(EOBId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);

                    }
                    break;
                case "DELETE_MANUAL_PAYMENT_DETAIL":
                    {

                       
                        long EOBDetlId = MDVUtility.ToInt64(context.Request["EOBDetlId"]);
                        string strJSONData = DeleteEOBManualPostingDetail(EOBDetlId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);

                    }
                    break;
                case "LOAD_MANUAL_POSTING_DOCUMENT":
                    {
                        long EOBId = MDVUtility.ToInt64(context.Request["EOBId"]);
                        string strJSONData = LoadEOBManualPostingDocument(EOBId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);

                    }
                    break;
                    
            }
        }
        #endregion
    }
}