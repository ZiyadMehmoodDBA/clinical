using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SurgicalHx
    {
        public string SurgicalHxDate { get; set; }
        public string SurgicalHxId { get; set; }
        public string SurgicalHxUnremarkable { get; set; }
        public string SurgicalHxComments { get; set; }
        public string SurgicalHxSoapText { get; set; }
        public string IsCreatedOrModified { get; set; }
        public string LastUpdated { get; set; }


       
    }
}