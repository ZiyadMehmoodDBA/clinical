using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class MedicationHx
    {
        public Int64 MedicationID { get; set; }
        public string BrandName { get; set; }
        public string GenericName { get; set; }
        public string Strength { get; set; }
        public string Form { get; set; }
        public string Action { get; set; }
        public string Dose { get; set; }
        public string DoseUnit { get; set; }
        public string Routeby { get; set; }
        public string DoseTiming { get; set; }
        public string DoseOther { get; set; }
        public string Duration { get; set; }
        public string Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public string Refill { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StopDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }
}
