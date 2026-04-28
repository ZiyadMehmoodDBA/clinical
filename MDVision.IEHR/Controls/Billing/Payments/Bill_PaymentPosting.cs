using System;
using System.Collections.Generic;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.IEHR.Controls.Patient.Demographics;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using MDVision.IEHR.Model.Billing.Payments;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using System.Threading.Tasks;
using MDVision.Model.Common;
using MDVision.Model.Billing.Payments;

namespace MDVision.IEHR.Controls.Billing.Payments
{
    public class Bill_PaymentPosting
    {
        private BLLBilling BLLBillingObj = null;
        public Bill_PaymentPosting()
        {
            BLLBillingObj = new BLLBilling();
        }

        #region FIELDS
        #region COMMON FIELDS

        private string paymentTypeField = "";
        private Int64 chargeIdField = -1;
        private Int64 patientIdField = -1;
        private Int64 visitIdField = -1;
        private Int64 paymentBatchField = -1;
        private Int64 providerIdField = -1;
        private Int64 facilityIdField = -1;
        private DateTime? datePaidField = null;
        private Int32 paymentModeField = -1;
        private string checkNumberField = "";
        private string checkDateField = null;
        private string cardNumberField = "";
        private string cardExpiryDateField = null;
        private Int32 cardTypeField = 0;
        private bool isDeniedField = false;
        private Int64 InsurancePlanId = 0;
        private Int64 PatientInsurancePlanId = 0;
        private Int32 InsurancePlanPriority = 0;


        #endregion

        #region INSURANCE FIELDS


        private double insuranceAllowedField = 0;
        private double insurancePaidField = 0;
        private Int32 insurancePaidAccountIdField = 0;
        private double insuranceWriteoffField = 0;
        private Int32 insuranceWriteoffAccountIdField = 0;
        private double insuranceChargeCopayField = 0;
        private Int32 insuranceRemitCodeField = 0;
        private string insuranceIcnDcnField = "";

        private double insuranceCoinsuranceField = 0;
        private double insuranceDeductablesField = 0;
        private double insurancePatientResponsibilityField = 0;
        private string insuranceNextResponsibilityField = "";
        private Int32 insuranceNextInsurancePlanIdField = 0;
        private bool insuranceCrossOverField = false;


        private bool insPrintOnPatStmtField = false;
        private string insuranceCommentsField = "";




        #endregion

        #region PATIENT FIELDS


        private double patientPaidField = 0;
        private Int32 patientPaidAccountIdField = 0;
        private double patientDiscountField = 0;
        private Int32 patientDiscountAccountIdField = 0;
        private long patientAdvancePaymentIdField = 0;

        private double patientCoinsuranceField = 0;
        private double patientDeductablesField = 0;
        private double patientPatientResponsibilityField = 0;
        private string patientNextResponsibilityField = "";
        private Int32 patientNextInsurancePlanIdField = 0;
        private bool patPrintOnPatStmtField = false;
        private string patientCommentsField = "";
        #endregion

        #region COPAYMENT FIELDS

        private double copaymentPaidField = 0;
        private Int32 copaymentPaidAccountIdField = 0;
        private double copaymentDiscountField = 0;
        private Int32 copaymentDiscountAccountIdField = 0;

        private string CopaymentNextResponsibilityField = "";
        private double CopaymentTransferField = 0;

        private bool copaymentPrintOnPatStmtField = false;
        private string copaymentCommentsField = "";



        #endregion

        #region ZERO PAYMENT FIELDS

        private string zeroPaymentCommentsField = "";
        private bool zeroPaymentPrintOnPatStmtField = false;

        #endregion

        #region RECOUPMENT FIELDS


        private double recoupmentAllowedField = 0;
        private double recoupmentPaidField = 0;
        private Int32 recoupmentPaidAccountIdField = 0;
        private double recoupmentWriteoffField = 0;
        private Int32 recoupmentWriteoffAccountIdField = 0;
        private double recoupmentChargeCopayField = 0;
        private Int32 recoupmentRemitCodeField = 0;
        private string recoupmentIcnDcnField = "";

        private double recoupmentCoinsuranceField = 0;
        private double recoupmentDeductablesField = 0;
        private double recoupmentPatientResponsibilityField = 0;
        private string recoupmentNextResponsibilityField = "";
        private Int32 recoupmentNextInsurancePlanIdField = 0;
        private bool recoupmentCrossOverField = false;


        private bool recoupmentPrintOnPatStmtField = false;
        private string recoupmentCommentsField = "";
        private bool recoupmentIsRecoupmentField = false;





        #endregion

        #endregion

        #region Singleton
        private static Bill_PaymentPosting _obj = null;
        public static Bill_PaymentPosting Instance()
        {
            if (_obj == null)
                _obj = new Bill_PaymentPosting();
            return _obj;
        }
        #endregion

        #region Private Functions


        public string SavePatientPayment(PaymentPostingModel model, string paymentType)
        {
            try
            {
                setAllFields(model, paymentType);
                // if payment type is recoupment then don't validate balance
                if (paymentType.ToLower() != "recoupment")
                {
                    validatePaymentBalance(model, paymentType);
                }
                DSPayment dsPayment = new DSPayment();

                

                //#region COPAY PAYMENT
                //else if (paymentTypeField == "copayment")
                //{
                //    if (copaymentPaidField > 0)
                //    {

                //        insertCopayPaidRow(dsPayment);
                //    }

                //    if (copaymentDiscountField > 0)
                //    {

                //        insertCopayDiscountRow(dsPayment);
                //    }


                //    //copay transfer entry
                //    if (CopaymentTransferField > 0)
                //    {

                //        insertCopayTransferRow(dsPayment);
                //    }

                //}
                //#endregion

                #region ZERO PAYMENT
                if (paymentTypeField == "zeropayment")
                {

                    if (!isDeniedField)
                    {
                        insertZeroPaymentRow(dsPayment);

                    }

                }
                #endregion

                #region RECOUPMENT PAYMENT
                else if (paymentTypeField == "recoupment")
                {
                    if (recoupmentPaidField > 0 || isDeniedField)
                    {
                        insertRecoupmentPaidRow(dsPayment);


                    }

                    if (recoupmentWriteoffField > 0 && !isDeniedField)
                    {
                        insertRecoupmentWriteoffRow(dsPayment);
                    }

                    //copay transfer
                    if (recoupmentChargeCopayField > 0 && !isDeniedField)
                    {
                        insertRecoupmentChargeCopayRow(dsPayment);
                    }

                    if (recoupmentCoinsuranceField > 0 && !isDeniedField)
                    {
                        insertRecoupmentCoinsuranceRow(dsPayment);
                    }

                    if (recoupmentDeductablesField > 0 && !isDeniedField)
                    {
                        insertRecoupmentDeductablesRow(dsPayment);
                    }

                    if (recoupmentPatientResponsibilityField > 0 && !isDeniedField)
                    {
                        insertRecoupmentParientResponsibilityRow(dsPayment);
                    }

                }
                #endregion

                #region Database Insertion

                //throw new Exception("ready to insert!"); 
                if (dsPayment.Tables[dsPayment.PatientPayments.TableName].Rows.Count > 0)
                {
                    BLObject<DSPayment> obj = BLLBillingObj.InsertPatientPayments(dsPayment);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            paymentDebitId = dsPayment.Tables[dsPayment.PatientPayments.TableName].Rows[0][dsPayment.PatientPayments.PaymentIdColumn.ColumnName]

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
                        return JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.NO_AMOUNT_ADDED,
                    };
                    return JsonConvert.SerializeObject(response);
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        private string SavePatientResponsibilityPayment(string PaymentData, string ChargeData, string pmtBatchId)
        {
            try
            {
                DSPayment dsPayment = new DSPayment();
                List<object> items = JsonConvert.DeserializeObject<List<object>>(ChargeData);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(PaymentData);

                Int32 PatientID = 0;
                Int32 FacilityID = 0;
                Int32 ChargeCapID = 0;
                Int32 VisitID = 0;
                float Balance = 0;
                Int32 ProviderID = 0;
                double paidAmount = MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientPaid"]);
                foreach (dynamic charge in items)
                {
                    if (paidAmount > 0)
                    {

                       

                        DSPayment.PatientPaymentsRow patientPaidRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

                        patientPaidRow.PatientId = PatientID;
                        patientPaidRow.FacilityId = FacilityID;
                        patientPaidRow.ChargeId = ChargeCapID;
                        patientPaidRow.VisitId = VisitID;
                        patientPaidRow.ProviderId = ProviderID;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfAdvancePaymentId"]))
                        {
                            patientPaidRow.AdvPmtId = MDVUtility.ToInt64(SearchedfieldsJSON["hfAdvancePaymentId"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientPaid"]))
                        {

                          
                            //double finalBalance = 0;
                            //if ((MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientPaid"]))  > Balance)

                            //patientPaidRow.PaidAmountDr = MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientPaid"]);
                            //SearchedfieldsJSON["txtPatientPaid"] = MDVUtility.ToStr(((MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientPaid"])) - Balance));

                            //if (((MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientPaid"])) / items.Count) > Balance)
                            //    isBalanceExceed = true;

                            //if (!isBalanceExceed)
                            //{
                            //    patientPaidRow.PaidAmountDr = (MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientPaid"])) / items.Count;
                            //}
                            //else {
                            //    patientPaidRow.PaidAmountDr = Balance;
                            //    SearchedfieldsJSON["txtPatientPaid"] = MDVUtility.ToStr(((MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientPaid"])) - Balance));
                            //    isFullBalancePaid = true;
                            //}


                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDatePaid"]))
                        {
                            patientPaidRow.PaymentDate = Convert.ToDateTime(SearchedfieldsJSON["dtpDatePaid"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPaymentType"]))
                        {
                            patientPaidRow.PmtTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPaymentType"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPatientPaid"]))
                        {
                            patientPaidRow.LedgerAccId = MDVUtility.ToLong(SearchedfieldsJSON["ddlPatientPaid"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCheckNumber"]))
                        {
                            patientPaidRow.CheckNo = MDVUtility.ToStr(SearchedfieldsJSON["txtCheckNumber"]);
                        }
                       
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpExpiryDate"]))
                        {
                            patientPaidRow.ExpiryDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpExpiryDate"]);
                        }
                        if (!string.IsNullOrEmpty(pmtBatchId))
                        {
                            patientPaidRow.PmtBatchId = MDVUtility.ToInt64(pmtBatchId);
                        }
                        patientPaidRow.IsActive = true;
                        patientPaidRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        patientPaidRow.CreatedOn = DateTime.Now;
                        patientPaidRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        patientPaidRow.ModifiedOn = DateTime.Now;

                        dsPayment.PatientPayments.AddPatientPaymentsRow(patientPaidRow);
                    }
                }





                #region Database Insertion

                //throw new Exception("ready to insert!"); 
                BLObject<DSPayment> obj = BLLBillingObj.InsertPatientPayments(dsPayment);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        paymentDebitId = dsPayment.Tables[dsPayment.PatientPayments.TableName].Rows[0][dsPayment.PatientPayments.PaymentIdColumn.ColumnName]

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
                    return JsonConvert.SerializeObject(response);
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        private string RefundPayment(Int64 PaymentID)
        {
            try
            {

                DSPayment dsNewPayment = new DSPayment();

                DSPayment.PatientPaymentsRow newPaymentRow = dsNewPayment.PatientPayments.NewPatientPaymentsRow();

                DSPayment dsPayment = null;


                BLObject<DSPayment> obj1 = BLLBillingObj.LoadPatientPayments(PaymentID, 0);
                dsPayment = obj1.Data;

                foreach (DSPayment.PatientPaymentsRow dr in dsPayment.Tables[dsPayment.PatientPayments.TableName].Rows)
                {

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.ChargeIdColumn.ColumnName]) != "")
                        newPaymentRow.ChargeId = MDVUtility.ToLong(dr[dsPayment.PatientPayments.ChargeIdColumn.ColumnName]);

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.PatientIdColumn.ColumnName]) != "")
                        newPaymentRow.PatientId = dr.PatientId;

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.AppointmentIdColumn.ColumnName]) != "")
                        newPaymentRow.AppointmentId = dr.AppointmentId;


                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.AdvPmtIdColumn.ColumnName]) != "")
                        newPaymentRow.AdvPmtId = dr.AdvPmtId;


                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.VisitIdColumn.ColumnName]) != "")
                        newPaymentRow.VisitId = dr.VisitId;

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.PmtBatchIdColumn.ColumnName]) != "")
                        newPaymentRow.PmtBatchId = dr.PmtBatchId;

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.ProviderIdColumn.ColumnName]) != "")
                        newPaymentRow.ProviderId = dr.ProviderId;



                   

                    //start syed zia 01-02-2016, bug #PMS-3801

                    //if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.CreatedByColumn.ColumnName]) != "")
                    //    newPaymentRow.CreatedBy = dr.CreatedBy;


                    //if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.CreatedOnColumn.ColumnName]) != "")
                    //    newPaymentRow.CreatedOn = dr.CreatedOn;

                    //if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.ModifiedByColumn.ColumnName]) != "")
                    //    newPaymentRow.ModifiedBy = dr.ModifiedBy;

                    //if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.ModifiedOnColumn.ColumnName]) != "")
                    //    newPaymentRow.ModifiedOn = dr.ModifiedOn;

                    newPaymentRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    newPaymentRow.CreatedOn = DateTime.Now;
                    newPaymentRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    newPaymentRow.ModifiedOn = DateTime.Now;

                    //end syed zia 01-02-2016, bug #PMS-3801

                    //setting the master payment id
                    newPaymentRow.MasterPaymentId = PaymentID;

                    if (dr.PmtTypeId == 2)
                    {
                        //cheque
                        if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.CheckNoColumn.ColumnName]) != "")
                            newPaymentRow.CheckNo = dr.CheckNo;

                        if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.CheckDateColumn.ColumnName]) != "")
                            newPaymentRow.CheckDate = dr.CheckDate;

                    }
                    else if (dr.PmtTypeId == 3)
                    {
                        //credit card
                        if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.CheckNoColumn.ColumnName]) != "")
                            newPaymentRow.CheckNo = dr.CheckNo;

                        if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.ExpiryDateColumn.ColumnName]) != "")
                            newPaymentRow.ExpiryDate = dr.ExpiryDate;

                        if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.CardTypeIdColumn.ColumnName]) != "")
                            newPaymentRow.CardTypeId = dr.CardTypeId;
                    }

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.AllowedColumn.ColumnName]) != "")
                        newPaymentRow.Allowed = dr.Allowed;

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.CopayColumn.ColumnName]) != "")
                        newPaymentRow.Copay = dr.Copay;

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.NextResponsibilityColumn.ColumnName]) != "")
                        newPaymentRow.NextResponsibility = dr.NextResponsibility;

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.CommentsColumn.ColumnName]) != "")
                        newPaymentRow.Comments = dr.Comments;

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.CoinsuranceColumn.ColumnName]) != "")
                        newPaymentRow.Coinsurance = dr.Coinsurance;

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.DeductablesColumn.ColumnName]) != "")
                        newPaymentRow.Deductables = dr.Deductables;


                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.PatientResponsibilityColumn.ColumnName]) != "")
                        newPaymentRow.PatientResponsibility = dr.PatientResponsibility;

                    if (MDVUtility.ToStr(dr[dsPayment.PatientPayments.CommentsColumn.ColumnName]) != "")
                        newPaymentRow.Comments = "Refunded |" + dr.Comments;

                    //ADDING ROW

                    dsNewPayment.PatientPayments.AddPatientPaymentsRow(newPaymentRow);

                }


                #region Database Insertion

                BLObject<DSPayment> obj = BLLBillingObj.InsertPatientPayments(dsNewPayment);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        //FIXME
                        Message = Common.AppPrivileges.Refund_Message,
                        PaymentId = dsNewPayment.Tables[dsNewPayment.PatientPayments.TableName].Rows[0][dsNewPayment.PatientPayments.PaymentIdColumn.ColumnName]
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Refund_Error_Message
                    };
                    return JsonConvert.SerializeObject(response);
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
                return (JsonConvert.SerializeObject(response));
            }

        }


        private string SearchCharge(string fieldsJSON, Int64 ChargeId, Int64 VisitId, Int32 PageNumber, Int32 RowsPerPage)
        {
            try
            {
               
                string PatientFullName = "";
                string ClaimNumber = "";
                string Paid = "";
                string accountNumber = "";
                string ClaimType = "";
                bool IncludeSecondaryClaim = true;
                string CaseNumber = "0";
                string CaseMgtId = "";
                string ClaimErrored = "";
                DateTime? DOSFrom = null, DOSTo = null;
                DSCharge dsCharge = null;
                BLObject<DSCharge> obj = null;

                if (ChargeId > 0 || VisitId > 0)
                    obj = BLLBillingObj.LoadPaymentCharges(CaseMgtId, ChargeId, "", "", 0, 0, "", 0, "", false, "", null, false, null, null, VisitId, 1, "", 0, PageNumber, RowsPerPage);
                else
                {

                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    if (SearchedfieldsJSON.ContainsKey("txtPatientFullName"))
                    {
                        PatientFullName = !string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientFullName"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtPatientFullName"]) : "";
                        string[] arrPatientName = PatientFullName.Split(',');
                    }
                    string LastName = null;// arrPatientName.Length > 0 ? arrPatientName[0] : "";
                    string FirstName = null;// arrPatientName.Length > 1 ? arrPatientName[1] : "";
                    if (SearchedfieldsJSON.ContainsKey("txtClaimNumber"))
                        ClaimNumber = !string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtClaimNumber"]) : "";

                    if (SearchedfieldsJSON.ContainsKey("chkIncludeSecondaryClaim"))
                        IncludeSecondaryClaim = Convert.ToBoolean(SearchedfieldsJSON["chkIncludeSecondaryClaim"]);

                    if (SearchedfieldsJSON.ContainsKey("txtCaseNumber"))
                        CaseNumber = !string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtCaseNumber"]) : "0";
                    if (SearchedfieldsJSON.ContainsKey("ddlAmountReceived"))
                        Paid = !string.IsNullOrEmpty(SearchedfieldsJSON["ddlAmountReceived"]) ? MDVUtility.ToStr(SearchedfieldsJSON["ddlAmountReceived"]) : "";
                    if (SearchedfieldsJSON.ContainsKey("txtPatientName"))
                        accountNumber = !string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientName"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtPatientName"]) : "";

                    if (SearchedfieldsJSON.ContainsKey("hfCaseId"))
                        CaseMgtId = !string.IsNullOrEmpty(SearchedfieldsJSON["hfCaseId"]) ? MDVUtility.ToStr(SearchedfieldsJSON["hfCaseId"]) : "";

                    if (SearchedfieldsJSON.ContainsKey("ddlClaimType"))
                        ClaimType = !string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimType"]) ? MDVUtility.ToStr(SearchedfieldsJSON["ddlClaimType"]) : "";

                    if (SearchedfieldsJSON.ContainsKey("dpDOSFrom"))
                        DOSFrom = String.IsNullOrEmpty(SearchedfieldsJSON["dpDOSFrom"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpDOSFrom"]);

                    if (SearchedfieldsJSON.ContainsKey("ddlClaimErrored"))
                        ClaimErrored = String.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimErrored"]);

                    if (SearchedfieldsJSON.ContainsKey("dpDOSTo"))
                        DOSTo = String.IsNullOrEmpty(SearchedfieldsJSON["dpDOSTo"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpDOSTo"]);

                    if (DOSFrom == null)
                        DOSFrom = DOSTo;
                    if (DOSTo == null)
                        DOSTo = DOSFrom;


                    obj = BLLBillingObj.LoadPaymentCharges(CaseMgtId, 0, LastName, FirstName, 0, 0, "", 0, ClaimNumber, IncludeSecondaryClaim, Paid, ClaimErrored, false, DOSFrom, DOSTo, 0, 1, accountNumber, 0, PageNumber, RowsPerPage, null, ClaimType);

                }
                dsCharge = obj.Data;
                if (dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        ChargeCount = dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count,
                        iTotalDisplayRecords = dsCharge.PatientCharges.Rows[0][dsCharge.PatientCharges.RecordCountColumn.ColumnName],
                        ChargeLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientCharges.TableName]),
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ChargeCount = 0,
                        Message = "Record not found."
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

        public string SearchCharge(PaymentPostingModel model)
        {
            try
            {

                DSCharge dsCharge = null;
                BLObject<DSCharge> obj = null;
                string PatientFullName = "";
                string ClaimNumber = "";
                string Paid = "";
                string accountNumber = "";
                string ClaimType = "";
                bool IncludeSecondaryClaim = true;
                string CaseNumber = "0";
                string CaseMgtId = "";
                DateTime? DOSFrom = null, DOSTo = null;
                bool isVoidedClaim = false;
                isVoidedClaim = Convert.ToBoolean(model.IncludeVoidedClaims);
                DOSFrom = String.IsNullOrEmpty(model.DateDOSFrom) ? (DateTime?)null : DateTime.Parse(model.DateDOSFrom);
                DOSTo = String.IsNullOrEmpty(model.DateDOSTo) ? (DateTime?)null : DateTime.Parse(model.DateDOSTo);


                if (DOSFrom == null)
                    DOSFrom = DOSTo;
                if (DOSTo == null)
                    DOSTo = DOSFrom;

                SharedVariable sharedVariable = SharedVariable.GetSharedVariable();

                BLLBillingObj = new BLLBilling(sharedVariable);
                List<GenericLookupModel> remitCodes = new List<GenericLookupModel>();
                List<LedgerAccountModel> ledgerAccountsList = new List<LedgerAccountModel>();

                List<Task> tasks = new List<Task>();

               

                if (MDVUtility.ToInt64(model.ChargeId) > 0 || MDVUtility.ToInt64(model.VisitId) > 0)
                {
                    obj = BLLBillingObj.LoadPaymentCharges(model.CaseId, MDVUtility.ToInt64(model.ChargeId), "", "", 0, 0, "", 0, "", false, "", model.ClaimErroredId, isVoidedClaim, DOSFrom, DOSTo, MDVUtility.ToInt64(model.VisitId), 1, "", 0, MDVUtility.ToInt(model.PageNumber), MDVUtility.ToInt(model.RowsPerPage), null, null, PatientFullName);
                }
                else
                {

                    PatientFullName = !string.IsNullOrEmpty(model.PatientName) ? MDVUtility.ToStr(model.PatientName) : "";
                    string[] arrPatientName = PatientFullName.Split(' ');
                    string LastName = null; //arrPatientName.Length > 0 ? arrPatientName[0] : "";
                    string FirstName = null;//arrPatientName.Length > 1 ? arrPatientName[1] : "";

                    ClaimNumber = !string.IsNullOrEmpty(model.ClaimNumber) ? MDVUtility.ToStr(model.ClaimNumber) : "";
                    IncludeSecondaryClaim = Convert.ToBoolean(model.IncludeSecondaryClaim);
                    CaseNumber = !string.IsNullOrEmpty(model.CaseNumber) ? MDVUtility.ToStr(model.CaseNumber) : "0";
                    Paid = !string.IsNullOrEmpty(model.AmountReceived) ? MDVUtility.ToStr(model.AmountReceived) : "";
                    accountNumber = !string.IsNullOrEmpty(model.PatientAccount) ? MDVUtility.ToStr(model.PatientAccount) : "";
                    CaseMgtId = !string.IsNullOrEmpty(model.CaseId) ? MDVUtility.ToStr(model.CaseId) : "";
                    ClaimType = !string.IsNullOrEmpty(model.ClaimType) ? MDVUtility.ToStr(model.ClaimType) : "";



                    obj = BLLBillingObj.LoadPaymentCharges(CaseMgtId, 0, LastName, FirstName, 0, 0, "", 0, ClaimNumber, IncludeSecondaryClaim, Paid, model.ClaimErroredId, isVoidedClaim, DOSFrom, DOSTo, 0, 1, accountNumber, 0, MDVUtility.ToInt(model.PageNumber), MDVUtility.ToInt(model.RowsPerPage), null, ClaimType, PatientFullName);

                }
                Task.WaitAll(tasks.ToArray());
                dsCharge = obj.Data;
                if (dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        ChargeCount = dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count,
                        iTotalDisplayRecords = dsCharge.PatientCharges.Rows[0][dsCharge.PatientCharges.RecordCountColumn.ColumnName],
                        ChargeLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientCharges.TableName]),
                        AllLedgerAccounts_JSON = ledgerAccountsList,
                        RemitCodes_JSON = remitCodes,

                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ChargeCount = 0,
                        Message = "Record not found."
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
        private Int64 getLedgerAccountId(Int64 TypeId, Int64 ApplyToId, Int64 SystemCategory = 0, Int64 IsSystem = -1)
        {
            BLObject<DSPaymentSetup> ledgerAccount = BLLBillingObj.LookupLedgerAccount(TypeId, ApplyToId, SystemCategory, IsSystem);
            DSPaymentSetup ds = ledgerAccount.Data;

            if (ds.Tables[ds.LedgerAccount1.TableName].Rows.Count > 0)
            {
                return MDVUtility.ToInt64(ds.Tables[ds.LedgerAccount1.TableName].Rows[0][ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName]);
            }

            throw new Exception(TypeId + " , " + ApplyToId + " Ledger Doesn't Exists");
        }

        private void validatePaymentBalance(PaymentPostingModel model, string paymentType)
        {

            double totalPaid = 0;
            double advancePaid = 0;

            if (chargeIdField <= 0)
                throw new Exception("Charge Doesn't Exists");

            if (paymentTypeField == "insurance")
            {

                if (insurancePaidField > 0)
                    totalPaid += insurancePaidField;

                if (insuranceWriteoffField > 0)
                    totalPaid += insuranceWriteoffField;


                //summing up transfer entries
                if (insuranceCoinsuranceField > 0)
                    totalPaid += insuranceCoinsuranceField;

                if (insuranceDeductablesField > 0)
                    totalPaid += insuranceDeductablesField;

                if (insurancePatientResponsibilityField > 0)
                    totalPaid += insurancePatientResponsibilityField;

                //summing up charge copay

                if (insuranceChargeCopayField > 0)
                    totalPaid += insuranceChargeCopayField;

            }
            else if (paymentTypeField == "patient")
            {

                //if payment type is advance payment then don't consider "txtPatientPaid"
                if (paymentModeField == 4)
                {
                    if (patientPaidField > 0)
                        advancePaid += patientPaidField;


                    //being called from advance payment CS
                    Patient_AdvancePayment.Instance().validateAdvancePayment(patientIdField, advancePaid, patientAdvancePaymentIdField);
                    //  validateAdvancePayment(patientIdField, advancePaid, patientAdvancePaymentIdField);

                }
                else
                {
                    if (patientPaidField > 0)
                        totalPaid += patientPaidField;

                }

                if (patientDiscountField > 0)
                    totalPaid += patientDiscountField;


                //summing up transfer entries
                if (patientCoinsuranceField > 0)
                    totalPaid += patientCoinsuranceField;

                if (patientDeductablesField > 0)
                    totalPaid += patientDeductablesField;

                if (patientPatientResponsibilityField > 0)
                    totalPaid += patientPatientResponsibilityField;

                if (CopaymentTransferField > 0)
                    totalPaid += CopaymentTransferField;

            }
            //else if (paymentTypeField == "copayment")
            //{
            //    if (copaymentPaidField > 0)
            //        totalPaid += copaymentPaidField;

            //    if (copaymentDiscountField > 0)
            //        totalPaid += copaymentDiscountField;


            //    if (CopaymentTransferField > 0)
            //        totalPaid += CopaymentTransferField;
            //}

            //rounding off
            totalPaid = Math.Round(totalPaid, 2);

            //DB CALL

            try
            {
                /*
                DSCharge dsCharge = null;
                BLObject<DSCharge> obj;

                obj = BLLBillingObj.LoadPaymentCharges(null,chargeIdField, "", "", 0, 0, "", 0, "", "", null, null, 0, 1, "", 0, 1, 1000);

                dsCharge = obj.Data;


                if (dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count > 0)
                {
                    double amountPayable = 0;
                    if (paymentTypeField == "insurance")
                    {
                        amountPayable = MDVUtility.ToDouble(dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows[0][dsCharge.PatientCharges.InsBalanceColumn.ColumnName]);
                    }
                    else if (paymentTypeField == "patient")
                    {
                        amountPayable = MDVUtility.ToDouble(dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows[0][dsCharge.PatientCharges.PatBalanceColumn.ColumnName]);
                    }
                    else if (paymentTypeField == "copayment")
                    {
                        amountPayable = MDVUtility.ToDouble(dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows[0][dsCharge.PatientCharges.CopayBalanceColumn.ColumnName]);
                    }
                */


                if (totalPaid <= 0 && advancePaid <= 0 && !isDeniedField && paymentTypeField != "zeropayment")
                {
                    throw new Exception("Please specify a valid amount");
                }

                //if (amountPayable >= totalPaid && amountPayable >= advancePaid)
                //{
                //    return;
                //}
                //else
                //{
                //    throw new Exception("Paid Amount should not be greater than " + paymentType + " Balance");
                //}

                // }


                //  throw new Exception(" Balance Doesn't Exists");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private void fillRow(ref DSPayment.PatientPaymentsRow drPatientPaymentPaidRow, string fieldsJSON, string paymentType)
        {

            /****** COMMON CONTROLS START **********/

            if (chargeIdField > 0)
            {
                drPatientPaymentPaidRow.ChargeId = chargeIdField;
            }

            if (patientIdField > 0)
            {
                drPatientPaymentPaidRow.PatientId = patientIdField;
            }

            if (visitIdField > 0)
            {
                drPatientPaymentPaidRow.VisitId = visitIdField;
            }
            if (paymentBatchField > 0)
            {
                drPatientPaymentPaidRow.PmtBatchId = paymentBatchField;
            }
            if (providerIdField > 0)
            {
                drPatientPaymentPaidRow.ProviderId = providerIdField;
            }
            if (facilityIdField > 0)
            {
                drPatientPaymentPaidRow.FacilityId = facilityIdField;
            }
            if (datePaidField != null)
            {
                drPatientPaymentPaidRow.PaymentDate = MDVUtility.ToDateTime(datePaidField);
            }
            if (paymentModeField > 0)
            {
                drPatientPaymentPaidRow.PmtTypeId = paymentModeField;
            }
            if (paymentModeField == 2)
            {
                //check

                drPatientPaymentPaidRow.CheckNo = checkNumberField;

                if (checkDateField != null)
                {
                    drPatientPaymentPaidRow.CheckDate = MDVUtility.ToStr(checkDateField);
                }
            }
            else if (paymentModeField == 3)
            {
                //credit card

                drPatientPaymentPaidRow.CheckNo = cardNumberField;

                if (cardExpiryDateField != null)
                {
                    drPatientPaymentPaidRow.ExpiryDate = MDVUtility.ToStr(cardExpiryDateField);
                }
                if (cardTypeField > 0)
                {
                    drPatientPaymentPaidRow.CardTypeId = cardTypeField;
                }
            }

            else if (paymentModeField == 4)
            {
                //Advance Payment

                if (patientAdvancePaymentIdField > 0)
                {

                    //  drPatientPaymentPaidRow.AdvPmtId = patientAdvancePaymentIdField;


                }

                //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientPaid"]))
                //    validateAdvancePayment(MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]), MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientPaid"]), MDVUtility.ToLong(SearchedfieldsJSON["hfAdvancePaymentId"]));
            }

            //if (paymentType.ToLower() == "insurance")
            //{
            //    drPatientPaymentPaidRow.NextResponsibility = MDVUtility.ToStr(SearchedfieldsJSON["RadInsuranceNextRespPatient"]) == "True" ? "Patient" : "Insurance";
            //}
            //else if (paymentType.ToLower() == "patient")
            //{
            //    drPatientPaymentPaidRow.NextResponsibility = MDVUtility.ToStr(SearchedfieldsJSON["RadPatNextRespPatient"]) == "True" ? "Patient" : "Insurance";
            //}

            /******COMMON CONTROLS END**********/

            drPatientPaymentPaidRow.IsActive = true;

            drPatientPaymentPaidRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            drPatientPaymentPaidRow.CreatedOn = DateTime.Now;
            drPatientPaymentPaidRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            drPatientPaymentPaidRow.ModifiedOn = DateTime.Now;



        }

        private void setAllFields(PaymentPostingModel model, string paymentType)
        {
            resetAllFields();


            paymentTypeField = paymentType.ToLower();


            #region SETTING COMMON CONTROLS

            if (!string.IsNullOrEmpty(model.HiddenFieldChargeId))
            {
                chargeIdField = MDVUtility.ToInt64(model.HiddenFieldChargeId);
            }

            if (!string.IsNullOrEmpty(model.PatientId))
            {
                patientIdField = MDVUtility.ToInt64(model.PatientId);
            }

            if (!string.IsNullOrEmpty(model.HiddenFieldVisitId))
            {
                visitIdField = MDVUtility.ToInt64(model.HiddenFieldVisitId);
            }
            if (!string.IsNullOrEmpty(model.HiddenFieldPaymentBatch))
            {
                paymentBatchField = MDVUtility.ToInt64(model.HiddenFieldPaymentBatch);
            }
            if (!string.IsNullOrEmpty(model.ProviderId))
            {
                providerIdField = MDVUtility.ToInt64(model.ProviderId);
            }
            if (!string.IsNullOrEmpty(model.FacilityId))
            {
                facilityIdField = MDVUtility.ToInt64(model.FacilityId);
            }
            if (!string.IsNullOrEmpty(model.DatePaid))
            {
                datePaidField = MDVUtility.ToDateTime(model.DatePaid);
            }
            if (!string.IsNullOrEmpty(model.PaymentMode))
            {
                paymentModeField = MDVUtility.ToInt32(model.PaymentMode);
            }
            if (!string.IsNullOrEmpty(model.IsDenied))
            {
                isDeniedField = MDVUtility.ToStr(model.IsDenied) == "true" ? true : false;
            }
            if (!string.IsNullOrEmpty(model.InsurancePlanId))
            {
                InsurancePlanId = MDVUtility.ToInt64(model.InsurancePlanId);
            }

            if (!string.IsNullOrEmpty(model.PatientInsurancePlanId))
            {
                PatientInsurancePlanId = MDVUtility.ToInt64(model.PatientInsurancePlanId);
            }

            if (!string.IsNullOrEmpty(model.InsurancePlanPriority))
            {
                InsurancePlanPriority = MDVUtility.ToInt32(model.InsurancePlanPriority);
            }

            if (paymentModeField == 2)
            {
                //check

                checkNumberField = model.checkNumber;

                if (!string.IsNullOrEmpty(model.checkDate))
                {
                    checkDateField = MDVUtility.ToStr(model.checkDate);
                }
            }
            else if (paymentModeField == 3)
            {
                //credit card

                cardNumberField = MDVUtility.ToStr(model.cardNumber);

                if (!string.IsNullOrEmpty(model.expiryDate))
                {
                    cardExpiryDateField = MDVUtility.ToStr(model.expiryDate);
                }
                if (!string.IsNullOrEmpty(model.creditCardType))
                {
                    cardTypeField = MDVUtility.ToInt32(model.creditCardType);
                }
            }

            else if (paymentModeField == 4)
            {
                //Advance Payment

                if (!string.IsNullOrEmpty(model.AdvancePaymentId))//"SearchedfieldsJSON.ContainsKey("hfAdvancePaymentId") &&" This check was commented here! during model and controller writing Author: Abdur Rehman Date March 29th,2016
                {

                    patientAdvancePaymentIdField = MDVUtility.ToLong(model.AdvancePaymentId);


                }


            }
            #endregion

            #region SETTING INSURANCE CONTROLS

            if (!string.IsNullOrEmpty(model.InsuranceAllowed))
            {
                insuranceAllowedField = MDVUtility.ToDouble(model.InsuranceAllowed);
            }

            if (!string.IsNullOrEmpty(model.InsurancePaid))
                insurancePaidField = MDVUtility.ToDouble(model.InsurancePaid);

            if (!string.IsNullOrEmpty(model.InsurancePaidAccount))
                insurancePaidAccountIdField = MDVUtility.ToInt32(model.InsurancePaidAccount);

            if (!string.IsNullOrEmpty(model.InsuranceWriteoff))
                insuranceWriteoffField = MDVUtility.ToDouble(model.InsuranceWriteoff);

            if (!string.IsNullOrEmpty(model.InsuranceWriteoffAccount))
                insuranceWriteoffAccountIdField = MDVUtility.ToInt32(model.InsuranceWriteoffAccount);


            if (!string.IsNullOrEmpty(model.RemittanceCode))
            {
                insuranceRemitCodeField = MDVUtility.ToInt32(model.RemittanceCode);
            }

            if (!string.IsNullOrEmpty(model.ICN_DCN))
            {
                insuranceIcnDcnField = MDVUtility.ToStr(model.ICN_DCN);
            }
            if (!string.IsNullOrEmpty(model.InsuranceCopay))
            {
                insuranceChargeCopayField = MDVUtility.ToDouble(model.InsuranceCopay);
            }

            if ((!string.IsNullOrEmpty(model.InsuranceCoinsurance)))
            {
                insuranceCoinsuranceField = MDVUtility.ToDouble(model.InsuranceCoinsurance);
            }

            if ((!string.IsNullOrEmpty(model.InsuranceDeductables)))
            {
                insuranceDeductablesField = MDVUtility.ToDouble(model.InsuranceDeductables);
            }

            if ((!string.IsNullOrEmpty(model.InsurancePatientResponsibilityr)))
            {
                insurancePatientResponsibilityField = MDVUtility.ToDouble(model.InsurancePatientResponsibilityr);
            }

            insuranceNextResponsibilityField = MDVUtility.ToStr(model.InsNextResPatient) == "True" ? "patient" : "insurance";

            //next insurance plan id will only be set when next responsibility is "Insurance"
            if ((insuranceNextResponsibilityField == "insurance") && (!string.IsNullOrEmpty(model.NextInsuranceInsurancePlan)))
            {
                insuranceNextInsurancePlanIdField = MDVUtility.ToInt32(model.NextInsuranceInsurancePlan);
            }
            if (paymentTypeField == "recoupment")
            {
                insuranceNextResponsibilityField = MDVUtility.ToStr(model.RecoupNextResPatient) == "True" ? "patient" : "insurance";

                if ((insuranceNextResponsibilityField == "insurance") && (!string.IsNullOrEmpty(model.RecoupmentNextInsurancePlanId)))
                {
                    insuranceNextInsurancePlanIdField = MDVUtility.ToInt32(model.RecoupmentNextInsurancePlanId);
                }
            }
            insuranceCrossOverField = MDVUtility.ToStr(model.InsuranceCrossOve).ToLower() == "true" ? true : false;

            insPrintOnPatStmtField = MDVUtility.ToStr(model.PrintOnPatientStatement) == "True" ? true : false;
            string DeniedCommment = "";
            //if (isDeniedField)
            //{
            //    DeniedCommment = " DENIED:" + (checkNumberField != "" ? " Under the check Number " + checkNumberField + "| " : "");
            //}
            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtInsuranceComments"]))
            //{
            //insuranceCommentsField = DeniedCommment + MDVUtility.ToStr(SearchedfieldsJSON["txtInsuranceComments"]) + "|";
            //insuranceCommentsField += MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + " " + DateTime.Now;
            //}

            //start syed Zia 29-01-2016,for denied comments
            if (isDeniedField)
            {
                DeniedCommment = "DENIED:" + (checkNumberField != "" ? " Under the check Number " + checkNumberField + "| " : "");
                insuranceCommentsField += DeniedCommment;
            }
            if (!string.IsNullOrEmpty(model.InsuranceComments))
            {
                insuranceCommentsField = DeniedCommment + MDVUtility.ToStr(model.InsuranceComments) + "|";               
            }
            //insuranceCommentsField += MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + " " + DateTime.Now;
            insuranceCommentsField += MDVSession.Current.AppUserFullName + " " + DateTime.Now;

            //end syed Zia 29-01-2016,for denied comments

            #endregion

            #region SETTING PATIENT CONTROLS


            if (!string.IsNullOrEmpty(model.PatientPaid))
                patientPaidField = MDVUtility.ToDouble(model.PatientPaid);

            if (!string.IsNullOrEmpty(model.PatientPaidAccount))
                patientPaidAccountIdField = MDVUtility.ToInt32(model.PatientPaidAccount);


            if (!string.IsNullOrEmpty(model.PatientDiscount))
                patientDiscountField = MDVUtility.ToDouble(model.PatientDiscount);

            if (!string.IsNullOrEmpty(model.PatientDiscountAccount))
                patientDiscountAccountIdField = MDVUtility.ToInt32(model.PatientDiscountAccount);

            if (!string.IsNullOrEmpty(model.Patientcoinsurance))
                patientCoinsuranceField = MDVUtility.ToDouble(model.Patientcoinsurance);

            if (!string.IsNullOrEmpty(model.Patientdeductables))
                patientDeductablesField = MDVUtility.ToDouble(model.Patientdeductables);

            if (!string.IsNullOrEmpty(model.PatientPatientResponsibility))
                patientPatientResponsibilityField = MDVUtility.ToDouble(model.PatientPatientResponsibility);


            patientNextResponsibilityField = MDVUtility.ToStr(model.RadPatNextResponsibility) == "True" ? "insurance" : "patient";

            patPrintOnPatStmtField = MDVUtility.ToStr(model.PatPrintOnPatientStatement) == "True" ? true : false;

            if (!string.IsNullOrEmpty(model.PatientComments))
            {
                patientCommentsField = MDVUtility.ToStr(model.PatientComments);
                //patientCommentsField += "|" + MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + " " + DateTime.Now;
                patientCommentsField += "|" + MDVSession.Current.AppUserFullName + " " + DateTime.Now;
            }

            if (!string.IsNullOrEmpty(model.PatientComments))
            {
                patientCommentsField = MDVUtility.ToStr(model.PatientComments) + "|";
            }

            patientCommentsField += MDVSession.Current.AppUserFullName + " " + DateTime.Now;
            //insuranceCommentsField += MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + " " + DateTime.Now;
            //insuranceCommentsField += MDVSession.Current.AppUserFullName + " " + DateTime.Now;

            #endregion

            #region SETTING COPAYMENT CONTROLS

            if (!string.IsNullOrEmpty(model.PatientPaid))
                copaymentPaidField = MDVUtility.ToDouble(model.PatientPaid);

            if (!string.IsNullOrEmpty(model.PatientPaidAccount))
                copaymentPaidAccountIdField = MDVUtility.ToInt32(model.PatientPaidAccount);


            if (!string.IsNullOrEmpty(model.PatientDiscount))
                copaymentDiscountField = MDVUtility.ToDouble(model.PatientDiscount);

            if (!string.IsNullOrEmpty(model.PatientDiscountAccount))
                copaymentDiscountAccountIdField = MDVUtility.ToInt32(model.PatientDiscountAccount);

            CopaymentNextResponsibilityField = MDVUtility.ToStr(model.RadPatNextResponsibility) == "True" ? "insurance" : "patient";

            if (!string.IsNullOrEmpty(model.CopayAmount))
            {
                CopaymentTransferField = MDVUtility.ToDouble(model.CopayAmount);
            }

            copaymentPrintOnPatStmtField = MDVUtility.ToStr(model.PatPrintOnPatientStatement) == "True" ? true : false;

            //if (!string.IsNullOrEmpty(model.PatientComments))
            //{
            //    copaymentCommentsField = MDVUtility.ToStr(model.PatientComments);
            //    //copaymentCommentsField += "|" + MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + " " + DateTime.Now;
            //    copaymentCommentsField += "|" + AppConfig.AppUserFullName + " " + DateTime.Now;
            //}
            if (!string.IsNullOrEmpty(model.PatientComments))
            {
                copaymentCommentsField = MDVUtility.ToStr(model.PatientComments) + "|";
               
            }

            copaymentCommentsField += MDVSession.Current.AppUserFullName + " " + DateTime.Now;



            #endregion

            #region SETTING ZERO PAYMENT CONTROLS

            zeroPaymentPrintOnPatStmtField = MDVUtility.ToStr(model.ZeroPayPrintOnPatientStatement) == "True" ? true : false;

            if (!string.IsNullOrEmpty(model.ZeroPaymentComments))
            {
                zeroPaymentCommentsField = MDVUtility.ToStr(model.ZeroPaymentComments);
                //zeroPaymentCommentsField += "|" + MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + " " + DateTime.Now;
                zeroPaymentCommentsField += "|" + MDVSession.Current.AppUserFullName + " " + DateTime.Now;
            }
            //adnan maqbool,PMS-4195, 25-02-2016 
            else
            {
                 zeroPaymentCommentsField = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + " " + DateTime.Now;

            }
            //end

            #endregion

            #region SETTING RECOUPMENT CONTROLS

            if (!string.IsNullOrEmpty(model.RecoupmentAllowed))
            {
                recoupmentAllowedField = MDVUtility.ToDouble(model.RecoupmentAllowed);
            }

            if (!string.IsNullOrEmpty(model.RecoupmentPaid))
                recoupmentPaidField = MDVUtility.ToDouble(model.RecoupmentPaid);

            if (!string.IsNullOrEmpty(model.RecoupmentPaidAccount))
                recoupmentPaidAccountIdField = MDVUtility.ToInt32(model.RecoupmentPaidAccount);

            if (!string.IsNullOrEmpty(model.RecoupmentWriteoff))
                recoupmentWriteoffField = MDVUtility.ToDouble(model.RecoupmentWriteoff);

            if (!string.IsNullOrEmpty(model.RecoupmentWriteoffAccount))
                recoupmentWriteoffAccountIdField = MDVUtility.ToInt32(model.RecoupmentWriteoffAccount);


            if (!string.IsNullOrEmpty(model.RecoupmentRemittanceCode))
            {
                recoupmentRemitCodeField = MDVUtility.ToInt32(model.RecoupmentRemittanceCode);
            }

            if (!string.IsNullOrEmpty(model.RecoupmentICN_DCN))
            {
                recoupmentIcnDcnField = MDVUtility.ToStr(model.RecoupmentICN_DCN);
            }
            if (!string.IsNullOrEmpty(model.RecoupmentCopay))
            {
                recoupmentChargeCopayField = MDVUtility.ToDouble(model.RecoupmentCopay);
            }

            if ((!string.IsNullOrEmpty(model.RecoupmentCoinsurance)))
            {
                recoupmentCoinsuranceField = MDVUtility.ToDouble(model.RecoupmentCoinsurance);
            }

            if ((!string.IsNullOrEmpty(model.RecoupmentDeductables)))
            {
                recoupmentDeductablesField = MDVUtility.ToDouble(model.RecoupmentDeductables);
            }

            if ((!string.IsNullOrEmpty(model.RecoupmentPatientResponsibility)))
            {
                recoupmentPatientResponsibilityField = MDVUtility.ToDouble(model.RecoupmentPatientResponsibility);
            }

            recoupmentNextResponsibilityField = MDVUtility.ToStr(model.RadRecoupmentNextResponsibility) == "True" ? "insurance" : "patient";

            //next insurance plan id will only be set when next responsibility is "Recoupment"
            //if ((recoupmentNextResponsibilityField == "insurance") && (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlRecoupmentNextInsuranceInsurancePlan"])))
            //{
            //    recoupmentNextInsurancePlanIdField = MDVUtility.ToInt32(SearchedfieldsJSON["ddlRecoupmentNextInsuranceInsurancePlan"]);
            //}

            //recoupmentCrossOverField = MDVUtility.ToStr(SearchedfieldsJSON["chkRecoupmentCrossOver"]).ToLower() == "true" ? true : false;

            recoupmentPrintOnPatStmtField = MDVUtility.ToStr(model.RecoupmentInsPrintOnPatientStatement) == "True" ? true : false;
            //string DeniedCommment = "";
            //if (isDeniedField)
            //{
            //    DeniedCommment = " DENIED:" + (checkNumberField != "" ? " Under the check Number " + checkNumberField + "| " : "");
            //}
            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtInsuranceComments"]))
            //{
            //insuranceCommentsField = DeniedCommment + MDVUtility.ToStr(SearchedfieldsJSON["txtInsuranceComments"]) + "|";
            //insuranceCommentsField += MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + " " + DateTime.Now;
            //}

            if (isDeniedField)
            {
                DeniedCommment = " DENIED:" + (checkNumberField != "" ? " Under the check Number " + checkNumberField + "| " : "");
                recoupmentCommentsField += DeniedCommment;
            }
            if (!string.IsNullOrEmpty(model.RecoupmentComments))
            {
                recoupmentCommentsField = DeniedCommment + MDVUtility.ToStr(model.RecoupmentComments) + "|";
             
            }
            recoupmentCommentsField += MDVSession.Current.AppUserFullName + " " + DateTime.Now;



            #endregion


        }

        private void resetAllFields()
        {
            #region FIELDS

            #region COMMON FIELDS

            paymentTypeField = "";
            chargeIdField = -1;
            patientIdField = -1;
            visitIdField = -1;
            paymentBatchField = -1;
            providerIdField = -1;
            facilityIdField = -1;
            datePaidField = null;
            paymentModeField = -1;
            checkNumberField = "";
            checkDateField = null;
            cardNumberField = "";
            cardExpiryDateField = null;
            cardTypeField = 0;
            isDeniedField = false;


            #endregion

            #region INSURANCE FIELDS


            insuranceAllowedField = 0;
            insurancePaidField = 0;
            insurancePaidAccountIdField = 0;
            insuranceWriteoffField = 0;
            insuranceWriteoffAccountIdField = 0;
            insuranceChargeCopayField = 0;
            insuranceRemitCodeField = 0;
            insuranceIcnDcnField = "";

            insuranceCoinsuranceField = 0;
            insuranceDeductablesField = 0;
            insurancePatientResponsibilityField = 0;
            insuranceNextResponsibilityField = "";
            insuranceNextInsurancePlanIdField = 0;
            insuranceCrossOverField = false;

            insPrintOnPatStmtField = false;
            insuranceCommentsField = "";




            #endregion

            #region PATIENT FIELDS


            patientPaidField = 0;
            patientPaidAccountIdField = 0;
            patientDiscountField = 0;
            patientDiscountAccountIdField = 0;
            patientAdvancePaymentIdField = 0;

            patientCoinsuranceField = 0;
            patientDeductablesField = 0;
            patientPatientResponsibilityField = 0;

            patientNextResponsibilityField = "";
            patientNextInsurancePlanIdField = 0;
            patPrintOnPatStmtField = false;
            patientCommentsField = "";

            #endregion

            #region COPAYMENT FIELDS

            copaymentPaidField = 0;
            copaymentPaidAccountIdField = 0;
            copaymentDiscountField = 0;
            copaymentDiscountAccountIdField = 0;
            CopaymentNextResponsibilityField = "";
            CopaymentTransferField = 0;

            copaymentPrintOnPatStmtField = false;
            copaymentCommentsField = "";



            #endregion

            #region ZERO PAYMENT FIELDS

            zeroPaymentPrintOnPatStmtField = false;
            zeroPaymentCommentsField = "";

            #endregion

            #region RECOUPMENT FIELDS


            recoupmentAllowedField = 0;
            recoupmentPaidField = 0;
            recoupmentPaidAccountIdField = 0;
            recoupmentWriteoffField = 0;
            recoupmentWriteoffAccountIdField = 0;
            recoupmentChargeCopayField = 0;
            recoupmentRemitCodeField = 0;
            recoupmentIcnDcnField = "";
            recoupmentCoinsuranceField = 0;
            recoupmentDeductablesField = 0;
            recoupmentPatientResponsibilityField = 0;
            recoupmentNextResponsibilityField = "";
            recoupmentNextInsurancePlanIdField = 0;
            recoupmentCrossOverField = false;
            recoupmentPrintOnPatStmtField = false;
            recoupmentCommentsField = "";
            recoupmentIsRecoupmentField = false;




            #endregion

            #endregion

        }


        #region INSURANCE ROWS FUNCTIONS
        private void insertInsurancePaidRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow insurancePaidRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref insurancePaidRow, "", "");

            if (insuranceAllowedField > 0 && !isDeniedField)
            {
                insurancePaidRow.Allowed = insuranceAllowedField;
            }

            if (insurancePaidField > 0 && !isDeniedField)
            {
                insurancePaidRow.PaidAmountDr = insurancePaidField;
            }
            else if (isDeniedField)
            {
                insurancePaidRow.PaidAmountDr = 0;
            }

            if (insurancePaidAccountIdField > 0)
            {
                insurancePaidRow.LedgerAccId = insurancePaidAccountIdField;
            }
            else
            {
                throw new Exception("Please select Paid A/C");
            }

            if (insuranceRemitCodeField > 0)
            {
                insurancePaidRow.RemitCodeId = insuranceRemitCodeField;
            }

            if (insuranceIcnDcnField != "")
            {
                insurancePaidRow.ICNDCN = insuranceIcnDcnField;
            }
            //if (insuranceChargeCopayField > 0)
            //{
            //    insurancePaidRow.Copay = insuranceChargeCopayField;
            //}

            if (insuranceCommentsField != "")
            {
                insurancePaidRow.Comments = insuranceCommentsField;
            }

            if (InsurancePlanId > 0)
            {
                insurancePaidRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                insurancePaidRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                insurancePaidRow.InsurancePlanPriority = InsurancePlanPriority;
            }


            insurancePaidRow.PrintOnPatStmt = insPrintOnPatStmtField;
            insurancePaidRow.IsDenied = isDeniedField;

            dsPayment.PatientPayments.AddPatientPaymentsRow(insurancePaidRow);


        }

        private void insertInsuranceWriteoffRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow insuranceWiteoffRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref insuranceWiteoffRow, "", "");


            if (insuranceAllowedField > 0)
            {
                insuranceWiteoffRow.Allowed = insuranceAllowedField;
            }

            if (insuranceWriteoffField > 0)
                insuranceWiteoffRow.PaidAmountDr = insuranceWriteoffField;

            if (insuranceWriteoffAccountIdField > 0)
                insuranceWiteoffRow.LedgerAccId = insuranceWriteoffAccountIdField;

            if (insuranceRemitCodeField > 0)
            {
                insuranceWiteoffRow.RemitCodeId = insuranceRemitCodeField;
            }

            if (insuranceIcnDcnField != "")
            {
                insuranceWiteoffRow.ICNDCN = insuranceIcnDcnField;
            }
            //if (insuranceChargeCopayField > 0)
            //{
            //    insuranceWiteoffRow.Copay = insuranceChargeCopayField;
            //}

            if (InsurancePlanId > 0)
            {
                insuranceWiteoffRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                insuranceWiteoffRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                insuranceWiteoffRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            if (insuranceCommentsField != "")
            {
                insuranceWiteoffRow.Comments = insuranceCommentsField;
            }

            insuranceWiteoffRow.IsDenied = isDeniedField;
            insuranceWiteoffRow.PrintOnPatStmt = insPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(insuranceWiteoffRow);

        }

      
        private void insertInsuranceChargeCopayRow(DSPayment dsPayment,string nextResponsibleInCaseofZeroAmt)
        {

            DSPayment.PatientPaymentsRow drPatientPaymentChargeCopayDebitRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref drPatientPaymentChargeCopayDebitRow, "", "");


            if (insuranceChargeCopayField > 0)
            {
                drPatientPaymentChargeCopayDebitRow.PaidAmountDr = insuranceChargeCopayField;
            }

            if (InsurancePlanId > 0)
            {
                drPatientPaymentChargeCopayDebitRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                drPatientPaymentChargeCopayDebitRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                drPatientPaymentChargeCopayDebitRow.InsurancePlanPriority = InsurancePlanPriority;
            }


            drPatientPaymentChargeCopayDebitRow.NextResponsibility = insuranceNextResponsibilityField;

            //in case of no transfer entry i.e copay,coinsurance,deductible and patient responsiblity is zero and only payment and write off is posting then set next resposible to "Patient" with zero copay entry 
            if (nextResponsibleInCaseofZeroAmt == "Patient")
            {
                insuranceNextResponsibilityField = "patient";
                drPatientPaymentChargeCopayDebitRow.NextResponsibility = "Patient";
            }

            if (insuranceNextResponsibilityField == "insurance")
            {
                drPatientPaymentChargeCopayDebitRow.CrossOver = insuranceCrossOverField;
                if (insuranceNextInsurancePlanIdField > 0)
                {
                    drPatientPaymentChargeCopayDebitRow.NextResponsibilityId = insuranceNextInsurancePlanIdField;
                }
                else
                {
                    throw new Exception("Please Select Next Insurance Plan");

                }

                // copay transfer to insurance
                drPatientPaymentChargeCopayDebitRow.LedgerAccId = 17;// getLedgerAccountId(5, 2, 11, 1);
                drPatientPaymentChargeCopayDebitRow.CrossOver = insuranceCrossOverField;
            }
            else if (insuranceNextResponsibilityField == "patient")
            {

                // copay transfer to Patient
                drPatientPaymentChargeCopayDebitRow.LedgerAccId = 18;// getLedgerAccountId(5, 2, 11, 1);

            }
            drPatientPaymentChargeCopayDebitRow.Copay = insuranceChargeCopayField;
            if (insuranceCommentsField != "")
                drPatientPaymentChargeCopayDebitRow.Comments = insuranceCommentsField;


            drPatientPaymentChargeCopayDebitRow.IsDenied = isDeniedField;
            drPatientPaymentChargeCopayDebitRow.PrintOnPatStmt = insPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(drPatientPaymentChargeCopayDebitRow);
        }

        private void insertInsuranceCoinsuranceRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow drPatientPaymentCoinDebitRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref drPatientPaymentCoinDebitRow, "", "");


            if (insuranceCoinsuranceField > 0)
            {
                drPatientPaymentCoinDebitRow.PaidAmountDr = insuranceCoinsuranceField;
            }

            if (InsurancePlanId > 0)
            {
                drPatientPaymentCoinDebitRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                drPatientPaymentCoinDebitRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                drPatientPaymentCoinDebitRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            drPatientPaymentCoinDebitRow.NextResponsibility = insuranceNextResponsibilityField;
            if (insuranceNextResponsibilityField == "insurance")
            {
                drPatientPaymentCoinDebitRow.CrossOver = insuranceCrossOverField;
                if (insuranceNextInsurancePlanIdField > 0)
                {
                    drPatientPaymentCoinDebitRow.NextResponsibilityId = insuranceNextInsurancePlanIdField;
                }
                else
                {
                    throw new Exception("Please Select Next Insurance Plan");

                }

                //transfer to insurance
                drPatientPaymentCoinDebitRow.LedgerAccId = 14;// getLedgerAccountId(5, 2, 11, 1);
                drPatientPaymentCoinDebitRow.CrossOver = insuranceCrossOverField;
            }
            else if (insuranceNextResponsibilityField == "patient")
            {

                //Transfer to Patient
                drPatientPaymentCoinDebitRow.LedgerAccId = 8;// getLedgerAccountId(5, 2, 11, 1);

            }




            drPatientPaymentCoinDebitRow.Coinsurance = insuranceCoinsuranceField;
            drPatientPaymentCoinDebitRow.PrintOnPatStmt = insPrintOnPatStmtField;
            drPatientPaymentCoinDebitRow.IsDenied = isDeniedField;

            if (insuranceCommentsField != "")
                drPatientPaymentCoinDebitRow.Comments = insuranceCommentsField;

            dsPayment.PatientPayments.AddPatientPaymentsRow(drPatientPaymentCoinDebitRow);
        }

        private void insertInsuranceDeductablesRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow drPatientPaymentDeductableDebitRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref drPatientPaymentDeductableDebitRow, "", "");


            if (insuranceDeductablesField > 0)
            {
                drPatientPaymentDeductableDebitRow.PaidAmountDr = insuranceDeductablesField;
            }

            if (InsurancePlanId > 0)
            {
                drPatientPaymentDeductableDebitRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                drPatientPaymentDeductableDebitRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                drPatientPaymentDeductableDebitRow.InsurancePlanPriority = InsurancePlanPriority;
            }


            drPatientPaymentDeductableDebitRow.NextResponsibility = insuranceNextResponsibilityField;

            if (insuranceNextResponsibilityField == "insurance")
            {
                if (insuranceNextInsurancePlanIdField > 0)
                {
                    drPatientPaymentDeductableDebitRow.NextResponsibilityId = insuranceNextInsurancePlanIdField;
                }
                else
                {
                    throw new Exception("Please Select Next Insurance Plan");

                }


                //transfer to insurance
                drPatientPaymentDeductableDebitRow.LedgerAccId = 15;// getLedgerAccountId(5, 2, 11, 1);
                drPatientPaymentDeductableDebitRow.CrossOver = insuranceCrossOverField;
            }
            else if (insuranceNextResponsibilityField == "patient")
            {

                //Transfer to Patient
                drPatientPaymentDeductableDebitRow.LedgerAccId = 9;// getLedgerAccountId(5, 2, 11, 1);

            }


            drPatientPaymentDeductableDebitRow.IsDenied = isDeniedField;
            drPatientPaymentDeductableDebitRow.Deductables = insuranceDeductablesField;
            drPatientPaymentDeductableDebitRow.PrintOnPatStmt = insPrintOnPatStmtField;

            if (insuranceCommentsField != "")
                drPatientPaymentDeductableDebitRow.Comments = insuranceCommentsField;

            dsPayment.PatientPayments.AddPatientPaymentsRow(drPatientPaymentDeductableDebitRow);
        }

        private void insertInsuranceParientResponsibilityRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow drPatientPaymentPatientResponsibilityDebitRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref drPatientPaymentPatientResponsibilityDebitRow, "", "");


            if (insurancePatientResponsibilityField > 0)
            {
                drPatientPaymentPatientResponsibilityDebitRow.PaidAmountDr = insurancePatientResponsibilityField;
            }

            if (InsurancePlanId > 0)
            {
                drPatientPaymentPatientResponsibilityDebitRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                drPatientPaymentPatientResponsibilityDebitRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                drPatientPaymentPatientResponsibilityDebitRow.InsurancePlanPriority = InsurancePlanPriority;
            }


            drPatientPaymentPatientResponsibilityDebitRow.NextResponsibility = insuranceNextResponsibilityField;

            if (insuranceNextResponsibilityField == "insurance")
            {
                if (insuranceNextInsurancePlanIdField > 0)
                {
                    drPatientPaymentPatientResponsibilityDebitRow.NextResponsibilityId = insuranceNextInsurancePlanIdField;
                }
                else
                {
                    throw new Exception("Please Select Next Insurance Plan");

                }

                drPatientPaymentPatientResponsibilityDebitRow.PrintOnPatStmt = insPrintOnPatStmtField;
                //transfer to insurance
                drPatientPaymentPatientResponsibilityDebitRow.LedgerAccId = 16;// getLedgerAccountId(5, 2, 11, 1);
                drPatientPaymentPatientResponsibilityDebitRow.CrossOver = insuranceCrossOverField;
            }
            else if (insuranceNextResponsibilityField == "patient")
            {
                //Transfer to Patient
                drPatientPaymentPatientResponsibilityDebitRow.LedgerAccId = 10;// getLedgerAccountId(5, 2, 11, 1);
            }

            drPatientPaymentPatientResponsibilityDebitRow.PatientResponsibility = insurancePatientResponsibilityField;

            if (insuranceCommentsField != "")
                drPatientPaymentPatientResponsibilityDebitRow.Comments = insuranceCommentsField;


            drPatientPaymentPatientResponsibilityDebitRow.IsDenied = isDeniedField;
            drPatientPaymentPatientResponsibilityDebitRow.PrintOnPatStmt = insPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(drPatientPaymentPatientResponsibilityDebitRow);
        }

        #endregion

        #region PATIENT ROWS FUNCTIONS


        private void insertPatientPaidRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow patientPaidRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref patientPaidRow, "", "");

            if (paymentModeField == 4)
            {
                //Advance Payment

                if (patientAdvancePaymentIdField > 0)
                {

                    patientPaidRow.AdvPmtId = patientAdvancePaymentIdField;
                }
                else
                {

                    //ERROR:: PLEASE SELECT ADVANCE PAYMENT
                }

            }

            if (patientPaidField > 0)
                patientPaidRow.PaidAmountDr = patientPaidField;

            if (patientPaidAccountIdField > 0)
                patientPaidRow.LedgerAccId = patientPaidAccountIdField;

            if (patientCommentsField != "")
            {
                patientPaidRow.Comments = patientCommentsField;
            }

            if (InsurancePlanId > 0)
            {
                patientPaidRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                patientPaidRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                patientPaidRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            patientPaidRow.IsDenied = isDeniedField;
            patientPaidRow.PrintOnPatStmt = patPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(patientPaidRow);


        }


        private void insertPatientDiscountRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow patientDiscountRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref patientDiscountRow, "", "");

            if (paymentModeField == 4)
            {
                //Advance Payment

                if (patientAdvancePaymentIdField > 0)
                {

                    //  patientDiscountRow.AdvPmtId = patientAdvancePaymentIdField;
                }
                else
                {

                    //ERROR:: PLEASE SELECT ADVANCE PAYMENT
                }

            }

            if (patientDiscountField > 0)
                patientDiscountRow.PaidAmountDr = patientDiscountField;

            if (patientDiscountAccountIdField > 0)
                patientDiscountRow.LedgerAccId = patientDiscountAccountIdField;

            if (patientCommentsField != "")
            {
                patientDiscountRow.Comments = patientCommentsField;
            }

            if (InsurancePlanId > 0)
            {
                patientDiscountRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                patientDiscountRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                patientDiscountRow.InsurancePlanPriority = InsurancePlanPriority;
            }


            patientDiscountRow.IsDenied = isDeniedField;
            patientDiscountRow.PrintOnPatStmt = patPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(patientDiscountRow);


        }


        private void insertPatientCoinsuranceRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow patientCoinsuranceRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref patientCoinsuranceRow, "", "");


            if (patientCoinsuranceField > 0)
            {
                patientCoinsuranceRow.PaidAmountDr = patientCoinsuranceField;
            }

            patientCoinsuranceRow.NextResponsibility = patientNextResponsibilityField;
            if (patientNextResponsibilityField == "insurance")
            {

                //transfer to insurance
                patientCoinsuranceRow.LedgerAccId = 11;// getLedgerAccountId(5, 2, 11, 1);
            }

            if (InsurancePlanId > 0)
            {
                patientCoinsuranceRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                patientCoinsuranceRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                patientCoinsuranceRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            patientCoinsuranceRow.IsDenied = isDeniedField;
            patientCoinsuranceRow.PrintOnPatStmt = patPrintOnPatStmtField;
            patientCoinsuranceRow.Coinsurance = patientCoinsuranceField;

            if (insuranceCommentsField != "")
                patientCoinsuranceRow.Comments = patientCommentsField;

            dsPayment.PatientPayments.AddPatientPaymentsRow(patientCoinsuranceRow);


        }


        private void insertPatientDeductablesRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow patientDeductablesRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref patientDeductablesRow, "", "");


            if (patientDeductablesField > 0)
            {
                patientDeductablesRow.PaidAmountDr = patientDeductablesField;
            }

            patientDeductablesRow.NextResponsibility = patientNextResponsibilityField;
            if (patientNextResponsibilityField == "insurance")
            {

                //transfer to insurance
                patientDeductablesRow.LedgerAccId = 12;// getLedgerAccountId(5, 2, 11, 1);
            }

            if (InsurancePlanId > 0)
            {
                patientDeductablesRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                patientDeductablesRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                patientDeductablesRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            patientDeductablesRow.IsDenied = isDeniedField;
            patientDeductablesRow.PrintOnPatStmt = patPrintOnPatStmtField;
            patientDeductablesRow.Deductables = patientDeductablesField;

            if (insuranceCommentsField != "")
                patientDeductablesRow.Comments = patientCommentsField;

            dsPayment.PatientPayments.AddPatientPaymentsRow(patientDeductablesRow);


        }

        private void insertPatientPatientResponsibilityRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow patientPatientResponsibilityRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref patientPatientResponsibilityRow, "", "");


            if (patientPatientResponsibilityField > 0)
            {
                patientPatientResponsibilityRow.PaidAmountDr = patientPatientResponsibilityField;
            }

            patientPatientResponsibilityRow.NextResponsibility = patientNextResponsibilityField;
            if (patientNextResponsibilityField == "insurance")
            {

                //transfer to insurance
                patientPatientResponsibilityRow.LedgerAccId = 13;// getLedgerAccountId(5, 2, 11, 1);
            }

            if (InsurancePlanId > 0)
            {
                patientPatientResponsibilityRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                patientPatientResponsibilityRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                patientPatientResponsibilityRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            patientPatientResponsibilityRow.IsDenied = isDeniedField;
            patientPatientResponsibilityRow.PrintOnPatStmt = patPrintOnPatStmtField;
            patientPatientResponsibilityRow.PatientResponsibility = patientPatientResponsibilityField;

            if (insuranceCommentsField != "")
                patientPatientResponsibilityRow.Comments = patientCommentsField;

            dsPayment.PatientPayments.AddPatientPaymentsRow(patientPatientResponsibilityRow);


        }


        #endregion

        #region COPAYMENT ROWS FUNCTIONS
        private void insertCopayTransferRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow copayTransferRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref copayTransferRow, "", "");

            if (CopaymentTransferField > 0)
                copayTransferRow.PaidAmountDr = CopaymentTransferField;

            copayTransferRow.NextResponsibility = CopaymentNextResponsibilityField;

            if (CopaymentNextResponsibilityField == "insurance")
            {
                //???
                copayTransferRow.LedgerAccId = 19;
            }

            copayTransferRow.Copay = CopaymentTransferField;

            if (copaymentCommentsField != "")
            {
                copayTransferRow.Comments = copaymentCommentsField;
            }


            if (InsurancePlanId > 0)
            {
                copayTransferRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                copayTransferRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                copayTransferRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            copayTransferRow.IsDenied = isDeniedField;
            copayTransferRow.PrintOnPatStmt = copaymentPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(copayTransferRow);

        }


        //private void insertCopayPaidRow(DSPayment dsPayment)
        //{

        //    DSPayment.PatientPaymentsRow copayPaidRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

        //    fillRow(ref copayPaidRow, "", "");



        //    if (paymentModeField == 4)
        //    {
        //        //Advance Payment

        //        if (patientAdvancePaymentIdField > 0)
        //        {

        //            copayPaidRow.AdvPmtId = patientAdvancePaymentIdField;
        //        }
        //        else
        //        {

        //            //ERROR:: PLEASE SELECT ADVANCE PAYMENT
        //        }

        //    }



        //    if (copaymentPaidField > 0)
        //        copayPaidRow.PaidAmountDr = copaymentPaidField;

        //    if (copaymentPaidAccountIdField > 0)
        //        copayPaidRow.LedgerAccId = copaymentPaidAccountIdField;

        //    if (copaymentCommentsField != "")
        //    {
        //        copayPaidRow.Comments = copaymentCommentsField;
        //    }

        //    if (InsurancePlanId > 0)
        //    {
        //        copayPaidRow.InsurancePlanId = InsurancePlanId;
        //    }

        //    if (PatientInsurancePlanId > 0)
        //    {
        //        copayPaidRow.PatientInsurancePlanId = PatientInsurancePlanId;
        //    }

        //    if (InsurancePlanPriority > 0)
        //    {
        //        copayPaidRow.InsurancePlanPriority = InsurancePlanPriority;
        //    }


        //    copayPaidRow.PrintOnPatStmt = copaymentPrintOnPatStmtField;
        //    dsPayment.PatientPayments.AddPatientPaymentsRow(copayPaidRow);


        //}

        //private void insertCopayDiscountRow(DSPayment dsPayment)
        //{

        //    DSPayment.PatientPaymentsRow copayDiscountRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

        //    fillRow(ref copayDiscountRow, "", "");


        //    if (copaymentDiscountField > 0)
        //        copayDiscountRow.PaidAmountDr = copaymentDiscountField;

        //    if (copaymentDiscountAccountIdField > 0)
        //        copayDiscountRow.LedgerAccId = copaymentDiscountAccountIdField;

        //    if (copaymentCommentsField != "")
        //    {
        //        copayDiscountRow.Comments = copaymentCommentsField;
        //    }

        //    if (InsurancePlanId > 0)
        //    {
        //        copayDiscountRow.InsurancePlanId = InsurancePlanId;
        //    }

        //    if (PatientInsurancePlanId > 0)
        //    {
        //        copayDiscountRow.PatientInsurancePlanId = PatientInsurancePlanId;
        //    }

        //    if (InsurancePlanPriority > 0)
        //    {
        //        copayDiscountRow.InsurancePlanPriority = InsurancePlanPriority;
        //    }

        //    copayDiscountRow.PrintOnPatStmt = copaymentPrintOnPatStmtField;
        //    dsPayment.PatientPayments.AddPatientPaymentsRow(copayDiscountRow);


        //}




        #endregion


        #region ZERO PAYMENT ROWS FUNCTION
        private void insertZeroPaymentRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow insuranceZeroPaymentRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref insuranceZeroPaymentRow, "", "");

            //if (insuranceAllowedField > 0 && !isDeniedField)
            //{
            //    insuranceZeroPaymentRow.Allowed = insuranceAllowedField;
            //}

            //if (insurancePaidField > 0 && !isDeniedField)
            //{
            insuranceZeroPaymentRow.PaidAmountDr = 0;
            //}
            //else if (isDeniedField)
            //{
            //    insuranceZeroPaymentRow.PaidAmountDr = 0;
            //}

            //if (insurancePaidAccountIdField > 0)
            //{
            insuranceZeroPaymentRow.LedgerAccId = 1;
            //}
            //else
            //{
            //    throw new Exception("Please select Paid A/C");
            //}

            //if (insuranceRemitCodeField > 0)
            //{
            //    insuranceZeroPaymentRow.RemitCodeId = insuranceRemitCodeField;
            //}

            //if (insuranceIcnDcnField != "")
            //{
            //    insuranceZeroPaymentRow.ICNDCN = insuranceIcnDcnField;
            //}
            //if (insuranceChargeCopayField > 0)
            //{
            //    insurancePaidRow.Copay = insuranceChargeCopayField;
            //}

            if (zeroPaymentCommentsField != "")
            {
                insuranceZeroPaymentRow.Comments = zeroPaymentCommentsField;
            }

            if (InsurancePlanId > 0)
            {
                insuranceZeroPaymentRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                insuranceZeroPaymentRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                insuranceZeroPaymentRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            insuranceZeroPaymentRow.IsDenied = isDeniedField;

            insuranceZeroPaymentRow.PrintOnPatStmt = zeroPaymentPrintOnPatStmtField;


            dsPayment.PatientPayments.AddPatientPaymentsRow(insuranceZeroPaymentRow);


        }
        #endregion
        #region ACCOUNT LEDGER
        private string FillPatientPayments(Int64 PaymentId, Int64 AppointmentId, Int64 VisitId, Int64 ChargeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PaymentId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AppointmentId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPayment dsPatientPayment = null;
                    BLObject<DSPayment> obj = BLLBillingObj.LoadPatientPayments(PaymentId, AppointmentId, VisitId, ChargeId, "FillBillLedger");
                    if (obj.Data != null)
                    {
                        dsPatientPayment = obj.Data;

                        if (dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows.Count > 0)
                        {
                            //  checkNumberField != "" ? " Under the check Number " + checkNumberField + "| " : ""
                            DataRow dr = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[0];

                            //start syed Zia 29-01-2016, bug #PMS-3729
                            var keyValues = new Dictionary<string, string>
                            {
                                { "txtPaymentNum", "PMT"+MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PaymentIdColumn.ColumnName])},
                                { "dtpCheckDate", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CheckDateColumn.ColumnName])},
                                { "dtpPaymentDate", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PaymentDateColumn.ColumnName])},
                                { "txtAmount", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PaidAmountDrColumn.ColumnName])},
                                {"ddlPaymentType", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PmtTypeIdColumn.ColumnName])},
                                //{ "txtCheckNumber", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CheckNoColumn.ColumnName])},
                                //{ "txtCardNumber", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CheckNoColumn.ColumnName])},
                                { "txtAccount", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.LedgerAccountNameColumn.ColumnName])},
                                { "EntryDate", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CreatedOnColumn.ColumnName])},
                                { "txtEnteredBy", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CreatedByColumn.ColumnName])},
                                { "txtInsuranceComments", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CommentsColumn.ColumnName])},
                                { "chkShwPatientStmt", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PrintOnPatStmtColumn.ColumnName])},
                                { "chkCrossOver", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CrossOverColumn.ColumnName])},
                                { "ddlCreditCardType", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CardTypeIdColumn.ColumnName])},
                                { "dtpExpiryDate", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.ExpiryDateColumn.ColumnName])},
                                { "txtICN_DCN", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.ICNDCNColumn.ColumnName])},
                                { "ddlRemitCode", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.RemitCodeIdColumn.ColumnName])},

                                //start syed zia 12-02-2016, bug #PMS-3927
                                { "txtPaymentBatch", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PmtBatchNumberColumn.ColumnName])},
                               
                                //end syed zia 12-02-2016, bug #PMS-3927
                                
                            };


                            if (MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PmtTypeNameColumn.ColumnName]).Trim() == "Credit Card")
                            {
                                keyValues.Add("txtCardNumber", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CheckNoColumn.ColumnName]));
                            }
                            else
                            {
                                keyValues.Add("txtCheckNumber", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CheckNoColumn.ColumnName]));
                            }

                            //end syed Zia 29-01-2016, bug #PMS-3729

                            //DataRow dr = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[0];

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientPayments_JSON = js.Serialize(keyValues)
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string UpdateLedger(Int64 PaymentId, string Comment, string ChkPrintonPatStmt, Int64 AppointmentId, Int64 VisitId, Int64 ChargeId, Int32 PaymentTypeId, Int32 CreditCardTypeId, string CraditCardExpiryDate, string CheckDate, string checkNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PaymentId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AppointmentId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {

                    // call loadPayment
                    DSPayment dsPatientPayment = new DSPayment();
                    
                    BLObject<DSPayment> obj = BLLBillingObj.LoadPatientPayments(PaymentId, AppointmentId, VisitId, ChargeId);
                    if (obj.Data != null)
                    {
                        dsPatientPayment = obj.Data;
                        if (dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows.Count > 0)
                        {
                            
                            DataRow dr = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[0];

                            String chkdate = MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CheckDateColumn.ColumnName]);
                            if (!string.IsNullOrEmpty(chkdate)){
                                DateTime checkDate = MDVUtility.ToDateTime(dr[dsPatientPayment.PatientPayments.CheckDateColumn.ColumnName]);
                                dr[dsPatientPayment.PatientPayments.CheckDateColumn.ColumnName] = String.Format("{0:MM/dd/yyyy}", checkDate);
                                dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].AcceptChanges();
                            }
                            
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtPaymentNum", "PMT"+MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PaymentIdColumn.ColumnName])},
                            { "CheckDate", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CheckDateColumn.ColumnName])},
                            { "txtAmount", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PaidAmountDrColumn.ColumnName])},
                            { "ddlPaymentType", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PmtTypeIdColumn.ColumnName])},
                            { "txtCheckNumber", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CheckNoColumn.ColumnName])},
                            { "txtCardNumber", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CardTypeIdColumn.ColumnName])},
                            { "txtAccount", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.LedgerAccountNameColumn.ColumnName])},
                            { "EntryDate", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CreatedOnColumn.ColumnName])},
                            { "txtEnteredBy", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CreatedByColumn.ColumnName])},
                            { "txtInsuranceComments", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CommentsColumn.ColumnName])},
                            { "chkShwPatientStmt", MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.PrintOnPatStmtColumn.ColumnName])},

                        };

                            //DataRow dr = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[0];

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            //var response = new
                            //{
                            //    status = true,
                            //    PatientPayments_JSON = js.Serialize(keyValues)
                            //};
                            //return (JsonConvert.SerializeObject(response));

                            DSPayment.PatientPaymentsRow patientPaidRow = (DSPayment.PatientPaymentsRow)dsPatientPayment.PatientPayments.Rows[0];

                            // prd-19 user name and current date append only one time.
                            string strComments = "";
                            if (!string.IsNullOrEmpty(Comment) && MDVUtility.ToStr(dr[dsPatientPayment.PatientPayments.CommentsColumn.ColumnName]) != Comment)
                            {
                                if (Comment.Contains("|"))
                                {
                                    string onlyComments = Comment.Substring(0, Comment.IndexOf('|'));
                                    strComments = MDVUtility.ToStr(onlyComments) + " |";
                                    strComments += MDVSession.Current.AppUserFullName + " " + DateTime.Now;
                                    patientPaidRow.Comments = strComments;
                                }
                                else{
                                    strComments = MDVUtility.ToStr(Comment) + " |";
                                    strComments += MDVSession.Current.AppUserFullName + " " + DateTime.Now;
                                    patientPaidRow.Comments = strComments;
                                    
                                }
                                
                            }
                            else {
                                patientPaidRow.Comments = Comment;
                            }
                            
                            // prd-19
                            if (checkNumber == "")
                                checkNumber = null;                                                        
                            patientPaidRow.PrintOnPatStmt = ChkPrintonPatStmt.ToLower() == "true" ? true : false;
                            patientPaidRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            patientPaidRow.ModifiedOn = DateTime.Now;
                            patientPaidRow.CardTypeId = CreditCardTypeId;                            
                            patientPaidRow.CheckNo = checkNumber;                            
                            patientPaidRow.PmtTypeId = PaymentTypeId;
                            if (CheckDate == "")
                            {
                                patientPaidRow.CheckDate = null;
                            }
                            else {
                                //DateTime chkdate = MDVUtility.ToDateTime(CheckDate);                                
                                //patientPaidRow.CheckDate = String.Format("{0:yyyy-MM-dd}", chkdate);
                                patientPaidRow.CheckDate = CheckDate;
                            }
                            if (CraditCardExpiryDate == "") {
                                patientPaidRow.ExpiryDate = null;
                            }
                            else {
                                patientPaidRow.ExpiryDate = CraditCardExpiryDate;                                
                            }                                                   
                            patientPaidRow.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                            BLObject<DSPayment> objLedger = BLLBillingObj.UpdateLegder(dsPatientPayment);
                            
                            dsPatientPayment = objLedger.Data;
                            if (objLedger.Data != null)
                            {

                                var response = new
                                {
                                    status = true,
                                    Message = Common.AppPrivileges.Update_Message
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = objLedger.Message
                                };
                                return JsonConvert.SerializeObject(response);
                            }
                        }

                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }




            /**/



        }

        #endregion


        #region RECOUPMENT ROWS FUNCTIONS
        private void insertRecoupmentPaidRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow recoupmentPaidRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref recoupmentPaidRow, "", "");

            if (recoupmentAllowedField > 0 && !isDeniedField)
            {
                recoupmentPaidRow.Allowed = recoupmentAllowedField;
            }

            if (recoupmentPaidField > 0 && !isDeniedField)
            {
                recoupmentPaidRow.PaidAmountCr = recoupmentPaidField;
            }
            else if (isDeniedField)
            {
                recoupmentPaidRow.PaidAmountDr = 0;
            }

            if (recoupmentPaidAccountIdField > 0)
            {
                recoupmentPaidRow.LedgerAccId = recoupmentPaidAccountIdField;
            }
            else
            {
                throw new Exception("Please select Paid A/C");
            }

            if (recoupmentRemitCodeField > 0)
            {
                recoupmentPaidRow.RemitCodeId = recoupmentRemitCodeField;
            }

            if (recoupmentIcnDcnField != "")
            {
                recoupmentPaidRow.ICNDCN = recoupmentIcnDcnField;
            }
            //if (insuranceChargeCopayField > 0)
            //{
            //    insurancePaidRow.Copay = insuranceChargeCopayField;
            //}

            if (recoupmentCommentsField != "")
            {
                recoupmentPaidRow.Comments = recoupmentCommentsField;
            }


            if (InsurancePlanId > 0)
            {
                recoupmentPaidRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                recoupmentPaidRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                recoupmentPaidRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            //recoupmentPaidRow.IsDenied = isDeniedField;
            recoupmentPaidRow.IsDenied = isDeniedField;
            recoupmentPaidRow.IsRecoupment = true;
            recoupmentPaidRow.PrintOnPatStmt = recoupmentPrintOnPatStmtField;

            dsPayment.PatientPayments.AddPatientPaymentsRow(recoupmentPaidRow);


        }

        private void insertRecoupmentWriteoffRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow recoupmentWiteoffRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref recoupmentWiteoffRow, "", "");


            if (recoupmentAllowedField > 0)
            {
                recoupmentWiteoffRow.Allowed = recoupmentAllowedField;
            }

            if (recoupmentWriteoffField > 0)
                recoupmentWiteoffRow.PaidAmountCr = recoupmentWriteoffField;

            if (recoupmentWriteoffAccountIdField > 0)
                recoupmentWiteoffRow.LedgerAccId = recoupmentWriteoffAccountIdField;

            if (recoupmentRemitCodeField > 0)
            {
                recoupmentWiteoffRow.RemitCodeId = recoupmentRemitCodeField;
            }

            if (recoupmentIcnDcnField != "")
            {
                recoupmentWiteoffRow.ICNDCN = recoupmentIcnDcnField;
            }
            //if (recoupmentChargeCopayField > 0)
            //{
            //    recoupmentWiteoffRow.Copay = recoupmentChargeCopayField;
            //}

            if (recoupmentCommentsField != "")
            {
                recoupmentWiteoffRow.Comments = recoupmentCommentsField;
            }

            if (InsurancePlanId > 0)
            {
                recoupmentWiteoffRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                recoupmentWiteoffRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                recoupmentWiteoffRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            recoupmentWiteoffRow.IsDenied = isDeniedField;
            recoupmentWiteoffRow.IsRecoupment = true;
            recoupmentWiteoffRow.PrintOnPatStmt = recoupmentPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(recoupmentWiteoffRow);

        }

        private void insertRecoupmentChargeCopayRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow drPatientPaymentChargeCopayDebitRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref drPatientPaymentChargeCopayDebitRow, "", "");


            if (recoupmentChargeCopayField > 0)
            {
                drPatientPaymentChargeCopayDebitRow.PaidAmountCr = recoupmentChargeCopayField;
            }


            drPatientPaymentChargeCopayDebitRow.NextResponsibility = recoupmentNextResponsibilityField;
            if (recoupmentNextResponsibilityField == "insurance")
            {
                ////drPatientPaymentChargeCopayDebitRow.CrossOver = recoupmentCrossOverField;
                if (insuranceNextInsurancePlanIdField > 0)
                {
                    drPatientPaymentChargeCopayDebitRow.NextResponsibilityId = insuranceNextInsurancePlanIdField;
                }
                //else
                //{
                //    throw new Exception("Please Select Next Insurance Plan");

                //}

                // copay transfer to insurance
                drPatientPaymentChargeCopayDebitRow.LedgerAccId = 17;// getLedgerAccountId(5, 2, 11, 1);
                //drPatientPaymentChargeCopayDebitRow.CrossOver = recoupmentCrossOverField;
            }
            else if (recoupmentNextResponsibilityField == "patient")
            {

                // copay transfer to Patient
                drPatientPaymentChargeCopayDebitRow.LedgerAccId = 18;// getLedgerAccountId(5, 2, 11, 1);

            }

            if (InsurancePlanId > 0)
            {
                drPatientPaymentChargeCopayDebitRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                drPatientPaymentChargeCopayDebitRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                drPatientPaymentChargeCopayDebitRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            drPatientPaymentChargeCopayDebitRow.Copay = recoupmentChargeCopayField;
            if (recoupmentCommentsField != "")
                drPatientPaymentChargeCopayDebitRow.Comments = recoupmentCommentsField;

            drPatientPaymentChargeCopayDebitRow.IsDenied = isDeniedField;
            drPatientPaymentChargeCopayDebitRow.IsRecoupment = true;
            drPatientPaymentChargeCopayDebitRow.PrintOnPatStmt = recoupmentPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(drPatientPaymentChargeCopayDebitRow);
        }

        private void insertRecoupmentCoinsuranceRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow drPatientPaymentCoinDebitRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref drPatientPaymentCoinDebitRow, "", "");


            if (recoupmentCoinsuranceField > 0)
            {
                drPatientPaymentCoinDebitRow.PaidAmountCr = recoupmentCoinsuranceField;
            }


            drPatientPaymentCoinDebitRow.NextResponsibility = recoupmentNextResponsibilityField;
            if (recoupmentNextResponsibilityField == "insurance")
            {
                //drPatientPaymentCoinDebitRow.CrossOver = recoupmentCrossOverField;
                if (insuranceNextInsurancePlanIdField > 0)
                {
                    drPatientPaymentCoinDebitRow.NextResponsibilityId = insuranceNextInsurancePlanIdField;
                }
                //else
                //{
                //    throw new Exception("Please Select Next Insurance Plan");

                //}

                //transfer to insurance
                drPatientPaymentCoinDebitRow.LedgerAccId = 14;// getLedgerAccountId(5, 2, 11, 1);
                // drPatientPaymentCoinDebitRow.CrossOver = recoupmentCrossOverField;
            }
            else if (recoupmentNextResponsibilityField == "patient")
            {

                //Transfer to Patient
                drPatientPaymentCoinDebitRow.LedgerAccId = 8;// getLedgerAccountId(5, 2, 11, 1);

            }


            if (InsurancePlanId > 0)
            {
                drPatientPaymentCoinDebitRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                drPatientPaymentCoinDebitRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                drPatientPaymentCoinDebitRow.InsurancePlanPriority = InsurancePlanPriority;
            }


            drPatientPaymentCoinDebitRow.Coinsurance = recoupmentCoinsuranceField;

            if (recoupmentCommentsField != "")
                drPatientPaymentCoinDebitRow.Comments = recoupmentCommentsField;

            drPatientPaymentCoinDebitRow.IsDenied = isDeniedField;
            drPatientPaymentCoinDebitRow.IsRecoupment = true;
            drPatientPaymentCoinDebitRow.PrintOnPatStmt = recoupmentPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(drPatientPaymentCoinDebitRow);
        }

        private void insertRecoupmentDeductablesRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow drPatientPaymentDeductableDebitRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref drPatientPaymentDeductableDebitRow, "", "");


            if (recoupmentDeductablesField > 0)
            {
                drPatientPaymentDeductableDebitRow.PaidAmountCr = recoupmentDeductablesField;
            }

            if (InsurancePlanId > 0)
            {
                drPatientPaymentDeductableDebitRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                drPatientPaymentDeductableDebitRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                drPatientPaymentDeductableDebitRow.InsurancePlanPriority = InsurancePlanPriority;
            }


            drPatientPaymentDeductableDebitRow.NextResponsibility = recoupmentNextResponsibilityField;

            if (recoupmentNextResponsibilityField == "insurance")
            {
                if (insuranceNextInsurancePlanIdField > 0)
                {
                    drPatientPaymentDeductableDebitRow.NextResponsibilityId = insuranceNextInsurancePlanIdField;
                }
                //else
                //{
                //    throw new Exception("Please Select Next Insurance Plan");

                //}


                //transfer to insurance
                drPatientPaymentDeductableDebitRow.LedgerAccId = 15;// getLedgerAccountId(5, 2, 11, 1);
                //drPatientPaymentDeductableDebitRow.CrossOver = recoupmentCrossOverField;
            }
            else if (recoupmentNextResponsibilityField == "patient")
            {

                //Transfer to Patient
                drPatientPaymentDeductableDebitRow.LedgerAccId = 9;// getLedgerAccountId(5, 2, 11, 1);

            }



            drPatientPaymentDeductableDebitRow.Deductables = recoupmentDeductablesField;

            if (recoupmentCommentsField != "")
                drPatientPaymentDeductableDebitRow.Comments = recoupmentCommentsField;


            drPatientPaymentDeductableDebitRow.IsDenied = isDeniedField;
            drPatientPaymentDeductableDebitRow.IsRecoupment = true;
            drPatientPaymentDeductableDebitRow.PrintOnPatStmt = recoupmentPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(drPatientPaymentDeductableDebitRow);
        }

        private void insertRecoupmentParientResponsibilityRow(DSPayment dsPayment)
        {

            DSPayment.PatientPaymentsRow drPatientPaymentPatientResponsibilityDebitRow = dsPayment.PatientPayments.NewPatientPaymentsRow();

            fillRow(ref drPatientPaymentPatientResponsibilityDebitRow, "", "");


            if (recoupmentPatientResponsibilityField > 0)
            {
                drPatientPaymentPatientResponsibilityDebitRow.PaidAmountCr = recoupmentPatientResponsibilityField;
            }


            drPatientPaymentPatientResponsibilityDebitRow.NextResponsibility = recoupmentNextResponsibilityField;

            if (recoupmentNextResponsibilityField == "insurance")
            {
                if (insuranceNextInsurancePlanIdField > 0)
                {
                    drPatientPaymentPatientResponsibilityDebitRow.NextResponsibilityId = insuranceNextInsurancePlanIdField;
                }
                //else
                //{
                //    throw new Exception("Please Select Next Insurance Plan");

                //}


                //transfer to insurance
                drPatientPaymentPatientResponsibilityDebitRow.LedgerAccId = 16;// getLedgerAccountId(5, 2, 11, 1);
                //drPatientPaymentPatientResponsibilityDebitRow.CrossOver = recoupmentCrossOverField;
            }
            else if (recoupmentNextResponsibilityField == "patient")
            {
                //Transfer to Patient
                drPatientPaymentPatientResponsibilityDebitRow.LedgerAccId = 10;// getLedgerAccountId(5, 2, 11, 1);
            }

            drPatientPaymentPatientResponsibilityDebitRow.PatientResponsibility = recoupmentPatientResponsibilityField;

            if (recoupmentCommentsField != "")
                drPatientPaymentPatientResponsibilityDebitRow.Comments = recoupmentCommentsField;

            if (InsurancePlanId > 0)
            {
                drPatientPaymentPatientResponsibilityDebitRow.InsurancePlanId = InsurancePlanId;
            }

            if (PatientInsurancePlanId > 0)
            {
                drPatientPaymentPatientResponsibilityDebitRow.PatientInsurancePlanId = PatientInsurancePlanId;
            }

            if (InsurancePlanPriority > 0)
            {
                drPatientPaymentPatientResponsibilityDebitRow.InsurancePlanPriority = InsurancePlanPriority;
            }

            drPatientPaymentPatientResponsibilityDebitRow.IsDenied = isDeniedField;
            drPatientPaymentPatientResponsibilityDebitRow.IsRecoupment = true;
            drPatientPaymentPatientResponsibilityDebitRow.PrintOnPatStmt = recoupmentPrintOnPatStmtField;
            dsPayment.PatientPayments.AddPatientPaymentsRow(drPatientPaymentPatientResponsibilityDebitRow);
        }

        #endregion



        private string IsCheckAlreadyPosted(string checkNo, Int64 chargeId = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(checkNo)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr("check No. is empty !!")
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.IsCheckAlreadyPosted(checkNo, chargeId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = ""
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string IsCheckAlreadyPosted(PaymentPostingModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.checkNumber)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr("check No. is empty !!")
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.IsCheckAlreadyPosted(MDVUtility.ToStr(model.checkNumber), MDVUtility.ToInt64(model.ChargeId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = ""
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
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

                case "REFUND_PATIENT_PAYMENT":
                    {
                        //string fieldsJSON = context.Request["AdvancePaymentData"];
                        Int64 PaymentID = MDVUtility.ToInt64(context.Request["PaymentID"]);
                        string strJSONData = RefundPayment(PaymentID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                case "SEARCH_CHARGE":
                    {
                        string fieldsJSON = context.Request["ChargeData"];
                        Int64 VisitId = 0;
                        if (context.Request["VisitId"] != null)
                        {
                            VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        }
                        Int64 ChargeId = 0;
                        if (context.Request["ChargeId"] != null)
                        {
                            ChargeId = MDVUtility.ToInt64(context.Request["ChargeId"]);
                        }

                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        string strJSONData = SearchCharge(fieldsJSON, ChargeId, VisitId, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PAT_RES_PAYMENT":
                    {
                        string PaymentData = context.Request["PaymentData"];
                        string ChargeData = context.Request["ChargeData"];
                        string PmtBatchId = context.Request["PmtBatchId"];

                        string strJSONData = SavePatientResponsibilityPayment(PaymentData, ChargeData, PmtBatchId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_BILL_LEDGER":
                    {
                        Int64 PaymentId = MDVUtility.ToInt64(context.Request["PaymentID"]);
                        Int64 AppointmentId = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        Int64 ChargeId = 0;
                        if (context.Request["ChargeID"] != null)
                        {
                            ChargeId = MDVUtility.ToInt64(context.Request["ChargeID"]);
                        }
                        string strJSONData = FillPatientPayments(PaymentId, AppointmentId, VisitId, ChargeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BILL_LEDGER":
                    {
                        Int64 PaymentId = MDVUtility.ToInt64(context.Request["PaymentID"]);
                        string Comment = context.Request["Comment"];
                        string ChkPrintonPatStmt = MDVUtility.ToStr(context.Request["ChkPrintonPatStmt"]);
                        Int64 AppointmentId = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);                
                        Int32 PaymentTypeId = MDVUtility.ToInt32(context.Request["PaymentTypeId"]);
                        Int32 CreditCardTypeId = MDVUtility.ToInt32(context.Request["CreditCardTypeId"]);
                        //string CardNumber = context.Request["CardNumber"];
                        string CraditCardExpiryDate = context.Request["CraditCardExpiryDate"];
                        string CheckDate = context.Request["CheckDate"];
                        string checkNumber = context.Request["checkNumber"];
                        Int64 ChargeId = 0;
                        if (context.Request["ChargeID"] != null)
                        {
                            ChargeId = MDVUtility.ToInt64(context.Request["ChargeID"]);
                        }
                        string strJSONData = UpdateLedger(PaymentId, Comment, ChkPrintonPatStmt, AppointmentId, VisitId, ChargeId, PaymentTypeId,CreditCardTypeId,CraditCardExpiryDate, CheckDate, checkNumber);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "IS_CHECK_POSTED":
                    {
                        string CheckNo = MDVUtility.ToStr(context.Request["CheckNo"]);
                        Int64 ChargeId = MDVUtility.ToInt64(context.Request["ChargeID"]);

                        if (context.Request["ChargeID"] != null)
                        {
                            ChargeId = MDVUtility.ToInt64(context.Request["ChargeID"]);
                        }
                        string strJSONData = IsCheckAlreadyPosted(CheckNo, ChargeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion

    }
}