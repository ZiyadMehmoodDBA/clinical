using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Notes.Notes
{
    public class TemplateData
    {
        public string ShortName { get; set; }
        public Int64 ROSDataTemptId { get; set; }
        public Int64 PEDataTemptId { get; set; }
        public Int64 TemplateTypeId { get; set; }
        public string HTMLTemplate { get; set; }
        public bool a { get; set; }
        public string b { get; set; }
    }
}
