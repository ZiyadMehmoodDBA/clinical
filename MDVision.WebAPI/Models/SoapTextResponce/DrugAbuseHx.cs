using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class DrugAbuseHx
    {
        public string DrugAbuseId { get; set; }
        public string DrugStatus { get; set; }
        public string DrugType { get; set; }
        public string DrugRoute { get; set; }
        public string DrugFrequencyDay { get; set; }
        public string DrugFrequencyMonth { get; set; }
        public string DrugUsagePeriod { get; set; }
        public string DrugCessationLength { get; set; }
        public string DrugCessationPeriod { get; set; }
        public string DrugRecentlyQuit { get; set; }
        public string DrugWouldQuit { get; set; }
        public string DrugComments { get; set; }
        public string DrugSoapText { get; set; }
    }
}