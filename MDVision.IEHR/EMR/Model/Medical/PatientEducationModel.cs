using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class PatientEducationModel
    {
        public string PatientEducationId { get; set; }
        public string commandType { get; set; }
        public string Comments { get; set; }
        public string PatientId { get; set; }
        public string iTotalDisplayRecords { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string Pages { get; set; }
        public string DocumentId { get; set; }
        public string FileType { get; set; }
        public byte[] FileStream { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string DocType { get; set; }
        public string DocumentName { get; set; }
        public string NoteId { get; set; }
        public string IsNonInfo { get; set; }
        

    }
}