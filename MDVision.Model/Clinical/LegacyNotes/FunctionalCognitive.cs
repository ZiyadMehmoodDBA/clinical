using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class FunctionalCognitive
    {
        public Int64 CognitiveId { get; set; }
        public string Type { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string Instruction { get; set; }

    }
}
