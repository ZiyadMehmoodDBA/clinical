using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Model.Dashboard
{
    public class DAppointmentNoteModel
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public string ProviderName { get; set; }
        public string RecordCount { get; set; }
        public string FacilityName { get; set; }
        public string NoteStatus { get; set; }
        public string AppReason { get; set; }
        public string VisitDate { get; set; }
        public string CC { get; set; }
    }
}
