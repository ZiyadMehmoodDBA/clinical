using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.FollowUp
{
    public class OrdertSetFollowUpModel
    {
        public string FollowUpId { get; set; }
        public string OrderSetId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Comments { get; set; }
        public string Reason { get; set; }
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string FacilityName { get; set; }
        public string FacilityId { get; set; }
        public string Duration { get; set; }
        public string Time { get; set; }
        public string ScheduleCount { get; set; }
        public string ScheduleType { get; set; }
        public string commandType { get; set; }
        public string FollowUpText { get; set; }
        public string ModifiedByName { get; set; }
        public string CreateAppointment { get; set; }
        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string IsDefault { get; set; }
        public string PageNumber { get; set; }
        public string RowspPage { get; set; }
        public string RecordCount { get; set; }
        public string FollowUpDays { get; set; }
        public string FollowUpMonths { get; set; }
        public string FollowUpYears { get; set; }



    }

    public class OrdertSetFollowUpResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public string FollowUpId { get; set; }
    }
}
