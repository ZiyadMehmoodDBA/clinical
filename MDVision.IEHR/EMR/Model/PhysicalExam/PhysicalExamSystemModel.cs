using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PhysicalExam
{
    public class PhysicalExamSystemModel
    {
        public int SystemId { get; set; }
        public int SystemOrder { get; set; }

        public string ShortName { get; set; }

        public string SortOrder { get; set; }

        public string SystemCustomSorted { get; set; }

        public string commandType { get; set; }
        public long PatientPhysicalExamId { get; set; }

        public long TemplateId { get; set; }

        public string isNormal { get; set; }

        public string Comments { get; set; }

        public List<PatientPhysicalExamSystemSectionModel> Sections { get; set; }
        public PhysicalExamSystemModel()
        {
            Sections = new List<PatientPhysicalExamSystemSectionModel>();
        }

    }
}