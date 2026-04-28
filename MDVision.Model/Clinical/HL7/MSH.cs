using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.HL7
{
    public class MSH
    {
        public string MessageCode { get; set; }
        public string TriggerEvent { get; set; }
        public string MessageStructure { get; set; }
        public string VersionID { get; set; }
    }
}
