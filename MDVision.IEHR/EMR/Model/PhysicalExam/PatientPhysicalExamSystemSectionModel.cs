using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PhysicalExam
{
    public class PatientPhysicalExamSystemSectionModel
    {
        public string PatientPhysicalExamSystemSectionId { get; set; }
        public string PatientPhysicalExamSystemId { get; set; }

        public string PhysicalExamDataTemplateSectionId { get; set; }
        public string PhysicalExamSectionId { get; set; }

        //public bool isActive { get; set; }

        private bool? _isActive;
        public bool isActive
        {
            get
            {
                if (_isActive == null)
                    return true;
                else
                    return _isActive.Value;
            }
            set { _isActive = value; }
        }

        public string Comments { get; set; }
        public string SectionId { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
        public string commandType { get; set; }
        public List<PatientPhysicalExamCharacteristicModel> Characteristics { get; set; }

        public PatientPhysicalExamSystemSectionModel()
        {
            Characteristics = new List<PatientPhysicalExamCharacteristicModel>();
        }

    }
}