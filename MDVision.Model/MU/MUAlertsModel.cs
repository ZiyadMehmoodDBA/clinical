using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.MU
{
    public class MUAlertsModel
    {
        public long AlertId { get; set; }
        public string ProfileName { get; set; }
        public string Fields { get; set; }
        public long PatientId { get; set; }
        public long UserId { get; set; }
        public bool IsShowAlert { get; set; }
        public string Type { get; set; }  
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public long NotesId  { get; set; }
        public long ProviderId { get; set; }
        public long PrescriptionId { get; set; }
        public string NoteDate { get; set; }
        public string NoteTime { get; set; }
        public bool IsHighPriority { get; set; }
        public string MeasureType { get; set; }
        public string Process { get; set; }
        public long MissingDataAlertCount { get; set; }

        public MUAlertsModel()
        {
            CreatedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;
        }
    }
}
