//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using MDVision.Datasets;
//using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
//using System.Data;
//using System.ComponentModel;

//namespace MDVision.DataAccess.DAL.Admin
//{
//    public class DALUsersActivity
//    {
//        #region Variable
//        public enum AuditableEvents
//        {   LogIn=1,
//            LogOut=2,
//            LockApplication=3,
//            PasswordReset=4,
//            Search=5,
//            View=6,
//            Insert=7,
//            Update=8,
//            Delete=9,
//            Print=10,
//            ExportFile=11,
//            ImportFile=12
//        }

//        public static Boolean LogIn { set; get; }
//        public static Boolean LogOut { set; get; }
//        public static Boolean PasswordReset { set; get; }
//        public static Boolean LockApplication { set; get; }
//        public static Boolean Search { set; get; }
//        public static Boolean View { set; get; }
//        public static Boolean Insert { set; get; }
//        public static Boolean Update { set; get; }
//        public static Boolean Delete { set; get; }
//        public static Boolean Print { set; get; }

//        public static Boolean ExportFile { set; get; }
//        public static Boolean ImportFile { set; get; }

//        



//        #endregion

//        #region "Stored Procedure Names"
//        private const string PROC_USER_ACTIVITY_LOG_INSERT = "System.sp_UsersActivityLogInsert";
//        private const string PROC_USER_ACTIVITY_LOG_SELECT = "System.sp_UsersActivityLogSelect";
//        private const string PROC_USER_ACTIVITY_EVENT_LOOKUP = "System.sp_UsersActivityEventLookup";
//        #endregion

//        #region "Parameters"
//        private const string PARM_USER_ACTIVITY_ID = "@UserActivityId";
//        private const string PARM_ACTIVITY_LOG_DATE = "@ActivityLogDate";
//        private const string PARM_ACTIVITY_EVENT_ID = "@ActivityEventId";
//        private const string PARM_USER_NAME = "@UserName";
//        private const string PARM_ACTIVITY_STATUS = "@ActivityStatus";
//        private const string PARM_DETAILS = "@Details";
//        private const string PARM_MRNO = "@MRNo";
//        private const string PARM_MACHINE_IP = "@MachineIp";
//        private const string PARM_IS_EMERGENCY = "@IsEmergency";
//        private const string PARM_VISIT_ACCOUNTNO = "@VisitAccountNo";
//        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
      
//        #endregion

//        #region Constructors
//        /// <summary>
//        /// Initializes a new instance of the <see cref="DALPlanFeeLink"/> class.
//        /// </summary>
//        /// <param name="Obj">The object.</param>
//        public DALUsersActivity()
//        {
//            InitializeComponent();
//            ClientConfiguration.SetClientObject();
//           
//        }
//        private IContainer components;
//        //NOTE: The following procedure is required by the Web Services Designer
//        //It can be modified using the Web Services Designer.  
//        //Do not modify it using the code editor.
//        [System.Diagnostics.DebuggerStepThrough()]
//        private void InitializeComponent()
//        {
//            components = new System.ComponentModel.Container();
//        }
//        #endregion

//        #region "Support Functions"
//        /// <summary>
//        /// Creates the parameters.
//        /// </summary>
//        /// <param name="dbManager">The database manager.</param>
//        /// <param name="ds">The ds.</param>
//        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
//        private void CreateParameters(IDBManager dbManager, DSUsersActivity ds, Boolean IsInsert)
//        {
//            dbManager.CreateParameters(ds.Tables[ds.UsersActivityLog.TableName].Columns.Count);

//            if (IsInsert == true)
//                dbManager.AddParameters(0, PARM_USER_ACTIVITY_ID, ds.UsersActivityLog.UserActivityIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
//            else
//                dbManager.AddParameters(0, PARM_USER_ACTIVITY_ID, ds.UsersActivityLog.UserActivityIdColumn.ColumnName, DbType.Int64);

//            dbManager.AddParameters(1, PARM_ACTIVITY_LOG_DATE, ds.UsersActivityLog.ActivityLogDateColumn.ColumnName, DbType.DateTime);
//            dbManager.AddParameters(2, PARM_ACTIVITY_EVENT_ID, ds.UsersActivityLog.ActivityEventIdColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(3, PARM_USER_NAME, ds.UsersActivityLog.UserNameColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(4, PARM_ACTIVITY_STATUS, ds.UsersActivityLog.ActivityStatusColumn.ColumnName, DbType.Boolean);
//            dbManager.AddParameters(5, PARM_DETAILS, ds.UsersActivityLog.DetailsColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(6, PARM_MRNO, ds.UsersActivityLog.MRNoColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(7, PARM_MACHINE_IP, ds.UsersActivityLog.MachineIpColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(8, PARM_IS_EMERGENCY, ds.UsersActivityLog.IsEmergencyColumn.ColumnName, DbType.Boolean);
//            dbManager.AddParameters(9, PARM_VISIT_ACCOUNTNO, ds.UsersActivityLog.VisitAccountNoColumn.ColumnName, DbType.String);
            
//        }
//        #endregion

//        #region "Insert, update and get using dataset Functions"


//        public DSUsersActivity LoadUsersActivityLog(long UserActivityId, string UserName, string ActivityLogDate, string MachineIp)
//        {
//            DSUsersActivity ds = new DSUsersActivity();
//            IDBManager dbManager =ClientConfiguration.GetDBManager();
//            try
//            {
               

//                dbManager.Open();
//                dbManager.CreateParameters(4);
//                if (UserActivityId == 0)
//                    dbManager.AddParameters(0, PARM_USER_ACTIVITY_ID, null);
//                else
//                    dbManager.AddParameters(0, PARM_USER_ACTIVITY_ID, UserActivityId);

//                if (UserName == "")
//                    dbManager.AddParameters(1, PARM_USER_NAME, null);
//                else
//                    dbManager.AddParameters(1, PARM_USER_NAME, UserName);

//                if (ActivityLogDate == "")
//                    dbManager.AddParameters(2, PARM_ACTIVITY_LOG_DATE, null);
//                else
//                    dbManager.AddParameters(2, PARM_ACTIVITY_LOG_DATE, ActivityLogDate);

//                if (MachineIp == "")
//                    dbManager.AddParameters(3, PARM_MACHINE_IP, null);
//                else
//                    dbManager.AddParameters(3, PARM_MACHINE_IP, MachineIp);

//                ds = (DSUsersActivity)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_ACTIVITY_LOG_SELECT, ds, ds.UsersActivityLog.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DALUsersActivity::LoadUsersActivityLog", PROC_USER_ACTIVITY_LOG_SELECT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }


//        public void InsertUsersActivityLog(AuditableEvents Event,string dbTableName, Boolean  IsSuccess , string MRNo = "", string VisitAccountNo="" , string Details = "")
//        {
//             DSModuleForm ds1 = new DSModuleForm();
//             ds1 = new DAL.Admin.DALModulesForms(SharedObj).LoadForms(0, "", "", dbTableName);

//            if (ds1.Forms.Rows.Count > 0)
//            {

//                string FormName = ds1.Forms.Rows[0][ds1.Forms.NameColumn.ColumnName].ToString();
//                if (IsActiveEvent((int)Event) == true)
//                {
//                    IDBManager dbManager =ClientConfiguration.GetDBManager();
//                    try
//                    {

//                        DSUsersActivity ds = new DSUsersActivity();
//                        DSUsersActivity.UsersActivityLogRow drUsersActivity = ds.UsersActivityLog.NewUsersActivityLogRow();
//                        drUsersActivity.ActivityLogDate = DateTime.Now;

//                        drUsersActivity.ActivityStatus = IsSuccess;
//                        drUsersActivity.ActivityEventId = (int)Event;
//                        drUsersActivity.IsEmergency = SharedObj.IsEmergencyAccess;
//                        drUsersActivity.UserName = MDVSession.Current.AppUserName;
//                        drUsersActivity.Details = Details;

//                        drUsersActivity.MRNo = MRNo;
//                        drUsersActivity.VisitAccountNo = VisitAccountNo;
//                        drUsersActivity.MachineIp = SharedObj.MachineIP;
//                        drUsersActivity.FormName = FormName;
//                        ds.UsersActivityLog.AddUsersActivityLogRow(drUsersActivity);

//                        //remove activity Edit

//                        dbManager.Open();
//                        CreateParameters(dbManager, ds, true);
//                        ds = (DSUsersActivity)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_USER_ACTIVITY_LOG_INSERT, ds, ds.UsersActivityLog.TableName);
//                        ds.AcceptChanges();

//                    }
//                    catch (Exception ex)
//                    {
//                        MDVLogger.LogErrorMessage("DALUsersActivity::InsertUsersActivityLog", PROC_USER_ACTIVITY_LOG_INSERT, ex);
//                        throw ex;
//                    }
//                    finally
//                    {
//                        dbManager.Dispose();
//                    }
//                }
//            }
             
//        }

       
//        public DSUsersActivity GetUsersActivityEventLookup()
//        {
//            DSUsersActivity ds = new DSUsersActivity();
//            IDBManager dbManager =ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();

//                ds = (DSUsersActivity)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_USER_ACTIVITY_EVENT_LOOKUP, ds, ds.UsersActivityEvent.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DALUsersActivity::GetUsersActivityEventLookup", PROC_USER_ACTIVITY_EVENT_LOOKUP, ex);
//                throw ex;
//                //Usual code              
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }

//        public static void SetEventProperty(DSUsersActivity ds)
//        {

//            try
//            {

//                foreach (DSUsersActivity.UsersActivityEventRow row in ds.UsersActivityEvent.Rows)
//                {

//                    if (row.ActivityEventId == (int)AuditableEvents.LogIn)

//                        LogIn = row.IsActive;

//                    if (row.ActivityEventId == (int)AuditableEvents.LogOut)

//                        LogOut = row.IsActive;
                    

//                    if (row.ActivityEventId == (int)AuditableEvents.PasswordReset)

//                        PasswordReset = row.IsActive;


//                    if (row.ActivityEventId == (int)AuditableEvents.LockApplication)

//                        LockApplication = row.IsActive;


//                    if (row.ActivityEventId == (int)AuditableEvents.Search)

//                        Search = row.IsActive;


//                    if (row.ActivityEventId == (int)AuditableEvents.View)
                    
//                        View = row.IsActive;
                    

//                    if (row.ActivityEventId == (int)AuditableEvents.Insert)
                    
//                        Insert = row.IsActive;
                    

//                    if (row.ActivityEventId == (int)AuditableEvents.Update)
                    
//                        Update = row.IsActive;
                    

//                    if (row.ActivityEventId == (int)AuditableEvents.Delete)
                    
//                        Delete = row.IsActive;
                    

//                    if (row.ActivityEventId == (int)AuditableEvents.Print)
                    
//                        Print = row.IsActive;
                    

//                    if (row.ActivityEventId == (int)AuditableEvents.ExportFile)
                    
//                        ExportFile = row.IsActive;
                    

//                    if (row.ActivityEventId == (int)AuditableEvents.ImportFile)
                    
//                        ImportFile = row.IsActive;
                    

//                }

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

       

//        private static bool IsActiveEvent(int AuditableEvent)
//        {
//            try
//            {
//                if (AuditableEvent == (int) AuditableEvents.LogIn)
               
//                    return LogIn;
               
//                if (AuditableEvent == (int)AuditableEvents.LogOut)
               
//                    return LogOut;
               
//                if (AuditableEvent == (int)AuditableEvents.PasswordReset)
                
//                    return PasswordReset;
                
//                if (AuditableEvent == (int)AuditableEvents.LockApplication)
                
//                    return LockApplication;
                
//                if (AuditableEvent == (int)AuditableEvents.Search)
                
//                    return Search;
                
//                if (AuditableEvent == (int)AuditableEvents.View)
                
//                    return View;
                
//                if (AuditableEvent == (int)AuditableEvents.Insert)
                
//                    return Insert;
                
//                if (AuditableEvent == (int)AuditableEvents.Update)
                
//                    return Update;
                
//                if (AuditableEvent == (int)AuditableEvents.Delete)
                
//                    return Delete;
                
//                if (AuditableEvent == (int)AuditableEvents.Print)
                
//                    return Print;
                
//                if (AuditableEvent == (int)AuditableEvents.ExportFile)
                
//                    return ExportFile;
                
//                if (AuditableEvent == (int)AuditableEvents.ImportFile)
                
//                    return ImportFile;
                

//                return false;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }


//        #endregion
//    }
//}
