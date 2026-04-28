using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.HL7
{
    public class RXA
    {
        public string GiveSubIDCounter { get; set; }
        public string AdministrationSubIDCounter { get; set; }
        public string AdministrationDate { get; set; }
        public string Code { get; set; }
        public string VaccineDescription { get; set; }
        public string CodeType { get; set; }
        public string CompletionStatus { get; set; }
        public string Dose { get; set; }
    }
}
