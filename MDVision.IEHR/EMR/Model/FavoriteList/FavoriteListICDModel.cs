using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.FavoriteList
{
    public class FavoriteListICDModel
    {
        public Int64 FavoriteListICDId { get; set; }
        public long DiagnosisId{get;set;}
        public string ICD9 { get; set; }
        public string ICD10 { get; set; }
        public string ICD10Description { get; set; }
        public string ICD9Description { get; set; }
        public string SNOMEDDescription { get; set; }
        public string SNOMED { get; set; }
    }
}