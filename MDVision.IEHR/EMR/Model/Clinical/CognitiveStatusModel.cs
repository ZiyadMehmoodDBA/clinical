using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical
{
    public class CognitiveStatusModel
    {

        public string CognitiveStatusId { get; set; }
        public string CognitiveId { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string LexiCode { get; set; }
        public string LexiCodeDescription { get; set; }
        public string commandType { get; set; }
        public long NoteId { get; set; }
        public string CognitiveStatusText { get; set; }
        public string Instruction { get; set; }
    }
}