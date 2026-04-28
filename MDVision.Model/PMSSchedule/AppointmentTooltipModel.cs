using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.PMSSchedule
{
    public class AppointmentTooltipModel
    {
        public long id { get; set; }
        public string CommandType { get; set; }
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public string AppointmentId { get; set; }

      
        public string AccountNumber { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string EmailAddress { get; set; }
        public string Age { get; set; }
        public string PatientAge { get; set; }
        public string CellNo { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string RescheduleDate { get; set; }
        public string Status { get; set; }
        public string KeyPress { get; set; }
        public string ResponseMessage { get; set; }
        public string PatientProfileImagePath { get; set; }
        public string PatientProfileThumbnailPath { get; set; }
        public string Duration { get; set; }
        public string PrimaryInsuranceName { get; set; }
        public bool IsReminderSent { get; set; }
        public byte[] PatientImage { get; set; }
        public string ImageType { get; set; }
        public double CopayBal { get; set; }
        public double AmtCopay { get; set; }
        public string imgPatient { get; set; }
        public string ReasonComments { get; set; }
        public string Comments { get; set; }
        public string ProviderName { get; set; }
        public string FacilityName { get; set; }
        public string PatientType { get; set; }
        public string VisitTypeName { get; set; }
        public string VisitTypeColor { get; set; }
        public string StatusColor { get; set; }
        public string CopayClass { get; set; }
        public string AppointmentStatus { get; set; }
        public string RescheduleProvider { get; set; }
        public string RescheduleFacility { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleProvider { get; set; }
        public string ScheduleFacility { get; set; }
        public string CancellationReason { get; set; }


    }
}
