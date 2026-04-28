using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.ProcedureOrder
{
    public class ProcedureOrderProblemModel
    {
        public string ProcedureOrderProblemId { get; set; }

        public string ProcedureOrderId { get; set; }

        public string ProblemId { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }
        public string commandType { get; set; }        
    }
}