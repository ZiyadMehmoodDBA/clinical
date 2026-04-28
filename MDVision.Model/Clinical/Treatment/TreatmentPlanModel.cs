using MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder;
using MDVision.IEHR.EMR.Model.OrdersAndResults.RadiologyOrder;
using MDVision.Model.Clinical.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Treatment
{
    public class TreatmentPlanModel
    {
        public string commandType { get; set; }
        public string NoteId { get; set; }
        public string Comment { get; set; }
        public List<TreatmentComponentModel> Treatments { get; set; }
        public List<TreatmentComponentModel> DeleteTreatments { get; set; }
    }

    public class TreatmentComponentModel
    {
        public string ProblemId { get; set; }
        public string TreatmentId { get; set; }
        public string ProblemDescription { get; set; }
        public string PrescriptionIds { get; set; }
        public string LabOrderIds { get; set; }
        public string DiagnosticImagingIds { get; set; }
        public string ProcedureIds { get; set; }
        public string ImmunizationIds { get; set; }
        public string TherapeuticIds { get; set; }
        public string ReferralIds { get; set; }
        public string Comments { get; set; }
    }
    public class SaveTreatmentModel
    {
        public string ProblemId { get; set; }
        public string TreatmentId { get; set; }
        public string TreatmentDataId { get; set; }
        public string NoteComponentsLookupId { get; set; }
        public string DataId { get; set; }
        public string Comment { get; set; }

    }

    public class TreatementSoapTextDataModel
    {
        public List<Treatments> TreatmentList { get; set; }
        public List<TreatmentData> TreatmentDataList { get; set; }
        public List<ClinicalPrescriptionModel> TreatementPrescriptionList { get; set; }
        public List<TreatmentProcedure> TreatmentProcedureList { get; set; }
        public List<LabOrderModel> LabOrderList { get; set; }
        public List<LabOrderTestModel> LabOrderTestList { get; set; }
        public List<LabOrderProblemModel> LabOrderProblemList { get; set; }
        public List<RadiologyOrderModel> RadiologyOrderList { get; set; }
        public List<RadiologyOrderTestModel> RadiologyOrderTestList { get; set; }
        public List<RadiologyOrderProblemModel> RadiologyOrderProblemList { get; set; }
        public List<ReferralModel> ReferralList { get; set; }
        public List<ReferralProcedureModel> ReferralProcedureList { get; set; }
        public List<ReferralProblemModel> ReferralProblemList { get; set; }
        public List<TreatmentVaccineHx> VaccineHxList { get; set; }
        public List<TreatmentTerapuetic> TerapeuticList { get; set; }
    }
    public class Treatments
    {
        public string TreatmentId { get; set; }
        public string NoteId { get; set; }
        public string ProblemListId { get; set; }
        public string ProblemDescription { get; set; }
        public string Comments { get; set; }

    }
    public class TreatmentData
    {
        public string TreatmentDataId { get; set; }
        public string TreatmentId { get; set; }
        public string ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string DataId { get; set; }
    }
    public class TreatmentProcedure
    {
        public string ProcedureId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }
        public string NoteId { get; set; }
        public string Diagnosis { get; set; }
        public string ProcedureCodeName { get; set; }
        public string ProcedureTemplateSoapTextExists { get; set; }
        public string ShowCptCode { get; set; }
    }
    public class ReferralModel
    {
        public string ReferralId { get; set; }
        public string PatientId { get; set; }
        public string Type { get; set; }
        public string ProviderId { get; set; }
        public string AssigneeId { get; set; }
        public string RefProviderId { get; set; }
        public string ProviderName { get; set; }
        public string AssigneeName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string IsActive { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string SoapText { get; set; }
        public string Status { get; set; }
        public string PAN { get; set; }
        public string Visits { get; set; }
        public string Reason { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string PatientInsuranceName { get; set; }
        public string RefProviderName { get; set; }
        public string FacilityFromName { get; set; }
        public string FacilityToName { get; set; }
        public string SpecialityToName { get; set; }
        public string StatusName { get; set; }
        public string Procedures { get; set; }
        public string Comments { get; set; }
    }
    public class ReferralProcedureModel
    {
        public string ReferralProcedureId { get; set; }
        public string ReferralId { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string Procedure { get; set; }
        public string UrgencyId { get; set; }
        public string Urgency { get; set; }
        public string UrgencyName { get; set; }
        public string ShowCptCode { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }

    }
    public class ReferralProblemModel
    {
        public string ReferralProblemId { get; set; }
        public string ReferralId { get; set; }
        public string ProblemId { get; set; }
        public string ProblemName { get; set; }
        public string Comments { get; set; }
        public string SoapText { get; set; }
        public string IsActive { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class TreatmentVaccineHx
    {
        public string VaccineHxId { get; set; }
        public string VaccineName { get; set; }
        public string Dose { get; set; }
        public string AdministrationDate { get; set; }
        public string Amount { get; set; }
        public string RouteDescription { get; set; }
        public string SiteDescription { get; set; }
        public string ManufacturerName { get; set; }
        public string ExpiryDate { get; set; }
        public string Type { get; set; }
        public bool VoidDose { get; set; }
        public string ProviderName { get; set; }
        public string RefusalReason { get; set; }
        public string Comments { get; set; }
        public string CPT { get; set; }
    }
    public class TreatmentTerapuetic
    {
        public string Type { get; set; }
        public string CPTCode { get; set; }
        public string TherapeuticInjection { get; set; }
        public string ImmTherInjectionId { get; set; }
        public string RouteDescription { get; set; }
        public string SiteDescription { get; set; }
        public string LotNumber { get; set; }
        public string ManufacturerName { get; set; }
        public float Dose { get; set; }
        public string Amount { get; set; }
        public string ProviderName { get; set; }
        public string AdministrationDate { get; set; }


    }
}
