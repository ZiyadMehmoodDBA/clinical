using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LabTrends
{
   public class LabTrends
    {
        public LabTrends()
        {
            PatPracticeModel = new PatPracticeModel();
        }
        public string TestCode { get; set; }
        public string TestDescription { get; set; }
        public string ResultDatesXML { get; set; }
        public string TestsXML { get; set; }
        public PatPracticeModel PatPracticeModel { get; set; }
        
    }
    public class Letter
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
    }
}
