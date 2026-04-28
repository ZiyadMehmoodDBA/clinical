using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.FavoriteList
{
    public class FavoriteListCPTModel
    {
        public long CPTId { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMED_DESCRIPTION { get; set; }
        public string LabId { get; set; }
        public string Unit { get; set; }
        public string Modifier { get; set; }
        public string FavoriteListCPTId { get; set; }
    }
}