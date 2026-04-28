using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.OrdersAndResults.ConsultationOrder
{
    public class ConsultationOrderProblemModel
    {
        public string ConsultationOrderProblemId { get; set; }

        public string ConsultationOrderId { get; set; }

        public string ProblemId { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }
        public string commandType { get; set; }
    }
}
