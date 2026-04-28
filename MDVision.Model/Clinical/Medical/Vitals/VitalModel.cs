namespace MDVision.Model.Clinical.Medical.Vitals
{
    public class VitalModel
    {

        public string Weight { get; set; }
        public string EntityId { get; set; }
        public string Height { get; set; }
        public string PageNo { get; set; }
        public string rpp { get; set; }
        public string commandType { get; set; }
        public string VitalSignsId { get; set; }
        public string PatientId { get; set; }
        public string VisitId { get; set; }
        public string SPO2 { get; set; }
        public string InhaledO2Concentration { get; set; }
        public string OxygenSource { get; set; }
        public string PeakFlow { get; set; }
        public string SeverityofPain { get; set; }
        public string SmokingStatus { get; set; }
        public string Comments { get; set; }
        public string VitalSignDate { get; set; }
        public string VitalsTime { get; set; }
        public string BMI { get; set; }
        public string BSA { get; set; }
        public string HeadCir { get; set; }
        public string BloodType { get; set; }
        public string DeleteComments { get; set; }
        // Notes Search Properties
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string provider { get; set; }
        public string NoteStatus { get; set; }
        public string VisitFrom { get; set; }
        public string VisitTo { get; set; }
        public string NoteType { get; set; }
        public string NotesId { get; set; }

        // Start 30/11/2015 Muhammad Irfan Bug # 37
        public string AccountNumber { get; set; }

        // End 30/11/2015 Muhammad Irfan Bug # 37
        public string UserId { get; set; }
        public long RiskAssessmentId { get; set; }
        public string IsFromNote { get; set; }
        public string NegationReason { get; set; }

    }
}