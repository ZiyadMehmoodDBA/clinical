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

namespace MDVision.IEHR.Controls.Patient.Demographics
{
    public class Patient_AdvancePayment
    {
        private BLLBilling BLLBillingObj = null;
        public Patient_AdvancePayment()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Patient_AdvancePayment _obj = null;
        public static Patient_AdvancePayment Instance()
        {
            if (_obj == null)
                _obj = new Patient_AdvancePayment();
            return _obj;
        }
        #endregion

        #region Private Functions


        private string FillAdvancePayment(Int64 AdvancePaymentID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AdvancePaymentID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPayment dsPayment = null;
                    BLObject<DSPayment> obj = BLLBillingObj.LoadAdvancePayment(0, AdvancePaymentID, 0, null, null, 0);
                    if (obj.Data != null)
                    {
                        dsPayment = obj.Data;
                        if (dsPayment.Tables[dsPayment.AdvancePayment.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPayment.Tables[dsPayment.AdvancePayment.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtPatientName", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.PatientAccountColumn.ColumnName])},
                            { "txtFullName", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.PatientNameColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.FacilityNameColumn.ColumnName])},
                            //{ "txtPaymentBatch", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.PaymentBatchIdColumn.ColumnName])},
                            { "txtPaid", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.AmtPaidDrColumn.ColumnName])},
                            { "ddlLedgerAccount", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.LedgerAccountIdColumn.ColumnName])},
                            { "dtpDatePaid", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.PaymentDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsPayment.AdvancePayment.PaymentDateColumn.ColumnName]).ToShortDateString():""},
                            { "ddlPaymentType", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.PaymentTypeIdColumn.ColumnName]) },
                            { "txtEnteredBy", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.CreatedByColumn.ColumnName])},
                            { "dtpEntryDate", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.CreatedOnColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.CommentsColumn.ColumnName])},
                            { "txtChequeNumber", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.CheckNumberColumn.ColumnName])},
                            { "dtpChequeDate", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.CheckDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsPayment.AdvancePayment.CheckDateColumn.ColumnName]).ToShortDateString():""},
                            { "txtCardNumber", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.CheckNumberColumn.ColumnName])},
                            { "dtpExpiryDate", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.ExpiryDateColumn.ColumnName]) !="" ? MDVUtility.ToStr(dr[dsPayment.AdvancePayment.ExpiryDateColumn.ColumnName]):""},
                            { "ddlCreditCardType", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.CardTypeIdColumn.ColumnName])},

                            { "hfPatientId", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.PatientIdColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.FacilityIdColumn.ColumnName])},
                          //  { "hfPaymentBatch", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.PaymentBatchIdColumn.ColumnName])},
                            { "isRefund", MDVUtility.ToStr(dr[dsPayment.AdvancePayment.IsRefundColumn.ColumnName])},
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                AdvancePaymentFill_JSON = js.Serialize(keyValues)

                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SavePatientAdvancePayment(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPayment dsAdvancePayment = new DSPayment();

                DSPayment.AdvancePaymentRow dr = dsAdvancePayment.AdvancePayment.NewAdvancePaymentRow();


                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                    dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                    dr.FacilityId = MDVUtility.ToLong(SearchedfieldsJSON["hfFacility"]);

                //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPaymentBatch"]))
                //    dr.PaymentBatchId = MDVUtility.ToInt32(SearchedfieldsJSON["hfPaymentBatch"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPaid"]))
                    dr.AmtPaidDr = MDVUtility.ToDouble(SearchedfieldsJSON["txtPaid"]);

                //  dr.AmtPaidCr = 0 ;

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlLedgerAccount"]))
                    dr.LedgerAccountId = MDVUtility.ToLong(SearchedfieldsJSON["ddlLedgerAccount"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDatePaid"]))
                    dr.PaymentDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDatePaid"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPaymentType"]))
                    dr.PaymentTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPaymentType"]);

                if (dr.PaymentTypeId == 2)
                {
                    //cheque

                    dr.CheckNumber = SearchedfieldsJSON["txtChequeNumber"];

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpChequeDate"]))
                        dr.CheckDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpChequeDate"]);

                }
                else if (dr.PaymentTypeId == 3)
                {
                    //credit card

                    dr.CheckNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtCardNumber"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpExpiryDate"]))
                        dr.ExpiryDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpExpiryDate"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCreditCardType"]))
                        dr.CardTypeId = MDVUtility.ToStr(SearchedfieldsJSON["ddlCreditCardType"]);
                }

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                    dr.Comments = SearchedfieldsJSON["txtComments"];

                dr.IsActive = true;

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsAdvancePayment.AdvancePayment.AddAdvancePaymentRow(dr);
                BLObject<DSPayment> obj = BLLBillingObj.InsertAdvancePayment(dsAdvancePayment);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        //FIXME
                        message = Common.AppPrivileges.Save_Message,
                        MessageId = dsAdvancePayment.Tables[dsAdvancePayment.AdvancePayment.TableName].Rows[0][dsAdvancePayment.AdvancePayment.AdvPaymentIdColumn.ColumnName]
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

        private string UpdateAdvancePayment(string fieldsJSON, Int64 AdvancePaymentID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPayment dsPayment = null;

                BLObject<DSPayment> obj1 = BLLBillingObj.LoadAdvancePayment(0, AdvancePaymentID, 0, null, null, 0);
                dsPayment = obj1.Data;
                if (AdvancePaymentID > 0)
                {
                    // DSPayment.AdvancePaymentRow dr = dsPayment.Tables[dsPayment.AdvancePayment.TableName].Rows[0] as DSPayment.AdvancePaymentRow;
                    foreach (DSPayment.AdvancePaymentRow dr in dsPayment.Tables[dsPayment.AdvancePayment.TableName].Rows)
                    {

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                            dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                            dr.FacilityId = MDVUtility.ToLong(SearchedfieldsJSON["hfFacility"]);

                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPaymentBatch"]))
                        //    dr.PaymentBatchId = MDVUtility.ToInt32(SearchedfieldsJSON["hfPaymentBatch"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPaid"]))
                            dr.AmtPaidDr = MDVUtility.ToDouble(SearchedfieldsJSON["txtPaid"]);


                        //  dr.AmtPaidCr = 0 ;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlLedgerAccount"]))
                            dr.LedgerAccountId = MDVUtility.ToLong(SearchedfieldsJSON["ddlLedgerAccount"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDatePaid"]))
                            dr.PaymentDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDatePaid"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPaymentType"]))
                            dr.PaymentTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPaymentType"]);

                        if (dr.PaymentTypeId == 2)
                        {
                            //cheque

                            dr.CheckNumber = SearchedfieldsJSON["txtChequeNumber"];

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpChequeDate"]))
                                dr.CheckDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpChequeDate"]);

                        }
                        else if (dr.PaymentTypeId == 3)
                        {
                            //credit card

                            dr.CheckNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtCardNumber"]);

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpExpiryDate"]))
                                dr.ExpiryDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpExpiryDate"]);

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCreditCardType"]))
                                dr.CardTypeId = MDVUtility.ToStr(SearchedfieldsJSON["ddlCreditCardType"]);
                        }

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                            dr.Comments = SearchedfieldsJSON["txtComments"];

                        dr.IsActive = true;

                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;




                    }
                    #region Database Updation
                    // dsPayment.AdvancePayment.AcceptChanges();

                    if (dsPayment.Tables[dsPayment.AdvancePayment.TableName].Rows.Count > 0)
                    {
                        // dsPayment.AdvancePayment.Rows[0].SetModified();
                        BLObject<DSPayment> obj = BLLBillingObj.UpdateAdvancePayment(dsPayment);
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Advance Payment not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
            //return "";
        }


        private string RefundAdvancePayment(Int64 AdvancePaymentID)
        {
            try
            {

                DSPayment dsAdvancePayment = new DSPayment();

                DSPayment.AdvancePaymentRow newAdvancePaymentRow = dsAdvancePayment.AdvancePayment.NewAdvancePaymentRow();

                DSPayment dsPayment = null;

                BLObject<DSPayment> obj1 = BLLBillingObj.LoadAdvancePayment(0, AdvancePaymentID, 0, null, null, 0);
                dsPayment = obj1.Data;


                foreach (DSPayment.AdvancePaymentRow dr in dsPayment.Tables[dsPayment.AdvancePayment.TableName].Rows)
                {

                    newAdvancePaymentRow.PatientId = dr.PatientId;

                    newAdvancePaymentRow.FacilityId = dr.FacilityId;

                    //   newAdvancePaymentRow.PaymentBatchId = dr.PaymentBatchId;


                    // newAdvancePaymentRow.AmtPaidDr = dr.AmtPaidDr;



                    //if (dr.Balance < dr.AmtPaidDr)
                    //{

                    if (dr.Balance > 0)
                    {
                        newAdvancePaymentRow.AmtPaidCr = dr.Balance;

                    }
                    else
                    {

                        throw new Exception("Advance Balance for this Payment is already " + dr.Balance);
                    }
                    //}
                    //else
                    //{
                    //    newAdvancePaymentRow.AmtPaidCr = dr.AmtPaidDr;
                    //}

                    newAdvancePaymentRow.LedgerAccountId = dr.LedgerAccountId;

                    newAdvancePaymentRow.PaymentDate = dr.PaymentDate;

                    newAdvancePaymentRow.PaymentTypeId = dr.PaymentTypeId;

                    if (dr.PaymentTypeId == 2)
                    {
                        //check

                        newAdvancePaymentRow.CheckNumber = dr.CheckNumber;


                        newAdvancePaymentRow.CheckDate = dr.CheckDate;

                    }
                    else if (dr.PaymentTypeId == 3)
                    {
                        //credit card

                        newAdvancePaymentRow.CheckNumber = dr.CheckNumber;

                        newAdvancePaymentRow.ExpiryDate = dr.ExpiryDate;

                        newAdvancePaymentRow.CardTypeId = dr.CardTypeId;
                    }

                    newAdvancePaymentRow.Comments = dr.Comments;

                    newAdvancePaymentRow.IsActive = dr.IsActive;

                    newAdvancePaymentRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);

                    newAdvancePaymentRow.CreatedOn = DateTime.Now;

                    newAdvancePaymentRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);

                    newAdvancePaymentRow.ModifiedOn = DateTime.Now;

                    newAdvancePaymentRow.MasterPaymentId = AdvancePaymentID;
                }


                #region Database Insertion

                dsAdvancePayment.AdvancePayment.AddAdvancePaymentRow(newAdvancePaymentRow);
                BLObject<DSPayment> obj = BLLBillingObj.InsertAdvancePayment(dsAdvancePayment);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        //FIXME
                        Message = Common.AppPrivileges.Refund_Message,
                        MessageId = dsAdvancePayment.Tables[dsAdvancePayment.AdvancePayment.TableName].Rows[0][dsAdvancePayment.AdvancePayment.AdvPaymentIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Refund_Error_Message
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
            //return "";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="PatientID"></param>
        /// <param name="PaymentID"></param>
        /// <returns></returns>
        private string SearchAdvancePayment(string fieldsJSON, int pageNumber, int rowsPerPage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                DSPayment dsAdvancePayment = null;
                BLObject<DSPayment> obj;

                Int64 PatientID = 0;
                Int64 FacilityID = 0;
                DateTime? PaidFrom = null;
                DateTime? PaidTo = null;
                Int64 PaymentType = 0;

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["hfPatientId"])))
                    PatientID = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["hfFacility"])))
                    FacilityID = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpPaidFrom"]))
                    PaidFrom = MDVUtility.ToStr(SearchedfieldsJSON["dtpPaidFrom"]) != "" ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpPaidFrom"]) : null;

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpPaidTo"]))
                    PaidTo = MDVUtility.ToStr(SearchedfieldsJSON["dtpPaidTo"]) != "" ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpPaidTo"]) : null;

                if (SearchedfieldsJSON.ContainsKey("ddlPaymentType") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPaymentType"]))
                    PaymentType = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPaymentType"]);

                obj = BLLBillingObj.searchAdvancePayment(PatientID, FacilityID, PaidFrom, PaidTo, PaymentType, pageNumber, rowsPerPage);


                dsAdvancePayment = obj.Data;
                if (obj.Data != null)
                {
                    if (dsAdvancePayment.Tables[dsAdvancePayment.AdvancePaymentSearch.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = dsAdvancePayment.Tables[dsAdvancePayment.AdvancePaymentSearch.TableName].Rows.Count,
                            AdvancePaymentLoad_JSON = MDVUtility.JSON_DataTable(dsAdvancePayment.Tables[dsAdvancePayment.AdvancePaymentSearch.TableName]),
                            iTotalDisplayRecords = (dsAdvancePayment.AdvancePaymentSearch.Rows.Count > 0) ? dsAdvancePayment.AdvancePaymentSearch.Rows[0][dsAdvancePayment.AdvancePaymentSearch.RecordCountColumn.ColumnName] : 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Public Functions
        public void validateAdvancePayment(Int64 PatientId, double paidAmount, long AdvancePaymentId)
        {
            if (PatientId <= 0)
                throw new Exception("Please select a patient");

            if (AdvancePaymentId <= 0)
                throw new Exception("Please Select an Advance Payment");
            try
            {
                DSPayment dsAdvancePayment = null;
                BLObject<DSPayment> obj;

                obj = BLLBillingObj.LoadAdvancePayment(PatientId, AdvancePaymentId, 0, null, null, 0);

                dsAdvancePayment = obj.Data;

                if (dsAdvancePayment.Tables[dsAdvancePayment.AdvancePayment.TableName].Rows.Count > 0)
                {
                    double Balance = MDVUtility.ToDouble(dsAdvancePayment.Tables[dsAdvancePayment.AdvancePayment.TableName].Rows[0][dsAdvancePayment.AdvancePayment.BalanceColumn.ColumnName]);

                    if (Balance >= paidAmount)
                    {
                        //legitimate amount is being paid
                        return;
                    }
                    else
                    {
                        throw new Exception("Advance Balance is less than Paid Amount");
                    }

                }

                throw new Exception("Advance Payment Doesn't Exists");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Advance Payment Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_PATIENT_ADVANCE_PAYMENT":
                    {
                        string fieldsJSON = context.Request["AdvancePaymentData"];
                        string page = context.Request["page"];
                        Int64 rpp = 15;//Utility.ToInt64(context.Request["rp"]);
                        Int64 pageNo = 1; //Utility.ToInt64(context.Request["rp"]);
                        //Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 AdvancePaymentID = MDVUtility.ToInt64(context.Request["AdvancePaymentID"]);
                        Int32 pageNumber = MDVUtility.ToInt32(context.Request["pageNumber"]);
                        Int32 rowsPerPage = MDVUtility.ToInt32(context.Request["rowsPerPage"]);
                        string strJSONData = SearchAdvancePayment(fieldsJSON, pageNumber, rowsPerPage);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_PATIENT_ADVANCE_PAYMENT":
                    {
                        string fieldsJSON = context.Request["AdvancePaymentData"];
                        string strJSONData = SavePatientAdvancePayment(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_PATIENT_ADVANCE_PAYMENT":
                    {
                        string fieldsJSON = context.Request["AdvancePaymentData"];
                        Int64 AdvancePaymentID = MDVUtility.ToInt64(context.Request["AdvancePaymentID"]);


                        string strJSONData = UpdateAdvancePayment(fieldsJSON, AdvancePaymentID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "REFUND_PATIENT_ADVANCE_PAYMENT":
                    {
                        //string fieldsJSON = context.Request["AdvancePaymentData"];
                        Int64 AdvancePaymentID = MDVUtility.ToInt64(context.Request["AdvancePaymentID"]);

                        string strJSONData = RefundAdvancePayment(AdvancePaymentID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                case "FILL_PATIENT_ADVANCE_PAYMENT":
                    {
                        Int64 advancePaymentId = MDVUtility.ToInt64(context.Request["AdvancePaymentId"]);
                        string strJSONData = FillAdvancePayment(advancePaymentId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }
        }
        #endregion


    }
}