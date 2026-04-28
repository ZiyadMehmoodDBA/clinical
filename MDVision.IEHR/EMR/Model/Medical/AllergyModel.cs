using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class AllergyModel
    {
        public string IsDeleted { get; set; }
        public string LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public string AllergyId { get; set; }
        public string Allergen { get; set; }
        public string Type { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
        public string OnSetDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }
        public string PatientId { get; set; }
        public string NoteId { get; set; }
        public string commandType { get; set; }
        public string InActiveChkBoxValue { get; set; }
        public string InActiveReason { get; set; }
        public string IsActiveRecord { get; set; }
        public string IsActive { get; set; }
        public string AllergySeverityType { get; set; }
        public string AllergyType { get; set; }
        public string iTotalDisplayRecords { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string RcopiaID { get; set; }
        public string VisitId { get; set; }
        // Author:  Muhammad Ahmad Imran
        // Created Date: 14/01/2016
        //OverView: Add new Attribute LastUpdateDate for Update AllergyLastUpdateDate in PatientLastUpdateInfo
        public string LastUpdateDate { get; set; }
        public string RxnormID { get; set; }
        public string RxnormIDType { get; set; }
        public string UserId { get; set; }
        public string EntityId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Status { get; set; }
        public string TypeSNOMEDCode { get; set; }
    }
}