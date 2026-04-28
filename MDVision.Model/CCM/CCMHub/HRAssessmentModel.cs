using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.CCM.CCMHub
{
    public class HRAssessmentModel
    {
        public long HRAssessmentId { get; set; }
        public long EnrollmentInfoId { get; set; }
        public string HRAssessmentHTML { get; set; }
        public long HRAssessmentTemplateId { get; set; }
        public long VitalsId { get; set; }
    }
    public class HRAssessmentTemptModel
    {
        public long TemplateId { get; set; }
        public string TemplateName { get; set; }
    }
    public class HRAssessmentSearchModel {
        public long HRAssessmentId { get; set; }
        public long PatientId { get; set; }
        public string IsActive { get; set; }
        public long EnrollmentInfoId { get; set; }
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
    }
    public class HRAssessmentFillModel {
        public long HRAssessmentId { get; set; }
        public string Createdby { get; set; }
        public string Description { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public long PatientId { get; set; }
        public bool IsActive { get; set; }
        public long RecordCount { get; set; }
        public string RiskScore { get; set; }

        public string ModifiedByName { get; set; }
        public string CreatedByName { get; set; }
    }
}
