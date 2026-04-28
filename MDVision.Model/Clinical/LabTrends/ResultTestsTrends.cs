using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LabTrends
{
    public class ResultTestsTrends
    {
        public ResultTestsTrends()
        {
            this.ResultsValues = new List<ResultValue>();
        }
        public string LOINC { get; set; }
        public string LOINCDescription { get; set; }
        public string ReferenceRange { get; set; }
        public string Unit { get; set; }
        public string Flag { get; set; }
        public List<ResultValue> ResultsValues { get; set; }
    }
}
