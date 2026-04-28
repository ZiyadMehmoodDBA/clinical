using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class OccupationHx
    {
        public string MiscHxId { get; set; }
        public string OccupationHxId { get; set; }
        public string MiscChildStatus { get; set; }
        public string OccupationPresent { get; set; }
        public string OccupationPast { get; set; }
        public string OccupationComments { get; set; }
        public string OccupationSoapText { get; set; }

    }
}