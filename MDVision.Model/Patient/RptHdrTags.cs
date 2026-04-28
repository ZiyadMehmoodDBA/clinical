using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Patient
{
    public class RptHdrTags
    {
        public string NotesId { get; set; }
        public string PracticeText { get; set; }
        public string PatientText { get; set; }
        public string ProviderText { get; set; }
        public string HeaderLogo { get; set; }
        public string FooterText { get; set; }
        public string PatientName { get; set; }
        public string PatientDOB { get; set; }
        public string ProviderName { get; set; }
        public string DOS { get; set; }
        public bool IsProviderHeader { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public int reportHeaderCount { get; set; }
    }
}
