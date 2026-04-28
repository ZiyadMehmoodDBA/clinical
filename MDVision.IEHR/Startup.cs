using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR("/Common/ProviderNoteAccess", new Microsoft.AspNet.SignalR.HubConfiguration());
        }
    }
}