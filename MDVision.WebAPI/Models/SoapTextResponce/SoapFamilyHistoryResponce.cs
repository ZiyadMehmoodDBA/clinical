using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SoapFamilyHistoryResponce
    {
        public string status { get; set; }
        public string FamilyHxFill_JSON { get; set; }
        public string FamilyHxLoad_JSON { get; set; }
        public string DiseaseLoad_JSON { get; set; }
        public string MemberLoad_JSON { get; set; }
        public string MemberHasDisease_JSON { get; set; }
    }
}