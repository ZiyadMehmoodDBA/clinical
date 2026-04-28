using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Notes
{
    public class NoteComponentAuditModel
    {
        public long NoteComponentAuditId { get; set; }
        public long NotesId { get; set; }
        public long? ComponentsLookupId { get; set; }
        public string OldSOAPText { get; set; }
        public string NewSOAPText { get; set; }
        public string DBAction { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string commandType { get; set; }
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
        public int RecordCount { get; set; }

        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }


        public string ComponentName { get; set; }

    }
}
