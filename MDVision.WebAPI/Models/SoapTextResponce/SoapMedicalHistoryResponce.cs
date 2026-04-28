using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SoapMedicalHistoryResponce
    {
       public string status { get; set; }
       public string MedicalHxFill_JSON { get; set; }
        public string DiseaseFill_JSON { get; set; }
        public string MedicalHxDiseaseLoad_JSON { get; set; }
    }
}