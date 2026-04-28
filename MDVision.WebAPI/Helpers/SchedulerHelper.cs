using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.Model.Native.Scheduler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace MDVision.WebAPI.Helpers
{
    
    public class SchedulerHelper
    {
        
        private BLLSchedule BLLScheduleObj = null;
        private string successMessage = "Your information has been received successfully!";
        public SchedulerHelper()
        {
            BLLScheduleObj = new BLLSchedule();
            
        }

        /// <summary>
        /// Returns provider's appointments for a day.
        /// </summary>
        /// <param name="ProviderId"></param>
        /// <param name="SlotDate"></param>
        /// <param name="StatusId"></param>
        /// <param name="PatientTypeId"></param>
        /// <param name="VisitTypeId"></param>
        /// <param name="Resourceid"></param>
        /// <param name="Facilityid"></param>
        /// <returns></returns>
        public string SearchDaySlotSchedule(Int64 ProviderId, string SlotDate, string StatusId, string PatientTypeId, string VisitTypeId, Int64 Resourceid = 0, Int64 Facilityid = 0)
        {
            try
            {
                
                if (string.IsNullOrEmpty(MDVUtility.ToStr(Facilityid)))
                {
                    
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SlotDate)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSAppointment dsSchedule = null;

                    BLObject<DSAppointment> obj = BLLScheduleObj.SearchDailySlots(ProviderId, Resourceid, Facilityid, SlotDate, StatusId, PatientTypeId, VisitTypeId);
                   if (obj.Data != null)
                    {
                        dsSchedule = obj.Data;

                        BLLScheduleObj.ScheduleAppSort(ref dsSchedule);

                        if (dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                                editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                                viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),

                                ProviderScheduleFill_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.DaySlots.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message =AppPrivileges.No_Record_Message,
                                addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                                editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                                viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
                                ProviderScheduleFill_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.DaySlots.TableName]),
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
                            addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                            editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                            viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),

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

        public string SearchAppointment(long ProviderId, string AppointmentDate, string StatusId, Int64 Resourceid, long Facilityid, int PageNumber, int RowsPerPage)
        {
            try
            {
                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;
                string isCheckedIn = "0";
                int PatientId = 0;
                string IsFaceSheet = "0";
                obj = BLLScheduleObj.LoadAppointmentsVisits(ProviderId, Facilityid, 0, AppointmentDate, "", "", "", null, isCheckedIn,PageNumber, RowsPerPage, "",PatientId,IsFaceSheet);
               
                    dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SchAppStatusCount = dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count,
                            iTotalDisplayRecords = dsSchedule.AppointmentsVisits.Rows[0][dsSchedule.AppointmentsVisits.RecordCountColumn.ColumnName],
                            SchAppStatus_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SchAppStatusCount = 0,
                            iTotalDisplayRecords = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SchAppStatusCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = obj.Message
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


        public string GetNearestEmptySlot( string providerId, string facilityId)
        {

            try
            {
                var nearestSlotData =  new BLLMobileApp().GetNearestEmptySlot( providerId, facilityId);
                            var response = new
                            {
                                status = true,
                                nearestSlotData = nearestSlotData
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


        public string SaveAppointment(JObject data)
        {
            try
            {


                JavaScriptSerializer ser = new JavaScriptSerializer();
                EmptySlotModel model = ser.Deserialize<EmptySlotModel>(MDVUtility.ToStr(data["data"]));
                BLLMobileApp BLLMobileAppObj = new BLLMobileApp();
                string returnVal = "";
                if ( (!string.IsNullOrEmpty(model.PatientId) || !string.IsNullOrEmpty(model.DimmyPatientId)))
                {

                    foreach (var item in model.DataChangeRequest)
                    {
                        item.ColumnKeyId = model.AppointmentId;
                        item.ColumnKeyName = "AppointmentId";


                        item.CreatedBy = MDVUtility.DecryptFrom64(model.UserName);
                        item.CreatedOn = DateTime.Now;

                        item.ModifiedBy = MDVUtility.DecryptFrom64(model.UserName);
                        item.ModifiedOn = DateTime.Now;
                        item.PatientId = !string.IsNullOrEmpty(model.PatientId)  ? Convert.ToInt64(model.PatientId) : (long?)null;
                        item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? model.DimmyPatientId : null;
                        item.IsSynced = false;
                        item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                        item.DBTableName = "PatientAppointments";
                    }
                    returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(model.DataChangeRequest);
                }
                

                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }

               
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    Message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }

        }

    }

}