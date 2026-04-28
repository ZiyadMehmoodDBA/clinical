using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.HL7
{
    public class HL7Patient
    {
        public string PatientId { get; set; }
        public string Patient_Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string Suffix { get; set; }
        public string Prefix { get; set; }
        public DateTime DOB { get; set; }
        public string MotherMaidenName { get; set; }
        public string Sex { get; set; }

        public string AccountNumber { get; set; }

        public string age { get; set; }

        public string Language { get; set; }

        public string PatientAddress1 { get; set; }

        public string Email { get; set; }

        public string HomePhoneNo { get; set; }

        public string WorkPhoneNo { get; set; }

        public string CellNo { get; set; }

        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string GurranterName { get; set; }
        public string MaritialStatus { get; set; }
        public string SSNNumberPatient { get; set; }
        public string RaceName { get; set; }
        public string RaceCode{ get; set; }
    }
}