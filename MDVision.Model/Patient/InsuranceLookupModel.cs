using System.ComponentModel.DataAnnotations;

namespace MDVision.Model.Patient
{
    public class InsuranceLookupModel
    {
        public string InsurancePlanId { get; set; }
        public string ShortName { get; set; }
    }
}
