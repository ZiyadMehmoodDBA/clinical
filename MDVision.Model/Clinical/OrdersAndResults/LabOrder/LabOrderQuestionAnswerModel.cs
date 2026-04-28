using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder
{
    public class LabOrderQuestionAnswerModel
    {
        public string CPTCode { get; set; }
        public string commandType { get; set; }
        public int LabOrderAOEAnswersID { get; set; }
        public long LabOrderTestId { get; set; }
        //public string TestCode { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}