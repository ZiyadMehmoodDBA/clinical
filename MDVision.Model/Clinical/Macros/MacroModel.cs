using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml.Serialization;

namespace MDVision.Model.Clinical.Macros
{
    public class MacroModel
    {
        public long MacroId { get; set; }
        public string MacroName { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
        public string IsIndependent { get; set; }
        public string NoteComponentsNames { get; set; }
        public string NoteComponentIds { get; set; }
        public string UserNames { get; set; }
        public string UsersIds { get; set; }
        public string commandType { get; set; }
        public string ErrorMessage { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public long UserId { get; set; }
        public long NoteComponentId { get; set; }
        public string IdsToDelete { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
