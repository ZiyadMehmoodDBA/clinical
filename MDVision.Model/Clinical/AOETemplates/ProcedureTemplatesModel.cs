using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml.Serialization;

namespace MDVision.Model.Clinical.AOETemplates
{
    public class ProcedureTemplatesModel : IBaseModel
    {
        public ProcedureTemplatesModel()
        {
            this.procedures = new List<Procedures>();
        }
        public string Name { get; set; }
        public string commandType { get; set; }
        public string ProcedureTemplateId { get; set; }
        public string DataTemplateId { get; set; }
        public string SpecialtyIds { get; set; }
        public string ProcedureId { get; set; }
        public string ProviderIds { get; set; }
        public string LabName { get; set; }
        public string LabId { get; set; }
        public string NotesId { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string PhysicalExamTemplateEntity { get; set; }
        public string IsActive { get; set; }
        public string IsDefault { get; set; }
        public string AssociatedWithIds { get; set; }
        public string LastUpdated { get; set; }
        public string EntityId { get; set; }
        public string SaveAsTemplateName { get; set; }
        public string ModifiedBy { get; set; }
        public string NoteViewTypeId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedOn { get; set; }
        public string ProviderNames { get; set; }
        public string TempProcedures { get; set; }
        public string SpecialityNames { get; set; }
        public List<ProcedureTemplatesystemModel> SystemObservationData { get; set; }
        public List<ProcedureNotesObservationModel> NotesObservationList { get; set; }
        public List<Procedures> procedures { get; set; }
        public string TemplatePreview { get; set; }
        public string ProviderId { get; set; }
        public void Map(IDataReader reader, List<string> columnsList)
        {
            ProcedureTemplateId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProcedureTemplateId", columnsList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", columnsList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", columnsList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", columnsList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", columnsList));
            LastUpdated = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LastUpdated", columnsList));
            ProviderIds = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProviderIds", columnsList));
            SpecialityNames = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SpecialityNames", columnsList));
            ProviderNames = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProviderNames", columnsList));
            LabName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LabName", columnsList));
            Name = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Name", columnsList));
            CPTCode = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CPTCode", columnsList));
            CPTCodeDescription = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CPTCodeDescription", columnsList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", columnsList));
            TempProcedures = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Procedures", columnsList));
        }
        public class ProcedureTemplatesystemModel
        {
            public string TemplateId { get; set; }
            public string TemplateSysId { get; set; }
            public string PESystemId { get; set; }
            public string SystemName { get; set; }
            public string IsChecked { get; set; }
            public string IsModified { get; set; }
            public List<ProcedureTemplatesystemObservationsModel> Systems { get; set; }
            public string ObservationId { get; set; }
            public string ObservationName { get; set; }
            public string IsSystemChecked { get; set; }
            public string SystemDescription { get; set; }
            public string IsSystemDeleted { get; set; }
            public string IsObservationDeleted { get; set; }
        }
        public class ProcedureTemplatesystemObservationsModel
        {
            public string TemplateId { get; set; }
            public string TemplateSectionId { get; set; }
            public string PESystemId { get; set; }
            public string ObservationId { get; set; }
            public string ObservationName { get; set; }
            public string IsChecked { get; set; }
            public string IsModified { get; set; }
            public string PETemplateSystemId { get; set; }
        }
        [Serializable, XmlRoot("ProcedureNotesObservationModel"), XmlType("ProcedureNotesObservationModel")]
        public class ProcedureNotesObservationModel
        {
            public string ProcedureNotesObservationId { get; set; }
            public string ProcedureTemplatesystemId { get; set; }
            public string Descr { get; set; }
            public string NotesId { get; set; }
            [XmlIgnore]
            public string IsActive { get; set; }
            [XmlIgnore]
            public string CreatedBy { get; set; }
            [XmlIgnore]
            public string CreatedOn { get; set; }
            [XmlIgnore]
            public string ModifiedBy { get; set; }
            [XmlIgnore]
            public string ModifiedOn { get; set; }
            public string ProcedureId { get; set; }
            public string Action { get; set; }
        }
        public class ProcedureOrderNotesObservationModel
        {
            public string ProcedureOrderNotesObservationId { get; set; }
            public string ProcedureTemplatesystemId { get; set; }
            public string Descr { get; set; }
            public string NotesId { get; set; }
            [XmlIgnore]
            public string IsActive { get; set; }
            [XmlIgnore]
            public string CreatedBy { get; set; }
            [XmlIgnore]
            public string CreatedOn { get; set; }
            [XmlIgnore]
            public string ModifiedBy { get; set; }
            public string ProcedureOrderId { get; set; }
            [XmlIgnore]
            public string ModifiedOn { get; set; }
            public string Action { get; set; }
        }
        public class ProcedureNotesRadiologyObservationModel
        {
            public string ProcedureNotesRadiologyObservationId { get; set; }
            public string ProcedureTemplatesystemId { get; set; }
            public string Descr { get; set; }
            public string NotesId { get; set; }
            [XmlIgnore]
            public string IsActive { get; set; }
            [XmlIgnore]
            public string CreatedBy { get; set; }
            [XmlIgnore]
            public string CreatedOn { get; set; }
            [XmlIgnore]
            public string ModifiedBy { get; set; }
            [XmlIgnore]
            public string ModifiedOn { get; set; }
            public string Action { get; set; }
        }
        public class Procedures
        {
            public long ProcedureTemplateTestsId { get; set; }
            public string CPTCode { get; set; }
            public string CPTCodeDescription { get; set; }
            public bool IsActive { get; set; }
            
        }

    }
}
