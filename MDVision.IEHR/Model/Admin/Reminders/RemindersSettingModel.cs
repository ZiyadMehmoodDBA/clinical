using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Admin.Reminders
{
    public class RemindersSettingModel
    {
        public string RemindersTemplateId { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string FormName { get; set; }
        public string NoteType { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateTypeId { get; set; }
        public string EntityId { get; set; }
        public string commandType { get; set; }
        public string HTMLTemplate { get; set; }
        public string ProviderIds { get; set; }
        public string IsActive { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string CallIDNumber { get; set; }
        public string Template { get; set; }
        public string Provider { get; set; }
        public string ReminderEmailSettingId { get; set; }
        public string SettingId { get; set; }
        public string RepliesEmail { get; set; }
        public string DisableVoiceReminder { get; set; }
        public string TextSettingsProvider { get; set; }
        public string ReminderTextSettingId { get; set; }
        public string ReminderVoiceSettingId { get; set; }
        public string DisableVoiceReminderFailover { get; set; }
        public string ConfirmationKey { get; set; }
        public string CancelKey { get; set; }
        public string PIN { get; set; }
        public string Texttospeech { get; set; }
        public string FacilityIds { get; set; }
        public string RepeatMessage { get; set; }
        public string ProviderVoiceSetting { get; set; }
        public string CallIDNumberforVoice { get; set; }
        public string SMSCallerName { get; set; }
        public string AppConfirmationMessage { get; set; }
        public string AppCancellationMessage { get; set; }
        public string AppReminderMessage { get; set; }
        public string TextDelivery { get; set; }
        public string VoiceDelivery { get; set; }
        public string TextTimeZone { get; set; }
        public string VoiceTimeZone { get; set; }
        public string TextTimeZone_RefValue { get; set; }
        public string VoiceTimeZone_RefValue { get; set; }
        public string Template_RefValue { get; set; }
        public string ChkVoiceReminderFailover { get; set; }
        public string EmailDelivery { get; set; }
    }
}