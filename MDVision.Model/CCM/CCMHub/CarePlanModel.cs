using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.CCM.PatientHub
{
    public class CarePlanSearchModel
    {
        public long PatientId { get; set; }
        public string IsActive { get; set; }
        public long CarePlanId { get; set; }
        public long EnrollmentInfoId { get; set; }
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
    }
    public class CarePlanFillModel
    {
        public long CarePlanId { get; set; }
        public string Createdby { get; set; }
        public string Description { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public long PatientId { get; set; }
        public bool IsActive { get; set; }
        public long RecordCount { get; set; }

        public string ModifiedByName { get; set; }
        public string CreatedByName { get; set; }
    }
    public class CarePlanModel
    {
        public long CarePlanId { get; set; }
        public long EnrollmentInfoId { get; set; }
        public string CarePlanHTML { get; set; }
        public long CarePlanTemplateId { get; set; }
    }
        public class CarePlanTemptModel
    {
        public long TemplateId { get; set; }
        public string TemplateName { get; set; }
    }
}
