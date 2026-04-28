using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Patient
{
    public class PatientFaxLog
    {
        public string ToFaxNumber { get; set; }
        public string SentStatus { get; set; }
        public string DateAndTime { get; set; }
        public string Pages { get; set; }
        public string SenderName { get; set; }
        public string Subject { get; set; }
        public string RecipientName { get; set; }
    }
}
