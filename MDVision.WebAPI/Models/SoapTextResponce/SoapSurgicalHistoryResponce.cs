using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SoapSurgicalHistoryResponce
    {
        public string status { get; set; }
        public string SurgicalHxFill_JSON { get; set; }
        public string surgicalHxDiseaseFill_JSON { get; set; }
        public string surgicalHxLoad_JSON { get; set; }
        public string surgicalHxDiseaseLoad_JSON { get; set; }
        public string DiseaseFillCount { get; set; }


       
    }
}