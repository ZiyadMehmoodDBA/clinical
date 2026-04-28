using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.ReviewofSystems
{
    public class ROSSystemInfoModel
    {
        public long ROSSystemInfoID { get; set; }
        public string Description { get; set; }
        public bool IsNormal { get; set; }
        public string ROSSystemDate { get; set; }
        public string Comments { get; set; }

        public long NotesId { get; set; }

        public long ROSTemplateId { get; set; }

        public long ROSDataTemplateId { get; set; }
    }
}