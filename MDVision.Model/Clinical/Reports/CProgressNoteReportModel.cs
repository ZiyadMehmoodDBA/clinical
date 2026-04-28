using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
    public class CProgressNoteReportModel
    {
        public string CreatedOn { get; set; }
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string HomePhone { get; set; }
        public string NotesStatus { get; set; }
        public string NotesType { get; set; }
        public string PracticeName { get; set; }
        public string ProviderName { get; set; }
        public string RefProviderName { get; set; }
        public string FacilityName { get; set; }
        public long ProviderId { get; set; }
        public long PatientId { get; set; }
        public long NotesId { get; set; }
    }
}
