using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SoapHospitalizationHxResponse
    {
        public string status { get; set; }
        public string HospitalizationHxFill_JSON { get; set; }
        public string DiseaseFill_JSON { get; set; }
        public string HospitalizationHxLoad_JSON { get; set; }
        public string HospitalizationHxDiseaseLoad_JSON { get; set; }
        public string DiseaseFillCount { get; set; }

        public string SoapText { get; set; }
    }
}