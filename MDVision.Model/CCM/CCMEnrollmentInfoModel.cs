namespace MDVision.Model.CCM
{
    public class CCMEnrollmentInfoModel
    {
        public string EnrollmentInfoId { get; set; }
        public string StatusId { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public string ProviderId { get; set; }
        public string Problems { get; set; }
        public string NoOfProblems { get; set; }

        public string InsuranceName { get; set; }
        public string ProviderName { get; set; }
        public string Program { get; set; }
        public string Program_text { get; set; }
        public string Duration { get; set; }
        public string Duration_text { get; set; }
        public string DurationUnit { get; set; }
        public string DurationUnit_text { get; set; }
        public string StartingFrom { get; set; }
        public string EndingOn { get; set; }
        public string CareTeam { get; set; }
        public string CareTeam_text { get; set; }
        public string Upload_Import_file { get; set; }
        public string ConsentPath { get; set; }
        public string ConsentFileName { get; set; }
        public string ConsentFileStream { get; set; }
        public string ConsentDate { get; set; }
        public string Consent { get; set; }
        public string DeclineReason { get; set; }
        public string Status { get; set; }
        public string TimeCompleted { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ISVerbal { get; set; }
        public string RecordCount { get; set; }
        public byte[] BinaryData { get; set; }
        public string NoteStatus { get; set; }
        public string Url { get; set; }
    }

    public class EnrollmentInfoProgram
    {
        public long ProgramId { get; set; }
        public string ProgramName { get; set; }
    }
}
