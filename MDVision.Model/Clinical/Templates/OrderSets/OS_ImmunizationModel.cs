using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Templates.OrderSets
{

    public class OS_ImmunizationModel
    {

        public string FormName { get; set; }
        public string UserId { get; set; }
        public bool IsVaccineInsert { get; set; }
        public string ImmunizationIds { get; set; }
        public string CompletionStatus { get; set; }
        public string IsActiveRecord { get; set; }
        public string Time { get; set; }
        public string commandType { get; set; }
        public string IsActive { get; set; }
        public string ImmunizationHxId { get; set; }
        public string PatientId { get; set; }
        public string PatientAge { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }

        public string CvxCode { get; set; }
        //    public string VisDate { get; set; }

        public string EntityId { get; set; }
        public string TabId { get; set; }

        // -------------------------------
        public string NotesId { get; set; }
        public string VaccineHxId { get; set; }
        public string OS_VaccineHxId { get; set; }
        
        public string CategoryID { get; set; }
        public string VaccineID { get; set; }
        public string LotNo { get; set; }
        public string RouteId { get; set; }
        public string Route { get; set; }
        public string VfcId { get; set; }
        public string Vfc { get; set; }
        public string Comments { get; set; }
        public string VisitDate { get; set; }
        public string AdministrationDate { get; set; }
        public string Dose { get; set; }
        public string Amount { get; set; }
        public string SiteId { get; set; }
        public string SiteDescription { get; set; }
        public string VisDateId { get; set; }
        public string VisDate { get; set; }
        public string ViewDocumentId { get; set; }
        public string ViewDocumentLink { get; set; }
        public string GivenById { get; set; }
        public string OrderSetId { get; set; }
        
        public string GivenByName { get; set; }
        public string ManufacturerId { get; set; }
        public string Manufacturer { get; set; }
        public string ExpiryDate { get; set; }
        public string SourceOfHxId { get; set; }
        public string SourceOfHx { get; set; }

        public string Type { get; set; }

        public string VaccineHxIds { get; set; }

        //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
        public string ProviderId { get; set; }
        public string CompletionStatusCode { get; set; }
        public string RefusalReasonCode { get; set; }
        public string PublicityCode { get; set; }
        public string ImmunizationRegistryStatusCode { get; set; }
        public string PublicityCodeExpiryDate { get; set; }
        public string IRSEffectiveDate { get; set; }
        public string PIEffectiveDate { get; set; }
        public bool ProtectionIndicator { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        //End   || 20 April, 2016 || ZeeshanAK || Changes for new fields

        // Add ||  june 2016 || Talha Tanweer
        public string AgeLimit { get; set; }

        // Add || 20 june 2016 || Talha Tanweer
        public string VaccineScheduleId { get; set; }

        public string Reaction { get; set; }
        public bool VoidDose { get; set; }
        public string RefusalReasonID { get; set; }
        public string VisitDateId { get; set; }
        public string VaccineGroupID { get; set; }
        public string Schedule { get; set; }
        public string ScheduleId { get; set; }
        public string ScheduleTypeId { get; set; }
        public string ScheduleType { get; set; }
        public string StartDueDate { get; set; }
        public string EndOverDueDate { get; set; }
        public string Maxage { get; set; }
        public string fromId { get; set; }
        public string toId { get; set; }
        public string LotId { get; set; }

        public string MainAgeGroup { get; set; }
        public string MainSchedule { get; set; }
        public string MainCategory { get; set; }


        public string ScheduleShortName { get; set; }

        public bool OverrideRule { get; set; }

        public string ImmTherInjectionIds { get; set; }


    }

    public class OS_ImmunizationAlert
    {
        public string VaccineName { get; set; }
        public string Alert { get; set; }
        public string NoOfDays { get; set; }
        public string DueDate { get; set; }
        public string RecordCount { get; set; }
        public string AlertId { get; set; }
        public string Category { get; set; }
        public string VaccineScheduleId { get; set; }



    }
    public class OS_ImmunizationAdministerVaccineHx
    {
        public string VaccineHxId { get; set; }
        public string AdministerVaccine_Category { get; set; }
        public string AdministerVaccine_VisitDate { get; set; }

        public string AdministerVaccine_Vaccine { get; set; }
        public string AdministerVaccine_AdministrationDate { get; set; }
        public string AdministerVaccine_AdministrationTime { get; set; }
        public string AdministerVaccine_GivenBy { get; set; }
        public string AdministerVaccine_LotNumber { get; set; }

        public string AdministerVaccine_Dose { get; set; }
        public string AdministerVaccine_Amount { get; set; }
        public string AdministerVaccine_Manufacturer { get; set; }
        public string AdministerVaccine_Route { get; set; }
        public string AdministerVaccine_Site { get; set; }
        public string AdministerVaccine_ExpiryDate { get; set; }
        public string AdministerVaccine_VFC { get; set; }
        public string AdministerVaccine_VISDate { get; set; }
        public string AdministerVaccine_Comments { get; set; }

        //Start || 21 April, 2016 || ZeeshanAK || Changes for new fields
        public string AdministerVaccine_Provider { get; set; }
        public string AdministerVaccine_CompletionStatus { get; set; }
        public string AdministerVaccine_RefusalReason { get; set; }
        public string AdministerVaccine_PublicityCode { get; set; }
        public string AdministerVaccine_ImmunizationRegistryStatus { get; set; }
        public string AdministerVaccine_PublicityExpiryDate { get; set; }
        public string AdministerVaccine_IRSEffectiveDate { get; set; }
        public string AdministerVaccine_PIEffectiveDate { get; set; }
        public string AdministerVaccine_ProtectionIndicator { get; set; }
        public string AdministerVaccine_CreatedBy { get; set; }
        public string AdministerVaccine_CreatedOn { get; set; }
        public string AdministerVaccine_ModifiedOn { get; set; }
        public string AdministerVaccine_ModifiedBy { get; set; }
        public string AdministerReaction { get; set; }
        public string AdministerVoidDose { get; set; }
        public string AdministerVaccine_IRS { get; set; }
        public string AdministerVaccine_IsActive { get; set; }
        public string LotText { get; set; }

        public string AdministerOverrideRule { get; set; }
        


        //End   || 21 April, 2016 || ZeeshanAK || Changes for new fields
    }

    public class OS_ImmunizationDocumentHxDoseHx
    {
        public string VaccineHxId { get; set; }
        public string DocumentHxDose_Category { get; set; }
        public string DocumentHxDose_VisitDate { get; set; }
        public string DocumentHxDose_Vaccine { get; set; }
        public string DocumentHxDose_AdministrationDate { get; set; }
        public string DocumentHxDose_AdministrationTime { get; set; }
        public string DocumentHxDose_GivenBy { get; set; }
        public string DocumentHxDose_LotNo { get; set; }

        public string DocumentHxDose_Dose { get; set; }
        public string DocumentHxDose_Amount { get; set; }
        public string DocumentHxDose_Manufacturer { get; set; }
        public string DocumentHxDose_Route { get; set; }
        public string DocumentHxDose_Site { get; set; }
        public string DocumentHxDose_ExpiryDate { get; set; }
        public string DocumentHxDose_VFC { get; set; }
        public string DocumentHxDose_VISDate { get; set; }
        public string DocumentHxDose_Comments { get; set; }
        public string DocumentHxDose_SourceOfHx { get; set; }
        //Start || 21 April, 2016 || ZeeshanAK || Changes for new fields
        public string DocumentHxDose_Provider { get; set; }
        public string DocumentHxDose_CompletionStatus { get; set; }
        public string DocumentHxDose_RefusalReason { get; set; }
        public string DocumentHxDose_PublicityCode { get; set; }
        public string DocumentHxDose_ImmunizationRegistryStatus { get; set; }
        public string DocumentHxDose_PublicityExpiryDate { get; set; }
        public string DocumentHxDose_IRSEffectiveDate { get; set; }
        public string DocumentHxDose_PIEffectiveDate { get; set; }
        public string DocumentHxDose_ProtectionIndicator { get; set; }
        public string DocumentHxDose_CreatedBy { get; set; }
        public string DocumentHxDose_CreatedOn { get; set; }
        public string DocumentHxDose_ModifiedOn { get; set; }
        public string DocumentHxDose_ModifiedBy { get; set; }
        public string DocumentHxVoidDose { get; set; }

        public string DocumentHxDose_IsActive { get; set; }
        


        //End   || 21 April, 2016 || ZeeshanAK || Changes for new fields

    }

    public class OS_ImmunizationRefusalRecord
    {
        public string VaccineHxId { get; set; }
        public string RecordRefusal_Category { get; set; }
        public string RecordRefusal_Vaccine { get; set; }
        public string RecordRefusal_Provider { get; set; }
        public string RecordRefusalReason { get; set; }
        public string RecordRefusalVaccine_ExpiryDate { get; set; }
        public string RecordRefusal_Comments { get; set; }
        public string RecordRefusalVoidDose { get; set; }
        public string RecordRefusal_IsActive { get; set; }
        
    }

    public class OS_Immunization
    {
        public string VaccineId { get; set; }
        public string VaccineName { get; set; }
        public string Dose { get; set; }
        public string AdministrationDate { get; set; }
        public string GivenBy { get; set; }
        public string Location { get; set; }
        public string LotNumber { get; set; }
        public string Type { get; set; }
        public string VaccineHxId { get; set; }
        public string IsNoteLinked { get; set; }
        public string Category { get; set; }
        public string VaccineScheduleId { get; set; }
    }

    public class OS_ImmunizationScheduler
    {

        public string AdministrationDate { get; set; }
        public string AdministrationTime { get; set; }
        public string GivenBy { get; set; }
        public string VaccineHxId { get; set; }
        public string PatientAge { get; set; }
        public string Category { get; set; }
        public string Schedule { get; set; }
        public string Type { get; set; }
        public string VaccineScheduleId { get; set; }

    }
    public class OS_ParentChildImunizationModel
    {
        public OS_Immunization ParentImmunization = new OS_Immunization();
        public List<OS_Immunization> ChildImmunizationList = new List<OS_Immunization>();
    }
}
