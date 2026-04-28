using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.IEHR.Controls.Patient.Demographics;

namespace MDVision.IEHR.Controls.CommonControls
{
    public class Patient_Ledger
    {
        private BLLBilling BLLBillingObj = null;
        private BLLPatient BLLPatientObj = null;
        public Patient_Ledger()
        {
            BLLBillingObj = new BLLBilling();
            BLLPatientObj = new BLLPatient();
        }

        #region Singleton
        private static Patient_Ledger _obj = null;
        public static Patient_Ledger Instance()
        {
            if (_obj == null)
                _obj = new Patient_Ledger();
            return _obj;
        }
        #endregion

        #region Private Functions


        private string SearchPatientLedger(string fieldsJSON, Int64 PatientId, Int32 PageNumber, Int32 RowsPerPage)
        {
            try
            {
                DSPatientLedger dsCharge = null;
                BLObject<DSPatientLedger> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                Int64 facilityId = 0, providerId = 0, insurancePlanId = 0;
                //int charges = 0, insPayment = 0, patPayment = 0, statement = 0, submit = 0;
                DateTime? DOSFrom = null, DOSTo = null;
                //Int32 BilledToId = 0;
                Int32 ClaimType = 0;
                bool IsCollection = false;
                bool isOtherClaims = true;
                bool IsVoidedClaims = false;
                bool isShowDetails = false;
                DOSFrom = String.IsNullOrEmpty(SearchedfieldsJSON["dpDOSFrom"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpDOSFrom"]);
                DOSTo = String.IsNullOrEmpty(SearchedfieldsJSON["dpDOSTo"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpDOSTo"]);
                //string ClaimType = !string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimType"]) ? MDVUtility.ToStr(SearchedfieldsJSON["ddlClaimType"]) : "";
                string InsurancePlaneId = !string.IsNullOrEmpty(SearchedfieldsJSON["txtInsurancePlan"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtInsurancePlan"]) : "";
                IsCollection = MDVUtility.ToStr(SearchedfieldsJSON["chkInculdeCollectionBal"]) == "True" ? true : false;
                isOtherClaims = MDVUtility.ToStr(SearchedfieldsJSON["chkOtherBalance"]) == "True" ? true : false;
                isShowDetails = MDVUtility.ToStr(SearchedfieldsJSON["chkDetails"]) == "True" ? true : false;
                // providerId = !string.IsNullOrEmpty(SearchedfieldsJSON["hfProviderId"]) ? MDVUtility.ToStr(SearchedfieldsJSON["hfProviderId"]) : "";
                //facilityId = !string.IsNullOrEmpty(SearchedfieldsJSON["hfFacilityId"]) ? MDVUtility.ToStr(SearchedfieldsJSON["hfFacilityId"]) : "";

                if (DOSFrom == null)
                    DOSFrom = DOSTo;
                if (DOSTo == null)
                    DOSTo = DOSFrom;

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["hfFacilityId"])))
                    facilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacilityId"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["hfProviderId"])))
                    providerId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProviderId"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["hfInsurancePlan"])))
                    insurancePlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfInsurancePlan"]);

                //if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBilledTo"])))
                //    BilledToId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlBilledTo"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlLedgerClaimType"])))
                    ClaimType = MDVUtility.ToInt32(SearchedfieldsJSON["ddlLedgerClaimType"]);


                IsVoidedClaims = MDVUtility.ToStr(SearchedfieldsJSON["chkVoidedClaims"]) == "True" ? true : false; 

                //insPayment = MDVUtility.ToStr(SearchedfieldsJSON["chkCPTPayments"]) == "True" ? 1 : 0;

                //patPayment = MDVUtility.ToStr(SearchedfieldsJSON["chkPatPaymentTotal"]) == "True" ? 1 : 0;

                //statement = MDVUtility.ToStr(SearchedfieldsJSON["chkStatement"]) == "True" ? 1 : 0;

                //submit = MDVUtility.ToStr(SearchedfieldsJSON["chkSubmit"]) == "True" ? 1 : 0;


                obj = BLLBillingObj.LoadPatientLedger(PatientId, facilityId, providerId, DOSFrom, DOSTo, insurancePlanId, 0, ClaimType, IsCollection, isOtherClaims, IsVoidedClaims, isShowDetails);

                dsCharge = obj.Data;

                //------

                DSPatient dsPatient = new DSPatient();
                BLObject<DSPatient> objLoad = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(PatientId), "Demographics");
                dsPatient = objLoad.Data;
                // start PMS-4506
                // update patient actual age
                DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                string AgeResponse = Patient_Demographic.Instance().GetAge(MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]));
                
                 SearchedfieldsJSON = ser.Deserialize<dynamic>(AgeResponse);

                string actualAge = MDVUtility.ToStr(SearchedfieldsJSON["ActualAge"]);
                System.Data.DataColumn newColumn = new System.Data.DataColumn("PatientAge", typeof(System.String));
                newColumn.DefaultValue = actualAge;
                dsPatient.Tables[dsPatient.Patients.TableName].Columns.Add(newColumn);
                // end PMS-4506
                //------

                if (obj.Data != null)
                {
                    if (dsCharge.Tables[dsCharge.PatientCharges_New.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ChargeCount = dsCharge.Tables[dsCharge.PatientCharges_New.TableName].Rows.Count,
                            //iTotalDisplayRecords = dsCharge.PatientCharges_New.Rows[0][dsCharge.PatientCharges_New.RecordCountColumn.ColumnName],
                            ChargeLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientCharges_New.TableName]),
                            //outstanding balance result
                            OutstandingBalanceCount = dsCharge.Tables[dsCharge.PatientOutstandingBalance.TableName].Rows.Count,
                            OutstandingLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientOutstandingBalance.TableName]),
                            PatientsLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ChargeCount = 0,
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
                        ChargeCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string SaveAccountNote(string AccountNote, Int64 PatientId)
        {
            try
            {
                if (MDVUtility.ToInt64(PatientId) > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    BLObject<DSPatient> objLoad = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(PatientId), "Demographics");
                    dsPatient = objLoad.Data;
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        foreach (DSPatient.PatientsRow dr in dsPatient.Tables[dsPatient.Patients.TableName].Rows)
                        {
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                            dr.AccountNoteComments = MDVUtility.ToStr(AccountNote);

                        }
                        #region Database Updation

                        if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                        {

                            BLObject<DSPatient> obj = BLLPatientObj.UpdatePatient(dsPatient);
                            if (obj.Data != null)
                            {
                                var response = new
                                {
                                    status = true,
                                    AccountNoteComments = "Last Updated By: " + MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + " " + DateTime.Now + " \n Notes: " + AccountNote,
                                    Message = Common.AppPrivileges.Update_Message
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
                            Message = objLoad.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient not found."
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


        public string SearchPatientPayments(Int64 patientId,int pageNo,int rowsPerPage)
        {
            try {
                
                DSPatientLedger dsPatientPayment = null;
                BLObject<DSPatientLedger> objPatientPayment = BLLBillingObj.SearchPatientPayments(MDVUtility.ToInt64(patientId), pageNo, rowsPerPage);
                dsPatientPayment = objPatientPayment.Data;
                if (dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        PatientPaymentLoad_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName]),
                        PatientPaymentLoadJSON_Count = dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName].Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = objPatientPayment.Message
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

        public string SearchReceivedPayments(Int64 pmtId)
        {
            try {
                
                DSPatientLedger dsPatientPayment = null;
                BLObject<DSPatientLedger> objPatientPayment = BLLBillingObj.SearchReceivedPayments(pmtId);
                dsPatientPayment = objPatientPayment.Data;
                if (dsPatientPayment.Tables[dsPatientPayment.ReceivedPatPayment.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        ReceivedPatientPaymentLoad_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables[dsPatientPayment.ReceivedPatPayment.TableName]),
                        ReceivedPatientPaymentLoadJSON_Count = dsPatientPayment.Tables[dsPatientPayment.ReceivedPatPayment.TableName].Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = objPatientPayment.Message
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

        public string LoadPrintPractice(Int64 PracticeId)
        {
            try
            {

                DSPatientLedger dsPatientPayment = null;
                BLObject<DSPatientLedger> objPatientPayment = BLLBillingObj.LoadPrintPractice(PracticeId);
                dsPatientPayment = objPatientPayment.Data;
                if (dsPatientPayment.Tables[dsPatientPayment.DT_Print_Practice.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        ReceivedPatientPaymentLoad_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables[dsPatientPayment.DT_Print_Practice.TableName]),
                        ReceivedPatientPaymentLoadJSON_Count = dsPatientPayment.Tables[dsPatientPayment.DT_Print_Practice.TableName].Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = objPatientPayment.Message
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
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Patient Message Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_CHARGE":
                    {
                        string fieldsJSON = context.Request["ChargeData"];
                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientId"]);
                        string strJSONData = SearchPatientLedger(fieldsJSON, PatientID, PageNumber, RowsPerPage);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_ACCOUNT_NOTE":
                    {
                        string AccountNote = MDVUtility.ToStr(context.Request["AccountNote"]);
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientId"]);
                        string strJSONData = SaveAccountNote(AccountNote, PatientID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_PATIENT_PAYMENTS":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientId"]);
                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        string strJSONData = SearchPatientPayments(PatientID, PageNumber, RowsPerPage);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "RECEVIED_PATIENT_PAYMENT":
                    {
                        Int64 PaymentID = MDVUtility.ToInt64(context.Request["PmtId"]);
                        string strJSONData = SearchReceivedPayments(PaymentID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PRINT_PRACTICE":
                    {
                        Int64 PracticeId = MDVUtility.ToInt64(context.Request["PracticeId"]);
                        string strJSONData = LoadPrintPractice(PracticeId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}