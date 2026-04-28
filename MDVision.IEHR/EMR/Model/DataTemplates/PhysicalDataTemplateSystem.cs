using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.DataTemplates
{
    public class PhysicalDataTemplateSystem
    {
        public long DataTemplateSysId { get; set; }
        public long PhysicalExamSystemId { get; set; }        
        public long DataTemplateId { get; set; }
        public long SystemId { get; set; }       
        public bool IsNormal { get; set; }
        public string Comments { get; set; }
        public List<PhysicalDataTemplateSection> Sections { get; set; }
        public PhysicalDataTemplateSystem()
        {
            Sections = new List<PhysicalDataTemplateSection>();
        }
    }
}