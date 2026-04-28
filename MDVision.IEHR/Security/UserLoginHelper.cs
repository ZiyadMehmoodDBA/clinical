using MDVision.Business.BLL;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.Model.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using MDVision.Business.BCommon;
using MDVision.Model.User;
using MDVision.IEHR.Common;
using MDVision.IEHR.Common.ProviderNoteAccess;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;
using System.Web.Configuration;

namespace MDVision.IEHR.Security
{
    public class UserLoginHelper
    {
        public string LogIn_()
        {
            try
            {
                if (MDVSession.Current.listCustomerInfo != null && MDVSession.Current.EntityId != "" && MDVSession.Current.AppUserName != "" && MDVSession.Current.AppPassWord != "")
                {
                    var list = MDVSession.Current.listCustomerInfo.Where(s => s.EntityId == MDVSession.Current.EntityId.ToString());
                    {
                        new UserLoginHelper().SetCustomerConfig(list.ToList());
                        BLObject<List<UserModel>> objLogin = new BLLAdminSecurity().Login_(MDVSession.Current.listCustomerInfo, "", MDVSession.Current.EntityRegCode);
                        if (objLogin.Data == null)
                        {
                            if (objLogin.Message != "")
                                return objLogin.Message;
                        }
                        else
                        {
                            new RcopiaHelper().setRcopiaNotificationCount(objLogin.Data);
                            new UserLoginHelper().SetApplicationConfig_(objLogin.Data);
                        }
                    }
                }
                else
                {
                    string message_ = GetMessage();
                    return !string.IsNullOrEmpty(message_) ? message_ : AppPrivileges.SESSION_TIMEOUT_MESSAGE; ;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        private string GetMessage()
        {
            string Message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(MDVSession.Current.RequestModel))
                {
                    PreRequestModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<PreRequestModel>(MDVSession.Current.RequestModel);

                    if (!string.IsNullOrEmpty(model.RedirectSet))
                    {
                        dynamic obj_ = Newtonsoft.Json.JsonConvert.DeserializeObject(model.RedirectSet);
                        string url = obj_ != null && obj_["Url"] != null ? obj_["Url"].Value.ToString() : "";

                        if (!string.IsNullOrEmpty(url) && url.Contains("error=") && url.Length > url.IndexOf("error=") + 6)
                            Message = url.Substring(url.IndexOf("error=") + 6);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Message;
            
        }
        /// <summary>
        /// Re-login this instance.
        /// </summary>
        public string ReLogIn()
        {
            return LogIn_();
        }
        public string Logout()
        {
            var webEntityUrl = MDVSession.Current.WebEntityURL;
            try
            {
                new BLLAdminSecurity().InsertUsersActivityLog(DALUsersActivity.AuditableEvents.LogOut, DALUsersActivity.TITLE_SOFTWARE_NAME, true, "Logout user");
                ClinicalNotesHelper.Instance().Clinical_RemoveUserNoteAccess(false, string.Empty);
                MDVSession.Current.ClearSession();
                FormsAuthentication.SignOut();
                return webEntityUrl + "MDVisionLogin.aspx";
            }
            catch (Exception ex)
            {
                MDVLogger.PresentationErrorLog("Logout", ex, MDVUtility.DecryptFrom64(MDVUtility.ToStr(MDVSession.Current.AppUserName)));
                return webEntityUrl + "MDVisionLogin.aspx?error=" + ex.Message;
            }
        }

        public bool IsCookieExists()
        {
            if (HttpContext.Current.Session.IsNewSession)
            {
                string CookieHeaders = HttpContext.Current.Request.Headers["Cookie"];

                if ((null != CookieHeaders) && (CookieHeaders.IndexOf("MDV") >= 0))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the customer configuration.
        /// </summary>
        /// <param name="drCustomerUser">The dr customer user.</param>
        public void SetCustomerConfig(DataRow drCustomerUser)
        {
            if (drCustomerUser != null)
            {

                MDVSession.Current.ReportURL = drCustomerUser[DSSoftwareCustomersInfo.FIELD_REPORTURL].ToString();
                MDVSession.Current.DomainName = drCustomerUser[DSSoftwareCustomersInfo.FIELD_DOMAINNAME].ToString();
                MDVSession.Current.DomainUserName = drCustomerUser[DSSoftwareCustomersInfo.FIELD_DOMAINUSERNAME].ToString();
                MDVSession.Current.DomainPassword = drCustomerUser[DSSoftwareCustomersInfo.FIELD_DOMAINPASSWORD].ToString();
                MDVSession.Current.WebEntityURL = drCustomerUser[DSSoftwareCustomersInfo.FIELD_WEB_SERVICE_URL].ToString();
                MDVSession.Current.EntityId = drCustomerUser[DSSoftwareCustomersInfo.FIELD_ENTITY_ID].ToString();
                MDVSession.Current.EntityRegCode = drCustomerUser[DSSoftwareCustomersInfo.FIELD_ENTITY_REG_CODE].ToString();
                MDVSession.Current.ImagePath = "~/Content/PatientImages";
                MDVSession.Current.ClientId = drCustomerUser[DSSoftwareCustomersInfo.FIELD_UNIQUE_CLIENT_ID].ToString();
                MDVSession.Current.AppUserId = MDVUtility.ToLong(drCustomerUser[DSSoftwareCustomersInfo.FIELD_USER_ID]);
                MDVSession.Current.DateFormat = drCustomerUser[DSSoftwareCustomersInfo.FIELD_DATE_FORMAT].ToString();
                MDVSession.Current.IMOHostName = drCustomerUser[DSSoftwareCustomersInfo.FIELD_IMO_HOST_NAME].ToString();
                MDVSession.Current.IMOICDPort = drCustomerUser[DSSoftwareCustomersInfo.FIELD_IMO_ICD_PORT].ToString();
                MDVSession.Current.IMOCPTPort = drCustomerUser[DSSoftwareCustomersInfo.FIELD_IMO_CPT_PORT].ToString();
                MDVSession.Current.IMO_ID = drCustomerUser[DSSoftwareCustomersInfo.FIELD_IMO_ID].ToString();
                MDVSession.Current.UserHostIP = MDVUtility.GetLanIPAddress();

                if (drCustomerUser[DSSoftwareCustomersInfo.FIELD_FILE_SIZE].ToString() == "")
                    MDVSession.Current.FileSize = 5;
                else
                    MDVSession.Current.FileSize = Convert.ToInt32(drCustomerUser[DSSoftwareCustomersInfo.FIELD_FILE_SIZE].ToString());

                MDVSession.Current.FTPPortNo = drCustomerUser[DSSoftwareCustomersInfo.FIELD_FTP_PORT_NO].ToString();
                MDVSession.Current.DocsHostName = drCustomerUser[DSSoftwareCustomersInfo.FIELD_DOCS_HOST_NAME].ToString();
                MDVSession.Current.DocsAlias = drCustomerUser[DSSoftwareCustomersInfo.FIELD_DOCS_ALIAS].ToString();
                MDVSession.Current.RefreshTime = drCustomerUser[DSSoftwareCustomersInfo.FIELD_REFRESH_TIME].ToString();
                MDVSession.Current.DefaultCurrency = drCustomerUser[DSSoftwareCustomersInfo.FIELD_CURRENCY].ToString();
                MDVSession.Current.DecimalPlaces = drCustomerUser[DSSoftwareCustomersInfo.FIELD_DECIMAL_PLACES].ToString();

                MDVSession.Current.ClaimScrubberEDIServer = drCustomerUser[DSSoftwareCustomersInfo.FIELD_CLAIM_SCRUBBER_EDI_SERVER].ToString();
                MDVSession.Current.ClaimScrubberPassword = drCustomerUser[DSSoftwareCustomersInfo.FIELD_CLAIM_SCRUBBER_PASSWORD].ToString();
                MDVSession.Current.ClaimScrubberSubmitterID = drCustomerUser[DSSoftwareCustomersInfo.FIELD_CLAIM_SCRUBBER_SUBMITTER_ID].ToString();
                MDVSession.Current.ClaimScrubberUser = drCustomerUser[DSSoftwareCustomersInfo.FIELD_CLAIM_SCRUBBER_USER].ToString();
            }

        }

        public void SetCustomerConfig(List<SoftwareCustomerInfoModel_> listCustomerUser)
        {
            if (listCustomerUser == null) return;

            MDVSession.Current.ReportURL = listCustomerUser[0].ReportURL;
            MDVSession.Current.DomainName = listCustomerUser[0].DomainName;
            MDVSession.Current.DomainUserName = listCustomerUser[0].DomainUserName;
            MDVSession.Current.DomainPassword = listCustomerUser[0].DomainPassword;
            MDVSession.Current.WebEntityURL = listCustomerUser[0].WebServerURL;
            MDVSession.Current.EntityId = listCustomerUser[0].EntityId;
            MDVSession.Current.EntityRegCode = listCustomerUser[0].EntityRegCode;
            MDVSession.Current.ImagePath = "~/Content/PatientImages";
            MDVSession.Current.ClientId = listCustomerUser[0].UniqueClientId;
            MDVSession.Current.AppUserId = MDVUtility.ToLong(listCustomerUser[0].UserId);
            MDVSession.Current.DateFormat = listCustomerUser[0].DateFormats;
            MDVSession.Current.IMOHostName = listCustomerUser[0].IMOHostName;
            MDVSession.Current.IMOICDPort = listCustomerUser[0].IMOICDPort;
            MDVSession.Current.IMOCPTPort = listCustomerUser[0].IMOCPTPort;
            MDVSession.Current.IMO_ID = listCustomerUser[0].IMO_ID;
            MDVSession.Current.UserHostIP = MDVUtility.GetLanIPAddress();

            MDVSession.Current.FTPPortNo = listCustomerUser[0].Ftp_PortNo;
            MDVSession.Current.DocsHostName = listCustomerUser[0].Docs_HostName;
            MDVSession.Current.DocsAlias = listCustomerUser[0].Docs_Alias;
            MDVSession.Current.RefreshTime = listCustomerUser[0].RefreshTime;
            MDVSession.Current.DefaultCurrency = listCustomerUser[0].Currency;
            MDVSession.Current.DecimalPlaces = listCustomerUser[0].DecimalPlaces;

            if (listCustomerUser[0].FileSize == "")
                MDVSession.Current.FileSize = 5;
            else
                MDVSession.Current.FileSize = Convert.ToInt32(listCustomerUser[0].FileSize);

            MDVSession.Current.ClaimScrubberEDIServer = listCustomerUser[0].ClaimScrubberEDIServer;
            MDVSession.Current.ClaimScrubberPassword = listCustomerUser[0].ClaimScrubberPassword;
            MDVSession.Current.ClaimScrubberSubmitterID = listCustomerUser[0].ClaimScrubberSubmitterID;
            MDVSession.Current.ClaimScrubberUser = listCustomerUser[0].ClaimScrubberUser;
            MDVSession.Current.OCRLicenseKey = listCustomerUser[0].OCRLicenseKey;
        }

        //public void SetApplicationConfig(DSUsers objLogin)
        //{
        //    if (objLogin == null) return;

        //    MDVSession.Current.dtEntityUserOption = objLogin.EntityUserOption;
        //    MDVSession.Current.dtUserPrivileges = objLogin.UsersPrivileges;
        //    MDVSession.Current.dtUser = objLogin.Users;
        //    if (MDVSession.Current.dtUser != null)
        //        if (MDVSession.Current.dtUser.Rows.Count > 0)
        //        {
        //            MDVSession.Current.AppUserFullName = Convert.ToString(MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.LastNameColumn.ColumnName] + ", " + MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.FirstNameColumn.ColumnName]);
        //            MDVSession.Current.AppUserFirstName = Convert.ToString(MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.FirstNameColumn.ColumnName].ToString());
        //            MDVSession.Current.AppUserLastName = Convert.ToString(MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.LastNameColumn.ColumnName].ToString());
        //            MDVSession.Current.IsAdmin = Convert.ToBoolean(MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.IsAdminColumn.ColumnName]);
        //            MDVSession.Current.MessagesCount = MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.MessagesCountColumn.ColumnName].ToString();
        //            MDVSession.Current.UserTasksCount = MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.UserTasksCountColumn.ColumnName].ToString();
        //            MDVSession.Current.AppointmentsCount = MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.AppointmentsCountColumn.ColumnName].ToString();
        //            MDVSession.Current.NotesCount = MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.NotesCountColumn.ColumnName].ToString();
        //            MDVSession.Current.SessionTimout = MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.AutoLogOffColumn].ToString();
        //            MDVSession.Current.isEMR = Convert.ToBoolean(MDVSession.Current.dtUser.Rows[0][MDVSession.Current.dtUser.IsEMRColumn.ColumnName]);
        //        }

        //    MDVSession.Current.UserLoggedIn = true;

        //    if (MDVSession.Current.dtEntityUserOption != null)
        //    {
        //        if (MDVSession.Current.dtEntityUserOption.Rows.Count > 0)
        //        {
        //            MDVSession.Current.DefaultFacilityId = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.FacilityIdColumn.ColumnName].ToString();
        //            MDVSession.Current.DefaultFacilityName = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.FacilityNameColumn.ColumnName].ToString();
        //            MDVSession.Current.DefaultFacilityDescription = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.FacilityDescriptionColumn.ColumnName].ToString();
        //            MDVSession.Current.DefaultProviderId = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.ProviderIdColumn.ColumnName].ToString();
        //            MDVSession.Current.DefaultProviderName = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.ProviderNameColumn.ColumnName].ToString();
        //            MDVSession.Current.DefaultBillingProviderId = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.BillingProviderIdColumn.ColumnName].ToString();
        //            MDVSession.Current.DefaultPracticeId = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.PracticeIdColumn.ColumnName].ToString();
        //            MDVSession.Current.DefaultPracticeName = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.PracticeNameColumn.ColumnName].ToString();
        //            MDVSession.Current.PreferredScreen = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.PreferredScreenColumn.ColumnName].ToString();
        //            MDVSession.Current.PreferredSchScreen = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.PreferredSchScreenColumn.ColumnName].ToString();
        //            MDVSession.Current.DefaultThemeId = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.ThemeIdColumn].ToString();
        //            MDVSession.Current.DefaultSuperBill = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.DefaultSuperBillColumn.ColumnName].ToString();
        //            MDVSession.Current.PBPhoneNumber1 = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.PBPhoneNumber1Column.ColumnName].ToString();
        //            MDVSession.Current.PBPhoneNumber2 = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.PBPhoneNumber2Column.ColumnName].ToString();
        //            MDVSession.Current.ClaimPrinter = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.ClaimPrinterColumn.ColumnName].ToString();
        //            MDVSession.Current.ClaimTray = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.ClaimTrayColumn.ColumnName].ToString();
        //            MDVSession.Current.AttachmentPrinter = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.AttachmentPrinterColumn.ColumnName].ToString();
        //            MDVSession.Current.AttachmentTray = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.AttachmentTrayColumn.ColumnName].ToString();
        //            var entityUserRefreshTime = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.RefreshTimeColumn.ColumnName].ToString();
        //            MDVSession.Current.DefaultThemeName = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.ThemeNameColumn.ColumnName].ToString();
        //            MDVSession.Current.PreferredAppointmentStatus = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.AppointmentStatusColumn.ColumnName].ToString();
        //            MDVSession.Current.RefreshTime = entityUserRefreshTime != "" ? entityUserRefreshTime : "5";
        //            MDVSession.Current.PendingPrescriptionsCount = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.PendingPrescriptionsCountColumn.ColumnName].ToString();
        //            MDVSession.Current.PrescriptionsRefillCount = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.PrescriptionsRefillCountColumn.ColumnName].ToString();
        //            MDVSession.Current.IsImmunizationAlert = MDVSession.Current.dtEntityUserOption.Rows[0][MDVSession.Current.dtEntityUserOption.IsImmunizationAlertColumn.ColumnName].ToString();
        //        }
        //    }
        //    else
        //    {
        //        MDVSession.Current.PrescriptionsRefillCount = "?";
        //        MDVSession.Current.PendingPrescriptionsCount = "?";
        //    }
        //    LoginUserSession(MDVSession.Current.AppUserName, MDVSession.Current.SessionID);
        //    FormsAuthentication.SetAuthCookie(MDVSession.Current.AppUserName, false);
        //}

        public void SetApplicationConfig_(List<MDVision.Model.User.UserModel> objLogin)
        {
            if (objLogin == null) return;

            List<MDVision.Model.User.EntityUserOptions> objEntityUserOptions = objLogin[0].EntityUserOptions;
            MDVSession.Current.ListUserPrivileges = objLogin[0].UserPrivileges;
            MDVSession.Current.ListUser = objLogin[0].UserLoginModel;
            const int iMk = 0;
            if (MDVSession.Current.ListUser != null)
                if (MDVSession.Current.ListUser.Count > 0)
                {
                    MDVSession.Current.AppUserFullName = Convert.ToString(MDVSession.Current.ListUser[iMk].LastName + ", " + MDVSession.Current.ListUser[iMk].FirstName);
                    MDVSession.Current.AppUserFirstName = Convert.ToString(MDVSession.Current.ListUser[iMk].FirstName);
                    MDVSession.Current.AppUserLastName = Convert.ToString(MDVSession.Current.ListUser[iMk].LastName);
                    MDVSession.Current.IsAdmin = Convert.ToBoolean(MDVSession.Current.ListUser[iMk].IsAdmin);
                    MDVSession.Current.MessagesCount = MDVSession.Current.ListUser[iMk].MessagesCount;
                    MDVSession.Current.UserTasksCount = MDVSession.Current.ListUser[iMk].UserTasksCount;
                    MDVSession.Current.AppointmentsCount = MDVSession.Current.ListUser[iMk].AppointmentsCount;
                    MDVSession.Current.PendingDocumentsCount = MDVSession.Current.ListUser[iMk].PendingDocumentsCount;
                    MDVSession.Current.NotesCount = MDVSession.Current.ListUser[iMk].NotesCount;
                    MDVSession.Current.SessionTimout = MDVSession.Current.ListUser[iMk].AutoLogOff;
                    MDVSession.Current.isEMR = Convert.ToBoolean(MDVSession.Current.ListUser[iMk].IsEMR);
                    MDVSession.Current.isDirectAddress = Convert.ToBoolean(MDVUtility.ToLong(MDVSession.Current.ListUser[iMk].IsDirectAddress));
                    MDVSession.Current.IsFullSSN = MDVSession.Current.ListUser[iMk].IsFullSSN;
                    MDVSession.Current.IsMedText = MDVSession.Current.ListUser[iMk].IsMedText;
                    MDVSession.Current.IsCollection = MDVSession.Current.ListUser[iMk].IsCollection;
                    MDVSession.Current.DocumentPendingCount = MDVSession.Current.ListUser[iMk].DocumentPendingCount;
                    MDVSession.Current.sendBillingInquiry = Convert.ToBoolean(MDVSession.Current.ListUser[iMk].isSendBillingInquiry);
                    MDVSession.Current.AssignedResultsCount = MDVSession.Current.ListUser[iMk].AssignedResultsCount;
                }

            MDVSession.Current.UserLoggedIn = true;

            if (objEntityUserOptions != null)
            {
                //if (MDVSession.Current.ListEntityUserOption.Count > 0)
                if (objEntityUserOptions.Count > 0)
                {
                    MDVSession.Current.DefaultFacilityId = objEntityUserOptions[iMk].FacilityId;
                    MDVSession.Current.DefaultFacilityName = objEntityUserOptions[iMk].FacilityName;
                    MDVSession.Current.DefaultFacilityDescription = objEntityUserOptions[iMk].FacilityDescription;

                    MDVSession.Current.DefaultProviderId = objEntityUserOptions[iMk].ProviderId;
                    MDVSession.Current.DefaultResourceId = objEntityUserOptions[iMk].ResourceId;
                    MDVSession.Current.DefaultProviderName = objEntityUserOptions[iMk].ProviderName;
                    MDVSession.Current.DefaultBillingProviderId = objEntityUserOptions[iMk].BillingProviderId;
                    MDVSession.Current.IsBulkSign = objEntityUserOptions[iMk].IsBulkSign;

                    MDVSession.Current.DefaultPracticeId = objEntityUserOptions[iMk].PracticeId;
                    MDVSession.Current.DefaultPracticeName = objEntityUserOptions[iMk].PracticeName;

                    MDVSession.Current.PreferredScreen = objEntityUserOptions[iMk].PreferredScreen;
                    MDVSession.Current.EntityUserOptionId = objEntityUserOptions[iMk].EntityUserOptionId;
                    MDVSession.Current.PreferredSchScreen = objEntityUserOptions[iMk].PreferredSchScreen;

                    MDVSession.Current.DefaultThemeId = objEntityUserOptions[iMk].ThemeId;
                    MDVSession.Current.DefaultSuperBill = objEntityUserOptions[iMk].DefaultSuperBill;

                    MDVSession.Current.PBPCP = objEntityUserOptions[iMk].PBPCP;
                    MDVSession.Current.PBRefProvider = objEntityUserOptions[iMk].PBRefProvider;

                    MDVSession.Current.PBPhoneNumber1 = objEntityUserOptions[iMk].PBPhoneNumber1;
                    MDVSession.Current.PBPhoneNumber2 = objEntityUserOptions[iMk].PBPhoneNumber2;
                    MDVSession.Current.PBPlanBalance = objEntityUserOptions[iMk].PBPlanBalance;
                    MDVSession.Current.PBPatientBalance = objEntityUserOptions[iMk].PBPatientBalance;
                    MDVSession.Current.IsPBCollection = objEntityUserOptions[iMk].IsPBCollection;

                    //MDVSession.Current.PBPhone1 = objEntityUserOptions[iMk].PBPhoneNumber1;
                    //MDVSession.Current.PBPhone2 = objEntityUserOptions[iMk].PBPhoneNumber2;
                    MDVSession.Current.PBPatientAdvanceBalance = objEntityUserOptions[iMk].PBPatientAdvanceBalance;

                    MDVSession.Current.ClaimPrinter = objEntityUserOptions[iMk].ClaimPrinter;
                    MDVSession.Current.ClaimTray = objEntityUserOptions[iMk].ClaimTray;
                    MDVSession.Current.AttachmentPrinter = objEntityUserOptions[iMk].AttachmentPrinter;
                    MDVSession.Current.AttachmentTray = objEntityUserOptions[iMk].AttachmentTray;
                    var entityUserRefreshTime = objEntityUserOptions[iMk].RefreshTime;
                    MDVSession.Current.DefaultThemeName = objEntityUserOptions[iMk].ThemeName;
                    MDVSession.Current.PreferredAppointmentStatus = objEntityUserOptions[iMk].Appointmentstatus;
                    MDVSession.Current.RefreshTime = entityUserRefreshTime != "" ? entityUserRefreshTime : "5";
                    MDVSession.Current.PendingPrescriptionsCount = objEntityUserOptions[iMk].PendingPrescriptionsCount;
                    MDVSession.Current.PrescriptionsRefillCount = objEntityUserOptions[iMk].PrescriptionsRefillCount;
                    MDVSession.Current.IsImmunizationAlert = objEntityUserOptions[iMk].IsImmunizationAlert;
                    MDVSession.Current.IsDocumentsAlert = objEntityUserOptions[iMk].IsDocumentsAlert;
                    MDVSession.Current.NoteFontSize = objEntityUserOptions[iMk].NoteFontSize;
                    MDVSession.Current.DefaultTemplate = objEntityUserOptions[iMk].DefaultTemplate;
                    MDVSession.Current.ENMCodesTime = objEntityUserOptions[iMk].ENMCodesTime;
                    MDVSession.Current.FreeTextNames = objEntityUserOptions[iMk].FreeTextICD;
                    MDVSession.Current.FavListNames = objEntityUserOptions[iMk].FavListNames;
                    MDVSession.Current.IsSelectNoteComponent = objEntityUserOptions[iMk].IsSelectNoteComponents;
                    MDVSession.Current.IsExpand = objEntityUserOptions[iMk].IsExpand;
                    MDVSession.Current.isPETemplateNameRequired = objEntityUserOptions[iMk].isPETemplateNameRequired;
                    MDVSession.Current.WeekWorkDaysIds = objEntityUserOptions[iMk].WorkWeekDays;
                    MDVSession.Current.IsSearchCriteriaExpand = objEntityUserOptions[iMk].IsSearchCriteriaExpand;
                    MDVSession.Current.NotePrevieStyle = objEntityUserOptions[iMk].NotePrevieStyle;
                    MDVSession.Current.PBPreferredPhone = objEntityUserOptions[iMk].PreferredPhone;
                    MDVSession.Current.PBPrimaryInsurance = objEntityUserOptions[iMk].PBPrimaryInsurance;
                    MDVSession.Current.PBRefProvider = objEntityUserOptions[iMk].PBRefProvider;
                    MDVSession.Current.PreferredSchScreenName = objEntityUserOptions[iMk].PreferredSchScreenName;
                    MDVSession.Current.PreferredScreenName = objEntityUserOptions[iMk].PreferredScreenName;
                    MDVSession.Current.PreferredBillingScreen = objEntityUserOptions[iMk].PreferredBillingScreen;
                    MDVSession.Current.PreferredBillingScreenName = objEntityUserOptions[iMk].PreferredBillingScreenName;
                    MDVSession.Current.IsDefaultHPI = objEntityUserOptions[iMk].IsDefaultHPI;
                    MDVSession.Current.EMCodeTypeIds = objEntityUserOptions[iMk].EMCodesTypeIds;
                    MDVSession.Current.RaceIds = objEntityUserOptions[iMk].RaceIds;
                    MDVSession.Current.IsPrescriptionsRefill = objEntityUserOptions[iMk].IsPrescriptionsRefill;
                    MDVSession.Current.IsPendingPrescriptions = objEntityUserOptions[iMk].IsPendingPrescriptions;
                    MDVSession.Current.IsSearchPatient = objEntityUserOptions[iMk].IsSearchPatient;
                    MDVSession.Current.IsQuickAddPatient = objEntityUserOptions[iMk].IsQuickAddPatient;
                    MDVSession.Current.IsNote = objEntityUserOptions[iMk].IsNote;
                    MDVSession.Current.IsAppointment = objEntityUserOptions[iMk].IsAppointment;
                    MDVSession.Current.IsTask = objEntityUserOptions[iMk].IsTask;
                    MDVSession.Current.IsFax = objEntityUserOptions[iMk].IsFax;
                    MDVSession.Current.IsMessage = objEntityUserOptions[iMk].IsMessage;
                    MDVSession.Current.DefaultDocumentPriorityId = objEntityUserOptions[iMk].DefaultDocumentPriorityId;
                    MDVSession.Current.DefaultDocumentPriorityName = objEntityUserOptions[iMk].DefaultDocumentPriorityName;
                    MDVSession.Current.RaceIds = objEntityUserOptions[iMk].RaceIds;
                    MDVSession.Current.IsShowFacilityShortName = objEntityUserOptions[iMk].IsShowFacilityShortName;
                    MDVSession.Current.DefaultSchedulerTimeInterval = objEntityUserOptions[iMk].SchedulerTimeInterval;
                    MDVSession.Current.IsDocument = objEntityUserOptions[iMk].IsDocument;
                    MDVSession.Current.IsShowSuccessMessages = objEntityUserOptions[iMk].IsShowSuccessMessages;
                    MDVSession.Current.IsOrdersExpand = objEntityUserOptions[iMk].IsOrdersExpand;
                    MDVSession.Current.IsResultsExpand = objEntityUserOptions[iMk].IsResultsExpand;
                    MDVSession.Current.IsReferralRequired = objEntityUserOptions[iMk].IsReferralRequired;

                    MDVSession.Current.isDemographics = objEntityUserOptions[iMk].IsDemographics;
                    MDVSession.Current.isMU3FamilyHistory = objEntityUserOptions[iMk].IsMu3FamilyHistory;
                    MDVSession.Current.isTransPubHealthAgHealthCareSurveys = objEntityUserOptions[iMk].IsHealthcareSurveys;
                    MDVSession.Current.isTransmittoImmunizationRegistries = objEntityUserOptions[iMk].IsImmunizationRegistries;
                    MDVSession.Current.isPatientHealthInformationCapture = objEntityUserOptions[iMk].IsPatientHealthInformationCapture;
                    MDVSession.Current.isTransPubHealthAgCaseReporting =objEntityUserOptions[iMk].IsTransimissionCaseReporting;
                    MDVSession.Current.isTransitionCareDirectProject = objEntityUserOptions[iMk].IsTransitionDirectProject;
                    MDVSession.Current.isImplantableDevices = objEntityUserOptions[iMk].IsImplantableDevices;
                    MDVSession.Current.isDataSegmentationPrivacy = objEntityUserOptions[iMk].IsDataSegmentationPrivacy;
                    MDVSession.Current.isTransitonCancerRegistries = objEntityUserOptions[iMk].IsTransitCancerRegistries;
                    MDVSession.Current.isConsolidatedCDACreationPreformance = objEntityUserOptions[iMk].IsConsolidatedCDA;
                    MDVSession.Current.isTransPubHealthAgAntimicobialUse = objEntityUserOptions[iMk].IsTransimissionAntimicobial;
                    MDVSession.Current.isMU3SocPsycBehaviourHx = objEntityUserOptions[iMk].IsMU3SocPsyBehaviourHx;
                    MDVSession.Current.isDataExport = objEntityUserOptions[iMk].IsDataExport;
                    MDVSession.Current.isCarePlan = objEntityUserOptions[iMk].IsCarePlan;
                    MDVSession.Current.IsSelectCompOnCopyNote = objEntityUserOptions[iMk].IsSelectCompOnCopyNote;
                    MDVSession.Current.IsPreviousNoteComplaints = objEntityUserOptions[iMk].IsPrevNoteComplaints;
                    MDVSession.Current.IsPreviousNotePE = objEntityUserOptions[iMk].IsPrevNotePE;
                    MDVSession.Current.IsPreviousNoteROS = objEntityUserOptions[iMk].IsPrevNoteROS;
                    //PRD-31 by:MAHMAD
                    MDVSession.Current.IsExpandFolderTree = objEntityUserOptions[iMk].IsExpandFolderTree;
                    MDVSession.Current.IsConfigureAlerts = objEntityUserOptions[iMk].IsConfigureAlerts;
                    //PRD-31 by:MAHMAD
                    MDVSession.Current.iTrackDashboardIds = objEntityUserOptions[iMk].iTrackDashboardIds;
                    MDVSession.Current.IsPreviousNoteProblems = objEntityUserOptions[iMk].IsPrevNoteProblems;
                    MDVSession.Current.PBPatientCCMTimer = objEntityUserOptions[iMk].PBPatientCCMTimer;
                    MDVSession.Current.IsPrevNoteTreatmentComents = objEntityUserOptions[iMk].IsPrevNoteTreatmentComents;
                    if (!string.IsNullOrEmpty(objEntityUserOptions[iMk].isLandOnComponent))
                    {
                        if (objEntityUserOptions[iMk].isLandOnComponent == "2")
                            MDVSession.Current.isLandOnComponent = "true";
                        else
                            MDVSession.Current.isLandOnComponent = "false";
                    }
                    else
                        MDVSession.Current.isLandOnComponent = "false";
                    MDVSession.Current.IsCMS65v7 = objEntityUserOptions[iMk].IsCMS65v7;
                    MDVSession.Current.IsCMS69v6 = objEntityUserOptions[iMk].IsCMS69v6;
                    MDVSession.Current.IsCMS68v7 = objEntityUserOptions[iMk].IsCMS68v7;
                    MDVSession.Current.IsCMS138v6 = objEntityUserOptions[iMk].IsCMS138v6;
                    MDVSession.Current.IsCMS165v6 = objEntityUserOptions[iMk].IsCMS165v6;
                    MDVSession.Current.IsCMS22v6 = objEntityUserOptions[iMk].IsCMS22v6;
                    MDVSession.Current.IsCDS = objEntityUserOptions[iMk].IsCDS;
                    MDVSession.Current.DefaultTabMessages = objEntityUserOptions[iMk].DefaultTabMessages;
                    MDVSession.Current.RecentMessagesTab = objEntityUserOptions[iMk].RecentMessagesTab;
                    MDVSession.Current.DefaultTabMedications = objEntityUserOptions[iMk].DefaultTabMedications;
                    MDVSession.Current.IsNoteCompExpanded = objEntityUserOptions[iMk].IsNoteCompExpanded;
                    MDVSession.Current.IsAssignedResults = objEntityUserOptions[iMk].IsAssignedResults;
                }
            }
            else
            {
                MDVSession.Current.PrescriptionsRefillCount = "?";
                MDVSession.Current.PendingPrescriptionsCount = "?";
            }
            LoginUserSession(MDVSession.Current.AppUserId, MDVSession.Current.AppUserName, MDVSession.Current.SessionID);
            FormsAuthentication.SetAuthCookie(MDVSession.Current.AppUserName, false);
        }

        #region UserLoginHistory section


        public static LoggedInUserModel CheckUserState(long UserId, string sessionId)
        {
            try
            {
                BLObject<LoggedInUserModel> obj = new BLLAdminSecurity().LoggedInUserCheck(UserId, sessionId);
                if (obj.Data != null)
                {
                    return obj.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MDVLogger.PresentationErrorLog("CheckUserState", ex, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                return null;

            }
        }

        public void LoginUserSession(long AppUserId, string appUser, string sessionId)
        {
            try
            {
                bool IsSimultaneousLoginAllowed = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["IsSimultaneousLoginAllowed"]);
                if (!IsSimultaneousLoginAllowed)
                {
                    new BLLAdminSecurity().LoggedInUserInsert(AppUserId, sessionId);
                }
            }
            catch (Exception ex)
            {
                MDVLogger.PresentationErrorLog("LoginUserSession", ex, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
            }

        }

        public PreRequestModel AuthenticateUserRequest()
        {

            PreRequestModel model = new PreRequestModel();
            try
            {
                //1- Check User Session
                if (!MDVSession.Current.UserLoggedIn)
                {
                    if (!IsCookieExists())
                        model.RedirectSet = LoggedOutUserSession(AppPrivileges.SESSION_TIMEOUT_MESSAGE);
                    else
                        model.RedirectSet = LoggedOutUserSession(AppPrivileges.DEPLOYMENT_OR_SESSION_TIMEOUT_MESSAGE);

                } //2- Simultaneous Login
                else if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["IsSimultaneousLoginAllowed"]))
                {
                    LoggedInUserModel user_model = CheckUserState(MDVSession.Current.AppUserId, MDVUtility.ToStr(MDVSession.Current.SessionID));
                    if (user_model != null)
                    {
                        model.IsLogIn = user_model.IsLogedIn;
                        model.RedirectSet = !user_model.IsLogedIn
                                            ? LoggedOutUserSession(AppPrivileges.SESSION_CANCELLED_MESSAGE)
                                            : string.Empty;
                    }
                    else
                    {
                        model.IsLogIn = true;
                    }
                }
                else
                {
                    model.IsLogIn = true;
                }

            }
            catch (Exception ex)
            {
                MDVLogger.PresentationErrorLog("MDVisionHandler", ex, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
            }

            MDVSession.Current.RequestModel = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            return model;
        }

        private string LoggedOutUserSession(string Message)
        {
            string url_ = "";
            string message_ = GetMessage();
            url_ = Logout();
            if (!url_.ToLower().Contains("error="))
            {
                url_ = "MDVisionLogin.aspx?error=" + Message;
            }
            else
                url_ = "MDVisionLogin.aspx";

            url_ = !string.IsNullOrEmpty(message_) ? "MDVisionLogin.aspx?error=" + message_ : url_;

            if (url_ != "")
            {
                var response = new
                {
                    Redirect = "Redirect",
                    Url = url_
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(string.Empty);
            }
        }

        #endregion

    }
}
