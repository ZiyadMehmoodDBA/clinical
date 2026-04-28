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
using System.IO;
using System.Xml.Xsl;
using System.Reflection;
using MDVision.IEHR.Common;
using MDVision.DataAccess.DCommon;



namespace MDVision.IEHR.EMR.Helpers.Clinical.Summary
{
    public class ClinicalSummaryHelper
    {
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
        public string loadClinicalSummaryXMLData(Int64 noteId, Int64 providerId, Int64 patientId, ClinicalSummaryModel model, string StreamType, CCDAGenrator.DocumentTemplateType documentType)
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
                Dictionary<int, string> lstComopent = new Dictionary<int, string>();
                if (model.Components != null)
                {
                    foreach (Component lst in model.Components)
                    {
                        try { lstComopent.Add(lst.componentId, lst.componentName); }
                        catch (Exception ex) { }
                    }
                }

                BLObject<DSNotes> dsNotesData = BusinessWrapper.Clinical.BusinessObj.loadClinicalSummaryHTMLData(patientId, providerId, noteId, lstComopent);

                dsClinicalSummaryHTMLData = dsNotesData.Data;

                if (dsNotesData.Data != null)
                {
                    DateTime dtNoteDate = DateTime.Now;
                    if (dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows.Count > 0)
                    {
                        dtNoteDate = Utility.ToDateTime(dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows[0][dsClinicalSummaryHTMLData.Notes.VisitDateColumn.ColumnName]);
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
                    List<Dictionary<string, string>> lstGoal = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstResults = new List<Dictionary<string, string>>();
                    List<Dictionary<string, string>> lstRefferalProviderData = new List<Dictionary<string, string>>();
                    string practiceName = "";
                    string providerFullName = "";
                    string providerOfficeAddress = "";
                    string providerOfficePhone = "";

                    #region Patient Data

                    var ReasonForVisit = string.Empty;
                    if (lstComopent.Where(p => p.Value.ToLower() == "visitreason").Count() > 0)
                    {
                        try
                        {
                            ReasonForVisit = Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows[0]["VisitReason"]);
                        }
                        catch (Exception ex) { }
                    }



                    #endregion

                    #region Patient Data

                    if (dsClinicalSummaryHTMLData.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        DataRow drPatient = dsClinicalSummaryHTMLData.Tables[dsPatient.Patients.TableName].Rows[dsClinicalSummaryHTMLData.Tables[dsPatient.Patients.TableName].Rows.Count - 1];
                        practiceName = Utility.ToStr(drPatient[dsPatient.Patients.PracticeNameColumn.ColumnName]);
                        var PatientDatakeyValues = new Dictionary<string, string>
                        {
                            { "PatientName",  Utility.ToStr(drPatient[dsPatient.Patients.FullNameColumn.ColumnName])},
                            { "PatientDOB", String.IsNullOrEmpty(Utility.ToStr(drPatient[dsPatient.Patients.DOBColumn.ColumnName])) ? "" : Utility.ToDateTime(drPatient[dsPatient.Patients.DOBColumn.ColumnName]).ToShortDateString()},
                            { "PatientGender", Utility.ToStr(drPatient[dsPatient.Patients.GenderColumn.ColumnName])},
                            { "PatientRace", Utility.ToStr(drPatient["RaceName"])},
                             { "PatientRaceCode", Utility.ToStr(drPatient["RaceCode"])},
                            { "PatientEthnicity", Utility.ToStr(drPatient["EthnicityName"])},
                            { "PatientEthnicityCode", Utility.ToStr(drPatient["EthnicityCode"])},
                            { "PatientLanguageCode", Utility.ToStr(drPatient["LanguageCode"])},
                            { "PatientContactInfo", "Primary Home:" + Utility.ToStr(drPatient[dsPatient.Patients.HomePhoneNoColumn.ColumnName]) + "<br>" + Utility.ToStr(drPatient[dsPatient.Patients.Address1Column.ColumnName])+"<br>"+Utility.ToStr(drPatient[dsPatient.Patients.CellNoColumn.ColumnName])},
                            { "PatientIDs", Utility.ToStr(drPatient[dsPatient.Patients.AccountNumberColumn.ColumnName])},
                              { "PatientSSN", Utility.ToStr(drPatient[dsPatient.Patients.SSNColumn.ColumnName])},
                              { "PatientStreetAddress", Utility.ToStr(drPatient[dsPatient.Patients.Address1Column.ColumnName])},
                              { "PatientCity", Utility.ToStr(drPatient[dsPatient.Patients.CityColumn.ColumnName])},
                               { "PatientState", Utility.ToStr(drPatient[dsPatient.Patients.StateColumn.ColumnName])},
                               { "PatientZIPCode", Utility.ToStr(drPatient[dsPatient.Patients.ZIPCodeColumn.ColumnName])},
                               { "PatientCountry", Utility.ToStr(drPatient[dsPatient.Patients.Address2Column.ColumnName])},
                               { "PatientCellNo", Utility.ToStr(drPatient[dsPatient.Patients.CellNoColumn.ColumnName])},
                               { "MaritialStatus", Utility.ToStr(drPatient[dsPatient.Patients.MaritialStatusColumn.ColumnName])},
                               {"PrefCommunicationId", Utility.ToStr(drPatient[dsPatient.Patients.PrefCommunicationIdColumn.ColumnName])},
                               {"PrefCommunicationName", Utility.ToStr(drPatient["PrefCommunicationName"]) },
                                  { "MiddleInitial", Utility.ToStr(drPatient[dsPatient.Patients.MIColumn.ColumnName])},
                                       { "PatientHomePhoneNo", Utility.ToStr(drPatient[dsPatient.Patients.HomePhoneNoColumn.ColumnName])},
                                   { "PatientWorkPhoneNo", Utility.ToStr(drPatient[dsPatient.Patients.WorkPhoneNoColumn.ColumnName])},
                            { "MI", Utility.ToStr(drPatient[dsPatient.Patients.MIColumn.ColumnName])},
                                 {"FirstName", Utility.ToStr(drPatient[dsPatient.Patients.FirstNameColumn.ColumnName])},
                               {"LastName", Utility.ToStr(drPatient[dsPatient.Patients.LastNameColumn.ColumnName])},
                            { "ReferringProviderName", Utility.ToStr(drPatient[dsPatient.Patients.ReferringProviderNameColumn.ColumnName])},
                            { "ReferringProviderNPI", Utility.ToStr(drPatient["ReferringProviderNPI"])},
                            { "ReferringFirstName", Utility.ToStr(drPatient["ReferringFirstName"])},
                            { "ReferringLastName", Utility.ToStr(drPatient["ReferringLastName"])},
                            { "ReferringMI", Utility.ToStr(drPatient["ReferringMI"])},
                            { "ReferrringPhoneNo", Utility.ToStr(drPatient["ReferrringPhoneNo"])},
                            { "ReferringAddress", Utility.ToStr(drPatient["ReferringAddress"])},
                            { "ReferringCity", Utility.ToStr(drPatient["ReferringCity"])},
                             { "ReferringState", Utility.ToStr(drPatient["ReferringState"])},
                              { "ReferringZipCode", Utility.ToStr(drPatient["ReferringZipCode"])},
                              { "PCPName", Utility.ToStr(drPatient["PCPName"])},
                              
                            //{ "DocumentCreated", Utility.ToStr(drPatient[dsPatient.Patients.ProviderNameColumn.ColumnName])},
                            //{ "HealthcareService", Utility.ToStr(drPatient[dsPatient.Patients.PracticeNameColumn.ColumnName])},
                            //{ "Performer", Utility.ToStr(drPatient[dsPatient.Patients.ProviderNameColumn.ColumnName])},
                            //{ "Author", Utility.ToStr(drPatient[dsPatient.Patients.ProviderNameColumn.ColumnName])},
                        };
                        lstPatientData.Add(PatientDatakeyValues);
                    }

                    #endregion

                    #region Provider Data

                    if (dsClinicalSummaryHTMLData.Tables[dsProvider.Provider.TableName].Rows.Count > 0)
                    {
                        DataRow drProvider = dsClinicalSummaryHTMLData.Tables[dsProvider.Provider.TableName].Rows[dsClinicalSummaryHTMLData.Tables[dsProvider.Provider.TableName].Rows.Count - 1];
                        providerFullName = Utility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName]) + ", " + Utility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName]);
                        providerOfficeAddress = Utility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName]);
                        providerOfficePhone = Utility.ToStr(drProvider[dsProvider.Provider.PhoneNoColumn.ColumnName]);
                        var ProviderDatakeyValues = new Dictionary<string, string>
                        {
                              { "ProviderNPI", Utility.ToStr(drProvider[dsProvider.Provider.NPIColumn.ColumnName])},
                            { "FirstName", Utility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName])},
                            { "LastName", Utility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName])},
                              { "MiddleInitial", Utility.ToStr(drProvider[dsProvider.Provider.MiddleInitialColumn.ColumnName])},
                            { "MI", Utility.ToStr(drProvider[dsProvider.Provider.MiddleInitialColumn.ColumnName])},
                            { "PhoneNo", Utility.ToStr(drProvider[dsProvider.Provider.PhoneNoColumn.ColumnName])},
                            { "PhoneExt", Utility.ToStr(drProvider[dsProvider.Provider.PhoneExtColumn.ColumnName])},
                               { "HomeAddress", Utility.ToStr(drProvider[dsProvider.Provider.HomeAddressColumn.ColumnName])},
							{ "WorkAddress", Utility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName])},
                            { "ZIPCode", Utility.ToStr(drProvider[dsProvider.Provider.ZIPCodeColumn.ColumnName])},
                            { "State", Utility.ToStr(drProvider[dsProvider.Provider.StateColumn.ColumnName])},
                            { "City", Utility.ToStr(drProvider[dsProvider.Provider.CityColumn.ColumnName])},
                            {"SpecialtyName", Utility.ToStr(drProvider["SpecialtyName"])},
                             { "Given", Utility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName])},
                            { "Family", Utility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName])},
                            { "DocumentCreated", providerFullName},
                            { "HealthcareService", practiceName},
                            { "Performer", providerFullName},
                            { "Author", providerFullName},
                             
                            { "AuthorContactInfo", "Work Place: "+Utility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName]) + "<br> Tel:" + Utility.ToStr(drProvider[dsProvider.Provider.CellNoColumn.ColumnName])},
                        };


                        lstProviderData.Add(ProviderDatakeyValues);
                    }

                    #endregion

                    #region PracticeData
                    DSProfile dsProfile = new DSProfile();
                    if (dsClinicalSummaryHTMLData.Tables[dsProfile.Practice.TableName].Rows.Count > 0)
                    {
                        DataRow drPractice = dsClinicalSummaryHTMLData.Tables[dsProfile.Practice.TableName].Rows[dsClinicalSummaryHTMLData.Tables[dsProfile.Practice.TableName].Rows.Count - 1];
                        var PracticeDatakeyValues = new Dictionary<string, string>
                        {
                            { "PracticeNPI", Utility.ToStr(drPractice[dsProfile.Practice.NPIColumn.ColumnName])},
                            { "Address", Utility.ToStr(drPractice[dsProfile.Practice.Address1Column.ColumnName])},
                            { "City", Utility.ToStr(drPractice[dsProfile.Practice.CityColumn.ColumnName])},
                            { "State", Utility.ToStr(drPractice[dsProfile.Practice.StateColumn.ColumnName])},
                            { "ZIPCode", Utility.ToStr(drPractice[dsProfile.Practice.ZIPCodeColumn.ColumnName])},
                            { "ShortName", Utility.ToStr(drPractice[dsProfile.Practice.ShortNameColumn.ColumnName])},
                             { "PhoneNo", Utility.ToStr(drPractice[dsProfile.Practice.PhoneNoColumn.ColumnName])},
                              { "PhoneExt", Utility.ToStr(drPractice[dsProfile.Practice.PhoneExtColumn.ColumnName])},
                          
                        };
                        lstPracticeData.Add(PracticeDatakeyValues);
                    }
                    #endregion

                    #region Note Data

                    if (dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows.Count > 0)
                    {
                        DataRow drNote = dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows[dsClinicalSummaryHTMLData.Tables[dsClinicalSummaryHTMLData.Notes.TableName].Rows.Count - 1];
                        var NoteDatakeyValues = new Dictionary<string, string>
                        {
                            { "EncounterId", Utility.ToStr(drNote[dsClinicalSummaryHTMLData.Notes.NotesIdColumn.ColumnName])},
                            { "EncounterDate", String.IsNullOrEmpty(Utility.ToStr(drNote[dsClinicalSummaryHTMLData.Notes.VisitDateColumn.ColumnName])) ? "" : Utility.ToDateTime(drNote[dsClinicalSummaryHTMLData.Notes.VisitDateColumn.ColumnName]).ToShortDateString()},
                            { "EncounterLocation", practiceName},
                            { "ResponsibleParty", providerFullName},
                            { "ResponsiblePartyContactInfo", "Work Place: "+ providerOfficeAddress + "<br> Tel:" + providerOfficePhone},
                            { "EnteredBy", providerFullName},
                            { "EnteredByContactInfo", "Work Place: "+ providerOfficeAddress + "<br> Tel:" + providerOfficePhone},
                            { "Informant", providerFullName},
                            { "InformantContactInfo", "Work Place: "+ providerOfficeAddress + "<br> Tel:" + providerOfficePhone},
                            { "InformationRecipient", Utility.ToStr(drNote["RefProviderName"])},
                            { "InformationRecipientContactInfo", Utility.ToStr(drNote["RefProviderAddress"])},
                            { "LegalAuthenticator", providerFullName},
                            { "LegalAuthenticatorContactInfo", "Work Place: "+ providerOfficeAddress + "<br> Tel:" + providerOfficePhone},
                            { "DocumentMaintainedBy", practiceName},
                        };
                        lstNoteData.Add(NoteDatakeyValues);
                    }

                    #endregion

                    #region Problems Data

                    if (dsClinicalSummaryHTMLData.Tables[dsProblems.ProblemList.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsProblems.ProblemList.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsProblems.ProblemList.TableName].Rows)
                        {
                            var ProblemNameSnomedId = string.Concat(Utility.ToStr(dr[dsProblems.ProblemList.ProblemNameColumn.ColumnName]));
                            if (Utility.ToStr(dr[dsProblems.ProblemList.SNOMEDIDColumn.ColumnName]) != string.Empty)
                                ProblemNameSnomedId = string.Concat(ProblemNameSnomedId, " (", Utility.ToStr(dr[dsProblems.ProblemList.SNOMEDIDColumn.ColumnName]), ")");
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "ProblemName",  ProblemNameSnomedId},
                                        { "Status", Convert.ToBoolean(dr[dsProblems.ProblemList.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                        { "EffectiveDate", Utility.ToStr(dr[dsProblems.ProblemList.StartDateColumn.ColumnName])},
                                        { "Type", "Problem"},
                                        { "SNOMEDID",Utility.ToStr(dr[dsProblems.ProblemList.SNOMEDIDColumn.ColumnName])},
                                    };
                            lstProblems.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region Vitals Signs Data

                    if (dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSigns.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSigns.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSigns.TableName].Rows)
                        {
                            string BPDiastolic = (dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows.Count > 0) ? Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows[0][dsVitals.VitalSignsBloodPressure.DiastolicColumn.ColumnName]) : string.Empty;
                            string BPSystolic = (dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows.Count > 0) ? Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows[0][dsVitals.VitalSignsBloodPressure.SystolicColumn.ColumnName]) : string.Empty;

                            var VitalsDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Height",  Utility.ToStr(dr[dsVitals.VitalSigns.HeightColumn.ColumnName])},
                                        { "Weight", Utility.ToStr(dr[dsVitals.VitalSigns.WeightColumn.ColumnName])},
                                        { "BMI", Utility.ToStr(dr[dsVitals.VitalSigns.BMIColumn.ColumnName])},
                                        { "BPSystolic", BPSystolic},
                                        { "BPDiastolic", BPDiastolic},
                                        { "Status", Utility.ToStr(dr[dsVitals.VitalSigns.IsActiveColumn.ColumnName])},
                                        { "createdOn", Utility.ToStr(dr[dsVitals.VitalSigns.CreatedOnColumn.ColumnName])}
                                    };
                            lstVitals.Add(VitalsDatakeyValues);
                        }

                    }
                    else
                    {
                        if (dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows.Count > 0)
                        {
                           
                                string BPDiastolic = (dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows.Count > 0) ? Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows[0][dsVitals.VitalSignsBloodPressure.DiastolicColumn.ColumnName]) : string.Empty;
                                string BPSystolic = (dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows.Count > 0) ? Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows[0][dsVitals.VitalSignsBloodPressure.SystolicColumn.ColumnName]) : string.Empty;

                                var VitalsDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Height",  Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.HeightColumn.ColumnName])},
                                        { "Weight", Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.WeightColumn.ColumnName])},
                                        { "BMI", Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.BMIColumn.ColumnName])},
                                        { "BPSystolic", BPSystolic},
                                        { "BPDiastolic", BPDiastolic},
                                        { "Status", Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.IsActiveColumn.ColumnName])},
                                        { "createdOn", Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsVitals.VitalSignSoap.TableName].Rows[0][dsVitals.VitalSignSoap.VitalSignDateColumn.ColumnName])}
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
                            var SubstanceRxnormId = string.Concat(Utility.ToStr(dr[dsAllergies.Allergy.AllergenColumn.ColumnName]), " ,[RxNorm:", Utility.ToStr(dr[dsAllergies.Allergy.RxnormIDColumn.ColumnName]), "]");
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Status", Convert.ToBoolean(dr[dsAllergies.Allergy.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                        { "Reaction", Utility.ToStr(dr[dsAllergies.Allergy.ReactionColumn.ColumnName]) },
                                        { "Substance", SubstanceRxnormId},
                                        { "RxNormID", Utility.ToStr(dr[dsAllergies.Allergy.RxnormIDColumn.ColumnName])},
                                        { "Severity",  Utility.ToStr(dr[dsAllergies.Allergy.SeverityColumn.ColumnName])},
                                        { "ResolvedDate",  Utility.ToDateTime(dr[dsAllergies.Allergy.LastModifiedColumn.ColumnName]).ToShortDateString()},
                                        { "CreatedOn",  Utility.ToDateTime(dr[dsAllergies.Allergy.OnSetDateColumn.ColumnName]).ToShortDateString()},
                                    };
                            lstAllergs.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region Medication Data

                    if (dsClinicalSummaryHTMLData.Tables["MedicationsAdministered"] != null && dsClinicalSummaryHTMLData.Tables["MedicationsAdministered"].Rows.Count > 0)
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


                            if (Utility.ToStr(dr[dsMedication.Medication.ActionColumn.ColumnName]) != "" || Utility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName]) != "" || Utility.ToStr(dr[dsMedication.Medication.DoseUnitColumn.ColumnName]) != "" || Utility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName]) != "" || Utility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName]) != "" || Utility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName]) != "")
                            {
                                strDrugDosage = Utility.ToStr(dr[dsMedication.Medication.ActionColumn.ColumnName]) + " " + Utility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName]) + " " + Utility.ToStr(dr[dsMedication.Medication.DoseUnitColumn.ColumnName]) + " " + Utility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName]) + " " + Utility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName]) + " " + Utility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName]);
                                strDrugDosage += " " + Utility.ToStr(dr[dsMedication.Medication.PatientNotesColumn.ColumnName]);
                            }
                            else
                            {
                                strDrugDosage = Utility.ToStr(dr[dsMedication.Medication.PatientNotesColumn.ColumnName]);
                            }
                            var MedicationRxNormId = string.Concat(Utility.ToStr(dr[dsMedication.Medication.MedicationNameColumn.ColumnName]), " ,[RxNorm:", Utility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName]), "]");

                            var MedicationDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Medication", MedicationRxNormId},
                                        { "Status", Convert.ToBoolean(dr[dsMedication.Medication.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                        { "StartDate", Utility.ToStr(dr[dsMedication.Medication.StartDateColumn.ColumnName])},
                                        { "Indication", string.Empty},// Not in System
                                        { "RouteCode", string.Empty},// Not in System
                                        { "EndDate", Utility.ToStr(dr[dsMedication.Medication.StopDateColumn.ColumnName])},
                                        { "NoOfTimes",Utility.ToStr(dr[dsMedication.Medication.QuantityColumn.ColumnName])},
                                        { "RouteDescription",Utility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName])},
                                        { "Dose",Utility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName])},
                                         { "Refill",Utility.ToStr(dr[dsMedication.Medication.RefillColumn.ColumnName])},
                                        { "FrequencyDescription",Utility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName])},
                                        { "Route/Frequency", strDrugDosage},
                                        { "Substitution", Utility.ToStr(dr[dsMedication.Medication.SubstitutionColumn.ColumnName])},
                                        { "Directions", strDrugDosage},
                                         { "RxnormID", Utility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName])},

                                        
                                    };
                            lstMedicationsAdministered.Add(MedicationDatakeyValues);
                        }
                    }

                    if (dsClinicalSummaryHTMLData.Tables[dsMedication.Medication.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsMedication.Medication.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsMedication.Medication.TableName].Rows)
                        {
                            var strDrugDosage = "";
                            if (Utility.ToStr(dr[dsMedication.Medication.ActionColumn.ColumnName]) != "" || Utility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName]) != "" || Utility.ToStr(dr[dsMedication.Medication.DoseUnitColumn.ColumnName]) != "" || Utility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName]) != "" || Utility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName]) != "" || Utility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName]) != "")
                            {
                                strDrugDosage = Utility.ToStr(dr[dsMedication.Medication.ActionColumn.ColumnName]) + " " + Utility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName]) + " " + Utility.ToStr(dr[dsMedication.Medication.DoseUnitColumn.ColumnName]) + " " + Utility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName]) + " " + Utility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName]) + " " + Utility.ToStr(dr[dsMedication.Medication.DoseOtherColumn.ColumnName]);
                                strDrugDosage += " " + Utility.ToStr(dr[dsMedication.Medication.PatientNotesColumn.ColumnName]);
                            }
                            else
                            {
                                strDrugDosage = Utility.ToStr(dr[dsMedication.Medication.PatientNotesColumn.ColumnName]);
                            }
                            var MedicationRxNormId = string.Concat(Utility.ToStr(dr[dsMedication.Medication.MedicationNameColumn.ColumnName]), " ,[RxNorm:", Utility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName]), "]");
                            var MedicationDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Medication", MedicationRxNormId},
                                        { "Status", Convert.ToBoolean(dr[dsMedication.Medication.IsActiveColumn.ColumnName]) ? "Active" : "Inactive"},
                                        { "StartDate", Utility.ToStr(dr[dsMedication.Medication.StartDateColumn.ColumnName])},
                                        { "Indication", string.Empty},// Not in System
                                        { "RouteCode", string.Empty},// Not in System
                                        { "EndDate", Utility.ToStr(dr[dsMedication.Medication.StopDateColumn.ColumnName])},
                                        { "NoOfTimes",Utility.ToStr(dr[dsMedication.Medication.QuantityColumn.ColumnName])},
                                        { "RouteDescription",Utility.ToStr(dr[dsMedication.Medication.RoutebyColumn.ColumnName])},
                                        { "Dose",Utility.ToStr(dr[dsMedication.Medication.DoseColumn.ColumnName])},
                                         { "Refill",Utility.ToStr(dr[dsMedication.Medication.RefillColumn.ColumnName])},
                                        { "FrequencyDescription",Utility.ToStr(dr[dsMedication.Medication.DoseTimingColumn.ColumnName])},
                                        { "Route/Frequency", strDrugDosage},
                                        { "Substitution", Utility.ToStr(dr[dsMedication.Medication.SubstitutionColumn.ColumnName])},
                                        { "Directions", strDrugDosage},
                                         { "RxnormID", Utility.ToStr(dr[dsMedication.Medication.RxnormIDColumn.ColumnName])},

                                        
                                    };
                            lstMedication.Add(MedicationDatakeyValues);
                        }
                    }
                    {

                    }

                    #endregion

                    #region SocialHx Data

                    if (dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_Tobacco.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_Tobacco.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_Tobacco.TableName].Rows)
                        {
                            var StatusId = Utility.ToLong(dr["StatusId"]);
                            var statusText = "";
                            var SNOMEDCTCode = "";
                            DataRow[] drFilter = dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx_Tobacco_SmokingStatus.TableName].Select(string.Concat(dsSocialHx.SocialHx_Tobacco_SmokingStatus.StatusIdColumn.ColumnName, "=", StatusId));
                            if (drFilter != null && drFilter.Count() > 0)
                            {
                                SNOMEDCTCode = Utility.ToStr(drFilter[0][dsSocialHx.SocialHx_Tobacco_SmokingStatus.SNOMEDCTCodeColumn.ColumnName]);
                                statusText = string.Concat(Utility.ToStr(drFilter[0][dsSocialHx.SocialHx_Tobacco_SmokingStatus.DescriptionColumn.ColumnName]), ", [SNOMED-CT:", Utility.ToStr(drFilter[0][dsSocialHx.SocialHx_Tobacco_SmokingStatus.SNOMEDCTCodeColumn.ColumnName]), "]");
                            }
                            else
                                break;

                            DataRow drSocialHx = dsClinicalSummaryHTMLData.Tables[dsSocialHx.SocialHx.TableName].Rows[0];
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "SocialHxDate",  Utility.ToStr(drSocialHx[dsSocialHx.SocialHx.SocialHxDateColumn.ColumnName])},
                                        { "SNOMEDID",  SNOMEDCTCode},
                                        { "Status", Utility.ToStr(drSocialHx[dsSocialHx.SocialHx.IsActiveColumn.ColumnName])},
                                        { "Comments", Utility.ToStr(dr[dsSocialHx.SocialHx_Tobacco.CommentsColumn.ColumnName])},
                                        { "Description",statusText},
                                        { "SocialHxElement","Smoking Status"},
                                        
                                    };
                            lstSocials.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region Procedure Data

                    if (dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName].Rows)
                        {
                            DateTime dtProcedureDate = Utility.ToDateTime(dr[dsProcedures.Procedures.StartDateColumn.ColumnName]);
                            if (dtNoteDate.Date == dtProcedureDate.Date || noteId == 0)
                            {
                                var SnomedText = string.Concat(Utility.ToStr(dr[dsProcedures.Procedures.SNOMED_DESCRIPTIONColumn.ColumnName]), ", [SNOMED-CT:", Utility.ToStr(dr[dsProcedures.Procedures.SNOMEDIDColumn.ColumnName]), "]");
                                var CPTText = string.Concat(Utility.ToStr(dr[dsProcedures.Procedures.CPT_DESCRIPTIONColumn.ColumnName]), ", [SNOMED-CT:", Utility.ToStr(dr[dsProcedures.Procedures.CPTCodeColumn.ColumnName]), "]");
                                var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "ProcedureName",  SnomedText == string.Empty ? CPTText : SnomedText},
                                        { "Status", Utility.ToStr(dr[dsSocialHx.SocialHx.IsActiveColumn.ColumnName])},
                                        { "ProcedureDate", Utility.ToDateTime(dr[dsProcedures.Procedures.StartDateColumn.ColumnName]).ToShortDateString()},
                                        { "SnomedId",Utility.ToStr(dr[dsProcedures.Procedures.SNOMEDIDColumn.ColumnName])},
                                        { "CPT", Utility.ToStr(dr[dsProcedures.Procedures.CPTCodeColumn.ColumnName])},
                                        
                                    };
                                lstProcedure.Add(ProblemDatakeyValues);
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
                                        { "MedicalHxDate",  Utility.ToStr(dr[dsMedicalHx.MedicalHx.MedicalHxDateColumn.ColumnName])},
                                        { "Status", Utility.ToStr(dr[dsMedicalHx.MedicalHx.IsActiveColumn.ColumnName])},
                                        { "Comments", Utility.ToStr(dr[dsMedicalHx.MedicalHx.CommentsColumn.ColumnName])},
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
                                        { "SurgicalHxDate",  Utility.ToStr(dr[dsSurgicalHx.SurgicalHx.SurgicalHxDateColumn.ColumnName])},
                                        { "Status", Utility.ToStr(dr[dsSurgicalHx.SurgicalHx.IsActiveColumn.ColumnName])},
                                        { "Comments", Utility.ToStr(dr[dsSurgicalHx.SurgicalHx.CommentsColumn.ColumnName])},
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
                                        { "HospitalizationHxDate",  Utility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxDateColumn.ColumnName])},
                                        { "Status", Utility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.IsActiveColumn.ColumnName])},
                                        { "Comments", Utility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.CommentsColumn.ColumnName])},
                                    };
                            lstHospitalizationHx.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region FamilyHx Data

                    if (dsClinicalSummaryHTMLData.Tables[dsFamilyHx.FamilyHx.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsFamilyHx.FamilyHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsFamilyHx.FamilyHx.TableName].Rows)
                        {
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "FamilyHxDate",  Utility.ToStr(dr[dsFamilyHx.FamilyHx.FamilyHxDateColumn.ColumnName])},
                                        { "Status", Utility.ToStr(dr[dsFamilyHx.FamilyHx.IsActiveColumn.ColumnName])},
                                        { "Comments", Utility.ToStr(dr[dsFamilyHx.FamilyHx.CommentsColumn.ColumnName])},
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
                                        { "BirthHxDate",  Utility.ToStr(dr[dsBirthHx.BirthHx.BirthHxDateColumn.ColumnName])},
                                        { "Status", Utility.ToStr(dr[dsBirthHx.BirthHx.IsActiveColumn.ColumnName])},
                                        { "Comments", Utility.ToStr(dr[dsBirthHx.BirthHx.CommentsColumn.ColumnName])},
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
                                        { "PatientPhysicalExam",  Utility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamDateColumn.ColumnName])},
                                        { "Status", Utility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.IsActiveColumn.ColumnName])},
                                        { "Comments", Utility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.CommentsColumn.ColumnName])},
                                    };
                            lstPhysicalExam.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region Immunization Data

                    if (dsClinicalSummaryHTMLData.Tables[dsImmunization.VaccineHx.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsImmunization.VaccineHx.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsImmunization.VaccineHx.TableName].Rows)
                        {
                            string VaccinceName = string.Empty, CVXCode = string.Empty;
                            var VaccinceRows = dsClinicalSummaryHTMLData.Tables[dsImmunization.VaccineInfo.TableName].Select(string.Concat(dsImmunization.VaccineInfo.VaccineIDColumn.ColumnName, "=", Utility.ToStr(dr[dsImmunization.VaccineHx.VaccineColumn.ColumnName])));
                            if (VaccinceRows != null && VaccinceRows.Length > 0)
                            {
                                VaccinceName = Utility.ToStr(VaccinceRows[0][dsImmunization.VaccineInfo.CVXShortDescriptionColumn.ColumnName]);
                                CVXCode = Utility.ToStr(VaccinceRows[0][dsImmunization.VaccineInfo.CVXCodeColumn.ColumnName]);
                            }

                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Immunization",  VaccinceName},
                                        { "DateAdministered", Utility.ToStr(dr[dsImmunization.VaccineHx.AdministrationDateColumn.ColumnName])},
                                        { "Status", Convert.ToBoolean(dr[dsImmunization.VaccineHx.ISActiveColumn.ColumnName]) ? "Active": "InAtive"},
                                        { "CVX" ,CVXCode},
                                    };
                            lstImmunization.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region Referral Provider

                    if (dsClinicalSummaryHTMLData.Tables["ReferralProvider"] != null && dsClinicalSummaryHTMLData.Tables["ReferralProvider"].Rows.Count > 0)
                    {
                        foreach (DataRow drProvider in dsClinicalSummaryHTMLData.Tables["ReferralProvider"].Rows)
                        {
                            providerFullName = Utility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName]) + ", " + Utility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName]);
                            providerOfficeAddress = Utility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName]);
                            providerOfficePhone = Utility.ToStr(drProvider[dsProvider.Provider.PhoneNoColumn.ColumnName]);
                            DSConsultationOrder dsConsultationOrder = new DSConsultationOrder();
                            var OrderDate = Utility.ToStr(dsClinicalSummaryHTMLData.Tables[dsConsultationOrder.ConsultationOrder.TableName].Rows[0][dsConsultationOrder.ConsultationOrder.OrderDateColumn.ColumnName]);
                            string name = string.Format("{0} {1}, Tel: {2}, Address 1: {3}, Address 2: {4}, Zip Code: {5}, Scheduled date: {6}, "
                                , Utility.ToStr(drProvider[dsProvider.Provider.LastNameColumn.ColumnName])
                                , Utility.ToStr(drProvider[dsProvider.Provider.FirstNameColumn.ColumnName])
                                , Utility.ToStr(drProvider[dsProvider.Provider.PhoneNoColumn.ColumnName])
                                , Utility.ToStr(drProvider[dsProvider.Provider.OfficeAddressColumn.ColumnName])
                                , Utility.ToStr(drProvider[dsProvider.Provider.HomeAddressColumn.ColumnName])
                                , Utility.ToStr(drProvider[dsProvider.Provider.ZIPCodeColumn.ColumnName])
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

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows)
                        {
                            string FutureInstruction = Utility.ToStr(dr[dsPlanOfCare.PlanofCare.FutureInstructionColumn.ColumnName]);
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Name",  FutureInstruction},
                                        { "Type", "Future Appointment" },
                                        { "Instruction" , string.Empty },
                                        { "Date", Utility.ToDateTime(dr[dsPlanOfCare.PlanofCare.CreatedOnColumn.ColumnName]).ToShortDateString()}
                                    };
                            lstFutureAppointment.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region Instructions

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanofCare.TableName].Rows)
                        {
                            string ClinicalInstruction = Utility.ToStr(dr[dsPlanOfCare.PlanofCare.ClinicalInstructionColumn.ColumnName]);
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Name",  ClinicalInstruction},
                                        { "Type", "Instruction" },
                                        { "Instruction" , string.Empty },
                                        { "Date", Utility.ToDateTime(dr[dsPlanOfCare.PlanofCare.CreatedOnColumn.ColumnName]).ToShortDateString()}
                                    };
                            lstFutureAppointment.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region PLAN OF CARE GOAL

                    if (dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanOfCareGoal.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanOfCareGoal.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsPlanOfCare.PlanOfCareGoal.TableName].Rows)
                        {
                            string FutureInstruction = Utility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.SNOMEDDescriptionColumn.ColumnName]);
                            if (Utility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.SNOMEDIDColumn.ColumnName]) != string.Empty)
                            {
                                FutureInstruction = string.Concat(FutureInstruction, " [SNOMED-CT:", Utility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.SNOMEDIDColumn.ColumnName]), "]");
                            }
                            var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Name",  FutureInstruction},
                                        { "Type", "Goal" },
                                        { "Instruction" , Utility.ToStr(dr[dsPlanOfCare.PlanOfCareGoal.InstructionColumn.ColumnName]) },
                                        { "Date", Utility.ToDateTime(dr[dsPlanOfCare.PlanOfCareGoal.CreatedOnColumn.ColumnName]).ToShortDateString()}
                                    };
                            lstGoal.Add(ProblemDatakeyValues);
                        }
                    }

                    #endregion

                    #region Scheduled Procedure Data

                    if (dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsProcedures.Procedures.TableName].Rows)
                        {
                            DateTime dtProcedureDate = Utility.ToDateTime(dr[dsProcedures.Procedures.StartDateColumn.ColumnName]);
                            if (dtNoteDate.Date < dtProcedureDate.Date)
                            {
                                var SnomedText = string.Concat(Utility.ToStr(dr[dsProcedures.Procedures.SNOMED_DESCRIPTIONColumn.ColumnName]), ", [SNOMED-CT:", Utility.ToStr(dr[dsProcedures.Procedures.SNOMEDIDColumn.ColumnName]), "]");
                                var CPTText = string.Concat(Utility.ToStr(dr[dsProcedures.Procedures.CPT_DESCRIPTIONColumn.ColumnName]), ", [SNOMED-CT:", Utility.ToStr(dr[dsProcedures.Procedures.CPTCodeColumn.ColumnName]), "]");
                                var ProblemDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "Name",  SnomedText == string.Empty ? CPTText : SnomedText},
                                        { "Status", Utility.ToStr(dr[dsSocialHx.SocialHx.IsActiveColumn.ColumnName])},
                                        { "Date", Utility.ToDateTime(dr[dsProcedures.Procedures.StartDateColumn.ColumnName]).ToShortDateString()},
                                        { "SnomedId",Utility.ToStr(dr[dsProcedures.Procedures.SNOMEDIDColumn.ColumnName])},
                                        { "CPT", Utility.ToStr(dr[dsProcedures.Procedures.CPTCodeColumn.ColumnName])},
                                        { "Type", "Scheduled Procedure" },
                                        { "Instruction" , string.Empty }
                                    };
                                lstscheduledProcedure.Add(ProblemDatakeyValues);
                            }
                        }
                    }

                    #endregion

                    #region Result

                    if (dsClinicalSummaryHTMLData.Tables[dsLabResult.LabOrderResultDetail.TableName] != null && dsClinicalSummaryHTMLData.Tables[dsLabResult.LabOrderResultDetail.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsClinicalSummaryHTMLData.Tables[dsLabResult.LabOrderResultDetail.TableName].Rows)
                        {
                            string Name = Utility.ToStr(dr[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName]);
                            Name = string.Format("{0}, [LOINC:{1}]", Name, Utility.ToStr(dr[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName]));
                            string ActualResult = Utility.ToStr(dr[dsLabResult.LabOrderResultDetail.ResultColumn.ColumnName]);
                            if (!string.IsNullOrEmpty(Utility.ToStr(dr[dsLabResult.LabOrderResultDetail.UoMColumn.ColumnName])))
                            {
                                ActualResult = string.Format("{0} ({1})", ActualResult, Utility.ToStr(dr[dsLabResult.LabOrderResultDetail.UoMColumn.ColumnName]));
                            }
                            var LabOrderResultDetailDatakeyValues = new Dictionary<string, string>
                                    {
                                        { "LabTest",  Name},
                                        { "ActualResult",  ActualResult},
                                        { "LoincCode", Utility.ToStr(dr[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName])},
                                        { "Unit" ,  Utility.ToStr(dr[dsLabResult.LabOrderResultDetail.UoMColumn.ColumnName]) },
                                        { "ResultValue",  Utility.ToStr(dr[dsLabResult.LabOrderResultDetail.ResultColumn.ColumnName])},
                                        { "ResultDate", Utility.ToDateTime(dr[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]).ToShortDateString()}
                                    };
                            lstResults.Add(LabOrderResultDetailDatakeyValues);
                        }
                    }

                    #endregion



                    #endregion dsNotesData

                    CCDADataModel objCCDAModel = new CCDADataModel(dtNoteDate.ToString(),
                                                                    ReasonForVisit,
                                                                    model.Components,
                                                                    lstPatientData,
                                                                    lstProviderData,
                                                                    lstPracticeData,
                                                                    lstNoteData,
                                                                    lstProblems,
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
                                                                    lstRefferalProviderData);

                    var ccdaDocument = CCDAGenrator.MakeCcda(objCCDAModel, noteId, providerId, patientId, StreamType, documentType);
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
                    Message = ex.Message
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
            model.Components.Add(new Component { componentId = --componentId, componentName = "immunization" });
            model.Components.Add(new Component { componentId = --componentId, componentName = "labresults" });
            List<Dictionary<string, string>> PatientsDataPortability = new List<Dictionary<string, string>>();
            if (model.PatientInfo != null)
            {
                foreach (var Patient in model.PatientInfo)
                {
                    Dictionary<string, string> PatientData = new Dictionary<string, string>();

                    model.PatientId = Patient["PatientId"];
                    model.ProviderId = Patient["ProviderId"];
                    string data = loadClinicalSummaryXMLData(Utility.ToInt64(model.NoteId), Utility.ToInt64(model.ProviderId), Utility.ToInt64(model.PatientId), model, "xml", CCDAGenrator.DocumentTemplateType.DataPortability);
                    var dataBytes = Convert.FromBase64String(data);

                    string XMLString = Encoding.UTF8.GetString(dataBytes);

                    //For Getting The Patient name from XML
                    XmlDocument doc = new XmlDocument();
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("ab", "urn:hl7-org:v3");
                    doc.LoadXml(XMLString);
                    var PatientName = doc.SelectSingleNode("/ab:ClinicalDocument/ab:recordTarget/ab:patientRole/ab:patient/ab:name/ab:given", nsmgr).InnerText;

                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(XMLString);
                    //XmlNode xmlNode = doc.SelectSingleNode("/ClinicalDocument/recordTarget/patientRole/patient/name/given");

                    string html = CCDAGenrator.GetHtmlFromXml(Utility.ToInt64(model.NoteId), Utility.ToInt64(model.ProviderId), Utility.ToInt64(model.PatientId), Encoding.UTF8.GetString(dataBytes), true, string.Empty);
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
                BLObject<DSClinicalSummary> obj = BusinessWrapper.Clinical.BusinessObj.loadPlanOfCare(Utility.ToInt64(model.PlanOfCareId), Utility.ToInt64(model.PatientId), Utility.ToInt64(model.NoteId), 1, 1000);

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
                        planOfCareRow.PatientId = Utility.ToInt64(model.PatientId);
                        planOfCareRow.ClinicalInstruction = Utility.ToStr(model.ClinicalInstruction);
                        planOfCareRow.FutureInstruction = Utility.ToStr(model.FutureInstruction);
                        planOfCareRow.NoteId = Utility.ToInt64(model.NoteId);
                        planOfCareRow.IsActive = true;
                        if (planOfCareRows.Length == 0)
                        {
                            planOfCareRow.CreatedBy = Utility.DecryptFrom64(Common.AppConfig.AppUserName);
                            planOfCareRow.CreatedOn = DateTime.Now;
                        }
                        planOfCareRow.ModifiedBy = Utility.DecryptFrom64(Common.AppConfig.AppUserName);
                        planOfCareRow.ModifiedOn = DateTime.Now;
                        planOfCareRow.SoapText = model.SoapText;
                        if (planOfCareRows.Length < 1)
                        {
                            dsPlanOfCare.PlanofCare.AddPlanofCareRow(planOfCareRow);
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSClinicalSummary> objPlanOfCare = BusinessWrapper.Clinical.BusinessObj.insertUpdatePlanOfCare(dsPlanOfCare);
                    dsPlanOfCare = objPlanOfCare.Data;

                    if (objPlanOfCare.Data != null)
                    {
                        long goalId = 0;
                        Int64 planOfCareId = Utility.ToInt64(dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows[0][dsPlanOfCare.PlanofCare.PlanofCareIdColumn.ColumnName]);
                        if (planOfCareId > 0)
                        {
                            if (lstGoalObject.Count > 0)
                            {
                                long responsefamilyhxdisease = insertUpdatePlanOfCareGoal(planOfCareId, lstGoalObject);
                                goalId = responsefamilyhxdisease;
                            }
                        }

                        BLObject<string> objValue = BusinessWrapper.Clinical.BusinessObj.updateSoapTextForPlanOfCare(planOfCareId);
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            planOfCareId = Utility.ToInt64(dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows[0][dsPlanOfCare.PlanofCare.PlanofCareIdColumn.ColumnName]),
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
                    Message = ex.Message,
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
                    Int64 currentGoalId = Utility.ToInt64(CurrentModel.GoalId);
                    currentGoalId = currentGoalId == 0 ? -1 : currentGoalId;
                    BLObject<DSClinicalSummary> objGoal = BusinessWrapper.Clinical.BusinessObj.loadPlanOfCareGoal(planOfCareId, currentGoalId);
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
                            RowGoal.ICD9Code = Utility.ToStr(CurrentModel.ICD9Code);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.ICD9CodeColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.ICD9CodeDescription))
                        {
                            RowGoal.ICD9CodeDescription = Utility.ToStr(CurrentModel.ICD9CodeDescription);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.ICD9CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10Code))
                        {
                            RowGoal.ICD10Code = Utility.ToStr(CurrentModel.ICD10Code);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.ICD10CodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10CodeDescription))
                        {
                            RowGoal.ICD10CodeDescription = Utility.ToStr(CurrentModel.ICD10CodeDescription);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.ICD10CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDID))
                        {
                            RowGoal.SNOMEDID = Utility.ToStr(CurrentModel.SNOMEDID);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.SNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDDescription))
                        {
                            RowGoal.SNOMEDDescription = Utility.ToStr(CurrentModel.SNOMEDDescription);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.SNOMEDDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCode))
                        {
                            RowGoal.LexiCode = Utility.ToStr(CurrentModel.LexiCode);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.LexiCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCodeDescription))
                        {
                            RowGoal.LexiCodeDescription = Utility.ToStr(CurrentModel.LexiCodeDescription);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.LexiCodeDescriptionColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.Instructions))
                        {
                            RowGoal.Instruction = Utility.ToStr(CurrentModel.Instructions);
                        }
                        else
                        {
                            RowGoal[dsPlanOfCare.PlanOfCareGoal.LexiCodeDescriptionColumn] = DBNull.Value;
                        }
                        RowGoal.IsActive = true;
                        RowGoal.CreatedBy = Utility.DecryptFrom64(Common.AppConfig.AppUserName);
                        RowGoal.CreatedOn = DateTime.Now;
                        RowGoal.ModifiedBy = Utility.DecryptFrom64(Common.AppConfig.AppUserName);
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

            BLObject<DSClinicalSummary> objInsertedGoal = BusinessWrapper.Clinical.BusinessObj.insertUpdatePlanOfCareGoal(dsPlanOfCare);
            if (objInsertedGoal.Data != null)
            {
                goalId = dsPlanOfCare.Tables[dsPlanOfCare.PlanOfCareGoal.TableName].Rows.Count > 0 ? Utility.ToInt64(dsPlanOfCare.Tables[dsPlanOfCare.PlanOfCareGoal.TableName].Rows[0][dsPlanOfCare.PlanOfCareGoal.GoalIdColumn.ColumnName]) : 0;
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
                sb.Append((string.IsNullOrEmpty(modelObj.GoalText) ? "" : "Goal: " + modelObj.GoalText) + (string.IsNullOrEmpty(modelObj.Instructions) ? "" : modelObj.Instructions + "</br>"));
                sb.Append((string.IsNullOrEmpty(modelObj.ClinicalInstructions) ? "" : "Clinical Instruction: " + (modelObj.ClinicalInstructions) + "</br>") + (string.IsNullOrEmpty(modelObj.FutureScheduleAppointments) ? "" : "Future Appointments: " + (modelObj.FutureScheduleAppointments) + "</br>") + "</div>");
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
                if (string.IsNullOrEmpty(Utility.ToStr(model.PatientId)) && planOfCareId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = Utility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSClinicalSummary dsPlanOfCare = null;
                    BLObject<DSClinicalSummary> obj = BusinessWrapper.Clinical.BusinessObj.loadPlanOfCare(planOfCareId, Utility.ToInt64(model.PatientId), Utility.ToInt64(model.NoteId), 1, 1000, "1", "");
                    dsPlanOfCare = obj.Data;
                    if (dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows[0];
                        Int64 PlanId = Utility.ToInt64(dr[dsPlanOfCare.PlanofCare.PlanofCareIdColumn.ColumnName]);
                        var PlanofCarekeyValues = new Dictionary<string, string>
                        {
                            { "PlanOfCareId",  Utility.ToStr(dr[dsPlanOfCare.PlanofCare.PlanofCareIdColumn.ColumnName])},
                            { "PlanOfCareSoapText", Utility.ToStr(dr[dsPlanOfCare.PlanofCare.SoapTextColumn.ColumnName])},
                            { "ClinicalInstructions", Utility.ToStr(dr[dsPlanOfCare.PlanofCare.ClinicalInstructionColumn.ColumnName])},
                            { "FutureAppointments", Utility.ToStr(dr[dsPlanOfCare.PlanofCare.FutureInstructionColumn.ColumnName])}
                        };


                        DSClinicalSummary dsPlanOfCareGoal = null;
                        BLObject<DSClinicalSummary> objGoal = BusinessWrapper.Clinical.BusinessObj.loadPlanOfCareGoal(PlanId, 0);
                        dsPlanOfCareGoal = objGoal.Data;

                        if (dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName].Rows.Count > 0)
                        {
                            DataRow drGoal = dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName].Rows[0];

                            var PlanofCareGoalkeyValues = new Dictionary<string, string>
                        {
                                { "GoalId",  Utility.ToStr(drGoal[dsPlanOfCareGoal.PlanOfCareGoal.GoalIdColumn.ColumnName])},
                                { "Instructions", Utility.ToStr(drGoal[dsPlanOfCareGoal.PlanOfCareGoal.InstructionColumn.ColumnName])},
                                { "PlanOfCareId", Utility.ToStr(drGoal[dsPlanOfCareGoal.PlanOfCareGoal.PlanOfCareIdColumn.ColumnName])},
                        };

                            ////Start 13/01/2016 Muhammad Irfan Fill Diseases in MedicalHx
                            List<Dictionary<string, string>> lstGoal = new List<Dictionary<string, string>>();
                            // var GoalkeyValues = new Dictionary<string, string> { { "", "" } };
                            if (Utility.ToInt64(model.GoalId) > 0)
                            {

                                DSClinicalSummary dsPlanOfCareGoal1 = null;
                                BLObject<DSClinicalSummary> objGoal1 = BusinessWrapper.Clinical.BusinessObj.loadPlanOfCareGoal(PlanId, Utility.ToInt64(model.GoalId));
                                dsPlanOfCareGoal1 = objGoal1.Data;
                                if (dsPlanOfCareGoal1.Tables[dsPlanOfCareGoal1.PlanOfCareGoal.TableName].Rows.Count > 0)
                                {
                                    DataRow drGoal1 = dsPlanOfCareGoal1.Tables[dsPlanOfCareGoal1.PlanOfCareGoal.TableName].Rows[0];

                                    var GoalkeyValues = new Dictionary<string, string>
                        {
                                { "GoalId",  Utility.ToStr(drGoal1[dsPlanOfCareGoal1.PlanOfCareGoal.GoalIdColumn.ColumnName])},
                                { "Instructions", Utility.ToStr(drGoal1[dsPlanOfCareGoal1.PlanOfCareGoal.InstructionColumn.ColumnName])},
                                { "PlanOfCareId", Utility.ToStr(drGoal1[dsPlanOfCareGoal1.PlanOfCareGoal.PlanOfCareIdColumn.ColumnName])},
                        };

                                    ////End 13/01/2016 Muhammad Irfan Fill Diseases in MedicalHx


                                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    var response = new
                                    {
                                        status = true,
                                        PlanOfCareFill_JSON = js.Serialize(PlanofCarekeyValues),
                                        GoalFill_JSON = js.Serialize(GoalkeyValues),
                                        GoalLoad_JSON = HttpUtility.HtmlDecode(Utility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName])),
                                        PlanOfCareGoalLoad_JSON = Utility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName]),
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
                                        GoalLoad_JSON = HttpUtility.HtmlDecode(Utility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName])),
                                        PlanOfCareGoalLoad_JSON = Utility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName]),
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
                                    GoalLoad_JSON = HttpUtility.HtmlDecode(Utility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName])),
                                    PlanOfCareGoalLoad_JSON = Utility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName]),
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
                                GoalLoad_JSON = HttpUtility.HtmlDecode(Utility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName])),
                                PlanOfCareGoalLoad_JSON = Utility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.PlanOfCareGoal.TableName]),
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
                            PlanOfCareGoalLoad_JSON = Utility.JSON_DataTable(dsPlanOfCare.Tables[dsPlanOfCare.PlanOfCareGoal.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                //  return "";
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        //internal string attachPlanOfCareWithNotes(string planOfCareId, long notesId)
        //{
        //    try
        //    {
        //        DSClinicalSummary dsPlanOfCare = null;
        //        if (string.IsNullOrEmpty(Utility.ToStr(planOfCareId)) || string.IsNullOrEmpty(Utility.ToStr(notesId)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = Utility.ToStr(Common.AppPrivileges.CheckBox_Message)
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {
        //            BLObject<DSClinicalSummary> obj = BusinessWrapper.Clinical.BusinessObj.attachPlanOfCareWithNotes(planOfCareId, notesId);
        //            if (obj.Data != null)
        //            {
        //                dsPlanOfCare = obj.Data;
        //                var response = new
        //                {
        //                    status = true,
        //                    PlanOfCareTotalCount = dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count,
        //                    PlanOfCareCount = dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count,
        //                    PlanOfCareLoad_JSON = Utility.JSON_DataTable(dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName]),
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
        //            Message = ex.Message,
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
                if (string.IsNullOrEmpty(Utility.ToStr(planOfCareId)) || string.IsNullOrEmpty(Utility.ToStr(notesId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = Utility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BusinessWrapper.Clinical.BusinessObj.detachPlanOfCareFromNotes(planOfCareId, notesId);
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
                    Message = ex.Message,
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
                BLObject<DSClinicalSummary> obj = BusinessWrapper.Clinical.BusinessObj.loadPlanOfCare(Utility.ToInt64(model.PlanOfCareId), Utility.ToInt64(model.PatientId), Utility.ToInt64(model.NoteId), 1, 2000);
                dsPlanOfCare = obj.Data;
                if (dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName].Rows.Count > 0)
                {

                    obj = BusinessWrapper.Clinical.BusinessObj.insertUpdatePlanOfCare(dsPlanOfCare);

                    var response = new
                    {
                        status = true,


                        PlanofCareLoad_JSON = Utility.JSON_DataTable(dsPlanOfCare.Tables[dsPlanOfCare.PlanofCare.TableName]),
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
                    Message = ex.Message,
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
                if (string.IsNullOrEmpty(Utility.ToStr(goalId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = Utility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BusinessWrapper.Clinical.BusinessObj.deletePlanOfCareGoal(Utility.ToStr(goalId));
                    if (obj.Data == "")
                    {
                        BLObject<string> objValue = BusinessWrapper.Clinical.BusinessObj.updateSoapTextForPlanOfCare(Utility.ToInt64(planOfCareId));
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
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public DataRow[] VaccinceRows { get; set; }
    }
}