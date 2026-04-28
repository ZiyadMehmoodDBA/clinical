/*
 * Author: Muhammad Arshad
 * Created Date: 07/12/2015
 * Created to define properties of AlcoholHx in SocialHx          
 */

using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SocialHxAlcoholModel
    {
        public string AlcoholId { get; set; }
        public string SocialHxId { get; set; }
        public string AlcoholStatus { get; set; }
        public string AlcoholType { get; set; }
        public string AlcoholUsagePeriod { get; set; }
        public string AlcoholFrequencyDays { get; set; }
        public string AlcoholCounsellingPeriod { get; set; }
        public string AlcoholCounsellingTopic { get; set; }
        public string AlcoholCessationLength { get; set; }
        public string AlcoholCessationPeriod { get; set; }
        public bool AlcoholRecentlyQuit { get; set; }
        public bool AlcoholNotReadyToQuit { get; set; }
        public bool AlcoholWouldQuit { get; set; }
        public string AlcoholComments { get; set; }
        /*
             Change Implement BY: Muhammad Azhar Shahzad
             Reason: These properties are being used in Progress note Soap Text Creation
             Created Date: Dec 15, 2015
         */
        public string Comments { get; set; }

        public string CounsellingTopic { get; set; }

        public string CounsellingPeriod { get; set; }

        public string CessationLength { get; set; }

        public bool bWouldQuit { get; set; }

        public string Status { get; set; }

        public bool bRecentlyQuit { get; set; }

        public string Frequency { get; set; }

        public string UsagePeriod { get; set; }

        public bool bNotReadyToQuit { get; set; }

        public string Type { get; set; }
        
        public string AlcoholType_text { get; set; }
        public string AlcoholUsagePeriod_text { get; set; }
        public string AlcoholFrequencyDays_text { get; set; }
        public string AlcoholCounsellingPeriod_text { get; set; }
        public string AlcoholCounsellingTopic_text { get; set; }
        public string AlcoholStatus_text { get; set; }
        public string AlcoholCessationPeriod_text { get; set; }
        public bool IsLast { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}