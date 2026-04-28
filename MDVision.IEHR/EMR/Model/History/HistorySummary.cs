using MDVision.Model.Clinical.History.HistorySummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    /*
    Author: Muhammad Azhar Shahzad
    * Date: Dec 16,2015
    * Reason: FOR DTO data of History Summary from it's childs
    */

    public class HistoryModelList {
        public BirthHxModel birthHxModel{ get; set; }
        public SocialHxModel socialHxmodel { get; set; }
        public SurgicalHxModel surgicalHxModel { get; set; }
        public FamilyHxModel familyModel { get; set; }
        public HospitalizationHxModel hospitalModel { get; set; }
        public MedicalHxModel medicalhXmodel { get; set; }

    }
    public partial class HistorySummary
    {
        public int HistoryId { get; set; }
        public long PatientId { get; set; }
        public long SocialHxId { get; set; }
        public long FamilyHxId { get; set; }
        public long MedicalHxId { get; set; }
        public long SurgicalHxId { get; set; }
        public long EnvironmentalHxId { get; set; }
        public long BirthHxId { get; set; }
        public long HospitalizationHxId { get; set; }
        public string commandType { get; set; }
        public long HxId { get; set; }
        public string HxType { get; set; }
        public string Status { get; set; }

        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }

        public string HxtabOrder { get; set; }

        public string NoteId { get; set; }
        public string UserId { get; set; }
        public string EntityId { get; set; }
        
        public string ComponentType { get; set; }
        
        public MedicalHxModel MedicalHxList { get; set; }
        public FamilyHxModel FamilyHxList { get; set; }
        public SurgicalHxModel SurgicalHxList { get; set; }
        public BirthHxModel BirthHxList { get; set; }
        public HospitalizationHxModel HospitalizationHxList { get; set; }
        public SocialHxModel SocialHxList { get; set; }
        public SocPsyandBehaviorHxModel SocPsyandBehaviorHx { get; set; }
    }
    /*
     Author: Muhammad Azhar Shahzad
     * Date: Dec 16,2015
     * Reason: FOR DTO Soap Text of History Summary from it's childs
     */
    public partial class HistorySummary 
    {
        public string SocialHxSoapText { get; set; }
        public string FamilyHxSoapText { get; set; }
        public string MedicalHxSoapText { get; set; }
        public string SurgicalHxSoapText { get; set; }
        public string EnvironmentalHxSoapText { get; set; }
        public string BirthHxSoapText { get; set; }
        public string HospitalizationHxSoapText { get; set; }
    }
}