using MDVision.Common.Utilities;
using System;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Web;

namespace MDVision.Common.Shared
{
    public class MDVApplication
    {
       
        private MDVApplication()
        {
            //add default value
        }

        private static MDVApplication application;

        public static MDVApplication Current
        {
            get
            {
                if (application == null)
                {
                    application = new MDVApplication();
                }
                return application;
            }
        }

        public HttpContext context
        {
            get
            {
                return HttpContext.Current;
            }
        }


        #region DB Connection Properties

        public static string DataSource = ConfigurationManager.AppSettings["Data Source"].ToString();
        public static string InitialCatalog = ConfigurationManager.AppSettings["Initial Catalog"].ToString();
        public static string DBUserId = MDVUtility.EncryptTo64(ConfigurationManager.AppSettings["User ID"]).ToString();
        public static string DBPassWord = MDVUtility.EncryptTo64(ConfigurationManager.AppSettings["Password"]).ToString();
        public static string DBProviderType = ConfigurationManager.AppSettings["Provider Type"].ToString();
        public static bool PersistSecurityInfo = Convert.ToBoolean(ConfigurationManager.AppSettings["PersistSecurityInfo"]);
        public static bool IsProxy = Convert.ToBoolean(ConfigurationManager.AppSettings["IsProxy"]);
        public static string PoolingString = MDVUtility.ToStr(ConfigurationManager.AppSettings["PoolingString"]);

        #endregion

        #region " Application Keys"

        public static string CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string CustomerRegCode = ConfigurationManager.AppSettings["CustomerRegCode"].ToString();
        public static bool isTestDatabase = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTestDatabase"]);
        public static string DocsHostName = MDVUtility.ToStr(ConfigurationManager.AppSettings["DocsHostName"]);
        public static string DocsAlias = MDVUtility.ToStr(ConfigurationManager.AppSettings["DocsAlias"]);
        public static string DomainName = MDVUtility.ToStr(ConfigurationManager.AppSettings["DomainName"]);
        public static string DomainUserName = MDVUtility.ToStr(ConfigurationManager.AppSettings["DomainUserName"]);
        public static string DomainPassword = MDVUtility.ToStr(ConfigurationManager.AppSettings["DomainPassword"]);
        public static string FaxNotifyURL = MDVUtility.ToStr(ConfigurationManager.AppSettings["FaxNotifyURL"]);
        public static string BillingInquiryEmail = MDVUtility.ToStr(ConfigurationManager.AppSettings["BillingInquiryEmail"]);
        public static string No_Reply_EmailAddress = MDVUtility.ToStr(ConfigurationManager.AppSettings["No_Reply_EmailAddress"]);
        public static string No_Reply_EmailPassword = MDVUtility.ToStr(ConfigurationManager.AppSettings["No_Reply_EmailPassword"]);

        #endregion

        #region Network Security

        public static string ServerIP
        {
            get
            {
                string Str = "";
                Str = Dns.GetHostName();

                IPHostEntry ipEntry = Dns.GetHostEntry(Str);
                IPAddress[] addr = ipEntry.AddressList;
                return addr[addr.Length - 1].ToString();
            }
        }

        public static string ServerName
        {
            get
            {
                string strHostName = "";
                strHostName = Dns.GetHostName();
                return strHostName;
            }
        }
        #endregion

        #region CCDA KEYS
        public static string CCDA_RemoteHost = ConfigurationManager.AppSettings["CCDA_RemoteHost"];
        public static string CCDA_RemoteUser = ConfigurationManager.AppSettings["CCDA_RemoteUser"];
        public static string CCDA_RemotePassword = ConfigurationManager.AppSettings["CCDA_RemotePassword"];
        public static string CCDA_RemotePort = ConfigurationManager.AppSettings["CCDA_RemotePort"];
        #endregion

    }
}
