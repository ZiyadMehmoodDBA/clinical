using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class ProcedureOrder
    {

        public Int64 ProcedureOrderId { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string Urgency { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string SoapText { get; set; }
    }
}
