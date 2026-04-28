using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Admin.Reminders
{
    public class RemindersTextSettingModel
    {
        public string RepliesEmail { get; set; }
        public string DisableVoiceReminder { get; set; }
        public string TextSettingsProvider { get; set; }
        public string HTMLTemplate { get; set; }
        public string commandType { get; set; }
        public string ReminderTextSettingId { get; set; }


    }
}