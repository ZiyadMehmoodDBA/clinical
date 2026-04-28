using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Batch.Fax
{
    public class FaxOutboxDetailModel
    {
        public string FaxDetailsID { get; set; }
        public string UserId { get; set; }
        public string ProviderId { get; set; }
        public string Subject { get; set; }
        public string SentToName { get; set; }
        public string ToFaxNumber { get; set; }
        public string CallerID { get; set; }
        public string FileName { get; set; }
        public string Pages { get; set; }
        public string SentStatus { get; set; }
        public string ErrorCode { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string IsDeleted { get; set; }
        public string SenderName { get; set; }
        public string RecordCount { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string PatientId { get; set; }
        public string ProviderName { get; set; }
    }
}
