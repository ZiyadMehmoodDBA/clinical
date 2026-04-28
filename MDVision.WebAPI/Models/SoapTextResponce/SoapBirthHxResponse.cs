using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SoapBirthHxResponse
    {
        public string status { get; set; } 
        public string BirthHxFill_JSON { get; set; }
        public string GeneralHxFill_JSON { get; set; }
        public string MaternalDeliveryHxFill_JSON { get; set; }
        public string NewBornFill_JSON { get; set; }
        public string BirthHxLoad_JSON { get; set; }
        public string LastUpdated { get; set; }
        public string IsCreatedOrModified { get; set; }
        public string BirthHxId { get; set; }

       
    }
}