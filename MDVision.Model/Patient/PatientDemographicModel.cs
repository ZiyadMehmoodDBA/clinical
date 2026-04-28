using MDVision.Model.Native;
using System;
using System.Collections.Generic;

namespace MDVision.Model.Patient
{
    public class PatientDemographicModel
    {
        public PatientDemographicModel()
        {
           
            DataChangeRequest = new List<DataChangeRequest>();
            this.PatientRaces = new List<PatientRacesModel>();
            //patientInsurance_JSON = new List<PatientInsuranceModel>();
        }
        public string AccountNo { get; set; }
        public string ClaimFlagDescription { get; set; }
        public string CommandType { get; set; }
        public string LastName { get; set; }
        public string MotherMaidenName { get; set; }
        public string FirstName { get; set; }
        public string DOB { get; set; }
        public string Ethnicity { get; set; }
        public string Race { get; set; }
        public string PrefLanguage { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ZipExt { get; set; }
        public string Active { get; set; }
        public string Comments { get; set; }
        public string FacilityID { get; set; }
        public string PracticeID { get; set; }
        public string ProviderID { get; set; }
        public string LanguageID { get; set; }
        public string Provider { get; set; }
        public string Facility { get; set; }
        public string Practice { get; set; }
        public string MRN { get; set; }
        public string SSN { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string MiddleInitial { get; set; }
        public string PreviousName { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string SelfPay { get; set; }
        public string InactiveReason { get; set; }
        public string Address2 { get; set; }
        public string HomeTel { get; set; }
        public string WorkTel { get; set; }
        public string Ext { get; set; }
        public string Cell { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string BadAddress { get; set; }
        public string RefProviderID { get; set; }
        public string PCPID { get; set; }
        public string GuarantorID { get; set; }
        public string TypeID { get; set; }
        public string StatusID { get; set; }
        public string CalledDate { get; set; }
        public string EntryDate { get; set; }
        public string PatientPortalStatus { get; set; }
        public string PatientID { get; set; }
        //public string PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public string MaritalStatus_text { get; set; }
        public string MaritalStatus { get; set; }
        public string MaritalStatusId { get; set; }
        public string Sex_text { get; set; }
        public string Suffix_text { get; set; }
        public string Prefix_text { get; set; }
        public string PCP { get; set; }
        public string Guarantor { get; set; }
        public string RefProvider { get; set; }
        public string imgPatient { get; set; }
        public string IsDemographicQuick { get; set; }
        public string FullName { get; set; }
        public string PatientBalance { get; set; }
        public string CollectionBalance { get; set; }
        public string InsuranceBalance { get; set; }
        public string AdvanceBalance { get; set; }
        public string Image_url { get; set; }
        public string BirthDate { get; set; }
        public string InsuranceName { get; set; }
        public string strRaceIds { get; set; }
        public string Scan { get; set; }
        public string OCR { get; set; }
        public string strEthnicityIds { get; set; }
        public string GenderIdentityId { get; set; }
        public string GenderIdentityName { get; set; }
        public string SexualOrientationId { get; set; }
        public string SexualOrientationName { get; set; }
        public string imagedata { get; set; }
        public string filetype { get; set; }
        public string filename { get; set; }
        public string foldername { get; set; }
        public string ScannerDOS { get; set; }
        public string AssignUserto { get; set; }
        public string ScannerComments { get; set; }

        public string AssignedToName { get; set; }

        public string PatDocId { get; set; }
        public string ScndPrefCommunicationId { get; set; }
        public string PrefCommunicationId { get; set; }
        public string HearFromId { get; set; }
        public string HearFromOther { get; set; }
        public string CommunicateWithGurantor { get; set; }

        public string ReferralId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string RefProviderReferral { get; set; }
        public string RefProviderIdReferral { get; set; }
        public string ProviderReferral { get; set; }
        public string ProviderIdReferral { get; set; }
        public string Reason { get; set; }
        public string Visits { get; set; }
        public string PatientInsurance { get; set; }
        public string Status { get; set; }
        public string Assignee { get; set; }
        public string AssigneeId { get; set; }
        public string CommentsReferral { get; set; }
        public string ProviderFName { get; set; }
        public string ProviderLName { get; set; }
        public string FacilityAddress { get; set; }
        public string FacilityCity { get; set; }
        public string FacilityState { get; set; }
        public string FacilityZip { get; set; }
        public string FacilityZipExt { get; set; }
        public string FacilityPhone { get; set; }
        public string PatientImageThumbnail { get; set; }
        public string EnrollmentInfoId { get; set; }
        public string CCMStatus { get; set; }

        public int IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string PatientDocumentImage { get; set; }

        public string PatientImage { get; set; }
        public string ImageType { get; set; }
        public string PatientInsuranceId { get; set; }
        public string PatientPHQScore { get; set; }
        public string TransitionalCareManagement { get; set; }
        public string DischargeDate { get; set; }
        public string InsurancePlanId { get; set; }
        public string DateOfDeath { get; set; }
        public string CauseOfDeath { get; set; }
        public string PatientRaceIds_text { get; set; }
        public string ConfidentialityCode { get; set; }
        public string BirthSex { get; set; }
        public string PatientProfileImagePath { get; set; }
        public string PatientProfileThumbnailPath { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
        public string Country { get; set; }
        public string CountryID { get; set; }
        public string PreferredAddressID { get; set; }
        public string PreferredPhoneID { get; set; }
        public List<PatientRacesModel> PatientRaces { get; set; }
        public string IsImageUpdated { get; set; }
        public string PatientWorkPhoneExt { get; set; }
        public string PatientEmergencyContactName { get; set; }
        public string PatientEmergencyContactAddress { get; set; }
        public string EmergencyContactRelationship { get; set; }
        public string PatientEmergencyContactPhone { get; set; }
        public string PatientEmergencyContactCell { get; set; }
        public string CityId { get; set; }
        public long MissingDataAlertsCount { get; set; }
        public string IsCCDAAvailable { get; set; }
        public string CareGiverIds { get; set; }
    }
    public class PatientRacesModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class BillingInquiryEmailModel
    {
        public string hfPatientId { get; set; }
        public string hfAccountNo { get; set; }
        public string hfPatientFullName { get; set; }
        public string hfPatientFullNameOnly { get; set; }
        public string hfPatientRefProviderId { get; set; }
        public string hfPatientRefProviderName { get; set; }
        public string hfPatientPracticeId { get; set; }
        public string hfPatientFacilityId { get; set; }
        public string hfPatientInsuranceId { get; set; }
        public string hfDischargeDate { get; set; }
        public string hfPatientBalance { get; set; }
        public string hfPatientFacilityName { get; set; }
        public string hfPatientProviderName { get; set; }
        public string hfPatientProviderId { get; set; }
        public string hfPatientSex { get; set; }
        public string hfPatientDOB { get; set; }
        public string hfPatientMaritalStatus { get; set; }
        public string hfPatientEthnicityIds { get; set; }
        public string hfPatientRaceIds { get; set; }
        public string hfPatientAddress1 { get; set; }
        public string hfPatientCity { get; set; }
        public string hfPatientState { get; set; }
        public string hfPatientZip { get; set; }
        public string hfPatientHomeTel { get; set; }
        public string CommandType { get; set; }
    }
}
