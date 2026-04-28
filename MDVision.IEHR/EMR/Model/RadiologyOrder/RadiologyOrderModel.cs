using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.RadiologyOrder
{
    public class RadiologyOrderModel
    {
        public string RadiologyOrderId { get; set; }
        public string RadiologyOrderIDs { get; set; }
        public string UserName { get; set; }
        public string PatientId { get; set; }
        public string LabId { get; set; }
        public string ProviderId { get; set; }
        public string Provider { get; set; }
        public string FacilityId { get; set; }
        public string Facility { get; set; }
        public string AssigneeId { get; set; }     
        public string OrderDate { get; set; }
        public string OrderTime { get; set; }
        public string BillingTypeId { get; set; }
        public string PrimaryInsuraceId { get; set; }
        public string SecondaryInsuraceId { get; set; }
        public string TertiaryInsuraceId { get; set; }
        public string GuarantorId { get; set; }
        public string GuarantorName { get; set; }
        public string RelationShipId { get; set; }
        public string IsActive { get; set; }
        public string OrderNo { get; set; }       
        public string ModifiedOn { get; set; }        
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string OrderQuery { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string NoteId { get; set; }
        
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public List<RadiologyOrderProblemModel> RadiologyOrderProblemList { get; set; }
        public string Test { get; set; }
        public string OrderFromDate { get; set; }
        public string OrderToDate { get; set; }
        public string Status { get; set; }
        public string FavListNames { get; set; }
        public string FreeTextNames { get; set; }
        public string FacilityTo { get; set; }
        public string Comments { get; set; }
    }
}