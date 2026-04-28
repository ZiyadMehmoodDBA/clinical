using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
    public class CAllergiesModel
    {
        public bool IncludeInactivePatient { get; set; }
        public string Allergy { get; set; }
        public string AllergyReaction { get; set; }
        public bool AllergyAND { get; set; }
        public string AllergyStatus { get; set; }

        public string AccountNumber { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
    }

    public class CAllergiesFillModel
    {

        public string AccountNumber { get; set; }

        public string PatientName { get; set; }

        public string DOB { get; set; }

        public string PatStatus { get; set; }

        public string Allergy { get; set; }

        public string Reaction { get; set; }

        public string OnSetDate { get; set; }

        public string AllergyStatus { get; set; }

       

        public long PatientId { get; set; }
    }
}
