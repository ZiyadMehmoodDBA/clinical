using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.Dashboard
{
    public class DashBoardModel
    {
        public string CommandType { get; set; }

        public string DBSId { get; set; }
        public string DBSType { get; set; }
        public string Switch { get; set; }
        public string VisitFrom { get; set; }
        public string VisitTo { get; set; }
        public string NoteStatus { get; set; }
        public bool IsDraftNote { get; set; }
        public string Month { get; set; }
        public string ActiveWidget { get; set; }
        public string ProviderId { get; set; }
        public string FacilityId { get; set; }
        public string SlotDate { get; set; }
        public string Color { get; set; }
        public string ResourceId { get; set; }
        public string PatientId { get; set; }
        public string MessageId { get; set; }
        public string AssignedToId { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string Status { get; set; }
        public string IsDashBoardData { get; set; }
        public string FromEntry { get; set; }
        public string ToEntry { get; set; }
        public string DOSFrom { get; set; }
        public string DOSTo { get; set; }
        public string EnteredBy { get; set; }
        public string AssignedtoReview { get; set; }
        public string DocumentId { get; set; }
        public string Active { get; set; }

        public string AppDate { get; set; }
        public string IsCheckedIn { get; set; }
        public string CurrentKPIId { get; set; }
        public string SelectedKPIId { get; set; }
        public string NotesID { get; set; }
        public string CommentsCosign { get; set; }
        public string Radioval { get; set; }
        public string Action { get; set; }
        public string CommentsAmendment { get; set; }
        public string Comments { get; set; }
        public string RequestedBy { get; set; }
        public string MessageDate { get; set; }
        public string Priority { get; set; }
        public string MessageName { get; set; }
        public string ProfileName { get; set; }
        public string MessageType { get; set; }
        public string DirectAddress { get; set; }
        public string PortalAppDate { get; set; }
        public string PortalAppStatus { get; set; }
        public string PortalAppRequestId { get; set; }
        public string PracticeId { get; set; }
        public string ProviderName { get; set; }
        public string PatientName { get; set; }
        public string CoSignedProviderId { get; set; }
        public string MsgStatusId { get; set; }
        public string InsurancePlanId { get; set; }
        public string IsReviewed { get; set; }
        public long? NoteSectionsLookupId { get; set; }
        public long? NoteComponentsLookupId { get; set; }
        public string NoteType { get; set; }
        public bool IsAmendmentForBilling { get; set; }
        public string DocPriority { get; set; }
        public string DocAssignToReview { get; set; }
        public long ID { get; set; }
        public string IDs { get; set; }
        public bool isdelivered { get; set; }
        public string DocStatus { get; set; }

        public string DocumentStatus { get; set; }
        public string AppointmentStatusIds { get; set; }
        public int IsReadyOrMissing { get; set; }
        public string strNotesIds { get; set; }
        public string MissingInfo { get; set; }

        public bool IsAssessmentAndPlanTreatment { get; set; }

        public bool IsCareTeamMembers { get; set; }

        public bool IsDemographics { get; set; }

        public bool IsGoals { get; set; }

        public bool IsHealthConcerns { get; set; }

        public bool IsHistory { get; set; }

        public bool IsImmunizations { get; set; }

        public bool IsImplantableDevices { get; set; }

        public bool IsLaboratoryResults { get; set; }

        public bool IsLaboratoryTest { get; set; }

        public bool IsMedications { get; set; }

        public bool IsMedicationsAllergies { get; set; }

        public bool IsProblems { get; set; }

        public bool IsProcedures { get; set; }

        public bool IsVitalSigns { get; set; }
        public string DOB { get; set; }
    }
}