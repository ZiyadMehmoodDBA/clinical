using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MDVision.Datasets;
using MDVision.DataAccess.DAL ;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Business.BCommon;
using System.Configuration;

namespace MDVision.Business.BLL
{
    ////public class BLLLogin
    ////{

    ////    #region Variable
    ////      
    ////    #endregion



       
    ////    //#region Singleton
    ////    //private static BLLLogin _instance = null;
    ////    ///// <summary>
    ////    ///// Singleton context
    ////    ///// </summary>
    ////    //public static BLLLogin Instance
    ////    //{
    ////    //    get
    ////    //    {
    ////    //        if (_instance == null)
    ////    //            _instance = new BLLLogin();
    ////    //        return _instance;
    ////    //    }
    ////    //}
    ////    //#endregion

    ////    //public struct ReturnParameters
    ////    //{
    ////    //    public string Token;
    ////    //    public string ServerDateTime;
    ////    //    public string EmployeeId;
    ////    //    public string EmployeeTypeId;
    ////    //    public string RoleId;
    ////    //    public string FullEmployeeName;
    ////    //    public string UserName;
    ////    //    public string EmailAddress;
    ////    //    public string VIPAccess;
    ////    //    public string IsLogin;
    ////    //    public string IsRoleActive;
    ////    //    public string IsEmployeeActive;
    ////    //    public string RoleData;
    ////    //    public string DsUserWorkFlow;
    ////    //    public string DsAuditEvents;
    ////    //    public string XMLFileData;
    ////    //    public string RegistrationCode;
    ////    //    public string DisplayWizard;

    ////    //    public string Fax;
    ////    //    public string PasswordResetByAdmin;
    ////    //    public string PasswordUpdatedDate;
    ////    //    public string PasswordExpiration;

    ////    //    public string PasswordExpired;
    ////    //    public string Blocked;
    ////    //    public string BlockedMessage;
    ////    //    public List<string> LicenceNames;
    ////    //    public List<long> LicenceExpDays;
    ////    //    public bool InvalidUser;
    ////    //    public string WelcomeScrRefresh;
    ////    //    public string AccountingClient;

    ////    //    public string AllowedFacilityIDs;
    ////    //    public string BEID;

    ////    //    public string DSUserLoginEntry;
    ////    //    public long LoginFacilityId;
    ////    //    public int LoginSessionTimeoutInterval;
    ////    //    public string IsEmergency;

    ////    //    public string EmergencyAllowed;

    ////    //}
    ////    // const string MAXATTEMPTS1  = "You have reached maximum attempts for incorrect login. Please contact your administrator. ";
    ////    // const string MAXATTEMPTS2 = "You have reached maximum attempts for incorrect login. Your account has been temporary disabled." + "\r\n Please contact your administrator.";

        
    ////    #region "Functions"

    ////     public BLObject<DSUsers> Login(ref DSSoftwareCustomersInfo ds, string UserName, string UserPassword, string CustomerRegCode = "", string EntityRegCode = "", string UserMachineIPAddress = "")
    ////    {
    ////        //ReturnParameters RtdParameter = new ReturnParameters();
    ////        UserName = MDVUtility.DecryptFrom64(UserName);
    ////        UserPassword = MDVUtility.DecryptFrom64(UserPassword);
    ////        try
    ////        {
    ////            if (ds == null)
    ////            {
    ////                if (EntityRegCode!="")
    ////                {
    ////                    BLObject<DSSoftwareCustomersInfo> ObjGetCustomer = GetCustomerSettings(UserName, UserPassword, CustomerRegCode, EntityRegCode);
    ////                    ds = ObjGetCustomer.Data;
    ////                }
    ////            }
                
    ////            if (ds!=null)
    ////            {
                    

    ////                if (ds.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows.Count > 0)
    ////                {
    ////                    // client connection information
    ////                    DataRow dr = ds.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows[0];
    ////                    ClientConfiguration.DBUserId = dr[DSSoftwareCustomersInfo.FIELD_DB_USER_ID].ToString();
    ////                    ClientConfiguration.DBPassword = dr[DSSoftwareCustomersInfo.FIELD_DB_PASSWORD].ToString();
    ////                    ClientConfiguration.DataSource = dr[DSSoftwareCustomersInfo.FIELD_DATA_SOURCE].ToString();
    ////                    ClientConfiguration.InitialCatalog = dr[DSSoftwareCustomersInfo.FIELD_DB_NAME].ToString();
    ////                    if (dr[DSSoftwareCustomersInfo.FIELD_PERSIST_SECURITY_INFO].ToString() != "")
    ////                        ClientConfiguration.PersistSecurityInfo = Convert.ToBoolean(dr[DSSoftwareCustomersInfo.FIELD_PERSIST_SECURITY_INFO].ToString());
    ////                    else
    ////                        ClientConfiguration.PersistSecurityInfo = false;

    ////                    if (dr[DSSoftwareCustomersInfo.FIELD_IS_PROXY].ToString() != "")
    ////                        ClientConfiguration.IsProxy = Convert.ToBoolean(dr[DSSoftwareCustomersInfo.FIELD_IS_PROXY].ToString());
    ////                    else
    ////                        ClientConfiguration.IsProxy = false;

                      
    ////                    ClientConfiguration.PoolingString = dr[DSSoftwareCustomersInfo.FIELD_POOLING_STRING].ToString();
    ////                    ClientConfiguration.DBProviderType = dr[DSSoftwareCustomersInfo.FIELD_PROVIDER_TYPE].ToString();

    ////                    ClientConfiguration.Entity_ID = dr[DSSoftwareCustomersInfo.FIELD_ENTITY_ID].ToString();
    ////                    ClientConfiguration.Client_ID = dr[DSSoftwareCustomersInfo.FIELD_UNIQUE_CLIENT_ID ].ToString();
    ////                    ClientConfiguration.WebEntityURL = dr[DSSoftwareCustomersInfo.FIELD_WEB_SERVICE_URL].ToString();
                     
    ////                     //get user information
    ////                    DSUsers dsUser =new DSUsers();
    ////                    dsUser = DALUser.Instance.LoadUser(ref dsUser, 0, UserName, null,"1");

    ////                    // InsertLoginAttempt(True, IPAddress, param.UserName)
    ////                    if (UserName.ToUpper()  != "MDVISION" && dsUser.Tables [dsUser.Users.TableName].Rows.Count <= 0 )
    ////                    {                          
                         
                           
    ////                        //     // check here user option " like login attempt
    ////                        //      dsUser = DLLUser.Instance.LoadEntityUserOption(ref dsUser, UserName, ClientConfiguration.Entity_ID);
    ////                        //DLLUser.Instance.LoadEntityUserOption                     
                       
                       
    ////                         throw new Exception("User is inactive or not created.");
    ////                    }

    ////                    // set property

    ////                    // get the user entity option
                        
    ////                    dsUser = DALUser.Instance.LoadEntityUserOption(ref dsUser, UserName, ClientConfiguration.Entity_ID);

    ////                    //get the user privileges
    ////                    dsUser = DALUser.Instance.LoadUserPrivileges (ref dsUser, UserName);

    ////                    //if (UserName != "MDVISION" && dsUser.Tables(0).Rows(0).Item("ACTIVE_FLAG").tostring = 'N')
    ////                    //{

    ////                    //}
    ////                    //    //drUser["Active_FLAG"].ToString() == "Y" ? true : false;

    ////                    //    throw new Exception(UserName + " is Inactive.");
    ////                    //}


    ////                    //ClientID = drUser["CLIENT_ID"].ToString();


    ////                    ClientConfiguration.UserLoginName = UserName;
    ////                    ClientConfiguration.UserLoginPassword = UserPassword;


    ////                    return new BLObject<DSUsers>(dsUser);
    ////                }
    ////                else
    ////                    throw new Exception("Selected entity not registered.");


    ////            }
    ////            return new BLObject<DSUsers>(null);
    ////        }


    ////        catch (Exception ex)
    ////        {

    ////            MDVLogger.LogErrorMessage("BLLLogin::Login", ex);
    ////            return new BLObject<DSUsers>(null, ex.Message);

    ////        }
    ////    }




    ////     public BLObject<DSSoftwareCustomersInfo> GetCustomerSettings(string UserName, string UserPassword, string CustomerRegCode = "", string EntityRegCode = "")
    ////     {
    ////         try
    ////         {
    ////             UserName = MDVUtility.DecryptFrom64(UserName);
    ////             UserPassword = MDVUtility.DecryptFrom64(UserPassword);
    ////             if (ConfigurationManager.AppSettings["CustomerRegCode"] != null)
    ////                 if (ConfigurationManager.AppSettings["CustomerRegCode"].ToString() != "")
    ////                     CustomerRegCode = ConfigurationManager.AppSettings["CustomerRegCode"].ToString();


    ////             if (ConfigurationManager.AppSettings["EntityRegCode"] != null)
    ////                 if (ConfigurationManager.AppSettings["EntityRegCode"].ToString() != "")
    ////                     EntityRegCode = ConfigurationManager.AppSettings["EntityRegCode"].ToString();

    ////             DALCustomers.Parameters Param = new DALCustomers.Parameters();

    ////             Param.USER_NAME = UserName;
    ////             Param.USER_PASSWORD = UserPassword;
    ////             Param.CUSTOMER_REG_CODE = CustomerRegCode;
    ////             Param.ENTITY_REG_CODE = EntityRegCode;

    ////             DSSoftwareCustomersInfo ds = DALCustomers.Instance.LoadCustomerInfo(Param);
    ////             if (ds.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows.Count <= 0)
    ////                 throw new Exception("Make sure your Username and Password are correct Or" + Environment.NewLine + "No entities assigned to user '" + UserName + "'.");


    ////             return new BLObject<DSSoftwareCustomersInfo>(ds);
    ////         }
    ////         catch (Exception ex)
    ////         {

    ////             MDVLogger.LogErrorMessage("BLLLogin::GetCustomerSettings", ex);
    ////             return new BLObject<DSSoftwareCustomersInfo>(null, ex.Message);

    ////         }
    ////     } 

    ////      static int attemptsMade = 0;
    ////      private bool GetLoginAttempts(int maxAttemptsAllowed)
    ////      {
    ////          if (attemptsMade >= maxAttemptsAllowed)
    ////          {
    ////              attemptsMade = 0;
    ////              return true;
    ////          }
    ////          else
    ////          {
    ////              attemptsMade = attemptsMade + 1;
    ////              return false;
    ////          }
    ////      }
        //private bool IsBlockedMachineIP(string machineIP, string lockTime,string UserName,string EntityId , ref ReturnParameters RtdParameter)
        //{
            
        //    bool result = false;
        //    DSUsers ds = new DSUsers();

        //    ds = DLLUser.Instance.LoadMachineIP(machineIP, UserName, EntityId);
          

        //    if (ds.Tables[ds.BlockMachine.TableName ].Rows.Count > 0)
        //    {


        //        DateTime nowDateTime = MDVUtility.StringToDate(ds.Tables[ds.BlockMachine.TableName].Rows[0][ds.BlockMachine.ServerDateTimeColumn.ColumnName].ToString() );
        //        DateTime blockedTime = MDVUtility.StringToDate(ds.Tables[ds.BlockMachine.TableName].Rows[0][ds.BlockMachine.BlockedTimeColumn.ColumnName].ToString());
                    
        //        if (((TimeSpan)(nowDateTime - blockedTime)).Seconds > (Convert .ToInt32 ( lockTime) * 60))
        //        {
        //            RtdParameter.Blocked = "block";
        //            result = true;
        //        }
        //        else
        //        {
        //            //functionStatus = "TIMEEXPIRE"
        //            //UnBlockMachineIP(machineIP)
        //            result = false;
        //        }
        //    }
        //    else
        //    {
              
        //        result = false;
        //    }
        //    return result;
            
        //}
       







        
                           
                            //if (IsBlockedMachineIP(UserMachineIPAddress, "",UserName ,EntityId ,ref RtdParameter ) == true) 
                            // {
                            //        RtdParameter.Blocked = "true";
                            //        RtdParameter.IsLogin = "Blocked";
                            //        RtdParameter.PasswordExpired = "false";
                            //        RtdParameter.BlockedMessage = MAXATTEMPTS2;
                            //}
                            //else
                            //{
                    //            if (noOfMaxAttempts.Length > 0) Then

                    //    if (RtdParameter.EmployeeId == String.Empty) Then
                    //        If GetLoginAttempts(IPAddress, noOfMaxAttempts, actionOnMaxAttempts, lockTime) Then
                    //            RtdParameter.Blocked = "True"
                    //            RtdParameter.PasswordExpired = False

                    //            BlockMachineIP(param.UserMachineIPAddress)
                    //            RtdParameter.BlockedMessage = MAXATTEMPTS2

                    //        Else
                    //            RtdParameter.BlockedMessage = MAXATTEMPTS1
                    //            RtdParameter.IsLogin = "InvalidUser"
                    //            RtdParameter.PasswordExpired = False

                    //        End If
                    //    Else
                    //        attemptsMade = 0
                    //    End If
                    //End If
    ////    #endregion
    ////}
}
