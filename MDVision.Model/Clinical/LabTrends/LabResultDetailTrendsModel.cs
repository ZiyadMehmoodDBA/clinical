using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LabTrends
{
    public class LabResultDetailTrendsModel
    {
        public LabResultDetailTrendsModel()
        {
            this.ResultDates = new List<string>();
            this.TestTrends = new List<ResultTestsTrends>();
        }
        public string TestCode { get; set; }
        public string TestDescription { get; set; }
        public List<string> ResultDates { get; set; }
        public List<ResultTestsTrends> TestTrends { get; set; }
    }
}
