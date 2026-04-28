using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.RadiologyResult
{
    public class RadiologyOrderResultDetailModel
    {
        public RadiologyOrderResultDetailModel()
        {
            ChildRows = new List<ChildResultDetailModel>();
        }      
        public string RadiologyOrderResultDetailId { get; set; } 
        public string RadiologyOrderResultId { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }       
        public string Status { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }

        public string DateTime { get; set; }
        public string ObservationDate { get; set; }
        public List<ChildResultDetailModel> ChildRows { get; set; }
        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }
        public string Result { get; set; }
        public string Remarks { get; set; }
    }

    public class ChildResultDetailModel
    {
        public string RadiologyOrderResultDetailId { get; set; }  
        public string ObservationDate { get; set; }
        public string LOINC { get; set; }
        public string LOINCDescription { get; set; }
        public string Result { get; set; }
        public string UoM { get; set; }
        public string Flag { get; set; }
        public string Range { get; set; }
        public string ProviderId { get; set; }
        public string PatientId { get; set; }
        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }
        public string Remarks { get; set; }
    }
}