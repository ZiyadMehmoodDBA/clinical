using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class ImmunizationHx
    {
        public string Type { get; set; }
        public Int64 VaccineHxId { get; set; }
        public string CVXShortDescription { get; set; }
        public string CPTCode { get; set; }
        public float Dose { get; set; }
        public string Amount { get; set; }
        public string ProviderName { get; set; }
        public DateTime AdministrationDate { get; set; }
        public Int64 ImmTherInjectionId { get; set; }
        public string TherapeuticInjection { get; set; }
        public string RouteDescription { get; set; }
        public string SiteDescription { get; set; }
        public string LotNumber { get; set; }
        public string ManufacturerName { get; set; }

    }
}
