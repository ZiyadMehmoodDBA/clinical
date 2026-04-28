using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.LabOrder
{
    public class LabOrderResultLatestNoteModel
    {
        public long NoteId { get; set; }
        public string NoteStatus { get; set; }
        public long ProviderId { get; set; }
    }
}