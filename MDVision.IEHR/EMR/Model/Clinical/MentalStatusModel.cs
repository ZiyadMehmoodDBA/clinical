using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical
{
    public class MentalStatusModel
    {
        public string MentalStatusId { get; set; }
        public string CognitiveId { get; set; }
        public string MentalStatusICD9Code { get; set; }
        public string MentalStatusICD9CodeDescription { get; set; }
        public string MentalStatusICD10Code { get; set; }
        public string MentalStatusICD10CodeDescription { get; set; }
        public string MentalStatusSNOMEDID { get; set; }
        public string MentalStatusSNOMEDDescription { get; set; }
        public string MentalStatusLexiCode { get; set; }
        public string MentalStatusLexiCodeDescription { get; set; }
        public string FreeTextICD { get; set; }
        public string MentalStatuscommandType { get; set; }
        public long NoteId { get; set; }
        public string MentalStatusText { get; set; }
        public string MentalStatusInstruction { get; set; }

    }
}