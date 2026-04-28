using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Model.Admin.Provider;

namespace MDVision.Model
{
    public class PatientCustomFormModel
    {
        public PatientCustomFormModel()
        {
            this.ProviderCPTsList = new List<ProviderCPTs>();
        }
        public string PatientCustomFormId { get; set; }
        public string CustomFormId { get; set; }
        public string FormName { get; set; }
        public string commandType { get; set; }
        public bool IsProviderAll { get; set; }
        public bool IsSpecialtyAll { get; set; }
        public string ProviderNames { get; set; }
        public string ProviderIds { get; set; }
        public string SpecialtyNames { get; set; }
        public string SpecialtyIds { get; set; }
        public string RecordCount { get; set; }
        public string IsActive { get; set; }
        public long PageNumber { get; set; }
        public long RowsPerPage { get; set; }
        public int? EntityId { get; set; }
        public string FormHeading { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedOn { get; set; }
        public string CustomFormHTML { get; set; }
        public string PatientId { get; set; }
        public string CustomFormBase64 { get; set; }
        public string Url { get; set; }
        public string IsSigned { get; set; }
        public string PatDocID { get; set; }
        public string ProviderId { get; set; }
        public List<ProviderCPTs> ProviderCPTsList { get; set; }
    }
}
