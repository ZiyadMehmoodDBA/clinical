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

namespace MDVision.IEHR.EMR.Helpers.Clinical.Summary
{
    public class CaseReportsHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public CaseReportsHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static CaseReportsHelper _instance = null;

        public static CaseReportsHelper Instance()
        {
            if (_instance == null)
                _instance = new CaseReportsHelper();
            return _instance;
        }

        // Author:  Farooq Ahmad
        // Created Date: 11/04/2016
        //OverView: Load Clinical Summary XML Data
        public string loadCaseReportsXMLData(Int64 noteId, Int64 providerId, Int64 patientId, CaseReportsModel model, string StreamType, CaseReportsCCDAGenerator.DocumentTemplateType documentType, Int64 referralProvider = 0, string referralReason = "")
        {
            try
            {
                DSNotes dsCaseReportsHTMLData = new DSNotes();
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
                DSCaseReports dsPlanOfCare = new DSCaseReports();
                DSLabResult dsLabResult = new DSLabResult();
                DSRadiologyResult dsRadiologyResult = new DSRadiologyResult();
                DSPatientReferral dsPatientReferral = new DSPatientReferral();
                Dictionary<int, string> lstComopent = new Dictionary<int, string>();
                DSLabOrder dsLabOrder = new DSLabOrder();
                List<ImplantableDevices> devicesList = new List<ImplantableDevices>();
                DSClinicalComplaint dsComplaint = new DSClinicalComplaint();
                DSCCM dsCCM = new DSCCM();
                if (model.Components != null)
                {
                    foreach (Component lst in model.Components)
                    {
                        try
                        {
                            lstComopent.Add(lst.componentId, lst.componentName);
                            if (documentType == CaseReportsCCDAGenerator.DocumentTemplateType.ClinicalSummary)
                            {
                                lstComopent.Add((model.Components.Count + 1) * -1, "refferral");
                            }
                        }
                        catch (Exception ex) { }
                    }
                }

                BLObject<DSNotes> dsNotesData = BLLClinicalObj.loadCaseReportsHTMLData(patientId, providerId, noteId, lstComopent, referralProvider);

                // Seperate Triggered Data
                

                

                dsCaseReportsHTMLData = dsNotesData.Data;
                var mdEqup = lstComopent.Where(a => a.Value == "MedicalEquipment").Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(mdEqup))
                {
                    BLObject<List<ImplantableDevices>> obj = BLLClinicalObj.loadImplantableDeviceForCCDA(MDVUtility.ToInt64(model.PatientId), noteId);
                    if (obj.Data != null)
                    {
                        devicesList = obj.Data;
                    }
                }
                if (dsNotesData.Data != null)
                {
                    DateTime dtNoteDate = DateTime.Now;
                    string VisitReason = "";
                    if (dsCaseReportsHTMLData.Tables[dsCaseReportsHTMLData.Notes.TableName] != null && dsCaseReportsHTMLData.Tables[dsCaseReportsHTMLData.Notes.TableName].Rows.Count > 0)
                    {
                        dtNoteDate = MDVUtility.ToDateTime(dsCaseReportsHTMLData.Tables[dsCaseReportsHTMLData.Notes.TableName].Rows[0][dsCaseReportsHTMLData.Notes.VisitDateColumn.ColumnName]);
                        VisitReason = MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsCaseReportsHTMLData.Notes.TableName].Rows[0][dsCaseReportsHTMLData.Notes.VisitReasonColumn.ColumnName]);
                    }
                    #region dsNotesData
                    List<Dictionary<string, string>> lstPatientData = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstProcedure = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstProviderData = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstPracticeData = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstNoteData = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstProblems = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstVitals = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstAllergs = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstSocials = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstSocialOccupations = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstSocialTravels = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstSocialSexuals = new List<Dictionary<string, string>>();
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
                    List<Dictionary<string, string>> lstEncounterDiagnosicNonTriggerDatakeyValues = new List<Dictionary<string, string>>();
                    List<string> lstEncounterProblemId = new List<string>();
                    List<string> lstReasonForVisit = new List<string>();
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
                    List<Dictionary<string, string>> lstComplaints = new List<Dictionary<string, string>>();
                    string practiceName = "";
                    string providerFullName = "";
                    string providerOfficeAddress = "";
                    string providerOfficePhone = "";

                    #region Patient Data
                    var ReasonForVisit = string.Empty;
                    try
                    {
                        //if (dsCaseReportsHTMLData.Tables[dsPlanOfCare.ComplaintDetail.TableName] != null && dsCaseReportsHTMLData.Tables[dsPlanOfCare.ComplaintDetail.TableName].Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPlanOfCare.ComplaintDetail.TableName].Rows)
                        //    {
                        //        var Description = MDVUtility.ToStr(dr[dsPlanOfCare.ComplaintDetail.ComplaintDescriptionColumn.ColumnName]);
                        //        lstReasonForVisit.Add(Description);
                        //    }

                        //}
                        lstReasonForVisit.Add(VisitReason);
                    }
                    catch (Exception ex) { }
                    #region Patient Races

                    if (dsCaseReportsHTMLData.Tables[dsPatient.PatientRace.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsCaseReportsHTMLData.Tables[dsPatient.PatientRace.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsPatient.PatientEthnicity.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsCaseReportsHTMLData.Tables[dsPatient.PatientEthnicity.TableName].Rows)
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
                    if (dsCaseReportsHTMLData.Tables[dsCCM.CareGivers.TableName] != null &&
                        dsCaseReportsHTMLData.Tables[dsCCM.CareGivers.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsCaseReportsHTMLData.Tables[dsCCM.CareGivers.TableName].Rows)
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
                    if (dsCaseReportsHTMLData.Tables[dsCCM.CareManagers.TableName] != null &&
                        dsCaseReportsHTMLData.Tables[dsCCM.CareManagers.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsCaseReportsHTMLData.Tables[dsCCM.CareManagers.TableName].Rows)
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
                    if (dsCaseReportsHTMLData.Tables[dsCCM.CareCoordinators.TableName] != null &&
                        dsCaseReportsHTMLData.Tables[dsCCM.CareCoordinators.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsCaseReportsHTMLData.Tables[dsCCM.CareCoordinators.TableName].Rows)
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
                    if (dsCaseReportsHTMLData.Tables[dsCCM.CareTeamPCP.TableName] != null &&
                        dsCaseReportsHTMLData.Tables[dsCCM.CareTeamPCP.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsCaseReportsHTMLData.Tables[dsCCM.CareTeamPCP.TableName].Rows)
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
                    if (dsCaseReportsHTMLData.Tables[dsCCM.CareTeamProvider.TableName] != null &&
                        dsCaseReportsHTMLData.Tables[dsCCM.CareTeamProvider.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drRace in dsCaseReportsHTMLData.Tables[dsCCM.CareTeamProvider.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        DataRow drPatient = dsCaseReportsHTMLData.Tables[dsPatient.Patients.TableName].Rows[dsCaseReportsHTMLData.Tables[dsPatient.Patients.TableName].Rows.Count - 1];
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
                        };
                        lstPatientData.Add(PatientDatakeyValues);
                    }

                    #endregion

                    #region Provider Data

                    if (dsCaseReportsHTMLData.Tables[dsProvider.Provider.TableName].Rows.Count > 0)
                    {
                        DataRow drProvider = dsCaseReportsHTMLData.Tables[dsProvider.Provider.TableName].Rows[dsCaseReportsHTMLData.Tables[dsProvider.Provider.TableName].Rows.Count - 1];
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
                    if (dsCaseReportsHTMLData.Tables[dsProfile.Practice.TableName].Rows.Count > 0)
                    {
                        DataRow drPractice = dsCaseReportsHTMLData.Tables[dsProfile.Practice.TableName].Rows[dsCaseReportsHTMLData.Tables[dsProfile.Practice.TableName].Rows.Count - 1];
                        var PracticeDatakeyValues = new Dictionary<string, string>
                        {
                            { "PracticeNPI", MDVUtility.ToStr(drPractice[dsProfile.Practice.NPIColumn.ColumnName])},
                            { "Address", MDVUtility.ToStr(drPractice[dsProfile.Practice.AddressColumn.ColumnName])},
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

                    if (dsCaseReportsHTMLData.Tables[dsCaseReportsHTMLData.Notes.TableName].Rows.Count > 0)
                    {
                        DataRow drNote = dsCaseReportsHTMLData.Tables[dsCaseReportsHTMLData.Notes.TableName].Rows[dsCaseReportsHTMLData.Tables[dsCaseReportsHTMLData.Notes.TableName].Rows.Count - 1];
                        var NoteDatakeyValues = new Dictionary<string, string>
                        {
                            { "EncounterId", MDVUtility.ToStr(drNote[dsCaseReportsHTMLData.Notes.NotesIdColumn.ColumnName])},
                            { "EncounterDate", String.IsNullOrEmpty(MDVUtility.ToStr(drNote[dsCaseReportsHTMLData.Notes.VisitDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(drNote[dsCaseReportsHTMLData.Notes.VisitDateColumn.ColumnName]).ToShortDateString()},
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

                    if (dsCaseReportsHTMLData.Tables[dsVitals.VitalSigns.TableName] != null && dsCaseReportsHTMLData.Tables[dsVitals.VitalSigns.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsVitals.VitalSigns.TableName].Rows)
                        {
                            string BPDiastolic = (dsCaseReportsHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName] != null && dsCaseReportsHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows.Count > 0) ? MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows[0][dsVitals.VitalSignsBloodPressure.DiastolicColumn.ColumnName]) : string.Empty;
                            string BPSystolic = (dsCaseReportsHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName] != null && dsCaseReportsHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows.Count > 0) ? MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows[0][dsVitals.VitalSignsBloodPressure.SystolicColumn.ColumnName]) : string.Empty;
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
                        if (dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName] != null && dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows.Count > 0)
                        {

                            string BPDiastolic = MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.DiastolicColumn.ColumnName]);
                            string BPSystolic = MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.SystolicColumn.ColumnName]);
                            var VitalsDatakeyValues = new Dictionary<string, string>
                            {
                                { "Height",  MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.HeightColumn.ColumnName])},
                                { "Weight", MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.WeightColumn.ColumnName])},
                                { "BMI", MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.BMIColumn.ColumnName])},
                                { "BPSystolic", BPSystolic},
                                { "BPDiastolic", BPDiastolic},
                                { "Status", MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.IsActiveColumn.ColumnName])},
                                { "createdOn", MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.VitalSignDateColumn.ColumnName])},
                                   { "Pulse", MDVUtility.ToStr(MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSigns.PulseResultColumn.ColumnName])) },
                                { "SPO2", MDVUtility.ToStr( MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSigns.SPO2Column.ColumnName]))},
                                { "Temprature", MDVUtility.ToStr( MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSigns.TemperatureResultColumn.ColumnName]))},
                                { "RespiratoryRate", MDVUtility.ToStr( MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSigns.RespirationResultColumn.ColumnName]))},
                                { "InhaledO2", MDVUtility.ToStr( MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSigns.InhaledO2ConcentrationColumn.ColumnName]))},
                            };
                            lstVitals.Add(VitalsDatakeyValues);

                        }
                    }
                    #endregion

                    #region Allergies Data

                    if (dsCaseReportsHTMLData.Tables[dsAllergies.Allergy.TableName] != null && dsCaseReportsHTMLData.Tables[dsAllergies.Allergy.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsAllergies.Allergy.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables["MedicationsAdministered"] != null && dsCaseReportsHTMLData.Tables["MedicationsAdministered"].Rows.Count > 0 && documentType != CaseReportsCCDAGenerator.DocumentTemplateType.ReferralSummary)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables["MedicationsAdministered"].Rows)
                        {
                            var strDrugDosage = "";
                            try
                            {
                                foreach (DataRow DR in dsCaseReportsHTMLData.Tables[dsMedication.Medication.TableName].Rows)
                                {
                                    int index = dsCaseReportsHTMLData.Tables[dsMedication.Medication.TableName].Rows.IndexOf(DR);
                                    if (DR["MedicationID"].ToString() == dr["MedicationID"].ToString())
                                    {
                                        dsCaseReportsHTMLData.Tables[dsMedication.Medication.TableName].Rows.RemoveAt(index);
                                        break;
                                    }
                                }
                                dsCaseReportsHTMLData.Tables[dsMedication.Medication.TableName].AcceptChanges();
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
                                { "DoseUnit", MDVUtility.ToStr(dr[dsMedication.Medication.DoseUnitColumn.ColumnName]) },
                                { "Refill",MDVUtility.ToStr(dr[dsMedication.Medication.RefillColumn.ColumnName])},
                                { "FrequencyDescription",MDVUtility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName])},
                                { "Substitution", MDVUtility.ToStr(dr[dsMedication.Medication.SubstitutionColumn.ColumnName])},
                                { "Directions", strDrugDosage},
                                { "RxnormID", MDVUtility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName])},
                            };
                            lstMedicationsAdministered.Add(MedicationDatakeyValues);
                        }
                    }

                    if (dsCaseReportsHTMLData.Tables[dsMedication.Medication.TableName] != null && dsCaseReportsHTMLData.Tables[dsMedication.Medication.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsMedication.Medication.TableName].Rows)
                        {
                            var strDrugDosage = "";
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
                                    };
                            lstMedication.Add(MedicationDatakeyValues);
                        }
                    }
                    #endregion

                    #region SocialHx Data

                    if (dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_Tobacco.TableName] != null && dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_Tobacco.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_Tobacco.TableName].Rows)
                        {
                            var StatusId = MDVUtility.ToLong(dr["StatusId"]);
                            var statusText = "";
                            var SmokingStatus = "";
                            var SNOMEDCTCode = "";
                            DataRow[] drFilter = dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_Tobacco_SmokingStatus.TableName].Select(string.Concat(dsSocialHx.SocialHx_Tobacco_SmokingStatus.StatusIdColumn.ColumnName, "=", StatusId));
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

                            DataRow drSocialHx = dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx.TableName].Rows[0];
                            var SocialHxDatakeyValues = new Dictionary<string, string>
                            {
                                { "SocialHxDate",  MDVUtility.ToStr(drSocialHx[dsSocialHx.SocialHx.SocialHxDateColumn.ColumnName])},
                                { "SNOMEDID",  SNOMEDCTCode},
                                { "Status", MDVUtility.ToStr(drSocialHx[dsSocialHx.SocialHx.IsActiveColumn.ColumnName])},
                                { "Comments", MDVUtility.ToStr(dr[dsSocialHx.SocialHx_Tobacco.CommentsColumn.ColumnName])},
                                { "SmokingStatus", SmokingStatus},
                                { "Description",statusText},
                                { "SocialHxElement","Smoking Status"}

                            };
                            lstSocials.Add(SocialHxDatakeyValues);
                        }
                    }
                    if (dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_MiscHx_OccupationHx.TableName] != null && dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_MiscHx_OccupationHx.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_MiscHx_OccupationHx.TableName].Rows)
                        {

                          
                            var SocialHxDatakeyValues = new Dictionary<string, string>
                            {
                                { "StartDate",  MDVUtility.ToStr(dr[dsSocialHx.SocialHx_MiscHx_OccupationHx.StartDateColumn.ColumnName])},
                                { "EndDate",  MDVUtility.ToStr(dr[dsSocialHx.SocialHx_MiscHx_OccupationHx.EndDateColumn.ColumnName])},
                                { "Details", MDVUtility.ToStr(dr[dsSocialHx.SocialHx_MiscHx_OccupationHx.OccupationDetailColumn.ColumnName])},
                                { "Comments", MDVUtility.ToStr(dr[dsSocialHx.SocialHx_MiscHx_OccupationHx.CommentsColumn.ColumnName])},
                                { "Experience", MDVUtility.ToStr(dr[dsSocialHx.SocialHx_MiscHx_OccupationHx.IsPastColumn.ColumnName])},
                                { "Type","Employment Detail"}

                            };
                            lstSocialOccupations.Add(SocialHxDatakeyValues);
                        }
                    }
                    if (dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_MiscHx_TravelHx.TableName] != null && dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_MiscHx_TravelHx.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_MiscHx_TravelHx.TableName].Rows)
                        {

                            var SocialHxDatakeyValues = new Dictionary<string, string>
                            {
                                { "StartDate",  MDVUtility.ToStr(dr[dsSocialHx.SocialHx_MiscHx_TravelHx.FromDateColumn.ColumnName])},
                                { "EndDate",  MDVUtility.ToStr(dr[dsSocialHx.SocialHx_MiscHx_TravelHx.TodateColumn.ColumnName])},
                                { "Location", MDVUtility.ToStr(dr[dsSocialHx.SocialHx_MiscHx_TravelHx.LocationColumn.ColumnName])},
                                { "Comments", MDVUtility.ToStr(dr[dsSocialHx.SocialHx_MiscHx_TravelHx.CommentsColumn.ColumnName])}

                            };
                            lstSocialTravels.Add(SocialHxDatakeyValues);
                        }
                    }
                    if (dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_SexualHx.TableName] != null && dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_SexualHx.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsSocialHx.SocialHx_SexualHx.TableName].Rows)
                        {

                            var SocialHxDatakeyValues = new Dictionary<string, string>
                            {
                                { "PregnancyStatus",  MDVUtility.ToStr(dr[dsSocialHx.SocialHx_SexualHx.bPregnancyStatusColumn.ColumnName])},
                                { "PregnancyDuration",  MDVUtility.ToStr(dr[dsSocialHx.SocialHx_SexualHx.PregnancyDurationColumn.ColumnName])}

                            };
                            lstSocialSexuals.Add(SocialHxDatakeyValues);
                        }
                    }

                    #endregion

                    #region Procedure Data

                    if (dsCaseReportsHTMLData.Tables[dsProcedures.Procedures.TableName] != null && dsCaseReportsHTMLData.Tables[dsProcedures.Procedures.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsProcedures.Procedures.TableName].Rows)
                        {
                            DateTime dtProcedureDate = MDVUtility.ToDateTime(dr[dsProcedures.Procedures.StartDateColumn.ColumnName]);
                            string notesIds = MDVUtility.ToStr(dr["NotesId"]);
                            if (noteId > 0 && !string.IsNullOrWhiteSpace(notesIds) && notesIds.Contains(MDVUtility.ToStr(noteId)))
                            {
                                var SnomedText = MDVUtility.ToStr(dr[dsProcedures.Procedures.SNOMED_DESCRIPTIONColumn.ColumnName]);
                                var CPTText = MDVUtility.ToStr(dr[dsProcedures.Procedures.CPT_DESCRIPTIONColumn.ColumnName]);
                                if (!string.IsNullOrWhiteSpace(SnomedText) || !string.IsNullOrWhiteSpace(CPTText))
                                {
                                    var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "ProcedureName",  SnomedText == string.Empty ? CPTText + " , [CPT:" + MDVUtility.ToStr(dr[dsProcedures.Procedures.CPTCodeColumn.ColumnName]) + "]" : SnomedText + " , [SNOMED-CT:" + MDVUtility.ToStr(dr[dsProcedures.Procedures.SNOMEDIDColumn.ColumnName]) + "]"},
                                        { "Status", MDVUtility.ToStr(dr[dsSocialHx.SocialHx.IsActiveColumn.ColumnName])},
                                        { "ProcedureDate", MDVUtility.ToDateTime(dr[dsProcedures.Procedures.StartDateColumn.ColumnName]).ToShortDateString()},
                                        { "SnomedId",MDVUtility.ToStr(dr[dsProcedures.Procedures.SNOMEDIDColumn.ColumnName])},
                                        { "CPT", MDVUtility.ToStr(dr[dsProcedures.Procedures.CPTCodeColumn.ColumnName])},

                                    };
                                    lstProcedure.Add(ProblemDatakeyValues);
                                }
                            }
                        }
                    }

                    if (documentType == CaseReportsCCDAGenerator.DocumentTemplateType.ReferralSummary)
                    {
                        if (dsCaseReportsHTMLData.Tables[dsLabResult.LabOrderResult.TableName] != null && dsCaseReportsHTMLData.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsLabResult.LabOrderResult.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsMedicalHx.MedicalHx.TableName] != null && dsCaseReportsHTMLData.Tables[dsMedicalHx.MedicalHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsMedicalHx.MedicalHx.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsSurgicalHx.SurgicalHx.TableName] != null && dsCaseReportsHTMLData.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsHospitalizationHx.HospitalizationHx.TableName] != null && dsCaseReportsHTMLData.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsFamilyHx.FamilyHx.TableName] != null && dsCaseReportsHTMLData.Tables[dsFamilyHx.FamilyHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsFamilyHx.FamilyHx.TableName].Rows)
                        {
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "FamilyHxDate",  MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.FamilyHxDateColumn.ColumnName])},
                                        { "Status", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.IsActiveColumn.ColumnName])},
                                        { "Comments", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.CommentsColumn.ColumnName])},
                                    };
                            lstFamilyHx.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region BirthHx Data

                    if (dsCaseReportsHTMLData.Tables[dsBirthHx.BirthHx.TableName] != null && dsCaseReportsHTMLData.Tables[dsBirthHx.BirthHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsBirthHx.BirthHx.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsPhysicalExam.PatientPhysicalExam.TableName] != null && dsCaseReportsHTMLData.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsPlanOfCare.Immunization.TableName] != null && dsCaseReportsHTMLData.Tables[dsPlanOfCare.Immunization.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPlanOfCare.Immunization.TableName].Rows)
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

                    //if (dsCaseReportsHTMLData.Tables[dsPlanOfCare.EncounterProblemList.TableName] != null && dsCaseReportsHTMLData.Tables[dsPlanOfCare.EncounterProblemList.TableName].Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPlanOfCare.EncounterProblemList.TableName].Rows)
                    //    {
                    //        var ProblemNamewithSnomed = MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.SNOMED_DESCRIPTIONColumn.ColumnName]);
                    //        if (!string.IsNullOrWhiteSpace(ProblemNamewithSnomed)) //if SNOMED description not found.
                    //        {
                    //            var ProblemListId = MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.ProblemListIdColumn.ColumnName]);
                    //            lstEncounterProblemId.Add(ProblemListId);

                    //            if (MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.SNOMED_DESCRIPTIONColumn.ColumnName]) != string.Empty)
                    //                ProblemNamewithSnomed = string.Concat(ProblemNamewithSnomed, " [SNOMEDID:", MDVUtility.ToStr(dr[dsProblems.ProblemList.SNOMEDIDColumn.ColumnName]), "]");
                    //            var EncounterDiagnosicDatakeyValues = new Dictionary<string, string>
                    //            {
                    //                { "ProblemNamewithSnomed",  ProblemNamewithSnomed},
                    //                { "ProblemName",  MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.SNOMED_DESCRIPTIONColumn.ColumnName])},
                    //                { "StartDate",  MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.StartDateColumn.ColumnName])},
                    //                { "IsActive",  MDVUtility.ToStr( dr[dsPlanOfCare.EncounterProblemList.IsActiveColumn.ColumnName])},
                    //                { "SNOMEDID", MDVUtility.ToStr( dr[dsPlanOfCare.EncounterProblemList.SNOMEDIDColumn.ColumnName])},
                    //                { "VisitDate", MDVUtility.ToStr(dr[dsPlanOfCare.EncounterProblemList.NoteDateColumn.ColumnName])}
                    //            };
                    //            lstEncounterDiagnosicDatakeyValues.Add(EncounterDiagnosicDatakeyValues);
                    //        }
                    //    }
                    //}

                    #endregion

                    #region Complaints
                    if (dsCaseReportsHTMLData.Tables[dsComplaint.ComplaintDetail.TableName] != null && dsCaseReportsHTMLData.Tables[dsComplaint.ComplaintDetail.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsComplaint.ComplaintDetail.TableName].Rows)
                        {
                            if (MDVUtility.ToStr(dr[dsComplaint.ComplaintDetail.ComplaintDetailIdColumn.ColumnName]) != string.Empty)
                            {

                                var DatakeyValues = new Dictionary<string, string>
                                    {
                                          { "ComplaintDescription",  MDVUtility.ToStr(dr[dsComplaint.ComplaintDetail.ComplaintDescriptionColumn.ColumnName])},
                                          { "IsReported" , MDVUtility.ToStr(dr[dsComplaint.ComplaintDetail.IsReportedColumn.ColumnName])},
                                          { "ReportDate" , MDVUtility.ToStr(dr[dsComplaint.ComplaintDetail.CreatedOnColumn.ColumnName])}
                                    };
                                lstComplaints.Add(DatakeyValues);
                            }
                        }
                    }
                    #endregion

                    #region Problems Data and  EncounterDiagnosic

                    if (dsCaseReportsHTMLData.Tables[dsProblems.ProblemList.TableName] != null && dsCaseReportsHTMLData.Tables[dsProblems.ProblemList.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsProblems.ProblemList.TableName].Rows)
                        {
                            var ProblemListId = MDVUtility.ToStr(dr[dsProblems.ProblemList.ProblemListIdColumn.ColumnName]);
                            if (!lstEncounterProblemId.Contains(ProblemListId))
                            {
                                var ProblemNamewithSnomed = string.Concat(MDVUtility.ToStr(dr[dsProblems.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName]));
                                if (!string.IsNullOrWhiteSpace(ProblemNamewithSnomed)) //if SNOMED description not found.
                                {
                                    if (MDVUtility.ToStr(dr[dsProblems.ProblemList.SNOMEDIDColumn.ColumnName]) != string.Empty)
                                        ProblemNamewithSnomed = string.Concat(ProblemNamewithSnomed, " [SNOMEDID:", MDVUtility.ToStr(dr[dsProblems.ProblemList.SNOMEDIDColumn.ColumnName]), "]");
                                    var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "ProblemNamewithSnomed",  ProblemNamewithSnomed},
                                        { "ProblemName",  MDVUtility.ToStr(dr[dsProblems.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName])},
                                        { "Status", Convert.ToBoolean(dr[dsProblems.ProblemList.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                        { "EffectiveDate", MDVUtility.ToStr(dr[dsProblems.ProblemList.StartDateColumn.ColumnName])},
                                        { "Type", "Problem"},
                                        { "SNOMEDID",MDVUtility.ToStr(dr[dsProblems.ProblemList.SNOMEDIDColumn.ColumnName])},
                                        { "VisitDate", MDVUtility.ToStr(dtNoteDate)},
                                        { "IsActive",  MDVUtility.ToStr( dr[dsProblems.ProblemList.IsActiveColumn.ColumnName])},
                                        { "StartDate",  MDVUtility.ToStr(dr[dsProblems.ProblemList.StartDateColumn.ColumnName])},
                                        { "ProblemShortName",  MDVUtility.ToStr(dr[dsProblems.ProblemList.ProblemNameColumn.ColumnName])},
                                        { "ProblemType",  MDVUtility.ToStr(dr[dsProblems.ProblemList.ProblemTypeColumn.ColumnName])},
                                        { "CodeSystem",  MDVUtility.ToStr(dr[dsProblems.ProblemList.CodeSystemColumn.ColumnName])},
                                        { "IsTriggered",  MDVUtility.ToStr(dr[dsProblems.ProblemList.IsTriggeredColumn.ColumnName]) }
                                    };
                                   
                                    if (MDVUtility.ToStr(dr[dsProblems.ProblemList.IsChiefComplaintColumn.ColumnName]) == "1" )
                                    {
                                        lstEncounterDiagnosicDatakeyValues.Add(ProblemDatakeyValues);
                                    }
                                    if (MDVUtility.ToStr(dr[dsProblems.ProblemList.IsChiefComplaintColumn.ColumnName]) == "1" && MDVUtility.ToBool(dr[dsProblems.ProblemList.IsTriggeredColumn.ColumnName]) == false)
                                    {
                                        lstEncounterDiagnosicNonTriggerDatakeyValues.Add(ProblemDatakeyValues);
                                    }
                                        
                                    if (MDVUtility.ToBool(dr[dsProblems.ProblemList.IsTriggeredColumn.ColumnName]) == false)
                                    {
                                        lstProblems.Add(ProblemDatakeyValues);
                                    }
                                }
                            }
                        }
                    }

                    #endregion Problems Data and  EncounterDiagnosic

                    #region Referral Provider

                    if (dsCaseReportsHTMLData.Tables["ReferralProvider"] != null && dsCaseReportsHTMLData.Tables["ReferralProvider"].Rows.Count > 0)
                    {
                        foreach (DataRow drProvider in dsCaseReportsHTMLData.Tables["ReferralProvider"].Rows)
                        {
                            providerFullName = MDVUtility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName]) + ", " + MDVUtility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName]);
                            providerOfficeAddress = MDVUtility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName]);
                            providerOfficePhone = MDVUtility.ToStr(drProvider[dsProvider.Provider.PhoneNoColumn.ColumnName]);
                            DSConsultationOrder dsConsultationOrder = new DSConsultationOrder();
                            var OrderDate = MDVUtility.ToStr(dsCaseReportsHTMLData.Tables[dsConsultationOrder.ConsultationOrder.TableName].Rows[0][dsConsultationOrder.ConsultationOrder.OrderDateColumn.ColumnName]);
                            string name = string.Format("{0} {1}, Tel: {2}, Address 1: {3}, Address 2: {4}, Zip Code: {5}, Scheduled date: {6}, "
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName])
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName])
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.PhoneNoColumn.ColumnName])
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName])
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.HomeAddressColumn.ColumnName])
                                , MDVUtility.ToStr(drProvider[dsProvider.Provider.ZIPCodeColumn.ColumnName])
                                , OrderDate);
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

                    if (dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName] != null && dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName] != null && dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows)
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
                    if (dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName] != null && dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanOfCareGoal.TableName] != null && dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanOfCareGoal.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPlanOfCare.PlanOfCareGoal.TableName].Rows)
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
                                lstGoal.Add(PlanOfCareDatakeyValues);
                            }
                        }
                    }

                    #endregion

                    #region Scheduled Procedure Data

                    if (dsCaseReportsHTMLData.Tables[dsProcedures.Procedures.TableName] != null && dsCaseReportsHTMLData.Tables[dsProcedures.Procedures.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsProcedures.Procedures.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsLabResult.LabOrderResultDetail.TableName] != null && dsCaseReportsHTMLData.Tables[dsLabResult.LabOrderResultDetail.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsLabResult.LabOrderResultDetail.TableName].Rows)
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
                                    DataRow[] drFilters = dsCaseReportsHTMLData.Tables[dsLabResult.LabOrderResult.TableName].Select(string.Format("LabOrderResultId = {0}", MDVUtility.ToLINQFormatString(labOrderResult)));
                                    labResultStatus = MDVUtility.ToStr(drFilters[0][dsLabResult.LabOrderResult.StatusColumn.ColumnName]);
                                }

                                if (labResultStatus == "Partial")
                                {
                                    labResultStatus = "Pending";
                                }

                            }
                            catch (Exception ex) { }
                            if (MDVUtility.ToDateTime(dr[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]).Date == dtNoteDate.Date || documentType == CaseReportsCCDAGenerator.DocumentTemplateType.DataPortability)
                            {
                                var LabOrderResultDetailDatakeyValues = new Dictionary<string, string>
                                {
                                    { "LabTest",  LabTest},
                                    { "LabTestName",  LabTestName},
                                    { "ActualResult",  ActualResult},
                                    { "LoincCode", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName])},
                                    { "CodeSystem", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.CodeSystemColumn.ColumnName]) },
                                    { "Unit" ,  MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.UoMColumn.ColumnName]) },
                                    { "ResultValue",  MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.ResultColumn.ColumnName])},
                                    { "ResultDate", MDVUtility.ToDateTime(dr[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]).ToShortDateString()},
                                    { "Status", labResultStatus},
                                    { "IsOrganism", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.IsOrganismAssociatedColumn.ColumnName]) },
                                    { "OrganismCode", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.OrganismCodeColumn.ColumnName]) },
                                    { "OrganismCodeDescription", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.OrganismCodeDescriptionColumn.ColumnName]) },
                                    { "IsTriggered", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.IsTriggeredColumn.ColumnName]) },
                                    { "Range", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.RangeColumn.ColumnName]) },
                                    { "Flag", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]) },
                                    { "TestTypeCode", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.TestTypeCodeColumn.ColumnName])},
                                    { "TestTypeName", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.TestTypeNameColumn.ColumnName])},
                                    { "TestTypeCodeSystem", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.TestTypeCodeSystemColumn.ColumnName])},
                                    { "TotalTestTypes", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.TotalTestTypesColumn.ColumnName]) },
                                    { "ReferenceRangeInt", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.ReferenceRangeInterprationColumn.ColumnName]) } ,
                                    { "ReferenceRangeDesc", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.ReferenceRangeDescriptionColumn.ColumnName]) },
                                    { "TestAntimicrobial", MDVUtility.ToStr(dr[dsLabResult.LabOrderResultDetail.TestAntimicrobialColumn.ColumnName]) } ,

                                };
                                lstResults.Add(LabOrderResultDetailDatakeyValues);
                            }
                            else if (MDVUtility.ToDateTime(dr[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]) > dtNoteDate)
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


                        }
                    }

                    // Add Radiology Result
                    if (dsCaseReportsHTMLData.Tables[dsRadiologyResult.RadiologyOrderResultDetail.TableName] != null && dsCaseReportsHTMLData.Tables[dsRadiologyResult.RadiologyOrderResultDetail.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsRadiologyResult.RadiologyOrderResultDetail.TableName].Rows)
                        {
                            string LabTest = MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.LOINCDescriptionColumn.ColumnName]);
                            LabTest = string.Format("{0}, [LOINC:{1}]", LabTest, MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.LOINCColumn.ColumnName]));
                            string LabTestName = MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.LOINCDescriptionColumn.ColumnName]);
                            string ActualResult = MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.ResultColumn.ColumnName]);
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.UoMColumn.ColumnName])))
                            {
                                ActualResult = string.Format("{0} ({1})", ActualResult, MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResultDetail.UoMColumn.ColumnName]));
                            }
                            string labResultStatus = "Final";
                            try
                            {

                                string labOrderResult = MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResult.RadiologyOrderResultIdColumn.ColumnName]);
                                if (labOrderResult != string.Empty)
                                {
                                    DataRow[] drFilters = dsCaseReportsHTMLData.Tables[dsRadiologyResult.RadiologyOrderResult.TableName].Select(string.Format("RadiologyOrderResultId = {0}", MDVUtility.ToLINQFormatString(labOrderResult)));
                                    labResultStatus = MDVUtility.ToStr(drFilters[0][dsRadiologyResult.RadiologyOrderResult.StatusColumn.ColumnName]);
                                }

                                if (labResultStatus == "Partial")
                                {
                                    labResultStatus = "Pending";
                                }

                            }
                            catch (Exception ex) { }
                            if (MDVUtility.ToDateTime(dr[dsRadiologyResult.RadiologyOrderResultDetail.ObservationDateColumn.ColumnName]).Date == dtNoteDate.Date || documentType == CaseReportsCCDAGenerator.DocumentTemplateType.DataPortability)
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
                                    { "Status", labResultStatus}
                                };
                                lstResults.Add(LabOrderResultDetailDatakeyValues);
                            }
                            else if (MDVUtility.ToDateTime(dr[dsRadiologyResult.RadiologyOrderResultDetail.ObservationDateColumn.ColumnName]) > dtNoteDate)
                            {
                                var LabOrderResultDetailDatakeyValues = new Dictionary<string, string>
                                {
                                    { "Name",  LabTestName},
                                    { "Type", "Scheduled Imaging" },
                                    { "Instruction" , string.Empty },
                                    { "Date", MDVUtility.ToDateTime(dr[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]).ToShortDateString()}
                                };
                                lstscheduledProcedure.Add(LabOrderResultDetailDatakeyValues);
                            }
                        }
                    }

                    #endregion

                    #region Cognitive Functional Status

                    if (dsCaseReportsHTMLData.Tables[dsPlanOfCare.FunctionalStatus.TableName] != null && dsCaseReportsHTMLData.Tables[dsPlanOfCare.FunctionalStatus.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPlanOfCare.FunctionalStatus.TableName].Rows)
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

                    if (dsCaseReportsHTMLData.Tables[dsProfile.ReferringProvider.TableName] != null && dsCaseReportsHTMLData.Tables[dsProfile.ReferringProvider.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsProfile.ReferringProvider.TableName].Rows)
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
                    if (dsCaseReportsHTMLData.Tables[dsPatientReferral.Referrals.TableName] != null && dsCaseReportsHTMLData.Tables[dsPatientReferral.Referrals.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPatientReferral.Referrals.TableName].Rows)
                        {
                            var Referral =
                                string.Format("{0}, {1}",
                                MDVUtility.ToStr(dr[dsPatientReferral.Referrals.ReasonColumn.ColumnName]),
                                MDVUtility.ToStr(dr[dsPatientReferral.Referrals.ProviderNameColumn.ColumnName]));
                            if (dsCaseReportsHTMLData.Tables[dsPatientReferral.Referrals.TableName].Rows.Count > 0)
                            {
                                DataRow drReferringProvider = dsCaseReportsHTMLData.Tables[dsPatientReferral.Referrals.TableName].Rows[0];
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
                            Referral = string.Format("{0}{1}", Referral, string.IsNullOrWhiteSpace(MDVUtility.ToStr(dr[dsPatientReferral.Referrals.DateColumn.ColumnName])) ? string.Empty : string.Concat(", Scheduled data:", MDVUtility.ToDateTime(dr[dsPatientReferral.Referrals.DateColumn.ColumnName]).ToString("MM/dd/yyyy")));

                            lstReasonReferral.Add(new Dictionary<string, string> { { "Reason", MDVUtility.ToStr(Referral) }, });
                        }
                    }
                    #endregion


                    #region Lab Orders/Tests

                    if (dsCaseReportsHTMLData.Tables[dsLabOrder.LabOrderTest.TableName] != null && dsCaseReportsHTMLData.Tables[dsLabOrder.LabOrderTest.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsLabOrder.LabOrderTest.TableName].Rows)
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
                                    { "CodeSystem",  MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.CodeSystemColumn.ColumnName])},
                                };
                                lstLabOrderTests.Add(PlanOfCareDatakeyValues);
                            }
                        }
                    }

                    #endregion

                    #region Medical Equipment

                    if (devicesList.Count > 0)
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
                                    { "Manufacturing_Date",item.Manufacturing_Date}
                                };
                            lstImplantableDevice.Add(PlanOfCareDatakeyValues);
                        }
                    }

                    #endregion  Medical Equipment

                    #region Insurance Data

                    if (dsCaseReportsHTMLData.Tables[dsCaseReportsHTMLData.PatientInsurance_CCDA.TableName] != null && dsCaseReportsHTMLData.Tables[dsCaseReportsHTMLData.PatientInsurance_CCDA.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dsInsurance in dsCaseReportsHTMLData.Tables[dsCaseReportsHTMLData.PatientInsurance_CCDA.TableName].Rows)
                        {

                            var InsuranceDatakeyValues = new Dictionary<string, string>
                        {
                            { "DOB", String.IsNullOrEmpty(MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.DOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.DOBColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "City", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.CityColumn.ColumnName])},
                            { "State", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.StateColumn.ColumnName])},
                            { "ZIPCode", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.ZIPCodeColumn.ColumnName])},
                            { "PlanAddress", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PlanAddressColumn.ColumnName])},
                            { "MI", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.MIColumn.ColumnName])},
                            { "FirstName", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.FirstNameColumn.ColumnName])},
                            { "LastName", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.LastNameColumn.ColumnName])},
                            { "RelationShip", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.RelationNameColumn.ColumnName])},
                            { "PlanName", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.InsurancePlanNameColumn.ColumnName])},
                            { "Priority", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PlanPriorityColumn.ColumnName])},
                            { "BirthTime", String.IsNullOrEmpty(MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.DOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.DOBColumn.ColumnName]).ToString("yyyyMMdd")},
                            { "SubscriberId", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.SubscriberIdColumn.ColumnName])},
                            { "GroupId", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.GroupIdColumn.ColumnName])},
                            { "PhoneNo", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PhoneNoColumn.ColumnName])},
                            { "PatientFullName", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PatientFullNameColumn.ColumnName])},
                            { "PhoneExt", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PhoneExtColumn.ColumnName])},
                            { "PartyFullName", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PartyFullNameColumn.ColumnName])},
                            { "PartyDOB", String.IsNullOrEmpty(MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PartyDOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PartyDOBColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "PartyMI", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PartyMIColumn.ColumnName])},
                            { "PartyFirstName", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PartyFirstNameColumn.ColumnName])},
                            { "PartyLastName", MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PartyLastNameColumn.ColumnName])},
                            { "PartyBirthTime", String.IsNullOrEmpty(MDVUtility.ToStr(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PartyDOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dsInsurance[dsCaseReportsHTMLData.PatientInsurance_CCDA.PartyDOBColumn.ColumnName]).ToString("yyyyMMdd")},

                        };
                            lstInsurance.Add(InsuranceDatakeyValues);
                        }

                    }

                    #endregion

                    #region Cognitive Mental Status

                    if (dsCaseReportsHTMLData.Tables[dsPlanOfCare.MentalStatus_CCDA.TableName] != null && dsCaseReportsHTMLData.Tables[dsPlanOfCare.MentalStatus_CCDA.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsCaseReportsHTMLData.Tables[dsPlanOfCare.MentalStatus_CCDA.TableName].Rows)
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

                    #endregion dsNotesData

                    CCDACaseReportDataModel objCCDAModel = new CCDACaseReportDataModel(dtNoteDate.ToString(),
                                                                    lstReasonForVisit,
                                                                    model.Components,
                                                                    lstPatientData,
                                                                    lstProviderData,
                                                                    lstPracticeData,
                                                                    lstNoteData,
                                                                    lstProblems,
                                                                    lstVitals,
                                                                    lstAllergs,
                                                                    lstSocials,
                                                                    lstSocialOccupations,
                                                                    lstSocialTravels,
                                                                    lstSocialSexuals,
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
                                                                    lstEncounterDiagnosicNonTriggerDatakeyValues,
                                                                    lstLabOrderTests,
                                                                    lstImplantableDevice,
                                                                    lstRaceCodes,
                                                                    lstEthnicityCodes,
                                                                    lstCaregivers,
                                                                    lstCareManagers,
                                                                    lstCareCoordinators,
                                                                    lstCareTeamProvider,
                                                                    lstCareTeamPCP,
                                                                    model.IsConfidential,
                                                                    lstMentalStatus,
                                                                    lstInsurance, lstComplaints);

                    var ccdaDocument = CaseReportsCCDAGenerator.MakeCcda(objCCDAModel, noteId, providerId, patientId, StreamType, documentType);

                    string CurrentDateTime = DateTime.Now.ToString("MM/dd/yyyy, H:mm");
                    
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    byte[] toBytes = Encoding.UTF8.GetBytes(ccdaDocument);
                    var response = new
                    {
                        status = true,
                        xmlData = Convert.ToBase64String(toBytes),
                        NotesData_JSON = js.Serialize(lstNoteData),
                        PatientData_JSON = js.Serialize(lstPatientData),
                        AuthoringData_JSON = js.Serialize(lstProviderData),
                        PlanOfTreatmentData_JSON = js.Serialize(lstLabOrderTests),
                        EncounterData_JSON = js.Serialize(lstEncounterDiagnosicDatakeyValues),
                        HistoryOfCurrentIllnessData_JSON = js.Serialize(lstComplaints),
                        MedicationData_JSON = js.Serialize(lstMedicationsAdministered),
                        ProblemsData_JSON = js.Serialize(lstProblems),
                        ReasonForVisitData_JSON = js.Serialize(lstReasonForVisit),
                        ResultsData_JSON = js.Serialize(lstResults),
                        SocialHistoryData_JSON = js.Serialize(lstSocials),
                        ComponentsData_JSON = CaseReportsCCDAGenerator.currentHTML,
                        GenerateDateTime = CurrentDateTime,
                        PracticeData = js.Serialize(lstPracticeData)
                    };

                    if (documentType == CaseReportsCCDAGenerator.DocumentTemplateType.DataPortability)
                        return Convert.ToBase64String(toBytes);
                    else
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                    };
                    if (documentType == CaseReportsCCDAGenerator.DocumentTemplateType.DataPortability)
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

        public string loadDataPortabilty(CaseReportsModel model)
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
                    string data = loadCaseReportsXMLData(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), model, "xml", CaseReportsCCDAGenerator.DocumentTemplateType.DataPortability);
                    var dataBytes = Convert.FromBase64String(data);

                    string XMLString = Encoding.UTF8.GetString(dataBytes);

                    //For Getting The Patient name from XML
                    XmlDocument doc = new XmlDocument();
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("ab", "urn:hl7-org:v3");
                    doc.LoadXml(XMLString);
                    var PatientName = doc.SelectSingleNode("/ab:ClinicalDocument/ab:recordTarget/ab:patientRole/ab:patient/ab:name/ab:given", nsmgr).InnerText;

                    string html = CaseReportsCCDAGenerator.GetHtmlFromXml(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.PatientId), Encoding.UTF8.GetString(dataBytes));
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
        public string insertUpdatePlanOfCare(CaseReportsModel model, List<object> lstGoalObject)
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
        public string fillPlanOfCare(CaseReportsModel model, Int64 planOfCareId)
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
        //        DSCaseReports dsPlanOfCare = null;
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
        //            BLObject<DSCaseReports> obj = BLLClinicalObj.attachPlanOfCareWithNotes(planOfCareId, notesId);
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
        internal string attachPlanOfCareWithNotes(CaseReportsModel model)
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






        public string SendEmailThroughPhiMailConnector(string toEmail, string XMLText, string HTMLText)
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
                pc.AuthenticateUser(MDVApplication.CCDA_RemoteUser, MDVApplication.CCDA_RemotePassword);

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
                pc.AddText("CCDA");

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
                    message = "Transmit successful."; //+ sendRes[0].MessageId;
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

            return message;
        }
    }
}