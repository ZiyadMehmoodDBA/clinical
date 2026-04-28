using System.Collections.Generic;

namespace MDVision.Model.User
{
    public class UserLoginModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string ResetPassword { get; set; }
        public string EntityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProviderId { get; set; }
        public string PhoneNo { get; set; }
        public string PhoneExt { get; set; }
        public string EmailAddress { get; set; }
        public string UserRoleId { get; set; }
        public string IsActive { get; set; }
        public string IsAdmin { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string AutoLogOff { get; set; }
        public string IsEMR { get; set; }
        public string DirectAddress { get; set; }
        public string IsDirectAddress { get; set; }
        public string ErrorMessage { get; set; }
        public string MessagesCount { get; set; }
        public string UserTasksCount { get; set; }
        public string RecordCount { get; set; }
        public string NotesCount { get; set; }
        public string AppointmentsCount { get; set; }
        public string PendingDocumentsCount { get; set; }
        public string EmergencyRoleId { get; set; }
        public string RCopialUser { get; set; }
        public string RcUserName { get; set; }
        public string RcPassword { get; set; }
        public string RcSigPassword { get; set; }
        public string IsLocked { get; set; }
        public string CoWorkersGroupId { get; set; }
        public string isFirstTimeloggedIn { get; set; }
        public string IsNoteUnSign { get; set; }
        public string IsFullSSN { get; set; }
        public string IsMedText{ get; set; }
        public string IsCollection { get; set; }
        public string DocumentPendingCount { get; set; }
        public string isSendBillingInquiry { get; set; }
        public string AssignedResultsCount { get; set; }
    }

    public class LoggedInUserModel
    {
        public bool IsLogedIn { get; set; }
    }
    public class EntityUserOptions
    {
        public string EntityUserOptionId { get; set; }
        public string UserId { get; set; }
        public string EntityId { get; set; }
        public string Appointmentstatus { get; set; }
        public string EntityRegCode { get; set; }
        public string IdleTime { get; set; }
        public string MaxPatientOpen { get; set; }
        public string MaxAttempt { get; set; }
        public string AccountLockActionType { get; set; }
        public string AccountLockUnlockTime { get; set; }
        public string AllowMultipleLogins { get; set; }
        public string SchWeekDay { get; set; }
        public string IsDefault { get; set; }
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string FacilityDescription { get; set; }
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string PracticeId { get; set; }
        public string PracticeName { get; set; }
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string IsActive { get; set; }
        public string IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string PreferredScreen { get; set; }
        public string PreferredSchScreen { get; set; }
        public string DefaultTemplate { get; set; }
        public string DefaultSuperBill { get; set; }
        public string PBPhoneNumber1 { get; set; }
        public string PBPhoneNumber2 { get; set; }
        public string PBPCP { get; set; }
        public string PBRefProvider { get; set; }
        public string PBPlanBalance { get; set; }
        public string PBPatientBalance { get; set; }
        public string ClaimPrinter { get; set; }
        public string ClaimTray { get; set; }
        public string AttachmentPrinter { get; set; }
        public string AttachmentTray { get; set; }
        public string PreferredSchScreenName { get; set; }
        public string RefreshTime { get; set; }
        public string ThemeId { get; set; }
        public string ThemeName { get; set; }
        public string SchedulePattern { get; set; }
        public string IsSearchPatient { get; set; }
        public string IsQuickAddPatient { get; set; }
        public string IsAppointment { get; set; }
        public string IsNote { get; set; }
        public string IsFax { get; set; }
        public string IsPatientName { get; set; }
        public string IsPatientDOB { get; set; }
        public string IsPatientAddress { get; set; }
        public string IsInsurancePlan { get; set; }
        public string IsSubscriberID { get; set; }
        public string IsPatientAccountNo { get; set; }
        public string IsProvider { get; set; }
        public string IsTask { get; set; }
        public string IsMessage { get; set; }
        public string BillingProviderId { get; set; }
        public string IsPrescriptionsRefill { get; set; }
        public string IsPendingPrescriptions { get; set; }
        public string RecentPatient { get; set; }
        public string ENMCodesTime { get; set; }
        public string FavListNames { get; set; }
        public string FreeTextICD { get; set; }
        public string PBPatientAdvanceBalance { get; set; }
        public string PBPrimaryInsurance { get; set; }
        public string IsCurrentMedications { get; set; }
        public string IsPastMedications { get; set; }
        public string IsActiveAllergies { get; set; }
        public string IsInactiveAllergies { get; set; }
        public string IsActiveProblems { get; set; }
        public string IsInactiveProblems { get; set; }
        public string IsVitals { get; set; }
        public string IsImmunizations { get; set; }
        public string IsFamilyHistory { get; set; }
        public string IsSocialHistory { get; set; }
        public string IsMedicalHistory { get; set; }
        public string IsSurgicalHistory { get; set; }
        public string IsBirthHistory { get; set; }
        public string IsHospitalizationHistory { get; set; }
        public string PreferredPhone { get; set; }
        public string BillingProviderName { get; set; }
        public string PreferredScreenName { get; set; }
        public string FavProceduresVal { get; set; }
        public string FavProblemsVal { get; set; }
        public string FavMedicalHxVal { get; set; }
        public string FavFamilyHxVal { get; set; }
        public string FavSurgicalHxVal { get; set; }
        public string FavLabOrderVal { get; set; }
        public string FavProcedureOrderVal { get; set; }
        public string FavRadiologyOrderVal { get; set; }
        public string FavConsultationVal { get; set; }
        public string FavComplaintVal { get; set; }
        public string IsImmunizationAlert { get; set; }
        public string IsDocumentsAlert { get; set; }
        public string IsSelectNoteComponents { get; set; }
        public string IsExpand { get; set; }
        public string WorkWeekDays { get;set;}
        public string IsSearchCriteriaExpand { get; set; }
        public string NoteFontSize { get; set; }
        public string IsShowICD10 { get; set; }
        public string IsActiveProcedures { get; set; }
        public string IsInActiveProcedures { get; set; }
        public string PrescriptionsRefillCount { get; set; }
        public string PendingPrescriptionsCount { get; set; }
        public string NotePrevieStyle { get; set; }
        public string PreferredBillingScreen { get; set; }
        public string PreferredBillingScreenName { get; set; }
        public string IsDefaultHPI { get; set; }
        public string FavImmunizationVal { get; set; }
        public string FavTherapeuticVal { get; set; }
        public string EMCodesTypeIds { get; set; }
        public string IsPBCollection { get; set; }
        public string DefaultDocumentPriorityId { get; set; }
        public string DefaultDocumentPriorityName { get; set; }
        public string RaceIds { get; set; }
        public string IsShowFacilityShortName { get; set; }
        public int SchedulerTimeInterval { get; set; }
        public string IsShowSuccessMessages { get; set; }
        public string IsReferralRequired { get; set; }
        public string IsDocument { get; set; }
        public string IsPrevNoteComplaints { get; set; }
        public string IsOrdersExpand { get; set; }
        public string IsResultsExpand { get; set; }
        public string IsDemographics { get; set; }
        public string IsMu3FamilyHistory { get; set; }
        public string IsHealthcareSurveys { get; set; }
        public string IsImmunizationRegistries { get; set; }
        public string IsPatientHealthInformationCapture { get; set; }
        public string IsTransimissionCaseReporting { get; set; }
        public string IsTransitionDirectProject { get; set; }
        public string IsDataSegmentationPrivacy { get; set; }
        public string IsImplantableDevices { get; set; }
        public string IsTransitCancerRegistries { get; set; }
        public string IsTransimissionAntimicobial { get; set; }
        public string IsConsolidatedCDA { get; set; }
        public string IsMU3SocPsyBehaviourHx { get; set; }
        public string IsDataExport { get; set; }
        public string IsCarePlan { get; set; }
        public string IsPrevNoteROS{ get; set; }
        public string IsPrevNotePE { get; set; }
        public string IsSelectCompOnCopyNote { get; set; }
        public string IsExpandFolderTree { get; set; }
        public string isPETemplateNameRequired { get; set; }
        public string IsConfigureAlerts { get; set; }
        public string iTrackDashboardIds { get; set; }
        public string IsPrevNoteProblems { get; set; }
        public string PBPatientCCMTimer { get; set; }
        public string IsBulkSign { get; set; }
        public string IsPrevNoteTreatmentComents { get; set; }
        public string IsCMS65v7 { get; set; }
        public string IsCMS69v6 { get; set; }
        public string IsCMS68v7  { get; set; }
        public string IsCMS138v6 { get; set; }
        public string IsCMS165v6 { get; set; }
        public string IsCMS22v6 { get; set; }
        public string IsCDS { get; set; }
        public string isLandOnComponent { get; set; }
        public string DefaultTabMedications { get; set; }
        public string DefaultTabMessages { get; set; }
        public string RecentMessagesTab { get; set; }
        public string IsNoteCompExpanded { get; set; }
        public string IsAssignedResults { get; set; }
    }

    public class UserPrivileges
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string ModuleId { get; set; }
        public string FormsId { get; set; }
        public string RoleName { get; set; }
        public string ModuleName { get; set; }
        public string FormName { get; set; }
        public string PrivilegeName { get; set; }
        public string IsPrivileged { get; set; }
        public string ModuleHTMLId { get; set; }
        public string FormHTMLId { get; set; }
        public string FormParentHTMLId { get; set; }
        public string ReportSSRSId { get; set; }
        public string IsEmergencyRole { get; set; }
    }

    public class UserModel
    {
        public List<UserLoginModel> UserLoginModel { get; set; }
        public List<EntityUserOptions> EntityUserOptions { get; set; }
        public List<UserPrivileges> UserPrivileges { get; set; }
    }

    public class UserMessages
    {
        public string PatMsgId { get; set; }
        public string PatientId { get; set; }
        public string PatientAccountName { get; set; }
        public string MsgDetail { get; set; }
        public string MessageType { get; set; }
        public string CallDate { get; set; }
        public string DOS { get; set; }
        public string MessageStatus { get; set; }
        public string AlertType { get; set; }
        public string AssigneeName { get; set; }
        public string PriorityName { get; set; }
        public string EntryDate { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string Medication { get; set; }
        public string Pharmacy { get; set; }
        public string LabOrder { get; set; }
        public string Lab { get; set; }
        public string AmendmentSource { get; set; }
        public string VisToPatient { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string PatDocId { get; set; }
        public string FilePath { get; set; }
        public string ProviderName { get; set; }
        public string FacilityName { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public string UserMessagesId { get; set; }
        public string IsRead { get; set; }
    }

    public class AppointmentVisits
    {
        public string Title { get; set; }
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Reason { get; set; }
        public string AppointmentTime { get; set; }
        public string VisitTime { get; set; }
        public string VisitDate { get; set; }
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string Duration { get; set; }
        public string minsWait { get; set; }
        public string AppointmentStatus { get; set; }
        public string Room { get; set; }
        public string VisitId { get; set; }
        public string NotesId { get; set; }
        public string NoteStatus { get; set; }
        public string NoteDate { get; set; }
        public string PatientType { get; set; }
        public string VisitType { get; set; }
        public string BillingInfoId { get; set; }
        public string BillingStatus { get; set; }
        public string ModifiedOn { get; set; }
        public string OrderByAppTime { get; set; }
        public string RecordCount { get; set; }
    }

    public class AppointmentNote
    {
        public string NotesId { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string VisitDate { get; set; }
        public string SchReasonId { get; set; }
        public string AppReason { get; set; }
        public string CC { get; set; }
        public string NoteStatus { get; set; }
        public string NoteType { get; set; }
        public string BillingInfoId { get; set; }
        public string AppointmentDate { get; set; }
        public string NoteDate { get; set; }
        public string VisitId { get; set; }
        public string PatientTypeId { get; set; }
        public string hourss { get; set; }
        public string CreatedBy { get; set; }
        public string RecordCount { get; set; }
    }

    public class UserMessagesCount
    {
        public string TotalCount { get; set; }
        public string TaskCount { get; set; }
        public string OtherCount { get; set; }

    }
}
