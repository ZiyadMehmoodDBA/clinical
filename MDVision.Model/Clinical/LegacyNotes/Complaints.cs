using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class Complaints
    {
        public Int64 ComplaintId { get; set; }
        public string ComplaintDescription { get; set; }
        public long ComplaintDetailId { get; set; }
        public string PreviousHistory { get; set; }
        public string Case { get; set; }
        public string Location { get; set; }
        public string Radiation { get; set; }
        public string Quality { get; set; }
        public string Severity { get; set; }
        public string Onset { get; set; }
        public string Duration { get; set; }
        public string DurationDesc { get; set; }
        public string Frequency { get; set; }
        public string Context { get; set; }
        public string Character { get; set; }
        public string AssociatedWith { get; set; }
        public string PrecipitatedBy { get; set; }
        public string AggravatedBy { get; set; }
        public string RelievedBy { get; set; }
        public string Comments { get; set; }
        public string OverallComments { get; set; }

        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Age { get; set; }
    }

}
