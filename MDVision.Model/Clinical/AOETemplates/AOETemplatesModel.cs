using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml.Serialization;

namespace MDVision.Model.Clinical.AOETemplates
{
    public class AOETemplatesModel : IBaseModel
    {
        public string Name { get; set; }
        public string commandType { get; set; }
        public string AOETemplateId { get; set; }
        public string DataTemplateId { get; set; }
        public string SpecialtyIds { get; set; }
        public string ProviderIds { get; set; }
        public string LabName { get; set; }
        public string LabId { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string PhysicalExamTemplateEntity { get; set; }
        public string IsActive { get; set; }
        public string IsDefault { get; set; }
        public string LastUpdated { get; set; }
        public string EntityId { get; set; }
        public string SaveAsTemplateName { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedOn { get; set; }
        public string ProviderNames { get; set; }
        public string SpecialityNames { get; set; }
        public List<AOETemplateSystemModel> SystemObservationData { get; set; }
        public List<AOENotesObservationModel> NotesObservationList { get; set; }
        public string TemplatePreview { get; set; }
        public string ProviderId { get; set; }
        public void Map(IDataReader reader, List<string> columnsList)
        {
            AOETemplateId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AOETemplateId", columnsList));
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
        }
        public class AOETemplateSystemModel
        {
            public string TemplateId { get; set; }
            public string TemplateSysId { get; set; }
            public string PESystemId { get; set; }
            public string SystemName { get; set; }
            public string IsChecked { get; set; }
            public string IsModified { get; set; }
            public List<AOETemplateSystemObservationsModel> Systems { get; set; }
            public string ObservationId { get; set; }
            public string ObservationName { get; set; }
            public string IsSystemChecked { get; set; }
            public string SystemDescription { get; set; }
            public string IsSystemDeleted { get; set; }
            public string IsObservationDeleted { get; set; }
        }
        public class AOETemplateSystemObservationsModel
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
        [Serializable, XmlRoot("AOENotesObservationModel"), XmlType("AOENotesObservationModel")]
        public class AOENotesObservationModel
        {
            public string AOENotesObservationId { get; set; }
            public string AOETemplateSystemId { get; set; }
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
        public class AOENotesRadiologyObservationModel
        {
            public string AOENotesRadiologyObservationId { get; set; }
            public string AOETemplateSystemId { get; set; }
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

    }
}
