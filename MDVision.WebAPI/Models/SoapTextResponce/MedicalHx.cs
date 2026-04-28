using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class MedicalHx
    {
        public string MedicalHxDate { get; set; }
        public string MedicalHxId { get; set; }
        public string MedicalHxUnremarkable { get; set; }
        public string MedicalHxComments { get; set; }
        public string MedicalHxSoapText { get; set; }
        public string IsCreatedOrModified { get; set; }
        public string LastUpdated { get; set; }
    }
}