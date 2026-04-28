using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class HospitalizationHx
    {
        public Int64 HospitalizationHxId { get; set; }
        public string Hospital { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string CPTDescription { get; set; }
        public string FreeTextICD { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public bool bUnremarkable { get; set; }


    }
}
