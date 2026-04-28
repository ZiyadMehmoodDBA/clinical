using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.HL7
{
    public class ImmunizationHL7ResponseModel
    {
        public MSA MSA { get; set; }
        public MSH MSH { get; set; }
        public OBX OBX { get; set; }
        public PID PID { get; set; }
        public QAK QAK { get; set; }

        public string Message { get; set; }
    }

    public class ImmunizationHL7ResponseRXA_OBXModel
    {
        public RXA RXA { get; set; }
        public List<OBX> OBX { get; set; }
        public string Message { get; set; }
    }
}
