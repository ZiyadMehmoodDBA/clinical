using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using MDVision.IEHR;
using MDVision.IEHR.Filters;
using System.Diagnostics;
using MDVision.IEHR.Common;
using MDVision.Business.BCommon;
using System.Web.SessionState;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.IEHR.Security;
using System.Web.Mvc;
using System.Web.Http;
using MDVision.IEHR.App_Start;

namespace MDVision.IEHR
{
    public class Global : HttpApplication
    {
        public static string patchNo { get; set; }
       public void Application_BeginRequest(object sender, EventArgs e) {

       }

        public void Application_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                string ext = System.IO.Path.GetExtension(HttpContext.Current.Request.FilePath);
                if (HttpContext.Current.Request.FilePath.IndexOf("MDVisionLogin") < 0 
                    && (ext == ".aspx" || ext == ".ashx" || IsWebApiRequest()))
                {
                    new UserLoginHelper().AuthenticateUserRequest();
                }
            }
            catch (Exception)
            { }
        }
        public void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new AuthHandler());
            patchNo = DateTime.UtcNow.ToString();
        }

        public void Application_End(object sender, EventArgs e) { }

        public void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            try
            {
                Exception exc = Server.GetLastError();
                MDVLogger.PresentationErrorLog(exc.Source.ToString(), exc, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
            }
            catch (Exception exp) { }
            finally
            {
                Server.ClearError();
            }
        }

        public void Session_Start(object sender, EventArgs e)
        {
            var abc = HttpContext.Current.Session.SessionID;
        }
        public void Session_End(object sender, EventArgs e)
        {
        }
        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        public bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }


    }
}