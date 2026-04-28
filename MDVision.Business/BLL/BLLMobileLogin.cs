using System;

using MDVision.DataAccess.DAL.Admin;

using MDVision.Common.Logging;
using System.ComponentModel;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using System.Configuration;
using System.Data;
using MDVision.DataAccess.DCommon;
using System.Collections.Generic;
using System.Text;

namespace MDVision.Business.BLL
{
 public  class BLLMobileLogin
    {
        public BLLMobileLogin()
        {
            //SharedVariable
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
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

        private static SharedVariable SetSharedObject(DataRow drSoftwareCustomerInfo, string UserName, string Password)
        {
            SharedVariable sharedObj = new SharedVariable
            {
                ClientId = drSoftwareCustomerInfo[DSSoftwareCustomersInfo.FIELD_UNIQUE_CLIENT_ID].ToString(),
                EntityId = drSoftwareCustomerInfo[DSSoftwareCustomersInfo.FIELD_ENTITY_ID].ToString(),
                WebEntityURL = drSoftwareCustomerInfo[DSSoftwareCustomersInfo.FIELD_WEB_SERVICE_URL].ToString(),
                UserName = MDVUtility.EncryptTo64(UserName),
                AppPassWord = MDVUtility.EncryptTo64(Password),
                AppUserId = Convert.ToInt64(drSoftwareCustomerInfo[DSSoftwareCustomersInfo.FIELD_USER_ID].ToString()),

            };
            return sharedObj;
        }

        DALLoginMobile DALLogin;
        public string InsertUpdateMobileLogin(string DeviceId, DateTimeOffset ExpirationTime, string GrantType)
        {
            

        
            string returnVal = "";
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
         //   dbManager.Open();
            try

            {
               

                returnVal = new DALLoginMobile().InsertUpdateMobileLogin( dbManager, DeviceId, ExpirationTime, GrantType);
                return returnVal;
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientPicNative", ex);
                return ex.Message;
            }

        }
        public string LogOutMobileLogin(string DeviceId)
        {
            //string UserName = "mdvision";
            //string Password = "Password1!";

            //BLObject<DSSoftwareCustomersInfo> obj = new BLLCommon().GetCustomerSettingsForServices(MDVUtility.EncryptTo64(UserName), MDVUtility.EncryptToSHA256(Password, UserName));
            //DSSoftwareCustomersInfo dsSoftwareCustomerInfo = obj.Data;

            //SharedVariable SharedVariable1 = null;
            //foreach (DataRow drSoftwareCustomerInfo in dsSoftwareCustomerInfo.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows)
            //{
            //    SharedVariable1 = SetSharedObject(drSoftwareCustomerInfo, UserName, Password);
            //}

            string returnVal = "";
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
          
            try
            {
                returnVal = new DALLoginMobile().LogOutMobileLogin(dbManager,DeviceId);
                return returnVal;
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientPicNative", ex);
                return ex.Message;
            }

        }
        public DataSet LoadDeviceIds()
        {
            //string UserName = "mdvision";
            //string Password = "Password1!";

            //BLObject<DSSoftwareCustomersInfo> obj = new BLLCommon().GetCustomerSettingsForServices(MDVUtility.EncryptTo64(UserName), MDVUtility.EncryptToSHA256(Password, UserName));
            //DSSoftwareCustomersInfo dsSoftwareCustomerInfo = obj.Data;

            //SharedVariable SharedVariable1 = null;
            //foreach (DataRow drSoftwareCustomerInfo in dsSoftwareCustomerInfo.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows)
            //{
            //    SharedVariable1 = SetSharedObject(drSoftwareCustomerInfo, UserName, Password);
            //}
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            DataSet ds=new DataSet();
            try
            {
                ds = new DALLoginMobile().LoadDeviceIds(dbManager);
                return ds;
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientPicNative", ex);
              throw ex ;
            }

        }
        public BLObject<DSInsuranceLookup> LookupDataChangeRequest(string Active)
        {
            try
            {
                DSInsuranceLookup ds = new DSInsuranceLookup();
                ds = new DALInsurancePlan().LookupInsurancePlan(Active);
                return new BLObject<DSInsuranceLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LookupInsurancePlan", ex);
                return new BLObject<DSInsuranceLookup>(null, ex.Message);
            }
        }

        public List<Model.User.EntityUserOptions> LoadEntityUserOptionForService(string userName, string entityId, string entityRegCode = null)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            try
            {
                byte[] data = Convert.FromBase64String(userName);
                string decodedUserName = Encoding.UTF8.GetString(data);

                var res = new DALLoginMobile().LoadEntityUserOptionForService(dbManager, decodedUserName, entityId, entityRegCode);
                return new List<Model.User.EntityUserOptions>(res);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadEntityUserOptionForService", ex);
                return new List<Model.User.EntityUserOptions>(0);

            }
        }

    }
}
