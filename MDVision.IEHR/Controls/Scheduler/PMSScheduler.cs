using System;
using System.Collections.Generic;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using Newtonsoft.Json;
using MDVision.IEHR.Common;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Model.Schedule;
using MDVision.Model.PMSSchedule;
using MDVision.Model.Native.Scheduler;

namespace MDVision.IEHR.Controls.Scheduler
{
    public class PMSSchedulerHelper
    {
        private BLLPatient BLLPatientObj = null;
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        private BLLSchedule BLLScheduleObj = null;

        public PMSSchedulerHelper()
        {
            BLLPatientObj = new BLLPatient();
            BLLAdminSecurityObj = new BLLAdminSecurity();
            BLLScheduleObj = new BLLSchedule();
        }

        #region Singleton
        private static PMSSchedulerHelper _obj = null;
        public static PMSSchedulerHelper Instance()
        {
            if (_obj == null)
                _obj = new PMSSchedulerHelper();
            return _obj;
        }
        #endregion

        #region "Search Day Slot Schedule"

        public string EDIEligibilityIdSelect(AppointmentModel model)
        {
            string strResult = string.Empty;
            try
            {
                strResult = BLLScheduleObj.EDIEligibilityIdSelect(model);
                var response = new
                {
                    status = true,
                    EDIEligibilityId = strResult,
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

        public string GetProviderSchedule(AppointmentModel model)
        {
            try
            {
                #region " ProviderIds Table "

                DataTable dtProvider = new DataTable();
                DataColumn COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                dtProvider.Columns.Add(COLUMN);
                if (!string.IsNullOrWhiteSpace(model.ProviderIds))
                {
                    string[] strArry = model.ProviderIds.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        DataRow Dr = dtProvider.NewRow();
                        Dr[0] = strArry[i];
                        dtProvider.Rows.Add(Dr);
                    }
                }
                else
                {
                    DataRow Dr = dtProvider.NewRow();
                    Dr[0] = 0;
                    dtProvider.Rows.Add(Dr);
                }

                #endregion  " ProviderIds Table "

                #region " FacilityIds Table "

                DataTable dtFacility = new DataTable();
                COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                dtFacility.Columns.Add(COLUMN);
                if (!string.IsNullOrWhiteSpace(model.FacilityIds))
                {
                    string[] strArry = model.FacilityIds.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        DataRow Dr = dtFacility.NewRow();
                        Dr[0] = strArry[i];
                        dtFacility.Rows.Add(Dr);
                    }
                }
                else
                {
                    DataRow Dr = dtFacility.NewRow();
                    Dr[0] = 0;
                    dtFacility.Rows.Add(Dr);
                }
                #endregion " FacilityIds Table "

                BLObject<List<AppointmentModel>> obj = new BLObject<List<AppointmentModel>>();
                if (model.IsProvider)
                {
                    obj = BLLScheduleObj.LoadProviderAppointmentDayViewScheduel(dtProvider, dtFacility, model.StartDate,model.SchViewType);
                }
                else
                {
                    obj = BLLScheduleObj.LoadResourceAppointmentDayViewScheduel(dtProvider, dtFacility, model.StartDate);
                }

                if (obj.Data != null)
                {
                    List<AppointmentModel> listSchedules = obj.Data;
                    var response = new
                    {
                        status = true,
                        ScheduleSlotsFill_JSON = listSchedules,

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
        public string SearchAppointmentSchedule(AppointmentModel model)
        {
            try
            {
                List<AppointmentModel> listApointment = new List<AppointmentModel>();
                List<AppointmentModel> listSchedules = new List<AppointmentModel>();
                List<AppointmentModel> listWorkWeekSchedules = new List<AppointmentModel>();
                List<AppointmentModel> listBlockHours = new List<AppointmentModel>();
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.StartDate)))
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
                    BLObject<List<AppointmentModel>> obj = null;
                    BLObject<List<AppointmentModel>> objProviderSchedule = null;
                    BLObject<List<AppointmentModel>> objWorkWeekSchedule = null;
                    #region ProviderIds Table

                    DataTable dtProvider = new DataTable();
                    DataColumn COLUMN = new DataColumn();
                    COLUMN.ColumnName = "Id";
                    COLUMN.DataType = typeof(int);
                    dtProvider.Columns.Add(COLUMN);
                    if (!string.IsNullOrWhiteSpace(model.ProviderIds))
                    {
                        string[] strArry = model.ProviderIds.Split(',');
                        for (int i = 0; i < strArry.Length; i++)
                        {
                            DataRow Dr = dtProvider.NewRow();
                            Dr[0] = strArry[i];
                            dtProvider.Rows.Add(Dr);
                        }
                    }
                    else
                    {
                        DataRow Dr = dtProvider.NewRow();
                        Dr[0] = 0;
                        dtProvider.Rows.Add(Dr);
                    }

                    #endregion  ProviderIds Table

                    #region ResourceIds Table

                    DataTable dtResource = new DataTable();
                    COLUMN = new DataColumn();
                    COLUMN.ColumnName = "Id";
                    COLUMN.DataType = typeof(int);
                    dtResource.Columns.Add(COLUMN);
                    if (!string.IsNullOrWhiteSpace(model.ResourceIds))
                    {
                        string[] strArry = model.ResourceIds.Split(',');
                        for (int i = 0; i < strArry.Length; i++)
                        {
                            DataRow Dr = dtResource.NewRow();
                            Dr[0] = strArry[i];
                            dtResource.Rows.Add(Dr);
                        }
                    }
                    else
                    {
                        DataRow Dr = dtResource.NewRow();
                        Dr[0] = 0;
                        dtResource.Rows.Add(Dr);
                    }
                    #endregion ResourceIds Table

                    #region FacilityIds Table

                    DataTable dtFacility = new DataTable();
                    COLUMN = new DataColumn();
                    COLUMN.ColumnName = "Id";
                    COLUMN.DataType = typeof(int);
                    dtFacility.Columns.Add(COLUMN);
                    if (!string.IsNullOrWhiteSpace(model.FacilityIds))
                    {
                        string[] strArry = model.FacilityIds.Split(',');
                        for (int i = 0; i < strArry.Length; i++)
                        {
                            DataRow Dr = dtFacility.NewRow();
                            Dr[0] = strArry[i];
                            dtFacility.Rows.Add(Dr);
                        }
                    }
                    else
                    {
                        DataRow Dr = dtFacility.NewRow();
                        Dr[0] = 0;
                        dtFacility.Rows.Add(Dr);
                    }
                    #endregion FacilityIds Table

                    #region VisitTypeIds Table
                    DataTable dtVisitType = new DataTable();
                    COLUMN = new DataColumn();
                    COLUMN.ColumnName = "Id";
                    COLUMN.DataType = typeof(int);
                    dtVisitType.Columns.Add(COLUMN);
                    if (!string.IsNullOrWhiteSpace(model.VisitTypeIds))
                    {
                        string[] strArry = model.VisitTypeIds.Split(',');
                        for (int i = 0; i < strArry.Length; i++)
                        {
                            DataRow Dr = dtVisitType.NewRow();
                            Dr[0] = strArry[i];
                            dtVisitType.Rows.Add(Dr);
                        }
                    }
                    else
                    {
                        DataRow Dr = dtVisitType.NewRow();
                        Dr[0] = 0;
                        dtVisitType.Rows.Add(Dr);
                    }
                    #endregion VisitTypeIds Table

                    #region AppointmentStatusIds Table
                    DataTable dtAppointmentStatus = new DataTable();
                    COLUMN = new DataColumn();
                    COLUMN.ColumnName = "Id";
                    COLUMN.DataType = typeof(int);
                    dtAppointmentStatus.Columns.Add(COLUMN);
                    if (!string.IsNullOrWhiteSpace(model.AppointmentStatusIds))
                    {
                        string[] strArry = model.AppointmentStatusIds.Split(',');
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
                    #endregion VisitTypeIds Table
                    string viewTpe = "0";
                    switch (model.SchViewType)
                    {
                        case "day":
                            if (model.IsResourceSch)
                            {
                                obj = BLLScheduleObj.LoadResourceAppointmentDayView(dtResource, dtFacility, model.PatientTypeId, dtVisitType, dtAppointmentStatus, model.StartDate);

                            }
                            else
                            {
                                obj = BLLScheduleObj.LoadProviderAppointmentDayView(dtProvider, dtFacility, model.PatientTypeId, dtVisitType, dtAppointmentStatus, model.StartDate);

                            }
                            break;
                        case "week":
                            viewTpe = "1";
                            if (model.IsResourceSch)
                            {
                                obj = BLLScheduleObj.LoadResourceAppointmentWeekView(dtResource, dtFacility, model.PatientTypeId, dtVisitType, dtAppointmentStatus, model.StartDate, model.EndDate);

                            }
                            else
                            {
                                obj = BLLScheduleObj.LoadProviderAppointmentWeekView(dtProvider, dtFacility, model.PatientTypeId, dtVisitType, dtAppointmentStatus, model.StartDate, model.EndDate);

                            }
                            break;
                        case "workWeek":
                            viewTpe = "2";
                            string WorkWeekDates = string.Empty;
                            if (!string.IsNullOrWhiteSpace(model.WorkWeekDays))
                            {
                                DateTime StartdDate = MDVUtility.ToDateTime(model.StartDate);
                                int diff = (7 + (StartdDate.DayOfWeek - DayOfWeek.Sunday)) % 7;
                                StartdDate = StartdDate.AddDays(-1 * diff).Date;
                                var Days = model.WorkWeekDays.Split(',');
                                foreach (var day in Days)
                                {
                                    if (!string.IsNullOrWhiteSpace(day))
                                    {
                                        if (day == "1")
                                        {
                                            WorkWeekDates += StartdDate.ToString("MM/dd/yyyy");
                                            WorkWeekDates += ",";
                                        }
                                        StartdDate = StartdDate.AddDays(1);
                                    }
                                }
                                WorkWeekDates = WorkWeekDates.Substring(0, WorkWeekDates.LastIndexOf(','));
                            }
                            if (model.IsResourceSch)
                            {
                                obj = BLLScheduleObj.LoadResourceAppointmentWorkWeekView(dtResource, dtFacility, model.PatientTypeId, dtVisitType, dtAppointmentStatus, model.StartDate, model.EndDate, WorkWeekDates);
                                objWorkWeekSchedule = BLLScheduleObj.LoadResourceWorkWeekSchedules(dtResource, dtFacility, model.StartDate);
                            }
                            else
                            {
                                obj = BLLScheduleObj.LoadProviderAppointmentWorkWeekView(dtProvider, dtFacility, model.PatientTypeId, dtVisitType, dtAppointmentStatus, model.StartDate, model.EndDate, WorkWeekDates);
                                objWorkWeekSchedule = BLLScheduleObj.LoadProviderWorkWeekSchedules(dtProvider, dtFacility, model.StartDate);
                            }

                            break;
                        case "month":
                            viewTpe = "3";
                            if (model.IsResourceSch)
                            {
                                obj = BLLScheduleObj.LoadResourceAppointmentMonthView(dtResource, dtFacility, model.PatientTypeId, dtVisitType, dtAppointmentStatus, model.Month, model.Year);
                            }
                            else
                            {
                                obj = BLLScheduleObj.LoadProviderAppointmentMonthView(dtProvider, dtFacility, model.PatientTypeId, dtVisitType, dtAppointmentStatus, model.Month, model.Year);
                            }

                            break;
                        default:
                            obj = BLLScheduleObj.LoadProviderAppointmentDayView(dtProvider, dtFacility, model.PatientTypeId, dtVisitType, dtAppointmentStatus, model.StartDate);
                            break;
                    }
                    if (model.IsResourceSch)
                    {
                        objProviderSchedule = BLLScheduleObj.LoadResourceAppointmentDayViewScheduel(dtResource, dtFacility, model.StartDate);
                    }
                    else
                    {
                        objProviderSchedule = BLLScheduleObj.LoadProviderAppointmentDayViewScheduel(dtProvider, dtFacility, model.StartDate,model.SchViewType);
                    }

                    BLObject<DSScheduleSetup> objBlockHours = BLLScheduleObj.LoadSchBlockHours(dtProvider, dtResource, MDVUtility.ToDateTime(model.StartDate), viewTpe);
                    dsSchedule = objBlockHours.Data;



                    if (obj.Data != null)
                    {
                        listApointment = obj.Data;
                        listSchedules = (objProviderSchedule != null && objProviderSchedule.Data != null) ? objProviderSchedule.Data : null;
                        listWorkWeekSchedules = (objWorkWeekSchedule != null && objWorkWeekSchedule.Data != null) ? objWorkWeekSchedule.Data : null;

                        if (listApointment != null && listApointment.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                                editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                                viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),

                                ProviderScheduleFill_JSON = listApointment,
                                ScheduleSlotsFill_JSON = listSchedules,
                                WorkWeekSchedules = listWorkWeekSchedules,

                                BlockHoursCount = dsSchedule != null ? dsSchedule.Tables[dsSchedule.BlockHoursSlots.TableName].Rows.Count : 0,
                                BlockHoursLoad_JSON = dsSchedule != null ? MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.BlockHoursSlots.TableName]) : null,

                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.No_Record_Message,
                                addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                                editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                                viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
                                ProviderScheduleFill_JSON = listApointment,
                                ScheduleSlotsFill_JSON = listSchedules,
                                WorkWeekSchedules = listWorkWeekSchedules,
                                BlockHoursCount = dsSchedule != null ? dsSchedule.Tables[dsSchedule.BlockHoursSlots.TableName].Rows.Count : 0,
                                BlockHoursLoad_JSON = dsSchedule != null ? MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.BlockHoursSlots.TableName]) : null,
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

        public string LoadWaitListSchedule(AppointmentModel model)
        {
            try
            {
                List<AppointmentModel> listApointment = new List<AppointmentModel>();
                BLObject<List<AppointmentModel>> obj = null;

                if (model.ResourceId == 0)
                {
                    obj = BLLScheduleObj.LoadWaitListSchedule(model);

                }
                else
                {
                    obj = BLLScheduleObj.LoadResourceWaitListSchedule(model);
                }


                if (obj.Data != null)
                {
                    listApointment = obj.Data;


                    if (listApointment != null && listApointment.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            //addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                            //editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                            //viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),

                            ProviderScheduleFill_JSON = listApointment,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.No_Record_Message,
                            //addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                            //editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                            //viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
                            ProviderScheduleFill_JSON = listApointment,
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
                        //addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                        //editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                        //viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),

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
        private string GetAge(DateTime birthday)
        {
            try
            {
                //int age = ((DateTime.Now.Year - birthday.Year) * 372 + (DateTime.Now.Month - birthday.Month) * 31 + (DateTime.Now.Day - birthday.Day)) / 372;

                //int age2 = Convert.ToInt32(Math.Round(today.Subtract(birthday).TotalDays * 0.00273790926));

                DateTime today = DateTime.Now;
                //TimeSpan ts = today - birthday;
                //int Age3 = ts.Days / 365;
                // DateTime Age = DateTime.MinValue.AddDays(ts.Days);

                int days = today.Day - birthday.Day;
                if (days < 0)
                {
                    today = today.AddMonths(-1);
                    days += DateTime.DaysInMonth(today.Year, today.Month);
                }

                int months = today.Month - birthday.Month;
                if (months < 0)
                {
                    today = today.AddYears(-1);
                    months += 12;
                }

                int years = today.Year - birthday.Year;


                string ActAge = string.Format(" {0} Year(s), {1} Month(s), {2} Day(s)  ", years,
                             months,
                             days);

                return ActAge;
            }
            catch (Exception ex)
            {

                return ("");
            }
        }
        public string FillToolTipData(string AppointmentId)
        {
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
                    AppointmentTooltipModel obj = new AppointmentTooltipModel();
                    obj = BLLScheduleObj.FillToolTipData(AppointmentId);


                    DateTime birthDate = MDVUtility.ToDateTime(obj.DOB);

                    obj.PatientAge = GetAge(birthDate);



                    bool lByteImgPath = true;
                    string imageBase64 = "";

                    //byte[] imageByteArr = dr[dsPatient.Patients.PatientImageColumn.ColumnName] as byte[];

                    string filetype = obj.ImageType as string;
                    //string strBase64 = Convert.ToBase64String(dr[dsPatient.Patients.PatientImageColumn.ColumnName] as byte[]);
                    //string FileName = string.Empty;

                    //  response_model.PatientProfileImagePath = MDVUtility.ToStr(dr[dsPatient.Patients.PatientProfileImagePathColumn]);


                    string lfileName = obj.PatientProfileImagePath;
                    //  string lfileNameThumbnail = MDVUtility.ToStr(dr[dsPatient.Patients.PatientProfileThumbnailPathColumn]);


                    string ServerPathForLoadFile = System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesPath"];

                    if (System.IO.Directory.Exists(ServerPathForLoadFile))
                    {
                        if (!lfileName.Equals(""))
                        {

                            //  MDVSession.Current.ImageID = lfileName + "|" + lfileNameThumbnail;


                            //byte[] imageBytes = System.IO.File.ReadAllBytes(System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesDetails"] + "\\" + lfileName);


                            string imgPath = System.IO.Path.Combine(ServerPathForLoadFile, lfileName);

                            if (System.IO.File.Exists(imgPath))
                            {
                                byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                                //byte[] imageBytes = Convert.FromBase64String(lfileName);

                                string base64String = Convert.ToBase64String(imageBytes);

                                //string imgBase64String = GetBase64StringForImage(ServerPathForLoadFile);
                                imageBase64 = "data:" + obj.ImageType + ";base64," + base64String;
                                lByteImgPath = false;
                            }


                        }
                    }
                    if (string.IsNullOrEmpty(imageBase64))

                    {

                        byte[] imageByteArr = obj.PatientImage as byte[];
                        if (imageByteArr != null)
                        {
                            imageBase64 = "data:" + obj.ImageType + ";base64," + Convert.ToBase64String(obj.PatientImage as byte[]);
                        }
                    }

                    if (!string.IsNullOrEmpty(imageBase64))
                    {
                        obj.imgPatient = imageBase64;
                    }
                    //else
                    //{
                    //    string imgPath = "";
                    //    if (obj.Gender == "Male")
                    //    {
                    //        imgPath = "~/~/Content/images/default_male_profile.gif";

                    //    }
                    //    else
                    //    {
                    //        imgPath = "~/~/Content/images/default_female_profile.gif";
                    //    }
                    //    if (System.IO.File.Exists(imgPath))
                    //    {
                    //        byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                    //        //byte[] imageBytes = Convert.FromBase64String(lfileName);

                    //        string base64String = Convert.ToBase64String(imageBytes);

                    //        //string imgBase64String = GetBase64StringForImage(ServerPathForLoadFile);
                    //        imageBase64 = "data:" + obj.ImageType + ";base64," + base64String;
                    //        lByteImgPath = false;
                    //        obj.imgPatient = imageBase64;
                    //    }
                    //}
                    var response = new
                    {
                        status = true,
                        ToolTipDataFill_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(obj)
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

        public string LoadAppointmentStatusOptions(long AppointmentStatusId, string AppointmentStatus)
        {
            try
            {
                if (AppointmentStatusId <= 0 && string.IsNullOrEmpty(AppointmentStatus))
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
                    List<AppointmentStatusOptionModel> obj = new List<AppointmentStatusOptionModel>();
                    obj = BLLScheduleObj.AppointmentStatusOptions(AppointmentStatusId, AppointmentStatus).Data;
                    var response = new
                    {
                        status = true,
                        AppointmentStatusOptions_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(obj)
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
        public string CopyPasteAppointment(AppointmentModel model)
        {
            try
            {
                BLObject<string> obj = BLLScheduleObj.CopyPasteAppointment(model);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        AppointmentId = obj.Data,
                        Message = Common.AppPrivileges.Save_Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        AppointmentId = 0,
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
                    AppointmentId = "0",
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);

            }
        }
        public string CutPasteAppointment(AppointmentModel model)
        {
            try
            {
                BLObject<string> obj = BLLScheduleObj.CutPasteAppointment(model);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        AppointmentId = obj.Data,
                        Message = Common.AppPrivileges.Save_Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        AppointmentId = 0,
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
                    AppointmentId = "0",
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);

            }
        }

        #endregion
        public string SaveAppointmentNative(EmptySlotModel model)
        {
            try
            {

                var rec = BLLScheduleObj.SaveAppointmentNative(model);
                if (rec != "")
                {
                    var response = new
                    {
                        status = true,
                        Message = rec
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = rec
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
                    AppointmentId = "0",
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);

            }
        }
        public string LoadSchBlockHours(AppointmentModel model)
        {
            try
            {
                DSScheduleSetup dsSchedule = null;
                #region ProviderIds Table

                DataTable dtProvider = new DataTable();
                DataColumn COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                dtProvider.Columns.Add(COLUMN);
                if (!string.IsNullOrWhiteSpace(model.ProviderIds))
                {
                    string[] strArry = model.ProviderIds.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        DataRow Dr = dtProvider.NewRow();
                        Dr[0] = strArry[i];
                        dtProvider.Rows.Add(Dr);
                    }
                }
                else
                {
                    DataRow Dr = dtProvider.NewRow();
                    Dr[0] = 0;
                    dtProvider.Rows.Add(Dr);
                }

                #endregion  ProviderIds Table

                #region ResourceIds Table

                DataTable dtResource = new DataTable();
                COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                dtResource.Columns.Add(COLUMN);
                if (!string.IsNullOrWhiteSpace(model.ResourceIds))
                {
                    string[] strArry = model.ResourceIds.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        DataRow Dr = dtResource.NewRow();
                        Dr[0] = strArry[i];
                        dtResource.Rows.Add(Dr);
                    }
                }
                else
                {
                    DataRow Dr = dtResource.NewRow();
                    Dr[0] = 0;
                    dtResource.Rows.Add(Dr);
                }
                #endregion ResourceIds Table
                string viewTpe = "0";
                if (model.SchViewType == "week")
                {
                    viewTpe = "1";
                }
                else if (model.SchViewType == "workWeek")
                {
                    viewTpe = "2";
                }
                else if (model.SchViewType == "month")
                {
                    viewTpe = "3";
                }
                BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadSchBlockHours(dtProvider, dtResource, model.start, viewTpe);

                dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        BlockHoursCount = dsSchedule.Tables[dsSchedule.BlockHoursSlots.TableName].Rows.Count,
                        iTotalDisplayRecords = (dsSchedule.BlockHoursSlots.Rows.Count > 0) ? dsSchedule.BlockHoursSlots.Rows[0][dsSchedule.BlockHoursSlots.RecordCountColumn.ColumnName] : 0,
                        BlockHoursLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.BlockHoursSlots.TableName]),
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        BlockHoursCount = 0,
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

    }
}