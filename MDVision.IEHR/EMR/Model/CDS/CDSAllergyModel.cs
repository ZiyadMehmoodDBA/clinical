using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.CDS
{
    public class CDSAllergyModel
    {
        public long? CDSAllergyId { get; set; }
        public string CDSId { get; set; }
        public string Allergen { get; set; }
        public string Comments { get; set; }
        public string ModifiedOn { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string CDSAllergyQuery { get; set; }
        public string AllergyOperator { get; set; }
        public string AllergyForQuery { get; set; }
    }
}