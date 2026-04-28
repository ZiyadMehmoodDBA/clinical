using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.LabOrder
{
    public class LabOrderTestModel
    {
        public LabOrderTestModel()
        {
            LabOrderResultDetails = new List<LabOrderResultDetailModel>();
        }
        public string LabOrderTestId { get; set; }
        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public string LabOrderId { get; set; }
        public string dtpLabDate { get; set; }
        public string tpLabTime { get; set; }
        public string LabProcedure { get; set; }
        public string CollectedAt { get; set; }
        public string ClinicalLabOrderDiet { get; set; }
        public string Urgency { get; set; }
        public string Specimen { get; set; }

        public string AlternativeSpecimen { get; set; }
        public string PatientInstructions { get; set; }
        public string VolumeText { get; set; }
        public string VolumeDDL { get; set; }
        public string FillerInstructions { get; set; }
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public string LabTestIds { get; set; }
        //Start 23/3/2016 Farooq Ahmad	field to Contain the text of the dropdown lists
        public string Urgency_text { get; set; }
        public string Specimen_text { get; set; }
        public string SampleStorage { get; set; }
        //End 23/3/2016 Farooq Ahmad	field to Contain the text of the dropdown lists
        public string IsABN { get; set; }
        public string AOEs { get; set; }
        public string FavoriteGroupTestLabId { get; set; }
        public List<LabOrderResultDetailModel> LabOrderResultDetails { get; set; }
    }
}