using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Schedule
{
   
    public class VisitTypeDuartions
    {
        public string VisitTypeId { get; set; }
        public string Duration { get; set; }
        public string Color { get; set; }
        public string VisitDurationGroupId { get; set; }
        
    }

   

    public class VisitTypeDurationsModel
    {
        public VisitTypeDuartions[] VisitTypeDurations { get; set; }
       
        public string IsActive { get; set; }
        public string VisitDurationId { get; set; }
        public string VisitDurationGroupName { get; set; }
        public string VisitGroupId { get; set; }
    }
    
}
