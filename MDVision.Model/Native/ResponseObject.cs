using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native
{
    public class ResponseObject
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
