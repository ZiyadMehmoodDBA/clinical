using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Model.CDS;
using MDVision.Model.Clinical.Medical.CDS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MDVision.IEHR.EMR.Helpers.Clinical.CDS
{
    public class CDSHelper
    {

        private BLLPatient BLLPatientObj = null;
        private BLLClinical BLLClinicalObj = null;
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        public CDSHelper()
        {
            BLLPatientObj = new BLLPatient();
            BLLClinicalObj = new BLLClinical();
            BLLAdminSecurityObj = new BLLAdminSecurity();
        }
        private static CDSHelper _instance = null;
        public static CDSHelper Instance()
        {
            if (_instance == null)
                _instance = new CDSHelper();
            return _instance;
        }

        #region CDS Insert/Update
        /// <summary>
        /// Module Name: loadCDS
        /// Author: Humaira Yousaf
        /// Created Date: 03-03-2016
        /// Description: Loads CDS
        /// </summary>
        /// <param name="model" type="CDSModel">CDS model containing data</param>
        public string loadCDS(CDSModel model, string lstIDs, string isPopup)
        {
            try
            {

                DSPatient dsPatient = new DSPatient();
                Int64 currentPatientId = MDVUtility.ToInt64(model.PatientId);
                BLObject<DSPatient> objpatient = new BLObject<DSPatient>();
                if (currentPatientId > 0)
                {
                    objpatient = BLLPatientObj.FillPatientById(currentPatientId);
                    dsPatient = objpatient.Data;
                }
                DSVitals dsVitals = new DSVitals();
                DSAllergies dsAllergies = new DSAllergies();
                DSProblemLists dsProblemList = new DSProblemLists();
                DSClinicalMedication dsMedications = new DSClinicalMedication();
                DSLabResult dsLabResult = new DSLabResult();
                DSPatientLookups dsPatientInsurance = new DSPatientLookups();
                DSCDS dsconditions = new DSCDS();
                BLObject<DSCDS> conditions = new BLObject<DSCDS>();
                if (currentPatientId > 0)
                {
                    conditions = BLLClinicalObj.cdsActualConditions(true, MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), currentPatientId, "", "", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    dsconditions = conditions.Data;
                }

                byte isActive = Convert.ToByte(model.IsActive);
                if (model.commandType.ToLower() == "search_cdsforalert")
                {
                    model.CDSIDs = model.CDSId;
                    isActive = 1;
                }

                DSCDS dsCDS = null;
                BLObject<DSCDS> obj;
                obj = BLLClinicalObj.loadCDS(MDVUtility.ToInt32(model.CDSIDs), isActive, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", "", MDVUtility.ToInt64(model.PatientId));
                dsCDS = obj.Data;

                if (obj.Data != null)
                {
                    //Start//10-03-2016//Ahmad Raza//Logic to show CDS Search Grid with refined IDs
                    if (isPopup == "Yes")
                    {
                        DSCDS.CDSRow[] arrCDSRows = (DSCDS.CDSRow[])dsCDS.Tables[dsCDS.CDS.TableName].Select(dsCDS.CDS.CDSIdColumn + " in (" + lstIDs + ") and (" + dsCDS.CDS.StatusColumn + "  like '%Due%' or " + dsCDS.CDS.StatusColumn + " is null)");
                        List<Dictionary<string, string>> lstCDSKeyValues = new List<Dictionary<string, string>>();
                        var cdsKeyValues = new Dictionary<string, string> { { "", "" } };

                        if (arrCDSRows.Length > 0)
                        {
                            MDVisionLookups ob = new MDVisionLookups();
                            var ethnicityNames = ob.GetEthnicity("true");
                            var raceNames = ob.GetRace("true");


                            foreach (var dr in arrCDSRows)
                            {
                                //Start//06-03-2016//Ahmad Raza//Logic to get Rac and Ethnicity names on the base of comma separated IDs
                                string CommaSeparatedIds = MDVUtility.ToStr(dr[dsCDS.CDS.EthnicityColumn.ColumnName]);
                                string CommaSeparatedRaceIds = MDVUtility.ToStr(dr[dsCDS.CDS.RaceColumn.ColumnName]);
                                string[] ethnicityIds = CommaSeparatedIds.Split(',');
                                string[] raceIds = CommaSeparatedRaceIds.Split(',');
                                List<Ethnicity> lstEth = JsonConvert.DeserializeObject<List<Ethnicity>>(ethnicityNames);
                                List<Race> lstRace = JsonConvert.DeserializeObject<List<Race>>(raceNames);
                                List<string> lstMatchingEthnicity = new List<string>();
                                List<string> lstMatchingRace = new List<string>();
                                foreach (var name in lstEth)
                                {
                                    foreach (string id in ethnicityIds)
                                    {
                                        if (id == name.Value && id != "")
                                        {
                                            lstMatchingEthnicity.Add(name.Name);
                                        }
                                    }
                                }

                                string EthnicityNames = string.Join(",", lstMatchingEthnicity);
                                foreach (var name in lstRace)
                                {
                                    foreach (string id in raceIds)
                                    {
                                        if (id == name.Value && id != "")
                                        {
                                            lstMatchingRace.Add(name.Name);
                                        }
                                    }
                                }
                                string RaceNames = string.Join(",", lstMatchingRace);
                                //End//06-03-2016//Ahmad Raza//Logic to get Rac and Ethnicity names on the base of comma separated IDs


                                cdsKeyValues = new Dictionary<string, string>
                        {
                            { "CDSId",  MDVUtility.ToStr(dr[dsCDS.CDS.CDSIdColumn.ColumnName])},
                            { "Title",   MDVUtility.ToStr(dr[dsCDS.CDS.TitleColumn.ColumnName])},
                            { "Comments", MDVUtility.ToStr(dr[dsCDS.CDS.CommentsColumn.ColumnName])},
                            { "IsActive", MDVUtility.ToStr(dr[dsCDS.CDS.IsActiveColumn.ColumnName])},
                            { "Ethnicity", MDVUtility.ToStr(EthnicityNames)},
                            { "Status", MDVUtility.ToStr(dr[dsCDS.CDS.StatusColumn.ColumnName])},
                            { "Race", MDVUtility.ToStr(RaceNames)},
                            { "TriggerLocation", MDVUtility.ToStr(dr[dsCDS.CDS.TriggerLocationColumn.ColumnName])},
                            { "Gender", MDVUtility.ToStr(dr[dsCDS.CDS.GenderColumn.ColumnName])},
                            { "RuleTypeDescription", MDVUtility.ToStr(dr[dsCDS.CDS.RuleTypeDesColumn.ColumnName])},
                            { "txtDeveloper", MDVUtility.ToStr(dr[dsCDS.CDS.DeveloperColumn.ColumnName])},
                            { "txtFundingSource", MDVUtility.ToStr(dr[dsCDS.CDS.FundingSourceColumn.ColumnName])},
                            { "txtRelease", MDVUtility.ToStr(dr[dsCDS.CDS.ReleaseColumn.ColumnName])},
                            { "txtReferenceUrl", MDVUtility.ToStr(dr[dsCDS.CDS.ReferenceURLColumn.ColumnName])},
                            { "CDSAgeCondition", MDVUtility.ToStr(dr[dsCDS.CDS.AgeConditionIdColumn.ColumnName])},
                            { "CDSAgeFrom", MDVUtility.ToStr(dr[dsCDS.CDS.FromAgeColumn.ColumnName])},
                            { "CDSAgeTo", MDVUtility.ToStr(dr[dsCDS.CDS.ToAgeColumn.ColumnName])},
                        };
                                CommaSeparatedIds = MDVUtility.ToStr(dr[dsCDS.CDS.EthnicityColumn.ColumnName]);
                                CommaSeparatedRaceIds = MDVUtility.ToStr(dr[dsCDS.CDS.RaceColumn.ColumnName]);
                                lstCDSKeyValues.Add(cdsKeyValues);
                            }
                        }



                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            CDSCount = arrCDSRows.Length,
                            CDSLoad_JSON = js.Serialize(lstCDSKeyValues),
                            PatientDemographicJSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                            VitalSignJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsVitals.VitalSignSoap.TableName]),
                            CDSVitalSignJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSVitals.TableName]),
                            MedicationJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsMedications.Medication.TableName]),
                            CDSMedicationJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSMedication.TableName]),
                            ProblemListJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsProblemList.ProblemList.TableName]),
                            CDSProblemListJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSProblem.TableName]),
                            AllergyJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsAllergies.Allergy.TableName]),
                            CDSAllergyJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSAllergy.TableName]),
                            LabResultJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsLabResult.LabOrderResultDetail.TableName]),
                            CDSLabResultJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSLabResult.TableName]),
                            InsuranceJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsPatientInsurance.PatientInsurance.TableName]),
                            CDSInsuranceJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSInsurance.TableName]),
                            iTotalDisplayRecords = (arrCDSRows.Length > 0) ? arrCDSRows.Length : 0,
                        };
                        return (JsonConvert.SerializeObject(response));


                    }
                    //End//10-03-2016//Ahmad Raza//Logic to show CDS Search Grid with refined IDs
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CDSCount = dsCDS.Tables[dsCDS.CDS.TableName].Rows.Count,
                            CDSLoad_JSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDS.TableName]),
                            PatientDemographicJSON = dsPatient.Tables[dsPatient.Patients.TableName] != null ? MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]) : "[]",
                            VitalSignJSON = dsconditions.Tables[dsVitals.VitalSignSoap.TableName] != null ? MDVUtility.JSON_DataTable(dsconditions.Tables[dsVitals.VitalSignSoap.TableName]) : "[]",
                            CDSVitalSignJSON = dsCDS.Tables[dsCDS.CDSVitals.TableName] != null ? MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSVitals.TableName]) : "[]",
                            MedicationJSON = dsconditions.Tables[dsMedications.Medication.TableName] != null ? MDVUtility.JSON_DataTable(dsconditions.Tables[dsMedications.Medication.TableName]) : "[]",
                            CDSMedicationJSON = dsCDS.Tables[dsCDS.CDSMedication.TableName] != null ? MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSMedication.TableName]) : "[]",
                            ProblemListJSON = dsconditions.Tables[dsProblemList.ProblemList.TableName] != null ? MDVUtility.JSON_DataTable(dsconditions.Tables[dsProblemList.ProblemList.TableName]) : "[]",
                            CDSProblemListJSON = dsCDS.Tables[dsCDS.CDSProblem.TableName] != null ? MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSProblem.TableName]) : "[]",
                            AllergyJSON = dsconditions.Tables[dsAllergies.Allergy.TableName] != null ? MDVUtility.JSON_DataTable(dsconditions.Tables[dsAllergies.Allergy.TableName]) : "[]",
                            CDSAllergyJSON = dsCDS.Tables[dsCDS.CDSAllergy.TableName] != null ? MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSAllergy.TableName]) : "[]",
                            LabResultJSON = dsconditions.Tables[dsLabResult.LabOrderResultDetail.TableName] != null ? MDVUtility.JSON_DataTable(dsconditions.Tables[dsLabResult.LabOrderResultDetail.TableName]) : "[]",
                            CDSLabResultJSON = dsCDS.Tables[dsCDS.CDSLabResult.TableName] != null ? MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSLabResult.TableName]) : "[]",
                            iTotalDisplayRecords = (dsCDS.CDS.Rows.Count > 0) ? dsCDS.CDS.Rows[0][dsCDS.CDS.RecordCountColumn.ColumnName] : 0,
                        };


                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        CDSCount = 0,
                        Message = obj.Message
                    };

                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Module Name: loadCDS
        /// Author: Humaira Yousaf
        /// Created Date: 03-03-2016
        /// Description: Loads CDS
        /// </summary>
        /// <param name="model" type="CDSModel">CDS model containing data</param>
        public string loadCDSWithPatientStatus(long CDSId, long PatientId, string NoteId, long CDSPatientStatusId)
        {
            try
            {
                DSPatient dsPatient = new DSPatient();
                Int64 currentPatientId = MDVUtility.ToInt64(PatientId);

                BLObject<DSPatient> objpatient = new BLObject<DSPatient>();
                if (currentPatientId > 0)
                {
                    objpatient = BLLPatientObj.FillPatientById(currentPatientId);
                    dsPatient = objpatient.Data;
                }

                DSVitals dsVitals = new DSVitals();
                DSAllergies dsAllergies = new DSAllergies();
                DSProblemLists dsProblemList = new DSProblemLists();
                DSClinicalMedication dsMedications = new DSClinicalMedication();
                DSLabResult dsLabResult = new DSLabResult();
                DSPatientLookups dsPatientInsurance = new DSPatientLookups();
                DSCDS dsconditions = new DSCDS();
                BLObject<DSCDS> conditions = new BLObject<DSCDS>();

                if (currentPatientId > 0)
                {
                    conditions = BLLClinicalObj.cdsActualConditions(true, MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), currentPatientId, "", "", 1, 15);
                    dsconditions = conditions.Data;
                }

                DSCDS dsCDS = null;
                BLObject<DSCDS> obj;
                BLObject<DSCDS> obj_;
                obj = BLLClinicalObj.loadCDSWithPatientStatus(PatientId, CDSId, CDSPatientStatusId);
                dsCDS = obj.Data;
                obj_ = BLLClinicalObj.loadCDSOrderSet(PatientId, CDSId.ToString(), NoteId);
                if (obj_.Data != null)
                    dsCDS.Merge(obj_.Data);



                if (obj.Data != null)
                {
                    List<Dictionary<string, string>> lstCDSKeyValues = new List<Dictionary<string, string>>();
                    var cdsKeyValues = new Dictionary<string, string> { { "", "" } };

                    MDVisionLookups ob = new MDVisionLookups();
                    var ethnicityNames = ob.GetEthnicity("true");
                    var raceNames = ob.GetRace("true");


                    //foreach (var dr in arrCDSRows)
                    //{
                    //Start//06-03-2016//Ahmad Raza//Logic to get Rac and Ethnicity names on the base of comma separated IDs
                    string CommaSeparatedIds = MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0]["Ethnicity"]);
                    string CommaSeparatedRaceIds = MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0]["Race"]);
                    string[] ethnicityIds = CommaSeparatedIds.Split(',');
                    string[] raceIds = CommaSeparatedRaceIds.Split(',');
                    List<Ethnicity> lstEth = JsonConvert.DeserializeObject<List<Ethnicity>>(ethnicityNames);
                    List<Race> lstRace = JsonConvert.DeserializeObject<List<Race>>(raceNames);
                    List<string> lstMatchingEthnicity = new List<string>();
                    List<string> lstMatchingRace = new List<string>();
                    foreach (var name in lstEth)
                    {
                        foreach (string id in ethnicityIds)
                        {
                            if (id == name.Value && id != "")
                            {
                                lstMatchingEthnicity.Add(name.Name);
                            }
                        }
                    }

                    string EthnicityNames = string.Join(",", lstMatchingEthnicity);
                    foreach (var name in lstRace)
                    {
                        foreach (string id in raceIds)
                        {
                            if (id == name.Value && id != "")
                            {
                                lstMatchingRace.Add(name.Name);
                            }
                        }
                    }

                    string RaceNames = string.Join(",", lstMatchingRace);
                    //End//06-03-2016//Ahmad Raza//Logic to get Rac and Ethnicity names on the base of comma separated IDs


                    cdsKeyValues = new Dictionary<string, string>
                        {
                            { "CDSId",  MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.CDSIdColumn.ColumnName])},
                            { "Title",   MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.TitleColumn.ColumnName])},
                            { "Comments", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.CommentsColumn.ColumnName])},
                            { "QuestionnaireHTML", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.QuestionnaireHTMLColumn.ColumnName])},
                            { "IsActive", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.IsActiveColumn.ColumnName])},
                            { "Ethnicity", MDVUtility.ToStr(EthnicityNames)},
                            { "Status", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.StatusColumn.ColumnName])},
                            { "Race", MDVUtility.ToStr(RaceNames)},
                            { "TriggerLocation", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.TriggerLocationColumn.ColumnName])},
                            { "Gender", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.GenderColumn.ColumnName])},
                            { "RuleTypeDescription", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.RuleTypeDesColumn.ColumnName])},
                            { "txtDeveloper", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.DeveloperColumn.ColumnName])},
                            { "txtFundingSource", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.FundingSourceColumn.ColumnName])},
                            { "txtRelease", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.ReleaseColumn.ColumnName])},
                            { "txtReferenceUrl", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.ReferenceURLColumn.ColumnName])},
                            { "CDSAgeCondition", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.AgeConditionIdColumn.ColumnName])},
                            { "CDSAgeFrom", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.FromAgeColumn.ColumnName])},
                            { "CDSAgeTo", MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.ToAgeColumn.ColumnName])},
                            { "CDSPatientStatusId",  MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.CDSPatientStatusIdColumn.ColumnName])},
                        };
                    CommaSeparatedIds = MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.EthnicityNameColumn.ColumnName]);
                    CommaSeparatedRaceIds = MDVUtility.ToStr(dsCDS.CDSwithPatientStatus[0][dsCDS.CDSwithPatientStatus.RaceNameColumn.ColumnName]);
                    lstCDSKeyValues.Add(cdsKeyValues);
                    //}
                    //}



                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        CDSCount = 1,
                        CDSLoad_JSON = js.Serialize(lstCDSKeyValues),
                        PatientDemographicJSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                        VitalSignJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsVitals.VitalSignSoap.TableName]),
                        CDSVitalSignJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSVitals.TableName]),
                        MedicationJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsMedications.Medication.TableName]),
                        CDSMedicationJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSMedication.TableName]),
                        ProblemListJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsProblemList.ProblemList.TableName]),
                        CDSProblemListJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSProblem.TableName]),
                        AllergyJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsAllergies.Allergy.TableName]),
                        CDSAllergyJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSAllergy.TableName]),
                        LabResultJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsLabResult.LabOrderResultDetail.TableName]),
                        CDSLabResultJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSLabResult.TableName]),
                        InsuranceJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsPatientInsurance.PatientInsurance.TableName]),
                        CDSInsuranceJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSInsurance.TableName]),
                        CDSOrderSetJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSOrderSet.TableName]),
                        CDSNoteOrderSetJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSNoteOrderSet.TableName]),
                        //  iTotalDisplayRecords = (arrCDSRows.Length > 0) ? arrCDSRows.Length : 0,
                    };
                    return (JsonConvert.SerializeObject(response));


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        CDSCount = 0,
                        Message = obj.Message
                    };

                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }


        /// <summary>
        /// Module Name: loadCDSAlert
        /// Author: Ahmad Raza
        /// Created Date: 08-03-2016
        /// </summary>
        /// <param name="model" type="CDSModel">CDS model containing data</param>
        public string loadCDSAlert(CDSModel model)
        {

            try
            {
                //DSPatient dsPatient = null;
                //BLObject<DSPatient> objpatient = BLLPatientObj.FillPatientById(MDVUtility.ToInt64(model.PatientId));
                //dsPatient = objpatient.Data;
                DSCDS dsCDS = null;
                BLObject<DSCDS> obj;
                obj = BLLClinicalObj.loadCDS(MDVUtility.ToInt32(model.CDSId), 1, 1, 1000, "", "", MDVUtility.ToLong(model.PatientId));
                dsCDS = obj.Data;

                DSVitals dsPatientVitals = new DSVitals();
                DSProblemLists dsPatientProblemLists = new DSProblemLists();
                DSClinicalMedication dsPatientMedication = new DSClinicalMedication();
                DSAllergies dsPatientAllergies = new DSAllergies();
                DSLabResult dsPatientLabs = new DSLabResult();
                DSPatientLookups dsPatientInsurance = new DSPatientLookups();

                DSCDS dsCDSPatientStatus = null;
                BLObject<DSCDS> objStatus;
                objStatus = BLLClinicalObj.loadCDSPatientStatus(MDVUtility.ToInt32(model.PatientId));
                dsCDSPatientStatus = objStatus.Data;



                int cdsCounter = 0;
                List<int> lstCDSIds = new List<int>();
                if (obj.Data != null)
                {

                    //var t = ""+ dsCDSPatientStatus.CDSPatientStatus.EndDateColumn.ColumnName.ToString() + " <='2017-04-09'";

                    string locationQuery = "";
                    if (model.CDSTriggerLocation != "")
                    {
                        locationQuery = dsCDS.CDS.TriggerLocationColumn + " Like ('%" + model.CDSTriggerLocation + "" + "%') and ( " + dsCDS.CDS.StatusColumn.ColumnName.ToString() + " like '%Due%' or " + dsCDS.CDS.StatusColumn.ColumnName.ToString() + " is null)";
                    }
                    else
                    {
                        locationQuery = dsCDS.CDS.StatusColumn.ColumnName.ToString() + "  like '%Due%' or " + dsCDS.CDS.StatusColumn.ColumnName.ToString() + " is null";
                    }


                    List<DSCDS.CDSRow> arrCDSRows = null;
                    List<long> notInIDs = new List<long>();
                    notInIDs.Add(0);


                    Dictionary<long, long> CDSStatusIds = new Dictionary<long, long>();
                    foreach (DataRow PatientStatusRow in dsCDSPatientStatus.Tables[dsCDSPatientStatus.CDSPatientStatus.TableName].Rows)
                    {
                        if (MDVUtility.ToStr(PatientStatusRow["Status"]) == "Due")
                            CDSStatusIds.Add(MDVUtility.ToLong(PatientStatusRow["CDSPatientStatusId"]), MDVUtility.ToLong(PatientStatusRow["CDSId"]));
                    }

                    if (dsCDSPatientStatus.Tables[dsCDSPatientStatus.CDSPatientStatus.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow PatientStatusRow in dsCDSPatientStatus.Tables[dsCDSPatientStatus.CDSPatientStatus.TableName].Rows)
                        {
                            foreach (DataRow CDSRuleRow in dsCDS.Tables[dsCDS.CDS.TableName].Rows)
                            {
                                if (MDVUtility.ToInt64(PatientStatusRow["CDSId"]) == MDVUtility.ToInt64(CDSRuleRow["CDSId"]))
                                {

                                    arrCDSRows =
                                       (from a in dsCDS.CDS
                                        join b in dsCDSPatientStatus.CDSPatientStatus on a.CDSId equals b.CDSId
                                        where b.Status != "Done" && b.Status != "Override" && a.TriggerLocation.Split(',').Contains(model.CDSTriggerLocation)
                                        select a
                                       ).ToList();

                                    var arrCDSRows2 = dsCDS.Tables[dsCDS.CDS.TableName].Select(locationQuery).ToList();

                                    if ((MDVUtility.ToInt64(PatientStatusRow["CDSId"]) == MDVUtility.ToInt64(CDSRuleRow["CDSId"])) && arrCDSRows.Count == 0)
                                    {
                                        notInIDs.Add(MDVUtility.ToInt64(PatientStatusRow["CDSId"]));
                                        //string.Concat(locationQuery, " and CDSId not in (" + MDVUtility.ToInt64(PatientStatusRow["CDSId"]) + ")");
                                        arrCDSRows2 = dsCDS.Tables[dsCDS.CDS.TableName].Select(string.Concat(locationQuery, " and CDSId not in (" + string.Join(",", notInIDs) + ")")).ToList();
                                        arrCDSRows = new List<DSCDS.CDSRow>();
                                        foreach (var current in arrCDSRows2)
                                        {

                                            var desRow = dsCDS.CDS.NewCDSRow();
                                            desRow.ItemArray = current.ItemArray.Clone() as object[];
                                            arrCDSRows.Add(desRow);
                                        }
                                    }


                                }
                                else
                                {
                                    var arrCDSRows2 = dsCDS.Tables[dsCDS.CDS.TableName].Select(string.Concat(locationQuery, " and CDSId not in (" + string.Join(",", notInIDs) + ")")).ToList();
                                    if (arrCDSRows == null)
                                    {
                                        arrCDSRows = new List<DSCDS.CDSRow>();
                                    }
                                    foreach (var current in arrCDSRows2)
                                    {

                                        var desRow = dsCDS.CDS.NewCDSRow();
                                        desRow.ItemArray = current.ItemArray.Clone() as object[];
                                        arrCDSRows.Add(desRow);
                                    }

                                }
                                break;
                            }
                            break;
                        }
                    }
                    else
                    {
                        var arrCDSRows2 = dsCDS.Tables[dsCDS.CDS.TableName].Select(locationQuery).ToList();
                        if (arrCDSRows == null)
                            arrCDSRows = new List<DSCDS.CDSRow>();
                        foreach (var current in arrCDSRows2)
                        {

                            var desRow = dsCDS.CDS.NewCDSRow();
                            desRow.ItemArray = current.ItemArray.Clone() as object[];
                            arrCDSRows.Add(desRow);
                        }
                    }

                    if (arrCDSRows != null)
                    {
                        DSUserLookup dsRole = null;
                        BLObject<DSUserLookup> objRole;
                        string currentUserId = MDVUtility.ToStr(MDVSession.Current.AppUserId);
                        objRole = BLLAdminSecurityObj.LookupUserRolesUser("1", currentUserId, "", "");
                        dsRole = objRole.Data;

                        int initialCounter = 0;
                        foreach (DSCDS.CDSRow dr in arrCDSRows)
                        {
                            if (dr.Role != "")
                            {
                                string strRoleQuery = dr.Role != "" ? dsRole.UsersRoleSelect.UserRoleIdColumn.ColumnName + " in (" + dr.Role + ")" : "";
                                if (!string.IsNullOrEmpty(strRoleQuery))
                                {
                                    DSUserLookup.UsersRoleSelectRow[] arrUsersRoleSelectRows = (DSUserLookup.UsersRoleSelectRow[])dsRole.UsersRoleSelect.Select(strRoleQuery);
                                    if (arrUsersRoleSelectRows.Length < 1)
                                    {
                                        continue;
                                    }

                                }
                            }

                            bool isMatched = BLLClinicalObj.isPatientMatched(MDVUtility.ToInt64(model.PatientId), dr.CDSQuery);
                            // DSPatient.PatientsRow[] objCDSPatient = (DSPatient.PatientsRow[])dsPatient.Tables[dsPatient.Patients.TableName].Select(dr.CDSQuery);
                            if (isMatched == true)//(objCDSPatient.Length > 0)
                            {
                                bool vitalRuleIsSelected = false, ProbleListRuleIsSelected = false, MedicationRuleIsSelected = false, AllergiesRuleIsSelected = false, LabResultsRuleIsSelected = false, InsuranceRuleIsSelected = false;

                                string[] ruleTypeIds = dr.RuleType.Split(',');
                                foreach (string ruleTypeId in ruleTypeIds)
                                {
                                    if (ruleTypeId == "1")
                                    {
                                        vitalRuleIsSelected = true;
                                    }
                                    else if (ruleTypeId == "2")
                                    {
                                        ProbleListRuleIsSelected = true;
                                    }
                                    else if (ruleTypeId == "3")
                                    {
                                        MedicationRuleIsSelected = true;
                                    }
                                    else if (ruleTypeId == "4")
                                    {
                                        AllergiesRuleIsSelected = true;
                                    }
                                    else if (ruleTypeId == "5")
                                    {
                                        LabResultsRuleIsSelected = true;
                                    }
                                    else if (ruleTypeId == "6")
                                    {
                                        InsuranceRuleIsSelected = true;
                                    }
                                }
                                if (initialCounter == 0)
                                {

                                    //Start Farooq Ahmad if contain query then load the component
                                    var arrCDSvitalRows = arrCDSRows.Where(p => p.VitalsQuery != string.Empty);
                                    if (arrCDSvitalRows.Count() > 0)
                                    {
                                        BLObject<DSVitals> objpatientVitals = BLLClinicalObj.LoadAllVitals(MDVUtility.ToInt64(model.PatientId), 0);
                                        dsPatientVitals = objpatientVitals.Data;
                                    }
                                    var arrCDSProblemRows = arrCDSRows.Where(p => p.CDSProblemListQuery != string.Empty);
                                    if (arrCDSProblemRows.Count() > 0)
                                    {
                                        BLObject<DSProblemLists> objpatientProblemList = BLLClinicalObj.LoadAllProblemLists(0, MDVUtility.ToInt64(model.PatientId), 0, "0", "");
                                        dsPatientProblemLists = objpatientProblemList.Data;
                                    }

                                    var arrCDSMedicationRows = arrCDSRows.Where(p => p.MedicationsQuery != string.Empty);
                                    if (arrCDSMedicationRows.Count() > 0)
                                    {
                                        BLObject<DSClinicalMedication> objpatientMedication = BLLClinicalObj.loadAllMedications(0, MDVUtility.ToInt64(model.PatientId), 0, true);
                                        dsPatientMedication = objpatientMedication.Data;
                                    }

                                    var arrCDSAllergiesRows = arrCDSRows.Where(p => p.AllergiesQuery != string.Empty);
                                    if (arrCDSAllergiesRows.Count() > 0)
                                    {
                                        BLObject<DSAllergies> objpatientAllergies = BLLClinicalObj.loadAllAllergies_Obsolete(0, MDVUtility.ToInt64(model.PatientId), 0, "0", "");
                                        dsPatientAllergies = objpatientAllergies.Data;
                                    }

                                    var arrCDSLabResultRows = arrCDSRows.Where(p => p.LabsQuery != string.Empty);
                                    if (arrCDSLabResultRows.Count() > 0)
                                    {
                                        BLObject<DSLabResult> objpatientLabs = BLLClinicalObj.LoadLabResultForCDS(MDVUtility.ToInt64(model.PatientId));
                                        dsPatientLabs = objpatientLabs.Data;
                                    }


                                    var arrCDSInsuranceRows = arrCDSRows.Where(p => p.InsuranceQuery != string.Empty);
                                    if (arrCDSInsuranceRows.Count() > 0)
                                    {
                                        BLObject<DSPatientLookups> objpatientInsurance = new BLLPatient().LookupPatientInsurance(MDVUtility.ToInt64(model.PatientId), "1");
                                        dsPatientInsurance = objpatientInsurance.Data;
                                    }

                                    //End Farooq Ahmad if contain query then load the component
                                    initialCounter += 1;
                                }

                                // bool isORQuery = false;
                                int recordCount = 0;
                                DSVitals.VitalSignSoapRow[] arrCDSPatientVitals = null;
                                DSClinicalMedication.MedicationRow[] arrCDSPatientMedications = null;
                                DSProblemLists.ProblemListRow[] arrCDSPatientProblemList = null;
                                DSAllergies.AllergyRow[] arrCDSPatientAllergies = null;
                                DSLabResult.LabOrderResultDetailRow[] arrCDSPatientLabs = null;
                                DSPatientLookups.PatientInsuranceRow[] arrCDSPatientInsurance = null;


                                #region Vital Query


                                if (vitalRuleIsSelected && dr.VitalsOperator.ToLower() != "" && dr.VitalsQuery != "")
                                {
                                    foreach (DSVitals.VitalSignSoapRow drr in dsPatientVitals.VitalSignSoap.Rows)
                                    {
                                        if (drr["Height"].ToString() == "")
                                            drr.Height = "0.0";

                                        if (drr["Weight"].ToString() == "")
                                            drr.Weight = "0.0";

                                        if (drr["BMI"].ToString() == "")
                                            drr.BMI = "0.0";

                                        if (drr["Systolic"].ToString() == "")
                                            drr.Systolic = "0.0";

                                        if (drr["Diastolic"].ToString() == "")
                                            drr.Diastolic = "0.0";

                                        if (drr["PulseResult"].ToString() == "")
                                            drr.PulseResult = "0.0";

                                        if (drr["TemperatureResult"].ToString() == "")
                                            drr.TemperatureResult = "0.0";

                                        if (drr["RespirationResult"].ToString() == "")
                                            drr.RespirationResult = "0.0";

                                        if (drr["SPO2"].ToString() == "")
                                            drr.SPO2 = "0.0";
                                    }
                                    if (dr.VitalsQuery.Contains("Temprature"))
                                        dr.VitalsQuery = dr.VitalsQuery.Replace("Temprature", "Convert(TemperatureResult, 'System.Decimal')");
                                    if (dr.VitalsQuery.Contains("Pulse"))
                                        dr.VitalsQuery = dr.VitalsQuery.Replace("Pulse", "Convert(PulseResult, 'System.Decimal')");
                                    if (dr.VitalsQuery.Contains("RespiratoryRate"))
                                        dr.VitalsQuery = dr.VitalsQuery.Replace("RespiratoryRate", "Convert(RespirationResult, 'System.Decimal')");
                                    if (dr.VitalsQuery.Contains("OxygenSaturation"))
                                        dr.VitalsQuery = dr.VitalsQuery.Replace("OxygenSaturation", "Convert(SPO2, 'System.Decimal')");
                                    if (dr.VitalsQuery.Contains("Weight"))
                                        dr.VitalsQuery = dr.VitalsQuery.Replace("Weight", "Convert(Weight, 'System.Decimal')");
                                    if (dr.VitalsQuery.Contains("Height"))
                                        dr.VitalsQuery = dr.VitalsQuery.Replace("Height", "Convert(Height, 'System.Decimal')");
                                    if (dr.VitalsQuery.Contains("BMI"))
                                        dr.VitalsQuery = dr.VitalsQuery.Replace("BMI", "Convert(BMI, 'System.Decimal')");
                                    if (dr.VitalsQuery.Contains("Systolic"))
                                        dr.VitalsQuery = dr.VitalsQuery.Replace("Systolic", "Convert(Systolic, 'System.Decimal')");
                                    if (dr.VitalsQuery.Contains("Diastolic"))
                                        dr.VitalsQuery = dr.VitalsQuery.Replace("Diastolic", "Convert(Diastolic, 'System.Decimal')");
                                    arrCDSPatientVitals = (DSVitals.VitalSignSoapRow[])dsPatientVitals.VitalSignSoap.Select(dr.VitalsQuery);
                                    if (arrCDSPatientVitals.Length > 0)
                                    {
                                        recordCount += 1;
                                    }
                                    else
                                    {
                                        if (dr.VitalsOperator.ToLower() == "and")
                                        {
                                            continue;
                                        }

                                    }
                                }
                                #endregion

                                #region Medication Query


                                if (MedicationRuleIsSelected && dr.MedicationsOperator.ToLower() != "" && dr.MedicationsQuery != "")
                                {
                                    var medicationResult = true;
                                    if (dr.MedicationsOperator.ToLower() == "and")
                                    {
                                        if (dr.MedicationsQuery.ToLower().IndexOf("and") > -1)
                                        {
                                            medicationResult = true;
                                            var query = dr.MedicationsQuery.Replace("AND", "&").Split('&');
                                            foreach (var q in query)
                                            {
                                                arrCDSPatientMedications = (DSClinicalMedication.MedicationRow[])dsPatientMedication.Tables[dsPatientMedication.Medication.TableName].Select(q);
                                                if (arrCDSPatientMedications == null || arrCDSPatientMedications.Length == 0)
                                                {
                                                    medicationResult = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            medicationResult = false;
                                            var query = dr.MedicationsQuery.Replace("OR", "|").Split('|');
                                            foreach (var q in query)
                                            {
                                                arrCDSPatientMedications = (DSClinicalMedication.MedicationRow[])dsPatientMedication.Tables[dsPatientMedication.Medication.TableName].Select(q);
                                                if (arrCDSPatientMedications != null && arrCDSPatientMedications.Length > 0)
                                                {
                                                    medicationResult = true;
                                                }
                                            }
                                        }
                                    }

                                    //arrCDSPatientMedications = (DSClinicalMedication.MedicationRow[])dsPatientMedication.Tables[dsPatientMedication.Medication.TableName].Select(dr.MedicationsQuery);
                                    if (medicationResult == true)
                                    {
                                        recordCount += 1;
                                    }
                                    else
                                    {
                                        if (dr.MedicationsOperator.ToLower() == "and")
                                        {
                                            continue;
                                        }

                                    }
                                }
                                #endregion

                                #region ProblemList Query

                                if (ProbleListRuleIsSelected && dr.ProblemListOperator.ToLower() != "" && dr.CDSProblemListQuery != "")
                                {
                                    var problemResult = true;
                                    if (dr.ProblemListOperator.ToLower() == "and")
                                    {
                                        if (dr.CDSProblemListQuery.ToLower().IndexOf("and") > -1)
                                        {
                                            problemResult = true;
                                            var query = dr.CDSProblemListQuery.Replace("AND", "&").Split('&');
                                            foreach (var q in query)
                                            {
                                                arrCDSPatientProblemList = (DSProblemLists.ProblemListRow[])dsPatientProblemLists.Tables[dsPatientProblemLists.ProblemList.TableName].Select(q);
                                                if (arrCDSPatientProblemList == null || arrCDSPatientProblemList.Length == 0)
                                                {
                                                    problemResult = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            problemResult = false;
                                            var query = dr.CDSProblemListQuery.Replace("OR", "|").Split('|');
                                            foreach (var q in query)
                                            {
                                                arrCDSPatientProblemList = (DSProblemLists.ProblemListRow[])dsPatientProblemLists.Tables[dsPatientProblemLists.ProblemList.TableName].Select(q);
                                                if (arrCDSPatientProblemList != null && arrCDSPatientProblemList.Length > 0)
                                                {
                                                    problemResult = true;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {

                                    }
                                    //arrCDSPatientProblemList = (DSProblemLists.ProblemListRow[])dsPatientProblemLists.Tables[dsPatientProblemLists.ProblemList.TableName].Select(dr.CDSProblemListQuery);

                                    if (problemResult == true)
                                    {
                                        recordCount += 1;
                                    }
                                    else
                                    {
                                        if (dr.ProblemListOperator.ToLower() == "and")
                                        {
                                            continue;
                                        }

                                    }
                                }

                                #endregion

                                #region Allergy Query
                                if (AllergiesRuleIsSelected && dr.AllergiesOperator.ToLower() != "" && dr.AllergiesQuery != "")
                                {
                                    var result = true;
                                    var qryCondition = " AND " + dsPatientAllergies.Allergy.IsActiveColumn.ColumnName + "='true' AND " + dsPatientAllergies.Allergy.IsDeletedColumn.ColumnName + "='false'";
                                    if (dr.AllergiesOperator.ToLower() == "and")
                                    {
                                        if (dr.AllergiesQuery.ToLower().IndexOf("and") > -1)
                                        {
                                            var query = dr.AllergiesQuery.Replace("AND", "&").Split('&');
                                            foreach (var q in query)
                                            {
                                                if (q != "")
                                                {
                                                    arrCDSPatientAllergies = (DSAllergies.AllergyRow[])dsPatientAllergies.Tables[dsPatientAllergies.Allergy.TableName].Select(q + qryCondition);
                                                    if (arrCDSPatientAllergies == null || arrCDSPatientAllergies.Length == 0)
                                                    {
                                                        result = false;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            result = false;
                                            var query = dr.AllergiesQuery.Replace("OR", "|").Split('|');
                                            foreach (var q in query)
                                            {
                                                if (q != "")
                                                {
                                                    arrCDSPatientAllergies = (DSAllergies.AllergyRow[])dsPatientAllergies.Tables[dsPatientAllergies.Allergy.TableName].Select(q + qryCondition);
                                                    if (arrCDSPatientAllergies != null && arrCDSPatientAllergies.Length > 0)
                                                    {
                                                        result = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (result == true)
                                    {
                                        recordCount += 1;
                                    }
                                    else
                                    {
                                        if (dr.AllergiesOperator.ToLower() == "and")
                                        {
                                            continue;
                                        }

                                    }
                                }
                                #endregion

                                #region Labs Query
                                if (LabResultsRuleIsSelected && dr.LabsOperator.ToLower() != "" && dr.LabsQuery != "")
                                {
                                    //  var result = false;
                                    var resultIsFound = false;
                                    decimal dummy;
                                    if (dr.LabsOperator.ToLower() == "and")
                                    {
                                        //Start
                                        var labTestQueries = dr.LabsQuery.Split(':');
                                        foreach (var labTestQuery in labTestQueries)
                                        {
                                            var orQueries = labTestQuery.Split('|');
                                            foreach (var q2 in orQueries)
                                            {
                                                if (q2.Contains("&"))
                                                {
                                                    bool innerResult = true;
                                                    var andQuery = q2.Split('&');

                                                    foreach (var q3 in andQuery)
                                                    {
                                                        var qry = q3;
                                                        if (qry.Contains("Result") && !qry.Contains("like"))
                                                            qry = qry.Replace("Result", "Convert(Result, 'System.Decimal')");
                                                        if (qry.Contains("Result"))
                                                        {
                                                            if (qry.Contains("like"))
                                                                arrCDSPatientLabs = (DSLabResult.LabOrderResultDetailRow[])dsPatientLabs.Tables[dsPatientLabs.LabOrderResultDetail.TableName].Select(qry);
                                                            else
                                                            {
                                                                var filteredListWithOutStringValues = dsPatientLabs.LabOrderResultDetail.AsEnumerable<DSLabResult.LabOrderResultDetailRow>().Where(row => decimal.TryParse(row["Result"].ToString(), out dummy)).ToArray<DSLabResult.LabOrderResultDetailRow>();
                                                                if (filteredListWithOutStringValues.Length > 0)
                                                                {
                                                                    var result = filteredListWithOutStringValues.CopyToDataTable().Select(qry);
                                                                    if (result != null && result.Length > 0)
                                                                    {
                                                                        //arrCDSPatientLabs = new DSLabResult.LabOrderResultDetailRow[1] { dsPatientLabs.LabOrderResultDetail.NewLabOrderResultDetailRow() };
                                                                        resultIsFound = true;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                            arrCDSPatientLabs = (DSLabResult.LabOrderResultDetailRow[])dsPatientLabs.Tables[dsPatientLabs.LabOrderResultDetail.TableName].Select(qry);

                                                        if (arrCDSPatientLabs == null || arrCDSPatientLabs.Length == 0)
                                                        {
                                                            innerResult = false;
                                                            break;
                                                        }
                                                    }
                                                    if (innerResult)
                                                        resultIsFound = true;
                                                }
                                                else
                                                {
                                                    var qry = q2;
                                                    if (qry.Contains("Result") && !qry.Contains("like"))
                                                        qry = qry.Replace("Result", "Convert(Result, 'System.Decimal')");
                                                    if (qry.Contains("Result"))
                                                    {
                                                        if (qry.Contains("like"))
                                                            arrCDSPatientLabs = (DSLabResult.LabOrderResultDetailRow[])dsPatientLabs.Tables[dsPatientLabs.LabOrderResultDetail.TableName].Select(qry);
                                                        else
                                                        {
                                                            var filteredListWithOutStringValues = dsPatientLabs.LabOrderResultDetail.AsEnumerable<DSLabResult.LabOrderResultDetailRow>().Where(row => decimal.TryParse(row["Result"].ToString(), out dummy)).ToArray<DSLabResult.LabOrderResultDetailRow>();
                                                            if (filteredListWithOutStringValues.Length > 0)
                                                            {
                                                                var result = filteredListWithOutStringValues.CopyToDataTable().Select(qry);
                                                                if (result != null && result.Length > 0)
                                                                {
                                                                    //arrCDSPatientLabs = new DSLabResult.LabOrderResultDetailRow[1] { dsPatientLabs.LabOrderResultDetail.NewLabOrderResultDetailRow() };
                                                                    resultIsFound = true;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                        arrCDSPatientLabs = (DSLabResult.LabOrderResultDetailRow[])dsPatientLabs.Tables[dsPatientLabs.LabOrderResultDetail.TableName].Select(qry);
                                                    if (arrCDSPatientLabs == null || arrCDSPatientLabs.Length == 0)
                                                        resultIsFound = false;
                                                    else
                                                        resultIsFound = true;
                                                }
                                                if (resultIsFound)
                                                    break;
                                            }

                                            if (resultIsFound)
                                                break;
                                        }
                                    }

                                    //fixme
                                    if (resultIsFound)
                                    {
                                        recordCount++;
                                    }
                                    else
                                    {
                                        if (dr.LabsOperator.ToLower() == "and")
                                        {
                                            continue;
                                        }

                                    }
                                }
                                #endregion

                                #region Insurance Query
                                if (InsuranceRuleIsSelected && dr.InsuranceOperator.ToLower() != "" && dr.InsuranceQuery != "")
                                {
                                    var insuranceResult = true;
                                    if (dr.InsuranceOperator.ToLower() == "and")
                                    {
                                        if (dr.InsuranceQuery.ToLower().IndexOf("and") > -1)
                                        {
                                            insuranceResult = true;
                                            var query = dr.InsuranceQuery.Replace("AND", "&").Split('&');
                                            foreach (var q in query)
                                            {
                                                arrCDSPatientInsurance = (DSPatientLookups.PatientInsuranceRow[])dsPatientInsurance.Tables[dsPatientInsurance.PatientInsurance.TableName].Select(q);
                                                if (arrCDSPatientInsurance == null || arrCDSPatientInsurance.Length == 0)
                                                {
                                                    insuranceResult = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            insuranceResult = false;
                                            var query = dr.InsuranceQuery.Replace("OR", "|").Split('|');
                                            foreach (var q in query)
                                            {
                                                arrCDSPatientInsurance = (DSPatientLookups.PatientInsuranceRow[])dsPatientInsurance.Tables[dsPatientInsurance.PatientInsurance.TableName].Select(q);
                                                if (arrCDSPatientInsurance != null && arrCDSPatientInsurance.Length > 0)
                                                {
                                                    insuranceResult = true;
                                                }
                                            }
                                        }
                                    }

                                    if (insuranceResult == true)
                                    {
                                        recordCount++;
                                    }
                                    else
                                    {
                                        if (dr.InsuranceOperator.ToLower() == "and")
                                        {
                                            continue;
                                        }

                                    }
                                }
                                #endregion


                                if (arrCDSPatientVitals != null && arrCDSPatientVitals.Length > 0 && dr.VitalsOperator.ToLower() == "and")
                                {

                                }

                                DataRow[] drs = dsCDSPatientStatus.CDSPatientStatus.
                                     Select(dsCDSPatientStatus.CDSPatientStatus.CDSIdColumn.ColumnName
                                     + " = " + dr.CDSId
                                     + " AND ( " + dsCDSPatientStatus.CDSPatientStatus.StatusColumn.ColumnName + " = 'Done' "
                                     + " OR " + dsCDSPatientStatus.CDSPatientStatus.StatusColumn.ColumnName + " = 'Override' ) ");

                                if (drs.Count() <= 0)
                                {
                                    cdsCounter++;

                                    lstCDSIds.Add(MDVUtility.ToInt(dr.CDSId));
                                    CDSModel cdsStatusModel = new CDSModel();
                                    cdsStatusModel.PatientId = model.PatientId;
                                    cdsStatusModel.CDSId = MDVUtility.ToStr(dr.CDSId);
                                    this.insertCDStatus(cdsStatusModel, objStatus);
                                }


                            }
                        }
                    }


                    var commaSeparatedIDs = string.Join(",", lstCDSIds);
                    var cds = CDSStatusIds.Where(p => p.Key > 0 && commaSeparatedIDs.Contains(p.Value.ToString()) == false);
                    if (cds.Count() > 0)
                    {
                        string CDSPatientStatusIds = string.Join(",", cds.Select(n => n.Key.ToString()).ToArray());
                        BLLClinicalObj.DeleteCDSPatientStatusId(CDSPatientStatusIds);
                    }


                    var response = new
                    {
                        status = true,
                        alertCount = cdsCounter,
                        CDSIDs = commaSeparatedIDs,
                        CDSCount = dsCDS.Tables[dsCDS.CDS.TableName].Rows.Count,
                        CDSLoad_JSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDS.TableName]),
                        iTotalDisplayRecords = (dsCDS.CDS.Rows.Count > 0) ? dsCDS.CDS.Rows[0][dsCDS.CDS.RecordCountColumn.ColumnName] : 0,
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        CDSCount = 0,
                        Message = obj.Message
                    };

                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Module Name: fillCDS
        /// Author: Humaira Yousaf
        /// Created Date: 04-03-2016
        /// Description: Fills CDS data
        /// </summary>
        /// <param name="model" type="CDSModel">CDS model</param>
        public string fillCDS(CDSModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.CDSId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.Select_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSCDS dsCDS = null;
                    //Start 13-05-2016 Edit By Humaira Yousaf Bug# EMR-1050
                    byte isActive = 1;
                    if (model.IsActive == true)
                        isActive = 1;
                    else
                        isActive = 0;
                    //End 13-05-2016 Edit By Humaira Yousaf Bug# EMR-1050
                    BLObject<DSCDS> obj = BLLClinicalObj.loadCDS(MDVUtility.ToInt32(model.CDSId), isActive, 1, 15, "1", "");
                    if (obj.Data != null)
                    {
                        dsCDS = obj.Data;
                        if (dsCDS.Tables[dsCDS.CDS.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsCDS.Tables[dsCDS.CDS.TableName].Rows[0];
                            //Start 08-03-2016 Humaira Yousaf to load CDSVitals
                            //  DataRow drVitals = dsCDS.Tables[dsCDS.CDSVitals.TableName].Select(dsCDS.CDSVitals.CDSIdColumn + "=" + MDVUtility.ToLINQFormatString(model.CDSId)).FirstOrDefault();
                            //End 08-03-2016 Humaira Yousaf to load CDSVitals
                            var keyValues = new Dictionary<string, string>
                            {
                                {"CDSId",MDVUtility.ToStr(dr[dsCDS.CDS.CDSIdColumn.ColumnName])},
                                { "txtCDSTitle", MDVUtility.ToStr(dr[dsCDS.CDS.TitleColumn.ColumnName])},
                                { "txtCDSDeveloper", MDVUtility.ToStr(dr[dsCDS.CDS.DeveloperColumn.ColumnName])},
                                { "txtCDSFundingSource", MDVUtility.ToStr(dr[dsCDS.CDS.FundingSourceColumn.ColumnName])},
                                { "txtCDSReferenceURL", MDVUtility.ToStr(dr[dsCDS.CDS.ReferenceURLColumn.ColumnName])},
                                { "txtCDSRelease", MDVUtility.ToStr(dr[dsCDS.CDS.ReleaseColumn.ColumnName])},
                                { "dtCDSRevisionDate", MDVUtility.ToStr(dr[dsCDS.CDS.RevisionDateColumn.ColumnName])},
                                { "ddlCDSTriggerLocation", MDVUtility.ToStr(dr[dsCDS.CDS.TriggerLocationColumn.ColumnName])},
                                { "lstCDSUserRoles", MDVUtility.ToStr(dr[dsCDS.CDS.RoleColumn.ColumnName])},
                                { "ddlCDSRuleType", MDVUtility.ToStr(dr[dsCDS.CDS.RuleTypeColumn.ColumnName])},
                                { "ddlCDSSex", MDVUtility.ToStr(dr[dsCDS.CDS.GenderColumn.ColumnName])},
                                { "ddlCDSEthnicity", MDVUtility.ToStr(dr[dsCDS.CDS.EthnicityColumn.ColumnName])},
                                { "ddlCDSRace", MDVUtility.ToStr(dr[dsCDS.CDS.RaceColumn.ColumnName])},
                                { "ddlCDSLanguage", MDVUtility.ToStr(dr[dsCDS.CDS.LanguageColumn.ColumnName])},
                                { "txtCDSReminderLength", MDVUtility.ToStr(dr[dsCDS.CDS.ReminderLengthColumn.ColumnName])},
                                { "ddlCDSReminderPeriod", MDVUtility.ToStr(dr[dsCDS.CDS.ReminderPeriodIdColumn.ColumnName])},
                                { "chkCDSRecursive", MDVUtility.ToStr(dr[dsCDS.CDS.IsRecursiveColumn.ColumnName])},
                                { "chkCDSActive", MDVUtility.ToStr(dr[dsCDS.CDS.IsActiveColumn.ColumnName])},
                                { "ddlProblemList", MDVUtility.ToStr(dr[dsCDS.CDS.ProblemListOperatorColumn.ColumnName])},
                                { "ddlAllergies", MDVUtility.ToStr(dr[dsCDS.CDS.AllergiesOperatorColumn.ColumnName])},
                                { "ddlMedications", MDVUtility.ToStr(dr[dsCDS.CDS.MedicationsOperatorColumn.ColumnName])},
                                { "ddlLabResults", MDVUtility.ToStr(dr[dsCDS.CDS.LabsOperatorColumn.ColumnName])},
                                { "ddlVitals", MDVUtility.ToStr(dr[dsCDS.CDS.VitalsOperatorColumn.ColumnName])},
                                { "txtCDSAlertNote", MDVUtility.ToStr(dr[dsCDS.CDS.CommentsColumn.ColumnName])},
                                //Start 10-03-2016 Humaira Yousaf for Age Condition
                                { "ddlCDSAgeCondition", MDVUtility.ToStr(dr[dsCDS.CDS.AgeConditionIdColumn.ColumnName])},
                                //End 10-03-2016 Humaira Yousaf for Age Condition
                                { "txtStatus", MDVUtility.ToStr(dr[dsCDS.CDS.StatusColumn.ColumnName])},
                                { "OrderSetIds", MDVUtility.ToStr(dr[dsCDS.CDS.OrderSetIdsColumn.ColumnName])},
                                { "txtCDSRecursiveLength", MDVUtility.ToStr(dr[dsCDS.CDS.RecursiveLengthColumn.ColumnName])},
                                { "ddlCDSRecursivePeriod", MDVUtility.ToStr(dr[dsCDS.CDS.RecursivePeriodIdColumn.ColumnName])},
                                { "ddlInsurance", MDVUtility.ToStr(dr[dsCDS.CDS.InsuranceOperatorColumn.ColumnName])},
                                { "ddlCDSProviders", MDVUtility.ToStr(dr[dsCDS.CDS.ProviderIdsColumn.ColumnName])}

                            };

                            //Start 10-03-2016 Humaira Yousaf for Age Condition
                            int ageId = MDVUtility.ToInt32(dr[dsCDS.CDS.AgeConditionIdColumn.ColumnName]);
                            if (ageId == 1 || ageId == 2 || ageId == 3)
                            {
                                keyValues.Add("txtCDSAgeValue", MDVUtility.ToStr(dr[dsCDS.CDS.FromAgeColumn.ColumnName]));
                            }
                            else if (ageId == 4 || ageId == 5)
                            {
                                keyValues.Add("txtCDSAgeValue", MDVUtility.ToStr(dr[dsCDS.CDS.ToAgeColumn.ColumnName]));
                            }
                            else if (ageId == 6)
                            {
                                keyValues.Add("txtCDSAgeFrom", MDVUtility.ToStr(dr[dsCDS.CDS.FromAgeColumn.ColumnName]));
                                keyValues.Add("txtCDSAgeTo", MDVUtility.ToStr(dr[dsCDS.CDS.ToAgeColumn.ColumnName]));
                            }
                            //End 10-03-2016 Humaira Yousaf for Age Condition

                            //Start 08-03-2016 Humaira Yousaf to load CDSVitals
                            //var vitalsValues = new Dictionary<string, string>();
                            //if (drVitals != null)
                            //{
                            //    vitalsValues = new Dictionary<string, string>
                            //    {
                            //        {"vitalsId", MDVUtility.ToStr(drVitals[dsCDS.CDSVitals.CDSVitalsIdColumn])},
                            //        { "txtWeight", MDVUtility.ToStr(drVitals[dsCDS.CDSVitals.WeightColumn])},
                            //        {"txtHeight", MDVUtility.ToStr(drVitals[dsCDS.CDSVitals.HeightColumn])},
                            //        {"txtBMI", MDVUtility.ToStr(drVitals[dsCDS.CDSVitals.BMIColumn])},
                            //        {"txtSystolicTemplate", MDVUtility.ToStr(drVitals[dsCDS.CDSVitals.SystolicColumn])},
                            //        {"txtDiastolicTemplate", MDVUtility.ToStr(drVitals[dsCDS.CDSVitals.DiastolicColumn])},
                            //    };
                            //}
                            //Start 10-03-2016 Humaira Yousaf for CDS Medications
                            List<Dictionary<string, string>> lstMedications = new List<Dictionary<string, string>>();
                            foreach (DataRow drMedication in dsCDS.Tables[dsCDS.CDSMedication.TableName].Rows)
                            {
                                var medicationValues = new Dictionary<string, string>
                                {
                                    {"drugId", MDVUtility.ToStr(drMedication[dsCDS.CDSMedication.DrugIdColumn])},
                                    {"medicationOperator", MDVUtility.ToStr(drMedication[dsCDS.CDSMedication.MedicationOperatorColumn])},
                                    {"medicationName", MDVUtility.ToStr(drMedication[dsCDS.CDSMedication.MedicationNameColumn])},
                                    {"MedicationId", MDVUtility.ToStr(drMedication[dsCDS.CDSMedication.CDSMedicationIdColumn])},
                                    {"MedicationCode", MDVUtility.ToStr(drMedication[dsCDS.CDSMedication.MedicationCodeColumn])},
                                    {"rxnormid", MDVUtility.ToStr(drMedication[dsCDS.CDSMedication.MedicationRxNormIdColumn])}
                                };
                                lstMedications.Add(medicationValues);
                            }


                            ////////////temppppppppp
                            List<Dictionary<string, string>> lstVital = new List<Dictionary<string, string>>();
                            foreach (DSCDS.CDSVitalsRow drVital in dsCDS.CDSVitals.Rows)
                            {
                                var vitalsValues = new Dictionary<string, string>
                                {
                                    {"CDSVitalsId", MDVUtility.ToStr( drVital[dsCDS.CDSVitals.CDSVitalsIdColumn])},
                                    { "CDSVitalsLogic", MDVUtility.ToStr( drVital[dsCDS.CDSVitals.VitalsLogicColumn])},
                                    {"CDSVitalType",MDVUtility.ToStr( drVital[dsCDS.CDSVitals.VitalTypeColumn])},
                                    {"VitalLogicalOperator", MDVUtility.ToStr( drVital[dsCDS.CDSVitals.VitalLogicalOperatorColumn])},
                                    {"VitalValue",MDVUtility.ToStr( drVital[dsCDS.CDSVitals.VitalValueColumn])},
                                    {"VitalValueFrom", MDVUtility.ToStr( drVital[dsCDS.CDSVitals.VitalValueFromColumn])},
                                    {"VitalValueTo",MDVUtility.ToStr( drVital[dsCDS.CDSVitals.VitalValueToColumn])},
                                    {"VitalUnit", MDVUtility.ToStr( drVital[dsCDS.CDSVitals.VitalUnitColumn])}
                                };
                                lstVital.Add(vitalsValues);
                            }
                            //////////////


                            ////Start 14-05-2016 Ahmad Raza for CDS Lab Result
                            List<Dictionary<string, string>> lstLabResult = new List<Dictionary<string, string>>();
                            foreach (DataRow drLabResult in dsCDS.Tables[dsCDS.CDSLabResult.TableName].Rows)
                            {
                                var labResultValues = new Dictionary<string, string>
                                {
                                    {"LabResults", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.LabResultOperatorColumn])},
                                    {"LabResultName", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.LabResultNameColumn])},
                                    {"LabTestAttributeId", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.AttributeIdColumn])},
                                    {"labResultId", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.CDSLabResultIdColumn])},
                                    {"LabId", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.LabIdColumn])},
                                    {"LabResultLogicalOperator", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.LabResultLogicalOperatorColumn])},
                                    {"LabResultValue", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.LabResultValueColumn])},
                                    {"LabResultValueFrom", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.LabResultValueFromColumn])},
                                    {"LabResultValueTo", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.LabResultValueToColumn])},
                                    {"LabTestId", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.TestIdColumn])},
                                    {"LabName", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.LabNameColumn])},
                                    {"TestName", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.TestNameColumn])},
                                    {"AttributeName", MDVUtility.ToStr(drLabResult[dsCDS.CDSLabResult.AttributeNameColumn])},
                                };
                                lstLabResult.Add(labResultValues);
                            }
                            ////End 14-05-2016 Ahmad Raza for CDS Lab Result
                            //Start//16-03-2016//Ahmad Raza//Logic to fill CDSAllergies
                            List<Dictionary<string, string>> lstAllergies = new List<Dictionary<string, string>>();
                            foreach (DataRow drAllergy in dsCDS.Tables[dsCDS.CDSAllergy.TableName].Rows)
                            {
                                var allergyValues = new Dictionary<string, string>
                                {
                                    {"AllergyId", MDVUtility.ToStr(drAllergy[dsCDS.CDSAllergy.AllergenIdColumn])},
                                    {"AllergyOperator", MDVUtility.ToStr(drAllergy[dsCDS.CDSAllergy.AllergyOperatorColumn])},
                                    {"Allergen", MDVUtility.ToStr(drAllergy[dsCDS.CDSAllergy.AllergenColumn])},
                                    {"AllergyForQuery", MDVUtility.ToStr(drAllergy[dsCDS.CDSAllergy.AllergyForQueryColumn])}
                                };
                                lstAllergies.Add(allergyValues);
                            }
                            //End//16-03-2016//Ahmad Raza//Logic to fill CDSAllergies

                            //Start//16-03-2016//Ahmad Raza//Logic to fill CDSProblems
                            List<Dictionary<string, string>> lstProblems = new List<Dictionary<string, string>>();
                            foreach (DataRow drProblem in dsCDS.Tables[dsCDS.CDSProblem.TableName].Rows)
                            {
                                var problemValues = new Dictionary<string, string>
                                {
                                    {"ProblemId", MDVUtility.ToStr(drProblem[dsCDS.CDSProblem.CDSProblemIdColumn])},
                                    {"ProblemOperator", MDVUtility.ToStr(drProblem[dsCDS.CDSProblem.ProblemOperatorColumn])},
                                    {"Problem", MDVUtility.ToStr(drProblem[dsCDS.CDSProblem.ProblemColumn])},
                                    {"ProblemForQuery", MDVUtility.ToStr(drProblem[dsCDS.CDSProblem.ProblemForQueryColumn])}
                                };
                                lstProblems.Add(problemValues);
                            }
                            List<Dictionary<string, string>> lstQuestionnaire = new List<Dictionary<string, string>>();
                            foreach (DataRow drQuestionnaire in dsCDS.Tables[dsCDS.CDSQuestionnaire.TableName].Rows)
                            {
                                var Questionnaire = new Dictionary<string, string>
                                {
                                    {"CDSQuestionnaireId", MDVUtility.ToStr(drQuestionnaire[dsCDS.CDSQuestionnaire.CDSQuestionnaireIdColumn])},
                                    {"QuestionnaireControlTypeId", MDVUtility.ToStr(drQuestionnaire[dsCDS.CDSQuestionnaire.QuestionnaireControlTypeIdColumn])},
                                    {"Description", MDVUtility.ToStr(drQuestionnaire[dsCDS.CDSQuestionnaire.DescriptionColumn])},
                                    {"dropDownValues", MDVUtility.ToStr(drQuestionnaire[dsCDS.CDSQuestionnaire.dropDownValuesColumn])}
                                };
                                lstQuestionnaire.Add(Questionnaire);
                            }


                            List<Dictionary<string, string>> lstInsurance = new List<Dictionary<string, string>>();
                            foreach (DataRow drInsurance in dsCDS.Tables[dsCDS.CDSInsurance.TableName].Rows)
                            {
                                var insuranceValues = new Dictionary<string, string>
                                {
                                    {"CDSInsuranceId", MDVUtility.ToStr(drInsurance[dsCDS.CDSInsurance.CDSInsuranceIdColumn])},
                                    {"InsuranceOperator", MDVUtility.ToStr(drInsurance[dsCDS.CDSInsurance.InsuranceOperatorColumn])},
                                    {"InsuranceName", MDVUtility.ToStr(drInsurance[dsCDS.CDSInsurance.InsuranceNameColumn])},
                                    {"InsurancePlanId", MDVUtility.ToStr(drInsurance[dsCDS.CDSInsurance.InsurancePlanIdColumn])}
                                };
                                lstInsurance.Add(insuranceValues);
                            }



                            //End//16-03-2016//Ahmad Raza//Logic to fill CDSProblems
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                CDSJSON = js.Serialize(keyValues),
                                CDSVitalsJSON = js.Serialize(lstVital),
                                CDSMedicationJSON = js.Serialize(lstMedications),
                                CDSLabResultJSON = js.Serialize(lstLabResult),
                                CDSAllergyJSON = js.Serialize(lstAllergies),
                                CDSProblemJSON = js.Serialize(lstProblems),
                                CDSInsuranceJSON = js.Serialize(lstInsurance),
                                CDSQuestionnaireJSON = js.Serialize(lstQuestionnaire)
                            };
                            //End 08-03-2016 Humaira Yousaf to load CDSVitals
                            //End 10-03-2016 Humaira Yousaf for CDS Medications
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {

                            var response = new
                            {
                                status = false,
                                Message = MDVCustomException.HumanReadableMessage(obj.Message),
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };

                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Module Name: insertUpdateCDS
        /// Author: Humaira Yousaf
        /// Created Date: 07-03-2016
        /// Description: Inserts/update CDS
        /// </summary>
        /// <param name="model" type="CDSModel">CDS model containing data</param>
        public string insertUpdateCDS(CDSModel model, string ruleTypeIds, string userRoleIds, string triggerLocationIds, string genderIds, string ethnicityIds, string raceIds, string languageIds, CDSVitalsModel vitalsModel)
        {
            try
            {
                DSCDS dsCDS = new DSCDS();
                if (model.CDSId == "")
                {
                    model.CDSId = "-1";
                }
                //Start 13-05-2016 Edit By Humaira Yousaf Bug# EMR-1050
                BLObject<DSCDS> obj = BLLClinicalObj.loadCDS(MDVUtility.ToInt32(model.CDSId), MDVUtility.ToByte(model.CDSActive == true ? "1" : "0"));
                //End 13-05-2016 Edit By Humaira Yousaf Bug# EMR-1050
                dsCDS = obj.Data;
                if (obj.Data != null)
                {
                    DSCDS.CDSRow cdsRow = null;
                    DSCDS.CDSRow[] cdsRows = (DSCDS.CDSRow[])dsCDS.CDS.Select(dsCDS.CDS.CDSIdColumn + "=" + MDVUtility.ToLINQFormatString(model.CDSId));
                    if (cdsRows.Length > 0)
                    {
                        cdsRow = cdsRows[0];
                    }
                    else
                    {
                        cdsRow = dsCDS.CDS.NewCDSRow();
                    }
                    if (cdsRow != null)
                    {
                        cdsRow.Title = model.CDSTitle;
                        cdsRow.Developer = model.CDSDeveloper;
                        cdsRow.FundingSource = model.CDSFundingSource;
                        cdsRow.ReferenceURL = model.CDSReferenceURL;
                        cdsRow.Release = model.CDSRelease;
                        if (!string.IsNullOrEmpty(model.CDSRevisionDate))
                        {
                            cdsRow.RevisionDate = MDVUtility.ToDateTime(model.CDSRevisionDate);
                        }
                        else
                        {
                            cdsRow[dsCDS.CDS.RevisionDateColumn] = DBNull.Value;
                        }
                        cdsRow.TriggerLocation = triggerLocationIds;
                        // cdsRow.TriggerLocation = model.CDSTriggerLocation;
                        cdsRow.Role = userRoleIds;
                        // cdsRow.Role = model.CDSUserRole;
                        //Start 08-03-2016 Humaira Yousaf for ruleTypes
                        cdsRow.RuleType = ruleTypeIds;
                        //End 08-03-2016 Humaira Yousaf for ruleTypes
                        // cdsRow.Gender = model.CDSSex;
                        cdsRow.Gender = genderIds;
                        // cdsRow.Ethnicity = model.CDSEthnicity;
                        cdsRow.Ethnicity = ethnicityIds;
                        // cdsRow.Race = model.CDSRace;
                        cdsRow.Race = raceIds;
                        cdsRow.Role = userRoleIds;
                        // cdsRow.Language = model.CDSLanguage;
                        cdsRow.Language = languageIds;
                        if (!string.IsNullOrEmpty(model.CDSReminderLength))
                            cdsRow.ReminderLength = MDVUtility.ToInt32(model.CDSReminderLength);
                        else
                            cdsRow[dsCDS.CDS.ReminderLengthColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.CDSReminderPeriod))
                            cdsRow.ReminderPeriodId = MDVUtility.ToInt32(model.CDSReminderPeriod);
                        else
                            cdsRow[dsCDS.CDS.ReminderPeriodIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.CDSRecursiveLength))
                            cdsRow.RecursiveLength = MDVUtility.ToInt32(model.CDSRecursiveLength);
                        else
                            cdsRow[dsCDS.CDS.RecursiveLengthColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.CDSRecursivePeriod))
                            cdsRow.RecursivePeriodId = MDVUtility.ToInt32(model.CDSRecursivePeriod);
                        else
                            cdsRow[dsCDS.CDS.RecursivePeriodIdColumn] = DBNull.Value;
                        cdsRow.IsRecursive = model.CDSRecursive;
                        cdsRow.ProblemListOperator = model.CDSProblemList;
                        cdsRow.AllergiesOperator = model.CDSAllergies;
                        cdsRow.MedicationsOperator = model.CDSMedications;
                        cdsRow.LabsOperator = model.CDSLabResults;
                        cdsRow.VitalsOperator = model.CDSVitals;
                        cdsRow.InsuranceOperator = model.CDSInsurance;
                        cdsRow.Comments = model.CDSAlertNote;
                        //Start 13-05-2016 Edit By Humaira Yousaf Bug# EMR-1050
                        cdsRow.IsActive = model.IsActive;
                        //End 13-05-2016 Edit By Humaira Yousaf Bug# EMR-1050
                        //Start 10-03-2016 Humaira Yousaf for CDS Age Condition
                        //Start 11-03-2016 Humaira Yousaf for CDS Age Condition
                        if (model.CDSAgeCondition != "")
                        {
                            int ageId = MDVUtility.ToInt32(model.CDSAgeCondition);
                            cdsRow.AgeConditionId = ageId;
                            if (ageId == 1 || ageId == 2 || ageId == 3)
                            {
                                cdsRow.FromAge = MDVUtility.ToInt32(model.CDSAgeValue);
                                cdsRow.ToAge = 0;
                            }
                            else if (ageId == 4 || ageId == 5)
                            {
                                cdsRow.ToAge = MDVUtility.ToInt32(model.CDSAgeValue);
                                cdsRow.FromAge = 0;
                            }
                            else if (ageId == 6)
                            {
                                cdsRow.FromAge = MDVUtility.ToInt32(model.CDSAgeFrom);
                                cdsRow.ToAge = MDVUtility.ToInt32(model.CDSAgeTo);
                            }
                        }
                        else
                        {
                            cdsRow[dsCDS.CDS.AgeConditionIdColumn] = DBNull.Value;
                            cdsRow[dsCDS.CDS.FromAgeColumn] = DBNull.Value;
                            cdsRow[dsCDS.CDS.ToAgeColumn] = DBNull.Value;
                        }
                        cdsRow.CDSQuery = createCDSQuery(model, genderIds, triggerLocationIds, userRoleIds, ruleTypeIds, ethnicityIds, raceIds, languageIds);
                        //cdsRow.UserRoleQuery = createCDSUserRoleQuery(userRoleIds);
                        //End 11-03-2016 Humaira Yousaf for CDS Age Condition
                        //End 10-03-2016 Humaira Yousaf for CDS Age Condition
                        if (cdsRows.Length == 0)
                        {
                            cdsRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsRow.CreatedOn = DateTime.Now;
                        }
                        cdsRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        cdsRow.ModifiedOn = DateTime.Now;
                        cdsRow.OrderSetIds = model.OrderSetIds;
                        cdsRow.QuestionnaireHTML = model.QuestionnaireHTML;
                        cdsRow.ProviderIds = model.ProviderIds;

                        if (cdsRows.Length < 1)
                        {
                            dsCDS.CDS.AddCDSRow(cdsRow);
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSCDS> objCDS = BLLClinicalObj.insertUpdateCDS(dsCDS);
                    dsCDS = objCDS.Data;

                    if (objCDS.Data != null)
                    {
                        //Start 08-03-2016 Humaira Yousaf to insert/update ruleTypes
                        long insertedCDSId = MDVUtility.ToInt64(dsCDS.Tables[dsCDS.CDS.TableName].Rows[0][dsCDS.CDS.CDSIdColumn.ColumnName]);
                        // if (model.IsVitalInsertUpdate == true)
                        // if((vitalsModel.SystolicTemplate != "") || (vitalsModel.Weight != "" || vitalsModel.Height != ""))
                        //  {
                        // string vitalsResponse = insertUpdateCDSVitals(MDVUtility.ToInt64(insertedCDSId), vitalsModel);
                        //  }

                        //Start 09-03-2016 Humaira Yousaf for CDS Medications
                        if (model.MedicationData != null && model.MedicationData.Count > 0)
                        {
                            string medicationResponse = insertUpdateCDSMedication(MDVUtility.ToInt64(insertedCDSId), model.MedicationData);
                        }
                        if (model.AllergyData != null && model.AllergyData.Count > 0)
                        {
                            string allergyResponse = insertUpdateCDSAllergy(MDVUtility.ToInt64(insertedCDSId), model.AllergyData);
                        }

                        if (model.ProblemData != null && model.ProblemData.Count > 0)
                        {
                            string problemResponse = insertUpdateCDSProblemList(MDVUtility.ToInt64(insertedCDSId), model.ProblemData);
                        }

                        if (model.LabResultData != null && model.LabResultData.Count > 0)
                        {
                            string labResultResponse = insertUpdateCDSLabResult(MDVUtility.ToInt64(insertedCDSId), model.LabResultData);
                        }
                        if (model.QuestionnaireData != null && model.QuestionnaireData.Count > 0)
                        {
                            string labResultResponse = insertUpdateCDSQuestionnaire(MDVUtility.ToInt64(insertedCDSId), model.QuestionnaireData);
                        }
                        if (model.VitalData != null && model.VitalData.Count > 0)
                        {
                            string vitalResponse = insertUpdateCDSVitals(MDVUtility.ToInt64(insertedCDSId), model.VitalData);
                        }
                        if (model.InsuranceData != null && model.InsuranceData.Count > 0)
                        {
                            string insuranceResponse = insertUpdateCDSInsurance(MDVUtility.ToInt64(insertedCDSId), model.InsuranceData);
                        }


                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Save_Message,
                            CDSId = insertedCDSId,
                        };
                        //End 08-03-2016 Humaira Yousaf to insert/update ruleTypes
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objCDS.Message.Contains("duplicate") ? "CDS Title already exists" : obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Method Name: updateCDStatus
        /// Author: Ahmad Raza
        /// Created Date: 15-03-2016
        /// Description: Updates status of CDS Alert
        /// </summary>
        /// <param name="model" type="CDSModel">CDS model containing data</param>
        public string updateCDStatus(CDSModel model)
        {
            try
            {
                DSCDS dsCDS = new DSCDS();

                BLObject<DSCDS> obj = BLLClinicalObj.loadCDSPatientStatus(MDVUtility.ToInt32(model.PatientId), MDVUtility.ToInt32(model.CDSPatientStatusId));
                dsCDS = obj.Data;
                if (obj.Data != null)
                {
                    DSCDS.CDSPatientStatusRow cdsRow = null;
                    DSCDS.CDSPatientStatusRow[] cdsRows = (DSCDS.CDSPatientStatusRow[])dsCDS.CDSPatientStatus.Select(dsCDS.CDSPatientStatus.PatientIdColumn + "=" + MDVUtility.ToLINQFormatString(model.PatientId));
                    if (cdsRows.Length > 0)
                    {
                        cdsRow = cdsRows[0];
                        cdsRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        cdsRow.ModifiedOn = DateTime.Now;
                    }
                    else
                    {
                        cdsRow = dsCDS.CDSPatientStatus.NewCDSPatientStatusRow();
                        cdsRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        cdsRow.CreatedOn = DateTime.Now;
                    }
                    if (cdsRow != null)
                    {
                        cdsRow.Status = model.CDSStatus;
                        cdsRow.QuestionnaireHTML = model.QuestionnaireHTML;
                        cdsRow.CDSId = MDVUtility.ToInt32(model.CDSId);
                        cdsRow.PatientId = MDVUtility.ToInt32(model.PatientId);
                    }
                    if (cdsRows.Length < 1)
                    {
                        dsCDS.CDSPatientStatus.AddCDSPatientStatusRow(cdsRow);
                    }
                    #region Database Insertion/Updation

                    //fixme
                    BLObject<DSCDS> objCDS = BLLClinicalObj.insertUpdateCDSPatientStatus(dsCDS);
                    dsCDS = objCDS.Data;

                    if (objCDS.Data != null)
                    {
                        long insertedCDSId = MDVUtility.ToInt64(dsCDS.Tables[dsCDS.CDSPatientStatus.TableName].Rows[0][dsCDS.CDSPatientStatus.CDSIdColumn.ColumnName]);

                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Save_Message,
                            CDSId = insertedCDSId,
                        };

                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objCDS.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string updateCDS(CDSModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.CDSId) > 0)
                {

                    DSCDS dsCDS = null;
                    BLObject<DSCDS> obj;

                    obj = BLLClinicalObj.loadCDS(MDVUtility.ToInt32(model.CDSId));
                    dsCDS = obj.Data;
                    foreach (DSCDS.CDSRow dr in dsCDS.Tables[dsCDS.CDS.TableName].Rows)
                    {
                        dr.QuestionnaireHTML = MDVUtility.ToStr(model.QuestionnaireHTML);
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        //end newly added
                    }
                    #region Database Updation
                    if (dsCDS.Tables[dsCDS.CDS.TableName].Rows.Count > 0)
                    {
                        BLObject<DSCDS> objUpdate = BLLClinicalObj.insertUpdateCDS(dsCDS);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = AppPrivileges.Update_Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Problem not found."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        /// <summary>
        /// Module Name: createCDSQuery
        /// Author: Humaira Yousaf
        /// Created Date: 07-03-2016
        /// Description: Creates CDS Query
        /// </summary>
        /// <param name="model" type="CDSModel">CDS model containing data</param>
        private string createCDSQuery(CDSModel model, string genderIds, string triggerLocationIds, string userRoleIds, string ruleTypeIds, string ethnicityIds, string raceIds, string languageIds)
        {
            try
            {
                //Start 11-03-2016 Humaira Yousaf for CDS Age Condition
                string cdsQuery = string.Empty;
                string genderCondition = "";
                if (!string.IsNullOrEmpty(genderIds))
                {
                    if (genderIds.Contains(','))
                    {
                        string[] genderIdsArr = genderIds.Split(',');
                        List<string> list = new List<string>();
                        foreach (var item in genderIdsArr)
                        {
                            if (MDVUtility.ToInt64(item) == 0)
                                list.Add("'Male'");
                            else if (MDVUtility.ToInt64(item) == 1)
                                list.Add("'Female'");
                            else if (MDVUtility.ToInt64(item) == 2)
                                list.Add("'Unknown'");

                        }
                        genderCondition = String.Join(",", list);
                    }
                    else
                    {
                        if (MDVUtility.ToInt64(genderIds) == 0)
                            genderCondition = "'Male'";
                        else if (MDVUtility.ToInt64(genderIds) == 1)
                            genderCondition = "'Female'";
                        else if (MDVUtility.ToInt64(genderIds) == 2)
                            genderCondition = "'Unknown'";
                    }
                }
                else
                    genderCondition = "'Male','Female','Unknown'";

                if (model.CDSAgeCondition != "")
                {
                    string ageQuery = string.Empty;
                    if (MDVUtility.ToInt32(model.CDSAgeCondition) == 1) //equal
                    {
                        ageQuery = "Age = " + model.CDSAgeValue;
                    }
                    else if (MDVUtility.ToInt32(model.CDSAgeCondition) == 2)
                    {
                        ageQuery = "Age > " + model.CDSAgeValue;
                    }
                    else if (MDVUtility.ToInt32(model.CDSAgeCondition) == 3)
                    {
                        ageQuery = "Age >= " + model.CDSAgeValue;
                    }
                    else if (MDVUtility.ToInt32(model.CDSAgeCondition) == 4)
                    {
                        ageQuery = "Age < " + model.CDSAgeValue;
                    }
                    else if (MDVUtility.ToInt32(model.CDSAgeCondition) == 5)
                    {
                        ageQuery = "Age <= " + model.CDSAgeValue;
                    }
                    else if (MDVUtility.ToInt32(model.CDSAgeCondition) == 6)
                    {
                        ageQuery = "( Age >= " + model.CDSAgeFrom + " or Age <= " + model.CDSAgeTo + ")";
                    }
                    //End 11-03-2016 Humaira Yousaf for CDS Age Condition


                    //Start//06-03-2016//Ahmad Raza///optimization of CDS Query
                    cdsQuery = (string.IsNullOrEmpty(genderCondition) ? "1=1" : string.Concat(" Gender in (" + genderCondition + ")")) + " and " + (string.IsNullOrEmpty(ethnicityIds) ? "1=1" : string.Concat("EthnicityId in(", ethnicityIds, ")")) + " and " + (string.IsNullOrEmpty(raceIds) ? "1=1" : string.Concat("strRaceIds in(", raceIds, ")")) + " and " + (string.IsNullOrEmpty(languageIds) ? "1=1" : string.Concat("PrefLanguageId in(", languageIds, ")")) + " and " + ageQuery;
                }
                else
                {
                    cdsQuery = (string.IsNullOrEmpty(genderCondition) ? "1=1" : string.Concat(" Gender in (" + genderCondition + ")")) + " and " + (string.IsNullOrEmpty(ethnicityIds) ? "1=1" : string.Concat("EthnicityId in(", ethnicityIds, ")")) + " and " + (string.IsNullOrEmpty(raceIds) ? "1=1" : string.Concat("strRaceIds in(", raceIds, ")")) + " and " + (string.IsNullOrEmpty(languageIds) ? "1=1" : string.Concat("PrefLanguageId in(", languageIds, ")"));
                    //cdsQuery = " Gender in (" + genderCondition + ") and " + "EthnicityId in(" + ethnicityIds + ") and " + "RaceId in(" + raceIds + ") and " + "PrefLanguageId in(" + languageIds + ")";
                }
                //End//06-03-2016//Ahmad Raza///optimization of CDS Query

                return cdsQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region CDSVitals
        /// <summary>
        /// Module Name: insertUpdateCDSVitals
        /// Author: Humaira Yousaf
        /// Created Date: 08-03-2016
        /// Description: Inserts/updates CDSVitals
        /// </summary>
        /// <param name="CDSId" type="long">CDSId</param>
        /// <param name="vitalsModel" type="CDSVitalsModel">model containing data</param>
        public string insertUpdateCDSVitals_DELETED(long CDSId, CDSVitalsModel vitalsModel)
        {
            try
            {
                DSCDS dsCDSVitals = new DSCDS();

                BLObject<DSCDS> obj = BLLClinicalObj.loadCDSVitals(CDSId, vitalsModel.CDSVitalsId);
                if (obj.Data != null)
                {
                    dsCDSVitals = obj.Data;
                    DSCDS.CDSVitalsRow cdsVitalsRow = null;
                    DSCDS.CDSVitalsRow[] cdsVitalsRows = (DSCDS.CDSVitalsRow[])dsCDSVitals.CDSVitals.Select(dsCDSVitals.CDSVitals.CDSIdColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(vitalsModel.CDSId));

                    if (cdsVitalsRows.Length > 0)
                    {
                        cdsVitalsRow = cdsVitalsRows[0];
                    }
                    else
                    {
                        cdsVitalsRow = dsCDSVitals.CDSVitals.NewCDSVitalsRow();
                    }

                    if (cdsVitalsRow != null)
                    {
                        cdsVitalsRow.CDSId = CDSId;
                        if (string.IsNullOrEmpty(vitalsModel.Height))
                            cdsVitalsRow[dsCDSVitals.CDSVitals.HeightColumn.ColumnName] = DBNull.Value;
                        else
                            cdsVitalsRow.Height = Math.Round(MDVUtility.Tofloat(vitalsModel.Height), 2);
                        if (string.IsNullOrEmpty(vitalsModel.Weight))
                            cdsVitalsRow[dsCDSVitals.CDSVitals.WeightColumn.ColumnName] = DBNull.Value;
                        else
                            cdsVitalsRow.Weight = Math.Round(MDVUtility.Tofloat(vitalsModel.Weight), 2);
                        if (string.IsNullOrEmpty(vitalsModel.BMI))
                            cdsVitalsRow[dsCDSVitals.CDSVitals.BMIColumn.ColumnName] = DBNull.Value;
                        else
                            cdsVitalsRow.BMI = Math.Round(MDVUtility.Tofloat(vitalsModel.BMI), 2);
                        if (string.IsNullOrEmpty(vitalsModel.SystolicTemplate))
                            cdsVitalsRow[dsCDSVitals.CDSVitals.SystolicColumn.ColumnName] = DBNull.Value;
                        else
                            cdsVitalsRow.Systolic = MDVUtility.ToByte(vitalsModel.SystolicTemplate);
                        if (string.IsNullOrEmpty(vitalsModel.DiastolicTemplate))
                            cdsVitalsRow[dsCDSVitals.CDSVitals.DiastolicColumn.ColumnName] = DBNull.Value;
                        else
                            cdsVitalsRow.Diastolic = MDVUtility.ToByte(vitalsModel.DiastolicTemplate);

                        //    cdsVitalsRow.VitalsQuery = createCDSVitalsQuery(vitalsModel);
                        cdsVitalsRow.Comments = vitalsModel.Comments;
                        cdsVitalsRow.IsActive = true;
                        if (cdsVitalsRows.Length == 0)
                        {
                            cdsVitalsRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsVitalsRow.CreatedOn = DateTime.Now;
                        }
                        cdsVitalsRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        cdsVitalsRow.ModifiedOn = DateTime.Now;

                        if (cdsVitalsRows.Length < 1)
                        {
                            dsCDSVitals.CDSVitals.AddCDSVitalsRow(cdsVitalsRow);
                        }

                        #region Database Insertion/Updation
                        BLObject<DSCDS> objVitals = BLLClinicalObj.insertUpdateCDSVitals(dsCDSVitals);
                        dsCDSVitals = objVitals.Data;

                        if (objVitals.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = AppPrivileges.Save_Message,
                                vitalsId = MDVUtility.ToInt64(dsCDSVitals.Tables[dsCDSVitals.CDSVitals.TableName].Rows[0][dsCDSVitals.CDSVitals.CDSVitalsIdColumn.ColumnName]),
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = obj.Message
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                        #endregion
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.No_Record_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string insertUpdateCDSVitals(long CDSId, List<CDSVitalsModel> vitalsList)
        {
            try
            {
                DSCDS dsCDSVitals = new DSCDS();
                BLObject<DSCDS> obj = BLLClinicalObj.loadCDSVitals(CDSId, 0);
                dsCDSVitals = obj.Data;

                if (obj.Data != null)
                {
                    int index = 0;
                    foreach (var vital in vitalsList)
                    {
                        DSCDS.CDSVitalsRow cdsVitalsRow = null;
                        DSCDS.CDSVitalsRow[] cdsVitalsRows = (DSCDS.CDSVitalsRow[])dsCDSVitals.CDSVitals.Select(dsCDSVitals.CDSVitals.CDSIdColumn + "=" + CDSId + " AND " + dsCDSVitals.CDSVitals.CDSVitalsIdColumn.ColumnName + "=" + vital.CDSVitalsId);

                        if (cdsVitalsRows.Length > 0)
                        {
                            cdsVitalsRow = cdsVitalsRows[0];
                        }
                        else
                        {
                            cdsVitalsRow = dsCDSVitals.CDSVitals.NewCDSVitalsRow();
                        }

                        if (cdsVitalsRow != null)
                        {
                            cdsVitalsRow.CDSId = CDSId;

                            cdsVitalsRow.VitalsLogic = vital.VitalsLogic;
                            cdsVitalsRow.VitalType = vital.VitalType;
                            cdsVitalsRow.VitalLogicalOperator = vital.VitalLogicalOperator;
                            cdsVitalsRow.VitalValue = vital.VitalValue;
                            cdsVitalsRow.VitalValueFrom = vital.VitalValueFrom;
                            cdsVitalsRow.VitalValueTo = vital.VitalValueTo;
                            cdsVitalsRow.VitalUnit = vital.VitalUnit;
                            cdsVitalsRow.IsActive = true;
                            if (cdsVitalsRows.Length == 0)
                            {
                                cdsVitalsRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                cdsVitalsRow.CreatedOn = DateTime.Now;
                            }
                            cdsVitalsRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsVitalsRow.ModifiedOn = DateTime.Now;
                            string currentRowQuery = createCDSVitalsQuery(vital, index);
                            cdsVitalsRow.VitalsQuery = currentRowQuery;

                            if (cdsVitalsRows.Length < 1)
                            {
                                dsCDSVitals.CDSVitals.AddCDSVitalsRow(cdsVitalsRow);
                            }
                            index++;
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSCDS> objVital = BLLClinicalObj.insertUpdateCDSVitals(dsCDSVitals);
                    dsCDSVitals = objVital.Data;

                    if (objVital.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Save_Message,
                            vitalId = MDVUtility.ToInt64(dsCDSVitals.CDSVitals.Rows[0][dsCDSVitals.CDSVitals.CDSVitalsIdColumn.ColumnName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string deleteCDSVital(string CDSVitalsId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CDSVitalsId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.deleteCDSVital(MDVUtility.ToStr(CDSVitalsId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }




        /// <summary>
        /// Module Name: createCDSVitalsQuery
        /// Author: Humaira Yousaf
        /// Created Date: 08-03-2016
        /// Description: Creates CDSVitals Query
        /// </summary>
        /// <param name="vitalsModel" type="CDSVitalsModel">model containing data</param>
        private string createCDSVitalsQuery(CDSVitalsModel vitalsModel, int index)
        {
            string value = string.Empty;
            string valueFrom = string.Empty;
            string valueTo = string.Empty;
            try
            {
                if (vitalsModel.VitalType == "Height")
                {
                    vitalsModel.VitalValue = string.IsNullOrEmpty(vitalsModel.VitalValue) ? vitalsModel.VitalValue : MDVUtility.convertFeetToInches(vitalsModel.VitalValue).ToString();
                    vitalsModel.VitalValueFrom = string.IsNullOrEmpty(vitalsModel.VitalValueFrom) ? vitalsModel.VitalValueFrom : MDVUtility.convertFeetToInches(vitalsModel.VitalValueFrom).ToString();
                    vitalsModel.VitalValueTo = string.IsNullOrEmpty(vitalsModel.VitalValueTo) ? vitalsModel.VitalValueTo : MDVUtility.convertFeetToInches(vitalsModel.VitalValueTo).ToString();
                }
                value = vitalsModel.VitalValue == null ? null : string.Format("{0:0.0}", MDVUtility.ToDouble(vitalsModel.VitalValue));
                valueFrom = vitalsModel.VitalValueFrom == null ? null : string.Format("{0:0.0}", MDVUtility.ToDouble(vitalsModel.VitalValueFrom));
                valueTo = vitalsModel.VitalValueTo == null ? null : string.Format("{0:0.0}", MDVUtility.ToDouble(vitalsModel.VitalValueTo));

                string cdsVitalsQuery = string.Empty;
                //   cdsVitalsQuery = (index == 0 ? "" : vitalsModel.VitalsLogic + " ") + (vitalsModel.VitalLogicalOperator == "6" ? (vitalsModel.VitalType + ">=" + valueFrom + " AND " + vitalsModel.VitalType + "<=" + valueTo) : vitalsModel.VitalType + getLogicalOperatorById(vitalsModel.VitalLogicalOperator) + value);

                cdsVitalsQuery = (index == 0 ? "" : vitalsModel.VitalsLogic + " ") + (vitalsModel.VitalLogicalOperator == "6" ? (vitalsModel.VitalType + ">=" + "'" + valueFrom + "'" + " AND " + vitalsModel.VitalType + "<=" + "'" + valueTo + "'") : vitalsModel.VitalType + getLogicalOperatorById(vitalsModel.VitalLogicalOperator) + "'" + value + "'");
                return cdsVitalsQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CDS Medication
        /// <summary>
        /// Module Name: insertUpdateCDSMedication
        /// Author: Humaira Yousaf
        /// Created Date: 09-03-2016
        /// Description: Inserts/Update CDS Medications
        /// </summary>
        /// <param name="CDSId" type="long">CDS Id</param>
        /// <param name="medicationList" type="List<CDSMedicationModel>">Medication List</param>
        public string insertUpdateCDSMedication(long CDSId, List<CDSMedicationModel> medicationList)
        {
            try
            {
                DSCDS dsCDSMedication = new DSCDS();

                BLObject<DSCDS> obj = BLLClinicalObj.loadCDSMedication(CDSId, 0);
                dsCDSMedication = obj.Data;

                if (obj.Data != null)
                {
                    int index = 0;
                    foreach (var medication in medicationList)
                    {
                        DSCDS.CDSMedicationRow cdsMedicationRow = null;
                        DSCDS.CDSMedicationRow[] cdsMedicationRows = (DSCDS.CDSMedicationRow[])dsCDSMedication.CDSMedication.Select(dsCDSMedication.CDSMedication.CDSIdColumn + "=" + CDSId + " AND " + dsCDSMedication.CDSMedication.DrugIdColumn.ColumnName + "=" + medication.DrugId);

                        if (cdsMedicationRows.Length > 0)
                        {
                            cdsMedicationRow = cdsMedicationRows[0];
                        }
                        else
                        {
                            cdsMedicationRow = dsCDSMedication.CDSMedication.NewCDSMedicationRow();
                        }

                        if (cdsMedicationRow != null)
                        {
                            cdsMedicationRow.CDSId = CDSId;
                            cdsMedicationRow.DrugId = MDVUtility.ToInt64(medication.DrugId);
                            cdsMedicationRow.Comments = medication.Comments;
                            cdsMedicationRow.MedicationOperator = medication.MedicationOperator;
                            cdsMedicationRow.MedicationCode = medication.MedicationCode;
                            cdsMedicationRow.IsActive = true;
                            if (cdsMedicationRows.Length == 0)
                            {
                                cdsMedicationRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                cdsMedicationRow.CreatedOn = DateTime.Now;
                            }
                            cdsMedicationRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsMedicationRow.ModifiedOn = DateTime.Now;
                            if (!string.IsNullOrEmpty(medication.rxnormid))
                                cdsMedicationRow.MedicationRxNormId = medication.rxnormid;
                            else
                                cdsMedicationRow[dsCDSMedication.CDSMedication.MedicationRxNormIdColumn] = DBNull.Value;
                            cdsMedicationRow.CDSMedicationQuery = createCDSMedicationQuery(medication, index);

                            if (cdsMedicationRows.Length < 1)
                            {
                                dsCDSMedication.CDSMedication.AddCDSMedicationRow(cdsMedicationRow);
                            }
                            index++;
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSCDS> objMedication = BLLClinicalObj.insertUpdateCDSMedication(dsCDSMedication);
                    dsCDSMedication = objMedication.Data;

                    if (objMedication.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Save_Message,
                            vitalsId = MDVUtility.ToInt64(dsCDSMedication.Tables[dsCDSMedication.CDSMedication.TableName].Rows[0][dsCDSMedication.CDSMedication.CDSMedicationIdColumn.ColumnName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public string insertUpdateCDSAllergy(long CDSId, List<CDSAllergyModel> allergyList)
        {
            try
            {
                DSCDS dsCDSAllergy = new DSCDS();

                BLObject<DSCDS> obj = BLLClinicalObj.loadCDSAllergy(CDSId, 0);
                dsCDSAllergy = obj.Data;

                if (obj.Data != null)
                {
                    int index = 0;
                    foreach (var allergy in allergyList)
                    {
                        DSCDS.CDSAllergyRow cdsAllergyRow = null;
                        DSCDS.CDSAllergyRow[] cdsAllergyRows = (DSCDS.CDSAllergyRow[])dsCDSAllergy.CDSAllergy.Select(dsCDSAllergy.CDSAllergy.CDSIdColumn + "=" + CDSId + " AND " + dsCDSAllergy.CDSAllergy.AllergenIdColumn.ColumnName + "=" + allergy.CDSAllergyId);

                        if (cdsAllergyRows.Length > 0)
                        {
                            cdsAllergyRow = cdsAllergyRows[0];
                        }
                        else
                        {
                            cdsAllergyRow = dsCDSAllergy.CDSAllergy.NewCDSAllergyRow();
                        }

                        if (cdsAllergyRow != null)
                        {
                            cdsAllergyRow.CDSId = CDSId;
                            cdsAllergyRow.Allergen = allergy.Allergen;
                            //   cdsAllergyRow.CDSAllergyId = MDVUtility.ToInt64(allergy.CDSAllergyId);
                            cdsAllergyRow.Comments = allergy.Comments;
                            cdsAllergyRow.AllergyOperator = allergy.AllergyOperator;
                            cdsAllergyRow.AllergyForQuery = allergy.AllergyForQuery;
                            cdsAllergyRow.IsActive = true;
                            if (cdsAllergyRows.Length == 0)
                            {
                                cdsAllergyRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                cdsAllergyRow.CreatedOn = DateTime.Now;
                            }
                            cdsAllergyRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsAllergyRow.ModifiedOn = DateTime.Now;
                            cdsAllergyRow.CDSAllergyQuery = createCDSAllergyQuery(allergy, index);

                            if (cdsAllergyRows.Length < 1)
                            {
                                dsCDSAllergy.CDSAllergy.AddCDSAllergyRow(cdsAllergyRow);
                            }
                            index++;
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSCDS> objAllergy = BLLClinicalObj.insertUpdateCDSAllergy(dsCDSAllergy);
                    dsCDSAllergy = objAllergy.Data;

                    if (objAllergy.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Save_Message,
                            allergyId = MDVUtility.ToInt64(dsCDSAllergy.Tables[dsCDSAllergy.CDSAllergy.TableName].Rows[0][dsCDSAllergy.CDSAllergy.CDSAllergyIdColumn.ColumnName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string insertUpdateCDSProblemList(long CDSId, List<CDSProblemListModel> problemList)
        {
            try
            {
                DSCDS dsCDSProblem = new DSCDS();

                BLObject<DSCDS> obj = BLLClinicalObj.loadCDSProblem(CDSId, 0);
                dsCDSProblem = obj.Data;

                if (obj.Data != null)
                {
                    int index = 0;
                    foreach (var problem in problemList)
                    {
                        DSCDS.CDSProblemRow cdsProblemRow = null;
                        DSCDS.CDSProblemRow[] cdsProblemRows = (DSCDS.CDSProblemRow[])dsCDSProblem.CDSProblem.Select(dsCDSProblem.CDSProblem.CDSIdColumn + "=" + CDSId + " AND " + dsCDSProblem.CDSProblem.CDSProblemIdColumn.ColumnName + "=" + problem.CDSProblemId);

                        if (cdsProblemRows.Length > 0)
                        {
                            cdsProblemRow = cdsProblemRows[0];
                        }
                        else
                        {
                            cdsProblemRow = dsCDSProblem.CDSProblem.NewCDSProblemRow();
                        }

                        if (cdsProblemRow != null)
                        {
                            cdsProblemRow.CDSId = CDSId;
                            cdsProblemRow.Problem = problem.Problem;
                            //  cdsProblemRow.CDSAllergyId = MDVUtility.ToInt64(allergy.CDSAllergyId);
                            cdsProblemRow.Comments = problem.Comments;
                            cdsProblemRow.ProblemOperator = problem.ProblemOperator;
                            cdsProblemRow.IsActive = true;
                            if (cdsProblemRows.Length == 0)
                            {
                                cdsProblemRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                cdsProblemRow.CreatedOn = DateTime.Now;
                            }
                            cdsProblemRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsProblemRow.ModifiedOn = DateTime.Now;
                            cdsProblemRow.ProblemForQuery = problem.ProblemForQuery;
                            cdsProblemRow.CDSProblemQuery = createCDSProblemQuery(problem, index);

                            if (cdsProblemRows.Length < 1)
                            {
                                dsCDSProblem.CDSProblem.AddCDSProblemRow(cdsProblemRow);
                            }
                            index++;
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSCDS> objAllergy = BLLClinicalObj.insertUpdateCDSProblem(dsCDSProblem);
                    dsCDSProblem = objAllergy.Data;

                    if (objAllergy.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Save_Message,
                            problemId = MDVUtility.ToInt64(dsCDSProblem.Tables[dsCDSProblem.CDSProblem.TableName].Rows[0][dsCDSProblem.CDSProblem.CDSProblemIdColumn.ColumnName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string insertUpdateCDSLabResult(long CDSId, List<CDSLabResultModel> labResultList)
        {
            try
            {
                DSCDS dsCDSLabResult = new DSCDS();

                BLObject<DSCDS> obj = BLLClinicalObj.loadCDSLabResult(CDSId, 0);
                dsCDSLabResult = obj.Data;

                if (obj.Data != null)
                {
                    string currentLabId = "";
                    string currentTestId = "";
                    bool attributeIsChanged = false;
                    bool labIsChanged = false;
                    bool testIsChanged = false;

                    int index = 0;
                    foreach (var labResult in labResultList)
                    {
                        DSCDS.CDSLabResultRow cdsProblemRow = null;
                        DSCDS.CDSLabResultRow[] cdsLabResultRows = (DSCDS.CDSLabResultRow[])dsCDSLabResult.CDSLabResult.Select(dsCDSLabResult.CDSLabResult.CDSIdColumn + "=" + CDSId + " AND " + dsCDSLabResult.CDSLabResult.CDSLabResultIdColumn.ColumnName + "=" + labResult.CDSLabResultId);

                        if (cdsLabResultRows.Length > 0)
                        {
                            cdsProblemRow = cdsLabResultRows[0];
                        }
                        else
                        {
                            cdsProblemRow = dsCDSLabResult.CDSLabResult.NewCDSLabResultRow();
                        }

                        if (cdsProblemRow != null)
                        {
                            cdsProblemRow.CDSId = CDSId;
                            cdsProblemRow.LabResultName = labResult.LabResultName;
                            cdsProblemRow.LabResultOperator = labResult.LabResultOperator;
                            cdsProblemRow.LabResultLogicalOperator = labResult.LabResultLogicalOperator;
                            cdsProblemRow.LabResultValue = labResult.LabResultValue;
                            cdsProblemRow.LabResultValueFrom = labResult.LabResultValueFrom;
                            cdsProblemRow.LabResultValueTo = labResult.LabResultValueTo;
                            cdsProblemRow.LabId = labResult.LabId;
                            cdsProblemRow.TestId = labResult.TestId;
                            cdsProblemRow.AttributeId = labResult.AttributeId;

                            cdsProblemRow.IsActive = true;
                            if (cdsLabResultRows.Length == 0)
                            {
                                cdsProblemRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                cdsProblemRow.CreatedOn = DateTime.Now;
                            }
                            cdsProblemRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsProblemRow.ModifiedOn = DateTime.Now;


                            if (index == 0)
                            {
                                currentLabId = labResult.LabId;
                                currentTestId = labResult.TestId;
                            }
                            else
                            {
                                // currentLabId = "";
                            }


                            if (index > 0)
                            {


                                if (currentLabId != labResult.LabId)
                                {

                                    labIsChanged = true;
                                    currentLabId = labResult.LabId;
                                }
                                else
                                {

                                    labIsChanged = false;
                                }

                                if (currentTestId != labResult.TestId)
                                {

                                    testIsChanged = true;
                                    currentTestId = labResult.TestId;
                                }
                                else
                                {

                                    testIsChanged = false;
                                }
                            }


                            if (index > 0 && !labIsChanged && !testIsChanged)
                            {
                                attributeIsChanged = true;
                            }
                            else
                            {
                                attributeIsChanged = false;
                            }

                            cdsProblemRow.CDSLabResultQuery = createCDSLabResultQuery(labResult, index, labIsChanged, testIsChanged, attributeIsChanged);

                            if (cdsLabResultRows.Length < 1)
                            {
                                dsCDSLabResult.CDSLabResult.AddCDSLabResultRow(cdsProblemRow);
                            }
                            index++;
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSCDS> objLabResult = BLLClinicalObj.insertUpdateCDSLabResult(dsCDSLabResult);
                    dsCDSLabResult = objLabResult.Data;

                    if (objLabResult.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Save_Message,
                            labResultId = MDVUtility.ToInt64(dsCDSLabResult.Tables[dsCDSLabResult.CDSLabResult.TableName].Rows[0][dsCDSLabResult.CDSLabResult.CDSLabResultIdColumn.ColumnName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Module Name: createCDSMedicationQuery
        /// Author: Humaira Yousaf
        /// Created Date: 10-03-2016
        /// Description: Creates CDS Medication Query
        /// </summary>
        /// <param name="medicationModel" type="List<CDSMedicationModel>">Medication Model</param>
        /// <param name="index" type="int">CDS Id</param>
        private string createCDSMedicationQuery(CDSMedicationModel medicationModel, int index)
        {
            try
            {
                string cdsMedicationQuery = string.Empty;
                if (index == 0)
                {
                    cdsMedicationQuery = " DrugId=" + medicationModel.DrugId + " OR MedicationName = '" + medicationModel.DrugDescription + "'" + (!string.IsNullOrEmpty(medicationModel.rxnormid) ? " OR RxnormID = '" + medicationModel.rxnormid + "'" : "");
                }
                else
                {
                    cdsMedicationQuery = medicationModel.MedicationOperator + " DrugId=" + medicationModel.DrugId + " OR MedicationName = '" + medicationModel.DrugDescription + "'" + (!string.IsNullOrEmpty(medicationModel.rxnormid) ? " OR RxnormID = '" + medicationModel.rxnormid + "'" : "");
                }

                return cdsMedicationQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        private string createCDSAllergyQuery(CDSAllergyModel allergyModel, int index)
        {
            try
            {
                string cdsAllergyQuery = string.Empty;
                if (allergyModel.CDSAllergyId > 0)
                {
                    if (index == 0)
                    {
                        cdsAllergyQuery = " AllergenId='" + allergyModel.CDSAllergyId + "' ";
                    }
                    else
                    {
                        cdsAllergyQuery = allergyModel.AllergyOperator + " AllergenId='" + allergyModel.CDSAllergyId + "' ";
                    }
                }
                return cdsAllergyQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string createCDSLabResultQuery(CDSLabResultModel labResultModel, int index, bool labIsChanged, bool testIsChanged, bool attributeIsChanged)
        {
            try
            {
                string cdsLabResultQuery = string.Empty;
                string labDelimiter = labIsChanged ? " ~ " : string.Empty;
                string testDelimiter = testIsChanged ? " : " : string.Empty;
                string attributeDelimiter = attributeIsChanged ? (labResultModel.LabResultOperator.ToLower() == "and" ? " & " : " | ") : string.Empty;
                string valuesComparisonQuery = string.Empty;
                string value = string.Empty;
                string valueFrom = string.Empty;
                string valueTo = string.Empty;


                value = labResultModel.LabResultValue == null ? null : string.Format("{0:0.0}", MDVUtility.ToDouble(labResultModel.LabResultValue));
                valueFrom = labResultModel.LabResultValueFrom == null ? null : string.Format("{0:0.0}", MDVUtility.ToDouble(labResultModel.LabResultValueFrom));
                valueTo = labResultModel.LabResultValueTo == null ? null : string.Format("{0:0.0}", MDVUtility.ToDouble(labResultModel.LabResultValueTo));


                cdsLabResultQuery = testDelimiter + attributeDelimiter + " LabTestId= " + labResultModel.TestId + " AND LabTestAttributeId= " + labResultModel.AttributeId;// + " AND ";


                //query for test values won't be created if logical operator is not selected
                if (!string.IsNullOrEmpty(labResultModel.LabResultLogicalOperator))
                {
                    //between operator is selected
                    if (labResultModel.LabResultLogicalOperator == "6")
                    {
                        //valueFrom and valueTo shouldn't be empty simultaneously
                        if (!string.IsNullOrEmpty(labResultModel.LabResultValueFrom) && !string.IsNullOrEmpty(labResultModel.LabResultValueTo))
                            valuesComparisonQuery = " AND Result >= " + valueFrom + " AND " + "Result <= " + valueTo;
                    }
                    // logical operator is = , > , >= , < , <=
                    else if (labResultModel.LabResultLogicalOperator == "1" && value == "0.0")
                    {
                        var val = labResultModel.LabResultValue == null ? null : labResultModel.LabResultValue;
                        if (!string.IsNullOrEmpty(val))
                            valuesComparisonQuery = " AND Result = like '%" + val + "%'";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(labResultModel.LabResultValue))
                            valuesComparisonQuery = " AND Result" + getLogicalOperatorById(labResultModel.LabResultLogicalOperator) + value;
                    }

                }

                cdsLabResultQuery += valuesComparisonQuery;
                return cdsLabResultQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string createCDSProblemQuery(CDSProblemListModel problemModel, int index)
        {
            try
            {
                string cdsProblemQuery = string.Empty;
                //  if (problemModel.CDSProblemId > 0)
                // {
                if (index == 0)
                {
                    cdsProblemQuery = " SNOMEDID='" + problemModel.ProblemForQuery + "' ";
                }
                else
                {
                    cdsProblemQuery = problemModel.ProblemOperator + " SNOMEDID='" + problemModel.ProblemForQuery + "' ";
                }
                //  }
                return cdsProblemQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string insertUpdateCDSQuestionnaire(long CDSId, List<CDSQuestionnaire> questionnaireList)
        {
            try
            {
                DSCDS dsCDSQuestionnaire = new DSCDS();

                BLObject<DSCDS> obj = BLLClinicalObj.loadCDSQuestionnaire(CDSId, 0);
                dsCDSQuestionnaire = obj.Data;

                if (obj.Data != null)
                {
                    int index = 0;
                    foreach (var questionnaire in questionnaireList)
                    {
                        DSCDS.CDSQuestionnaireRow cdsQuestionnaireRow = null;
                        DSCDS.CDSQuestionnaireRow[] cdsQuestionnaireRows = (DSCDS.CDSQuestionnaireRow[])dsCDSQuestionnaire.CDSQuestionnaire.Select(dsCDSQuestionnaire.CDSLabResult.CDSIdColumn + "=" + CDSId + " AND " + dsCDSQuestionnaire.CDSQuestionnaire.CDSQuestionnaireIdColumn.ColumnName + "=" + questionnaire.CDSQuestionnaireId);

                        if (cdsQuestionnaireRows.Length > 0)
                        {
                            cdsQuestionnaireRow = cdsQuestionnaireRows[0];
                        }
                        else
                        {
                            cdsQuestionnaireRow = dsCDSQuestionnaire.CDSQuestionnaire.NewCDSQuestionnaireRow();
                        }

                        if (cdsQuestionnaireRow != null)
                        {
                            cdsQuestionnaireRow.CDSId = CDSId;
                            cdsQuestionnaireRow.Description = questionnaire.Description;
                            cdsQuestionnaireRow.dropDownValues = questionnaire.dropDownValues;
                            //  cdsProblemRow.CDSAllergyId = MDVUtility.ToInt64(allergy.CDSAllergyId);
                            cdsQuestionnaireRow.QuestionnaireControlTypeId = MDVUtility.ToInt64(questionnaire.QuestionnaireControlTypeId);

                            if (cdsQuestionnaireRows.Length == 0)
                            {
                                cdsQuestionnaireRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                cdsQuestionnaireRow.CreatedOn = DateTime.Now;
                            }
                            cdsQuestionnaireRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsQuestionnaireRow.ModifiedOn = DateTime.Now;
                            if (cdsQuestionnaireRows.Length < 1)
                            {
                                dsCDSQuestionnaire.CDSQuestionnaire.AddCDSQuestionnaireRow(cdsQuestionnaireRow);
                            }
                            index++;
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSCDS> objQuestionnaire = BLLClinicalObj.insertUpdateCDSQuestionnaire(dsCDSQuestionnaire);
                    dsCDSQuestionnaire = objQuestionnaire.Data;

                    if (objQuestionnaire.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Save_Message,
                            CDSQuestionnaireId = MDVUtility.ToInt64(dsCDSQuestionnaire.Tables[dsCDSQuestionnaire.CDSQuestionnaire.TableName].Rows[0][dsCDSQuestionnaire.CDSQuestionnaire.CDSQuestionnaireIdColumn.ColumnName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Method Name: deleteCDSProblemList
        /// Author: Ahmad Raza
        /// Created Date: 16-03-2016
        /// Description: this function deletes CDS Problem List
        /// </summary>
        /// <param name="cdsProblemListId" type="string">cdsProblemListId to be deleted</param>
        public string deleteCDSProblemList(string cdsProblemListId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(cdsProblemListId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.deleteCDSProblemList(MDVUtility.ToStr(cdsProblemListId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string deleteCDSQuestionnaire(string cdsQuestionnairetId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(cdsQuestionnairetId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.deleteCDSQuestionnaire(MDVUtility.ToStr(cdsQuestionnairetId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Method Name: deleteCDSAllergy
        /// Author: Ahmad Raza
        /// Created Date: 16-03-2016
        /// Description: this function deletes CDS Allergy
        /// </summary>
        /// <param name="cdsAllergyId" type="string">cdsAllergyId to be deleted</param>
        public string deleteCDSAllergy(string cdsAllergyId, string CDSId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(cdsAllergyId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.deleteCDSAllergy(MDVUtility.ToStr(cdsAllergyId), MDVUtility.ToStr(CDSId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Method Name: deleteCDSMedication
        /// Author: Ahmad Raza
        /// Created Date: 16-03-2016
        /// Description: this function deletes CDS Medication
        /// </summary>
        /// <param name="cdsMedicationId" type="string">cdsMedicationId to be deleted</param>
        public string deleteCDSMedication(string cdsMedicationId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(cdsMedicationId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.deleteCDSMedication(MDVUtility.ToStr(cdsMedicationId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }


        public string activeInActiveCDS(CDSModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.CDSId) > 0)
                {

                    DSCDS dsCDS = null;
                    BLObject<DSCDS> obj;
                    byte isActive = 1;
                    if (model.IsActive == true)
                    {
                        isActive = 0;
                    }
                    else if (model.IsActive == false)
                    {
                        isActive = 1;
                    }

                    obj = BLLClinicalObj.loadCDS(MDVUtility.ToInt32(model.CDSId), isActive, 1, 2000);
                    dsCDS = obj.Data;
                    foreach (DSCDS.CDSRow dr in dsCDS.Tables[dsCDS.CDS.TableName].Rows)
                    {
                        dr.IsActive = MDVUtility.ToStr(model.IsActive) == "True" ? true : false;

                        //dr.IsActive = false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        //end newly added
                    }
                    #region Database Updation
                    if (dsCDS.Tables[dsCDS.CDS.TableName].Rows.Count > 0)
                    {
                        BLObject<DSCDS> objUpdate = BLLClinicalObj.insertUpdateCDS(dsCDS);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = AppPrivileges.Update_Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Problem not found."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }




        public string deleteCDS(CDSModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.CDSId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {

                    BLObject<string> obj = BLLClinicalObj.deleteCDS(MDVUtility.ToStr(model.CDSId), Convert.ToByte(model.IsActive));
                    if (obj.Data == "")
                    {

                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string GetCDSOrderSetAndPriviliges(CDSModel model)
        {
            try
            {
                var strJsonData = AppPrivileges.GetMultipleFormSecurity(model.PrivilegeData);
                var cds_data = model.IsLoadCDS ? loadCDSOrderSet(model) : string.Empty;

                var response = new
                {
                    status = true,
                    CDSOrderSet_JSON = cds_data,
                    PrivilegeData_JSON = strJsonData
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string loadCDSOrderSet(CDSModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.CDSId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.No_Record_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {

                    BLObject<DSCDS> obj = BLLClinicalObj.loadCDSOrderSet(MDVUtility.ToLong(model.PatientId), model.CDSId, model.NoteId);
                    if (obj.Data != null)
                    {
                        DSCDS ds = obj.Data;

                        var response = new
                        {
                            status = true,
                            OrderSetCount = ds.Tables[ds.CDSOrderSet.TableName].Rows.Count,
                            CDSOrderSet_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.CDSOrderSet.TableName]),
                            CDSNoteOrderSetJSON = MDVUtility.JSON_DataTable(ds.Tables[ds.CDSNoteOrderSet.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Method Name: deleteCDSLabResult
        /// Author: Ahmad Raza
        /// Created Date: 16-05-2016
        /// Description: this function deletes CDS LabResult
        /// </summary>
        /// <param name="cdsLabResultId" type="string">cdsLabResultId to be deleted</param>
        public string deleteCDSLabResult(string cdsId, string testId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(cdsId)) && string.IsNullOrEmpty(MDVUtility.ToStr(testId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.deleteCDSLabResult(cdsId, testId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string loadCDSAgainstPatient_Obsolete(CDSModel model)
        {
            try
            {
                DSCDS dsCDS = null;
                BLObject<DSCDS> obj;
                obj = BLLClinicalObj.loadCDSAgainstPatient_Obsolete(MDVUtility.ToInt64(model.CDSIDs), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), MDVUtility.ToInt64(model.PatientId));
                dsCDS = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        CDSCount = dsCDS.Tables[dsCDS.CDSForAlerts.TableName].Rows.Count,
                        CDSLoad_JSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSForAlerts.TableName]),
                        iTotalDisplayRecords = (dsCDS.CDSForAlerts.Rows.Count > 0) ? dsCDS.CDSForAlerts.Rows[0][dsCDS.CDSForAlerts.RecordCountColumn.ColumnName] : 0,
                    };
                    return (JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        CDSCount = 0,
                        Message = obj.Message
                    };

                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string loadCDSAgainstPatient(CDSModel model)
        {
            try
            {
                List<CDSForAlerts> cdsforAlertsList = BLLClinicalObj.loadCDSAgainstPatient(MDVUtility.ToInt64(model.CDSIDs), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), MDVUtility.ToInt64(model.PatientId));

                if (cdsforAlertsList != null)
                {
                    var response = new
                    {
                        status = true,
                        CDSCount = cdsforAlertsList.Count,
                        CDSLoad_JSON = JsonConvert.SerializeObject(cdsforAlertsList),
                        iTotalDisplayRecords = (cdsforAlertsList.Count > 0) ? cdsforAlertsList[0].RecordCount : "0",
                    };
                    return (JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        CDSCount = 0,
                        Message = "No data found"
                    };

                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Method Name: updateCDStatus
        /// Author: Ahmad Raza
        /// Created Date: 15-03-2016
        /// Description: Updates status of CDS Alert
        /// </summary>
        /// <param name="model" type="CDSModel">CDS model containing data</param>
        public string updateCDStatusForSelectedAlert(CDSModel model)
        {
            try
            {
                DSCDS dsCDS = new DSCDS();

                BLObject<DSCDS> obj = BLLClinicalObj.loadCDSPatientStatus(MDVUtility.ToInt32(model.PatientId), MDVUtility.ToInt32(model.CDSPatientStatusId));
                dsCDS = obj.Data;
                if (obj.Data != null)
                {
                    DSCDS.CDSPatientStatusRow cdsRow = null;
                    DSCDS.CDSPatientStatusRow[] cdsRows = (DSCDS.CDSPatientStatusRow[])dsCDS.CDSPatientStatus.Select(dsCDS.CDSPatientStatus.PatientIdColumn + "=" + MDVUtility.ToLINQFormatString(model.PatientId));
                    if (cdsRows.Length > 0)
                    {
                        cdsRow = cdsRows[0];
                    }
                    else
                    {
                        cdsRow = dsCDS.CDSPatientStatus.NewCDSPatientStatusRow();
                    }
                    if (cdsRow != null)
                    {
                        cdsRow.Status = model.CDSStatus;
                        cdsRow.QuestionnaireHTML = model.QuestionnaireHTML;
                        cdsRow.CDSId = MDVUtility.ToInt32(model.CDSId);
                        cdsRow.PatientId = MDVUtility.ToInt32(model.PatientId);
                    }
                    if (cdsRows.Length < 1)
                    {
                        dsCDS.CDSPatientStatus.AddCDSPatientStatusRow(cdsRow);
                    }
                    #region Database Insertion/Updation

                    BLObject<DSCDS> objCDS = BLLClinicalObj.insertUpdateCDSPatientStatus(dsCDS);
                    dsCDS = objCDS.Data;

                    if (objCDS.Data != null)
                    {
                        long insertedCDSId = MDVUtility.ToInt64(dsCDS.Tables[dsCDS.CDSPatientStatus.TableName].Rows[0][dsCDS.CDSPatientStatus.CDSIdColumn.ColumnName]);

                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Save_Message,
                            CDSId = insertedCDSId,
                        };

                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objCDS.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string getLogicalOperatorById(string id)
        {
            string logicalOperator = "";
            switch (id)
            {
                case "1":
                    logicalOperator = "=";
                    break;
                case "2":
                    logicalOperator = ">";
                    break;
                case "3":
                    logicalOperator = ">=";
                    break;
                case "4":
                    logicalOperator = "<";
                    break;
                case "5":
                    logicalOperator = "<=";
                    break;
                case "6":
                    logicalOperator = "between";
                    break;
                default:
                    break;
            }
            return " " + logicalOperator + " ";
        }
        public string insertCDStatus(CDSModel model, BLObject<DSCDS> obj)
        {
            try
            {
                bool hasNewAlert = false;
                DSCDS dsCDS = new DSCDS();
                dsCDS = obj.Data;
                if (obj.Data != null)
                {
                    DSCDS.CDSPatientStatusRow cdsRow = null;
                    DSCDS.CDSPatientStatusRow[] cdsRows = (DSCDS.CDSPatientStatusRow[])dsCDS.CDSPatientStatus.Select(dsCDS.CDSPatientStatus.PatientIdColumn + "=" + MDVUtility.ToLINQFormatString(model.PatientId) + " AND " + dsCDS.CDSPatientStatus.CDSIdColumn + "=" + model.CDSId);
                    int index = 0;
                    if (cdsRows.Length > 0)
                    {
                        foreach (DSCDS.CDSPatientStatusRow row in cdsRows)
                        {
                            cdsRow = null;
                            DSCDS.CDSPatientStatusRow[] cdsRowsSub = (DSCDS.CDSPatientStatusRow[])dsCDS.CDSPatientStatus.Select(dsCDS.CDSPatientStatus.PatientIdColumn + "=" + MDVUtility.ToLINQFormatString(model.PatientId) + " AND " + dsCDS.CDSPatientStatus.CDSPatientStatusIdColumn + "=" + row.CDSPatientStatusId);
                            if (cdsRowsSub.Length > 0)
                            {
                                //entry already exists in CDSPatientStatus table
                                cdsRow = cdsRowsSub[0];
                            }
                            else
                            {
                                cdsRow = dsCDS.CDSPatientStatus.NewCDSPatientStatusRow();
                                cdsRow.Status = "Due";
                                cdsRow.CDSPatientStatusId = -1;
                                cdsRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                cdsRow.CreatedOn = DateTime.Now;
                                dsCDS.CDSPatientStatus.AddCDSPatientStatusRow(cdsRow);
                                hasNewAlert = true;
                            }
                            if (cdsRow != null)
                            {
                                cdsRow.CDSId = MDVUtility.ToInt32(model.CDSId);
                                cdsRow.PatientId = MDVUtility.ToInt32(model.PatientId);

                                if (cdsRow["EndDate"] != DBNull.Value && cdsRow.EndDate <= DateTime.Now)
                                {
                                    cdsRow.ModifiedOn = DateTime.Now;
                                    cdsRow.Status = "Due";
                                    hasNewAlert = true;
                                }
                            }
                            index++;
                        }
                    }
                    else
                    {
                        cdsRow = dsCDS.CDSPatientStatus.NewCDSPatientStatusRow();
                        cdsRow.CDSId = MDVUtility.ToInt32(model.CDSId);
                        cdsRow.PatientId = MDVUtility.ToInt32(model.PatientId);
                        cdsRow.Status = "Due";
                        cdsRow.CDSPatientStatusId = -1;

                        cdsRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        cdsRow.CreatedOn = DateTime.Now;

                        dsCDS.CDSPatientStatus.AddCDSPatientStatusRow(cdsRow);
                        hasNewAlert = true;
                    }
                    #region Database Insertion/Updation
                    if (hasNewAlert == true)
                    {
                        BLObject<DSCDS> objCDS = BLLClinicalObj.insertUpdateCDSPatientStatus(dsCDS);
                        dsCDS = objCDS.Data;

                        if (objCDS.Data != null)
                        {
                            long insertedCDSId = MDVUtility.ToInt64(dsCDS.Tables[dsCDS.CDSPatientStatus.TableName].Rows[0][dsCDS.CDSPatientStatus.CDSIdColumn.ColumnName]);

                            var response = new
                            {
                                status = true,
                                message = AppPrivileges.Save_Message,
                                CDSId = insertedCDSId,
                            };

                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objCDS.Message
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = "No record to insert"
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string loadCDSForAlerts(CDSModel model, string lstIDs, string isPopup)
        {
            try
            {
                Int64 currentPatientId = MDVUtility.ToInt64(model.PatientId);
                DSCDS dsCDS = null;
                BLObject<DSCDS> obj;
                obj = BLLClinicalObj.loadCDSAgainstPatient_Obsolete(MDVUtility.ToInt32(model.CDSIDs), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), MDVUtility.ToInt64(model.PatientId));
                dsCDS = obj.Data;

                if (obj.Data != null)
                {
                    DSCDS.CDSForAlertsRow[] arrCDSRows = (DSCDS.CDSForAlertsRow[])dsCDS.Tables[dsCDS.CDSForAlerts.TableName].Select(dsCDS.CDSForAlerts.CDSIdColumn + " in (" + lstIDs + ") and (" + dsCDS.CDSForAlerts.StatusColumn + "  like '%Due%' or " + dsCDS.CDSForAlerts.StatusColumn + " is null)");
                    List<Dictionary<string, string>> lstCDSKeyValues = new List<Dictionary<string, string>>();
                    var cdsKeyValues = new Dictionary<string, string> { { "", "" } };

                    if (arrCDSRows.Length > 0)
                    {
                        MDVisionLookups ob = new MDVisionLookups();
                        var ethnicityNames = ob.GetEthnicity("true");
                        var raceNames = ob.GetRace("true");


                        foreach (var dr in arrCDSRows)
                        {
                            //Start//06-03-2016//Ahmad Raza//Logic to get Rac and Ethnicity names on the base of comma separated IDs
                            string CommaSeparatedIds = MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.EthnicityColumn.ColumnName]);
                            string CommaSeparatedRaceIds = MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.RaceColumn.ColumnName]);
                            string[] ethnicityIds = CommaSeparatedIds.Split(',');
                            string[] raceIds = CommaSeparatedRaceIds.Split(',');
                            List<Ethnicity> lstEth = JsonConvert.DeserializeObject<List<Ethnicity>>(ethnicityNames);
                            List<Race> lstRace = JsonConvert.DeserializeObject<List<Race>>(raceNames);
                            List<string> lstMatchingEthnicity = new List<string>();
                            List<string> lstMatchingRace = new List<string>();
                            foreach (var name in lstEth)
                            {
                                foreach (string id in ethnicityIds)
                                {
                                    if (id == name.Value && id != "")
                                    {
                                        lstMatchingEthnicity.Add(name.Name);
                                    }
                                }
                            }

                            string EthnicityNames = string.Join(",", lstMatchingEthnicity);
                            foreach (var name in lstRace)
                            {
                                foreach (string id in raceIds)
                                {
                                    if (id == name.Value && id != "")
                                    {
                                        lstMatchingRace.Add(name.Name);
                                    }
                                }
                            }
                            string RaceNames = string.Join(",", lstMatchingRace);
                            //End//06-03-2016//Ahmad Raza//Logic to get Rac and Ethnicity names on the base of comma separated IDs


                            cdsKeyValues = new Dictionary<string, string>
                        {
                            { "CDSId",  MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.CDSIdColumn.ColumnName])},
                            { "Title",   MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.TitleColumn.ColumnName])},
                            { "Comments", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.CommentsColumn.ColumnName])},
                            { "IsActive", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.IsActiveColumn.ColumnName])},
                            { "Ethnicity", MDVUtility.ToStr(EthnicityNames)},
                            { "Status", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.StatusColumn.ColumnName])},
                            { "Race", MDVUtility.ToStr(RaceNames)},
                            { "TriggerLocation", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.TriggerLocationColumn.ColumnName])},
                            { "Gender", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.GenderColumn.ColumnName])},
                            { "RuleTypeDescription", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.RuleTypeDesColumn.ColumnName])},
                            { "txtDeveloper", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.DeveloperColumn.ColumnName])},
                            { "txtFundingSource", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.FundingSourceColumn.ColumnName])},
                            { "txtRelease", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.ReleaseColumn.ColumnName])},
                            { "txtReferenceUrl", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.ReferenceURLColumn.ColumnName])},
                            { "CDSAgeCondition", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.AgeConditionIdColumn.ColumnName])},
                            { "CDSAgeFrom", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.FromAgeColumn.ColumnName])},
                            { "CDSAgeTo", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.ToAgeColumn.ColumnName])},
                            { "CDSPatientStatusId", MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.CDSPatientStatusIdColumn.ColumnName])},
                        };
                            CommaSeparatedIds = MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.EthnicityColumn.ColumnName]);
                            CommaSeparatedRaceIds = MDVUtility.ToStr(dr[dsCDS.CDSForAlerts.RaceColumn.ColumnName]);
                            lstCDSKeyValues.Add(cdsKeyValues);
                        }
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        CDSCount = arrCDSRows.Length,
                        CDSLoad_JSON = js.Serialize(lstCDSKeyValues),
                        iTotalDisplayRecords = (arrCDSRows.Length > 0) ? arrCDSRows.Length : 0,
                    };
                    return (JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        CDSCount = 0,
                        Message = obj.Message
                    };

                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string searchCDS(CDSModel model, string lstIDs, string isPopup)
        {
            try
            {

                DSPatient dsPatient = new DSPatient();
                Int64 currentPatientId = MDVUtility.ToInt64(model.PatientId);
                BLObject<DSPatient> objpatient = new BLObject<DSPatient>();
                if (currentPatientId > 0)
                {
                    objpatient = BLLPatientObj.FillPatientById(currentPatientId);
                    dsPatient = objpatient.Data;
                }
                DSVitals dsVitals = new DSVitals();
                DSAllergies dsAllergies = new DSAllergies();
                DSProblemLists dsProblemList = new DSProblemLists();
                DSClinicalMedication dsMedications = new DSClinicalMedication();
                DSLabResult dsLabResult = new DSLabResult();
                DSCDS dsconditions = new DSCDS();
                BLObject<DSCDS> conditions = new BLObject<DSCDS>();
                if (currentPatientId > 0)
                {
                    conditions = BLLClinicalObj.cdsActualConditions(true, MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), MDVUtility.ToInt64(""), currentPatientId, "", "", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    dsconditions = conditions.Data;
                }

                byte isActive = Convert.ToByte(model.IsActive);
                if (model.commandType.ToLower() == "search_cdsforalert")
                {
                    model.CDSIDs = model.CDSId;
                    isActive = 1;
                }

                DSCDS dsCDS = null;
                BLObject<DSCDS> obj;
                obj = BLLClinicalObj.searchCDS(MDVUtility.ToInt32(model.CDSIDs), isActive, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", "", MDVUtility.ToInt64(model.PatientId));
                dsCDS = obj.Data;

                if (obj.Data != null)
                {
                    //Start//10-03-2016//Ahmad Raza//Logic to show CDS Search Grid with refined IDs
                    if (isPopup == "Yes")
                    {
                        DSCDS.CDSRow[] arrCDSRows = (DSCDS.CDSRow[])dsCDS.Tables[dsCDS.CDS.TableName].Select(dsCDS.CDS.CDSIdColumn + " in (" + lstIDs + ") and (" + dsCDS.CDS.StatusColumn + "  like '%Due%' or " + dsCDS.CDS.StatusColumn + " is null)");
                        List<Dictionary<string, string>> lstCDSKeyValues = new List<Dictionary<string, string>>();
                        var cdsKeyValues = new Dictionary<string, string> { { "", "" } };

                        if (arrCDSRows.Length > 0)
                        {
                            MDVisionLookups ob = new MDVisionLookups();
                            var ethnicityNames = ob.GetEthnicity("true");
                            var raceNames = ob.GetRace("true");


                            foreach (var dr in arrCDSRows)
                            {
                                //Start//06-03-2016//Ahmad Raza//Logic to get Rac and Ethnicity names on the base of comma separated IDs
                                string CommaSeparatedIds = MDVUtility.ToStr(dr[dsCDS.CDS.EthnicityColumn.ColumnName]);
                                string CommaSeparatedRaceIds = MDVUtility.ToStr(dr[dsCDS.CDS.RaceColumn.ColumnName]);
                                string[] ethnicityIds = CommaSeparatedIds.Split(',');
                                string[] raceIds = CommaSeparatedRaceIds.Split(',');
                                List<Ethnicity> lstEth = JsonConvert.DeserializeObject<List<Ethnicity>>(ethnicityNames);
                                List<Race> lstRace = JsonConvert.DeserializeObject<List<Race>>(raceNames);
                                List<string> lstMatchingEthnicity = new List<string>();
                                List<string> lstMatchingRace = new List<string>();
                                foreach (var name in lstEth)
                                {
                                    foreach (string id in ethnicityIds)
                                    {
                                        if (id == name.Value && id != "")
                                        {
                                            lstMatchingEthnicity.Add(name.Name);
                                        }
                                    }
                                }

                                string EthnicityNames = string.Join(",", lstMatchingEthnicity);
                                foreach (var name in lstRace)
                                {
                                    foreach (string id in raceIds)
                                    {
                                        if (id == name.Value && id != "")
                                        {
                                            lstMatchingRace.Add(name.Name);
                                        }
                                    }
                                }
                                string RaceNames = string.Join(",", lstMatchingRace);
                                //End//06-03-2016//Ahmad Raza//Logic to get Rac and Ethnicity names on the base of comma separated IDs


                                cdsKeyValues = new Dictionary<string, string>
                        {
                            { "CDSId",  MDVUtility.ToStr(dr[dsCDS.CDS.CDSIdColumn.ColumnName])},
                            { "Title",   MDVUtility.ToStr(dr[dsCDS.CDS.TitleColumn.ColumnName])},
                            { "Comments", MDVUtility.ToStr(dr[dsCDS.CDS.CommentsColumn.ColumnName])},
                            { "IsActive", MDVUtility.ToStr(dr[dsCDS.CDS.IsActiveColumn.ColumnName])},
                            { "Ethnicity", MDVUtility.ToStr(EthnicityNames)},
                            { "Status", MDVUtility.ToStr(dr[dsCDS.CDS.StatusColumn.ColumnName])},
                            { "Race", MDVUtility.ToStr(RaceNames)},
                            { "TriggerLocation", MDVUtility.ToStr(dr[dsCDS.CDS.TriggerLocationColumn.ColumnName])},
                            { "Gender", MDVUtility.ToStr(dr[dsCDS.CDS.GenderColumn.ColumnName])},
                            { "RuleTypeDescription", MDVUtility.ToStr(dr[dsCDS.CDS.RuleTypeDesColumn.ColumnName])},
                            { "txtDeveloper", MDVUtility.ToStr(dr[dsCDS.CDS.DeveloperColumn.ColumnName])},
                            { "txtFundingSource", MDVUtility.ToStr(dr[dsCDS.CDS.FundingSourceColumn.ColumnName])},
                            { "txtRelease", MDVUtility.ToStr(dr[dsCDS.CDS.ReleaseColumn.ColumnName])},
                            { "txtReferenceUrl", MDVUtility.ToStr(dr[dsCDS.CDS.ReferenceURLColumn.ColumnName])},
                            { "CDSAgeCondition", MDVUtility.ToStr(dr[dsCDS.CDS.AgeConditionIdColumn.ColumnName])},
                            { "CDSAgeFrom", MDVUtility.ToStr(dr[dsCDS.CDS.FromAgeColumn.ColumnName])},
                            { "CDSAgeTo", MDVUtility.ToStr(dr[dsCDS.CDS.ToAgeColumn.ColumnName])},
                        };
                                CommaSeparatedIds = MDVUtility.ToStr(dr[dsCDS.CDS.EthnicityColumn.ColumnName]);
                                CommaSeparatedRaceIds = MDVUtility.ToStr(dr[dsCDS.CDS.RaceColumn.ColumnName]);
                                lstCDSKeyValues.Add(cdsKeyValues);
                            }
                        }



                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            CDSCount = arrCDSRows.Length,
                            CDSLoad_JSON = js.Serialize(lstCDSKeyValues),
                            PatientDemographicJSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                            VitalSignJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsVitals.VitalSignSoap.TableName]),
                            CDSVitalSignJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSVitals.TableName]),
                            MedicationJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsMedications.Medication.TableName]),
                            CDSMedicationJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSMedication.TableName]),
                            ProblemListJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsProblemList.ProblemList.TableName]),
                            CDSProblemListJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSProblem.TableName]),
                            AllergyJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsAllergies.Allergy.TableName]),
                            CDSAllergyJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSAllergy.TableName]),
                            LabResultJSON = MDVUtility.JSON_DataTable(dsconditions.Tables[dsLabResult.LabOrderResultDetail.TableName]),
                            CDSLabResultJSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSLabResult.TableName]),
                            iTotalDisplayRecords = (arrCDSRows.Length > 0) ? arrCDSRows.Length : 0,
                        };
                        return (JsonConvert.SerializeObject(response));


                    }
                    //End//10-03-2016//Ahmad Raza//Logic to show CDS Search Grid with refined IDs
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CDSCount = dsCDS.Tables[dsCDS.CDS.TableName].Rows.Count,
                            CDSLoad_JSON = MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDS.TableName]),
                            PatientDemographicJSON = dsPatient.Tables[dsPatient.Patients.TableName] != null ? MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]) : "[]",
                            VitalSignJSON = dsconditions.Tables[dsVitals.VitalSignSoap.TableName] != null ? MDVUtility.JSON_DataTable(dsconditions.Tables[dsVitals.VitalSignSoap.TableName]) : "[]",
                            CDSVitalSignJSON = dsCDS.Tables[dsCDS.CDSVitals.TableName] != null ? MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSVitals.TableName]) : "[]",
                            MedicationJSON = dsconditions.Tables[dsMedications.Medication.TableName] != null ? MDVUtility.JSON_DataTable(dsconditions.Tables[dsMedications.Medication.TableName]) : "[]",
                            CDSMedicationJSON = dsCDS.Tables[dsCDS.CDSMedication.TableName] != null ? MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSMedication.TableName]) : "[]",
                            ProblemListJSON = dsconditions.Tables[dsProblemList.ProblemList.TableName] != null ? MDVUtility.JSON_DataTable(dsconditions.Tables[dsProblemList.ProblemList.TableName]) : "[]",
                            CDSProblemListJSON = dsCDS.Tables[dsCDS.CDSProblem.TableName] != null ? MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSProblem.TableName]) : "[]",
                            AllergyJSON = dsconditions.Tables[dsAllergies.Allergy.TableName] != null ? MDVUtility.JSON_DataTable(dsconditions.Tables[dsAllergies.Allergy.TableName]) : "[]",
                            CDSAllergyJSON = dsCDS.Tables[dsCDS.CDSAllergy.TableName] != null ? MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSAllergy.TableName]) : "[]",
                            LabResultJSON = dsconditions.Tables[dsLabResult.LabOrderResultDetail.TableName] != null ? MDVUtility.JSON_DataTable(dsconditions.Tables[dsLabResult.LabOrderResultDetail.TableName]) : "[]",
                            CDSLabResultJSON = dsCDS.Tables[dsCDS.CDSLabResult.TableName] != null ? MDVUtility.JSON_DataTable(dsCDS.Tables[dsCDS.CDSLabResult.TableName]) : "[]",
                            iTotalDisplayRecords = (dsCDS.CDS.Rows.Count > 0) ? dsCDS.CDS.Rows[0][dsCDS.CDS.RecordCountColumn.ColumnName] : 0,
                        };


                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        CDSCount = 0,
                        Message = obj.Message
                    };

                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }





        #region CDS INSURANCE

        public string insertUpdateCDSInsurance(long CDSId, List<CDSInsuranceModel> insuranceList)
        {
            try
            {
                DSCDS dsCDSInsurance = new DSCDS();

                BLObject<DSCDS> obj = BLLClinicalObj.loadCDSInsurance(CDSId, 0);
                dsCDSInsurance = obj.Data;

                if (obj.Data != null)
                {
                    int index = 0;
                    foreach (var insurance in insuranceList)
                    {
                        DSCDS.CDSInsuranceRow cdsInsuranceRow = null;
                        DSCDS.CDSInsuranceRow[] cdsInsuranceRows = (DSCDS.CDSInsuranceRow[])dsCDSInsurance.CDSInsurance.Select(dsCDSInsurance.CDSInsurance.CDSIdColumn + "=" + CDSId + " AND " + dsCDSInsurance.CDSInsurance.InsurancePlanIdColumn.ColumnName + "=" + insurance.InsurancePlanId);

                        if (cdsInsuranceRows.Length > 0)
                        {
                            cdsInsuranceRow = cdsInsuranceRows[0];
                        }
                        else
                        {
                            cdsInsuranceRow = dsCDSInsurance.CDSInsurance.NewCDSInsuranceRow();
                        }

                        if (cdsInsuranceRow != null)
                        {
                            cdsInsuranceRow.CDSId = CDSId;
                            cdsInsuranceRow.InsurancePlanId = MDVUtility.ToInt64(insurance.InsurancePlanId);
                            cdsInsuranceRow.Comments = insurance.Comments;
                            cdsInsuranceRow.InsuranceOperator = insurance.InsuranceOperator;
                            cdsInsuranceRow.IsActive = true;
                            if (cdsInsuranceRows.Length == 0)
                            {
                                cdsInsuranceRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                cdsInsuranceRow.CreatedOn = DateTime.Now;
                            }
                            cdsInsuranceRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsInsuranceRow.ModifiedOn = DateTime.Now;
                            cdsInsuranceRow.CDSInsuranceQuery = createCDSInsuranceQuery(insurance, index);

                            if (cdsInsuranceRows.Length < 1)
                            {
                                dsCDSInsurance.CDSInsurance.AddCDSInsuranceRow(cdsInsuranceRow);
                            }
                            index++;
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSCDS> objInsurance = BLLClinicalObj.insertUpdateCDSInsurance(dsCDSInsurance);
                    dsCDSInsurance = objInsurance.Data;

                    if (objInsurance.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Save_Message,
                            insuranceId = MDVUtility.ToInt64(dsCDSInsurance.Tables[dsCDSInsurance.CDSInsurance.TableName].Rows[0][dsCDSInsurance.CDSInsurance.CDSInsuranceIdColumn.ColumnName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string deleteCDSInsurance(string cdsInsuranceId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(cdsInsuranceId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.deleteCDSInsurance(MDVUtility.ToStr(cdsInsuranceId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string createCDSInsuranceQuery(CDSInsuranceModel insuranceModel, int index)
        {
            try
            {
                string cdsInsuranceQuery = string.Empty;
                if (index == 0)
                {
                    cdsInsuranceQuery = " InsurancePlanId=" + insuranceModel.InsurancePlanId;
                }
                else
                {
                    cdsInsuranceQuery = insuranceModel.InsuranceOperator + " InsurancePlanId=" + insuranceModel.InsurancePlanId;
                }

                return cdsInsuranceQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}