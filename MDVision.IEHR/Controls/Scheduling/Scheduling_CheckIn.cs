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
using MDVision.DataAccess.DCommon;
using MDVision.Model.PMSSchedule;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_CheckIn
    {
        private BLLPatient BLLPatientObj = null;
        private BLLBilling BLLBillingObj = null;
        private BLLClinical BLLClinicalObj = null;
        private BLLSchedule BLLScheduleObj = null;
        private BLLVisits BLLVisitsObj = null;
        public Scheduling_CheckIn()
        {
            BLLPatientObj = new BLLPatient();
            BLLBillingObj = new BLLBilling();
            BLLClinicalObj = new BLLClinical();
            BLLScheduleObj = new BLLSchedule();
            BLLVisitsObj = new BLLVisits();
        }

        #region Singleton
        private static Scheduling_CheckIn _obj = null;
        public static Scheduling_CheckIn Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_CheckIn();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string FillPatientAppointment(Int64 PatientId, Int64 AppointmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
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
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatientAndInsuranceById(PatientId, AppointmentId, 0, "AppointmentCheckIn");
                    dsPatient = obj.Data;

                    DSPayment dsPatientPayment = null;
                    BLObject<DSPayment> obj1 = BLLBillingObj.LoadPatientPayments(0, AppointmentId, 0, 0, "CheckIn");
                    dsPatientPayment = obj1.Data;

                    //DSAppointment dsSchedule = null;
                    //BLObject<DSAppointment> objSchedule =BLLScheduleObj.LoadPatientBalance(PatientId);
                    //dsSchedule = objSchedule.Data;

                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];

                        string AgeResponse = Patient_Demographic.Instance().GetAge(MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]));
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var SearchedfieldsJSON = ser.Deserialize<dynamic>(AgeResponse);

                        string actualAge = MDVUtility.ToStr(SearchedfieldsJSON["ActualAge"]);
                        var keyValues = new Dictionary<string, string>
                        {
                            { "hfAppointmentId", MDVUtility.ToStr(AppointmentId)},
                            { "txtPatientName", MDVUtility.ToStr(dr[dsPatient.Patients.FullNameColumn.ColumnName])},
                            { "txtDOB", MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "hfpatientid", MDVUtility.ToStr(dr[dsPatient.Patients.PatientIdColumn.ColumnName])},
                            { "txtAccountNumber", MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName])},
                            { "txtFirstName", MDVUtility.ToStr(dr[dsPatient.Patients.FirstNameColumn.ColumnName])},
                            { "txtLastName", MDVUtility.ToStr(dr[dsPatient.Patients.LastNameColumn.ColumnName])},
                            { "txtGender", MDVUtility.ToStr(dr[dsPatient.Patients.GenderColumn.ColumnName])},
                            { "txtHomePhoneNo", MDVUtility.ToStr(dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName])},
                            { "txtAge", actualAge},

                            //{ "txtProvider", MDVUtility.ToStr(dr[dsPatient.Patients.ProviderNameColumn.ColumnName])},
                            ////{ "txtResource", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ResourceNameColumn.ColumnName])},
                            //{ "txtFacility", MDVUtility.ToStr(dr[dsPatient.Patients.FacilityNameColumn.ColumnName])},
                            
                            //{ "hfProviderId", MDVUtility.ToStr(dr[dsPatient.Patients.ProviderIdColumn.ColumnName])},
                            //{ "hfFacilityId", MDVUtility.ToStr(dr[dsPatient.Patients.FacilityIdColumn.ColumnName])},
                            
                            //{ "hfRefProvider", MDVUtility.ToStr(dr[dsPatient.Patients.ReferringProviderIdColumn.ColumnName])},
                            //{ "txtRefProvider", MDVUtility.ToStr(dr[dsPatient.Patients.ReferringProviderNameColumn.ColumnName])},
                            
                        };
                        DSAppointment dsApp = new DSAppointment();
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {

                            status = true,
                            AppointmentFill_JSON = js.Serialize(keyValues),
                            PatientBalance_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                            PatientAppointment_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsApp.PatientAppointments.TableName]),
                            PatientInsuranceDetail_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientInsurance.TableName]),
                            PatientCopayment_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName]),

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
        private string SavePatientCheckIn(string fieldsJSON, string referralId, Int64 ResourceproviderId)
        {
           
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
               
                // Check In Region 
                DSVisits dsVisits = new DSVisits();
                bool IsAlreadyVisitCreated = true;
                string VisitId = "";
                long PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfpatientid"]);
                long AppointmentId = MDVUtility.ToInt64(SearchedfieldsJSON["hfAppointmentId"]);
                //check is already visit created for this appointment. http://192.168.0.16:8080/browse/PMS-2992
                BLObject<DSVisits> objTemp = BLLVisitsObj.LoadPatientsVisits(0, PatientId, 0, 0, null, null, "", "", "");
                if (objTemp != null)
                {
                    DSVisits dsTemp = objTemp.Data;
                    DataRow[] drs = dsTemp.Tables[dsTemp.PatientVisits.TableName].Select(dsTemp.PatientVisits.AppointmentIdColumn.ColumnName + " = " + AppointmentId);
                    if (drs.Count() > 0)
                    {
                        VisitId = MDVUtility.ToStr(dsTemp.Tables[dsTemp.PatientVisits.TableName].Rows[0][dsTemp.PatientVisits.VisitIdColumn.ColumnName]);
                        IsAlreadyVisitCreated = true;
                    }
                    else
                        IsAlreadyVisitCreated = false;
                }
                else
                {
                    IsAlreadyVisitCreated = false;
                }

                if (IsAlreadyVisitCreated == false)
                {
                    
                    DSVisits.PatientVisitsRow dr = dsVisits.PatientVisits.NewPatientVisitsRow();
                    dr.PatientId = PatientId;
                    dr.AppointmentId = AppointmentId;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProviderId"]))
                    {
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProviderId"]);
                        if (ResourceproviderId == null || ResourceproviderId == 0)
                        {
                            dr.ResourceProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProviderId"]);
                        }
                        else
                        {
                            dr.ResourceProviderId = ResourceproviderId;
                        }
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacilityId"]))
                    {
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacilityId"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfRefProvider"]))
                    {
                        dr.RefProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfRefProvider"]);
                    }


                    dr.VisitCopayment = MDVUtility.Tofloat(SearchedfieldsJSON["txtCopayment"]);
                   // dr.isAllVisitUsed = MDVUtility.ToBool(SearchedfieldsJSON["hfisAllVisitUsed"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlInsurancePlan"]))
                    {
                        dr.PatientInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlInsurancePlan"]);
                    }


                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSchReasonId"]))
                    //    dr.SchReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["hfSchReasonId"]);
                    //else
                    //    dr[dsVisits.PatientVisits.SchReasonIdColumn.ColumnName] = DBNull.Value;

                    dr.ReasonComments = MDVUtility.ToStr(SearchedfieldsJSON["txtReason"]);

                    dr.ClinicalTempId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClinicalTemplate"]);

                    if (SearchedfieldsJSON.ContainsKey("rdPCP") && SearchedfieldsJSON["rdPCP"] == true)
                    {
                        dr.IsSpecialist = false;
                    }
                    else if (SearchedfieldsJSON.ContainsKey("rdSpecialist") && SearchedfieldsJSON["rdSpecialist"] == true)
                    {
                        dr.IsSpecialist = true;
                    }
                    dr.IsActive = MDVUtility.ToInt64(1);
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    dr.VisitStatus = MDVUtility.ToStr("CheckIn");
                    dr.ClaimTypeId = MDVUtility.ToInt64(1);
                    //dr.VisitStatusId = MDVUtility.ToInt64(SearchedfieldsJSON["hfSchStatusId"]);
                    dsVisits.PatientVisits.AddPatientVisitsRow(dr);
                }

                #region Database Insertion.

                //******Appointment Status Update Region**************\\

                DSAppointment dsAppointment = null;
                BLObject<DSAppointment> objAppointment = BLLScheduleObj.LoadAppointment(MDVUtility.ToInt64(SearchedfieldsJSON["hfAppointmentId"]), null, null, null, null);
                if (objAppointment.Data != null)
                {
                    dsAppointment = objAppointment.Data;

                    if (dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drAppointment in dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows)
                        {
                            drAppointment[dsAppointment.PatientAppointments.SchStatusIdColumn] = MDVUtility.ToInt64(0);
                            drAppointment[dsAppointment.PatientAppointments.AppointmentStatusColumn] = MDVUtility.ToStr("Check In");

                            if (SearchedfieldsJSON["rdPCP"] == true)
                            {
                                drAppointment[dsAppointment.PatientAppointments.IsSpecialistColumn] = false;
                            }                                                     
                            else if (SearchedfieldsJSON["rdSpecialist"] == true)
                            {
                                drAppointment[dsAppointment.PatientAppointments.IsSpecialistColumn] = true;
                            }
                            // deAttach Patient Refrral Number with appointments
                            if (MDVUtility.ToBool(SearchedfieldsJSON["hfisAllVisitUsed"]) == true)
                            {
                                drAppointment[dsAppointment.PatientAppointments.ReferralNoColumn] = "";
                                drAppointment[dsAppointment.PatientAppointments.PatientReferralIdColumn] = DBNull.Value;
                                drAppointment[dsAppointment.PatientAppointments.IsAllVisitUsedColumn] = true;

                            }
                            else {
                                drAppointment[dsAppointment.PatientAppointments.IsAllVisitUsedColumn] = false;
                            }

                            drAppointment[dsAppointment.PatientAppointments.PatientInsuranceIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlInsurancePlan"]);
                            drAppointment[dsAppointment.PatientAppointments.CheckInReasonColumn] = MDVUtility.ToStr(SearchedfieldsJSON["hfCheckInReason"]);
                            //Start 22-08-2016 Humaira Yousaf for referral Id
                            if (!string.IsNullOrEmpty(referralId))
                            {
                                drAppointment[dsAppointment.PatientAppointments.ReferralIdColumn] = MDVUtility.ToInt64(referralId);
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfRefProvider"]))
                            {
                                drAppointment[dsAppointment.PatientAppointments.RefProviderIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["hfRefProvider"]);
                            }

                            //End 22-08-2016 Humaira Yousaf for referral Id
                            drAppointment[dsAppointment.PatientAppointments.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            drAppointment[dsAppointment.PatientAppointments.ModifiedOnColumn] = DateTime.Now;
                        }
                    }


                    BLObject<ResponseModel> model = BLLScheduleObj.UpdateAppointmentAndVisit(dsAppointment, dsVisits, IsAlreadyVisitCreated, VisitId);
                    if (model.Data.IsSuccess)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            VisitId = model.Data.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    { 
                      var response = new
                        {
                            status = false,
                            Message = "Cannot perform Check In  on Reschedule appointment status",
                            VisitId = ""
                        };
                      return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                  
                }
                else
                {
                    throw new Exception(objAppointment.Message);
                }

                #endregion


            }
            catch (Exception ex)
            {
                string Message = "";
                if (ex.Message.Contains('|'))
                {
                    Message = ex.Message.Split('|')[0];
                }
                else
                {
                    Message = ex.Message;
                }
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string CancelPatientCheckIn(Int64 VisitId, Int64 AppointmentId, Int64 statusId, string status)
        {

            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(VisitId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else if (string.IsNullOrEmpty(MDVUtility.ToStr(AppointmentId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLScheduleObj.AppointmentCancelCheckIn(MDVUtility.ToStr(VisitId));
                    if (obj.Data == "")
                    {

                        //******Appointment Status Update Region**************\\

                        DSAppointment dsAppointment = null;
                        BLObject<DSAppointment> objAppointment = BLLScheduleObj.LoadAppointment(AppointmentId, null, null, null, null);
                        dsAppointment = objAppointment.Data;

                        if (dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow drAppointment in dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows)
                            {
                                drAppointment[dsAppointment.PatientAppointments.SchStatusIdColumn] = statusId;
                                drAppointment[dsAppointment.PatientAppointments.AppointmentStatusColumn] = status;
                                drAppointment[dsAppointment.PatientAppointments.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                drAppointment[dsAppointment.PatientAppointments.ModifiedOnColumn] = DateTime.Now;
                            }
                        }

                        BLObject<DSAppointment> objType = null;
                        objType = BLLScheduleObj.UpdateAppointmentStatus(dsAppointment);

                        //*********End Appointment Status Update***********\\
                        if (objType.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objType.Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }


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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string LoadTempalateLookup(Int64 specialityId, Int64 providerId, Int64 rpp, Int64 pageNo)
        {
            try
            {
                DSTemplateBuilder dsTemplate = null;
                var objTemplate = BLLClinicalObj.LoadClinicalTemplate(0, null, null, 0, specialityId, providerId, null, rpp, pageNo);

                dsTemplate = objTemplate.Data;

                var response = new
                {
                    status = true,
                    TemplateCount = dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName].Rows.Count,
                    iTotalDisplayRecords = dsTemplate.ClinicalTemplate.Rows[0][dsTemplate.ClinicalTemplate.RecordCountColumn],
                    TemplateLoad_JSON = MDVUtility.JSON_DataTable(dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName]),
                };
                return (JsonConvert.SerializeObject(response));
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
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_PATIENT_CHECKIN":
                    {
                        string fieldsJSON = context.Request["PatientCheckInData"];
                        string referralId = context.Request["ReferralId"];
                        Int64 ResourceproviderId = MDVUtility.ToInt64(context.Request["ResourceproviderId"]);
                        string strJSONData = SavePatientCheckIn(fieldsJSON, referralId, ResourceproviderId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_PATIENT_DATA":
                    {
                        Int64 strPatientId = MDVUtility.ToInt64(context.Request["PatientId"]);
                        Int64 strAppointmentId = MDVUtility.ToInt64(context.Request["AppointmentId"]);
                        if (strPatientId == 0)
                            strPatientId = -1;
                        string strJSONData = FillPatientAppointment(strPatientId, strAppointmentId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "CANCEL_PATIENT_CHECKIN":
                    {
                        string VisitId = context.Request["VisitId"];
                        Int64 AppointmentId = MDVUtility.ToInt64(context.Request["AppointmentId"]);
                        Int64 StatusId = MDVUtility.ToInt64(context.Request["StatusId"]);
                        string Status = context.Request["Status"];
                        string strJSONData = CancelPatientCheckIn(MDVUtility.ToInt64(VisitId), AppointmentId, StatusId, Status);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "LOAD_TEMPLATE_LOOKUP":
                    {
                        Int64 specialityId = MDVUtility.ToInt64(context.Request["SpecialityId"]);
                        Int64 providerId = MDVUtility.ToInt64(context.Request["ProviderId"]);
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJsonData = LoadTempalateLookup(specialityId, providerId, rpp, pageNo);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }
        #endregion

    }
}