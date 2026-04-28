using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.FaceSheet
{
    public class FSAppointmentModel
    {
        public string ScheduleDate { get; set; }
        public string ScheduleTimeFrom { get; set; }
        public string SchReason { get; set; }
        public string ProviderName { get; set; }

        public string PatientId { get; set; }

        public string PatientName { get; set; }

        public string Gender { get; set; }
        public string AccountNumber { get; set; }
        public string DOB { get; set; }
        public string CellNo { get; set; }
        public string EmailAddress { get; set; }
        public string InsurancePlanName { get; set; }
        public string Duration { get; set; }
        public string AppointmentId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Comments { get; set; }
        public string PatientType { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderFirstName { get; set; }
        public string ReminderStatus { get; set; }
        public string ReminderResponseDelivery { get; set; }
        public string ReminderResponseMessage { get; set; }
        public string copayment { get; set; }
        public string AppointmentStatus { get; set; }
       
        public string PatientVisitType { get; set; }
        public string VisitTypeColor { get; set; }
        public string FacilityName { get; set; }

        public string CancellationReason { get; set; }

    }
}
