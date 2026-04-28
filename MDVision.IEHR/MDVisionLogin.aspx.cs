using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.Security;
using MDVision.Model.Security;
using MDVision.DataAccess.DCommon; 

namespace MDVision.IEHR.Account
{
    public partial class MDVisionLogin : Page
    {
        string _username;
        string _password;
        private bool _isPasswordExpired;
        private static int _userId = 0;
        public bool IsEmrRequire = true;
        public string ErrorMessage { get; set; }
        //string selectedText = "";
        //string selectedValue = "";
        //BLObject<DSSoftwareCustomersInfo> obj;
        //string Password = "";
        //String Entity = "1000";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["OpCode"] == System.Web.Configuration.WebConfigurationManager.AppSettings["OpCode"])
                System.Web.Optimization.BundleTable.EnableOptimizations = false;
            else
                System.Web.Optimization.BundleTable.EnableOptimizations = true;


            if (IsPostBack == false)
            {
                var error = "";
                if (Request.QueryString["error"] != null)
                    error = Request.QueryString["error"];

                if (error != "")
                {
                    //AdminSecurity.BusinessObj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.LogIn, DALUsersActivity.TITLE_SOFTWARE_NAME, false, "Error during Login user: " + Error);
                    lblErrMsg.Text = error;   //+ " \r\n" + "Please contact your system Administrator.";
                }
                else
                {
                    if (!MDVSession.Current.UserLoggedIn) return;
                    //if (AppConfig.isTestDatabase)
                    //{
                    Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionDefault.aspx", false);
                    //}
                    //else
                    //{
                    //    JsFuncs.RunJS(this.Page, "openNewWindow('" + AppConfig.WebEntityURL + "MDVisionDefault.aspx" + "');");
                    //}
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Login_Click(object sender, EventArgs e)
        {
            try
            {
                //lblErrMsg.Text = "";
                string error = "";
                _username = txtLoginUserName.Text.ToUpper().Trim();
                _password = txtLoginPassword.Text.Trim();
                if (_username.Length <= 0 || _username == "ENTER USER NAME")
                    error = "Please enter Username.";
                else if (_password.Length <= 0 || _password.ToUpper() == "PASSWORD")
                    error = "Please enter Password.";
                else
                {
                    ViewState.Remove("UserId");
                    MDVSession.Current.ClearAppData();
                    //BLObject<DSSoftwareCustomersInfo> obj = new BLLCommon().GetCustomerSettings(MDVUtility.EncryptTo64(username), MDVUtility.EncryptToSHA256(password, username));
                    var listSoftwareCustomersInfo = new BLLCommon().GetCustomerSettingsList(MDVUtility.EncryptTo64(_username), MDVUtility.EncryptToSHA256(_password, _username));

                    if (listSoftwareCustomersInfo.Count > 0)
                    {
                        _isPasswordExpired = Convert.ToBoolean(listSoftwareCustomersInfo[0].IsPasswordExpired != "0");
                        ViewState["UserId"] = Convert.ToInt32(listSoftwareCustomersInfo[0].UserId);
                        var isFirstTimeLoggedIn = Convert.ToBoolean(listSoftwareCustomersInfo[0].isFirstTimeLoggedIn);
                        if (!isFirstTimeLoggedIn)
                        {
                            ShowChangePasswordOnFirstLogin(true);
                            return;
                        }
                        if (_isPasswordExpired)
                        {
                            _isPasswordExpired = false;
                            ShowChangePassword(true);
                            return;
                        }                       

                        // DataSet ds;
                        //Session["dsCustomerInfo"] = MDVUtility.EncryptDataSet((DataSet)obj.Data);
                        MDVSession.Current.DefaultEntity = MDVUtility.ToInt32(listSoftwareCustomersInfo[0].DefaultEntity);
                        MDVSession.Current.PasswordRegex = _username == AppPrivileges.DefaultUser ? ".{1,}" : listSoftwareCustomersInfo[0].PasswordRegex;
                        //MDVSession.Current.dsCustomerInfo = obj.Data;
                        MDVSession.Current.listCustomerInfo = listSoftwareCustomersInfo;
                        MDVSession.Current.AppUserName = MDVUtility.EncryptTo64(_username);
                        MDVSession.Current.AppPassWord = MDVUtility.EncryptToSHA256(_password, _username);// MDVUtility.EncryptTo64(password);

                        if (listSoftwareCustomersInfo.Count > 1)
                        {
                            tdlogin.Visible = false;
                            tbEntity.Visible = true;
                            lblFormTitle.Text = "Select Entity";

                            //Onload functionality of Entities Page
                            blistEntity.DataSource = listSoftwareCustomersInfo;
                            blistEntity.DataValueField = DSSoftwareCustomersInfo.FIELD_ENTITY_ID;
                            blistEntity.DataTextField = DSSoftwareCustomersInfo.FIELD_ENTITY_REG_CODE;
                            blistEntity.DataBind();
                            if (_username == AppPrivileges.DefaultUser) //|| Convert.ToBoolean(obj.Data.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows[0][DSSoftwareCustomersInfo.FIELD_IS_ADMIN]) == true)
                            {

                                DefaultEntityLogin_(listSoftwareCustomersInfo, ref error);
                                //DefaultEntityLogin(obj.Data.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows[0], ref error);
                            }
                        }

                        else if (listSoftwareCustomersInfo.Count == 1)
                        {
                            MDVSession.Current.EntityId = listSoftwareCustomersInfo.FirstOrDefault().EntityId;
                            MDVSession.Current.EntityRegCode = listSoftwareCustomersInfo.FirstOrDefault().EntityRegCode;
                            // check the user login
                            //DefaultEntityLogin(obj.Data.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows[0], ref error);
                            DefaultEntityLogin_(listSoftwareCustomersInfo, ref error);
                        }
                        else
                        {
                            error = "Invalid Credentials." + " \r\n" + "Please contact your system Administrator.";
                            //lblErrMsg.Text = error.Replace("\r\n", "<br/>");
                        }
                    }
                    else
                    {
                        error = listSoftwareCustomersInfo + " \r\n" + "Please contact your system Administrator.";
                        // lblErrMsg.Text = error.Replace("\r\n", "<br/>");
                    }
                }

                lblProfileLoginError.Text = error.Replace("\r\n", "<br/>");
                lblErrMsg.Text = error.Replace("\r\n", "<br/>");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid login details." || ex.Message == "This User is Locked" || ex.Message == "No Provider or Facility is assigned. Please contact administrator.")
                {
                    var message = ex.Message;
                    if (ex.Message == "No Provider or Facility is assigned. Please contact administrator.")
                    {
                        message = "No Provider or Facility is assigned.\n Please contact administrator.";
                    }
                    var error = message;
                    lblProfileLoginError.Text = error;
                    lblErrMsg.Text = error;
                }
                // ignored
            }
        }

        /// <summary>
        /// Defaults the entity login.
        /// </summary>
        /// <param name="listCustomer"></param>
        /// <param name="error">The error.</param>
        //private void DefaultEntityLogin(DataRow drCustomer, ref string error)
        //{
        //    new UserLoginHelper().SetCustomerConfig(drCustomer);
        //    if (MDVSession.Current.EntityId == null || Convert.ToInt64(MDVSession.Current.EntityId) == 0)
        //    {
        //        error = "Selected entity " + MDVSession.Current.EntityRegCode + " is not configure." + " \r\n" + "Please contact your Administrator.";
        //        lblProfileLoginError.Text = error.Replace("\r\n", "<br/>");
        //        return;
        //    }

        //    if (MDVSession.Current.WebEntityURL != "")
        //    {
        //        tdlogin.Visible = true;
        //        tbEntity.Visible = false;
        //        //  if (AppConfig.isTestDatabase)
        //        Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionDefault.aspx", false);
        //        //  else
        //        //     JsFuncs.RunJS(this.Page, "openNewWindow('" + AppConfig.WebEntityURL + "MDVisionDefault.aspx" + "');");

        //        //LoginUser
        //        //bool IsLogin = MDVisionHandler.LogInUser(MDVUtility.ToStr(AppConfig.AppUserId), System.Web.HttpContext.Current.Session.SessionID);

        //        // Session["sessionid"] = System.Web.HttpContext.Current.Session.SessionID;
        //        //SqlConnection conn = new SqlConnection("Data Source=192.168.0.11;Initial Catalog=MDVision;Persist Security Info=True;User ID=mdvision;Password=mdvision786");
        //        //string query = "insert into  ##LoginTest values(" + AppConfig.AppUserId + ",'" + System.Web.HttpContext.Current.Session.SessionID + "','" + true + "')";
        //        //conn.Open();
        //        //SqlCommand cmd = new SqlCommand(query, conn);
        //        //cmd.ExecuteNonQuery();
        //        //conn.Close();
        //    }
        //    else
        //    {
        //        error = "Client URL is invalid." + "\r\n" + "Please contact your system Administrator.";
        //        // lblProfileLoginError.Text = error.Replace("\r\n", "<br/>");
        //    }
        //}
        private void DefaultEntityLogin_(List<SoftwareCustomerInfoModel_> listCustomer, ref string error)
        {
            //new UserLoginHelper().SetCustomerConfig(listCustomer); // This line only seting CustomerConfig But..
            new UserLoginHelper().LogIn_();  // LogIn_  seting both Cutomer and Application object that handel some issues. before to change this please inform Architecture team.

            if (MDVSession.Current.EntityId == null || Convert.ToInt64(MDVSession.Current.EntityId) == 0)
            {
                error = "Selected entity " + MDVSession.Current.EntityRegCode + " is not configure." + " \r\n" + "Please contact your Administrator.";
                lblProfileLoginError.Text = error.Replace("\r\n", "<br/>");
                return;
            }


            tdlogin.Visible = true;
            tbEntity.Visible = false;
            if (_username == AppPrivileges.DefaultUser)
            {
                Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionAdmin.aspx", false);
            }
            else
            {
                Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionDefault.aspx", false);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            MDVSession.Current.ClearAppData();
            txtLoginPassword.Text = "";
            tdlogin.Visible = true;
            tbEntity.Visible = false;
            lblFormTitle.Text = "Member Login";
        }

        /// <summary>
        /// Called when [selection].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="BulletedListEventArgs"/> instance containing the event data.</param>
        protected void OnlistSelection(object sender, BulletedListEventArgs e)
        {
            try
            {
                var error = "";
                MDVSession.Current.EntityId = blistEntity.Items[e.Index].Value;
                MDVSession.Current.EntityRegCode = blistEntity.Items[e.Index].Text;

                if (MDVSession.Current.EntityId == null || Convert.ToInt64(MDVSession.Current.EntityId) == 0)
                {
                    error = "Selected entity " + MDVSession.Current.EntityRegCode + " is not configure." + " \r\n" + "Please contact your Administrator.";
                    lblProfileLoginError.Text = error.Replace("\r\n", "<br/>");
                    return;
                }

                if (MDVSession.Current.listCustomerInfo != null)
                {
                    var list = MDVSession.Current.listCustomerInfo.Where(s => s.EntityId == MDVSession.Current.EntityId.ToString()); //MDVSession.Current.listCustomerInfo.Select("EntityId" + "=" + MDVSession.Current.EntityId);
                    IEnumerable<SoftwareCustomerInfoModel_> softwareCustomerInfoModelS = list as IList<SoftwareCustomerInfoModel_> ?? list.ToList();
                    if (softwareCustomerInfoModelS.Count() == 1)
                    {

                        if (MDVSession.Current.UserLoggedIn == false)
                            error = new UserLoginHelper().LogIn_();
                        if (error != "")
                        {
                            ErrorMessage = error;
                            MDVLogger.PresentationLog("MDVisionLogin.aspx", "Page_PreInit", ErrorMessage, "MDVisionLogin", "none");
                            new BLLAdminSecurity().InsertUsersActivityLog(DALUsersActivity.AuditableEvents.LogIn, DALUsersActivity.TITLE_SOFTWARE_NAME, false, "Error during Login user: " + ErrorMessage);
                            Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionLogin.aspx?error=" + ErrorMessage, false);
                        }
                        MDVSession.Current.WebEntityURL = softwareCustomerInfoModelS.First().WebServerURL; //dr[0][DSSoftwareCustomersInfo.FIELD_WEB_SERVICE_URL].ToString();


                        // MDVSession.Current.CustomerRegCode = dr[0][DSSoftwareCustomersInfo.FIELD_CUSTOMER_REG_CODE].ToString();
                        // MDVSession.Current.isTestDatabase = Convert.ToBoolean(dr[0][DSSoftwareCustomersInfo.FIELD_IS_TEST_DATABASE]);

                        //LoginUser
                        //bool IsLogin = MDVisionHandler.LogInUser(MDVUtility.ToStr(AppConfig.AppUserId), System.Web.HttpContext.Current.Session.SessionID);

                        //Session["sessionid"] = System.Web.HttpContext.Current.Session.SessionID;
                        //SqlConnection conn = new SqlConnection("Data Source=192.168.0.11;Initial Catalog=MDVision;Persist Security Info=True;User ID=mdvision;Password=mdvision786");
                        //string query = "insert into  ##LoginTest values('" + MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + true + "')";
                        //conn.Open();
                        //SqlCommand cmd = new SqlCommand(query, conn);
                        //cmd.ExecuteNonQuery();
                        //conn.Close();

                        //if (AppConfig.isTestDatabase)
                        //{
                        if (MDVSession.Current.PreferredScreenName != null && MDVSession.Current.PreferredScreenName == "Billing")
                        {
                            Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionBilling.aspx", false);
                        }
                        else if (MDVSession.Current.PreferredScreenName != null && MDVSession.Current.PreferredScreenName == "Auditable Events")
                        {
                            Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionAuditableEvents.aspx", false);
                        }
                        else if (MDVSession.Current.PreferredScreenName != null && MDVSession.Current.PreferredScreenName == "Admin")
                        {
                            Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionAdmin.aspx", false);
                        }
                        else
                        {
                            Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionDefault.aspx", false);
                        }
                        //}
                        //else
                        //{
                        //    JsFuncs.RunJS(this.Page, "openNewWindow('" + AppConfig.WebEntityURL + "MDVisionDefault.aspx" + "');");
                        //}                        
                        //lblProfileLoginError.Text = 
                    }
                    else
                        error = "Selected entity " + MDVSession.Current.EntityRegCode + " URL is invalid." + " \r\n" + "Please contact your Administrator.";
                }
                else
                {

                    error = "You have been logged out" + "\r\n" + "Please Login again.";
                    lblErrMsg.Text = error.Replace("\r\n", "<br/>");
                    btnClose_Click(sender, e);
                }
                lblProfileLoginError.Text = error.Replace("\r\n", "<br/>");

            }
            catch (Exception ex)
            {

                MDVLogger.PresentationErrorLog("OnlistSelection", ex, MDVSession.Current.AppUserName);

            }


        }
        private void ShowChangePasswordOnFirstLogin(bool showChangePasswordFields)
        {
            tdlogin.Visible = !showChangePasswordFields;
            tblChangePassword.Visible = showChangePasswordFields;
            lblFormTitle.Text = "Change Password";
            lblPwdChangeMsg.Text = "Password Reset Required !";
        }
        private void ShowChangePassword(bool showChangePasswordFields)
        {
            tdlogin.Visible = !showChangePasswordFields;
            tblChangePassword.Visible = showChangePasswordFields;
            lblFormTitle.Text = "Change Password";
        }


        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            if (IsNewPasswordValid() && ViewState["UserId"] != null)
            {
                txtLoginPassword.Text = txtNewPassword.Text;

                try
                {
                    BLLAdminSecurity bllAdminSecurityObj = new BLLAdminSecurity();
                    BLObject<string> obj = bllAdminSecurityObj.UpdateUserPassword(MDVUtility.ToInt32(ViewState["UserId"]), txtLoginUserName.Text, MDVUtility.EncryptToSHA256(txtNewPassword.Text, txtLoginUserName.Text));

                    if (obj != null && obj.Data == "")
                    {
                        tblChangePassword.Visible = false;
                        lblPwdChangeMsg.Text = "Your Password has been Successfully Changed.";
                        Login_Click(sender, e);
                    }
                    else
                    {
                        if (obj != null) lblPasswordChangeError.Text = obj.Data;
                    }
                    //        if (obj.Data != null)
                    //        {
                    //            //successfully changed!
                    //            Login_Click(sender, e);
                    //        }
                    //        else
                    //        {
                    //            //could not update password
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //row count 0
                    //    }
                    //}
                    //else
                    //{
                    //    //user doesn't exists
                    //}
                }
                catch
                {
                    //
                }

            }
            else
            {


                //password isn't valid
            }


        }

        private bool IsNewPasswordValid()
        {
            string newPassword = txtNewPassword.Text;

            if (newPassword == "")
            {
                lblPasswordChangeError.Text = "Please Enter New Password";
                return false;
            }
            if (txtConfirmPassword.Text == "")
            {
                lblPasswordChangeError.Text = "Please Enter Confirm Password";
                return false;
            }
            if (newPassword != txtConfirmPassword.Text)
            {
                lblPasswordChangeError.Text = "New Password and Confirm Password Mismatched";
                return false;
            }

            Regex regex = new Regex(MDVSession.Current.PasswordRegex);
            if (regex.IsMatch(newPassword))
            {
                return (true);
            }
            else
            {
                lblPasswordChangeError.Text = "New Password does not meet the password complexity criteria. Please Contact Your System Administrator";
                return (false);
            }
        }

    }
}
