using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Model.Dashboard
{
    public class DAppointmentModel
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public string ProviderName { get; set; }
        public string RecordCount { get; set; }
        public string FacilityName { get; set; }
        public string Room { get; set; }
        public string AppointmentStatus { get; set; }
        public string VisitDate { get; set; }
        public string minsWait { get; set; }
        public string Duration { get; set; }
        public string AppointmentTime { get; set; }
        public string NotesId { get; set; }
        public string VisitId { get; set; }
        public string Reason { get; set; }
        public string AppointmentId { get; set; }
        public string FacilityId { get; set; }
        public string ProviderId { get; set; }
        public string PatientType { get; set; }
        public string VisitType { get; set; }
    }
}
