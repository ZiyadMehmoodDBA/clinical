using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Billing.FollowUp
{
    public class FollowUpPatientARModel
    {
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string ActionID { get; set; }
        public string ActionName { get; set; }
        public string Reason { get; set; }
        public string ReasonName { get; set; }
        public string InsurancePlan { get; set; }
        public string PatientAccount { get; set; }
        public string Facility { get; set; }
        public string Provider { get; set; }
        public string ClaimNumber { get; set; }
        public string DatePaidFrom { get; set; }
        public string DatePaidTo { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Suspend { get; set; }
        public string Age { get; set; }
        public string FacilityID { get; set; }
        public string ProviderID { get; set; }
        public string PatientID { get; set; }
        public string VisitID { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string ARType { get; set; }
        public string CommandType { get; set; }
        public string FollowUpPatientARID { get; set; }
        public string FollowUpPatientARDetailID { get; set; }
        public string RemitanceCode { get; set; }
        public string RemitanceCodeText { get; set; }
        public string SuspendedDate { get; set; }
        public string Comments { get; set; }
        public string SSN { get; set; }
        public string MiddleInitial { get; set; }
        public string DateofBirth { get; set; }
        public string Practice { get; set; }
        public string TaxID { get; set; }
        public string InsuranceTelephone { get; set; }
        public string SubscriberID { get; set; }
        public string EligibilityDate { get; set; }
        public string EligibilityStatus { get; set; }
        public string ICN { get; set; }
        public string FirstStatementDate { get; set; }
        public string StatementDate { get; set; }
        public string PracticeID { get; set; }
        public string InsSSN { get; set; }
        public string InsLastName { get; set; }
        public string InsFirstName { get; set; }
        public string InsGroupID { get; set; }
        public string InsMiddleInitial { get; set; }
        public string LastModified { get; set; }
        public string LastComment { get; set; }

    }
}