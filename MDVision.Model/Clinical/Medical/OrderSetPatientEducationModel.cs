using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Medical
{
    public class OrderSetPatientEducationModel
    {
        public string OrderSetId { get; set; }
        public string OrderSetPatEducationId { get; set; }
        public string DocId { get; set; }
        public string DocType { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string DocumentName { get; set; }
        public string FileStream { get; set; }
        public string Pages { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Comments { get; set; }
        public long PageNumber { get; set; }
        public long RowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string commandType { get; set; }
        public string NonInfoDoc { get; set; }
        public string InfoDoc { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }

    }

    public class OrderSetPatientEducationResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public string OrderSetPatEducationId { get; set; }
    }
}
