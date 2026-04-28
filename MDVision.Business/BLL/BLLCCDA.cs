using System;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.CCDA;
using MDVision.Model.Common;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using System.Data;

namespace MDVision.Business.BLL
{
    public class BLLCCDA
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLPatient"/> class.
        /// </summary>
        public BLLCCDA()
        {
            InitializeComponent();
        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region CCDS

        public string ImportCCDAData(DSCCDA ds)
        {
            string errors = string.Empty;
            try
            {
                DALCCDA obj = new DALCCDA();

                #region Encounter
                if (ds.NotesEncounter.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtNotesEncounter = ds.NotesEncounter.GetChanges();
                        obj.InsertUpdateEncounter_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors = "Encounter: " + ex.Message + ", ";
                    }
                }
                #endregion Encounter

                #region Problems List               

                if (ds.ProblemList.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtTempProblemList = ds.ProblemList.GetChanges();
                        obj.InsertUpdateProblemList_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Problems List: " + ex.Message + ", ";
                    }
                }

                #endregion Problems List

                #region Allergies

                if (ds.Allergy.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtTempAllergy = ds.Allergy.GetChanges();
                        obj.InsertUpdateAllergies_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Allergies: " + ex.Message + ", ";
                    }
                }

                #endregion Allergies

                #region Medication
                if (ds.Medication.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtTempMedication = ds.Medication.GetChanges();
                        obj.InsertUpdateMedications_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Medication: " + ex.Message + ", ";
                    }
                }
                #endregion Medication

                #region Immunization
                if (ds.Immunization.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtTempImmunization = ds.Immunization.GetChanges();
                        obj.InsertUpdateImmunization_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Immunization: " + ex.Message + ", ";
                    }
                }
                #endregion Immunization

                #region Vital Signs
                if (ds.VitalSigns.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtTempVitalSigns = ds.VitalSigns.GetChanges();
                        obj.InsertUpdateVitalSigns_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Vital Signs: " + ex.Message + ", ";
                    }
                }
                #endregion Vital Signs

                #region Social History     
                if (ds.SocialHistory.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtTempSocialHistory = ds.SocialHistory.GetChanges();
                        obj.InsertUpdateSocialHistory_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Social History: " + ex.Message + ", ";
                    }
                }
                #endregion Social History

                #region Procedures
                if (ds.NotesProcedures.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtTempNotesProcedures = ds.NotesProcedures.GetChanges();
                        obj.InsertUpdateProcedures_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Procedures: " + ex.Message + ", ";
                    }
                }
                #endregion Procedures

                #region Lab Test              
                if (ds.LabOrderTest.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtTempLabOrderTest = ds.LabOrderTest.GetChanges();
                        obj.InsertUpdateLabTest_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Lab Test: " + ex.Message + ", ";
                    }
                }
                #endregion Lab Test

                #region Results
                if (ds.ResultDetail.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtTempResultDetail = ds.ResultDetail.GetChanges();
                        obj.InsertUpdateResults_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Results: " + ex.Message + ", ";
                    }
                }
                #endregion Results

                #region Implantable Device
                if (ds.MedicalDeviceEquipment.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtMedicalDeviceEquipment = ds.MedicalDeviceEquipment.GetChanges();
                        obj.InsertUpdateImplantableDevice_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Implantable Device: " + ex.Message + ", ";
                    }
                }
                #endregion Implantable Device

                #region Goals
                if (ds.EnrolledGoals.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtEnrolledGoals = ds.EnrolledGoals.GetChanges();
                        obj.InsertUpdateGoals_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Goals: " + ex.Message + ", ";
                    }
                }
                #endregion Goals

                #region Care Plan Goals
                if (ds.CarePlanGoals.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtCarePlanGoals = ds.CarePlanGoals.GetChanges();
                        obj.InsertUpdateCarePlanGoals_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Care Plan - Goals: " + ex.Message + ", ";
                    }
                }
                #endregion Care Plan Goals

                #region Health Concerns
                if (ds.HealthConcerns.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtHealthConcerns = ds.HealthConcerns.GetChanges();
                        obj.InsertUpdateHealthConcerns_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Health Concerns: " + ex.Message + ", ";
                    }
                }
                #endregion Health Concerns

                #region Interventions
                if (ds.Interventions.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtInterventions = ds.Interventions.GetChanges();
                        obj.InsertUpdateCarePlanIntervention_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Interventions: " + ex.Message + ", ";
                    }
                }
                #endregion Interventions

                #region Health Status Evaluations/Outcomes Section
                if (ds.HealthStatus.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtHealthStatus = ds.HealthStatus.GetChanges();
                        obj.InsertUpdateCarePlanOutcome_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Health Status Evaluations/Outcomes: " + ex.Message + ", ";
                    }
                }
                #endregion Health Status Evaluations/Outcomes Section

                #region Cognitive, Functional , Mental Status
                if (ds.FunctionalStatus.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtFunctionalStatus = ds.FunctionalStatus.GetChanges();
                        obj.InsertUpdateCognitiveStatus_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Cognitive, Functional , Mental Status: " + ex.Message + ", ";
                    }
                }
                #endregion Cognitive, Functional , Mental Status

                #region Reason For Referral
                if (ds.ReasonForReferral.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtReasonForReferral = ds.ReasonForReferral.GetChanges();
                        obj.InsertUpdateReferralReason_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Reason For Referral: " + ex.Message + ", ";
                    }
                }
                #endregion Reason For Referral 

                #region Insurance Provider
                if (ds.InsuranceProvider.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtInsuranceProvider = ds.InsuranceProvider.GetChanges();
                        obj.InsertUpdateInsuranceProvider_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Insurance Provider: " + ex.Message + ", ";
                    }
                }
                #endregion Insurance Provider 

                #region CARE Team
                if (ds.CareTeamMembers.Rows.Count > 0)
                {
                    try
                    {
                        DataTable dtCareTeamMembers = ds.CareTeamMembers.GetChanges();
                        obj.InsertUpdate_CareTeamMembers_CCDAData(ds);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                        errors += "Care Team Membersr: " + ex.Message + ", ";
                    }
                }
                #endregion CARE Team 


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportCCDAData", ex);
                throw ex;
            }


            return errors;
        }

        #region Encounter

        public void ImportEncounter_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateEncounter_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportEncounter_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Encounter

        #region Lab Test

        public void ImportLabTest_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateLabTest_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportLabTest_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Lab Test

        #region Medication

        public void ImportMedication_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateMedications_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportMedication_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Medication

        #region Immunization

        public void ImportImmunization_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateImmunization_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportImmunization_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Immunization

        #region Allergies

        public void ImportAllergies_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateAllergies_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportAllergies_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Allergies

        #region Procedures

        public void ImportProcedures_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateProcedures_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportProcedures_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Procedures

        #region Vital Signs

        public void ImportVitalSigns_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateVitalSigns_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportVitalSigns_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Vital Signs

        #region Problems List

        public void ImportProblemsList_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateProblemList_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportProblemsList_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Problems List

        #region Social History

        public void ImportSocialHistory_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateSocialHistory_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportSocialHistory_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Social History

        #region Results

        public void ImportResults_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateResults_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportResults_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Results

        #region Implantable Device

        public void ImportImplantableDevices_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateImplantableDevice_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportImplantableDevices_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Implantable Device

        #region Goals

        public void ImportGoals_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateGoals_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportGoals_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Goals

        #region Health Concerns

        public void ImportHealthConcernss_CCDAData(DSCCDA ds)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                obj.InsertUpdateHealthConcerns_CCDAData(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::ImportHealthConcernss_CCDAData", ex);
                throw ex;
            }
        }

        #endregion Health Concerns

        #endregion CCDS

        public BLObject<DSPatient> LoadPatient(DSPatient ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            try
            {
                ds = new DALCCDA().LoadPatient(ds);
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Search, DALUsersActivity.TITLE_SEARCH_PATIENT, true, "Patient search");
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Search, DALUsersActivity.TITLE_SEARCH_PATIENT, false, "Error during patient search : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLCCDA::LoadPatient", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSCCDA> InsertUpdateEncounter(DSCCDA ds)
        {

            try
            {
                ds = new DALCCDA().InsertUpdateEncounter(ds);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateEncounter", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }

        public BLObject<DSCCDA> InsertMUSetting(long patientId, long providerId, long facilityId, long noteId, bool lstMedication, bool lstProblems, bool lstAllergies,bool IsPatientEducation,bool IsTOC, long IsTOCDelivered, long TOCId, bool IsSummaryOfCare)
        {

            try
            {
                DSCCDA ds = new DSCCDA();
                ds = new DALCCDA().InsertMUSetting( patientId, providerId, facilityId, noteId, lstMedication,  lstProblems,  lstAllergies, IsPatientEducation, IsTOC, IsTOCDelivered, TOCId, IsSummaryOfCare);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateEncounter", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }

        public string InsertUpdatePatientInsurance(long insurancePlanId, long patientId)
        {

            try
            {
                string temp = new DALCCDA().insertUpdatePatientInsurance(insurancePlanId, patientId);
                return "success";
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateEncounter", ex);
                return "error";
            }
        }

        public BLObject<DSCCDA> InsertUpdateProcedure(DSCCDA ds)
        {

            try
            {
                ds = new DALCCDA().InsertUpdateProcedure(ds);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateProcedure", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }

        public BLObject<DSCCDA> InsertUpdateDiagnose(DSCCDA ds)
        {

            try
            {
                ds = new DALCCDA().InsertUpdateDiagnosis(ds);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateDiagnose", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }

        public BLObject<DSCCDA> InsertUpdateIntervention(DSCCDA ds)
        {

            try
            {
                ds = new DALCCDA().InsertUpdateIntervention(ds);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateIntervention", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }
        public BLObject<DSCCDA> InsertUpdateImmunization(DSCCDA ds)
        {

            try
            {
                ds = new DALCCDA().InsertUpdateImmunization(ds);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateImmunization", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }
        public BLObject<DSCCDA> InsertUpdateMedication(DSCCDA ds)
        {

            try
            {
                ds = new DALCCDA().InsertUpdateMedication(ds);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateMedication", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }

        public BLObject<DSCCDA> InsertUpdateCommProviderToProvider(DSCCDA ds)
        {

            try
            {
                ds = new DALCCDA().InsertUpdateCommProviderToProvider(ds);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateCommProviderToProvider", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }

        public BLObject<DSCCDA> InsertUpdateCommPatientToProvider(DSCCDA ds)
        {

            try
            {
                ds = new DALCCDA().InsertUpdateCommPatientToProvider(ds);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateCommPatientToProvider", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }
        public BLObject<DSCCDA> InsertUpdatePatientCharacteristics(DSCCDA ds)
        {

            try
            {
                ds = new DALCCDA().InsertUpdatePatientCharacteristics(ds);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdatePatientCharacteristics", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }

        public BLObject<DSCCDA> InsertUpdateDiagnosticStudy(DSCCDA ds)
        {

            try
            {
                ds = new DALCCDA().InsertUpdateDiagnosticStudy(ds);
                return new BLObject<DSCCDA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertUpdateDiagnosticStudy", ex);
                return new BLObject<DSCCDA>(null, ex.Message);
            }
        }
        public string InsertLabData(long PatientId, DateTime EffectiveTimeLow, DateTime EffectiveTimeHigh, string StatusCode, string Text, string Code,
                                    string CodeDescripion, string ResultValue, string ResultUnit, string ResultRange, string ActionPerformed)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                string status = obj.InsertLabData(PatientId, EffectiveTimeLow, EffectiveTimeHigh, StatusCode, Text, Code, CodeDescripion,
                                               ResultValue, ResultUnit, ResultRange, ActionPerformed);
                return status;
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLCCDA::InsertLabData", ex);
                return ex.Message;
            }
        }

        public string InsertPEData(long PatientId, DateTime EffectiveTimeLow, DateTime EffectiveTimeHigh, string StatusCode, string Text, string Code,
                            string CodeDescripion, string ResultCode, string ResultCodeDescription, string ResultValue, string ResultUnit, string ActionPerformed, string originalText = "", string NegationValueset = null)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                string status = obj.InsertPhysicalExam(PatientId, EffectiveTimeLow, EffectiveTimeHigh, StatusCode, Text, Code, CodeDescripion,
                                              ResultCode, ResultCodeDescription, ResultValue, ResultUnit, ActionPerformed, originalText, NegationValueset);
                return status;
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLCCDA::InsertLabData", ex);
                return ex.Message;
            }
        }

        public string InsertUpdateRiskAssessment(long PatientId, DateTime StartDate, DateTime EndDate, string StatusCode, string Text, string CPTCode, string SNOMEDID,
                            string CodeType, string ResultCode, string ResultCodeDescription, bool negationIndex, string negationReason, string NegationValueset = null, string PHQScore = null)
        {
            DALCCDA obj = new DALCCDA();
            try
            {
                string status = obj.InsertUpdateRiskAssessment(PatientId, StartDate, EndDate, StatusCode, Text, CPTCode, SNOMEDID, CodeType,
                                              ResultCode, ResultCodeDescription, negationIndex, negationReason, NegationValueset, PHQScore);
                return "";
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLCCDA::InsertLabData", ex);
                return ex.Message;
            }
        }

        public BLObject<DSPatient> InsertPrivacySegmentedDocument(DSPatient ds)
        {

            try
            {
                ds = new DALCCDA().InsertPrivacySegmentedDocument(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::InsertPrivacySegmentedDocument", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> LoadPrivacySegmentedDocument(long Id, long EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {

            try
            {
                DSPatient ds = new DALCCDA().LoadPrivacySegmentedDocument(Id, EntityId, PageNumber, RowsPerPage);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::LoadPrivacySegmentedDocument", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public string GetIsDataPrivacy()
        {
            try
            {
                string IsDataPrivacy = new DALCCDA().GetIsDataPrivacy();
                return IsDataPrivacy;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCCDA::GetIsDataPrivacy", ex);
                return ex.Message;
            }
        }

    }
}
