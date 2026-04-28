using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
    public class CMedicationModel
    {
        public bool IncludeInactivePatient { get; set; }
        public string AccountNumber { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public bool MedicationAND { get; set; }
        public string MedicationStatus { get; set; }
        public string Medication { get; set; }
        public string StartDate { get; set; }
        public string ENDDate { get; set; }
    }

    public class CMedicationFillModel
    {

        public string AccountNumber { get; set; }

        public string PatientName { get; set; }

        public string DOB { get; set; }

        public string PatStatus { get; set; }

        public string Medication { get; set; }

        public string MedStatus { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public long PatientId { get; set; }
    }
}
