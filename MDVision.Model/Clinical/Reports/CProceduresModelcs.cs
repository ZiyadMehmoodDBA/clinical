using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
    public class CProceduresModelcs
    {
        public string AccountNumber { get; set; }
        public bool IncludeInactivePatient { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public long ProviderId { get; set; }
        public string CPTCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class CProceduresFillModelcs
    {
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string PatStatus { get; set; }
        public string Provider { get; set; }
        public string Procedure { get; set; }
        public string ICD { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public long PatientId { get; set; }
        
    }
}
