using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class SocialHx
    {
        public Int64 SocialHxId { get; set; }
        public string HistoryType { get; set; }
        public string Comments { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string UsagePeriod { get; set; }
        public string Frequency { get; set; }
        public string CounsellingPeriod { get; set; }
        public string CounsellingTopic { get; set; }
        public string CessationLength { get; set; }
        public string CessationPeriod { get; set; }
        public string Drug { get; set; }
        public string Route { get; set; }
        public string FrequencyMothly { get; set; }
        public string SoapText { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Present { get; set; }
        public string Past { get; set; }
        public float SleepHours { get; set; }
        public string Diet { get; set; }
        public bool IsHarmful { get; set; }
        public string OverAllComments { get; set; }
        public string Preference { get; set; }
        public string ProtectionMethod { get; set; }
        public string ProtectionPeriod { get; set; }
        public string Complaint { get; set; }
        public string bUSingProtection { get; set; }
        public string bExposedToSTD { get; set; }
        public string bSexuallyAbused { get; set; }
        public string bPainWithIntercourse { get; set; }
        public string STDIds { get; set; }
        public DateTime? LMP { get; set; }
        public string bNotReadyToQuit { get; set; }
        public string bWouldQuit { get; set; }
        public string bRecentlyQuit { get; set; }
        public string bPregnancyStatus { get; set; }
        public string PregnancyDuration { get; set; }
        public DateTime occupationStartDate { get; set;}
        public DateTime occupationEndDate { get; set; }
    } 
}
