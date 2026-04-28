using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LabTrends
{
    public class PatPracticeModel
    {
        public long   PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string Gender { get; set; }
        public string PatientAddress { get; set; }
        public string  PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZipCode { get; set; }
        public string PatientDOB { get; set; }
        public string PracticeName { get; set; }
        public string PracticeAddress { get; set; }
        public string PracticeCity { get; set; }
        public string PracticeState { get; set; }
        public string PracticeZIP { get; set; }

        public string ProviderName { get; set; }
        public string ProviderNPI { get; set; }
        public string ProviderSpecialty { get; set; }

    }
}
