using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Templates
{
    public class HPITemplateModel
    {
        public string HPITemplateId { get; set; }
        public string Name { get; set; }
        public string ProviderId { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string EntityId { get; set; }
        public string TemplatePreview { get; set; }
        public string commandType { get; set; }
        public string RecordCount { get; set; }
        public string SpecialtyIds { get; set; }
        public string ProviderIds { get; set; }
        public string SaveAsTemplateName { get; set; }
        public List<HPITemplateSymptomsModel> SymptomFindingData { get; set; }
        public string ProviderXML { get; set; }
        public string SpecialtyXML { get; set; }
        public string SymptomFindingXML { get; set; }

        public List<HPITemplateSymptomsModel> SymptomData { get; set; }
        public List<HPITemplateSymptomFindingsModel> FindingsData { get; set; }
        public List<HPINotesFindings> NotesFindingsData { get; set; }
        public string Comments { get; set; }
        public string NotesId { get; set; }
    }

    public class HPITemplateSymptomsModel
    {
        public string HPITemplateId { get; set; }
        public string HPITemplateSymptomId { get; set; }
        public string HPISymptomId { get; set; }
        public string SymptomName { get; set; }
        public string IsChecked { get; set; }
        public string IsModified { get; set; }
        public List<HPITemplateSymptomFindingsModel> SymptomsFindings { get; set; }
        public string FindingId { get; set; }
        public string FindingName { get; set; }
        public string IsSymptomChecked { get; set; }
        public string SymptomDescription { get; set; }
        public string IsSymptomDeleted { get; set; }
        public string IsFindingDeleted { get; set; }
        public string TemplateName { get; set; }
        public string IsSelectedSymptom { get; set; }
        public string HPISymptomsAnswersId { get; set; }
        public string HPISymptoms_LocationIds { get; set; }
        public string HPISymptoms_RadiationId { get; set; }
        public string HPISymptoms_QualityId { get; set; }
        public string HPISymptoms_SeverityId { get; set; }
        public string Onset { get; set; }
        public string AssociatedWith { get; set; }
        public string HPISymptoms_AggravatedById { get; set; }
        public string HPISymptoms_RelievedById { get; set; }
        public bool IsSymptomsDetail { get; set; }
        public string NotesId { get; set; }
        public string IsGlobal { get; set; }
        public string IsSelected { get; set; }
        public string ProviderId { get; set; }
        public string IsActive { get; set; }
        public string Comments { get; set; }
        public string SymptomOrder { get; set; }
        public string FindingOrder { get; set; }
    }
    public class HPITemplateSymptomFindingsModel
    {
        public string HPITemplateId { get; set; }
        public string TemplateSymptomId { get; set; }
        public string HPISymptomId { get; set; }
        public string HPIFindingId { get; set; }
        public string FindingName { get; set; }
        public string IsChecked { get; set; }
        public string IsModified { get; set; }
        public string HPITemplateSymptomId { get; set; }
        public string SymptomName { get; set; }
        public string IsActive { get; set; }
        public string IsSelected { get; set; }
        public string HPISymptoms_LocationIds { get; set; }
        public string HPISymptoms_RadiationId { get; set; }
        public string HPISymptoms_QualityId { get; set; }
        public string HPISymptoms_SeverityId { get; set; }
        public string Onset { get; set; }
        public string AssociatedWith { get; set; }
        public string HPISymptoms_AggravatedById { get; set; }
        public string HPISymptoms_RelievedById { get; set; }
        public string HPISymptomsDetailId { get; set; }
        public string HPITemplateSympFindingId { get; set; }
        public bool IsSymptomsDetail { get; set; }
        public string HPISymptomFindingsId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string IsFindingSelected { get; set; }
        public string TemplatePreview { get; set; }
        public string FindingOrder { get; set; }

    }
    public class HPINotesFindings
    {
        public string HPINotesFindingsId { get; set; }
        public string HPITemplateSymptomId { get; set; }
        public string Desc { get; set; }
        public string NotesId { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string TemplateName { get; set; }
        public string SymptomName { get; set; }
        public string HPISymptomId { get; set; }
        public string HPITemplateId { get; set; }
        public string IsSelected { get; set; }
        public string Action { get; set; }
        public string SymptomDescription { get; set; }
        public string IsHPIComplaint { get; set; }
        public string HPISymptomsAnswersId { get; set; }
        public string Comments { get; set; }
        public string SymptomOrder { get; set; }
    }
    public class HPISymptomsLookupModel
    {
        public string HPISymptomId { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public string IsGlobal { get; set; }
    }
    public class HPIResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public string HPITemplateId { get; set; }
    }

    public class ComplaintsHPI
    {
        public bool ComplaintId { get; set; }
        public string NotesId { get; set; }
        public string HPITemplateName { get; set; }
        public string HPISymptomName { get; set; }
        public string Description { get; set; }
        public string OverallComments { get; set; }
        public string Comments { get; set; }
    }
}
