using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Common;
using System.Data.Entity;
using System.Configuration;
using WinSCP;
using SFTP = MDVision.Business.BCommon.SFTP.SFTP;
using System.IO;
using System.Reflection;
using MDVision.IEHR.Controls.Admin;
using System.Text;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using static MDVision.IEHR.Common.MDVisionLookups;
using System.Threading;
using MDVision.Model.Clinical.Medical.ProblemLists;
using MDVision.IEHR.Controls.Clinical;
using MDVision.Common.Logging;
using MDVision.Model.PMSSchedule;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_Appointment_Detail
    {
        private BLLPatient BLLPatientObj = null;
        private BLLMessage BLLMessageObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        private BLLSchedule BLLScheduleObj = null;

        public Scheduling_Appointment_Detail()
        {
            BLLPatientObj = new BLLPatient();
            BLLMessageObj = new BLLMessage();
            BLLAdminProfileObj = new BLLAdminProfile();
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Scheduling_Appointment_Detail _obj = null;

        public static Scheduling_Appointment_Detail Instance()
        {
            if (_obj == null)
            {
                _obj = new Scheduling_Appointment_Detail();

            }
            return _obj;
        }
        #endregion



        #region Private Functions

        private string FillPatient(Int64 PatientId)
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
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatientAndInsuranceById(PatientId, 0, 0, "Appointment");
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;

                        //DSAppointment dsSchedule = null;
                        //BLObject<DSAppointment> obj1 =BLLScheduleObj.LoadPatientBalance(PatientId);
                        //dsSchedule = obj1.Data;

                        if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtAccountNo", MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName])},
                            { "txtFullName", MDVUtility.ToStr(dr[dsPatient.Patients.FullNameColumn.ColumnName])},
                            //{ "ddlReferringProvider", MDVUtility.ToStr(dr[dsPatient.Patients.ReferringProviderIdColumn.ColumnName])},
                            //{ "ddlInsurancePlan", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName])},
                            { "dtpDOB", MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "hfpatientid", MDVUtility.ToStr(dr[dsPatient.Patients.PatientIdColumn.ColumnName])},
                            {"hfIsPatientActive", MDVUtility.ToStr(dr[dsPatient.Patients.IsActiveColumn.ColumnName])},
                            //{ "txtPatientBalance", MDVUtility.ToStr(dr[dsSchedule.PatientBalance.TotalBalanceColumn.ColumnName])},
                          
                        };

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientFill_JSON = js.Serialize(keyValues),
                                PatientBalance_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                                PatientInsurance_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientInsurance.TableName]),
                                Patient_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName])
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string FillReasonDuration(Int64 ScheduleReasonId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ScheduleReasonId)))
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
                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadScheduleReasonDuration(ScheduleReasonId);
                    if (obj.Data != null)
                    {
                        dsSchedule = obj.Data;
                        if (dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                           {"Duration", MDVUtility.ToStr(dr[dsSchedule.ScheduleReasons.DurationColumn.ColumnName])},

                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ProviderScheduleFill_JSON = js.Serialize(keyValues)
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        //private string FillCopayment(Int64 InsurancePlanId)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(MDVUtility.ToStr(InsurancePlanId)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {
        //            DSInsurance dsInsurance = null;
        //            BLObject<DSInsurance> obj = BLLAdminInsuranceObj.LoadInsurancePlan(InsurancePlanId,null,null,null,null,null,null,null);
        //            dsInsurance = obj.Data;
        //            if (dsInsurance.Tables[dsInsurance.InsurancePlan.TableName].Rows.Count > 0)
        //            {
        //                DataRow dr = dsInsurance.Tables[dsInsurance.InsurancePlan.TableName].Rows[0];
        //                var keyValues = new Dictionary<string, string>
        //                {
        //                   {"CoPayment", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.CoPaymentColumn.ColumnName])},

        //                };
        //                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //                var response = new
        //                {
        //                    status = true,
        //                    CopaymentFill_JSON = js.Serialize(keyValues)
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        public string SearchPatients(string AccountNo)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj;
                obj = BLLPatientObj.LookupPatient(0, AccountNo, "1");
                dsPatient = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count,
                            PatientLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string SearchPatientsByName(string SearchString)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj;
                obj = BLLPatientObj.LookupPatientByName(0, SearchString, "1");
                dsPatient = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count,
                            PatientLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string CheckifCancelledAppointmentExists(string fieldsJSON)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
            long PatientId = 0;
            long ProviderId = 0;
            long ResourceId = 0;
            long FacilityId = 0;
            string AppointmentDateColumn = "";
            string TimeFromColumn = "";
            string TimeToColumn = "";
            PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfpatientid"]);
            if (SearchedfieldsJSON.ContainsKey("hfProviderId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfProviderId"]))
            {
                ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProviderId"]);
            }
            if (SearchedfieldsJSON.ContainsKey("hfResourceId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfResourceId"]))
            {
                ResourceId = MDVUtility.ToInt64(SearchedfieldsJSON["hfResourceId"]);
            }
            FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacilityId"]);
            AppointmentDateColumn = MDVUtility.ToStr(SearchedfieldsJSON["txtScheduleDate"]);
            TimeFromColumn = MDVUtility.ToStr(SearchedfieldsJSON["txtFromTime"]);
            TimeToColumn = MDVUtility.ToStr(SearchedfieldsJSON["txtToTime"]);
            var IfExists = BLLScheduleObj.CheckifCancelledAppointmentExists(PatientId, FacilityId, 
                AppointmentDateColumn, TimeFromColumn, TimeToColumn, ProviderId, ResourceId);
            if (IfExists)
            {
                var response = new {
                    status = true,
                    CancelledAppointmentExists = true,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new {
                    status = true,
                    CancelledAppointmentExists = false
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            
        }
        private string SaveAppointment(string fieldsJSON, string fieldsJSON1, string referralId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                var SearchedfieldsJSON1 = ser.Deserialize<dynamic>(fieldsJSON);
                if (fieldsJSON1 != null)
                {
                    SearchedfieldsJSON1 = ser.Deserialize<dynamic>(fieldsJSON1);
                }

                DSAppointment dsAppointment = new DSAppointment();
                DSAppointment.PatientAppointmentsRow dr = dsAppointment.PatientAppointments.NewPatientAppointmentsRow();
                if (SearchedfieldsJSON.ContainsKey("FromFollowUp") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["FromFollowUp"])))
                {
                    BLObject<DSScheduleLookups> objVisitType = new BLLSchedule().LookupPatientVisitType(MDVUtility.ToStr(SearchedfieldsJSON["hfProviderId"]));
                    DSScheduleLookups ds = objVisitType.Data;

                    DataRow[] dRows = ds.Tables[ds.PatientVisitType.TableName].Select(ds.PatientVisitType.PatientTypeColumn.ColumnName.ToString() + "='Established Patient' AND "
                        + ds.PatientVisitType.VisitTypeColumn.ColumnName.ToString() + " like '%Follow Up%'");
                    
                    if (dRows != null && dRows.Count() > 0)
                    {
                        dr.VisitTypeID = MDVUtility.ToInt64(dRows[0][ds.PatientVisitType.VisitTypeIDColumn.ColumnName]);
                    }
                }

                    dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfpatientid"]);
                if (SearchedfieldsJSON.ContainsKey("txtFacility") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtFacility"]))
                {
                    dr.FacilityName = MDVUtility.ToStr(SearchedfieldsJSON["txtFacility"]);
                }
                if (SearchedfieldsJSON.ContainsKey("txtFullName") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtFullName"]))
                {
                    dr.PatientName = MDVUtility.ToStr(SearchedfieldsJSON["txtFullName"]);
                }
                if (SearchedfieldsJSON.ContainsKey("txtProvider") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtProvider"]))
                {
                    dr.ProviderName = MDVUtility.ToStr(SearchedfieldsJSON["txtProvider"]);
                }
                if (SearchedfieldsJSON.ContainsKey("FollowUpAppointmentNotesId") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["FollowUpAppointmentNotesId"])))
                {
                    dr.NotesId = MDVUtility.ToInt64(SearchedfieldsJSON["FollowUpAppointmentNotesId"]);
                }
                if (SearchedfieldsJSON.ContainsKey("hfProviderId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfProviderId"]))
                {
                    dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProviderId"]);
                }
                if (SearchedfieldsJSON.ContainsKey("hfResourceId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfResourceId"]))
                {
                    dr.ResourceId = MDVUtility.ToInt64(SearchedfieldsJSON["hfResourceId"]);
                }
                dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacilityId"]);

                if (SearchedfieldsJSON.ContainsKey("hfRefProvider") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfRefProvider"]))
                {
                    dr.RefProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfRefProvider"]);
                }

                if (SearchedfieldsJSON.ContainsKey("ddlInsurancePlan") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlInsurancePlan"]))
                {
                    dr.PatientInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlInsurancePlan"]);
                }
                if (SearchedfieldsJSON.ContainsKey("CancellationReason") && !string.IsNullOrEmpty(SearchedfieldsJSON["CancellationReason"]))
                {
                    dr.CancellationReason = MDVUtility.ToStr(SearchedfieldsJSON["CancellationReason"]);
                }

                //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSchReasonId"]))
                //{
                //    dr.SchReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["hfSchReasonId"]);
                //}
                //dr.SchReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlReason"]);
                dr[dsAppointment.PatientAppointments.SchReasonIdColumn] = DBNull.Value;
                if (SearchedfieldsJSON.ContainsKey("ddlStatus") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlStatus"]))
                {
                    dr.SchStatusId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlStatus"]);
                }
                //dr.SchStatusId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlStatus"]);
                if (SearchedfieldsJSON.ContainsKey("hfSlottimeid") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["hfSlottimeid"])))
                    dr.TimeSlotId = Convert.ToInt32(MDVUtility.ToStr(SearchedfieldsJSON["hfSlottimeid"]));
                else
                    dr.TimeSlotId = 0;
                if (SearchedfieldsJSON.ContainsKey("hfSlottimedtlid") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["hfSlottimedtlid"])))
                    dr.TimeSlotDtlId = Convert.ToInt32(MDVUtility.ToStr(SearchedfieldsJSON["hfSlottimedtlid"]));
                else
                    dr.TimeSlotDtlId = 0;

                if (SearchedfieldsJSON.ContainsKey("Duration") && !string.IsNullOrEmpty(SearchedfieldsJSON["Duration"]))
                {
                    dr.Duration = Convert.ToInt32(MDVUtility.ToStr(SearchedfieldsJSON["Duration"]));
                }
                if (SearchedfieldsJSON.ContainsKey("txtReferralNo") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtReferralNo"]))
                {
                    dr.ReferralNo = MDVUtility.ToStr(SearchedfieldsJSON["txtReferralNo"]);
                }

                if (SearchedfieldsJSON.ContainsKey("hfReferralNumerId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfReferralNumerId"]))
                {
                    dr.PatientReferralId = MDVUtility.ToInt64(SearchedfieldsJSON["hfReferralNumerId"]);
                }
                              
                if (SearchedfieldsJSON.ContainsKey("txtPatientBalance") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientBalance"]))
                {
                    dr.PatientBalance = MDVUtility.ToDouble(SearchedfieldsJSON["txtPatientBalance"]);
                }
                if (SearchedfieldsJSON.ContainsKey("txtComments") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                {
                    dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                }


                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.IsReminderSent = false;
                if (SearchedfieldsJSON.ContainsKey("chkcopayalert"))
                {
                    bool showCopayAlert = Convert.ToBoolean(SearchedfieldsJSON["chkcopayalert"]);
                    if (showCopayAlert)
                        dr.IsCopayAlert = true;
                    else
                        dr.IsCopayAlert = false;
                }
                if (SearchedfieldsJSON.ContainsKey("rdPCP") && SearchedfieldsJSON["rdPCP"] == true)
                {
                    dr.IsSpecialist = false;
                }
                else if (SearchedfieldsJSON.ContainsKey("rdSpecialist") && SearchedfieldsJSON["rdSpecialist"] == true)
                {
                    dr.IsSpecialist = true;
                }

                if (fieldsJSON1 != null)
                {
                    SearchedfieldsJSON1 = ser.Deserialize<dynamic>(fieldsJSON1);
                    if (SearchedfieldsJSON1["pattern"] == "Daily")
                    {

                        if (SearchedfieldsJSON1["Every"] == true && SearchedfieldsJSON1["rdEveryWeekdays"] == false)
                        {
                            dr.PatternEvery = true;
                            dr.Value = MDVUtility.ToStr(SearchedfieldsJSON1["txtevrydays"]);
                            dr.PatternDays = "0";
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = "0";
                        }
                        if (SearchedfieldsJSON1["Every"] == true && SearchedfieldsJSON1["rdEveryWeekdays"] == true)
                        {
                            dr.PatternEvery = true;
                            dr.Value = MDVUtility.ToStr(SearchedfieldsJSON1["txtevrydays"]);
                            dr.PatternDays = "11111";
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = "0";
                        }
                        if (SearchedfieldsJSON1["rdEveryWeekdays"] == true)
                        {
                            dr.PatternEvery = false;
                            dr.Value = "0";
                            dr.PatternDays = "0";
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = "0";
                        }
                        if (SearchedfieldsJSON1["rdEveryEndByDate"] == true)
                        {
                            dr.EndByDate = MDVUtility.ToDateTime(SearchedfieldsJSON1["dailyEndDate"]);
                        }
                        if (SearchedfieldsJSON1["rdEveryEndByAppointment"] == true)
                        {
                            dr.EndByAppointment = MDVUtility.ToStr(SearchedfieldsJSON1["txtDailyAppointment"]);
                        }
                    }
                    if (SearchedfieldsJSON1["pattern"] == "Weekly")
                    {
                        if (SearchedfieldsJSON1["rdEveryWeekdayOn"] == true)
                        {
                            dr.PatternEvery = false;
                            dr.Value = "0";
                            dr.PatternDays = (SearchedfieldsJSON1["chkwekSunday"] + "," + SearchedfieldsJSON1["chkwekMonday"] + "," + SearchedfieldsJSON1["chkwekTuesday"] + "," + SearchedfieldsJSON1["chkwekWednesday"] + "," + SearchedfieldsJSON1["chkwekThursday"] + "," + SearchedfieldsJSON1["chkwekFriday"] + "," + SearchedfieldsJSON1["chkwekSaturday"]).Replace("True", "1").Replace("False", "0");
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = "0";
                        }
                        if (SearchedfieldsJSON1["rdEveryweekely"] == true)
                        {
                            dr.PatternEvery = true;
                            dr.Value = MDVUtility.ToStr(SearchedfieldsJSON1["txtactiveweek"]);
                            dr.PatternDays = (SearchedfieldsJSON1["chkwekSunday"] + "," + SearchedfieldsJSON1["chkwekMonday"] + "," + SearchedfieldsJSON1["chkwekTuesday"] + "," + SearchedfieldsJSON1["chkwekWednesday"] + "," + SearchedfieldsJSON1["chkwekThursday"] + "," + SearchedfieldsJSON1["chkwekFriday"] + "," + SearchedfieldsJSON1["chkwekSaturday"]).Replace("True", "1").Replace("False", "0");
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = "0";
                        }
                        if (SearchedfieldsJSON1["rdWeeklyEndByDate"] == true)
                        {
                            dr.EndByDate = MDVUtility.ToDateTime(SearchedfieldsJSON1["weeklyEndDate"]);
                        }
                        if (SearchedfieldsJSON1["rdWeeklyEndByAppointment"] == true)
                        {
                            dr.EndByAppointment = MDVUtility.ToStr(SearchedfieldsJSON1["txtWeeklyAppointment"]);
                        }
                    }
                    if (SearchedfieldsJSON1["pattern"] == "Monthly")
                    {
                        if (SearchedfieldsJSON1["rdmntdaay"] == true)
                        {
                            dr.PatternEvery = false;
                            dr.Value = MDVUtility.ToStr(SearchedfieldsJSON1["txtmntActive"]);
                            dr.PatternDays = "0";
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = MDVUtility.ToStr(SearchedfieldsJSON1["txtmntofevry"]);
                        }
                        if (SearchedfieldsJSON1["rdmntthe"] == true)
                        {
                            dr.PatternEvery = true;
                            dr.Value = "0";
                            dr.PatternDays = (SearchedfieldsJSON1["chkmntSunday"] + "," + SearchedfieldsJSON1["chkmntMonday"] + "," + SearchedfieldsJSON1["chkmntTuesday"] + "," + SearchedfieldsJSON1["chkmntWednesday"] + "," + SearchedfieldsJSON1["chkmntThursday"] + "," + SearchedfieldsJSON1["chkmntFriday"] + "," + SearchedfieldsJSON1["chkmntSaturday"]).Replace("True", "1").Replace("False", "0");
                            dr.PatternWeeks = (SearchedfieldsJSON1["chkmntFirst"] + "," + SearchedfieldsJSON1["chkmntSecond"] + "," + SearchedfieldsJSON1["chkmntThird"] + "," + SearchedfieldsJSON1["chkmntFourth"] + "," + SearchedfieldsJSON1["chkmntLast"]).Replace("True", "1").Replace("False", "0");
                            dr.PatternMonths = MDVUtility.ToStr(SearchedfieldsJSON1["txtmnttheofevery"]);
                        }
                        if (SearchedfieldsJSON1["rdMonthlyEndByDate"] == true)
                        {
                            dr.EndByDate = MDVUtility.ToDateTime(SearchedfieldsJSON1["monthlyEndDate"]);
                        }
                        if (SearchedfieldsJSON1["rdMonthlyEndByAppointment"] == true)
                        {
                            dr.EndByAppointment = MDVUtility.ToStr(SearchedfieldsJSON1["txtMonthlyAppointment"]);
                        }
                    }
                  
                }
                if (SearchedfieldsJSON.ContainsKey("hfWaitListId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfWaitListId"]))
                {
                    dr.WaitListId = MDVUtility.ToInt64(SearchedfieldsJSON["hfWaitListId"]);
                }

                if (SearchedfieldsJSON.ContainsKey("ddlPatientType") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPatientType"]))
                {
                    dr.PatientTypeID = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPatientType"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlVisitType") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlVisitType"]))
                {
                    dr.VisitTypeID = MDVUtility.ToInt64(SearchedfieldsJSON["ddlVisitType"]);
                }
               
                //Start 22-08-2016 Humaira Yousaf for referral id
                if (!string.IsNullOrEmpty(referralId))
                {
                    dr.ReferralId = MDVUtility.ToInt64(referralId);
                }
                //End 22-08-2016 Humaira Yousaf for referral id
                if (SearchedfieldsJSON.ContainsKey("CoPayment") && !string.IsNullOrEmpty(SearchedfieldsJSON["CoPayment"]))
                {
                    dr.Copayment = MDVUtility.Tofloat(SearchedfieldsJSON["CoPayment"]);
                }

                if (SearchedfieldsJSON.ContainsKey("chkNonBilable") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkNonBilable"])))
                    dr.IsNonBilable = MDVUtility.ToStr(SearchedfieldsJSON["chkNonBilable"]) == "True" ? true : false;
                else
                    dr[dsAppointment.PatientAppointments.IsNonBilableColumn] = DBNull.Value;

                if (SearchedfieldsJSON.ContainsKey("FromFollowUp") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["FromFollowUp"])))
                    dr.FromFollowUp = MDVUtility.ToStr(SearchedfieldsJSON["FromFollowUp"]) == "True" ? true : false;
                else
                    dr[dsAppointment.PatientAppointments.FromFollowUpColumn] = DBNull.Value;

                if (SearchedfieldsJSON.ContainsKey("txtReasonComments") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtReasonComments"]))
                    dr.ReasonComments = MDVUtility.ToStr(SearchedfieldsJSON["txtReasonComments"]);

                if (SearchedfieldsJSON.ContainsKey("ReasonCommentType"))
                    dr.ReasonCommentsTypeName = MDVUtility.ToStr(SearchedfieldsJSON["ReasonCommentType"]);

                if (SearchedfieldsJSON.ContainsKey("hfICDCode10") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfICDCode10"]))
                    dr.ICDCode10 = MDVUtility.ToStr(SearchedfieldsJSON["hfICDCode10"]);
                else
                    dr[dsAppointment.PatientAppointments.ICDCode10Column] = DBNull.Value;

                if (SearchedfieldsJSON.ContainsKey("hfICDDescription10") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfICDDescription10"]))
                    dr.ICDCode10Description = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription10"]);
                else
                    dr[dsAppointment.PatientAppointments.ICDCode10DescriptionColumn] = DBNull.Value;

                if (SearchedfieldsJSON.ContainsKey("hfSNOMEDCode") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfSNOMEDCode"]))
                    dr.SnomedCode = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDCode"]);
                else
                    dr[dsAppointment.PatientAppointments.SnomedCodeColumn] = DBNull.Value;

                if (SearchedfieldsJSON.ContainsKey("hfSNOMEDDescription") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfSNOMEDDescription"]))
                    dr.SnomedCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription"]);
                else
                    dr[dsAppointment.PatientAppointments.SnomedCodeDescriptionColumn] = DBNull.Value;

                //Start: Author: Abdur Rehman, Date: Nov 27th, 2017, Issue: PMS-2519, Comments: Added on Usman Akram's Request For Appointment Procedure changes
                dr[dsAppointment.PatientAppointments.AppointmentDateColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtScheduleDate"]);
                dr[dsAppointment.PatientAppointments.TimeFromColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtFromTime"]);
                dr[dsAppointment.PatientAppointments.TimeToColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtToTime"]);
                //End: Author: Abdur Rehman, Date: Nov 27th, 2017, Issue: PMS-2519, Comments: Added on Usman Akram's Request For Appointment Procedure changes
                #region Database Insertion
                dsAppointment.PatientAppointments.AddPatientAppointmentsRow(dr);
                BLObject<DSAppointment> obj = BLLScheduleObj.InsertAppointment(dsAppointment);
                dsAppointment = obj.Data;

                #region SIU Message

                //long patientId = 0;
                //if (SearchedfieldsJSON.ContainsKey("hfpatientid")) { patientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfpatientid"]); }

                //long appointmentId = 0;
                //if (dsAppointment != null)
                //{
                //    if (dsAppointment.PatientAppointments.Rows.Count > 0)
                //    {
                //        appointmentId =
                //        long.Parse(
                //            dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0][
                //                dsAppointment.PatientAppointments.AppointmentIdColumn.ColumnName].ToString());
                //    }
                //}

                //if (patientId >= 0 && appointmentId >= 0)
                //{
                //    CreateSiuMessage(patientId, appointmentId);
                //}

                #endregion

                if (obj.Data != null)
                {
                    Int64 AppointmentId = MDVUtility.ToInt64(dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0][dsAppointment.PatientAppointments.AppointmentIdColumn.ColumnName]);

                    #region Add Problem On Dr. First
                    if (MDVUtility.ToStr(dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0][dsAppointment.PatientAppointments.ReasonCommentsTypeNameColumn.ColumnName]) == "ICD")
                        AddProblemOnDrFirst(AppointmentId, ref dsAppointment);
                    #endregion

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProviderId"]))
                    {
                        Int64 ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProviderId"]);
                        // Send Preference Base Reminder to Patient.
                        //BLLAdminObj.SendPreferenceBasedReminders(AppointmentId);

                    }
                    #region "Portal Appointment Request"

                    if (SearchedfieldsJSON.ContainsKey("isRequestFromPortal") && SearchedfieldsJSON["isRequestFromPortal"] == "1")
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["PortalAppRequestId"]))
                        {
                            DSAppointment dsAppRequest = new DSAppointment();
                            BLObject<DSAppointment> objLoad_AppRequest = BLLScheduleObj.LoadPortalAppRequest(MDVUtility.ToInt64(SearchedfieldsJSON["PortalAppRequestId"]), "", "");
                            dsAppRequest = objLoad_AppRequest.Data;

                            foreach (DSAppointment.PortalAppRequestRow dr_AppRequest in dsAppRequest.Tables[dsAppRequest.PortalAppRequest.TableName].Rows)
                            {
                                dr_AppRequest.AppointmentId = AppointmentId;
                                dr_AppRequest.RequestStatus = "Booked";
                                dr_AppRequest.ModifiedOn = DateTime.Now;
                            }
                            BLObject<DSAppointment> obj_AppRequest = BLLScheduleObj.UpdatePortalAppRequest(dsAppRequest);
                            string pracId = SearchedfieldsJSON["hfPracticeId"];
                            string facId = SearchedfieldsJSON["hfFacilityId"];
                            string appDate = SearchedfieldsJSON["hfPortalSchDate"];
                            string status = "Scheduled";
                            string provName = SearchedfieldsJSON["hfProviderName"];
                            string patId = SearchedfieldsJSON["hfpatientid"];
                            string patName = SearchedfieldsJSON["hfPatientFirstLastName"];

                            SavePatientMessage(pracId, facId, appDate, status, provName, patId, patName);
                        }
                    }

                    #endregion
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        AppointmentDetail = MDVUtility.JSON_DataTable(dsAppointment.Tables[dsAppointment.PatientAppointments.TableName]),
                        AppointmentId = dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0][dsAppointment.PatientAppointments.AppointmentIdColumn.ColumnName]
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private void AddProblemOnDrFirst(Int64 AppointmentId, ref DSAppointment dsAppointment)
        {
            string PatientId = MDVUtility.ToStr(dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0][dsAppointment.PatientAppointments.PatientIdColumn.ColumnName]);
            BLObject<DSAppointment> obj1 = BLLScheduleObj.LoadAppointment(AppointmentId, PatientId, null, null, null);
            DSAppointment dsapp = new DSAppointment();
            if (obj1.Data != null)
            {
                dsapp = obj1.Data;
                if (dsapp.Tables[dsapp.PatientAppointments.TableName].Rows.Count > 0)
                {
                    Int64 ProblemListId = MDVUtility.ToInt64(dsapp.Tables[dsapp.PatientAppointments.TableName].Rows[0][dsapp.PatientAppointments.ProblemListIdColumn.ColumnName]);
                    if (ProblemListId > 0)
                    {
                        DSProblemLists dsProb = new DSProblemLists();
                        DSProblemLists.ProblemListRow row = dsProb.ProblemList.NewProblemListRow();
                        row["PatientId"] = MDVUtility.ToInt64(PatientId);
                        row["ProblemListId"] = ProblemListId;
                        row["StartDate"] = DateTime.Now;
                        row["ICD10"] = MDVUtility.ToStr(dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0][dsAppointment.PatientAppointments.ICDCode10Column.ColumnName]);
                        row["ICD10_Description"] = MDVUtility.ToStr(dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0][dsAppointment.PatientAppointments.ICDCode10DescriptionColumn.ColumnName]);
                        row["Description"] = MDVUtility.ToStr(dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0][dsAppointment.PatientAppointments.ReasonCommentsColumn.ColumnName]);
                        string AppUserName = MDVSession.Current.AppUserName;
                        SharedVariable sharedVariable = SharedVariable.GetSharedVariable();
                        HttpContext hCtrl = HttpContext.Current;
                        Thread thread = new Thread(new ThreadStart(delegate ()
                        {
                            try
                            {
                                ProblemListModel ProModel = new ProblemListModel();
                                ProModel.ProblemListId = MDVUtility.ToStr(ProblemListId);
                                ProblemListHelper probHelp = new ProblemListHelper();
                                probHelp.SaveProblemOnDrFirst(ProModel, row, AppUserName, sharedVariable, hCtrl);
                            }
                            catch (Exception ex)
                            {
                                MDVLogger.SendExcepToDB(ex, "AddProblemList On DrFirst", null);
                            }
                        }));
                        thread.IsBackground = true;
                        thread.Start();
                    }
                }
            }
        }
        private void CreateSiuMessage(long patientId, long appointmentId)
        {
            #region Configuration

            //var winScpPath = AppConfig.WebConfig_AppSettings.WinSCP.WinSCP_WinSCP_Path;

            //var docAsapUploadPath = AppConfig.WebConfig_AppSettings.DocASAP.DocASAP_SFTP_UploadPath;
            //var sftpRemoteHost = AppConfig.WebConfig_AppSettings.DocASAP.DocASAP_SFTP_RemoteHostName;
            //var sftpRemoteUser = AppConfig.WebConfig_AppSettings.DocASAP.DocASAP_SFTP_RemoteUserName;
            //var sftpRemotePassword = AppConfig.WebConfig_AppSettings.DocASAP.DocASAP_SFTP_RemoteUserPassword;
            //var sftpRemotePort = AppConfig.WebConfig_AppSettings.DocASAP.DocASAP_SFTP_RemotePort;
            //var sftpRemoteHostKey = AppConfig.WebConfig_AppSettings.DocASAP.DocASAP_SFTP_RemoteHostKey;

            var winScpPath = ConfigurationManager.AppSettings["WinSCP.WinSCP_Path"];
            var docAsapUploadPath = ConfigurationManager.AppSettings["DocASAP.SFTP_File_UploadPath"];

            var sftpRemoteHost = ConfigurationManager.AppSettings["DocASAP.SFTP_RemoteHostName"];
            var sftpRemoteUser = ConfigurationManager.AppSettings["DocASAP.SFTP_RemoteUserName"];
            var sftpRemotePassword = ConfigurationManager.AppSettings["DocASAP.SFTP_RemoteUserPassword"];
            var sftpRemotePort = ConfigurationManager.AppSettings["DocASAP.SFTP_RemotePort"];
            var sftpRemoteHostKey = ConfigurationManager.AppSettings["DocASAP.SFTP_RemoteHostKey"];

            #endregion

            var segmentsList = new List<string>();

            SFTP.clsSFTP clsSftp = new SFTP.clsSFTP(sftpRemoteHost, sftpRemoteUser, sftpRemotePassword, sftpRemotePort, sftpRemoteHostKey);

            bool status = false;
            status = clsSftp.UserLoginAuthorization_SFTP(sftpRemoteHost, sftpRemoteUser, sftpRemotePassword, sftpRemotePort, sftpRemoteHostKey, false);

            if (status)
            {
                DSHL7 dsHl7 = null;

                if (patientId >= 0 && appointmentId >= 0)
                {
                    BLObject<DSHL7> objHl7 = null;// AdminHL7.BusinessObj.Load_Patient_Provider_Appointment_Visit(patientId, appointmentId);

                    dsHl7 = objHl7.Data;

                    if (objHl7.Data != null)
                    {
                        //HL7.HL7Parser hl7ObjectToMessage = new HL7.HL7Parser(dsHl7.Tables[dsHl7.CreateSIU.TableName]);

                        //if (dsHl7.Tables[dsHl7.CreateSIU.TableName] != null) {

                        //    if (dsHl7.Tables[dsHl7.CreateSIU.TableName].Rows.Count > 0) {

                        //        foreach (DataRow dr_ in dsHl7.Tables[dsHl7.CreateSIU.TableName].Rows) {

                        //            segmentsList = hl7ObjectToMessage.CreateHl7SiuMessage(dr_);
                        //        }
                        //    }
                        //}

                        SessionOptions _SessionOptions = new SessionOptions();
                        try
                        {
                            var with = _SessionOptions;
                            with.Protocol = Protocol.Sftp;
                            with.HostName = sftpRemoteHost;
                            with.UserName = sftpRemoteUser;
                            with.Password = sftpRemotePassword;
                            try
                            {
                                with.SshHostKeyFingerprint = sftpRemoteHostKey;
                            }
                            catch (Exception)
                            {
                                //throw new Exception("SSH host key \"" + sftpRemoteHostKey +
                                //"\" does not match pattern.");
                            }

                            with.PortNumber = Convert.ToInt32(sftpRemotePort);
                            var sessionOptions = _SessionOptions;
                            if (sessionOptions.SshHostKeyFingerprint != null)
                            {

                                using (Session session = new Session())
                                {

                                    session.DisableVersionCheck = true;
                                    session.ExecutablePath = winScpPath;
                                    session.Open(sessionOptions);
                                    TransferOptions transferOptions = new TransferOptions
                                    {
                                        TransferMode = TransferMode.Binary
                                    };

                                    #region Temp Path For File

                                    string path = Path.GetTempPath() + @"\\HL7\\SIU\\";

                                    #endregion

                                    #region File Name

                                    //var random = new Random();
                                    //var randomNumber = random.Next(0, 100000);
                                    //var nowDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    //var filename = patientId + nowDateTime + randomNumber + ".txt";


                                    var filename = patientId + MDVUtility.GetUniqueCode() + ".txt";

                                    int fixedLength = 17;

                                    if (filename.Length >= fixedLength)
                                    {
                                        fixedLength = ((filename.Length) - fixedLength);
                                        filename = "OR1" + filename.Substring(fixedLength);
                                    }

                                    #endregion

                                    #region Create Local File

                                    if (!(Directory.Exists(path)))
                                        Directory.CreateDirectory(path);
                                    path = path + filename;

                                    StreamWriter swrt = new StreamWriter(path, true);
                                    if (segmentsList != null)
                                    {
                                        for (int i = 0; i < segmentsList.Count; i++)
                                        {
                                            swrt.WriteLine(segmentsList[i]);
                                        }
                                    }
                                    swrt.Close();

                                    #endregion

                                    #region Put file to SFTP

                                    var transferResult = session.PutFiles(path, docAsapUploadPath, false, transferOptions);
                                    transferResult.Check();

                                    #endregion
                                }
                            }
                            //return "";
                        }
                        catch (Exception ex)
                        {
                            //throw ex;
                        }
                    }
                }
            }
        }

        private string FillAppointment(Int64 AppointmentId, Int64 patientId, Int64 providerId)
        {
            string Format = "{0:0.00}";
            try
            {
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
                    DSAppointment dsAppointment = null;

                    MDVision.IEHR.EMR.Model.ReportHeader.ReportHeader_TagsSelectModel model = MDVision.IEHR.EMR.Helpers.Clinical.ReportHeader.ReportHeaderHelper.Instance().getReportHeaderTagsHTML(patientId, providerId, 0, "eSuperbill");


                    BLObject<DSAppointment> obj = BLLScheduleObj.LoadAppointment(AppointmentId, null, null, null, null);
                    if (obj.Data != null)
                    {
                        dsAppointment = obj.Data;
                        if (dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0];
                            if (MDVUtility.ToBool(dr[dsAppointment.PatientAppointments.IsAllVisitUsedColumn.ColumnName])==true) {
                                dr[dsAppointment.PatientAppointments.ReferralNoColumn.ColumnName]= "";
                            }
                            var keyValues = new Dictionary<string, string>
                        {
                            { "hfpatientid", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatientIdColumn.ColumnName])},
                            { "txtProvider", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ProviderNameColumn.ColumnName])},
                            { "txtResource", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ResourceNameColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.FacilityNameColumn.ColumnName])},

                            { "hfProviderId", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ProviderIdColumn.ColumnName])},
                            { "hfFacilityId", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.FacilityIdColumn.ColumnName])},
                            { "hfResourceId", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ResourceIdColumn.ColumnName])},

                            { "hfRefProvider", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.RefProviderIdColumn.ColumnName])},
                            { "txtRefProvider", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.RefProviderNameColumn.ColumnName])},
                            //{ "ddlReferringProvider", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.RefProviderIdColumn.ColumnName])},
                            { "ddlInsurancePlan", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatientInsuranceIdColumn.ColumnName])},
                            { "ddlReason", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.SchReasonIdColumn.ColumnName])},
                            { "ddlStatus", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.SchStatusIdColumn.ColumnName])},
                            { "hfSlottimeid", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.TimeSlotIdColumn.ColumnName])},
                            { "hfSlottimedtlid", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.TimeSlotDtlIdColumn.ColumnName])},
                            { "hfPatientVisitStatus", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.StatusColumn.ColumnName])},
                            { "Duration", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.DurationColumn.ColumnName])},
                            { "txtReferralNo", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ReferralNoColumn.ColumnName])},
                            //{ "patternevery", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatternEveryColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.CommentsColumn.ColumnName])},
                            { "rdSpecialist", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.IsSpecialistColumn.ColumnName])},
                            { "hfAppointmentDate", MDVUtility.ToDateTime(dr[dsAppointment.PatientAppointments.CreatedOnColumn.ColumnName]).ToShortDateString()},
                            //{ "value", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ValueColumn.ColumnName])},
                            //{ "patterdays", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatternDaysColumn.ColumnName])},
                            //{ "patterweeks", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatternWeeksColumn.ColumnName])},
                            //{ "pattermonths", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatternMonthsColumn.ColumnName])},
                            { "PatientName", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatientNameColumn.ColumnName])},
                            { "AccountNumber", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.AccountNumberColumn.ColumnName])},
                            { "txtFromTime", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.TimeFromColumn.ColumnName])},
                            { "txtToTime", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.TimeToColumn.ColumnName])},
                            { "ddlPatientType", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatientTypeIDColumn.ColumnName])},
                            { "ddlVisitType", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.VisitTypeIDColumn.ColumnName])},
                            { "AppointmentStatus", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.StatusColumn.ColumnName])},
                            { "txtSchReason", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ReasonColumn.ColumnName])},
                            { "hfSchReasonId", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.SchReasonIdColumn.ColumnName])},
                            { "ReferralId", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ReferralIdColumn.ColumnName])},
                            { "CoPayment", String.Format(Format,dr[dsAppointment.PatientAppointments.CopaymentColumn.ColumnName])},
                            { "chkNonBilable", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.IsNonBilableColumn.ColumnName])},
                            { "txtScheduleDate", MDVUtility.ToDateTime(dr[dsAppointment.PatientAppointments.AppointmentDateColumn.ColumnName]).ToShortDateString()},
                            { "ReasonCommentType", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ReasonCommentsTypeNameColumn.ColumnName])},
                            { "txtReasonComments", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ReasonCommentsColumn.ColumnName])},
                            { "chkcopayalert", (dr[dsAppointment.PatientAppointments.IsCopayAlertColumn.ColumnName] != System.DBNull.Value? MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.IsCopayAlertColumn.ColumnName]) :"false")},
                            { "isNoteCreated",MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.isNoteCreatedColumn.ColumnName])},
                             { "CancellationReason",MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.CancellationReasonColumn.ColumnName])},
                        };
                            DSReportHeader obj1 = new DSReportHeader();
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                AppointmentFill_JSON = js.Serialize(keyValues),
                                ReportHeaderInfo = HttpUtility.HtmlDecode(model.Header),
                                ReportFooterInfo = HttpUtility.HtmlDecode(model.Footer)
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string FillDurationOnVisitType(Int64 providerId, Int64 PatientVisitTypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(providerId)) && string.IsNullOrEmpty(MDVUtility.ToStr(PatientVisitTypeId)))
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
                    BLObject<string> obj = BLLScheduleObj.LoadDurationOnVisitType(providerId, PatientVisitTypeId);
                    if (obj.Data != null && obj.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            Duration = obj.Data
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

        private string UpdateAppointment(string fieldsJSON, Int64 AppointmentId, string referralId)
        {

            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJSON);
                DSAppointment dsAppointment = null;
                BLObject<DSAppointment> obj = BLLScheduleObj.LoadAppointment(AppointmentId, null, null, null, null);
                dsAppointment = obj.Data;
                if (dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("IsReschedule"))
                            dr[dsAppointment.PatientAppointments.IsRescheduleColumn] = (searchedfieldsJson["IsReschedule"]);

                        if (searchedfieldsJson.ContainsKey("hfRefProvider"))
                            dr[dsAppointment.PatientAppointments.RefProviderIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["hfRefProvider"]);

                        if (searchedfieldsJson.ContainsKey("hfResourceId") && !string.IsNullOrWhiteSpace(searchedfieldsJson["hfResourceId"]))
                            dr[dsAppointment.PatientAppointments.ResourceIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["hfResourceId"]);
                        else
                            dr[dsAppointment.PatientAppointments.ResourceIdColumn] = DBNull.Value;

                        if (searchedfieldsJson.ContainsKey("ddlInsurancePlan"))
                            dr[dsAppointment.PatientAppointments.PatientInsuranceIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["ddlInsurancePlan"]);

                        dr[dsAppointment.PatientAppointments.SchReasonIdColumn] = DBNull.Value;

                        if (searchedfieldsJson.ContainsKey("ddlStatus"))
                            dr[dsAppointment.PatientAppointments.SchStatusIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["ddlStatus"]);

                        if (searchedfieldsJson.ContainsKey("txtReferralNo"))
                            dr[dsAppointment.PatientAppointments.ReferralNoColumn] = MDVUtility.ToStr(searchedfieldsJson["txtReferralNo"]);

                        if (searchedfieldsJson.ContainsKey("txtComments"))
                            dr[dsAppointment.PatientAppointments.CommentsColumn] = MDVUtility.ToStr(searchedfieldsJson["txtComments"]);

                        if (searchedfieldsJson.ContainsKey("Duration"))
                            dr[dsAppointment.PatientAppointments.DurationColumn] = MDVUtility.ToInt32(searchedfieldsJson["Duration"]);

                        if (searchedfieldsJson["rdPCP"] == true)
                        {
                            dr[dsAppointment.PatientAppointments.IsSpecialistColumn] = false;
                        }
                        else if (searchedfieldsJson["rdSpecialist"] == true)
                        {
                            dr[dsAppointment.PatientAppointments.IsSpecialistColumn] = true;
                        }
                        if (searchedfieldsJson.ContainsKey("CancellationReason") && !string.IsNullOrEmpty(searchedfieldsJson["CancellationReason"]))
                        {
                            dr[dsAppointment.PatientAppointments.CancellationReasonColumn] = MDVUtility.ToStr(searchedfieldsJson["CancellationReason"]);
                        }
                        dr[dsAppointment.PatientAppointments.IsActiveColumn] = true;

                        dr[dsAppointment.PatientAppointments.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsAppointment.PatientAppointments.ModifiedOnColumn] = DateTime.Now;

                        if (!string.IsNullOrEmpty(searchedfieldsJson["ddlPatientType"]))
                        {
                            dr[dsAppointment.PatientAppointments.PatientTypeIDColumn] = MDVUtility.ToInt64(searchedfieldsJson["ddlPatientType"]);
                        }
                        if (!string.IsNullOrEmpty(searchedfieldsJson["ddlVisitType"]))
                        {
                            dr[dsAppointment.PatientAppointments.VisitTypeIDColumn] = MDVUtility.ToInt64(searchedfieldsJson["ddlVisitType"]);
                        }
                        //Start 22-08-2016 Humaira Yousaf for referral id
                        if (!string.IsNullOrEmpty(referralId))
                        {
                            dr[dsAppointment.PatientAppointments.ReferralIdColumn] = MDVUtility.ToInt64(referralId);
                        }
                        //End 22-08-2016 Humaira Yousaf for referral id
                        if (!string.IsNullOrEmpty(searchedfieldsJson["CoPayment"]))
                        {
                            dr[dsAppointment.PatientAppointments.CopaymentColumn] = MDVUtility.Tofloat(searchedfieldsJson["CoPayment"]);
                        }
                        else
                        {
                            dr[dsAppointment.PatientAppointments.CopaymentColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(searchedfieldsJson["hfFacilityId"]))
                        {
                            dr[dsAppointment.PatientAppointments.FacilityIdColumn] = MDVUtility.Tofloat(searchedfieldsJson["hfFacilityId"]);
                        }
                        else
                        {
                            dr[dsAppointment.PatientAppointments.FacilityIdColumn] = DBNull.Value;
                        }
                        dr[dsAppointment.PatientAppointments.IsNonBilableColumn] = MDVUtility.ToStr(searchedfieldsJson["chkNonBilable"]) == "True" ? true : false;

                        dr[dsAppointment.PatientAppointments.ReasonCommentsColumn] = MDVUtility.ToStr(searchedfieldsJson["txtReasonComments"]);
                        dr[dsAppointment.PatientAppointments.ProviderIdColumn] = MDVUtility.ToStr(searchedfieldsJson["hfProviderId"]);
                        dr[dsAppointment.PatientAppointments.AppointmentDateColumn] = MDVUtility.ToStr(searchedfieldsJson["txtScheduleDate"]);
                        dr[dsAppointment.PatientAppointments.TimeFromColumn] = MDVUtility.ToStr(searchedfieldsJson["txtFromTime"]);
                        dr[dsAppointment.PatientAppointments.TimeToColumn] = MDVUtility.ToStr(searchedfieldsJson["txtToTime"]);

                        if (searchedfieldsJson.ContainsKey("ReasonCommentType"))
                            dr[dsAppointment.PatientAppointments.ReasonCommentsTypeNameColumn] = MDVUtility.ToStr(searchedfieldsJson["ReasonCommentType"]);

                        if (searchedfieldsJson.ContainsKey("hfICDCode10") && !string.IsNullOrEmpty(searchedfieldsJson["hfICDCode10"]))
                            dr[dsAppointment.PatientAppointments.ICDCode10Column] = MDVUtility.ToStr(searchedfieldsJson["hfICDCode10"]);
                        else
                            dr[dsAppointment.PatientAppointments.ICDCode10Column] = DBNull.Value;

                        if (searchedfieldsJson.ContainsKey("hfICDDescription10") && !string.IsNullOrEmpty(searchedfieldsJson["hfICDDescription10"]))
                            dr[dsAppointment.PatientAppointments.ICDCode10DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["hfICDDescription10"]);
                        else
                            dr[dsAppointment.PatientAppointments.ICDCode10DescriptionColumn] = DBNull.Value;

                        if (searchedfieldsJson.ContainsKey("hfSNOMEDCode") && !string.IsNullOrEmpty(searchedfieldsJson["hfSNOMEDCode"]))
                            dr[dsAppointment.PatientAppointments.SnomedCodeColumn] = MDVUtility.ToStr(searchedfieldsJson["hfSNOMEDCode"]);
                        else
                            dr[dsAppointment.PatientAppointments.SnomedCodeColumn] = DBNull.Value;

                        if (searchedfieldsJson.ContainsKey("hfSNOMEDDescription") && !string.IsNullOrEmpty(searchedfieldsJson["hfSNOMEDDescription"]))
                            dr[dsAppointment.PatientAppointments.SnomedCodeDescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["hfSNOMEDDescription"]);
                        else
                            dr[dsAppointment.PatientAppointments.SnomedCodeDescriptionColumn] = DBNull.Value;

                        dr[dsAppointment.PatientAppointments.FacilityNameColumn] = MDVUtility.ToStr(searchedfieldsJson["txtFacility"]);
                        dr[dsAppointment.PatientAppointments.PatientNameColumn] = MDVUtility.ToStr(searchedfieldsJson["txtFullName"]);
                        dr[dsAppointment.PatientAppointments.ProviderNameColumn] = MDVUtility.ToStr(searchedfieldsJson["txtProvider"]);
                        if (searchedfieldsJson.ContainsKey("chkcopayalert"))
                        {
                            bool showCopayAlert = Convert.ToBoolean(searchedfieldsJson["chkcopayalert"]);
                            if (showCopayAlert)
                                dr[dsAppointment.PatientAppointments.IsCopayAlertColumn] = true;
                            else
                                dr[dsAppointment.PatientAppointments.IsCopayAlertColumn] = false;
                        }

                    }
                    BLObject<DSAppointment> objType = null;
                    objType = BLLScheduleObj.UpdateAppointment(dsAppointment);

                    #region SIU

                    //long patientId = 0;
                    //if (searchedfieldsJson.ContainsKey("hfpatientid")) { patientId = MDVUtility.ToInt64(searchedfieldsJson["hfpatientid"]); }
                    //if (patientId >= 0 && AppointmentId >= 0)
                    //{
                    //    CreateSiuMessage(patientId, AppointmentId);
                    //}

                    #endregion

                    #region Add Problem On Dr. First
                    if (MDVUtility.ToStr(dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0][dsAppointment.PatientAppointments.ReasonCommentsTypeNameColumn.ColumnName]) == "ICD")
                        AddProblemOnDrFirst(AppointmentId, ref dsAppointment);

                    #endregion

                    if (objType.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            AppointmentDetail = MDVUtility.JSON_DataTable(dsAppointment.Tables[dsAppointment.PatientAppointments.TableName]),
                            message = Common.AppPrivileges.Update_Message
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
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        private string UpdateAppointmentStatus(Int64 AppointmentId, Int64 SchStatusId, string CancellationReason = null)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                //var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                DSAppointment dsAppointment = new DSAppointment();
                BLObject<DSAppointment> obj = BLLScheduleObj.LoadAppointment(AppointmentId, null, null, null, null);
                dsAppointment = obj.Data;
                if (dsAppointment.PatientAppointments.Count > 0)
                {
                    DSAppointment.PatientAppointmentsRow dr = dsAppointment.PatientAppointments[0];
                    dr.SchStatusId = SchStatusId;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (CancellationReason != null)
                    {
                        dr.CancellationReason = CancellationReason;
                    }
                    BLObject<DSAppointment> objUpdate = BLLScheduleObj.UpdateAppointmentStatus(dsAppointment);
                    if (objUpdate.Data != null)
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
                            Message = objUpdate.Message
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
        private string UpdateAppointmentCancellationReason(Int64 AppointmentId, string CancellationReason = null)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                //var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                DSAppointment dsAppointment = new DSAppointment();
                BLObject<DSAppointment> obj = BLLScheduleObj.LoadAppointment(AppointmentId, null, null, null, null);
                dsAppointment = obj.Data;
                if (dsAppointment.PatientAppointments.Count > 0)
                {
                    DSAppointment.PatientAppointmentsRow dr = dsAppointment.PatientAppointments[0];
                  
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (CancellationReason != null)
                    {
                        dr.CancellationReason = CancellationReason;
                    }
                    BLObject<DSAppointment> objUpdate = BLLScheduleObj.UpdateAppointmentCancellationReason(dsAppointment);
                    if (objUpdate.Data != null)
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
                            Message = objUpdate.Message
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

        private string DeletePatientAppointment(Int64 AppointmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AppointmentId)))
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
                    BLObject<string> obj = BLLScheduleObj.DeletePatAppointment(MDVUtility.ToStr(AppointmentId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        //private string LoadPatientBalance(Int64 AppointmentId)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(MDVUtility.ToStr(AppointmentId)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {
        //            DSAppointment dsSchedule = null;
        //            BLObject<DSAppointment> obj =BLLScheduleObj.LoadPatientBalance(AppointmentId);
        //            dsSchedule = obj.Data;
        //            if (dsSchedule.Tables[dsSchedule.PatientBalance.TableName].Rows.Count > 0)
        //            {
        //                DataRow dr = dsSchedule.Tables[dsSchedule.PatientBalance.TableName].Rows[0];
        //                var keyValues = new Dictionary<string, string>
        //                {
        //                    { "txtPatientBalance", MDVUtility.ToStr(dr[dsSchedule.PatientBalance.TotalBalanceColumn.ColumnName])}
        //                };
        //                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //                var response = new
        //                {
        //                    status = true,
        //                    PatientBalanceFill_JSON = js.Serialize(keyValues)
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        public string SavePatientMessage(string pracId, string facId, string appDate, string status, string provName, string patId, string patName)
        {
            try
            {
                string message = GetFormattedMessage(pracId, facId, appDate, status, provName, patName);
                //DSMessage dsMessage = new DSMessage();
                //DSMessage.PatMessagesRow dr = dsMessage.PatMessages.NewPatMessagesRow();
                //dr.PatientId = MDVUtility.ToInt64(patId);
                //dr.MsgDetail = message;
                //dr.UserId = MDVUtility.ToStr(Common.AppConfig.AppUserId);
                //dr.IsActive = true;
                //dr.CreatedBy = MDVUtility.DecryptFrom64(Common.AppConfig.AppUserName);
                //dr.CreatedOn = DateTime.Now;
                //dr.ModifiedBy = MDVUtility.DecryptFrom64(Common.AppConfig.AppUserName);
                //dr.ModifiedOn = DateTime.Now;
                //dr.Subject = "Appointment Request " + status;
                //dr.IsRead = false;
                Guid id = Guid.NewGuid();
                string unique_no = id.ToString();
                DSMessage dsMessage = new DSMessage();
                DSMessage.UserMessagesRow dr = dsMessage.UserMessages.NewUserMessagesRow();

                dr.AssignedFromId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                dr.AttatchedPatientId = MDVUtility.ToInt64(patId);
                dr.Subject = "Appointment Request " + status;
                dr.MessageDetail = message;
                dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.IsRead = false;
                dr.Entityid = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.MessagerType = "Patient";
                dr.UniqueNumber = unique_no;


                #region Database Insertion
                dsMessage.UserMessages.AddUserMessagesRow(dr);
                BLObject<DSMessage> obj = BLLMessageObj.InsertPracticeMessage(dsMessage);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserMessageId = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.UserMessagesIdColumn.ColumnName],
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string GetFormattedMessage(string pracId, string facId, string appDate, string status, string provName, string patName)
        {

            DateTime dt = MDVUtility.ToDateTime(appDate);
            string new_date = dt.ToString("D");

            DSProfile dsPractice = null;
            BLObject<DSProfile> objPractice = BLLAdminProfileObj.LoadPractice(MDVUtility.ToInt64(pracId), null, null, null, null, null);
            dsPractice = objPractice.Data;

            DSProfile dsFacility = null;
            BLObject<DSProfile> objFacility = BLLAdminProfileObj.LoadFacility(MDVUtility.ToInt64(facId), null, null, null, null, null);
            dsFacility = objFacility.Data;

            DataRow drFacility = dsFacility.Tables[dsFacility.Facility.TableName].Rows[0];
            DataRow drPractice = dsPractice.Tables[dsPractice.Practice.TableName].Rows[0];

            string facilityname = MDVUtility.ToStr(drFacility[dsFacility.Facility.ShortNameColumn.ColumnName]);
            string facilityaddress = MDVUtility.ToStr(drPractice[dsFacility.Facility.AddressColumn.ColumnName]);
            string facilityaddress2 = MDVUtility.ToStr(drPractice[dsFacility.Facility.AddressColumn.ColumnName]);

            string facilityCity = MDVUtility.ToStr(drPractice[dsFacility.Facility.CityColumn.ColumnName]);
            string facilityState = MDVUtility.ToStr(drPractice[dsFacility.Facility.StateColumn.ColumnName]);
            string facilityZip = MDVUtility.ToStr(drPractice[dsFacility.Facility.ZIPCodeColumn.ColumnName]);
            string facilitPhone = MDVUtility.ToStr(drPractice[dsFacility.Facility.PhoneNoColumn.ColumnName]);
            string facilitFax = MDVUtility.ToStr(drPractice[dsFacility.Facility.FaxColumn.ColumnName]);

            string practicename = MDVUtility.ToStr(drPractice[dsPractice.Practice.ShortNameColumn.ColumnName]);
            string practiceadd1 = MDVUtility.ToStr(drPractice[dsPractice.Practice.AddressColumn.ColumnName]);
            string practiceadd2 = MDVUtility.ToStr(drPractice[dsPractice.Practice.Address2Column.ColumnName]);
            string practicecity = MDVUtility.ToStr(drPractice[dsPractice.Practice.CityColumn.ColumnName]);
            string practicestate = MDVUtility.ToStr(drPractice[dsPractice.Practice.StateColumn.ColumnName]);
            string practicezip = MDVUtility.ToStr(drPractice[dsPractice.Practice.ZIPCodeColumn.ColumnName]);
            string practicephone = MDVUtility.ToStr(drPractice[dsPractice.Practice.PhoneNoColumn.ColumnName]);
            string practicefax = MDVUtility.ToStr(drPractice[dsPractice.Practice.FaxColumn.ColumnName]);
            string provname = provName;

            string message = "Dear " + patName + "," + "\n";
            message += "Your request for an appointment with " + provname + " on " + new_date + " at " + facilityname + " " + facilityaddress + " has been " + status + ".\n";
            message += "If you are having a medical emergency, call 911 or emergency medical help. \n \n";
            message += "Please call the office for further details. \n";
            message += "Thank you, \n \n";
            message += facilityname + "\n";
            //message += practiceadd1 + "\n";
            message += facilityCity + ", " + facilityState + " " + facilityZip + "\n";
            message += "Phone: " + facilitPhone + "\n";
            message += "Fax: " + facilitFax + "\n";
            return message;
        }

        public string GetPatientVisitType(string ProviderId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSScheduleLookups> objVisitType = new BLLSchedule().LookupPatientVisitType(ProviderId);
            DSScheduleLookups ds = objVisitType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.PatientVisitType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PatientVisitType.TableName].Select("1=1", ds.PatientVisitType.VisitTypeColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PatientVisitType.VisitTypeColumn.ColumnName].ToString(), dr[ds.PatientVisitType.VisitTypeIDColumn.ColumnName].ToString(), dr[ds.PatientVisitType.PatientTypeIDColumn.ColumnName].ToString(), dr[ds.PatientVisitType.DurationColumn.ColumnName].ToString()));
                    }
                }
            }
            return JsonConvert.SerializeObject(list);
        }

        public string GetAppointmentReferralNo(string ProviderId, string FacilityId, string PatientInsuranceId, string PatientId, string AppointmentDate)
        {
            try
            {
                List<AppointmentModel> listApointmentReferral = new List<AppointmentModel>();
                BLObject<List<AppointmentModel>> obj = BLLScheduleObj.GetAppointmentReferralNo(ProviderId, FacilityId, PatientInsuranceId, PatientId, AppointmentDate);
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                if (obj.Data != null)
                {
                    listApointmentReferral = obj.Data;

                    if (listApointmentReferral != null && listApointmentReferral.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ReferralCount = listApointmentReferral[0].RecordCount,
                            ReferralLoad_JSON = js.Serialize(listApointmentReferral),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.No_Record_Message,
                            ReferralLoad_JSON = listApointmentReferral,
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
                case "FILL_PATIENT":
                    {
                        string PatientID = context.Request["PatientID"];

                        string strJSONData = FillPatient(MDVUtility.ToInt64(PatientID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_SCHEDULEREASON_DURATION":
                    {
                        Int64 strScheduleReasonId = MDVUtility.ToInt64(context.Request["ScheduleReasonID"]);
                        string strJSONData = FillReasonDuration(MDVUtility.ToInt64(strScheduleReasonId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_COPAYMENT":
                    {
                        //Int64 strInsurancePlanId = MDVUtility.ToInt64(context.Request["InsurancePlanID"]);
                        //string strJSONData;// = FillCopayment(MDVUtility.ToInt64(strInsurancePlanId));

                        //context.Response.ContentType = "text/plain";
                        //context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_PATIENT":
                    {
                        string AccountNo = context.Request["AccountNo"];

                        string strJSONData = SearchPatients(AccountNo);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_PATIENT_BY_NAME":
                    {
                        string searchstring = context.Request["AccountNo"];

                        string strJSONData = SearchPatientsByName(searchstring);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_APPOINTMENT":
                    {
                        string fieldsJSON = context.Request["AppointmentData"];
                        string fieldsJSON1 = context.Request["AppointmentPatternData"];
                        string referralId = context.Request["ReferralId"];

                        string strJSONData = SaveAppointment(fieldsJSON, fieldsJSON1, referralId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PATIENT_APPOINTMENT":
                    {
                        string fieldsJSON = context.Request["AppointmentData"];
                        string fieldsJSON1 = context.Request["AppointmentPatternData"];
                        string referralId = context.Request["ReferralId"];

                        string strJSONData = SaveAppointment(fieldsJSON, fieldsJSON1, referralId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CHECK_CANCELLED_APPOINTMENT":
                    {
                        string fieldsJSON = context.Request["AppointmentData"];


                        string strJSONData = CheckifCancelledAppointmentExists(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_APPOINTMENT":
                    {
                        Int64 patientId = MDVUtility.ToInt64(context.Request["patientID"]);
                        Int64 providerId = MDVUtility.ToInt64(context.Request["providerID"]);
                        Int64 strAppointmentId = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        if (strAppointmentId == 0)
                            strAppointmentId = -1;
                        string strJSONData = FillAppointment(strAppointmentId, patientId, providerId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_DURATION_ON_VISIT_TYPE":
                    {
                        Int64 ProviderId = MDVUtility.ToInt64(context.Request["ProviderID"]);
                        Int64 PatientVisitTypeId = MDVUtility.ToInt64(context.Request["PatientVisitTypeID"]);
                        string strJSONData = FillDurationOnVisitType(ProviderId, PatientVisitTypeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_APPOINTMENT":
                    {
                        string fieldsJSON = context.Request["AppointmentData"];

                        Int64 AppointmentID = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        string referralId = context.Request["ReferralId"];

                        string strJSONData = UpdateAppointment(fieldsJSON, AppointmentID, referralId);



                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_PATIENT_APPOINTMENT":
                    {
                        string fieldsJSON = context.Request["AppointmentData"];

                        Int64 AppointmentID = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        string referralId = context.Request["ReferralId"];

                        string strJSONData = UpdateAppointment(fieldsJSON, AppointmentID, referralId);



                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_APPOINTMENT_STATUS":
                    {
                        string fieldsJSON = context.Request["AppointmentData"];
                        Int64 AppointmentID = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        Int64 SchStatusID = MDVUtility.ToInt64(context.Request["SchStatusID"]);
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var CancellationReason = "";
                        if (fieldsJSON != null)
                        {
                             var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);     
                             if (!string.IsNullOrEmpty(SearchedfieldsJSON["CancelReason"]))
                             {
                                 CancellationReason = Convert.ToString(SearchedfieldsJSON["CancelReason"]);
                             }
                        }

                        string strJSONData = "";
                        if (AppointmentID == 0 || SchStatusID == 0)
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Appointment not exist"
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                            if (CancellationReason != null && CancellationReason != "")
                            {
                                strJSONData = UpdateAppointmentStatus(AppointmentID, SchStatusID, CancellationReason);
                            }
                            else
                            {
                                strJSONData = UpdateAppointmentStatus(AppointmentID, SchStatusID);
                            }
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_APPOINTMENT_CANCELLATION_REASON":
                    {
                        string fieldsJSON = context.Request["AppointmentData"];
                        Int64 AppointmentID = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                       
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var CancellationReason = "";
                        if (fieldsJSON != null)
                        {
                            var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["CancelReason"]))
                            {
                                CancellationReason = Convert.ToString(SearchedfieldsJSON["CancelReason"]);
                            }
                        }

                        string strJSONData = "";
                        if (AppointmentID == 0)
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Appointment not exist"
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                                strJSONData = UpdateAppointmentCancellationReason(AppointmentID, CancellationReason);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PATIENT_APPOINTMENT":
                    {
                        string strAppointmentId = context.Request["AppointmentID"];
                        string strJSONData = DeletePatientAppointment(MDVUtility.ToInt64(strAppointmentId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_VISIT_TYPE_DROPDOWN":
                    {
                        string ProviderId = context.Request["ProviderId"];
                        string strJSONData = GetPatientVisitType(ProviderId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "GET_REFERRAL_NO":
                    {
                        string ProviderId = context.Request["ProviderId"];
                        string FacilityId = context.Request["FacilityId"];
                        string PatientInsuranceId = context.Request["PatientInsuranceId"];
                        string PatientId = context.Request["PatientId"];
                        string AppointmentDate = context.Request["AppointmentDate"];

                        string strJSONData = GetAppointmentReferralNo(ProviderId, FacilityId, PatientInsuranceId, PatientId, AppointmentDate);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                    //case "FILL_PATIENT_BALANCE":
                    //    {
                    //        string strAppointmentId = context.Request["AppointmentID"];
                    //        string strJSONData = LoadPatientBalance(MDVUtility.ToInt64(strAppointmentId));

                    //        context.Response.ContentType = "text/plain";
                    //        context.Response.Write(strJSONData);
                    //    }
                    //    break;
            }
        }
        #endregion
    }
}