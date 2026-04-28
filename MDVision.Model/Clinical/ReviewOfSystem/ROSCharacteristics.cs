using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MDVision.Model.Clinical.ReviewOfSystem
{
    public class ROSCharacteristics
    {
        public string commandType { get; set; }
        public string ROSSystemId { get; set; }
        public string ROSTemplateSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string RecordCount { get; set; }
        public string ROSCharatristicsids { get; set; }
        public string IsGlobal { get; set; }
        public string ROSSystemCharacteristicsId { get; set; }
        public string IsError { get; set; }
        public string TemplateId { get; set; }

    }

    public class ROSSystems
    {
        public string commandType { get; set; }
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string RecordCount { get; set; }

    }

    public class ROSTemplateModel
    {
        public string Name { get; set; }
        public string commandType { get; set; }
        public string TemplateId { get; set; }
        public string DataTemplateId { get; set; }
        [XmlIgnore]
        public string SpecialtyIds { get; set; }
        [XmlIgnore]
        public string ProviderIds { get; set; }

        public string PhysicalExamTemplateEntity { get; set; }
        public string IsActive { get; set; }
        public long? BodyPartId { get; set; }
        public string IsDefault { get; set; }
        public string LastUpdated { get; set; }

        public string Comments { get; set; }

        public string EntityId { get; set; }

        public string SaveAsTemplateName { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public List<ROSSytemTemplateModel> ROSSystems { get; set; }
        public List<ROSCharTemplateModel> SystemChartristicsdetail { get; set; }
        public string ProviderId { get; set; }
        public string SpecialtyXML { get; set; }
        public string ProviderXML { get; set; }
        public string SystemCharacteristicsXML { get; set; }
        public string NoteId { get; set; }
        public string UserId { get; set; }
        public List<ROSCharaDetailGenralModel> ROSCharaDetailGenral { get; set; }
        public string NormalCommetnsAll { get; set; }
        public string IsNormalAll { get; set; }
        [XmlIgnore]
        public string SOAPText { get; set; }
        public string IsPositive { get; set; }
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public bool IsRecordDelete { get; set; }
    }
    public class ROSSytemTemplateModel
    {
        public string ROSSystemId { get; set; }
        public string IsNormalComments { get; set; }
        public string IsNormal { get; set; }
        public string IsChecked { get; set; }
        public string SystemName { get; set; }
        public List<ROSCharTemplateModel> RosChartristicsDetail { get; set; }
    }
    public class ROSCharTemplateModel
    {
        
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsName { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string IsSystemDeleted { get; set; }
        public string IsSystemChecked { get; set; }
        public string IsObservationDeleted { get; set; }
        public string IsChecked { get; set; }
        public string CharComments { get; set; }
        public string IsPositive { get; set; }
        public string PreviousHistory { get; set; }
        public string ROSCharacteristicsDetailStatusId { get; set; }
        public string Onset { get; set; }
        public string Duration { get; set; }
        public string ROSCharacteristicsDetailDurationId { get; set; }
        public string ROSCharacteristicsDetailPatternId { get; set; }
        public string ROSCharacteristicsDetailSeverityId { get; set; }
        public string ROSCharacteristicsDetailCourseId { get; set; }
        public string ROSCharacteristicsDetailRadiationId { get; set; }
        public string ROSCharacteristicsDetailFrequencyId { get; set; }
        public string ROSCharacteristicsDetailContextId { get; set; }
        public string ROSCharacteristicsDetailCharacterCSZId { get; set; }
        public string ROSCharacteristicsDetailAggravedById { get; set; }
        public string ROSCharacteristicsDetailRelievedById { get; set; }
        public string Location { get; set; }
        public string PrecipitatedBY { get; set; }
        public string AssociatedWith { get; set; }
        public string hfROSCharacteristicsDetailsId { get; set; }

    }

    public class ROSTempSysCharInfo
    {
        public string ROSTempSysCharId { get; set; }
        public string IsSelected { get; set; }
        public string ROSTemplateId { get; set; }
        public string ROSSystemCharacteristicsId { get; set; }
        public string ROSTemplateSystemId { get; set; }
        public string CharacteristicsName { get; set; }
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string SystemName { get; set; }
        public string TemplateName { get; set; }
        public string IsActive { get; set; }
        public string ProviderIds { get; set; }
        public string SpecialtyIds { get; set; }
        public string IsSelectedSystem { get; set; }
        public string PreviousHistory { get; set; }
        public string ROSCharacteristicsDetailStatusId { get; set; }
        public string Onset { get; set; }
        public string Duration { get; set; }
        public string ROSCharacteristicsDetailDurationId { get; set; }
        public string ROSCharacteristicsDetailPatternId { get; set; }
        public string ROSCharacteristicsDetailSeverityId { get; set; }
        public string ROSCharacteristicsDetailCourseId { get; set; }
        public string ROSCharacteristicsDetailRadiationId { get; set; }
        public string ROSCharacteristicsDetailFrequencyId { get; set; }
        public string ROSCharacteristicsDetailContextId { get; set; }
        public string ROSCharacteristicsDetailCharacterCSZId { get; set; }
        public string ROSCharacteristicsDetailAggravedById { get; set; }
        public string ROSCharacteristicsDetailRelievedById { get; set; }
        public string Location { get; set; }
        public string PrecipitatedBY { get; set; }
        public string AssociatedWith { get; set; }
        public string hfROSCharacteristicsDetailsId { get; set; }

    }
    public class ROSCharaDetailGenralModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string OldLookupId { get; set; }
        public string LookupId { get; set; }
        public string Value { get; set; }
        [XmlIgnore]
        public string Text { get; set; }
        [XmlIgnore]
        public string LookupTypeName { get; set; }
        public string IsDeleted { get; set; }

    }
    public class OnsetModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string Onset { get; set; }


    }
    public class DurationModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string Duration { get; set; }
        public string DurationValue { get; set; }

    }
    public class ROSCharacteristicsDetailDurationIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailDurationId { get; set; }


    }
    public class ROSCharacteristicsDetailPatternIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailPatternId { get; set; }


    }
    public class ROSCharacteristicsDetailSeverityIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailSeverityId { get; set; }


    }
    public class ROSCharacteristicsDetailCourseIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailCourseId { get; set; }


    }
    public class ROSCharacteristicsDetailRadiationIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailRadiationId { get; set; }


    }
    public class ROSCharacteristicsDetailFrequencyIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailFrequencyId { get; set; }


    }
    public class ROSCharacteristicsDetailContextIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailContextId { get; set; }


    }
    public class ROSCharacteristicsDetailCharacterCSZIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailCharacterCSZId { get; set; }


    }
    public class ROSCharacteristicsDetailAggravedByIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailAggravedById { get; set; }


    }
    public class ROSCharacteristicsDetailRelievedByIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailRelievedById { get; set; }


    }
    public class LocationModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string Location { get; set; }
        public string LocationValue { get; set; }


    }
    public class PrecipitatedBYModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string PrecipitatedBY { get; set; }
        public string PrecipitatedBYValue { get; set; }


    }
    public class AssociatedWithModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string AssociatedWith { get; set; }
        public string AssociatedWithValue { get; set; }


    }
    public class ROSCharacteristicsDetailStatusIdModel
    {
        public string ROSSystemId { get; set; }
        public string ROSCharacteristicsId { get; set; }
        public string ROSCharacteristicsDetailStatusId { get; set; }
    }


    public class ROSLookUps
    {
        public string ROSCharacteristicsDetailLookupId { get; set; }
        public string LookupValueName { get; set; }
        public string LookupTypeName { get; set; }
    }
    public class ROSCharateristicsDetail
    {

        public string CharateristicsDetailName { get; set; }
        public List<ROSLookUps> CharatristicsDetail { get; set; }
    }
    public class VMROSCharateristicsDetail
    {
        public List<ROSCharateristicsDetail> Charateristics { get; set; }
    }
}