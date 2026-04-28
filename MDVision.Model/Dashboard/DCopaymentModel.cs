using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Model.Dashboard
{
    public class DCopaymentModel
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string ProviderName { get; set; }
        public string RecordCount { get; set; }
        public string FacilityName { get; set; }
        public string Copay { get; set; }
        public string AppointmentStatus { get; set; }
        public string CopayDiscount { get; set; }
        public string CopayPaid { get; set; }
        public string VisitId { get; set; }
        public string AppointmentId { get; set; }
        public string FacilityId { get; set; }
        public string ProviderId { get; set; }
        public string ResourceId { get; set; }
        public string VisitStatus { get; set; }
        public string PatientAccount { get; set; }
    }
}
