using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.LabResult
{
    public class LabOrderResultDetailModel
    {
        public LabOrderResultDetailModel()
        {
            ChildRows = new List<ChildResultDetailModel>();
            ResultSpecimens = new List<ResultSpecimen>();
        }
        
        public string LabOrderResultDetailId { get; set; } 
        public string LabOrderResultId { get; set; }
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
        public List<ResultSpecimen> ResultSpecimens { get; set; }
    }

    public class ChildResultDetailModel
    {
        public string LabOrderResultDetailId { get; set; }
        public string LabOrderResultId { get; set; }
        public string ObservationDate { get; set; }
        public string LOINC { get; set; }
        public string LOINCDescription { get; set; }
        public string Result { get; set; }
        public string UoM { get; set; }
        public string Flag { get; set; }
        public string Range { get; set; }
        public string ProviderId { get; set; }
        public string PatientId { get; set; }
        public string ObservationValue { get; set; }
        public string ObservationResultStatus { get; set; }
        public List<ChildResultDetailModel> ChildRows { get; set; }
        public string IsAttribute { get; set; }
        public string LabTestId { get; set; }
        public string LabTestAttributeId { get; set; }
        public string LabId { get; set; }
        public string IsElectronicResult { get; set; }

    }

    public class ResultSpecimen
    {
        public ResultSpecimen()
        {
            ResultSpecimenRejectReasons = new List<ResultSpecimenRejectReason>();
        }
        public string SpecimenType { get; set; }
        public string Text { get; set; }
        public string OriginalText { get; set; }
        public string NameOfCodingSystem { get; set; }
        public string Quantity { get; set; }
        public string CollectionDateTime { get; set; }
        public List<ResultSpecimenRejectReason> ResultSpecimenRejectReasons { get; set; }
    }

    public class ResultSpecimenRejectReason
    {        
        public string Identifer { get; set; }
        public string Text { get; set; }
        public string NameOfCodingSystem { get; set; }
        public string AlternateIdentifier { get; set; }
        public string AlternateText { get; set; }
        public string NameOfAletrnateCodingSystem { get; set; }
        public string OriginalText { get; set; }

      
    }
}