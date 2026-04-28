using MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder;
using MDVision.IEHR.EMR.Model.OrdersAndResults.RadiologyOrder;
using MDVision.Model.Clinical.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Notes
{
    public class ClinicalNotesModel
    {
        public string SyndromicType { get; set; }
        public string ShortName { get; set; }
        public string FacilityDescription { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string VisitReason { get; set; }
        public string NoteType { get; set; }
        public string NoteTypeText { get; set; }
        public string NoteTemplate { get; set; }
        public string ProviderId { get; set; }
        public string Provider { get; set; }
        public string FacilityId { get; set; }
        public string Facility { get; set; }
        public string FacilityName { get; set; }
        public string ResourceId { get; set; }
        public string ResourceProviderId { get; set; }
        public string ResourceProvider { get; set; }
        public string ResourceProviderName { get; set; }
        public string RoomNo { get; set; }
        public string RefProvider { get; set; }
        public bool IsLinkedAppointment { get; set; }
        public string LinkedAppointment { get; set; }
        public bool IsCopayPreviousNote { get; set; }
        public string CopayPreviousNote { get; set; }
        public string NoteText { get; set; }
        public string NoteStatus { get; set; }
        public string commandType { get; set; }
        public string NotesId { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public Int64 ClinicalNotesCount { get; set; }
        public Int64 iTotalDisplayRecords { get; set; }
        public string PatientId { get; set; }
        public string AppointmentID { get; set; }
        public string IsActive { get; set; }
        public string VitalSignId { get; set; }
        public string VisitId { get; set; }
        public string RefProviderId { get; set; }
        public string VisitReasonId { get; set; }
        public string CheifComplaint { get; set; }
        public string SignedBy { get; set; }
        public string Comments { get; set; }
        public string TemplateName { get; set; }
        public string TemplateTypeName { get; set; }
        public string TemplateId { get; set; }
        public string TemplateTypeId { get; set; }
        public string PrevNotesId { get; set; }
        public string NoteDate { get; set; }
        public string ROSDataTemptId { get; set; }
        public string PEDataTemptId { get; set; }
        public string PETemplateId { get; set; }
        public string ROSTemplateId { get; set; }
        public string HPITemplateId { get; set; }
        public string BillingInfoId { get; set; }
        public int PatientTypeId { get; set; }
        public string Duration { get; set; }
        public string DurationText { get; set; }
        public bool isPhoneEncounter { get; set; }
        public string bMedReconciled { get; set; }
        public string MedReconciledId { get; set; }
        public string HxtabOrder { get; set; }
        public string ComeFromCopyNote { get; set; }
        public bool IsNonBilable { get; set; }
        public string ComponentsIdsString { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string reportType { get; set; }
        public string cqmId { get; set; }
        public string eitherDetail { get; set; }
        public string PatientIds { get; set; }
        public string UserId { get; set; }
        public string EncounterType { get; set; }
        public string Caller { get; set; }
        public string Receiver { get; set; }
        public string User { get; set; }
        public string PatientName { get; set; }
        public string ProviderFullName { get; set; }
        public string ChiefComplaint { get; set; }
        public bool IsPhoneEncounter { get; set; }
        public string VisitType { get; set; }
        public string VisitTypeId { get; set; }
        public string FacilityPOSCodeDesc { get; set; }
        public string FacilityPOSCode { get; set; }
        public string FacilityPOSDesc { get; set; }
        public string IsAnyDocumentAttached { get; set; }
        public string FilePath { get; set; }
        public long PrevNotesIdPE { get; set; }
        public long PrevNotesIdROS { get; set; }
        public bool IsPrevNotesPE { get; set; }
        public bool IsPrevNotesROS { get; set; }
        public string OrderSetId { get; set; }
        public string OrderSetName { get; set; }
        public string OrderSetComments { get; set; }
        public bool IsPrevNoteComplaints { get; set; }
        public bool IsPrevNoteProblems { get; set; }
        public bool IsBodyPart { get; set; }

    }

    public class ClinicalNotesWaper
    {
        public ClinicalNotesModel ClinicalNotesModel { get; set; }
        public ClinicalNoteComponents ClinicalNoteComponents { get; set; }
        public ClinicalNotesWaper()
        {
            ClinicalNotesModel = new ClinicalNotesModel();
            ClinicalNoteComponents = new ClinicalNoteComponents();
        }
    }

    public class ClinicalNoteComponents
    {
        public ProblemListModelWaper Problems { get; set; }
        public ProceduresModelWaper Procedures { get; set; }
        public VitalsModelWaper Vitals { get; set; }

        public List<AllergiesModel> Allergies { get; set; }
        public SocialHxModel SocialHistory { get; set; }
        public MedicalHxModel MedicalHistory { get; set; }
        public BirthHxModel BirthHistory { get; set; }
        public FamilyHxSoapModel FamilyHistory { get; set; }

        public SurgicalHxSoapModel SurgicalHistory { get; set; }
        public HospitalizationHxModel HospitalizationHistory { get; set; }
        public ComplaintsModelWaper Complaints { get; set; }
        public MedicationWaper Medications { get; set; }
        public List<PrescriptionModel> Prescription { get; set; }
        public ROSSoapNote ReviewofSystem { get; set; }
        public PhysicalExam PhysicalExam { get; set; }
        public Immunization Immunization { get; set; }
        public LabOrderModelWraper ClinicalLabOrder { get; set; }
        public DiagnosticImagingOrderModelWraper ClinicalDiagnosticImagingOrder { get; set; }
        public ProcedureOrderModelWraper ClinicalProcedureOrder { get; set; }
        public SocPsyandBehaviorHxMod SocPsyandBehaviorHx { get; set; }
        public PatientEducationWraper PatientEducation { get; set; }
        public FollowUpModel FollowUp { get; set; }
        public TreatmentPlanCommentModel TreatmentPlanCommentModel { get; set; }
        public ClinicalNoteComponents()
        {
            Problems = new ProblemListModelWaper();
            Procedures = new ProceduresModelWaper();
            Vitals = new VitalsModelWaper();
            Allergies = new List<AllergiesModel>();
            SocialHistory = new SocialHxModel();
            MedicalHistory = new MedicalHxModel();
            BirthHistory = new BirthHxModel();
            FamilyHistory = new FamilyHxSoapModel();
            SurgicalHistory = new SurgicalHxSoapModel();
            HospitalizationHistory = new HospitalizationHxModel();
            Complaints = new ComplaintsModelWaper();
            Medications = new MedicationWaper();
            Prescription = new List<PrescriptionModel>();
            ReviewofSystem = new ROSSoapNote();
            PhysicalExam = new PhysicalExam();
            Immunization = new Immunization();
            SocPsyandBehaviorHx = new SocPsyandBehaviorHxMod();
            PatientEducation = new PatientEducationWraper();
            FollowUp = new FollowUpModel();
            TreatmentPlanCommentModel = new TreatmentPlanCommentModel();
        }
    }

    public class PhysicalExam
    {
        public string Notes_PETemplate_JSON { get; set; }
        public int Notes_PETemplateCount { get; set; }
        public string Notes_PETemplateSystems_JSON { get; set; }
        public int Notes_PETemplateSystemsCount { get; set; }
        public PhysicalExam()
        {
            Notes_PETemplate_JSON = "[]";
            Notes_PETemplateSystems_JSON = "[]";
            Notes_PETemplateCount = 0;
            Notes_PETemplateSystemsCount = 0;

        }
    }
    public class ClinicalNoteTemplateModel
    {
        public ClinicalNotesModel ClinicalNotesModel { get; set; }
        public string Message { get; set; }
        public ClinicalNoteTemplateModel()
        {
            this.ClinicalNotesModel = new ClinicalNotesModel();
        }

    }

    public class ClinicalNotesFillModel
    {
        public string SyndromicType { get; set; }
        public string CPTCodeDescription { get; set; }
        public string ShortName { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string VisitReason { get; set; }
        public string NoteType { get; set; }
        public string NoteTypeText { get; set; }
        public string NoteTemplate { get; set; }
        //public string PhoneEncounterNoteTemplate { get; set; }
        public string ProviderId { get; set; }
        public string Provider { get; set; }
        public string ResourceId { get; set; }
        public string Resource { get; set; }
        public string ResourceProviderId { get; set; }
        public string ResourceProvider { get; set; }
        public string FacilityId { get; set; }
        public string Facility { get; set; }
        public string FacilityName { get; set; }
        public string RoomNo { get; set; }
        public string RefProvider { get; set; }
        public bool IsLinkedAppointment { get; set; }
        public string LinkedAppointment { get; set; }
        public bool IsCopayPreviousNote { get; set; }
        public string CopayPreviousNote { get; set; }
        public string NoteText { get; set; }

        public string NoteStatus { get; set; }
        public string commandType { get; set; }
        public string NotesId { get; set; }
        public string NotesIds { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public Int64 ClinicalNotesCount { get; set; }
        public Int64 iTotalDisplayRecords { get; set; }

        public string PatientId { get; set; }

        public string AppointmentID { get; set; }

        public int IsActive { get; set; }

        public string VitalSignId { get; set; }

        public string VisitId { get; set; }

        public string RefProviderId { get; set; }
        public string VisitReasonId { get; set; }

        public string CheifComplaint { get; set; }
        public string SignedBy { get; set; }
        public string Comments { get; set; }
        public string TemplateName { get; set; }
        public string TemplateTypeName { get; set; }
        public string TemplateId { get; set; }
        public string TemplateTypeId { get; set; }

        public string PrevNotesId { get; set; }
        public string NoteDate { get; set; }
        public string ROSDataTemptId { get; set; }
        public string PEDataTemptId { get; set; }
        public string PETemplateId { get; set; }
        public string ROSTemplateId { get; set; }

        public string BillingInfoId { get; set; }
        public int PatientTypeId { get; set; }

        public string Duration { get; set; }
        public string DurationText { get; set; }
        public bool isPhoneEncounter { get; set; }
        public string bMedReconciled { get; set; }
        public string MedReconciledId { get; set; }
        public string HxtabOrder { get; set; }
        public string ComeFromCopyNote { get; set; }
        public bool IsNonBilable { get; set; }
        public string ComponentsIdsString { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string reportType { get; set; }
        public string cqmId { get; set; }
        public string eitherDetail { get; set; }
        public string PatientIds { get; set; }
        public string UserId { get; set; }
        public string EncounterType { get; set; }
        public string Caller { get; set; }
        public string Receiver { get; set; }
        public string User { get; set; }
        public string ComponentName { get; set; }
        public bool IsComponentOnly { get; set; }
        public string Action { get; set; }
        public long PriorUserId { get; set; }
        public string PriorUserName { get; set; }
        public string NoteAccessTime { get; set; }
        public string MeasureNumber { get; set; }
        // User FirstName and User LastName added by faizan ameen to sign the note from Web API.
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string IsHPIComplaint { get; set; }
        public string PatientDocumentIds { get; set; }
        public string PatientDocumentId { get; set; }
        public string OrderIds { get; set; }
        public string OrderType { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FolderName { get; set; }
        public bool IsFindingUpdated { get; set; }

        public string FacilityPOSCode { get; set; }

        public string ProviderFullName { get; set; }

        public string ResourceProviderName { get; set; }


        public string IsFromProgressNote { get; set; }

        public string FromCCM { get; set; }

        public bool IsDirectRollBack { get; set; }
        public bool ConfirmSign { get; set; }
        public string PatientVisitType { get; set; }
        public string OrderSetId { get; set; }
        public string NoteMissingDataReason { get; set; }

        public string IsPreviousNoteROS { get; set; }
        public string IsPreviousNotePE { get; set; }
        public string IsPreviousNoteComplaints { get; set; }
        public List<NoteComponentModel> NoteComponentList { get; set; }
        public long TransitionId { get; set; }
        public string RefModuleId { get; set; }
        public bool IsToCheckForTodaysNote { get; set; }
        public string Base64String { get; set; }
        public string ResultId { get; set; }
        public string IsPreviousNoteProblems { get; set; }
    }

    public class ROSSoapNote
    {
        public long ROSSystemInfoID { get; set; }
        public string SoapText { get; set; }
    }

    public class ProblemListModelWaper
    {
        public List<ProblemsListModel> ProblemListModel { get; set; }
        public string Attatched_ProblemList_JSON { get; set; }
        public ProblemListModelWaper()
        {
            ProblemListModel = new List<ProblemsListModel>();
            Attatched_ProblemList_JSON = "[]";
        }
    }
    public class ProblemsListModel
    {



        public long ProblemListId { get; set; }
        public string ProblemName { get; set; }
        public string Description { get; set; }
        public string ChronicityLevel { get; set; }
        public string Severity { get; set; }
        public string StartDate { get; set; }
        public string Comments { get; set; }
        public long PatientId { get; set; }
        public string NoteId { get; set; }
        public string ModifiedOn { get; set; }
        public string ProblemOrder { get; set; }
        public string IsActive { get; set; }
        public string ICD10 { get; set; }
        public string ICD10_Description { get; set; }
        public string reasoncomments { get; set; }
        public string ProviderName { get; set; }
        public string VisitDate { get; set; }
        public string EndDate { get; set; }

        public string IsCancerDisease { get; set; }
        public string CancerDiagnosisDate { get; set; }
        public string CancerEffectiveDate { get; set; }
        public string CancerClinicalDiagnosisDate { get; set; }
        public string CancerIsActive { get; set; }

        public string PrimarySite { get; set; }
        public string HistologicType { get; set; }
        public string NKOClinical { get; set; }
        public string NKOPathologic { get; set; }


        public string DiagnosisConfirmation { get; set; }

        public string Laterality { get; set; }

        public string Behavior { get; set; }
        public string Grade { get; set; }
        public string ClinicalStageGroup { get; set; }
        public string ClinicalStageDescriptor { get; set; }
        public string PrimaryClinicalTumor { get; set; }
        public string RLNC { get; set; }
        public string DistanceMestastatases { get; set; }
        public string StagerClinicalCancer { get; set; }
        public string PathologicStageGroup { get; set; }
        public string PathologicStageDescriptor { get; set; }
        public string PrimaryTumorPathologic { get; set; }
        public string RLNP { get; set; }
        public string DistanceMestastatasesPathologic { get; set; }
        public string StagerPathologicCancer { get; set; }

    }

    public class ProceduresModelWaper
    {
        public List<ProceduresModel> Procedures { get; set; }
        public string Attatched_Procedures_JSON { get; set; }
        public ProceduresModelWaper()
        {
            Procedures = new List<ProceduresModel>();
            Attatched_Procedures_JSON = "[]";
        }
    }
    public class LabOrderModelWraper
    {
        public string MedicationSoapCount { get; set; }
        public string MedicationSoap_JSON { get; set; }
        public string LabOrderTest_JSON { get; set; }
        public string LabOrderProblem_JSON { get; set; }
        public string ProblemListSoapCount { get; set; }
        public string ProblemListSoap_JSON { get; set; }
        public string Message { get; set; }
        public bool status { get; set; }
        public LabOrderModelWraper()
        {
            status = false;
            MedicationSoap_JSON = "[]";
            LabOrderTest_JSON = "[]";
            ProblemListSoap_JSON = "[]";
            LabOrderProblem_JSON = "[]";
        }
    }
    public class DiagnosticImagingOrderModelWraper
    {
        public string radiologySoapCount { get; set; }
        public string radiologySoap_JSON { get; set; }
        public string radiologyOrderTest_JSON { get; set; }
        public string radiologyOrderProblem_JSON { get; set; }
        public string ProblemListSoapCount { get; set; }
        public string ProblemListSoap_JSON { get; set; }
        public string Message { get; set; }
        public bool status { get; set; }
        public DiagnosticImagingOrderModelWraper()
        {
            status = false;
            radiologySoap_JSON = "[]";
            radiologyOrderTest_JSON = "[]";
            radiologyOrderProblem_JSON = "[]";
            radiologyOrderProblem_JSON = "[]";
            ProblemListSoap_JSON = "[]";
        }
    }
    public class ProcedureOrderModelWraper
    {
        public string ProcedureOrderSoapCount { get; set; }
        public string ProcedureOrderSoap_JSON { get; set; }
        public string ProblemListSoapCount { get; set; }
        public string ProblemListSoap_JSON { get; set; }
        public string Message { get; set; }
        public bool status { get; set; }
        public ProcedureOrderModelWraper()
        {
            status = false;
            ProcedureOrderSoap_JSON = "[]";
            ProblemListSoap_JSON = "[]";
        }
    }

    public class PatientEducationWraper
    {
        public List<PatientEducationModel> PatientEducation { get; set; }
        public PatientEducationWraper()
        {
            PatientEducation = new List<PatientEducationModel>();
        }

    }
    public class ProceduresModel
    {
        public string ProcedureId { get; set; }
        public long PatientId { get; set; }
        public string NoteId { get; set; }
        public string IsActive { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string CPTCode { get; set; }
        public string CPT_DESCRIPTION { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMED_DESCRIPTION { get; set; }
        public string ICD9 { get; set; }
        public string ICD10 { get; set; }
        public string ICD9_DESCRIPTION { get; set; }
        public string ICD10_DESCRIPTION { get; set; }
        public string ShowCPTCode { get; set; }
    }

    public class AllergiesModel
    {
        public string AllergyId { get; set; }
        public string Allergen { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
        public string OnSetDate { get; set; }
        public string Comments { get; set; }
        public string NoteId { get; set; }
        public string IsActive { get; set; }
    }

    public class SocialHxModel
    {
        public string SocialHxId { get; set; }
        public string PatientId { get; set; }
        public string SocialHxDate { get; set; }
        public string bUnremarkable { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
        public string bDrugExist { get; set; }
        public string bTobaccoExist { get; set; }
        public string bAlcoholExist { get; set; }
        public string bSexualExist { get; set; }
        public string bMiscHxExist { get; set; }
    }

    public class VitalsModel
    {
        public long VitalSignId { get; set; }
        public string CreatedBy { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string BSA { get; set; }
        public string BMI { get; set; }
        public string SPO2 { get; set; }
        public string OxygenSource { get; set; }
        public string PeakFlow { get; set; }
        public string Comments { get; set; }
        public string VitalSignDate { get; set; }
        public string VitalSignTime { get; set; }
        public string NotesId { get; set; }
        public string NoteStatus { get; set; }
        public string SeverityofPain { get; set; }
        public string SmokingStatus { get; set; }
        public string HeadCr { get; set; }
        public string BloodGroup { get; set; }
        public string BPId { get; set; }
        public string Systolic { get; set; }
        public string Diastolic { get; set; }
        public string BPModifiedBy { get; set; }
        public string BPModifiedOn { get; set; }
        public string Position { get; set; }
        public string CuffLocation { get; set; }
        public string CuffSize { get; set; }
        public string PulseId { get; set; }
        public string PulseResult { get; set; }
        public string PulseRhythm { get; set; }
        public string PulseModifiedBy { get; set; }
        public string PulseModifiedOn { get; set; }
        public string TemperatureId { get; set; }
        public string TemperatureResult { get; set; }
        public string TemperatureRhythm { get; set; }
        public string TempModifiedBy { get; set; }
        public string TempModifiedOn { get; set; }
        public string RespirationId { get; set; }
        public string RespirationResult { get; set; }
        public string RespirationRateRhythm { get; set; }
        public string RespModifiedBy { get; set; }
        public string RespModifiedOn { get; set; }

    }

    public class PatientEducationModel
    {
        public string OrderSetId { get; set; }
        public string OrderSetPatEducationId { get; set; }
        public string DocId { get; set; }
        public string DocType { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string DocumentName { get; set; }
        public string FileStream { get; set; }
        public string Pages { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Comments { get; set; }
        public long PageNumber { get; set; }
        public long RowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string commandType { get; set; }
        public string NonInfoDoc { get; set; }
        public string InfoDoc { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
    }

    public class FollowUpModel
    {

        public string FollowUpId { get; set; }
        public string OrderSetId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Comments { get; set; }
        public string Reason { get; set; }
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string FacilityName { get; set; }
        public string FacilityId { get; set; }
        public string Duration { get; set; }
        public string Time { get; set; }
        public string ScheduleCount { get; set; }
        public string ScheduleType { get; set; }
        public string commandType { get; set; }
        public string FollowUpText { get; set; }
        public string ModifiedByName { get; set; }
        public string CreateAppointment { get; set; }
        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
    }
    public class VitalsModelWaper
    {
        public VitalsModel VitalsModel { get; set; }
        public string Vitals_JSON { get; set; }
        public VitalsModelWaper()
        {
            VitalsModel = new VitalsModel();
            Vitals_JSON = "[]";
        }
    }


    public class ComplaintsModelWaper
    {
        public List<ComplaintsModel> Complaints { get; set; }
        public List<HPINotesFindings> HPINotesFindings { get; set; }
        public string ComplaintId { get; set; }

        public long NotesId { get; set; }
        public ComplaintsModelWaper()
        {
            Complaints = new List<ComplaintsModel>();
            HPINotesFindings = new List<HPINotesFindings>();
        }
        public bool isComplaintExists { get; set; }
        public string PrevComplaintFreeText { get; set; }
    }
    public class ComplaintsModel
    {
        public string ComplaintDetailId { get; set; }
        public string ComplaintDescription { get; set; }
        public string IsChiefComplaint { get; set; }
        //public string ComplaintId { get; set; }

        public string Complaint_CaseId { get; set; }
        public string Complaint_LocationIds { get; set; }
        public string Complaint_RadiationId { get; set; }
        public string Complaint_QualityId { get; set; }
        public string Complaint_SeverityId { get; set; }
        public string Onset { get; set; }
        public string Duration { get; set; }
        public string Complaint_DurationId { get; set; }
        public string Complaint_FrequencyId { get; set; }
        public string Complaint_ContextId { get; set; }
        public string Complaint_CharacterIds { get; set; }
        public string AssociatedWith { get; set; }
        public string PrecipitatedBy { get; set; }
        public string Complaint_AggravatedById { get; set; }
        public string Complaint_RelievedById { get; set; }
        public string Comments { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string PreviousHistory { get; set; }
        public string Expr1 { get; set; }
        public string OverallComments { get; set; }
        public string Complaint_CaseId_text { get; set; }
        public string Complaint_LocationIds_text { get; set; }
        public string Complaint_RadiationId_text { get; set; }
        public string Complaint_QualityId_text { get; set; }
        public string Complaint_SeverityId_text { get; set; }
        public string Complaint_DurationId_text { get; set; }
        public string Complaint_FrequencyId_text { get; set; }
        public string Complaint_ContextId_text { get; set; }
        public string Complaint_CharacterIds_text { get; set; }
        public string Complaint_AggravatedById_text { get; set; }
        public string Complaint_RelievedById_text { get; set; }
    }

    public class HospitalizationHxModel
    {
        public string HospitalizationHxId { get; set; }
        public string PatientId { get; set; }
        public string HospitalizationHxDate { get; set; }
        public string bUnremarkable { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
    }

    public class MedicalHxModel
    {
        public string MedicalHxId { get; set; }
        public string PatientId { get; set; }
        public string MedicalHxDate { get; set; }
        public string bUnremarkable { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
    }

    public class BirthHxModel
    {
        public string BirthHxId { get; set; }
        public string PatientId { get; set; }
        public string BirthHxDate { get; set; }
        public string bUnremarkable { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
        public string GeneralId { get; set; }
        public string MaternalDeliveryId { get; set; }
        public string NewbornId { get; set; }
    }

    public class MedicationWaper
    {
        public List<MedicationModel> Medications { get; set; }
        public string Attached_Medication_JSON { get; set; }

        public MedicationWaper()
        {
            Medications = new List<MedicationModel>();
            Attached_Medication_JSON = "[]";
        }
    }
    public class MedicationModel
    {
        public string MedicationID { get; set; }
        public string MedicationName { get; set; }
        public string RcopiaID { get; set; }
        public string PatientID { get; set; }
        public string PrescriptionID { get; set; }
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string Preparer_UserID { get; set; }
        public string DrugID { get; set; }
        public string Action { get; set; }
        public string Dose { get; set; }
        public string DoseUnit { get; set; }
        public string Routeby { get; set; }
        public string DoseTiming { get; set; }
        public string DoseOther { get; set; }
        public string Duration { get; set; }
        public string Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public string Substitution { get; set; }
        public string OtherNote { get; set; }


        public string PatientNotes { get; set; }
        public string Comments { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
        public string StopReason { get; set; }
        public string SigChangedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string IntendedUse { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string NDCID { get; set; }
        public string ReviewedOn { get; set; }
        public string ReviewedBy { get; set; }
        public string Refill { get; set; }

        //public string Renew { get; set; }
    }

    public class PrescriptionModel
    {
        public string PrescriptionID { get; set; }
        public string MedicationName { get; set; }
        public string Status { get; set; }
        public string RcopiaID { get; set; }
        public string PatientID { get; set; }
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string PharmacyID { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyZip { get; set; }
        public string PharmacyState { get; set; }
        public string PharmacyCity { get; set; }
        public string PharmacyAddress { get; set; }
        public string CreatedDate { get; set; }
        public string CompletedDate { get; set; }
        public string SignedDate { get; set; }
        public string StopDate { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string SendMethod { get; set; }
        public string Refill { get; set; }
        public string Cancelleddate { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Action { get; set; }
        public string Dose { get; set; }
        public string DoseUnit { get; set; }
        public string Routeby { get; set; }
        public string DoseTiming { get; set; }
        public string DoseOther { get; set; }
        public string Duration { get; set; }
        public string Quantity { get; set; }
        public string QuantityUnit { get; set; }


        public string Substitution { get; set; }
        public string NDCID { get; set; }
    }

    public class FamilyHxSoapModel
    {
        public string FamilyHxId { get; set; }
        public string PatientId { get; set; }
        public string FamilyHxDate { get; set; }
        public string bUnremarkable { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }

        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
    }

    public class SurgicalHxSoapModel
    {
        public string SurgicalHxId { get; set; }
        public string PatientId { get; set; }
        public string SurgicalHxDate { get; set; }
        public string bUnremarkable { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
    }
    public class SocPsyandBehaviorHxMod
    {
        public string SocialandBehaviorHxId { get; set; }
        public string SoapText { get; set; }
    }
    public class RosSoapModel
    {
        public long ROSSystemInfoID { get; set; }

        public bool? RosSystemInfoIsNormal { get; set; }

        public string RosSystemDate { get; set; }

        public string RosSystemInfoComments { get; set; }

        public string RosSystemInfoDescription { get; set; }

        public long ROSSystemPatientID { get; set; }

        public long ROSSystemId { get; set; }

        public string SystemName { get; set; }

        public string SystemDescription { get; set; }

        public bool? SystemIsNormal { get; set; }
        public long ROSSystemPatientCharacteristicsID { get; set; }
        public long ROSSystemCharacteristicsId { get; set; }
        public string RosSystemPatientCharc_IsPositive { get; set; }

        private bool? _RosSystemPatientCharc_IsPositive { get; set; }

        public bool? GetRosSystemPatientChar_IsPositive()
        {
            bool? flag;

            if (string.IsNullOrEmpty(this.RosSystemPatientCharc_IsPositive))
            {
                flag = null;
            }
            else
            {
                flag = Convert.ToBoolean(this.RosSystemPatientCharc_IsPositive);
            }

            return flag;
        }

        public string RosSystemCharcDescription { get; set; }
        public string RosSystemCharcName { get; set; }
        public string ROSCharacteristicsDetailsId { get; set; }
        public long ROSCharcDetailPatCharID { get; set; }
        public string PreviousHistory { get; set; }
        public string ROSCharacteristicsDetailStatusId { get; set; }
        public string Onset { get; set; }
        public long Duration { get; set; }
        public int ROSCharacteristicsDetailDurationId { get; set; }
        public int ROSCharacteristicsDetailPatternId { get; set; }
        public int ROSCharacteristicsDetailSeverityId { get; set; }
        public int ROSCharacteristicsDetailCourseId { get; set; }
        public int ROSCharacteristicsDetailRadiationId { get; set; }
        public int ROSCharacteristicsDetailFrequencyId { get; set; }
        public int ROSCharacteristicsDetailContextId { get; set; }
        public int ROSCharacteristicsDetailCharacterCSZId { get; set; }
        public int ROSCharacteristicsDetailAggravedById { get; set; }
        public int ROSCharacteristicsDetailRelievedById { get; set; }
        public string Location { get; set; }
        public string PrecipitatedBY { get; set; }
        public string AssociatedWith { get; set; }
        public string DetailStatusName { get; set; }
        public string DurationName { get; set; }
        public string PatternName { get; set; }
        public string SeverityName { get; set; }
        public string ContextName { get; set; }
        public string RadiationName { get; set; }
        public string CourseName { get; set; }
        public string FrequencyName { get; set; }
        public string CharacterCSZName { get; set; }
        public string AggravedByName { get; set; }
        public string RelievedByName { get; set; }

    }


    public class VaccineSoapModel
    {
        public long VaccineHxId { get; set; }
        public string VaccineName { get; set; }
        public float Dose { get; set; }
        public string Amount { get; set; }
        public string RouteDescription { get; set; }
        public string SiteDescription { get; set; }
        public string ManufacturerName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Type { get; set; }
        public bool VoidDose { get; set; }
        public string ProviderName { get; set; }
        public DateTime AdministrationDate { get; set; }
        public string RefusalReason { get; set; }
        public string Comments { get; set; }
        public string CPT { get; set; }
        public string LotNumber { get; set; }
    }

    public class TherapeuticInjectionSoapModel
    {
        public string Type { get; set; }
        public string CPTCode { get; set; }
        public string TherapeuticInjection { get; set; }
        public long ImmTherInjectionId { get; set; }
        public string siteText { get; set; }
        public string LotNumber { get; set; }
        public string RouteDescription { get; set; }
        public string SiteDescription { get; set; }
        public string ManufacturerName { get; set; }
        public string ProviderName { get; set; }
        public DateTime AdministrationDate { get; set; }
        public float Dose { get; set; }
        public string Amount { get; set; }
    }

    public class Immunization
    {
        public List<VaccineSoapModel> Vaccines { get; set; }
        public List<TherapeuticInjectionSoapModel> Injections { get; set; }
        public string Attached_TherInjection_JSON { get; set; }
        public string Attached_Vaccine_JSON { get; set; }
        public Immunization()
        {
            Vaccines = new List<VaccineSoapModel>();
            Injections = new List<TherapeuticInjectionSoapModel>();
            Attached_TherInjection_JSON = "[]";
            Attached_Vaccine_JSON = "[]";
        }

        public Immunization(List<VaccineSoapModel> vaccines, List<TherapeuticInjectionSoapModel> injections, string dsTherInjection, string dsVaccine)
        {
            Vaccines = vaccines;
            Injections = injections;
            Attached_TherInjection_JSON = dsTherInjection;
            Attached_Vaccine_JSON = dsVaccine;
        }
    }

    public class AmendmentNoteModel
    {
        public string OldProblemSoapText { get; set; }
        public string CurrentProblemSoapText { get; set; }
        public string OldProcedureSoapText { get; set; }
        public string CurrentProcedureSoapText { get; set; }
    }
    public class AmendmentNoteReportModel
    {
        public string NoteComponentsId { get; set; }
        public string NotesId { get; set; }
        public string NoteComponentsLookupId { get; set; }
        public string SOAPText { get; set; }
        public string OrderNo { get; set; }
        public string NoteSectionsLookupId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
    }
    public class PhysicalExamSOAPModel
    {
        public List<PETemplateModel> PETemplateList { get; set; }
        public List<PETemplateSystemModel> PETempSystemList { get; set; }
        public List<PESystemObservationModel> PESysObservationList { get; set; }
    }
    public class PETemplateModel
    {
        public string PETemplateId { get; set; }
        public string TemplateName { get; set; }
        public string SpecialityIds { get; set; }
        public string ProviderIds { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string TemplatePreview { get; set; }
    }
    public class PETemplateSystemModel
    {
        public string PETemplateId { get; set; }
        public string TemplateName { get; set; }
        public string PESystemId { get; set; }
        public string SystemName { get; set; }
        public bool IsSelectedSystem { get; set; }
        public string PETemplateSystemId { get; set; }
    }
    public class PESystemObservationModel
    {
        public string PESystemId { get; set; }
        public string SystemName { get; set; }
        public string PEObservationId { get; set; }
        public string ObservationName { get; set; }
        public bool IsSelected { get; set; }
    }
    public class NotesVisitDate
    {
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NotesId { get; set; }
    }
    public class TreatmentPlanCommentModel
    {
        public string SoapText { get; set; }
    }

}
