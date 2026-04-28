using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder
{
    public class LabOrderProblemModel
    {
        public string LabOrderProblemId { get; set; }

        public string LabOrderId { get; set; }

        public string ProblemId { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}