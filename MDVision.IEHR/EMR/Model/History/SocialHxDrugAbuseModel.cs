
/*
 * Author: Muhammad Arshad
 * Created Date: 07/12/2015
 * Created to define properties of DrugHx in SocialHx          
 */

using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SocialHxDrugAbuseModel
    {
        public string DrugAbuseId { get; set; }
        public string SocialHxId { get; set; }
        public string DrugStatus { get; set; }
        public string DrugType { get; set; }
        public string DrugRoute { get; set; }
        public string DrugFrequencyDay { get; set; }
        public string DrugFrequencyMonth { get; set; }
        public string DrugUsagePeriod { get; set; }
        public string DrugCessationLength { get; set; }
        public string DrugCessationPeriod { get; set; }
        public bool DrugRecentlyQuit { get; set; }
        public bool DrugWouldQuit { get; set; }
        public string DrugComments { get; set; }
        /*
             Change Implement BY: Muhammad Azhar Shahzad
             Reason: These properties are being used in Progress note Soap Text Creation
             Created Date: Dec 15, 2015
         */
        public bool bWouldQuit { get; set; }

        public string CessationLength { get; set; }

        public string Status { get; set; }

        public bool bRecentlyQuit { get; set; }

        public string FrequencyDaily { get; set; }

        public string UsagePeriod { get; set; }

        public string Comments { get; set; }

        public string DrugStatus_text { get; set; }
        public string DrugType_text { get; set; }
        public string DrugRoute_text { get; set; }
        public string DrugFrequencyDay_text { get; set; }
        public string DrugFrequencyMonth_text { get; set; }
        public string DrugUsagePeriod_text { get; set; }
        public string DrugCessationPeriod_text { get; set; }
        public bool IsLast { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}