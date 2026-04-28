using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.LabOrder
{
    public class LabOrderProblemModel
    {
        public string LabOrderProblemId { get; set; }

        public string LabOrderId { get; set; }

        public string ProblemId { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }
        public string commandType { get; set; }
    }
}