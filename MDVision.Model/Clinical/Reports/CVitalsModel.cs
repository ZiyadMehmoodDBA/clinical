using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
    public class CVitalsModel
    {
        public bool IncludeInactivePatient { get; set; }
        public string AccountNumber { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string SystolicFrom { get; set; }
        public string SystolicTo { get; set; }
        public string TemperatureFrom { get; set; }
        public string TemperatureTo { get; set; }
        public string HeightFrom { get; set; }
        public string HeightTo { get; set; }
        public string SPO2From { get; set; }
        public string SPO2To { get; set; }
        public string DiastolicFrom { get; set; }
        public string DiastolicTo { get; set; }
        public string RespirationResultFrom { get; set; }
        public string RespirationResultTo { get; set; }
        public string BMIFrom { get; set; }
        public string BMITo { get; set; }
        public string PulseRateFrom { get; set; }
        public string PulseRateTo { get; set; }
        public string WeightFrom { get; set; }
        public string WeightTo { get; set; }
        public string BSAFrom { get; set; }
        public string BSATo { get; set; }

        public string DOSTo { get; set; }
        public string DOSFrom { get; set; }
        public bool AdvancedSearch { get; set; }
    }

    public class CVitalsFillModel
    {

        public string AccountNumber { get; set; }

        public string PatientName { get; set; }

        public string DOB { get; set; }

        public string PatStatus { get; set; }

        public string Systolic { get; set; }

        public string Diastolic { get; set; }

        public string Pulse { get; set; }

        public string Temprature { get; set; }

        public string Respiration { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string BSA { get; set; }
        public string BMI { get; set; }
        public string SPO2 { get; set; }
        public string DOS { get; set; }

        public long PatientId { get; set; }
    }
}
