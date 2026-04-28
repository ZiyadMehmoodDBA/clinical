using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class NotesComponents
    {
        public Int64 NotesId { get; set; }
        public Int64 PatientId { get; set; }
        public string Component { get; set; }
        public Int64 ProviderId { get; set; }
    }

    public class NotesComponentViewModel
    {
        public NotesComponentViewModel()
        {
            NotesComponents = new List<NotesComponents>();
        }
        public List<NotesComponents> NotesComponents { get; set; }
        public List<string> ExcludedImages { get; set; }
        public bool IsSaveDiagnosticResult { get; set; }
        public string NotesPreviewStyle { get; set; }
    }
    public class NotesComponentDataFixModel
    {
        public string NotesIds { get; set; }
    }
}
