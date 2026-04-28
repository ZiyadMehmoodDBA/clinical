using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
    public class CProblemsModel
    {

        public string AccountNumber { get; set; }
        public string PatientFirstName { get; set; }
        public bool IncludeInactivePatient { get; set; }
        public string PatientLastName { get; set; }
        public string ChronicityLevel { get; set; }
        public string Problem { get; set; }
        public string Severity { get; set; }
        public string ProblemStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int EntityId { get; set; }

    }



    public class CProblemsFillModel
    {
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string PatStatus { get; set; }
        public string Problem { get; set; }
        public string ProblemStatus { get; set; }
        public string ChronicityLevel { get; set; }
        public string Severity { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string PatientId { get; set; }

    }


}
