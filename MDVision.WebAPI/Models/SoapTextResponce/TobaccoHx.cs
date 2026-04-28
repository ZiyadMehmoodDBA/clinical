using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class TobaccoHx
    {
        public string TobaccoId { get; set; }
        public string SmokingStatus { get; set; }
        public string TobaccoType { get; set; }
        public string TobaccoUsagePeriod { get; set; }
        public string TobaccoFrequencyDaily { get; set; }
        public string TobaccoCounsellingPeriod { get; set; }
        public string TobaccoCounsellingTopic { get; set; }
        public string TobaccoCessationLength { get; set; }
        public string TobaccoCessationPeriod { get; set; }
        public string TobaccoRecentlyQuit { get; set; }
        public string TobaccoWouldQuit { get; set; }
        public string TobaccoComments { get; set; }
        public string TobaccoSoapText { get; set; }
    }
}