using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Common;
using System.Collections;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Model.Schedule;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_Calendar
    {
        private BLLPatient BLLPatientObj = null;
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        private BLLSchedule BLLScheduleObj = null;

        public Scheduling_Calendar()
        {
            BLLPatientObj = new BLLPatient();
            BLLAdminSecurityObj = new BLLAdminSecurity();
            BLLScheduleObj = new BLLSchedule();
        }

        #region Singleton
        private static Scheduling_Calendar _obj = null;
        public static Scheduling_Calendar Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_Calendar();
            return _obj;
        }
        #endregion

        #region "Search Day Slot Schedule"

        private string SearchDaySlotSchedule(Int64 ProviderId, Int64 Resourceid, Int64 Facilityid, string SlotDate, string StatusId, string PatientTypeId, string VisitTypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(Facilityid)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SlotDate)))
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
                    DSAppointment dsSchedule = null;

                    BLObject<DSAppointment> obj = BLLScheduleObj.SearchDailySlots(ProviderId, Resourceid, Facilityid, SlotDate, StatusId, PatientTypeId, VisitTypeId);

                    // BLObject<DSAppointment> obj =BLLScheduleObj.SearchDailySlots(51, 0, 38, "2015-03-16", StatusId);
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
                                Message = Common.AppPrivileges.No_Record_Message,
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
        public static string[] RemoveDuplicates(string[] s)
        {
            HashSet<string> set = new HashSet<string>(s);
            string[] result = new string[set.Count];
            set.CopyTo(result);
            return result;
        }
        private string SearchWeekDaySlotSchedule(Int64 ProviderId, Int64 Resourceid, Int64 Facilityid, string SlotDate, string StatusId, string PatientTypeId, string VisitTypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(Facilityid)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SlotDate)))
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
                    DSAppointment dsSchedule = null;

                    string[] singledate = SlotDate.Split(',');

                    string day = singledate[0];
                    string passingdate = singledate[1];

                    BLObject<DSAppointment> obj = BLLScheduleObj.SearchDailySlots(ProviderId, Resourceid, Facilityid, passingdate, StatusId, PatientTypeId, VisitTypeId);

                    // BLObject<DSAppointment> obj =BLLScheduleObj.SearchDailySlots(42, Resourceid, 32, "2015-01-01");
                    if (obj.Data != null)
                    {
                        dsSchedule = obj.Data;


                        BLLScheduleObj.ScheduleAppSort(ref dsSchedule);

                        if (dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                tableday = day,

                                tabledate = passingdate,

                                ProviderScheduleFill_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.DaySlots.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                tableday = day,

                                tabledate = passingdate,
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

        private DSAppointment sort1(DSAppointment dsSchedule)
        {
            for (int i = 0; i < dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count; i++)
            {
                //if (i > 0)
                //{
                if (i + 1 < dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count)
                {
                    string[] AppDtl = dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName].ToString().Split('|');
                    string[] AppDtl1 = dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i + 1][dsSchedule.DaySlots.AppDtlColumn.ColumnName].ToString().Split('|');



                    for (int j = 0; j < AppDtl.Length; j++)
                    {
                        if (AppDtl[j].ToString() != "")
                        {
                            //  string value1 = Array.FindIndex(AppDtl1, AppDtl[j]);
                            string apps = "";
                            int index1 = Array.FindIndex(AppDtl1, item => item == AppDtl[j]);
                            if (index1 > 0)
                            {
                                foreach (var str in AppDtl)
                                {
                                    if (str != "")
                                    {
                                        for (int l = 0; l < index1; l++)
                                        {

                                            apps = apps + '|' + str;
                                            //apps = str + '|' + apps;
                                        }
                                    }
                                }

                                if (apps != "")
                                {
                                    dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName] = apps;
                                    break;

                                }
                            }
                        }

                    }

                }
            }
            return dsSchedule;
        }
        #endregion

        #region "Select Monthly Appointment"
        private string SelectMonthlyAppointment(long ProviderId, long FacilityId, long ResourceId, string MonthYear, string StatusId, string PatientTypeId, string VisitTypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(FacilityId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(MonthYear)))
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
                    DSAppointment dsSchedule = null;

                    BLObject<DSAppointment> obj = BLLScheduleObj.SelectMonthlyAppointment(ProviderId, FacilityId, ResourceId, MonthYear, StatusId, PatientTypeId, VisitTypeId);
                    // BLObject<DSAppointment> obj =BLLScheduleObj.SelectMonthlyAppointment(42, 32, ResourceId, "01-2015");
                    if (obj.Data != null)
                    {
                        dsSchedule = obj.Data;

                        if (dsSchedule.Tables[dsSchedule.MonthlyAppointment.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,

                                MonthlyAppointmentFill_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.MonthlyAppointment.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,

                                MonthlyAppointmentFill_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.MonthlyAppointment.TableName]),
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

        #endregion

        #region "Select Weekly Appointment"

        private string SelectWeeklyAppointment(long ProviderId, long FacilityId, long ResourceId, string StartDate, string EndDate, string DaysOfWeek)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(FacilityId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProviderId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(StartDate)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(EndDate)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(DaysOfWeek)))
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
                    DSAppointment dsSchedule = null;

                    BLObject<DSAppointment> obj = BLLScheduleObj.SelectWeeklyAppointment(42, 32, ResourceId, "2014-12-28", "2015-01-05", "1,2,3,4,5,6,7");
                    dsSchedule = obj.Data;

                    if (dsSchedule.Tables[dsSchedule.WeeklyAppointment.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,

                            WeeklyAppointmentFill_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.WeeklyAppointment.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    if (dsSchedule.Tables[dsSchedule.MonthlyAppointment.TableName].Rows.Count <= 0)
                    {
                        var response = new
                        {
                            status = false,

                            WeeklyAppointmentFill_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.WeeklyAppointment.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    var response1 = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));

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

        #region "Select Weekly Slot Appointment"

        private string LoadWeeklySlotAppointment(Int64 ProviderId, Int64 Resourceid, Int64 Facilityid, string checkdays, string DatesString, string StatusId, string PatientTypeId, string VisitTypeId)
        {
            List<object> Allrecord = new List<object>();

            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(Facilityid)))
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

                    string[] WeekDates = DatesString.ToString().Split('|');

                    for (int wd = 0; wd < WeekDates.Length; wd++)
                    {
                        if (WeekDates[wd] != "")
                        {
                            string criteriadate = WeekDates[wd];
                            string response = SearchWeekDaySlotSchedule(MDVUtility.ToInt64(ProviderId), MDVUtility.ToInt64(Resourceid), MDVUtility.ToInt64(Facilityid), criteriadate, StatusId, PatientTypeId, VisitTypeId);
                            Allrecord.Add(response);
                        }
                    }

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(Allrecord));

                }
                return "";
                // return (Newtonsoft.Json.JsonConvert.SerializeObject(Allrecord));
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

        #region "Cut and Paste Appointments"

        private string MoveAppointment(string AppointmentId, string TimeSlotDtlId)
        {
            try
            {

                {
                    BLObject<string> obj = BLLScheduleObj.MoveAppointment(AppointmentId, TimeSlotDtlId, null, null);
                    if (obj.Data == "Moved successfully")
                    {
                        var response = new
                        {
                            status = true,
                            Message = obj.Data
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

        private string RescheduleAppointment(string AppointmentId, string TimeSlotDtlId, string NewDate, string NewTime)
        {
            try
            {

                {
                    BLObject<string> obj = BLLScheduleObj.MoveAppointment(AppointmentId, TimeSlotDtlId, NewDate, NewTime, "1");
                    if (obj.Data == "Moved successfully")
                    {
                        var response = new
                        {
                            status = true,
                            Message = "Appointment Rescheduled Successfully"
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
        /// <summary>
        /// Load Provider ALL appointment on multiple facilites for print
        /// </summary>
        /// <param name="ProviderId"></param>
        /// <param name="FacilityId"></param>
        /// <param name="AppointmentDate"></param>
        /// <returns></returns>
        private string LoadProviderAppointmentPrint(string ProviderId, string FacilityId, string AppointmentDate, string SchStatusIds, string ResourceId)
        {
            try
            {
                #region AppointmentStatusIds Table
                DataColumn COLUMN = new DataColumn();
                DataTable dtAppointmentStatus = new DataTable();
                COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                dtAppointmentStatus.Columns.Add(COLUMN);
                if (!string.IsNullOrWhiteSpace(SchStatusIds))
                {
                    string[] strArry = SchStatusIds.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        DataRow Dr = dtAppointmentStatus.NewRow();
                        Dr[0] = strArry[i];
                        dtAppointmentStatus.Rows.Add(Dr);
                    }
                }
                else
                {
                    DataRow Dr = dtAppointmentStatus.NewRow();
                    Dr[0] = 0;
                    dtAppointmentStatus.Rows.Add(Dr);
                }
                #endregion

                List<ProviderAppointmentPrint> ProviderAppointmentPrintList = null;
                BLObject<List<ProviderAppointmentPrint>> objProviderAppointmentPrint;
                objProviderAppointmentPrint = BLLScheduleObj.LoadProviderAppointmentPrint(ProviderId, FacilityId, AppointmentDate, dtAppointmentStatus, ResourceId);
                ProviderAppointmentPrintList = objProviderAppointmentPrint.Data;
                if (objProviderAppointmentPrint.Data != null)
                {
                    if (ProviderAppointmentPrintList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ProviderAppointmentPrintListCount = ProviderAppointmentPrintList.Count,

                            ProviderAppointmentPrintListInfo_JSON = ProviderAppointmentPrintList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            UnAllocatedCopayListRecordCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objProviderAppointmentPrint.Message
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region "Update Slot Status"

        private string UpdateSlotStatus(string SlotId, long BlockReasonId, string BlockStatus, string Comments)
        {
            try
            {

                {
                    BLObject<string> obj = BLLScheduleObj.UpdateSlotStatus(SlotId, BlockReasonId, BlockStatus, Comments);
                    if (obj.Data == "")
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
                            status = true,
                            Message = obj.Data
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        #endregion

        #region "Get All Appointment Statuses"
        private string GetAppointmentStatuses()
        {
            try
            {

                DSScheduleLookups dsSchedule = null;


                BLObject<DSScheduleLookups> obj = BLLScheduleObj.LookupAppointmentStatus();
                dsSchedule = obj.Data;

                if (dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,

                        AppointmentStatuses_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                if (dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows.Count <= 0)
                {
                    var response = new
                    {
                        status = false,

                        AppointmentStatuses_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    // }

                }
                return "";
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

        #region "Get Patient Appointment and Insurance Detail"
        private string GetPatientDetails(Int64 PatientId, Int64 AppointmentId)
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

                    DSPatient dsPatientDetail = null;
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatientAndInsuranceById(PatientId, AppointmentId, 0, "Calendar");
                    dsPatientDetail = obj.Data;

                    if (dsPatientDetail.Tables[dsPatientDetail.Patients.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatientDetail.Tables[dsPatientDetail.Patients.TableName].Rows[0];
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var keyValues = new Dictionary<string, string>
                        {
                            //{ "txtAccountNo", MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName])},
                            //{ "txtFullName", MDVUtility.ToStr(dr[dsPatient.Patients.FullNameColumn.ColumnName])},
                            //{ "dtpDOB", MDVUtility.ToStr(dr[dsPatient.Patients.DOBColumn.ColumnName])},
                            //{ "hfpatientid", MDVUtility.ToStr(dr[dsPatient.Patients.PatientIdColumn.ColumnName])},

                        };
                        DSAppointment dsApp = new DSAppointment();
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            PatientDetail_JSON = MDVUtility.JSON_DataTable(dsPatientDetail.Tables[dsPatientDetail.Patients.TableName]),
                            PatientInsuranceDetail_JSON = MDVUtility.JSON_DataTable(dsPatientDetail.Tables[dsPatientDetail.PatientInsurance.TableName]),
                            PatientAppointment_JSON = MDVUtility.JSON_DataTable(dsPatientDetail.Tables[dsApp.PatientAppointments.TableName])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                return "";
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

        #region Week days settings in Entity User options
        private string FillDefaultSetting(string userName)
        {
            try
            {

                DSUsers dsUser = new DSUsers();
                DSUsers dsUser1 = new DSUsers();
                BLObject<DSUsers> obj = null;
                obj = BLLAdminSecurityObj.LoadEntityUserOption(ref dsUser1, userName, MDVSession.Current.EntityId);
                dsUser = obj.Data;
                if (obj.Data != null)
                {
                    if (dsUser.Tables[dsUser.EntityUserOption.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            EntityGroupSttings_JSON = MDVUtility.JSON_DataTable(dsUser.Tables[dsUser.EntityUserOption.TableName]),
                            EntityGroupCount = dsUser.Tables[dsUser.EntityUserOption.TableName].Rows.Count,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EntityGroupCount = 0,
                            EntityGroupSttings_JSON = MDVUtility.JSON_DataTable(dsUser.Tables[dsUser.EntityUserOption.TableName]),
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
        private string UpdateDefaultSetting(string fieldsJSON, int EntityUserOptionId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSUsers dsDefaultsetting = new DSUsers();

                DSUsers dsUser1 = new DSUsers();
                BLObject<DSUsers> objUser = null;
                objUser = BLLAdminSecurityObj.LoadEntityUserOption(ref dsUser1, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), MDVSession.Current.EntityId);
                dsDefaultsetting = objUser.Data;
                foreach (DSUsers.EntityUserOptionRow dr in dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows)
                {
                    //dr.EntityUserOptionId = EntityUserOptionId;
                    dr.UserId = MDVSession.Current.AppUserId;
                    dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    dr.EntityRegCode = MDVSession.Current.EntityRegCode;
                    dr.IsActive = true;
                    dr.IsDefault = false;

                    dr.SchedulePattern = (SearchedfieldsJSON["chkcalMonday"] + "," + SearchedfieldsJSON["chkcalTuesday"] + "," + SearchedfieldsJSON["chkcalWednesday"] + "," + SearchedfieldsJSON["chkcalThursday"] + "," + SearchedfieldsJSON["chkcalFriday"] + "," + SearchedfieldsJSON["chkcalSaturday"] + "," + SearchedfieldsJSON["chkcalSunday"]).Replace("True", "1").Replace("False", "0");

                    //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    //dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                }

                #region Database Updation
                //dsDefaultsetting.EntityUserOption.AddEntityUserOptionRow(dr);
                //dsDefaultsetting.EntityUserOption.AcceptChanges();

                if (dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows.Count > 0)
                {
                    //dsDefaultsetting.EntityUserOption.Rows[0].SetModified();
                    BLObject<DSUsers> obj = BLLAdminSecurityObj.UpdatDefaultSettings(dsDefaultsetting);
                    if (obj.Data != null)
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
        private string SaveDefaultSetting(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSUsers dsDefaultsetting = new DSUsers();
                DSUsers.EntityUserOptionRow dr = dsDefaultsetting.EntityUserOption.NewEntityUserOptionRow();


                dr.SchedulePattern = (SearchedfieldsJSON["chkcalMonday"] + "," + SearchedfieldsJSON["chkcalTuesday"] + "," + SearchedfieldsJSON["chkcalWednesday"] + "," + SearchedfieldsJSON["chkcalThursday"] + "," + SearchedfieldsJSON["chkcalFriday"] + "," + SearchedfieldsJSON["chkcalSaturday"] + "," + SearchedfieldsJSON["chkcalSunday"]).Replace("True", "1").Replace("False", "0");

                dr.UserId = MDVSession.Current.AppUserId;
                dr.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                dr.EntityRegCode = MDVSession.Current.EntityRegCode;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.IsDefault = true;
                dr.IsActive = true;
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;




                #region Database Insertion
                dsDefaultsetting.EntityUserOption.AddEntityUserOptionRow(dr);
                BLObject<DSUsers> obj = BLLAdminSecurityObj.InsertDefaultSettings(dsDefaultsetting);
                dsDefaultsetting = obj.Data;
                if (obj.Data != null)
                {
                    //SetApplicationConfig(dsDefaultsetting);
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        EntityUserOptionId = dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows[0][dsDefaultsetting.EntityUserOption.EntityUserOptionIdColumn.ColumnName]
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

        #endregion

        #region"week select new function"

        private string LoadWeeklySlotAppointmentNew(Int64 ProviderId, Int64 Facilityid, Int64 Resourceid, string checkdays, string DatesString, string StatusId, string PatientTypeId, string VisitTypeId)
        {
            List<object> Allrecord = new List<object>();
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(Facilityid)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (string.IsNullOrEmpty(MDVUtility.ToStr(DatesString)))
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
                    DSAppointment dsSchedule = null;

                    string scheduleDates = "";
                    string scheduleDays = "";
                    string[] WeekDates = DatesString.Split('|');

                    var xx = new List<string>();
                    var yy = new List<string>();

                    for (int wd = 0; wd < WeekDates.Length; wd++)
                    {
                        if (WeekDates[wd] != "")
                        {
                            string[] singledate = WeekDates[wd].Split(',');
                            string day = singledate[0];
                            string passingdate = singledate[1];
                            scheduleDates += singledate[1] + ",";
                            scheduleDays += singledate[0] + ",";

                            xx.Add(singledate[0]);
                            yy.Add(singledate[1]);
                        }
                    }

                    BLObject<DSAppointment> obj = BLLScheduleObj.SelectWeeklySlots(ProviderId, Resourceid, Facilityid, scheduleDates, StatusId, PatientTypeId, VisitTypeId);
                    dsSchedule = obj.Data;
                    BLLScheduleObj.ScheduleAppSort(ref dsSchedule);
                    if (dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count > 0)
                    {
                        for (int i = 0; i < xx.Count; i++)
                        {

                            DataTable table = new DataTable();
                            table = dsSchedule.DaySlots.Clone();
                            table.Clear();

                            foreach (DSAppointment.DaySlotsRow dr in dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows)
                            {
                                if (dr.Day == xx[i])
                                {
                                    table.ImportRow(dr);
                                }
                            }
                            if (table.Rows.Count > 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    tableday = xx[i],
                                    tabledate = yy[i],
                                    ProviderScheduleFill_JSON = MDVUtility.JSON_DataTable(table),
                                    addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                                    editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                                    viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),

                                };
                                Allrecord.Add(Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    tableday = xx[i],
                                    tabledate = yy[i],
                                    ProviderScheduleFill_JSON = MDVUtility.JSON_DataTable(table),
                                    addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                                    editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                                    viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),

                                };
                                Allrecord.Add(Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(Allrecord));
                    }
                    else
                    {
                        for (int i = 0; i < xx.Count; i++)
                        {
                            DataTable table = new DataTable();
                            table = dsSchedule.DaySlots.Clone();
                            table.Clear();
                            var response = new
                            {
                                status = false,
                                tableday = xx[i],
                                tabledate = yy[i],
                                ProviderScheduleFill_JSON = MDVUtility.JSON_DataTable(table),
                                addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                                editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                                viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
                            };
                            Allrecord.Add(Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(Allrecord));
                    }


                }
                return "";
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                    addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                    editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                    viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
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
                case "SEARCH_PROVIDERSCHEDULE":
                    {
                        string strProviderId = context.Request["ProviderID"];
                        string strResourceId = context.Request["ResourceID"];
                        string strFacilityd = context.Request["FacilityID"];
                        string SlotDate = context.Request["SlotDate"];
                        string StatusId = context.Request["StatusId"];
                        string PatientTypeId = context.Request["PatientTypeId"];
                        string VisitTypeId = context.Request["VisitTypeId"];
                        string strJSONData = SearchDaySlotSchedule(MDVUtility.ToInt64(strProviderId), MDVUtility.ToInt64(strResourceId), MDVUtility.ToInt64(strFacilityd), MDVUtility.ToStr(SlotDate), MDVUtility.ToStr(StatusId), MDVUtility.ToStr(PatientTypeId), MDVUtility.ToStr(VisitTypeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_WEEKPROVIDERSCHEDULE":
                    {
                        string strProviderId = context.Request["ProviderID"];
                        string strResourceId = context.Request["ResourceID"];
                        string strFacilityd = context.Request["FacilityID"];
                        string SlotDate = context.Request["SlotDate"];
                        string StatusId = context.Request["StatusId"];
                        string PatientTypeId = context.Request["PatientTypeId"];
                        string VisitTypeId = context.Request["VisitTypeId"];
                        string strJSONData = SearchWeekDaySlotSchedule(MDVUtility.ToInt64(strProviderId), MDVUtility.ToInt64(strResourceId), MDVUtility.ToInt64(strFacilityd), MDVUtility.ToStr(SlotDate), MDVUtility.ToStr(StatusId), MDVUtility.ToStr(PatientTypeId), MDVUtility.ToStr(VisitTypeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "MOVE_APPOINTMENT":
                    {

                        string AppointmentID = MDVUtility.ToStr(context.Request["AppointmentID"]);
                        string TimeSlotDtlId = MDVUtility.ToStr(context.Request["TimeSlotDtlId"]);
                        string strJSONData = MoveAppointment(AppointmentID, TimeSlotDtlId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SELECT_MONTH_APPOINTMENT":
                    {
                        string strProviderId = context.Request["ProviderID"];
                        string strFacilityd = context.Request["FacilityID"];
                        string strResourceId = context.Request["ResourceID"];
                        string MonthYear = context.Request["MonthYear"];
                        string StatusId = context.Request["StatusId"];
                        string PatientTypeId = context.Request["PatientTypeId"];
                        string VisitTypeId = context.Request["VisitTypeId"];
                        string strJSONData = SelectMonthlyAppointment(MDVUtility.ToInt64(strProviderId), MDVUtility.ToInt64(strFacilityd), MDVUtility.ToInt64(strResourceId), MDVUtility.ToStr(MonthYear), MDVUtility.ToStr(StatusId), MDVUtility.ToStr(PatientTypeId), MDVUtility.ToStr(VisitTypeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SELECT_WEEKLY_APPOINTMENT":
                    {
                        string strProviderId = context.Request["ProviderID"];
                        string strFacilityd = context.Request["FacilityID"];
                        string strResourceId = context.Request["ResourceID"];
                        string strStartDate = context.Request["StartDate"];
                        string strEndDate = context.Request["EndDate"];
                        string strDaysOfWeek = context.Request["DaysOfWeek"];

                        string strJSONData = SelectWeeklyAppointment(MDVUtility.ToInt64(strProviderId), MDVUtility.ToInt64(strFacilityd), MDVUtility.ToInt64(strResourceId), MDVUtility.ToStr(strStartDate), MDVUtility.ToStr(strEndDate), MDVUtility.ToStr(strDaysOfWeek));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SLOT_STATUS":
                    {

                        string SlotId = MDVUtility.ToStr(context.Request["SlotId"]);
                        string BlockReasonId = MDVUtility.ToStr(context.Request["BlockReasonId"]);
                        string BlockStatus = MDVUtility.ToStr(context.Request["BlockStatus"]);
                        string Comments = MDVUtility.ToStr(context.Request["Comments"]);
                        string strJSONData = UpdateSlotStatus(SlotId, MDVUtility.ToInt64(BlockReasonId), BlockStatus, Comments);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SELECT_APPOINTMENT_STATUSES":
                    {
                        string strJSONData = GetAppointmentStatuses();

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "GET_PATIENT_DETAILS":
                    {
                        Int64 PatientId = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 AppointmentId = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        string strJSONData = GetPatientDetails(PatientId, AppointmentId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SELECT_WEEKLY_SLOT_APP":
                    {
                        string strProviderId = context.Request["ProviderID"];
                        string strResourceId = context.Request["ResourceID"];
                        string strFacilityd = context.Request["FacilityID"];
                        string strcheckdays = context.Request["checkeddays"];
                        string strdatestring = context.Request["DatesString"];
                        string StatusId = context.Request["StatusId"];
                        string PatientTypeId = context.Request["PatientTypeId"];
                        string VisitTypeId = context.Request["VisitTypeId"];
                        string strJSONData = LoadWeeklySlotAppointmentNew(MDVUtility.ToInt64(strProviderId), MDVUtility.ToInt64(strFacilityd), MDVUtility.ToInt64(strResourceId), MDVUtility.ToStr(strcheckdays), MDVUtility.ToStr(strdatestring), MDVUtility.ToStr(StatusId), MDVUtility.ToStr(PatientTypeId), MDVUtility.ToStr(VisitTypeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                // Week days against User Entity
                case "FILL_DEFAULT_SETTINGS":
                    {
                        string userName = MDVUtility.ToStr(context.Request["userName"]);
                        string strJSONData = FillDefaultSetting(userName);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);

                    }
                    break;

                case "SAVE_DEFAULT_SETTING":
                    {

                        string fieldsJSON = context.Request["DefaultSettingData"];
                        string strJSONData = SaveDefaultSetting(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_DEFAULT_SETTING":
                    {
                        string fieldsJSON = context.Request["DefaultSettingData"];
                        int EntityUserOptionId = MDVUtility.ToInt32(context.Request["EntityUserOptionId"]);
                        string strJSONData = UpdateDefaultSetting(fieldsJSON, EntityUserOptionId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "RESCHEDULE_APPOINTMENT":
                    {

                        string AppointmentID = MDVUtility.ToStr(context.Request["AppointmentID"]);
                        string NewDate = MDVUtility.ToStr(context.Request["NewDate"]);
                        string NewTime = MDVUtility.ToStr(context.Request["NewTime"]);
                        string strJSONData = RescheduleAppointment(AppointmentID, null, NewDate, NewTime);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "PROVIDER_APPOINTMENT_PRINT":
                    {
                        string ProviderId = MDVUtility.ToStr(context.Request["ProviderId"]);
                        string FacilityId = MDVUtility.ToStr(context.Request["FacilityId"]);
                        string AppointmentDate = MDVUtility.ToStr(context.Request["AppointmentDate"]);
                        string SchStatusIds = MDVUtility.ToStr(context.Request["SchStatusIds"]);
                        string ResourceId = MDVUtility.ToStr(context.Request["ResourceId"]);
                        string strJSONData = LoadProviderAppointmentPrint(ProviderId, FacilityId, AppointmentDate, SchStatusIds, ResourceId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);

                    }
                    break;
            }
        }
        #endregion

    }
}