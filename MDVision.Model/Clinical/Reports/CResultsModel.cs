using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
    public class CResultsModel
    {
        public string AccountNumber { get; set; }
        public bool IncludeInactivePatient { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public long ProviderId { get; set; }
        public string LabId { get; set; }
        public string LabTest { get; set; }
        public string ResultNo { get; set; }
        public string ResultStatus { get; set; }
        public string PatientId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string ResultType { get; set; }

        public long AssigneeProvider { get; set; }
        public string Procedures { get; set; }

    }

    public class CResultsFillModel
    {
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string PatStatus { get; set; }
        public string Test { get; set; }
        public string Laboratory { get; set; }
        public string ResultNo { get; set; }
        public string ResultStatus { get; set; }
        public string Provider { get; set; }
        public string ResultDateTime { get; set; }
        public long PatientId { get; set; }
        public string ObservationDate { get; set; }

        public string Procedures { get; set; }

    }
}
