using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.Dashboard
{
    public class PatientPortalSignupModel
    {
        public string PatientId { get; set; }
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string EmailAddress { get; set; }
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public string UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public string RequestDateTime { get; set; }
        public string UpdateDateTime { get; set; }
        public string providerId { get; set; }
        public string ProviderName { get; set; }
        public string SercurityQuestionId { get; set; }
        public string Answer { get; set; }
        public string RecordCount { get; set; }
        public string FirstName { get; set; }
        public string lastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}