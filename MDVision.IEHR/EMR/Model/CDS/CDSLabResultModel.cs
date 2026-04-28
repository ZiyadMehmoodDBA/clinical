using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.CDS
{
    public class CDSLabResultModel
    {
        public string CDSLabResultId { get; set; }
        public string CDSId { get; set; }
        public string LabResultName { get; set; }
        public string Comments { get; set; }
        public string ModifiedOn { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string CDSLabResultQuery { get; set; }
        public string LabResultOperator { get; set; }
        public string LabResultLogicalOperator { get; set; }
        public string LabResultValue { get; set; }
        public string LabResultValueFrom { get; set; }
        public string LabResultValueTo { get; set; }

        public string LabId { get; set; }
        public string LabName { get; set; }

        public string TestId { get; set; }
        public string TestName { get; set; }
        public string AttributeId { get; set; }

    }
}