using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 namespace MDVision.Model.Native.Scheduler
{
    public class EmptySlotModel
    {
        public string PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public string AppointmentId { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string AppointmentDate { get; set; }
        public string GapMinutes { get; set; }
        public string NoOfAppointments { get; set; }
        public string Duration { get; set; }
        public string ExpectedDuration { get; set; }
        public string ProviderId { get; set; }
        public string FacilityId { get; set; }
        public string PatientTypeId { get; set; }
        public string MRNumber { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string IsActive { get; set; }
        public string UserName { get; set; }
        public string Approve { get; set; }
        public string RejectionReason { get; set; }

        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}
