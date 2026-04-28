using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Notes
{
    public class ClinicalNotesInfo
    {
        public string NoteStatus { get; set; }
        public long NoteId { get; set; }
        public string VisitId { get; set; }
    }
}
