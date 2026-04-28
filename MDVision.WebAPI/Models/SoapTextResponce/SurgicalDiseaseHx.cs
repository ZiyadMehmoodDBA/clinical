using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SurgicalDiseaseHx
    {
     
        public string CPTCode { get; set; }
        public string CPTCodeId { get; set; }
        public string CPTDescription { get; set; }
        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }
        public string SurgicalStatus { get; set; }
        public string SurgicalLocation { get; set; }
        public string SurgicalSurgeryDate { get; set; }
        public string AgeAtSurgery { get; set; }
        public string SurgicalReason { get; set; }
        public string SurgicalOrderingProviderId { get; set; }
        public string SurgicalOrderingProvider { get; set; }
        public string SurgicalPerformingProviderId { get; set; }
        public string SurgicalPerformingProvider { get; set; }
        public string SurgicalComments { get; set; }
        

    }
}