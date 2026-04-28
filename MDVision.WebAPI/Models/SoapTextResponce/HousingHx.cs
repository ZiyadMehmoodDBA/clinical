using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class HousingHx
    {
        public string MiscHxId { get; set; }
        public string HousingHxId { get; set; }
        public string MiscChildStatus { get; set; }
        public string HousingPresent { get; set; }
        public string HousingPast { get; set; }
        public string HousingComments { get; set; }
        public string HousingSoapText { get; set; }
        
    }
}