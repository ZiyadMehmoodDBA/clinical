using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Security
{
    public class LoginHelperModel
    {

        public string SessionId { get; set; }
        public bool IsLogin { get; set; }
        public string AppUsername { get; set; }
        public long AppUserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string LastActivity { get; set; }
        public string ChangeSet { get; set; }
        public bool IsChangeReflected { get; set; }
        public string EntityId { get; set; }
        public bool IsSuspended { get; set; }
        public string IP { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string MachineInfo { get; set; }
        public string Platform { get; set; }

    }
}
