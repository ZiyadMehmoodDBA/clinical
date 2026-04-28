using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;

using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.IEHR.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MDVision.Model.Security;

namespace MDVision.IEHR.Controls.CommonControls
{
    public class User_ReLogin
    {
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        private BLLCommon BLLCommonObj = null;

        public User_ReLogin()
        {
            BLLAdminSecurityObj = new BLLAdminSecurity();
            BLLCommonObj = new BLLCommon();
        }

        #region Singleton
        private static User_ReLogin _obj = null;
        public static User_ReLogin Instance()
        {
            return _obj ?? (_obj = new User_ReLogin());
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// when session timeout, User Try To Re Login
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string User_ReLoginFunc(string userInfo, string entity)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(userInfo);
                var responseMessage = ""; var statusMessage = false;
                string username = searchedfieldsJson["hdnUserName"].ToUpper().Trim();
                string password = searchedfieldsJson["txtpwd"].Trim();

                if (username.Length <= 0 || username == "ENTER USER NAME")
                    responseMessage = "Please enter Username.";
                else if (password.Length <= 0 || password.ToUpper() == "PASSWORD")
                    responseMessage = "Please enter Password.";
                else
                {
                    // AppConfig.ClearAppData();

                    MDVLogger.PresentationLog("User_ReLoginFunc.cs", "User_ReLoginFunc", "Line#51" + " username:" + username + " password:" + password, "Ali Awan", "none");
                    //var obj = BLLCommonObj.GetCustomerSettings(MDVUtility.EncryptTo64(username), MDVUtility.EncryptToSHA256(password, username));
                    var listSoftwareCustomersInfo = new BLLCommon().GetCustomerSettingsList(MDVUtility.EncryptTo64(username), MDVUtility.EncryptToSHA256(password, username));

                    if (listSoftwareCustomersInfo != null)
                    {
                        // DataSet ds;
                        //Session["dsCustomerInfo"] = MDVUtility.EncryptDataSet((DataSet)obj.Data);
                        //MDVSession.Current.dsCustomerInfo = obj.Data;
                        MDVSession.Current.listCustomerInfo = listSoftwareCustomersInfo;
                        MDVSession.Current.AppUserName = MDVUtility.EncryptTo64(username);
                        MDVSession.Current.AppPassWord = MDVUtility.EncryptToSHA256(password, username);

                        if (username == AppPrivileges.DefaultUser) //|| Convert.ToBoolean(obj.Data.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows[0][DSSoftwareCustomersInfo.FIELD_IS_ADMIN]) == true)
                        {
                            //var list = MDVSession.Current.listCustomerInfo.Where(s => s.EntityId == MDVSession.Current.EntityId.ToString());
                            responseMessage = DefaultEntityLogin_(MDVSession.Current.listCustomerInfo, ref responseMessage);
                            if (string.IsNullOrEmpty(responseMessage))
                            {
                                responseMessage = "Logged in Successfully!";
                                statusMessage = true;
                            }


                        }
                        if (listSoftwareCustomersInfo.Count > 1)
                        {
                            responseMessage = "Logged in Successfully!";
                            statusMessage = true;
                        }
                        else
                        {
                            responseMessage = "Invalid Credentials." + " \r\n" + "Please contact your system Administrator.";
                        }
                        if (string.IsNullOrEmpty(entity))
                        {
                            //entity = MDVSession.Current.dsCustomerInfo.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows[0][DSSoftwareCustomersInfo.FIELD_ENTITY_ID].ToString();
                            entity = listSoftwareCustomersInfo[0].EntityId;
                        }
                        MDVSession.Current.EntityId = entity;
                        //DataRow dr = MDVSession.Current.dsCustomerInfo.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Select(DSSoftwareCustomersInfo.FIELD_ENTITY_ID + "=" + MDVSession.Current.EntityId)[0];
                        var list = MDVSession.Current.listCustomerInfo.Where(s => s.EntityId == MDVSession.Current.EntityId.ToString());

                        if (list.Any())
                        {
                            //DSSoftwareCustomersInfo ds = new DSSoftwareCustomersInfo();
                            //ds.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].ImportRow(dr);
                            new UserLoginHelper().SetCustomerConfig(MDVSession.Current.listCustomerInfo);
                            var objLogin = new BLLAdminSecurity().Login_(MDVSession.Current.listCustomerInfo, "", MDVSession.Current.EntityRegCode, "User_ReLoginFunc");
                            if (objLogin.Data != null)
                            {
                                new RcopiaHelper().setRcopiaNotificationCount(objLogin.Data);
                                new UserLoginHelper().SetApplicationConfig_(objLogin.Data);
                                responseMessage = "Logged in Successfully!";
                                statusMessage = true;
                            }
                            else if (objLogin.Message != "")
                            {
                                // Response.Redirect(AppConfig.WebEntityURL + "MDVisionLogin.aspx?error=" + error);
                                responseMessage = objLogin.Message;
                            }
                        }
                        else
                        {
                            //MK
                            //responseMessage = obj.Message + " \r\n" + "Please contact your system Administrator.";
                            responseMessage = "Please contact your system Administrator.";
                        }
                    }
                    else
                    {
                        responseMessage = "Invalid Credentials." + " \r\n" + "Please contact your system Administrator.";
                    }
                }
                var response = new
                {
                    status = statusMessage,
                    Message = responseMessage,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        //private string DefaultEntityLogin(DataRow drCustomer, ref string error)
        //{
        //    new UserLoginHelper().SetCustomerConfig(drCustomer);

        //    if (MDVSession.Current.EntityId == null || Convert.ToInt64(MDVSession.Current.EntityId) == 0)
        //    {
        //        error = "Selected entity " + MDVSession.Current.EntityRegCode + " is not configure." + " \r\n" + "Please contact your Administrator.";
        //        return error.Replace("\r\n", "<br/>");

        //    }

        //    if (MDVSession.Current.WebEntityURL != "")
        //    {
        //        return "";
        //    }
        //    else
        //    {
        //        error = "Client URL is invalid." + "\r\n" + "Please contact your system Administrator.";
        //        return error; // lblProfileLoginError.Text = error.Replace("\r\n", "<br/>");
        //    }

        //}

        private string DefaultEntityLogin_(List<SoftwareCustomerInfoModel_> listCustomer, ref string error)
        {
            new UserLoginHelper().SetCustomerConfig(listCustomer);
            if (MDVSession.Current.EntityId == null || Convert.ToInt64(MDVSession.Current.EntityId) == 0)
            {
                error = "Selected entity " + MDVSession.Current.EntityRegCode + " is not configure." + " \r\n" + "Please contact your Administrator.".Replace("\r\n", "<br/>");
                return error;
            }

            if (MDVSession.Current.WebEntityURL != "")
                return "";
            else
            {
                error = "Client URL is invalid." + "\r\n" + "Please contact your system Administrator.";
                return error;
            }
        }

        /// <summary>
        /// this function is used to User session reset and stayed user as logged in
        /// </summary>
        /// <returns></returns>
        private string StayedUser_ReLoginFunc()
        {
            try
            {
                var responseMessage = ""; bool statusMessage = false;
                // AppConfig.ClearAppData();
                //BLObject<DSSoftwareCustomersInfo> obj = BLLCommonObj.GetCustomerSettings(MDVSession.Current.AppUserName, MDVSession.Current.AppPassWord);
                var listSoftwareCustomersInfo = new BLLCommon().GetCustomerSettingsList(MDVSession.Current.AppUserName, MDVSession.Current.AppPassWord);
                if (listSoftwareCustomersInfo != null)
                {
                    // DataSet ds;
                    //Session["dsCustomerInfo"] = MDVUtility.EncryptDataSet((DataSet)obj.Data);
                    //MDVSession.Current.dsCustomerInfo = obj.Data;
                    MDVSession.Current.listCustomerInfo = listSoftwareCustomersInfo;
                    if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == AppPrivileges.DefaultUser) //|| Convert.ToBoolean(obj.Data.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows[0][DSSoftwareCustomersInfo.FIELD_IS_ADMIN]) == true)
                    {
                        responseMessage = DefaultEntityLogin_(MDVSession.Current.listCustomerInfo, ref responseMessage);
                        if (string.IsNullOrEmpty(responseMessage))
                        {
                            responseMessage = "Logged in Successfully!";
                            statusMessage = true;
                        }
                    }
                    if (listSoftwareCustomersInfo.Count > 1)
                    {
                        responseMessage = "Logged in Successfully!";
                        statusMessage = true;
                    }
                    else
                    {
                        responseMessage = "Invalid Credentials." + " \r\n" + "Please contact your system Administrator.";
                    }
                    if (string.IsNullOrEmpty(MDVSession.Current.EntityId))
                    {
                        MDVSession.Current.EntityId = listSoftwareCustomersInfo[0].EntityId;//MDVSession.Current.dsCustomerInfo.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows[0][DSSoftwareCustomersInfo.FIELD_ENTITY_ID].ToString();
                    }

                    //DataRow dr = MDVSession.Current.dsCustomerInfo.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Select(DSSoftwareCustomersInfo.FIELD_ENTITY_ID + "=" + MDVSession.Current.EntityId)[0];
                    var list = MDVSession.Current.listCustomerInfo.Where(s => s.EntityId == MDVSession.Current.EntityId.ToString());

                    if (list.Any())
                    {
                        //DSSoftwareCustomersInfo ds = new DSSoftwareCustomersInfo();
                        //ds.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].ImportRow(dr);
                        //new UserLoginHelper().SetCustomerConfig(dr);
                        //BLObject<DSUsers> objLogin = BLLAdminSecurityObj.Login(ref ds, "", MDVSession.Current.EntityRegCode, "StayedUser_ReLoginFunc");

                        //new UserLoginHelper().SetCustomerConfig(list.ToList());
                        var objLogin = new BLLAdminSecurity().Login_(MDVSession.Current.listCustomerInfo, "", MDVSession.Current.EntityRegCode, "User_ReLoginFunc");

                        if (objLogin.Data != null)
                        {
                            //RcopiaHelper rcopiaHelper = new RcopiaHelper();
                            //rcopiaHelper.setRcopiaNotifucationCount(ref objLogin);
                            //new UserLoginHelper().SetApplicationConfig(objLogin.Data);

                            new RcopiaHelper().setRcopiaNotificationCount(objLogin.Data);
                            new UserLoginHelper().SetApplicationConfig_(objLogin.Data);

                            responseMessage = "Logged in Successfully!";
                            statusMessage = true;
                        }
                        else if (objLogin.Message != "")
                        {
                            // Response.Redirect(AppConfig.WebEntityURL + "MDVisionLogin.aspx?error=" + error);
                            responseMessage = objLogin.Message;
                        }
                    }
                    else
                    {
                        // MK
                        //responseMessage = obj.Message + " \r\n" + "Please contact your system Administrator.";
                        responseMessage = "Please contact your system Administrator.";
                    }
                }
                else
                {
                    responseMessage = "Invalid Credentials." + " \r\n" + "Please contact your system Administrator.";
                }
                if (statusMessage)
                {
                    HttpContext.Current.Session.Timeout = MDVUtility.ToInt32(string.IsNullOrEmpty(MDVSession.Current.SessionTimout) ? "30" : MDVSession.Current.SessionTimout);
                }
                var response = new
                {
                    status = statusMessage,
                    Message = responseMessage,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Basic Free Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "USER_SESSION_UNLOCK":
                    {
                        string userInfo = MDVUtility.ToStr(context.Request["UserInfo"]);
                        string entity = MDVUtility.ToStr(context.Request["Entity"]);

                        string strJsonData = User_ReLoginFunc(userInfo, entity);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "USER_SESSION_RESET_RELOGIN":
                    {
                        string strJsonData = StayedUser_ReLoginFunc();
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;


            }
        }
        #endregion
    }
}
