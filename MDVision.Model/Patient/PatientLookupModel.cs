using System.ComponentModel.DataAnnotations;

namespace MDVision.Model.Patient
{
    public class PatientLookupModel
    {
        public string PatientID { get; set; }
        public string AccountNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string DOB { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public string SSN { get; set; }
        public string imagedata { get; set; }
        public string MaritalStatusId { get; set; }
        public string MaritalStatus { get; set; }
        public string EthnicityId { get; set; }
        public string EthnicityDescription { get; set; }
        public string RaceId { get; set; }
        public string RaceDescription { get; set; }
        public string LanguagesId { get; set; }
        public string LanguagesDescription { get; set; }
        public string RelationshipId { get; set; }
        public string RelationshipDescription { get; set; }
        public string PatientImageThumbnail { get; set; }



    }
}
