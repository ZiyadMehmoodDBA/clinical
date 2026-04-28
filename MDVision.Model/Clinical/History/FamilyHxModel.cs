using System.ComponentModel.DataAnnotations;

namespace MDVision.Model.Clinical.History
{
    public class FamilyHxModel
    {
        public string HealthStatusId { get; set; }
        public string SoapText{ get; set; }
        public string Action { get; set; }
        public string MemberDetailId { get; set; }
        public string MemberId { get; set; }
        public string DiseaseId { get; set; }
    }
}
