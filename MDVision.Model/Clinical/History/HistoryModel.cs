using System.ComponentModel.DataAnnotations;

namespace MDVision.Model.Clinical.History
{
    public class HistoryModel
    {
        public string SocialHxId { get; set; }
        public string TobaccoId { get; set; }
        public string SocialHxStatusId { get; set; }
        public string SocialHxTypeId { get; set; }
        public string SocialHxUsagePeriodId { get; set; }
        public string SocialHxFrequencyId { get; set; }
        public string SocialHxSoapText { get; set; }
        public string SocialHxAction { get; set; }
    }
}
