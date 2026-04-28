using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LabTrends
{
    public class ResultValue
    {
        public string Value { get; set; }
        public string Date { get; set; }
        public string ReferenceRange { get; set; }
        public string Unit { get; set; }
        public string Flag { get; set; }
        public string OrderNumber { get; set; }
        public string Comments { get; set; }
        public string ReferenceRangeInterpration { get; set; }
        public string ReferenceRangeDescription { get; set; }
    }
}
