using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical
{
    public class ClinicalSummaryModel
    {
        public string PlanOfCareId { get; set; }
        public string CognitiveId { get; set; }
        public string GoalId { get; set; }
        public string CognitiveStatusId { get; set; }
        public string FunctionalStatusId { get; set; }
        public string MentalStatusId { get; set; }
        public string IsFromNote { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public string ClinicalInstruction { get; set; }
        public string FutureInstruction { get; set; }
        public string PatientDecisionAid { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
        public string NoteId { get; set; }
        public string commandType { get; set; }
        public string Password { get; set; }
        public string PlanOfCareType { get; set; }
        public string SummaryHtml { get; set; }
        public string IncludeHashCode { get; set; }
        public string Encryption { get; set; }
        public string DataIsEncrypted { get; set; }
        public string XMLData { get; set; }
        public List<Component> Components { get; set; }
        public string referralProvider { get; set; }
        public string raferralReason { get; set; }
        public string Patients { get; set; }
        public string Template { get; set; }
        public string toEmail { get; set; }
        public string IncludeXML { get; set; }
        public string IncludeHTML { get; set; }
        public List<Dictionary<string, string>> PatientInfo { get; set; }
        public string SummaryType { get; set; }
        public string IsConfidential { get; set; }
        public string QueryString { get; set; }
        public string msgType { get; set; }
        public string MessageDetail { get; set; }
        public string PatientAccountNo { get; set; }
        public DateTime DOS { get; set; }
        public string DocType { get; set; }
    }

    public class CCDADataModel
    {
        public CCDADataModel
            (
              string VisitDate,
              List<Dictionary<string, string>> ReasonForVisit,
              List<Component> Components,
              List<Dictionary<string, string>> lstPatientData,
              List<Dictionary<string, string>> lstProviderData,
              List<Dictionary<string, string>> lstPracticeData,
              List<Dictionary<string, string>> lstNoteData,
              List<Dictionary<string, string>> lstProblems,
              List<Dictionary<string, string>> lstProblemsCancer,
              List<Dictionary<string, string>> lstVitals,
              List<Dictionary<string, string>> lstAllergs,
              List<Dictionary<string, string>> lstSocials,
              List<Dictionary<string, string>> lstMedicalHx,
              List<Dictionary<string, string>> lstSurgicalHx,
              List<Dictionary<string, string>> lstHospitalizationHx,
              List<Dictionary<string, string>> lstFamilyHx,
              List<Dictionary<string, string>> lstBirthHx,
              List<Dictionary<string, string>> lstPhysicalExam,
              List<Dictionary<string, string>> lstMedication,
              List<Dictionary<string, string>> lstMedicationsAdministered,
              List<Dictionary<string, string>> lstProcedure,
              List<Dictionary<string, string>> lstImmunization,
              List<Dictionary<string, string>> lstPlanOfCare,
              List<Dictionary<string, string>> lstscheduledProcedure,
              List<Dictionary<string, string>> lstFutureAppointment,
              List<Dictionary<string, string>> lstGoal,
              List<Dictionary<string, string>> lstResults,
              List<Dictionary<string, string>> lstRefferalProviderData,
              List<Dictionary<string, string>> lstCognitiveFunctionalStatus,
              List<Dictionary<string, string>> lstReasonReferral,
              List<Dictionary<string, string>> lstPartialResultPending,
              List<Dictionary<string, string>> lstInstructionsAndDecisionAids,
              List<Dictionary<string, string>> lstEncounterDiagnosicDatakeyValues,
              List<Dictionary<string, string>> lstLabOrderTests,
              List<Dictionary<string, string>> lstImplantableDevice,
              List<Dictionary<string, string>> lstRaceCodes,
              List<Dictionary<string, string>> lstEthnicityCodes,
              List<Dictionary<string, string>> lstCaregivers,
              List<Dictionary<string, string>> lstCareManagers,
              List<Dictionary<string, string>> lstCareCoordinators,
              List<Dictionary<string, string>> lstCareTeamPCP,
              List<Dictionary<string, string>> lstCareTeamProvider,
              string IsConfidential,
              List<Dictionary<string, string>> lstMentalStatus,
              List<Dictionary<string, string>> lstInsurance,
              List<Dictionary<string, string>> lstHealthConsern,
              List<Dictionary<string, string>> lstHealthObservation,
              List<Dictionary<string, string>> lstHealthRisks,
              List<Dictionary<string, string>> lstPlanedMedications,
              List<Dictionary<string, string>> lstInterventions,
              List<Dictionary<string, string>> lstOutComes,
              List<Dictionary<string, string>> lstAROOrganizm,
              List<Dictionary<string, string>> lstAUP,
              List<Dictionary<string, string>> lstOutPatientEncounter,
              List<Dictionary<string, string>> lstAROAntimicrobial,
              List<Dictionary<string, string>> lstAROObservations,
              List<Dictionary<string, string>> lstEmploymentHx,
              List<Dictionary<string, string>> lstRadiologyResults,
              List<Dictionary<string,string>> lstPatientParticipant
            )
        {
            this.VisitDate = VisitDate;
            this.lstPatientData = lstPatientData;
            this.lstProviderData = lstProviderData;
            this.lstPracticeData = lstPracticeData;
            this.lstNoteData = lstNoteData;
            this.lstProblems = lstProblems;
            this.lstProblemsCancer = lstProblemsCancer;
            this.lstVitals = lstVitals;
            this.lstAllergs = lstAllergs;
            this.lstSocials = lstSocials;
            this.lstMedicalHx = lstMedicalHx;
            this.lstSurgicalHx = lstSurgicalHx;
            this.lstHospitalizationHx = lstHospitalizationHx;
            this.lstFamilyHx = lstFamilyHx;
            this.lstBirthHx = lstBirthHx;
            this.lstPhysicalExam = lstPhysicalExam;
            this.lstMedication = lstMedication;
            this.lstMedicationsAdministered = lstMedicationsAdministered;
            this.lstProcedure = lstProcedure;
            this.lstImmunization = lstImmunization;
            this.ReasonForVisit = ReasonForVisit;
            this.lstPlanOfCare = lstPlanOfCare;
            this.lstscheduledProcedure = lstscheduledProcedure;
            this.lstFutureAppointment = lstFutureAppointment;
            this.lstGoal = lstGoal;
            this.lstResults = lstResults;
            this.lstRefferalProviderData = lstRefferalProviderData;
            this.Components = Components;
            this.lstCognitiveFunctionalStatus = lstCognitiveFunctionalStatus;
            this.lstReasonReferral = lstReasonReferral;
            this.lstPartialResultPending = lstPartialResultPending;
            this.lstInstructionsAndDecisionAids = lstInstructionsAndDecisionAids;
            this.lstEncounterDiagnosicDatakeyValues = lstEncounterDiagnosicDatakeyValues;
            this.lstLabOrderTests = lstLabOrderTests;
            this.lstImplantableDevice = lstImplantableDevice;
            this.lstRaceCodes = lstRaceCodes;
            this.lstEthnicityCodes = lstEthnicityCodes;
            this.lstCaregivers = lstCaregivers;
            this.lstCareManagers = lstCareManagers;
            this.lstCareCoordinators = lstCareCoordinators;
            this.IsConfidential = IsConfidential;
            this.lstMentalStatus = lstMentalStatus;
            this.lstInsurance = lstInsurance;
            this.lstCareTeamPCP = lstCareTeamPCP;
            this.lstCareTeamProvider = lstCareTeamProvider;
            this.lstHealthConsern = lstHealthConsern;
            this.lstHealthObservation = lstHealthObservation;
            this.lstPlanedMedications = lstPlanedMedications;
            this.lstHealthRisks = lstHealthRisks;
            this.lstInterventions = lstInterventions;
            this.lstOutComes = lstOutComes;
            this.lstAROOrganizm = lstAROOrganizm;
            this.lstAUP = lstAUP;
            this.lstOutPatientEncounter = lstOutPatientEncounter;
            this.lstAROAntimicrobial = lstAROAntimicrobial;
            this.lstAROObservations = lstAROObservations;
            this.lstEmploymentHx = lstEmploymentHx;
            this.lstRadiologyResults = lstRadiologyResults;
            this.lstPatientParticipant = lstPatientParticipant;
        }

        public List<Dictionary<string, string>> lstPatientData { get; set; }
        public List<Dictionary<string, string>> lstProviderData { get; set; }
        public List<Dictionary<string, string>> lstPracticeData { get; set; }
        public List<Dictionary<string, string>> lstNoteData { get; set; }
        public List<Dictionary<string, string>> lstProblems { get; set; }
        public List<Dictionary<string, string>> lstProblemsCancer { get; set; }
        public List<Dictionary<string, string>> lstVitals { get; set; }
        public List<Dictionary<string, string>> lstAllergs { get; set; }
        public List<Dictionary<string, string>> lstSocials { get; set; }
        public List<Dictionary<string, string>> lstMedicalHx { get; set; }
        public List<Dictionary<string, string>> lstSurgicalHx { get; set; }
        public List<Dictionary<string, string>> lstHospitalizationHx { get; set; }
        public List<Dictionary<string, string>> lstFamilyHx { get; set; }
        public List<Dictionary<string, string>> lstBirthHx { get; set; }
        public List<Dictionary<string, string>> lstPhysicalExam { get; set; }
        public List<Dictionary<string, string>> lstMedication { get; set; }
        public List<Dictionary<string, string>> lstImmunization { get; set; }
        public List<Dictionary<string, string>> lstProcedure { get; set; }
        public List<Dictionary<string, string>> lstMedicationsAdministered { get; set; }
        public List<Dictionary<string, string>> lstPlanOfCare { get; set; }
        public List<Dictionary<string, string>> lstscheduledProcedure { get; set; }
        public List<Dictionary<string, string>> lstFutureAppointment { get; set; }
        public List<Dictionary<string, string>> lstGoal { get; set; }
        public List<Dictionary<string, string>> lstResults { get; set; }
        public List<Dictionary<string, string>> lstRefferalProviderData { get; set; }
        public List<Dictionary<string, string>> lstCognitiveFunctionalStatus { get; set; }
        public List<Dictionary<string, string>> lstReasonReferral { get; set; }

        public List<Dictionary<string, string>> lstPartialResultPending { get; set; }
        public List<Dictionary<string, string>> lstInstructionsAndDecisionAids { get; set; }
        public List<Component> Components { get; set; }

        public List<Dictionary<string, string>> lstEncounterDiagnosicDatakeyValues { get; set; }
        public string VisitDate { get; set; }
        public List<Dictionary<string, string>> ReasonForVisit { get; set; }
        public List<Dictionary<string, string>> lstLabOrderTests { get; set; }
        public List<Dictionary<string, string>> lstImplantableDevice { get; set; }
        public List<Dictionary<string, string>> lstRaceCodes { get; set; }
        public List<Dictionary<string, string>> lstEthnicityCodes { get; set; }
        public List<Dictionary<string, string>> lstCaregivers { get; set; }
        public List<Dictionary<string, string>> lstCareManagers { get; set; }
        public List<Dictionary<string, string>> lstCareCoordinators { get; set; }
        public string IsConfidential { get; set; }
        public List<Dictionary<string, string>> lstMentalStatus { get; set; }
        public List<Dictionary<string, string>> lstInsurance { get; set; }
        public List<Dictionary<string, string>> lstCareTeamPCP { get; set; }
        public List<Dictionary<string, string>> lstCareTeamProvider { get; set; }
        public List<Dictionary<string, string>> lstHealthConsern { get; set; }
        public List<Dictionary<string, string>> lstHealthObservation { get; set; }
        public List<Dictionary<string, string>> lstPlanedMedications { get; set; }
        public List<Dictionary<string, string>> lstHealthRisks { get; set; }
        public List<Dictionary<string, string>> lstInterventions { get; set; }
        public List<Dictionary<string, string>> lstOutComes { get; set; }
        public List<Dictionary<string, string>> lstAROOrganizm { get; set; }
        public List<Dictionary<string, string>> lstAUP { get; set; }
        public List<Dictionary<string, string>> lstOutPatientEncounter { get; set; }
        public List<Dictionary<string, string>> lstAROAntimicrobial { get; set; }
        public List<Dictionary<string, string>> lstAROObservations { get; set; }
        public List<Dictionary<string, string>> lstEmploymentHx { get; set; }
        public List<Dictionary<string, string>> lstRadiologyResults { get; set; }
        public List<Dictionary<string, string>> lstPatientParticipant { get; set; }
    }


    public class Component
    {
        public int componentId { get; set; }

        public string componentName { get; set; }
    }

    public class componentName
    {

        public static string ProblemLists { get { return "ProblemLists"; } }
        public static string SocialHx { get { return "SocialHx"; } }
        public static string Medications { get { return "Medications"; } }
        public static string Allergies { get { return "Allergies"; } }
        public static string Immunization { get { return "Immunization"; } }
        public static string LabResults { get { return "LabResults"; } }
        public static string Vitals { get { return "Vitals"; } }
        public static string Procedures { get { return "Procedures"; } }
        public static string VisitReason { get { return "VisitReason"; } }
        public static string PlanOfCare { get { return "PlanOfCare"; } }
    }
}