using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class ConsultationOrder
    {
        public Int64 ConsultationOrderId { get; set; }
        public string Type { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string Urgency { get; set; }
        public string ProblemName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
