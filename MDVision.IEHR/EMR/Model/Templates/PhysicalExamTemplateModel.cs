/* Author: Farooq Ahmad
 * Created Date: 07/03/2016
 * OverView: Created to Model Physical Exam Templates
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;


namespace MDVision.IEHR.EMR.Model.Templates
{
    public class PhysicalExamTemplateModel
    {
        public string PhysicalExamTemplateName { get; set; }
        public string commandType { get; set; }
        public string TemplateId { get; set; }
        public string DataTemplateId { get; set; }
        public string SpecialtyIds { get; set; }
        public string ProviderIds { get; set; }
        public string PhysicalExamTemplateEntity { get; set; }
        public string IsActive { get; set; }
        public string IsDefault { get; set; }
        public string LastUpdated { get; set; }

        public string EntityId { get; set; }
        public string NotesId { get; set; }

        public string SaveAsTemplateName { get; set; }

        public string ModifiedBy { get; set; }
        public List<PhysExamSysTemplateModel> SysSecCharSubcharData { get; set; }
        public List<PhysExamNotesObservationModelECW> NotesObservationList { get; set; }
        public string ProviderId { get; set; }
    }
    public class PhysicalExamTemplateSystemsModel
    {
        public string PETemplateSystemId { get; set; }
        public string PETemplateId { get; set; }
        public string PESystemId { get; set; }
        public string NotesId { get; set; }
        public string TemplateName { get; set; }
        public string SystemName { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public List<PhysExamSysTemplateModel> SysSecCharSubcharData { get; set; }
    }
    public class PhysicalExamTemplateModelECW
    {
        public string PhysicalExamTemplateName { get; set; }
        public string commandType { get; set; }
        public string TemplateId { get; set; }
        public string DataTemplateId { get; set; }
        public string SpecialtyIds { get; set; }
        public string ProviderIds { get; set; }
        public string PhysicalExamTemplateEntity { get; set; }
        public string BodyPartId { get; set; }
        public string IsActive { get; set; }
        public string IsDefault { get; set; }
        public string LastUpdated { get; set; }
        public string EntityId { get; set; }
        public string SaveAsTemplateName { get; set; }
        public string ModifiedBy { get; set; }
        public List<PhysExamTemplateSystemModelECW> SystemObservationData { get; set; }

        public string TemplatePreview { get; set; }
        public string ProviderId { get; set; }
    }

    public class PhysExamTemplateSystemModelECW
    {
        public string TemplateId { get; set; }
        public string TemplateSysId { get; set; }
        public string PESystemId { get; set; }
        public string SystemName { get; set; }
        public string IsChecked { get; set; }
        public string IsModified { get; set; }
        public List<PhysExamTemplateSystemObservationsModelECW> Systems { get; set; }
        public string ObservationId { get; set; }
        public string ObservationName { get; set; }
        public string IsSystemChecked { get; set; }
        public string SystemDescription { get; set; }
        public string IsSystemDeleted { get; set; }
        public string IsObservationDeleted { get; set; }
        public string ObservationOrder { get; set; }
    }

    public class PhysExamTemplateSystemObservationsModelECW
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
    [Serializable, XmlRoot("PhysExamNotesObservationModelECW"), XmlType("PhysExamNotesObservationModelECW")]
    public class PhysExamNotesObservationModelECW
    {
        public string PENotesObservationId { get; set; }
        public string PETemplateSystemId { get; set; }
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