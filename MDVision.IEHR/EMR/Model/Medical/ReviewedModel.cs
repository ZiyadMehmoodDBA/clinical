using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class ReviewedModel
    {
        public long PatientId { get; set; }
        public string WhichReviewed { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }
    }
}