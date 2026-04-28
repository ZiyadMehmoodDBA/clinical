using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.OrdersAndResults.ProcedureOrder
{
    public class ProcedureOrderTestModel
    {
        public string ProcedureOrderTestId { get; set; }
        public string ProcedureOrderId { get; set; }
        public string dtpProcedureDate { get; set; }
        public string tpProcedureTime { get; set; }
        public string ProcedureProcedure { get; set; }

        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }

        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }

        public string Urgency { get; set; }
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public string ProcedureTestIds { get; set; }
        public string Comments { get; set; }
        public string PatientId { get; set; }

    }
}