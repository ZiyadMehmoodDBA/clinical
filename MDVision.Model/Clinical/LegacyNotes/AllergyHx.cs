using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class AllergyHx
    {
        public Int64 AllergyId { get; set; }
        public string Type { get; set; }
        public string Allergen { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
        public string RxnormIDType { get; set; }
        public string Comments { get; set; }
        public string InActiveCheckBoxValue { get; set; }
        public string InActiveReason { get; set; }
        public DateTime? OnSetDate { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
