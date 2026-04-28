using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models
{
    public class SoapTextBirthHxModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string NewbornId { get; set; }
        public string MaternalDeliveryId { get; set; }
        public string GeneralId { get; set; }
        public string SoapText { get; set; }
        public string BirthHxId { get; set; }
        public string IsCreatedOrModified { get; set; }
        public string LastUpdated { get; set; }
    }
}