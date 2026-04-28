using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.CCM.CCMHub
{
    public class PatientHubStatic
    {
        public string PatientImage { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string EmergencyContact { get; set; }
        public string HomePhone { get; set; }
        public string Relation { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIPCode { get; set; }
        public string Patientphone { get; set; }
        public string NextAppointment { get; set; }
        public string LastAppointment { get; set; }
        public string Comments { get; set; }
        public string StatusId { get; set; }
        public string CCMStatus { get; set; }
        public string Reason { get; set; }
        public string EnrollmentDate { get; set; }
        public string Program { get; set; }
        public string ProviderName { get; set; }
        public string ImageType { get; set; }
        public string PatientImage_ { get; set; }
    }

    public class PatientHubProblems
    {
        public string Id { get; set; }
        public string ICD10 { get; set; }
        public string ICD10_Description { get; set; }
    }
    public class RiskAssessment
    {
        public string EnrolledRiskAssesId { get; set; }
        public string RiskAssessmentId { get; set; }
        public string EnrollmentInfoId { get; set; }
        public string RiskAssessTemptId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public float RiskScore { get; set; }
    }

    public class EnrolledRiskAssessment
    {
        public string EnrollmentInfoId { get; set; }
        public string RiskAssessmentId { get; set; }
        public string TemplateId { get; set; }
        public string TemplateDescription { get; set; }
        public string RiskScore { get; set; }
    }
    public class EnrolledRiskAssessmentTemp
    {
        public string EnrolledRiskAssessmentTempId { get; set; }
        public string EnrollmentInfoId { get; set; }
        public string TemplateId { get; set; }
        public string AssessHTML { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
    }
    public class EnrolledCareTeam
    {
        public string EnrolledCareTeamId { get; set; }
        public string EnrollmentInfoId { get; set; }
        public string CareTeamId { get; set; }
        //public string EndSelectionBy { get; set; }
        //public string EndSelectionDate { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
    public class ProviderCareTeam
    {
        public string CareTeamId { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public string PCPName { get; set; }
        public string CareManagerName { get; set; }
        public string CareCoordinatorName { get; set; }
        public string CareGiverName { get; set; }
        public string LastUpdated { get; set; }
        public string Specialty { get; set; }
        public string ProviderPhone { get; set; }
        public string CareManagerPhone { get; set; }
        public string CareCoordinatorPhone { get; set; }
        public string CareGiverPhone { get; set; }
        public string ProviderId { get; set; }
    }

    public class EnrolledGoals
    {
        public Int64 EnrolledGoalsId { get; set; }
        public Int64 EnrollmentInfoId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public class EnrolledGoalsCPT
    {
        public Int64 EnrolledGoalsICDId { get; set; }
        public Int64 EnrolledGoalsId { get; set; }
        public Int64 CPTCodeId { get; set; }
        public string Instruction { get; set; }
    }

    public class PatientHubEnrolledGoalsCPT
    {
        public string EnrolledGoalsICDId { get; set; }
        public string EnrolledGoalsId { get; set; }
        public string EnrollmentInfoId { get; set; }
        public string CPTpkID { get; set; }
        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public string SNOMEDCode { get; set; }
        public string SNOMEDDescription { get; set; }
        public string Instruction { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedOn { get; set; }
    }

    public class EnrolledGoals_EnrolledGoalsCPT
    {
        public Int64 EnrolledGoalsICDId { get; set; }
        public Int64 EnrolledGoalsId { get; set; }
        public Int64 CPTCodeId { get; set; }
        public string Instruction { get; set; }
        public Int64 EnrollmentInfoId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class CCMTermination
    {
        public Int64 EnrollmentInfoId { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
