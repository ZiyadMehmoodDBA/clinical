using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class NotesComponent
    {

        public Int64 NoteComponentsLookupId { get; set; }
        public string SOAPText { get; set; }
        public int OrderNo { get; set; }
        public string ComponentName { get; set; }

    }
}
