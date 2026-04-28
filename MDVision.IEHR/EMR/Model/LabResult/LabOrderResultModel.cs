using MDVision.IEHR.EMR.Model.LabOrder;
/* Author:  Muhammad Arshad
 * Created Date: 15/04/2016
 * OverView: Created to handel Lab Result
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.LabResult
{
    public class LabOrderResultModel
    {
        public LabOrderResultModel()
        {
            LabOrderResultDetailModels = new List<LabOrderResultDetailModel>();
            LabOrderTests = new List<LabOrderTestModel>();
        }


        public string LabId { get; set; }
        public string AssigneeName { get; set; }
        public string ResultNumber { get; set; }
        public string PatientName { get; set; }
        public string LabResultId { get; set; }
        public string LabResultIDs { get; set; }
        public string PatientId { get; set; }
        public string LabOrderId { get; set; }
        public string LabName { get; set; }
        public string ProviderId { get; set; }
        public string Provider { get; set; }
        public string FacilityId { get; set; }
        public string Facility { get; set; }
        public string AssigneeId { get; set; }
        public string OrderDate { get; set; }
        public string OrderTime { get; set; }
        public string Comments { get; set; }
        public string Remarks { get; set; }
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

        public string IsNoteLinked { get; set; }

        public string commandType { get; set; }
        public string SoapText { get; set; }
   
        public string Test { get; set; }
        public string OrderFromDate { get; set; }
        public string OrderToDate { get; set; }
        public string Status { get; set; }
        public string LOINCCode { get; set; }
        public string LOINCDescription { get; set; }

        public bool IsSentToPortal { get; set; }
        public bool IsAknowledged { get; set; }
        public bool IsReviewed { get; set; }
        public bool IsAllResult { get; set; }
        public string CountStatus { get; set; }
        public string CountLabId { get; set; }
        public string CountOrderFromDate { get; set; }
        public string CountOrderToDate { get; set; }
        public bool CountIsReviewed { get; set; }
        public bool CountIsAllResult { get; set; }
        public string PatientFullName { get; set; }

        public string AccountNumber { get; set; }

        public bool isManually { get; set; }
    
        public string ReviewedById { get; set; }
        public string ReviewedBy { get; set; }
        public string callFromGrid { get; set; }

        public string BarCodeHtml { get; set; }

        public List<long> DeletedResultDetailIds { get; set; }
        public List<LabOrderResultDetailModel> LabOrderResultDetailModels { get; set; }
        public List<LabOrderTestModel> LabOrderTests { get; set; }
        public string PracticeId { get; set; }
        public string NTEText { get; set; }
        public string LabOrderResultDetailId { get; set; }
        public LabOrderResultLatestNoteModel LabOrderResultLatestNoteModel { get; set; }
        public string PatientFacilityId { get; set; }
        public string IsAttribute { get; set; }
        public bool isReviewedFromDashBoard { get; set; }
        public bool isReviewedDashBoardUnsolicited { get; set; }
        public string LabTestId { get; set; }
        public string LabTestAttributeId { get; set; } 
        public long LabOrderResultExternalPDFId { get; set; }
        public bool IsElectronicResult { get; set; }
        public bool MarkAsReviewed { get; set; }
        public string CollectionDateTime { get; set; }

    }
}