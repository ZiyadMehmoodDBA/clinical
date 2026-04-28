using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class PatientLastUpdateInfoModel
    {
        public string PatientID { get; set; }
        public DateTime AllergyLastUpdateDate { get; set; }
    }
}