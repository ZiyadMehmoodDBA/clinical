using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Reports
{
    public class PhoneEncounterSearchModel
    {
        public string commandType { get; set; }
        public string ProviderIds { get; set; }
        public string FacilityIds { get; set; }
        public string PracticeIds { get; set; }
        public string RefProviderIds { get; set; }
        public long NoteType { get; set; }
        public string NotesDuration { get; set; }
        public string DurationFrom { get; set; }
        public string DurationTo { get; set; }
        public string NoteStatus { get; set; }
        public string CreateDateFrom { get; set; }
        public string CreateDateTo { get; set; }
        public bool IsAmendedNote { get; set; }
    }
}