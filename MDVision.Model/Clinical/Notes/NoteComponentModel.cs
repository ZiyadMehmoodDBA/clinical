using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Notes
{
    public class NoteComponentModel
    {
        public long NoteComponentId { get; set; }
        public long NotesId { get; set; }
        public string NotesIds { get; set; }
        public long? NoteComponentsLookupId { get; set; }
        public string SOAPText { get; set; }
        public int OrderNo { get; set; }
        public string commandType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ComponentName { get; set; }
        public string SectionName { get; set; }
        public long? NoteSectionsLookupId { get; set; }
        public string NoteComponentIds { get; set; }
        public List<NoteComponentModel> NoteComponentist { get; set; }
        public string UniqueId { get; set; }
        public string ComponentLookupIDs { get; set; }
        public bool IsFromTemplate { get; set; }
        public long VisitId { get; set; }
        public long PatientId { get; set; }
        public long ProviderId { get; set; }
        public long BillingInfoId { get; set; }
        public bool IsProblemMissed { get; set; }
        public bool IsNonBillable { get; set; }
        public long MUAlertsCount { get; set; }
        public bool IsProcedureMissed { get; set; }
        public bool IsNoteSignWOCPTCode { get; set; }
        public bool IsNoteSignWOICDCode { get; set; }
        public bool IsPhoneEncounter { get; set; }
        public string ErrorMessage { get; set; }
        public bool CDSAlerts { get; set; }
        public bool ICD { get; set; }
        public bool CPT { get; set; }
        public string isCopy { get; set; }
        public string customComponentName { get; set; }
        public bool IsNoteUpdate { get; set; }

    }
    public class NoteComponentResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }

    public class NotesPDFModel
    {
        public string NotePDF_FilePath { get; set; }
        public string NotePDF_FileName { get; set; }
        public string PatientId { get; set; }
        public string NotesId { get; set; }
    }
}
