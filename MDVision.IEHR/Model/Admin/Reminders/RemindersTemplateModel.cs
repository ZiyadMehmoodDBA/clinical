using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Admin.Reminders
{
    public class RemindersTemplateModel
    {
        public string RemindersTemplateId { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string NoteTypeText { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateTypeId { get; set; }
        public string EntityId { get; set; }
        public string commandType { get; set; }
        public string HTMLTemplate { get; set; }
        public string ProviderIds { get; set; }
        public string IsActive { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string ReminderTemplateType { get; set; }
        public string HTMLTemplateWithIds { get; set; }

    }
}