using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Billing.PatientStatement
{

 

    public class PatientStatementModel
    {
        public string AccountNumber { get; set; }
        public string Provider { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Facility { get; set; }
        public string Practice { get; set; }
        public string Age { get; set; }
        public string DOSFrom { get; set; }
        public string DOSTo { get; set; }
        public string ClearingHouse { get; set; }
        public string ClearingHouseId { get; set; }
        public string StatementFormat { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public string FacilityId { get; set; }
        public string PracticeId { get; set; }
        public string CommandType { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string PatientStatementID { get; set; }
        public string ItemList { get; set; }
        public string MarkUp { get; set; }
        public string isFromSubmitted { get; set; }
        public string SelectedStatementCount { get; set; }
        public string IsFirstSubmit { get; set; }
        public string PatBalGreater { get; set; }
        public string PatBalLess { get; set; }
        public string LastStatmentDateFrom { get; set; }
        public string LastStatmentDateTo { get; set; }

        public string BatchId { get; set; }
        public string BatchNumber { get; set; }
        public string SubmittedDate { get; set; }
        public string SubmittedBy  { get; set; }

        public string SubmittedById { get; set; }
        public string SubmitType { get; set; }
        public string BatchStatus { get; set; }

        public string SubmittedChargeIds { get; set; }
        public string IsResubmit { get; set; }
        public string BatchResubmit { get; set; }
        public string SubmittedStatementId { get; set; }

        public bool Status = false;
        public bool IgnoreCycleDays;
        public string Message = string.Empty;
        public int SubmittedStatementCount = 0;
    }

}