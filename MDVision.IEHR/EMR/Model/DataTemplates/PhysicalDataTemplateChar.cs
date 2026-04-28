using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.DataTemplates
{
    public class PhysicalDataTemplateChar
    {
        public PhysicalDataTemplateChar()
        {
            SubCharacteristics = new List<PhysicalDataTemplateSubChar>();
            SectionCharacteristicDetailModel = new PhysicalExamDataTemplateDetailModel();
        }
        public long DataTemplateCharId { get; set; }
        public long DataTemplateId { get; set; }     
        public string Comments { get; set; }        
        public bool IsPositive { get; set; }
        public bool IsNegative { get; set; }
        public bool IsDetailExists { get; set; }
        public long SectionCharacteristicId { get; set; }
        public long SectionId { get; set; }
        public long CharId { get; set; }         
         
        public List<PhysicalDataTemplateSubChar> SubCharacteristics { get; set; }
        public PhysicalExamDataTemplateDetailModel SectionCharacteristicDetailModel { get; set; }
    }
}