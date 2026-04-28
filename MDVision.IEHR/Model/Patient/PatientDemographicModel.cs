using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Patient
{
    public class PatientDemographicModel
    {
        public string AccountNo { get; set; }
        public string CommandType { get; set; }
        public string LastName { get; set; }
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
        public string MaritalStatus_text { get; set; }
        public string MaritalStatus { get; set; }
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
        public string InsuranceBalance { get; set; }
        public string AdvanceBalance { get; set; }
        public string Image_url { get; set; }
        public string BirthDate { get; set; }
        public string InsuranceName { get; set; }
        public string strRaceIds { get; set; }
        public string Scan { get; set; }
        public string OCR { get; set; }

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
        public string PatientProfileImagePath { get; set; }
        public string PatientProfileThumbnailPath { get; set; }

    }
}
