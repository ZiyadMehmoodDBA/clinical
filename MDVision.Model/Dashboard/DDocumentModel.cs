using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Model.Dashboard
{
    public class DDocumentModel
    {
        public string PatDocId { get; set; }
        public string FacilityName { get; set; }
        public string DocumentName { get; set; }
        public string DOS { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string PageNumber { get; set; }
        public string RowspPage { get; set; }
        public string RecordCount { get; set; }
        public string Pending { get; set; }
        public string Reviewed { get; set; }
        public string PatientId { get; set; }
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string DocPriority { get; set; }
        public string DocAssignToReview { get; set; }
        public string isReviewed { get; set; }
    }
    public class DPandingPatientDoc
    {
        public string PatDocId { get; set; }
        public string PatientId { get; set; }
    }
    public class DPatientDoucmnetModel
    {
        public List<DDocumentModel> ListDDocumentModel { get; set; }

        public List<DPandingPatientDoc> ListDPandingPatientDoc { get; set; }
    }
}
