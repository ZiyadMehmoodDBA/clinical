using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Admin.Codes
{
    public class CPTNdcModel
    {
        public string CPTNdcId { get; set; }
        public string CPTCodeId { get; set; }
        public string NDCCode { get; set; }
        public string NDCDescription { get; set; }
        public string Unit { get; set; }
        public string UnitPrice { get; set; }
        public string NDCMeasurementId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string NDCMeasurementText { get; set; }

    }
}
