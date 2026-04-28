using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Clinical.Notes;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_CheckOut
    {
        private BLLPatient BLLPatientObj = null;
        private BLLBilling BLLBillingObj = null;
        private BLLSchedule BLLScheduleObj = null;
        private BLLVisits BLLVisitsObj = null;
        public Scheduling_CheckOut()
        {

            BLLPatientObj = new BLLPatient();
            BLLBillingObj = new BLLBilling();
            BLLScheduleObj = new BLLSchedule();
            BLLVisitsObj = new BLLVisits();
        }

        #region Singleton
        private static Scheduling_CheckOut _obj = null;
        public static Scheduling_CheckOut Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_CheckOut();
            return _obj;
        }
        #endregion

        #region "Private Functions"

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
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatientAndInsuranceById(PatientId, AppointmentId, 0, "AppointmentCheckOut");
                    dsPatient = obj.Data;

                    DSPayment dsPatientPayment = null;
                    BLObject<DSPayment> obj1 = BLLBillingObj.LoadPatientPayments(0, AppointmentId, 0, 0, "CheckOut");
                    dsPatientPayment = obj1.Data;

                    BLLClinical BLLClinicalObj = new BLLClinical();
                    NoteComponentModel model = new NoteComponentModel();
                    BLObject<List<NoteComponentModel>> Noteobj = BLLClinicalObj.loadNoteFollowUpComponent(AppointmentId);
                    if (Noteobj.Data != null && Noteobj.Data.Count > 0)
                        model = Noteobj.Data[0];

                    //DSAppointment dsSchedule = null;
                    //BLObject<DSAppointment> objSchedule =BLLScheduleObj.LoadPatientBalance(PatientId);
                    //dsSchedule = objSchedule.Data;

                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                        var keyValues = new Dictionary<string, string>
                        {
                            { "hfAppointmentId", MDVUtility.ToStr(AppointmentId)},
                            { "txtPatientName", MDVUtility.ToStr(dr[dsPatient.Patients.FullNameColumn.ColumnName])},
                            { "txtDOB", MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "hfpatientid", MDVUtility.ToStr(dr[dsPatient.Patients.PatientIdColumn.ColumnName])},

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
                            CheckoutAppointment_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsApp.AppointmentCheckout.TableName]),
                            PatientBalance_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                            PatientAppointment_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsApp.PatientAppointments.TableName]),
                            PatientInsuranceDetail_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientInsurance.TableName]),
                            PatientCopayment_JSON = MDVUtility.JSON_DataTable(dsPatientPayment.Tables[dsPatientPayment.PatientPayments.TableName]),
                            NotesFollowUp_Count = Noteobj.Data.Count,
                            NotesFollowUp_JSON = js.Serialize(model),
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
        private string SavePatientCheckOut(string fieldsJSON, Int64 VisitId, string referralId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJSON);

                DSVisits dsVisits = null;
                BLObject<DSVisits> obj = null;
                obj = BLLVisitsObj.LoadPatientsVisits(VisitId, MDVUtility.ToInt64(searchedfieldsJson["hfpatientid"]), 0, 0, null, null, "", "", "");
                dsVisits = obj.Data;
                if (dsVisits.Tables[dsVisits.PatientVisits.TableName].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsVisits.Tables[dsVisits.PatientVisits.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("ddlInsurancePlan"))
                            dr[dsVisits.PatientVisits.PatientInsuranceIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["ddlInsurancePlan"]);

                        if (searchedfieldsJson.ContainsKey("hfRefProvider"))
                            dr[dsVisits.PatientVisits.RefProviderIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["hfRefProvider"]);

                        if (searchedfieldsJson.ContainsKey("txtCopayment"))
                            dr[dsVisits.PatientVisits.VisitCopaymentColumn] = MDVUtility.Tofloat(searchedfieldsJson["txtCopayment"]);

                        dr[dsVisits.PatientVisits.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsVisits.PatientVisits.ModifiedOnColumn] = DateTime.Now;

                        dr[dsVisits.PatientVisits.VisitStatusColumn] = MDVUtility.ToStr("CheckOut");
                    }

                    dbManager.BeginTransaction();
                    BLObject<DSVisits> objType = null;
                    objType = BLLVisitsObj.UpdatePatientsVisitWithTran(dsVisits, dbManager);

                    if (objType.Data != null)
                    {
                        //******Appointment Status Update Region**************\\

                        DSAppointment dsAppointment = null;
                        BLObject<DSAppointment> objAppointment = BLLScheduleObj.LoadAppointment(MDVUtility.ToInt64(searchedfieldsJson["hfAppointmentId"]), null, null, null, null);
                        dsAppointment = objAppointment.Data;

                        if (dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow drAppointment in dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows)
                            {
                                drAppointment[dsAppointment.PatientAppointments.SchStatusIdColumn] = MDVUtility.ToInt64(0);
                                drAppointment[dsAppointment.PatientAppointments.AppointmentStatusColumn] = MDVUtility.ToStr("Check Out");

                                if (searchedfieldsJson["rdPCP"] == true)
                                {
                                    drAppointment[dsAppointment.PatientAppointments.IsSpecialistColumn] = false;
                                }
                                else if (searchedfieldsJson["rdSpecialist"] == true)
                                {
                                    drAppointment[dsAppointment.PatientAppointments.IsSpecialistColumn] = true;
                                }

                                drAppointment[dsAppointment.PatientAppointments.PatientInsuranceIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["ddlInsurancePlan"]);
                                //Start 22-08-2016 Humaira Yousaf for referral Id
                                if (!string.IsNullOrEmpty(referralId))
                                {
                                    drAppointment[dsAppointment.PatientAppointments.ReferralIdColumn] = MDVUtility.ToInt64(referralId);
                                }
                                if (searchedfieldsJson.ContainsKey("hfRefProvider"))
                                    drAppointment[dsAppointment.PatientAppointments.RefProviderIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["hfRefProvider"]);


                                //End 22-08-2016 Humaira Yousaf for referral Id
                                drAppointment[dsAppointment.PatientAppointments.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                drAppointment[dsAppointment.PatientAppointments.ModifiedOnColumn] = DateTime.Now;
                            }
                        }

                        BLObject<DSAppointment> objTypeAppointment = null;
                        objTypeAppointment = BLLScheduleObj.UpdateAppointmentWithTran(dsAppointment, dbManager);
                        //*********End Appointment Status Update***********\\

                        if (objTypeAppointment.Data != null)
                        {
                            dbManager.CommitTransaction();
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            dbManager.RollBackTransaction();
                            var response = new
                            {
                                status = false,
                                Message = objTypeAppointment.Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        dbManager.RollBackTransaction();
                        var response = new
                        {
                            status = false,
                            Message = objType.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Encounter Linked with this Appointment is deleted"
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Module Name: UpdateAppointmentReferral
        /// Author: Humaira Yousaf
        /// Created Date: 05-10-2016
        /// Description: Updates Appointment for Referral Id
        /// </summary> 
        private string UpdateAppointmentReferral(Int64 appointmentId, Int64 referralId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSAppointment dsAppointment = null;
                BLObject<DSAppointment> objTypeAppointment = null;

                BLObject<DSAppointment> objAppointment = BLLScheduleObj.LoadAppointment(appointmentId, null, null, null, null);
                dsAppointment = objAppointment.Data;

                if (dsAppointment != null && dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows.Count > 0)
                {
                    objTypeAppointment = BLLScheduleObj.UpdateAppointmentReferral(appointmentId, referralId);

                    if (objTypeAppointment != null)
                    {
                        var response = new
                        {
                            status = true,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objTypeAppointment.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objAppointment.Message
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

                case "SAVE_PATIENT_CHECKOUT":
                    {
                        string fieldsJSON = context.Request["CheckOutData"];
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        string referralId = context.Request["ReferralId"];
                        string strJSONData = SavePatientCheckOut(fieldsJSON, VisitId, referralId);



                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_APPOINTMENT_REFERRAL":
                    {
                        Int64 appointmentId = MDVUtility.ToInt64(context.Request["AppointmentId"]);
                        Int64 referralId = MDVUtility.ToInt64(context.Request["ReferralId"]);
                        string strJSONData = UpdateAppointmentReferral(appointmentId, referralId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}