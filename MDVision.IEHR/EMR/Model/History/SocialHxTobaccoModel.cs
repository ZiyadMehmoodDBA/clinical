/*
 * Author: Muhammad Irfan
 * Created Date: 04/12/2015
 * Created to define properties of TobaccoHx in SocialHx          
 */


using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SocialHxTobaccoModel
    {
        public string TobaccoId { get; set; }
        public string SocialHxId { get; set; }
        public string SmokingStatus { get; set; }
        public string TobaccoType { get; set; }
        public string TobaccoUsagePeriod { get; set; }
        public string TobaccoFrequencyDaily { get; set; }
        public string TobaccoCounsellingPeriod { get; set; }
        public string TobaccoCounsellingTopic { get; set; }
        public string TobaccoCessationLength { get; set; }
        public string TobaccoCessationPeriod { get; set; }
        public bool TobaccoRecentlyQuit { get; set; }
        public bool TobaccoWouldQuit { get; set; }
        public string TobaccoComments { get; set; }

        /*
             Change Implement BY: Muhammad Azhar Shahzad
             Reason: These properties are being used in Progress note Soap Text Creation
             Created Date: Dec 15, 2015
         */
        public string Status { get; set; }

        public string UsagePeriod { get; set; }

        public bool bRecentlyQuit { get; set; }

        public bool bWouldQuit { get; set; }

        public string Type { get; set; }

        public string Frequency { get; set; }

        public string CessationLength { get; set; }

        public object CounsellingTopic { get; set; }

        public string CounsellingPeriod { get; set; }

        public string Comments { get; set; }     

        public string TobaccoType_text { get; set; }
        public string TobaccoUsagePeriod_text { get; set; }
        public string TobaccoFrequencyDaily_text { get; set; }
        public string TobaccoCounsellingPeriod_text { get; set; }
        public string TobaccoCounsellingTopic_text { get; set; }        
        public string TobaccoCessationPeriod_text { get; set; }
        public string SmokingStatus_text { get; set; }
        public bool IsLast { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}