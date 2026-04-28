using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.Common.Logging;
using System.Configuration;

namespace MDVision.IEHR.Controls.CommonControls
{
    public partial class DocumentScan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {

                try
                {
                    if (MDVSession.Current.UserLoggedIn)
                    {

                        int timeOut = MDVUtility.ToInt32(MDVSession.Current.SessionTimout);
                        timeOut = (timeOut > 30 || timeOut <= 0) ? 30 : timeOut;
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
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultProviderName','" + MDVSession.Current.DefaultProviderName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultFacilityName','" + MDVSession.Current.DefaultFacilityName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultFacilityDescription','" + MDVSession.Current.DefaultFacilityDescription + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultPracticeName','" + MDVSession.Current.DefaultPracticeName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('EntityUserOptionId','" + MDVSession.Current.EntityUserOptionId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('AppUserId','" + MDVSession.Current.AppUserId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('UserMessagesCount','" + MDVSession.Current.MessagesCount + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DocumentPendingCount','" + MDVSession.Current.DocumentPendingCount + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IMO_ID','" + MDVSession.Current.IMO_ID + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('OCRLicenseKey','" + MDVSession.Current.OCRLicenseKey + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DateFormat','" + MDVSession.Current.DateFormat + "');");
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
                        JsFuncs.RunJS(Page, "SetGlobalAppData('WeekWorkDaysIds','" + MDVSession.Current.WeekWorkDaysIds + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsSearchCriteriaExpand','" + MDVSession.Current.IsSearchCriteriaExpand + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('NotePrevieStyle','" + MDVSession.Current.NotePrevieStyle + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PreferredBillingScreenName','" + MDVSession.Current.PreferredBillingScreenName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('RaceIds','" + MDVSession.Current.RaceIds + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsShowFacilityShortName','" + MDVSession.Current.IsShowFacilityShortName + "');");
                        if (MDVSession.Current.AppUserName == "MDVISION")
                        {
                            JsFuncs.RunJS(Page, "SetGlobalAppData('SessionTimout','" + 30 + "');");
                        }
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsSearchPatient','" + MDVSession.Current.IsSearchPatient + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsQuickAddPatient','" + MDVSession.Current.IsQuickAddPatient + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsPETemplateNameRequired','" + MDVSession.Current.isPETemplateNameRequired + "');");
                        JsFuncs.RunJS(this.Page, "SetGlobalAppData('IsDocument','" + MDVSession.Current.IsDocument + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsAssignedResults','" + MDVSession.Current.IsAssignedResults + "');");
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
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBPCP','" + MDVSession.Current.PBPCP + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBRefProvider','" + MDVSession.Current.PBRefProvider + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBPlanBalance','" + MDVSession.Current.PBPlanBalance + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBPatientBalance','" + MDVSession.Current.PBPatientBalance + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsPBCollection','" + MDVSession.Current.IsPBCollection + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PasswordRegex',/" + MDVSession.Current.PasswordRegex + "/);");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('EntityRegCode','" + MDVSession.Current.EntityRegCode + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('PBPreferredPhone','" + MDVSession.Current.PBPreferredPhone + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultSuperBill','" + MDVSession.Current.DefaultSuperBill + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultTemplate','" + MDVSession.Current.DefaultTemplate + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('ENMCodesTime','" + MDVSession.Current.ENMCodesTime + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('NoteFontSize','" + MDVSession.Current.NoteFontSize + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsDefaultHPI','" + MDVSession.Current.IsDefaultHPI + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('EMCodeTypeIds','" + MDVSession.Current.EMCodeTypeIds + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsFullSSN','" + MDVSession.Current.IsFullSSN + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsMedText','" + MDVSession.Current.IsMedText + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsCollection','" + MDVSession.Current.IsCollection + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultDocumentPriorityId','" + MDVSession.Current.DefaultDocumentPriorityId + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('DefaultDocumentPriorityName','" + MDVSession.Current.DefaultDocumentPriorityName + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('SchedulerTimeInterval','" + MDVSession.Current.DefaultSchedulerTimeInterval + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('sendBillingInquiry','" + MDVSession.Current.sendBillingInquiry + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsShowSuccessMessages','" + MDVSession.Current.IsShowSuccessMessages + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsOrdersExpand','" + MDVSession.Current.IsOrdersExpand + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsResultsExpand','" + MDVSession.Current.IsResultsExpand + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsReferralRequired','" + MDVSession.Current.IsReferralRequired + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsReferralRequired','" + MDVSession.Current.IsReferralRequired + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isDemographics','" + MDVSession.Current.isDemographics + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isMU3FamilyHistory','" + MDVSession.Current.isMU3FamilyHistory + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isTransPubHealthAgHealthCareSurveys','" + MDVSession.Current.isTransPubHealthAgHealthCareSurveys + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isTransmittoImmunizationRegistries','" + MDVSession.Current.isTransmittoImmunizationRegistries + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isPatientHealthInformationCapture','" + MDVSession.Current.isPatientHealthInformationCapture + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isTransPubHealthAgCaseReporting','" + MDVSession.Current.isTransPubHealthAgCaseReporting + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isTransitionCareDirectProject','" + MDVSession.Current.isTransitionCareDirectProject + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isImplantableDevices','" + MDVSession.Current.isImplantableDevices + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isTransitonCancerRegistries','" + MDVSession.Current.isTransitonCancerRegistries + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isDataSegmentationPrivacy','" + MDVSession.Current.isDataSegmentationPrivacy + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isConsolidatedCDACreationPreformance','" + MDVSession.Current.isConsolidatedCDACreationPreformance + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isTransPubHealthAgAntimicobialUse','" + MDVSession.Current.isTransPubHealthAgAntimicobialUse + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isMU3SocPsycBehaviourHx','" + MDVSession.Current.isMU3SocPsycBehaviourHx + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isDataExport','" + MDVSession.Current.isDataExport + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('isCarePlan','" + MDVSession.Current.isCarePlan + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('iTrackDashboardIds','" + MDVSession.Current.iTrackDashboardIds + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsCMS65v7','" + MDVSession.Current.IsCMS65v7 + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsCMS69v6','" + MDVSession.Current.IsCMS69v6 + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsCMS68v7','" + MDVSession.Current.IsCMS68v7 + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsCMS138v6','" + MDVSession.Current.IsCMS138v6 + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsCMS165v6','" + MDVSession.Current.IsCMS165v6 + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsCMS22v6','" + MDVSession.Current.IsCMS22v6 + "');");
                        JsFuncs.RunJS(Page, "SetGlobalAppData('IsCDS','" + MDVSession.Current.IsCDS + "');");

                        var isEmergencyRole = false;
                        if (MDVSession.Current.ListUserPrivileges.Count > 0)
                        {
                            isEmergencyRole = MDVSession.Current.ListUserPrivileges[0].IsEmergencyRole != null && Convert.ToBoolean(MDVUtility.ToLong(MDVSession.Current.ListUserPrivileges[0].IsEmergencyRole));
                            JsFuncs.RunJS(Page, "SetGlobalAppData('IsEmergencyRole','" + isEmergencyRole + "');");

                        }
                    }
                }
                catch (Exception ex)
                {
                    MDVLogger.PresentationErrorLog("pageLoad", ex, "Arsal:p");
                    Response.Redirect(MDVSession.Current.WebEntityURL + "DocumentScan.aspx?error=" + ex.Message, false);
                    // System.Web.Security.FormsAuthentication.RedirectToLoginPage("error=" + ex.Message);
                }
            }
        }
    }
}