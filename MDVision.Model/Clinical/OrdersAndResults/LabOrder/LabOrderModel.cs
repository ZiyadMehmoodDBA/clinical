using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder
{
    public class LabOrderModel
    {
        public LabOrderModel()
        {
            LabOrderTests = new List<LabOrderTestModel>();
        }


        public string PatientName { get; set; }
        public string LabOrderId { get; set; }
        public string LabOrderIDs { get; set; }
        public string PatientId { get; set; }
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
        public string Specimen { get; set; }
        public string SpecimenSource { get; set; }
        public string OrderNo { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string OrderQuery { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string NoteId { get; set; }
        public Boolean IsFavoriteGroupTypeSave { get; set; }
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public List<LabOrderProblemModel> LabOrderProblemList { get; set; }
        public string Test { get; set; }
        public string OrderFromDate { get; set; }
        public string OrderToDate { get; set; }
        public string Status { get; set; }
        public string FavListNames { get; set; }
        public string FreeTextNames { get; set; }
        
        public string bResultExists { get; set; }
        public string bResultAcknowledged { get; set; }
        public string IsNoteLinked { get; set; }
        public LabOrderResultLatestNoteModel LabOrderResultLatestNoteModel { get; set; }
        public string BarCodeHtml { get; set; }
        public List<LabOrderQuestionAnswerModel> CPTCodeQuestionsAnswers { get; set; }

        public List<LabOrderTestModel> LabOrderTests { get; set; }
        public string LabIds { get; set; }
        public string UpdateTestValues { get; set; }
        public string NegationReasonId { get; set; }
        public string NegationReason { get; set; }
        public string FacilityShortName { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string AssigneeFirstName { get; set; }
        public string AssigneeLastName { get; set; }
        public string PrimaryInsuraceFirstName { get; set; }
        public string PrimaryInsuraceLastName { get; set; }
        public string SecondaryInsuraceFirstName { get; set; }
        public string SecondaryInsuraceLastName { get; set; }
        public string TertiaryInsuraceFirstName { get; set; }
        public string TertiaryInsuraceLastName { get; set; }
        public string RelationCode { get; set; }
        public string GuarantorFirstName { get; set; }
        public string GuarantorLastName { get; set; }
    }
}