using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Lab
{
    public class ClinicalLabTestModel
    {
        public string LabId { get; set; }
        public string LabTestId { get; set; }
        public string LabTestAttributeId { get; set; }
        public string LOINICCODE { get; set; }
        public string LOINICDescription { get; set; }
        public string OBSERVATION { get; set; }
        public string Template { get; set; }
        public string Attribute { get; set; }
        public string UoM { get; set; }
        public string Range { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string commandType { get; set; }
        public string IsActive { get; set; }
        public string isUOM { get; set; }
        public string LabTestAttributeResultId { get; set; }
        public string ResultName { get; set; }
    }
}