using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Model.Admin.Reminders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_Reminders_Detail
    {
          private BLLAdmin BLLAdminObj = null;
          public Admin_Reminders_Detail()
        {
            BLLAdminObj = new BLLAdmin();
        }

        #region Singleton

        private static Admin_Reminders_Detail _instance = null;
        public static Admin_Reminders_Detail Instance()
        {
            if (_instance == null)
                _instance = new Admin_Reminders_Detail();
            return _instance;
        }

        #endregion

        #region Functions
        public string SaveQuickVoiceReminder(RemindersQuickModel model)
        {
            try
            {

                Int32 minuts = 0;
                DateTime currentTime = DateTime.Now;
                if (model.DeliveryMinutes == "no")
                    minuts = Convert.ToInt32(2);
                else
                    minuts = Convert.ToInt32(model.DeliveryMinutes);

                if (minuts == 1)
                    minuts = 60;
                else if (model.DeliveryMinutes != "no" && minuts == 2)
                    minuts = 120;
                else if (minuts == 3)
                    minuts = 180;
                else if (minuts == 4)
                    minuts = 240;

                currentTime = currentTime.AddMinutes(minuts);

                DSReminders dsReminders = new DSReminders();
                DSReminders.QuickVoiceReminderRow dr = dsReminders.QuickVoiceReminder.NewQuickVoiceReminderRow();
                dr.CalleeName = model.VoiceCalleeName;
                if (model.IsFromSchedule == "1")
                {
                    dr.RequestDelivery = MDVUtility.ToDateTime(model.ScheduleDateTime);
                }
                else
                {
                    dr.RequestDelivery = currentTime;
                }
                dr.PhoneNumber = model.VoicePhoneNumber;
                dr.TimeZone = MDVUtility.ToStr(ConfigurationManager.AppSettings["ReminderCall_TimeZone"]);
                dr.TimeZoneId = MDVUtility.ToLong(1);
                dr.MessageVoiceId = MDVUtility.ToInt32(model.MessageVoice);
                dr.LeadInVoiceId = MDVUtility.ToInt32(model.MessageLeadInVoice);
                dr.LeadOutVoiceId = MDVUtility.ToInt32(model.MessageLeadOutVoice);
                dr.MessageTemplate = model.HTMLTemplate;
                dr.IsProcessed = false;
                dr.AppointmentId = MDVUtility.ToLong(model.AppointmentId);
                dr.PatientId = MDVUtility.ToLong(model.PatientId);
                dr.ProviderId = MDVUtility.ToLong(model.ProviderId);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.GuarantorPhNumber = model.VoiceGuarantorPhoneNumber;
                dr.RepeatMessage = model.ChkVoiceRepeatMessage == "True" ? true : false;
                dsReminders.QuickVoiceReminder.AddQuickVoiceReminderRow(dr);
                dr.KeyPress = 0;
                dr.ReminderSetBy = "MDVision.IEHR";
                #region Database Insertion

                BLObject<bool> obj = BLLAdminObj.SendQuickVoiceReminder(dsReminders);
                if (obj.Data)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Quick_Reminder_Success_Message,
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
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public string getHTMLString(RemindersQuickModel model)
        {
            try
            {
                string templateString = BLLAdminObj.getHTMLString(model.HTMLTemplate, MDVUtility.ToInt64(model.AppointmentId));
                var response = new
                {
                    status = true,
                    newHTMLTemplate = templateString,
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


        public string SaveQuickSMSReminder(RemindersQuickModel model)
        {
            try
            {
                Int32 minuts = 0;
                DateTime currentTime = DateTime.Now;
                if (model.DeliveryMinutes == "no")
                    minuts = Convert.ToInt32(2);
                else
                    minuts = Convert.ToInt32(model.DeliveryMinutes);

                if (minuts == 1)
                    minuts = 60;
                else if (model.DeliveryMinutes != "no" && minuts == 2)
                    minuts = 120;
                else if (minuts == 3)
                    minuts = 180;
                else if (minuts == 4)
                    minuts = 240;

                currentTime = currentTime.AddMinutes(minuts);

                DSReminders dsReminders = new DSReminders();
                DSReminders.QuickSMSReminderRow dr = dsReminders.QuickSMSReminder.NewQuickSMSReminderRow();
                dr.CalleeName = model.SMSCalleeName;
                dr.PhoneNumber = model.SMSPhoneNumber;
                dr.MessageTemplate = model.HTMLTemplate;
                dr.IsProcessed = false;

                if (model.IsFromSchedule == "1")
                {
                    dr.RequestDelivery = MDVUtility.ToDateTime(model.ScheduleDateTime);
                }
                else
                {
                    dr.RequestDelivery = currentTime;
                }
                dr.TimeZone = MDVUtility.ToStr(ConfigurationManager.AppSettings["ReminderCall_TimeZone"]);
                dr.TimeZoneId = MDVUtility.ToLong(1);
                dr.AppointmentId = MDVUtility.ToLong(model.AppointmentId);
                dr.PatientId = MDVUtility.ToLong(model.PatientId);
                dr.ProviderId = MDVUtility.ToLong(model.ProviderId);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.GuarantorPhNumber = model.SMSGuarantorPhoneNumber;
                dr.VoiceReminderFailover = model.ChkQuickSMSVoiceReminderFailover == "True" ? true : false;
                dr.KeyPress = 0;
                dr.ReminderSetBy = "MDVision.IEHR";
                dsReminders.QuickSMSReminder.AddQuickSMSReminderRow(dr);
                #region Database Insertion
                BLObject<bool> obj = BLLAdminObj.SendQuickSMSReminder(dsReminders);
                if (obj.Data)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Quick_Reminder_Success_Message,
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
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public string SaveQuickEmailReminder(RemindersQuickModel model)
        {
            try
            {
             
                Int32 minuts = 0;
                DateTime currentTime = DateTime.Now;
                if (model.DeliveryMinutes == "no")
                    minuts = Convert.ToInt32(2);
                else
                    minuts = Convert.ToInt32(model.DeliveryMinutes);

                if (minuts == 1)
                    minuts = 60;
                else if (model.DeliveryMinutes != "no" && minuts == 2)
                    minuts = 120;
                else if (minuts == 3)
                    minuts = 180;
                else if (minuts == 4)
                    minuts = 240;

                currentTime = currentTime.AddMinutes(minuts);

                DSReminders dsReminders = new DSReminders();
                DSReminders.QuickEmailReminderRow dr = dsReminders.QuickEmailReminder.NewQuickEmailReminderRow();
                if (model.IsFromSchedule == "1")
                {
                    dr.RequestDelivery = MDVUtility.ToDateTime(model.ScheduleDateTime);
                }
                else
                {
                    dr.RequestDelivery = currentTime;
                }
                dr.FromName = model.FromName;
                dr.ToEmail = model.EmailTo;
                dr.Subject = model.Subject;
                dr.TimeZone = MDVUtility.ToStr(ConfigurationManager.AppSettings["ReminderCall_TimeZone"]);
                dr.TimeZoneId = MDVUtility.ToLong(1);
                dr.MessageTemplate = model.HTMLTemplate;
                dr.IsProcessed = false;
                dr.AppointmentId = MDVUtility.ToLong(model.AppointmentId);
                dr.PatientId = MDVUtility.ToLong(model.PatientId);
                dr.ProviderId = MDVUtility.ToLong(model.ProviderId);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.AppointmentDate = MDVUtility.ToDateTime(model.AppointmentDate);
                dr.PatientName = model.PatientName;
                dr.KeyPress = 0;
                dr.ReminderSetBy = "MDVision.IEHR";
                dsReminders.QuickEmailReminder.AddQuickEmailReminderRow(dr);

                #region Database Insertion

                BLObject<bool> obj = BLLAdminObj.SendQuickEmailReminder(dsReminders);
                if (obj.Data)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Quick_Reminder_Success_Message,
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
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        #endregion
    }
}