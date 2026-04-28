using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Templates
{
    public class ProviderNoteTemplateModel
    {
        public long NotesTemplateId { get; set; }
        public string NoteTemplateName { get; set; }
        public string Description { get; set; }
        public int TemplateTypeId { get; set; }
        public long ROSDataTemptId { get; set; }
        public long PEDataTemptId { get; set; }
        public long HPITemplateId { get; set; }
        public string commandType { get; set; }
        public string HTMLTemplate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
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

        public string NoteTypeText { get; set; }

        public string TemplateType { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public string OrderSetId { get; set; }
    }
}