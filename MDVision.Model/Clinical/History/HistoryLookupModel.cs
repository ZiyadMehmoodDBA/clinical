using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.History
{
    public class HistoryLookupModel
    {
        public string SocialHxStatusId { get; set; }
        public string SocialHxStatusDescription { get; set; }
        public string SocialHxSnomedCtCode { get; set; }
        public string TobaccoTypeId { get; set; }
        public string TobaccoTypeDescription { get; set; }
        public string TobaccoUsagePeriodId { get; set; }
        public string TobaccoUsagePeriodDescription { get; set; }
        public string TobaccoFrequencyId { get; set; }
        public string TobaccoFrequencyDescription { get; set; }
        public string MedicalHxStatusId { get; set; }
        public string MedicalHxStatusDescription { get; set; }
        public string FamilyHxFamilyMemberId { get; set; }
        public string FamilyHxFamilyMemberDescription { get; set; }
        public string FamilyHxHealthStatusId { get; set; }
        public string FamilyHxHealthStatusDescription { get; set; }
    }
}
