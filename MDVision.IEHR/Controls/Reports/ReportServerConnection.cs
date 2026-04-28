using MDVision.Common.Shared;
using MDVision.IEHR.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;

namespace MDVision.IEHR.Controls.Reports
{
    public sealed class ReportServerConnection : IReportServerConnection2
    {
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                // Use the default Windows user.  Credentials will be
                // provided by the NetworkCredentials property.
                return null;
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {
                // Read the user information from the web.config file.  
                // By reading the information on demand instead of 
                // storing it, the credentials will not be stored in 
                // session, reducing the vulnerable surface area to the
                // web.config file, which can be secured with an ACL.

                // User name
                string userName = MDVSession.Current.DomainUserName.ToString();

                // if (string.IsNullOrEmpty(userName))
                // throw new Exception(MDVUtility.Constants.AppConst.MESSAGE_MISSING_USER_NAME);

                // Password
                string password = MDVSession.Current.DomainPassword.ToString();

                //if (string.IsNullOrEmpty(password))
                //  throw new Exception(MDVUtility.Constants.AppConst.MESSAGE_MISSING_PWD);

                // Domain
                string domain = MDVSession.Current.DomainName.ToString();

                //  if (string.IsNullOrEmpty(domain))
                // throw new Exception(MDVUtility.Constants.AppConst.MESSAGE_MISSING_DOMAIN);

                return new NetworkCredential(userName, password, domain);
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;
            // Not using form credentials
            return false;
        }
        public Uri ReportServerUrl
        {
            get
            {
                string url = MDVSession.Current.ReportURL.ToString();

                //if (string.IsNullOrEmpty(url))
                //    throw new Exception(MDVUtility.Constants.AppConst.MESSAGE_MISSING_URL);

                return new Uri(url);
            }
        }
        public int Timeout
        {
            get
            {
                return int.Parse("60000");
                // return 60000; // 60 seconds
            }
        }
        public IEnumerable<Cookie> Cookies
        {
            get
            {
                // No custom cookies
                return null;
            }
        }
        public IEnumerable<string> Headers
        {
            get
            {
                // No custom headers
                return null;
            }
        }
    }
}