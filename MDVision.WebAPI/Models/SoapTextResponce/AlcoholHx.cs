using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class AlcoholHx
    {
        public string AlcoholId { get; set; }
        public string AlcoholStatus { get; set; }
        public string AlcoholType { get; set; }
        public string AlcoholUsagePeriod { get; set; }
        public string AlcoholFrequencyDays { get; set; }
        public string AlcoholCounsellingPeriod { get; set; }
        public string AlcoholCounsellingTopic { get; set; }
        public string AlcoholCessationLength { get; set; }
        public string AlcoholCessationPeriod { get; set; }
        public string AlcoholRecentlyQuit { get; set; }
        public string AlcoholNotReadyToQuit { get; set; }
        public string AlcoholWouldQuit { get; set; }
        public string AlcoholComments { get; set; }
        public string AlcoholSoapText { get; set; }
    }
}