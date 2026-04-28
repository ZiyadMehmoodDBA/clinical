using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.CDS
{
    //Author Name: Humaira Yousaf
    //Created Date: 08-03-2016
    //Description: CDS Vitals Model
    public class CDSVitalsModel
    {
        public long CDSVitalsId { get; set; }
        public string CDSId { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string BMI { get; set; }
        public string SystolicTemplate { get; set; }
        public string DiastolicTemplate { get; set; }
        public string VitalsQuery { get; set; }
        public string Comments { get; set; }
        public string ModifiedOn { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }


        //according to new revised change request
        public string VitalsLogic { get; set; }
        public string VitalType { get; set; }
        public string VitalLogicalOperator { get; set; }
        public string VitalValue { get; set; }
        public string VitalValueFrom { get; set; }
        public string VitalValueTo { get; set; }
        public string VitalUnit { get; set; }
    }
}