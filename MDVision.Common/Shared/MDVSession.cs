using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace MDVision.Common.Shared
{
    public class MDVSession
    {
        private MDVSession()
        {
            //add default value

            AccountLockoutThreshold = -1;
        }

        #region current context
        private static HttpContext _currentContext
        {
            get
            {
                return HttpContext.Current;
            }
        }

        #endregion
        public static MDVSession Current
        {
            get
            {
                MDVSession session = (MDVSession)HttpContext.Current.Session["__MDVSession__"];
                if (session == null)
                {
                    session = new MDVSession();
                    HttpContext.Current.Session["__MDVSession__"] = session;
                }
                return session;
            }
        }

        #region Data Properties

        public string DateFormat
        {

            get { return MDVUtility.ToStr(_currentContext.Session["DateFormat"]); }
            set { _currentContext.Session["DateFormat"] = value; }
        }
        public string DocsHostName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DocsHostName"]); }
            set { _currentContext.Session["DocsHostName"] = value; }
        }
        public string DocsAlias
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DocsAlias"]); }
            set { _currentContext.Session["DocsAlias"] = value; }
        }
        public string FTPPortNo
        {
            get { return MDVUtility.ToStr(_currentContext.Session["FTPPortNo"]); }
            set { _currentContext.Session["FTPPortNo"] = value; }
        }
        public string IMO_ID
        {
            get {
                if (string.IsNullOrEmpty(Convert.ToString(_currentContext.Session["IMO_ID"])))
                {
                    return System.Web.Configuration.WebConfigurationManager.AppSettings["IMO_ID"];
                }

                return MDVUtility.ToStr(_currentContext.Session["IMO_ID"]); }
            set { _currentContext.Session["IMO_ID"] = value; }
        }
        public string IMOHostName
        {
            get {
                if (string.IsNullOrEmpty(Convert.ToString(_currentContext.Session["IMOHostName"])))
                {
                    return System.Web.Configuration.WebConfigurationManager.AppSettings["IMO_HostName"];
                }

                return MDVUtility.ToStr(_currentContext.Session["IMOHostName"]); }
            set { _currentContext.Session["IMOHostName"] = value; }
        }
        public string IMOCPTPort
        {
            get {
                if (string.IsNullOrEmpty(Convert.ToString(_currentContext.Session["IMOCPTPort"])))
                {
                    return System.Web.Configuration.WebConfigurationManager.AppSettings["IMO_CPTPort"];
                }

                return MDVUtility.ToStr(_currentContext.Session["IMOCPTPort"]); }
            set { _currentContext.Session["IMOCPTPort"] = value; }
        }
        public string IMOICDPort
        {
            get {
                if (string.IsNullOrEmpty(Convert.ToString(_currentContext.Session["IMOICDPort"])))
                {
                    return System.Web.Configuration.WebConfigurationManager.AppSettings["IMO_ICDPort"];
                }

                return MDVUtility.ToStr(_currentContext.Session["IMOICDPort"]); }
            set { _currentContext.Session["IMOICDPort"] = value; }
        }
        public string ImagePath
        {
            get { return MDVUtility.ToStr(_currentContext.Session["ImagePath"]); }
            set { _currentContext.Session["ImagePath"] = value; }
        }
        public DSProblemLists.ProblemListRow ProblemList4DrFirst
        {
            get { return (DSProblemLists.ProblemListRow)(_currentContext.Session["ProblemList4DrFirst"]); }
            set { _currentContext.Session["ProblemList4DrFirst"] = value; }
        }
        public object DeleteProblemList4DrFirst
        {
            get { return (_currentContext.Session["DeleteProblemList4DrFirst"]); }
            set { _currentContext.Session["DeleteProblemList4DrFirst"] = value; }
        }
        public DSProblemLists.ProblemListRow ProblemList4INActiveDrFirst
        {
            get { return (DSProblemLists.ProblemListRow)(_currentContext.Session["ProblemList4INActiveDrFirst"]); }
            set { _currentContext.Session["ProblemList4INActiveDrFirst"] = value; }
        }
        public DSProblemLists.ProblemListRow ProblemList4GridDrFirst
        {
            get { return (DSProblemLists.ProblemListRow)(_currentContext.Session["ProblemList4GridDrFirst"]); }
            set { _currentContext.Session["ProblemList4GridDrFirst"] = value; }
        }

        #region Session Level Properties
        public string SessionID
        {
            get { return _currentContext.Session.SessionID; }
            private set { }
        }
        public string UserHostIP
        {

            get { return MDVUtility.ToStr(_currentContext.Session["UserHostIP"]); }
            set { _currentContext.Session["UserHostIP"] = value; }
        }
        public long AppUserId
        {

            get {
                if (string.IsNullOrEmpty( Convert.ToString(_currentContext.Session["AppUserId"])))
                {
                    return 1;
                }
                return MDVUtility.ToLong(_currentContext.Session["AppUserId"]); }
            set { _currentContext.Session["AppUserId"] = value; }
        }
        public string AppUserName
        {
            get
            {
                //if (string.IsNullOrEmpty(Convert.ToString(_currentContext.Session["AppUserName"])))
                //{
                //    return "QklMQVdBTA==";
                //}

                return MDVUtility.ToStr(_currentContext.Session["AppUserName"]);
            }
            set { _currentContext.Session["AppUserName"] = value; }
        }
        public string AppUserFullName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["AppUserNameFullName"]); }
            set { _currentContext.Session["AppUserNameFullName"] = value; }
        }
        public string AppUserFirstName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["AppUserFirstName"]); }
            set { _currentContext.Session["AppUserFirstName"] = value; }
        }
        public string AppUserLastName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["AppUserLastName"]); }
            set { _currentContext.Session["AppUserLastName"] = value; }
        }
        public string AppPassWord
        {
            get {
                
                    //if (string.IsNullOrEmpty(Convert.ToString(_currentContext.Session["AppPassWord"])))
                    //{
                    //    return "566c27fd147ff47b4b8e3b4f44cec0a10bfb923fa5f4b51ea8f3d90ec70ba6f2";
                    //}

                    return MDVUtility.ToStr(_currentContext.Session["AppPassWord"]); }
            set { _currentContext.Session["AppPassWord"] = value; }
        }
        public string EntityId
        {
            get {
                if (string.IsNullOrEmpty(Convert.ToString(_currentContext.Session["EntityId"])))
                {
                    return "100";
                }

                return MDVUtility.ToStr(_currentContext.Session["EntityId"]); }
            set {  _currentContext.Session["EntityId"] = value; }
        }
        public string EntityRegCode
        {
            get { return MDVUtility.ToStr(_currentContext.Session["EntityRegCode"]); }
            set { _currentContext.Session["EntityRegCode"] = value; }
        }
        public string ClientId
        {
            get {
                //if (string.IsNullOrEmpty(Convert.ToString(_currentContext.Session["ClientId"])))
                //{
                //    return "7";
                //}
                return MDVUtility.ToStr(_currentContext.Session["ClientId"]); }
            set { _currentContext.Session["ClientId"] = value; }
        }
        public Boolean IsAdmin
        {
            get { return Convert.ToBoolean(_currentContext.Session["IsAdmin"]); }
            set { _currentContext.Session["IsAdmin"] = value; }
        }
        public string RequestModel
        {
            get { return Convert.ToString(_currentContext.Session["RequestModel"]); }
            set { _currentContext.Session["RequestModel"] = value; }
        }
        public string WebEntityURL
        {
            get { return MDVUtility.ToStr(string.Empty); }
            set { _currentContext.Session["WebEntityURL"] = value; }
        }
        public string ReportURL
        {
            get { return MDVUtility.ToStr(_currentContext.Session["ReportURL"]); }
            set { _currentContext.Session["ReportURL"] = value; }
        }
        public string DomainName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DomainName"]); }
            set { _currentContext.Session["DomainName"] = value; }
        }
        public string DomainUserName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DomainUserName"]); }
            set { _currentContext.Session["DomainUserName"] = value; }
        }
        public string DomainPassword
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DomainPassword"]); }
            set { _currentContext.Session["DomainPassword"] = value; }
        }
        public bool UserLoggedIn
        {
            get { return Convert.ToBoolean(_currentContext.Session["UserLoggedIn"]); }
            set { _currentContext.Session["UserLoggedIn"] = value; }
        }
        public DSSoftwareCustomersInfo dsCustomerInfo
        {
            get { return (DSSoftwareCustomersInfo)_currentContext.Session["dsCustomerInfo"]; }
            set { _currentContext.Session["dsCustomerInfo"] = value; }
        }
        public List<Model.Security.SoftwareCustomerInfoModel_> listCustomerInfo
        {
            get { return (List<Model.Security.SoftwareCustomerInfoModel_>)_currentContext.Session["listCustomerInfo"]; }
            set { _currentContext.Session["listCustomerInfo"] = value; }
        }
        public DSUsers.EntityUserOptionDataTable dtEntityUserOption
        {
            get { return (DSUsers.EntityUserOptionDataTable)_currentContext.Session["dtEntityUserOption"]; }
            set { _currentContext.Session["dtEntityUserOption"] = value; }
        }
        public List<Model.User.EntityUserOptions> ListEntityUserOption
        {
            get { return (List<Model.User.EntityUserOptions>)_currentContext.Session["ListEntityUserOption"]; }
            set { _currentContext.Session["ListEntityUserOption"] = value; }
        }

        public DSUsers.UsersPrivilegesDataTable dtUserPrivileges
        {
            get { return (DSUsers.UsersPrivilegesDataTable)_currentContext.Session["dtUserPrivileges"]; }
            set { _currentContext.Session["dtUserPrivileges"] = value; }
        }

        public List<Model.User.UserPrivileges> ListUserPrivileges
        {
            get { return (List<Model.User.UserPrivileges>)_currentContext.Session["ListUserPrivileges"]; }
            set { _currentContext.Session["ListUserPrivileges"] = value; }
        }
        public DSUsers.UsersDataTable dtUser
        {
            get { return (DSUsers.UsersDataTable)_currentContext.Session["dtUser"]; }
            set { _currentContext.Session["dtUser"] = value; }
        }

        public List<Model.User.UserLoginModel> ListUser
        {
            get { return (List<Model.User.UserLoginModel>)_currentContext.Session["ListUser"]; }
            set { _currentContext.Session["ListUser"] = value; }
        }
        public string DefaultProviderId
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultProviderId"]); }
            set { _currentContext.Session["DefaultProviderId"] = value; }
        }
        public string IsBulkSign
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsBulkSign"]); }
            set { _currentContext.Session["IsBulkSign"] = value; }
        }
        public string DefaultResourceId
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultResourceId"]); }
            set { _currentContext.Session["DefaultResourceId"] = value; }
        }
        public string DefaultProviderName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultProviderName"]); }
            set { _currentContext.Session["DefaultProviderName"] = value; }
        }
        public string DefaultBillingProviderId
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultBillingProviderId"]); }
            set { _currentContext.Session["DefaultBillingProviderId"] = value; }
        }
        public string DefaultFacilityId
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultFacilityId"]); }
            set { _currentContext.Session["DefaultFacilityId"] = value; }
        }
        public string DefaultFacilityName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultFacilityName"]); }
            set { _currentContext.Session["DefaultFacilityName"] = value; }
        }
        public string DefaultFacilityDescription
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultFacilityDescription"]); }
            set { _currentContext.Session["DefaultFacilityDescription"] = value; }
        }
        public string DefaultPracticeId
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultPracticeId"]); }
            set { _currentContext.Session["DefaultPracticeId"] = value; }
        }
        public string DefaultPracticeName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultPracticeName"]); }
            set { _currentContext.Session["DefaultPracticeName"] = value; }
        }
        public string MessagesCount
        {
            get { return MDVUtility.ToStr(_currentContext.Session["MessagesCount"]); }
            set { _currentContext.Session["MessagesCount"] = value; }
        }

        public string DocumentPendingCount
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DocumentPendingCount"]); }
            set { _currentContext.Session["DocumentPendingCount"] = value; }
        }
        public string UserTasksCount
        {
            get { return MDVUtility.ToStr(_currentContext.Session["UserTasksCount"]); }
            set { _currentContext.Session["UserTasksCount"] = value; }
        }
        public Int32 FileSize
        {
            get { return MDVUtility.ToInt32(_currentContext.Session["FileSize"]); }
            set { _currentContext.Session["FileSize"] = value; }
        }
        public string EntityUserOptionId
        {
            get { return MDVUtility.ToStr(_currentContext.Session["EntityUserOptionId"]); }
            set { _currentContext.Session["EntityUserOptionId"] = value; }
        }
        public string PreferredScreen
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PreferredScreen"]); }
            set { _currentContext.Session["PreferredScreen"] = value; }
        }
        public string PreferredSchScreen
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PreferredSchScreen"]); }
            set { _currentContext.Session["PreferredSchScreen"] = value; }
        }
        public string PreferredSchScreenName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PreferredSchScreenName"]); }
            set { _currentContext.Session["PreferredSchScreenName"] = value; }
        }
        public string PreferredAppointmentStatus
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PreferredAppointmentStatus"]); }
            set { _currentContext.Session["PreferredAppointmentStatus"] = value; }
        }
        public string IsImmunizationAlert
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsImmunizationAlert"]); }
            set { _currentContext.Session["IsImmunizationAlert"] = value; }
        }
        public string IsDocumentsAlert
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsDocumentsAlert"]); }
            set { _currentContext.Session["IsDocumentsAlert"] = value; }
        }
        public string DefaultTemplate
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultTemplate"]); }
            set { _currentContext.Session["DefaultTemplate"] = value; }
        }
        public string DefaultSuperBill
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultSuperBill"]); }
            set { _currentContext.Session["DefaultSuperBill"] = value; }
        }
        public string PBPhoneNumber1
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PBPhoneNumber1"]); }
            set { _currentContext.Session["PBPhoneNumber1"] = value; }
        }
        public string PBPhoneNumber2
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PBPhoneNumber2"]); }
            set { _currentContext.Session["PBPhoneNumber2"] = value; }
        }
        public string ClaimPrinter
        {
            get { return MDVUtility.ToStr(_currentContext.Session["ClaimPrinter"]); }
            set { _currentContext.Session["ClaimPrinter"] = value; }
        }
        public string ClaimTray
        {
            get { return MDVUtility.ToStr(_currentContext.Session["ClaimTray"]); }
            set { _currentContext.Session["ClaimTray"] = value; }
        }
        public string AttachmentPrinter
        {
            get { return MDVUtility.ToStr(_currentContext.Session["AttachmentPrinter"]); }
            set { _currentContext.Session["AttachmentPrinter"] = value; }
        }
        public string AttachmentTray
        {
            get { return MDVUtility.ToStr(_currentContext.Session["AttachmentTray"]); }
            set { _currentContext.Session["AttachmentTray"] = value; }
        }
        public string RefreshTime
        {
            get { return MDVUtility.ToStr(_currentContext.Session["RefreshTime"]); }
            set { _currentContext.Session["RefreshTime"] = value; }
        }
        public bool isEMR
        {
            get { return Convert.ToBoolean(_currentContext.Session["isEMR"]); }
            set { _currentContext.Session["isEMR"] = value; }
        }
        public bool isDirectAddress
        {
            get { return Convert.ToBoolean(_currentContext.Session["IsDirectAddress"]); }
            set { _currentContext.Session["IsDirectAddress"] = value; }
        }
        public DSIMO dsImo
        {
            get { return (DSIMO)_currentContext.Session["dsImo"]; }
            set { _currentContext.Session["dsImo"] = value; }
        }
        public string DefaultThemeName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultThemeName"]); }
            set { _currentContext.Session["DefaultThemeName"] = value; }
        }
        public string DefaultThemeId
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultThemeId"]); }
            set { _currentContext.Session["DefaultThemeId"] = value; }
        }
        public string DefaultCurrency
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultCurrency"]); }
            set { _currentContext.Session["DefaultCurrency"] = value; }
        }
        public string DecimalPlaces
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DecimalPlaces"]); }
            set { _currentContext.Session["DecimalPlaces"] = value; }
        }
        public string AppointmentsCount
        {
            get { return MDVUtility.ToStr(_currentContext.Session["AppointmentsCount"]); }
            set { _currentContext.Session["AppointmentsCount"] = value; }
        }
        public string PendingDocumentsCount
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PendingDocumentsCount"]); }
            set { _currentContext.Session["PendingDocumentsCount"] = value; }
        }
        public string AssignedResultsCount
        {
            get { return MDVUtility.ToStr(_currentContext.Session["AssignedResultsCount"]); }
            set { _currentContext.Session["AssignedResultsCount"] = value; }
        }
        public string NotesCount
        {
            get { return MDVUtility.ToStr(_currentContext.Session["NotesCount"]); }
            set { _currentContext.Session["NotesCount"] = value; }
        }

        public string IsSearchPatient
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsSearchPatient"]); }
            set { _currentContext.Session["IsSearchPatient"] = value; }
        }
        public string IsQuickAddPatient
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsQuickAddPatient"]); }
            set { _currentContext.Session["IsQuickAddPatient"] = value; }
        }
        public string IsAssignedResults
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsAssignedResults"]); }
            set { _currentContext.Session["IsAssignedResults"] = value; }
        }
        public string IsAppointment
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsAppointment"]); }
            set { _currentContext.Session["IsAppointment"] = value; }
        }
        public string IsNote
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsNote"]); }
            set { _currentContext.Session["IsNote"] = value; }
        }
        public string IsTask
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsTask"]); }
            set { _currentContext.Session["IsTask"] = value; }
        }
        public string IsFax
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsFax"]); }
            set { _currentContext.Session["IsFax"] = value; }
        }
        public string IsMessage
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsMessage"]); }
            set { _currentContext.Session["IsMessage"] = value; }
        }
        public string IsPrescriptionsRefill
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsPrescriptionsRefill"]); }
            set { _currentContext.Session["IsPrescriptionsRefill"] = value; }
        }
        public string IsPendingPrescriptions
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsPendingPrescriptions"]); }
            set { _currentContext.Session["IsPendingPrescriptions"] = value; }
        }
        public string PrescriptionsRefillCount
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PrescriptionsRefillCount"]); }
            set { _currentContext.Session["PrescriptionsRefillCount"] = value; }
        }

        public string PendingPrescriptionsCount
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PendingPrescriptionsCount"]); }
            set { _currentContext.Session["PendingPrescriptionsCount"] = value; }
        }
        public string SessionTimout
        {
            get { return MDVUtility.ToStr(_currentContext.Session["SessionTimout"]); }
            set { _currentContext.Session["SessionTimout"] = value; }
        }

        public string ClaimScrubberSubmitterID
        {
            get { return MDVUtility.ToStr(_currentContext.Session["ClaimScrubberSubmitterID"]); }
            set { _currentContext.Session["ClaimScrubberSubmitterID"] = value; }
        }
        public string ClaimScrubberUser
        {
            get { return MDVUtility.ToStr(_currentContext.Session["ClaimScrubberUser"]); }
            set { _currentContext.Session["ClaimScrubberUser"] = value; }
        }
        public string ClaimScrubberPassword
        {
            get { return MDVUtility.ToStr(_currentContext.Session["ClaimScrubberPassword"]); }
            set { _currentContext.Session["ClaimScrubberPassword"] = value; }
        }
        public string ClaimScrubberEDIServer
        {
            get { return MDVUtility.ToStr(_currentContext.Session["ClaimScrubberEDIServer"]); }
            set { _currentContext.Session["ClaimScrubberEDIServer"] = value; }
        }

        public bool IsEmergencyAccess
        {
            get { return Convert.ToBoolean(_currentContext.Session["IsEmergencyAccess"]); }
            set { _currentContext.Session["IsEmergencyAccess"] = value; }
        }
        public string ENMCodesTime
        {
            get { return MDVUtility.ToStr(_currentContext.Session["ENMCodesTime"]); }
            set { _currentContext.Session["ENMCodesTime"] = value; }
        }

        public string isPETemplateNameRequired
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isPETemplateNameRequired"]); }
            set { _currentContext.Session["isPETemplateNameRequired"] = value; }
        }
        public string IsDocument
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsDocument"]); }
            set { _currentContext.Session["IsDocument"] = value; }
        }
        public string isDemographics
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isDemographics"]); }
            set { _currentContext.Session["isDemographics"] = value; }
        }

        public string isMU3FamilyHistory
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isMU3FamilyHistory"]); }
            set { _currentContext.Session["isMU3FamilyHistory"] = value; }
        }
        public string isTransPubHealthAgHealthCareSurveys

        {
            get { return MDVUtility.ToStr(_currentContext.Session["isTransPubHealthAgHealthCareSurveys"]); }
            set { _currentContext.Session["isTransPubHealthAgHealthCareSurveys"] = value; }
        }
        public string isTransmittoImmunizationRegistries

        {
            get { return MDVUtility.ToStr(_currentContext.Session["isTransmittoImmunizationRegistries"]); }
            set { _currentContext.Session["isTransmittoImmunizationRegistries"] = value; }
        }
        public string isPatientHealthInformationCapture
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isPatientHealthInformationCapture"]); }
            set { _currentContext.Session["isPatientHealthInformationCapture"] = value; }
        }
        public string isTransPubHealthAgCaseReporting

        {
            get { return MDVUtility.ToStr(_currentContext.Session["isTransPubHealthAgCaseReporting"]); }
            set { _currentContext.Session["isTransPubHealthAgCaseReporting"] = value; }
        }
        public string isTransitionCareDirectProject
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsTransitionCareDirectProject"]); }
            set { _currentContext.Session["IsTransitionCareDirectProject"] = value; }
        }
        public string isDataSegmentationPrivacy

        {
            get { return MDVUtility.ToStr(_currentContext.Session["isDataSegmentationPrivacy"]); }
            set { _currentContext.Session["isDataSegmentationPrivacy"] = value; }
        }
        public string isImplantableDevices
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isImplantableDevices"]); }
            set { _currentContext.Session["isImplantableDevices"] = value; }
        }
        public string isTransitonCancerRegistries
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isTransitonCancerRegistries"]); }
            set { _currentContext.Session["isTransitonCancerRegistries"] = value; }
        }
        public string isConsolidatedCDACreationPreformance
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isConsolidatedCDACreationPreformance"]); }
            set { _currentContext.Session["isConsolidatedCDACreationPreformance"] = value; }
        }
        public string isTransPubHealthAgAntimicobialUse
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isTransPubHealthAgAntimicobialUse"]); }
            set { _currentContext.Session["isTransPubHealthAgAntimicobialUse"] = value; }
        }
        public string isMU3SocPsycBehaviourHx
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isMU3SocPsycBehaviourHx"]); }
            set { _currentContext.Session["isMU3SocPsycBehaviourHx"] = value; }
        }
        public string isDataExport
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isDataExport"]); }
            set { _currentContext.Session["isDataExport"] = value; }
        }
        public string isCarePlan
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isCarePlan"]); }
            set { _currentContext.Session["isCarePlan"] = value; }
        }
        public string IsCMS65v7
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsCMS65v7"]); }
            set { _currentContext.Session["IsCMS65v7"] = value; }
        }


        public string IsCMS69v6
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsCMS69v6"]); }
            set { _currentContext.Session["IsCMS69v6"] = value; }
        }


        public string IsCMS68v7
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsCMS68v7"]); }
            set { _currentContext.Session["IsCMS68v7"] = value; }
        }


        public string IsCMS138v6
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsCMS138v6"]); }
            set { _currentContext.Session["IsCMS138v6"] = value; }
        }


        public string IsCMS165v6
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsCMS165v6"]); }
            set { _currentContext.Session["IsCMS165v6"] = value; }
        }


        public string IsCMS22v6
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsCMS22v6"]); }
            set { _currentContext.Session["IsCMS22v6"] = value; }
        }
        public string IsCDS
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsCDS"]); }
            set { _currentContext.Session["IsCDS"] = value; }
        }
        public string DefaultTabMessages
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultTabMessages"]); }
            set { _currentContext.Session["DefaultTabMessages"] = value; }
        }
        public string RecentMessagesTab
        {
            get { return MDVUtility.ToStr(_currentContext.Session["RecentMessagesTab"]); }
            set { _currentContext.Session["RecentMessagesTab"] = value; }
        }

        /// <summary>
        /// if regex is not defined for an entity a default regex will be returned
        /// </summary>
        public string PasswordRegex
        {
            get
            {
                if (MDVUtility.ToStr(_currentContext.Session["PasswordRegex"]) != "")
                {
                    return MDVUtility.ToStr(_currentContext.Session["PasswordRegex"]);
                }
                else
                {
                    return "(?=(.*\\d){1})(?=(.*[a-z]){1})(?=(.*[A-Z]){1})(?=(.*\\W){1}).{8,50}";
                }
            }
            set { _currentContext.Session["PasswordRegex"] = value; }
        }

        public int DefaultEntity
        {
            get { return MDVUtility.ToInt32(_currentContext.Session["DefaultEntity"]); }
            set { _currentContext.Session["DefaultEntity"] = value; }
        }
        public int AccountLockoutThreshold
        {
            get { return MDVUtility.ToInt32(_currentContext.Session["AccountLockoutThreshold"]); }
            set { _currentContext.Session["AccountLockoutThreshold"] = value; }
        }

        public int LoginAttemptsCount
        {
            get { return MDVUtility.ToInt32(_currentContext.Session["LoginAttemptsCount"]); }
            set { _currentContext.Session["LoginAttemptsCount"] = value; }
        }

        public string PBPreferredPhone
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PBPreferredPhone"]); }
            set { _currentContext.Session["PBPreferredPhone"] = value; }
        }
        public string PBPCP
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PBPCP"]); }
            set { _currentContext.Session["PBPCP"] = value; }
        }
        public string PBRefProvider
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PBRefProvider"]); }
            set { _currentContext.Session["PBRefProvider"] = value; }
        }
        public string PBPlanBalance
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PBPlanBalance"]); }
            set { _currentContext.Session["PBPlanBalance"] = value; }
        }
        public string PBPatientBalance
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PBPatientBalance"]); }
            set { _currentContext.Session["PBPatientBalance"] = value; }
        }
        public string PBPatientAdvanceBalance
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PBPatientAdvanceBalance"]); }
            set { _currentContext.Session["PBPatientAdvanceBalance"] = value; }
        }

        public string PBPrimaryInsurance
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PBPrimaryInsurance"]); }
            set { _currentContext.Session["PBPrimaryInsurance"] = value; }
        }
        public string NoteFontSize
        {
            get { return MDVUtility.ToStr(_currentContext.Session["NoteFontSize"]); }
            set { _currentContext.Session["NoteFontSize"] = value; }
        }

        public string FavListNames
        {
            get { return MDVUtility.ToStr(_currentContext.Session["FavListNames"]); }
            set { _currentContext.Session["FavListNames"] = value; }
        }
        public string FreeTextNames
        {
            get { return MDVUtility.ToStr(_currentContext.Session["FreeTextNames"]); }
            set { _currentContext.Session["FreeTextNames"] = value; }
        }
        public string IsDefaultHPI
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsDefaultHPI"]); }
            set { _currentContext.Session["IsDefaultHPI"] = value; }
        }
        public string EMCodeTypeIds
        {
            get { return MDVUtility.ToStr(_currentContext.Session["EMCodeTypeIds"]); }
            set { _currentContext.Session["EMCodeTypeIds"] = value; }
        }
        public string RaceIds
        {
            get { return MDVUtility.ToStr(_currentContext.Session["RaceIds"]); }
            set { _currentContext.Session["RaceIds"] = value; }
        }
        public string IsShowFacilityShortName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsShowFacilityShortName"]); }
            set { _currentContext.Session["IsShowFacilityShortName"] = value; }
        }
        public string IsShowSuccessMessages
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsShowSuccessMessages"]); }
            set { _currentContext.Session["IsShowSuccessMessages"] = value; }
        }
        public string IsReferralRequired
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsReferralRequired"]); }
            set { _currentContext.Session["IsReferralRequired"] = value; }
        }
        #region Network Security


        //public string CientLocalIP()
        //{
        //    get
        //    {

        //        return GetVisitorIPAddress();
        //    }

        //}

        public void GetVisitorIPAddress(ref string ip, ref string hostName, ref bool IsLocal)
        {

            ip = _currentContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = _currentContext.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(ip))
                ip = _currentContext.Request.UserHostAddress;

            if (string.IsNullOrEmpty(ip) || ip.Trim() == "::1")
            {
                IsLocal = true;
                ip = string.Empty;
            }

            if (IsLocal && string.IsNullOrEmpty(ip))
            {
                //This is for Local(LAN) Connected ID Address
                hostName = Dns.GetHostName();
                //Get Ip Host Entry
                IPHostEntry ipHostEntries = Dns.GetHostEntry(hostName);
                //Get Ip Address From The Ip Host Entry Address List
                IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                try
                {
                    ip = arrIpAddress[1].ToString();
                }
                catch
                {
                    try
                    {
                        ip = arrIpAddress[0].ToString();
                    }
                    catch
                    {
                        try
                        {
                            arrIpAddress = Dns.GetHostAddresses(hostName);
                            ip = arrIpAddress[0].ToString();
                        }
                        catch
                        {
                            ip = "127.0.0.1";
                        }
                    }
                }
            }

            // visitorIPAddress;
        }

        #endregion

        //public string PBPhone1
        //{
        //    get { return MDVUtility.ToStr(_currentContext.Session["PBPhone1"]); }
        //    set { _currentContext.Session["PBPhone1"] = value; }
        //}
        //public string PBPhone2
        //{
        //    get { return MDVUtility.ToStr(_currentContext.Session["PBPhone2"]); }
        //    set { _currentContext.Session["PBPhone2"] = value; }
        //}

        public string PreferredScreenName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PreferredScreenName"]); }
            set { _currentContext.Session["PreferredScreenName"] = value; }
        }

        public string IsSelectNoteComponent
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsSelectNoteComponent"]); }
            set { _currentContext.Session["IsSelectNoteComponent"] = value; }
        }
        public string DefaultTabMedications
        {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultTabMedications"]); }
            set { _currentContext.Session["DefaultTabMedications"] = value; }
        }
        public string isLandOnComponent
        {
            get { return MDVUtility.ToStr(_currentContext.Session["isLandOnComponent"]); }
            set { _currentContext.Session["isLandOnComponent"] = value; }
        }
        public string IsExpand
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsExpand"]); }
            set { _currentContext.Session["IsExpand"] = value; }
        }
        public string WeekWorkDaysIds
        {
            get { return MDVUtility.ToStr(_currentContext.Session["WeekWorkDaysIds"]); }
            set { _currentContext.Session["WeekWorkDaysIds"] = value; }
        }

        public string IsSearchCriteriaExpand
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsSearchCriteriaExpand"]); }
            set { _currentContext.Session["IsSearchCriteriaExpand"] = value; }
        }

        public string NotePrevieStyle
        {
            get { return MDVUtility.ToStr(_currentContext.Session["NotePrevieStyle"]); }
            set { _currentContext.Session["NotePrevieStyle"] = value; }
        }

        public string OCRLicenseKey
        {
            get { return MDVUtility.ToStr(_currentContext.Session["OCRLicenseKey"]); }
            set { _currentContext.Session["OCRLicenseKey"] = value; }
        }

        public string PreferredBillingScreen
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PreferredBillingScreen"]); }
            set { _currentContext.Session["PreferredBillingScreen"] = value; }
        }

        public string PreferredBillingScreenName
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PreferredBillingScreenName"]); }
            set { _currentContext.Session["PreferredBillingScreenName"] = value; }
        }
        public string IsFullSSN
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsFullSSN"]); }
            set { _currentContext.Session["IsFullSSN"] = value; }
        }
        public string IsMedText
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsMedText"]); }
            set { _currentContext.Session["IsMedText"] = value; }
        }
        public string IsCollection
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsCollection"]); }
            set { _currentContext.Session["IsCollection"] = value; }
        }

        public string IsPBCollection
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsPBCollection"]); }
            set { _currentContext.Session["IsPBCollection"] = value; }
        }
        public string ImageID
        {
            get { return MDVUtility.ToStr(_currentContext.Session["ImageID"]); }
            set { _currentContext.Session["ImageID"] = value; }
        }
        public string DefaultDocumentPriorityId {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultDocumentPriorityId"]); }
            set { _currentContext.Session["DefaultDocumentPriorityId"] = value; }
        }
        public string DefaultDocumentPriorityName {
            get { return MDVUtility.ToStr(_currentContext.Session["DefaultDocumentPriorityName"]); }
            set { _currentContext.Session["DefaultDocumentPriorityName"] = value; }
        }
        public int DefaultSchedulerTimeInterval
        {
            get { return MDVUtility.ToInt32(_currentContext.Session["DefaultSchedulerTimeInterval"]); }
            set { _currentContext.Session["DefaultSchedulerTimeInterval"] = value; }
        }
        
        public bool sendBillingInquiry
        {
            get { return Convert.ToBoolean(_currentContext.Session["sendBillingInquiry"]); }
            set { _currentContext.Session["sendBillingInquiry"] = value; }
        }
        public string IsOrdersExpand
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsOrdersExpand"]); }
            set { _currentContext.Session["IsOrdersExpand"] = value; }
        }
        public string IsResultsExpand
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsResultsExpand"]); }
            set { _currentContext.Session["IsResultsExpand"] = value; }
        }
        public string IsSelectCompOnCopyNote
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsSelectCompOnCopyNote"]); }
            set { _currentContext.Session["IsSelectCompOnCopyNote"] = value; }
        }
        public string IsPreviousNoteComplaints
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsPreviousNoteComplaints"]); }
            set { _currentContext.Session["IsPreviousNoteComplaints"] = value; }
        }
        public string IsPreviousNoteROS
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsPreviousNoteROS"]); }
            set { _currentContext.Session["IsPreviousNoteROS"] = value; }
        }
        public string IsPreviousNotePE
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsPreviousNotePE"]); }
            set { _currentContext.Session["IsPreviousNotePE"] = value; }
        }
        //PRD-31 by:MAHMAD
        public string IsExpandFolderTree
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsExpandFolderTree"]); }
            set { _currentContext.Session["IsExpandFolderTree"] = value; }
        }
        //PRD-31 by:MAHMAD
        public string IsConfigureAlerts
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsConfigureAlerts"]); }
            set { _currentContext.Session["IsConfigureAlerts"] = value; }
        }
        public string iTrackDashboardIds
        {
            get { return MDVUtility.ToStr(_currentContext.Session["iTrackDashboardIds"]); }
            set { _currentContext.Session["iTrackDashboardIds"] = value; }
        }

        public string IsPreviousNoteProblems
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsPreviousNoteProblems"]); }
            set { _currentContext.Session["IsPreviousNoteProblems"] = value; }
        }
        public string PBPatientCCMTimer
        {
            get { return MDVUtility.ToStr(_currentContext.Session["PBPatientCCMTimer"]); }
            set { _currentContext.Session["PBPatientCCMTimer"] = value; }
        }
        public string IsPrevNoteTreatmentComents
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsPrevNoteTreatmentComents"]); }
            set { _currentContext.Session["IsPrevNoteTreatmentComents"] = value; }
        }
        public string IsNoteCompExpanded
        {
            get { return MDVUtility.ToStr(_currentContext.Session["IsNoteCompExpanded"]); }
            set { _currentContext.Session["IsNoteCompExpanded"] = value; }
        }
        #endregion

        #endregion

        #region Support Functions


        /// <summary>
        /// This method clears the application data which has been set on load of application.
        /// </summary>
        public void ClearAppData()
        {
            PreferredScreenName = "";
            OCRLicenseKey = "";
            //PBPhone1 = "";
            //PBPhone2 = "";
            IsSelectNoteComponent = "";
            IsExpand = "";
            WeekWorkDaysIds = "";
            IsSearchCriteriaExpand = "";
            NotePrevieStyle = "";
            FavListNames = "";
            FreeTextNames = "";
            PBPatientAdvanceBalance = "";
            PBPreferredPhone = "";
            PBPrimaryInsurance = "";
            EntityUserOptionId = "";
            AppUserName = "";
            AppPassWord = "";
            EntityId = "";
            EntityRegCode = "";
            WebEntityURL = "";
            UserLoggedIn = false;
            dsCustomerInfo = null;
            //listCustomerInfo = null;
            //dtEntityUserOption = null;
            //dtUserPrivileges = null;
            //dtUser = null;
            //ListEntityUserOption = null;
            //ListUserPrivileges = null;
            //ListUser = null;
            ReportURL = "";
            DomainName = "";
            DomainUserName = "";
            DomainPassword = "";
            ClientId = "";
            AppUserId = -1;
            IsAdmin = false;
            DefaultProviderId = "";
            DefaultProviderName = "";
            DefaultBillingProviderId = "";
            DefaultFacilityId = "";
            DefaultFacilityName = "";
            DefaultFacilityDescription = "";
            DefaultPracticeId = "";
            DefaultPracticeName = "";
            ImagePath = "";
            MessagesCount = "";
            UserTasksCount = "";
            UserHostIP = "";
            DateFormat = "";
            DocsHostName = "";
            DocsAlias = "";
            FTPPortNo = "";
            IMOHostName = "";
            IMOCPTPort = "";
            IMOICDPort = "";
            IMO_ID = "";
            PreferredScreen = "";
            PreferredSchScreen = "";
            DefaultSuperBill = "";
            PBPhoneNumber1 = "";
            PBPhoneNumber2 = "";
            PBPCP = "";
            PBRefProvider = "";
            PBPlanBalance = "";
            PBPatientBalance = "";
            ClaimPrinter = "";
            ClaimTray = "";
            AttachmentPrinter = "";
            AttachmentTray = "";
            PreferredAppointmentStatus = "";
            RefreshTime = "";
            SessionTimout = "30";
            dsImo = null;
            DefaultThemeName = "";
            DefaultCurrency = "";
            DecimalPlaces = "";
            NotesCount = "";
            IsSearchPatient = "";
            IsQuickAddPatient = "";
            IsAppointment = "";
            IsNote = "";
            IsTask = "";
            IsMessage = "";
            IsPrescriptionsRefill = "";
            IsPendingPrescriptions = "";
            ClaimScrubberUser = "";
            ClaimScrubberPassword = "";
            ClaimScrubberSubmitterID = "";
            ClaimScrubberEDIServer = "";
            PasswordRegex = "";
            PreferredSchScreenName = "";
            DefaultTemplate = "";
            FileSize = 0;
            PreferredBillingScreen = "";
            PreferredBillingScreenName = "";
            IsFullSSN = "";
            IsMedText = "";
            IsCollection = "";
            IsPBCollection = "";
            DefaultDocumentPriorityId = "";
            DefaultDocumentPriorityName = "";
            IsDocument = "";
            isPETemplateNameRequired = "";
            DocumentPendingCount = "";
            IsSelectCompOnCopyNote = "";
            //PRD-31 by:MAHMAD
            IsExpandFolderTree = "";
            //PRD-31 by:MAHMAD
            iTrackDashboardIds = "";
            PBPatientCCMTimer = "";
            IsAssignedResults = "";
        }
        public void ClearSession()
        {
            _currentContext.Session.Clear();
        }
        #endregion
    }
}


