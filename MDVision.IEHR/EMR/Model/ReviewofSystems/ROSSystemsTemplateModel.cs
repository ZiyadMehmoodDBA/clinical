using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.ReviewofSystems
{
    public class ROSSystemsTemplateModel
    {

        public long ROSTemplateId { get; set; }
        public string TemplateName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string commandType { get; set; }
        public int? IsActive { get; set; }
        public bool IsDefault { get; set; }
        public bool IsProviderAll { get; set; }
        public bool IsSpecialityAll { get; set; }
        public string ProviderNames { get; set; }

        public string ProviderIds { get; set; }
        public string SpecialityNames { get; set; }
        public string SpecialityIds { get; set; }
        public long RecordCount { get; set; }

        public long PageNumber { get; set; }
        public long RowsPerPage { get; set; }

        public long ROSSystemId { get; set; }
    }

    public class ROSTemplateResponse {
        public bool status { get; set; }
        public string Message { get; set; }
        public long ROSTemplateId { get; set; }
    }
    public class ROSTemplateWrapperModel
    {

        public long ROSTemplateId { get; set; }
        public string TemplateName { get; set; }
       
        public string commandType { get; set; }
       
        public bool IsProviderAll { get; set; }
        public bool IsSpecialityAll { get; set; }
        public string ProviderNames { get; set; }

        public string ProviderIds { get; set; }
        public string SpecialityNames { get; set; }
        public string SpecialityIds { get; set; }
        public long RecordCount { get; set; }
        public int IsActive { get; set; }
        public long PageNumber { get; set; }
        public long RowsPerPage { get; set; }

        public long ROSSystemId { get; set; }
        public int? EntityId { get; set; }

        public List<ROSTemptSystemCharCModel> CharacteristicsList { get; set; }
        public List<ROSSystemsModel> SystemsList { get; set; }
        public bool? isSaveAS { get; set; }
    }

    public class ROSTemptSystemCharCModel
    {
        public string CharacteristicsName { get; set; }
        public long CharacteristicsId { get; set; }
        public long ROSTemplateId { get; set; }
        public string SystemName { get; set; }
        public long ROSSystemId { get; set; }



        public int SortingOrder { get; set; }
    }
    
}