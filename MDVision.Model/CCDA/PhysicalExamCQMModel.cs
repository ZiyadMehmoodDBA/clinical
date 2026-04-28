using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.CCDA
{
    public class PhysicalExamCQMModel
    {
        public PhysicalExamCQMModel()
        {
            this.Time = new TimeModel();
            this.codes = new List<CodeModel>();
        }
        public List<CodeModel> codes { get; set; }
        public TimeModel Time { get; set; }
        public string text { get; set; }
        public string Status { get; set; }
        public string ResultValue { get; set; }
        public string ResultUnit { get; set; }
        public string ActionPerformed { get; set; }
        public string originalText { get; set; }
        public string NegationValueset { get; set; }
    }
}