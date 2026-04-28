using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.CDS
{
    //Author Name: Humaira Yousaf
    //Created Date: 09-03-2016
    //Description: CDS Medication Model
    public class CDSMedicationModel
    {
        public long CDSMedicationId { get; set; }
        public string CDSId { get; set; }
        public string DrugId { get; set; }
        public string rxnormid { get; set; }
        public string Comments { get; set; }
        public string ModifiedOn { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string CDSMedicationQuery { get; set; }
        public string MedicationOperator { get; set; }
        public string DrugDescription { get; set; }
        public string MedicationCode { get; set; }


    }
}