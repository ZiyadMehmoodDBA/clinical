using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.HL7
{
    public class PID
    {

        public string SetIDPID { get; set; }
        public string PatientID { get; set; }
        public string PatientIdentifierList { get; set; }
        public string AlternatePatientIDPID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string MotherMaidenName { get; set; }
        public string DateTimeOfBirth { get; set; }
        public string AdministrativeSex { get; set; }
        public string PatientAlias { get; set; }
        public string Race { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CountyCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberBusiness { get; set; }
        public string PrimaryLanguage { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string PatientAccountNumber { get; set; }
        public string SSNNumberPatient { get; set; }
        public string DriverLicenseNumberPatient { get; set; }
        public string MotherIdentifier { get; set; }
        public string EthnicGroup { get; set; }
        public string BirthPlace { get; set; }
        public string MultipleBirthIndicator { get; set; }
        public string BirthOrder { get; set; }
        public string Citizenship { get; set; }
        public string VeteransMilitaryStatus { get; set; }
        public string Nationality { get; set; }
        public string PatientDeathDateandTime { get; set; }
        public string PatientDeathIndicator { get; set; }
        public string IdentityUnknownIndicator { get; set; }
        public string IdentityReliabilityCode { get; set; }
        public string LastUpdateDateTime { get; set; }
        public string LastUpdateFacility { get; set; }
        public string SpeciesCode { get; set; }
        public string BreedCode { get; set; }
        public string Strain { get; set; }
        public string ProductionClassCode { get; set; }
    }
}
