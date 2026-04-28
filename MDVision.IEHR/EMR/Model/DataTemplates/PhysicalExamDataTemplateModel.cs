using MDVision.IEHR.EMR.Helpers.Clinical.PhysicalExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.DataTemplates
{
    public class PhysicalExamDataTemplateModel
    {
        public string DataTemplateId { get; set; }
        public string DataTemplateName { get; set; }
        public string TemplateName { get; set; }        
        public string TemplateId { get; set; }
        public string Comments { get; set; }        
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string EntityId { get; set; }

        public string ProviderNames { get; set; }
        public string ProviderIds { get; set; }
        public string SpecialtyNames { get; set; }
        public string SpecialtyIds { get; set; }
        public string commandType { get; set; }

        public int PageNo { get; set; }
        public int rpp { get; set; }

        public List<PhysicalDataTemplateSystem> Systems;
        public PhysicalExamDataTemplateModel()
        {
            Systems = new List<PhysicalDataTemplateSystem>();
        }
        
    }
}