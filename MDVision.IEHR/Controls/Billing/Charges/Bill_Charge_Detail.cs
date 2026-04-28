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
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Billing
{
    public class Bill_Charge_Detail
    {
        private BLLBilling BLLBillingObj = null;
        private BLLVisits BLLVisitsObj = null;
        public Bill_Charge_Detail()
        {
            BLLBillingObj = new BLLBilling();
            BLLVisitsObj = new BLLVisits();
        }
        #region Singleton
        private static Bill_Charge_Detail _obj = null;
        public static Bill_Charge_Detail Instance()
        {
            if (_obj == null)
                _obj = new Bill_Charge_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string FillChargeDetail(Int64 ChargeCapId)
        {
            string Format = "{0:0.00}";
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ChargeCapId)))
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
                    DSCharge dsCharge = null;
                    BLObject<DSCharge> obj = BLLBillingObj.LoadPatientCharges(ChargeCapId, "", "", 0, 0, "", 0, "", "", null, null, 0, 1);
                    if (obj.Data != null)
                    {
                        dsCharge = obj.Data;
                        if (dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows[0];
                            float totalFee = MDVUtility.Tofloat(dr[dsCharge.PatientCharges.FeeColumn.ColumnName]) * (MDVUtility.Tofloat(dr[dsCharge.PatientCharges.UnitsColumn.ColumnName]));
                            if (MDVUtility.ToStr(dr[dsCharge.PatientCharges.IsVNCColumn.ColumnName]) == "False")
                            {
                                totalFee = totalFee * -1;

                            }
                            float totalBalance = MDVUtility.Tofloat(dr[dsCharge.PatientCharges.InsBalanceColumn.ColumnName]) + MDVUtility.Tofloat(dr[dsCharge.PatientCharges.PatBalanceColumn.ColumnName]) + MDVUtility.Tofloat(dr[dsCharge.PatientCharges.CopayBalanceColumn.ColumnName]);
                            var keyValues = new Dictionary<string, string>
                        {
                            { "dtpDOSFrom", string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsCharge.PatientCharges.DOSFromColumn.ColumnName]))?"":MDVUtility.ToDateTime(dr[dsCharge.PatientCharges.DOSFromColumn.ColumnName]).ToShortDateString()},
                            { "dtpDOSTo",string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsCharge.PatientCharges.DOSToColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsCharge.PatientCharges.DOSToColumn.ColumnName]).ToShortDateString()},
                            { "txtICD1", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode1Column.ColumnName])},
                            { "hfICD1", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode1Column.ColumnName])},
                            { "hfICDDescription1", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode1DescriptionColumn.ColumnName])},
                            { "hfICD101", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICD10Code1Column.ColumnName])},
                            { "hfICD10Description1", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICD10Code1DescriptionColumn.ColumnName])},
                            { "hfSNOMED1", MDVUtility.ToStr(dr[dsCharge.PatientCharges.SNOMEDIDColumn.ColumnName])},
                            { "hfSNOMEDDescription1", MDVUtility.ToStr(dr[dsCharge.PatientCharges.SNOMEDDescriptionColumn.ColumnName])},
                            { "txtICD2", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode2Column.ColumnName])},
                            { "hfICD2", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode2Column.ColumnName])},
                            { "hfICDDescription2", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode2DescriptionColumn.ColumnName])},
                            { "hfICD102", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICD10Code2Column.ColumnName])},
                            { "hfICD10Description2", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICD10Code2DescriptionColumn.ColumnName])},
                            { "hfSNOMED2", MDVUtility.ToStr(dr[dsCharge.PatientCharges.SNOMEDIDColumn.ColumnName])},
                            { "hfSNOMEDDescription2", MDVUtility.ToStr(dr[dsCharge.PatientCharges.SNOMEDDescriptionColumn.ColumnName])},
                            { "txtICD3", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode3Column.ColumnName])},
                            { "hfICD3", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode3Column.ColumnName])},
                            { "hfICDDescription3", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode3DescriptionColumn.ColumnName])},
                            { "hfICD103", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICD10Code3Column.ColumnName])},
                            { "hfICD10Description3", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICD10Code3DescriptionColumn.ColumnName])},
                            { "hfSNOMED3", MDVUtility.ToStr(dr[dsCharge.PatientCharges.SNOMEDIDColumn.ColumnName])},
                            { "hfSNOMEDDescription3", MDVUtility.ToStr(dr[dsCharge.PatientCharges.SNOMEDDescriptionColumn.ColumnName])},
                            { "txtICD4", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode4Column.ColumnName])},
                            { "hfICD4", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode4Column.ColumnName])},
                            { "hfICDDescription4", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICDCode4DescriptionColumn.ColumnName])},
                            { "hfICD104", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICD10Code4Column.ColumnName])},
                            { "hfICD10Description4", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ICD10Code4DescriptionColumn.ColumnName])},
                            { "hfSNOMED4", MDVUtility.ToStr(dr[dsCharge.PatientCharges.SNOMEDIDColumn.ColumnName])},
                            { "hfSNOMEDDescription4", MDVUtility.ToStr(dr[dsCharge.PatientCharges.SNOMEDDescriptionColumn.ColumnName])},
                            { "txtPOS", MDVUtility.ToStr(dr[dsCharge.PatientCharges.POSCodeColumn.ColumnName])},
                            { "txtPrimaryFEE", String.Format(Format,dr[dsCharge.PatientCharges.PrimaryFeeColumn.ColumnName])},
                            { "txtFEE", String.Format(Format,dr[dsCharge.PatientCharges.FeeColumn.ColumnName])},
                            { "txtTotalFEE", String.Format(Format,totalFee)},
                            { "txtTotalBalance", String.Format(Format,totalBalance)},
                            { "txtExpectedFee", String.Format(Format,dr[dsCharge.PatientCharges.ExpectedFeeColumn.ColumnName])},
                            { "txtCPT", MDVUtility.ToStr(dr[dsCharge.PatientCharges.CPTCodeColumn.ColumnName])},
                            { "hfCPTDescription", MDVUtility.ToStr(dr[dsCharge.PatientCharges.CPTDescriptionColumn.ColumnName])},
                            { "txtUnits", MDVUtility.ToStr(dr[dsCharge.PatientCharges.UnitsColumn.ColumnName])},                          
                            { "txtTotalUnits", MDVUtility.ToStr(dr[dsCharge.PatientCharges.TotalUnitsColumn.ColumnName])},    
                            { "hfFacility", MDVUtility.ToStr(dr[dsCharge.PatientCharges.FacilityIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsCharge.PatientCharges.FacilityNameColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ProviderIdColumn.ColumnName])},
                            { "txtProvider", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ProviderNameColumn.ColumnName])},
                            { "hfInsurancePlan", MDVUtility.ToStr(dr[dsCharge.PatientCharges.PatientInsuranceIdColumn.ColumnName])},
                            { "txtInsurancePlan", MDVUtility.ToStr(dr[dsCharge.PatientCharges.InsurancePlanNameColumn.ColumnName])},
                            { "txtTOS", MDVUtility.ToStr(dr[dsCharge.PatientCharges.TOSCodeColumn.ColumnName])},
                            { "txtINSCharges", String.Format(Format,dr[dsCharge.PatientCharges.InsChargesColumn.ColumnName])},
                            { "txtPATCharges", String.Format(Format,dr[dsCharge.PatientCharges.PatChargesColumn.ColumnName])},
                            { "txtCOPAY", String.Format(Format,dr[dsCharge.PatientCharges.CopayColumn.ColumnName])},
                            { "txtNDC", MDVUtility.ToStr(dr[dsCharge.PatientCharges.NDCColumn.ColumnName])},
                            { "txtNDCUnit", MDVUtility.ToStr(dr[dsCharge.PatientCharges.NDCUnitColumn.ColumnName])},
                            { "txtNDCUnitPrice", String.Format(Format,dr[dsCharge.PatientCharges.NDCUnitPriceColumn.ColumnName])},
                            { "dtpEOD",  dr[dsCharge.PatientCharges.EODColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsCharge.PatientCharges.EODColumn.ColumnName]).ToShortDateString()},
                            { "txtModifier1", MDVUtility.ToStr(dr[dsCharge.PatientCharges.Modifier1Column.ColumnName])},
                            { "txtModifier2", MDVUtility.ToStr(dr[dsCharge.PatientCharges.Modifier2Column.ColumnName])},
                            { "txtModifier3", MDVUtility.ToStr(dr[dsCharge.PatientCharges.Modifier3Column.ColumnName])},
                            { "txtModifier4", MDVUtility.ToStr(dr[dsCharge.PatientCharges.Modifier4Column.ColumnName])},
                            { "txtStatus", MDVUtility.ToStr(dr[dsCharge.PatientCharges.StatusColumn.ColumnName])},
                            { "ddlNDCMeasurement", MDVUtility.ToStr(dr[dsCharge.PatientCharges.NDCMeasurCodeIdColumn.ColumnName])},
                            { "chkHold", MDVUtility.ToStr(dr[dsCharge.PatientCharges.IsHoldColumn.ColumnName])},
                            { "txtHoldDays", MDVUtility.ToStr(dr[dsCharge.PatientCharges.HoldDaysColumn.ColumnName])},
                            { "chkEMG", MDVUtility.ToStr(dr[dsCharge.PatientCharges.EMGColumn.ColumnName])},
                            { "txtEntryDate",string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsCharge.PatientCharges.CreatedOnColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsCharge.PatientCharges.CreatedOnColumn.ColumnName]).ToShortDateString()},
                            { "txtEnteredBy", MDVUtility.ToStr(dr[dsCharge.PatientCharges.EnteredByFullNameColumn.ColumnName])},
                            { "txtInsBalance", String.Format(Format,dr[dsCharge.PatientCharges.InsBalanceColumn.ColumnName])},
                            { "txtInsPaid", String.Format(Format,dr[dsCharge.PatientCharges.InsPaidColumn.ColumnName])},
                            { "txtWriteoff", String.Format(Format,dr[dsCharge.PatientCharges.InsWriteOffColumn.ColumnName])},
                            { "txtChargeNumber", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName])},
                            { "txtBatchNo", MDVUtility.ToStr(dr[dsCharge.PatientCharges.BatchNumberColumn.ColumnName])},
                            { "txtSubmitDate", MDVUtility.ToStr(dr[dsCharge.PatientCharges.SubmittedDateColumn.ColumnName])},
                            { "txtSubmittedBy", MDVUtility.ToStr(dr[dsCharge.PatientCharges.SubimittedByFullNameColumn.ColumnName])},
                            { "txtCopayPaid", MDVUtility.ToStr(dr[dsCharge.PatientCharges.CopayPaidColumn.ColumnName])},
                            { "txtCopayDiscount", MDVUtility.ToStr(dr[dsCharge.PatientCharges.CopayDiscountColumn.ColumnName])},
                            { "txtCopayBalance", MDVUtility.ToStr(dr[dsCharge.PatientCharges.CopayBalanceColumn.ColumnName])},
                            { "txtPatientDiscount", MDVUtility.ToStr(dr[dsCharge.PatientCharges.PatDiscountColumn.ColumnName])},
                            { "txtPatientPaid", MDVUtility.ToStr(dr[dsCharge.PatientCharges.PatPaidColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsCharge.PatientCharges.LineNotesColumn.ColumnName])},
                            { "txtServiceDescription", MDVUtility.ToStr(dr[dsCharge.PatientCharges.ServiceDescriptionColumn.ColumnName])},
                            { "txtPatBalance", MDVUtility.ToStr(dr[dsCharge.PatientCharges.PatBalanceColumn.ColumnName])},
                            { "chkPrimary", MDVUtility.ToStr(dr[dsCharge.PatientCharges.IsPrimaryColumn.ColumnName])},
                            { "IsLocked", MDVUtility.ToStr(dr[dsCharge.PatientCharges.IsLockedColumn.ColumnName])},
                            
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ChargesFill_JSON = js.Serialize(keyValues),
                                ChargesDetail_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientCharges.TableName])
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

        private string GetLedgerBalance(long PaymentId, long AppointmentId, long VisitId, long ChargeId)
        {
            try
            {
                DSPayment dsPatientPayment = null;
                BLObject<DSPayment> obj = BLLBillingObj.LoadPatientPayments(PaymentId, AppointmentId, VisitId, ChargeId, "GetLedgerBalance");
                dsPatientPayment = obj.Data;
                long balance = 0;
                foreach (DataRow dr in dsPatientPayment.PatientPayments)
                {
                    string AmountCr = dr[dsPatientPayment.PatientPayments.PaidAmountCrColumn.ColumnName].ToString();
                    string AmountDr = dr[dsPatientPayment.PatientPayments.PaidAmountDrColumn.ColumnName].ToString();
                    if (AmountCr == "")
                    {
                        balance = balance + MDVUtility.ToLong(AmountDr);
                    }
                    if (AmountDr == "")
                    {
                        balance = balance - MDVUtility.ToLong(AmountCr);
                    }

                }
                var response = new
                           {
                               status = true,
                               PatientPaymentsCount = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows.Count,
                               PatientPayments_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName]),
                               Balance = balance,
                               //PatientCoPayments_JSON = MDVUtility.JSON_DataTable(dsPatientPayment1.Tables[dsPatientPayment1.PatientCopayments.TableName])
                           };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

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
        private string LoadTransferCharges(long VisitId)
        {

            try
            {

                DSCharge dsCharge = null;
                BLObject<DSCharge> obj;
                obj = BLLBillingObj.LoadPatientTransferCharges(VisitId);
                dsCharge = obj.Data;
                if (obj.Data != null)
                {
                    if (dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            TransferedChargesCount = dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count,
                            TransferedChargesLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientCharges.TableName]),
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

        private string ResubmitCharge(string VisitId, string ChargeId)
        {

            try
            {
                if (string.IsNullOrEmpty(VisitId) || string.IsNullOrEmpty(ChargeId))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Resubmit_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    long visitId = MDVUtility.ToLong(VisitId);
                    long chargeId = MDVUtility.ToLong(ChargeId);
                    DSCharge dsCharge = null;
                    DSVisits dsVisit = null;
                    BLObject<DSCharge> objDsCharge = BLLBillingObj.LoadPatientCharges(chargeId, "", "", 0, 0, "", 0, "", "", null, null, 0, 1);
                    dsCharge = objDsCharge.Data;

                    foreach (DSCharge.PatientChargesRow drcharge in dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows)
                    {
                        drcharge.StatusId = 3;
                        drcharge.Status = "ReSubmit";
                        //PMS-1640
                        drcharge.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        drcharge.ModifiedOn = DateTime.Now;
                    }
                    if (dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count > 0)
                    {
                        BLObject<DSCharge> objCharge = BLLBillingObj.UpdatePatientCharges(dsCharge);
                        BLObject<DSVisits> objDSVisit =BLLVisitsObj.LoadPatientsVisits(visitId, 0, 0, 0, null, null, "", "", "");

                        dsVisit = objDSVisit.Data;
                        if (dsVisit.PatientVisits.Rows.Count > 0)
                        {
                            //Edited by Azeem Raza Tayyab on 14-Mar-2016 to fix Bug#PMS-4411
                            //ReSubmit Visit while resubmitting Charge,
                            foreach (DSVisits.PatientVisitsRow drVisits in dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows)
                            {
                                drVisits.ClaimStatusId = 3;
                                drVisits.VisitStatus = "Seen";
                                drVisits.SubmittedBy = null;
                                drVisits.SubmittedDate = null;
                                //in case of resubmit also update visit updatedby and updated on
                                drVisits.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                drVisits.ModifiedOn = DateTime.Now;
                            }
                            if (dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count > 0)
                            {
                                BLObject<DSVisits> objVisit =BLLVisitsObj.UpdatePatientsVisit(dsVisit);

                                if (objVisit.Data != null)
                                {
                                    var response = new
                                    {
                                        status = true,
                                        Message = Common.AppPrivileges.Update_Message
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                }
                            }
                        }

                        //BLObject<bool> obj = BusinessWrapper.BillingClaim.BusinessObj.UpdateVisitChargeStatus(3, visitId, chargeId);

                        //if (obj.Data.Equals(true))
                        //{
                        //    var response = new
                        //    {
                        //        status = true,
                        //        Message = Common.AppPrivileges.Resubmit_Visit_Charge_Message
                        //    };
                        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        //}
                        //else
                        //{
                        //var resp = new
                        //{
                        //    status = false,
                        //    Message = Common.AppPrivileges.Resubmit_Error_Message
                        //};
                        //return (Newtonsoft.Json.JsonConvert.SerializeObject(resp));
                        //}
                    }
                    var res = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Resubmit_Error_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(res));
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
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "FILL_CHARGE_DETAIL":
                    {
                        string strChargeCapId = context.Request["ChargeCapId"];
                        string strJSONData = FillChargeDetail(MDVUtility.ToInt64(strChargeCapId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "GET_LEDGER_BALANCE":
                    {
                        Int64 ChargeId = 0;
                        Int64 VisitId = 0;
                        Int64 AppointmentId = 0;
                        Int64 PaymentId = 0;

                        if (context.Request["ChargeId"] != null)
                        {
                            ChargeId = MDVUtility.ToInt64(context.Request["ChargeId"]);
                        }
                        if (context.Request["VisitId"] != null)
                        {
                            VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        }
                        if (context.Request["AppointmentId"] != null)
                        {
                            AppointmentId = MDVUtility.ToInt64(context.Request["AppointmentId"]);
                        }
                        if (context.Request["PaymentId"] != null)
                        {
                            PaymentId = MDVUtility.ToInt64(context.Request["PaymentId"]);
                        }
                        string strJSONData = GetLedgerBalance(PaymentId, AppointmentId, VisitId, ChargeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }

                case "LOAD_TRANSFER_CHARGES":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charges", "SEARCH")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long VisitId = MDVUtility.ToLong(context.Request["VisitId"]);
                            strJSONData = LoadTransferCharges(VisitId);
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
                        break;
                    }
                case "RESUBMIT_CHARGE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charges", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string VisitID = MDVUtility.ToStr(context.Request["VisitID"]);
                            string ChargeId = MDVUtility.ToStr(context.Request["ChargeId"]);
                            strJSONData = ResubmitCharge(VisitID, ChargeId);
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
                        break;
                    }
            }
        }
        #endregion
    }
}