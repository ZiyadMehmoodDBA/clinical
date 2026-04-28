using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.HL7
{
    public class QAK
    {
        public string QueryTag { get; set; }
        public string Identifier { get; set; }
        public string QueryResponseStatus { get; set; }
    }
}
