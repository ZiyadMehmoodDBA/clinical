using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PhysicalExam
{
    public class PatientPhysicalExamSystemModel
    {
        public string PatientPhysicalExamId { get; set; }

        public string PhysicalExamDataTemplateSystemId { get; set; }
        public string PhysicalExamSystemId { get; set; }
        public bool IsNormal { get; set; }
        public bool isActive { get; set; }
        public string Comments { get; set; }
        public string SystemId { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
        public string commandType { get; set; }
        public string NormalExamsDetail { get; set; }
        //Start//19-02-2016//Ahmad Raza//Property added for Normal System comments
        public string NormalComments { get; set; }
        //End//19-02-2016//Ahmad Raza//Property added for Normal System comments
        public List<PatientPhysicalExamSystemSectionModel> Sections { get; set; }
    }
}