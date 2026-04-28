using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.HL7
{
    public class OBX_HL7
    {
        public string SetIDOBX { get; set; }
        public string ValueType { get; set; }
        public string observationIdentifier { get; set; }
        public string ObIdentifierSystem { get; set; }
        public string LoincCode { get; set; }
        public string LoincDesc { get; set; }
        public string obServationSubId { get; set; }
        public string ObservationValue { get; set; }
        public string Units { get; set; }
        public string ReferencesRange { get; set; }
        public string AbnormalFlags { get; set; }
        public string ObservationResultStatus { get; set; }
        public DateTime? DateTimeOfTheObservation { get; set; }
        public string  FillerInstructions { get; set; }


        public DateTime? EndDateTimeOfTheObservation { get; set; }
        public string Comments { get; set; }
    }
}