using MDVision.IEHR.EMR.Model.ReviewofSystems;
/*  
    Author: ZeeshanAK
    Creation Date: January 27, 2016
    OverView:This File Is created for ROS Systems Model
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem
{
    public class ROSSystemsModel
    {
        public long ROSSystemId { get; set; }
        public string SystemName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string commandType { get; set; }
        public long PatientId { get; set; }
        public long ROSSystemInfoID { get; set; }

        public long NotesId { get; set; }

        public long ROSSystemPatientID { get; set; }

        public long ROSTemplateId { get; set; }
        public long ROSDataTemplateId { get; set; }
        public long SortingOrder { get; set; }
    }
    public class ROSSystemsWrapperModel
    {
        public bool? isSaveAS { get; set; }
        public string DataTemplateName { get; set; }
        public long ROSSystemId { get; set; }
        public string commandType { get; set; }
        public long PatientId { get; set; }
        public List<string> ROSSystemNames { get; set; }
        public List<string> ROSSystemIds { get; set; }
        public List<ROSCharacteristicsDetailsModel> rosPatientCharcObjList { get; set; }

        public List<string> NormalSystems { get; set; }
        public List<string> isNormalDescription { get; set; }
        public List<string> AllPositiveSystems { get; set; }
        public List<string> AllNegativeSystems { get; set; }
        public List<string> charcPosSystem { get; set; }
        public List<string> charcPosCharacteristics { get; set; }
        public List<string> charcDescNeg { get; set; }
        public List<string> charcDescPos { get; set; }
        public List<string> charcPosName { get; set; }
        public List<string> charcNegName { get; set; }
        public List<string> charcNegSystem { get; set; }
        public List<string> charcNegCharacteristics { get; set; }
        public List<string> UncheckedCharacteristics { get; set; }
        public List<string> SystemsWithDetails { get; set; }

        public long ROSSystemInfoID { get; set; }
        public string ROSComments { get; set; }
        public string ROSNormalDescription { get; set; }
        public bool ROSisNormal { get; set; }
        public string ReviewofSystemsDate { get; set; }

        public string systemSortingOrder { get; set; }
        public string ROSSystemPatSortingOrder { get; set; }
        //   public string systemNormalDescriptionList { get; set; }
        public long NotesId { get; set; }
        public string SoapText { get; set; }

        public long ROSTemplateId { get; set; }

        public long ROSDataTemplateId { get; set; }

        public string ROSDataTempName { get; set; }

    }
}
