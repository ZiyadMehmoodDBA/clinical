namespace MDVision.Model.CCM.CCMHub
{
    public class CCMCallDetailsModel
    {
        public string EnrollmentInfoId { get; set; }

        public string CallId { get; set; }
        public string Caller { get; set; }
        public string Caller_text { get; set; }
        public string ReceiverName { get; set; }
        public string ReasonId { get; set; }
        public string PatientId { get; set; }
        public string CallDate { get; set; }
        public string CallTime { get; set; }
        public string Duration { get; set; }
        public string DurationUnit { get; set; }
        public string CallReason { get; set; }
        public string DurationUnit_text { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string CareTeamId { get; set; }
        public string CallerType { get; set; }

        public string Action { get; set; }

    }
}
