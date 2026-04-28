using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Security;


namespace MDVision.DataAccess.DAL.Admin
{
    public class DALCustomers
    {
        #region " Stored Procedure Names"
        //private const string PROC_INSERT = "";
        //private const string PROC_UPDATE = "";
        //private const string PROC_DELETE = "";
        private const string PROC_CUSTOMER_LOGIN_INFO_SELECT = "system.sp_GetCustomerLoginInfo";
        private const string PROC_ACCOUNT_LOCKOUT_THRESHOLD_SELECT = "system.sp_GetAccountLockoutThreshold";
        private const string PROC_USERS_ACCOUNT_LOCK = "system.sp_UsersAccountLock";
        private const string PROC_CLIENT_INFO_SELECT = "System.sp_MDVClientSelect";
        private const string PROC_CUSTOMER_LOGIN_INFO_FOR_SERVICE_SELECT = "System.sp_GetCustomerLoginInfoForService";
        private const string PROC_CUSTOMER_LOGIN_INFO_FOR_CheckInApp = "System.sp_GetCustomerLoginInfoForServiceCheckInApp";
        
        private const string PROC_USER_GET_IS_FULL_SSN = "system.sp_GetIsFullSSN";
        private const string PROC_USER_ACTIVITY_LOG_INSERT = "System.sp_UsersActivityLogInsert";
        private const string PROC_ENTITYDATA_SAVE = "[Mobile].[sp_ConfigurationInsert]";
        private const string PROC_NETWORKIP_LOAD = "Mobile.sp_NetworkSelect";
        #endregion

        #region "Query "

        #endregion

        #region "Parameters"
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_USER_PASSWORD = "@UserPassword";
        private const string PARM_CUSTOMER_REG_CODE = "@CustomerRegCode";
        private const string PARM_ENTITY_REG_CODE = "@EntityRegCode";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_CLIENT_ID = "@ClientId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_SECURITY_USER_ID = "@SecurityUserId";

        private const string PARM_USER_ACTIVITY_ID = "@UserActivityId";
        private const string PARM_ACTIVITY_LOG_DATE = "@ActivityLogDate";
        private const string PARM_ACTIVITY_EVENT_ID = "@ActivityEventId";

        private const string PARM_ACTIVITY_STATUS = "@ActivityStatus";
        private const string PARM_DETAILS = "@Details";
        private const string PARM_MRNO = "@MRNo";
        private const string PARM_MACHINE_IP = "@MachineIp";
        private const string PARM_IS_EMERGENCY = "@IsEmergency";
        private const string PARM_VISIT_ACCOUNTNO = "@VisitAccountNo";

        private const string PARM_FORM_NAME = "@FormName";


        //NETWORD RELATED PARAMS
        private const string PARM_SERVER_IP = "@serverIp";
        private const string PARM_SERVER_NAME = "@serverName";
        private const string PARM_IP = "@ip";
        private const string PARM_COUNTRY = "@country";
        private const string PARM_COUNTRY_CODE = "@countryCode";
        private const string PARM_TIME_ZONE = "@timezone";
        private const string PARM_CITY = "@city";
        private const string PARM_REGION = "@region";
        private const string PARM_REGION_NAME = "@regionName";
        private const string PARM_ZIP = "@zip";
        private const string PARM_AS = "@_as";
        private const string PARM_ISP = "@isp";
        private const string PARM_LATITUDE = "@lat";
        private const string PARM_LONGITUDE = "@lon";
        private const string PARM_ORG = "@Org";
        private const string PARM_STATUS = "@status";
        private const string PARM_ERROR = "@error";
        private const string PARM_HOST_NAME = "@hostName";
        private const string PARM_IS_LOCAL = "@IsLocal";
        public struct Parameters
        {
            public string USER_NAME;
            public string USER_PASSWORD;
            public string CUSTOMER_REG_CODE;
            public string ENTITY_REG_CODE;
            public string ERROR_MESSAGE;
        }

        #endregion

        #region Singleton
        private static DALCustomers _instance;
        /// <summary>
        /// Singleton context
        /// </summary>
        public static DALCustomers Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DALCustomers();
                return _instance;
            }
        }

        #endregion


        #region "Support Functions"

        private void CreateParameters(IDBManager dbManager, DSUsersActivity ds, Boolean IsInsert)
        {
            int i = 0;
            dbManager.CreateParameters(30);

            if (IsInsert == true)
                dbManager.AddParameters(i++, PARM_USER_ACTIVITY_ID, ds.UsersActivityLog.UserActivityIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(i++, PARM_USER_ACTIVITY_ID, ds.UsersActivityLog.UserActivityIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(i++, PARM_ACTIVITY_LOG_DATE, ds.UsersActivityLog.ActivityLogDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(i++, PARM_ACTIVITY_EVENT_ID, ds.UsersActivityLog.ActivityEventIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_USER_NAME, ds.UsersActivityLog.UserNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ACTIVITY_STATUS, ds.UsersActivityLog.ActivityStatusColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(i++, PARM_DETAILS, ds.UsersActivityLog.DetailsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_MRNO, ds.UsersActivityLog.MRNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_MACHINE_IP, ds.UsersActivityLog.MachineIpColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_IS_EMERGENCY, ds.UsersActivityLog.IsEmergencyColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(i++, PARM_VISIT_ACCOUNTNO, ds.UsersActivityLog.VisitAccountNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_FORM_NAME, ds.UsersActivityLog.FormNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(i++, PARM_SERVER_IP, ds.UsersActivityLog.serverIpColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_SERVER_NAME, ds.UsersActivityLog.serverNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_IP, ds.UsersActivityLog.IpColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_COUNTRY, ds.UsersActivityLog.countryColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_COUNTRY_CODE, ds.UsersActivityLog.countryCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_TIME_ZONE, ds.UsersActivityLog.timezoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_CITY, ds.UsersActivityLog.cityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_REGION, ds.UsersActivityLog.regionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_REGION_NAME, ds.UsersActivityLog.regionNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ZIP, ds.UsersActivityLog.zipColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_AS, ds.UsersActivityLog.asColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ISP, ds.UsersActivityLog.ispColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_LATITUDE, ds.UsersActivityLog.latColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(i++, PARM_LONGITUDE, ds.UsersActivityLog.lonColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(i++, PARM_ORG, ds.UsersActivityLog.orgColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_STATUS, ds.UsersActivityLog.statusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ERROR, ds.UsersActivityLog.errorColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_HOST_NAME, ds.UsersActivityLog.hostNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_IS_LOCAL, ds.UsersActivityLog.IsLocalColumn.ColumnName, DbType.Byte);

        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        public DSSoftwareCustomersInfo LoadCustomerInfo(Parameters param)
        {
            DSSoftwareCustomersInfo ds = new DSSoftwareCustomersInfo();
            string connectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref connectionString, param.USER_NAME);
            try
            {
                if (param.USER_NAME == "")
                    param.USER_NAME = null;

                if (param.USER_PASSWORD == "")
                    param.USER_PASSWORD = null;

                if (param.CUSTOMER_REG_CODE == "")
                    param.CUSTOMER_REG_CODE = null;

                if (param.ENTITY_REG_CODE == "")
                    param.ENTITY_REG_CODE = null;

                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_USER_NAME, param.USER_NAME);
                dbManager.AddParameters(1, PARM_USER_PASSWORD, param.USER_PASSWORD);
                dbManager.AddParameters(2, PARM_CUSTOMER_REG_CODE, param.CUSTOMER_REG_CODE);
                dbManager.AddParameters(3, PARM_ENTITY_REG_CODE, param.ENTITY_REG_CODE);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                ds = (DSSoftwareCustomersInfo)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CUSTOMER_LOGIN_INFO_FOR_SERVICE_SELECT, ds, DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO);
                foreach (DataRow dr in ds.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows)
                {
                    dr[DSSoftwareCustomersInfo.FIELD_CUSTOMER_CONNECTION_STRING] = connectionString;
                    dr[DSSoftwareCustomersInfo.FIELD_CUSTOMER_WEB_SERVER_URL] = ConfigurationManager.AppSettings["WebServerURL"] ?? "";

                }
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCustomers::LoadCustomerInfo", PROC_CUSTOMER_LOGIN_INFO_FOR_SERVICE_SELECT, ex);

                //string[] str = ex.Message.Split('|');
                // if (str.Length > 1)
                // throw new Exception(str[1].ToString());
                //else
                throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        public List<SoftwareCustomerInfoModel_> LoadCustomerInfoList(Parameters param)
        {
            var softwareCustomerInfoModel = new List<SoftwareCustomerInfoModel_>();
            var connectionString = "";

            var dbManager = ClientConfiguration.GetCustomerDBManager(ref connectionString, param.USER_NAME);
            try
            {
                if (param.USER_NAME == "")
                    param.USER_NAME = null;

                if (param.USER_PASSWORD == "")
                    param.USER_PASSWORD = null;

                if (param.CUSTOMER_REG_CODE == "")
                    param.CUSTOMER_REG_CODE = null;

                if (param.ENTITY_REG_CODE == "")
                    param.ENTITY_REG_CODE = null;

                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_USER_NAME, param.USER_NAME);
                dbManager.AddParameters(1, PARM_USER_PASSWORD, param.USER_PASSWORD);
                dbManager.AddParameters(2, PARM_CUSTOMER_REG_CODE, param.CUSTOMER_REG_CODE);
                dbManager.AddParameters(3, PARM_ENTITY_REG_CODE, param.ENTITY_REG_CODE);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                var reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CUSTOMER_LOGIN_INFO_SELECT);
                string errorMessage = "";
                while (reader.Read())
                {
                    errorMessage = reader["ErrorMessage"].ToString();
                }
                if (string.IsNullOrEmpty(errorMessage))
                {
                    reader.NextResult();
                    while (reader.Read())
                    {
                        var model = new SoftwareCustomerInfoModel_
                        {
                            WebServerURL =
                                !string.IsNullOrEmpty(reader["WebServiceURL"].ToString())
                                    ? reader["WebServiceURL"].ToString()
                                    : "",
                            CustomerConnectionString = connectionString,
                            CustomerId =
                                !string.IsNullOrEmpty(reader["CustomerId"].ToString())
                                    ? reader["CustomerId"].ToString()
                                    : "",
                            CustomerRegCode =
                                !string.IsNullOrEmpty(reader["CustomerRegCode"].ToString())
                                    ? reader["CustomerRegCode"].ToString()
                                    : "",
                            ReportURL =
                                !string.IsNullOrEmpty(reader["ReportURL"].ToString()) ? reader["ReportURL"].ToString() : "",
                            DomainName =
                                !string.IsNullOrEmpty(reader["DomainName"].ToString())
                                    ? reader["DomainName"].ToString()
                                    : "",
                            DomainUserName =
                                !string.IsNullOrEmpty(reader["DomainUserName"].ToString())
                                    ? reader["DomainUserName"].ToString()
                                    : "",
                            DomainPassword =
                                !string.IsNullOrEmpty(reader["DomainPassword"].ToString())
                                    ? reader["DomainPassword"].ToString()
                                    : "",
                            IsActive =
                                !string.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "",
                            DBName = !string.IsNullOrEmpty(reader["DBName"].ToString()) ? reader["DBName"].ToString() : "",
                            DataSource =
                                !string.IsNullOrEmpty(reader["DataSource"].ToString())
                                    ? reader["DataSource"].ToString()
                                    : "",
                            DBUserId =
                                !string.IsNullOrEmpty(reader["DBUserId"].ToString()) ? reader["DBUserId"].ToString() : "",
                            DBPassword =
                                !string.IsNullOrEmpty(reader["DBPassword"].ToString())
                                    ? reader["DBPassword"].ToString()
                                    : "",
                            PersistSecurityInfo =
                                !string.IsNullOrEmpty(reader["PersistSecurityInfo"].ToString())
                                    ? reader["PersistSecurityInfo"].ToString()
                                    : "",
                            IsProxy =
                                !string.IsNullOrEmpty(reader["IsProxy"].ToString()) ? reader["IsProxy"].ToString() : "",
                            PoolingString =
                                !string.IsNullOrEmpty(reader["PoolingString"].ToString())
                                    ? reader["PoolingString"].ToString()
                                    : "",
                            ProviderType =
                                !string.IsNullOrEmpty(reader["ProviderType"].ToString())
                                    ? reader["ProviderType"].ToString()
                                    : "",
                            IsTestDatabase =
                                !string.IsNullOrEmpty(reader["IsTestDatabase"].ToString())
                                    ? reader["IsTestDatabase"].ToString()
                                    : "",
                            NoOfLicenses =
                                !string.IsNullOrEmpty(reader["NoOfLicenses"].ToString())
                                    ? reader["NoOfLicenses"].ToString()
                                    : "",
                            NoOfUsers =
                                !string.IsNullOrEmpty(reader["NoOfUsers"].ToString()) ? reader["NoOfUsers"].ToString() : "",
                            EntityRegCode =
                                !string.IsNullOrEmpty(reader["EntityRegCode"].ToString())
                                    ? reader["EntityRegCode"].ToString()
                                    : "",
                            EntityId =
                                !string.IsNullOrEmpty(reader["EntityId"].ToString()) ? reader["EntityId"].ToString() : "",
                            UniqueClientId =
                                !string.IsNullOrEmpty(reader["UniqueClientId"].ToString())
                                    ? reader["UniqueClientId"].ToString()
                                    : ""
                        };

                        model.EntityRegCode = !string.IsNullOrEmpty(reader["EntityRegCode"].ToString()) ? reader["EntityRegCode"].ToString() : "";
                        model.UserId = !string.IsNullOrEmpty(reader["UserId"].ToString()) ? reader["UserId"].ToString() : "";
                        model.IsAdmin = !string.IsNullOrEmpty(reader["IsAdmin"].ToString()) ? reader["IsAdmin"].ToString() : "";
                        model.DateFormats = !string.IsNullOrEmpty(reader["DateFormats"].ToString()) ? reader["DateFormats"].ToString() : "";
                        model.IMOHostName = !string.IsNullOrEmpty(reader["IMOHostName"].ToString()) ? reader["IMOHostName"].ToString() : "";
                        model.IMOCPTPort = !string.IsNullOrEmpty(reader["IMOCPTPort"].ToString()) ? reader["IMOCPTPort"].ToString() : "";
                        model.IMOICDPort = !string.IsNullOrEmpty(reader["IMOICDPort"].ToString()) ? reader["IMOICDPort"].ToString() : "";
                        model.IMO_ID = !string.IsNullOrEmpty(reader["IMO_ID"].ToString()) ? reader["IMO_ID"].ToString() : "";
                        model.OCRLicenseKey = !string.IsNullOrEmpty(reader["OCRLicenseKey"].ToString()) ? reader["OCRLicenseKey"].ToString() : "";
                        model.FileSize = !string.IsNullOrEmpty(reader["FileSize"].ToString()) ? reader["FileSize"].ToString() : "";
                        model.Ftp_PortNo = !string.IsNullOrEmpty(reader["Ftp_PortNo"].ToString()) ? reader["Ftp_PortNo"].ToString() : "";
                        model.Docs_HostName = !string.IsNullOrEmpty(reader["Docs_HostName"].ToString()) ? reader["Docs_HostName"].ToString() : "";
                        model.Docs_Alias = !string.IsNullOrEmpty(reader["Docs_Alias"].ToString()) ? reader["Docs_Alias"].ToString() : "";
                        model.RefreshTime = !string.IsNullOrEmpty(reader["RefreshTime"].ToString()) ? reader["RefreshTime"].ToString() : "";
                        model.Currency = !string.IsNullOrEmpty(reader["Currency"].ToString()) ? reader["Currency"].ToString() : "";
                        model.DecimalPlaces = !string.IsNullOrEmpty(reader["DecimalPlaces"].ToString()) ? reader["DecimalPlaces"].ToString() : "";
                        model.ClaimScrubberEDIServer = !string.IsNullOrEmpty(reader["ClaimScrubberEDIServer"].ToString()) ? reader["ClaimScrubberEDIServer"].ToString() : "";
                        model.ClaimScrubberPassword = !string.IsNullOrEmpty(reader["ClaimScrubberPassword"].ToString()) ? reader["ClaimScrubberPassword"].ToString() : "";
                        model.ClaimScrubberSubmitterID = !string.IsNullOrEmpty(reader["ClaimScrubberSubmitterID"].ToString()) ? reader["ClaimScrubberSubmitterID"].ToString() : "";
                        model.ClaimScrubberUser = !string.IsNullOrEmpty(reader["ClaimScrubberUser"].ToString()) ? reader["ClaimScrubberUser"].ToString() : "";
                        model.IsPasswordExpired = !string.IsNullOrEmpty(reader["IsPasswordExpired"].ToString()) ? reader["IsPasswordExpired"].ToString() : "";
                        model.PasswordRegex = !string.IsNullOrEmpty(reader["PasswordRegex"].ToString()) ? reader["PasswordRegex"].ToString() : "";
                        model.DefaultEntity = !string.IsNullOrEmpty(reader["DefaultEntity"].ToString()) ? reader["DefaultEntity"].ToString() : "";
                        model.isFirstTimeLoggedIn = !string.IsNullOrEmpty(reader["isFirstTimeLoggedIn"].ToString()) ? reader["isFirstTimeLoggedIn"].ToString() : "";
                        softwareCustomerInfoModel.Add(model);
                    }
                }
                else
                {
                    reader.Close();
                    throw new Exception(errorMessage);
                }

                return softwareCustomerInfoModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCustomers::LoadCustomerInfo", PROC_CUSTOMER_LOGIN_INFO_SELECT, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public Client GetClientInformationNative(string id, string customeRegCode)
        {
            Client client = null;
            string connectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref connectionString, "");
            SqlDataReader reader = null;
            try
            {
                //client.Id = "MDVisionIOS";
                ////MDV!$!0N!0$@99
                //client.Secret = "GPTKemj3cz+UfeLoW0rzDMNBsEtnAVyNLo2FbTqIkXU=";
                //client.Name = "MDVision IOS App";
                //client.ApplicationType = Client.getApplicationTypeById(1);
                //client.Active = false;
                //client.RefreshTokenLifeTime = 7200;
                //client.AllowedOrigin = "*";
                //return client;

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CLIENT_ID, id);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLIENT_INFO_SELECT);
                while (reader.Read())
                {
                    client = new Client();
                    client.Id = MDVUtility.ToStr(reader["Id"]);
                    client.Secret = MDVUtility.ToStr(reader["Secret"]);
                    client.Name = MDVUtility.ToStr(reader["Name"]);
                    client.appType = Client.getApplicationTypeById(MDVUtility.ToInt32(reader["ApplicationType"]));
                    client.Active = MDVUtility.ToBool(reader["Active"]);
                    client.RefreshTokenLifeTime = MDVUtility.ToInt32(reader["RefreshTokenLifeTime"]);
                    client.AllowedOrigin = MDVUtility.ToStr(reader["AllowedOrigin"]);
                }
                return client;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCustomers::GetClientInformationNative", PROC_CLIENT_INFO_SELECT, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                reader.Close();
                dbManager.Dispose();
            }
        }
        public string GetIsFullSSN(long userId, string userName, string entityId, string isActive, long SecurityUserId)
        {

            string connectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref connectionString, "");
            dbManager.Open();
            dbManager.CreateParameters(5);
            try
            {
                if (userName == "") userName = null;
                if (entityId == "") entityId = null;
                if (isActive == "") isActive = null;

                if (userId == 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, userId);

                dbManager.AddParameters(1, PARM_USER_NAME, userName);
                dbManager.AddParameters(2, PARM_ENTITY_ID, entityId);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, isActive);
                dbManager.AddParameters(4, PARM_SECURITY_USER_ID, SecurityUserId);


                var reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_USER_GET_IS_FULL_SSN);
                string IsFullSSN = "";
                while (reader.Read())
                {
                    IsFullSSN = reader["IsFullSSN"].ToString();
                }
                return IsFullSSN;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoginUser_", PROC_USER_GET_IS_FULL_SSN, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public void InsertUsersActivityLog(DALUsersActivity.AuditableEvents Event, string FormName, Boolean IsSuccess, Boolean IsEmergencyAccess, string UserName, string Details = "", string MRNo = "", string VisitAccountNo = "")
        {
            string connectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref connectionString, "");

            try
            {

                DSUsersActivity ds = new DSUsersActivity();
                DSUsersActivity.UsersActivityLogRow drUsersActivity = ds.UsersActivityLog.NewUsersActivityLogRow();
                drUsersActivity.ActivityLogDate = DateTime.Now;

                drUsersActivity.ActivityStatus = IsSuccess;
                drUsersActivity.ActivityEventId = (int)Event;
                drUsersActivity.IsEmergency = IsEmergencyAccess;
                drUsersActivity.UserName = ClientConfiguration.DecryptFrom64(UserName);

                drUsersActivity.MachineIp = MDVUtility.GetLanIPAddress();
                drUsersActivity.FormName = FormName;






                ds.UsersActivityLog.AddUsersActivityLogRow(drUsersActivity);

                //remove activity Edit

                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSUsersActivity)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_USER_ACTIVITY_LOG_INSERT, ds, ds.UsersActivityLog.TableName);
                ds.AcceptChanges();

            }
            catch (Exception ex)
            {
                MDVLogger.SendExcepToDBForMobileAPI(ex, "MobileAPI::sp_UsersActivityLogInsert", "MobileAppLogin");
                MDVLogger.LogMobileLoginException(ex.ToString(), MDVUtility.DecryptFrom64( UserName));
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }




        public List<SoftwareCustomerInfoModel> LoadCustomerInfoNative(Parameters param)
        {
            List<SoftwareCustomerInfoModel> softwareCustomerInfoList = new List<SoftwareCustomerInfoModel>();
            string connectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref connectionString, param.USER_NAME);
            SqlDataReader reader = null;
            try
            {
                if (param.USER_NAME == "")
                    param.USER_NAME = null;

                if (param.USER_PASSWORD == "")
                    param.USER_PASSWORD = null;

                if (param.CUSTOMER_REG_CODE == "")
                    param.CUSTOMER_REG_CODE = null;

                if (param.ENTITY_REG_CODE == "")
                    param.ENTITY_REG_CODE = null;

                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_USER_NAME, param.USER_NAME);
                dbManager.AddParameters(1, PARM_USER_PASSWORD, param.USER_PASSWORD);
                dbManager.AddParameters(2, PARM_CUSTOMER_REG_CODE, param.CUSTOMER_REG_CODE);
                dbManager.AddParameters(3, PARM_ENTITY_REG_CODE, param.ENTITY_REG_CODE);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CUSTOMER_LOGIN_INFO_FOR_SERVICE_SELECT);
                string errorMessage = "";
                while (reader.Read())
                {
                    errorMessage = reader["ErrorMessage"].ToString();
                }
                if (string.IsNullOrEmpty(errorMessage))
                {
                    reader.NextResult();

                    while (reader.Read())
                    {
                        SoftwareCustomerInfoModel model = new SoftwareCustomerInfoModel
                        {
                            CustomerId =
                                !string.IsNullOrEmpty(reader["CustomerId"].ToString())
                                    ? reader["CustomerId"].ToString()
                                    : "",
                            UserId = !string.IsNullOrEmpty(reader["UserId"].ToString()) ? reader["UserId"].ToString() : "",
                            CustomerRegCode =
                                !string.IsNullOrEmpty(reader["CustomerRegCode"].ToString())
                                    ? reader["CustomerRegCode"].ToString()
                                    : "",
                            EntityId =
                                !string.IsNullOrEmpty(reader["EntityId"].ToString()) ? reader["EntityId"].ToString() : "",
                            EntityRegCode =
                                !string.IsNullOrEmpty(reader["EntityRegCode"].ToString())
                                    ? reader["EntityRegCode"].ToString()
                                    : "",
                            IsAdmin =
                                !string.IsNullOrEmpty(reader["IsAdmin"].ToString()) ? reader["IsAdmin"].ToString() : ""
                                ,
                            IsMobileLogin =
                                !string.IsNullOrEmpty(reader["IsMobileLogin"].ToString()) ? reader["IsMobileLogin"].ToString() : ""
                            ,
                            IsFullSSN =
                                !string.IsNullOrEmpty(reader["IsFuLLSSN"].ToString()) ? reader["IsFuLLSSN"].ToString() : ""
                            ,MobSessionExpTime =
                                !string.IsNullOrEmpty(reader["MobSessionExpTime"].ToString()) ? reader["MobSessionExpTime"].ToString() : ""
                           

                        }; 
 
                        softwareCustomerInfoList.Add(model);
                    }
                }
                else
                {
                    reader.Close();
                    throw new Exception(errorMessage);
                }

                return softwareCustomerInfoList;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCustomers::LoadCustomerInfoNative", PROC_CUSTOMER_LOGIN_INFO_SELECT, ex);


                throw new Exception(ex.Message);
            }
            finally
            {
                reader.Close();
                dbManager.Dispose();
            }


        }

        public SoftwareCustomerInfoModelwithEintiy LoadCustomerInfoNativeForCheckInApp(Parameters param)
        {
            SoftwareCustomerInfoModelwithEintiy ListModel = new SoftwareCustomerInfoModelwithEintiy();
            ListModel.SoftwareCustomerInfoModelList = new List<SoftwareCustomerInfoModel>();
            ListModel.CustomerRegCodeList = new List<EntityList>();
            List<SoftwareCustomerInfoModel> softwareCustomerInfoList = new List<SoftwareCustomerInfoModel>();
            List<EntityList> softwareCustomerEntityList = new List<EntityList>();
            string connectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref connectionString, param.USER_NAME);
            SqlDataReader reader = null;
            try
            {
                if (param.USER_NAME == "")
                    param.USER_NAME = null;

                if (param.USER_PASSWORD == "")
                    param.USER_PASSWORD = null;

                if (param.CUSTOMER_REG_CODE == "")
                    param.CUSTOMER_REG_CODE = null;

                if (param.ENTITY_REG_CODE == "")
                    param.ENTITY_REG_CODE = null;

                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_USER_NAME, param.USER_NAME);
                dbManager.AddParameters(1, PARM_USER_PASSWORD, param.USER_PASSWORD);
                dbManager.AddParameters(2, PARM_CUSTOMER_REG_CODE, param.CUSTOMER_REG_CODE);
                dbManager.AddParameters(3, PARM_ENTITY_REG_CODE, param.ENTITY_REG_CODE);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CUSTOMER_LOGIN_INFO_FOR_CheckInApp);
                string errorMessage = "";
                while (reader.Read())
                {
                    errorMessage = reader["ErrorMessage"].ToString();
                }
                if (string.IsNullOrEmpty(errorMessage))
                {
                    reader.NextResult();

                    while (reader.Read())
                    {
                        SoftwareCustomerInfoModel model = new SoftwareCustomerInfoModel
                        {
                            CustomerId =
                                !string.IsNullOrEmpty(reader["CustomerId"].ToString())
                                    ? reader["CustomerId"].ToString()
                                    : "",
                            UserId = !string.IsNullOrEmpty(reader["UserId"].ToString()) ? reader["UserId"].ToString() : "",
                            CustomerRegCode =
                                !string.IsNullOrEmpty(reader["CustomerRegCode"].ToString())
                                    ? reader["CustomerRegCode"].ToString()
                                    : "",
                            EntityId =
                                !string.IsNullOrEmpty(reader["EntityId"].ToString()) ? reader["EntityId"].ToString() : "",
                            EntityRegCode =
                                !string.IsNullOrEmpty(reader["EntityRegCode"].ToString())
                                    ? reader["EntityRegCode"].ToString()
                                    : "",
                            IsAdmin =
                                !string.IsNullOrEmpty(reader["IsAdmin"].ToString()) ? reader["IsAdmin"].ToString() : ""
                                ,
                            IsMobileLogin =
                                !string.IsNullOrEmpty(reader["IsMobileLogin"].ToString()) ? reader["IsMobileLogin"].ToString() : ""
                            ,
                            IsFullSSN =
                                !string.IsNullOrEmpty(reader["IsFuLLSSN"].ToString()) ? reader["IsFuLLSSN"].ToString() : ""
                            ,
                            MobSessionExpTime =
                                !string.IsNullOrEmpty(reader["MobSessionExpTime"].ToString()) ? reader["MobSessionExpTime"].ToString() : ""


                        };

                        softwareCustomerInfoList.Add(model);
                    }
                    reader.NextResult();

                    while (reader.Read())
                    {
                        EntityList model = new EntityList
                        {
                            EntityId =
                                !string.IsNullOrEmpty(reader["EntityId"].ToString())
                                    ? reader["EntityId"].ToString()
                                    : "",
                            EntityRegCode = !string.IsNullOrEmpty(reader["EntityName"].ToString()) ? reader["EntityName"].ToString() : "",
                            FacilityId =
                                !string.IsNullOrEmpty(reader["FacilityId"].ToString())
                                    ? reader["FacilityId"].ToString()
                                    : "",
                            FacilityName =
                                !string.IsNullOrEmpty(reader["FacilityName"].ToString()) ? reader["FacilityName"].ToString() : "",
                           

                        };

                        softwareCustomerEntityList.Add(model);
                    }

                }
                else
                {
                    reader.Close();
                    throw new Exception(errorMessage);
                }
                ListModel.SoftwareCustomerInfoModelList = softwareCustomerInfoList;
                ListModel.CustomerRegCodeList = softwareCustomerEntityList;
                return ListModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCustomers::LoadCustomerInfoNativeForCheckInApp", PROC_CUSTOMER_LOGIN_INFO_FOR_CheckInApp, ex);


                throw new Exception(ex.Message);
            }
            finally
            {
                reader.Close();
                dbManager.Dispose();
            }


        }
        public string SaveEntityData(EntityData Model)
        {
            List<EntityData> listobj = new List<EntityData>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                
                dbManager.AddParameters("@ConfigurationId", Model.ConfigurationId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters("@IMEI", Model.DeviceId);
                dbManager.AddParameters("@UserId", Model.UserId);
                dbManager.AddParameters("@FacilityId", Model.FacilityId);
                dbManager.AddParameters("@EntityId", Model.EntityId);
                listobj = dbManager.ExecuteReaderMapper<EntityData>(PROC_ENTITYDATA_SAVE);

                return null;
            }
            catch (Exception e)
            {
                
                return e.Message.ToString();
            }


        }
        public List<NetworkIp> LoadNetworkIP()
        {
            List<NetworkIp> listobj = new List<NetworkIp>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                listobj = dbManager.ExecuteReaderMapper<NetworkIp>(PROC_NETWORKIP_LOAD);

                return listobj;
            }
            catch (Exception e)
            {

                return null;
            }


        }
        public string GetAccountLockoutThreshold(Parameters param)
        {
            string connectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref connectionString, param.USER_NAME);
            try
            {
                if (param.USER_NAME == "")
                    param.USER_NAME = null;

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_USER_NAME, param.USER_NAME);
                var returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ACCOUNT_LOCKOUT_THRESHOLD_SELECT).ToString();
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCustomers::GetAccountLockoutThreshold", PROC_ACCOUNT_LOCKOUT_THRESHOLD_SELECT, ex);
                var str = ex.Message.Split('|');
                return str.Length > 1 ? str[1] : ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string LockUserAccount(Parameters param)
        {
            string connectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref connectionString, param.USER_NAME);
            try
            {
                if (param.USER_NAME == "")
                    param.USER_NAME = null;

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_USER_NAME, param.USER_NAME);
                var returnValue = MDVUtility.ToStr(dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_USERS_ACCOUNT_LOCK));
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCustomers::LockUserAccount", PROC_USERS_ACCOUNT_LOCK, ex);
                string[] str = ex.Message.Split('|');
                return str.Length > 1 ? str[1] : ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion
    }
}
