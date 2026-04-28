using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Model.Dashboard
{
    public class DUserTaskModel
    {
        public string PatMsgId { get; set; }
        public string PatientId { get; set; }
        public string MsgDetail { get; set; }
        public string MsgStatusId { get; set; }
        public string MessageStatus { get; set; }
        public string EntryDate { get; set; }
        public string AssignedToId { get; set; }
        public string AssigneeName { get; set; }
        public string CreatedBy { get; set; }
        public string FacilityName { get; set; }
        public string RecordCount { get; set; }
        public string ProviderName { get; set; }
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
    }
}
