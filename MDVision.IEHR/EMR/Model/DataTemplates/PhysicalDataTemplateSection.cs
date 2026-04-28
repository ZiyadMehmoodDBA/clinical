using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.DataTemplates
{
    public class PhysicalDataTemplateSection
    {
        public PhysicalDataTemplateSection()
        {
            Characteristics = new List<PhysicalDataTemplateChar>();
        }
        public long DataTemplateSectionId { get; set; }
        public long DataTemplateId { get; set; }
        public long SectionId { get; set; }
        public long SystemId { get; set; }
        public List<PhysicalDataTemplateChar> Characteristics { get; set; }
    }
}