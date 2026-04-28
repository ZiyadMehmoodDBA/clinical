using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.DataTemplates
{
    public class PhysicalDataTemplateSubChar
    {
        public PhysicalDataTemplateSubChar()
        {
            SubCharacteristicDetailModel = new PhysicalExamDataTemplateDetailModel();
        }
        public long DataTemplateSubCharId { get; set; }
        public long SubCharacteristicId { get; set; }        
        public bool IsPositive { get; set; }
        public bool IsNegative { get; set; }
        public string Comments { get; set; }    

        public bool IsDetailExists { get; set; }
        public PhysicalExamDataTemplateDetailModel SubCharacteristicDetailModel { get; set; }
    }
}