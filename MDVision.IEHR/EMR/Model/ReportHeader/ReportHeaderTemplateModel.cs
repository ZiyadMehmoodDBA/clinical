using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.ReportHeader
{
    public class ReportHeaderTemplateModel
    {
    }
    public class ReportHeaderModel
    {
        public long ReportHeaderId { get; set; }
        public string ReportHeaderName { get; set; }
        public string commandType { get; set; }
        public string HTMLTemplate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public long RecordCount { get; set; }
        public int? IsActive { get; set; }
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }

        public int? EntityId { get; set; }
        public string ProviderNames { get; set; }
        public string ProviderIds { get; set; }
        public string SpecialtyNames { get; set; }
        public string SpecialtyIds { get; set; }
        public string NotesTagNames { get; set; }
        public string NotesTagNameIds { get; set; }



    }
    public class ReportHeader_searchModel
    {
        public long ReportHeaderId { get; set; }
        public string ReportHeaderName { get; set; }
        public string ProviderIds { get; set; }
        public string SpecialtyIds { get; set; }
        public string FacilityIds { get; set; }
        public string commandType { get; set; }
        public string IsActive { get; set; }
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
        public long RptHeaderSettingId { get; set; }
        public long RptHeaderConfigurationId { get; set; }
        public long ProviderId { get; set; }
        public long PatientId { get; set; }
        public string FormName { get;  set; }
    }

    public class ReportHeader_selectModel
    {
        public long ReportHeaderId { get; set; }
        public string ReportHeaderName { get; set; }
        public string ProviderNames { get; set; }
        public string SpecialtyNames { get; set; }
        public string FacilityNames { get; set; }
        public string LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }

    public class ReportHeader_FillModel
    {
        public long ReportHeaderId { get; set; }
        public string ReportHeaderName { get; set; }
        public string SpecialtyIds { get; set; }
        public string ProviderIds { get; set; }
        public string FacilityIds { get; set; }
        public string EntityId { get; set; }

        public string FooterText { get; set; }
        public bool IsActive { get; set; }
        public string PracticeTags { get; set; }

        public string PatientTags { get; set; }
        public string ProviderTags { get; set; }
        public string HeaderLogo { get; set; }
        public string HeaderLogoName { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }


        public string HeaderLogoUpldDate { get; set; }

        public string EntityIds { get; set; }
    }

    public class ReportHeader_TagsModel
    {
        public string PracticeText { get; set; }
        public string PatientText { get; set; }
        public string ProviderText { get; set; }
        public string HeaderLogo { get; set; }
        public string FooterText { get; set; }
        public string PatientName { get; set; }
        public DateTime PatientDOB { get; set; }
        public string ProviderName { get; set; }
        public DateTime DOS { get; set; }
    }

    public class ReportHeader_TagsSelectModel
    {
        public string Header { get; set; }
        public string Footer { get; set; }
        public int reportHeaderCount { get; set; }
        public string PatientName { get; set; }
        public DateTime PatientDOB { get; set; }
        public string ProviderName { get; set; }
        public DateTime DOS { get; set; }
    }
}