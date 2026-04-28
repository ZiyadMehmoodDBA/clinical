using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model
{
    public class CustomFormModel
    {

        public string CustomFormId { get; set; }
        public string FavoriteListCustomFormId { get; set; }
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
        public string AttachCatIds { get; set; }
        public string CanvasCols { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedOn { get; set; }
        public string CustomFormHTML { get; set; }
        public string NoteId { get; set; }
        public string CustomFormDocName { get; set; }
        public string ProblemListIds { get; set; }
        public string ProcedureIds { get; set; }       
        public string FavoriteListId { get; set; }
        public string ProviderId { get; set; }
        public string IsFromNotes { get; set; }
    }

    public class CustomFormResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public string CustomFormId { get; set; }
    }
}