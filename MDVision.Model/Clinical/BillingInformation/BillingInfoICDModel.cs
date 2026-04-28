using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.BillingInformation
{
    public class BillingInfoICDModel
    {
        public string BillingInfoICDId { get; set; }
        public string BillingInfoId { get; set; }
        public string ICDType { get; set; }
        public string ICDCode9 { get; set; }
        public string ICDDescription9 { get; set; }
        public string ICDCode10 { get; set; }
        public string ICDDescription10 { get; set; }
        public string SNOMEDCode { get; set; }
        public string SNOMEDDescription { get; set; }
        public string CustomFormId { get; set; }

    }
}
