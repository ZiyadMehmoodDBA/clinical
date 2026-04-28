using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Lookups
{
    public class NoteComponentsLookupModel
    {
        public string NoteComponentId { get; set; }
        public string NoteComponentName { get; set; }
    }
    public class NoteSectionsLookupModel
    {
        public long NoteSectionsLookupId { get; set; }
        public string SectionName { get; set; }
        public string SectionMarkup { get; set; }
    }
}
