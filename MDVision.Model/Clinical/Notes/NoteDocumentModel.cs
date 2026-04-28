using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Notes
{
    public class NoteDocumentModel
    {
        public string PatDocId { get; set; }
        public string NoteDocumentId { get; set; }
        public string NoteId { get; set; }
        public string DocumentName { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public string IsActive { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string PageNumber { get; set; }
        public string RowspPage { get; set; }
        public string RecordCount { get; set; }
        public string Type { get; set; }
        public string FileType { get; set; }
        public string DocumentURL { get; set; }

    }
}
