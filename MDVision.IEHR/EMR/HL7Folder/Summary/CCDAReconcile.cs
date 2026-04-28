using MDVision.IEHR.EMR.Helpers.Clinical.Summary;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.IEHR.Common;
using System.Runtime.Serialization.Formatters.Binary;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.HL7Folder.Summary
{
    public static class CCDAReconcile
    {
       
        public class PatientPreliminary
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? DOB { get; set; }
        }

        public class USRealmName : PatientPreliminary
        {
            public string MiddleName { get; set; }
        }

        public static class DocumentSections
        {
            public static string Allergies = "2.16.840.1.113883.10.20.22.2.6.1";
            public static string Medications = "2.16.840.1.113883.10.20.22.2.1.1";
            public static string Problem = "2.16.840.1.113883.10.20.22.2.5.1";
        }

        enum TableSchema
        {
            Patient,
            Allergies,
            Medications,
            Problem
        };

        public enum TextFormateType
        {
            Normal,
            Upper,
            Lower,
            Title
        }

        public static DSPatient ReconcileCCDA(string xmlContent, List<string> sectionList, ref List<string> errors, long PatientId = 0)
        {

            ClinicalDocument CCDA;
            DSPatient ds = new DSPatient();
            errors = new List<string>();

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                CCDA = GetDeserializeDocument(byteArray, ref errors);
                if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                {
                    GetDoucmentData(CCDA, ref ds, ref sectionList, ref errors, PatientId);
                    return ds;
                }
                else
                {
                    throw new Exception("The selected file is not valid");
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }

        }

        private static ClinicalDocument GetDeserializeDocument(byte[] byteArray, ref List<string> errors)
        {
            ClinicalDocument document = null;
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(ClinicalDocument));
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(byteArray, 0, byteArray.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                document = (ClinicalDocument)mySerializer.Deserialize(memStream);
                memStream.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return document;
        }

        public static void GetDoucmentData(ClinicalDocument document, ref DSPatient mainDataSet, ref List<string> sectionList, ref List<string> errors, long PatientId = 0)
        {
            if (document != null)
            {
                if (PatientId == 0)
                {
                    BLObject<DSClinicalSummary> dsPatientLookUp = new BLLClinical().PatientLookup();
                    PatientRole pr = document.GetPatientRole();
                    DataTable dtPatient = mainDataSet.Tables[mainDataSet.Patients.TableName];
                    GetPatientData(pr, ref dtPatient, ref errors, dsPatientLookUp.Data);
                    if (errors.IsNullOrEmpty() && dtPatient.HasRows())
                    {
                        mainDataSet.Merge(dtPatient);
                    }
                    else
                    {
                        //Patient Not Exists
                        return;
                    }
                    string ConfidenitalityCode = document.GetConfidentialityCode();
                    if (!string.IsNullOrEmpty(ConfidenitalityCode))
                    {
                        DSPatient dsPatient = new DSPatient();
                        mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.ConfidentialityCodeColumn.ColumnName] = ConfidenitalityCode;
                        mainDataSet.AcceptChanges();



                    }
                }
                else
                {
                    BLObject<DSPatient> dsPatient = new BLLClinical().FillPatient(PatientId);
                    if (dsPatient.Data != null)
                    {
                        mainDataSet.Merge(dsPatient.Data);
                    }
                }

                if (sectionList.IsNullOrEmpty()) return;
                foreach (string sectionId in sectionList)
                {
                    if (sectionId == DocumentSections.Allergies)
                    {
                        DSAllergies ds = new DSAllergies();
                        DataTable dtAllergies = ds.Tables[ds.Allergy.TableName];

                        GetAllergiesData(document, ref dtAllergies, ref errors, PatientId);
                        if (dtAllergies.Rows.Count > 0)
                        {
                            mainDataSet.Merge(dtAllergies);
                        }

                    }
                    else if (sectionId == DocumentSections.Medications)
                    {
                        DSClinicalMedication ds = new DSClinicalMedication();
                        DataTable dtMedication = ds.Tables[ds.Medication.TableName];

                        GetMedicationsData(document, ref dtMedication, ref errors, PatientId);
                        if (dtMedication.Rows.Count > 0)
                        {
                            mainDataSet.Merge(dtMedication);
                        }

                    }
                    else if (sectionId == DocumentSections.Problem)
                    {
                        DSProblemLists ds = new DSProblemLists();
                        DataTable dtProblems = ds.Tables[ds.ProblemList.TableName];

                        GetProblemsData(document, ref dtProblems, ref errors, PatientId);
                        if (dtProblems.Rows.Count > 0)
                        {
                            mainDataSet.Merge(dtProblems);
                        }

                    }
                }
            }
        }

        private static void GetPatientData(PatientRole patientRole, ref DataTable dtPatient, ref List<string> errors, DSClinicalSummary dsPatientLookUp)
        {
            DSPatient dsPatient = new DSPatient();
            if (patientRole != null)
            {
                DataRow dr = dtPatient.NewRow();
                try
                {

                    if (!patientRole.addr.IsNullOrEmpty() && patientRole.addr[0] != null
                    && !patientRole.addr[0].Items.IsNullOrEmpty())
                    {
                        List<ADXP> addList = patientRole.addr[0].Items;
                        foreach (ADXP adxp in addList)
                        {
                            if (adxp != null && !adxp.Text.IsNullOrEmpty() && !String.IsNullOrEmpty(adxp.Text[0]))
                            {
                                if (adxp is adxpstate)
                                {
                                    dr[dsPatient.Patients.StateColumn.ColumnName] = adxp.Text[0].ToUpper();
                                }
                                else if (adxp is adxpcity)
                                {
                                    dr[dsPatient.Patients.CityColumn.ColumnName] = adxp.Text[0].ToUpper();
                                }
                                else if (adxp is adxppostalCode)
                                {
                                    dr[dsPatient.Patients.ZIPCodeColumn.ColumnName] = adxp.Text[0].ToUpper();
                                }
                                else if (adxp is adxpstreetAddressLine)
                                {
                                    string streetAdd = "";
                                    foreach (string str in adxp.Text)
                                    {
                                        if (!String.IsNullOrWhiteSpace(str))
                                        {
                                            streetAdd = streetAdd + " " + str.Trim();
                                        }
                                    }
                                    dr[dsPatient.Patients.Address1Column.ColumnName] = streetAdd.ToUpper();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add("Patient Address");
                }


                #region Patient

                Patient p = patientRole.patient;
                if (p != null)
                {
                    #region Name
                    try
                    {
                        if (!p.name.IsNullOrEmpty() && !p.name[0].Items.IsNullOrEmpty())
                        {
                            USRealmName name = GetUSRealmName(p.name[0], ref errors);

                            dr[dsPatient.Patients.LastNameColumn.ColumnName] = name.LastName;
                            dr[dsPatient.Patients.FirstNameColumn.ColumnName] = name.FirstName;
                            dr[dsPatient.Patients.MIColumn.ColumnName] = name.MiddleName;
                        }
                    }
                    catch (Exception)
                    {
                        errors.Add("Patient Name");
                    }
                    #endregion

                    #region Gender
                    try
                    {
                        if (p.administrativeGenderCode != null &&
                            !String.IsNullOrWhiteSpace(p.administrativeGenderCode.code))
                        {
                            //Get List From db to access Code
                            dr[dsPatient.Patients.GenderColumn.ColumnName] = CCDAReconcile.GetGender(p.administrativeGenderCode.code); ;
                        }
                    }
                    catch (Exception exc)
                    {
                        errors.Add("Patient Gender " + exc.Message);
                    }

                    #endregion

                    #region DOB
                    try
                    {
                        if (p.birthTime != null && !String.IsNullOrWhiteSpace(p.birthTime.value)
                            && p.birthTime.value.Length >= 8)
                        {
                            dr[dsPatient.Patients.DOBColumn.ColumnName] = p.birthTime.value.ToFormatedDateTimeByFormat("yyyyMMdd");
                        }
                    }
                    catch (Exception exc)
                    {
                        errors.Add("Patient DOB" + exc.Message);
                    }

                    #endregion

                    #region Martial Status
                    try
                    {
                        if (p.maritalStatusCode != null
                            && !String.IsNullOrWhiteSpace(p.maritalStatusCode.code))
                        {
                            //Get List From db to access Code

                            dr[dsPatient.Patients.MaritialStatusColumn.ColumnName] = CCDAReconcile.GetMaritalStatusCode(p.maritalStatusCode.code);
                        }
                    }
                    catch (Exception exc)
                    {
                        errors.Add("Patient Marital Status " + exc.Message);
                    }

                    #endregion

                    #region Race
                    try
                    {
                        if (p.raceCode != null && !String.IsNullOrWhiteSpace(p.raceCode.code))
                        {

                            dr[dsPatient.Patients.RaceIdColumn.ColumnName] = GetRaceIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Race.TableName], MDVUtility.ToStr(p.raceCode.code));
                        }
                    }
                    catch (Exception exc)
                    {
                        errors.Add("Patient Race " + exc.Message);
                    }
                    #endregion

                    #region Ethnicity
                    try
                    {
                        if (p.ethnicGroupCode != null && !String.IsNullOrWhiteSpace(p.ethnicGroupCode.code))
                        {
                            dr[dsPatient.Patients.EthnicityIdColumn.ColumnName] = GetEthnicityIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Ethnicity.TableName], MDVUtility.ToStr(p.ethnicGroupCode.code));
                        }
                    }
                    catch (Exception exc)
                    {
                        errors.Add("Patient Ethnicity " + exc.Message);
                    }
                    #endregion

                    #region Language Communication
                    try
                    {
                        if (!p.languageCommunication.IsNullOrEmpty() && p.languageCommunication[0].languageCode != null
                            && !String.IsNullOrWhiteSpace(p.languageCommunication[0].languageCode.code))
                        {
                            //Get Application Relevant Code to embed in db
                            dr[dsPatient.Patients.PrefLanguageIdColumn.ColumnName] = GetLanguageIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Languages.TableName], MDVUtility.ToStr(p.languageCommunication[0].languageCode.code));
                        }

                    }
                    catch (Exception exc)
                    {
                        errors.Add("Patient Language" + exc.Message);
                    }
                    #endregion
                }

                #endregion

                #region Location

                try
                {

                    //Fetch LocationID from db
                    //dr["UserLocationID"] = "";
                }
                catch (Exception)
                {
                    errors.Add("Patient Location");
                }

                #endregion

                #region Provider

                try
                {

                    dr[dsPatient.Patients.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;
                    dr[dsPatient.Patients.PracticeIdColumn.ColumnName] = MDVSession.Current.DefaultPracticeId;
                    dr[dsPatient.Patients.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                    dr[dsPatient.Patients.EntityIdColumn.ColumnName] = MDVSession.Current.EntityId;




                    //Fetch ProviderID from db
                    //dr["UserProviderID"] = "";
                }
                catch (Exception)
                {
                    errors.Add("Patient Provider");
                }

                #endregion

                #region StaticData

                dr[dsPatient.Patients.IsActiveColumn.ColumnName] = true;


                //Fetch PracticeID from db
                //dr["PracticeID"] = "";
                #endregion

                dtPatient.Rows.Add(dr);
            }
        }

        private static USRealmName GetUSRealmName(PN personName, ref List<string> errors)
        {
            USRealmName name = new USRealmName();

            if (personName != null)
            {
                foreach (ENXP item in personName.Items)
                {
                    if (item != null && !item.Text.IsNullOrEmpty()
                        && !String.IsNullOrWhiteSpace(item.Text[0]))
                    {
                        if (item is enfamily)
                        {
                            name.LastName = item.Text[0].ToUpper();
                        }
                        else if (item is engiven)
                        {
                            if (String.IsNullOrWhiteSpace(name.FirstName))
                            {
                                name.FirstName = item.Text[0].ToUpper();
                            }
                            else
                            {
                                name.MiddleName = item.Text[0].ToUpper();
                            }
                        }
                    }
                }
            }

            return name;
        }

        private static void GetAllergiesData(ClinicalDocument document, ref DataTable dtAllergies, ref List<string> errors, long PatientId = 0)
        {
            try
            {
                DSAllergies ds = new DSAllergies();
                Component3 medicationAllergiesComponent = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref medicationAllergiesComponent, DocumentSections.Allergies);

                List<Entry> entryList = null;
                Observation allergyIntoleranceObservation = null;
                if (isComponentExist && medicationAllergiesComponent.GetEntry(ref entryList))
                {
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item is Act)
                        {
                            Act allergyProblemAct = e.Item as Act;

                            if (!allergyProblemAct.entryRelationship.IsNullOrEmpty() && allergyProblemAct.entryRelationship[0].Item != null
                                && allergyProblemAct.entryRelationship[0].Item is Observation)
                            {
                                allergyIntoleranceObservation = allergyProblemAct.entryRelationship[0].Item as Observation;
                                if (allergyIntoleranceObservation.participant.Count > 0) // no known allergy check
                                {
                                    DataRow dr = dtAllergies.NewRow();

                                    //Get Value from Global Variable
                                    dr[ds.Allergy.PatientIdColumn.ColumnName] = PatientId;

                                    if (allergyIntoleranceObservation.effectiveTime != null)
                                    {
                                        string lowTime, highTime;
                                        GetLowHighTime(allergyIntoleranceObservation.effectiveTime, out lowTime, out highTime);

                                        if (!string.IsNullOrWhiteSpace(lowTime) && lowTime != "UNK")
                                        {
                                            dr[ds.Allergy.OnSetDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                        }

                                        if (!string.IsNullOrWhiteSpace(highTime))
                                        {
                                            dr[ds.Allergy.LastModifiedColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                        }
                                    }

                                    if (!allergyIntoleranceObservation.value.IsNullOrEmpty() && allergyIntoleranceObservation.value[0] is CD
                                        && !String.IsNullOrWhiteSpace((allergyIntoleranceObservation.value[0] as CD).code))
                                    {
                                        string code = (allergyIntoleranceObservation.value[0] as CD).code;

                                        if (code == "416098002")
                                        {
                                            dr[ds.Allergy.TypeColumn.ColumnName] = "Drug allergy";
                                        }
                                    }


                                    if (!allergyIntoleranceObservation.participant.IsNullOrEmpty() && allergyIntoleranceObservation.participant[0] != null
                                        && allergyIntoleranceObservation.participant[0].participantRole != null
                                        && allergyIntoleranceObservation.participant[0].participantRole.Item != null
                                        && allergyIntoleranceObservation.participant[0].participantRole.Item is PlayingEntity
                                        && (allergyIntoleranceObservation.participant[0].participantRole.Item as PlayingEntity).code != null
                                        && !String.IsNullOrWhiteSpace((allergyIntoleranceObservation.participant[0].participantRole.Item as PlayingEntity).code.code))
                                    {
                                        string rxNormId = (allergyIntoleranceObservation.participant[0].participantRole.Item as PlayingEntity).code.code;
                                        string substance = (allergyIntoleranceObservation.participant[0].participantRole.Item as PlayingEntity).code.displayName;

                                        dr[ds.Allergy.RxnormIDColumn.ColumnName] = rxNormId;
                                        dr[ds.Allergy.AllergenColumn.ColumnName] = substance;
                                    }
                                    else if (!allergyIntoleranceObservation.participant.IsNullOrEmpty() &&
                                                allergyIntoleranceObservation.participant[0] != null
                                                && allergyIntoleranceObservation.participant[0].participantRole != null
                                                && allergyIntoleranceObservation.participant[0].participantRole.Item != null
                                                && allergyIntoleranceObservation.participant[0].participantRole.Item is PlayingEntity
                                                && (allergyIntoleranceObservation.participant[0].participantRole.Item as PlayingEntity).name != null
                                                && !String.IsNullOrWhiteSpace((allergyIntoleranceObservation.participant[0].participantRole.Item as PlayingEntity).name[0].Text[0])
                                                )
                                    {
                                        string substance = (allergyIntoleranceObservation.participant[0].participantRole.Item as PlayingEntity).name[0].Text[0];
                                        dr[ds.Allergy.RxnormIDColumn.ColumnName] = 0;
                                        dr[ds.Allergy.AllergenColumn.ColumnName] = substance;
                                    }

                                    if (!allergyIntoleranceObservation.entryRelationship.IsNullOrEmpty())
                                    {
                                        foreach (EntryRelationship er in allergyIntoleranceObservation.entryRelationship)
                                        {
                                            if (er != null && er.Item != null && er.Item is Observation)
                                            {
                                                allergyIntoleranceObservation = er.Item as Observation;

                                                //Allergy Status Observation
                                                if (!allergyIntoleranceObservation.templateId.IsNullOrEmpty() && allergyIntoleranceObservation.templateId[0] != null
                                                    && !string.IsNullOrWhiteSpace(allergyIntoleranceObservation.templateId[0].root)
                                                    && allergyIntoleranceObservation.templateId[0].root == "2.16.840.1.113883.10.20.22.4.28"
                                                    && !allergyIntoleranceObservation.value.IsNullOrEmpty() && allergyIntoleranceObservation.value[0] != null && allergyIntoleranceObservation.value[0] is CE
                                                    && !String.IsNullOrWhiteSpace((allergyIntoleranceObservation.value[0] as CE).code))
                                                {
                                                    if ((allergyIntoleranceObservation.value[0] as CE).code == "55561003") //active
                                                    {
                                                        dr[ds.Allergy.IsActiveColumn.ColumnName] = true;
                                                    }
                                                    else if ((allergyIntoleranceObservation.value[0] as CE).code == "73425007") //inactive
                                                    {
                                                        dr[ds.Allergy.IsActiveColumn.ColumnName] = false;
                                                    }
                                                }

                                                //Allergy Reaction Observation
                                                else if (!allergyIntoleranceObservation.templateId.IsNullOrEmpty() && allergyIntoleranceObservation.templateId[0] != null
                                                    && !string.IsNullOrWhiteSpace(allergyIntoleranceObservation.templateId[0].root)
                                                    && allergyIntoleranceObservation.templateId[0].root == "2.16.840.1.113883.10.20.22.4.9"
                                                    && !allergyIntoleranceObservation.value.IsNullOrEmpty() && allergyIntoleranceObservation.value[0] != null && allergyIntoleranceObservation.value[0] is CD &&
                                                    !String.IsNullOrWhiteSpace((allergyIntoleranceObservation.value[0] as CD).code))
                                                {
                                                    string reactionRxNormId = (allergyIntoleranceObservation.value[0] as CD).code;

                                                    dr[ds.Allergy.RxnormIDColumn.ColumnName] = reactionRxNormId;

                                                    //Allergin Reaction Observation Severity
                                                    if (!allergyIntoleranceObservation.entryRelationship.IsNullOrEmpty() && allergyIntoleranceObservation.entryRelationship[0] != null
                                                        && allergyIntoleranceObservation.entryRelationship[0].Item != null && allergyIntoleranceObservation.entryRelationship[0].Item is Observation
                                                        && !(allergyIntoleranceObservation.entryRelationship[0].Item as Observation).value.IsNullOrEmpty()
                                                        && (allergyIntoleranceObservation.entryRelationship[0].Item as Observation).value[0] is CD
                                                        && !String.IsNullOrWhiteSpace(((allergyIntoleranceObservation.entryRelationship[0].Item as Observation).value[0] as CD).code))
                                                    {
                                                        string severitySnomedId = ((allergyIntoleranceObservation.entryRelationship[0].Item as Observation).value[0] as CD).code;

                                                        dr[ds.Allergy.SeverityColumn.ColumnName] = severitySnomedId;
                                                    }
                                                }

                                                else if (!allergyIntoleranceObservation.templateId.IsNullOrEmpty() && allergyIntoleranceObservation.templateId[0] != null
                                                    && !string.IsNullOrWhiteSpace(allergyIntoleranceObservation.templateId[0].root)
                                                    && allergyIntoleranceObservation.templateId[0].root == "2.16.840.1.113883.10.20.22.4.8"
                                                    && !allergyIntoleranceObservation.value.IsNullOrEmpty() && allergyIntoleranceObservation.value[0] != null && allergyIntoleranceObservation.value[0] is CD &&
                                                    !String.IsNullOrWhiteSpace((allergyIntoleranceObservation.value[0] as CD).code))
                                                {
                                                    string severitySnomedId = (allergyIntoleranceObservation.value[0] as CD).code;

                                                    dr[ds.Allergy.SeverityColumn.ColumnName] = severitySnomedId;
                                                }
                                            }
                                        }
                                    }

                                    // Add Allergy Row
                                    if (dr[ds.Allergy.AllergenColumn.ColumnName] != DBNull.Value)
                                    {
                                        dtAllergies.Rows.Add(dr);
                                    }

                                }
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                errors.Add("Allergies");
            }
        }

        private static List<string> GetMedicationsSig(object obj)
        {
            try
            {
                List<string> lstMedication = new List<string>();
                StrucDocTable HtmlTable = (StrucDocTable)obj;

                List<StrucDocTbody> Body = HtmlTable.tbody;
                if (Body != null && Body.Count > 0)
                {
                    List<StrucDocTr> listTr = Body[0].tr;
                    foreach (var tr in listTr)
                    {
                        StrucDocTd td = (StrucDocTd)tr.Items[1];
                        lstMedication.Add((string)td.Text.FirstOrDefault());
                    }
                }
                return lstMedication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void GetMedicationsData(ClinicalDocument document, ref DataTable dtMedication, ref List<string> errors, long PatientId = 0)
        {
            try
            {
                DSClinicalMedication dsMedication = new DSClinicalMedication();
                Component3 medicationsComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref medicationsComp, DocumentSections.Medications);

                List<Entry> entryList = null;
                List<string> SigText = null;

                if (isComponentExist && medicationsComp.GetEntry(ref entryList))
                {

                    SigText = GetMedicationsSig(medicationsComp.section.text.Items.FirstOrDefault());
                    var medicationId = 0;
                    for (var i = 0; i < entryList.Count; i++ )
                    {
                        if (entryList[i] != null && entryList[i].Item != null && entryList[i].Item is SubstanceAdministration)
                        {
                            SubstanceAdministration medicationActivity = entryList[i].Item as SubstanceAdministration;
                            var MedciationSigText = SigText[i];
                            if (string.IsNullOrEmpty(medicationActivity.nullFlavor))
                            {
                                DataRow dr = dtMedication.NewRow();
                                dr[dsMedication.Medication.MedicationIDColumn.ColumnName] = --medicationId;
                                //Get Value from Global Variable
                                dr[dsMedication.Medication.PatientIDColumn.ColumnName] = PatientId;

                                if (medicationActivity.statusCode != null && !String.IsNullOrWhiteSpace(medicationActivity.statusCode.code))
                                {
                                    dr[dsMedication.Medication.StatusColumn.ColumnName] = medicationActivity.statusCode.code;
                                    if (medicationActivity.text != null && medicationActivity.text.Text.Count > 0)
                                        dr[dsMedication.Medication.MedicationNameColumn.ColumnName] = medicationActivity.text.Text[0];
                                }
                                else
                                {
                                    errors.Add("Medication --> Rx Status code not available in the file");
                                }

                                if (!medicationActivity.effectiveTime.IsNullOrEmpty() && medicationActivity.effectiveTime[0] != null
                                    && medicationActivity.effectiveTime[0] is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(medicationActivity.effectiveTime[0] as IVL_TS, out lowTime, out highTime);

                                    if (!String.IsNullOrWhiteSpace(lowTime))
                                    {
                                        dr[dsMedication.Medication.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }

                                    if (!String.IsNullOrWhiteSpace(highTime))
                                    {
                                        dr[dsMedication.Medication.StopDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else
                                {
                                    errors.Add("Medication --> Start Date not available in the file");
                                }

                                if (!medicationActivity.effectiveTime.IsNullOrEmpty(1) && medicationActivity.effectiveTime[1] != null
                                    && medicationActivity.effectiveTime[1] is PIVL_TS)
                                {
                                    PIVL_TS time = medicationActivity.effectiveTime[1] as PIVL_TS;
                                    if (time.period != null && !String.IsNullOrWhiteSpace(time.period.value) && !String.IsNullOrWhiteSpace(time.period.unit))
                                    {
                                        dr[dsMedication.Medication.QuantityColumn.ColumnName] = time.period.value;
                                    }
                                }

                                if (medicationActivity.repeatNumber != null && !String.IsNullOrWhiteSpace(medicationActivity.repeatNumber.value))
                                {
                                    if (medicationActivity.repeatNumber.value.ToFormatedInt64() > 0)
                                    {
                                        dr[dsMedication.Medication.RefillColumn.ColumnName] = medicationActivity.repeatNumber.value.ToFormatedInt64();
                                    }
                                }

                                //Route Code
                                if (medicationActivity.routeCode != null && !String.IsNullOrWhiteSpace(medicationActivity.routeCode.code))
                                {
                                    dr[dsMedication.Medication.RoutebyColumn.ColumnName] = medicationActivity.routeCode.displayName;
                                }

                                //Dose Qunatity
                                //if (medicationActivity.doseQuantity != null && !String.IsNullOrWhiteSpace(medicationActivity.doseQuantity.value))
                                //{
                                //    dr[dsMedication.Medication.DoseColumn.ColumnName] = medicationActivity.doseQuantity.value;
                                //}

                                dr[dsMedication.Medication.DoseOtherColumn.ColumnName] = MedciationSigText;

                                if (medicationActivity.consumable != null && medicationActivity.consumable.manufacturedProduct != null
                                    && medicationActivity.consumable.manufacturedProduct.Item is Material)
                                {
                                    Material manufacturedMaterial = medicationActivity.consumable.manufacturedProduct.Item as Material;
                                    if (manufacturedMaterial.code != null && !String.IsNullOrWhiteSpace(manufacturedMaterial.code.code))
                                    {
                                        dr[dsMedication.Medication.RxnormIDColumn.ColumnName] = manufacturedMaterial.code.code;

                                        long drugId = new BLLClinical().getDrugIdByRxnormId(MDVUtility.ToLong(manufacturedMaterial.code.code), MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), manufacturedMaterial.code.displayName);

                                        if (drugId > 0)
                                            dr[dsMedication.Medication.DrugIDColumn.ColumnName] = drugId; //Based on RxNormID get DRUG ID from Drug table
                                        else
                                            dr[dsMedication.Medication.DrugIDColumn.ColumnName] = DBNull.Value;

                                        dr[dsMedication.Medication.MedicationNameColumn.ColumnName] = manufacturedMaterial.code.displayName;
                                    }
                                }
                                else
                                {
                                    errors.Add("Medication --> Rx Norm Code not found in the file");
                                }


                                if (dr[dsMedication.Medication.DrugIDColumn.ColumnName] != DBNull.Value)
                                    dtMedication.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Medications");
            }
        }

        private static void GetProblemsData(ClinicalDocument document, ref DataTable dtProblems, ref List<string> errors, long PatientId)
        {
            try
            {
                DSProblemLists dsProblems = new DSProblemLists();
                Component3 problemsComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref problemsComp, DocumentSections.Problem);

                List<Entry> entryList = null;
                if (isComponentExist && problemsComp.GetEntry(ref entryList))
                {
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Act)
                        {
                            Act act = e.Item as Act;
                            if (!act.entryRelationship.IsNullOrEmpty())
                            {
                                foreach (EntryRelationship er in act.entryRelationship)
                                {
                                    if (er.Item != null && er.Item is Observation)
                                    {
                                        Observation problemObservation = er.Item as Observation;
                                        DataRow dr = dtProblems.NewRow();

                                        //Get Value from Global Variable
                                        dr[dsProblems.ProblemList.PatientIdColumn.ColumnName] = PatientId;

                                        if (problemObservation.effectiveTime != null)
                                        {
                                            string lowTime, highTime;
                                            GetLowHighTime(problemObservation.effectiveTime, out lowTime, out highTime);

                                            if (!String.IsNullOrWhiteSpace(lowTime))
                                            {
                                                dr[dsProblems.ProblemList.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                            }
                                        }
                                        else
                                        {
                                            errors.Add("Problems --> DiagnosedDate not found");
                                        }

                                        if (!problemObservation.value.IsNullOrEmpty() && problemObservation.value[0] != null
                                            && problemObservation.value[0] is CD)
                                        {
                                            CD snomedID = problemObservation.value[0] as CD;
                                            if (!String.IsNullOrWhiteSpace(snomedID.code))
                                            {
                                                dr[dsProblems.ProblemList.SNOMEDIDColumn.ColumnName] = snomedID.code;
                                                if (!String.IsNullOrWhiteSpace(snomedID.displayName))
                                                {
                                                    dr[dsProblems.ProblemList.ProblemNameColumn.ColumnName] = snomedID.displayName;
                                                    dr[dsProblems.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName] = snomedID.displayName;
                                                }
                                            }

                                            //Check for ICD
                                            if (dr[dsProblems.ProblemList.ProblemNameColumn.ColumnName] == DBNull.Value)
                                            {
                                                if (!snomedID.translation.IsNullOrEmpty() && !String.IsNullOrWhiteSpace(snomedID.translation[0].code))
                                                {
                                                    dr[dsProblems.ProblemList.ProblemNameColumn.ColumnName] = snomedID.translation[0].displayName;
                                                    dr[dsProblems.ProblemList.ICD10_DescriptionColumn.ColumnName] = snomedID.translation[0].displayName;
                                                    dr[dsProblems.ProblemList.ICD10Column.ColumnName] = snomedID.translation[0].code;
                                                }
                                                else
                                                {
                                                    errors.Add("Problems --> ICD code not in the file");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            errors.Add("Problems --> Snomed AND ICD is not in the file");
                                        }



                                        if (!problemObservation.entryRelationship.IsNullOrEmpty())
                                        {
                                            foreach (EntryRelationship entryRelationship in problemObservation.entryRelationship)
                                            {
                                                if (entryRelationship != null && entryRelationship.Item != null
                                                    && entryRelationship.Item is Observation)
                                                {
                                                    Observation problemStatusObs = entryRelationship.Item as Observation;

                                                    if (!problemStatusObs.templateId.IsNullOrEmpty()
                                                        && problemStatusObs.templateId[0].root == "2.16.840.1.113883.10.20.22.4.6"
                                                        && !problemStatusObs.value.IsNullOrEmpty()
                                                        && problemStatusObs.value[0] != null
                                                        && problemStatusObs.value[0] is CD
                                                        && !String.IsNullOrWhiteSpace((problemStatusObs.value[0] as CD).code))
                                                    {
                                                        if ((problemStatusObs.value[0] as CD).displayName.Equals("Active"))
                                                        {
                                                            dr[dsProblems.ProblemList.IsActiveColumn.ColumnName] = true;
                                                        }
                                                        else
                                                        {
                                                            dr[dsProblems.ProblemList.IsActiveColumn.ColumnName] = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (dr[dsProblems.ProblemList.ProblemNameColumn.ColumnName] != DBNull.Value)
                                            dtProblems.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Problems");
            }
        }

        private static void GetLowHighTime(IVL_TS effectiveTime, out string lowTime, out string highTime)
        {
            lowTime = highTime = "";

            if (effectiveTime.ItemsElementName != null && effectiveTime.Items != null)
            {
                ItemsChoiceType2[] timeName = effectiveTime.ItemsElementName;
                QTY[] timeValues = effectiveTime.Items;

                for (int i = 0; i < 2; i++)
                {
                    if (timeName.Length > i && timeValues.Length > i && timeValues[i] != null && timeValues[i] is IVXB_TS &&
                        String.IsNullOrWhiteSpace((timeValues[i] as IVXB_TS).nullFlavor))
                    {
                        IVXB_TS timeVal = timeValues[i] as IVXB_TS;

                        if (timeName[i] == ItemsChoiceType2.low)
                        {
                            lowTime = timeVal.value != "" ? timeVal.value : "";
                        }

                        if (timeName[i] == ItemsChoiceType2.high)
                        {
                            highTime = timeVal.value != "" ? timeVal.value : "";
                        }
                    }
                }
            }
        }

        #region MDVUtility Methods
        private static string RemoveChars(string str, char[] charsToRemove)
        {
            if (!String.IsNullOrWhiteSpace(str) && charsToRemove.Length > 0)
            {
                string[] chars = new string[charsToRemove.Length];

                for (int i = 0; i < charsToRemove.Length; i++)
                {
                    chars[i] = charsToRemove[i].ToString();
                }

                foreach (string c in chars)
                {
                    str = str.Replace(c, "");
                }
            }
            return str;
        }

        private static DataTable GetTableSchema(string schema)
        {
            DataTable dt = new DataTable(schema);

            if (schema == TableSchema.Patient.ToString())
            {
                dt.Columns.Add("State");
                dt.Columns.Add("City");
                dt.Columns.Add("ZipCode");
                dt.Columns.Add("Address");
                dt.Columns.Add("LastName");
                dt.Columns.Add("FirstName");
                dt.Columns.Add("MiddleName");
                dt.Columns.Add("Gender");
                dt.Columns.Add("DOB");
                dt.Columns.Add("MaritalStatusID");
                dt.Columns.Add("Race");
                dt.Columns.Add("Ethnicity");
                dt.Columns.Add("LanguageID");
                dt.Columns.Add("UserLocationID");
                dt.Columns.Add("UserProviderID");
                dt.Columns.Add("Active");
                dt.Columns.Add("PracticeID");
            }

            return dt;
        }

        public static string GetMaritalStatusCode(string maritalStatus)
        {
            string returnVal = string.Empty;
            if (maritalStatus.ToUpper() == "D")
                returnVal = "Divorced";
            else if (maritalStatus.ToUpper() == "M")
                returnVal = "Married";
            else if (maritalStatus.ToUpper() == "P")
                returnVal = "Partner";
            else if (maritalStatus.ToUpper() == "S")
                returnVal = "Single";
            else if (maritalStatus.ToUpper() == "W")
                returnVal = "Widowed";
            else if (maritalStatus.ToUpper() == "L")
                returnVal = "Legally separated";
            return returnVal;
        }

        private static string GetRaceIdFromCode(DataTable dataTable, string raceCode)
        {
            string RaceId = string.Empty;
            DSClinicalSummary ds = new DSClinicalSummary();
            foreach (DataRow row in dataTable.Rows)
            {
                if (MDVUtility.ToStr(row[ds.Race.CodeColumn.ColumnName]) == raceCode)
                {
                    RaceId = MDVUtility.ToStr(row[ds.Race.IdColumn.ColumnName]);
                    break;
                }
            }
            return RaceId;
        }

        private static string GetGender(string gender)
        {
            string Gender = "Other";
            if (gender.ToUpper() == "M")
                Gender = "Male";
            else if (gender.ToUpper() == "F")
                Gender = "Female";
            return Gender;
        }

        private static string GetEthnicityIdFromCode(DataTable dataTable, string ethnicityCode)
        {
            string EthnicityId = string.Empty;
            DSClinicalSummary ds = new DSClinicalSummary();
            foreach (DataRow row in dataTable.Rows)
            {
                if (MDVUtility.ToStr(row[ds.Ethnicity.CodeColumn.ColumnName]) == ethnicityCode)
                {
                    EthnicityId = MDVUtility.ToStr(row[ds.Ethnicity.IdColumn.ColumnName]);
                    break;
                }
            }
            return EthnicityId;
        }

        private static string GetLanguageIdFromCode(DataTable dataTable, string languageCode)
        {
            string LanguageId = string.Empty;
            DSClinicalSummary ds = new DSClinicalSummary();
            foreach (DataRow row in dataTable.Rows)
            {
                if (MDVUtility.ToStr(row[ds.Languages.CodeColumn.ColumnName]) == languageCode)
                {
                    LanguageId = MDVUtility.ToStr(row[ds.Languages.IdColumn.ColumnName]);
                    break;
                }
            }
            return LanguageId;
        }

        //public static string GetRaceIDByCode(string RaceCode)
        //{
        //    BLObject<DSPatientLookups> objPatient = BLLPatientObj.LookupRace();
        //    DSPatientLookups ds = objPatient.Data;
        //    string raceId = "10";
        //    if (ds != null)
        //    {
        //        if (ds.Tables[ds.Race.TableName] != null)
        //        {
        //            foreach (DataRow dr in ds.Tables[ds.Race.TableName].Rows)
        //            {
        //               if(MDVUtility.ToStr(dr[ds.Race.]))
        //            }
        //        }
        //    }
        //    return getJSONofList(list);
        //}

        #endregion


    }
}