using MDVision.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Common.Shared
{
    /// <summary>
    /// This Class is used to share MDVSession Current Context Variables in multi threaded calls.
    /// This class will only for the multi threaded calls.
    /// </summary>
    public class SharedVariable
    {
        public string EntityId { get; set; }
        public long AppUserId { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string WebEntityURL { get; set; }
        public string AppPassWord { get; set; }
        public bool IsEmergencyAccess { get; set; }
        public string UserHostIP { get; set; }

        public SharedVariable()
        {
            this.IsEmergencyAccess = false;
            this.UserHostIP = MDVUtility.GetLanIPAddress();
        }

        public static SharedVariable GetSharedVariable()
        {
            SharedVariable Sharedobj = new SharedVariable();

            Sharedobj.EntityId = MDVSession.Current.EntityId;
            Sharedobj.AppUserId = MDVSession.Current.AppUserId;
            Sharedobj.UserName = MDVSession.Current.AppUserName;
            Sharedobj.ClientId = MDVSession.Current.ClientId;
            Sharedobj.AppPassWord = MDVSession.Current.AppPassWord;
            Sharedobj.WebEntityURL = MDVSession.Current.WebEntityURL;

            return Sharedobj;
        }
    }
}
