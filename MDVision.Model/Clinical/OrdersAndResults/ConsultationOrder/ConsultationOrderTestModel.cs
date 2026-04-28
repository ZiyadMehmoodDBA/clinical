using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.OrdersAndResults.ConsultationOrder
{
    public class ConsultationOrderTestModel
    {
        public string ConsultationOrderTestId { get; set; }
        public string ConsultationOrderId { get; set; }
        public string dtpConsultationDate { get; set; }
        public string tpConsultationTime { get; set; }
        public string ConsultationProcedure { get; set; }

        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }

        public string Urgency { get; set; }
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public string ConsultationTestIds { get; set; }
        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }
    }
}
