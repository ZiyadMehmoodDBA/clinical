using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Admin.Reminder
{
    public class ReminderSetting
    {
        public string ReminderSettingId { get; set; }
        public string ProviderId { get; set; }
        public string APIKey { get; set; }
        public string CalleeName { get; set; }
        public string TimeZone { get; set; }
    }

    public class RemidnerProviderModel
    {
        public List<long> ReminderIds { get; set; }
        public long ProviderId { get; set; }

        public RemidnerProviderModel()
        {
            ReminderIds = new List<long>();
        }

    }
}
