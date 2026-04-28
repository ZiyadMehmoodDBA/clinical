using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Controls.Patient.Demographics;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Model.Billing.Payments;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_Copayment
    {
        private BLLBilling BLLBillingObj = null;
        public Scheduling_Copayment() {
            BLLBillingObj = new BLLBilling();
        }

        #region Singleton
        private static Scheduling_Copayment _obj = null;
        public static Scheduling_Copayment Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_Copayment();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string SavePatientCoPayment(string fieldsJSON, long PatientVisitId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPayment dsPayment = new DSPayment();
                DSPayment.PatientPaymentsRow dr = dsPayment.PatientPayments.NewPatientPaymentsRow();

                //DSPayment dsPayment1 = new DSPayment();
                //DSPayment.PatientPaymentsRow dr1 = dsPayment1.PatientPayments.NewPatientPaymentsRow();

                //--------------------------------------/For First Entry/-------------------//----------------------

                if (SearchedfieldsJSON["txtPaid"] != "")
                {
                    dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfAppointmentId"]))
                        dr.AppointmentId = MDVUtility.ToInt64(SearchedfieldsJSON["hfAppointmentId"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProviderId"]))
                    {
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProviderId"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacilityId"]))
                    {
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacilityId"]);
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPaidLedger"]))
                    {
                        dr.LedgerAccId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPaidLedger"]);
                    }
                    dr.PaidAmountDr = MDVUtility.ToDouble(SearchedfieldsJSON["txtPaid"]);

                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (SearchedfieldsJSON.ContainsKey("txtComments") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                    {
                        dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                    }
                    


                    string time = DateTime.Now.ToString("HH:mm:ss tt");

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dpDatePaid"])))
                        dr.PaymentDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dpDatePaid"] + " " + time);


                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPaymentType"]))
                    {
                        dr.PmtTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPaymentType"]);
                    }

                    if (dr.PmtTypeId == 2)
                    {
                        //check

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCheckNumber"])))
                            dr.CheckNo = SearchedfieldsJSON["txtCheckNumber"];

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dpCheckDate"])))
                            dr.CheckDate = SearchedfieldsJSON["dpCheckDate"];

                    }
                    else if (dr.PmtTypeId == 3)
                    {
                        //credit card

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCardNumber"])))
                            dr.CheckNo = SearchedfieldsJSON["txtCardNumber"];

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dpExpiryDate"])))
                            dr.ExpiryDate = MDVUtility.ToStr(SearchedfieldsJSON["dpExpiryDate"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCardType"]))
                        {
                            dr.CardTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlCardType"]);
                        }
                    }

                    else if (dr.PmtTypeId == 4)
                    {
                        //Advance Payment
                        long advancePaymentId = 0;
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["hfAdvancePaymentId"])))
                        {
                            dr.AdvPmtId = MDVUtility.ToInt64(SearchedfieldsJSON["hfAdvancePaymentId"]);
                            advancePaymentId = dr.AdvPmtId;
                        }

                        Patient_AdvancePayment.Instance().validateAdvancePayment(dr.PatientId, dr.PaidAmountDr, advancePaymentId);

                    }

                    if (PatientVisitId != 0)
                        dr.VisitId = MDVUtility.ToLong(PatientVisitId);

                    dsPayment.PatientPayments.AddPatientPaymentsRow(dr);
                }
                //--------------------------------------/For Second Entry/-------------------//----------------------

                if (SearchedfieldsJSON["txtDiscount"] != "")
                {
                    dr = dsPayment.PatientPayments.NewPatientPaymentsRow();
                    dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);
                    dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProviderId"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacilityId"]))
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacilityId"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDiscountLedger"]))
                    {
                        dr.LedgerAccId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlDiscountLedger"]);
                    }
                    dr.PaidAmountDr = MDVUtility.ToDouble(SearchedfieldsJSON["txtDiscount"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfAppointmentId"]))
                        dr.AppointmentId = MDVUtility.ToInt64(SearchedfieldsJSON["hfAppointmentId"]);

                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (SearchedfieldsJSON.ContainsKey("txtComments") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                    {
                        dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                    }
                    




                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dpDatePaid"])))
                        dr.PaymentDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dpDatePaid"]);



                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPaymentType"]))
                    {
                        dr.PmtTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPaymentType"]);
                    }


                    if (dr.PmtTypeId == 2)
                    {
                        //check

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCheckNumber"])))
                            dr.CheckNo = SearchedfieldsJSON["txtCheckNumber"];

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dpCheckDate"])))
                            dr.CheckDate = SearchedfieldsJSON["dpCheckDate"];

                    }
                    else if (dr.PmtTypeId == 3)
                    {
                        //credit card

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCardNumber"])))
                            dr.CheckNo = SearchedfieldsJSON["txtCardNumber"];

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dpExpiryDate"])))
                            dr.ExpiryDate = MDVUtility.ToStr(SearchedfieldsJSON["dpExpiryDate"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCardType"]))
                        {
                            dr.CardTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlCardType"]);
                        }
                    }

                    else if (dr.PmtTypeId == 4)
                    {
                        //Advance Payment
                        // if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["hfAdvancePaymentId"])))
                        //   dr.AdvPmtId = MDVUtility.ToInt64(SearchedfieldsJSON["hfAdvancePaymentId"]);

                        // Patient_AdvancePayment.Instance().validateAdvancePayment(dr.PatientId, dr.PaidAmountDr, dr.AdvPmtId);

                    }

                    /***
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCheckNumber"])))
                        dr.CheckNo = SearchedfieldsJSON["txtCheckNumber"];
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCardNumber"])))
                        dr.CheckNo = SearchedfieldsJSON["txtCardNumber"];
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dpCheckDate"])))
                        dr.CheckDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dpCheckDate"]);
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dpExpiryDate"])))
                        dr.ExpiryDate = MDVUtility.ToStr(SearchedfieldsJSON["dpExpiryDate"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCardType"]))
                    {
                        dr.CardTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlCardType"]);
                    }
                    */
                    if (PatientVisitId != 0)
                        dr.VisitId = MDVUtility.ToLong(PatientVisitId);

                    dsPayment.PatientPayments.AddPatientPaymentsRow(dr);
                }

                //--------------------------------------//-------------------//----------------------
                #region Database Insertion

                //BLObject<DSPayment> obj = Payment.BusinessObj.InsertPatientPayments(dsPayment);


                //if (check == true)
                //{
                //    dsPayment.PatientPayments.AddPatientPaymentsRow(dr1);
                //    //BLObject<DSPayment> obj1 = BLLBillingObj.InsertPatientPayments(dsPayment1);
                //}


                BLObject<DSPayment> obj = BLLBillingObj.InsertPatientPayments(dsPayment);

                dsPayment = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        PaymentId = dsPayment.Tables[dsPayment.PatientPayments.TableName].Rows[0][dsPayment.PatientPayments.PaymentIdColumn.ColumnName],
                        AppointmentId = dsPayment.Tables[dsPayment.PatientPayments.TableName].Rows[0][dsPayment.PatientPayments.AppointmentIdColumn.ColumnName]
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

        private string LoadPatientPayments(Int64 PaymentId, Int64 AppointmentId, Int64 VisitId, Int64 ChargeId)
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
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AppointmentId)))
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

                    DSPayment dsPatientPayment = null;

                    if (VisitId <= 0)
                    {
                        BLObject<DSPayment> obj = BLLBillingObj.LoadPatientPayments(PaymentId, AppointmentId, VisitId, ChargeId, "CopaymentLoad");
                        dsPatientPayment = obj.Data;
                    }
                    else
                    {
                        BLObject<DSPayment> obj = BLLBillingObj.LoadPatientPayments(PaymentId, 0, VisitId, ChargeId, "CopaymentLoad");
                        dsPatientPayment = obj.Data;
                    }

                    DSPayment dsPatientPayment1 = null;
                    BLObject<DSPayment> obj1 = BLLBillingObj.LoadPatientCopayByIds(AppointmentId, VisitId);
                    dsPatientPayment1 = obj1.Data;
                    //DataRow dr = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[0];
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PatientPaymentsCount = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows.Count,
                        PatientPayments_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName]),
                        PatientCoPayments_JSON = MDVUtility.JSON_DataTable(dsPatientPayment1.Tables[dsPatientPayment1.PatientCopayments.TableName]),
                        PatientMessageCount_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables["Table1"])
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

        private string RefundPatCopayment(Int64 PaymentId, Int64 AppointmentId, Int64 AdvPaymentId)
        {
            try
            {
                DSPayment dsPatientPayment = null;
                BLObject<DSPayment> obj = BLLBillingObj.LoadPatientPayments(PaymentId, AppointmentId);
                dsPatientPayment = obj.Data;

                DSPayment dsPatientPayment1 = new DSPayment();
                DSPayment.PatientPaymentsRow dr = dsPatientPayment1.PatientPayments.NewPatientPaymentsRow();

                string paymentId = null;
                string AdvPmtId = null;
                string patientId = null;
                string appointmentId = null;
                string providerId = null;
                string facilityId = null;
                string paymentDate = null;
                string paidAmountDr = null;
                string pmtTypeId = null;
                string ledgerAccId = null;
                string createdBy = null;
                string createdOn = null;
                string modifiedBy = null;
                string modifiedOn = null;
                string checkNo = null;
                string checkDate = null;
                string expiryDate = null;
                string cardTypeId = null;
                string visitid = null;

                if (dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows.Count > 0)
                {
                    for (int i = 0; i < dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows.Count; i++)
                    {
                        paymentId = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.PaymentIdColumn.ColumnName].ToString();
                        AdvPmtId = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.AdvPmtIdColumn.ColumnName].ToString();
                        patientId = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.PatientIdColumn.ColumnName].ToString();
                        appointmentId = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.AppointmentIdColumn.ColumnName].ToString();
                        providerId = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.ProviderIdColumn.ColumnName].ToString();
                        facilityId = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.FacilityIdColumn.ColumnName].ToString();
                        paymentDate = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.PaymentDateColumn.ColumnName].ToString();
                        paidAmountDr = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.PaidAmountDrColumn.ColumnName].ToString();

                        pmtTypeId = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.PmtTypeIdColumn.ColumnName].ToString();
                        ledgerAccId = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.LedgerAccIdColumn.ColumnName].ToString();

                        createdBy = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.CreatedByColumn.ColumnName].ToString();
                        createdOn = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.CreatedOnColumn.ColumnName].ToString();
                        modifiedBy = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.ModifiedByColumn.ColumnName].ToString();
                        modifiedOn = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.ModifiedOnColumn.ColumnName].ToString();

                        checkNo = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.CheckNoColumn.ColumnName].ToString();
                        checkDate = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.CheckDateColumn.ColumnName].ToString();
                        expiryDate = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.ExpiryDateColumn.ColumnName].ToString();
                        cardTypeId = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.CardTypeIdColumn.ColumnName].ToString();

                        visitid = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[i][dsPatientPayment.PatientPayments.VisitIdColumn.ColumnName].ToString();
                    }

                }

                dr.PatientId = MDVUtility.ToInt64(patientId);
                dr.MasterPaymentId = MDVUtility.ToInt64(paymentId);

                if (!string.IsNullOrEmpty(appointmentId))
                    dr.AppointmentId = MDVUtility.ToInt64(appointmentId);

                if (!string.IsNullOrEmpty(AdvPmtId))
                    dr.AdvPmtId = MDVUtility.ToInt64(AdvPmtId);

                if (!string.IsNullOrEmpty(visitid))
                    dr.VisitId = MDVUtility.ToInt64(visitid);

                dr.ProviderId = MDVUtility.ToInt64(providerId);
                if (!string.IsNullOrEmpty(facilityId))
                    dr.FacilityId = MDVUtility.ToInt64(facilityId);

                if (!string.IsNullOrEmpty(ledgerAccId))
                {
                    dr.LedgerAccId = MDVUtility.ToInt32(ledgerAccId);
                }
                dr.PaidAmountCr = MDVUtility.ToDouble(paidAmountDr);

                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;



                if (!string.IsNullOrEmpty(pmtTypeId))
                {
                    dr.PmtTypeId = MDVUtility.ToInt32(pmtTypeId);
                }

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(paymentDate)))
                    dr.PaymentDate = MDVUtility.ToDateTime(paymentDate);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(checkNo)))
                    dr.CheckNo = checkNo;
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(checkDate)))
                    dr.CheckDate = MDVUtility.ToStr(checkDate);
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(expiryDate)))
                    dr.ExpiryDate = MDVUtility.ToStr(expiryDate);

                if (!string.IsNullOrEmpty(cardTypeId))
                {
                    dr.CardTypeId = MDVUtility.ToInt32(cardTypeId);
                }
                dsPatientPayment1.PatientPayments.AddPatientPaymentsRow(dr);

                BLObject<DSPayment> obj1 = BLLBillingObj.InsertPatientPayments(dsPatientPayment1);


                if (obj1.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        PaymentId = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[0][dsPatientPayment.PatientPayments.PaymentIdColumn.ColumnName]
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

        private string LoadPatientCopayDetailByIds(Int64 AppointmentId, Int64 VisitId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(VisitId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AppointmentId)))
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

                    DSPayment dsPatientPayment = null;
                    BLObject<DSPayment> obj = BLLBillingObj.LoadPatientCopayByIds(AppointmentId, VisitId);
                    dsPatientPayment = obj.Data;


                    //DataRow dr = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[0];
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PatientPaymentsCount = dsPatientPayment.Tables[dsPatientPayment.PatientCopayments.TableName].Rows.Count,
                        PatientCoPayments_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables[dsPatientPayment.PatientCopayments.TableName])
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

        private string LoadPatientPaymentsByChrgId(Int64 PaymentId, Int64 AppointmentId, Int64 VisitId, Int64 ChargeId)
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
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AppointmentId)))
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

                    DSPayment dsPatientPayment = null;
                    BLObject<DSPayment> obj = BLLBillingObj.LoadPatientPayments(PaymentId, AppointmentId, VisitId, ChargeId, "ChargePaidPayment");
                    dsPatientPayment = obj.Data;

                    DSPayment dsPatientPayment1 = null;
                    BLObject<DSPayment> obj1 = BLLBillingObj.LoadPatientCopayByIds(AppointmentId, VisitId);
                    dsPatientPayment1 = obj1.Data;
                    //DataRow dr = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows[0];
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PatientPaymentsCount = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows.Count,
                        PatientPayments_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName]),
                        PatientCoPayments_JSON = MDVUtility.JSON_DataTable(dsPatientPayment1.Tables[dsPatientPayment1.PatientCopayments.TableName]),
                        PatientMessageCount_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables["Table1"])
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

        private string LoadCopayReceiptInfo(string PaymentId)
        {
            try
            {
                List<PaymentReceiptInfoModel> PaymentReceiptInfoList = null;
                BLObject<List<PaymentReceiptInfoModel>> ObjPayment;
                ObjPayment = BLLBillingObj.LoadPatientPaymentReceiptInfo(PaymentId);
                if (ObjPayment.Data != null)
                {
                    PaymentReceiptInfoList = ObjPayment.Data;
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PaymentReceiptInfoListRecordCount = PaymentReceiptInfoList.Count,
                        PaymentReceiptInfo_JSON = PaymentReceiptInfoList,
                    };
                    return (JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ObjPayment.Message
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

        #endregion

        #region Service Command Handler

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "SAVE_PATIENT_COPAYMENT":
                    {
                        string fieldsJSON = context.Request["PatientCopaymentData"];
                        long PatientVisitId = MDVUtility.ToLong(context.Request["PatientVisitId"]);
                        string strJSONData = SavePatientCoPayment(fieldsJSON, PatientVisitId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "GET_PATIENT_COPAYMENT":
                    {
                        Int64 PaymentId = MDVUtility.ToInt64(context.Request["PaymentId"]);
                        Int64 AppointmentId = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        Int64 ChargeId = 0;
                        if (context.Request["ChargeId"] != null)
                        {
                            ChargeId = MDVUtility.ToInt64(context.Request["ChargeId"]);
                        }
                        string strJSONData = LoadPatientPayments(PaymentId, AppointmentId, VisitId, ChargeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "GET_PATIENT_COPAYMENTBYIDS":
                    {
                        Int64 AppointmentId = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        string strJSONData = LoadPatientCopayDetailByIds(AppointmentId, VisitId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "REFUND_PATIENT_COPAYMENT":
                    {
                        Int64 PaymentId = MDVUtility.ToInt64(context.Request["PaymentId"]);
                        Int64 AppointmentId = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        Int64 AdvPmtId = MDVUtility.ToInt64(context.Request["AdvancePaymnetId"]);
                        string strJSONData = RefundPatCopayment(PaymentId, AppointmentId, AdvPmtId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "GET_PATIENT_COPAYMENT_BY_CHRGID":
                    {
                        Int64 PaymentId = MDVUtility.ToInt64(context.Request["PaymentId"]);
                        Int64 AppointmentId = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        Int64 ChargeId = 0;
                        if (context.Request["ChargeId"] != null)
                        {
                            ChargeId = MDVUtility.ToInt64(context.Request["ChargeId"]);
                        }
                        string strJSONData = LoadPatientPaymentsByChrgId(PaymentId, AppointmentId, VisitId, ChargeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_RECEIPT_INFO":
                    {
                        string PaymentId = MDVUtility.ToStr(context.Request["PaymentId"]);
                        string strJSONData = LoadCopayReceiptInfo(PaymentId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion

    }
}