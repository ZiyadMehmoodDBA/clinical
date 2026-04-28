using System.Collections.Generic;
namespace MDVision.Model.Clinical.BillingInformation
{
    public class BillingInformationModel
    {
        public BillingInformationModel()
        {
            if (ICDs == null)
                ICDs = new List<BillingInfoICDModel>();
            if (CPTs == null)
                CPTs = new List<BillingInfoCPTModel>();
        }
        public long NotesId { get; set; }
        public long AppointmentId { get; set; }
        public string commandType { get; set; }
        public string BillingInfoType { get; set; }
        public string POS { get; set; }
        public string FacilityId { get; set; }
        public string Facility { get; set; }
        public string RefProviderId { get; set; }
        public string RefProvider { get; set; }
        public string Status { get; set; }
        public string TimeMin { get; set; }
        public string Type { get; set; }
        public string BillingInfoId { get; set; }
        public string ENMTypeId { get; set; }
        public string ENMTimeId { get; set; }
        public string ENMCPTCode { get; set; }
        public string ENMCPTDescription { get; set; }
        public string ENMCPTUnit { get; set; }
        public string ENMCPTDOSFrom { get; set; }
        public string ENMCPTDOSTo { get; set; }
        public string AdmissionDate { get; set; }
        public string DischargeDate { get; set; }
        public string PatientId { get; set; }
        public string ICDCode { get; set; }
        public string CPTCode { get; set; }
        public string VisitId { get; set; }
        public string ProviderId { get; set; }
        public string Provider { get; set; }
        public string ResourceProvider { get; set; }
        public string ResourceProviderId { get; set; }
        public string ICDCodes { get; set; }
        public string CPTCodes { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
        public string DOSFrom { get; set; }
        public string DOSTo { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<BillingInfoICDModel> ICDs { get; set; }
        public List<BillingInfoCPTModel> CPTs { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string CPTid { get; set; }
        public bool ShowCustomFormData { get; set; }
        public string ShowImplantableDeviceData { get; set; }
    }

    public class BLLBillingInformationModel
    {
        public long BillingInfoId { get; set; }
        public string CPTS_JSON { get; set; }
    }
}
