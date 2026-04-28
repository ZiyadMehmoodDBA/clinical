using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.IEHR.EMR.Model.Clinical;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using MDVision.IEHR.Common;
using System.IO;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.Medical.Implantable;
using MDVision.Model.Clinical.Medical.CarePlan;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Summary
{
    public class ClinicalSummaryHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public ClinicalSummaryHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static ClinicalSummaryHelper _instance = null;

        public static ClinicalSummaryHelper Instance()
        {
            if (_instance == null)
                _instance = new ClinicalSummaryHelper();
            return _instance;
        }

        // Author:  Farooq Ahmad
        // Created Date: 11/04/2016
        //OverView: Load Clinical Summary XML Data
        public string loadClinicalSummaryXMLData(Int64 noteId, Int64 providerId, Int64 patientId, ClinicalSummaryModel model, string StreamType, CCDAGenrator.DocumentTemplateType documentType, Int64 referralProvider = 0, string referralReason = "",
            Dictionary<string, object> ReportsParamaters = null, bool isEncryptionRequired = true)
        {
            try
            {
                DSNotes dsClinicalSummaryHTMLData = new DSNotes();
                DSProfile dsProvider = new DSProfile();
                DSPatient dsPatient = new DSPatient();
                DSProblemLists dsProblems = new DSProblemLists();
                DSProcedures dsProcedures = new DSProcedures();
                DSVitals dsVitals = new DSVitals();
                DSAllergies dsAllergies = new DSAllergies();
                DSSocialHistory dsSocialHx = new DSSocialHistory();
                DSMedicalHx dsMedicalHx = new DSMedicalHx();
                DSSurgicalHx dsSurgicalHx = new DSSurgicalHx();
                DSHospitalizationHx dsHospitalizationHx = new DSHospitalizationHx();
                DSFamilyHx dsFamilyHx = new DSFamilyHx();
                DSBirthHistory dsBirthHx = new DSBirthHistory();
                DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
                DSClinicalMedication dsMedication = new DSClinicalMedication();
                DSImmunization dsImmunization = new DSImmunization();
                DSClinicalSummary dsPlanOfCare = new DSClinicalSummary();
                DSLabResult dsLabResult = new DSLabResult();
                DSRadiologyResult dsRadiologyResult = new DSRadiologyResult();
                DSPatientReferral dsPatientReferral = new DSPatientReferral();
                Dictionary<int, string> lstComopent = new Dictionary<int, string>();
                DSLabOrder dsLabOrder = new DSLabOrder();
                List<ImplantableDevices> devicesList = new List<ImplantableDevices>();
                DSCCM dsCCM = new DSCCM();
                List<CarePlanGoalsModel> goalsList = null;
                List<CarePlanHealthConcernsModel> healthConsernsList = null;
                List<CarePlanInterventionsModel> interventionsList = null;
                List<CarePlanOutcomesModel> outComesList = null;
                if (model.Components != null)
                {
                    foreach (Component lst in model.Components)
                    {
                        try
                        {
                            lstComopent.Add(lst.componentId, lst.componentName);
                            if (documentType == CCDAGenrator.DocumentTemplateType.ClinicalSummary && !lstComopent.ContainsValue("refferral"))
                            {
                                lstComopent.Add((model.Components.Count + 2) * -1, "refferral");
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
                bool loadSexual = documentType == CCDAGenrator.DocumentTemplateType.HealthCareSurveys ? true : false;

                BLObject<DSNotes> dsNotesData = BLLClinicalObj.loadClinicalSummaryHTMLData(patientId, providerId, noteId, lstComopent, referralProvider, ReportsParamaters, loadSexual);

                dsClinicalSummaryHTMLData = dsNotesData.Data;
                var mdEqup = lstComopent.Where(a => a.Value.ToLower().IndexOf("medicalequipment") > -1).Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(mdEqup))
                {
                    try
                    {
                        BLObject<List<ImplantableDevices>> obj = BLLClinicalObj.loadImplantableDeviceForCCDA(MDVUtility.ToInt64(model.PatientId), noteId);
                        if (obj.Data != null)
                        {
                            devicesList = obj.Data;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                var goals = lstComopent.Where(a => a.Value.ToLower().IndexOf("planofcare") > -1).Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(goals))
                {
                    try
                    {
                        if (documentType == CCDAGenrator.DocumentTemplateType.CarePlan)
                            goalsList = BLLClinicalObj.LoadCarePlanGoal_CCDA(MDVUtility.ToInt64(model.PatientId), 0);
                        else
                            goalsList = BLLClinicalObj.LoadCarePlanGoal_CCDA(MDVUtility.ToInt64(model.PatientId), noteId);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                var healthconcern = lstComopent.Where(a => a.Value.ToLower().IndexOf("healthconcern") > -1).Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(healthconcern))
                {
                    try
                    {
                        if (documentType == CCDAGenrator.DocumentTemplateType.CarePlan)
                            healthConsernsList = BLLClinicalObj.LoadHealthConcernCCDA(MDVUtility.ToInt64(model.PatientId), 0);
                        else
                            healthConsernsList = BLLClinicalObj.LoadHealthConcernCCDA(MDVUtility.ToInt64(model.PatientId), noteId);

                    }
                    catch (Exception ex)
                    {
                    }
                }
                var interventions = lstComopent.Where(a => a.Value.ToLower().IndexOf("interventions") > -1).Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(interventions))
                {
                    try
                    {
                        interventionsList = BLLClinicalObj.LoadInterventionsCCDA(MDVUtility.ToInt64(model.PatientId), noteId);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                var carePlanOutcomes = lstComopent.Where(a => a.Value.ToLower().IndexOf("careplanoutcomes") > -1).Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(carePlanOutcomes))
                {
                    try
                    {
                        outComesList = BLLClinicalObj.LoadOutcomesCCDA(MDVUtility.ToInt64(model.PatientId), noteId);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (dsNotesData.Data != null || documentType == CCDAGenrator.DocumentTemplateType.AROReport || documentType == CCDAGenrator.DocumentTemplateType.AUPReport)
                {
                    DateTime dtNoteDate = DateTime.Now;
                    if (dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows.Count > 0)
                    {
                        dtNoteDate = MDVUtility.ToDateTime(dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows[0][dsClinicalSummaryHTMLData.Notes.VisitDateColumn.ColumnName]);
                    }
                    #region dsNotesData
                    List<Dictionary<string, string>> lstPatientData = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstProcedure = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstProviderData = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstPracticeData = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstNoteData = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstProblems = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstProblemsCancer = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstVitals = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstAllergs = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstSocials = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstMedicalHx = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstSurgicalHx = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstHospitalizationHx = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstFamilyHx = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstBirthHx = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstPhysicalExam = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstMedication = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstMedicationsAdministered = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstImmunization = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstPlanOfCare = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstscheduledProcedure = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstFutureAppointment = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstInstructionsAndDecisionAids = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstGoal = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstResults = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstPartialResultPending = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstRefferalProviderData = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstCognitiveFunctionalStatus = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstFutureResults = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstReasonReferral = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstEncounterDiagnosicDatakeyValues = new List<Dictionary<string, string>>();
                    List<string> lstEncounterProblemId = new List<string>();
                    List<Dictionary<string, string>> lstReasonForVisit = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstLabOrderTests = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstImplantableDevice = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstRaceCodes = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstEthnicityCodes = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstCaregivers = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstCareManagers = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstCareCoordinators = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstMentalStatus = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstInsurance = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstCareTeamPCP = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstCareTeamProvider = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstHealthConsern = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstHealthObservation = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstPlanedMedications = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstHealthRisks = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstInterventions = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstOutcomes = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstAROOrganizm = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstAUP = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstChiefComplaints = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstOutPatientEncounter = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstAROAntimicrobial = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstAROObservations = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstEmploymentHx = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstRadiologyResults = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstPatientParticipant = new List<Dictionary<string, string>>();
                    string practiceName = "";
                    string providerFullName = "";
                    string providerOfficeAddress = "";
                    string providerOfficePhone = "";

                    #region Patient Data
                    var ReasonForVisit = string.Empty;
                    try
                    {
                        if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ComplaintDetail.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ComplaintDetail.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ComplaintDetail.TableName].Rows)
                            {
                                var DatakeyValues = new Dictionary<string, string>
                                {
                                    { "Description",  MDVUtility.ToStr(dr[dsPlanOfCare.ComplaintDetail.ComplaintDescriptionColumn.ColumnName])},
                                    { "SNOMEDID",  MDVUtility.ToStr(dr[dsPlanOfCare.ComplaintDetail.SNOMEDIDColumn.ColumnName])},
                                    { "SNOMEDDescription",  MDVUtility.ToStr(dr[dsPlanOfCare.ComplaintDetail.SNOMEDDescriptionColumn.ColumnName])},
                                    { "ICD10Code",  MDVUtility.ToStr(dr[dsPlanOfCare.ComplaintDetail.ICD10CodeColumn.ColumnName])},
                                    { "ICD10CodeDescription",  MDVUtility.ToStr(dr[dsPlanOfCare.ComplaintDetail.ICD10CodeDescriptionColumn.ColumnName])},
                                    { "CreatedOn", MDVUtility.ToStr(dr[dsPlanOfCare.ComplaintDetail.CreatedOnColumn.ColumnName])},

                                 };
                                lstReasonForVisit.Add(DatakeyValues);
                            }

                        }
                    }
                    catch (Exception ex) { }
                    #region Patient Races

                    if (dsClinicalSummaryHTMLData.Tables[dsPatient.PatientRace.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPatient.PatientRace.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsClinicalSummaryHTMLData.Tables[dsPatient.PatientRace.TableName].Rows)
                        {
                            var PatientRaceDatakeyValues = new Dictionary<string, string>
                            {
                                { "RaceCode",  MDVUtility.ToStr(drRace[dsPatient.PatientRace.CodeColumn.ColumnName])},
                                { "RaceName",  MDVUtility.ToStr(drRace[dsPatient.PatientRace.DescriptionColumn.ColumnName])},
                                { "ParentCode",  MDVUtility.ToStr(drRace[dsPatient.PatientRace.ParentCodeColumn.ColumnName])},
                                { "PatentRaceName",  MDVUtility.ToStr(drRace[dsPatient.PatientRace.ParentDecriptionColumn.ColumnName])},
                             };
                            lstRaceCodes.Add(PatientRaceDatakeyValues);
                        }

                    }
                    #endregion Patient Races

                    #region Patient Ethnicity

                    if (dsClinicalSummaryHTMLData.Tables[dsPatient.PatientEthnicity.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPatient.PatientEthnicity.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsClinicalSummaryHTMLData.Tables[dsPatient.PatientEthnicity.TableName].Rows)
                        {
                            var PatientRaceDatakeyValues = new Dictionary<string, string>
                            {
                                { "EthnicityCode",  MDVUtility.ToStr(drRace[dsPatient.PatientEthnicity.CodeColumn.ColumnName])},
                                { "EthnicityName",  MDVUtility.ToStr(drRace[dsPatient.PatientEthnicity.DescriptionColumn.ColumnName])},
                                { "ParentCode",  MDVUtility.ToStr(drRace[dsPatient.PatientEthnicity.ParentCodeColumn.ColumnName])},
                                { "PatentEthnicityName",  MDVUtility.ToStr(drRace[dsPatient.PatientEthnicity.ParentDecriptionColumn.ColumnName])},
                             };
                            lstEthnicityCodes.Add(PatientRaceDatakeyValues);
                        }

                    }
                    #endregion Patient Ethnicity

                    #endregion

                    #region CareTeam
                    /*Care Givers*/
                    if (dsClinicalSummaryHTMLData.Tables[dsCCM.CareGivers.TableName] != null &&
                        dsClinicalSummaryHTMLData.Tables[dsCCM.CareGivers.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsClinicalSummaryHTMLData.Tables[dsCCM.CareGivers.TableName].Rows)
                        {
                            var careTeamDataKeyeValues = new Dictionary<string, string>
                            {
                                { "LastName",  MDVUtility.ToStr(drRace[dsCCM.CareGivers.LastNameColumn.ColumnName])},
                                { "FirstName",  MDVUtility.ToStr(drRace[dsCCM.CareGivers.FirstNameColumn.ColumnName])},
                                { "PhoneNo",  MDVUtility.ToStr(drRace[dsCCM.CareGivers.PhoneNoColumn.ColumnName])},
                                { "PhoneExt",  MDVUtility.ToStr(drRace[dsCCM.CareGivers.PhoneExtColumn.ColumnName])},
                             };
                            lstCaregivers.Add(careTeamDataKeyeValues);
                        }

                    }
                    /*Care Managers*/
                    if (dsClinicalSummaryHTMLData.Tables[dsCCM.CareManagers.TableName] != null &&
                        dsClinicalSummaryHTMLData.Tables[dsCCM.CareManagers.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsClinicalSummaryHTMLData.Tables[dsCCM.CareManagers.TableName].Rows)
                        {
                            var careTeamDataKeyeValues = new Dictionary<string, string>
                            {
                                { "LastName",  MDVUtility.ToStr(drRace[dsCCM.CareManagers.LastNameColumn.ColumnName])},
                                { "FirstName",  MDVUtility.ToStr(drRace[dsCCM.CareManagers.FirstNameColumn.ColumnName])},
                                { "PhoneNo",  MDVUtility.ToStr(drRace[dsCCM.CareManagers.PhoneNoColumn.ColumnName])},
                                { "PhoneExt",  MDVUtility.ToStr(drRace[dsCCM.CareManagers.PhoneExtColumn.ColumnName])},
                             };
                            lstCareManagers.Add(careTeamDataKeyeValues);
                        }

                    }
                    /*Care Coordinators*/
                    if (dsClinicalSummaryHTMLData.Tables[dsCCM.CareCoordinators.TableName] != null &&
                        dsClinicalSummaryHTMLData.Tables[dsCCM.CareCoordinators.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsClinicalSummaryHTMLData.Tables[dsCCM.CareCoordinators.TableName].Rows)
                        {
                            var careTeamDataKeyeValues = new Dictionary<string, string>
                            {
                                { "LastName",  MDVUtility.ToStr(drRace[dsCCM.CareCoordinators.LastNameColumn.ColumnName])},
                                { "FirstName",  MDVUtility.ToStr(drRace[dsCCM.CareCoordinators.FirstNameColumn.ColumnName])},
                                { "PhoneNo",  MDVUtility.ToStr(drRace[dsCCM.CareCoordinators.PhoneNoColumn.ColumnName])},
                                { "PhoneExt",  MDVUtility.ToStr(drRace[dsCCM.CareCoordinators.PhoneExtColumn.ColumnName])},
                             };
                            lstCareCoordinators.Add(careTeamDataKeyeValues);
                        }

                    }
                    /*Care Team PCP*/
                    if (dsClinicalSummaryHTMLData.Tables[dsCCM.CareTeamPCP.TableName] != null &&
                        dsClinicalSummaryHTMLData.Tables[dsCCM.CareTeamPCP.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsClinicalSummaryHTMLData.Tables[dsCCM.CareTeamPCP.TableName].Rows)
                        {
                            var careTeamDataKeyeValues = new Dictionary<string, string>
                            {
                                { "LastName",  MDVUtility.ToStr(drRace[dsCCM.CareTeamPCP.LastNameColumn.ColumnName])},
                                { "FirstName",  MDVUtility.ToStr(drRace[dsCCM.CareTeamPCP.FirstNameColumn.ColumnName])},
                                { "MI",  MDVUtility.ToStr(drRace[dsCCM.CareTeamPCP.MIColumn.ColumnName])},
                                { "PhoneNo",  MDVUtility.ToStr(drRace[dsCCM.CareTeamPCP.PhoneNoColumn.ColumnName])},
                                { "PhoneExt",  MDVUtility.ToStr(drRace[dsCCM.CareTeamPCP.PhoneExtColumn.ColumnName])},
                             };
                            lstCareTeamPCP.Add(careTeamDataKeyeValues);
                        }

                    }
                    /*Care Team Provider*/
                    if (dsClinicalSummaryHTMLData.Tables[dsCCM.CareTeamProvider.TableName] != null &&
                        dsClinicalSummaryHTMLData.Tables[dsCCM.CareTeamProvider.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsClinicalSummaryHTMLData.Tables[dsCCM.CareTeamProvider.TableName].Rows)
                        {
                            var careTeamDataKeyeValues = new Dictionary<string, string>
                            {
                                { "LastName",  MDVUtility.ToStr(drRace[dsCCM.CareTeamProvider.LastNameColumn.ColumnName])},
                                { "FirstName",  MDVUtility.ToStr(drRace[dsCCM.CareTeamProvider.FirstNameColumn.ColumnName])},
                                { "MI",  MDVUtility.ToStr(drRace[dsCCM.CareTeamProvider.MIColumn.ColumnName])},
                                { "PhoneNo",  MDVUtility.ToStr(drRace[dsCCM.CareTeamProvider.PhoneNoColumn.ColumnName])},
                                { "PhoneExt",  MDVUtility.ToStr(drRace[dsCCM.CareTeamProvider.PhoneExtColumn.ColumnName])},
                             };
                            lstCareTeamProvider.Add(careTeamDataKeyeValues);
                        }

                    }
                    #endregion CareTeam

                    #region Patient Data

                    if (dsClinicalSummaryHTMLData.Tables[dsPatient.Patients.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        DataRow drPatient = dsClinicalSummaryHTMLData.Tables[dsPatient.Patients.TableName].Rows[dsClinicalSummaryHTMLData.Tables[dsPatient.Patients.TableName].Rows.Count - 1];
                        practiceName = MDVUtility.ToStr(drPatient[dsPatient.Patients.PracticeNameColumn.ColumnName]);
                        var PatientDatakeyValues = new Dictionary<string, string>
                        {
                            { "PatientName",  MDVUtility.ToStr(drPatient[dsPatient.Patients.FullNameColumn.ColumnName])},
                            { "PatientDOB", String.IsNullOrEmpty(MDVUtility.ToStr(drPatient[dsPatient.Patients.DOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(drPatient[dsPatient.Patients.DOBColumn.ColumnName]).ToShortDateString()},
                            { "PatientGender", MDVUtility.ToStr(drPatient[dsPatient.Patients.GenderColumn.ColumnName])},
                            { "PatientRace", MDVUtility.ToStr(drPatient["RaceName"])},
                            { "PatientRaceCode", MDVUtility.ToStr(drPatient["RaceCode"])},
                            { "PatientEthnicity", MDVUtility.ToStr(drPatient["EthnicityName"])},
                            { "PatientEthnicityCode", MDVUtility.ToStr(drPatient["EthnicityCode"])},
                            { "PatientLanguageCode", MDVUtility.ToStr(drPatient["LanguageCode"])},
                            { "PatientContactInfo", "Primary Home:" + MDVUtility.ToStr(drPatient[dsPatient.Patients.HomePhoneNoColumn.ColumnName]) + "<br>" + MDVUtility.ToStr(drPatient[dsPatient.Patients.Address1Column.ColumnName])+"<br>"+MDVUtility.ToStr(drPatient[dsPatient.Patients.CellNoColumn.ColumnName])},
                            { "PatientIDs", MDVUtility.ToStr(drPatient[dsPatient.Patients.AccountNumberColumn.ColumnName])},
                            { "PatientSSN", MDVUtility.ToStr(drPatient[dsPatient.Patients.SSNColumn.ColumnName])},
                            { "PatientStreetAddress", MDVUtility.ToStr(drPatient[dsPatient.Patients.Address1Column.ColumnName])},
                            { "PatientCity", MDVUtility.ToStr(drPatient[dsPatient.Patients.CityColumn.ColumnName])},
                            { "PatientState", MDVUtility.ToStr(drPatient[dsPatient.Patients.StateColumn.ColumnName])},
                            { "PatientZIPCode", MDVUtility.ToStr(drPatient[dsPatient.Patients.ZIPCodeColumn.ColumnName])},
                            { "PatientCountry", MDVUtility.ToStr(drPatient[dsPatient.Patients.Address2Column.ColumnName])},
                            { "PatientCellNo", MDVUtility.ToStr(drPatient[dsPatient.Patients.CellNoColumn.ColumnName])},
                            { "MaritialStatus", MDVUtility.ToStr(drPatient[dsPatient.Patients.MaritialStatusColumn.ColumnName])},
                            { "PrefCommunicationId", MDVUtility.ToStr(drPatient[dsPatient.Patients.PrefCommunicationIdColumn.ColumnName])},
                            { "PrefCommunicationName", MDVUtility.ToStr(drPatient["PrefCommunicationName"]) },
                            { "MiddleInitial", MDVUtility.ToStr(drPatient[dsPatient.Patients.MIColumn.ColumnName])},
                            { "PatientHomePhoneNo", MDVUtility.ToStr(drPatient[dsPatient.Patients.HomePhoneNoColumn.ColumnName])},
                            { "PatientWorkPhoneNo", MDVUtility.ToStr(drPatient[dsPatient.Patients.WorkPhoneNoColumn.ColumnName])},
                            { "PatientWorkPhoneExt", MDVUtility.ToStr(drPatient[dsPatient.Patients.WorkPhoneExtColumn.ColumnName])},
                            { "MI", MDVUtility.ToStr(drPatient[dsPatient.Patients.MIColumn.ColumnName])},
                            { "FirstName", MDVUtility.ToStr(drPatient[dsPatient.Patients.FirstNameColumn.ColumnName])},
                            { "LastName", MDVUtility.ToStr(drPatient[dsPatient.Patients.LastNameColumn.ColumnName])},
                            { "ReferringProviderName", MDVUtility.ToStr(drPatient[dsPatient.Patients.ReferringProviderNameColumn.ColumnName])},
                            { "ReferringProviderNPI", MDVUtility.ToStr(drPatient["ReferringProviderNPI"])},
                            { "ReferringFirstName", MDVUtility.ToStr(drPatient["ReferringFirstName"])},
                            { "ReferringLastName", MDVUtility.ToStr(drPatient["ReferringLastName"])},
                            { "ReferringMI", MDVUtility.ToStr(drPatient["ReferringMI"])},
                            { "ReferrringPhoneNo", MDVUtility.ToStr(drPatient["ReferrringPhoneNo"])},
                            { "ReferringAddress", MDVUtility.ToStr(drPatient["ReferringAddress"])},
                            { "ReferringCity", MDVUtility.ToStr(drPatient["ReferringCity"])},
                            { "ReferringState", MDVUtility.ToStr(drPatient["ReferringState"])},
                            { "ReferringZipCode", MDVUtility.ToStr(drPatient["ReferringZipCode"])},
                            { "PCPName", MDVUtility.ToStr(drPatient["PCPName"])},
                            { "FacilityName", MDVUtility.ToStr(drPatient[dsPatient.Patients.FacilityNameColumn.ColumnName])},
                            { "Suffix", MDVUtility.ToStr(drPatient[dsPatient.Patients.SuffixColumn.ColumnName])},
                            { "PreviousName", MDVUtility.ToStr(drPatient[dsPatient.Patients.PreviousNameColumn.ColumnName])},
                            { "Prefix", MDVUtility.ToStr(drPatient[dsPatient.Patients.PrefixColumn.ColumnName])},
                            { "BirthSex", MDVUtility.ToStr(drPatient[dsPatient.Patients.BirthSexColumn.ColumnName])},
                            { "MRNumber", MDVUtility.ToStr(drPatient[dsPatient.Patients.MRNumberColumn.ColumnName])},
                            { "FacilityId", MDVUtility.ToStr(drPatient[dsPatient.Patients.FacilityIdColumn.ColumnName])},
                            { "FacilityLocation", MDVUtility.ToStr(drPatient[dsPatient.Patients.FacilityLocationColumn.ColumnName])},
                        };
                        lstPatientData.Add(PatientDatakeyValues);
                    }

                    #endregion

                    #region Provider Data

                    if (dsClinicalSummaryHTMLData.Tables[dsProvider.Provider.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsProvider.Provider.TableName].Rows.Count > 0)
                    {
                        DataRow drProvider = dsClinicalSummaryHTMLData.Tables[dsProvider.Provider.TableName].Rows[dsClinicalSummaryHTMLData.Tables[dsProvider.Provider.TableName].Rows.Count - 1];
                        providerFullName = MDVUtility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName]) + ", " + MDVUtility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName]);
                        providerOfficeAddress = MDVUtility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName]);
                        providerOfficePhone = MDVUtility.ToStr(drProvider[dsProvider.Provider.PhoneNoColumn.ColumnName]);
                        var ProviderDatakeyValues = new Dictionary<string, string>
                        {
                            { "ProviderNPI", MDVUtility.ToStr(drProvider[dsProvider.Provider.NPIColumn.ColumnName])},
                            { "FirstName", MDVUtility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName])},
                            { "LastName", MDVUtility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName])},
                            { "MiddleInitial", MDVUtility.ToStr(drProvider[dsProvider.Provider.MiddleInitialColumn.ColumnName])},
                            { "MI", MDVUtility.ToStr(drProvider[dsProvider.Provider.MiddleInitialColumn.ColumnName])},
                            { "PhoneNo", MDVUtility.ToStr(drProvider[dsProvider.Provider.PhoneNoColumn.ColumnName])},
                            { "PhoneExt", MDVUtility.ToStr(drProvider[dsProvider.Provider.PhoneExtColumn.ColumnName])},
                            { "HomeAddress", MDVUtility.ToStr(drProvider[dsProvider.Provider.HomeAddressColumn.ColumnName])},
                            { "WorkAddress", MDVUtility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName])},
                            { "ZIPCode", MDVUtility.ToStr(drProvider[dsProvider.Provider.ZIPCodeColumn.ColumnName])},
                            { "State", MDVUtility.ToStr(drProvider[dsProvider.Provider.StateColumn.ColumnName])},
                            { "City", MDVUtility.ToStr(drProvider[dsProvider.Provider.CityColumn.ColumnName])},
                            {"SpecialtyName", MDVUtility.ToStr(drProvider["SpecialtyName"])},
                            { "Given", MDVUtility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName])},
                            { "Family", MDVUtility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName])},
                            { "DocumentCreated", providerFullName},
                            { "HealthcareService", practiceName},
                            { "Performer", providerFullName},
                            { "Author", providerFullName},
                            { "AuthorContactInfo", "Work Place: "+MDVUtility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName]) + "<br> Tel:" + MDVUtility.ToStr(drProvider[dsProvider.Provider.CellNoColumn.ColumnName])},
                        };


                        lstProviderData.Add(ProviderDatakeyValues);
                    }

                    #endregion

                    #region PracticeData
                    DSProfile dsProfile = new DSProfile();
                    if (dsClinicalSummaryHTMLData.Tables[dsProfile.Practice.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsProfile.Practice.TableName].Rows.Count > 0)
                    {
                        DataRow drPractice = dsClinicalSummaryHTMLData.Tables[dsProfile.Practice.TableName].Rows[dsClinicalSummaryHTMLData.Tables[dsProfile.Practice.TableName].Rows.Count - 1];
                        var PracticeDatakeyValues = new Dictionary<string, string>
                        {
                            { "PracticeNPI", MDVUtility.ToStr(drPractice[dsProfile.Practice.NPIColumn.ColumnName])},
                            { "Address",string.IsNullOrWhiteSpace( MDVUtility.ToStr(drPractice[dsProfile.Practice.AddressColumn.ColumnName]))?  MDVUtility.ToStr(drPractice[dsProfile.Practice.Address1Column.ColumnName]): MDVUtility.ToStr(drPractice[dsProfile.Practice.AddressColumn.ColumnName])},
                            { "City", MDVUtility.ToStr(drPractice[dsProfile.Practice.CityColumn.ColumnName])},
                            { "State", MDVUtility.ToStr(drPractice[dsProfile.Practice.StateColumn.ColumnName])},
                            { "ZIPCode", MDVUtility.ToStr(drPractice[dsProfile.Practice.ZIPCodeColumn.ColumnName])},
                            { "ShortName", MDVUtility.ToStr(drPractice[dsProfile.Practice.ShortNameColumn.ColumnName])},
                             { "PhoneNo", MDVUtility.ToStr(drPractice[dsProfile.Practice.PhoneNoColumn.ColumnName])},
                              { "PhoneExt", MDVUtility.ToStr(drPractice[dsProfile.Practice.PhoneExtColumn.ColumnName])},

                        };
                        lstPracticeData.Add(PracticeDatakeyValues);
                    }
                    #endregion

                    #region Note Data

                    if (dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows.Count > 0)
                    {
                        DataRow drNote = dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows[dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows.Count - 1];
                        var NoteDatakeyValues = new Dictionary<string, string>
                        {
                            { "EncounterId", MDVUtility.ToStr(drNote[dsClinicalSummaryHTMLData.Notes.NotesIdColumn.ColumnName])},
                            { "EncounterDate", String.IsNullOrEmpty(MDVUtility.ToStr(drNote[dsClinicalSummaryHTMLData.Notes.VisitDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(drNote[dsClinicalSummaryHTMLData.Notes.VisitDateColumn.ColumnName]).ToShortDateString()},
                            { "EncounterLocation", practiceName},
                            { "ResponsibleParty", providerFullName},
                            { "ResponsiblePartyContactInfo", "Work Place: "+ providerOfficeAddress + "<br> Tel:" + providerOfficePhone},
                            { "EnteredBy", providerFullName},
                            { "EnteredByContactInfo", "Work Place: "+ providerOfficeAddress + "<br> Tel:" + providerOfficePhone},
                            { "Informant", providerFullName},
                            { "InformantContactInfo", "Work Place: "+ providerOfficeAddress + "<br> Tel:" + providerOfficePhone},
                            { "InformationRecipient", MDVUtility.ToStr(drNote["RefProviderName"])},
                            { "InformationRecipientContactInfo", MDVUtility.ToStr(drNote["RefProviderAddress"])},
                            { "LegalAuthenticator", providerFullName},
                            { "LegalAuthenticatorContactInfo", "Work Place: "+ providerOfficeAddress + "<br> Tel:" + providerOfficePhone},
                            { "DocumentMaintainedBy", practiceName},
                        };
                        lstNoteData.Add(NoteDatakeyValues);
                    }

                    #endregion

                    #region Vitals Signs Data

                    if (dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSigns.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSigns.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSigns.TableName].Rows)
                        {
                            string BPDiastolic = (dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows.Count > 0) ? MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows[0][dsVitals.VitalSignsBloodPressure.DiastolicColumn.ColumnName]) : string.Empty;
                            string BPSystolic = (dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows.Count > 0) ? MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows[0][dsVitals.VitalSignsBloodPressure.SystolicColumn.ColumnName]) : string.Empty;
                            var VitalsDatakeyValues = new Dictionary<string, string>
                            {
                                { "Height",  MDVUtility.ToStr(dr[dsVitals.VitalSigns.HeightColumn.ColumnName])},
                                { "Weight", MDVUtility.ToStr(dr[dsVitals.VitalSigns.WeightColumn.ColumnName])},
                                { "BMI", MDVUtility.ToStr(dr[dsVitals.VitalSigns.BMIColumn.ColumnName])},
                                { "BPSystolic", BPSystolic},
                                { "BPDiastolic", BPDiastolic},
                                { "Status", MDVUtility.ToStr(dr[dsVitals.VitalSigns.IsActiveColumn.ColumnName])},
                                { "createdOn", MDVUtility.ToStr(dr[dsVitals.VitalSigns.CreatedOnColumn.ColumnName])},
                                { "Pulse", MDVUtility.ToStr(dr[dsVitals.VitalSigns.PulseResultColumn.ColumnName]) },
                                { "SPO2", MDVUtility.ToStr(dr[dsVitals.VitalSigns.SPO2Column.ColumnName])},
                                { "Temprature", MDVUtility.ToStr(dr[dsVitals.VitalSigns.TemperatureResultColumn.ColumnName])},
                                { "RespiratoryRate", MDVUtility.ToStr(dr[dsVitals.VitalSigns.RespirationResultColumn.ColumnName])},
                                { "InhaledO2", MDVUtility.ToStr(dr[dsVitals.VitalSigns.InhaledO2ConcentrationColumn.ColumnName])},
                            };
                            lstVitals.Add(VitalsDatakeyValues);
                        }

                    }
                    else
                    {
                        if (dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows.Count > 0)
                        {

                            string BPDiastolic = MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.DiastolicColumn.ColumnName]);
                            string BPSystolic = MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.SystolicColumn.ColumnName]);
                            var VitalsDatakeyValues = new Dictionary<string, string>
                            {
                                { "Height",  MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.HeightColumn.ColumnName])},
                                { "Weight", MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.WeightColumn.ColumnName])},
                                { "BMI", MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.BMIColumn.ColumnName])},
                                { "BPSystolic", BPSystolic},
                                { "BPDiastolic", BPDiastolic},
                                { "Status", MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.IsActiveColumn.ColumnName])},
                                { "createdOn", MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.VitalSignDateColumn.ColumnName])},
                                   { "Pulse", MDVUtility.ToStr(MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSigns.PulseResultColumn.ColumnName])) },
                                { "SPO2", MDVUtility.ToStr( MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSigns.SPO2Column.ColumnName]))},
                                { "Temprature", MDVUtility.ToStr( MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSigns.TemperatureResultColumn.ColumnName]))},
                                { "RespiratoryRate", MDVUtility.ToStr( MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSigns.RespirationResultColumn.ColumnName]))},
                                { "InhaledO2", MDVUtility.ToStr( MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSigns.InhaledO2ConcentrationColumn.ColumnName]))},
                            };
                            lstVitals.Add(VitalsDatakeyValues);

                        }
                    }

                    #endregion

                    #region Allergies Data

                    if (dsClinicalSummaryHTMLData.Tables[dsAllergies.Allergy.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsAllergies.Allergy.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsAllergies.Allergy.TableName].Rows)
                        {
                            var Allergen = MDVUtility.ToStr(dr[dsAllergies.Allergy.AllergenColumn.ColumnName]).Replace("- RxNormId:" + MDVUtility.ToStr(dr[dsAllergies.Allergy.RxnormIDColumn.ColumnName]), "");
                            var AllergenWithRxnorm = string.Concat(Allergen, ", [RxNorm:", MDVUtility.ToStr(dr[dsAllergies.Allergy.RxnormIDColumn.ColumnName]), "]");
                            var ProblemDatakeyValues = new Dictionary<string, string>
                            {
                                { "Status", Convert.ToBoolean(dr[dsAllergies.Allergy.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                { "Reaction", MDVUtility.ToStr(dr[dsAllergies.Allergy.ReactionColumn.ColumnName]) },
                                { "Substance", AllergenWithRxnorm},
                                { "Allergen", Allergen},
                                { "RxNormID", MDVUtility.ToStr(dr[dsAllergies.Allergy.RxnormIDColumn.ColumnName])},
                                { "Severity",  MDVUtility.ToStr(dr[dsAllergies.Allergy.SeverityColumn.ColumnName])},
                                { "ResolvedDate",  MDVUtility.ToDateTime(dr[dsAllergies.Allergy.LastModifiedColumn.ColumnName]).ToShortDateString()},
                                { "CreatedOn",  MDVUtility.ToDateTime(dr[dsAllergies.Allergy.OnSetDateColumn.ColumnName]).ToShortDateString()},
                            };
                            lstAllergs.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region Medication Data

                    if (dsClinicalSummaryHTMLData.Tables["MedicationsAdministered"] != null && dsClinicalSummaryHTMLData.Tables["MedicationsAdministered"].Rows.Count > 0 && documentType != CCDAGenrator.DocumentTemplateType.ReferralSummary)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables["MedicationsAdministered"].Rows)
                        {
                            var strDrugDosage = "";
                            try
                            {
                                foreach (DataRow DR in dsClinicalSummaryHTMLData.Tables[dsMedication.Medication.TableName].Rows)
                                {
                                    int index = dsClinicalSummaryHTMLData.Tables[dsMedication.Medication.TableName].Rows.IndexOf(DR);
                                    if (DR["MedicationID"].ToString() == dr["MedicationID"].ToString())
                                    {
                                        dsClinicalSummaryHTMLData.Tables[dsMedication.Medication.TableName].Rows.RemoveAt(index);
                                        break;
                                    }
                                }
                                dsClinicalSummaryHTMLData.Tables[dsMedication.Medication.TableName].AcceptChanges();
                            }
                            catch (Exception ex) { }


                            if (MDVUtility.ToStr(dr[dsMedication.Medication.ActionColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsMedication.Medication.DoseUnitColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName]) != "")
                            {
                                strDrugDosage = MDVUtility.ToStr(dr[dsMedication.Medication.ActionColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseUnitColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName]);
                                if (!string.IsNullOrWhiteSpace(MDVUtility.ToStr(dr[dsMedication.Medication.DurationColumn.ColumnName])) && MDVUtility.ToInt32(dr[dsMedication.Medication.DurationColumn.ColumnName]) > 0)
                                {
                                    strDrugDosage += " for " + MDVUtility.ToStr(dr[dsMedication.Medication.DurationColumn.ColumnName]);
                                    if (MDVUtility.ToInt32(dr[dsMedication.Medication.DurationColumn.ColumnName]) > 1)
                                        strDrugDosage += " days";
                                    else
                                        strDrugDosage += " day";
                                }
                            }
                            else
                            {
                                strDrugDosage = MDVUtility.ToStr(dr[dsMedication.Medication.PatientNotesColumn.ColumnName]);
                            }

                            var MedicationName = MDVUtility.ToStr(dr[dsMedication.Medication.MedicationNameColumn.ColumnName]).Replace("- RxNormId:" + MDVUtility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName]), "");
                            var MedicationWithRxNorm = string.Concat(MedicationName, ", [RxNorm:", MDVUtility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName]), "]");

                            var MedicationDatakeyValues = new Dictionary<string, string>
                            {
                                { "Medication", MedicationWithRxNorm},
                                { "MedicationName", MedicationName},
                                { "Status", Convert.ToBoolean(dr[dsMedication.Medication.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                { "StartDate", MDVUtility.ToStr(dr[dsMedication.Medication.StartDateColumn.ColumnName])},
                                { "Indication", string.Empty},// Not in System
                                { "RouteCode", string.Empty},// Not in System
                                { "EndDate", MDVUtility.ToStr(dr[dsMedication.Medication.StopDateColumn.ColumnName])},
                                { "NoOfTimes",MDVUtility.ToStr(dr[dsMedication.Medication.QuantityColumn.ColumnName])},
                                { "RouteDescription",MDVUtility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName])},
                                { "Dose",MDVUtility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName])},
                                { "Refill",MDVUtility.ToStr(dr[dsMedication.Medication.RefillColumn.ColumnName])},
                                { "FrequencyDescription",MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName])},
                                { "Substitution", MDVUtility.ToStr(dr[dsMedication.Medication.SubstitutionColumn.ColumnName])},
                                { "Directions", strDrugDosage},
                                { "RxnormID", MDVUtility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName])},
                            };
                            lstMedicationsAdministered.Add(MedicationDatakeyValues);
                        }
                    }

                    if (dsClinicalSummaryHTMLData.Tables[dsMedication.Medication.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsMedication.Medication.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsMedication.Medication.TableName].Rows)
                        {
                            var strDrugTiming = "";
                            var strDrugDosage = "";
                            var strDrugAndDosage = "";
                            var MedicationName = MDVUtility.ToStr(dr[dsMedication.Medication.MedicationNameColumn.ColumnName]).Replace("- RxNormId:" + MDVUtility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName]), "");

                            if (MDVUtility.ToStr(dr[dsMedication.Medication.ActionColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsMedication.Medication.DoseUnitColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName]) != "")
                            {
                                strDrugDosage = MDVUtility.ToStr(dr[dsMedication.Medication.ActionColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseUnitColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName]);
                                if (!string.IsNullOrWhiteSpace(MDVUtility.ToStr(dr[dsMedication.Medication.DurationColumn.ColumnName])) && MDVUtility.ToInt32(dr[dsMedication.Medication.DurationColumn.ColumnName]) > 0)
                                {
                                    strDrugDosage += " for " + MDVUtility.ToStr(dr[dsMedication.Medication.DurationColumn.ColumnName]);
                                    if (MDVUtility.ToInt32(dr[dsMedication.Medication.DurationColumn.ColumnName]) > 1)
                                        strDrugDosage += " days";
                                    else
                                        strDrugDosage += " day";
                                }
                                strDrugAndDosage = MDVUtility.ToStr(dr[dsMedication.Medication.ActionColumn.ColumnName]) + " " + MedicationName + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseUnitColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName]);
                                if (!string.IsNullOrWhiteSpace(MDVUtility.ToStr(dr[dsMedication.Medication.DurationColumn.ColumnName])) && MDVUtility.ToInt32(dr[dsMedication.Medication.DurationColumn.ColumnName]) > 0)
                                {
                                    strDrugAndDosage += " for " + MDVUtility.ToStr(dr[dsMedication.Medication.DurationColumn.ColumnName]);
                                    if (MDVUtility.ToInt32(dr[dsMedication.Medication.DurationColumn.ColumnName]) > 1)
                                        strDrugAndDosage += " days";
                                    else
                                        strDrugAndDosage += " day";
                                }
                                strDrugTiming = (string.IsNullOrWhiteSpace(MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName])) ? "" : MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName])) + " " + (string.IsNullOrWhiteSpace(MDVUtility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName])) ? "" : MDVUtility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName]) + " ");
                            }
                            else
                            {
                                strDrugDosage = MDVUtility.ToStr(dr[dsMedication.Medication.PatientNotesColumn.ColumnName]);
                            }


                            var MedicationwithRxNorm = string.Concat(MedicationName, ", [RxNorm:", MDVUtility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName]), "]");
                            var MedicationDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Medication", MedicationwithRxNorm},
                                        { "MedicationName", MedicationName},
                                        { "Status", Convert.ToBoolean(dr[dsMedication.Medication.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                        { "StartDate", MDVUtility.ToStr(dr[dsMedication.Medication.StartDateColumn.ColumnName])},
                                        { "Indication", string.Empty},// Not in System
                                        { "RouteCode", string.Empty},// Not in System
                                        { "EndDate", MDVUtility.ToStr(dr[dsMedication.Medication.StopDateColumn.ColumnName])},
                                        { "NoOfTimes",MDVUtility.ToStr(dr[dsMedication.Medication.QuantityColumn.ColumnName])},
                                        { "RouteDescription",MDVUtility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName])},
                                        { "Dose",MDVUtility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName])},
                                        { "Refill",MDVUtility.ToStr(dr[dsMedication.Medication.RefillColumn.ColumnName])},
                                        { "FrequencyDescription",MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName])},
                                        { "Substitution", MDVUtility.ToStr(dr[dsMedication.Medication.SubstitutionColumn.ColumnName])},
                                        { "Directions", strDrugDosage},
                                        { "RxnormID", MDVUtility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName])},
                                        { "DrugTiming", strDrugTiming },
                                        { "CreatedDate", MDVUtility.ToStr(dr[dsMedication.Medication.CreatedOnColumn.ColumnName])},
                                        { "DrugAndDosage", strDrugAndDosage},
                                    };

                            if (!string.IsNullOrWhiteSpace(MDVUtility.ToStr(dr[dsMedication.Medication.IntendedUseColumn.ColumnName])) && MDVUtility.ToStr(dr[dsMedication.Medication.IntendedUseColumn.ColumnName]) == "Yes")
                            {
                                var MedicationDatakeyValues1 = new Dictionary<string, string>
                                    {
                                        { "Medication", MedicationwithRxNorm},
                                        { "MedicationName", MedicationName},
                                        { "Status", Convert.ToBoolean(dr[dsMedication.Medication.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                        { "StartDate", MDVUtility.ToStr(dr[dsMedication.Medication.StartDateColumn.ColumnName])},
                                        { "Indication", string.Empty},// Not in System
                                        { "RouteCode", string.Empty},// Not in System
                                        { "EndDate", MDVUtility.ToStr(dr[dsMedication.Medication.StopDateColumn.ColumnName])},
                                        { "NoOfTimes",MDVUtility.ToStr(dr[dsMedication.Medication.QuantityColumn.ColumnName])},
                                        { "RouteDescription",MDVUtility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName])},
                                        { "Dose",MDVUtility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName])},
                                        { "Refill",MDVUtility.ToStr(dr[dsMedication.Medication.RefillColumn.ColumnName])},
                                        { "FrequencyDescription",MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName])},
                                        { "Substitution", MDVUtility.ToStr(dr[dsMedication.Medication.SubstitutionColumn.ColumnName])},
                                        { "Directions", strDrugDosage},
                                        { "RxnormID", MDVUtility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName])},
                                        { "DrugTiming", strDrugTiming },
                                        { "CreatedDate", MDVUtility.ToStr(dr[dsMedication.Medication.CreatedOnColumn.ColumnName])},
                                        { "DrugAndDosage", MDVUtility.ToStr(dr[dsMedication.Medication.PatientNotesColumn.ColumnName])},
                                    };
                                lstPlanedMedications.Add(MedicationDatakeyValues1);
                            }
                            else if (string.IsNullOrWhiteSpace(MDVUtility.ToStr(dr[dsMedication.Medication.StartDateColumn.ColumnName])))
                            {
                                lstPlanedMedications.Add(MedicationDatakeyValues);
                            }
                            else
                            {
                                lstMedication.Add(MedicationDatakeyValues);
                            }
                        }
                    }
                    #endregion

                    #region SocialHx Data

                    #region SexualHx Data
                    string IsPatientPregnant = "No - pregnant";
                    string EDD = "";
                    if (dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_SexualHx.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_SexualHx.TableName].Rows.Count > 0)
                    {
                        DataRow drSocialHx = dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_SexualHx.TableName].Rows[0];
                        string pIsPrg = MDVUtility.ToStr(drSocialHx[dsSocialHx.SocialHx_SexualHx.bPregnancyStatusColumn.ColumnName]);
                        string pEdd = MDVUtility.ToStr(drSocialHx[dsSocialHx.SocialHx_SexualHx.CreatedOnColumn.ColumnName]);
                        if (!string.IsNullOrWhiteSpace(pIsPrg))
                        {
                            IsPatientPregnant = MDVUtility.ToBool(pIsPrg) == true ? "Yes - pregnant" : "No - pregnant";
                        }
                        if (!string.IsNullOrWhiteSpace(pEdd))
                        {
                            EDD = MDVUtility.ToDateTime(pIsPrg).ToShortDateString();
                        }

                    }

                    #endregion SexualHx Data

                    if (dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_Tobacco.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_Tobacco.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_Tobacco.TableName].Rows)
                        {
                            var StatusId = MDVUtility.ToLong(dr["StatusId"]);
                            var statusText = "";
                            var SmokingStatus = "";
                            var SNOMEDCTCode = "";
                            DataRow[] drFilter = dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_Tobacco_SmokingStatus.TableName].Select(string.Concat(dsSocialHx.SocialHx_Tobacco_SmokingStatus.StatusIdColumn.ColumnName, "=", StatusId));
                            if (drFilter != null && drFilter.Any())
                            {
                                SNOMEDCTCode = MDVUtility.ToStr(drFilter[0][dsSocialHx.SocialHx_Tobacco_SmokingStatus.SNOMEDCTCodeColumn.ColumnName]);
                                //List<string> NotAvaliableSnomedCode = new List<string> { "266927001", "428071000124103", "428061000124105", "81703003", "228511006" };
                                //if (NotAvaliableSnomedCode.Contains(SNOMEDCTCode))
                                //    break;
                                statusText = string.Concat(MDVUtility.ToStr(drFilter[0][dsSocialHx.SocialHx_Tobacco_SmokingStatus.DescriptionColumn.ColumnName]), ", [SNOMED-CT: ", SNOMEDCTCode, "]");
                                SmokingStatus = MDVUtility.ToStr(drFilter[0][dsSocialHx.SocialHx_Tobacco_SmokingStatus.DescriptionColumn.ColumnName]);
                            }
                            else
                                break;

                            DataRow drSocialHx = dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx.TableName].Rows[0];
                            var SocialHxDatakeyValues = new Dictionary<string, string>
                            {
                                { "SocialHxDate",  MDVUtility.ToStr(drSocialHx[dsSocialHx.SocialHx.SocialHxDateColumn.ColumnName])},
                                { "SNOMEDID",  SNOMEDCTCode},
                                { "Status", MDVUtility.ToStr(drSocialHx[dsSocialHx.SocialHx.IsActiveColumn.ColumnName])},
                                { "Comments", MDVUtility.ToStr(dr[dsSocialHx.SocialHx_Tobacco.CommentsColumn.ColumnName])},
                                { "SmokingStatus", SmokingStatus},
                                { "Description",statusText},
                                { "SocialHxElement","Smoking Status"},
                                { "IsPatientPregnant",IsPatientPregnant},
                                { "EDD",EDD},

                            };
                            lstSocials.Add(SocialHxDatakeyValues);
                        }
                    }

                    #endregion

                    #region Procedure Data

                    if (dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName].Rows)
                        {
                            DateTime dtProcedureDate = MDVUtility.ToDateTime(dr[dsProcedures.Procedures.StartDateColumn.ColumnName]);
                            string notesIds = MDVUtility.ToStr(dr["NotesId"]);
                            //if (noteId > 0 && !string.IsNullOrWhiteSpace(notesIds) && notesIds.Contains(MDVUtility.ToStr(noteId)))
                            //{
                            var SnomedText = MDVUtility.ToStr(dr[dsProcedures.Procedures.SNOMED_DESCRIPTIONColumn.ColumnName]);
                            var CPTText = MDVUtility.ToStr(dr[dsProcedures.Procedures.CPT_DESCRIPTIONColumn.ColumnName]);
                            if (!string.IsNullOrWhiteSpace(SnomedText) || !string.IsNullOrWhiteSpace(CPTText))
                            {
                                var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "ProcedureName",  SnomedText == string.Empty ? CPTText + " , [CPT:" + MDVUtility.ToStr(dr[dsProcedures.Procedures.CPTCodeColumn.ColumnName]) + "]" : SnomedText + " , [SNOMED-CT:" + MDVUtility.ToStr(dr[dsProcedures.Procedures.SNOMEDIDColumn.ColumnName]) + "]"},
                                        { "Status", MDVUtility.ToStr(dr[dsProcedures.Procedures.IsActiveColumn.ColumnName])},
                                        { "ProcedureDate", MDVUtility.ToDateTime(dr[dsProcedures.Procedures.StartDateColumn.ColumnName]).ToShortDateString()},
                                        { "SnomedId",MDVUtility.ToStr(dr[dsProcedures.Procedures.SNOMEDIDColumn.ColumnName])},
                                        { "CPT", MDVUtility.ToStr(dr[dsProcedures.Procedures.CPTCodeColumn.ColumnName])},
                                        { "SNOMEDID", MDVUtility.ToStr(dr[dsProcedures.Procedures.SNOMEDIDColumn.ColumnName])},
                                        { "SNOMED_DESCRIPTION", SnomedText},
                                        {"IsSurgical", MDVUtility.ToStr(dr[dsProcedures.Procedures.SurgicalColumn.ColumnName])}

                                    };
                                lstProcedure.Add(ProblemDatakeyValues);
                            }
                            //}
                        }
                    }

                    if (documentType == CCDAGenrator.DocumentTemplateType.ReferralSummary)
                    {
                        if (dsClinicalSummaryHTMLData.Tables[dsLabResult.LabOrderResult.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsLabResult.LabOrderResult.TableName].Rows)
                            {
                                if (MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.StatusColumn.ColumnName]).ToLower() == "partial")
                                {
                                    var PartialResultPending = new Dictionary<string, string>
                                    {
                                       { "ProcedureName",  MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CommentsColumn.ColumnName])},
                                       { "Status", MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.IsActiveColumn.ColumnName])},
                                       { "ProcedureDate", MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CreatedOnColumn.ColumnName])},
                                       { "SnomedId",string.Empty},
                                       { "CPT", MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CPTCodeColumn.ColumnName])},
                                    };
                                    lstProcedure.Add(PartialResultPending);
                                }
                            }
                        }
                    }


                    #endregion

                    #region MedicalHx Data

                    if (dsClinicalSummaryHTMLData.Tables[dsMedicalHx.MedicalHx.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsMedicalHx.MedicalHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsMedicalHx.MedicalHx.TableName].Rows)
                        {
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "MedicalHxDate",  MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.MedicalHxDateColumn.ColumnName])},
                                        { "Status", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.IsActiveColumn.ColumnName])},
                                        { "Comments", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.CommentsColumn.ColumnName])},
                                    };
                            lstMedicalHx.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region SurgicalHx Data

                    if (dsClinicalSummaryHTMLData.Tables[dsSurgicalHx.SurgicalHx.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows)
                        {
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "SurgicalHxDate",  MDVUtility.ToStr(dr[dsSurgicalHx.SurgicalHx.SurgicalHxDateColumn.ColumnName])},
                                        { "Status", MDVUtility.ToStr(dr[dsSurgicalHx.SurgicalHx.IsActiveColumn.ColumnName])},
                                        { "Comments", MDVUtility.ToStr(dr[dsSurgicalHx.SurgicalHx.CommentsColumn.ColumnName])},
                                    };
                            lstSurgicalHx.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region HospitalizationHx Data

                    if (dsClinicalSummaryHTMLData.Tables[dsHospitalizationHx.HospitalizationHx.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows)
                        {
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "HospitalizationHxDate",  MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxDateColumn.ColumnName])},
                                        { "Status", MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.IsActiveColumn.ColumnName])},
                                        { "Comments", MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.CommentsColumn.ColumnName])},
                                    };
                            lstHospitalizationHx.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region FamilyHx Data

                    if (dsClinicalSummaryHTMLData.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName].Rows)
                        {
                            var ProblemDatakeyValues = new Dictionary<string, string>
                            {
                                //{ "FamilyHxDate",  MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.FamilyHxDateColumn.ColumnName])},
                                //{ "Status", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.IsActiveColumn.ColumnName])},
                                //{ "Comments", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.CommentsColumn.ColumnName])},
                                { "RelationshipCode", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.RelationshipColumn.ColumnName])},
                                { "Relationship", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.RelationshipColumn.ColumnName])},
                                { "SNOMEDID", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_Disease.SNOMEDIDColumn.ColumnName])},
                                { "SNOMEDDescription", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_Disease.SNOMEDDescriptionColumn.ColumnName])},
                                { "AgeAtDiagnosis", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.AgeAtDiagnosisColumn.ColumnName])},
                                { "AgeAtDeath", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.AgeAtDeathColumn.ColumnName])},
                                { "IsRelativeDied", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.IsRelativeDiedColumn.ColumnName])},
                                { "BirthYear", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.BirthYearColumn.ColumnName])},
                                { "RelationshipSNOMEDId", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.RelationshipSNOMEDIdColumn.ColumnName])},
                                { "RelationshipSNOMEDDescription", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.RelationshipColumn.ColumnName])},
                                { "HealthStatus", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.HealthStatusColumn.ColumnName])},
                            };
                            lstFamilyHx.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region BirthHx Data

                    if (dsClinicalSummaryHTMLData.Tables[dsBirthHx.BirthHx.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsBirthHx.BirthHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsBirthHx.BirthHx.TableName].Rows)
                        {
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "BirthHxDate",  MDVUtility.ToStr(dr[dsBirthHx.BirthHx.BirthHxDateColumn.ColumnName])},
                                        { "Status", MDVUtility.ToStr(dr[dsBirthHx.BirthHx.IsActiveColumn.ColumnName])},
                                        { "Comments", MDVUtility.ToStr(dr[dsBirthHx.BirthHx.CommentsColumn.ColumnName])},
                                    };
                            lstBirthHx.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region PhysicalExam Data

                    if (dsClinicalSummaryHTMLData.Tables[dsPhysicalExam.PatientPhysicalExam.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows)
                        {
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "PatientPhysicalExam",  MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamDateColumn.ColumnName])},
                                        { "Status", MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.IsActiveColumn.ColumnName])},
                                        { "Comments", MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.CommentsColumn.ColumnName])},
                                    };
                            lstPhysicalExam.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region Immunization Data

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.Immunization.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.Immunization.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.Immunization.TableName].Rows)
                        {

                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Immunization",   MDVUtility.ToStr(dr[dsPlanOfCare.Immunization.VaccineNameColumn.ColumnName])},
                                        { "DateAdministered", MDVUtility.ToStr(dr[dsPlanOfCare.Immunization.AdministrationDateColumn.ColumnName])},
                                        { "Status", MDVUtility.ToStr(dr[dsPlanOfCare.Immunization.VaccineStatusColumn.ColumnName]) },
                                        { "CVX" ,MDVUtility.ToStr(dr[dsPlanOfCare.Immunization.CVXCodeColumn.ColumnName]) },
                                        { "CVXCode" ,MDVUtility.ToStr(dr[dsPlanOfCare.Immunization.CVXColumn.ColumnName]) },
                                        { "CompletionStatusCode" ,MDVUtility.ToStr(dr[dsPlanOfCare.Immunization.CompletionStatusCodeColumn.ColumnName]) },
                                        { "ExpiryDate" ,MDVUtility.ToStr(dr[dsPlanOfCare.Immunization.ExpiryDateColumn.ColumnName]) }
                                    };
                            lstImmunization.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region EncounterDiagnosic
                    /*
                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.EncounterProblemList.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.EncounterProblemList.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.EncounterProblemList.TableName].Rows)
                        {
                            var ProblemNamewithSnomed = MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.SNOMED_DESCRIPTIONColumn.ColumnName]);
                            if (!string.IsNullOrWhiteSpace(ProblemNamewithSnomed)) //if SNOMED description not found.
                            {
                                var ProblemListId = MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.ProblemListIdColumn.ColumnName]);
                                lstEncounterProblemId.Add(ProblemListId);

                                if (MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.SNOMED_DESCRIPTIONColumn.ColumnName]) != string.Empty)
                                    ProblemNamewithSnomed = string.Concat(ProblemNamewithSnomed, " [SNOMEDID:", MDVUtility.ToStr(dr[dsProblems.ProblemList.SNOMEDIDColumn.ColumnName]), "]");
                                var EncounterDiagnosicDatakeyValues = new Dictionary<string, string>
                                {
                                    { "ProblemNamewithSnomed",  ProblemNamewithSnomed},
                                    { "ProblemName",  MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.SNOMED_DESCRIPTIONColumn.ColumnName])},
                                    { "StartDate",  MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.StartDateColumn.ColumnName])},
                                    { "IsActive",  MDVUtility.ToStr( dr[dsPlanOfCare.EncounterProblemList.IsActiveColumn.ColumnName])},
                                    { "SNOMEDID", MDVUtility.ToStr( dr[dsPlanOfCare.EncounterProblemList.SNOMEDIDColumn.ColumnName])},
                                    { "VisitDate", MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.NoteDateColumn.ColumnName])}
                                };
                                lstEncounterDiagnosicDatakeyValues.Add(EncounterDiagnosicDatakeyValues);
                            }
                        }
                    }
                    */
                    #endregion

                    #region Problems Data and  EncounterDiagnosic

                    if (dsClinicalSummaryHTMLData.Tables[dsProblems.ProblemList_CCDA.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsProblems.ProblemList_CCDA.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsProblems.ProblemList_CCDA.TableName].Rows)
                        {
                            var ProblemListId = MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.ProblemListIdColumn.ColumnName]);
                            //if (!lstEncounterProblemId.Contains(ProblemListId))
                            //{
                            string ProblemNotesId = MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.ProblemNotesIdColumn.ColumnName]);
                            var ProblemNamewithSnomed = string.Concat(MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.SNOMED_DESCRIPTIONColumn.ColumnName]));
                            //var PrimarySite = MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PrimarySiteColumn.ColumnName]);
                            if (!string.IsNullOrWhiteSpace(ProblemNamewithSnomed)) //if SNOMED description not found.
                            {
                                if (MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.SNOMEDIDColumn.ColumnName]) != string.Empty)
                                    ProblemNamewithSnomed = string.Concat(ProblemNamewithSnomed, " [SNOMEDID:", MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.SNOMEDIDColumn.ColumnName]), "]");
                                var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "ProblemNamewithSnomed",  ProblemNamewithSnomed},
                                        { "ProblemName",  MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.SNOMED_DESCRIPTIONColumn.ColumnName])},
                                        { "Status", Convert.ToBoolean(dr[dsProblems.ProblemList_CCDA.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                        { "EffectiveDate", MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.StartDateColumn.ColumnName])},
                                        { "Type", "Problem"},
                                        { "SNOMEDID",MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.SNOMEDIDColumn.ColumnName])},
                                        { "VisitDate", MDVUtility.ToStr(dtNoteDate)},
                                        { "IsActive",  MDVUtility.ToStr( dr[dsProblems.ProblemList_CCDA.IsActiveColumn.ColumnName])},
                                        { "StartDate",  MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.StartDateColumn.ColumnName])},
                                        { "ProblemShortName",  MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.ProblemNameColumn.ColumnName])},
                                        { "ProblemComments",  MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.CommentsColumn.ColumnName])},
                                        { "ComplaintComments",  MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.ComplaintCommentsColumn.ColumnName])},
                                        { "ProblemListId",  ProblemListId},

                                    };
                                if ((noteId > 0 && !string.IsNullOrWhiteSpace(ProblemNotesId) && ProblemNotesId.Contains(noteId.ToString()) || noteId == 0) && MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.IsChiefComplaintColumn.ColumnName]) == "0")
                                {
                                    lstProblems.Add(ProblemDatakeyValues);
                                }

                                if (noteId > 0 && MDVUtility.ToStr(dr[dsProblems.ProblemList_CCDA.IsChiefComplaintColumn.ColumnName]) == "1")
                                {
                                    lstEncounterDiagnosicDatakeyValues.Add(ProblemDatakeyValues);
                                } 
                            }
                            // }
                        }
                    }

                    #endregion Problems Data and  EncounterDiagnosic

                    #region Problems Cancer Data

                    if (lstComopent != null && lstComopent.Where(a => a.Value.ToLower() == "cancerreportdataelement").Select(a => a.Value).FirstOrDefault() != null && dsClinicalSummaryHTMLData.Tables[dsProblems.ProblemList.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsProblems.ProblemList.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsProblems.ProblemList.TableName].Rows)
                        {
                            var ProblemListId = MDVUtility.ToStr(dr[dsProblems.ProblemList.ProblemListIdColumn.ColumnName]);
                            var PrimarySite = MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PrimarySiteColumn.ColumnName]);
                            var ProblemNamewithSnomed = string.Concat(MDVUtility.ToStr(dr[dsProblems.ProblemList.DescriptionColumn.ColumnName]));
                            if (!string.IsNullOrWhiteSpace(PrimarySite))
                            {
                                var ProblemDatakeyValues = new Dictionary<string, string>
                                {
                                    { "ProblemNamewithSnomed",  ProblemNamewithSnomed},
                                    { "DiagnosisDate",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.CancerDiagnosisDateColumn.ColumnName])},
                                    { "CancerClinicalDiagnosisDate",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.CancerClinicalDiagnosisDateColumn.ColumnName])},
                                    { "EffectiveDate",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.CancerEffectiveDateColumn.ColumnName])},
                                    { "PrimarySite", PrimarySite},
                                    { "PrimarySiteCode", MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PrimarySiteCodeColumn.ColumnName])},
                                    { "Laterality", MDVUtility.ToStr(dr[dsProblems.ProblemDetails.LateralityColumn.ColumnName])},
                                    { "LateralityCode", MDVUtility.ToStr(dr[dsProblems.ProblemDetails.LateralityCodeColumn.ColumnName])},
                                    { "Histology", MDVUtility.ToStr(dr[dsProblems.ProblemDetails.HistologicTypeColumn.ColumnName])},
                                    { "HistologyCode", MDVUtility.ToStr(dr[dsProblems.ProblemDetails.HistologicCodeColumn.ColumnName])},
                                    { "Behavior",MDVUtility.ToStr(dr[dsProblems.ProblemDetails.BehaviorColumn.ColumnName])},
                                    { "BehaviorCode",MDVUtility.ToStr(dr[dsProblems.ProblemDetails.BehaviorCodeColumn.ColumnName])},
                                    { "DiagnosticConfirmation", MDVUtility.ToStr(dr[dsProblems.ProblemDetails.DiagnosisConfirmationColumn.ColumnName])},
                                    { "DiagnosisConfirmationCode", MDVUtility.ToStr(dr[dsProblems.ProblemDetails.DiagnosisConfirmationCodeColumn.ColumnName])},
                                    { "Grade", MDVUtility.ToStr(dr[dsProblems.ProblemDetails.GradeColumn.ColumnName])},
                                    { "GradeCode", MDVUtility.ToStr(dr[dsProblems.ProblemDetails.GradeCodeColumn.ColumnName])},
                                    { "ClinicalStageGroup",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.ClinicalStageGroupColumn.ColumnName])},
                                    { "ClinicalStageGroupCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.ClinicalStageGroupCodeColumn.ColumnName])},
                                    { "PrimaryClinicalTumor",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PrimaryClinicalTumorColumn.ColumnName])},
                                    { "PrimaryClinicalTumorCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PrimaryClinicalTumorCodeColumn.ColumnName])},
                                    { "RLNC",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.RLNCColumn.ColumnName])},
                                    { "RLNCCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.RLNCCodeColumn.ColumnName])},
                                    { "DistanceMestastatases",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.DistanceMestastatasesColumn.ColumnName])},
                                    { "DistanceMestastatasesCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.DistanceMestastatasesCodeColumn.ColumnName])},
                                    { "StagerClinicalCancer",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.StagerClinicalCancerColumn.ColumnName])},
                                    { "StagerClinicalCancerCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.StagerClinicalCancerCodeColumn.ColumnName])},
                                    { "ClinicalStageDescriptor",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.ClinicalStageDescriptorColumn.ColumnName])},
                                    { "ClinicalStageDescriptorCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.ClinicalStageDescriptorCodeColumn.ColumnName])},
                                     { "PathologicStageGroup",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PathologicStageGroupColumn.ColumnName])},
                                    { "PathologicStageGroupCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PathologicStageGroupCodeColumn.ColumnName])},
                                    { "PathologicStageDescriptor",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PathologicStageDescriptorColumn.ColumnName])},
                                    { "PathologicStageDescriptorCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PathologicStageDescriptorCodeColumn.ColumnName])},
                                    { "PrimaryTumorPathologic",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PrimaryTumorPathologicColumn.ColumnName])},
                                    { "PrimaryTumorPathologicCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.PrimaryTumorPathologicCodeColumn.ColumnName])},
                                    { "RLNP",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.RLNPColumn.ColumnName])},
                                    { "RLNPCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.RLNPCodeColumn.ColumnName])},
                                    { "DistanceMestastatasesPathologic",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.DistanceMestastatasesPathologicColumn.ColumnName])},
                                    { "DistanceMestastatasesPathologicCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.DistanceMestastatasesPathologicCodeColumn.ColumnName])},
                                     { "StagerPathologicCancer",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.StagerPathologicCancerColumn.ColumnName])},
                                    { "StagerPathologicCancerCode",  MDVUtility.ToStr(dr[dsProblems.ProblemDetails.StagerPathologicCancerCodeColumn.ColumnName])},
                                    { "ProblemName",  MDVUtility.ToStr(dr[dsProblems.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName])},
                                    { "Status", Convert.ToBoolean(dr[dsProblems.ProblemList.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                    { "Type", "Problem"},
                                    { "VisitDate", MDVUtility.ToStr(dtNoteDate)},
                                    { "IsActive",  MDVUtility.ToStr( dr[dsProblems.ProblemList.IsActiveColumn.ColumnName])},
                                    { "ProblemShortName",  MDVUtility.ToStr(dr[dsProblems.ProblemList.ProblemNameColumn.ColumnName])},
                                    { "ProblemComments",  MDVUtility.ToStr(dr[dsProblems.ProblemList.CommentsColumn.ColumnName])},
                                    { "ComplaintComments",  MDVUtility.ToStr(dr[dsProblems.ProblemList.ComplaintCommentsColumn.ColumnName])},
                                    { "StartDate",  MDVUtility.ToStr(dr[dsProblems.ProblemList.StartDateColumn.ColumnName])},
                                    { "SNOMEDID",MDVUtility.ToStr(dr[dsProblems.ProblemList.SNOMEDIDColumn.ColumnName])},
                                };
                                lstProblemsCancer.Add(ProblemDatakeyValues);
                                if (MDVUtility.ToStr(dr[dsProblems.ProblemList.IsChiefComplaintColumn.ColumnName]) == "1")
                                {
                                    lstEncounterDiagnosicDatakeyValues.Add(ProblemDatakeyValues);
                                }
                            }
                        }
                    }

                    #endregion Problems Cancer Data 

                    #region Referral Provider

                    if (dsClinicalSummaryHTMLData.Tables["ReferralProvider"] != null && dsClinicalSummaryHTMLData.Tables["ReferralProvider"].Rows.Count > 0)
                    {
                        foreach (DataRow drProvider in dsClinicalSummaryHTMLData.Tables["ReferralProvider"].Rows)
                        {
                            providerFullName = MDVUtility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName]) + ", " + MDVUtility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName]);
                            providerOfficeAddress = MDVUtility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName]);
                            providerOfficePhone = MDVUtility.ToStr(drProvider[dsProvider.Provider.PhoneNoColumn.ColumnName]);
                            DSConsultationOrder dsConsultationOrder = new DSConsultationOrder();
                            var OrderDate = MDVUtility.ToStr(dsClinicalSummaryHTMLData.Tables[dsConsultationOrder.ConsultationOrder.TableName].Rows[0][dsConsultationOrder.ConsultationOrder.OrderDateColumn.ColumnName]);
                            string name = string.Format("{0} {1}, Tel: {2}, Address 1: {3}, Address 2: {4}, Zip Code: {5}, "
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName])
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName])
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.PhoneNoColumn.ColumnName])
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName])
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.HomeAddressColumn.ColumnName])
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.ZIPCodeColumn.ColumnName])
                                );
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Name",  name},
                                        { "Type", "Referral to Other Provider." },
                                        { "Instruction" , string.Empty },
                                        { "Date", OrderDate}
                                    };
                            lstFutureAppointment.Add(ProblemDatakeyValues);


                        }
                    }

                    #endregion

                    #region Future Appointment

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows)
                        {
                            string FutureInstruction = MDVUtility.ToStr(dr[dsPlanOfCare.PlanofCare.FutureInstructionColumn.ColumnName]);
                            if (!string.IsNullOrWhiteSpace(FutureInstruction))
                            {
                                var ProblemDatakeyValues = new Dictionary<string, string>
                                {
                                    { "Name",  FutureInstruction},
                                    { "Type", "Future Appointment" },
                                    { "Instruction" , string.Empty },
                                    { "Date", MDVUtility.ToDateTime(dr[dsPlanOfCare.PlanofCare.CreatedOnColumn.ColumnName]).ToShortDateString()}
                                };
                                lstFutureAppointment.Add(ProblemDatakeyValues);
                            }
                        }
                    }

                    #endregion

                    #region Instructions

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows)
                        {
                            var clinicalInstruction = MDVUtility.ToStr(dr[dsPlanOfCare.PlanofCare.ClinicalInstructionColumn.ColumnName]);
                            if (!string.IsNullOrWhiteSpace(clinicalInstruction))
                            {
                                var InstructionsAndDecisionAids = new Dictionary<string, string>
                                {
                                    { "Instruction",  clinicalInstruction},
                                    { "Type", "Clinical Instructions" },
                                    { "CreatedOn", MDVUtility.ToDateTime(dr[dsPlanOfCare.PlanofCare.CreatedOnColumn.ColumnName]).ToShortDateString()}
                                };
                                lstInstructionsAndDecisionAids.Add(InstructionsAndDecisionAids);
                            }
                        }
                    }

                    #endregion

                    #region Patient Decision Aids
                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows)
                        {
                            var patientDecisionAid = MDVUtility.ToStr(dr[dsPlanOfCare.PlanofCare.PatientDecisionAidColumn.ColumnName]);
                            if (!string.IsNullOrWhiteSpace(patientDecisionAid))
                            {
                                var InstructionsAndDecisionAids = new Dictionary<string, string>
                                {
                                    { "Instruction",  patientDecisionAid},
                                    { "Type", "Patient Decision Aids" },
                                    { "CreatedOn", MDVUtility.ToDateTime(dr[dsPlanOfCare.PlanofCare.CreatedOnColumn.ColumnName]).ToShortDateString()}
                                };
                                lstInstructionsAndDecisionAids.Add(InstructionsAndDecisionAids);
                            }
                        }
                    }

                    #endregion

                    #region PLAN OF CARE GOAL

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanOfCareGoal.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanOfCareGoal.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanOfCareGoal.TableName].Rows)
                        {
                            string GoalDescription = MDVUtility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.SNOMEDDescriptionColumn.ColumnName]);
                            if (GoalDescription != string.Empty)
                            {
                                GoalDescription = string.Concat(GoalDescription, " [SNOMED-CT:", MDVUtility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.SNOMEDIDColumn.ColumnName]), "]");
                            }
                            else if (MDVUtility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.ICD10CodeDescriptionColumn.ColumnName]) != string.Empty)
                            {
                                GoalDescription = MDVUtility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.ICD10CodeDescriptionColumn.ColumnName]);
                                GoalDescription = string.Concat(GoalDescription, " [ICD 10:", MDVUtility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.ICD10CodeColumn.ColumnName]), "]");
                            }
                            else if (MDVUtility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.ICD9CodeDescriptionColumn.ColumnName]) != string.Empty)
                            {
                                GoalDescription = MDVUtility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.ICD9CodeDescriptionColumn.ColumnName]);
                                GoalDescription = string.Concat(GoalDescription, " [ICD 9:", MDVUtility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.ICD9CodeColumn.ColumnName]), "]");
                            }

                            if (!string.IsNullOrWhiteSpace(GoalDescription))
                            {
                                var PlanOfCareDatakeyValues = new Dictionary<string, string>
                                {
                                    { "Name",  GoalDescription},
                                    { "Type", "Goal" },
                                    { "Instruction" , MDVUtility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.InstructionColumn.ColumnName]) },
                                    { "Date", MDVUtility.ToDateTime(dr[dsPlanOfCare.PlanOfCareGoal.CreatedOnColumn.ColumnName]).ToShortDateString()}
                                };
                                // lstGoal.Add(PlanOfCareDatakeyValues);
                            }
                        }
                    }

                    #endregion

                    #region Care Plan Goals
                    if (goalsList != null && goalsList.Count > 0)
                    {
                        foreach (var item in goalsList)
                        {
                            var GoalDatakeyValues = new Dictionary<string, string>
                                {
                                    { "GoalComments",  item.GoalComments},
                                    { "Value",  item.GoalValue },
                                    { "ICD10Code" , item.ICD10Code },
                                    { "ICD10CodeDescription", item.ICD10CodeDescription},
                                    { "GoalDate" , item.GoalDate },
                                    { "GoalStatusValue", item.GoalStatusValue}
                                };
                            lstGoal.Add(GoalDatakeyValues);
                        }

                    }
                    #endregion  Care Plan Goals

                    #region Health Conserns
                    if (healthConsernsList != null && healthConsernsList.Count > 0)
                    {
                        foreach (var item in healthConsernsList)
                        {
                            string ObserDesc = MDVUtility.ToStr(item.Observation_SNOMEDDescription);
                            string ConDescr = MDVUtility.ToStr(item.Concerns_SNOMEDDescription);
                            string RiskDescr = MDVUtility.ToStr(item.Risk_SNOMEDDescription);

                            if (!string.IsNullOrWhiteSpace(ObserDesc))
                            {
                                var DatakeyValues = new Dictionary<string, string>
                                {
                                    { "Name",  ObserDesc},
                                    { "Status",item.ConcernsStatusValue },
                                    { "Date", MDVUtility.ToDateTime(item.ObservationDate).ToShortDateString()},
                                    { "ICD10Code" , item.Observation_ICD10Code },
                                    { "ICD10CodeDescription", item.Observation_ICD10CodeDescription},
                                    { "SNOMEDID" , item.Observation_SNOMEDID },
                                    { "SNOMEDDescription", item.Observation_SNOMEDDescription},
                                };
                                lstHealthObservation.Add(DatakeyValues);
                            }
                            if (!string.IsNullOrWhiteSpace(ConDescr))
                            {
                                var DatakeyValues = new Dictionary<string, string>
                                {
                                    { "Name",  ConDescr},
                                    { "Status","Active" },
                                    { "Date", MDVUtility.ToDateTime(item.ConcernsDate).ToShortDateString()},
                                    { "ICD10Code" , item.Concerns_ICD10Code },
                                    { "ICD10CodeDescription", item.Concerns_ICD10CodeDescription},
                                    { "SNOMEDID" , item.Concerns_SNOMEDID },
                                    { "SNOMEDDescription", item.Concerns_SNOMEDDescription},
                                };
                                lstHealthConsern.Add(DatakeyValues);
                            }
                            if (!string.IsNullOrWhiteSpace(RiskDescr))
                            {
                                var DatakeyValues = new Dictionary<string, string>
                                {
                                    { "Name",  RiskDescr},
                                    { "Status",item.RiskStatusValue },
                                    { "Date", MDVUtility.ToDateTime(item.RiskDate).ToShortDateString()},
                                    { "ICD10Code" , item.Risk_ICD10Code },
                                    { "ICD10CodeDescription", item.Risk_ICD10CodeDescription},
                                    { "SNOMEDID" , item.Risk_SNOMEDID },
                                    { "SNOMEDDescription", item.Risk_SNOMEDDescription},
                                };
                                lstHealthRisks.Add(DatakeyValues);
                            }
                        }

                    }
                    #endregion  Health Conserns

                    #region Interventions
                    if (interventionsList != null && interventionsList.Count > 0)
                    {
                        foreach (var item in interventionsList)
                        {
                            //if (!string.IsNullOrWhiteSpace(ObserDesc))
                            //{
                            var DatakeyValues = new Dictionary<string, string>
                                {
                                    { "Status",  item.InterventionStatusValue},
                                    { "Goals",item.GoalIds },
                                    { "Date", MDVUtility.ToDateTime(item.InterventionDate).ToShortDateString()},
                                    { "ICD10Code" , item.ICD10Code },
                                    { "ICD10CodeDescription", item.ICD10CodeDescription},
                                    { "SNOMEDID" , item.SNOMEDID },
                                    { "SNOMEDDescription", item.SNOMEDDescription},
                                };
                            lstInterventions.Add(DatakeyValues);
                            //}
                        }

                    }
                    #endregion  Interventions

                    #region Outcomes
                    if (outComesList != null && outComesList.Count > 0)
                    {
                        foreach (var item in outComesList)
                        {
                            //if (!string.IsNullOrWhiteSpace(ObserDesc))
                            //{
                            var DatakeyValues = new Dictionary<string, string>
                                {
                                    { "Value",  item.OutcomeValue},
                                    { "Goals",item.GoalIds },
                                    { "Date", MDVUtility.ToDateTime(item.OutcomeDate).ToShortDateString()},
                                    { "ICD10Code" , item.ICD10Code },
                                    { "ICD10CodeDescription", item.ICD10CodeDescription},
                                    { "SNOMEDID" , item.SNOMEDID },
                                    { "SNOMEDDescription", item.SNOMEDDescription},
                                    { "Interventions",item.InterventionIds },
                                    { "GoalsStatus",item.GoalsStatus },
                                };
                            lstOutcomes.Add(DatakeyValues);
                            // }
                        }

                    }
                    #endregion  Outcomes

                    #region Scheduled Procedure Data

                    if (dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName].Rows)
                        {
                            DateTime dtProcedureDate = MDVUtility.ToDateTime(dr[dsProcedures.Procedures.StartDateColumn.ColumnName]);
                            if (dtNoteDate.Date < dtProcedureDate.Date)
                            {
                                var SnomedText = MDVUtility.ToStr(dr[dsProcedures.Procedures.SNOMED_DESCRIPTIONColumn.ColumnName]);
                                var CPTText = MDVUtility.ToStr(dr[dsProcedures.Procedures.CPT_DESCRIPTIONColumn.ColumnName]);
                                if (!string.IsNullOrWhiteSpace(SnomedText) || !string.IsNullOrWhiteSpace(CPTText))
                                {
                                    var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Name",  SnomedText == string.Empty ? CPTText + " , [CPT:" + MDVUtility.ToStr(dr[dsProcedures.Procedures.CPTCodeColumn.ColumnName]) + "]" : SnomedText + " , [SNOMED-CT:" + MDVUtility.ToStr(dr[dsProcedures.Procedures.SNOMEDIDColumn.ColumnName]) + "]"},
                                        { "Status", MDVUtility.ToStr(dr[dsProcedures.Procedures.IsActiveColumn.ColumnName])},
                                        { "Date", MDVUtility.ToDateTime(dr[dsProcedures.Procedures.StartDateColumn.ColumnName]).ToShortDateString()},
                                        { "Type", "Scheduled Procedure" },
                                        { "Instruction" , string.Empty }
                                    };
                                    lstscheduledProcedure.Add(ProblemDatakeyValues);
                                }
                            }
                        }
                    }

                    #endregion

                    #region Result

                    if (dsClinicalSummaryHTMLData.Tables[dsLabResult.LabOrderResultDetail.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsLabResult.LabOrderResultDetail.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsLabResult.LabOrderResultDetail.TableName].Rows)
                        {
                            string LabTest = MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName]);
                            LabTest = string.Format("{0}, [LOINC:{1}]", LabTest, MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName]));
                            string LabTestName = MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName]);
                            string ActualResult = MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.ResultColumn.ColumnName]);
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.UoMColumn.ColumnName])))
                            {
                                ActualResult = string.Format("{0} ({1})", ActualResult, MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.UoMColumn.ColumnName]));
                            }
                            string labResultStatus = "Final";
                            try
                            {

                                string labOrderResult = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName]);
                                if (labOrderResult != string.Empty)
                                {
                                    DataRow[] drFilters = dsClinicalSummaryHTMLData.Tables[dsLabResult.LabOrderResult.TableName].Select(string.Format("LabOrderResultId = {0}", MDVUtility.ToLINQFormatString(labOrderResult)));
                                    labResultStatus = MDVUtility.ToStr(drFilters[0][dsLabResult.LabOrderResult.StatusColumn.ColumnName]);
                                }

                                if (labResultStatus == "Partial")
                                {
                                    labResultStatus = "Pending";
                                }

                            }
                            catch (Exception ex) { }
                            //if (MDVUtility.ToDateTime(dr[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]).Date == dtNoteDate.Date || documentType == CCDAGenrator.DocumentTemplateType.DataPortability)
                            //{
                            string flag = MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]);
                            string range = MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.RangeColumn.ColumnName]);
                            var LabOrderResultDetailDatakeyValues = new Dictionary<string, string>
                                {
                                    { "LabTest",  LabTest},
                                    { "LabTestName",  LabTestName},
                                    { "ActualResult",  ActualResult},
                                    { "LoincCode", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName])},
                                    { "Unit" ,  MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.UoMColumn.ColumnName]) },
                                    { "ResultValue",  MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.ResultColumn.ColumnName])},
                                    { "ResultDate", MDVUtility.ToDateTime(dr[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]).ToShortDateString()},
                                    { "Status", labResultStatus},
                                    { "TestTypeCode", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.TestTypeCodeColumn.ColumnName])},
                                    { "TestTypeName", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.TestTypeNameColumn.ColumnName])},
                                    { "TestTypeCodeSystem", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.TestTypeCodeSystemColumn.ColumnName])},
                                    { "LabName", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.LabNameColumn.ColumnName])},
                                    { "City",  MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.CityColumn.ColumnName])},
                                    { "ZIPCode",  MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.ZIPCodeColumn.ColumnName])},
                                    { "State", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.StateColumn.ColumnName])},
                                    { "PhoneNo",  MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.PhoneNoColumn.ColumnName]) },
                                    { "Address", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.AddressColumn.ColumnName])},
                                    { "Country", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.CountryColumn.ColumnName])},
                                    { "Flag",flag},
                                    { "Range",range}
                                };
                            lstResults.Add(LabOrderResultDetailDatakeyValues);
                            //}
                            /*else if (MDVUtility.ToDateTime(dr[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]) > dtNoteDate)
                            {
                                var LabOrderResultDetailDatakeyValues = new Dictionary<string, string>
                                {
                                    { "Name",  LabTestName},
                                    { "Type", "Scheduled Lab" },
                                    { "Instruction" , string.Empty },
                                    { "Date", MDVUtility.ToDateTime(dr[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]).ToShortDateString()}
                                };
                                lstscheduledProcedure.Add(LabOrderResultDetailDatakeyValues);
                            }
                            */

                        }
                    }

                    // Add Radiology Result
                    if (dsClinicalSummaryHTMLData.Tables[dsRadiologyResult.RadiologyOrderResultDetail.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsRadiologyResult.RadiologyOrderResultDetail.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsRadiologyResult.RadiologyOrderResultDetail.TableName].Rows)
                        {
                            string LabTest = MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.LOINCDescriptionColumn.ColumnName]);
                            LabTest = string.Format("{0}, [LOINC:{1}]", LabTest, MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.LOINCColumn.ColumnName]));
                            string LabTestName = MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.LOINCDescriptionColumn.ColumnName]);
                            string ActualResult = MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.ResultColumn.ColumnName]);
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.UoMColumn.ColumnName])))
                            {
                                ActualResult = string.Format("{0} ({1})", ActualResult, MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.UoMColumn.ColumnName]));
                            }
                            string labResultStatus = "Completed";
                            string flag = MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.FlagColumn.ColumnName]);
                            string range = MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.RangeColumn.ColumnName]);
                            if (MDVUtility.ToDateTime(dr[dsRadiologyResult.RadiologyOrderResultDetail.ObservationDateColumn.ColumnName]).Date == dtNoteDate.Date || documentType == CCDAGenrator.DocumentTemplateType.DataPortability
                                || documentType == CCDAGenrator.DocumentTemplateType.ReferralNote || documentType == CCDAGenrator.DocumentTemplateType.CCDVDT)
                            {
                                var LabOrderResultDetailDatakeyValues = new Dictionary<string, string>
                                {
                                    { "LabTest",  LabTest},
                                    { "LabTestName",  LabTestName},
                                    { "ActualResult",  ActualResult},
                                    { "LoincCode", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.LOINCColumn.ColumnName])},
                                    { "Unit" ,  MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.UoMColumn.ColumnName]) },
                                    { "ResultValue",  MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.ResultColumn.ColumnName])},
                                    { "ResultDate", MDVUtility.ToDateTime(dr[dsRadiologyResult.RadiologyOrderResultDetail.ObservationDateColumn.ColumnName]).ToShortDateString()},
                                    { "Status", labResultStatus},
                                    { "TestTypeCode", ""},
                                    { "TestTypeName", ""},
                                    { "TestTypeCodeSystem", ""},
                                    { "LabName", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.LabNameColumn.ColumnName])},
                                    { "City",  MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.CityColumn.ColumnName])},
                                    { "ZIPCode",  MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.ZIPCodeColumn.ColumnName])},
                                    { "State", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.StateColumn.ColumnName])},
                                    { "PhoneNo",  MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.PhoneNoColumn.ColumnName]) },
                                    { "Address", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.AddressColumn.ColumnName])},
                                    { "Country", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.CountryColumn.ColumnName])},
                                    { "Remarks",   MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.RemarksColumn.ColumnName])},
                                    { "Flag",flag},
                                    { "Range",range}
                                };
                                if (documentType == CCDAGenrator.DocumentTemplateType.CCDVDT || documentType == CCDAGenrator.DocumentTemplateType.ReferralNoteVDT)
                                {
                                    lstRadiologyResults.Add(LabOrderResultDetailDatakeyValues);
                                }
                                else
                                {
                                    lstResults.Add(LabOrderResultDetailDatakeyValues);
                                }
                            }
                            else if (MDVUtility.ToDateTime(dr[dsRadiologyResult.RadiologyOrderResultDetail.ObservationDateColumn.ColumnName]) > dtNoteDate)
                            {
                                var LabOrderResultDetailDatakeyValues = new Dictionary<string, string>
                                {
                                    { "Name",  LabTestName},
                                    { "Type", "Scheduled Imaging" },
                                    { "Instruction" , string.Empty },
                                    { "Date", MDVUtility.ToDateTime(dr[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]).ToShortDateString()},
                                    { "TestTypeCode", ""},
                                    { "TestTypeName", ""},
                                    { "TestTypeCodeSystem", ""},
                                    { "LabName", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.LabNameColumn.ColumnName])},
                                    { "City",  MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.CityColumn.ColumnName])},
                                    { "ZIPCode",  MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.ZIPCodeColumn.ColumnName])},
                                    { "State", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.StateColumn.ColumnName])},
                                    { "PhoneNo",  MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.PhoneNoColumn.ColumnName]) },
                                    { "Address", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.AddressColumn.ColumnName])},
                                    { "Country", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.CountryColumn.ColumnName])},
                                    { "Flag",flag},
                                    { "Range",range}
                                };
                                lstscheduledProcedure.Add(LabOrderResultDetailDatakeyValues);
                            }
                        }
                    }

                    #endregion

                    #region Cognitive Functional Status

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.FunctionalStatus.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.FunctionalStatus.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.FunctionalStatus.TableName].Rows)
                        {
                            var CognitiveDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Name",   MDVUtility.ToStr(dr[dsPlanOfCare.FunctionalStatus.NameColumn.ColumnName])},
                                        { "SNOMEDID",   MDVUtility.ToStr(dr[dsPlanOfCare.FunctionalStatus.SNOMEDIDColumn.ColumnName])},
                                        { "EffectiveDate", MDVUtility.ToStr(dr[dsPlanOfCare.FunctionalStatus.EffectiveDateColumn.ColumnName])},
                                        { "Type" ,  MDVUtility.ToStr(dr[dsPlanOfCare.FunctionalStatus.TypeColumn.ColumnName]) }

                                    };
                            lstCognitiveFunctionalStatus.Add(CognitiveDatakeyValues);
                        }
                    }

                    #endregion

                    #region Referring Provider

                    if (dsClinicalSummaryHTMLData.Tables[dsProfile.ReferringProvider.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsProfile.ReferringProvider.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsProfile.ReferringProvider.TableName].Rows)
                        {
                            var Name = string.Format("{0} {1}, Tel : {2}, Address 1 : {3}, Address 2 : {4}, Zip Code: {5}", MDVUtility.ToStr(dr[dsProfile.ReferringProvider.FirstNameColumn.ColumnName])
                                , MDVUtility.ToStr(dr[dsProfile.ReferringProvider.LastNameColumn.ColumnName])
                                , MDVUtility.ToStr(dr[dsProfile.ReferringProvider.PhoneNoColumn.ColumnName])
                                , MDVUtility.ToStr(dr[dsProfile.ReferringProvider.PhoneNoColumn.ColumnName])
                                , MDVUtility.ToStr(dr[dsProfile.ReferringProvider.AddressColumn.ColumnName])
                                , MDVUtility.ToStr(dr[dsProfile.ReferringProvider.Address2Column.ColumnName])
                                  , MDVUtility.ToStr(dr[dsProfile.ReferringProvider.ZipCodeColumn.ColumnName]));
                            var CognitiveDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Name",   Name},
                                        { "Instruction",  string.Empty},
                                        { "Date", MDVUtility.ToStr(dr[dsProfile.ReferringProvider.CreatedOnColumn.ColumnName])},
                                        { "Type" ,  "Referral to Other Provider" }

                                    };
                            lstRefferalProviderData.Add(CognitiveDatakeyValues);
                        }
                    }

                    #endregion

                    #region ReasonForReferral
                    if (dsClinicalSummaryHTMLData.Tables[dsPatientReferral.Referrals.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPatientReferral.Referrals.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPatientReferral.Referrals.TableName].Rows)
                        {
                            var Referral =
                                string.Format("{0}, {1}",
                                MDVUtility.ToStr(dr[dsPatientReferral.Referrals.ReasonColumn.ColumnName]),
                                MDVUtility.ToStr(dr[dsPatientReferral.Referrals.ProviderNameColumn.ColumnName]));
                            if (dsClinicalSummaryHTMLData.Tables[dsPatientReferral.Referrals.TableName].Rows.Count > 0)
                            {
                                DataRow drReferringProvider = dsClinicalSummaryHTMLData.Tables[dsPatientReferral.Referrals.TableName].Rows[0];
                                Referral =
                             string.Format("{0}{1}{2}{3}{4}{5}",
                             Referral,
                             string.IsNullOrWhiteSpace(MDVUtility.ToStr(drReferringProvider[dsPatientReferral.Referrals.RefProviderPhoneNoColumn.ColumnName])) ? string.Empty : string.Concat(", Tel: ", MDVUtility.ToStr(dr[dsPatientReferral.Referrals.RefProviderPhoneNoColumn.ColumnName])),
                             string.IsNullOrWhiteSpace(MDVUtility.ToStr(drReferringProvider[dsPatientReferral.Referrals.RefProviderAddressColumn.ColumnName])) ? string.Empty : string.Concat(", ", MDVUtility.ToStr(dr[dsPatientReferral.Referrals.RefProviderAddressColumn.ColumnName])),
                             string.IsNullOrWhiteSpace(MDVUtility.ToStr(drReferringProvider[dsPatientReferral.Referrals.RefProviderCityColumn.ColumnName])) ? string.Empty : string.Concat(", ", MDVUtility.ToStr(dr[dsPatientReferral.Referrals.RefProviderCityColumn.ColumnName])),
                             string.IsNullOrWhiteSpace(MDVUtility.ToStr(drReferringProvider[dsPatientReferral.Referrals.RefProviderStateColumn.ColumnName])) ? string.Empty : string.Concat(", ", MDVUtility.ToStr(dr[dsPatientReferral.Referrals.RefProviderStateColumn.ColumnName])),
                             string.IsNullOrWhiteSpace(MDVUtility.ToStr(drReferringProvider[dsPatientReferral.Referrals.RefProviderZipCodeColumn.ColumnName])) ? string.Empty : string.Concat(", ", MDVUtility.ToStr(dr[dsPatientReferral.Referrals.RefProviderZipCodeColumn.ColumnName]))
                             );
                            }
                            //Referral = string.Format("{0}{1}", Referral, string.IsNullOrWhiteSpace(MDVUtility.ToStr(dr[dsPatientReferral.Referrals.DateColumn.ColumnName])) ? string.Empty : string.Concat(", Scheduled data:", MDVUtility.ToDateTime(dr[dsPatientReferral.Referrals.DateColumn.ColumnName]).ToString("MM/dd/yyyy")));

                            lstReasonReferral.Add(new Dictionary<string, string> { { "Reason", MDVUtility.ToStr(Referral) }, });
                        }
                    }
                    #endregion


                    #region Lab Orders/Tests

                    if (dsClinicalSummaryHTMLData.Tables[dsLabOrder.LabOrderTest.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsLabOrder.LabOrderTest.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsLabOrder.LabOrderTest.TableName].Rows)
                        {
                            string TestName = MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.CPTCodeDescriptionColumn.ColumnName]);


                            if (!string.IsNullOrWhiteSpace(TestName))
                            {
                                var PlanOfCareDatakeyValues = new Dictionary<string, string>
                                {
                                    { "Name", TestName},
                                    { "Details" , MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.SoapTextColumn.ColumnName]) },
                                    { "TestDate", MDVUtility.ToDateTime(dr[dsLabOrder.LabOrderTest.CreatedOnColumn.ColumnName]).ToShortDateString()},
                                    { "CPTCode" , MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.CPTCodeColumn.ColumnName]) },
                                    { "CPTCodeDescription", TestName},
                                };
                                lstLabOrderTests.Add(PlanOfCareDatakeyValues);
                            }
                        }
                    }

                    #endregion

                    #region Medical Equipment

                    if (devicesList != null && devicesList.Count > 0)
                    {
                        foreach (var item in devicesList)
                        {
                            var PlanOfCareDatakeyValues = new Dictionary<string, string>
                                {
                                    { "Implanted", item.GMDNPName},
                                    { "Area" , item.Rx },
                                    { "Manufacturer", item.CompanyName},
                                    { "DeviceIdentifier" , item.DI },
                                    { "Details", item.DeviceDescription},
                                    { "Model", item.VersionModelNumber},
                                    { "Serial", item.Serial_Number},
                                    { "Lot", item.Lot_Number},
                                    { "Manufacturing_Date",item.Manufacturing_Date},
                                    { "UDI",item.UDI},
                                };
                            lstImplantableDevice.Add(PlanOfCareDatakeyValues);
                        }
                    }

                    #endregion  Medical Equipment

                    #region Insurance Data

                    if (dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dsInsurance in dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.TableName].Rows)
                        {

                            var InsuranceDatakeyValues = new Dictionary<string, string>
                        {
                            { "DOB", String.IsNullOrEmpty(MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.DOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.DOBColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "City", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.CityColumn.ColumnName])},
                            { "State", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.StateColumn.ColumnName])},
                            { "ZIPCode", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.ZIPCodeColumn.ColumnName])},
                            { "PlanAddress", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PlanAddressColumn.ColumnName])},
                            { "MI", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.MIColumn.ColumnName])},
                            { "FirstName", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.FirstNameColumn.ColumnName])},
                            { "LastName", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.LastNameColumn.ColumnName])},
                            { "RelationShip", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.RelationNameColumn.ColumnName])},
                            { "PlanName", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.InsurancePlanNameColumn.ColumnName])},
                            { "Priority", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PlanPriorityColumn.ColumnName])},
                            { "BirthTime", String.IsNullOrEmpty(MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.DOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.DOBColumn.ColumnName]).ToString("yyyyMMdd")},
                            { "SubscriberId", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.SubscriberIdColumn.ColumnName])},
                            { "GroupId", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.GroupIdColumn.ColumnName])},
                            { "PhoneNo", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PhoneNoColumn.ColumnName])},
                            { "PatientFullName", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PatientFullNameColumn.ColumnName])},
                            { "PhoneExt", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PhoneExtColumn.ColumnName])},
                            { "PartyFullName", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PartyFullNameColumn.ColumnName])},
                            { "PartyDOB", String.IsNullOrEmpty(MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PartyDOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PartyDOBColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "PartyMI", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PartyMIColumn.ColumnName])},
                            { "PartyFirstName", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PartyFirstNameColumn.ColumnName])},
                            { "PartyLastName", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PartyLastNameColumn.ColumnName])},
                            { "PartyBirthTime", String.IsNullOrEmpty(MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PartyDOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.PartyDOBColumn.ColumnName]).ToString("yyyyMMdd")},
                             { "InsurancePlanId", MDVUtility.ToStr(dsInsurance[dsClinicalSummaryHTMLData.PatientInsurance_CCDA.InsurancePlanIdColumn.ColumnName])},

                        };
                            lstInsurance.Add(InsuranceDatakeyValues);
                        }

                    }

                    #endregion

                    #region Cognitive Mental Status

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.MentalStatus_CCDA.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.MentalStatus_CCDA.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.MentalStatus_CCDA.TableName].Rows)
                        {
                            var DatakeyValues = new Dictionary<string, string>
                                    {
                                        { "CreatedOn",   MDVUtility.ToStr(dr[dsPlanOfCare.MentalStatus_CCDA.CreatedOnColumn.ColumnName])},
                                        { "SNOMEDID",   MDVUtility.ToStr(dr[dsPlanOfCare.MentalStatus_CCDA.SNOMEDIDColumn.ColumnName])},
                                        { "SNOMEDDescription", MDVUtility.ToStr(dr[dsPlanOfCare.MentalStatus_CCDA.SNOMEDDescriptionColumn.ColumnName])},
                                        { "Instruction" ,  MDVUtility.ToStr(dr[dsPlanOfCare.MentalStatus_CCDA.InstructionColumn.ColumnName]) }

                                    };
                            lstMentalStatus.Add(DatakeyValues);
                        }
                    }

                    #endregion Cognitive Mental Status

                    #region Problems Data and  EncounterDiagnosic

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.OutPatientEncounter.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.OutPatientEncounter.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.OutPatientEncounter.TableName].Rows)
                        {
                            var SNOMEDDes = string.Concat(MDVUtility.ToStr(dr[dsPlanOfCare.OutPatientEncounter.SNOMED_DescriptionColumn.ColumnName]));
                            if (!string.IsNullOrWhiteSpace(SNOMEDDes)) //if SNOMED description not found.
                            {
                                if (MDVUtility.ToStr(dr[dsPlanOfCare.OutPatientEncounter.SNOMEDIDColumn.ColumnName]) != string.Empty)
                                    SNOMEDDes = string.Concat(SNOMEDDes, " [SNOMEDID:", MDVUtility.ToStr(dr[dsPlanOfCare.OutPatientEncounter.SNOMEDIDColumn.ColumnName]), "]");
                                var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "ProblemNamewithSnomed",  SNOMEDDes},
                                        { "ProblemName",  MDVUtility.ToStr(dr[dsPlanOfCare.OutPatientEncounter.SNOMED_DescriptionColumn.ColumnName])},
                                        { "VisitDate", MDVUtility.ToStr(dr[dsPlanOfCare.OutPatientEncounter.VisitDateColumn.ColumnName])},
                                        { "Type", "Problem"},
                                        { "SNOMEDID",MDVUtility.ToStr(dr[dsPlanOfCare.OutPatientEncounter.SNOMEDIDColumn.ColumnName])},
                                        { "IsFollowUp",  Convert.ToBoolean(dr[dsPlanOfCare.OutPatientEncounter.IsFollowUpColumn.ColumnName])?"Yes":"No"},
                                        { "IsNewPatient",  Convert.ToBoolean(dr[dsPlanOfCare.OutPatientEncounter.IsNewPatientColumn.ColumnName])?"Yes":"No"},
                                        { "IsPatientRefferd",  Convert.ToBoolean(dr[dsPlanOfCare.OutPatientEncounter.IsPatientRefferdColumn.ColumnName])?"Yes":"No"},
                                        { "TotalVisit",  MDVUtility.ToStr(dr[dsPlanOfCare.OutPatientEncounter.TotalVisitColumn.ColumnName])},
                                    };

                                lstOutPatientEncounter.Add(ProblemDatakeyValues);
                            }
                        }
                    }

                    #endregion Problems Data and  EncounterDiagnosic

                    #region AUP Report

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.AUP_Report.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.AUP_Report.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.AUP_Report.TableName].Rows)
                        {
                            var DatakeyValues = new Dictionary<string, string>
                                    {
                                        { "FacilityId", MDVUtility.ToStr(dr[dsPlanOfCare.AUP_Report.FacilityOrgIdColumn.ColumnName]) },
                                        { "FacilityName",  MDVUtility.ToStr(dr[dsPlanOfCare.AUP_Report.FacilityNameColumn.ColumnName])},
                                        { "Location", MDVUtility.ToStr(dr[dsPlanOfCare.AUP_Report.LocationColumn.ColumnName])},
                                        { "AntimicrobialDays",MDVUtility.ToStr(dr[dsPlanOfCare.AUP_Report.AntimicrobialDaysColumn.ColumnName])},
                                        { "RouteIM",  MDVUtility.ToStr(dr[dsPlanOfCare.AUP_Report.RouteIMColumn.ColumnName])},
                                        { "RouteIV",  MDVUtility.ToStr(dr[dsPlanOfCare.AUP_Report.RouteIVColumn.ColumnName])},
                                        { "RouteDigestive",  MDVUtility.ToStr(dr[dsPlanOfCare.AUP_Report.RouteDigestiveColumn.ColumnName])},
                                        { "RouteRespiratory",  MDVUtility.ToStr(dr[dsPlanOfCare.AUP_Report.RouteRespiratoryColumn.ColumnName])},
                                        { "RxnormID",  MDVUtility.ToStr(dr[dsPlanOfCare.AUP_Report.RxnormIDColumn.ColumnName])},
                                        { "MedicationName",  MDVUtility.ToStr(dr[dsPlanOfCare.AUP_Report.MedicineColumn.ColumnName])},
                                    };

                            lstAUP.Add(DatakeyValues);
                        }
                    }

                    #endregion  AUP Report

                    #region ARO Report

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ARO_FacilitySpecimen.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ARO_FacilitySpecimen.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ARO_FacilitySpecimen.TableName].Rows)
                        {
                            var DatakeyValues = new Dictionary<string, string>
                                    {
                                        { "FacilityId", MDVUtility.ToStr(dr[dsPlanOfCare.ARO_FacilitySpecimen.FacilityIdColumn.ColumnName]) },
                                        { "FacilityName",  MDVUtility.ToStr(dr[dsPlanOfCare.ARO_FacilitySpecimen.FacilityNameColumn.ColumnName])},
                                        { "Location", MDVUtility.ToStr(dr[dsPlanOfCare.ARO_FacilitySpecimen.LocationColumn.ColumnName])},
                                        { "SpecimenGroup",MDVUtility.ToStr(dr[dsPlanOfCare.ARO_FacilitySpecimen.SpecimenGroupColumn.ColumnName])},
                                        { "DateSpecimenCollected",  MDVUtility.ToStr(dr[dsPlanOfCare.ARO_FacilitySpecimen.DateSpecimenCollectedColumn.ColumnName])},
                                        { "PathogenDescription",  MDVUtility.ToStr(dr[dsPlanOfCare.ARO_FacilitySpecimen.AROPathogenCategoryColumn.ColumnName])},
                                        { "SNOMEDID",  MDVUtility.ToStr(dr[dsPlanOfCare.ARO_FacilitySpecimen.SNOMEDIDColumn.ColumnName])},
                                    };

                            lstAROOrganizm.Add(DatakeyValues);
                        }
                    }
                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ARO_Antimicrobial.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ARO_Antimicrobial.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ARO_Antimicrobial.TableName].Rows)
                        {
                            var DatakeyValues = new Dictionary<string, string>
                                    {
                                        { "RxNormID", MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Antimicrobial.RxnormIDColumn.ColumnName]) },
                                        { "AntimicrobialId",  MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Antimicrobial.AntimicrobialIdColumn.ColumnName])},
                                        { "BrandName", MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Antimicrobial.BrandnameColumn.ColumnName])},
                                        { "FinalInterpretation",MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Antimicrobial.FinalinterpretationColumn.ColumnName])},
                                    };

                            lstAROAntimicrobial.Add(DatakeyValues);
                        }
                    }
                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ARO_Observations.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ARO_Observations.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.ARO_Observations.TableName].Rows)
                        {
                            string resultDesc = "";
                            string result = MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Observations.ResultColumn.ColumnName]);
                            string flag = MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Observations.FlagColumn.ColumnName]);
                            string UOM = MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Observations.UOMColumn.ColumnName]);
                            string condition = MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Observations.ConditionStatementColumn.ColumnName]);

                            if (!string.IsNullOrWhiteSpace(result) && !string.IsNullOrWhiteSpace(flag) && !string.IsNullOrWhiteSpace(condition))
                            {
                                switch (MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Observations.ConditionStatementColumn.ColumnName]))
                                {
                                    case ">":
                                        resultDesc = "Greater Than " + result + " " + UOM + " " + flag;
                                        break;
                                    case ">=":
                                        resultDesc = "Greater Than or equal to " + result + " " + UOM + " " + flag;
                                        break;
                                    case "<":
                                        resultDesc = "Less Than " + result + " " + UOM + " " + flag;
                                        break;
                                    case "<=":
                                        resultDesc = "Less Than or equal to " + result + " " + UOM + " " + flag;
                                        break;
                                    case "=":
                                        resultDesc = "Equal to " + result + " " + UOM + " " + flag;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                resultDesc = "Not Applicable";
                            }

                            var DatakeyValues = new Dictionary<string, string>
                                    {
                                        { "LOINC", MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Observations.LOINCColumn.ColumnName]) },
                                        { "AntimicrobialId",  MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Observations.AntimicrobialIdColumn.ColumnName])},
                                        { "LOINCDescription", MDVUtility.ToStr(dr[dsPlanOfCare.ARO_Observations.LOINCDescriptionColumn.ColumnName])},
                                        { "Result",result},
                                        { "UOM",UOM},
                                        { "ConditionStatement",condition},
                                        { "Flag",flag},
                                        { "ResultDesc",resultDesc},
                                        { "PathogenDescription",  MDVUtility.ToStr(dr[dsPlanOfCare.ARO_FacilitySpecimen.AROPathogenCategoryColumn.ColumnName])},

                                    };

                            lstAROObservations.Add(DatakeyValues);
                        }
                    }
                    #endregion  ARO Report

                    #region Employment History

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.OccupationStatus.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.OccupationStatus.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.OccupationStatus.TableName].Rows)
                        {
                            var DatakeyValues = new Dictionary<string, string>
                                    {
                                        { "ConceptCode", MDVUtility.ToStr(dr[dsPlanOfCare.OccupationStatus.ConceptCodeColumn.ColumnName]) },
                                        { "Description",  MDVUtility.ToStr(dr[dsPlanOfCare.OccupationStatus.DescriptionColumn.ColumnName])},
                                        { "IsOccupation", MDVUtility.ToStr(dr[dsPlanOfCare.OccupationStatus.IsOccupationColumn.ColumnName])},
                                        { "Present",MDVUtility.ToStr(dr[dsPlanOfCare.OccupationStatus.PresentColumn.ColumnName])},
                                        { "Past",  MDVUtility.ToStr(dr[dsPlanOfCare.OccupationStatus.PastColumn.ColumnName])},
                                        { "StartDate",  MDVUtility.ToStr(dr[dsPlanOfCare.OccupationStatus.StartDateColumn.ColumnName])},
                                        { "EndDate",  MDVUtility.ToStr(dr[dsPlanOfCare.OccupationStatus.EndDateColumn.ColumnName])},
                                        { "OccupationDetail",  MDVUtility.ToStr(dr[dsPlanOfCare.OccupationStatus.OccupationDetailColumn.ColumnName])},
                                    };

                            lstEmploymentHx.Add(DatakeyValues);
                        }
                    }

                    #endregion Employment History

                    #region Patient Data

                    if (dsClinicalSummaryHTMLData.Tables[dsPatient.PatientParticipants.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPatient.PatientParticipants.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drPatient in dsClinicalSummaryHTMLData.Tables[dsPatient.PatientParticipants.TableName].Rows)
                        {

                            var PatientDatakeyValues = new Dictionary<string, string>
                            {
                                { "Address", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.Address1Column.ColumnName])},
                                { "City", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.CityColumn.ColumnName])},
                                { "State", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.StateColumn.ColumnName])},
                                { "ZIPCode", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.ZipcodeColumn.ColumnName])},
                                { "Country", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.Address2Column.ColumnName])},
                                { "CellNo", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.CellNoColumn.ColumnName])},
                                { "MiddleInitial", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.MIColumn.ColumnName])},
                                { "HomePhoneNo", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.HomePhoneNoColumn.ColumnName])},
                                { "WorkPhoneNo", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.WorkPhoneNoColumn.ColumnName])},
                                { "WorkPhoneExt", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.WorkPhExtColumn.ColumnName])},
                                { "MI", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.MIColumn.ColumnName])},
                                { "FirstName", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.FirstNameColumn.ColumnName])},
                                { "LastName", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.LastNameColumn.ColumnName])},
                                { "RelationShipName", MDVUtility.ToStr(drPatient[dsPatient.PatientParticipants.RelationShipNameColumn.ColumnName])},
                            };
                            lstPatientParticipant.Add(PatientDatakeyValues);
                        }
                    }

                    #endregion

                    #endregion dsNotesData

                    CCDADataModel objCCDAModel = new CCDADataModel(dtNoteDate.ToString(),
                                                                    lstReasonForVisit,
                                                                    model.Components,
                                                                    lstPatientData,
                                                                    lstProviderData,
                                                                    lstPracticeData,
                                                                    lstNoteData,
                                                                    lstProblems,
                                                                    lstProblemsCancer,
                                                                    lstVitals,
                                                                    lstAllergs,
                                                                    lstSocials,
                                                                    lstMedicalHx,
                                                                    lstSurgicalHx,
                                                                    lstHospitalizationHx,
                                                                    lstFamilyHx,
                                                                    lstBirthHx,
                                                                    lstPhysicalExam,
                                                                    lstMedication,
                                                                    lstMedicationsAdministered,
                                                                    lstProcedure,
                                                                    lstImmunization,
                                                                    lstPlanOfCare,
                                                                    lstscheduledProcedure,
                                                                    lstFutureAppointment,
                                                                    lstGoal,
                                                                    lstResults,
                                                                    lstRefferalProviderData,
                                                                    lstCognitiveFunctionalStatus,
                                                                    lstReasonReferral,
                                                                    lstPartialResultPending,
                                                                    lstInstructionsAndDecisionAids,
                                                                    lstEncounterDiagnosicDatakeyValues,
                                                                    lstLabOrderTests,
                                                                    lstImplantableDevice,
                                                                    lstRaceCodes,
                                                                    lstEthnicityCodes,
                                                                    lstCaregivers,
                                                                    lstCareManagers,
                                                                    lstCareCoordinators,
                                                                    lstCareTeamPCP,
                                                                    lstCareTeamProvider,
                                                                    model.IsConfidential,
                                                                    lstMentalStatus,
                                                                    lstInsurance,
                                                                    lstHealthConsern,
                                                                    lstHealthObservation,
                                                                    lstHealthRisks,
                                                                    lstPlanedMedications,
                                                                    lstInterventions,
                                                                    lstOutcomes,
                                                                    lstAROOrganizm,
                                                                    lstAUP,
                                                                    lstOutPatientEncounter,
                                                                    lstAROAntimicrobial,
                                                                    lstAROObservations,
                                                                    lstEmploymentHx,
                                                                    lstRadiologyResults,
                                                                    lstPatientParticipant);

                    var ccdaDocument = CCDAGenrator.MakeCcda(objCCDAModel, noteId, providerId, patientId, StreamType, documentType);


                    if (isEncryptionRequired)
                    {
                        byte[] toBytes = Encoding.UTF8.GetBytes(ccdaDocument);
                        var response = new
                        {
                            status = true,
                            xmlData = Convert.ToBase64String(toBytes)
                        };

                        if (documentType == CCDAGenrator.DocumentTemplateType.DataPortability)
                            return Convert.ToBase64String(toBytes);
                        else
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            xmlData = ccdaDocument
                        };
                        return response.xmlData;
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                    };
                    if (documentType == CCDAGenrator.DocumentTemplateType.DataPortability)
                        return string.Empty;
                    else
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string loadDataPortabilty(ClinicalSummaryModel model)
        {
            int componentId = 0;
            model.Components = new List<Component>();
            model.Components.Add(new Component { componentId = --componentId, componentName = "problem" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "vital" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "allerg" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "social" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "medicalhx" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "surgicalhx" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "hospitalizationhx" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "familyhx" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "birthhx" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "physicalex" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "medication" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "procedure" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "procedure" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "immunization" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "labresults" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "planofcare" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "cognitive" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "refferral" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "encounterdiagnostic" });

            List<Dictionary<string, string>> PatientsDataPortability = new List<Dictionary<string, string>>();
            if (model.PatientInfo != null)
            {
                foreach (var Patient in model.PatientInfo)
                {
                    Dictionary<string, string> PatientData = new Dictionary<string, string>();

                    model.PatientId = Patient["PatientId"];
                    model.ProviderId = Patient["ProviderId"];
                    string data = loadClinicalSummaryXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, "xml", CCDAGenrator.DocumentTemplateType.DataPortability);
                    var dataBytes = Convert.FromBase64String(data);

                    string XMLString = Encoding.UTF8.GetString(dataBytes);

                    //For Getting The Patient name from XML
                    XmlDocument doc = new XmlDocument();
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("ab", "urn:hl7-org:v3");
                    doc.LoadXml(XMLString);
                    var PatientName = "";
                    if (doc.SelectSingleNode("/ab:ClinicalDocument/ab:recordTarget/ab:patientRole/ab:patient/ab:name/ab:given", nsmgr) == null)
                    {
                        PatientName = "";
                    }else
                    {
                        PatientName = doc.SelectSingleNode("/ab:ClinicalDocument/ab:recordTarget/ab:patientRole/ab:patient/ab:name/ab:given", nsmgr).InnerText;
                    }

                    string html = CCDAGenrator.GetHtmlFromXml(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), Encoding.UTF8.GetString(dataBytes));
                    var HtmlBytes = System.Text.Encoding.UTF8.GetBytes(html);
                    var base64Html = Convert.ToBase64String(HtmlBytes);

                    PatientData.Add("PatientId", model.PatientId);
                    PatientData.Add("ProviderId", model.ProviderId);
                    PatientData.Add("PatientName", PatientName);
                    PatientData.Add("Data", data);
                    PatientData.Add("HTMLData", base64Html);


                    PatientsDataPortability.Add(PatientData);
                }
            }
            return (Newtonsoft.Json.JsonConvert.SerializeObject(PatientsDataPortability));
        }


        public Dictionary<string, string> HashingAndEncryption(string XMLData, bool hashing, bool encryption, string password)
        {

            Dictionary<string, string> encryptContent = new Dictionary<string, string>();
            AESAlgorithm aes = new AESAlgorithm();

            if (hashing || encryption)
            {
                if (encryption)
                {
                    try
                    {
                        XMLData = XMLData.TrimEnd(new char[] { '\n', '\r' });
                        XMLData = aes.Encrypt(XMLData, password);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                if (hashing)
                {
                    try
                    {
                        var hashCode = aes.GenerateHash(XMLData);
                        encryptContent["hashCode"] = hashCode;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            encryptContent["xmlData"] = XMLData; //Base64 string

            return encryptContent;
        }

        //Method Name: insertUpdatePlanOfCare
        // Author:  Ahmad Raza
        // Created Date: 05/04/2016
        //OverView: insertUpdate PlanOfCare data
        public string insertUpdatePlanOfCare(ClinicalSummaryModel model, List<object> lstGoalObject)
        {
            try
            {
                DSClinicalSummary dsPlanOfCare = new DSClinicalSummary();
                BLObject<DSClinicalSummary> obj = BLLClinicalObj.loadPlanOfCare(MDVUtility.ToInt64(model.PlanOfCareId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), 1, 1000);

                dsPlanOfCare = obj.Data;
                if (obj.Data != null)
                {
                    DSClinicalSummary.PlanofCareRow planOfCareRow = null;
                    DSClinicalSummary.PlanofCareRow[] planOfCareRows = (DSClinicalSummary.PlanofCareRow[])dsPlanOfCare.PlanofCare.Select(dsPlanOfCare.PlanofCare.PlanofCareIdColumn + "=" + model.PlanOfCareId);
                    if (planOfCareRows.Length > 0)
                    {
                        planOfCareRow = planOfCareRows[0];
                    }
                    else
                    {
                        planOfCareRow = dsPlanOfCare.PlanofCare.NewPlanofCareRow();
                    }
                    if (planOfCareRow != null)
                    {
                        planOfCareRow.PatientId = MDVUtility.ToInt64(model.PatientId);
                        planOfCareRow.ClinicalInstruction = MDVUtility.ToStr(model.ClinicalInstruction);
                        planOfCareRow.FutureInstruction = MDVUtility.ToStr(model.FutureInstruction);
                        planOfCareRow.PatientDecisionAid = MDVUtility.ToStr(model.PatientDecisionAid);
                        planOfCareRow.NoteId = MDVUtility.ToInt64(model.NoteId);
                        planOfCareRow.IsActive = true;
                        if (planOfCareRows.Length == 0)
                        {
                            planOfCareRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            planOfCareRow.CreatedOn = DateTime.Now;
                        }
                        planOfCareRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        planOfCareRow.ModifiedOn = DateTime.Now;
                        planOfCareRow.SoapText = model.SoapText;
                        if (planOfCareRows.Length < 1)
                        {
                            dsPlanOfCare.PlanofCare.AddPlanofCareRow(planOfCareRow);
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSClinicalSummary> objPlanOfCare = BLLClinicalObj.insertUpdatePlanOfCare(dsPlanOfCare);
                    dsPlanOfCare = objPlanOfCare.Data;

                    if (objPlanOfCare.Data != null)
                    {
                        long goalId = 0;
                        Int64 planOfCareId = MDVUtility.ToInt64(dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows[0][dsPlanOfCare.PlanofCare.PlanofCareIdColumn.ColumnName]);
                        if (planOfCareId > 0)
                        {
                            if (lstGoalObject.Count > 0)
                            {
                                long responsefamilyhxdisease = insertUpdatePlanOfCareGoal(planOfCareId, lstGoalObject);
                                goalId = responsefamilyhxdisease;
                            }
                        }

                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForPlanOfCare(planOfCareId);
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            planOfCareId = MDVUtility.ToInt64(dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows[0][dsPlanOfCare.PlanofCare.PlanofCareIdColumn.ColumnName]),
                            goalId = goalId,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        //Method Name: insertUpdatePlanOfCareGoal
        // Author:  Ahmad Raza
        // Created Date: 05/04/2016
        //OverView: insertUpdate PlanOfCare Goal data
        private Int64 insertUpdatePlanOfCareGoal(long planOfCareId, List<object> lstGoalObject)
        {
            #region Goal
            DSClinicalSummary dsPlanOfCare = new DSClinicalSummary();
            List<PlanOfCareGoalModel> lstGoal = lstGoalObject.OfType<PlanOfCareGoalModel>().ToList();
            bool isFirstChild = false;
            foreach (PlanOfCareGoalModel CurrentModel in lstGoal)
            {
                if (CurrentModel.GoalId != null)
                {
                    Int64 currentGoalId = MDVUtility.ToInt64(CurrentModel.GoalId);
                    currentGoalId = currentGoalId == 0 ? -1 : currentGoalId;
                    BLObject<DSClinicalSummary> objGoal = BLLClinicalObj.loadPlanOfCareGoal(planOfCareId, currentGoalId);
                    dsPlanOfCare = objGoal.Data;
                    DSClinicalSummary.PlanOfCareGoalRow RowGoal = null;
                    if (dsPlanOfCare.PlanOfCareGoal.Rows.Count > 0)
                    {
                        RowGoal = (DSClinicalSummary.PlanOfCareGoalRow)dsPlanOfCare.PlanOfCareGoal.Rows[0];
                    }
                    else
                    {
                        RowGoal = dsPlanOfCare.PlanOfCareGoal.NewPlanOfCareGoalRow();
                    }

                    if (RowGoal != null)
                    {
                        bool isValueDifferent = false;
                        bool istoUpdateRow = false;
                        if (dsPlanOfCare.PlanOfCareGoal.Rows.Count < 1)
                        {
                            RowGoal.GoalId = currentGoalId;
                        }
                        RowGoal.PlanOfCareId = planOfCareId;

                        if (!string.IsNullOrEmpty(CurrentModel.ICD9Code))
                        {
                            RowGoal.ICD9Code = MDVUtility.ToStr(CurrentModel.ICD9Code);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.ICD9CodeColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.ICD9CodeDescription))
                        {
                            RowGoal.ICD9CodeDescription = MDVUtility.ToStr(CurrentModel.ICD9CodeDescription);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.ICD9CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10Code))
                        {
                            RowGoal.ICD10Code = MDVUtility.ToStr(CurrentModel.ICD10Code);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.ICD10CodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10CodeDescription))
                        {
                            RowGoal.ICD10CodeDescription = MDVUtility.ToStr(CurrentModel.ICD10CodeDescription);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.ICD10CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDID))
                        {
                            RowGoal.SNOMEDID = MDVUtility.ToStr(CurrentModel.SNOMEDID);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.SNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDDescription))
                        {
                            RowGoal.SNOMEDDescription = MDVUtility.ToStr(CurrentModel.SNOMEDDescription);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.SNOMEDDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCode))
                        {
                            RowGoal.LexiCode = MDVUtility.ToStr(CurrentModel.LexiCode);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.LexiCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCodeDescription))
                        {
                            RowGoal.LexiCodeDescription = MDVUtility.ToStr(CurrentModel.LexiCodeDescription);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.LexiCodeDescriptionColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.Instructions))
                        {
                            RowGoal.Instruction = MDVUtility.ToStr(CurrentModel.Instructions);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.LexiCodeDescriptionColumn] = DBNull.Value;
                        }
                        RowGoal.IsActive = true;
                        RowGoal.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowGoal.CreatedOn = DateTime.Now;
                        RowGoal.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowGoal.ModifiedOn = DateTime.Now;

                        if (dsPlanOfCare.PlanOfCareGoal.Rows.Count < 1)
                        {
                            dsPlanOfCare.PlanOfCareGoal.AddPlanOfCareGoalRow(RowGoal);
                        }
                    }
                }
            }
            int counter = 0;
            // start//26/01/2016//Ahmad Raza//for soap text 
            foreach (DataRow RowGoal in dsPlanOfCare.PlanOfCareGoal.Rows)
            {
                RowGoal[dsPlanOfCare.PlanOfCareGoal.SoapTextColumn] = getSoapTextForPlanOfCareGoal(dsPlanOfCare, lstGoal[counter]);
                counter++;
            }
            //// End//26/01/2016//Ahmad Raza//for soap text 
            //#region Database Insertion/Updation

            Int64 goalId = 0;

            BLObject<DSClinicalSummary> objInsertedGoal = BLLClinicalObj.insertUpdatePlanOfCareGoal(dsPlanOfCare);
            if (objInsertedGoal.Data != null)
            {
                goalId = dsPlanOfCare.Tables[dsPlanOfCare.PlanOfCareGoal.TableName].Rows.Count > 0 ? MDVUtility.ToInt64(dsPlanOfCare.Tables[dsPlanOfCare.PlanOfCareGoal.TableName].Rows[0][dsPlanOfCare.PlanOfCareGoal.GoalIdColumn.ColumnName]) : 0;
                return goalId;
            }
            else
            {
                return 0;
            }

            #endregion



        }

        //Method Name: getSoapTextForPlanOfCareGoal
        // Author:  Ahmad Raza
        // Created Date: 05/04/2016
        //OverView: This function will get SoapText For PlanOfCare Goal
        internal string getSoapTextForPlanOfCareGoal(DSClinicalSummary dsPlanOfCareGoal, PlanOfCareGoalModel modelObj)
        {
            string soapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                sb.Append("<div id='planOfCareGoal_" + modelObj.GoalId + "' title='PlanOfCare'  name='Plan Of Care'><strong>Plan Of Care: </strong>");
                sb.Append((string.IsNullOrEmpty(modelObj.GoalText) ? "" : "Goal: " + modelObj.GoalText) + (string.IsNullOrEmpty(modelObj.Instructions) ? "" : ", Instruction: " + modelObj.Instructions + "</br>") + "</div>");
                //  sb.Append((string.IsNullOrEmpty(modelObj.ClinicalInstructions) ? "" : "Clinical Instruction: " + (modelObj.ClinicalInstructions) + "</br>") + (string.IsNullOrEmpty(modelObj.FutureScheduleAppointments) ? "" : "Future Appointments: " + (modelObj.FutureScheduleAppointments) + "</br>") + (string.IsNullOrEmpty(modelObj.PatientDecisionAid) ? "" : "Patient Decision Aid: " + (modelObj.PatientDecisionAid) + "</br>") + "</div>");
            }
            else
            {
                return string.Empty;
            }
            return sb.ToString();
        }

        //Method Name: fillPlanOfCare
        // Author:  Ahmad Raza
        // Created Date: 05/04/2016
        //OverView: This function will fill PlanOfCare data
        public string fillPlanOfCare(ClinicalSummaryModel model, Int64 planOfCareId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)) && planOfCareId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSClinicalSummary dsPlanOfCare = null;
                    BLObject<DSClinicalSummary> obj = BLLClinicalObj.loadPlanOfCare(planOfCareId, MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), 1, 1000, "1", "");
                    dsPlanOfCare = obj.Data;
                    if (dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows[0];
                        Int64 PlanId = MDVUtility.ToInt64(dr[dsPlanOfCare.PlanofCare.PlanofCareIdColumn.ColumnName]);
                        var PlanofCarekeyValues = new Dictionary<string, string>
                        {
                            { "PlanOfCareId",  MDVUtility.ToStr(dr[dsPlanOfCare.PlanofCare.PlanofCareIdColumn.ColumnName])},
                            { "PlanOfCareSoapText", MDVUtility.ToStr(dr[dsPlanOfCare.PlanofCare.SoapTextColumn.ColumnName])},
                            { "ClinicalInstructions", MDVUtility.ToStr(dr[dsPlanOfCare.PlanofCare.ClinicalInstructionColumn.ColumnName])},
                            { "FutureAppointments", MDVUtility.ToStr(dr[dsPlanOfCare.PlanofCare.FutureInstructionColumn.ColumnName])},
                            { "PatientDecisionAid", MDVUtility.ToStr(dr[dsPlanOfCare.PlanofCare.PatientDecisionAidColumn.ColumnName])}
                        };


                        DSClinicalSummary dsPlanOfCareGoal = null;
                        BLObject<DSClinicalSummary> objGoal = BLLClinicalObj.loadPlanOfCareGoal(PlanId, 0);
                        dsPlanOfCareGoal = objGoal.Data;

                        if (dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName].Rows.Count > 0)
                        {
                            DataRow drGoal = dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName].Rows[0];

                            var PlanofCareGoalkeyValues = new Dictionary<string, string>
                        {
                                { "GoalId",  MDVUtility.ToStr(drGoal[dsPlanOfCareGoal.PlanOfCareGoal.GoalIdColumn.ColumnName])},
                                { "Instructions", MDVUtility.ToStr(drGoal[dsPlanOfCareGoal.PlanOfCareGoal.InstructionColumn.ColumnName])},
                                { "PlanOfCareId", MDVUtility.ToStr(drGoal[dsPlanOfCareGoal.PlanOfCareGoal.PlanOfCareIdColumn.ColumnName])},
                        };

                            ////Start 13/01/2016 Muhammad Irfan Fill Diseases in MedicalHx
                            List<Dictionary<string, string>> lstGoal = new List<Dictionary<string, string>>();
                            // var GoalkeyValues = new Dictionary<string, string> { { "", "" } };
                            if (MDVUtility.ToInt64(model.GoalId) > 0)
                            {

                                DSClinicalSummary dsPlanOfCareGoal1 = null;
                                BLObject<DSClinicalSummary> objGoal1 = BLLClinicalObj.loadPlanOfCareGoal(PlanId, MDVUtility.ToInt64(model.GoalId));
                                dsPlanOfCareGoal1 = objGoal1.Data;
                                if (dsPlanOfCareGoal1.Tables[dsPlanOfCareGoal1.PlanOfCareGoal.TableName].Rows.Count > 0)
                                {
                                    DataRow drGoal1 = dsPlanOfCareGoal1.Tables[dsPlanOfCareGoal1.PlanOfCareGoal.TableName].Rows[0];

                                    var GoalkeyValues = new Dictionary<string, string>
                        {
                                { "GoalId",  MDVUtility.ToStr(drGoal1[dsPlanOfCareGoal1.PlanOfCareGoal.GoalIdColumn.ColumnName])},
                                { "Instructions", MDVUtility.ToStr(drGoal1[dsPlanOfCareGoal1.PlanOfCareGoal.InstructionColumn.ColumnName])},
                                { "PlanOfCareId", MDVUtility.ToStr(drGoal1[dsPlanOfCareGoal1.PlanOfCareGoal.PlanOfCareIdColumn.ColumnName])},
                        };

                                    ////End 13/01/2016 Muhammad Irfan Fill Diseases in MedicalHx


                                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    var response = new
                                    {
                                        status = true,
                                        PlanOfCareFill_JSON = js.Serialize(PlanofCarekeyValues),
                                        GoalFill_JSON = js.Serialize(GoalkeyValues),
                                        GoalLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName])),
                                        PlanOfCareGoalLoad_JSON = MDVUtility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName]),
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                }
                                else
                                {
                                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    var response = new
                                    {
                                        status = true,
                                        PlanOfCareFill_JSON = js.Serialize(PlanofCarekeyValues),
                                        GoalFill_JSON = "[]",
                                        GoalLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName])),
                                        PlanOfCareGoalLoad_JSON = MDVUtility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName]),
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                }

                            }
                            else
                            {
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    PlanOfCareFill_JSON = js.Serialize(PlanofCarekeyValues),
                                    //  GoalFill_JSON = js.Serialize(GoalkeyValues),
                                    GoalLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName])),
                                    PlanOfCareGoalLoad_JSON = MDVUtility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else
                        {
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PlanOfCareFill_JSON = js.Serialize(PlanofCarekeyValues),
                                //GoalFill_JSON = js.Serialize(PlanofCareGoalkeyValues),
                                GoalLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName])),
                                PlanOfCareGoalLoad_JSON = MDVUtility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,

                            PlanOfCareFill_JSON = "[]",
                            GoalFill_JSON = "[]",
                            GoalLoad_JSON = "[]",
                            PlanOfCareGoalLoad_JSON = MDVUtility.JSON_DataTable(dsPlanOfCare.Tables[dsPlanOfCare.PlanOfCareGoal.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        //internal string attachPlanOfCareWithNotes(string planOfCareId, long notesId)
        //{
        //    try
        //    {
        //        DSClinicalSummary dsPlanOfCare = null;
        //        if (string.IsNullOrEmpty(MDVUtility.ToStr(planOfCareId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {
        //            BLObject<DSClinicalSummary> obj = BLLClinicalObj.attachPlanOfCareWithNotes(planOfCareId, notesId);
        //            if (obj.Data != null)
        //            {
        //                dsPlanOfCare = obj.Data;
        //                var response = new
        //                {
        //                    status = true,
        //                    PlanOfCareTotalCount = dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count,
        //                    PlanOfCareCount = dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count,
        //                    PlanOfCareLoad_JSON = MDVUtility.JSON_DataTable(dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName]),
        //                    Message = Common.AppPrivileges.Update_Message
        //                };

        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = obj.Data
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        //Method Name: detachPlanOfCareFromNotes
        // Author:  Ahmad Raza
        // Created Date: 05/04/2016
        //OverView: This function will handle detach of PlanOfCare From Notes
        internal string detachPlanOfCareFromNotes(string planOfCareId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(planOfCareId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.detachPlanOfCareFromNotes(planOfCareId, notesId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        //Method Name: attachPlanOfCareWithNotes
        // Author:  Ahmad Raza
        // Created Date: 05/04/2016
        //OverView: This function will handle attach of PlanOfCare with Notes
        internal string attachPlanOfCareWithNotes(ClinicalSummaryModel model)
        {
            try
            {
                DSClinicalSummary dsPlanOfCare = null;
                BLObject<DSClinicalSummary> obj = BLLClinicalObj.loadPlanOfCare(MDVUtility.ToInt64(model.PlanOfCareId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), 1, 2000);
                dsPlanOfCare = obj.Data;
                if (dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                {

                    obj = BLLClinicalObj.insertUpdatePlanOfCare(dsPlanOfCare);

                    var response = new
                    {
                        status = true,


                        PlanofCareLoad_JSON = MDVUtility.JSON_DataTable(dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName]),
                        Message = Common.AppPrivileges.Update_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    string response = null;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        public static string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        //Method Name: deletPlanOfCareGoal
        // Author:  Ahmad Raza
        // Created Date: 05/04/2016
        //OverView: This function will handle delete of PlanOfCare Goal 
        public string deletPlanOfCareGoal(string goalId, string planOfCareId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(goalId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.deletePlanOfCareGoal(MDVUtility.ToStr(goalId));
                    if (obj.Data == "")
                    {
                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForPlanOfCare(MDVUtility.ToInt64(planOfCareId));
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public DataRow[] VaccinceRows { get; set; }






        public object SendEmailThroughPhiMailConnector(string toEmail, string XMLText, string HTMLText, string phiMail_RemoteUser,string MessageDetail)
        {
            var message = "";
            PhiMailConnector pc = null;
            try
            {
                pc = new PhiMailConnector(MDVApplication.CCDA_RemoteHost, MDVUtility.ToInt32(MDVApplication.CCDA_RemotePort));
            }
            catch (Exception e)
            {
                message = e.Message;
                throw new Exception(message);
            }

            try
            {
                bool receive = true;
                pc.AuthenticateUser(phiMail_RemoteUser, MDVApplication.CCDA_RemotePassword);

                try
                {
                    string recip = pc.AddRecipient(toEmail);
                }
                catch (Exception e)
                {
                    message = "Aborting send; could not add recipient: " + e.Message;
                    throw new Exception(message);
                }

                pc.SetSubject("CCDA");

                // Add the main body of the message.
                if (MessageDetail.Length>0)
                {
                    pc.AddText(MessageDetail);
                }
                else
                {
                    pc.AddText("CCDA");
                }

                // Add a CDA attachment and use default attachment filename.
                XMLText = XMLText.Replace("\a", string.Empty);

                XMLText = XMLText.Replace("\b", string.Empty);
                XMLText = XMLText.Replace("\f", string.Empty);
                XMLText = XMLText.Replace("\n", string.Empty);
                XMLText = XMLText.Replace("\r", string.Empty);
                XMLText = XMLText.Replace("\t", string.Empty);
                XMLText = XMLText.Replace("\v", string.Empty);

                if (XMLText != string.Empty)
                    pc.AddCDA(XMLText);


                // Add an HTML attachment and specify the attachment filename.
                HTMLText = HTMLText.Replace("�", string.Empty);
                if (HTMLText != string.Empty)
                    pc.AddText(HTMLText, "HTMLData.htm");

                // Optionally, request a final delivery notification message.
                // Note that not all HISPs can provide this notification when requested.
                // If the receiving HISP does not support this feature, the message will
                // result in a failure notification after the timeout period has elapsed.
                // Additional information on final delivery notification can be found in
                // the API Guide.
                // This command will override the default setting set by the server.
                //
                // The default setting is false.
                pc.SetDeliveryNotification(true);

                // Send the message. sendRes will contain one entry for each recipient.
                // If more than one recipient was specified, then each would have an entry.
                List<PhiMailConnector.SendResult> sendRes = pc.Send();

                if (sendRes[0].Succeeded)
                {
                    // The MessageID is unique for each message/recipient pair and should 
                    // be saved since any future status notifications that might be received
                    // from the phiMail server relating to this message will also reference 
                    // this MessageID.
                   // message = "Transmit successful."+ sendRes[0].MessageId;
                    var output = new
                    {
                        status = true,
                        Message = sendRes[0].MessageId
                    };
                    return output;
                }
                else
                {
                    // Clear the current outgoing message on failure.
                    // Outgoing messages are automatically cleared if all recipients
                    // were successful.
                    pc.Clear();
                    message = "Send failed. " + sendRes[0].ErrorText;
                    throw new Exception(message);
                }

                // Sample code to check for any incoming messages. Generally, this
                // code would run in a separate background process to poll the
                // phiMail server at regular intervals for new messages. In production
                // phiMailUser above would be set to an address group to efficiently
                // retrieve messages for all addresses in the address group, rather
                // than iterating through individual addresses.  Please see the
                // API documentation for further information about address groups.
                //
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                pc.Close();
            }

            //return message;
        }
    }
}