using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.RadiologyOrder
{
    public class RadiologyOrderTestModel
    {
        public string RadiologyOrderTestId { get; set; }
        public string RadiologyOrderId { get; set; }

        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public string dtpRadiologyDate { get; set; }
        public string tpRadiologyTime { get; set; }
        public string RadiologyProcedure { get; set; }
        public string CollectedAt { get; set; }
        public string Urgency { get; set; }
        public string Specimen { get; set; }
        public string PatientInstructions { get; set; }
        public string VolumeText { get; set; }
        public string VolumeDDL { get; set; }
        public string FillerInstructions { get; set; }
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public string RadiologyTestIds { get; set; }
		//Start 23/3/2016 Farooq Ahmad	field to Contain the text of the dropdown lists
        public string Urgency_text { get; set; }
        public string Specimen_text { get; set; }
		//End 23/3/2016 Farooq Ahmad	field to Contain the text of the dropdown lists

        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }

        public string Reason { get; set; }
        public string Reason_text { get; set; }
        public string BodySite { get; set; }
    }
}