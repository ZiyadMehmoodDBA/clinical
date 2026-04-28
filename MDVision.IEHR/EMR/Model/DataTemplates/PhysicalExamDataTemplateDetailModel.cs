using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.DataTemplates
{
    public class PhysicalExamDataTemplateDetailModel
    {

        public string PhysExamDataTemplateId { get; set; }
        public string DetailId { get; set; }    
        public string PrevHistory { get; set; }
        public string Status { get; set; }
        public string Onset { get; set; }
        public string DurationLength { get; set; }
        public string DurationPeriod { get; set; }
        public string Pattern { get; set; }
        public string Severity { get; set; }
        public string Course { get; set; }
        public string Radiation { get; set; }
        public string Frequency { get; set; }
        public string Context { get; set; }
        public string Character { get; set; }
        public string AggravatedBy { get; set; }
        public string Relievedby { get; set; }
        public string Location { get; set; }
        public string Percipitatedby { get; set; }
        public string Associatedwith { get; set; }    
 
        public string Status_text { get; set; }
        public string Context_text { get; set; }
        public string DurationPeriod_text { get; set; }
        public string Pattern_text { get; set; }
        public string Severity_text { get; set; }
        public string Course_text { get; set; }
        public string Character_text { get; set; }
        public string Agggravatedby_text { get; set; }
        public string Relievedby_text { get; set; }
        public string Radiation_text { get; set; }
        public string Frequency_text { get; set; }
     
        public string SystemId { get; set; }
        public string SectionId { get; set; }
        public string CharacteristicId { get; set; }
        public string SubCharacteristicId { get; set; }
        public string IsCharacteristicPositive { get; set; }
        public string IsSubCharacteristicPositive { get; set; }
    }
}