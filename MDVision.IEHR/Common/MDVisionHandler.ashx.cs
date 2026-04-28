using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.SessionState;
using System.Web;
using System.IO;
using System.Threading;
using MDVision.Business.BCommon;
using System.Data.SqlClient;
using System.Web.Providers.Entities;
using MDVision.Datasets;
using System.Data;
using System.Web.Configuration;
using System.Net.NetworkInformation;
using System.Net;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.IEHR.Security;
using MDVision.IEHR.Controls.CommonControls;
using MDVision.IEHR.Controls.Billing;
using MDVision.IEHR.Controls.Admin;
using Newtonsoft.Json;
using MDVision.IEHR.Controls.Live_Requests.MobileAppRequest;

namespace MDVision.IEHR.Common
{
    /// <summary>
    /// Summary description for MDVisionHandler
    /// </summary>
    public class MDVisionHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            ApplicationServerContent(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region IHttpAsyncHandler Members

        private AsyncProcessorDelegate _Delegate;
        protected delegate void AsyncProcessorDelegate(HttpContext context);

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            _Delegate = new AsyncProcessorDelegate(ProcessRequest);
            return _Delegate.BeginInvoke(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            _Delegate.EndInvoke(result);
        }

        #endregion

        #region Support Functions
        private void ApplicationServerContent(HttpContext context)
        {
            try
            {
                if (MDVSession.Current.UserLoggedIn == false && (context.Request.QueryString["controlName"].Equals("USER_RELOGIN") && (context.Request.QueryString["cammandAction"].Equals("USER_SESSION_UNLOCK") || context.Request.QueryString["cammandAction"].Equals("USER_SESSION_RESET_RELOGIN")) == false))
                {
                    string error = "";
                    error = new UserLoginHelper().LogIn_();
                    if (error != "")
                    {
                        string url = MDVSession.Current.WebEntityURL + "MDVisionLogin.aspx?error=" + error;
                        var response = new
                        {
                            Redirect = true,
                            Url = url
                        };
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(JsonConvert.SerializeObject(response));
                    }
                }
                else
                {

                    PreRequestModel  model = JsonConvert.DeserializeObject<PreRequestModel>(MDVSession.Current.RequestModel);

                    if (model.IsLogIn)
                    {
                        CommandHandler(context);
                    }
                    else
                    {
                        if ((context.Request.QueryString["controlName"].Equals("USER_RELOGIN") && context.Request.QueryString["cammandAction"].Equals("USER_SESSION_UNLOCK")))
                        {

                            MDVLogger.PresentationLog(context.Request.QueryString["controlName"].ToUpper(), context.Request.QueryString["cammandAction"].ToUpper(), "", MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), "");

                            CommandHandler(context);
                        }
                        else
                        {
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(model.RedirectSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.PresentationErrorLog("MDVisionHandler", ex, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
            }
        }

        /// <summary>
        /// The command handler behaves like a controller for whole application.
        /// </summary>
        /// <param name="context">The context.</param>
        private void CommandHandler(HttpContext context)
        {
            try
            {
                string controlName = context.Request.QueryString["controlName"].ToUpper();
                if (controlName.ToLower().Contains("admin"))
                {
                    AdminCommandHandler(context);
                }
                else if (controlName.ToLower().Contains("patient"))
                {
                    PatientCommandHandler(context);
                }
                else if (controlName.ToLower().Contains("scheduling"))
                {
                    SchedulingCommandHandler(context);
                }
                else if (controlName.ToLower().Contains("clinical") || controlName.ToLower().Contains("letter"))
                {
                    ClinicalCommandHandler(context);
                }
                else if (controlName.ToLower().Contains("billing") || controlName.ToLower().Contains("bill"))
                {
                    BillingCommandHandler(context);
                }
                else if (controlName.ToLower().Contains("reports"))
                {
                    ReportsCommandHandler(context);
                }
                else if (controlName.ToLower().Contains("dashboard"))
                {
                    DashBoardCommandHandler(context);
                }
                else if (controlName.ToLower().Contains("relogin"))
                {
                    UserReLoginommandHandler(context);
                }
                else if (controlName.ToLower().Contains("mobile"))
                {
                    MobileAppRequestCommandHandler(context);
                }

                else
                {
                    switch (controlName)
                    {
                        case "FORM_PRIVILEGE":
                            Common.AppPrivileges.CommandHandler(context);
                            break;
                        case "CITY_STATE_CONFIG":
                            Controls.CommonControls.AdminCityStateZip.Instance().CommandHandler(context);
                            break;
                        case "UPLOAD_IMAGE":
                            Controls.CommonControls.Upload_Image.Instance().CommandHandler(context);
                            break;
                        case "COMMON_CODE":
                            Controls.CommonControls.AdminCodes.Instance.CommandHandler(context);
                            break;
                        case "COMMON_IMO_CODE":
                            IMO.Instance.CommandHandler(context);
                            break;
                        //case "OCR_SCAN_CARD":
                        //    BusinessWrapper.CommonObject.Instance.CommandHandler(context);
                        //    break;
                        case "USER_TASK":
                            Controls.CommonControls.User_Task.Instance().CommandHandler(context);
                            break;
                        case "USER_LOGOUT":
                            new UserLoginHelper().Logout();
                            break;
                        case "PROVIDER_NPI_CONFIG":
                            Controls.Batch.Batch_ClinicalQualityMeasure.Instance().CommandHandler(context);
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            finally
            {

            }
        }

        private void AdminCommandHandler(HttpContext context)
        {
            try
            {
                string controlName = context.Request.QueryString["controlName"].ToUpper();
                switch (controlName)
                {
                    case "ADMIN_PRACTICE":
                        Controls.Admin.Admin_Practice.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_FACILITY":
                        Controls.Admin.Admin_Facility.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PROVIDER":
                        Controls.Admin.Admin_Provider.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_RESOURCES":
                        Controls.Admin.Admin_Resources.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_SPECIALTY":
                        Controls.Admin.Admin_Specialty.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PARTICIPANT_PROVIDER":
                        Controls.Admin.Admin_ParticipentProvider.Instance().CommandHandler(context);
                        break;

                    case "ADMIN_SECURITY_ROLE":
                        Controls.Admin.Admin_SecurityRoles.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_USER":
                        Controls.Admin.Admin_User.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PASSWORD_CONFIGURATION":
                        Admin_PasswordConfiguration.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PRIVILEGE_GROUP":
                        Controls.Admin.Admin_PrivilegeGroup.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_BILLING_PROVIDER_SETTINGS":
                        Controls.Admin.Admin_BillingProviderSettings.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_BILLING_PROVIDER":
                        Controls.Admin.Admin_BillingProvider.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PLAN_CATEGORY":
                        Controls.Admin.Admin_PlanCategory.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_INSURANCE":
                        Controls.Admin.Admin_Insur.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_ICD":
                        Controls.Admin.Admin_ICD.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_MODIFIER":
                        Controls.Admin.Admin_Modifier.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_INSURANCE_PLAN":
                        Controls.Admin.Admin_InsurancePlan.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PROCEDURE_CATEGORY":
                        Controls.Admin.Admin_ProcedureCategory.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_TYPE_OF_SERVICE":
                        Controls.Admin.Admin_TypeOfService.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_CPT_CODE":
                        Controls.Admin.Admin_CPTCode.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_REFERRING_PROVIDER":
                        Controls.Admin.Admin_ReferringProvider.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_EDI_SUBMIT_INSURANCE":
                        Controls.Admin.Admin_EDISubmitInsurance.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_REVENUE_CODE":
                        Controls.Admin.Admin_RevenueCode.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_EDI_CLAIM_STATUS_INSURANCE":
                        Controls.Admin.Admin_EDIClaimStatusInsurance.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_LEDGER_ACCOUNT":
                        Controls.Admin.Admin_LedgerAccount.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_EDI_ELIGIBILITY_INSURANCE":
                        Controls.Admin.Admin_EDIEligibilityInsurance.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PLAN_FEE_LINK":
                        Controls.Admin.Admin_PlanFeeLink.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_FEE_GROUP":
                        Controls.Admin.Admin_FeeGroup.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_CLEARING_HOUSE":
                        Controls.Admin.Admin_ClearingHouse.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_EDI_SERVICE_HANDLE":
                        Controls.Admin.Admin_EDIServiceHandle.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PATIENT_ELIGIBILITY_SERVICE":
                        Controls.Admin.Admin_PatientEligibilityService.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_HL7":
                        Controls.Admin.Admin_HL7.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PLACE_OF_SERVICE":
                        Controls.Admin.Admin_PlaceOfService.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_BASIC_FEE_GROUP":
                        Controls.Admin.Admin_BasicFeeGroup.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_BASIC_FEE_SCHEDULE":
                        Controls.Admin.Admin_BasicFeeSchedule.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_EDI_TAX_ID_SETUP":
                        Controls.Admin.Admin_EDITaxIDSetup.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PROCEDURE_FEE_SCHEDULE":
                        Controls.Admin.Admin_ProcedureFeeSchedule.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_POS_FEE_SCHEDULE":
                        Controls.Admin.Admin_POSFeeSchedule.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_SUBMITTER_SETUP":
                        Controls.Admin.Admin_SubmitterSetup.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_RECEIVER_SETUP":
                        Controls.Admin.Admin_EDIReceiver.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_SCHEDULE_REASON":
                        Controls.Admin.Admin_ScheduleReason.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_HOLIDAYS":
                        Controls.Admin.Admin_Holidays.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_BLOCK_HOURS":
                        Controls.Admin.Admin_BlockHours.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PROVIDERSCHEDULE":
                        Controls.Admin.Admin_ProviderSchedule.Instance().CommandHandler(context);
                        break;

                    case "ADMIN_APPOINTMENT_STATUS":
                        Controls.Admin.Admin_AppointmentStatus.Instance().CommandHandler(context);
                        break;

                    case "ADMIN_RESOURCESCHEDULE":
                        Controls.Admin.Admin_ResourceSchedule.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_DRUG_CODE_COST":
                        Controls.Admin.Admin_Drug_Code_Cost.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_VISITTYPEDURATIONGROUP":
                        Controls.Admin.Admin_VisitTypeDurationGroup.Instance().CommandHandler(context);
                        break;

                    // -----------------------------------------------------------------------
                    case "ADMIN_PRACTICE_DETAIL":
                        Controls.Admin.Admin_Practice_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_FACILITY_DETAIL":
                        Controls.Admin.Admin_Facility_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PROVIDER_DETAIL":
                        Controls.Admin.Admin_Provider_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_RESOURCES_DETAIL":
                        Controls.Admin.Admin_Resources_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_SPECIALTY_DETAIL":
                        Controls.Admin.Admin_Specialty_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PARTICIPANT_PROVIDER_DETAIL":
                        Controls.Admin.Admin_ParticipentProviderDetail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_SECURITY_ROLE_DETAIL":
                        Controls.Admin.Admin_SecurityRole_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_USER_DETAIL":
                        Controls.Admin.Admin_User_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_COWORKERGROUP_DETAIL":
                        Controls.Admin.Admin_CoWorkersGroupDetail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PRIVILEGE_GROUP_DETAIL":
                        Controls.Admin.Admin_PrivilegeGroup_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_BILLING_PROVIDER_SETTINGS_DETAIL":
                        Controls.Admin.Admin_BillingProviderSettings_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_BILLING_PROVIDER_DETAIL":
                        Controls.Admin.Admin_BillingProvider_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PLAN_CATEGORY_DETAIL":
                        Controls.Admin.Admin_PlanCategory_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_INSURANCE_DETAIL":
                        Controls.Admin.Admin_Insurance_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_ICD_DETAIL":
                        Controls.Admin.Admin_ICD_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_MODIFIER_DETAIL":
                        Controls.Admin.Admin_Modifier_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_INSURANCE_PLAN_DETAIL":
                        Controls.Admin.Admin_InsurancePlan_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PROCEDURE_CATEGORY_DETAIL":
                        Controls.Admin.Admin_ProcedureCategory_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_TYPE_OF_SERVICE_DETAIL":
                        Controls.Admin.Admin_TypeOfService_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_CPT_CODE_DETAIL":
                        Controls.Admin.Admin_CPTCode_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_REFERRING_PROVIDER_DETAIL":
                        Controls.Admin.Admin_ReferringProvider_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_EDI_SUBMIT_INSURANCE_DETAIL":
                        Controls.Admin.Admin_EDISubmitInsurance_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_REVENUE_CODE_DETAIL":
                        Controls.Admin.Admin_RevenueCode_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_EDI_CLAIM_STATUS_INSURANCE_DETAIL":
                        Controls.Admin.Admin_EDIClaimStatusInsurance_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_LEDGER_ACCOUNT_DETAIL":
                        Controls.Admin.Admin_LedgerAccount_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_EDI_ELIGIBILITY_INSURANCE_DETAIL":
                        Controls.Admin.Admin_EDIEligibilityInsurance_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PLAN_FEE_LINK_DETAIL":
                        Controls.Admin.Admin_PlanFeeLink_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_CLEARING_HOUSE_DETAIL":
                        Controls.Admin.Admin_ClearingHouse_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_EDI_SERVICE_HANDLE_DETAIL":
                        Controls.Admin.Admin_EDIServiceHandle_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_FEE_GROUP_DETAIL":
                        Controls.Admin.Admin_FeeGroup_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PLACE_OF_SERVICE_DETAIL":
                        Controls.Admin.Admin_PlaceOfService_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_BASIC_FEE_GROUP_DETAIL":
                        Controls.Admin.Admin_BasicFeeGroup_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_BASIC_FEE_SCHEDULE_DETAIL":
                        Controls.Admin.Admin_BasicFeeSchedule_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_EDI_TAX_ID_SETUP_DETAIL":
                        Controls.Admin.Admin_EDITaxIDSetup_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL":
                        Controls.Admin.Admin_ProcedureFeeSchedule_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_POS_FEE_SCHEDULE_DETAIL":
                        Controls.Admin.Admin_POSFeeSchedule_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_SUBMITTER_SETUP_DETAIL":
                        Controls.Admin.Admin_SubmitterSetup_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_RECEIVER_SETUP_DETAIL":
                        Controls.Admin.Admin_EDIReceiver_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_SCHEDULE_REASON_DETAIL":
                        Controls.Admin.Admin_ScheduleReason_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_HOLIDAYS_DETAIL":
                        Controls.Admin.Admin_Holidays_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_BLOCK_HOURS_DETAIL":
                        Controls.Admin.Admin_BlockHours_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PROVIDERSCHEDULE_DETAIL":
                        Controls.Admin.Admin_ProviderSchedule_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_APPOINTMENT_STATUS_DETAIL":
                        Controls.Admin.Admin_AppointmentStatus_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_DRUGCODECOST_DETAIL":
                        Controls.Admin.Admin_DrugCodeCost_Detail.Instance().CommandHandler(context);
                        break;


                    case "ADMIN_RESOURCESCHEDULE_DETAIL":
                        Controls.Admin.Admin_ResourceSchedule_Detail.Instance().CommandHandler(context);
                        break;

                    case "ADMIN_FOLDER_TYPE":
                        Controls.Admin.Admin_FolderType.Instance().CommandHandler(context);
                        break;

                    case "ADMIN_FOLDER_TYPE_DETAIL":
                        Controls.Admin.Admin_FolderType.Instance().CommandHandler(context);
                        break;

                    case "ADMIN_FOLDER":
                        Controls.Admin.Admin_Folder.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_SUPPER_BILL":
                        Controls.Admin.Admin_SupperBill.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_SUPPER_BILL_DETAIL":
                        Controls.Admin.Admin_SupperBillDetail.Instance().CommandHandler(context);
                        break;

                    case "ADMIN_FOLLOWUPREASON":
                        Controls.Admin.FollowUp.Admin_FollowUpReason.Instance().CommandHandler(context);
                        break;

                    case "ADMIN_FOLLOWUPTYPE":
                        Controls.Admin.FollowUp.Admin_FollowUpType.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_ERA_ADJUSTMENT_CODES":
                        Controls.Admin.ERA.Admin_ERAAdjustmentCodes.Instance().CommandHandler(context);
                        break;

                    case "ADMIN_ERAADJUSTMENTCODES_DETAIL":
                        Controls.Admin.ERA.Admin_ERAAdjustmentCodes_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_FOLLOWUP_REMITTANCECODE":
                        Controls.Admin.FollowUp.Admin_FollowUpRemittanceCode.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_FOLLOWUP_ACTION":
                        Controls.Admin.FollowUp.Admin_FollowUpAction.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_FOLLOWUP_CLAIMSTATUSCODE":
                        Controls.Admin.FollowUp.Admin_FollowUpClaimStatusCode.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_FOLLOWUP_CLAIMSTATUSCATCODE":
                        Controls.Admin.FollowUp.Admin_FollowUpClaimStatusCategoryCode.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_FOLLOWUP_CODESMAPPING":
                        Controls.Admin.FollowUp.Admin_FollowUpCodesMapping.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_STATEMENT_MESSAGE":
                        Controls.Admin.Statement.Admin_StatementMessage.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_STATEMENT_GROUP":
                        Controls.Admin.Statement.Admin_StatementGroup.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_FOLLOWUP_GROUP":
                        Controls.Admin.FollowUp.Admin_FollowUpGroup.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_IMOSEARCH_DETAIL":
                        Controls.Admin.Admin_IMOSearch_Detail.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_IMOICD":
                        Controls.Admin.Admin_IMOICD.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_IMOCPT":
                        Controls.Admin.Admin_IMOCPT.Instance().CommandHandler(context);
                        break;

                    case "ADMIN_FOLLOWUP_ADJUSTMENTCODE":
                        Controls.Admin.FollowUp.Admin_FollowUpAdjustmentCode.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_PROVIDER_FAXSETTINGS":
                        Controls.Admin.Admin_FaxSettings.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_CCMTEMPLATES":
                        Controls.Admin.CCM.Admin_CCMTemplates.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_CCMICDGROUPS":
                        Controls.Admin.CCM.Admin_CCMICDGroups.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_CCMCARETEAM":
                        Controls.Admin.CCM.Admin_CCMCareTeam.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_CCM_TEMPLATE_DETAILS":
                        Controls.Admin.CCM.Admin_CCMTemplateDetails.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_CCM_QUESTION_DETAILS":
                        Controls.Admin.CCM.Admin_CCMQuestionDetails.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_COWORKERGROUP":
                        Controls.Admin.Admin_CoWorkersGroup.Instance().CommandHandler(context);
                        break;
                    //case "Admin_CCMICDGroups":
                    //    Controls.Admin.CCM.Admin_CCMICDGroups.Instance().CommandHandler(context);
                    //    break;
                    //case "Admin_CCMCareTeam":
                    //    Controls.Admin.CCM.Admin_CCMCareTeam.Instance().CommandHandler(context);
                    //    break;
                    case "ADMIN_OCCUPATION_STATUS_DETAIL":
                        Controls.Admin.Admin_OccupationStatus.Instance().CommandHandler(context);
                        break;
                    case "ADMIN_VISITTYPEDURATIONGROUP_DETAIL":
                        Controls.Admin.Admin_VisitTypeDurationGroup_Detail.Instance().CommandHandler(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            finally
            {

            }
        }

        /// <summary>
        /// Handler of Patients Command
        /// Behaves like a controller between Javascript and CS Files
        /// </summary>
        /// <param name="context">The context.</param>
        private void PatientCommandHandler(HttpContext context)
        {
            try
            {
                string controlName = context.Request.QueryString["controlName"].ToUpper();
                switch (controlName)
                {
                    case "PATIENT_DEMOGRAPHIC":
                        Controls.Patient.Demographics.Patient_Demographic.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_PREFERENCES":
                        Controls.Patient.Demographics.Patient_Preferences.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_INSURANCE":
                        Controls.Patient.Insurance.Patient_Insurance.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_ELIGIBILITY":
                        Controls.Patient.Insurance.Patient_Eligibility.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_ELIGIBILITY_DETAIL":
                        Controls.Patient.Insurance.Patient_Eligibility_Detail.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_REFERRAL":
                        Controls.Patient.Insurance.Patient_Referral.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_LAWYER":
                        Controls.Patient.Insurance.Patient_Lawyer.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_LAWYER_DETAIL":
                        Controls.Patient.Insurance.Patient_Lawyer_Detail.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_EMPLOYER":
                        Controls.Patient.Insurance.Patient_Employer.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_EMPLOYER_DETAIL":
                        Controls.Patient.Insurance.Patient_Employer_Detail.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_GUARANTOR":
                        Controls.Patient.Demographics.Patient_Guarantor.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_GUARANTOR_DETAIL":
                        Controls.Patient.Demographics.Patient_Guarantor_Detail.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_SCHOOL":
                        Controls.Patient.Demographics.Patient_School.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_SCHOOL_DETAIL":
                        Controls.Patient.Demographics.Patient_School_Detail.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_SEARCH":
                        Controls.Patient.Patient_Search.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_EMERGENCYCONTACT":
                        Controls.Patient.Demographics.Patient_EmergencyContact.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_EMERGENCYCONTACT_DETAIL":
                        Controls.Patient.Demographics.Patient_EmergencyContact_Detail.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_FAMILY":
                        Controls.Patient.Demographics.Patient_Family.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_FAMILY_DETAIL":
                        Controls.Patient.Demographics.Patient_Family_Detail.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_DEMOGRAPHIC_QUICK":
                        Controls.Patient.Demographics.Patient_Demographic_Quick.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_PREAUTHORIZATION":
                        Controls.Patient.Insurance.Patient_PreAuthorization.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_PREAUTHORIZATION_DETAIL":
                        Controls.Patient.Insurance.Patient_PreAuthorization_Detail.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_ACTIVITY_LOG":
                        Controls.CommonControls.Activity_Log.Instance().CommandHandler(context);
                        break;
                    case "DRUGCODECOST_ACTIVITY_LOG":
                        Controls.CommonControls.Activity_Log.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_MESSAGE":
                        Controls.Patient.Messages.Patient_Message.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_DOCUMENT":
                        Controls.Patient.Document.Patient_Document.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_DOCUMENT_IMAGE_ANNOTATION":
                        Controls.Patient.Document.Patient_Document_Image_Annotation.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_CASE":
                        Controls.Patient.Case.Patient_Case.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_CASE_ADJUSTER":
                        Controls.Patient.Case.Patient_CaseAdjuster.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_ENCOUNTER_VISITS":
                        Controls.Patient.Encounter.Encounter_Visits.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_CHARGE_CAPTURE":
                        Controls.Patient.Encounter.Encounter_ChargeCapture.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_CLAIM_SUMMARY":
                        Controls.Patient.Encounter.Encounter_ClaimSummary.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_ADVANCE_PAYMENT":
                        Controls.Patient.Demographics.Patient_AdvancePayment.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_OUTSTANDING_VISIT":
                        Controls.CommonControls.Outstanding_Visit.Instance().CommandHandler(context);
                        break;

                    case "PATIENT_LEDGER":
                        Controls.CommonControls.Patient_Ledger.Instance().CommandHandler(context);
                        break;

                    case "PATIENT_ACCOUNT_MANAGER":
                        Controls.Patient.PatientPortal.Patient_AccountManager.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_REMINDER_LOG":
                        Controls.Patient.Demographics.Patient_Reminder_Log.Instance().CommandHandler(context);
                        break;
                    case "PATIENT_FAX_LOG":
                        Controls.Patient.Demographics.Patient_Fax_Log.Instance().CommandHandler(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            finally
            {

            }
        }

        private void SchedulingCommandHandler(HttpContext context)
        {
            try
            {
                string controlName = context.Request.QueryString["controlName"].ToUpper();
                switch (controlName)
                {
                    case "SCHEDULING_APPOINTMENT_DETAIL":
                        Controls.Scheduling.Scheduling_Appointment_Detail.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_APPOINTMENT_HISTORY":
                        Controls.Scheduling.Scheduling_Appointment_History.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_CALENDAR":
                        Controls.Scheduling.Scheduling_Calendar.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_SEARCH":
                        Controls.Scheduling.Scheduling_Search.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_BLOCK_UNBLOCK":
                        Controls.Scheduling.Scheduling_SlotBlockUnblock.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_SEARCH_APPSTATUS":
                        Controls.Scheduling.Scheduling_SearchAppointmentByStatus.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_SELECT_SLOT_DETAIL":
                        Controls.Scheduling.Scheduling_EditSlot.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_MULTIPLEVIEW_GROUP":
                        Controls.Scheduling.Scheduling_MultipleView_Group.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_MULIVIEW":
                        Controls.Scheduling.Scheduling_MuliView.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_MULTIPLEVIEW_GROUP_DETAIL":
                        Controls.Scheduling.Scheduling_MultipleView_Group_Detail.Instance().CommandHandler(context);
                        break;

                    case "SCHEDULING_CHANGE_FACILITY_DETAIL":
                        Controls.Scheduling.Scheduling_ChangeFacility.Instance().CommandHandler(context);
                        break;

                    case "SCHEDULING_CHECKIN":
                        Controls.Scheduling.Scheduling_CheckIn.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_CHECKOUT":
                        Controls.Scheduling.Scheduling_CheckOut.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_COPAYMENT":
                        Controls.Scheduling.Scheduling_Copayment.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_WAITLIST":
                        Controls.Scheduling.Scheduling_WaitList.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_WAITLIST_DETAIL":
                        Controls.Scheduling.Scheduling_WaitListDeatil.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_APPOINTMENT_SEARCH":
                        Controls.Scheduling.Scheduling_AppointmentSearch.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_APPOINTMENT_SUMMARY":
                        Controls.Scheduling.Scheduling_Appointment_Summary.Instance().CommandHandler(context);
                        break;
                    case "SCHEDULING_UNALLOCATEDCOPAYMENT":
                        Controls.Scheduling.Scheduling_UnallocatedCopayment.Instance().CommandHandler(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            finally
            {

            }
        }

        private void ClinicalCommandHandler(HttpContext context)
        {
            try
            {
                string controlName = context.Request.QueryString["controlName"].ToUpper();
                switch (controlName)
                {
                    case "CLINICAL_TEMPLATE":
                        Controls.Clinical.Clinical_Template.Instance().CommandHandler(context);
                        break;
                    case "CLINICAL_SECTION":
                        Controls.Clinical.Clinical_Section.Instance().CommandHandler(context);
                        break;
                    case "CLINICAL_QUESTION":
                        Controls.Clinical.Clinical_Question.Instance().CommandHandler(context);
                        break;
                    case "DESIGN_LETTER":
                        Controls.Clinical.Design_Letter.Instance().CommandHandler(context);
                        break;
                    case "CLINICAL_QUESTION_GROUP":
                        Controls.Clinical.TemplateBuilder.Clinical_Question_Group.Instance().CommandHandler(context);
                        break;
                    case "DESIGN_LETTERPRINTING":
                        Controls.Clinical.LetterDesign.Design_LetterPrinting.Instance().CommandHandler(context);
                        break;
                    case "ENCOUNTER_CLINICALNOTE":
                        Controls.Patient.Encounter.Encounter_ClinicalNote.Instance().CommandHandler(context);
                        break;
                    case "CLINICALMENUSETTINGS":
                        Controls.Clinical.ClinicalMenuSettings.Instance().CommandHandler(context);
                        break;
                    case "BATCH_CLINICALQUALITYMEASURE":
                        Controls.Batch.Batch_ClinicalQualityMeasure.Instance().CommandHandler(context);
                        break;
                    case "BATCH_FAX_CLINICAL":
                        Controls.Batch.Batch_Fax.Instance().CommandHandler(context);
                        break;
                    case "BATCH_IMPORT_HL7_BATCH_CLINICAL":
                        Controls.Batch.Batch_ImportHL7ImmunizationBatch.Instance().CommandHandler(context);
                        break;
                    //Upload Lab Results added By azhar shahzad
                    case "CLINICAL_LABRESULT":
                        EMR.Helpers.Results.LabResultHelper.Instance().CommandHandler(context);
                        break;

                    case "BATCH_CLINICALIMPORTCCDA":
                        Controls.Batch.Batch_ImportCCDA.Instance().CommandHandler(context);
                        break;
                        //case "CLINICAL_NOTES":
                        //    Controls.Clinical.Notes.Clinical_Notes.Instance().CommandHandler(context);
                        //    break;
                        //case "CLINICAL_VITALS":
                        //Controls.Clinical.Medical.Clinical_Vitals.Instance().CommandHandler(context);
                        //  break;
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            finally
            {

            }
        }

        private void BillingCommandHandler(HttpContext context)
        {
            try
            {
                string controlName = context.Request.QueryString["controlName"].ToUpper();
                switch (controlName)
                {
                    case "BILLING_CHARGE":
                        new Bill_ChargeSearch().CommandHandler(context);
                        break;
                    case "BILLING_BATCHCHARGE":
                        Controls.Billing.Bill_ChargeBatchSearch.Instance().CommandHandler(context);
                        break;
                    case "BILLING_BATCHCHARGE_DETAIL":
                        Controls.Billing.Bill_ChargeBatch_Detail.Instance().CommandHandler(context);
                        break;
                    case "BILLING_CHARGEDETAIL":
                        Controls.Billing.Bill_Charge_Detail.Instance().CommandHandler(context);
                        break;

                    case "BILLING_CHARGEVIEW_DETAIL":
                        Controls.Billing.Charges.Bill_ChargeView.Instance().CommandHandler(context);
                        break;

                    //-------------------------CLAIM--------------------------------
                    case "BILLING_CLAIMVIEW_DETAIL":
                        Controls.Billing.Claims.Bill_ClaimView.Instance().CommandHandler(context);
                        break;
                    case "BILLING_UNCLAIMED_APP":
                        Controls.Billing.Claims.Bill_UnClaimed_Appointment.Instance().CommandHandler(context);
                        break;
                    case "BILLING_EDI_CLAIMVIEW_DETAIL":
                        Controls.Billing.Claims.Bill_EDIClaimView.Instance().CommandHandler(context);
                        break;
                    case "BILLING_EDI_BATCH_DETAIL":
                        Controls.Billing.Claims.Bill_EDIBatchDetail.Instance().CommandHandler(context);
                        break;
                    //--------------------------Payment Batch---------------------------------
                    case "BILLING_PAYMENT_BATCH_DETAIL":
                        Controls.Billing.Payments.Bill_PaymentBatch_Detail.Instance().CommandHandler(context);
                        break;
                    case "BILLING_PAYMENT_BATCH":
                        Controls.Billing.Payments.Bill_PaymentBatchSearch.Instance().CommandHandler(context);
                        break;
                    //--------------------------Payment Posting---------------------------------
                    case "BILLING_PAYMENT_POSTING":
                        Controls.Billing.Payments.Bill_PaymentPosting.Instance().CommandHandler(context);
                        break;

                    //--------------------------Patient Statement---------------------------------
                    case "BILLING_STATEMENT":
                        Controls.Billing.PatientStatement.Bill_PatientStatement.Instance().CommandHandler(context);
                        break;
                    //----------------------ERA---------------------------------------------

                    case "BILL_ERA_DETAIL":
                        Controls.Billing.ERA.Bill_ERA_Detail.Instance().CommandHandler(context);
                        break;
                    //----------------------FOLLOWUP---------------------------------------------
                    case "BILLING_FOLLOWUP_AR_CALL":
                        Controls.Billing.FollowUp.Bill_FollowUpARCall.Instance().CommandHandler(context);
                        break;
                    case "BILLING_FOLLOWUP_CLAIM_SPLIT":
                        Controls.Billing.FollowUp.Bill_FollowUpClaimSplit.Instance().CommandHandler(context);
                        break;

                    case "BILLING_FOLLOWUP_HISTORY":
                        Controls.Billing.FollowUp.Bill_FollowUpARHistory.Instance().CommandHandler(context);
                        break;
                    case "BILLING_FOLLOWUP_COMMENTS":
                        Controls.Billing.FollowUp.Bill_FollowUpARComments.Instance().CommandHandler(context);
                        break;
                    case "BILLING_EOB_MANUAL_POSTING":
                        Controls.Billing.ERA.EOB_Manual_Posting.Instance().CommandHandler(context);
                        break;

                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            finally
            {

            }
        }

        private void ReportsCommandHandler(HttpContext context)
        {
            try
            {
                string controlName = context.Request.QueryString["controlName"].ToUpper();
                switch (controlName)
                {
                    case "REPORTS_DASHBOARD":
                        Controls.Reports.ReportsSsrsDashboard.Instance().CommandHandler(context);
                        break;
                    case "REPORTS_MONTHLY_PAYMENT_TREND_DETAIL":
                        Controls.Reports.FinancialKPI.MonthlyPaymentTrend_Detail.Instance().CommandHandler(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            finally
            {

            }
        }
        private void DashBoardCommandHandler(HttpContext context)
        {
            try
            {
                string controlName = context.Request.QueryString["controlName"].ToUpper();
                switch (controlName)
                {
                    case "DASHBOARD":
                        Controls.DashBoard.DashBoard.Instance().CommandHandler(context);
                        break;
                    case "DASHBOARDSETTING":
                        Controls.DashBoard.DashBoardSetting.Instance().CommandHandler(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            finally
            {

            }
        }


        private void UserReLoginommandHandler(HttpContext context)
        {
            try
            {
                string controlName = context.Request.QueryString["controlName"].ToUpper();
                switch (controlName)
                {
                    case "USER_RELOGIN":
                        Controls.CommonControls.User_ReLogin.Instance().CommandHandler(context);
                        break;

                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            finally
            {

            }
        }
        private void MobileAppRequestCommandHandler(HttpContext context)
        {
            try
            {
                string controlName = context.Request.QueryString["controlName"].ToUpper();
                switch (controlName)
                {

                    case "MOBILEAPPREQUEST":
                        Controls.Live_Requests.MobileAppRequest.MobileAppRequest.Instance().CommandHandler(context);
                        break;

                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            finally
            {

            }
        }
        #endregion
    }
}
