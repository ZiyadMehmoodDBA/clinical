using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.Clinical.Templates.OrderSets
{
    public class OS_RadiologyOrderModel
    {
        public string RadiologyOrderId { get; set; }
        public string RadiologyOrderIDs { get; set; }
        public string OrderSetId { get; set; }
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
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public List<OS_RadiologyOrderProblemModel> RadiologyOrderProblemList { get; set; }
        public string Test { get; set; }
        public string OrderFromDate { get; set; }
        public string OrderToDate { get; set; }
        public string Status { get; set; }
        public string FavListNames { get; set; }
        public string FreeTextNames { get; set; }
        public string Comments { get; set; }
    }
    public class OS_RadiologyOrderProblemModel
    {
        public string RadiologyOrderProblemId { get; set; }

        public string RadiologyOrderId { get; set; }

        public string ProblemId { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }
        public string commandType { get; set; }
    }
    public class OS_RadiologyOrderTestModel
    {
        public string RadiologyOrderTestId { get; set; }
        public string RadiologyOrderId { get; set; }

        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public string dtpRadiologyDate { get; set; }
        public string tpRadiologyTime { get; set; }
        public string RadiologyProcedure { get; set; }
        public string CollectedAt { get; set; }
        public string Urgency { get; set; }
        public string Specimen { get; set; }
        public string PatientInstructions { get; set; }
        public string VolumeText { get; set; }
        public string VolumeDDL { get; set; }
        public string FillerInstructions { get; set; }
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public string RadiologyTestIds { get; set; }
        //Start 23/3/2016 Farooq Ahmad	field to Contain the text of the dropdown lists
        public string Urgency_text { get; set; }
        public string Specimen_text { get; set; }
        //End 23/3/2016 Farooq Ahmad	field to Contain the text of the dropdown lists

        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }
    }
}