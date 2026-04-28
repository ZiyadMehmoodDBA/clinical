using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using MDVision.IEHR.Common;
using System.Web.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IO;
using System.Text;

namespace MDVision.IEHR
{

    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254726
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            DefaultPageBundles(bundles);
            AdminPageBundles(bundles);
            BillingPageBundles(bundles);
            AuditableEventsBundles(bundles);
            iTrackPageBundles(bundles);
            ReportsBundles(bundles);
            CommonBundles(bundles);
            DocumentScanBundles(bundles);



            BundleTable.EnableOptimizations = true;
        }

        private static void ReportsBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/ReportsFiles")

                 .Include("~/Scripts/MDVisionReports.js")
                 .Include("~/EMR/AppScripts/Common/KenduReports.js")
                  .Include("~/EMR/AppScripts/iTrack/iTrack_Dashboard.js")
                 .Include("~/Controls/Patient/Patient_Search.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Demographic_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Demographic.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Family.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Family_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Preferences.js")
                 .Include("~/Controls/Patient/Demographics/Patient_AdvancePayment.js")
                 .Include("~/Controls/Patient/Demographics/Patient_School.js")
                 .Include("~/Controls/Patient/Demographics/Patient_School_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_AdvancePayment_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_EmergencyContact.js")
                 .Include("~/Controls/Patient/Demographics/Patient_EmergencyContact_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_DemographicLabel.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Demographic_Quick.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Guarantor.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Guarantor_Detail.js")
                 .Include("~/Controls/Patient/PatientPortal/Patient_AccountManager.js")
                 .Include("~/Controls/Patient/Case/Patient_Case.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_Visits.js")
                 .Include("~/Controls/Patient/Case/Patient_Case_Detail.js")
                 .Include("~/Controls/Batch/Batch_Fax.js")
                 .Include("~/Controls/Batch/Batch_Fax_QuickLink.js")
                 .Include("~/Controls/Batch/Batch_FaxSendAnnotate.js")
                 .Include("~/Controls/Batch/Batch_FaxSend.js")
                 .Include("~/Controls/Batch/Batch_FaxContacts.js")
                 .Include("~/Controls/Batch/Batch_FaxView.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_ChargeCapture.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Scan.js")
                 .Include("~/Controls/Patient/Document/Patient_DocumentTag.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Import.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Viewer.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Export.js")
                 .Include("~/Controls/Patient/Document/Patient_Information_Submission.js")
                 .Include("~/Controls/Patient/Document/Patient_Documents_Audit.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_AssignedTo.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Search.js")
                 .Include("~/Controls/Patient/Messages/Patient_Demographic_Quick.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Edit.js")
                 .Include("~/Controls/Patient/Messages/Patient_UserMessagesQuickLink.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Create.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Compose.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Alert.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Add.js")
                 .Include("~/EMR/AppScripts/Patient/Patient_CustomForm.js")
                 .Include("~/EMR/AppScripts/Clinical/FaceSheet/Clinical_FaceSheetView.js")
                 .Include("~/EMR/AppScripts/Clinical/FaceSheet/Clinical_FaceSheetComponentSelection.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Referral_Search.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Insurance.js")
                 .Include("~/Controls/Patient/Insurance/Patient_PreAuthorization.js")
                 .Include("~/Controls/Patient/Insurance/Patient_PreAuthorization_Detail.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Referral.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Eligibility.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Eligibility_Detail.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_CreateClaim.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_ClaimSummary.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Search.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Lawyer.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Lawyer_Detail.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Employer.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Employer_Detail.js")
                 .Include("~/Controls/Admin/Admin_BillingProvider.js")
                 .Include("~/Controls/Admin/Admin_BillingProvider_Detail.js")
                 .Include("~/EMR/AppScripts/Common/Clinical_ProblemLists.js")
                 .Include("~/Controls/Scheduling/Scheduling_CheckIn.js")
                 .Include("~/Controls/Scheduling/Scheduling_Copayment.js")
                 .Include("~/Controls/Scheduling/Scheduling_Appointment_History.js")
                 .Include("~/Controls/Scheduling/Scheduling_UnallocatedCopayment.js")
                 .Include("~/Controls/Scheduling/Scheduling_UnallocatedCopayment_Search.js")
                 .Include("~/Controls/Scheduling/Scheduling_Unallocated_CopaymentView.js")
                 .Include("~/Controls/Scheduling/Scheduling_Appointment_Detail.js")
                 .Include("~/Controls/Scheduling/Scheduling_CopaymentView.js")
                 .Include("~/CCM/AppScripts/CCM_Patient_Hub.js")
                 .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_Problems.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/OutOfOfficeVisits.js")

                 .Include("~/EMR/AppScripts/Clinical/Orders/Clinical_LabOrderDetail.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/BillingInformation.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_Notes.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_ProgressNote.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/ENMCodeSuggest.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/Clinical_SuperBillTemplate.js")
                 .Include("~/Controls/Admin/Admin_PlaceOfService.js")
                 .Include("~/Controls/Admin/Admin_PlaceOfService_Detail.js")
                 .Include("~/Controls/Clinical/LetterDesign/Design_LetterPrinting.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_NotesView.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_NotesCoSign.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_NotesAmendment.js")
                .Include("~/Controls/Patient/Messages/Patient_Message.js")
                .Include("~/Controls/Patient/Document/Patient_Document.js")

                .Include("~/EMR/AppScripts/Common/Restrict_User.js")
                .Include("~/EMR/AppScripts/Common/Clinical_FaceSheet.js")
                .Include("~/EMR/AppScripts/Clinical/MUReport/MUStage1.js")

                 .Include("~/Controls/Billing/ERA/Bill_ERA_ElectronicEOB.js")
                 .Include("~/Controls/Billing/Claims/Bill_ClaimHcfaForm.js")
                 .Include("~/Controls/Billing/Payments/Bill_PaymentPosting.js")
                 .Include("~/Controls/Billing/Charges/Bill_ChargeSearch.js")
                 .Include("~/Controls/Billing/Payments/Bill_PatientPayments.js")
                 .Include("~/Controls/Billing/Payments/Bill_ReceivedPatientPayments.js")
                 .Include("~/Controls/Billing/Payments/Bill_PatientPaymentsPrint.js")
                 .Include("~/EMR/AppScripts/Batch/PatientList/Batch_PatientList.js")
                 .Include("~/EMR/AppScripts/AuditbleEvents/AuditbleEvents_ActivityLog.js")
               .Include("~/EMR/AppScripts/Common/Clinical_LabOrder.js")
               .Include("~/EMR/AppScripts/Common/Clinical_Lab.js")
               .Include("~/Controls/Billing/FollowUp/Bill_FollowUpARComments.js")
               .IncludeDirectory("~/Controls/Reports", "*.js", true)
             );

            bundles.Add(new ScriptBundle("~/bundles/StartupReports")
                  .Include("~/Scripts/theme.js")
                  .Include("~/Scripts/ApplicationCommand/StartupReports.js")
            );
        }

        private static void DefaultPageBundles(BundleCollection bundles)
        {
            /*  
                .Include("~/Scripts/MDVisionDefault.js")
                */
            bundles.Add(new ScriptBundle("~/bundles/StartupDashboard")
              .Include("~/Scripts/theme.js")
              // to initiate the calls entry point of the application
              .Include("~/Scripts/ApplicationCommand/Startup.js")
          );

            bundles.Add(new ScriptBundle("~/bundles/DashboardFiles")

                 .Include("~/Scripts/MDVisionDefault.js")

                .Include("~/Controls/Patient/Messages/Patient_Message.js")
                .Include("~/Controls/Patient/Document/Patient_Document.js")
                .Include("~/Controls/Scheduling/Scheduling_Search.js")
                .Include("~/Controls/LiveRequests/MobileAppRequests/MobileAppRequest.js")
                .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_ProgressNote.js") //EMR-1123 , adnan maqbool
                                                                                     // Start Ast-295
                 .Include("~/Controls/Billing/PatientStatement/Bill_PatientStatement.js")
                 .Include("~/Controls/Billing/PatientStatement/Bill_PatientStatementBatch.js")
                 //End Ast-295
                 .Include("~/Controls/Billing/ERA/Bill_ERA_ElectronicEOB.js")
                 .Include("~/Controls/Billing/Claims/Bill_ClaimHcfaForm.js")
                 .Include("~/Controls/Billing/Payments/Bill_PaymentPosting.js")
                 .Include("~/Controls/Billing/Charges/Bill_ChargeSearch.js")
                 .Include("~/Controls/Billing/Payments/Bill_PatientPayments.js")
                 .Include("~/Controls/Billing/Payments/Bill_ReceivedPatientPayments.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_ProgressNote.js") //EMR-1123 , adnan maqbool
                 .Include("~/Controls/Billing/Payments/Bill_PatientPaymentsPrint.js")
                 .Include("~/EMR/AppScripts/Clinical/PQRSAdmin/IA_DiabetesScreening.js")
                 .Include("~/EMR/AppScripts/Clinical/PQRSAdmin/IATabacooScreening.js")
                 .Include("~/Controls/Billing/FollowUp/Bill_FollowUpARComments.js")
                //AST-294 Start
                .Include("~/Controls/Billing/Charges/Bill_Charge_Detail.js")
                //AST-294 End
                .Include("~/Controls/Admin/CCM/Admin_CCMTemplateDetails.js")

               .IncludeDirectory("~/Controls/Scheduler", "*.js", false)
               .IncludeDirectory("~/EMR/AppScripts/Common", "*.js", true)
               .IncludeDirectory("~/Controls/Batch", "*.js", true)
               .IncludeDirectory("~/Controls/Clinical", "*.js", true)
              

          );
            bundles.Add(new ScriptBundle("~/bundles/DashboardFilesSecondLayer")
                
                 .IncludeDirectory("~/Controls/Patient", "*.js", true)
                 .IncludeDirectory("~/Controls/Reports", "*.js", true)
                 .IncludeDirectory("~/Controls/Scheduling", "*.js", true)
                .IncludeDirectory("~/CCM/AppScripts", "*.js", true)
               
                );

            bundles.Add(new ScriptBundle("~/bundles/DashboardFilesThirdLayer")

                .IncludeDirectory("~/EMR/AppScripts/Batch", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/AuditReport", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/BillingInformation", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/CaseReports", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/CDS", "*.js", true)

                .IncludeDirectory("~/EMR/AppScripts/Clinical/ClinicalSummary", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/CustomForms", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/DataTemplates", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/FaceSheet", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Favorites", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/FollowUp", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/History", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Immunization", "*.js", true)

                );

            bundles.Add(new ScriptBundle("~/bundles/DashboardFilesForthLayer")

                .IncludeDirectory("~/EMR/AppScripts/Clinical/InfoButton", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Lab", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/LOINC", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Macros", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Medical", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/MUReport", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Notes", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Orders", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Orthopedic", "*.js", true)

                );

            bundles.Add(new ScriptBundle("~/bundles/DashboardFilesFifthLayer")

                .IncludeDirectory("~/EMR/AppScripts/Clinical/PhysicalExam", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/PQRSAdmin", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/ReportHeader", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Results", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/ReviewOfSystem", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/ReviewofSystems", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/TemplateBuilder", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/TemplateBuilderNew", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Templates", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Clinical/Treatment", "*.js", true)
                .IncludeDirectory("~/EMR/AppScripts/Patient", "*.js", true)
                );
        }

        private static void AuditableEventsBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/AuditableEventFiles")
                .Include("~/Scripts/MDVisionAuditableEvents.js")
                .Include("~/EMR/AppScripts/AuditbleEvents/AuditbleEvents_ActivityLog.js")
                .Include("~/Controls/Patient/Demographics/Patient_Demographic.js")
                .Include("~/Controls/Patient/Demographics/Patient_DemographicLabel.js")
                .Include("~/Controls/Batch/Batch_Fax.js")
                 .Include("~/Controls/Batch/Batch_Fax_QuickLink.js")
                .Include("~/Controls/Patient/Messages/Patient_Message.js")
                 .Include("~/Controls/Admin/Admin_SecurityRole_Detail.js")
                );


            bundles.Add(new ScriptBundle("~/bundles/AuditableEventsStartup")
                .Include("~/Scripts/theme.js")
                .Include("~/Scripts/ApplicationCommand/StartupAuditableEvents.js")
            );
        }

        private static void BillingPageBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/StartupBilling")
                .Include("~/Scripts/theme.js")
                .Include("~/Scripts/ApplicationCommand/StartupBilling.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/BillingFiles")
                 .Include("~/Scripts/MDVisionDefaultCommon.js")
                 .Include("~/Scripts/MDVisionDefaultBilling.js")
                 .Include("~/Controls/Patient/Patient_Search.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Demographic_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Demographic.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Family.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Family_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Preferences.js")
                 .Include("~/Controls/Patient/Demographics/Patient_AdvancePayment.js")
                 .Include("~/Controls/Patient/Demographics/Patient_School.js")
                 .Include("~/Controls/Patient/Demographics/Patient_School_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_AdvancePayment_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_EmergencyContact.js")
                 .Include("~/Controls/Patient/Demographics/Patient_EmergencyContact_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_DemographicLabel.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Demographic_Quick.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Guarantor.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Guarantor_Detail.js")
                 .Include("~/Controls/Patient/PatientPortal/Patient_AccountManager.js")
                 .Include("~/Controls/Patient/Case/Patient_Case.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_Visits.js")
                 .Include("~/Controls/Patient/Case/Patient_Case_Detail.js")
                 .Include("~/Controls/Batch/Batch_Fax.js")
                  .Include("~/Controls/Batch/Batch_Fax_QuickLink.js")
                 .Include("~/Controls/Batch/Batch_FaxSendAnnotate.js")
                 .Include("~/Controls/Batch/Batch_FaxSend.js")
                 .Include("~/Controls/Batch/Batch_FaxContacts.js")
                 .Include("~/Controls/Batch/Batch_FaxView.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_ChargeCapture.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_NDCSelection.js")

                 .Include("~/Controls/Patient/Document/Patient_Document_Scan.js")
                 .Include("~/Controls/Patient/Document/Patient_DocumentTag.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Import.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Viewer.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Export.js")
                 .Include("~/Controls/Patient/Document/Patient_Information_Submission.js")
                 .Include("~/Controls/Patient/Document/Patient_Documents_Audit.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_AssignedTo.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Search.js")
                 .Include("~/Controls/Patient/Messages/Patient_Demographic_Quick.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Edit.js")
                 .Include("~/Controls/Patient/Messages/Patient_UserMessagesQuickLink.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Create.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Compose.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Alert.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Add.js")
                 .Include("~/EMR/AppScripts/Patient/Patient_CustomForm.js")
                 .Include("~/EMR/AppScripts/Clinical/FaceSheet/Clinical_FaceSheetView.js")
                 .Include("~/EMR/AppScripts/Clinical/FaceSheet/Clinical_FaceSheetComponentSelection.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Referral_Search.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Insurance.js")
                 .Include("~/Controls/Patient/Insurance/Patient_PreAuthorization.js")
                 .Include("~/Controls/Patient/Insurance/Patient_PreAuthorization_Detail.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Referral.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Eligibility.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Eligibility_Detail.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_CreateClaim.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_ClaimSummary.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Search.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Lawyer.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Lawyer_Detail.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Employer.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Employer_Detail.js")
                 .Include("~/Controls/Admin/Admin_BillingProvider.js")
                 .Include("~/Controls/Admin/Admin_BillingProvider_Detail.js")
                 .Include("~/EMR/AppScripts/Common/Clinical_ProblemLists.js")
                 .Include("~/Controls/Scheduling/Scheduling_CheckIn.js")
                 .Include("~/Controls/Scheduling/Scheduling_Copayment.js")
                 .Include("~/Controls/Scheduling/Scheduling_Appointment_History.js")
                 .Include("~/Controls/Scheduling/Scheduling_UnallocatedCopayment.js")
                 .Include("~/Controls/Scheduling/Scheduling_UnallocatedCopayment_Search.js")
                 .Include("~/Controls/Scheduling/Scheduling_Unallocated_CopaymentView.js")
                 .Include("~/Controls/Scheduling/Scheduling_Appointment_Detail.js")
                 .Include("~/Controls/Scheduling/Scheduling_CopaymentView.js")
                 .Include("~/CCM/AppScripts/CCM_Patient_Hub.js")
                 .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_Problems.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/OutOfOfficeVisits.js")

                 .Include("~/EMR/AppScripts/Clinical/Orders/Clinical_LabOrderDetail.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/BillingInformation.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_Notes.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_ProgressNote.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/ENMCodeSuggest.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/Clinical_SuperBillTemplate.js")
                 .Include("~/Controls/Admin/Admin_PlaceOfService.js")
                 .Include("~/Controls/Admin/Admin_PlaceOfService_Detail.js")
                 .Include("~/Controls/Clinical/LetterDesign/Design_LetterPrinting.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_NotesView.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_NotesCoSign.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_NotesAmendment.js")
                .Include("~/Controls/Patient/Messages/Patient_Message.js")
                .Include("~/Controls/Patient/Document/Patient_Document.js")
                
                .Include("~/EMR/AppScripts/Common/Restrict_User.js")
                .Include("~/EMR/AppScripts/Common/Clinical_FaceSheet.js")
                .IncludeDirectory("~/Controls/Billing", "*.js", true)
           );

        }
        private static void iTrackPageBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/StartupiTrack")
                .Include("~/Scripts/theme.js")
                .Include("~/Scripts/ApplicationCommand/StartupiTrack.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/iTrackFiles")
                .Include("~/EMR/AppScripts/iTrack/iTrack_eCQMsDetail.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_Cost.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_Submission.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_Dashboard.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_eCQMs.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_MUReport.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_MUStage3.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_MIPSummary.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_MIPSGraph.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_QualityGraph.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_PromotingInteroperabilityGraph.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_AdminPreferenceIndividualReporting.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_PromotingInteroperabilityDetail.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_MIPSPrintPreview.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_Quality.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_PromotingInteroperability.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_AdminPreferenceIndividualReporting.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_ImprovementActivities.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_QualityMeasureDetail.js")
                 .Include("~/Scripts/MDVisionDefaultCommon.js")
                 .Include("~/Scripts/MDVisionDefaultiTrack.js")
                 .Include("~/Controls/Patient/Patient_Search.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Demographic_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Demographic.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Family.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Family_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Preferences.js")
                 .Include("~/Controls/Patient/Demographics/Patient_AdvancePayment.js")
                 .Include("~/Controls/Patient/Demographics/Patient_School.js")
                 .Include("~/Controls/Patient/Demographics/Patient_School_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_AdvancePayment_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_EmergencyContact.js")
                 .Include("~/Controls/Patient/Demographics/Patient_EmergencyContact_Detail.js")
                 .Include("~/Controls/Patient/Demographics/Patient_DemographicLabel.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Demographic_Quick.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Guarantor.js")
                 .Include("~/Controls/Patient/Demographics/Patient_Guarantor_Detail.js")
                 .Include("~/Controls/Patient/PatientPortal/Patient_AccountManager.js")
                 .Include("~/Controls/Patient/Case/Patient_Case.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_Visits.js")
                 .Include("~/Controls/Patient/Case/Patient_Case_Detail.js")
                 .Include("~/Controls/Batch/Batch_Fax.js")
                  .Include("~/Controls/Batch/Batch_Fax_QuickLink.js")
                 .Include("~/Controls/Batch/Batch_FaxSendAnnotate.js")
                 .Include("~/Controls/Batch/Batch_FaxSend.js")
                 .Include("~/Controls/Batch/Batch_FaxContacts.js")
                 .Include("~/Controls/Batch/Batch_FaxView.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_ChargeCapture.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Scan.js")
                 .Include("~/Controls/Patient/Document/Patient_DocumentTag.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Import.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Viewer.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Export.js")
                 .Include("~/Controls/Patient/Document/Patient_Information_Submission.js")
                 .Include("~/Controls/Patient/Document/Patient_Documents_Audit.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_AssignedTo.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Search.js")
                 .Include("~/Controls/Patient/Messages/Patient_Demographic_Quick.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Edit.js")
                 .Include("~/Controls/Patient/Messages/Patient_UserMessagesQuickLink.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Create.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Compose.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Alert.js")
                 .Include("~/Controls/Patient/Messages/Patient_Message_Add.js")
                 .Include("~/EMR/AppScripts/Patient/Patient_CustomForm.js")
                 .Include("~/EMR/AppScripts/Clinical/FaceSheet/Clinical_FaceSheetView.js")
                 .Include("~/EMR/AppScripts/Clinical/FaceSheet/Clinical_FaceSheetComponentSelection.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Referral_Search.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Insurance.js")
                 .Include("~/Controls/Patient/Insurance/Patient_PreAuthorization.js")
                 .Include("~/Controls/Patient/Insurance/Patient_PreAuthorization_Detail.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Referral.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Eligibility.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Eligibility_Detail.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_CreateClaim.js")
                 .Include("~/Controls/Patient/Encounter/Encounter_ClaimSummary.js")
                 .Include("~/Controls/Patient/Document/Patient_Document_Search.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Lawyer.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Lawyer_Detail.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Employer.js")
                 .Include("~/Controls/Patient/Insurance/Patient_Employer_Detail.js")
                 .Include("~/Controls/Admin/Admin_BillingProvider.js")
                 .Include("~/Controls/Admin/Admin_BillingProvider_Detail.js")
                 .Include("~/EMR/AppScripts/Common/Clinical_ProblemLists.js")
                 .Include("~/Controls/Scheduling/Scheduling_CheckIn.js")
                 .Include("~/Controls/Scheduling/Scheduling_Copayment.js")
                 .Include("~/Controls/Scheduling/Scheduling_Appointment_History.js")
                 .Include("~/Controls/Scheduling/Scheduling_UnallocatedCopayment.js")
                 .Include("~/Controls/Scheduling/Scheduling_UnallocatedCopayment_Search.js")
                 .Include("~/Controls/Scheduling/Scheduling_Unallocated_CopaymentView.js")
                 .Include("~/Controls/Scheduling/Scheduling_Appointment_Detail.js")
                 .Include("~/Controls/Scheduling/Scheduling_CopaymentView.js")
                 .Include("~/CCM/AppScripts/CCM_Patient_Hub.js")
                 .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_Problems.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/OutOfOfficeVisits.js")

                 .Include("~/EMR/AppScripts/Clinical/Orders/Clinical_LabOrderDetail.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/BillingInformation.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_Notes.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_ProgressNote.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/ENMCodeSuggest.js")
                 .Include("~/EMR/AppScripts/Clinical/BillingInformation/Clinical_SuperBillTemplate.js")
                 .Include("~/Controls/Admin/Admin_PlaceOfService.js")
                 .Include("~/Controls/Admin/Admin_PlaceOfService_Detail.js")
                 .Include("~/Controls/Clinical/LetterDesign/Design_LetterPrinting.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_NotesView.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_NotesCoSign.js")
                 .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_NotesAmendment.js")
                .Include("~/Controls/Patient/Messages/Patient_Message.js")
                .Include("~/Controls/Patient/Document/Patient_Document.js")

                .Include("~/EMR/AppScripts/Common/Restrict_User.js")
                .Include("~/EMR/AppScripts/Common/Clinical_FaceSheet.js")
                .Include("~/EMR/AppScripts/iTrack/MIPS_AdminPreferenceGroup.js")
                .Include("~/EMR/AppScripts/iTrack/MIPS_AdminPreferenceGroupDetail.js")
                .Include("~/EMR/AppScripts/iTrack/iTrack_AdminIPPreference.js")
                .Include("~/Controls/Billing/FollowUp/Bill_FollowUpARComments.js")
                .IncludeDirectory("~/CCM/AppScripts", "*.js", true)
           );
        }

        private static void AdminPageBundles(BundleCollection bundles)
        {
            /*.Include("~/Controls/Patient/Demographics/Patient_DemographicLabel.js")
           .Include("~/Controls/Patient/Demographics/Patient_Demographic.js")
           */

            bundles.Add(new ScriptBundle("~/bundles/StartupAdmin")
                    .Include("~/Scripts/theme.js")
                    .Include("~/Scripts/ApplicationCommand/StartupAdmin.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/AdminFiles")
                    .Include("~/EMR/AppScripts/iTrack/iTrack_AdminPreferenceIndividualReporting.js")
                     .Include("~/EMR/AppScripts/iTrack/iTrack_AdminPreferenceIndividualReportingDetail.js")
                     .Include("~/EMR/AppScripts/iTrack/iTrack_AdminPreferenceGroupReporting.js")
                     .Include("~/EMR/AppScripts/iTrack/iTrack_AdminPreferenceGroupReportingDetail.js")
                    .Include("~/Scripts/MDVisionDefaultAdmin.js")

                    .Include("~/EMR/AppScripts/Clinical/PhysicalExam/Clinical_PhysicalExamObservations.js")
                    .Include("~/EMR/AppScripts/Clinical/PhysicalExam/Clinical_PhysicalExamObservationsDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/PhysicalExam/Clinical_PhysicalExamSystems.js")
                    .Include("~/EMR/AppScripts/Clinical/PhysicalExam/Clinical_PhysicalExamSystemsDetail.js")

                    .Include("~/EMR/AppScripts/Clinical/ReviewOfSystem/Clinical_ROSCharatristics.js")
                    .Include("~/EMR/AppScripts/Clinical/ReviewOfSystem/Clinical_ROSCharatristicsDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/ReviewOfSystem/Clinical_ROSSystems.js")
                    .Include("~/EMR/AppScripts/Clinical/ReviewOfSystem/Clinical_ROSSystemsDetail.js")

                    .Include("~/EMR/AppScripts/Clinical/AuditReport/Clinical_AuditReport.js")
                    .Include("~/EMR/AppScripts/Clinical/AuditReport/Clinical_AuditReportView.js")

                    .Include("~/EMR/AppScripts/Clinical/ReportHeader/Clinical_SearchReportHeaderTemplate.js")
                    .Include("~/EMR/AppScripts/Clinical/ReportHeader/Clinical_AddReportHeaderTemplate.js")
                    .Include("~/EMR/AppScripts/Clinical/ReportHeader/Clinical_PreviewReportHeaderTemplate.js")

                    .Include("~/EMR/AppScripts/Clinical/CDS/Clinical_CDS.js")
                    .Include("~/EMR/AppScripts/Clinical/CDS/Clinical_CDSDetail.js")

                    .Include("~/EMR/AppScripts/Clinical/PQRSAdmin/PQRS_MeasureGroups.js")
                    .Include("~/EMR/AppScripts/Clinical/PQRSAdmin/PQRS_MeasureGroups_Detail.js")
                    .Include("~/EMR/AppScripts/Clinical/PQRSAdmin/PQRS_IndividualReporting.js")
                    .Include("~/EMR/AppScripts/Clinical/PQRSAdmin/PQRS_IndividualReporting_Detail.js")

                    .Include("~/EMR/AppScripts/Clinical/Lab/Clinical_LabDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Lab/Clinical_LabTestAttributes.js")
                    .Include("~/EMR/AppScripts/Clinical/Lab/Clinical_LabTestAttributeResult.js")


                    .Include("~/EMR/AppScripts/Clinical/ReviewOfSystem/ROSTemplateRevamp.js")
                    .Include("~/EMR/AppScripts/Clinical/ReviewOfSystem/ROSTemplateDetailRevamp.js")
                    .Include("~/EMR/AppScripts/Clinical/CustomForms/Clinical_CustomForms.js")
                    .Include("~/EMR/AppScripts/Clinical/CustomForms/Clinical_CustomFormsDetails.js")
                    .Include("~/EMR/AppScripts/Clinical/CustomForms/Clinical_CustomFormsPreview.js")
                    .Include("~/EMR/AppScripts/Clinical/CustomForms/Clinical_GlobalQuestionGroup.js")
                    .Include("~/EMR/AppScripts/Clinical/CustomForms/Clinical_GlobalQuestionDetail.js")
                    .Include("~/EMR/AppScripts/iTrack/MIPSPreference_IndividualProvider.js")
                    .Include("~/EMR/AppScripts/iTrack/MIPS_AdminPreferenceGroup.js")
                    .Include("~/EMR/AppScripts/iTrack/MIPS_AdminPreferenceGroupDetail.js")
                    .Include("~/EMR/AppScripts/iTrack/iTrack_AdminIPPreference.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/Clinical_Provider_Note_Template.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/Clinical_Add_Provider_Note_Template.js")
                    .Include("~/EMR/AppScripts/Clinical/TemplateBuilderNew/Letter_Template.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/PhysicalExamTemplate.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/PhysicalExamTemplateDetailRevamp.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/AOETemplate.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/AOETemplateDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/ProcedureTemplate.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/ProcedureTemplateDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/Clinical_OrderSets.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/Clinical_OrderSetDetails.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/Clinical_HPISymptoms.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/Clinical_HPISymptomsDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/Clinical_HPIFindings.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/Clinical_HPIFindingsDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/Clinical_HPIFindings.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/Clinical_HPIFindingsDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/HPITemplate.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/HPITemplateDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_Problems.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_Medications.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_ProblemListGrid.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_ImmunizationDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_TherapeuticDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_Procedures.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_LabOrderDetails.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_RadiologyOrderDetails.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_ProcedureOrderDetails.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_FollowUp.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_PatientEducation.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_Patient_Referrals_Outgoing_Detail.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_ProceduresGrid.js")
                    .Include("~/EMR/AppScripts/Clinical/Templates/OrderSet/OrderSet_LabOrder.js")

                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_VaccineCrosswalk.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_Registery.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_RegisteryDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_Category.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_ScheduleSetup.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_LotNumber.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_LotNumberDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_LotTypeSelection.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_Manufacturer.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_ManufacturerDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_ImmunizationAddImmInj.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_SearchVaccine.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_VaccineTypeSelection.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_TherapeuticDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Immunization/Immunization_AddVaccine.js")

                    .Include("~/EMR/AppScripts/Clinical/InfoButton/Clinical_InfoButtonView.js")

                    .Include("~/EMR/AppScripts/Clinical/Notes/Clinical_ProgressNote.js")

                    .Include("~/EMR/AppScripts/Clinical/Medical/Clinical_Procedures.js")
                    //AST-15 BY:MAhmad
                    .Include("~/EMR/AppScripts/Clinical/Medical/Clinical_ProceduresComments.js")
                    //AST-15 BY:MAhmad
                    .Include("~/EMR/AppScripts/Clinical/Orders/Clinical_ConsultationOrder.js")
                    .Include("~/EMR/AppScripts/Clinical/Orders/Clinical_LabOrderDetail.js")
                    .Include("~/EMR/AppScripts/Clinical/Orders/Clinical_RadiologyOrder.js")
                    .Include("~/EMR/AppScripts/Clinical/Orders/ClinicalLabOrderDetailAOE.js")
                    //AST-14 BY:MAhmad
                    .Include("~/EMR/AppScripts/Clinical/Medical/Clinical_ProblemListsComments.js")
                    //AST-14 BY:MAhmad

                    .Include("~/EMR/AppScripts/Clinical/LOINC/Clinical_LOINC.js")


                    .Include("~/EMR/AppScripts/Common/Clinical_Lab.js")
                    .Include("~/EMR/AppScripts/Clinical/Macros/Clinical_Macro.js")
                    .Include("~/EMR/AppScripts/Clinical/Macros/Clinical_MacroDetail.js")
                    .Include("~/EMR/AppScripts/Common/Add_Letter_Template.js")
                    .Include("~/EMR/AppScripts/Common/Clinical_ProblemLists.js")
                    .Include("~/EMR/AppScripts/Common/Clinical_LabOrder.js")

                    .Include("~/Controls/Scheduling/Scheduling_Search.js")
                    .Include("~/Controls/Scheduling/Scheduling_Appointment_Detail.js")
                    .Include("~/Controls/Scheduling/Scheduling_SlotBlockUnblock.js")

                    .Include("~/Controls/Patient/Patient_Search.js")
                    .Include("~/EMR/AppScripts/Clinical/PQRSAdmin/PQRS_CMSView.js")
                    .Include("~/CCM/AppScripts/CCM_Patient_Hub.js")

                    .IncludeDirectory("~/Controls/Admin", "*.js", true)
                    .IncludeDirectory("~/EMR/AppScripts/Clinical/Favorites", "*.js", false)

                    );
        }

        private static void CommonBundles(BundleCollection bundles)
        {
            #region css
            bundles.Add(new StyleBundle("~/bundles/Default")
                .Include("~/Content/Default/connect.css")
                .Include("~/Content/Default/jquery-ui.css")
                .Include("~/Content/Default/jasny-bootstrap.css")
                .Include("~/Content/Default/timepicker.css")
                .Include("~/Content/Default/bootstrap-colorpicker.css")
                .Include("~/Content/Default/jquery-ui.css")
                .Include("~/Content/Default/pqgrid.min.css")
                .Include("~/Content/Default/jquery.toastmessage.css")
                .Include("~/Content/Default/bootstrap-colorpicker.css")
                .Include("~/Content/Default/fullcalendar.css")
                .Include("~/Content/Default/fullcalendar.print.css")
                .Include("~/Content/Default/animate.css")
                .Include("~/Content/Default/tipso.css")
                .Include("~/Content/Default/tooltipster.css")
                .Include("~/Content/Default/jstree.css")
                .Include("~/Content/Default/morris.css")
                .Include("~/Content/Default/slick.css")
                .Include("~/Content/Default/jquery.autocomplete.css")
                .Include("~/Content/Default/scrolltabs.css")
                .Include("~/Content/Default/VirtualKeyboard/keyboard.css")
                .Include("~/Content/Default/jqGrid/ui.jqgrid.css")
                .Include("~/Content/HighCharts/css/highcharts.css")
                .Include("~/Content/Mention TinyMCE/css/autocomplete.css")
                .Include("~/Content/Mention TinyMCE/css/rte-content.css")
                //.Include("~/Content/Kendo/kendo.common.min.css")
                //.Include("~/Content/Kendo/kendo.default.min.css")
                //.Include("~/Content/Kendo/kendo.default.mobile.min.css")
                .Include("~/Content/Kendo/kendo-custom.css")
                .Include("~/Content/Kendo/theme-kendo.css")

                //.Include("~/Content/Default/jquery.gridster.min.css")
                //.Include("~/Scripts/s/literallycanvas/css/literallycanvas.css")
                //.Include("~/Scripts/s/literallycanvas/css/sly-vertical.css")
                );

            bundles.Add(new StyleBundle("~/bundles/Blue")
                .Include("~/Content/Blue/theme.css")
                .Include("~/Content/Blue/default.css")
                .Include("~/Content/Blue/theme-custom.css"));


            bundles.Add(new StyleBundle("~/bundles/Black")
               .Include("~/Content/Black/theme.css")
               .Include("~/Content/Black/default.css")
               .Include("~/Content/Black/theme-custom.css"));

            bundles.Add(new StyleBundle("~/bundles/Gray")
               .Include("~/Content/Gray/theme.css")
               .Include("~/Content/Gray/default.css")
               .Include("~/Content/Gray/theme-custom.css"));

            bundles.Add(new StyleBundle("~/bundles/Medlineplus")
                .Include("~/Content/medlineplus/connect.css"));

            #endregion

            #region js

            bundles.Add(new ScriptBundle("~/bundles/Common")
                  .Include("~/Controls/Scheduler/PMSScheduler.js")
              .Include("~/Scripts/ApplicationCommand/EMRUtility.js")
             .Include("~/Scripts/CacheManager.js")
             .Include("~/Scripts/ApplicationCommand/AppSecurity.js")
             .Include("~/Scripts/ApplicationCommand/ControlName.js")
             .Include("~/Scripts/ApplicationCommand/MDVisionServices.js")
             .Include("~/Scripts/ApplicationCommand/store.js")
             .Include("~/Scripts/ApplicationCommand/utility.js")
             .Include("~/Scripts/ApplicationCommand/StaticLookups.js")
             .Include("~/Scripts/ApplicationCommand/AppCommands.js")
             .Include("~/Controls/Patient/Demographics/Patient_Demographic_PrintView.js")
             .Include("~/Controls/DashBoard/DashBoard.js")
             .Include("~/Controls/DashBoard/DashBoardSetting.js")
             .Include("~/Controls/DashBoard/DashBoardChangePass.js")
             .IncludeDirectory("~/Controls/CommonControls", "*.js", true)

         );
            ScriptBundle js3rdParty = new ScriptBundle("~/bundles/3rdPartyjs");
            js3rdParty.Transforms.Clear();

            js3rdParty
               .Include("~/Scripts/js/jquery-2.1.1.min.js")
               .Include("~/Scripts/js/bootstrap.min.js")
               .Include("~/Scripts/js/jquery-ui.min.js")
               .Include("~/Scripts/js/jquery.ui.touch-punch.min.js")
               .Include("~/Scripts/js/moment.min.js")
               .Include("~/Scripts/js/fullcalendar.min.js")
               //multiselect
               .Include("~/Scripts/js/bootstrap-multiselect.min.js")
               //.Include("~/Scripts/js/pqgrid.min.js")
               .Include("~/Scripts/js/jquery.dataTables.min.js")
               .Include("~/Scripts/js/amplify.core.min.js")
               .Include("~/Scripts/js/amplify.store.min.js")
               .Include("~/Scripts/js/bootstrap-datepicker.min.js")
               .Include("~/Scripts/js/timepicker.min.js")
               .Include("~/Scripts/js/bootstrap-colorpicker.min.js")
               .Include("~/Scripts/js/bootstrapValidator.min.js")
               .Include("~/Scripts/js/jasny-bootstrap.min.js")

               //these Files are not available in solution
               .Include("~/Scripts/js/datatables.default.min.js")
               .Include("~/Scripts/js/datatables.min.js")
               //END
               .Include("~/Scripts/js/jquery.browser.mobile.min.js")
               .Include("~/Scripts/js/jquery.jsticky.min.js")
               .Include("~/Scripts/js/jquery.placeholder.min.js")
               .Include("~/Scripts/js/json2.min.js")
               .Include("~/Scripts/js/bootstrap-colorpicker.min.js")
               .Include("~/Scripts/js/modernizr.min.js")
               .Include("~/Scripts/js/nanoscroller.min.js")
               .Include("~/Scripts/js/select2.min.js")
               .Include("~/Scripts/js/jquery.blockUI.min.js")
               .Include("~/Scripts/js/theme.custom.js")
               .Include("~/Scripts/js/theme.init.js")
               .Include("~/Scripts/js/jquery.toastmessage.min.js")
               .Include("~/Scripts/js/jquery.autocomplete.min.js")
               .Include("~/Scripts/js/jszip.min.js")
               .Include("~/Scripts/js/FileSaver.min.js")
               .Include("~/Scripts/js/scanner_init.js")
               .Include("~/Scripts/js/scanner_operation.js")
               .Include("~/Scripts/js/jquery.ba-resize.min.js")
               //.Include("~/Scripts/js/swfobject.js")
               //.Include("~/Scripts/js/scriptcam.js")
               .Include("~/Scripts/js/jquery.maskMoney.min.js")
               .Include("~/Scripts/js/jquery.tooltipster.min.js")
                 //.Include("~/Scripts/js/tinyMCE/js/tinymce/tinymce.min.js")
                 .Include("~/Scripts/js/jstree.js")
               .Include("~/Scripts/js/jquery.mousewheel.min.js")
               .Include("~/Scripts/js/jquery.jscrollpane.min.js")
               .Include("~/Scripts/js/jquery-printme.min.js")
               .Include("~/Scripts/js/dataTables.fixedHeader.min.js")
               .Include("~/Scripts/js/jquery.slimscroll.min.js")
               .Include("~/Scripts/js/ios7-switch.min.js")
               .Include("~/Scripts/js/morris.min.js")
               .Include("~/Scripts/js/raphael.js")
               .Include("~/Scripts/js/slick.min.js")
               .Include("~/Scripts/js/Shortcut.min.js")
               .Include("~/Scripts/js/jquery.base64.min.js")
               .Include("~/Scripts/js/jquery.scrolltabs.min.js")
               .Include("~/Scripts/js/VirtualKeyboard/jquery.keyboard.min.js")
               .Include("~/Scripts/js/VirtualKeyboard/jquery.mousewheel.min.js")
               .Include("~/Scripts/js/VirtualKeyboard/jquery.keyboard.extension-typing.min.js")
               .Include("~/Scripts/js/VirtualKeyboard/jquery.keyboard.extension-autocomplete.min.js")
               .Include("~/Scripts/js/Blob.min.js")
               .Include("~/Scripts/js/download.min.js")
               //  .Include("~/Scripts/js/jszip.js")
               //.Include("~/Scripts/js/jqGrid/jquery.jqGrid.min.js")
               //.Include("~/Scripts/js/jqGrid/grid.locale-en.js")
               .Include("~/Scripts/js/idle-timer.min.js")
               .Include("~/Scripts/js/find5.min.js")
               .Include("~/Scripts/js/signature_pad.js")
               .Include("~/Scripts/js/jquery-barcode.min.js")
               .Include("~/Scripts/js/jquery.gridster.min.js")
               //.Include("~/Scripts/js/gridster.resize-patch.js")
               .Include("~/Scripts/js/clipboard.min.js")

               .Include("~/Scripts/js/dymo.label.framework.js")

               .Include("~/Scripts/js/jspdf.debug.min.js")
               .Include("~/Scripts/js/react-with-addons.min.js")
               .Include("~/Scripts/js/react-dom.min.js")
               .Include("~/Scripts/js/jquery-easing-plugins.min.js")
               .Include("~/Scripts/js/literallycanvas/js/literallycanvas.min.js")

               .Include("~/Scripts/js/jquery.signalR-2.2.1.min.js")
               //  .Include("~/Scripts/js/sly.js")

               .Include("~/Scripts/js/HighCharts/code/js/highcharts.js")
               .Include("~/Scripts/js/HighCharts/code/js/highcharts-3d.js")

               // .Include("~/Scripts/js/Kendo/kendo.all.min.js")

               //.Include("~/Scripts/js/pdf/build/pdf.min.js")
               //.Include("~/Scripts/js/pdf/build/pdf.min.worker.js")
               .Include("~/Scripts/js/multiselectlist.min.js")
               .Include("~/Scripts/js/Scripts/js/MutationObserver.min.js")

               .Include("~/Scripts/js/sly.min.js")
               .Include("~/Scripts/js/swfobject.js")
               .Include("~/Scripts/js/scriptcam.js")

               .IncludeDirectory("~/Scripts/OCRScanner", "*.min.js", false)
               .IncludeDirectory("~/Scripts/js/eSignature", "*.min.js", false);

            bundles.Add(js3rdParty);

            #endregion

            #region CommonAdminFile4Other
            bundles.Add(new ScriptBundle("~/bundles/CommonAdminFile4Other")
                    .Include("~/Controls/Admin/Admin_Facility.js")
                    .Include("~/Controls/Admin/Admin_Facility_Detail.js")
                    .Include("~/Controls/Admin/Admin_IMOICD.js")
                    .Include("~/Controls/Admin/Admin_Provider.js")
                    .Include("~/Controls/Admin/Admin_Provider_Detail.js")
                    .Include("~/Controls/Admin/Admin_Practice_Detail.js")
                    .Include("~/Controls/Admin/Admin_ReferringProvider.js")
                    .Include("~/Controls/Admin/Admin_ReferringProvider_Detail.js")
                    .Include("~/Controls/Admin/Admin_InsurancePlan.js")
                    .Include("~/Controls/Admin/Admin_InsurancePlanAddress.js")
                    .Include("~/Controls/Admin/Admin_CPTCode.js")
                    .Include("~/Controls/Admin/Admin_OccupationStatus.js")
                    .Include("~/Controls/Admin/Admin_Provider_eSignature.js")
                    .Include("~/Controls/Admin/Admin_InsurancePlan_Detail.js")
                    .Include("~/Controls/Admin/Admin_EDISubmitInsurance.js")
                    .Include("~/Controls/Admin/Admin_EDIEligibilityInsurance.js")
                    .Include("~/Controls/Admin/Admin_Insur.js")
                    .Include("~/Controls/Admin/Admin_EDISubmitInsurance_Detail.js")
                    .Include("~/Controls/Admin/Admin_EDIEligibilityInsurance_Detail.js")
                    .Include("~/Controls/Admin/Admin_ParticipentProvider.js")
                    .Include("~/Controls/Admin/Admin_Specialty.js")
                    .Include("~/Controls/Admin/Admin_Specialty_Detail.js")
                    .Include("~/Controls/Admin/Admin_ParticipentProviderDetail.js")
                    .Include("~/Controls/Admin/Admin_Folder_Detail.js")
                    .Include("~/Controls/Admin/Admin_SupperBillDetail.js")
                    .Include("~/Controls/Admin/Admin_IMOCPT.js")
                    .Include("~/Controls/Admin/Admin_IMOSearch_Detail.js")
                    .Include("~/Controls/Admin/Admin_CPTCode_Detail.js")
                    .Include("~/Controls/Admin/Admin_ICD_Detail.js")
                    .Include("~/Controls/Admin/Admin_Modifier.js")
                    .Include("~/Controls/Admin/Admin_Reminders_Detail.js")
                    .Include("~/Controls/Admin/Admin_Practice.js")
                    .Include("~/Controls/Admin/Admin_Practice_Detail.js")
                    .Include("~/Controls/Admin/Admin_SecurityRole_Detail.js")
                    .Include("~/Controls/Admin/Admin_Insurance_Detail.js")
                    .Include("~/Controls/Admin/Admin_BlockHours.js")
                    .Include("~/Controls/Admin/Admin_BlockHours_Detail.js")
                    .Include("~/Controls/Admin/Admin_Resources.js")
                    .Include("~/Controls/Admin/Admin_Resources_Detail.js")
                    .Include("~/Controls/Admin/Admin_TypeOfService.js")
                );

            #endregion
        }

        private static void DocumentScanBundles(BundleCollection bundles)
        {
            #region " CSS "

            StyleBundle ScanCSS = new StyleBundle("~/bundles/ScanCSS");
            ScanCSS.Transforms.Clear();

            ScanCSS.Include("~/Content/Default/bootstrap.css")
                   .Include("~/Scripts/js/literallycanvas/css/literallycanvas.css")
                   .Include("~/Scripts/js/literallycanvas/css/sly-vertical.css")
                   .Include("~/Content/Common/style-common.css");

            bundles.Add(ScanCSS);

            #endregion

            #region " JS "

            ScriptBundle ScanJS = new ScriptBundle("~/bundles/ScanJS");
            ScanJS.Transforms.Clear();

            ScanJS.Include("~/Scripts/ApplicationCommand/store.js")
                  .Include("~/Scripts/ApplicationCommand/utility.js")
                  .Include("~/Scripts/ApplicationCommand/AppCommands.js")
                  .Include("~/Scripts/ApplicationCommand/MDVisionServices.js")
                  .Include("~/Scripts/ApplicationCommand/StaticLookups.js")
                  .Include("~/Scripts/ApplicationCommand/EMRUtility.js")
                  .Include("~/Scripts/CacheManager.js")
                  .Include("~/Controls/Admin/Admin_Practice_Detail.js")
                  .Include("~/Controls/Patient/Demographics/Patient_Demographic.js")
                  .Include("~/Controls/Patient/Demographics/Patient_Demographic_Detail.js")
                  .Include("~/Controls/Patient/Demographics/Patient_Demographic_Quick.js")
                  .Include("~/Controls/Patient/Insurance/Patient_Insurance.js")
                  .Include("~/Controls/Patient/Insurance/OCR_Scanner.js")
                  .Include("~/Controls/Patient/Document/Patient_Document_Import.js")
                  .Include("~/Controls/Patient/Document/Patient_Document_Export.js")
                  .Include("~/Controls/Patient/Document/Patient_Document.js")
                  .Include("~/Controls/Patient/Document/Patient_Document_Scan.js")
                  .Include("~/Controls/Patient/Document/Patient_Document_Viewer.js")
                  .Include("~/Controls/Patient/Document/Patient_DocumentTag.js")
                  .Include("~/Scripts/js/scanner_init.js")
                  .Include("~/Scripts/js/scanner_operation.js")
                  .Include("~/Scripts/js/dynamsoft.imagecapturesuite.initiate.js")
                  .Include("~/Scripts/js/dynamsoft.imagecapturesuite.initiate.min.js")
                  .Include("~/Scripts/theme.js")
                  .Include("~/Scripts/js/tinyMCE/js/tinymce/tinymce.min.js")
                  .Include("~/Scripts/ApplicationCommand/StartupDocumentScan.js");

            ScriptBundle Scan3rdPartyJS = new ScriptBundle("~/bundles/Scan3rdPartyJS");
            Scan3rdPartyJS.Transforms.Clear();

            Scan3rdPartyJS
               .Include("~/Scripts/js/jquery-2.1.1.min.js")
               .Include("~/Scripts/js/bootstrap.min.js")
               .Include("~/Scripts/js/jquery-ui.min.js")
               .Include("~/Scripts/js/bootstrap-multiselect.min.js")
               .Include("~/Scripts/js/amplify.core.min.js")
               .Include("~/Scripts/js/amplify.store.min.js")
               .Include("~/Scripts/js/bootstrap-datepicker.min.js")
               .Include("~/Scripts/js/bootstrapValidator.min.js")
               .Include("~/Scripts/js/jasny-bootstrap.min.js")
               .Include("~/Scripts/js/jquery.placeholder.min.js")
               .Include("~/Scripts/js/json2.min.js")
               .Include("~/Scripts/js/modernizr.min.js")
               .Include("~/Scripts/js/nanoscroller.min.js")
               .Include("~/Scripts/js/jquery.blockUI.min.js")
               .Include("~/Scripts/js/theme.custom.js")
               .Include("~/Scripts/js/theme.init.js")
               .Include("~/Scripts/js/jquery.toastmessage.min.js")
               .Include("~/Scripts/js/jquery.autocomplete.min.js")
               .Include("~/Scripts/js/jszip.min.js")
               .Include("~/Scripts/js/scanner_init.js")
               .Include("~/Scripts/js/scanner_operation.js")
               .Include("~/Scripts/js/Shortcut.min.js")
               .Include("~/Scripts/js/jquery.base64.min.js")
               .Include("~/Scripts/js/dymo.label.framework.js")
               .Include("~/Scripts/js/jspdf.debug.min.js")
               .IncludeDirectory("~/Scripts/OCRScanner", "*.min.js", false)
               .IncludeDirectory("~/Scripts/js/eSignature", "*.min.js", false);



            #endregion

            bundles.Add(ScanJS);
            bundles.Add(Scan3rdPartyJS);
        }

    }
}
