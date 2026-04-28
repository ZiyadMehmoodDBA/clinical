using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.Common.Utilities;

namespace MDVision.IEHR.Common
{
    public class UserEntityList
    {
        private static List<UserEntity> ObjUserEntityList = new List<UserEntity>();

        #region Variables
        private static UserEntityList instance = null;

        #endregion

        public static UserEntityList Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserEntityList();

                }
                return instance;
            }
        }

        public  string UserEntityAdd(DataRow drCustomerUser, string SessionID, DSUsers objUser)
        {
            try
            {
                string AppUserId_EntityId_SessionID = drCustomerUser[DSSoftwareCustomersInfo.FIELD_USER_ID].ToString() + "-" + drCustomerUser[DSSoftwareCustomersInfo.FIELD_ENTITY_ID].ToString() + "-" + SessionID;


                if (ObjUserEntityList.Exists(x => x.AppUserId_EntityId_SessionID == AppUserId_EntityId_SessionID) == false)
                {
                    UserEntity userEntity = new UserEntity();
                    SetUserEntity(drCustomerUser, userEntity, SessionID, objUser);
                    ObjUserEntityList.Add(userEntity);
                }
                else
                {
                    UserEntity results = ObjUserEntityList.Find(x => x.AppUserId_EntityId_SessionID == AppUserId_EntityId_SessionID);
                    SetUserEntity(drCustomerUser, results, SessionID, objUser);


                }
               
            }
            catch (Exception ex)
            {
               return ex.Message ;
            }

            return "";
        }

        private  static void SetUserEntity(DataRow drCustomerUser, UserEntity userEntity, string SessionID, DSUsers objUser)
        {

           
               // UserEntity userEntity = new UserEntity();
                if (drCustomerUser != null)
                {
                    
                    userEntity.CustomerRegCode = drCustomerUser[DSSoftwareCustomersInfo.FIELD_CUSTOMER_REG_CODE].ToString();
                    userEntity.WebEntityURL = drCustomerUser[DSSoftwareCustomersInfo.FIELD_WEB_SERVICE_URL].ToString();
                    userEntity.EntityId = drCustomerUser[DSSoftwareCustomersInfo.FIELD_ENTITY_ID].ToString();
                    userEntity.EntityRegCode = drCustomerUser[DSSoftwareCustomersInfo.FIELD_ENTITY_REG_CODE].ToString();
                 

                    userEntity.DBUserId = MDVUtility.EncryptTo64(drCustomerUser[DSSoftwareCustomersInfo.FIELD_DB_USER_ID].ToString());
                    userEntity.DBPassWord = MDVUtility.EncryptTo64(drCustomerUser[DSSoftwareCustomersInfo.FIELD_DB_PASSWORD].ToString());
                    userEntity.DataSource = drCustomerUser[DSSoftwareCustomersInfo.FIELD_DATA_SOURCE].ToString();
                    userEntity.InitialCatalog = drCustomerUser[DSSoftwareCustomersInfo.FIELD_DB_NAME].ToString();
                    if (drCustomerUser[DSSoftwareCustomersInfo.FIELD_PERSIST_SECURITY_INFO].ToString() != "")
                        userEntity.PersistSecurityInfo = Convert.ToBoolean(drCustomerUser[DSSoftwareCustomersInfo.FIELD_PERSIST_SECURITY_INFO].ToString());
                    else
                        userEntity.PersistSecurityInfo = false;

                    if (drCustomerUser[DSSoftwareCustomersInfo.FIELD_IS_PROXY].ToString() != "")
                        userEntity.IsProxy = Convert.ToBoolean(drCustomerUser[DSSoftwareCustomersInfo.FIELD_IS_PROXY].ToString());
                    else
                        userEntity.IsProxy = false;


                    userEntity.PoolingString = drCustomerUser[DSSoftwareCustomersInfo.FIELD_POOLING_STRING].ToString();
                    userEntity.DBProviderType = drCustomerUser[DSSoftwareCustomersInfo.FIELD_PROVIDER_TYPE].ToString();
                    userEntity.ClientId = drCustomerUser[DSSoftwareCustomersInfo.FIELD_UNIQUE_CLIENT_ID].ToString();
                    userEntity.AppUserId = MDVUtility.ToLong(drCustomerUser[DSSoftwareCustomersInfo.FIELD_USER_ID]);

                    userEntity.AppUserId_EntityId_SessionID = userEntity.AppUserId.ToString() + "-" + userEntity.EntityId + "-" + SessionID;
                }

                if (objUser != null)
                {
                    userEntity.dtEntityUserOption = objUser.EntityUserOption;
                    userEntity.dtUserPrivileges = objUser.UsersPrivileges;
                    userEntity.dtUser = objUser.Users;
                    if (userEntity.dtUser != null)
                        if (userEntity.dtUser.Rows.Count > 0)
                        {
                            userEntity.IsAdmin = Convert.ToBoolean(userEntity.dtUser.Rows[0][userEntity.dtUser.IsAdminColumn.ColumnName]);
                            userEntity.AppUserName = MDVUtility.EncryptTo64(userEntity.dtUser.Rows[0][userEntity.dtUser.UserNameColumn.ColumnName].ToString());

                            userEntity.AppPassWord = MDVUtility.EncryptToSHA256(userEntity.dtUser.Rows[0][userEntity.dtUser.UserPasswordColumn.ColumnName].ToString(), userEntity.dtUser.Rows[0][userEntity.dtUser.UserNameColumn.ColumnName].ToString());
                        }
                    userEntity.UserLoggedIn = true;
                }
        }


        public  string FindUserEntity(string AppUserId_EntityId_SessionID, string PropertyName)
        {

            UserEntity results = ObjUserEntityList.Find(x => x.AppUserId_EntityId_SessionID == AppUserId_EntityId_SessionID);

            if ("AppUserName" == PropertyName)
            {
                return results.AppUserName;
            }
            else if ("AppPassWord" == PropertyName)
            {
                return results.AppPassWord;
            }
            else if ("EntityId" == PropertyName)
            {
                return results.EntityId;
            }
            else if ("EntityRegCode" == PropertyName)
            {
                return results.EntityRegCode;
            }
            else if ("ClientId" == PropertyName)
            {
                return results.ClientId;
            }

            else if ("WebEntityURL" == PropertyName)
            {
                return results.WebEntityURL;
            }
            else if ("CustomerRegCode" == PropertyName)
            {
                return results.CustomerRegCode;
            }
            else if ("DBUserId" == PropertyName)
            {
                return results.DBUserId;
            }
            else if ("DBPassWord" == PropertyName)
            {
                return results.DBPassWord;

            }
            else if ("DataSource" == PropertyName)
            {
                return results.DataSource;

            }
            else if ("DBPassWord" == PropertyName)
            {
                return results.DBPassWord;
            }

            else if ("InitialCatalog" == PropertyName)
            {
                return results.InitialCatalog;
            }

            else if ("PoolingString" == PropertyName)
            {
                return results.PoolingString;

            }
            else if ("DBProviderType" == PropertyName)
            {
                return results.DBProviderType;

            }


            return "";






        }
        public  long FindLongUserEntity(string AppUserId_EntityId_SessionID, string PropertyName)
        {

            UserEntity results = ObjUserEntityList.Find(x => x.AppUserId_EntityId_SessionID == AppUserId_EntityId_SessionID);

            if ("AppUserId" == PropertyName)
            {
                return results.AppUserId;

            }

            return 0;



        }

        public  Boolean FindBoolUserEntity(string AppUserId_EntityId_SessionID, string PropertyName)
        {

            UserEntity results = ObjUserEntityList.Find(x => x.AppUserId_EntityId_SessionID == AppUserId_EntityId_SessionID);


            if ("IsAdmin" == PropertyName)
            {
                return results.IsAdmin;
            }

            else if ("PersistSecurityInfo" == PropertyName)
            {
                return results.PersistSecurityInfo;
            }
            else if ("IsProxy" == PropertyName)
            {
                return results.IsProxy;
            }

            else if ("UserLoggedIn" == PropertyName)
            {
                return results.UserLoggedIn;

            }


            return false;






        }

        public  DSUsers.UsersPrivilegesDataTable FindDtUserPrivileges(string AppUserId_EntityId_SessionID, string PropertyName)
        {
            UserEntity results = ObjUserEntityList.Find(x => x.AppUserId_EntityId_SessionID == AppUserId_EntityId_SessionID);


            if ("dtUserPrivileges" == PropertyName)
            {
                return results.dtUserPrivileges;

            }

            return null;
        }

        public  DSUsers.UsersDataTable FindDtUser(string AppUserId_EntityId_SessionID, string PropertyName)
        {
            UserEntity results = ObjUserEntityList.Find(x => x.AppUserId_EntityId_SessionID == AppUserId_EntityId_SessionID);

            if ("dtUser" == PropertyName)
            {
                return results.dtUser;

            }
            return null;
        }

        public DSUsers.EntityUserOptionDataTable FindDtEntityUserOption(string AppUserId_EntityId_SessionID, string PropertyName)
        {
            UserEntity results = ObjUserEntityList.Find(x => x.AppUserId_EntityId_SessionID == AppUserId_EntityId_SessionID);

            if ("dtEntityUserOption" == PropertyName)
            {
                return results.dtEntityUserOption;

            }
            return null;
        }
        private class UserEntity
        {
            #region Data Property


            public string AppUserId_EntityId_SessionID { get; set; }

            public long AppUserId { get; set; }



            public string AppUserName { get; set; }

            public string AppPassWord { get; set; }

            public string EntityId { get; set; }

            public string EntityRegCode { get; set; }



            public string ClientId { get; set; }


            public Boolean IsAdmin { get; set; }


            public string WebEntityURL { get; set; }



            public string CustomerRegCode { get; set; }


            public string DBUserId { get; set; }



            public string DBPassWord { get; set; }

            public string DataSource { get; set; }


            public string InitialCatalog { get; set; }

            public bool PersistSecurityInfo { get; set; }

            public bool IsProxy { get; set; }


            public string PoolingString { get; set; }


            public string DBProviderType { get; set; }


            public bool UserLoggedIn { get; set; }


            //public DSSoftwareCustomersInfo dsCustomerInfo { get; set; }

            public DSUsers.EntityUserOptionDataTable dtEntityUserOption { get; set; }


            public DSUsers.UsersPrivilegesDataTable dtUserPrivileges { get; set; }

            public DSUsers.UsersDataTable dtUser { get; set; }


            #endregion
        }
    }

    
   
}
