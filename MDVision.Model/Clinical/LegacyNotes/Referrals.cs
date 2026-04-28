using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class Referrals
    {

        public Int64 ReferralId { get; set; }
        public string Value { get; set; }
        public string TYPE { get; set; }
        public DateTime RefDate { get; set; }
        public TimeSpan RefTime { get; set; }
        public string ReferralTo { get; set; }
        public string ReferralFrom { get; set; }
        public string FacilityFrom { get; set; }
        public string FacilityTo { get; set; }
        public string ToSpecialty { get; set; }
        public string CPTCodeDescription { get; set; }
        public string ProblemName { get; set; }
        public string Comments { get; set; }
        public long VisitTypeId { get; set; }
    }
}
