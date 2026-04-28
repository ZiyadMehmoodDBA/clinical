using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.CCDA
{
    public class ICDLookupModel
    {
        public string title { get; set; }
        public string SCT_CONCEPT_ID { get; set; }
        public string ICD9CM_CODE { get; set; }
        public string ICD9CM_TITLE { get; set; }
        public string ICD10CM_CODE { get; set; }
        public string ICD10CM_TITLE { get; set; }
    }

    public class CPTLookupModel
    {
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
    }
}
