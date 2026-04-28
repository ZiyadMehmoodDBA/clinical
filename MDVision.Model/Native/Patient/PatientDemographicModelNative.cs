using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Patient
{
  public  class PatientDemographicModelNative
    {
        public string AccountNo { get; set; }
        //public string ClaimFlagDescription { get; set; }
        //public string CommandType { get; set; }

        public PatientDemographicModelNative()
        {
           
            lstChangedColumns = new List<ChangedColumnsNative>();
            //patientInsurance_JSON = new List<PatientInsuranceModel>();
        }      



        public string LastName { get; set; }
        public string CommandType { get; set; }

        public string PatientID { get; set; }
        public string DimmyPatientId { get; set; }
        public string FirstName { get; set; }
        public string DOB { get; set; }  
        public string EthnicityId { get; set; }
        //public string Race { get; set; }
        public string PrefLanguageId { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ZipCodeExt { get; set; }
        //public string Active { get; set; }
        //public string Comments { get; set; }
        //public string FacilityID { get; set; }
        //public string PracticeID { get; set; }
        //public string ProviderID { get; set; }
        public string PrefLanguage { get; set; }
        //public string Provider { get; set; }
        //public string Facility { get; set; }
        //public string Practice { get; set; }
        //public string MRN { get; set; }
        public string SSN { get; set; }
        //public string Prefix { get; set; }
        //public string Suffix { get; set; }
        public string MI { get; set; }
        //public string PreviousName { get; set; }
        //public string Age { get; set; }
        public string Gender { get; set; }
        public string SelfPay { get; set; }
        //public string InactiveReason { get; set; }
        public string Address2 { get; set; }
        public string HomePhoneNo { get; set; }
        //public string WorkTel { get; set; }
        //public string Ext { get; set; }
        public string CellNo { get; set; }
        //public string Fax { get; set; }
        public string EmailAddress { get; set; }
        //public string BadAddress { get; set; }
        //public string RefProviderID { get; set; }
        //public string PCPID { get; set; }
        //public string GuarantorID { get; set; }
        //public string TypeID { get; set; }
        //public string StatusID { get; set; }
        //public string CalledDate { get; set; }
        //public string EntryDate { get; set; }
        //public string PatientPortalStatus { get; set; }
        //public string PatientID { get; set; }
        //public string MaritalStatus_text { get; set; }
        public string MaritialStatus { get; set; }
        //public string Sex_text { get; set; }
        //public string Suffix_text { get; set; }
        //public string Prefix_text { get; set; }
        //public string PCP { get; set; }
        //public string Guarantor { get; set; }
        //public string RefProvider { get; set; }
        //public string imgPatient { get; set; }
        //public string IsDemographicQuick { get; set; }
        //public string FullName { get; set; }
        //public string PatientBalance { get; set; }
        //public string InsuranceBalance { get; set; }
        //public string AdvanceBalance { get; set; }
        //public string Image_url { get; set; }
        //public string BirthDate { get; set; }
        //public string InsuranceName { get; set; }
        public string strRaceIds { get; set; }
        public string RaceId { get; set; }
        //public string Scan { get; set; }
        //public string OCR { get; set; }

        //public string imagedata { get; set; }
        //public string filetype { get; set; }
        //public string filename { get; set; }
        //public string foldername { get; set; }
        //public string ScannerDOS { get; set; }
        //public string AssignUserto { get; set; }
        //public string ScannerComments { get; set; }

        //public string AssignedToName { get; set; }

        //public string PatDocId { get; set; }
        //public string ScndPrefCommunicationId { get; set; }
        //public string PrefCommunicationId { get; set; }
        //public string HearFromId { get; set; }
        //public string HearFromOther { get; set; }
        //public string CommunicateWithGurantor { get; set; }

        //public string ReferralId { get; set; }
        //public string Date { get; set; }
        //public string Time { get; set; }
        //public string RefProviderReferral { get; set; }
        //public string RefProviderIdReferral { get; set; }
        //public string ProviderReferral { get; set; }
        //public string ProviderIdReferral { get; set; }
        //public string Reason { get; set; }
        //public string Visits { get; set; }
        //public string PatientInsurance { get; set; }
        public string RequestStatus { get; set; }
        //public string Assignee { get; set; }
        //public string AssigneeId { get; set; }
        //public string CommentsReferral { get; set; }
        //public string ProviderFName { get; set; }
        //public string ProviderLName { get; set; }
        //public string FacilityAddress { get; set; }
        //public string FacilityCity { get; set; }
        //public string FacilityState { get; set; }
        //public string FacilityZip { get; set; }
        //public string FacilityZipExt { get; set; }
        //public string FacilityPhone { get; set; }
        public byte[] PatientImageThumbnail { get; set; }
        //public string EnrollmentInfoId { get; set; }
        //public string CCMStatus { get; set; }

        //public int IsActive { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        //public string PatientDocumentImage { get; set; }

        public byte[] PatientImage { get; set; }
        public string ImageType { get; set; }
        public string imgPatient { get; set; }
        public string PatientProfileImagePath { get; set; }
        public string PatientProfileThumbnailPath { get; set; }
        //public string PatientInsuranceId { get; set; }
        //public string PatientPHQScore { get; set; }
        //public string TransitionalCareManagement { get; set; }
        //public string DischargeDate { get; set; }
        //public string InsurancePlanId { get; set; }
        //public string DateOfDeath { get; set; }
        //   public string CauseOfDeath { get; set; }
        public string listChangedColumns { get; set; }
        public  List<ChangedColumnsNative> lstChangedColumns { get; set; }

       

    }
}
