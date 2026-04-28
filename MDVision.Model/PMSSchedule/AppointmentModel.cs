using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.PMSSchedule
{
    public class AppointmentModel
    {
        public long id { get; set; } = 0;
        public string CommandType { get; set; } = "";
        public long PatientId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string PatientName { get; set; } = "";
        public Int64 InsuranceId { get; set; }
        public Int64 InsurancePlanId { get; set; }
        public string AppointmentId { get; set; } = "";
        public DateTime AppointmentDate { get; set; }
        public long ProviderId { get; set; }
        public long ResourceId { get; set; }
        public string ProviderName { get; set; } = "";
        public string ResourceName { get; set; } = "";
        public long PatientTypeId { get; set; }
        public string PatientType { get; set; } = "";
        public string SchStatusId { get; set; } = "";
        string ProviderIDs { get; set; } = "";
        string ScheduleDate { get; set; }
        public string AppointmentStatus { get; set; } = "";
        public string VisitTypeId { get; set; } = "";
        public string VisitTypeName { get; set; } = "";
        public long FacilityId { get; set; }
        public string FacilityName { get; set; } = "";
        public string FacilityColor { get; set; } = "";
        public string Comments { get; set; } = "";
        public bool IsNonBilable { get; set; } 
        public string ReasonComments { get; set; } = "";
        public string EligibilityStatus { get; set; } = "";
        public float CopayBal { get; set; } = 0;
        public string ProviderIds { get; set; } = "";
        public string ResurceIds { get; set; } = "";
        public string FacilityIds { get; set; } = "";
        public string StartDate { get; set; } = "";
        public string EndDate { get; set; } = "";
        public string ResourceIds { get; set; } = "";
        public string VisitTypeIds { get; set; } = "";
        public string VisitTypeColor { get; set; } = "";
        public string AppointmentStatusIds { get; set; } = "";
        public string AppointmentStatusId { get; set; } = "";
        public DateTime AppointmentDateFrom { get; set; } 
        public DateTime AppointmentDateTo { get; set; }
        public string TimeFrom { get; set; } = "";
        public string BeginningTime { get; set; } = "";
        public string EndingTime { get; set; } = "";
        public string PreferredDate { get; set; } = "";
        public string TimeTo { get; set; } = "";
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int ownerId { get; set; } = 0;
        public string StatusColor { get; set; } = "";
        public float AmtCopay { get; set; } = 0;
        public string CopayClass { get; set; } = "";
        public string View { get; set; } = "";
        public int Month { get; set; } 
        public int Year { get; set; }
        public string SchViewType { get; set; } = "";
        public bool IsResourceSch { get; set; }
        public bool IsProvider { get; set; }
        public string VisitId { get; set; } = "";
        public string LastScheduleStatusId { get; set; } = "";
        public string LastAppointmentStatus { get; set; } = "";
        
        public string RefProviderName { get; set; } = "";
        public string RefProviderId { get; set; } = "";
        public bool isNoteCreated { get; set; }
        public string Notesid { get; set; } = "";
        public bool IsNoteSigned { get; set; }
        public string GroupType { get; set; } = "";
        public string FacilityPhoneNo { get; set; } = "";
        public string Status { get; set; }
        public string EDIEligibilityId { get; set; }
        public string PatientSex { get; set; }
        public string PatientDOB { get; set; }
        public string PatientAddress1 { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZip { get; set; }
        public string PatientEthnicityIds { get; set; }
        public string PatientRaceIds { get; set; }
        public string PatientMaritalStatus { get; set; }
        public string PatientHomeTel { get; set; }
        public string WorkWeekDays { get; set; }
        public string ReferralAuthNo { get; set; }
        public string ReferringFromName { get; set; }
        public long ReferringFromId { get; set; }       
        public string RecordCount { get; set; }
        public string CancellationReason { get; set; }

        public string  PatientReferralId{ get; set; }
        public string NewPatientColor { get; set; }
        public string EstablishedPatientColor { get; set; }
    }
}
