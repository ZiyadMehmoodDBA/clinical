using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.PMSSchedule
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}
