using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class RadOrderResult
    {
        public Int64 RadiologyOrderResultId { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public DateTime ObservationDate { get; set; }
        public string LOINCDescription { get; set; }
        public string Result { get; set; }
        public string UoM { get; set; }
        public string Flag { get; set; }
        public string Range { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }

    }
}
