using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;
using System.Net;
using MDVision.Model.Security;
using System.IO;
using Newtonsoft.Json;

namespace MDVision.DataAccess.DCommon
{
    public class DALUsersActivity
    {
        #region Variable



        public const string TITLE_SOFTWARE_NAME = "MD Vision";
        public const string TITLE_SEARCH_PATIENT = "Search Patient";
        public const string TITLE_DEMOGRAPHIC = "Demographic";
        public const string TITLE_Appointment = "Appointment";
        public const string TITLE_PAYMENT = "Payment";
        public const string TITLE_PATIENT_INSURANCE = "Patient Insurance";
        public const string TITLE_DRUG_CODE_COST = "Drug Code Cost";
        public const string TITLE_USER = "User";
        public const string TITLE_USER_ENTITY_GROUP = "User Entity Group";
        //public const string  TITLE_BL_INVENTORY_CONSUMPTION  = "Int Consumption";
        //public const string TITLE_BL_CHARGE_CAPTURE = "Charge Capture";
        public enum AuditableEvents
        {
            LogIn = 1,
            LogOut = 2,
            LockApplication = 3,
            PasswordReset = 4,
            Search = 5,
            View = 6,
            Insert = 7,
            Update = 8,
            Delete = 9,
            Print = 10,
            ExportFile = 11,
            ImportFile = 12
        }

        public static Boolean LogIn { set; get; }
        public static Boolean LogOut { set; get; }
        public static Boolean PasswordReset { set; get; }
        public static Boolean LockApplication { set; get; }
        public static Boolean Search { set; get; }
        public static Boolean View { set; get; }
        public static Boolean Insert { set; get; }
        public static Boolean Update { set; get; }
        public static Boolean Delete { set; get; }
        public static Boolean Print { set; get; }

        public static Boolean ExportFile { set; get; }
        public static Boolean ImportFile { set; get; }





        #endregion

        #region "Stored Procedure Names"
        private const string PROC_USER_ACTIVITY_LOG_INSERT = "System.sp_UsersActivityLogInsert";
        private const string PROC_USER_ACTIVITY_LOG_SELECT = "System.sp_UsersActivityLogSelect";
        private const string PROC_USER_ACTIVITY_EVENT_LOOKUP = "System.sp_UsersActivityEventLookup";
        #endregion

        #region "Parameters"
        private const string PARM_USER_ACTIVITY_ID = "@UserActivityId";
        private const string PARM_ACTIVITY_LOG_DATE = "@ActivityLogDate";
        private const string PARM_ACTIVITY_EVENT_ID = "@ActivityEventId";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ACTIVITY_STATUS = "@ActivityStatus";
        private const string PARM_DETAILS = "@Details";
        private const string PARM_MRNO = "@MRNo";
        private const string PARM_MACHINE_IP = "@MachineIp";
        private const string PARM_IS_EMERGENCY = "@IsEmergency";
        private const string PARM_VISIT_ACCOUNTNO = "@VisitAccountNo";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
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


        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPlanFeeLink"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALUsersActivity()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALUsersActivity(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);

        }
        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
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

        #region "Insert, update and get using dataset Functions"


        public DSUsersActivity LoadUsersActivityLog(long UserActivityId, string UserName, string ActivityLogDate, string MachineIp)
        {
            DSUsersActivity ds = new DSUsersActivity();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(4);
                if (UserActivityId == 0)
                    dbManager.AddParameters(0, PARM_USER_ACTIVITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ACTIVITY_ID, UserActivityId);

                if (UserName == "")
                    dbManager.AddParameters(1, PARM_USER_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_USER_NAME, UserName);

                if (ActivityLogDate == "")
                    dbManager.AddParameters(2, PARM_ACTIVITY_LOG_DATE, null);
                else
                    dbManager.AddParameters(2, PARM_ACTIVITY_LOG_DATE, ActivityLogDate);

                if (MachineIp == "")
                    dbManager.AddParameters(3, PARM_MACHINE_IP, null);
                else
                    dbManager.AddParameters(3, PARM_MACHINE_IP, MachineIp);

                ds = (DSUsersActivity)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_ACTIVITY_LOG_SELECT, ds, ds.UsersActivityLog.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUsersActivity::LoadUsersActivityLog", PROC_USER_ACTIVITY_LOG_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public void InsertUsersActivityLog(AuditableEvents Event, string FormName, Boolean IsSuccess, string Details = "", string MRNo = "", string VisitAccountNo = "")
        {
            //string FormName = "";
            //if (dbTableName != "")
            //{
            //    DSModuleForm dsModulesForms = new DSModuleForm();
            //    dsModulesForms = new DAL.Admin.DALModulesForms(SharedObj).LoadForms(0, "", "", dbTableName);

            //    if (dsModulesForms.Forms.Rows.Count > 0)
            //    {
            //       FormName = dsModulesForms.Forms.Rows[0][dsModulesForms.Forms.NameColumn.ColumnName].ToString();
            //    }
            //}



            if (IsActiveEvent((int)Event) == true)
            {

                IDBManager dbManager = ClientConfiguration.GetDBManager();
                try
                {

                    DSUsersActivity ds = new DSUsersActivity();
                    DSUsersActivity.UsersActivityLogRow drUsersActivity = ds.UsersActivityLog.NewUsersActivityLogRow();
                    drUsersActivity.ActivityLogDate = DateTime.Now;

                    drUsersActivity.ActivityStatus = IsSuccess;
                    drUsersActivity.ActivityEventId = (int)Event;
                    drUsersActivity.IsEmergency = MDVSession.Current.IsEmergencyAccess;
                    drUsersActivity.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                    drUsersActivity.Details = Details;

                    drUsersActivity.MRNo = MRNo;
                    drUsersActivity.VisitAccountNo = VisitAccountNo;
                    drUsersActivity.MachineIp = MDVSession.Current.UserHostIP;
                    drUsersActivity.FormName = FormName;

                    ClientGeoLocationModel obj = getUserGeoLocation();

                    if (obj != null && obj.status == "success")
                    {
                        drUsersActivity.serverIp = MDVApplication.ServerIP;
                        drUsersActivity.serverName = MDVApplication.ServerName;
                        drUsersActivity.Ip = obj.query;
                        drUsersActivity.country = obj.country;
                        drUsersActivity.countryCode = obj.countryCode;
                        drUsersActivity.timezone = obj.timezone;
                        drUsersActivity.city = obj.city;
                        drUsersActivity._as = obj.@as;
                        drUsersActivity.isp = obj.isp;
                        drUsersActivity.lat = obj.lat;
                        drUsersActivity.lon = obj.lon;
                        drUsersActivity.org = obj.org;
                        drUsersActivity.error = obj.error;
                        drUsersActivity.IsLocal = false;

                    }
                    else
                    {
                        drUsersActivity.serverIp = MDVApplication.ServerIP;
                        drUsersActivity.serverName = MDVApplication.ServerName;
                        drUsersActivity.Ip = obj.query;
                        drUsersActivity.hostName = obj.hostName;
                        drUsersActivity.error = obj.error;
                        drUsersActivity.IsLocal = true;

                    }
                    ds.UsersActivityLog.AddUsersActivityLogRow(drUsersActivity);

                    //remove activity Edit

                    dbManager.Open();
                    CreateParameters(dbManager, ds, true);
                    ds = (DSUsersActivity)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_USER_ACTIVITY_LOG_INSERT, ds, ds.UsersActivityLog.TableName);
                    ds.AcceptChanges();

                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALUsersActivity::InsertUsersActivityLog", PROC_USER_ACTIVITY_LOG_INSERT, ex);
                    //  throw ex;
                }
                finally
                {
                    dbManager.Dispose();
                }
            }


        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="Event"></param>
        /// <param name="FormName"></param>
        /// <param name="IsSuccess"></param>
        /// <param name="Details"></param>
        /// <param name="MRNo"></param>
        /// <param name="VisitAccountNo"></param>
        public void InsertUsersActivityLog(SharedVariable SharedVariable, AuditableEvents Event, string FormName, Boolean IsSuccess, string Details = "", string MRNo = "", string VisitAccountNo = "")
        {
            //string FormName = "";
            //if (dbTableName != "")
            //{
            //    DSModuleForm dsModulesForms = new DSModuleForm();
            //    dsModulesForms = new DAL.Admin.DALModulesForms(SharedObj).LoadForms(0, "", "", dbTableName);

            //    if (dsModulesForms.Forms.Rows.Count > 0)
            //    {
            //       FormName = dsModulesForms.Forms.Rows[0][dsModulesForms.Forms.NameColumn.ColumnName].ToString();
            //    }
            //}



            if (IsActiveEvent((int)Event) == true)
            {

                IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
                try
                {

                    DSUsersActivity ds = new DSUsersActivity();
                    DSUsersActivity.UsersActivityLogRow drUsersActivity = ds.UsersActivityLog.NewUsersActivityLogRow();
                    drUsersActivity.ActivityLogDate = DateTime.Now;

                    drUsersActivity.ActivityStatus = IsSuccess;
                    drUsersActivity.ActivityEventId = (int)Event;
                    drUsersActivity.IsEmergency = SharedVariable.IsEmergencyAccess;
                    drUsersActivity.UserName = ClientConfiguration.DecryptFrom64(SharedVariable.UserName);
                    drUsersActivity.Details = Details;

                    drUsersActivity.MRNo = MRNo;
                    drUsersActivity.VisitAccountNo = VisitAccountNo;
                    drUsersActivity.MachineIp = SharedVariable.UserHostIP;
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
                    MDVLogger.DALErrorLog(SharedVariable, "DALUsersActivity::InsertUsersActivityLog", PROC_USER_ACTIVITY_LOG_INSERT, ex);
                    throw ex;
                }
                finally
                {
                    dbManager.Dispose();
                }
            }


        }


        public DSUsersActivity GetUsersActivityEventLookup()
        {
            DSUsersActivity ds = new DSUsersActivity();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();

                ds = (DSUsersActivity)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_ACTIVITY_EVENT_LOOKUP, ds, ds.UsersActivityEvent.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUsersActivity::GetUsersActivityEventLookup", PROC_USER_ACTIVITY_EVENT_LOOKUP, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public static void SetEventProperty(DSUsersActivity ds)
        {

            try
            {

                foreach (DSUsersActivity.UsersActivityEventRow row in ds.UsersActivityEvent.Rows)
                {

                    if (row.ActivityEventId == (int)AuditableEvents.LogIn)

                        LogIn = row.IsActive;

                    if (row.ActivityEventId == (int)AuditableEvents.LogOut)

                        LogOut = row.IsActive;


                    if (row.ActivityEventId == (int)AuditableEvents.PasswordReset)

                        PasswordReset = row.IsActive;


                    if (row.ActivityEventId == (int)AuditableEvents.LockApplication)

                        LockApplication = row.IsActive;


                    if (row.ActivityEventId == (int)AuditableEvents.Search)

                        Search = row.IsActive;


                    if (row.ActivityEventId == (int)AuditableEvents.View)

                        View = row.IsActive;


                    if (row.ActivityEventId == (int)AuditableEvents.Insert)

                        Insert = row.IsActive;


                    if (row.ActivityEventId == (int)AuditableEvents.Update)

                        Update = row.IsActive;


                    if (row.ActivityEventId == (int)AuditableEvents.Delete)

                        Delete = row.IsActive;


                    if (row.ActivityEventId == (int)AuditableEvents.Print)

                        Print = row.IsActive;


                    if (row.ActivityEventId == (int)AuditableEvents.ExportFile)

                        ExportFile = row.IsActive;


                    if (row.ActivityEventId == (int)AuditableEvents.ImportFile)

                        ImportFile = row.IsActive;


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        private static bool IsActiveEvent(int AuditableEvent)
        {
            try
            {
                if (AuditableEvent == (int)AuditableEvents.LogIn)

                    return LogIn;

                if (AuditableEvent == (int)AuditableEvents.LogOut)

                    return LogOut;

                if (AuditableEvent == (int)AuditableEvents.PasswordReset)

                    return PasswordReset;

                if (AuditableEvent == (int)AuditableEvents.LockApplication)

                    return LockApplication;

                if (AuditableEvent == (int)AuditableEvents.Search)

                    return Search;

                if (AuditableEvent == (int)AuditableEvents.View)

                    return View;

                if (AuditableEvent == (int)AuditableEvents.Insert)

                    return Insert;

                if (AuditableEvent == (int)AuditableEvents.Update)

                    return Update;

                if (AuditableEvent == (int)AuditableEvents.Delete)

                    return Delete;

                if (AuditableEvent == (int)AuditableEvents.Print)

                    return Print;

                if (AuditableEvent == (int)AuditableEvents.ExportFile)

                    return ExportFile;

                if (AuditableEvent == (int)AuditableEvents.ImportFile)

                    return ImportFile;


                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ClientGeoLocationModel getUserGeoLocation()
        {
            string ip = string.Empty;
            bool IsLocal = false;
            string hostName = string.Empty;



            try
            {

                MDVSession.Current.GetVisitorIPAddress(ref ip, ref hostName, ref IsLocal);
                if (IsLocal)
                {
                    return new ClientGeoLocationModel()
                    {
                        // status = "success",
                        query = ip,
                        IsLocal = true,
                        hostName = hostName
                    };

                }
                else
                {
                    return new ClientGeoLocationModel()
                    {
                        // status = "success",
                        query = ip,
                        IsLocal = true,
                        hostName = hostName
                    };

                    /*
                    string rt;

                    WebRequest request = WebRequest.Create("http://ip-api.com/json/" + ip);

                    WebResponse response = request.GetResponse();

                    Stream dataStream = response.GetResponseStream();

                    StreamReader reader = new StreamReader(dataStream);

                    rt = reader.ReadToEnd();

                    ClientGeoLocationModel clientGeoLocationModel = JsonConvert.DeserializeObject<ClientGeoLocationModel>(rt);

                    if (!string.IsNullOrEmpty(hostName))
                    {
                        clientGeoLocationModel.hostName = hostName;
                        clientGeoLocationModel.IsLocal = false;

                    }


                    reader.Close();
                    response.Close();

                    return clientGeoLocationModel;
                    */
                }
            }

            catch (Exception ex)
            {
                return new ClientGeoLocationModel()
                {
                    status = "failed",
                    query = ip,
                    error = ex.Message
                };
            }

        }


        #endregion
    }
}
