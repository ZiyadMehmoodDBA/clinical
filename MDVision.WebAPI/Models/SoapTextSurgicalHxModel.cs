using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models
{
    public class SoapTextSurgicalHxModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string diseaseId { get; set; }
        public string SoapText { get; set; }
        public string SurgicalHxId { get; set; }
        public string IsCreatedOrModified { get; set; }
        public string LastUpdated { get; set; }
    }
}