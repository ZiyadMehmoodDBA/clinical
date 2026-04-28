using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using Microsoft.Reporting.WebForms;
using MDVision.Model.CCDA;
using MDVision.IEHR.EMR.HL7Folder.Summary.CCDA;

//using static System.String;

namespace MDVision.IEHR.EMR.HL7Folder.Summary.CCDA
{
    public class ImportCCDA
    {
        private BLLPatient _bllPatientObj = null;
        private static BLLCCDA _bllCCDAobj = null;
        public ImportCCDA()
        {
            _bllCCDAobj = new BLLCCDA();
            _bllPatientObj = new BLLPatient();
        }

        #region Singleton
        private static ImportCCDA _obj = null;
        public static ImportCCDA Instance()
        {
            return _obj ?? (_obj = new ImportCCDA());
        }

        #endregion


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
            public static string PatientData = "2.16.840.1.113883.10.20.17.2.4";
            public static string Procedures = "2.16.840.1.113883.10.20.24.3.64";
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
        public DSPatient ReconcileCcda(List<string> xmlContent, List<string> sectionList, List<string> errors, long patientId = 0)
        {
            DSPatient ds = new DSPatient();
            errors = new List<string>();
            try
            {
                foreach (var item in xmlContent)
                {
                    var byteArray = Encoding.UTF8.GetBytes(item);
                    var ccda = GetDeserializeDocument(byteArray, ref errors);
                    if (ccda.Validate("2.16.840.1.113883.10.20.22.1.1"))
                    {
                        GetDoucmentData(ccda, ds, sectionList, errors, patientId);

                    }
                    else
                    {
                        throw new Exception("The selected file is not valid");
                    }
                }
                return ds;
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

        public void GetDoucmentData(MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.ClinicalDocument document,
             DSPatient mainDataSet, List<string> sectionList, List<string> errors, long patientId = 0)
        {

            long providerId = 0;
            string TIN = null;
            string NPI = null;

            if (document.author.Count > 0 && document.author[0].assignedAuthor.id.Count > 0)
            {
                NPI = document.author[0].assignedAuthor.id[0].extension;
            }
            if (document.documentationOf.Count > 0 && document.documentationOf[0].serviceEvent.performer.Count > 0 && document.documentationOf[0].serviceEvent.performer[0].assignedEntity.representedOrganization.id.Count > 0)
            {
                TIN = document.documentationOf[0].serviceEvent.performer[0].assignedEntity.representedOrganization.id[0].extension;
            }


            if (document == null) return;

            #region Patient

            if (patientId == 0)
            {
                var dsPatientLookUp = new BLLClinical().PatientLookup();
                var pr = document.GetPatientRole();
                mainDataSet = new DSPatient();
                var dtPatient = mainDataSet.Tables[mainDataSet.Patients.TableName];
                GetPatientData(pr, ref dtPatient, ref errors, dsPatientLookUp.Data);
                if (errors.IsNullOrEmpty() && dtPatient.HasRows())
                {
                    mainDataSet.Merge(dtPatient);
                }
            }
            else
            {
                var dsPatient = new BLLClinical().FillPatient(patientId);
                if (dsPatient.Data != null)
                {
                    mainDataSet.Merge(dsPatient.Data);
                }
            }

            if (mainDataSet.Patients.Rows.Count > 0)
            {

                mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.RecordCountColumn.ColumnName] = 15;
                var tempFacilityId = mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.FacilityIdColumn.ColumnName];
                var tempProviderId = mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.ProviderIdColumn.ColumnName];
                var tempAccountNumber = mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.AccountNumberColumn.ColumnName];
                var tempDOB = mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.DOBColumn.ColumnName];
                var tempGender = mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.GenderColumn.ColumnName];
                var tempHomeNumber = mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.HomePhoneNoColumn.ColumnName];

                mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.FacilityIdColumn.ColumnName] = DBNull.Value;
                mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.ProviderIdColumn.ColumnName] = DBNull.Value;
                mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.AccountNumberColumn.ColumnName] = DBNull.Value;
                mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.DOBColumn.ColumnName] = DBNull.Value;
                mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.GenderColumn.ColumnName] = DBNull.Value;
                mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.HomePhoneNoColumn.ColumnName] = DBNull.Value;
                mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.ForImportCqmColumn.ColumnName] = 1;

                BLObject<DSPatient> obj = new BLLPatient().LoadPatient(mainDataSet);
                if (obj.Data.Patients.Rows.Count > 0)
                    patientId = obj.Data.Patients[0].PatientId;
                else
                {
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.CreatedByColumn.ColumnName] = "Cypress";
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.CreatedOnColumn.ColumnName] = DateTime.Now;
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.ModifiedByColumn.ColumnName] = "Cypress";
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.ModifiedOnColumn.ColumnName] = DateTime.Now;
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.CommunicatewithGuarantorColumn.ColumnName] = false;
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.CommunicationOptoutColumn.ColumnName] = false;
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.FacilityIdColumn.ColumnName] = tempFacilityId;
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.ProviderIdColumn.ColumnName] = tempProviderId;
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.AccountNumberColumn.ColumnName] = tempAccountNumber;
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.DOBColumn.ColumnName] = tempDOB;
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.GenderColumn.ColumnName] = tempGender;
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.HomePhoneNoColumn.ColumnName] = tempHomeNumber;
                   

                    BLObject<DSPatient> objj = _bllPatientObj.InsertPatient(mainDataSet);
                    mainDataSet.Tables[mainDataSet.Patients.TableName].Rows[0][mainDataSet.Patients.ForImportCqmColumn.ColumnName] = 1;
                    BLObject<DSPatient> obj_ = new BLLPatient().LoadPatient(mainDataSet);
                    if (obj_.Data.Patients.Rows.Count > 0)
                        patientId = obj_.Data.Patients[0].PatientId;
                }
            }
            else
            {
                return;
            }

            #endregion

            #region Encounter

            DSCCDA ds = new DSCCDA();
            DataTable dtEncounter = new DSCCDA().Tables[ds.NotesEncounter.TableName];

            GetEncounterData(document, ref dtEncounter, ref errors, patientId);
            if (dtEncounter.Rows.Count > 0)
            {
                ds.Merge(dtEncounter);
                mainDataSet.Merge(dtEncounter);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateEncounter(ds);
                ds = obj.Data;
            }

            #endregion
            #region PatientInsurance

            string insurancePlanId = GetPatientInsurance(document);
            if (insurancePlanId != null)
            {
                _bllCCDAobj.InsertUpdatePatientInsurance(MDVUtility.ToInt64(insurancePlanId), patientId);

            }

            #endregion


            #region Procedure
            DSCCDA dsp = new DSCCDA();
            DataTable dtProcedures = new DSCCDA().Tables[dsp.NotesProcedures.TableName];
            GetProceduresData(document, ref dtProcedures, ref errors, patientId);
            if (dtProcedures.Rows.Count > 0)
            {
                mainDataSet.Merge(dtProcedures);
                dsp.Merge(dtProcedures);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateProcedure(dsp);
                ds = obj.Data;
            }


            #endregion

            //GetRiskAssessmentData(document, ref errors, patientId);

            //GetPhysicalExamData(document, ref errors, patientId);
            DSCCDA dsD = new DSCCDA();
            List<DiagnosisModel> lstDiagnosis = GetDiagnosisData(document, ref errors, patientId);
            DataTable dtDiagnosis = new DSCCDA().Tables[dsD.Diagnosis.TableName]; // MDVUtility.ConvertListToDataTable(lstDiagnosis[0].Codes);


            if (lstDiagnosis.Count > 0)
            {
                foreach (var item in lstDiagnosis)
                {
                    DataRow dr = dtDiagnosis.NewRow();
                    dr[dsD.Diagnosis.CodeColumn.ColumnName] = item.Codes[0].Code;
                    dr[dsD.Diagnosis.ActionPerformedColumn.ColumnName] = item.Codes[0].ActionPerformed;
                    dr[dsD.Diagnosis.CodetypeColumn.ColumnName] = item.Codes[0].CodeType;
                    dr[dsD.Diagnosis.DescriptionColumn.ColumnName] = item.Codes[0].Description;
                    dr[dsD.Diagnosis.PatientIdColumn.ColumnName] = patientId;
                    dr[dsD.Diagnosis.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                    dr[dsD.Diagnosis.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;
                    dr[dsD.Diagnosis.CreatedByColumn.ColumnName] = "Cypress";
                    dr[dsD.Diagnosis.CreatedOnColumn.ColumnName] = DateTime.Now;
                    dr[dsD.Diagnosis.ModifiedByColumn.ColumnName] = "Cypress";
                    dr[dsD.Diagnosis.ModifiedOnColumn.ColumnName] = DateTime.Now;
                    dr[dsD.Diagnosis.StartDateColumn.ColumnName] = item.Time.low != "" ? MDVUtility.ToDateTime(item.Time.low.ToFormatedDateTimeByFormat()) : dr[dsD.Diagnosis.StartDateColumn.ColumnName] = DBNull.Value;
                    dr[dsD.Diagnosis.EndDateColumn.ColumnName] = item.Time.high != "" ? MDVUtility.ToDateTime(item.Time.high.ToFormatedDateTimeByFormat()) : dr[dsD.Diagnosis.EndDateColumn.ColumnName] = DateTime.Now;
                    dtDiagnosis.Rows.Add(dr);
                }
            }

            if (dtDiagnosis.Rows.Count > 0)
            {
                mainDataSet.Merge(dtDiagnosis);
                dsD.Merge(dtDiagnosis);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateDiagnose(dsD);
                dsD = obj.Data;
            }

            //GetLaboratoryTestsData(document, ref errors, patientId);

            //DataTable dtProcedres = new DSCCDA().Tables[ds.NotesProcedures.TableName];
            List<HistoriesModel> histories = GetRiskAssessmentData(document, ref errors, patientId);

            List<PhysicalExamCQMModel> PEModels = GetPhysicalExamData(document, ref errors, patientId);
            //GetDiagnosisData(document, ref errors, patientId);

            List<LaboratoryTestsModelCQM> models = GetLaboratoryTestsData(document, ref errors, patientId);

            //  Insert Histories
            if (histories != null)
            {
                foreach (HistoriesModel model in histories)
                {
                    _bllCCDAobj.InsertUpdateRiskAssessment(patientId, model.Time.StartDate, model.Time.EndDate, model.StatusCode, model.text, model.Code.CPTCode, model.Code.SNOMEDID, model.Code.CodeType,
                                                 model.Result.ResultCode.Code, model.Result.ResultCode.Description, model.Code.NegationIndex, model.Code.NegationReason, model.NegationValueset, model.Result.ResultCode.PHQScore);
                }
            }

            // Insert Lab Orders
            if (models != null)
            {
                foreach (LaboratoryTestsModelCQM model in models)
                {

                    string cd = model.codes.Count <= 0 ? "" : model.codes[0].Code;
                    string cdesc = model.codes.Count <= 0 ? "" : model.codes[0].Description;

                    _bllCCDAobj.InsertLabData(patientId, MDVUtility.ToDateTime(model.Time.low.ToFormatedDateTimeByFormat()), MDVUtility.ToDateTime(model.Time.high.ToFormatedDateTimeByFormat()), model.Status, model.text, cd, cdesc
                                                , model.ResultValue, model.ResultUnit, "", model.ActionPerformed);
                }
            }

            // Insert Physical Exam

            foreach (PhysicalExamCQMModel model in PEModels)
            {
                string code = model.codes.Count > 0 ? model.codes[0].Code : null;
                string description = model.codes.Count > 0 ? model.codes[0].Description : null;
                string resultCode = model.codes.Count > 0 ? model.codes[0].Code : null;
                string resultDescription = model.codes.Count > 0 ? model.codes[0].Description : null;
                _bllCCDAobj.InsertPEData(patientId, MDVUtility.ToDateTime(model.Time.low.ToFormatedDateTimeByFormat()), MDVUtility.ToDateTime(model.Time.high.ToFormatedDateTimeByFormat()), model.Status, model.text, code, description
                                            , resultCode, resultDescription, model.ResultValue, model.ResultUnit, model.ActionPerformed, model.originalText, model.NegationValueset);
            }


            #region Immunization
            DSCCDA dsi = new DSCCDA();
            DataTable dtImmunization = new DSCCDA().Tables[dsi.Immunization.TableName];
            GetImmunizationData(document, ref dtImmunization, ref errors, patientId);
            if (dtImmunization.Rows.Count > 0)
            {
                mainDataSet.Merge(dtImmunization);
                dsi.Merge(dtImmunization);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateImmunization(dsi);
                dsi = obj.Data;
            }

            DSCCDA dsintolerance = new DSCCDA();
            DataTable dtImmunizationIntolerance = new DSCCDA().Tables[dsintolerance.Immunization.TableName];
            GetImmunizationAllergyandIntoleranceData(document, ref dtImmunizationIntolerance, ref errors, patientId);
            if (dtImmunizationIntolerance.Rows.Count > 0)
            {
                //mainDataSet.Merge(dtImmunizationIntolerance);
                dsintolerance.Merge(dtImmunizationIntolerance);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateImmunization(dsintolerance);
                dsintolerance = obj.Data;
            }
            #endregion

            #region MedicationAdministered
            DSCCDA dsMedicationAdministered = new DSCCDA();
            DataTable dtMedicationAdministered = new DSCCDA().Tables[dsMedicationAdministered.Immunization.TableName];
            GetMedicationAdministeredData(document, ref dtMedicationAdministered, ref errors, patientId);
            if (dtMedicationAdministered.Rows.Count > 0)
            {
                mainDataSet.Merge(dtMedicationAdministered);
                dsMedicationAdministered.Merge(dtMedicationAdministered);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateImmunization(dsMedicationAdministered);
                dsMedicationAdministered = obj.Data;
            }
            #endregion

            #region Intervention

            DSCCDA dsm = new DSCCDA();
            DataTable dtMedication = new DSCCDA().Tables[dsm.Medication.TableName];
            GetMedication(document, ref dtMedication, ref errors, patientId);
            if (dtMedication.Rows.Count > 0)
            {
                mainDataSet.Merge(dtMedication);
                dsm.Merge(dtMedication);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateMedication(dsm);
                dsi = obj.Data;
            }

            DataTable dtIntervention = new DSCCDA().Tables[dsi.Interventions.TableName];
            GetInterventionData(document, ref dtIntervention, ref errors, patientId);
            if (dtIntervention.Rows.Count > 0)
            {
                mainDataSet.Merge(dtIntervention);
                dsi.Merge(dtIntervention);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateIntervention(dsi);
                dsi = obj.Data;
            }

            #endregion

            #region Communication Provide to Provider

            DSCCDA dsProviderToProvider = new DSCCDA();
            DataTable dtProviderToProvider = new DSCCDA().Tables[dsProviderToProvider.ComProviderToProvider.TableName];
            GetComDataForProviderToProvider(document, ref dtProviderToProvider, ref errors, patientId);
            if (dtProviderToProvider.Rows.Count > 0)
            {
                mainDataSet.Merge(dtProviderToProvider);
                dsProviderToProvider.Merge(dtProviderToProvider);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateCommProviderToProvider(dsProviderToProvider);
                dsProviderToProvider = obj.Data;
            }

            #endregion

            #region Communication Patient to Provider

            DSCCDA dsPatientToProvider = new DSCCDA();
            DataTable dtPatientToProvider = new DSCCDA().Tables[dsPatientToProvider.ComPatientToProvider.TableName];
            GetComDataForPatientToProvider(document, ref dtPatientToProvider, ref errors, patientId);
            if (dtPatientToProvider.Rows.Count > 0)
            {
                mainDataSet.Merge(dtPatientToProvider);
                dsPatientToProvider.Merge(dtPatientToProvider);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateCommPatientToProvider(dsPatientToProvider);
                dsPatientToProvider = obj.Data;
            }
            #endregion

            #region Patient Characteristics
            DSCCDA dsPatientCharacteristics = new DSCCDA();
            DataTable dtPatientCharacteristics = new DSCCDA().Tables[dsPatientCharacteristics.PatientCharacteristics.TableName];
            GetPatientCharacteristicsData(document, ref dtPatientCharacteristics, ref errors, patientId);
            if (dtPatientCharacteristics.Rows.Count > 0)
            {
                mainDataSet.Merge(dtPatientCharacteristics);
                dsPatientCharacteristics.Merge(dtPatientCharacteristics);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdatePatientCharacteristics(dsPatientCharacteristics);
                dsPatientCharacteristics = obj.Data;
            }
            #endregion

            #region Diagnostic Study
            DSCCDA dsDiagnosticStudy = new DSCCDA();
            DataTable dtDiagnosticStudy = new DSCCDA().Tables[dsDiagnosticStudy.DiagnosticStudy.TableName];
            GetDiagnosticStudyData(document, ref dtDiagnosticStudy, ref errors, patientId);
            if (dtDiagnosticStudy.Rows.Count > 0)
            {
                mainDataSet.Merge(dtDiagnosticStudy);
                dsDiagnosticStudy.Merge(dtDiagnosticStudy);
                BLObject<DSCCDA> obj = _bllCCDAobj.InsertUpdateDiagnosticStudy(dsDiagnosticStudy);
                dsDiagnosticStudy = obj.Data;
            }
            #endregion



        }

        private static string GetCodeType(string CodeSystem)
        {
            switch (CodeSystem)
            {
                case "2.16.840.1.113883.6.1":
                    return "LOINC";

                case "2.16.840.1.113883.6.12":
                    return "CPT";

                case "2.16.840.1.113883.6.96":
                    return "SNOMED-CT";

                case "2.16.840.1.113883.6.103":
                    return "ICD-9-CM";

                case "2.16.840.1.113883.6.90":
                    return "ICD-10-CM";

                case "2.16.840.1.113883.6.285":
                    return "HCPCS";

                default:
                    return "";
            }
        }
        #region Patient Data Extraction Routine

        private void GetPatientData(PatientRole patientRole, ref DataTable dtPatient, ref List<string> errors, DSClinicalSummary dsPatientLookUp)
        {
            var dsPatient = new DSPatient();
            if (patientRole == null) return;
            var dr = dtPatient.NewRow();

            #region Patient

            var p = patientRole.patient;
            if (p != null)
            {
                #region DOB
                try
                {
                    if (!string.IsNullOrEmpty(p.birthTime.value) && p.birthTime.value.Length >= 8)
                    {
                        if (!string.IsNullOrEmpty(p.birthTime.value))
                        {
                            var dob = p.birthTime.value.Substring(0, 8);
                            dr[dsPatient.Patients.DOBColumn.ColumnName] = dob.ToFormatedDateTimeByFormat();
                        }
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Patient DOB" + exc.Message);
                }

                #endregion
                #region Name
                try
                {
                    if (!p.name.IsNullOrEmpty() && !p.name[0].Items.IsNullOrEmpty())
                    {
                        USRealmName name = GetUsRealmName(p.name[0], ref errors);
                        if (name.LastName.Length < 2 || name.FirstName.Length < 2)
                            return;

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
                #region Race
                try
                {
                    if (!string.IsNullOrEmpty(p.raceCode.code))
                    {

                        dr[dsPatient.Patients.RaceIdColumn.ColumnName] = GetRaceIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Race.TableName], MDVUtility.ToStr(p.raceCode.code));
                        dr[dsPatient.Patients.strRaceIdsColumn.ColumnName] = GetRaceIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Race.TableName], MDVUtility.ToStr(p.raceCode.code));
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Race " + exc.Message);
                }
                #endregion
                #region Gender
                try
                {
                    if (!string.IsNullOrEmpty(p.administrativeGenderCode.code))
                        dr[dsPatient.Patients.GenderColumn.ColumnName] = GetGender(p.administrativeGenderCode.code);
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Gender " + exc.Message);
                }

                #endregion
                #region Address
                try
                {
                    if (!patientRole.addr.IsNullOrEmpty() && patientRole.addr[0] != null
                        && !patientRole.addr[0].Items.IsNullOrEmpty())
                    {
                        List<ADXP> addList = patientRole.addr[0].Items;
                        foreach (ADXP adxp in addList.Where(adxp => adxp != null && !adxp.Text.IsNullOrEmpty() && !string.IsNullOrEmpty(adxp.Text[0])))
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
                                var streetAdd = adxp.Text.Where(str => !string.IsNullOrEmpty(str)).Aggregate("", (current, str) => current + " " + str.Trim());
                                dr[dsPatient.Patients.Address1Column.ColumnName] = streetAdd.ToUpper();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    errors.Add("Patient Address");
                }

                #endregion
                #region Ethnicity
                try
                {
                    if (p.ethnicGroupCode != null)
                    {
                        if (!string.IsNullOrEmpty(p.ethnicGroupCode.code))
                            dr[dsPatient.Patients.EthnicityIdColumn.ColumnName] = GetEthnicityIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Ethnicity.TableName], MDVUtility.ToStr(p.ethnicGroupCode.code));
                        dr[dsPatient.Patients.strEthnicityIdsColumn.ColumnName] = dr[dsPatient.Patients.EthnicityIdColumn.ColumnName].ToString();
                    }
                    else
                        dr[dsPatient.Patients.EthnicityIdColumn.ColumnName] = "4"; // Unknown
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Ethnicity " + exc.Message);
                }
                #endregion
                #region Martial Status
                try
                {
                    if (p.maritalStatusCode != null)
                    {
                        if (!string.IsNullOrEmpty(p.maritalStatusCode.code))
                            dr[dsPatient.Patients.MaritialStatusColumn.ColumnName] = GetMaritalStatusCode(p.maritalStatusCode.code);
                        else
                            dr[dsPatient.Patients.MaritialStatusColumn.ColumnName] = "Unknown";
                    }
                    else
                        dr[dsPatient.Patients.MaritialStatusColumn.ColumnName] = "Unknown";

                }
                catch (Exception exc)
                {
                    errors.Add("Patient Marital Status " + exc.Message);
                }

                #endregion
                #region Language Communication
                try
                {
                    if (!p.languageCommunication.IsNullOrEmpty() && !string.IsNullOrEmpty(p.languageCommunication[0].languageCode.code))
                        dr[dsPatient.Patients.PrefLanguageIdColumn.ColumnName] = GetLanguageIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Languages.TableName], MDVUtility.ToStr(p.languageCommunication[0].languageCode.code));
                    else
                        dr[dsPatient.Patients.PrefLanguageIdColumn.ColumnName] = "146";

                }
                catch (Exception exc)
                {
                    errors.Add("Patient Language" + exc.Message);
                }
                #endregion
                #region TeleCommunication
                try
                {
                    if (!patientRole.telecom.IsNullOrEmpty() && !string.IsNullOrEmpty(patientRole.telecom[0].value))
                    {
                        dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName] = GetTeleCommunication(patientRole.telecom[0].value);
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Language" + exc.Message);
                }
                #endregion
            }

            #endregion

            #region Practice/Provider/Facility

            try
            {
                dr[dsPatient.Patients.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;
                dr[dsPatient.Patients.PracticeIdColumn.ColumnName] = MDVSession.Current.DefaultPracticeId;
                dr[dsPatient.Patients.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                dr[dsPatient.Patients.EntityIdColumn.ColumnName] = MDVSession.Current.EntityId;
                dr[dsPatient.Patients.IsActiveColumn.ColumnName] = true;
            }
            catch (Exception)
            {
                errors.Add("Patient Provider");
            }

            #endregion

            dtPatient.Rows.Add(dr);
        }
        private static USRealmName GetUsRealmName(PN personName, ref List<string> errors)
        {
            var name = new USRealmName();
            if (personName == null) return name;
            foreach (ENXP item in personName.Items.Where(item => item != null && !item.Text.IsNullOrEmpty()
                                                                 && !string.IsNullOrEmpty(item.Text[0])))
            {
                if (item is enfamily)
                {
                    name.LastName = item.Text[0].ToUpper();
                }
                else if (item is engiven)
                {
                    if (string.IsNullOrEmpty(name.FirstName))
                    {
                        name.FirstName = item.Text[0].ToUpper();
                    }
                    else
                    {
                        name.MiddleName = item.Text[0].ToUpper();
                    }
                }
            }

            return name;
        }
        private static string GetGender(string gender)
        {
            var sex = "Other";
            switch (gender.ToUpper())
            {
                case "M":
                    sex = "Male";
                    break;
                case "F":
                    sex = "Female";
                    break;
            }
            return sex;
        }
        public static string GetMaritalStatusCode(string maritalStatus)
        {
            var returnVal = string.Empty;
            switch (maritalStatus.ToUpper())
            {
                case "D":
                    returnVal = "Divorced";
                    break;
                case "M":
                    returnVal = "Married";
                    break;
                case "P":
                    returnVal = "Partner";
                    break;
                case "S":
                    returnVal = "Single";
                    break;
                case "W":
                    returnVal = "Widowed";
                    break;
                case "L":
                    returnVal = "Legally separated";
                    break;
            }
            return returnVal;
        }
        private static string GetRaceIdFromCode(DataTable dataTable, string raceCode)
        {
            string raceId = string.Empty;
            var ds = new DSClinicalSummary();
            foreach (var row in dataTable.Rows.Cast<DataRow>().Where(row => MDVUtility.ToStr(row[ds.Race.CodeColumn.ColumnName]) == raceCode))
            {
                raceId = MDVUtility.ToStr(row[ds.Race.IdColumn.ColumnName]);
                break;
            }
            return raceId;
        }

        private static string GetEthnicityIdFromCode(DataTable dataTable, string ethnicityCode)
        {
            string ethnicityId = string.Empty;
            var ds = new DSClinicalSummary();
            foreach (var row in dataTable.Rows.Cast<DataRow>().Where(row => MDVUtility.ToStr(row[ds.Ethnicity.CodeColumn.ColumnName]) == ethnicityCode))
            {
                ethnicityId = MDVUtility.ToStr(row[ds.Ethnicity.IdColumn.ColumnName]);
                break;
            }
            return ethnicityId;
        }
        private static string GetLanguageIdFromCode(DataTable dataTable, string languageCode)
        {
            if (languageCode == "en")
            {
                languageCode = "eng";
            }
            string languageId = string.Empty;
            var ds = new DSClinicalSummary();
            foreach (var row in dataTable.Rows.Cast<DataRow>().Where(row => MDVUtility.ToStr(row[ds.Languages.CodeColumn.ColumnName]) == languageCode))
            {
                languageId = MDVUtility.ToStr(row[ds.Languages.IdColumn.ColumnName]);
                break;
            }
            return languageId;
        }
        public static string GetTeleCommunication(string teleComm)
        {
            teleComm = teleComm.Trim();
            var straight = teleComm.Replace("tel:+1", "").Replace("tel: 1-", "").Replace("-", "");
            if (straight.Length == 10)
            {
                straight = string.Format("{0:(###) ###-####}", straight); //"{straight:0(###) ###-####}";
            }
            return straight;
        }

        #endregion

        #region Get Clinical Segments


        private static void GetEncounterData(ClinicalDocument document, ref DataTable dtEncounters, ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsEncounters = new DSCCDA();
                Component3 encounterComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref encounterComp, DocumentSections.PatientData);

                List<Entry> entryList = null;

                if (isComponentExist && encounterComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Act)
                        {
                            Act act = e.Item as Act;
                            if (!act.entryRelationship.IsNullOrEmpty())
                            {
                                foreach (EntryRelationship er in act.entryRelationship)
                                {
                                    if (er.Item != null && er.Item is Encounter)
                                    {
                                        Encounter encounter = er.Item as Encounter;
                                        DataRow dr = dtEncounters.NewRow();

                                        dr[dsEncounters.NotesEncounter.PatientIdColumn.ColumnName] = patientId;


                                        if (encounter.code.codeSystem == "2.16.840.1.113883.6.96")
                                        {
                                            dr[dsEncounters.NotesEncounter.CodeColumn.ColumnName] = encounter.code.code;
                                            dr[dsEncounters.NotesEncounter.CodeTypeColumn.ColumnName] = "SNOMED";
                                        }
                                        else if (encounter.code.codeSystem == "2.16.840.1.113883.6.285")
                                        {
                                            dr[dsEncounters.NotesEncounter.CodeColumn.ColumnName] = encounter.code.code;
                                            dr[dsEncounters.NotesEncounter.CodeTypeColumn.ColumnName] = "HCPCS";
                                        }
                                        else if (encounter.code.codeSystem == "2.16.840.1.113883.6.12")
                                        {
                                            dr[dsEncounters.NotesEncounter.CodeColumn.ColumnName] = encounter.code.code;
                                            dr[dsEncounters.NotesEncounter.CodeTypeColumn.ColumnName] = "CPT";
                                        }
                                        else if (encounter.code.codeSystem == "2.16.840.1.113883.6.13")
                                        {
                                            dr[dsEncounters.NotesEncounter.CodeColumn.ColumnName] = encounter.code.code;
                                            dr[dsEncounters.NotesEncounter.CodeTypeColumn.ColumnName] = "CDT";
                                        }

                                        dr[dsEncounters.NotesEncounter.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                                        dr[dsEncounters.NotesEncounter.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;

                                        dr[dsEncounters.NotesEncounter.IsActiveColumn.ColumnName] = 1;
                                        dr[dsEncounters.NotesEncounter.CreatedByColumn.ColumnName] = "Cypress";
                                        dr[dsEncounters.NotesEncounter.CreatedOnColumn.ColumnName] = DateTime.Now;
                                        dr[dsEncounters.NotesEncounter.ModifiedByColumn.ColumnName] = "Cypress";
                                        dr[dsEncounters.NotesEncounter.ModifiedOnColumn.ColumnName] = DateTime.Now;

                                        dr[dsEncounters.NotesEncounter.bMedReconciledColumn.ColumnName] = 0;
                                        dr[dsEncounters.NotesEncounter.MedReconciledIdColumn.ColumnName] = 0;
                                        dr[dsEncounters.NotesEncounter.IsPhoneEncounterColumn.ColumnName] = 0;

                                        dr[dsEncounters.NotesEncounter.EntityIdColumn.ColumnName] = MDVSession.Current.EntityId;
                                        dr[dsEncounters.NotesEncounter.TemplateTypeIdColumn.ColumnName] = 0;
                                        dr[dsEncounters.NotesEncounter.NoteTextColumn.ColumnName] = "";
                                        dr[dsEncounters.NotesEncounter.NotesIdColumn.ColumnName] = i;

                                        if (encounter.entryRelationship.Count > 0)
                                        {
                                            var IsprincipalDisease = ((Summary.CCDA.Observation)encounter.entryRelationship[0].Item).value;
                                            var PrincipalDisease = IsprincipalDisease[0];
                                            var principalDiseaseCode = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(PrincipalDisease)).code;
                                            dr[dsEncounters.NotesEncounter.ActionResultColumn.ColumnName] = principalDiseaseCode;
                                        }


                                        if (encounter.effectiveTime != null)
                                        {
                                            string lowTime, highTime;
                                            GetLowHighTime(encounter.effectiveTime, out lowTime, out highTime);

                                            if (!string.IsNullOrEmpty(lowTime))
                                            {
                                                dr[dsEncounters.NotesEncounter.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                            }
                                            if (!string.IsNullOrEmpty(highTime))
                                            {
                                                dr[dsEncounters.NotesEncounter.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                            }
                                        }
                                        else
                                        {
                                            errors.Add("Encounter --> DOS not found");
                                        }
                                        dtEncounters.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        i--;
                    }


                }
            }
            catch (Exception ex)
            {
                errors.Add("Problems");
            }
        }

        private static string GetPatientInsurance(ClinicalDocument document)
        {
            string InsurancePlanId = null;
            try
            {
                //ClinicalDocument document = new ClinicalDocument();
                Component3 encounterComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref encounterComp, DocumentSections.PatientData);

                List<Entry> entryList = null;

                if (isComponentExist && encounterComp.GetEntry(ref entryList))
                {

                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e.Item != null && e.Item is Observation)
                        {
                            Observation obv = e.Item as Observation;
                            if (obv.value.Count > 0 && obv.value[0] is CD)
                            {
                                var a = (CD)obv.value[0];

                                if (a.codeSystem == "2.16.840.1.113883.3.221.5")
                                {
                                    InsurancePlanId = a.code;
                                }

                            }

                        }
                        i--;
                    }

                    return InsurancePlanId;
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return InsurancePlanId;
        }
        private static List<DiagnosisModel> GetDiagnosisData(ClinicalDocument document, ref List<string> errors, long PatientId)
        {
            try
            {
                List<DiagnosisModel> Diagnosis = new List<DiagnosisModel>();
                Component3 Comp3 = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref Comp3, DocumentSections.PatientData);

                List<Entry> entryList = null;


                if (isComponentExist && Comp3.GetEntry(ref entryList))
                {
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Act)
                        {
                            Act act = e.Item as Act;
                            if (string.IsNullOrEmpty(act.code.nullFlavor))
                            {
                                if (act.code.code == "CONC") // Diagnosis Section
                                {
                                    DiagnosisModel diag = new DiagnosisModel();
                                    foreach (EntryRelationship er in act.entryRelationship)
                                    {
                                        if (er.Item != null && er.Item is Observation)
                                        {
                                            Observation obv = er.Item as Observation;
                                            string lowTime, highTime;
                                            GetLowHighTime(obv.effectiveTime, out lowTime, out highTime);
                                            diag.Time.low = lowTime;
                                            diag.Time.high = highTime;
                                            diag.Status = obv.statusCode.code;
                                            if (obv.value.Count > 0)
                                            {
                                                CD result = (CD)obv.value[0];
                                                if (string.IsNullOrEmpty(result.nullFlavor))
                                                {
                                                    CodeModel code = new CodeModel();
                                                    code.Code = result.code;
                                                    code.CodeType = GetCodeType(result.codeSystem);
                                                    code.Description = result.originalText.Text[0];
                                                    if (obv.targetSiteCode.Count > 0)
                                                    {
                                                        code.ActionPerformed = obv.targetSiteCode[0].code;
                                                    }
                                                    diag.Codes.Add(code);
                                                    if (result.translation.Count > 0)
                                                    {
                                                        foreach (var item in result.translation)
                                                        {
                                                            CodeModel altCode = new CodeModel();
                                                            altCode.Code = item.code;
                                                            altCode.CodeType = GetCodeType(item.codeSystem);
                                                            diag.Codes.Add(altCode);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    Diagnosis.Add(diag);
                                }
                            }
                        }
                    }
                }
                return Diagnosis;
            }
            catch (Exception e)
            {
                errors.Add("Diagnosis");
                return null;
            }
        }
        private static List<LaboratoryTestsModelCQM> GetLaboratoryTestsData(ClinicalDocument document, ref List<string> errors, long patientId)
        {
            try
            {
                List<LaboratoryTestsModelCQM> labTestsList = new List<LaboratoryTestsModelCQM>();
                Component3 Comp3 = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref Comp3, DocumentSections.PatientData);

                List<Entry> entryList = null;

                if (isComponentExist && Comp3.GetEntry(ref entryList))
                {
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation obv = e.Item as Observation;
                            if (obv.text != null && string.IsNullOrEmpty(obv.text.nullFlavor) && (obv.text.Text.Count > 0))
                            {
                                if (obv.text.Text[0].StartsWith("Laboratory Test"))
                                {
                                    LaboratoryTestsModelCQM labTests = new LaboratoryTestsModelCQM();
                                    string lowTime, highTime;
                                    if (obv.negationInd != true)
                                    {
                                        GetLowHighTime(obv.effectiveTime, out lowTime, out highTime);
                                        labTests.Time.low = lowTime;
                                        labTests.Time.high = highTime;

                                    }
                                    // labTests.text = obv.text.Text[0];

                                    if (string.IsNullOrEmpty(obv.code.nullFlavor))
                                    {
                                        CodeModel code = new CodeModel();
                                        code.Code = obv.code.code;
                                        code.CodeType = GetCodeType(obv.code.codeSystem);
                                        code.Description = obv.code.originalText.Text[0];
                                        labTests.codes.Add(code);

                                        if (obv.code.translation.Count > 0)
                                        {
                                            foreach (var item in obv.code.translation)
                                            {
                                                CodeModel altCode = new CodeModel();
                                                altCode.Code = item.code;
                                                altCode.CodeType = GetCodeType(item.codeSystem);
                                                labTests.codes.Add(altCode);
                                            }
                                        }
                                        if (obv.value.Count > 0)
                                        {
                                            if (obv.value[0] is PQ)
                                            {
                                                PQ result = (PQ)obv.value[0];
                                                labTests.ResultValue = result.value;
                                                labTests.ResultUnit = result.unit;
                                            }
                                            else if (obv.value[0] is ST)
                                            {
                                                ST result = (ST)obv.value[0];
                                                labTests.ResultValue = result.Text[0];

                                            }
                                        }
                                    }

                                    if (obv.negationInd == true)
                                    {
                                        if (obv.entryRelationship.Count > 0)
                                        {

                                            var obs1 = (MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.Observation)obv.entryRelationship[0].Item;
                                            var Negation = (MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)obs1.value[0];
                                            labTests.ActionPerformed = Negation.code;
                                            GetLowHighTime(obs1.effectiveTime, out lowTime, out highTime);
                                            labTests.Time.low = lowTime;
                                            labTests.Time.high = highTime == "" ? lowTime : highTime;


                                        }
                                    }
                                    labTestsList.Add(labTests);
                                }
                            }
                        }
                    }
                }
                return labTestsList;
            }
            catch (Exception e)
            {
                errors.Add("Laboratory Tests");
                return null;
            }
        }
        private static List<PhysicalExamCQMModel> GetPhysicalExamData(ClinicalDocument document, ref List<string> errors, long patientId)
        {
            try
            {
                List<PhysicalExamCQMModel> PhyExamList = new List<PhysicalExamCQMModel>();
                Component3 Comp3 = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref Comp3, DocumentSections.PatientData);

                List<Entry> entryList = null;

                if (isComponentExist && Comp3.GetEntry(ref entryList))
                {
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation obv = e.Item as Observation;
                            if (obv.text != null && string.IsNullOrEmpty(obv.text.nullFlavor) && (obv.text.Text.Count > 0))
                            {
                                if (obv.text.Text[0].StartsWith("Physical Exam"))
                                {
                                    PhysicalExamCQMModel phyExam = new PhysicalExamCQMModel();
                                    string lowTime, highTime;
                                    GetLowHighTime(obv.effectiveTime, out lowTime, out highTime);
                                    phyExam.Time.low = lowTime;
                                    phyExam.Time.high = highTime;
                                    phyExam.text = obv.text.Text[0];
                                    phyExam.originalText = obv.text.Text[0];

                                    if (string.IsNullOrEmpty(obv.code.nullFlavor))
                                    {
                                        CodeModel code = new CodeModel();
                                        code.Code = obv.code.code;
                                        code.CodeType = GetCodeType(obv.code.codeSystem);
                                        code.Description = obv.code.originalText.Text[0];
                                        phyExam.codes.Add(code);
                                        if (obv.code.translation.Count > 0)
                                        {
                                            foreach (var item in obv.code.translation)
                                            {
                                                CodeModel altCode = new CodeModel();
                                                altCode.Code = item.code;
                                                altCode.CodeType = GetCodeType(item.codeSystem);
                                                phyExam.codes.Add(altCode);
                                            }
                                        }
                                        if (obv.value.Count > 0)
                                        {
                                            if (obv.value[0] is PQ)
                                            {
                                                PQ result = (PQ)obv.value[0];
                                                phyExam.ResultValue = result.value;
                                                phyExam.ResultUnit = result.unit;
                                            }
                                        }
                                    }
                                    if (obv.code.nullFlavor == "NA")
                                    {
                                        phyExam.NegationValueset = obv.code.valueSet;
                                        var NegationNode = ((Summary.CCDA.Observation)obv.entryRelationship[0].Item).value;
                                        var Negation = NegationNode[0];
                                        var NegationCode = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(Negation)).code;
                                        phyExam.ActionPerformed = NegationCode;
                                    }
                                    PhyExamList.Add(phyExam);
                                }
                            }
                        }
                    }
                }
                return PhyExamList;
            }
            catch (Exception e)
            {
                errors.Add("PhysicalExam");
                return null;
            }
        }
        private static List<HistoriesModel> GetRiskAssessmentData(ClinicalDocument document, ref List<string> errors, long patientId)
        {
            try
            {
                List<HistoriesModel> historiesList = new List<HistoriesModel>();
                Component3 Comp3 = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref Comp3, DocumentSections.PatientData);

                List<Entry> entryList = null;

                if (isComponentExist && Comp3.GetEntry(ref entryList))
                {
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation obv = e.Item as Observation;
                            if (obv.text != null && string.IsNullOrEmpty(obv.text.nullFlavor) && (obv.text.Text.Count > 0))
                            {
                                if (obv.text.Text[0].StartsWith("Risk Category Assessment:"))
                                {
                                    HistoriesModel histories = new HistoriesModel();


                                    if (obv.negationInd == true)
                                    {
                                        histories.NegationValueset = obv.code.valueSet;
                                        var t = ((Summary.CCDA.Observation)obv.entryRelationship[0].Item).value;
                                        var s = t[0];
                                        var NegationCode = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(s)).code;

                                        histories.Code.NegationIndex = true;
                                        histories.Code.NegationReason = NegationCode;

                                        histories.Code.CodeType = GetCodeType(((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(s)).codeSystem);

                                        if (histories.Code.CodeType == "SNOMED-CT")
                                        {
                                            histories.Code.SNOMEDID = NegationCode;
                                        }
                                        else
                                        {
                                            histories.Code.CPTCode = NegationCode;
                                        }
                                    }
                                    else
                                    {
                                        histories.Code.CodeType = GetCodeType(obv.code.codeSystem);

                                        if (histories.Code.CodeType == "SNOMED-CT")
                                        {
                                            histories.Code.SNOMEDID = obv.code.code;
                                        }
                                        else
                                        {
                                            histories.Code.CPTCode = obv.code.code;
                                        }
                                    }

                                    //if (obv.code.originalText.Text.Count > 0)
                                    //{
                                    //    histories.Code.Description = obv.code.originalText.Text[0];
                                    //}
                                    string lowTime, highTime;
                                    GetLowHighTime(obv.effectiveTime, out lowTime, out highTime);
                                    histories.Time.StartDate = MDVUtility.ToDateTime(lowTime.ToFormatedDateTimeByFormat());
                                    histories.Time.EndDate = MDVUtility.ToDateTime(highTime.ToFormatedDateTimeByFormat());
                                    histories.StatusCode = obv.statusCode.code;
                                    if (obv.value.Count > 0)
                                    {
                                        if (obv.value[0] is CD)
                                        {
                                            CD result = (CD)obv.value[0];
                                            if (string.IsNullOrEmpty(result.nullFlavor))
                                            {
                                                histories.Result.ResultCode.Code = result.code;
                                                histories.Result.ResultCode.CodeType = GetCodeType(result.codeSystem);
                                                histories.Result.ResultCode.Description = result.originalText.Text[0];
                                            }
                                        }
                                        else if (obv.value[0] is PQ)
                                        {
                                            PQ result = (PQ)obv.value[0];
                                            if (string.IsNullOrEmpty(result.nullFlavor))
                                            {
                                                histories.Result.ResultCode.PHQScore = result.value;
                                               
                                            }
                                        }
                                    }
                                    historiesList.Add(histories);
                                }
                            }
                        }
                    }
                }
                return historiesList;
            }
            catch (Exception e)
            {
                errors.Add("Risk Category Assessments");
                return null;
            }
        }
        private static void GetProceduresData(ClinicalDocument document, ref DataTable dtProcedures, ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsEncounters = new DSCCDA();
                Component3 encounterComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref encounterComp, DocumentSections.PatientData);

                List<Entry> entryList = null;
                if (isComponentExist && encounterComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e.Item is Procedure)
                        {

                            Procedure procedure = e.Item as Procedure;
                            DataRow dr = dtProcedures.NewRow();


                            dr[dsEncounters.NotesProcedures.ProcedureIdColumn.ColumnName] = i;
                            dr[dsEncounters.NotesProcedures.PatientIdColumn.ColumnName] = patientId;

                            if (procedure.negationInd == true)
                            {
                                dr[dsEncounters.NotesProcedures.NegationValuesetColumn.ColumnName] = procedure.code.valueSet;
                                var t = ((Summary.CCDA.Observation)procedure.entryRelationship[0].Item).value;
                                var s = t[0];
                                var NegationCode = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(s)).code;
                                dr[dsEncounters.NotesProcedures.NegationReasonColumn.ColumnName] = NegationCode;
                                dr[dsEncounters.NotesProcedures.NegationIndexColumn.ColumnName] = 1;

                                if (GetCodeType(((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(s)).codeSystem) == "2.16.840.1.113883.6.96")
                                {
                                    dr[dsEncounters.NotesProcedures.SNOMEDIDColumn.ColumnName] = NegationCode;
                                    dr[dsEncounters.NotesProcedures.CodeTypeColumn.ColumnName] = "SNOMED";
                                }
                                else
                                {
                                    dr[dsEncounters.NotesProcedures.CPTCodeColumn.ColumnName] = NegationCode;
                                    dr[dsEncounters.NotesProcedures.CodeTypeColumn.ColumnName] = "CPT";
                                }
                            }
                            else
                            {
                                if (procedure.code.codeSystem == "2.16.840.1.113883.6.96")
                                {
                                    dr[dsEncounters.NotesProcedures.SNOMEDIDColumn.ColumnName] = procedure.code.code;
                                    dr[dsEncounters.NotesProcedures.CodeTypeColumn.ColumnName] = "SNOMED";
                                }
                                else
                                {
                                    dr[dsEncounters.NotesProcedures.CPTCodeColumn.ColumnName] = procedure.code.code;
                                    dr[dsEncounters.NotesProcedures.CodeTypeColumn.ColumnName] = "CPT";
                                }
                            }

                            dr[dsEncounters.NotesProcedures.IsActiveColumn.ColumnName] = 1;
                            dr[dsEncounters.NotesProcedures.CreatedByColumn.ColumnName] = "Cypress";
                            dr[dsEncounters.NotesProcedures.CreatedOnColumn.ColumnName] = DateTime.Now;
                            dr[dsEncounters.NotesProcedures.ModifiedByColumn.ColumnName] = "Cypress";
                            dr[dsEncounters.NotesProcedures.ModifiedOnColumn.ColumnName] = DateTime.Now;
                            dr[dsEncounters.NotesProcedures.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                            dr[dsEncounters.NotesProcedures.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;

                            if (procedure.effectiveTime != null)
                            {
                                string lowTime, highTime;
                                GetLowHighTime(procedure.effectiveTime, out lowTime, out highTime);

                                if (!string.IsNullOrEmpty(lowTime))
                                {
                                    dr[dsEncounters.NotesProcedures.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                }
                                if (!string.IsNullOrEmpty(highTime))
                                {
                                    dr[dsEncounters.NotesProcedures.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                }
                            }
                            else
                            {
                                errors.Add("Procedure --> DOS not found");
                            }
                            dtProcedures.Rows.Add(dr);
                        }

                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Problems");
            }
        }
        private static void GetImmunizationData(ClinicalDocument document, ref DataTable dtImmunization, ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsImmunization = new DSCCDA();
                Component3 immunizationComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref immunizationComp, DocumentSections.PatientData);

                List<Entry> entryList = null;
                if (isComponentExist && immunizationComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Act)
                        {
                            Act act = e.Item as Act;
                            if (!act.entryRelationship.IsNullOrEmpty())
                            {
                                foreach (EntryRelationship er in act.entryRelationship)
                                {
                                    if (er.Item != null && er.Item is SubstanceAdministration)
                                    {
                                        SubstanceAdministration substanceAdministration = (SubstanceAdministration)er.Item;
                                        DataRow dr = dtImmunization.NewRow();

                                        dr[dsImmunization.Immunization.VaccineHxIdColumn.ColumnName] = i;
                                        dr[dsImmunization.Immunization.PatientIdColumn.ColumnName] = patientId;


                                        if (!substanceAdministration.effectiveTime.IsNullOrEmpty() && substanceAdministration.effectiveTime[0] is IVL_TS)
                                        {
                                            string lowTime, highTime;
                                            GetLowHighTime((IVL_TS)substanceAdministration.effectiveTime[0], out lowTime, out highTime);

                                            if (!string.IsNullOrEmpty(lowTime))
                                            {
                                                dr[dsImmunization.Immunization.AdministrationDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                            }
                                        }
                                        else
                                        {
                                            errors.Add("Immunizatoin --> Date Administred not found");
                                        }

                                        var consumable = substanceAdministration.consumable;
                                        var mp = consumable.manufacturedProduct;


                                        dr[dsImmunization.Immunization.VaccineGroupCategoryColumn.ColumnName] = 0;
                                        if (act.negationInd == true)
                                        {
                                            dr[dsImmunization.Immunization.NegationValuesetColumn.ColumnName] = ((Material)mp.Item).code.valueSet;
                                            var t = ((Summary.CCDA.Observation)act.entryRelationship[1].Item).value;
                                            var s = t[0];
                                            var NegationCode = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(s)).code;

                                            dr[dsImmunization.Immunization.NegationIndexColumn.ColumnName] = 1;
                                            dr[dsImmunization.Immunization.NegationReasonColumn.ColumnName] = NegationCode;
                                        }
                                        else
                                        {
                                            dr[dsImmunization.Immunization.NegationIndexColumn.ColumnName] = 0;
                                        }
                                        dr[dsImmunization.Immunization.VaccineColumn.ColumnName] = ((Material)mp.Item).code.code;


                                        // dr[dsImmunization.Immunization.LotNumberColumn.ColumnName] = ((Material)mp.Item).lotNumberText.Text[0];
                                        // dr[dsImmunization.Immunization.RouteColumn.ColumnName] = "";
                                        dr[dsImmunization.Immunization.TypeColumn.ColumnName] = "";
                                        dr[dsImmunization.Immunization.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                                        dr[dsImmunization.Immunization.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;
                                        //  dr[dsImmunization.Immunization.SchedulerIdColumn.ColumnName] = 0;
                                        // dr[dsImmunization.Immunization.PatientAgeColumn.ColumnName] = "";
                                        //  dr[dsImmunization.Immunization.GivenByColumn.ColumnName] = "";

                                        dr[dsImmunization.Immunization.IsActiveColumn.ColumnName] = 1;
                                        dr[dsImmunization.Immunization.CreatedByColumn.ColumnName] = "Cypress";
                                        dr[dsImmunization.Immunization.CreatedOnColumn.ColumnName] = DateTime.Now;
                                        dr[dsImmunization.Immunization.ModifiedByColumn.ColumnName] = "Cypress";
                                        dr[dsImmunization.Immunization.ModifiedOnColumn.ColumnName] = DateTime.Now;

                                        dtImmunization.Rows.Add(dr);

                                    }
                                }
                            }
                        }
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Problems");
            }
        }

        private static void GetImmunizationAllergyandIntoleranceData(ClinicalDocument document, ref DataTable dtImmunization, ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsImmunization = new DSCCDA();
                Component3 immunizationComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref immunizationComp, DocumentSections.PatientData);

                List<Entry> entryList = null;
                if (isComponentExist && immunizationComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation obs = e.Item as Observation;
                            if (obs.participant.Count > 0)
                            {

                                var a = obs.participant[0].participantRole;
                                DataRow dr = dtImmunization.NewRow();
                                dr[dsImmunization.Immunization.VaccineHxIdColumn.ColumnName] = -1;
                                dr[dsImmunization.Immunization.PatientIdColumn.ColumnName] = patientId;
                                dr[dsImmunization.Immunization.VaccineColumn.ColumnName] = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.PlayingEntity)a.Item).code.code;
                                dr[dsImmunization.Immunization.TypeColumn.ColumnName] = "";
                                dr[dsImmunization.Immunization.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                                dr[dsImmunization.Immunization.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;
                                dr[dsImmunization.Immunization.IsActiveColumn.ColumnName] = 1;
                                dr[dsImmunization.Immunization.CreatedByColumn.ColumnName] = "Cypress";
                                dr[dsImmunization.Immunization.CreatedOnColumn.ColumnName] = DateTime.Now;
                                dr[dsImmunization.Immunization.ModifiedByColumn.ColumnName] = "Cypress";
                                dr[dsImmunization.Immunization.ModifiedOnColumn.ColumnName] = DateTime.Now;
                                if (((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.PlayingEntity)a.Item).code.originalText.Text.Count > 0)
                                {
                                    dr[dsImmunization.Immunization.CommentsColumn.ColumnName] = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.PlayingEntity)a.Item).code.originalText.Text[0];
                                }

                                if (obs.effectiveTime != null)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(obs.effectiveTime, out lowTime, out highTime);

                                    if (!string.IsNullOrEmpty(lowTime))
                                    {
                                        dr[dsImmunization.Immunization.AdministrationDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }

                                }
                                var type = "Administer";

                                if (obs.value.Count > 0)
                                {
                                    type = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)obs.value[0]).displayName;
                                    dr[dsImmunization.Immunization.TypeColumn.ColumnName] = type;
                                }
                               

                                dtImmunization.Rows.Add(dr);

                            }
                        }

                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Problems");
            }
        }

        private static void GetMedicationAdministeredData(ClinicalDocument document, ref DataTable dtImmunization, ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsImmunization = new DSCCDA();
                Component3 immunizationComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref immunizationComp, DocumentSections.PatientData);

                List<Entry> entryList = null;
                if (isComponentExist && immunizationComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Act)
                        {
                            Act act = e.Item as Act;
                            if (!act.entryRelationship.IsNullOrEmpty())
                            {
                                foreach (EntryRelationship er in act.entryRelationship)
                                {
                                    if (er.Item != null && er.Item is SubstanceAdministration)
                                    {
                                        SubstanceAdministration substanceAdministration = (SubstanceAdministration)er.Item;
                                        var isMedication = substanceAdministration.text.Text[0].Split(':')[0];
                                        if (isMedication == "Medication, Administered")
                                        {

                                            DataRow dr = dtImmunization.NewRow();

                                            dr[dsImmunization.Immunization.VaccineHxIdColumn.ColumnName] = i;
                                            dr[dsImmunization.Immunization.PatientIdColumn.ColumnName] = patientId;


                                            if (!substanceAdministration.effectiveTime.IsNullOrEmpty() && substanceAdministration.effectiveTime[0] is IVL_TS)
                                            {
                                                string lowTime, highTime;
                                                GetLowHighTime((IVL_TS)substanceAdministration.effectiveTime[0], out lowTime, out highTime);

                                                if (!string.IsNullOrEmpty(lowTime))
                                                {
                                                    dr[dsImmunization.Immunization.AdministrationDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                                }
                                            }
                                            else
                                            {
                                                errors.Add("Immunizatoin --> Date Administred not found");
                                            }
                                            var consumable = substanceAdministration.consumable;
                                            var mp = consumable.manufacturedProduct;
                                            dr[dsImmunization.Immunization.VaccineGroupCategoryColumn.ColumnName] = 0;
                                            if (act.negationInd == true)
                                            {
                                                var t = ((Summary.CCDA.Observation)act.entryRelationship[1].Item).value;
                                                var s = t[0];
                                                var NegationCode = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(s)).code;

                                                dr[dsImmunization.Immunization.NegationIndexColumn.ColumnName] = 1;
                                                dr[dsImmunization.Immunization.NegationReasonColumn.ColumnName] = NegationCode;
                                            }
                                            else
                                            {
                                                dr[dsImmunization.Immunization.NegationIndexColumn.ColumnName] = 0;
                                            }
                                            dr[dsImmunization.Immunization.VaccineColumn.ColumnName] = ((Material)mp.Item).code.code;
                                            dr[dsImmunization.Immunization.TypeColumn.ColumnName] = "";
                                            dr[dsImmunization.Immunization.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                                            dr[dsImmunization.Immunization.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;
                                            dr[dsImmunization.Immunization.IsActiveColumn.ColumnName] = 1;
                                            dr[dsImmunization.Immunization.CreatedByColumn.ColumnName] = "Cypress";
                                            dr[dsImmunization.Immunization.CreatedOnColumn.ColumnName] = DateTime.Now;
                                            dr[dsImmunization.Immunization.ModifiedByColumn.ColumnName] = "Cypress";
                                            dr[dsImmunization.Immunization.ModifiedOnColumn.ColumnName] = DateTime.Now;

                                            dtImmunization.Rows.Add(dr);

                                        }
                                    }
                                }
                            }
                        }
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Problems");
            }
        }

        private static void GetInterventionData(ClinicalDocument document, ref DataTable dtIntervention,
            ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsIntervention = new DSCCDA();
                Component3 interventionComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref interventionComp, DocumentSections.PatientData);

                List<Entry> entryList = null;
                if (isComponentExist && interventionComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Act)
                        {
                            Act act = e.Item as Act;
                            if (act.text != null && act.text.Text[0].StartsWith("Intervention"))
                            {
                                DataRow dr = dtIntervention.NewRow();
                                //  dr[dsIntervention.Interventions.SNOMEDCodeColumn.ColumnName] = act.code.code;
                                // dr[dsIntervention.Interventions.SNOMEDDescriptionColumn.ColumnName] = act.code.originalText;
                                dr[dsIntervention.Interventions.DescriptionColumn.ColumnName] = act.text.Text[0];
                                if (act.statusCode.code == "completed")
                                {
                                    dr[dsIntervention.Interventions.StatusColumn.ColumnName] = "performed";
                                }
                                else if (act.statusCode.code == "active")
                                {
                                    dr[dsIntervention.Interventions.StatusColumn.ColumnName] = "ordered";
                                }

                                if (act.code != null && act.negationInd != true)
                                {


                                    string codetype = GetCodeType(act.code.codeSystem);

                                    if (codetype == "SNOMED-CT")
                                    {
                                        dr[dsIntervention.Interventions.SNOMEDIDColumn.ColumnName] = act.code.code;
                                        dr[dsIntervention.Interventions.CodeSystemColumn.ColumnName] = codetype;
                                    }
                                    else
                                    {
                                        dr[dsIntervention.Interventions.CPTCodeColumn.ColumnName] = act.code.code;
                                        dr[dsIntervention.Interventions.CodeSystemColumn.ColumnName] = codetype;
                                    }

                                }

                                if (act.negationInd == true)
                                {
                                    dr[dsIntervention.Interventions.NegationValuesetColumn.ColumnName] = act.code.valueSet;

                                    if (act.entryRelationship.Count > 0)
                                    {

                                        var obs = (MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.Observation)act.entryRelationship[0].Item;

                                        string codetype1 = GetCodeType(obs.code.codeSystem);



                                        if (codetype1 == "SNOMED-CT")
                                        {
                                            dr[dsIntervention.Interventions.SNOMEDIDColumn.ColumnName] = obs.code.code;
                                        }
                                        else
                                        {
                                            dr[dsIntervention.Interventions.CPTCodeColumn.ColumnName] = obs.code.code;
                                        }
                                        var NegationVal = (MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)obs.value[0];
                                        string NegationCodeSystem = GetCodeType(NegationVal.codeSystem);
                                        dr[dsIntervention.Interventions.NegationCodeColumn.ColumnName] = NegationVal.code;
                                        dr[dsIntervention.Interventions.NegationIndexColumn.ColumnName] = 1;
                                        dr[dsIntervention.Interventions.NegationCodeSystemColumn.ColumnName] = NegationCodeSystem;
                                    }
                                }

                                if (act.entryRelationship.Count > 0)
                                {
                                    var obs = (MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.Observation)act.entryRelationship[0].Item;
                                    var Negation = (MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)obs.value[0];
                                    dr[dsIntervention.Interventions.ActionResultColumn.ColumnName] = Negation.code;

                                }

                                //dr[dsIntervention.Interventions.ExtensionColumn.ColumnName] = act.id[0].extension;
                                dr[dsIntervention.Interventions.IsActiveColumn.ColumnName] = 1;
                                dr[dsIntervention.Interventions.CreatedByColumn.ColumnName] = "Cypress";
                                dr[dsIntervention.Interventions.CreatedOnColumn.ColumnName] = DateTime.Now;
                                dr[dsIntervention.Interventions.ModifiedByColumn.ColumnName] = "Cypress";
                                dr[dsIntervention.Interventions.ModifiedOnColumn.ColumnName] = DateTime.Now;
                                dr[dsIntervention.Interventions.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                                dr[dsIntervention.Interventions.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;
                                dr[dsIntervention.Interventions.PatientIdColumn.ColumnName] = patientId;
                                if (act.effectiveTime != null)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(act.effectiveTime, out lowTime, out highTime);
                                    if (!string.IsNullOrEmpty(lowTime))
                                    {
                                        dr[dsIntervention.Interventions.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrEmpty(highTime))
                                    {
                                        dr[dsIntervention.Interventions.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else
                                {
                                    errors.Add("Intervention --> DOS not found");
                                }

                                dtIntervention.Rows.Add(dr);

                            }
                        }
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Problems");
            }
        }


        private static void GetComDataForProviderToProvider(ClinicalDocument document, ref DataTable dtProviderToProvider,
           ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsProviderToProvider = new DSCCDA();
                Component3 ProviderToProviderComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref ProviderToProviderComp, DocumentSections.PatientData);

                List<Entry> entryList = null;
                if (isComponentExist && ProviderToProviderComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Act)
                        {
                            Act act = e.Item as Act;
                            if (act.text != null && act.text.Text[0].StartsWith("Communication From Provider"))
                            {
                                DataRow dr = dtProviderToProvider.NewRow();

                                dr[dsProviderToProvider.ComProviderToProvider.DescriptionColumn.ColumnName] = act.text.Text[0];
                                dr[dsProviderToProvider.ComProviderToProvider.CodeColumn.ColumnName] = act.code.code;
                                string codesystem = GetCodeType(act.code.codeSystem);
                                if (codesystem == "2.16.840.1.113883.6.96")
                                {
                                    dr[dsProviderToProvider.ComProviderToProvider.SNOMEDIDColumn.ColumnName] = act.code.code;
                                }
                                else
                                {
                                    dr[dsProviderToProvider.ComProviderToProvider.CPTCodeColumn.ColumnName] = act.code.code;
                                }
                                dr[dsProviderToProvider.ComProviderToProvider.CodeSystemColumn.ColumnName] = codesystem;
                                dr[dsProviderToProvider.ComProviderToProvider.DirectionColumn.ColumnName] = "communication_from_provider_to_provider";
                                dr[dsProviderToProvider.ComProviderToProvider.IsActiveColumn.ColumnName] = 1;
                                dr[dsProviderToProvider.ComProviderToProvider.CreatedByColumn.ColumnName] = "Cypress";
                                dr[dsProviderToProvider.ComProviderToProvider.CreatedOnColumn.ColumnName] = DateTime.Now;
                                dr[dsProviderToProvider.ComProviderToProvider.ModifiedByColumn.ColumnName] = "Cypress";
                                dr[dsProviderToProvider.ComProviderToProvider.ModifiedOnColumn.ColumnName] = DateTime.Now;
                                dr[dsProviderToProvider.ComProviderToProvider.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                                dr[dsProviderToProvider.ComProviderToProvider.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;
                                dr[dsProviderToProvider.ComProviderToProvider.PatientIdColumn.ColumnName] = patientId;
                                if (act.effectiveTime != null)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(act.effectiveTime, out lowTime, out highTime);

                                    if (!string.IsNullOrEmpty(lowTime))
                                    {
                                        dr[dsProviderToProvider.ComProviderToProvider.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrEmpty(highTime))
                                    {
                                        dr[dsProviderToProvider.ComProviderToProvider.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else
                                {
                                    errors.Add("Communication Provider To Provider --> DOS not found");
                                }

                                dtProviderToProvider.Rows.Add(dr);

                            }
                        }
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Problems");
            }
        }

        private static void GetComDataForPatientToProvider(ClinicalDocument document, ref DataTable dtPatientToProvider,
           ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsPatientToProvider = new DSCCDA();
                Component3 PatientToProviderComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref PatientToProviderComp, DocumentSections.PatientData);

                List<Entry> entryList = null;
                if (isComponentExist && PatientToProviderComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Act)
                        {
                            Act act = e.Item as Act;
                            if (act.text != null && act.text.Text[0].StartsWith("Communication From Patient to Provider"))
                            {
                                DataRow dr = dtPatientToProvider.NewRow();

                                dr[dsPatientToProvider.ComPatientToProvider.DescriptionColumn.ColumnName] = act.text.Text[0];
                                dr[dsPatientToProvider.ComPatientToProvider.CodeColumn.ColumnName] = act.code.code;
                                string codesystem = GetCodeType(act.code.codeSystem);
                                if (act.code.codeSystem == "2.16.840.1.113883.6.96")
                                {
                                    dr[dsPatientToProvider.ComPatientToProvider.SNOMEDIDColumn.ColumnName] = act.code.code;
                                    //dr[dsPatientToProvider.ComPatientToProvider.CodeTypeColumn.ColumnName] = "SNOMED";
                                }
                                else
                                {
                                    dr[dsPatientToProvider.ComPatientToProvider.CPTCodeColumn.ColumnName] = act.code.code;
                                    //dr[dsPatientToProvider.ComPatientToProvider.CodeTypeColumn.ColumnName] = "CPT";
                                }
                                dr[dsPatientToProvider.ComPatientToProvider.CodeSystemColumn.ColumnName] = codesystem;
                                dr[dsPatientToProvider.ComPatientToProvider.IsActiveColumn.ColumnName] = 1;
                                dr[dsPatientToProvider.ComPatientToProvider.CreatedByColumn.ColumnName] = "Cypress";
                                dr[dsPatientToProvider.ComPatientToProvider.CreatedOnColumn.ColumnName] = DateTime.Now;
                                dr[dsPatientToProvider.ComPatientToProvider.ModifiedByColumn.ColumnName] = "Cypress";
                                dr[dsPatientToProvider.ComPatientToProvider.ModifiedOnColumn.ColumnName] = DateTime.Now;
                                dr[dsPatientToProvider.ComPatientToProvider.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                                dr[dsPatientToProvider.ComPatientToProvider.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;
                                dr[dsPatientToProvider.ComPatientToProvider.PatientIdColumn.ColumnName] = patientId;


                                dr[dsPatientToProvider.ComPatientToProvider.CodeSystemColumn.ColumnName] = codesystem;

                                if (act.effectiveTime != null)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(act.effectiveTime, out lowTime, out highTime);

                                    if (!string.IsNullOrEmpty(lowTime))
                                    {
                                        dr[dsPatientToProvider.ComPatientToProvider.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrEmpty(highTime))
                                    {
                                        dr[dsPatientToProvider.ComPatientToProvider.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else
                                {
                                    errors.Add("Communication Patient To Provider --> DOS not found");
                                }

                                dtPatientToProvider.Rows.Add(dr);

                            }
                        }
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Communication Patient To Provider");
            }
        }

        private static void GetPatientCharacteristicsData(ClinicalDocument document, ref DataTable dtPatientCharacteristics,
          ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsPatientCharacteristics = new DSCCDA();
                Component3 PatientCharacteristicsComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref PatientCharacteristicsComp, DocumentSections.PatientData);

                List<Entry> entryList = null;
                if (isComponentExist && PatientCharacteristicsComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation obs = e.Item as Observation;
                            if (obs.value.Count > 0 && obs.value[0] is CD)
                            {
                                var objVal = (MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)obs.value[0];
                                if (objVal.originalText != null && objVal.originalText.Text.Count > 0 && objVal.originalText.Text[0].StartsWith("Patient Characteristic"))
                                {
                                    DataRow dr = dtPatientCharacteristics.NewRow();

                                    dr[dsPatientCharacteristics.PatientCharacteristics.DescriptionColumn.ColumnName] = objVal.originalText.Text[0];
                                    dr[dsPatientCharacteristics.PatientCharacteristics.CodeColumn.ColumnName] = objVal.code;
                                    string codesystem = GetCodeType(objVal.codeSystem);
                                    dr[dsPatientCharacteristics.PatientCharacteristics.CodeSystemColumn.ColumnName] = codesystem;
                                    dr[dsPatientCharacteristics.PatientCharacteristics.IsActiveColumn.ColumnName] = 1;
                                    dr[dsPatientCharacteristics.PatientCharacteristics.CreatedByColumn.ColumnName] = "Cypress";
                                    dr[dsPatientCharacteristics.PatientCharacteristics.CreatedOnColumn.ColumnName] = DateTime.Now;
                                    dr[dsPatientCharacteristics.PatientCharacteristics.ModifiedByColumn.ColumnName] = "Cypress";
                                    dr[dsPatientCharacteristics.PatientCharacteristics.ModifiedOnColumn.ColumnName] = DateTime.Now;
                                    dr[dsPatientCharacteristics.PatientCharacteristics.PatientIdColumn.ColumnName] = patientId;
                                    dr[dsPatientCharacteristics.PatientCharacteristics.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                                    dr[dsPatientCharacteristics.PatientCharacteristics.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;

                                    if (obs.effectiveTime != null)
                                    {
                                        string lowTime, highTime;
                                        GetLowHighTime(obs.effectiveTime, out lowTime, out highTime);

                                        if (!string.IsNullOrEmpty(lowTime))
                                        {
                                            dr[dsPatientCharacteristics.PatientCharacteristics.TimeColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                        }
                                        if (!string.IsNullOrEmpty(lowTime))
                                        {
                                            dr[dsPatientCharacteristics.PatientCharacteristics.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                        }
                                        if (!string.IsNullOrEmpty(highTime))
                                        {
                                            dr[dsPatientCharacteristics.PatientCharacteristics.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                        }
                                    }
                                    else
                                    {
                                        errors.Add("Patient Characteristics --> DOS not found");
                                    }

                                    dtPatientCharacteristics.Rows.Add(dr);

                                }
                            }
                        }
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Problems");
            }
        }

        private static void GetDiagnosticStudyData(ClinicalDocument document, ref DataTable dtDiagnosticStudy,
         ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsDiagnosticStudy = new DSCCDA();
                Component3 DiagnosticStudyComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref DiagnosticStudyComp, DocumentSections.PatientData);

                List<Entry> entryList = null;
                if (isComponentExist && DiagnosticStudyComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation obs = e.Item as Observation;
                            //var objVal = (MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)obs.value[0];
                            if (obs.text != null && obs.text.Text.Count > 0 && obs.text.Text[0].StartsWith("Diagnostic Study"))
                            {
                                DataRow dr = dtDiagnosticStudy.NewRow();

                                dr[dsDiagnosticStudy.DiagnosticStudy.DescriptionColumn.ColumnName] = obs.text.Text[0];
                                dr[dsDiagnosticStudy.DiagnosticStudy.LOINCColumn.ColumnName] = obs.code.code;
                                string codesystem = GetCodeType(obs.code.codeSystem);
                                dr[dsDiagnosticStudy.DiagnosticStudy.CodeSystemColumn.ColumnName] = codesystem;
                                dr[dsDiagnosticStudy.DiagnosticStudy.CreatedByColumn.ColumnName] = "Cypress";
                                dr[dsDiagnosticStudy.DiagnosticStudy.CreatedOnColumn.ColumnName] = DateTime.Now;
                                dr[dsDiagnosticStudy.DiagnosticStudy.ModifiedByColumn.ColumnName] = "Cypress";
                                dr[dsDiagnosticStudy.DiagnosticStudy.ModifiedOnColumn.ColumnName] = DateTime.Now;
                                dr[dsDiagnosticStudy.DiagnosticStudy.PatientIdColumn.ColumnName] = patientId;
                                dr[dsDiagnosticStudy.DiagnosticStudy.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                                dr[dsDiagnosticStudy.DiagnosticStudy.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;

                                if (obs.effectiveTime != null)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(obs.effectiveTime, out lowTime, out highTime);

                                    if (!string.IsNullOrEmpty(lowTime))
                                    {
                                        dr[dsDiagnosticStudy.DiagnosticStudy.TimeColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrEmpty(lowTime))
                                    {
                                        dr[dsDiagnosticStudy.DiagnosticStudy.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrEmpty(highTime))
                                    {
                                        dr[dsDiagnosticStudy.DiagnosticStudy.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else
                                {
                                    errors.Add("Diagnostic Study --> DOS not found");
                                }

                                if (obs.negationInd == true)
                                {
                                    if (obs.entryRelationship.Count > 0)
                                    {
                                        var obs1 = (MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.Observation)obs.entryRelationship[0].Item;
                                        var Negation = (MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)obs1.value[0];
                                        dr[dsDiagnosticStudy.DiagnosticStudy.ActionPerformedColumn.ColumnName] = Negation.code;

                                        string lowdate, highdate;
                                        GetLowHighTime(obs1.effectiveTime, out lowdate, out highdate);

                                        dr[dsDiagnosticStudy.DiagnosticStudy.StartDateColumn.ColumnName] = lowdate.ToFormatedDateTimeByFormat();

                                    }
                                }

                                dtDiagnosticStudy.Rows.Add(dr);

                            }
                        }
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Problems");
            }
        }
        private static void GetMedication(ClinicalDocument document, ref DataTable dtMedication, ref List<string> errors, long patientId)
        {
            try
            {
                DSCCDA dsMedications = new DSCCDA();
                Component3 encounterComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref encounterComp,
                    DocumentSections.PatientData);

                List<Entry> entryList = null;
                if (isComponentExist && encounterComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is SubstanceAdministration)
                        {
                            SubstanceAdministration sa = e.Item as SubstanceAdministration;
                            var isMedication = sa.text.Text[0].Split(':')[0];
                            if (isMedication == "Medication, Active" || isMedication == "Medication, Order")
                            {
                                DataRow dr = dtMedication.NewRow();
                                var isMedicationActive = sa.statusCode.code == "active" ? 1 : 0;
                                if (!sa.effectiveTime.IsNullOrEmpty() && sa.effectiveTime[0] is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime((IVL_TS)sa.effectiveTime[0], out lowTime, out highTime);

                                    if (!string.IsNullOrEmpty(lowTime))
                                    {
                                        dr[dsMedications.Medication.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrEmpty(highTime))
                                    {
                                        dr[dsMedications.Medication.StopDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else
                                {
                                    errors.Add("Medication --> Date Administred not found");
                                }

                                if (!sa.effectiveTime.IsNullOrEmpty() && sa.effectiveTime.Count > 1 && sa.effectiveTime[1] is PIVL_TS)
                                {
                                    var effectiveTime = sa.effectiveTime[1];
                                    if (effectiveTime != null)
                                    {
                                        var institutionSpecified1Field = ((PIVL_TS)(sa.effectiveTime[1])).institutionSpecified1;
                                        var _operator = (sa.effectiveTime[1]).@operator;
                                        var unit = ((PIVL_TS)(sa.effectiveTime[1])).period.unit;
                                        var value = ((PIVL_TS)(sa.effectiveTime[1])).period.value;
                                        if (value != null)
                                        {
                                            dr[dsMedications.Medication.DurationColumn.ColumnName] = value;
                                        }
                                    }
                                }
                                else
                                {
                                    errors.Add("Medication --> Something");
                                }
                                if (sa.doseQuantity != null)
                                {
                                    dr[dsMedications.Medication.DoseUnitColumn.ColumnName] = sa.doseQuantity.unit;
                                }
                                var consumable = sa.consumable;
                                var mp = consumable.manufacturedProduct;
                                dr[dsMedications.Medication.RxNormCodeColumn.ColumnName] = ((Material)mp.Item).code.code;
                                var codeSystem = ((Material)mp.Item).code.codeSystem;
                                dr[dsMedications.Medication.PatientIdColumn.ColumnName] = patientId;
                                dr[dsMedications.Medication.ProviderIdColumn.ColumnName] = MDVSession.Current.DefaultProviderId;
                                dr[dsMedications.Medication.FacilityIdColumn.ColumnName] = MDVSession.Current.DefaultFacilityId;
                                dr[dsMedications.Medication.CreatedByColumn.ColumnName] = "Cypress";
                                dr[dsMedications.Medication.CreatedOnColumn.ColumnName] = DateTime.Now;
                                dr[dsMedications.Medication.ModifiedByColumn.ColumnName] = "Cypress";
                                dr[dsMedications.Medication.ModifiedOnColumn.ColumnName] = DateTime.Now;


                                if (sa.negationInd == true)
                                {
                                    dr[dsMedications.Medication.NegationValuesetColumn.ColumnName] = ((Material)mp.Item).code.valueSet;
                                    var t = ((Summary.CCDA.Observation)sa.entryRelationship[0].Item).value;
                                    var s = t[0];
                                    var NegationCode = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(s)).code;
                                    dr[dsMedications.Medication.NegationIndexColumn.ColumnName] = 1;
                                    dr[dsMedications.Medication.NegationReasonColumn.ColumnName] = NegationCode;
                                }

                                else if (sa.entryRelationship.Count > 0 && !(sa.entryRelationship[0].Item is Supply))
                                {
                                    var eRealationship = ((Summary.CCDA.Observation)sa.entryRelationship[0].Item).value;
                                    var eRealationshipIndex = eRealationship[0];
                                    var ReasonCode = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(eRealationshipIndex)).code;

                                    dr[dsMedications.Medication.ActionResultColumn.ColumnName] = ReasonCode;
                                }


                                dtMedication.Rows.Add(dr);
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

        private static
            void GetLowHighTime(IVL_TS effectiveTime, out string lowTime, out string highTime)
        {
            lowTime = highTime = "";

            if (effectiveTime.ItemsElementName != null && effectiveTime.Items != null)
            {
                ItemsChoiceType2[] timeName = effectiveTime.ItemsElementName;
                QTY[] timeValues = effectiveTime.Items;

                for (int i = 0; i < 2; i++)
                {
                    if (timeName.Length > i && timeValues.Length > i && timeValues[i] != null && timeValues[i] is IVXB_TS &&
                        string.IsNullOrEmpty((timeValues[i] as IVXB_TS).nullFlavor))
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

        #endregion
    }
}