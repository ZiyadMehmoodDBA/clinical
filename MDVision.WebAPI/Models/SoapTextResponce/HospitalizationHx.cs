using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class HospitalizationHx
    {
        public string HospitalizationHxDate { get; set; }
        public string HospitalizationHxId { get; set; }
        public string HospitalizationHxUnremarkable { get; set; }
        public string HospitalizationHxComments { get; set; }
        public string HospitalizationHxSoapText { get; set; }
        public string IsCreatedOrModified { get; set; }
        public string LastUpdated { get; set; }
       
    }
}