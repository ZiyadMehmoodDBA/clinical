using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.Clinical.EMCodeGenerator
{

    public class EMCodeGeneratorHistoryModel
    {
        public Boolean IsChiefComplaint { get; set; }
        public string HPI_Location { get; set; }
        public long HPI_Quality { get; set; }
        public long HPI_Severity { get; set; }
        public long HPI_Duration { get; set; }
        public long HPI_Timing { get; set; }
        public long HPI_Context { get; set; }
        public long HPI_ModifyingFactors { get; set; }
        public string HPI_AssociatedSignsAndSymptoms { get; set; }
        public long FamilyHx { get; set; }
        public long SocialHx { get; set; }
        public long MedicalHx { get; set; }
        public long HPI_Count { get; set; }
        public long ROS_Count { get; set; }
        public long Hx_Count { get; set; }
    }
}