using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class CaffeineHx
    {
        public string MiscHxId { get; set; }
        public string CaffeineIntakeHxId { get; set; }
        public string MiscChildStatus { get; set; }
        public string CaffeineIntakFrequency { get; set; }

        public string CaffeineHarmful { get; set; }
        public string CaffeineComments { get; set; }
        public string OccupationSoapText { get; set; }
        
    }
}