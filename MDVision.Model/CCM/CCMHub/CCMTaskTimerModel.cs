namespace MDVision.Model.CCM.CCMHub
{
    public class CCMTaskTimerModel
    {
        public string EnrollmentInfoId { get; set; }
        public string TaskTimerAmalgamatedId { get; set; }
        public string TaskTimerId { get; set; }
        public string TaskReason { get; set; }
        public string AddedBy { get; set; }
        public string PatientId { get; set; }
        public string TaskDate { get; set; }
        public string TaskTime { get; set; }
        public string TaskHours { get; set; }
        public string TaskMinutes { get; set; }
        public string TaskSeconds { get; set; }
        public string TaskDuration { get; set; }
        public string Duration_text { get; set; }
        public string DurationUnit { get; set; }
        public string DurationUnit_text { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public string Action { get; set; }
        public long RecordCount { get; set; }
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
        public int SelectedMonth { get; set; }
    }

    public class TaskAmalgamatedModel
    {
        public string EnrollmentInfoId { get; set; }
        public string PatientId { get; set; }
        public string TaskTimerId { get; set; }

        public string TaskTimerAmalgamatedId { get; set; }

        public string TaskDate { get; set; }
        public string TaskTime { get; set; }
        public string ReasonId { get; set; }
        public string TaskReason { get; set; }
        public string AddedBy { get; set; }

        public string CallDate { get; set; }
        public string CallTime { get; set; }

        public string TaskHours { get; set; }
        public string TaskMinutes { get; set; }
        public string TaskSeconds { get; set; }

        public string TaskDuration { get; set; }
        public string Duration_text { get; set; }
        public string DurationUnit { get; set; }
        public string DurationUnit_text { get; set; }

        public string Comments { get; set; }
        public string Caller { get; set; }
        public string Caller_RefValue { get; set; }
        public string Caller_text { get; set; }
        public string CallerType { get; set; }
        public string ReceiverName { get; set; }

        public string CareTeamId { get; set; }

        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Action { get; set; }
        public long RecordCount { get; set; }
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
        public int SelectedMonth { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public string ModifiedByName { get; set; }
        public string CreatedByName { get; set; }
    }
}
