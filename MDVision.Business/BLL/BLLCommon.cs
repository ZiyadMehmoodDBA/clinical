using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.DataAccess.DAL.Admin;
using MDVision.Business.BCommon;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using System.Configuration;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Model.Security;

namespace MDVision.Business.BLL
{
    public class BLLCommon
    {
        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLCommon"/> class.
        /// </summary>
        public BLLCommon()
        {
            //This call is required by the Web Services Designer.
            InitializeComponent();
            //Add your own initialization code after the InitializeComponent() call
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

        #region "Functions"
        /// <summary>
        /// Gets the customer settings. its respective native version is also available with GetCustomerSettingsNative name
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userPassword">The user password.</param>
        /// <param name="customerRegCode">The customer reg code.</param>
        /// <param name="entityRegCode">The entity reg code.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Make sure your Username and Password are correct Or + Environment.NewLine + No entities assigned to user ' + UserName + '.</exception>
        public BLObject<DSSoftwareCustomersInfo> GetCustomerSettings(string userName, string userPassword, string customerRegCode = "", string entityRegCode = "")
        {
            try
            {
                userName = MDVUtility.DecryptFrom64(userName);
                //  UserPassword = MDVUtility.DecryptFrom64(UserPassword);
                // UserPassword = MDVUtility.EncryptSHA256(UserPassword);
                if (ConfigurationManager.AppSettings["CustomerRegCode"] != null)
                    if (ConfigurationManager.AppSettings["CustomerRegCode"] != "")
                        customerRegCode = ConfigurationManager.AppSettings["CustomerRegCode"];

                if (userName.ToUpper() != ClientConfiguration.DefaultUser)
                    if (ConfigurationManager.AppSettings["EntityRegCode"] != null)
                        if (ConfigurationManager.AppSettings["EntityRegCode"] != "")
                            entityRegCode = ConfigurationManager.AppSettings["EntityRegCode"];

                var param = new DALCustomers.Parameters
                {
                    USER_NAME = userName,
                    USER_PASSWORD = userPassword,
                    CUSTOMER_REG_CODE = customerRegCode,
                    ENTITY_REG_CODE = entityRegCode
                };

                var ds = DALCustomers.Instance.LoadCustomerInfo(param);
                if (ds.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows.Count <= 0)
                {
                    //keep counting the false attempts !
                    MDVSession.Current.LoginAttemptsCount++;
                    //throw new Exception("Make sure your username and password are correct or" + Environment.NewLine + "entity is not assigned to user '" + UserName + "'.");
                    //default value is -1,  if -1 is the value then send a request to get new value
                    if (MDVSession.Current.AccountLockoutThreshold < 0)
                    {
                        MDVSession.Current.AccountLockoutThreshold = MDVUtility.ToInt32(DALCustomers.Instance.GetAccountLockoutThreshold(param));
                        //send the call and set MDVSession.Current.AccountLockoutThreshold
                    }
                    if (MDVSession.Current.AccountLockoutThreshold <= 0 ||
                        MDVSession.Current.LoginAttemptsCount <= MDVSession.Current.AccountLockoutThreshold)
                        throw new Exception("Invalid login details.");
                    DALCustomers.Instance.LockUserAccount(param);
                    throw new Exception("This User is Locked");

                    throw new Exception("Invalid login details.");
                    //lock account
                    throw new Exception("Invalid login details.");
                }
                //logged in successfully so reset the counter
                MDVSession.Current.LoginAttemptsCount = 0;

                return new BLObject<DSSoftwareCustomersInfo>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::GetCustomerSettings", ex);
                return new BLObject<DSSoftwareCustomersInfo>(null, ex.Message);
            }
        }
        public List<SoftwareCustomerInfoModel_> GetCustomerSettingsList(string userName, string userPassword, string customerRegCode = "", string entityRegCode = "")
        {
            try
            {
                userName = MDVUtility.DecryptFrom64(userName);

                if (ConfigurationManager.AppSettings["CustomerRegCode"] != null)
                    if (ConfigurationManager.AppSettings["CustomerRegCode"] != "")
                        customerRegCode = ConfigurationManager.AppSettings["CustomerRegCode"];

                if (userName.ToUpper() != ClientConfiguration.DefaultUser && ConfigurationManager.AppSettings["EntityRegCode"] != null && ConfigurationManager.AppSettings["EntityRegCode"] != "")
                    entityRegCode = ConfigurationManager.AppSettings["EntityRegCode"];

                var param = new DALCustomers.Parameters
                {
                    USER_NAME = userName,
                    USER_PASSWORD = userPassword,
                    CUSTOMER_REG_CODE = customerRegCode,
                    ENTITY_REG_CODE = entityRegCode
                };

                var softwareCustomerInfoModel = DALCustomers.Instance.LoadCustomerInfoList(param);
                if (softwareCustomerInfoModel.Count <= 0)
                {
                    MDVSession.Current.LoginAttemptsCount++;
                    if (MDVSession.Current.AccountLockoutThreshold < 0)
                        MDVSession.Current.AccountLockoutThreshold = MDVUtility.ToInt32(DALCustomers.Instance.GetAccountLockoutThreshold(param));

                    if (MDVSession.Current.AccountLockoutThreshold <= 0 ||
                        MDVSession.Current.LoginAttemptsCount <= MDVSession.Current.AccountLockoutThreshold)
                        throw new Exception("Invalid login details.");

                    DALCustomers.Instance.LockUserAccount(param);
                    throw new Exception("This User is Locked");
                    throw new Exception("Invalid login details.");
                }
                MDVSession.Current.LoginAttemptsCount = 0;

                return softwareCustomerInfoModel;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::GetCustomerSettings", ex);
                throw ex;
            }
        }

        public BLObject<DSSoftwareCustomersInfo> GetCustomerSettingsForServices(string userName, string userPassword, string customerRegCode = "", string entityRegCode = "")
        {
            try
            {
                userName = MDVUtility.DecryptFrom64(userName);
                //  UserPassword = MDVUtility.DecryptFrom64(UserPassword);
                // UserPassword = MDVUtility.EncryptSHA256(UserPassword);

                if (ConfigurationManager.AppSettings["CustomerRegCode"] != null)
                    if (ConfigurationManager.AppSettings["CustomerRegCode"] != "")
                        customerRegCode = ConfigurationManager.AppSettings["CustomerRegCode"];

                if (userName.ToUpper() != ClientConfiguration.DefaultUser)
                {
                    if (ConfigurationManager.AppSettings["EntityRegCode"] != null)
                        if (ConfigurationManager.AppSettings["EntityRegCode"] != "")
                            entityRegCode = ConfigurationManager.AppSettings["EntityRegCode"];
                }

                DALCustomers.Parameters param = new DALCustomers.Parameters
                {
                    USER_NAME = userName,
                    USER_PASSWORD = userPassword,
                    CUSTOMER_REG_CODE = customerRegCode,
                    ENTITY_REG_CODE = entityRegCode
                };

                DSSoftwareCustomersInfo ds = DALCustomers.Instance.LoadCustomerInfo(param);
                if (ds.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows.Count <= 0)
                {
                    throw new Exception("Invalid login details.");
                }
                return new BLObject<DSSoftwareCustomersInfo>(ds);
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAdminSecurity::GetCustomerSettingsForServices", ex);
                return new BLObject<DSSoftwareCustomersInfo>(null, ex.Message);

            }
        }


        #region NATIVE FUNCTIONS
        public Client GetClientInformationNative(string id)
        {
            try
            {
                string customerRegCode = "";
                if (ConfigurationManager.AppSettings["CustomerRegCode"] != null && ConfigurationManager.AppSettings["CustomerRegCode"] != "")
                    customerRegCode = ConfigurationManager.AppSettings["CustomerRegCode"];
                var client = DALCustomers.Instance.GetClientInformationNative(id, customerRegCode);
                return client;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::GetClientInformationNative", ex);
                throw ex;
            }
        }
        public string GetIsFullSSNNative(long userId, string userName, string entityId, string isActive,long SecurityUserId)
        {
            try
            {
                string IsFullSSN = "";
                string customerRegCode = "";
                if (ConfigurationManager.AppSettings["CustomerRegCode"] != null && ConfigurationManager.AppSettings["CustomerRegCode"] != "")
                    customerRegCode = ConfigurationManager.AppSettings["CustomerRegCode"];
                userName = MDVUtility.DecryptFrom64(userName);
                 IsFullSSN =  DALCustomers.Instance.GetIsFullSSN(userId, userName, entityId, isActive,SecurityUserId);
   
                return IsFullSSN;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::GetClientInformationNative", ex);
                throw ex;
            }
        }
        public List<SoftwareCustomerInfoModel> GetCustomerSettingsNative(string userName, string userPassword, string customerRegCode = "")
        {
            try
            {

                if (ConfigurationManager.AppSettings["CustomerRegCode"] != null && ConfigurationManager.AppSettings["CustomerRegCode"] != "")
                {
                    customerRegCode = ConfigurationManager.AppSettings["CustomerRegCode"];
                }
                DALCustomers.Parameters param = new DALCustomers.Parameters
                {
                    USER_NAME = userName,
                    USER_PASSWORD = userPassword,
                    CUSTOMER_REG_CODE = customerRegCode
                };
                return DALCustomers.Instance.LoadCustomerInfoNative(param);       
                
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAdminSecurity::GetCustomerSettingsNative", ex);
                throw ex;

            }
        }

        public List<SoftwareCustomerInfoModel> GetCustomerSettingsNativeForCheckInApp(string userName, string userPassword, string customerRegCode = "")
        {
            try
            {
                List<SoftwareCustomerInfoModel> TempModel = new List<SoftwareCustomerInfoModel>();
                List<SoftwareCustomerInfoModel> ListofModel = new List<SoftwareCustomerInfoModel>();
                SoftwareCustomerInfoModelwithEintiy model = new SoftwareCustomerInfoModelwithEintiy();

                if (ConfigurationManager.AppSettings["CustomerRegCode"] != null && ConfigurationManager.AppSettings["CustomerRegCode"] != "")
                {
                    customerRegCode = ConfigurationManager.AppSettings["CustomerRegCode"];
                }
                DALCustomers.Parameters param = new DALCustomers.Parameters
                {
                    USER_NAME = userName,
                    USER_PASSWORD = userPassword,
                    CUSTOMER_REG_CODE = customerRegCode
                };
                model= DALCustomers.Instance.LoadCustomerInfoNativeForCheckInApp(param);
                TempModel = model.SoftwareCustomerInfoModelList;
                 for(int i=0;i< TempModel.Count;i++)
                {
                    Entity MdelObj = new Entity();
                    MdelObj.Facilities = new List<FacilityList>();
                    List<FacilityList> MdelFacObj = new List<FacilityList>();
                    //TempModel[i].EntityList =  model.CustomerRegCodeList.Where(m => m.EntityId == TempModel[i].EntityId).ToList();
                    MdelObj.EntityId = TempModel[i].EntityId;
                    MdelObj.EntityName = TempModel[i].EntityRegCode;
                    //MdelFacObj=TempModel[i].EntityList.Select(x=> new FacilityList { FacilityId=x.FacilityId, FacilityName = x.FacilityName, }).ToList();
                   // MdelObj.Facilities = ;
                    if (TempModel[i].Facilities == null)
                        TempModel[i].Facilities = new List<FacilityList>();
                    TempModel[i].Facilities = model.CustomerRegCodeList.Where(m => m.EntityId == TempModel[i].EntityId).Select(x => new FacilityList { FacilityId = x.FacilityId, FacilityName = x.FacilityName, }).OrderBy(t=>t.FacilityName).ToList();
                    ListofModel.Add(TempModel[i]);
                }
                 
                return ListofModel;
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAdminSecurity::GetCustomerSettingsNativeForCheckInApp", ex);
                throw ex;

            }
        }
        #endregion

        #endregion
    }
}
