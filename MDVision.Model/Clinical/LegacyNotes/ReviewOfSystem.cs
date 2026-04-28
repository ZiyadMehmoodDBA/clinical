using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class ReviewOfSystem
    {
        public string Type { get; set; }
        public Int64 ROSSystemId { get; set; }
        public string Name { get; set; }
        public bool IsNormal { get; set; }
        public string SystemNormalDescription { get; set; }
        public Int64 ROSDataSystemId { get; set; }
        public string CharacteristicsName { get; set; }
        public string Description { get; set; }
        public bool IsPositive { get; set; }
        public Int64 ROSDataSystemCharcID { get; set; }
        public Int64 ROSDataCharcDetailId { get; set; }
        public string PreviousHistory { get; set; }
        public string Onset { get; set; }
        public float Duration { get; set; }
        public string Location { get; set; }
        public string PrecipitatedBY { get; set; }
        public string AssociatedWith { get; set; }
        public string DetailStatusName { get; set; }
        public string DurationName { get; set; }
        public string PatternName { get; set; }
        public string SeverityName { get; set; }
        public string ContextName { get; set; }
        public string RadiationName { get; set; }
        public string CourseName { get; set; }
        public string FrequencyName { get; set; }
        public string CharacterCSZName { get; set; }
        public string AggravedByName { get; set; }
        public string RelievedByName { get; set; }
    }
}
