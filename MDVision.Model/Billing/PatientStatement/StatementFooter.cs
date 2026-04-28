using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Billing.PatientStatement
{
    public class StatementFooter
    {

        public string Age { get; set; }
        public string PatBalance { get; set; }
        public string Age0_30 { get; set; }
        public string Age31_60 { get; set; }
        public string Age61_90 { get; set; }
        public string Age91_120 { get; set; }
        public string Age121_Onward { get; set; }
        public string StatementMessage { get; set; }

    }
}
