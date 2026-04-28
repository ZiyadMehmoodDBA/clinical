using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Procedures
{
    public class BarCodeLabelPrint
    {
        //public string LabOrderId { get; set; }
        //public string PatientId { get; set; }
        public string FullName { get; set; }
        public string PatientDOB { get; set; }
        public string LabName { get; set; }
        public string TestName { get; set; }
        public string OrderNo { get; set; }
        public string ClientNo { get; set; }
        public string TxtNoOfPrintName { get; set; }
    }
}
