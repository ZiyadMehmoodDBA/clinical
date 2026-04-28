using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;

namespace MDVision.Business.BCommon
{
     
    public enum LogonType
    {

        LOGON32_LOGON_INTERACTIVE = 2,

        LOGON32_LOGON_NETWORK = 3,

        LOGON32_LOGON_BATCH = 4,

        LOGON32_LOGON_SERVICE = 5,

        LOGON32_LOGON_UNLOCK = 7,

        LOGON32_LOGON_NETWORK_CLEARTEXT = 8,

        LOGON32_LOGON_NEW_CREDENTIALS = 9,
    }
    public enum LogonProvider
    {

        LOGON32_PROVIDER_DEFAULT = 0,

        LOGON32_PROVIDER_WINNT35 = 1,

        LOGON32_PROVIDER_WINNT40 = 2,

        LOGON32_PROVIDER_WINNT50 = 3,
    }
    class SecuUtil32
    {

        //[DllImport("advapi32.dll")]
        //public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr TokenHandle);
      
      
        //[DllImport("kernel32.dll")]
        //public static bool CloseHandle(IntPtr handle);


        //[DllImport("advapi32.dll")]
        //public static bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);
      

        // ''''Model 2
        //[DllImport("advapi32.dll")]
        //public static long RevertToSelf();

             


        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr TokenHandle);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);


        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);


        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();
     
    }
    public class NetworkSecurity
    {

        private static NetworkSecurity _instance;

        //public static NetworkSecurity Instance()
        //{ 
        //    get
        //    {
        //    if (_instance == null)
        //    {
        //        try
        //        {
        //            System.Threading.Monitor.Enter(typeof(NetworkSecurity));
        //            _instance = new NetworkSecurity();
        //        }
        //        finally
        //        {
        //            System.Threading.Monitor.Exit(typeof(NetworkSecurity));
        //        }
        //    }
        //    return _instance;
        //    }
        //}
        public static NetworkSecurity Instance
        {
            get
            {
                if (_instance == null)
                {
                    try
                    {
                        System.Threading.Monitor.Enter(typeof(NetworkSecurity));
                        _instance = new NetworkSecurity();
                    }
                    finally
                    {
                        System.Threading.Monitor.Exit(typeof(NetworkSecurity));
                    }

                }
                return _instance;
            }
        }

        private NetworkSecurity()
        {
        }

        

        public WindowsImpersonationContext Login()
        {
            WindowsImpersonationContext impContext = null;
            string strDomainName = "";
            string strUserName = "";
            string strPasword = "";
            try
            {
                strDomainName = MDVApplication.DomainName;
                strUserName = MDVApplication.DomainUserName;
                strPasword = MDVApplication.DomainPassword;
                impContext = ImpersonateUser(strDomainName, strUserName, strPasword, LogonType.LOGON32_LOGON_NEW_CREDENTIALS, LogonProvider.LOGON32_PROVIDER_DEFAULT);
            }
            catch (ApplicationException ex)
            {
              
                 throw new Exception("Cannot login to the system using domain " + strDomainName + " , username "  + strUserName + " given password ");
                  MDVLogger.BLLErrorLog("NetworkSecurity::Login", ex);
                return null;
            }
            return impContext;
        }

        //  Do not use this function any more
        private WindowsImpersonationContext ImpersonateUser(string strDomain, string strLogin, string strPwd, LogonType logonType, LogonProvider logonProvider)
        {
            // Microsoft Recommended method. 
            // http://support.microsoft.com/kb/306158
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;
            if (SecuUtil32.RevertToSelf())
            {
                if (SecuUtil32.LogonUser(strLogin, strDomain, strPwd, MDVUtility.ToConvertInt32(logonType), MDVUtility.ToConvertInt32(logonProvider), ref token))
                {
                    if (SecuUtil32.DuplicateToken(token, 2,ref tokenDuplicate))
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        WindowsImpersonationContext impersonationContext = tempWindowsIdentity.Impersonate();
                        
                        if (impersonationContext!=null)
                        {
                            return impersonationContext;
                        }
                    }
                }
            }
            if (!tokenDuplicate.Equals(IntPtr.Zero))
            {
                SecuUtil32.CloseHandle(tokenDuplicate);
            }
            if (!token.Equals(IntPtr.Zero))
            {
                SecuUtil32.CloseHandle(token);
            }
            return null;
        }
    }
}
