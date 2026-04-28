using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.ReviewofSystems
{
    public class ROSSystemPatientModel
    {
        public long ROSSystemPatientID { get; set; }
        public long ROSSystemID { get; set; }
        public long PatientId { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public bool IsNormal { get; set; }
        public long ROSSystemInfoID { get; set; }
        public List<string> ROSSystemIds { get; set; }
    }
}