using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.CCDA
{
    public class DiagnosisModel
    {
        public DiagnosisModel()
        {
            this.Codes = new List<CodeModel>();
            this.Time = new TimeModel();
        }
        public List<CodeModel> Codes { get; set; }
        public TimeModel Time { get; set; }
        public string Status { get; set; }
    }
}