namespace MDVision.Model.Patient
{
    public class PatientModel
    {
        public string CommandType { get; set; }
        public string AccountNo { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
        public string MRN { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string Provider { get; set; }
        public string Facility { get; set; }
        public string Practice { get; set; }

        public string HomePhone { get; set; }
        public string Email { get; set; }
        public string BadAddress { get; set; }
        public string Sex { get; set; }
        public string CoverageType { get; set; }
        public string Active { get; set; }
        public string Active_Text { get; set; }
        public string ClaimNumber { get; set; }
        public string ProviderID { get; set; }
        public string FacilityID { get; set; }
        public string PracticeID { get; set; }
        public string PatientID { get; set; }
        public string InsurancePlanID { get; set; }
        public string PageNo { get; set; }
        public string rpp { get; set; }
        public string IsRecentPatients { get; set; }
        public string AppointmentDate { get; set; }
        public string IncompleteDemographics { get; set; }
        public string GuarantorId { get; set; }
    }
}
