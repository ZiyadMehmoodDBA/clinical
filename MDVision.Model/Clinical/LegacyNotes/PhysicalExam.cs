using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class PhysicalExam
    {

        public string TemplateName { get; set; }
        public string PETemplateId { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public Int64 PETemplateSystemId { get; set; }
        public string ObservationName { get; set; }
        public Int64 PESystemId { get; set; }
        public bool IsSelectedSystem { get; set; }

    }
}
