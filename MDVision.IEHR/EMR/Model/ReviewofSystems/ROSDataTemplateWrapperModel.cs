using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.ReviewofSystems
{
    public class ROSDataTemplateWrapperModel
    {
        public ROSDataTemplateWrapperModel()
        {
            SystemsWrapperModel = new ROSSystemsWrapperModel();
        }
        public long ROSSystemInfoID { get; set; }
        public long ROSDataTempInfoId { get; set; }
        public long ROSDataTemplateId { get; set; }
        public long ROSTemplateId { get; set; }
        public string DataTemplateName { get; set; }

        public string commandType { get; set; }

        public string ROSComments { get; set; }

        public bool IsProviderAll { get; set; }
        public bool IsSpecialityAll { get; set; }

        public string SpecialityNames { get; set; }
        public string SpecialityIds { get; set; }
        public long RecordCount { get; set; }
        public int? IsActive { get; set; }
        public long PageNumber { get; set; }
        public long RowsPerPage { get; set; }

        public long ROSSystemId { get; set; }
        public int? EntityId { get; set; }

        public List<ROSTemptSystemCharCModel> CharacteristicsList { get; set; }
        public List<ROSSystemsModel> SystemsList { get; set; }
        public bool? isSaveAS { get; set; }

        public string SystemsWrapperString { get; set; }
        public ROSSystemsWrapperModel SystemsWrapperModel { get; set; }
        public long ROSDataSystemID { get; set; }
        public long ROSDataSystemCharcID { get;  set; }
        public bool RemoveSystemCharcDetails { get;  set; }
        public string ProviderId { get; set; }
    }
    public class ROSDataTempResponseModel
    {
        public long ROSDataTemplateId { get; set; }
        public long ROSTemplateId { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }

        public long ROSDataTempInfoId { get; set; }

        public bool IsTemplateChanged { get; set; }
    }
    public class ROSDataTempInfoModel
    {
        public long ROSDataTempInfoId { get; set; }
        public string Description { get; set; }
        public bool IsNormal { get; set; }
        public string ROSSystemDate { get; set; }
        public string Comments { get; set; }

        public long ROSDataTemplateId { get; set; }
        public long ROSTemplateId { get; set; }

        public string DataTemplateName { get; set; }

        public bool IsTemplateChanged { get; set; }
    }
}