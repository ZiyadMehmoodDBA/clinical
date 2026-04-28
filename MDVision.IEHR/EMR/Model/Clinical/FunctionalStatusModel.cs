using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical
{
    public class FunctionalStatusModel
    {

        public string FunctionalStatusId { get; set; }
        public string CognitiveId { get; set; }
        public string FunctionalStatusICD9Code { get; set; }
        public string FunctionalStatusICD9CodeDescription { get; set; }
        public string FunctionalStatusICD10Code { get; set; }
        public string FunctionalStatusICD10CodeDescription { get; set; }
        public string FunctionalStatusSNOMEDID { get; set; }
        public string FunctionalStatusSNOMEDDescription { get; set; }
        public string FunctionalStatusLexiCode { get; set; }
        public string FunctionalStatusLexiCodeDescription { get; set; }
        public string FunctionalStatuscommandType { get; set; }
        public long NoteId { get; set; }
        public string FunctionalStatusText { get; set; }
        public string FunctionalStatusInstruction { get; set; }

    }
}