using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class VitalSigns
    {
        public Int64 VitalSignId { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public DateTime VitalSignDate { get; set; }
        public string VitalSignTime { get; set; }
        public Int16 Systolic { get; set; }
        public Int16 Diastolic { get; set; }
        public string Position { get; set; }
        public string CuffLocation { get; set; }
        public string CuffSize { get; set; }
        public string Pulse { get; set; }
        public string Rhythm { get; set; }
        public TimeSpan? TempTime { get; set; }
        public string Temprature { get; set; }
        public string Method { get; set; }
        public string Rate { get; set; }
        public string Pattern { get; set; }
        public float Weight { get; set; }
        public string Height { get; set; }
        public float BSA { get; set; }
        public float BMI { get; set; }
        public float HeadCr { get; set; }
        public string SPO2 { get; set; }
        public string OxygenSource { get; set; }
        public string PeakFlow { get; set; }
        public string BloodType { get; set; }
        public string Comments { get; set; }

    }
}
