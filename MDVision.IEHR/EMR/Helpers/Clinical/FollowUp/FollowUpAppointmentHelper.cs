using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.EMR.Model.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using System.Threading;
using MDVision.IEHR.Common;
using Newtonsoft.Json.Linq;
using MDVision.IEHR.EMR.Model.FollowUp;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.FollowUp
//namespace MDVision.IEHR.Controls.Clinical
{
    public class FollowUpAppointmentHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private BLLSchedule BLLScheduleObj = null;
        private BLLVisits BLLVisitsObj = null;
        public FollowUpAppointmentHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLScheduleObj = new BLLSchedule();
            BLLVisitsObj = new BLLVisits();
        }

        private static FollowUpAppointmentHelper _instance = null;
        public static FollowUpAppointmentHelper Instance()
        {
            if (_instance == null)
                _instance = new FollowUpAppointmentHelper();
            return _instance;
        }
        public string FillAppointment(FollowUpAppointmentModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.AppointmentID)))
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
                    BLObject<DSAppointment> obj = BLLScheduleObj.LoadAppointment(MDVUtility.ToInt64(model.AppointmentID), null, null, null, null);
                    if (obj.Data != null)
                    {
                        dsAppointment = obj.Data;
                        if (dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows[0];
                            model.Date = MDVUtility.ToDateTime(dr[dsAppointment.PatientAppointments.CreatedOnColumn.ColumnName]).ToShortDateString();
                            model.Time = MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.TimeToColumn.ColumnName]);
                            model.Provider = MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ProviderNameColumn.ColumnName]);
                            model.Facility = MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.FacilityNameColumn.ColumnName]);
                            model.Resource = MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ResourceNameColumn.ColumnName]);
                            model.schreason = MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ReasonCommentsColumn.ColumnName]);
                            model.Duration = MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.DurationColumn.ColumnName]);
                            model.Comments = MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.CommentsColumn.ColumnName]);
                            model.PatientId = MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatientIdColumn.ColumnName]);
                            //    var keyValues = new Dictionary<string, string>
                            //{
                            //    { "hfpatientid", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatientIdColumn.ColumnName])},
                            //    { "txtProvider", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ProviderNameColumn.ColumnName])},
                            //    { "txtResource", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ResourceNameColumn.ColumnName])},
                            //    { "txtFacility", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.FacilityNameColumn.ColumnName])},

                            //    { "hfProviderId", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ProviderIdColumn.ColumnName])},
                            //    { "hfFacilityId", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.FacilityIdColumn.ColumnName])},
                            //    { "hfResourceId", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ResourceIdColumn.ColumnName])},

                            //    { "hfRefProvider", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.RefProviderIdColumn.ColumnName])},
                            //    { "txtRefProvider", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.RefProviderNameColumn.ColumnName])},
                            //    //{ "ddlReferringProvider", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.RefProviderIdColumn.ColumnName])},
                            //    { "ddlInsurancePlan", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatientInsuranceIdColumn.ColumnName])},
                            //    { "ddlReason", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.SchReasonIdColumn.ColumnName])},
                            //    { "ddlStatus", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.SchStatusIdColumn.ColumnName])},
                            //    { "hfSlottimeid", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.TimeSlotIdColumn.ColumnName])},
                            //    { "hfSlottimedtlid", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.TimeSlotDtlIdColumn.ColumnName])},
                            //    { "hfPatientVisitStatus", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.StatusColumn.ColumnName])},
                            //    { "Duration", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.DurationColumn.ColumnName])},
                            //    { "txtReferralNo", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ReferralNoColumn.ColumnName])},
                            //    //{ "patternevery", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatternEveryColumn.ColumnName])},
                            //    { "txtComments", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.CommentsColumn.ColumnName])},
                            //    { "rdSpecialist", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.IsSpecialistColumn.ColumnName])},
                            //    { "hfAppointmentDate", MDVUtility.ToDateTime(dr[dsAppointment.PatientAppointments.CreatedOnColumn.ColumnName]).ToShortDateString()},
                            //    //{ "value", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ValueColumn.ColumnName])},
                            //    //{ "patterdays", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatternDaysColumn.ColumnName])},
                            //    //{ "patterweeks", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatternWeeksColumn.ColumnName])},
                            //    //{ "pattermonths", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatternMonthsColumn.ColumnName])},
                            //    { "PatientName", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatientNameColumn.ColumnName])},
                            //    { "AccountNumber", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.AccountNumberColumn.ColumnName])},
                            //    { "txtFromTime", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.TimeFromColumn.ColumnName])},
                            //    { "txtToTime", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.TimeToColumn.ColumnName])},
                            //    { "ddlPatientType", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.PatientTypeIDColumn.ColumnName])},
                            //    { "ddlVisitType", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.VisitTypeIDColumn.ColumnName])},
                            //    { "AppointmentStatus", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.StatusColumn.ColumnName])},
                            //    { "txtSchReason", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.ReasonColumn.ColumnName])},
                            //    { "hfSchReasonId", MDVUtility.ToStr(dr[dsAppointment.PatientAppointments.SchReasonIdColumn.ColumnName])},

                            //};
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                AppointmentFill_JSON = js.Serialize(model)
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

        public string LoadPatientAppointment(FollowUpAppointmentModel model)
        {
            try
            {

                BLObject<List<MDVision.Model.Clinical.FollowUp.FollowUpAppointmentModel>> obj;
                obj = BLLScheduleObj.LoadPatientAppointment(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.Provider), MDVUtility.ToInt64(model.Facility), MDVUtility.ToStr(model.Date));


                if (obj.Data != null)
                {

                    var response = new
                    {
                        status = true,
                        AvailableAppointmentsCount = obj.Data.Count,
                        AvailableAppointmentsLoad_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(obj.Data),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = false,
                        AvailableAppointmentsCount = 0,
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

        public string LoadAvailableSlots(FollowUpAppointmentModel model)
        {
            try
            {

                BLObject<List<MDVision.Model.Clinical.FollowUp.FollowUpAppointmentModel>> obj;
                obj = BLLScheduleObj.LoadAvailableSlots(MDVUtility.ToInt64(model.Provider), MDVUtility.ToInt64(model.Facility), MDVUtility.ToStr(model.Date));


                if (obj.Data != null)
                {

                    var response = new
                    {
                        status = true,
                        AvailableSlotsCount = obj.Data.Count,
                        AvailableSlotsLoad_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(obj.Data),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = false,
                        AvailableSlotsCount = 0,
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

        public string SaveFollowUpAppointment(FollowUpAppointmentModel model)
        {
            try
            {
                DSNotes dsFollowUp = new DSNotes();
                DSNotes.FollowUpAppointmentRow dr = dsFollowUp.FollowUpAppointment.NewFollowUpAppointmentRow();

                long PatientId = MDVUtility.ToInt64(model.PatientId);
                long ProviderId = MDVUtility.ToInt64(model.Provider);
                long ResourceId = MDVUtility.ToInt64(model.Resource);
                long FacilityId = MDVUtility.ToInt64(model.Facility);
                DateTime AppointmentDate = MDVUtility.ToDateTime(model.Date);
                string FromTime = MDVUtility.ToStr(model.Time);
                Int32 Duration = MDVUtility.ToInt32(model.Duration);
                string Comments = MDVUtility.ToStr(model.Comments);
                //long SchReasonId = MDVUtility.ToInt64(model.schreason);
                string reasonComments = MDVUtility.ToStr(model.schreason);
                Byte IsActive = 1;
                string CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime CreatedOn = DateTime.Now;
                string ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime ModifiedOn = DateTime.Now;

                #region Database Insertion

                BLObject<string> obj = BLLClinicalObj.InsertFollowUpAppointment(PatientId, ProviderId, ResourceId, FacilityId,
                    AppointmentDate, FromTime, Duration, Comments, 0, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, reasonComments);

                if (obj.Data == "Object reference not set to an instance of an object." || string.IsNullOrEmpty(obj.Data) || MDVUtility.ToLong(obj.Data) > 0 )
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        AppointmentId = obj.Data,
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


        public string UpdateAppointment(FollowUpAppointmentModel model)
        {

            try
            {

                DSAppointment dsAppointment = null;
                BLObject<DSAppointment> obj = BLLScheduleObj.LoadAppointment(MDVUtility.ToInt64(model.AppointmentID), null, null, null, null);
                dsAppointment = obj.Data;
                if (dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsAppointment.Tables[dsAppointment.PatientAppointments.TableName].Rows)
                    {

                        if (!string.IsNullOrEmpty(model.schreason))
                        {
                            dr[dsAppointment.PatientAppointments.ReasonCommentsColumn] = MDVUtility.ToStr(model.schreason);
                        }
                        else
                        {
                            dr[dsAppointment.PatientAppointments.SchReasonIdColumn] = DBNull.Value;
                        }

                        dr[dsAppointment.PatientAppointments.CommentsColumn] = MDVUtility.ToStr(model.Comments);

                        dr[dsAppointment.PatientAppointments.DurationColumn] = MDVUtility.ToInt32(model.Duration);
                        dr[dsAppointment.PatientAppointments.SchStatusIdColumn] = 0;
                        dr[dsAppointment.PatientAppointments.AppointmentStatusColumn] = "ReSchedule";
                        dr[dsAppointment.PatientAppointments.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsAppointment.PatientAppointments.ModifiedOnColumn] = DateTime.Now;


                    }
                    BLObject<DSAppointment> objType = null;
                    objType = BLLScheduleObj.UpdateAppointment(dsAppointment);
                    if (objType.Data != null)
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

        public string UpdateVisit(FollowUpAppointmentModel model)
        {

            try
            {


                DSVisits dsVisit = null;
                BLObject<DSVisits> objVisits = null;
                objVisits = BLLVisitsObj.LoadPatientsVisits(MDVUtility.ToInt64(model.VisitId), MDVUtility.ToInt64(model.PatientId),0, 0, null, null, "", "", "");
                dsVisit = objVisits.Data;

                if (dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows)
                    {

                        if (!string.IsNullOrEmpty(model.AppointmentID))
                        {
                            dr[dsVisit.PatientVisits.AppointmentIdColumn] = MDVUtility.ToInt64(model.AppointmentID);
                        }
                        else
                        {
                            dr[dsVisit.PatientVisits.AppointmentIdColumn] = DBNull.Value;
                        }

                        dr[dsVisit.PatientVisits.VisitStatusColumn] = "CheckIn";
                        dr[dsVisit.PatientVisits.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsVisit.PatientVisits.ModifiedOnColumn] = DateTime.Now;


                    }
                    BLObject<DSVisits> objType = null;
                    objType = BLLScheduleObj.UpdateVisit(dsVisit);
                    if (objType.Data != null)
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
                            Message = objType.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false
                      //  Message = obj.Message
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

        public string GetAppointmentSlotInfo(FollowUpAppointmentModel model)
        {

            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.Provider))
                    || string.IsNullOrEmpty(MDVUtility.ToStr(model.Facility))
                    || string.IsNullOrEmpty(MDVUtility.ToStr(model.Duration))
                    || string.IsNullOrEmpty(MDVUtility.ToStr(model.SlotType))
                    || string.IsNullOrEmpty(MDVUtility.ToStr(model.SlotValue)))
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

                    string AppointmentDate = string.Empty;
                    string FromTime = string.Empty;
                    string ToTime = string.Empty;
                    DateTime now = DateTime.Now;
                    //FromTime = now.ToString("hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                    //ToTime = now.AddMinutes(MDVUtility.ToDouble(model.Duration)).ToString("hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);

                    if (model.SlotValue.Contains('-') && model.SlotType == "D")
                    {
                        var values = model.SlotValue.Split('-');
                        AppointmentDate = now.AddDays(MDVUtility.ToLong(values[0])).ToShortDateString();
                        AppointmentDate += ",";
                        AppointmentDate += now.AddDays(MDVUtility.ToLong(values[1])).ToShortDateString();
                    }
                    else
                    {
                        switch (model.SlotType)
                        {
                            case "D":
                                AppointmentDate = now.AddDays(MDVUtility.ToInt(model.SlotValue)).ToShortDateString();
                                break;
                            case "W":
                                AppointmentDate = now.AddDays(MDVUtility.ToInt(model.SlotValue) * 7).ToShortDateString();
                                break;
                            case "M":
                                AppointmentDate = now.AddMonths(MDVUtility.ToInt(model.SlotValue)).ToShortDateString();
                                break;
                            case "Y":
                                AppointmentDate = now.AddYears(MDVUtility.ToInt(model.SlotValue)).ToShortDateString();
                                break;
                        }
                    }

                    DSAppointment dsAppointment = null;
                    BLObject<DSAppointment> obj = BLLScheduleObj.LoadProviderAppointmentSlotInfo(MDVUtility.ToInt64(model.Provider), MDVUtility.ToInt64(model.Facility), AppointmentDate, MDVUtility.ToInt64(model.Duration));
                    if (obj.Data != null)
                    {
                        dsAppointment = obj.Data;
                        if (dsAppointment.Tables[dsAppointment.TimeSlotsDetail.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsAppointment.Tables[dsAppointment.TimeSlotsDetail.TableName].Rows[0];
                            model.Date = MDVUtility.ToDateTime(dr[dsAppointment.TimeSlotsDetail.ScheduleDateColumn.ColumnName]).ToShortDateString();
                            model.FromTimeSlots = MDVUtility.ToStr(dr[dsAppointment.TimeSlotsDetail.FromTimeSlotsColumn.ColumnName]);
                            model.ToTimeSlots = MDVUtility.ToStr(dr[dsAppointment.TimeSlotsDetail.ToTimeSlotsColumn.ColumnName]);
                            model.Duration = MDVUtility.ToStr(dr[dsAppointment.TimeSlotsDetail.SlotMinutesColumn.ColumnName]);
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                AppointmentFill_JSON = js.Serialize(model)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Slot not available.",
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

    }
}