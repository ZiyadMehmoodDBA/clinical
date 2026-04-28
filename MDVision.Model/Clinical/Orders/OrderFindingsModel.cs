using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Orders
{
   public class OrderFindingsModel
    {
        public long OrderId { get; set; }
        public long OrderTestId { get; set; }
        public string TestName { get; set; }
        public string SystemName { get; set; }
        public string Findings { get; set; }
    }
}
