using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.HL7
{
    public class OBR_HL7
    {
        public OBR_HL7()
        {
            OBXmodalList = new List<OBX_HL7>();
            SPMmodalList = new List<SPM_HL7>();
        }
        public string OBRSetId { get; set; }
        public string PlacerOrderNumberLabTestId { get; set; }
        public string FillerOrderNumber { get; set; }
        public string USIIdentifier { get; set; }
        public string USIText { get; set; }
        public string USICodingSystem { get; set; }
        public DateTime ObserVationDateTime { get; set; }
        public DateTime ObserVationEndDateTime { get; set; }
        public string SpecimenActionCode { get; set; }
        public string SpecimenSource { get; set; }
        public string OBROrderingProvider { get; set; }
        public string FillerField1 { get; set; }
        public string FillerField2 { get; set; }
        public DateTime ResultsRptStatusChngDateTime { get; set; }
        public string ResultStatus { get; set; }
        public int OBRcount { get; set; }
        public List<OBX_HL7> OBXmodalList { get; set; }

        public List<SPM_HL7> SPMmodalList { get; set; }
        public string OrderId { get; set; }
        public string Comments { get; set; }
    }
    public class SPM_HL7
    {
        public SPM_HL7()
        {
            RejectReasonList = new List<SPM_RejectReason_HL7>();
        }
        public int LabResultSpecimenId { get; set; }
        public string SpecimenType { get; set; }
        public string Text { get; set; }
        public string OriginalText { get; set; }
        public string NameofCodingSystem { get; set; }
        public string Quantity { get; set; }
        public DateTime CollectionDateTime { get; set; }
        public int SpecimenId { get; set; }
        public Int64 LabOrderId { get; set; }
        public List<SPM_RejectReason_HL7> RejectReasonList { get; set; }
        //SPECIMEN CONDITION PROPERTIES
        public string Identifier { get; set; }
        public string ConditionText { get; set; }
        public string ConditionNOCSystem { get; set; }
        public string AlternateIdentifier { get; set; }
        public string AlternateText { get; set; }
        public string NameofAlternateCodingSystem { get; set; }
        public string ConditionOriginalText { get; set; }
    }
    public class SPM_RejectReason_HL7
    {
        public string Identifier { get; set; }
        public string Text { get; set; }
        public string NameofCodingSystem { get; set; }
        public string AlternateIdentifier { get; set; }
        public string AlternateText { get; set; }
        public string NameofAlternateCodingSystem { get; set; }
        public string OriginalText { get; set; }
    }
}