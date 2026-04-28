using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SocialHxMiscHxCaffeineIntakeModel
    {
        public string CaffeineIntakeHxId { get; set; }
        public string MiscHxId { get; set; }
        public string MiscChildStatus { get; set; }
        public string CaffeineIntakFrequency { get; set; }

        //Start 08/01/2016 Syed Zia Code, for Status Text
        public string MiscChildStatusText { get; set; }
        public string CaffeineIntakFrequency_text { get; set; }
        //End 11/01/2016 Syed Zia Code, for Status Text
        
        public string RadCaffieneharmful { get; set; }
        public string CaffeineComments { get; set; }
        public string commandType { get; set; }
        public string SocialHxType { get; set; }
        public bool IsLast { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}