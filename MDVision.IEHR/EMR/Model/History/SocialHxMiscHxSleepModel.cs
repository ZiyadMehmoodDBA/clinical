using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SocialHxMiscHxSleepModel
    {
        public string SleepHxId { get; set; }
        public string MiscHxId { get; set; }
        public string MiscChildStatus { get; set; }


        public string MiscChildStatusText { get; set; }
        public string SleepHours { get; set; }
        public string SleepComments { get; set; }
        public string commandType { get; set; }
        public string SocialHxType { get; set; }

     
        public string SoapText { get; set; }
        public bool IsLast { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}