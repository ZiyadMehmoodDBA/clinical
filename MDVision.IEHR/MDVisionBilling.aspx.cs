using System;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MDVision.Business.BLL;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;
using MDVision.IEHR.Security;
using MDVision.Business.BCommon;
using System.Collections.Generic;
using MDVision.Model.User;

namespace MDVision.IEHR
{
    public partial class _Billing : System.Web.UI.Page
    {
        public bool TestDb = true;
        public bool TTTD = false;
        public bool IsEmrRequire = true;
        public string ErrorMessage { get; set; }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            TestDb = Convert.ToBoolean(WebConfigurationManager.AppSettings["IsTestDatabase"]);
            if (IsPostBack) return;

            var error = "";
            if (MDVSession.Current.UserLoggedIn == false)
                error = new UserLoginHelper().LogIn_();
            IsEmrRequire = MDVSession.Current.isEMR;
            if (error != "")
            {
                ErrorMessage = error;
                MDVLogger.PresentationLog("mdvisionDefault.aspx", "Page_PreInit", ErrorMessage, "AliAwan", "none");
                new BLLAdminSecurity().InsertUsersActivityLog(DALUsersActivity.AuditableEvents.LogIn, DALUsersActivity.TITLE_SOFTWARE_NAME, false, "Error during Login user: " + ErrorMessage);
                Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionLogin.aspx?error=" + ErrorMessage, false);
                ////Response.Redirect(AppConfig.WebEntityURL + "MDVisionLogin.aspx");
                ////throw new Exception(error);
            }
            userCurrentTime.InnerText = DateTime.Now.ToString("ddd, MMM dd, yyyy h:mm:ss tt");
            ////JsFuncs.RunJS(this.Page, "store.clearAllSession();");
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                lblUserName.Text = "";
                lblUserEntity.Text = "";

                try
                {
                    if (MDVSession.Current.UserLoggedIn)
                    {

                        //UserLoginHelper.AddClientInfo(MDVSession.Current.SessionID, Request.Browser);

                        lblUserName.Text = MDVSession.Current.AppUserLastName + ", " + MDVSession.Current.AppUserFirstName;
                        lblUserEntity.Text = MDVSession.Current.EntityRegCode;


                        ////IEnumerable<MDVision.Model.Security.SoftwareCustomerInfoModel_> list = MDVSession.Current.listCustomerInfo.Where(s => s.EntityId == MDVSession.Current.EntityId.ToString());
                        ClinicalNotesHelper.Instance().Clinical_RemoveUserNoteAccess(false, string.Empty);

                        if (MDVSession.Current.listCustomerInfo.Count > 1 && MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != AppPrivileges.DefaultUser)
                        {
                            BLObject<List<UserModel>> objLogin = new BLLAdminSecurity().Login_(MDVSession.Current.listCustomerInfo, "", MDVSession.Current.EntityRegCode);
                            new UserLoginHelper().SetApplicationConfig_(objLogin.Data);
                            ////blistSwitchEntity.DataSource = MDVSession.Current.dsCustomerInfo.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Select(DSSoftwareCustomersInfo.FIELD_ENTITY_ID + "<>" + MDVSession.Current.EntityId).CopyToDataTable();

                            blistSwitchEntity.DataSource = MDVSession.Current.listCustomerInfo.Where(s => s.EntityId != MDVSession.Current.EntityId.ToString()).ToList();
                            blistSwitchEntity.DataValueField = DSSoftwareCustomersInfo.FIELD_ENTITY_ID;
                            blistSwitchEntity.DataTextField = DSSoftwareCustomersInfo.FIELD_ENTITY_REG_CODE;
                            blistSwitchEntity.DataBind();
                        }
                        //// lblUserTitle.Text = MDVSession.Current.AppUserName;
                        if (MDVSession.Current.MessagesCount != "")
                        {
                            ////Show User Messages Icon and MessagesCount
                            btnPatientMessages.Style.Add("display", "inline-block");
                            //if (MDVUtility.ToInt32(MDVSession.Current.MessagesCount) > 0)
                            //{
                            //    //spnMessageCount.InnerText = MDVSession.Current.MessagesCount;
                            //    //hfAppUserId.Value = MDVUtility.ToStr(AppConfig.AppUserId);
                            //}
                            ////else
                            ////{
                            ////    btnPatientMessages.Title = "New Message";
                            ////}
                            ////Show User Tasks Icon and MessagesCount
                            btnUserTasks.Style.Add("display", "inline-block");
                            if (MDVUtility.ToInt32(MDVSession.Current.UserTasksCount) > 0)
                            {
                                spnUserTasksCount.InnerText = MDVSession.Current.UserTasksCount;
                            }


                            ////else
                            ////{
                            ////    btnUserTasks.Title = "New Task"; 
                            ////}

                        }

                        //if (MDVUtility.ToInt32(MDVSession.Current.AppointmentsCount) > 0)
                        //{
                        //    spnAppCount.InnerText = MDVSession.Current.AppointmentsCount;
                        //}
                        //if (MDVUtility.ToInt32(MDVSession.Current.PendingDocumentsCount) > 0)
                        //{
                        //    spnPendingDocumentsCount.InnerText = MDVSession.Current.PendingDocumentsCount;
                        //}
                        //if (MDVUtility.ToInt32(MDVSession.Current.NotesCount) > 0)
                        //{
                        //    spnNotesCount.InnerText = MDVSession.Current.NotesCount;
                        //}
                        //if (MDVSession.Current.PrescriptionsRefillCount != "0")
                        //{
                        //    spnPrescriptionsRefillCount.InnerText = MDVSession.Current.PrescriptionsRefillCount;
                        //}
                        //if (MDVSession.Current.PendingPrescriptionsCount != "0")
                        //{
                        //    spnPendingPrescriptionsCount.InnerText = MDVSession.Current.PendingPrescriptionsCount;
                        //}

                        int timeOut = MDVUtility.ToInt32(MDVSession.Current.SessionTimout);
                        timeOut = (timeOut > 30 || timeOut <= 0) ? 30 : timeOut;
                        AppPrivileges.UserPrivilages(Page);
                        JsFuncs.RunJS(Page, "SetGlobalAppData('AppUserName','" + MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('AppUserNameFullName','" + MDVSession.Current.AppUserFullName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('AppUserFirstName','" + MDVSession.Current.AppUserFirstName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('AppUserLastName','" + MDVSession.Current.AppUserLastName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsAdmin','" + MDVSession.Current.IsAdmin + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('SeletedEntityId','" + MDVSession.Current.EntityId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultProviderId','" + MDVSession.Current.DefaultProviderId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultResourceId','" + MDVSession.Current.DefaultResourceId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultBillingProviderId','" + MDVSession.Current.DefaultBillingProviderId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultFacilityId','" + MDVSession.Current.DefaultFacilityId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultPracticeId','" + MDVSession.Current.DefaultPracticeId + "');");
                        ////JsFuncs.RunJS(this.Page, "SetGlobalAppData('DefaultResourceId','" + MDVSession.Current.DefaultResourceId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultProviderName','" + MDVSession.Current.DefaultProviderName + "');");
                        ////JsFuncs.RunJS(this.Page, "SetGlobalAppData('DefaultBillingProviderName','" + MDVSession.Current.DefaultBillingProviderName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultFacilityName','" + MDVSession.Current.DefaultFacilityName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultFacilityDescription','" + MDVSession.Current.DefaultFacilityDescription + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultPracticeName','" + MDVSession.Current.DefaultPracticeName + "');");
                        ////JsFuncs.RunJS(this.Page, "SetGlobalAppData('DefaultResourceName','" + MDVSession.Current.DefaultResourceName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('EntityUserOptionId','" + MDVSession.Current.EntityUserOptionId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('AppUserId','" + MDVSession.Current.AppUserId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('UserMessagesCount','" + MDVSession.Current.MessagesCount + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DocumentPendingCount','" + MDVSession.Current.DocumentPendingCount + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IMO_ID','" + MDVSession.Current.IMO_ID + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('OCRLicenseKey','" + MDVSession.Current.OCRLicenseKey + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DateFormat','" + MDVSession.Current.DateFormat + "');");

                        ////if (ConfigurationManager.AppSettings["VersionNo"] != null)
                        ////  if (ConfigurationManager.AppSettings["VersionNo"].ToString() != "")
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PatchNo','" + Global.patchNo + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('VersionNo','" + MDVApplication.CurrentVersion + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('FileSize','" + MDVSession.Current.FileSize + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('RefreshTime','" + TimeSpan.FromMinutes(MDVUtility.ToDouble(MDVSession.Current.RefreshTime)).TotalMilliseconds + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultThemeName','" + MDVSession.Current.DefaultThemeName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultCurrency','" + MDVSession.Current.DefaultCurrency + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DecimalPlaces','" + MDVSession.Current.DecimalPlaces + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('SessionTimout','" + timeOut + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PreferredScreenName','" + MDVSession.Current.PreferredScreenName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PreferredSchScreenName','" + MDVSession.Current.PreferredSchScreenName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PreferredAppointmentStatus','" + MDVSession.Current.PreferredAppointmentStatus + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsSelectNoteComponent','" + MDVSession.Current.IsSelectNoteComponent + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsExpand','" + MDVSession.Current.IsExpand + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsSearchCriteriaExpand','" + MDVSession.Current.IsSearchCriteriaExpand + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('NotePrevieStyle','" + MDVSession.Current.NotePrevieStyle + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PreferredBillingScreenName','" + MDVSession.Current.PreferredBillingScreenName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('RaceIds','" + MDVSession.Current.RaceIds + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsShowFacilityShortName','" + MDVSession.Current.IsShowFacilityShortName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsConfigureAlerts','" + MDVSession.Current.IsConfigureAlerts + "');");
                        if (MDVSession.Current.AppUserName == "MDVISION")
                        {
                            JsFuncs.RunJS(Page, "SetGlobalAppData('SessionTimout','" + 30 + "');");
                        }

                        ////Session["sessionid"] = System.Web.HttpContext.Current.Session.SessionID;
                        ////SqlConnection conn = new SqlConnection("Data Source=192.168.0.11;Initial Catalog=MDVision;Persist Security Info=True;User ID=mdvision;Password=mdvision786");
                        ////string query = "insert into  ##LoginTest values(" + AppConfig.AppUserId+ ",'" + System.Web.HttpContext.Current.Session.SessionID+"','"+true+"')";
                        ////conn.Open();
                        ////SqlCommand cmd = new SqlCommand(query,conn);
                        ////cmd.ExecuteNonQuery();
                        ////conn.Close();

                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsSearchPatient','" + MDVSession.Current.IsSearchPatient + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsQuickAddPatient','" + MDVSession.Current.IsQuickAddPatient + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsDocument','" + MDVSession.Current.IsDocument + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsPETemplateNameRequired','" + MDVSession.Current.isPETemplateNameRequired + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsAppointment','" + MDVSession.Current.IsAppointment + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsNote','" + MDVSession.Current.IsNote + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsFax','" + MDVSession.Current.IsFax + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsTask','" + MDVSession.Current.IsTask + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsMessage','" + MDVSession.Current.IsMessage + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsPrescriptionsRefill','" + MDVSession.Current.IsPrescriptionsRefill + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsPendingPrescriptions','" + MDVSession.Current.IsPendingPrescriptions + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsImmunizationAlert','" + MDVSession.Current.IsImmunizationAlert + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsDocumentsAlert','" + MDVSession.Current.IsDocumentsAlert + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsSimultaneousLoginAllowed','" + ConfigurationManager.AppSettings["IsSimultaneousLoginAllowed"] + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsDirectAddress','" + MDVSession.Current.isDirectAddress + "');");

                        JsFuncs.RunJS(Page, "SetGlobalAppData('FavListNames','" + MDVSession.Current.FavListNames + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('FreeTextNames','" + MDVSession.Current.FreeTextNames + "');");

                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBPatientAdvanceBalance','" + MDVSession.Current.PBPatientAdvanceBalance + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBPrimaryInsurance','" + MDVSession.Current.PBPrimaryInsurance + "');");

                        ////JsFuncs.RunJS(this.Page, "SetGlobalAppData('PBPhone1','" + MDVSession.Current.PBPhone1 + "');");
                        ////JsFuncs.RunJS(this.Page, "SetGlobalAppData('PBPhone2','" + MDVSession.Current.PBPhone2 + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBPCP','" + MDVSession.Current.PBPCP + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBRefProvider','" + MDVSession.Current.PBRefProvider + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBPlanBalance','" + MDVSession.Current.PBPlanBalance + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBPatientBalance','" + MDVSession.Current.PBPatientBalance + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsPBCollection','" + MDVSession.Current.IsPBCollection + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PasswordRegex',/" + MDVSession.Current.PasswordRegex + "/);");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('EntityRegCode','" + MDVSession.Current.EntityRegCode + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBPreferredPhone','" + MDVSession.Current.PBPreferredPhone + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsAssignedResults','" + MDVSession.Current.IsAssignedResults + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultSuperBill','" + MDVSession.Current.DefaultSuperBill + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultTemplate','" + MDVSession.Current.DefaultTemplate + "');");

                        JsFuncs.RunJS(Page, "SetGlobalAppData('ENMCodesTime','" + MDVSession.Current.ENMCodesTime + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('NoteFontSize','" + MDVSession.Current.NoteFontSize + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsDefaultHPI','" + MDVSession.Current.IsDefaultHPI + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('EMCodeTypeIds','" + MDVSession.Current.EMCodeTypeIds + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsFullSSN','" + MDVSession.Current.IsFullSSN + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsCollection','" + MDVSession.Current.IsCollection + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultDocumentPriorityId','" + MDVSession.Current.DefaultDocumentPriorityId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultDocumentPriorityName','" + MDVSession.Current.DefaultDocumentPriorityName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('SchedulerTimeInterval','" + MDVSession.Current.DefaultSchedulerTimeInterval + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('sendBillingInquiry','" + MDVSession.Current.sendBillingInquiry + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsShowSuccessMessages','" + MDVSession.Current.IsShowSuccessMessages + "');"); JsFuncs.RunJS(Page, "SetGlobalAppData('PBPatientCCMTimer','" + MDVSession.Current.PBPatientCCMTimer + "');");
                        var isEmergencyRole = false;
                        if (MDVSession.Current.ListUserPrivileges.Count > 0)
                        {
                            isEmergencyRole = MDVSession.Current.ListUserPrivileges[0].IsEmergencyRole != null && Convert.ToBoolean(MDVUtility.ToLong(MDVSession.Current.ListUserPrivileges[0].IsEmergencyRole));
                            JsFuncs.RunJS(Page, "SetGlobalAppData('IsEmergencyRole','" + isEmergencyRole + "');");

                        }
                        if (isEmergencyRole)
                        {
                            regularUserIcon.Visible = false;
                            emergencyUserIcon.Visible = true;
                        }
                        else
                        {
                            regularUserIcon.Visible = true;
                            emergencyUserIcon.Visible = false;
                        }
                        ////end change azhar

                        if (!IsEmrRequire)
                            mstrMenuClinical.Visible = false;
                    }
                    else
                    {
                        Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionLogin.aspx" + (!string.IsNullOrEmpty(ErrorMessage) ? "?error=" + ErrorMessage : ""), false);
                        ////System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }
                }
                catch (Exception ex)
                {
                    MDVLogger.PresentationErrorLog("pageLoad", ex, "AliAwan");
                    Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionLogin.aspx?error=" + ex.Message, false);
                    //// System.Web.Security.FormsAuthentication.RedirectToLoginPage("error=" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Logout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Logout_Click(object sender, EventArgs e)
        {
            ////if (AppConfig.isTestDatabase)
            var Message = "";
            if (hdnIsSessionTimeout.Value == "1")
            {
                Message = "?error=" + Common.AppPrivileges.SESSION_TIMEOUT_MESSAGE;
            }
            Response.Redirect(new UserLoginHelper().Logout() + Message, true);
        }


        protected void Dashboardsetting_Click(object sender, EventArgs e)
        {

        }

        protected void SwitchEntity_Click(object sender, BulletedListEventArgs e)
        {
            var tempUsername = MDVSession.Current.AppUserName;
            var tempPassWord = MDVSession.Current.AppPassWord;
            ////DSSoftwareCustomersInfo tempDataSet = MDVSession.Current.dsCustomerInfo;   MK
            var tempList = MDVSession.Current.listCustomerInfo;


            MDVSession.Current.ClearAppData();
            ////MDVSession.Current.dsCustomerInfo = tempDataSet; MK
            MDVSession.Current.listCustomerInfo = tempList;
            MDVSession.Current.AppUserName = tempUsername;
            MDVSession.Current.AppPassWord = tempPassWord;

            var error = "";

            MDVSession.Current.EntityId = blistSwitchEntity.Items[e.Index].Value;
            MDVSession.Current.EntityRegCode = blistSwitchEntity.Items[e.Index].Text;

            if (MDVSession.Current.EntityId == null || Convert.ToInt64(MDVSession.Current.EntityId) == 0)
            {
                error = "Selected entity " + MDVSession.Current.EntityRegCode + " is not configure." + " \r\n" + "Please contact your Administrator.";
                return;
            }

            if (MDVSession.Current.listCustomerInfo != null)
            {
                ////DataRow[] dr = MDVSession.Current.dsCustomerInfo.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Select(DSSoftwareCustomersInfo.FIELD_ENTITY_ID + "=" + MDVSession.Current.EntityId);
                var list = MDVSession.Current.listCustomerInfo.Where(s => s.EntityId == MDVSession.Current.EntityId.ToString()).ToList();
                if (list.Count == 1)
                {
                    MDVSession.Current.WebEntityURL = list[0].WebServerURL;


                    ////  MDVSession.Current.CustomerRegCode = dr[0][DSSoftwareCustomersInfo.FIELD_CUSTOMER_REG_CODE].ToString();
                    //// MDVSession.Current.isTestDatabase = Convert.ToBoolean(dr[0][DSSoftwareCustomersInfo.FIELD_IS_TEST_DATABASE]);

                    ////if (AppConfig.isTestDatabase)
                    ////{
                    if (string.IsNullOrEmpty(PreInt()))
                        Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionBilling.aspx");
                    ////}
                    ////else
                    ////{
                    ////    JsFuncs.RunJS(this.Page, "openNewWindow('" + AppConfig.WebEntityURL + "MDVisionDefault.aspx" + "');");
                    ////}

                }
                else
                    error = "Selected entity " + MDVSession.Current.EntityRegCode + " URL is invalid." + " \r\n" + "Please contact your Administrator.";
            }
            else
            {

                error = "You have been logged out" + "\r\n" + "Please Login again.";
            }
        }

        protected void test_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private string PreInt()
        {
            var error = "";
            if (MDVSession.Current.UserLoggedIn == false)
                error = new UserLoginHelper().LogIn_();
            IsEmrRequire = MDVSession.Current.isEMR;
            if (error != "")
            {
                ErrorMessage = error;
                MDVLogger.PresentationLog("mdvisionDefault.aspx", "Page_PreInit", ErrorMessage, "AliAwan", "none");
                new BLLAdminSecurity().InsertUsersActivityLog(DALUsersActivity.AuditableEvents.LogIn, DALUsersActivity.TITLE_SOFTWARE_NAME, false, "Error during Login user: " + ErrorMessage);
                Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionLogin.aspx?error=" + ErrorMessage, false);
                //Response.Redirect(AppConfig.WebEntityURL + "MDVisionLogin.aspx");
                //throw new Exception(error);
            }
            userCurrentTime.InnerText = DateTime.Now.ToString("ddd, MMM dd, yyyy h:mm:ss tt");
            //JsFuncs.RunJS(this.Page, "store.clearAllSession();");

            return error;
        }
    }

}