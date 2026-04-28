using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class Prescription
    {

        public Int64 PrescriptionID { get; set; }
        public string DrugDescription { get; set; }
        public string ProviderName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Refill { get; set; }
        public string Action { get; set; }
        public string Dose { get; set; }
        public string DoseUnit { get; set; }
        public string Routeby { get; set; }
        public string DoseTiming { get; set; }
        public int Duration { get; set; }
        public string Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public string Substitution { get; set; }

    }
}
