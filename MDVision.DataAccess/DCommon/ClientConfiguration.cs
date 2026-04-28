using MDVision.Common.Logging;
using MDVision.Common.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MDVision.DataAccess.DCommon
{
    public class ClientConfiguration
    {
        public const string DefaultUser = "MDVISION";

        public static string ConnectionString(DataProvider providerType)
        {
            if (DataSource == "")
            {
                throw new Exception("Database not define");
            }

            string str = "";
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    {
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                        {
                            DataSource = DataSource,
                            InitialCatalog = InitialCatalog,
                            IntegratedSecurity = true,
                    };

                        return builder.ToString();
                    }
                case DataProvider.OleDb:
                    {
                        OleDbConnectionStringBuilder builder2 = new OleDbConnectionStringBuilder();
                        return str;
                    }
                case DataProvider.Odbc:
                    {
                        OdbcConnectionStringBuilder builder3 = new OdbcConnectionStringBuilder();
                        return str;
                    }
            }
            return "";
        }

        public static string CustomerConnectionString(DataProvider providerType, string DBUserName)
        {
            if ((ConfigurationManager.AppSettings["Data Source"] == null) && (ConfigurationManager.AppSettings["Data Source"].ToString() == ""))
            {
                throw new Exception("Customer Database not define");
            }
            if (ConfigurationManager.AppSettings["User ID"].ToString() == "")
            {
                throw new Exception("Customer DB user not define");
            }
            string str = "";
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    {
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                        try
                        {
                            builder.DataSource = ConfigurationManager.AppSettings["Data Source"].ToString();
                            builder.InitialCatalog = ConfigurationManager.AppSettings["Initial Catalog"].ToString();
                            builder.IntegratedSecurity = true;
                            str = builder.ToString();
                        }
                        catch (Exception exception)
                        {
                            throw exception;
                        }
                        return str;
                    }
                case DataProvider.OleDb:
                    {
                        OleDbConnectionStringBuilder builder2 = new OleDbConnectionStringBuilder();
                        return str;
                    }
                case DataProvider.Odbc:
                    {
                        OdbcConnectionStringBuilder builder3 = new OdbcConnectionStringBuilder();
                        return str;
                    }
            }
            return "";
        }

        public static string DecryptFrom64(string encodedData)
        {
            byte[] bytes = Convert.FromBase64String(encodedData);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string EncryptTo64(string toEncode)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(toEncode));
        }

        public static IDBManager GetCustomerDBManager(ref string ConnectionString, string DBUserName)
        {
            DataProvider oracle = DataProvider.Oracle;
            if (ConfigurationManager.AppSettings["Provider Type"].ToString() == DataProvider.SqlServer.ToString())
            {
                oracle = DataProvider.SqlServer;
            }
            IDBManager manager = new DBManager(oracle);
            ConnectionString = CustomerConnectionString(oracle, DBUserName);
            manager.ConnectionString = ConnectionString;
            return manager;
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="Obj"> Variables that are no in MDVSession Context </param>
        /// <returns></returns>
        public static IDBManager GetDBManager(SharedVariable Obj)
        {
            IDBManager manager2;
            try
            {
                DataProvider oracle = DataProvider.Oracle;
                if (DBProviderType == DataProvider.SqlServer.ToString())
                {
                    oracle = DataProvider.SqlServer;
                }
                else
                {
                    if (MDVApplication.DBProviderType == "")
                    {
                        StringBuilder str = new StringBuilder();
                        Type myType = Obj.GetType();
                        FieldInfo[] fields = myType.GetFields();
                        foreach (FieldInfo field in fields)
                        {
                            string name = field.Name; // Get string name
                            object temp = field.GetValue(Obj); // Get value
                            if (temp is int)
                            {
                                int value = (int)temp;
                                str.Append(name + ": " + value + " ");
                            }
                            else if (temp is string)
                            {
                                string value = temp as string;
                                str.Append(name + ": " + value + " ");
                            }

                        }

                        MDVLogger.DALErrorLog(Obj, "ClientConfiguration::GetDBManager", str.ToString(), null);
                        throw new Exception("Database provider not define");
                    }

                    SetClientObject(Obj);
                }

                manager2 = new DBManager(oracle)
                {
                    ConnectionString = ConnectionString(oracle)
                };
            }
            catch (Exception exception)
            {
                MDVLogger.DALErrorLog(Obj, "ClientConfiguration::GetDBManager", "", exception);
                throw exception;
            }
            return manager2;
        }

        public static IDBManager GetDBManager()
        {
            IDBManager manager2;
            try
            {
                DataProvider oracle = DataProvider.Oracle;
                if (DBProviderType == DataProvider.SqlServer.ToString())
                {
                    oracle = DataProvider.SqlServer;
                }
                else
                {
                    if (MDVApplication.DBProviderType == "")
                    {
                        StringBuilder str = new StringBuilder();
                        Type myType = MDVSession.Current.GetType();
                        FieldInfo[] fields = myType.GetFields();
                        foreach (FieldInfo field in fields)
                        {
                            string name = field.Name; // Get string name
                            object temp = field.GetValue(MDVSession.Current); // Get value
                            if (temp is int)
                            {
                                int value = (int)temp;
                                str.Append(name + ": " + value + " ");
                            }
                            else if (temp is string)
                            {
                                string value = temp as string;
                                str.Append(name + ": " + value + " ");
                            }

                        }

                        MDVLogger.DALErrorLog("ClientConfiguration::GetDBManager", str.ToString(), null);
                        throw new Exception("Database provider not define");
                    }

                    SetClientObject();
                }

                manager2 = new DBManager(oracle)
                {
                    ConnectionString = ConnectionString(oracle)
                };
            }
            catch (Exception exception)
            {
                MDVLogger.DALErrorLog("ClientConfiguration::GetDBManager", "", exception);
                throw exception;
            }
            return manager2;
        }

        public static void SetClientObject()
        {
            DataSource = MDVApplication.DataSource;
            DBProviderType = MDVApplication.DBProviderType;
            InitialCatalog = MDVApplication.InitialCatalog;
            PersistSecurityInfo = MDVApplication.PersistSecurityInfo;
            IsProxy = MDVApplication.IsProxy;
            PoolingString = MDVApplication.PoolingString;
            Client_ID = MDVSession.Current.ClientId;
            Entity_ID = MDVSession.Current.EntityId;
            WebEntityURL = MDVSession.Current.WebEntityURL;
            UserName = DecryptFrom64(MDVSession.Current.AppUserName);
            //UserPassword = DecryptFrom64(Obj.UserPassword);
            UserPassword = MDVSession.Current.AppPassWord;

        }

        public static void SetClientObject(SharedVariable SharedVariable)
        {
            DataSource = MDVApplication.DataSource;
            DBProviderType = MDVApplication.DBProviderType;
            InitialCatalog = MDVApplication.InitialCatalog;
            PersistSecurityInfo = MDVApplication.PersistSecurityInfo;
            IsProxy = MDVApplication.IsProxy;
            PoolingString = MDVApplication.PoolingString;
            Client_ID = SharedVariable.ClientId;
            Entity_ID = SharedVariable.EntityId;
            WebEntityURL = SharedVariable.WebEntityURL;
            UserName = DecryptFrom64(SharedVariable.UserName);
            //UserPassword = DecryptFrom64(Obj.UserPassword);
            UserPassword = SharedVariable.AppPassWord;

        }

        public static string Client_ID { get; set; }

        public static string DataSource { get; set; }

        public static string DBPassword { get; set; }

        public static string DBProviderType { get; set; }

        public static string DBUserId { get; set; }

        public static string Entity_ID { get; set; }

        public static string InitialCatalog { get; set; }

        public static bool IsProxy { get; set; }

        public static bool PersistSecurityInfo { get; set; }

        public static string PoolingString { get; set; }

        public static string UserName { get; set; }

        public static string UserPassword { get; set; }

        public static string WebEntityURL { get; set; }

    }
}

