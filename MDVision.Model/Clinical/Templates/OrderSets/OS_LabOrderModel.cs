using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical.Templates.OrderSets
{
    public class OS_LabOrderModel
    {
        public OS_LabOrderModel()
        {
            LabOrderTests = new List<OS_LabOrderTestModel>();
        }
        public List<OS_LabOrderQuestionAnswerModel> CPTCodeQuestionsAnswers { get; set; }
        public List<OS_LabOrderProblemModel> LabOrderProblemList { get; set; }
        public string OrderSetName { get; set; }
        public string LabOrderId { get; set; }
        public string LabOrderIDs { get; set; }
        public string OrderSetId { get; set; }
        public string LabId { get; set; }
        public string LabName { get; set; }
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
        public string AssigneeName { get; set; }
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
        public string Test { get; set; }
        public string OrderFromDate { get; set; }
        public string OrderToDate { get; set; }
        public string Status { get; set; }
        public string FavListNames { get; set; }
        public string FreeTextNames { get; set; }

        public string bResultExists { get; set; }
        public string bResultAcknowledged { get; set; }

        public string BarCodeHtml { get; set; }
        public List<OS_LabOrderTestModel> LabOrderTests { get; set; }
    }
    public class OS_LabOrderTestModel {
        public OS_LabOrderTestModel()
        {
            LabOrderResultDetails = new List<OS_LabOrderResultDetailModel>();
        }
        public string LabOrderTestId { get; set; }
        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public string LabOrderId { get; set; }
        public string dtpLabDate { get; set; }
        public string tpLabTime { get; set; }
        public string LabProcedure { get; set; }
        public string CollectedAt { get; set; }
        public string ClinicalLabOrderDiet { get; set; }
        public string Urgency { get; set; }
        public string Specimen { get; set; }

        public string AlternativeSpecimen { get; set; }
        public string PatientInstructions { get; set; }
        public string VolumeText { get; set; }
        public string VolumeDDL { get; set; }
        public string FillerInstructions { get; set; }
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public string LabTestIds { get; set; }
        //Start 23/3/2016 Farooq Ahmad	field to Contain the text of the dropdown lists
        public string Urgency_text { get; set; }
        public string Specimen_text { get; set; }
        public string SampleStorage { get; set; }
        public string Modifier { get; set; }
        //End 23/3/2016 Farooq Ahmad	field to Contain the text of the dropdown lists
        public string IsABN { get; set; }
        public List<OS_LabOrderResultDetailModel> LabOrderResultDetails { get; set; }
    }
    public class OS_LabOrderQuestionAnswerModel {
        public string CPTCode { get; set; }
        public string commandType { get; set; }
        public int LabOrderAOEAnswersID { get; set; }
        public long LabOrderTestId { get; set; }
        //public string TestCode { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
    public class OS_LabOrderProblemModel {
        public string LabOrderProblemId { get; set; }

        public string LabOrderId { get; set; }

        public string ProblemId { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }
        public string commandType { get; set; }
    }
    public class OS_LabOrderResultDetailModel{
    }
}