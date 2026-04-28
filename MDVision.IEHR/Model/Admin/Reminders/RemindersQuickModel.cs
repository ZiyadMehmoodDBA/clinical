using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Admin.Reminders
{
    public class RemindersQuickModel
    {
        public string QuickVoiceReminderId { get; set; }
        public string QuickSMSReminderId { get; set; }
        public string VoicePhoneNumber { get; set; }
        public string SMSCalleeName { get; set; }
        public string VoiceCalleeName { get; set; }
        public string SMSPhoneNumber { get; set; }
        public string MessageVoice { get; set; }
        public string MessageLeadInVoice { get; set; }
        public string MessageLeadOutVoice { get; set; }
        public string HTMLTemplate { get; set; }
        public string commandType { get; set; }
        public string ProviderId { get; set; }
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string DeliveryDateTime { get; set; }
        public string DeliveryMinutes { get; set; }
        public string IsFromSchedule { get; set; }
        public string ScheduleDateTime { get; set; }
        public string TextTimeZone { get; set; }
        public string VoiceTimeZone { get; set; }
        public string TextTimeZone_RefValue { get; set; }
        public string VoiceTimeZone_RefValue { get; set; }
        public string SMSGuarantorPhoneNumber { get; set; }
        public string ChkQuickSMSVoiceReminderFailover { get; set; }
        public string EmailTimeZone_RefValue { get; set; }
        public string FromName { get; set; }
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string EmailTimeZone { get; set; }
        public string VoiceGuarantorPhoneNumber { get; set; }
        public string ChkVoiceRepeatMessage { get; set; }
        public string PatientName { get; set; }
        public string ProviderName { get; set; }
        public string AppointmentDate { get; set; }
    }
}