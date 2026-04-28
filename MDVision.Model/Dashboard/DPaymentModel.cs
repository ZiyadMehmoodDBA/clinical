using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Model.Dashboard
{
    public class DPaymentModel
    {
        public string PracticeName { get; set; }
        public string ProviderName { get; set; }
        public string RecordCount { get; set; }
        public string FacilityName { get; set; }
        public string InsurancePlan { get; set; }
        public string InsurancePaid { get; set; }
        public string PatientPaid { get; set; }
        public string CopayPaid { get; set; }
        public string ClaimNumber { get; set; }
        public string VisitId { get; set; }
        public string PatientId { get; set; }
    }
}
