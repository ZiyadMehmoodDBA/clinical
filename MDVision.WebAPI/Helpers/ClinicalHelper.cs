using MDVision.Business.BLL;
using MDVision.Model.Clinical.History;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Newtonsoft.Json.Linq;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.Common.Shared;
using MDVision.IEHR.EMR.Helpers.Clinical.History;
using System.Web.Script.Serialization;
using MDVision.DataAccess.DCommon;
using MDVision.IEHR.EMR.Model.History;
using System.Data;
using MDVision.WebAPI.Models;
using MDVision.WebAPI.Models.SoapTextResponce;
using MDVision.IEHR.Controls.CommonControls;
using System.Text.RegularExpressions;
using MDVision.WebAPI.Entities;
using MDVision.Model.Native;
using System.Web.Http;

namespace MDVision.WebAPI.Helpers
{
    public class ClinicalHelper
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        BLLClinical BLLClinicalObj;
        BLLMobileApp BLLMobileAppObj;
        private string successMessage = "Your information has been received successfully!";
        public ClinicalHelper()
        {

            BLLClinicalObj = new BLLClinical();
            BLLMobileAppObj = new BLLMobileApp();
            BLLAdminProfileObj = new BLLAdminProfile();
        }

        public string GetSocialHistory(int PatientId, string Gender)
        {
            try
            {

                //data:{"PatientId":"333623","SocialHxType":"miscellaneous_occupation","commandType":"FILL_SocialHx"}
                //miscellaneous_sleep      miscellaneous_exercises     miscellaneous_housing     miscellaneous_caffeineintake   
                object response=null;
                if (PatientId > 0)
                {

                    SocialHxHelper helperSocialHx = new SocialHxHelper();
                    string historyJsonTobacco = helperSocialHx.fillSocialHx(new SocialHxModel { PatientId = MDVUtility.ToStr(PatientId), SocialHxType = "tobacco" }, 0, "tobacco");
                    SoapSocialHistoryResponce historyTobacco = JsonConvert.DeserializeObject<SoapSocialHistoryResponce>(historyJsonTobacco);

                    string historyJsonAlcohol = helperSocialHx.fillSocialHx(new SocialHxModel { PatientId = MDVUtility.ToStr(PatientId), SocialHxType = "alcohol" }, 0, "alcohol");
                    SoapSocialHistoryResponce historyAlcohol = JsonConvert.DeserializeObject<SoapSocialHistoryResponce>(historyJsonAlcohol);

                    string historyJsonDrugAbuse = helperSocialHx.fillSocialHx(new SocialHxModel { PatientId = MDVUtility.ToStr(PatientId), SocialHxType = "drug" }, 0, "drug");
                    SoapSocialHistoryResponce historyDrugAbuse = JsonConvert.DeserializeObject<SoapSocialHistoryResponce>(historyJsonDrugAbuse);

                    string historyJsonSexual = helperSocialHx.fillSocialHx(new SocialHxModel { PatientId = MDVUtility.ToStr(PatientId), SocialHxType = "sexual" }, 0, "sexual");
                    SoapSocialHistoryResponce historySexual = JsonConvert.DeserializeObject<SoapSocialHistoryResponce>(historyJsonSexual);


                    // MAPP-45


                    string historyJsonMiscellaneous_Components = helperSocialHx.fillSocialHx(new SocialHxModel { PatientId = MDVUtility.ToStr(PatientId), SocialHxType = "miscellaneous_components" }, 0, "miscellaneous_components");
                    SoapSocialHistoryResponce MiscellaneousHxComponents = JsonConvert.DeserializeObject<SoapSocialHistoryResponce>(historyJsonMiscellaneous_Components);

                    string historyJsonMiscellaneous_Occupation = helperSocialHx.fillSocialHx(new SocialHxModel { PatientId = MDVUtility.ToStr(PatientId), SocialHxType = "miscellaneous_occupation" }, 0, "miscellaneous_occupation");
                    SoapSocialHistoryResponce MiscellaneousOccupation = JsonConvert.DeserializeObject<SoapSocialHistoryResponce>(historyJsonMiscellaneous_Occupation);

                    string historyJsonMiscellaneousSleep = helperSocialHx.fillSocialHx(new SocialHxModel { PatientId = MDVUtility.ToStr(PatientId), SocialHxType = "miscellaneous_sleep" }, 0, "miscellaneous_sleep");
                    SoapSocialHistoryResponce MiscellaneousSleep = JsonConvert.DeserializeObject<SoapSocialHistoryResponce>(historyJsonMiscellaneousSleep);



                    string historyJsonMiscellaneousExercises = helperSocialHx.fillSocialHx(new SocialHxModel { PatientId = MDVUtility.ToStr(PatientId), SocialHxType = "miscellaneous_exercises" }, 0, "miscellaneous_exercises");
                    SoapSocialHistoryResponce MiscellaneousExercises = JsonConvert.DeserializeObject<SoapSocialHistoryResponce>(historyJsonMiscellaneousExercises);


                    string historyJsonMiscellaneousHousing = helperSocialHx.fillSocialHx(new SocialHxModel { PatientId = MDVUtility.ToStr(PatientId), SocialHxType = "miscellaneous_housing" }, 0, "miscellaneous_housing");
                    SoapSocialHistoryResponce MiscellaneousHousing = JsonConvert.DeserializeObject<SoapSocialHistoryResponce>(historyJsonMiscellaneousHousing);


                    string historyJsonMiscellaneousCaffeineintake = helperSocialHx.fillSocialHx(new SocialHxModel { PatientId = MDVUtility.ToStr(PatientId), SocialHxType = "miscellaneous_caffeineintake" }, 0, "miscellaneous_caffeineintake");
                    SoapSocialHistoryResponce MiscellaneousCaffeineintake = JsonConvert.DeserializeObject<SoapSocialHistoryResponce>(historyJsonMiscellaneousCaffeineintake);



                    var SocialHxFill_JSON = historyTobacco.SocialHxFill_JSON != null && historyTobacco.SocialHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<SocialHx>(historyTobacco.SocialHxFill_JSON) : null;
                    var TobaccoHxFill_JSON = historyTobacco.TobaccoHxFill_JSON != null && historyTobacco.TobaccoHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<TobaccoHx>>(historyTobacco.TobaccoHxFill_JSON) : null;
                    var AlcoholHxFill_JSON = historyAlcohol.AlcoholHxFill_JSON != null && historyAlcohol.AlcoholHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<AlcoholHx>>(historyAlcohol.AlcoholHxFill_JSON) : null;
                    var DrugAbuseFill_JSON = historyDrugAbuse.DrugAbuseFill_JSON != null && historyDrugAbuse.DrugAbuseFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<DrugAbuseHx>>(historyDrugAbuse.DrugAbuseFill_JSON) : null;
                    var SexualHxFill_JSON = historySexual.SexualHxFill_JSON != null && historySexual.SexualHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<SexualHx>>(historySexual.SexualHxFill_JSON) : null;
                    var socialHxLoad_JSON = historyTobacco.socialHxLoad_JSON != null && historyTobacco.socialHxLoad_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<SocialHxLoad>>(historyTobacco.socialHxLoad_JSON) : null;



                    //  MAPP - 45
                    var MisHxComponentLoad_JSON = MiscellaneousHxComponents.socialHxMiscHxComponentLoad_JSON != null && MiscellaneousHxComponents.socialHxMiscHxComponentLoad_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<MiscHxComponent>>(MiscellaneousHxComponents.socialHxMiscHxComponentLoad_JSON) : null;
                    var MisOccupationHxFill_JSON = MiscellaneousOccupation.OccupationHxFill_JSON != null && MiscellaneousOccupation.OccupationHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<OccupationHx>>(MiscellaneousOccupation.OccupationHxFill_JSON) : null;
                    var MisSleepHxFill_JSON = MiscellaneousSleep.SleepHxFill_JSON != null && MiscellaneousSleep.SleepHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<SleepHx>>(MiscellaneousSleep.SleepHxFill_JSON) : null;
                    var MisExercisesHxFill_JSON = MiscellaneousExercises.ExercisesHxFill_JSON != null && MiscellaneousExercises.ExercisesHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<ExercisesHx>>(MiscellaneousExercises.ExercisesHxFill_JSON) : null;
                    var MisHousingHxFill_JSON = MiscellaneousHousing.HousingHxFill_JSON != null && MiscellaneousHousing.HousingHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<HousingHx>>(MiscellaneousHousing.HousingHxFill_JSON) : null;
                    var MisCaffeineintakeHxFill_JSON = MiscellaneousCaffeineintake.CaffeineIntakeHxFill_JSON != null && MiscellaneousCaffeineintake.CaffeineIntakeHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<CaffeineHx>>(MiscellaneousCaffeineintake.CaffeineIntakeHxFill_JSON) : null;

                     response = new
                    {
                        status = true,
                        tobaccoTypeLookup_JSON = GetTobaccoType("true"),
                        alcoholTypeLookup_JSON = GetAlcoholType("true"),
                        alcoholFrequencyLookup_JSON = GetAlcoholFrequency("true"),
                        alcoholStatusLookup_JSON = GetAlcoholStatus("true"),
                        drugAbuseLookup_JSON = GetDrugAbuseStatus("true"),
                        drugAbuseDrugsLookup_JSON = GetDrugAbuseDrug("true"),
                        drugAbuseFrequencyDailyLookup_JSON = GetDrugAbuseFrequencyDaily("true"),
                        sexualHxStatusLookup_JSON = GetSexualHxStatus("true"),
                        socialHxUsagePeriodLookup_JSON = GetSocialHxUsagePeriod("true"),
                        tobaccoFrequencyLookup_JSON = GetTobaccoFrequency("true"),
                        socialhxStatusLookup_JSON = GetTobaccoSmokingStatus("true"),
                        socialhxPreferencesLookup_JSON = GetSocialHxPreferences("true"),
                        socialhxProtectionMethodLookup_JSON = GetSexualHxProtectionMethod("true"),
                        socialhxProtectionPeriodLookup_JSON = GetSexualHxProtectionPeriod("true"),

                        socialhxComplaintsPeriodLookup_JSON = GetSexualHxComplaints("true", Gender),
                        MiscHxOccupationHxStatus = GetOccupationStatus("true"),
                        MiscHxSleepHxStatus = getSleepHxStatus("true"),
                        MiscHxExercisesHxStatus = getExercisesHxStatus("true"),
                        MiscHxHousingHxStatus = getHousingHxStatus("true"),
                        MiscHxCaffeineIntakeHxStatus = getCaffeineIntakeHxStatus("true"),

                        MiscHxExercisesHxType = getExercisesHxType("true"),
                        MiscHxExercisesHxDiet = getExercisesHxDiet("true"),
                        MiscHxCaffeineIntakHxFrequency = getCaffeineIntakHxFrequency("true"),



                        soapText_JSON = new
                        {
                            status = historyTobacco.status,
                            SoapText = historyTobacco.SoapText,
                            IsCreatedOrModified = historyTobacco.IsCreatedOrModified,
                            LastUpdated = historyTobacco.LastUpdated,
                            SocialHxFill_JSON = SocialHxFill_JSON,
                            TobaccoHxFill_JSON = TobaccoHxFill_JSON,
                            AlcoholHxFill_JSON = AlcoholHxFill_JSON,
                            DrugAbuseFill_JSON = DrugAbuseFill_JSON,
                            SexualHxFill_JSON = SexualHxFill_JSON,
                            socialHxLoad_JSON = socialHxLoad_JSON,
                            MisHxComponentLoad_JSON = MisHxComponentLoad_JSON,
                            MisOccupationHxFill_JSON = MisOccupationHxFill_JSON,
                            MisSleepHxFill_JSON = MisSleepHxFill_JSON,
                            MisExercisesHxFill_JSON = MisExercisesHxFill_JSON,
                            MisHousingHxFill_JSON = MisHousingHxFill_JSON,
                            MisCaffeineintakeHxFill_JSON = MisCaffeineintakeHxFill_JSON

                        }
                    };
                }
                else
                {

                     response = new
                    {
                        status = true,
                        tobaccoTypeLookup_JSON = GetTobaccoType("true"),
                        alcoholTypeLookup_JSON = GetAlcoholType("true"),
                        alcoholFrequencyLookup_JSON = GetAlcoholFrequency("true"),
                        alcoholStatusLookup_JSON = GetAlcoholStatus("true"),
                        drugAbuseLookup_JSON = GetDrugAbuseStatus("true"),
                        drugAbuseDrugsLookup_JSON = GetDrugAbuseDrug("true"),
                        drugAbuseFrequencyDailyLookup_JSON = GetDrugAbuseFrequencyDaily("true"),
                        sexualHxStatusLookup_JSON = GetSexualHxStatus("true"),
                        socialHxUsagePeriodLookup_JSON = GetSocialHxUsagePeriod("true"),
                        tobaccoFrequencyLookup_JSON = GetTobaccoFrequency("true"),
                        socialhxStatusLookup_JSON = GetTobaccoSmokingStatus("true"),
                        socialhxPreferencesLookup_JSON = GetSocialHxPreferences("true"),
                        socialhxProtectionMethodLookup_JSON = GetSexualHxProtectionMethod("true"),
                        socialhxProtectionPeriodLookup_JSON = GetSexualHxProtectionPeriod("true"),

                        socialhxComplaintsPeriodLookup_JSON = GetSexualHxComplaints("true", Gender),
                        MiscHxOccupationHxStatus = GetOccupationStatus("true"),
                        MiscHxSleepHxStatus = getSleepHxStatus("true"),
                        MiscHxExercisesHxStatus = getExercisesHxStatus("true"),
                        MiscHxHousingHxStatus = getHousingHxStatus("true"),
                        MiscHxCaffeineIntakeHxStatus = getCaffeineIntakeHxStatus("true"),

                        MiscHxExercisesHxType = getExercisesHxType("true"),
                        MiscHxExercisesHxDiet = getExercisesHxDiet("true"),
                        MiscHxCaffeineIntakHxFrequency = getCaffeineIntakHxFrequency("true"),



                        soapText_JSON = new
                        {
                            status = false,
                            SoapText = "",
                            IsCreatedOrModified = "",
                            LastUpdated = "",
                            SocialHxFill_JSON = "",
                            TobaccoHxFill_JSON = "",
                            AlcoholHxFill_JSON = "",
                            DrugAbuseFill_JSON = "",
                            SexualHxFill_JSON = "",
                            socialHxLoad_JSON = "",
                            MisHxComponentLoad_JSON = "",
                            MisOccupationHxFill_JSON = "",
                            MisSleepHxFill_JSON = "",
                            MisExercisesHxFill_JSON = "",
                            MisHousingHxFill_JSON = "",
                            MisCaffeineintakeHxFill_JSON = ""

                        }
                    };

                }

                return (JsonConvert.SerializeObject(response));
                //return replaceJsonProperty("socialHx_JSON", history, jsonResponce);

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string GetFamilyHistory(int PatientId)
        {
            FamilyHxHelper helperFamilyHx = new FamilyHxHelper();
            string historyJson = helperFamilyHx.fillFamilyHx(new IEHR.EMR.Model.History.FamilyHxModel { PatientId = MDVUtility.ToStr(PatientId) }, 0);
            SoapFamilyHistoryResponce history = JsonConvert.DeserializeObject<SoapFamilyHistoryResponce>(historyJson);
            try
            {
                var FamilyHxFill_JSON = history.FamilyHxFill_JSON != null && history.FamilyHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<FamilyHx>(history.FamilyHxFill_JSON) : null;
                var FamilyHxLoad_JSON = history.FamilyHxLoad_JSON != null && history.FamilyHxLoad_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<FamilyHxLoad>>(history.FamilyHxLoad_JSON) : null;
                var DiseaseLoad_JSON = history.DiseaseLoad_JSON != null && history.DiseaseLoad_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<DiseaseLoad>>(history.DiseaseLoad_JSON) : null;
                var MemberLoad_JSON = history.MemberLoad_JSON != null && history.MemberLoad_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<MemberLoad>>(history.MemberLoad_JSON) : null;
                var MemberHasDisease_JSON = history.MemberHasDisease_JSON != null && history.MemberHasDisease_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<MemberHasDisease>>(history.MemberHasDisease_JSON) : null;

                var response = new
                {
                    status = true,
                    familyMembersLookup_JSON = GetFamilyHxFamilyMember("true"),
                    familyMembersStatusLookup_JSON = GetFamilyHxHealthStatus("true"),
                    soapText_JSON = new
                    {
                        status = history.status,
                        FamilyHxFill_JSON = FamilyHxFill_JSON,
                        FamilyHxLoad_JSON = FamilyHxLoad_JSON,
                        DiseaseLoad_JSON = DiseaseLoad_JSON,
                        MemberLoad_JSON = MemberLoad_JSON,
                        MemberHasDisease_JSON = MemberHasDisease_JSON,
                    }
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string GetDiagnosis(string Name, string EntityId, string isMDVision)
        {
            string strJSONData = IMO.GetIMOICDCode(EntityId, Name, MDVUtility.ToBool(isMDVision));
            var icds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NameValuePair>>(strJSONData);
            return (Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                status = true,
                Message = "Success!",
                diagnosis_JSON = icds
            }));
        }
        public string GetProcedures(string Name, string EntityId, string isMDVision)
        {
            string strJSONData = IMO.GetIMOCPTCode(EntityId, Name, MDVUtility.ToBool(isMDVision));
            var icds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NameValuePair>>(strJSONData);
            return (Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                status = true,
                Message = "Success!",
                diagnosis_JSON = icds
            }));
        }

        public string GetMedicalHistory(int PatientId)
        {
            MedicalHxHelper helperMedicalHx = new MedicalHxHelper();
            MedicalHxModel model = new MedicalHxModel { PatientId = MDVUtility.ToStr(PatientId), MedicalHxId = "0", MedicalHxType = "disease" };

            var historyJson = helperMedicalHx.fillMedicalHx(model, Convert.ToInt64(model.MedicalHxId), model.MedicalHxType);

            SoapMedicalHistoryResponce history = JsonConvert.DeserializeObject<SoapMedicalHistoryResponce>(historyJson);
            var MedicalHxFill_JSON = history.MedicalHxFill_JSON != null && history.MedicalHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<MedicalHx>(history.MedicalHxFill_JSON) : null;
            var DiseaseFill_JSON = history.DiseaseFill_JSON != null && history.DiseaseFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<DiseaseHx>(history.DiseaseFill_JSON) : null;
            var MedicalHxDiseaseLoad_JSON = history.MedicalHxDiseaseLoad_JSON != null && history.MedicalHxDiseaseLoad_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<MedicalHxDiseaseLoad>>(history.MedicalHxDiseaseLoad_JSON) : null;

            if (MedicalHxFill_JSON != null && !string.IsNullOrEmpty(MedicalHxFill_JSON.MedicalHxSoapText))
            {
                MedicalHxFill_JSON.MedicalHxSoapText = FormateSoapText(MedicalHxFill_JSON.MedicalHxSoapText);
            }
            //List<HistoryLookupModel> MedicalHxStatusListLookup = new List<HistoryLookupModel>();
            //List<Model.Clinical.History.MedicalHxDiseaseModel> MedicalHxDiseaseList = new List<Model.Clinical.History.MedicalHxDiseaseModel>();
            //List<Model.Clinical.History.MedicalHxDiseaseModel> MedicalHxLog = new List<Model.Clinical.History.MedicalHxDiseaseModel>();
            try
            {
                //MedicalHxStatusListLookup = BLLClinicalObj.LookupMedicalHxStatusNative();
                //MedicalHxDiseaseList = BLLClinicalObj.LoadMedicalHxDiseaseNative(PatientId);

                //int totalRecords = MedicalHxDiseaseList.Count;
                //if (totalRecords > 0)
                //{
                //    Int64 MedicalHxId = Convert.ToInt64(MedicalHxDiseaseList[0].MedicalHxId);
                //    MedicalHxLog = BLLClinicalObj.LoadMedicalHxLogNative(MedicalHxId);
                //}

                var response = new
                {
                    status = true,
                    medicalHxStatusLookup_JSON = GetMedicalHxStatus("true"),
                    soapText_JSON = new
                    {
                        status = history.status,
                        MedicalHxFill_JSON = MedicalHxFill_JSON,
                        DiseaseFill_JSON = DiseaseFill_JSON,
                        MedicalHxDiseaseLoad_JSON = MedicalHxDiseaseLoad_JSON
                    }
                };

                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string GetSurgicalHistory(int PatientId, Int64 diseaseId)
        {

            // data:{"PatientId":"333633","SurgicalHxType":"disease","SurgicalHxId":0,"commandType":"FILL_SurgicalHx","DiseaseId":0}

            SurgicalHxHelper helperSurgicalHx = new SurgicalHxHelper();
            SurgicalHxModel model = new SurgicalHxModel { PatientId = MDVUtility.ToStr(PatientId), SurgicalHxId = "0", SurgicalHxType = "disease", DiseaseId = diseaseId };

            var historyJson = helperSurgicalHx.fillSurgicalHx(model, Convert.ToInt64(model.SurgicalHxId), Convert.ToInt64(model.DiseaseId));

            SoapSurgicalHistoryResponce history = JsonConvert.DeserializeObject<SoapSurgicalHistoryResponce>(historyJson);
            var SurgicalHxFill_JSON = history.SurgicalHxFill_JSON != null && history.SurgicalHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<SurgicalHx>(history.SurgicalHxFill_JSON) : null;
            var DiseaseFill_JSON = history.surgicalHxDiseaseFill_JSON != null && history.surgicalHxDiseaseFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<SurgicalDiseaseHx>(history.surgicalHxDiseaseFill_JSON) : null;
            var SurgicalHxDiseaseLoad_JSON = history.surgicalHxDiseaseLoad_JSON != null && history.surgicalHxDiseaseLoad_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<SurgicalHistoryDiseaseLoad>>(history.surgicalHxDiseaseLoad_JSON) : null;

            if (SurgicalHxFill_JSON != null && !string.IsNullOrEmpty(SurgicalHxFill_JSON.SurgicalHxSoapText))
            {
                SurgicalHxFill_JSON.SurgicalHxSoapText = FormateSoapText(SurgicalHxFill_JSON.SurgicalHxSoapText);
            }
            //List<HistoryLookupModel> MedicalHxStatusListLookup = new List<HistoryLookupModel>();
            //List<Model.Clinical.History.MedicalHxDiseaseModel> MedicalHxDiseaseList = new List<Model.Clinical.History.MedicalHxDiseaseModel>();
            //List<Model.Clinical.History.MedicalHxDiseaseModel> MedicalHxLog = new List<Model.Clinical.History.MedicalHxDiseaseModel>();
            try
            {
                //MedicalHxStatusListLookup = BLLClinicalObj.LookupMedicalHxStatusNative();
                //MedicalHxDiseaseList = BLLClinicalObj.LoadMedicalHxDiseaseNative(PatientId);

                //int totalRecords = MedicalHxDiseaseList.Count;
                //if (totalRecords > 0)
                //{
                //    Int64 MedicalHxId = Convert.ToInt64(MedicalHxDiseaseList[0].MedicalHxId);
                //    MedicalHxLog = BLLClinicalObj.LoadMedicalHxLogNative(MedicalHxId);
                //}

                var response = new
                {
                    status = true,
                    SurgicalHxStatusLookup_JSON = GetSurgicalHxStatus("true"),
                    soapText_JSON = new
                    {
                        status = history.status,
                        SurgicalHxFill_JSON = SurgicalHxFill_JSON,
                        DiseaseFill_JSON = DiseaseFill_JSON,
                        SurgicalHxDiseaseLoad_JSON = SurgicalHxDiseaseLoad_JSON
                    }
                };

                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string GetHospitalizationHistory(int PatientId, Int64 diseaseId)
        {
            //   data: { "PatientId":"333633","HospitalizationHxType":"disease","commandType":"FILL_HospitalizationHx","DiseaseId":0,"HospitalizationHxId":0}
            // data:{"PatientId":"333633","SurgicalHxType":"disease","SurgicalHxId":0,"commandType":"FILL_SurgicalHx","DiseaseId":0}

            HospitalizationHxHelper helperHospitalizationHx = new HospitalizationHxHelper();
            HospitalizationHxModel model = new HospitalizationHxModel { PatientId = MDVUtility.ToStr(PatientId), HospitalizationHxId = "0", HospitalizationHxType = "disease", DiseaseId = diseaseId };

            var historyJson = helperHospitalizationHx.fillHospitalizationHx(model, Convert.ToInt64(model.HospitalizationHxId), Convert.ToString(model.DiseaseId));

            SoapHospitalizationHxResponse history = JsonConvert.DeserializeObject<SoapHospitalizationHxResponse>(historyJson);
            var HospitalizationHxFill_JSON = history.HospitalizationHxFill_JSON != null && history.HospitalizationHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<HospitalizationHx>(history.HospitalizationHxFill_JSON) : null;
            var DiseaseFill_JSON = history.DiseaseFill_JSON != null && history.DiseaseFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<HospitalizationDiseaseHx>(history.DiseaseFill_JSON) : null;
            var HospitalizationHxDiseaseLoad_JSON = history.HospitalizationHxDiseaseLoad_JSON != null && history.HospitalizationHxDiseaseLoad_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<HospitalizationHistoryDiseaseLoad>>(history.HospitalizationHxDiseaseLoad_JSON) : null;

            if (HospitalizationHxFill_JSON != null && !string.IsNullOrEmpty(HospitalizationHxFill_JSON.HospitalizationHxSoapText))
            {
                HospitalizationHxFill_JSON.HospitalizationHxSoapText = FormateSoapTextForHospitalizationHx(HospitalizationHxFill_JSON.HospitalizationHxSoapText);
            }
            //List<HistoryLookupModel> MedicalHxStatusListLookup = new List<HistoryLookupModel>();
            //List<Model.Clinical.History.MedicalHxDiseaseModel> MedicalHxDiseaseList = new List<Model.Clinical.History.MedicalHxDiseaseModel>();
            //List<Model.Clinical.History.MedicalHxDiseaseModel> MedicalHxLog = new List<Model.Clinical.History.MedicalHxDiseaseModel>();
            try
            {
                //MedicalHxStatusListLookup = BLLClinicalObj.LookupMedicalHxStatusNative();
                //MedicalHxDiseaseList = BLLClinicalObj.LoadMedicalHxDiseaseNative(PatientId);

                //int totalRecords = MedicalHxDiseaseList.Count;
                //if (totalRecords > 0)
                //{
                //    Int64 MedicalHxId = Convert.ToInt64(MedicalHxDiseaseList[0].MedicalHxId);
                //    MedicalHxLog = BLLClinicalObj.LoadMedicalHxLogNative(MedicalHxId);
                //}

                var response = new
                {
                    status = true,
                    HospitalizationHxStatusLookup_JSON = GetHospitalizationHxStatus("true"),
                    HospitalizationHxStayLookup_JSON = GetHospitalizationHxStay("true"),
                    soapText_JSON = new
                    {
                        status = history.status,
                        HospitalizationHxFill_JSON = HospitalizationHxFill_JSON,
                        DiseaseFill_JSON = DiseaseFill_JSON,
                        HospitalizationHxDiseaseLoad_JSON = HospitalizationHxDiseaseLoad_JSON
                    }
                };

                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string GetBirthHistory(Int64 PatientId, Int64 birthHxId)
        {
            //   data: { "PatientId":"333633","HospitalizationHxType":"disease","commandType":"FILL_HospitalizationHx","DiseaseId":0,"HospitalizationHxId":0}
            // data:{"PatientId":"333633","SurgicalHxType":"disease","SurgicalHxId":0,"commandType":"FILL_SurgicalHx","DiseaseId":0}

            BirthHxHelper helperBirthHx = new BirthHxHelper();
            BirthHxModel model = new BirthHxModel { PatientId = PatientId, BirthHxId = birthHxId };

            var historyJson = helperBirthHx.fillBirthHx(model);

            SoapBirthHxResponse history = JsonConvert.DeserializeObject<SoapBirthHxResponse>(historyJson);
            var BirthHxFill_JSON = history.BirthHxFill_JSON != null && history.BirthHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<BirthHx>(history.BirthHxFill_JSON) : null;
            var BirthHxLoad_JSON = history.BirthHxLoad_JSON != null && history.BirthHxLoad_JSON.Length > 3 ? JsonConvert.DeserializeObject<List<BirthHxLoad>>(history.BirthHxLoad_JSON) : null;
            var GeneralHxFill_JSON = history.GeneralHxFill_JSON != null && history.GeneralHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<BirthHx_GeneralHx>(history.GeneralHxFill_JSON) : null;
            var MaternalDeliveryHxFill_JSON = history.MaternalDeliveryHxFill_JSON != null && history.MaternalDeliveryHxFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<BirthHx_MaternalDeliveryHx>(history.MaternalDeliveryHxFill_JSON) : null;
            var NewBornFill_JSON = history.NewBornFill_JSON != null && history.NewBornFill_JSON.Length > 3 ? JsonConvert.DeserializeObject<BirthHx_NewBornHx>(history.NewBornFill_JSON) : null;

            if (BirthHxFill_JSON != null && !string.IsNullOrEmpty(BirthHxFill_JSON.BirthHxSoapText))
            {
                BirthHxFill_JSON.BirthHxSoapText = FormateSoapTextForBirthHx(BirthHxFill_JSON.BirthHxSoapText);
            }

            if (BirthHxLoad_JSON != null && !string.IsNullOrEmpty(BirthHxLoad_JSON[0].SoapText))
            {
                BirthHxLoad_JSON[0].SoapText = FormateSoapTextForBirthHx(BirthHxLoad_JSON[0].SoapText);
            }
            try
            {


                var response = new
                {
                    status = true,
                    BirthHxDeliveryMethod_JSON = getBirthHxDeliveryMethod("true"),
                    BirthHxDeliveryPresentation_JSON = getBirthHxDeliveryPresentation("true"),
                    BirthHxMaternalHistory_JSON = getBirthHxMaternalHistory("true"),
                    BirthHxNewbornPatientBloodType_JSON = getBirthHxNewbornPatientBloodType("true"),
                    BirthHxNewbornProblemsAtBirth_JSON = getBirthHxNewbornProblemsAtBirth("true"),

                    soapText_JSON = new
                    {
                        status = history.status,
                        BirthHxFill_JSON = BirthHxFill_JSON,
                        BirthHxLoad_JSON = BirthHxLoad_JSON,
                        GeneralHxFill_JSON = GeneralHxFill_JSON,
                        MaternalDeliveryHxFill_JSON = MaternalDeliveryHxFill_JSON,
                        NewBornFill_JSON = NewBornFill_JSON
                    }
                };

                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string SaveSocialHistory(JObject data)
        {

            string response = "";
            List<object> lstTobaccoModel = new List<object>();
            List<object> lstAlcoholModel = new List<object>();
            List<object> lstDrugAbuseModel = new List<object>();
            List<object> lstSexualHxModel = new List<object>();

            // Start 07/01/2016 Muhammad Arshad MiscHx Related
            List<object> lstOccupationHxModel = new List<object>();
            List<object> lstSleepHxModel = new List<object>();
            List<object> lstExercisesHxModel = new List<object>();
            List<object> lstHousingHxModel = new List<object>();
            List<object> lstCaffeineIntakHxModel = new List<object>();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            SocialHxModel model = ser.Deserialize<SocialHxModel>(MDVUtility.ToStr(data["data"]));

            SocialHxTobaccoModel socialHxTobaccoModel = JsonConvert.DeserializeObject<SocialHxTobaccoModel>(MDVUtility.ToStr(data["data"]));
            SocialHxAlcoholModel socialHxAlcoholModel = JsonConvert.DeserializeObject<SocialHxAlcoholModel>(MDVUtility.ToStr(data["data"]));
            SocialHxDrugAbuseModel socialHxDrugAbuseModel = JsonConvert.DeserializeObject<SocialHxDrugAbuseModel>(MDVUtility.ToStr(data["data"]));
            SocialHxSexualHxModel socialHxSexualHxModel = JsonConvert.DeserializeObject<SocialHxSexualHxModel>(MDVUtility.ToStr(data["data"]));

            // IMP-45 Faizan Ameen.
            SocialHxMiscHxOccupationModel modelOccupationHx = JsonConvert.DeserializeObject<SocialHxMiscHxOccupationModel>(MDVUtility.ToStr(data["data"]));
            SocialHxMiscHxSleepModel modelSleepHx = JsonConvert.DeserializeObject<SocialHxMiscHxSleepModel>(MDVUtility.ToStr(data["data"]));
            SocialHxMiscHxExercisesModel modelExercisesHx = JsonConvert.DeserializeObject<SocialHxMiscHxExercisesModel>(MDVUtility.ToStr(data["data"]));
            SocialHxMiscHxHousingModel modelHousingHx = JsonConvert.DeserializeObject<SocialHxMiscHxHousingModel>(MDVUtility.ToStr(data["data"]));
            SocialHxMiscHxCaffeineIntakeModel modelCaffeineIntakHx = JsonConvert.DeserializeObject<SocialHxMiscHxCaffeineIntakeModel>(MDVUtility.ToStr(data["data"]));


            if (model.commandType.ToLower() == "save_socialhx")
            {
                if (model.SocialHxType.ToLower() == "tobacco")
                {
                    response = UpdateSocialHistoryTabacco(model, socialHxTobaccoModel);
                }
                else if (model.SocialHxType.ToLower() == "alcohol")
                {
                    response = UpdateSocialHistoryAlcohol(model, socialHxAlcoholModel);
                }
                else if (model.SocialHxType.ToLower() == "drug")
                {
                    response = UpdateSocialHistoryDrugAbuse(model, socialHxDrugAbuseModel);
                }
                else if (model.SocialHxType.ToLower() == "sexual")
                {
                    response = UpdateSocialHistorySexualHx(model, socialHxSexualHxModel);
                }
                else if (model.SocialHxType.ToLower() == "miscellaneous_occupation")
                {
                    response = UpdateSocialHistoryMiscOccupation(model, modelOccupationHx);
                }
                else if (model.SocialHxType.ToLower() == "miscellaneous_sleep")
                {
                    response = UpdateSocialHistoryMiscSleep(model, modelSleepHx);
                }
                else if (model.SocialHxType.ToLower() == "miscellaneous_exercises")
                {
                    response = UpdateSocialHistoryMiscExercise(model, modelExercisesHx);
                }
                else if (model.SocialHxType.ToLower() == "miscellaneous_housing")
                {
                    response = UpdateSocialHistoryMiscHousing(model, modelHousingHx);
                }
                else if (model.SocialHxType.ToLower() == "miscellaneous_caffeineintake")
                {
                    response = UpdateSocialHistoryMiscCaffeineIntak(model, modelCaffeineIntakHx);
                }
            }

            return response;

        }
        public string SaveFamilyHistory(JObject data)
        {
            try
            {



                List<object> lstDiseaseModel = new List<object>();
                List<object> lstMembersModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                IEHR.EMR.Model.History.FamilyHxModel model = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.FamilyHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxDiseaseModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.FamilyHxFamilyMemberModel modelMembers = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxFamilyMemberModel>(MDVUtility.ToStr(data["data"]));

                lstDiseaseModel.Add(modelDisease);
                lstMembersModel.Add(modelMembers);
                string PatientId = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToStr(model.PatientId) : MDVUtility.ToStr(model.DimmyPatientId);
                if (Convert.ToInt64(modelDisease.DiseaseId) < 0)
                {
                    int ExistingMinimumId = CheckExistingRecord(PatientId, "FamilyHx_Mobile", "FamilyHx_Mobile_DiseaseId");

                    if (ExistingMinimumId != 0)
                    {
                        modelDisease.DiseaseId = Convert.ToString(Convert.ToInt32(modelDisease.DiseaseId) + (ExistingMinimumId));
                    }
                }

                // ICD Rule.
                Int64 ICDLookupId = 0;
                string ICD9 = null;
                string ICD10 = null;
                string ICD_SNOMEDID = null;
                string ICD_SNOMED_DESCRIPTION = null;
                string ICD9_DESCRIPTION = null;
                string ICD10_DESCRIPTION = null;
                foreach (var item in modelDisease.DataChangeRequest)
                {
                    // ICD Check
                    if (item.columnName == "ICD9")
                    {
                        ICD9 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10")
                    {
                        ICD10 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMEDID")
                    {
                        ICD_SNOMEDID = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMED_DESCRIPTION")
                    {
                        ICD_SNOMED_DESCRIPTION = item.CurrentValueDisplay;

                    }
                    else if (item.columnName == "ICD9_DESCRIPTION")
                    {
                        ICD9_DESCRIPTION = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10_DESCRIPTION")
                    {
                        ICD10_DESCRIPTION = item.CurrentValueDisplay;
                    }
                }

                ICDLookupId = GetICDID(ICD9, ICD10, ICD_SNOMEDID, ICD_SNOMED_DESCRIPTION, ICD9_DESCRIPTION, ICD10_DESCRIPTION);
             
                // get CPTID and ICDID from CPT Lookup and ICD Lookup...              
                DataChangeRequest ICDItem = null;
                if (ICDLookupId > 0)
                {
                    ICDItem = new DataChangeRequest();

                    ICDItem.CurrentValueDisplay = Convert.ToString(ICDLookupId);
                    ICDItem.OriginalValueDisplay = "";
                    ICDItem.columnName = "ICDId";
                    modelDisease.DataChangeRequest.Add(ICDItem);
                }

                 modelDisease.DataChangeRequest.RemoveAll(t =>t.columnName == "ICD9" || t.columnName == "ICD10" || t.columnName == "ICD_SNOMEDID" || t.columnName == "ICD_SNOMED_DESCRIPTION" || t.columnName == "ICD9_DESCRIPTION" || t.columnName == "ICD10_DESCRIPTION");
                List<string> ParentTableMatch = new List<string> { "healthstatusid" };
                foreach (var item in modelDisease.DataChangeRequest)
                {

                   item.ColumnKeyId = modelDisease.DiseaseId;
                   item.ColumnKeyName = "FamilyHx_Mobile_DiseaseId";
                   item.DBTableName = "FamilyHx_Mobile";
                                                         
                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;
                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    
                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelDisease.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
                // save family history. 
                //List<object> lstDiseaseModel = new List<object>();
                //List<object> lstMembersModel = new List<object>();

                //JavaScriptSerializer ser = new JavaScriptSerializer();
                //IEHR.EMR.Model.History.FamilyHxModel model = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxModel>(MDVUtility.ToStr(data["data"]));
                //IEHR.EMR.Model.History.FamilyHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxDiseaseModel>(MDVUtility.ToStr(data["data"]));
                //IEHR.EMR.Model.History.FamilyHxFamilyMemberModel modelMembers = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxFamilyMemberModel>(MDVUtility.ToStr(data["data"]));

                //lstDiseaseModel.Add(modelDisease);
                //lstMembersModel.Add(modelMembers);

                //  return new FamilyHxHelper().saveFamilyHx(model, lstDiseaseModel, lstMembersModel, MDVUtility.ToInt64(model.PatientId));
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }





        }


        public string SaveMedicalHistory(JObject data)
        {
            string response = "";
            try
            {
                List<object> lstDiseaseModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                IEHR.EMR.Model.History.MedicalHxModel model = ser.Deserialize<IEHR.EMR.Model.History.MedicalHxModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.MedicalHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.MedicalHxDiseaseModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.FamilyHxFamilyMemberModel modelMembers = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxFamilyMemberModel>(MDVUtility.ToStr(data["data"]));
                string PatientId = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToStr(model.PatientId) : MDVUtility.ToStr(model.DimmyPatientId);
                if (Convert.ToInt64(modelDisease.DiseaseId) < 0)
                {
                    int ExistingMinimumId = CheckExistingRecord(PatientId, "MedicalHx_Disease", "MedicalHx_DiseaseId");

                    if (ExistingMinimumId != 0)
                    {
                        modelDisease.DiseaseId = Convert.ToString(Convert.ToInt32(modelDisease.DiseaseId) + (ExistingMinimumId));
                    }
                }

                // ICD Rule.
                Int64 ICDLookupId = 0;
                string ICD9 = null;
                string ICD10 = null;
                string ICD_SNOMEDID = null;
                string ICD_SNOMED_DESCRIPTION = null;
                string ICD9_DESCRIPTION = null;
                string ICD10_DESCRIPTION = null;

                foreach (var item in modelDisease.DataChangeRequest)
                {

                    // ICD Check
                    if (item.columnName == "ICD9")
                    {
                        ICD9 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10")
                    {
                        ICD10 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMEDID")
                    {
                        ICD_SNOMEDID = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMED_DESCRIPTION")
                    {
                        ICD_SNOMED_DESCRIPTION = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD9_DESCRIPTION")
                    {
                        ICD9_DESCRIPTION = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10_DESCRIPTION")
                    {
                        ICD10_DESCRIPTION = item.CurrentValueDisplay;
                    }
                }

                ICDLookupId = GetICDID(ICD9, ICD10, ICD_SNOMEDID, ICD_SNOMED_DESCRIPTION, ICD9_DESCRIPTION, ICD10_DESCRIPTION);
                // get   ICDID from ICD Lookup...
                DataChangeRequest ICDItem = null;

                if (ICDLookupId > 0)
                {
                    ICDItem = new DataChangeRequest();

                    ICDItem.CurrentValueDisplay = Convert.ToString(ICDLookupId);
                    ICDItem.OriginalValueDisplay = "";
                    ICDItem.columnName = "ICDId";
                    modelDisease.DataChangeRequest.Add(ICDItem);
                }

                modelDisease.DataChangeRequest.RemoveAll( t=> t.columnName == "ICD9" || t.columnName == "ICD10" || t.columnName == "ICD_SNOMEDID" || t.columnName == "ICD_SNOMED_DESCRIPTION" || t.columnName == "ICD9_DESCRIPTION" || t.columnName == "ICD10_DESCRIPTION");

                List<string> ParentTableMatch = new List<string> { "isrcpneumococcal", "isrcinfluenza", "rcpneumococcaldate", "rcinfluenzadate" };
                foreach (var item in modelDisease.DataChangeRequest)
                {
                   if (ParentTableMatch.Contains(item.columnName.ToLower()))
                    {
                        item.ColumnKeyId = model.MedicalHxId;
                        item.ColumnKeyName = "MedicalHxId";
                        item.DBTableName = "MedicalHx";
                    }
                    else
                    {
                        item.ColumnKeyId = modelDisease.DiseaseId;
                        item.ColumnKeyName = "MedicalHx_DiseaseId";
                        item.DBTableName = "MedicalHx_Disease";
                    }

                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;
                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
            
                    item.MedicalHxId = Convert.ToInt64(model.MedicalHxId);
                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelDisease.DataChangeRequest);




                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }




                //lstDiseaseModel.Add(modelDisease);
                //response = new MedicalHxHelper().saveMedicalHx(model, "", lstDiseaseModel);
                //SoapTextVMModel soapModel = JsonConvert.DeserializeObject<SoapTextVMModel>(response);
                //if (soapModel != null && !string.IsNullOrEmpty(soapModel.SoapText))
                //{
                //    soapModel.SoapText = FormateSoapText(soapModel.SoapText);
                //    return JsonConvert.SerializeObject(soapModel);
                //}
                //else
                //{
                //    return response;
                //}
            }
            catch
            {
                return response;
            }

        }


        public string UpdateSocialHistory(JObject data)
        {
            string response = "";
            List<object> lstTobaccoModel = new List<object>();
            List<object> lstAlcoholModel = new List<object>();
            List<object> lstDrugAbuseModel = new List<object>();
            List<object> lstSexualHxModel = new List<object>();

            // Start 07/01/2016 Muhammad Arshad MiscHx Related
            List<object> lstOccupationHxModel = new List<object>();
            List<object> lstSleepHxModel = new List<object>();
            List<object> lstExercisesHxModel = new List<object>();
            List<object> lstHousingHxModel = new List<object>();
            List<object> lstCaffeineIntakHxModel = new List<object>();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            SocialHxModel model = ser.Deserialize<SocialHxModel>(MDVUtility.ToStr(data["data"]));

            SocialHxTobaccoModel socialHxTobaccoModel = JsonConvert.DeserializeObject<SocialHxTobaccoModel>(MDVUtility.ToStr(data["data"]));
            SocialHxAlcoholModel socialHxAlcoholModel = JsonConvert.DeserializeObject<SocialHxAlcoholModel>(MDVUtility.ToStr(data["data"]));
            SocialHxDrugAbuseModel socialHxDrugAbuseModel = JsonConvert.DeserializeObject<SocialHxDrugAbuseModel>(MDVUtility.ToStr(data["data"]));
            SocialHxSexualHxModel socialHxSexualHxModel = JsonConvert.DeserializeObject<SocialHxSexualHxModel>(MDVUtility.ToStr(data["data"]));

            // IMP-45 Faizan Ameen.
            SocialHxMiscHxOccupationModel modelOccupationHx = JsonConvert.DeserializeObject<SocialHxMiscHxOccupationModel>(MDVUtility.ToStr(data["data"]));
            SocialHxMiscHxSleepModel modelSleepHx = JsonConvert.DeserializeObject<SocialHxMiscHxSleepModel>(MDVUtility.ToStr(data["data"]));
            SocialHxMiscHxExercisesModel modelExercisesHx = JsonConvert.DeserializeObject<SocialHxMiscHxExercisesModel>(MDVUtility.ToStr(data["data"]));
            SocialHxMiscHxHousingModel modelHousingHx = JsonConvert.DeserializeObject<SocialHxMiscHxHousingModel>(MDVUtility.ToStr(data["data"]));
            SocialHxMiscHxCaffeineIntakeModel modelCaffeineIntakHx = JsonConvert.DeserializeObject<SocialHxMiscHxCaffeineIntakeModel>(MDVUtility.ToStr(data["data"]));


            if (model.commandType.ToLower() == "update_socialhx")
            {
                if (model.SocialHxType.ToLower() == "tobacco")
                {
                    response = UpdateSocialHistoryTabacco(model, socialHxTobaccoModel);
                }
                else if (model.SocialHxType.ToLower() == "alcohol")
                {
                    response = UpdateSocialHistoryAlcohol(model, socialHxAlcoholModel);
                }
                else if (model.SocialHxType.ToLower() == "drug")
                {
                    response = UpdateSocialHistoryDrugAbuse(model, socialHxDrugAbuseModel);
                }
                else if (model.SocialHxType.ToLower() == "sexual")
                {
                    response = UpdateSocialHistorySexualHx(model, socialHxSexualHxModel);
                }
                else if (model.SocialHxType.ToLower() == "miscellaneous_occupation")
                {
                    response = UpdateSocialHistoryMiscOccupation(model, modelOccupationHx);
                }
                else if (model.SocialHxType.ToLower() == "miscellaneous_sleep")
                {
                    response = UpdateSocialHistoryMiscSleep(model, modelSleepHx);
                }
                else if (model.SocialHxType.ToLower() == "miscellaneous_exercises")
                {
                    response = UpdateSocialHistoryMiscExercise(model, modelExercisesHx);
                }
                else if (model.SocialHxType.ToLower() == "miscellaneous_housing")
                {
                    response = UpdateSocialHistoryMiscHousing(model, modelHousingHx);
                }
                else if (model.SocialHxType.ToLower() == "miscellaneous_caffeineintake")
                {
                    response = UpdateSocialHistoryMiscCaffeineIntak(model, modelCaffeineIntakHx);
                }
            }

            return response;
        }
        public string UpdateSocialHistoryTabacco(SocialHxModel model, SocialHxTobaccoModel socialHxTobaccoModel)
        {
            try
            {
                foreach (var item in socialHxTobaccoModel.DataChangeRequest)
                {
                    item.ColumnKeyId = socialHxTobaccoModel.TobaccoId;
                    item.ColumnKeyName = "SocialHxTobaccoId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SocialHx_Tobacco";
                    item.SocialHxId = Convert.ToInt64(socialHxTobaccoModel.SocialHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(socialHxTobaccoModel.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }
        }
        public string UpdateSocialHistoryAlcohol(SocialHxModel model, SocialHxAlcoholModel socialHxAlcoholModel)
        {
            try
            {
                foreach (var item in socialHxAlcoholModel.DataChangeRequest)
                {
                    item.ColumnKeyId = socialHxAlcoholModel.AlcoholId;
                    item.ColumnKeyName = "SocialHxAlcoholId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SocialHx_Alcohol";
                    item.SocialHxId = Convert.ToInt64(socialHxAlcoholModel.SocialHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(socialHxAlcoholModel.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }
        }
        public string UpdateSocialHistoryDrugAbuse(SocialHxModel model, SocialHxDrugAbuseModel socialHxDrugAbuseModel)
        {
            try
            {
                foreach (var item in socialHxDrugAbuseModel.DataChangeRequest)
                {
                    item.ColumnKeyId = socialHxDrugAbuseModel.DrugAbuseId;
                    item.ColumnKeyName = "SocialHxDrugAbuseId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SocialHx_DrugAbuse";
                    item.SocialHxId = Convert.ToInt64(socialHxDrugAbuseModel.SocialHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(socialHxDrugAbuseModel.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }
        }
        public string UpdateSocialHistorySexualHx(SocialHxModel model, SocialHxSexualHxModel socialHxSexualHxModel)
        {
            try
            {
                foreach (var item in socialHxSexualHxModel.DataChangeRequest)
                {
                    item.ColumnKeyId = socialHxSexualHxModel.SexualHxId;
                    item.ColumnKeyName = "SocialHxSexualHxIdId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SocialHx_SexualHx";
                    item.SocialHxId = Convert.ToInt64(socialHxSexualHxModel.SocialHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(socialHxSexualHxModel.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }
        }
        public string UpdateSocialHistoryMiscOccupation(SocialHxModel model, SocialHxMiscHxOccupationModel modelOccupationHx)
        {
            try
            {
                foreach (var item in modelOccupationHx.DataChangeRequest)
                {
                    item.ColumnKeyId = modelOccupationHx.OccupationHxId;
                    item.ColumnKeyName = "SocialHxOccupationHxId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SocialHx_MiscHx_OccupationHx";
                    item.SocialHxId = Convert.ToInt64(model.SocialHxId);
                    item.MiscHxId = Convert.ToInt64(modelOccupationHx.MiscHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelOccupationHx.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }
        }
        public string UpdateSocialHistoryMiscSleep(SocialHxModel model, SocialHxMiscHxSleepModel modelSleepHx)
        {
            try
            {
                foreach (var item in modelSleepHx.DataChangeRequest)
                {
                    item.ColumnKeyId = modelSleepHx.SleepHxId;
                    item.ColumnKeyName = "SocialHxSleepHxId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SocialHx_MiscHx_SleepHx";
                    item.SocialHxId = Convert.ToInt64(model.SocialHxId);
                    item.MiscHxId = Convert.ToInt64(modelSleepHx.MiscHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelSleepHx.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }
        }
        public string UpdateSocialHistoryMiscExercise(SocialHxModel model, SocialHxMiscHxExercisesModel modelExercisesHx)
        {
            try
            {
                foreach (var item in modelExercisesHx.DataChangeRequest)
                {
                    item.ColumnKeyId = modelExercisesHx.ExercisesHxId;
                    item.ColumnKeyName = "SocialHxExercisesHxId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SocialHx_MiscHx_ExercisesHx";
                    item.SocialHxId = Convert.ToInt64(model.SocialHxId);
                    item.MiscHxId = Convert.ToInt64(modelExercisesHx.MiscHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelExercisesHx.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }
        }
        public string UpdateSocialHistoryMiscHousing(SocialHxModel model, SocialHxMiscHxHousingModel modelHousingHx)
        {
            try
            {
                foreach (var item in modelHousingHx.DataChangeRequest)
                {
                    item.ColumnKeyId = modelHousingHx.HousingHxId;
                    item.ColumnKeyName = "SocialHxHousingHxId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SocialHx_MiscHx_HousingHx";
                    item.SocialHxId = Convert.ToInt64(model.SocialHxId);
                    item.MiscHxId = Convert.ToInt64(modelHousingHx.MiscHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelHousingHx.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }
        }
        public string UpdateSocialHistoryMiscCaffeineIntak(SocialHxModel model, SocialHxMiscHxCaffeineIntakeModel modelCaffeineIntakHx)
        {
            try
            {
                foreach (var item in modelCaffeineIntakHx.DataChangeRequest)
                {
                    item.ColumnKeyId = modelCaffeineIntakHx.CaffeineIntakeHxId;
                    item.ColumnKeyName = "SocialHxCaffeineIntakHxId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SocialHx_MiscHx_CaffeineIntakHx";
                    item.SocialHxId = Convert.ToInt64(model.SocialHxId);
                    item.MiscHxId = Convert.ToInt64(modelCaffeineIntakHx.MiscHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelCaffeineIntakHx.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }
        }
        public string UpdateFamilyHistory(JObject data)
        {
            //FamilyHxHelper helperFamilyHx = new FamilyHxHelper();

            //List<object> lstDiseaseModel = new List<object>();
            //List<object> lstMembersModel = new List<object>();

            //JavaScriptSerializer ser = new JavaScriptSerializer();
            //IEHR.EMR.Model.History.FamilyHxModel model = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxModel>(MDVUtility.ToStr(data["data"]));
            //IEHR.EMR.Model.History.FamilyHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxDiseaseModel>(MDVUtility.ToStr(data["data"]));
            //FamilyHxFamilyMemberModel modelMembers = ser.Deserialize<FamilyHxFamilyMemberModel>(MDVUtility.ToStr(data["data"]));

            //lstDiseaseModel.Add(modelDisease);
            //lstMembersModel.Add(modelMembers);

            //string response = helperFamilyHx.updateFamilyHx(model, MDVUtility.ToInt64(model.FamilyHxId), lstDiseaseModel, lstMembersModel, MDVUtility.ToInt64(model.PatientId));
            //return response;
            string response = "";
            try
            {



                List<object> lstDiseaseModel = new List<object>();
                List<object> lstMembersModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                IEHR.EMR.Model.History.FamilyHxModel model = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.FamilyHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxDiseaseModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.FamilyHxFamilyMemberModel modelMembers = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxFamilyMemberModel>(MDVUtility.ToStr(data["data"]));

                lstDiseaseModel.Add(modelDisease);
                lstMembersModel.Add(modelMembers);
                string PatientId = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToStr(model.PatientId) : MDVUtility.ToStr(model.DimmyPatientId);
                if (Convert.ToInt64(modelDisease.DiseaseId) < 0)
                {
                    int ExistingMinimumId = CheckExistingRecord(PatientId, "FamilyHx_Mobile", "FamilyHx_Mobile_DiseaseId");

                    if (ExistingMinimumId != 0)
                    {
                        modelDisease.DiseaseId = Convert.ToString(Convert.ToInt32(modelDisease.DiseaseId) + (ExistingMinimumId));
                    }
                }
                // ICD Rule.
                Int64 ICDLookupId = 0;
                string ICD9 = null;
                string ICD10 = null;
                string ICD_SNOMEDID = null;
                string ICD_SNOMED_DESCRIPTION = null;
                string ICD9_DESCRIPTION = null;
                string ICD10_DESCRIPTION = null;
                foreach (var item in modelDisease.DataChangeRequest)
                {
                    // ICD Check
                    if (item.columnName == "ICD9")
                    {
                        ICD9 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10")
                    {
                        ICD10 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMEDID")
                    {
                        ICD_SNOMEDID = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMED_DESCRIPTION")
                    {
                        ICD_SNOMED_DESCRIPTION = item.CurrentValueDisplay;

                    }
                    else if (item.columnName == "ICD9_DESCRIPTION")
                    {
                        ICD9_DESCRIPTION = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10_DESCRIPTION")
                    {
                        ICD10_DESCRIPTION = item.CurrentValueDisplay;
                    }
                }

                ICDLookupId = GetICDID(ICD9, ICD10, ICD_SNOMEDID, ICD_SNOMED_DESCRIPTION, ICD9_DESCRIPTION, ICD10_DESCRIPTION);

                // get CPTID and ICDID from CPT Lookup and ICD Lookup...              
                DataChangeRequest ICDItem = null;
                if (ICDLookupId > 0)
                {
                    ICDItem = new DataChangeRequest();

                    ICDItem.CurrentValueDisplay = Convert.ToString(ICDLookupId);
                    ICDItem.OriginalValueDisplay = "";
                    ICDItem.columnName = "ICDId";
                    modelDisease.DataChangeRequest.Add(ICDItem);
                }

                modelDisease.DataChangeRequest.RemoveAll(t => t.columnName == "ICD9" || t.columnName == "ICD10" || t.columnName == "ICD_SNOMEDID" || t.columnName == "ICD_SNOMED_DESCRIPTION" || t.columnName == "ICD9_DESCRIPTION" || t.columnName == "ICD10_DESCRIPTION");
                List<string> ParentTableMatch = new List<string> { "healthstatusid" };
                foreach (var item in modelDisease.DataChangeRequest)
                {

                    item.ColumnKeyId = modelDisease.DiseaseId;
                    item.ColumnKeyName = "FamilyHx_Mobile_DiseaseId";
                    item.DBTableName = "FamilyHx_Mobile";

                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;
                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = Convert.ToInt64(model.PatientId);
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelDisease.DataChangeRequest);
                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
            }
            

            catch (Exception ex)
            {
                return response;
            }
        }
        public string UpdateMedicalHistory(JObject data)
        {
            string response = "";
            try
            {
                List<object> lstDiseaseModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                IEHR.EMR.Model.History.MedicalHxModel model = ser.Deserialize<IEHR.EMR.Model.History.MedicalHxModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.MedicalHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.MedicalHxDiseaseModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.FamilyHxFamilyMemberModel modelMembers = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxFamilyMemberModel>(MDVUtility.ToStr(data["data"]));
                string PatientId = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToStr(model.PatientId) : MDVUtility.ToStr(model.DimmyPatientId);
                if (Convert.ToInt64(modelDisease.DiseaseId) < 0)
                {
                    int ExistingMinimumId = CheckExistingRecord(PatientId, "MedicalHx_Disease", "MedicalHx_DiseaseId");

                    if (ExistingMinimumId != 0)
                    {
                        modelDisease.DiseaseId = Convert.ToString(Convert.ToInt32(modelDisease.DiseaseId) + (ExistingMinimumId));
                    }

                }


                // ICD Rule.
                Int64 ICDLookupId = 0;
                string ICD9 = null;
                string ICD10 = null;
                string ICD_SNOMEDID = null;
                string ICD_SNOMED_DESCRIPTION = null;
                string ICD9_DESCRIPTION = null;
                string ICD10_DESCRIPTION = null;



                foreach (var item in modelDisease.DataChangeRequest)
                {

                    // ICD Check




                    if (item.columnName == "ICD9")
                    {
                        ICD9 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10")
                    {
                        ICD10 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMEDID")
                    {
                        ICD_SNOMEDID = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMED_DESCRIPTION")
                    {
                        ICD_SNOMED_DESCRIPTION = item.CurrentValueDisplay;

                    }
                    else if (item.columnName == "ICD9_DESCRIPTION")
                    {
                        ICD9_DESCRIPTION = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10_DESCRIPTION")
                    {
                        ICD10_DESCRIPTION = item.CurrentValueDisplay;
                    }


                }

                ICDLookupId = GetICDID(ICD9, ICD10, ICD_SNOMEDID, ICD_SNOMED_DESCRIPTION, ICD9_DESCRIPTION, ICD10_DESCRIPTION);



                // get   ICDID from ICD Lookup...

                DataChangeRequest ICDItem = null;


                if (ICDLookupId > 0)
                {
                    ICDItem = new DataChangeRequest();

                    ICDItem.CurrentValueDisplay = Convert.ToString(ICDLookupId);
                    ICDItem.OriginalValueDisplay = "";
                    ICDItem.columnName = "ICDId";
                    modelDisease.DataChangeRequest.Add(ICDItem);
                }

                modelDisease.DataChangeRequest.RemoveAll(t => t.columnName == "ICD9" || t.columnName == "ICD10" || t.columnName == "ICD_SNOMEDID" || t.columnName == "ICD_SNOMED_DESCRIPTION" || t.columnName == "ICD9_DESCRIPTION" || t.columnName == "ICD10_DESCRIPTION");



                List<string> ParentTableMatch = new List<string> { "isrcpneumococcal", "isrcinfluenza", "rcpneumococcaldate", "rcinfluenzadate" };

                foreach (var item in modelDisease.DataChangeRequest)
                {



                    if (ParentTableMatch.Contains(item.columnName.ToLower()))
                    {
                        item.ColumnKeyId = model.MedicalHxId;
                        item.ColumnKeyName = "MedicalHxId";
                        item.DBTableName = "MedicalHx";
                    }
                    else
                    {

                        item.ColumnKeyId = modelDisease.DiseaseId;
                        item.ColumnKeyName = "MedicalHx_DiseaseId";
                        item.DBTableName = "MedicalHx_Disease";
                    }

                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = Convert.ToInt64(model.PatientId);
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);

                    item.MedicalHxId = Convert.ToInt64(model.MedicalHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelDisease.DataChangeRequest);




                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }
               
                //    List<object> lstDiseaseModel = new List<object>();

                //    MedicalHxHelper helperMedicalHx = new MedicalHxHelper();

                //    JavaScriptSerializer ser = new JavaScriptSerializer();
                //    MedicalHxModel model = ser.Deserialize<MedicalHxModel>(MDVUtility.ToStr(data["data"]));
                //    IEHR.EMR.Model.History.MedicalHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.MedicalHxDiseaseModel>(MDVUtility.ToStr(data["data"]));
                //    lstDiseaseModel.Add(modelDisease);

                //    response = helperMedicalHx.updateMedicalHx(model, MDVUtility.ToInt64(model.MedicalHxId), lstDiseaseModel);
                //    SoapTextVMModel soapModel = JsonConvert.DeserializeObject<SoapTextVMModel>(response);
                //    if (soapModel != null && !string.IsNullOrEmpty(soapModel.SoapText))
                //    {
                //        soapModel.SoapText = FormateSoapText(soapModel.SoapText);
                //        return JsonConvert.SerializeObject(soapModel);
                //    }
                //    else
                //    {
                //        return response;
                //    }


            
            }
            catch (Exception ex)
            {
                return response;
            }
        }

        public string SaveSurigicalHistory(JObject data)
        {
            try
            {
                List<object> lstDiseaseModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                IEHR.EMR.Model.History.SurgicalHxModel model = ser.Deserialize<IEHR.EMR.Model.History.SurgicalHxModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.SurgicalHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.SurgicalHxDiseaseModel>(MDVUtility.ToStr(data["data"]));
                string PatientId = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToStr(model.PatientId) : MDVUtility.ToStr(model.DimmyPatientId);
                if (Convert.ToInt64(modelDisease.DiseaseId) < 0)
                {
                    int ExistingMinimumId = CheckExistingRecord(PatientId, "SurgicalHx_Disease", "SurgicalHx_DiseaseId");

                    if (ExistingMinimumId != 0)
                    {
                        modelDisease.DiseaseId = Convert.ToString(Convert.ToInt32(modelDisease.DiseaseId) + (ExistingMinimumId));
                    }

                }
                // CPT Rule
                Int64 CPTLookupId = 0;

                string CPTCode = null;
                string CPT_Description = null;
                string CPT_SNOMEDId = null;
                string CPT_SNOMED_Description = null;

               



                foreach (var item in modelDisease.DataChangeRequest)
                {

              
                    if (item.columnName == "CPTCode")
                    {
                        CPTCode = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "CPT_Description")
                    {
                        CPT_Description = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "CPT_SNOMEDId")
                    {
                        CPT_SNOMEDId = item.CurrentValueDisplay;

                    }
                    else if (item.columnName == "CPT_SNOMED_Description")
                    {

                        CPT_SNOMED_Description = item.CurrentValueDisplay;
                    }



                   


                }

               

                CPTLookupId = GetCPTID(CPTCode, CPT_Description, CPT_SNOMEDId, CPT_SNOMED_Description);

                // get CPTID and ICDID from CPT Lookup and ICD Lookup...
                DataChangeRequest CPTItem = null;
             
                if (CPTLookupId > 0)
                {
                    CPTItem = new DataChangeRequest();

                    CPTItem.CurrentValueDisplay = Convert.ToString(CPTLookupId);
                    CPTItem.OriginalValueDisplay = "";
                    CPTItem.columnName = "CPTId";
                    modelDisease.DataChangeRequest.Add(CPTItem);
                }

              

                modelDisease.DataChangeRequest.RemoveAll(t => t.columnName == "CPTCode" || t.columnName == "CPT_Description" || t.columnName == "CPT_SNOMEDId" || t.columnName == "CPT_SNOMED_Description" );



                foreach (var item in modelDisease.DataChangeRequest)
                {
                    item.ColumnKeyId = modelDisease.DiseaseId;
                    item.ColumnKeyName = "SurgicalHx_DiseaseId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SurgicalHx_Disease";
              

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelDisease.DataChangeRequest);




                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }


              //  IEHR.EMR.Model.History.FamilyHxFamilyMemberModel modelMembers = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxFamilyMemberModel>(MDVUtility.ToStr(data["data"]));

                //lstDiseaseModel.Add(modelDisease);
                //response = new SurgicalHxHelper().saveSurgicalHx(model, "", lstDiseaseModel);
                //SoapTextSurgicalHxModel soapModel = JsonConvert.DeserializeObject<SoapTextSurgicalHxModel>(response);
                //if (soapModel != null && !string.IsNullOrEmpty(soapModel.SoapText))
                //{
                //    soapModel.SoapText = FormateSoapText(soapModel.SoapText);
                //    return JsonConvert.SerializeObject(soapModel);
                //}
                //else
                //{
                //    return response;
                //}
            }
            catch(Exception ex)
            {
                var responseResult = new
                {
                    status = false,
                    Message = ex.InnerException.ToString()
                };
                return JsonConvert.SerializeObject(responseResult);

               
            }

        }
        public string UpdateSurgicalHistory(JObject data)
        {
            
            try
            {
                List<object> lstDiseaseModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                IEHR.EMR.Model.History.SurgicalHxModel model = ser.Deserialize<IEHR.EMR.Model.History.SurgicalHxModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.SurgicalHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.SurgicalHxDiseaseModel>(MDVUtility.ToStr(data["data"]));
                string PatientId = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToStr(model.PatientId) : MDVUtility.ToStr(model.DimmyPatientId);
                if (Convert.ToInt64(modelDisease.DiseaseId) < 0)
                {
                    int ExistingMinimumId = CheckExistingRecord(PatientId, "SurgicalHx_Disease", "SurgicalHx_DiseaseId");

                    if (ExistingMinimumId != 0)
                    {
                        modelDisease.DiseaseId = Convert.ToString(Convert.ToInt32(modelDisease.DiseaseId) + (ExistingMinimumId));
                    }
                }

                // CPT Rule
                Int64 CPTLookupId = 0;

                string CPTCode = null;
                string CPT_Description = null;
                string CPT_SNOMEDId = null;
                string CPT_SNOMED_Description = null;

              



                foreach (var item in modelDisease.DataChangeRequest)
                {

                    // ICD Check
                    if (item.columnName == "CPTCode")
                    {
                        CPTCode = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "CPT_Description")
                    {
                        CPT_Description = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "CPT_SNOMEDId")
                    {
                        CPT_SNOMEDId = item.CurrentValueDisplay;

                    }
                    else if (item.columnName == "CPT_SNOMED_Description")
                    {

                        CPT_SNOMED_Description = item.CurrentValueDisplay;
                    }



                  


                }


                CPTLookupId = GetCPTID(CPTCode, CPT_Description, CPT_SNOMEDId, CPT_SNOMED_Description);

                // get CPTID and ICDID from CPT Lookup and ICD Lookup...
                DataChangeRequest CPTItem = null;
              
                if (CPTLookupId > 0)
                {
                    CPTItem = new DataChangeRequest();

                    CPTItem.CurrentValueDisplay = Convert.ToString(CPTLookupId);
                    CPTItem.OriginalValueDisplay = "";
                    CPTItem.columnName = "CPTId";
                    modelDisease.DataChangeRequest.Add(CPTItem);
                }

              

                modelDisease.DataChangeRequest.RemoveAll(t => t.columnName == "CPTCode" || t.columnName == "CPT_Description" || t.columnName == "CPT_SNOMEDId" || t.columnName == "CPT_SNOMED_Description" );



                foreach (var item in modelDisease.DataChangeRequest)
                {
                    item.ColumnKeyId = modelDisease.DiseaseId;
                    item.ColumnKeyName = "SurgicalHx_DiseaseId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = Convert.ToInt64(model.PatientId);
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "SurgicalHx_Disease";


                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelDisease.DataChangeRequest);




                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }


               
            }
            catch (Exception ex)
            {
                var responseResult = new
                {
                    status = false,
                    Message = ex.InnerException.ToString()
                };
                return JsonConvert.SerializeObject(responseResult);


            }



            //  IEHR.EMR.Model.History.FamilyHxFamilyMemberModel modelMembers = ser.Deserialize<IEHR.EMR.Model.History.FamilyHxFamilyMemberModel>(MDVUtility.ToStr(data["data"]));

            //    lstDiseaseModel.Add(modelDisease);
            //    response = new SurgicalHxHelper().updateSurgicalHx(model, Convert.ToInt64(model.SurgicalHxId), lstDiseaseModel);
            //    SoapTextSurgicalHxModel soapModel = JsonConvert.DeserializeObject<SoapTextSurgicalHxModel>(response);
            //    if (soapModel != null && !string.IsNullOrEmpty(soapModel.SoapText))
            //    {
            //        soapModel.SoapText = FormateSoapText(soapModel.SoapText);
            //        return JsonConvert.SerializeObject(soapModel);
            //    }
            //    else
            //    {
            //        return response;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return response;
            //}
        }


        public int CheckExistingRecord(string PatientId, string DbTableName, string ColumnKeyName)
        {

            try
            {
                int ReturnVal = 0;

                ReturnVal = BLLMobileAppObj.CheckExistingRecord(PatientId, DbTableName, ColumnKeyName);
                return ReturnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SaveHospitalizationHistory(JObject data)
        {
            try
            {
                List<object> lstDiseaseModel = new List<object>();



                JavaScriptSerializer ser = new JavaScriptSerializer();
                IEHR.EMR.Model.History.HospitalizationHxModel model = ser.Deserialize<IEHR.EMR.Model.History.HospitalizationHxModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.HospitalizationHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.HospitalizationHxDiseaseModel>(MDVUtility.ToStr(data["data"]));
               

                lstDiseaseModel.Add(modelDisease);
                // List<DataChangeRequest> dtcmodel = new List<DataChangeRequest>();
                //dtcmodel = ser.Deserialize<List<DataChangeRequest>>(MDVUtility.ToStr(data["data"]));
                string PatientId = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToStr(model.PatientId) : MDVUtility.ToStr(model.DimmyPatientId);
                if (Convert.ToInt64(modelDisease.DiseaseId) < 0)
                {
                    int ExistingMinimumId = CheckExistingRecord(PatientId, "HospitalizationHx_Disease", "HospitalizationHxDiseaseId");

                    if (ExistingMinimumId != 0)
                    {
                        modelDisease.DiseaseId = Convert.ToString(Convert.ToInt32(modelDisease.DiseaseId) + (ExistingMinimumId));
                    }
                }


                // CPT Rule
                Int64 CPTLookupId = 0;

                string CPTCode =null;
                string CPT_Description = null;
                string CPT_SNOMEDId = null;
                string CPT_SNOMED_Description = null;

                // ICD Rule.
                Int64 ICDLookupId = 0;
                string ICD9 = null;
                string ICD10 = null;
                string ICD_SNOMEDID = null;
                string ICD_SNOMED_DESCRIPTION = null;
                string ICD9_DESCRIPTION = null;
                string ICD10_DESCRIPTION = null;

               

               foreach (var item in modelDisease.DataChangeRequest)
                {

                    // ICD Check
                    if (item.columnName == "CPTCode")
                    {
                        CPTCode = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "CPT_Description")
                    {
                        CPT_Description = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "CPT_SNOMEDId")
                    {
                        CPT_SNOMEDId = item.CurrentValueDisplay;

                    }
                    else if (item.columnName == "CPT_SNOMED_Description")
                    {

                        CPT_SNOMED_Description = item.CurrentValueDisplay;
                    }


                  
                    else if (item.columnName == "ICD9")
                    {
                        ICD9 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10")
                    {
                        ICD10 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMEDID")
                    {
                        ICD_SNOMEDID = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMED_DESCRIPTION")
                    {
                        ICD_SNOMED_DESCRIPTION = item.CurrentValueDisplay;

                    }
                    else if (item.columnName == "ICD9_DESCRIPTION")
                    {
                        ICD9_DESCRIPTION = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10_DESCRIPTION")
                    {
                        ICD10_DESCRIPTION = item.CurrentValueDisplay;
                    }
                    
                    
                }

                ICDLookupId = GetICDID(ICD9, ICD10, ICD_SNOMEDID, ICD_SNOMED_DESCRIPTION, ICD9_DESCRIPTION,ICD10_DESCRIPTION);

                CPTLookupId = GetCPTID(CPTCode,CPT_Description,CPT_SNOMEDId,CPT_SNOMED_Description);

                // get CPTID and ICDID from CPT Lookup and ICD Lookup...
                DataChangeRequest CPTItem = null;
                DataChangeRequest ICDItem = null;
                if (CPTLookupId > 0)
                {
                    CPTItem = new DataChangeRequest();

                    CPTItem.CurrentValueDisplay = Convert.ToString(CPTLookupId);
                    CPTItem.OriginalValueDisplay = "";
                    CPTItem.columnName = "CPTId";
                    modelDisease.DataChangeRequest.Add(CPTItem);
                }

                if (ICDLookupId > 0)
                {
                    ICDItem = new DataChangeRequest();

                    ICDItem.CurrentValueDisplay = Convert.ToString(ICDLookupId);
                    ICDItem.OriginalValueDisplay = "";
                    ICDItem.columnName = "ICDId";
                    modelDisease.DataChangeRequest.Add(ICDItem);
                }

                modelDisease.DataChangeRequest.RemoveAll(t => t.columnName == "CPTCode" || t.columnName == "CPT_Description" || t.columnName == "CPT_SNOMEDId" || t.columnName == "CPT_SNOMED_Description" || t.columnName == "ICD9" || t.columnName == "ICD10" || t.columnName == "ICD_SNOMEDID" || t.columnName == "ICD_SNOMED_DESCRIPTION" || t.columnName == "ICD9_DESCRIPTION" || t.columnName == "ICD10_DESCRIPTION");

                foreach (var item in modelDisease.DataChangeRequest)
                {
                    item.ColumnKeyId = modelDisease.DiseaseId;
                    item.ColumnKeyName = "HospitalizationHxDiseaseId";


                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;

                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                    item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "HospitalizationHx_Disease";
                    item.HospitalizationHxId = Convert.ToInt64(model.HospitalizationHxId);

                }

                    string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelDisease.DataChangeRequest);




                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }






                //response = new HospitalizationHxHelper().saveHospitalizationHx(model, "", lstDiseaseModel);
                //SoapTextHospitalizationHxModel soapModel = JsonConvert.DeserializeObject<SoapTextHospitalizationHxModel>(response);
                //if (soapModel != null && !string.IsNullOrEmpty(soapModel.SoapText))
                //{
                //    soapModel.SoapText = FormateSoapTextForHospitalizationHx(soapModel.SoapText);
                //    return JsonConvert.SerializeObject(soapModel);
                //}
                //else
                //{
                //    return response;
                //}
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }

        }

        public long GetICDID(string ICD9, string ICD10, string ICD_SNOMEDID, string ICD_SNOMED_DESCRIPTION, string ICD9_DESCRIPTION, string ICD10_DESCRIPTION)
        {
            DSCodeLookup dsCodes = new DSCodeLookup();
            DSCodeLookup.ICDLookupRow dr = dsCodes.ICDLookup.NewICDLookupRow();

            long ICDLookupId = 0;

            dr.ICD9 = ICD9;
            dr.ICD9_Description = ICD9_DESCRIPTION;
            dr.ICD10 = ICD10;
            dr.ICD10_Description = ICD10_DESCRIPTION;
            dr.SNOMEDId = ICD_SNOMEDID;
            dr.SNOMED_Description = ICD_SNOMED_DESCRIPTION;

            #region Database Insertion
            dsCodes.ICDLookup.AddICDLookupRow(dr);
            BLObject<DSCodeLookup> obj = BLLMobileAppObj.InsertICDLookup(ref dsCodes);
            dsCodes = obj.Data;
            if (obj.Data != null)
            {
                ICDLookupId = MDVUtility.ToLong(dsCodes.Tables[dsCodes.ICDLookup.TableName].Rows[0][dsCodes.ICDLookup.IdColumn.ColumnName].ToString());
            }
            #endregion
            return ICDLookupId;
           
        }
        public long GetCPTID(string CPTCode,string CPTDescription,string SNOMEDCode,string SNOMEDDescription)
        {
            long CPTID = 0;
            Datasets.DSProcedures dsprocedures = new Datasets.DSProcedures();
            Datasets.DSProcedures.CPTLookupRow dr = dsprocedures.CPTLookup.NewCPTLookupRow();


            dr.CPTCode = CPTCode;
            dr.CPT_Description = CPTDescription;
            dr.SNOMEDId = SNOMEDCode;
            dr.SNOMED_Description = SNOMEDDescription;

            dsprocedures.CPTLookup.AddCPTLookupRow(dr);
            BLObject<Datasets.DSProcedures> obj = BLLMobileAppObj.insertCPTLookup(dsprocedures);
            dsprocedures = obj.Data;
            if (obj.Data != null)
            {
                CPTID = MDVUtility.ToLong(dsprocedures.Tables[dsprocedures.CPTLookup.TableName].Rows[0][dsprocedures.CPTLookup.CPTLookupIdColumn.ColumnName].ToString());

            }
            return CPTID;
        }
        public string UpdateHospitalizationHistory(JObject data)
        {
            try
            {
                List<object> lstDiseaseModel = new List<object>();



                JavaScriptSerializer ser = new JavaScriptSerializer();
                IEHR.EMR.Model.History.HospitalizationHxModel model = ser.Deserialize<IEHR.EMR.Model.History.HospitalizationHxModel>(MDVUtility.ToStr(data["data"]));
                IEHR.EMR.Model.History.HospitalizationHxDiseaseModel modelDisease = ser.Deserialize<IEHR.EMR.Model.History.HospitalizationHxDiseaseModel>(MDVUtility.ToStr(data["data"]));


                lstDiseaseModel.Add(modelDisease);
                // List<DataChangeRequest> dtcmodel = new List<DataChangeRequest>();
                //dtcmodel = ser.Deserialize<List<DataChangeRequest>>(MDVUtility.ToStr(data["data"]));
                string PatientId = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToStr(model.PatientId) : MDVUtility.ToStr(model.DimmyPatientId);
                if (Convert.ToInt64(modelDisease.DiseaseId) < 0)
                {
                    int ExistingMinimumId = CheckExistingRecord(PatientId, "HospitalizationHx_Disease", "HospitalizationHxDiseaseId");

                    if (ExistingMinimumId != 0)
                    {
                        modelDisease.DiseaseId = Convert.ToString(Convert.ToInt32(modelDisease.DiseaseId) + (ExistingMinimumId));
                    }
                }


                // CPT Rule
                Int64 CPTLookupId = 0;

                string CPTCode = null;
                string CPT_Description = null;
                string CPT_SNOMEDId = null;
                string CPT_SNOMED_Description = null;

                // ICD Rule.
                Int64 ICDLookupId = 0;
                string ICD9 = null;
                string ICD10 = null;
                string ICD_SNOMEDID = null;
                string ICD_SNOMED_DESCRIPTION = null;
                string ICD9_DESCRIPTION = null;
                string ICD10_DESCRIPTION = null;



                foreach (var item in modelDisease.DataChangeRequest)
                {

                    // ICD Check
                    if (item.columnName == "CPTCode")
                    {
                        CPTCode = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "CPT_Description")
                    {
                        CPT_Description = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "CPT_SNOMEDId")
                    {
                        CPT_SNOMEDId = item.CurrentValueDisplay;

                    }
                    else if (item.columnName == "CPT_SNOMED_Description")
                    {

                        CPT_SNOMED_Description = item.CurrentValueDisplay;
                    }



                    else if (item.columnName == "ICD9")
                    {
                        ICD9 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10")
                    {
                        ICD10 = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMEDID")
                    {
                        ICD_SNOMEDID = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD_SNOMED_DESCRIPTION")
                    {
                        ICD_SNOMED_DESCRIPTION = item.CurrentValueDisplay;

                    }
                    else if (item.columnName == "ICD9_DESCRIPTION")
                    {
                        ICD9_DESCRIPTION = item.CurrentValueDisplay;
                    }
                    else if (item.columnName == "ICD10_DESCRIPTION")
                    {
                        ICD10_DESCRIPTION = item.CurrentValueDisplay;
                    }


                }

                ICDLookupId = GetICDID(ICD9, ICD10, ICD_SNOMEDID, ICD_SNOMED_DESCRIPTION, ICD9_DESCRIPTION, ICD10_DESCRIPTION);

                CPTLookupId = GetCPTID(CPTCode, CPT_Description, CPT_SNOMEDId, CPT_SNOMED_Description);

                // get CPTID and ICDID from CPT Lookup and ICD Lookup...
                DataChangeRequest CPTItem = null;
                DataChangeRequest ICDItem = null;
                if (CPTLookupId > 0)
                {
                    CPTItem = new DataChangeRequest();

                    CPTItem.CurrentValueDisplay = Convert.ToString(CPTLookupId);
                    CPTItem.OriginalValueDisplay = "";
                    CPTItem.columnName = "CPTId";
                    modelDisease.DataChangeRequest.Add(CPTItem);
                }

                if (ICDLookupId > 0)
                {
                    ICDItem = new DataChangeRequest();

                    ICDItem.CurrentValueDisplay = Convert.ToString(ICDLookupId);
                    ICDItem.OriginalValueDisplay = "";
                    ICDItem.columnName = "ICDId";
                    modelDisease.DataChangeRequest.Add(ICDItem);
                }

                modelDisease.DataChangeRequest.RemoveAll(t => t.columnName == "CPTCode" || t.columnName == "CPT_Description" || t.columnName == "CPT_SNOMEDId" || t.columnName == "CPT_SNOMED_Description" || t.columnName == "ICD9" || t.columnName == "ICD10" || t.columnName == "ICD_SNOMEDID" || t.columnName == "ICD_SNOMED_DESCRIPTION" || t.columnName == "ICD9_DESCRIPTION" || t.columnName == "ICD10_DESCRIPTION");

                foreach (var item in modelDisease.DataChangeRequest)
                {
                    item.ColumnKeyId = modelDisease.DiseaseId;
                    item.ColumnKeyName = "HospitalizationHxDiseaseId";
                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;
                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = Convert.ToInt64(model.PatientId);
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    item.DBTableName = "HospitalizationHx_Disease";
                    item.HospitalizationHxId = Convert.ToInt64(model.HospitalizationHxId);

                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelDisease.DataChangeRequest);




                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }

               //response = new HospitalizationHxHelper().updateHospitalizationHx(model, Convert.ToInt64(model.HospitalizationHxId), lstDiseaseModel);
                //SoapTextHospitalizationHxModel soapModel = JsonConvert.DeserializeObject<SoapTextHospitalizationHxModel>(response);
                //if (soapModel != null && !string.IsNullOrEmpty(soapModel.SoapText))
                //{
                //    soapModel.SoapText = FormateSoapTextForHospitalizationHx(soapModel.SoapText);
                //    return JsonConvert.SerializeObject(soapModel);
                //}
                //else
                //{
                //    return response;
                //}
        }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    Message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }
        }
        public string SaveBirthHistory(JObject data)
        {
            //string response = "";
            try
            {


                JavaScriptSerializer ser = new JavaScriptSerializer();
                IEHR.EMR.Model.History.BirthHxModel model = ser.Deserialize<IEHR.EMR.Model.History.BirthHxModel>(MDVUtility.ToStr(data["data"]));
                BirthHxGeneralModel modelGeneralObj = null;
                BirthHxNewbornModel modelNewbornObj = null;
                BirthHxMaternalDeliveryModel modelMaternalDeliveryObj = null;
                string returnVal = "";
                if ((model.IsGeneralUpdate != null && model.IsGeneralUpdate.Equals("true")))// && (model.birthHxSection.Equals("general") || model.birthHxSection.Equals("general")))
                {
                    modelGeneralObj = ser.Deserialize<BirthHxGeneralModel>(MDVUtility.ToStr(data["data"]));

                    foreach (var item in modelGeneralObj.DataChangeRequest)
                    {
                        item.ColumnKeyId = modelGeneralObj.GeneralId.ToString();
                        item.ColumnKeyName = "GeneralId";


                        item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.CreatedOn = DateTime.Now;

                        item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.ModifiedOn = DateTime.Now;
                        item.PatientId = model.PatientId > 0 ? Convert.ToInt64(model.PatientId) : (long?)null;
                        item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                        item.IsSynced = false;  
                        item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                        item.DBTableName = "BirthHx_General";
       
                         
                    }
                    returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelGeneralObj.DataChangeRequest);
                }
                if ((model.IsDeliveryUpdate != null && model.IsDeliveryUpdate.Equals("true")))// && (model.birthHxSection.Equals("maternalDelivery") || model.birthHxSection.Equals("maternalDelivery")))
                {
                    modelMaternalDeliveryObj = ser.Deserialize<BirthHxMaternalDeliveryModel>(MDVUtility.ToStr(data["data"]));

                    foreach (var item in modelMaternalDeliveryObj.DataChangeRequest)
                    {
                        item.ColumnKeyId = modelMaternalDeliveryObj.MaternalDeliveryId.ToString();
                        item.ColumnKeyName = "MaternalDeliveryId";


                        item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.CreatedOn = DateTime.Now;

                        item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.ModifiedOn = DateTime.Now;
                        item.PatientId = model.PatientId > 0 ? Convert.ToInt64(model.PatientId) : (long?)null;
                        item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                        item.IsSynced = false;
                        item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                        item.DBTableName = "BirthHx_MaternalDelivery";
                       
                    }
                    returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelMaternalDeliveryObj.DataChangeRequest);
                }
                if ((model.IsNewbornUpdate != null && model.IsNewbornUpdate.Equals("true")))// && (model.birthHxSection.Equals("newborn") || model.birthHxSection.Equals("newborn")))
                {
                    modelNewbornObj = ser.Deserialize<BirthHxNewbornModel>(MDVUtility.ToStr(data["data"]));

                    foreach (var item in modelNewbornObj.DataChangeRequest)
                    {
                        item.ColumnKeyId = modelNewbornObj.NewbornId.ToString();
                        item.ColumnKeyName = "NewbornId";


                        item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.CreatedOn = DateTime.Now;

                        item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.ModifiedOn = DateTime.Now;
                        item.PatientId = model.PatientId > 0 ? Convert.ToInt64(model.PatientId) : (long?)null;
                        item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                        item.IsSynced = false;
                        item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                        item.DBTableName = "BirthHx_Newborn";

                       
                    }
                    returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelNewbornObj.DataChangeRequest);
                }

                
           

                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }

                //response = new BirthHxHelper().saveBirthHx(model, modelGeneralObj, modelMaternalDeliveryObj, modelNewbornObj);
                //SoapTextBirthHxModel soapModel = JsonConvert.DeserializeObject<SoapTextBirthHxModel>(response);
                //if (soapModel != null && !string.IsNullOrEmpty(soapModel.SoapText))
                //{
                //    soapModel.SoapText = FormateSoapTextForBirthHx(soapModel.SoapText);
                //    return JsonConvert.SerializeObject(soapModel);
                //}
                //else
                //{
              //  return response;
                //}
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    Message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }

        }
        public string UpdateBirthHistory(JObject data)
        {
            try
            {


                JavaScriptSerializer ser = new JavaScriptSerializer();
                IEHR.EMR.Model.History.BirthHxModel model = ser.Deserialize<IEHR.EMR.Model.History.BirthHxModel>(MDVUtility.ToStr(data["data"]));
                BirthHxGeneralModel modelGeneralObj = null;
                BirthHxNewbornModel modelNewbornObj = null;
                BirthHxMaternalDeliveryModel modelMaternalDeliveryObj = null;
                string returnVal = "";
                if ((model.IsGeneralUpdate != null && model.IsGeneralUpdate.Equals("true")))// && (model.birthHxSection.Equals("general") || model.birthHxSection.Equals("general")))
                {
                    modelGeneralObj = ser.Deserialize<BirthHxGeneralModel>(MDVUtility.ToStr(data["data"]));

                    foreach (var item in modelGeneralObj.DataChangeRequest)
                    {
                        item.ColumnKeyId = modelGeneralObj.GeneralId.ToString();
                        item.ColumnKeyName = "GeneralId";


                        item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.CreatedOn = DateTime.Now;

                        item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.ModifiedOn = DateTime.Now;
                        item.PatientId = Convert.ToInt64(model.PatientId);
                        item.IsSynced = false;
                        item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                        item.DBTableName = "BirthHx_General";


                    }
                    returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelGeneralObj.DataChangeRequest);
                }
                if ((model.IsDeliveryUpdate != null && model.IsDeliveryUpdate.Equals("true")))// && (model.birthHxSection.Equals("maternalDelivery") || model.birthHxSection.Equals("maternalDelivery")))
                {
                    modelMaternalDeliveryObj = ser.Deserialize<BirthHxMaternalDeliveryModel>(MDVUtility.ToStr(data["data"]));

                    foreach (var item in modelMaternalDeliveryObj.DataChangeRequest)
                    {
                        item.ColumnKeyId = modelMaternalDeliveryObj.MaternalDeliveryId.ToString();
                        item.ColumnKeyName = "MaternalDeliveryId";


                        item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.CreatedOn = DateTime.Now;

                        item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.ModifiedOn = DateTime.Now;
                        item.PatientId = Convert.ToInt64(model.PatientId);
                        item.IsSynced = false;
                        item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                        item.DBTableName = "BirthHx_MaternalDelivery";

                    }
                    returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelMaternalDeliveryObj.DataChangeRequest);
                }
                if ((model.IsNewbornUpdate != null && model.IsNewbornUpdate.Equals("true")))// && (model.birthHxSection.Equals("newborn") || model.birthHxSection.Equals("newborn")))
                {
                    modelNewbornObj = ser.Deserialize<BirthHxNewbornModel>(MDVUtility.ToStr(data["data"]));

                    foreach (var item in modelNewbornObj.DataChangeRequest)
                    {
                        item.ColumnKeyId = modelNewbornObj.NewbornId.ToString();
                        item.ColumnKeyName = "NewbornId";


                        item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.CreatedOn = DateTime.Now;

                        item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.ModifiedOn = DateTime.Now;
                        item.PatientId = Convert.ToInt64(model.PatientId);
                        item.IsSynced = false;
                        item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                        item.DBTableName = "BirthHx_Newborn";


                    }
                    returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(modelNewbornObj.DataChangeRequest);
                }




                if (returnVal == "")
                {
                    var responseResult = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(responseResult));
                }
                else
                {
                    var responseResult = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(responseResult);
                }

               
            }
            catch (Exception ex)
            {
                var ExceptionResponse = new
                {
                    status = false,
                    Message = ex.InnerException
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExceptionResponse);

            }





            //string response = "";
            //try
            //{


            //    JavaScriptSerializer ser = new JavaScriptSerializer();
            //    IEHR.EMR.Model.History.BirthHxModel model = ser.Deserialize<IEHR.EMR.Model.History.BirthHxModel>(MDVUtility.ToStr(data["data"]));
            //    BirthHxGeneralModel modelGeneralObj = null;
            //    BirthHxNewbornModel modelNewbornObj = null;
            //    BirthHxMaternalDeliveryModel modelMaternalDeliveryObj = null;
            //    if ((model.IsGeneralUpdate != null && model.IsGeneralUpdate.Equals("true")))// && (model.birthHxSection.Equals("general") || model.birthHxSection.Equals("general")))
            //    {
            //        modelGeneralObj = ser.Deserialize<BirthHxGeneralModel>(MDVUtility.ToStr(data["data"]));
            //    }
            //    if ((model.IsDeliveryUpdate != null && model.IsDeliveryUpdate.Equals("true")))// && (model.birthHxSection.Equals("maternalDelivery") || model.birthHxSection.Equals("maternalDelivery")))
            //    {
            //        modelMaternalDeliveryObj = ser.Deserialize<BirthHxMaternalDeliveryModel>(MDVUtility.ToStr(data["data"]));
            //    }
            //    if ((model.IsNewbornUpdate != null && model.IsNewbornUpdate.Equals("true")))// && (model.birthHxSection.Equals("newborn") || model.birthHxSection.Equals("newborn")))
            //    {
            //        modelNewbornObj = ser.Deserialize<BirthHxNewbornModel>(MDVUtility.ToStr(data["data"]));
            //    }

            //    Int64 BirthHxId = model.BirthHxId;

            //    if (BirthHxId > 0)
            //    {

            //        response = new BirthHxHelper().updateBirthHx(model, BirthHxId, modelGeneralObj, modelMaternalDeliveryObj, modelNewbornObj);
            //        SoapTextBirthHxModel soapModel = JsonConvert.DeserializeObject<SoapTextBirthHxModel>(response);
            //        if (soapModel != null && !string.IsNullOrEmpty(soapModel.SoapText))
            //        {
            //            soapModel.SoapText = FormateSoapTextForBirthHx(soapModel.SoapText);
            //            return JsonConvert.SerializeObject(soapModel);
            //        }
            //        else
            //        {
            //            return response;
            //        }
            //    }
            //    else
            //    {

            //        var NotFoundresponse = new
            //        {
            //            status = false,
            //            Message = "Birth History not found."
            //        };
            //        return Newtonsoft.Json.JsonConvert.SerializeObject(NotFoundresponse);

            //    }
        
          
        
    }
        

        #region Look Up helper method
        // these are same as in MDVisionLookup.ashx.cs

        public HashSet<NameValuePair> GetTobaccoFrequency(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupTobaccoFrequency();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Tobacco_Frequency.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Tobacco_Frequency.TableName].Select("1=1", ds.SocialHx_Tobacco_Frequency.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_Tobacco_Frequency.FrequencyIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Tobacco_Frequency.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Tobacco_Frequency.FrequencyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> GetTobaccoSmokingStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupTobaccoSmokingStatus();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Tobacco_SmokingStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Tobacco_SmokingStatus.TableName].Select("1=1", ds.SocialHx_Tobacco_SmokingStatus.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Tobacco_SmokingStatus.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Tobacco_SmokingStatus.StatusIdColumn.ColumnName].ToString(), dr[ds.SocialHx_Tobacco_SmokingStatus.SNOMEDCTCodeColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
       
        public HashSet<NameValuePair> GetTobaccoType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupTobaccoType();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Tobacco_Type.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Tobacco_Type.TableName].Select("1=1", ds.SocialHx_Tobacco_Type.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Tobacco_Type.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Tobacco_Type.TypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> GetSocialHxUsagePeriod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSocialHxUsagePeriod();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_UsagePeriod.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_UsagePeriod.TableName].Select("1=1", ds.SocialHx_UsagePeriod.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_UsagePeriod.UsagePeriodIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_UsagePeriod.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_UsagePeriod.UsagePeriodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetFamilyHxFamilyMember(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFamilyHxLookup> obHistory = new BLLClinical().LookupFamilyHx_FamilyMember();
            DSFamilyHxLookup ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FamilyHx_FamilyMember.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.FamilyHx_FamilyMember.TableName].Select("1=1", ds.FamilyHx_FamilyMember.DescriptionColumn.ColumnName);
                    //Start//11-02-2016//Ahmad Raza//EMR Bug#291 fixed
                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.FamilyHx_FamilyMember.FamilyMemberIdColumn.ColumnName]))
                    //End//11-02-2016//Ahmad Raza//EMR Bug#291 fixed
                    {
                        list.Add(new NameValuePair(dr[ds.FamilyHx_FamilyMember.DescriptionColumn.ColumnName].ToString(), dr[ds.FamilyHx_FamilyMember.FamilyMemberIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> GetFamilyHxHealthStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFamilyHxLookup> obHistory = new BLLClinical().LookupFamilyHx_HealthStatus();
            DSFamilyHxLookup ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FamilyHx_FamilyMember.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.FamilyHx_HealthStatus.TableName].Select("1=1", ds.FamilyHx_HealthStatus.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.FamilyHx_HealthStatus.DescriptionColumn.ColumnName].ToString(), dr[ds.FamilyHx_HealthStatus.HealthStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetMedicalHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMedicalHxLookup> obHistory = new BLLClinical().LookupMedicalHxStatus();
            DSMedicalHxLookup ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MedicalHx_Status.TableName] != null)
                {
                    //Start 27-01-2016 Muhammad Arshad Bug#	EMR-236	Medical Hx in Clinical Module -> Status -> Values of Dropdown list
                    DataRow[] dRows = ds.Tables[ds.MedicalHx_Status.TableName].Select("1=1", ds.MedicalHx_Status.StatusIdColumn.ColumnName);
                    //End 27-01-2016 Muhammad Arshad Bug#	EMR-236	Medical Hx in Clinical Module -> Status -> Values of Dropdown list
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.MedicalHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.MedicalHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> GetSexualHxProtectionMethod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxProtectionMethod();
            DSSocialHistory ds = obHistory.Data;
            //Start 29/12/2015 Muhammad Irfan for bug # EMR-183
            // list.Add(new NameValuePair("- Method -", ""));
            //End 29/12/2015 Muhammad Irfan for bug # EMR-183
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_ProtectionMethod.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_ProtectionMethod.TableName].Select("1=1", ds.SocialHx_SexualHx_ProtectionMethod.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_ProtectionMethod.ProtectionMethodIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_ProtectionMethod.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_ProtectionMethod.ProtectionMethodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> GetSocialHxPreferences(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxPreferences();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_Preferences.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_Preferences.TableName].Select("1=1", ds.SocialHx_SexualHx_Preferences.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_Preferences.PreferenceIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_Preferences.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_Preferences.PreferenceIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetSexualHxProtectionPeriod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxProtectionPeriod();
            DSSocialHistory ds = obHistory.Data;
            //Start 29/12/2015 Muhammad Irfan for bug # EMR-183
            // list.Add(new NameValuePair("- Period -", ""));
            //End 29/12/2015 Muhammad Irfan for bug # EMR-183
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_ProtectionPeriod.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_ProtectionPeriod.TableName].Select("1=1", ds.SocialHx_SexualHx_ProtectionPeriod.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_ProtectionPeriod.ProtectionPeriodIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_ProtectionPeriod.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_ProtectionPeriod.ProtectionPeriodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetSexualHxComplaints(string IsActive, string Gender)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxComplaints();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_Complaints.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_Complaints.TableName].Select("1=1", ds.SocialHx_SexualHx_Complaints.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_Complaints.ComplaintIdColumn.ColumnName]))
                    {
                        // Start 09/12/2015 Muhammad Irfan To get the dropdown values gender specific
                        if (dr[ds.SocialHx_SexualHx_Complaints.GenderColumn.ColumnName].ToString() == Gender.ToString())
                        {
                            list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_Complaints.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_Complaints.ComplaintIdColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_Complaints.GenderColumn.ColumnName].ToString()));
                        }
                        // End 09/12/2015 Muhammad Irfan To get the dropdown values gender specific
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetAlcoholType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupAlcoholType();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Alcohol_Type.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Alcohol_Type.TableName].Select("1=1", ds.SocialHx_Alcohol_Type.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_Alcohol_Type.TypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Alcohol_Type.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Alcohol_Type.TypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetAlcoholFrequency(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupAlcoholFrequency();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Alcohol_Frequency.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Alcohol_Frequency.TableName].Select("1=1", ds.SocialHx_Alcohol_Frequency.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_Alcohol_Frequency.FrequencyIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Alcohol_Frequency.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Alcohol_Frequency.FrequencyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetAlcoholStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupAlcoholStatus();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Alcohol_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Alcohol_Status.TableName].Select("1=1", ds.SocialHx_Alcohol_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_Alcohol_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Alcohol_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Alcohol_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetDrugAbuseStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupDrugAbuseStatus();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_DrugAbuse_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_DrugAbuse_Status.TableName].Select("1=1", ds.SocialHx_DrugAbuse_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_DrugAbuse_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_DrugAbuse_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_DrugAbuse_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> GetDrugAbuseDrug(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupDrugAbuseDrug();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_DrugAbuse_Drug.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_DrugAbuse_Drug.TableName].Select("1=1", ds.SocialHx_DrugAbuse_Drug.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_DrugAbuse_Drug.DrugIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_DrugAbuse_Drug.ShortNameColumn.ColumnName].ToString(), dr[ds.SocialHx_DrugAbuse_Drug.DrugIdColumn.ColumnName].ToString(), dr[ds.SocialHx_DrugAbuse_Drug.DescriptionColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetDrugAbuseFrequencyDaily(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupDrugAbuseFrequencyDaily();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_DrugAbuse_FrequencyDaily.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_DrugAbuse_FrequencyDaily.TableName].Select("1=1", ds.SocialHx_DrugAbuse_FrequencyDaily.DescriptionColumn.ColumnName);

                    // Start 28/12/2015 Muhammad Irfan bug # EMR-165
                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_DrugAbuse_FrequencyDaily.FrequencyDailyIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_DrugAbuse_FrequencyDaily.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_DrugAbuse_FrequencyDaily.FrequencyDailyIdColumn.ColumnName].ToString()));
                    }
                    // End 28/12/2015 Muhammad Irfan bug # EMR-165
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetSexualHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxStatus();
            DSSocialHistory ds = obHistory.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_Status.TableName].Select("1=1", ds.SocialHx_SexualHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> GetOccupationStatus(string IsActive)
        {
            

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxOccupationStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
      //      list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_OccupationHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_OccupationHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_OccupationHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_OccupationHx_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_OccupationHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_OccupationHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> getSleepHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxSleepHxStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
        //    list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_SleepHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_SleepHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_SleepHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_SleepHx_Status.StatusIdColumn.ColumnName]))
                    {

                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_SleepHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_SleepHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> getExercisesHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxExercisesHxStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
        //    list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_ExercisesHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_ExercisesHx_Status.StatusIdColumn.ColumnName]))
                    {

                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_ExercisesHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_ExercisesHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> getHousingHxStatus(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxHousingHxStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
        //    list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_HousingHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_HousingHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_HousingHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_HousingHx_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_HousingHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_HousingHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> getCaffeineIntakeHxStatus(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxCaffeineIntakeHxStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
         //   list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> getCaffeineIntakHxFrequency(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxCaffeineIntakeHxFrequency();
            DSSocialHistoryLookup ds = obHistory.Data;
        //    list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.TableName].Select("1=1", ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.FrequencyIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.FrequencyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> getExercisesHxType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxExercisesHxType();
            DSSocialHistoryLookup ds = obHistory.Data;
        //    list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Type.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Type.TableName].Select("1=1", ds.SocialHx_MiscHx_ExercisesHx_Type.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_ExercisesHx_Type.TypeIdColumn.ColumnName]))
                    {

                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_ExercisesHx_Type.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_ExercisesHx_Type.TypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> getExercisesHxDiet(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxExercisesHxDiet();
            DSSocialHistoryLookup ds = obHistory.Data;
          //  list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Diet.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Diet.TableName].Select("1=1", ds.SocialHx_MiscHx_ExercisesHx_Diet.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_ExercisesHx_Diet.DietIdColumn.ColumnName]))
                    {

                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_ExercisesHx_Diet.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_ExercisesHx_Diet.DietIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public HashSet<NameValuePair> GetSurgicalHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSurgicalHxLookup> obHistory = new BLLClinical().LookupSurgicalHxStatus();
            DSSurgicalHxLookup ds = obHistory.Data;
          //  list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SurgicalHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SurgicalHx_Status.TableName].Select("1=1", ds.SurgicalHx_Status.StatusIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SurgicalHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SurgicalHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        public string GetProviderEntityBased(string Name,string IsActive, string EntityId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<NameValuePair> FilteredList = new List<NameValuePair>();

            try
            {
                BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupProviderEntityBased(IsActive, EntityId, false);
                DSProfileLookup ds = objProvider.Data;
                //  list.Add(new NameValuePair("- Select -", ""));
                if (ds != null && EntityId != "")
                {

                    if (ds.Tables[ds.Provider.TableName] != null)
                    {
                        DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1", ds.Provider.ShortNameColumn.ColumnName);

                        foreach (DataRow dr in dRows)
                        {
                            if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                                list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Provider.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                            else
                                list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));

                        }
                    }
                }
                FilteredList = list.OfType<NameValuePair>().Where(a => a.Name.Trim().ToLower().Contains(Name.Trim().ToLower())).ToList();

                if (FilteredList.Count > 0)
                {

                    var response = new
                    {
                        status = true,
                        message = "Record Found",
                        ProviderListJSON=FilteredList
                 


                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No Record Found",
                        ProviderListJSON = "[]"



                    };
                    return (JsonConvert.SerializeObject(response));
                }
               
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }


        }

        public object GetProviderWithSpeciality(string Name, string IsActive, string EntityId)
        {
            

            try
            {
                BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupProviderEntityBased(IsActive, EntityId, false);
                DSProfileLookup ds = objProvider.Data;
                if (ds != null && ds.Tables[ds.Provider.TableName] != null)
                {
                        var ProviderList =ds.Tables[ds.Provider.TableName];
                        var response = new
                        {
                            status = true,
                            message = "Record Found",
                            ProviderListJSON = ProviderList
                        };
                        return response;
                  
              
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No Record Found",
                        ProviderListJSON = "[]"



                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }


        }
        public HashSet<NameValuePair> GetHospitalizationHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSHospitalizationHxLookup> obHistory = new BLLClinical().LookupHospitalizationHxStatus();
            DSHospitalizationHxLookup ds = obHistory.Data;
          //  list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.HospitalizationHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.HospitalizationHx_Status.TableName].Select("1=1", ds.HospitalizationHx_Status.DescriptionColumn.ColumnName);
                    //Start//11-02-2016//Abid Ali//EMR Bug#303 fixed
                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.HospitalizationHx_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.HospitalizationHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.HospitalizationHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                    //End//1-02-2016//Abid Ali//EMR Bug#303 fixed
                }
            }
            return list;
        }
        public List<NameValuePair> GetHospitalizationHxStay(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSHospitalizationHxLookup> obHistory = new BLLClinical().LookupHospitalizationHxStay();
            DSHospitalizationHxLookup ds = obHistory.Data;
         //   list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.HospitalizationHx_Stay.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.HospitalizationHx_Stay.TableName].Select("1=1", ds.HospitalizationHx_Stay.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.HospitalizationHx_Stay.DescriptionColumn.ColumnName].ToString(), dr[ds.HospitalizationHx_Stay.StayIdColumn.ColumnName].ToString()));
                    }
                }
            }
            var temp = list.OrderBy(l => l.Value).ToList();
            return temp;
        }
        public HashSet<NameValuePair> getBirthHxDeliveryMethod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBirthHistory> obHistory = new BLLClinical().lookupBirthHxDeliveryMethods();
            DSBirthHistory ds = obHistory.Data;
          //  list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BirthHx_MaternalDelivery_DeliveryMethod.TableName] != null)
                {
                    //emr 433 fix by azhar shahzad on april 12, 2016
                    DataRow[] dRows = ds.Tables[ds.BirthHx_MaternalDelivery_DeliveryMethod.TableName].Select("1=1", ds.BirthHx_MaternalDelivery_DeliveryMethod.DeliveryMethodIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BirthHx_MaternalDelivery_DeliveryMethod.DescriptionColumn.ColumnName].ToString(), dr[ds.BirthHx_MaternalDelivery_DeliveryMethod.DeliveryMethodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for the lookup of Birth Delivery Presentation.
        /// Date : 5 january 2016
        /// </summary>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public HashSet<NameValuePair> getBirthHxDeliveryPresentation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBirthHistory> obHistory = new BLLClinical().lookupBirthHxDeliveryPresentation();
            DSBirthHistory ds = obHistory.Data;
          //  list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BirthHx_MaternalDelivery_DeliveryPresentation.TableName] != null)
                {
                    //emr 432 fix by azhar shahzad on april 12, 2016
                    DataRow[] dRows = ds.Tables[ds.BirthHx_MaternalDelivery_DeliveryPresentation.TableName].Select("1=1", ds.BirthHx_MaternalDelivery_DeliveryPresentation.DeliveryPresentationIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BirthHx_MaternalDelivery_DeliveryPresentation.DescriptionColumn.ColumnName].ToString(), dr[ds.BirthHx_MaternalDelivery_DeliveryPresentation.DeliveryPresentationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for the lookup of Birth Maternal history.
        /// Date : 5 january 2016
        /// </summary>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public HashSet<NameValuePair> getBirthHxMaternalHistory(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBirthHistory> obHistory = new BLLClinical().lookupBirthHxMaternalHistory();
            DSBirthHistory ds = obHistory.Data;
          //  list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BirthHx_MaternalDelivery_MaternalHistory.TableName] != null)
                {
                    //emr 431 fix by azhar shahzad on april 12, 2016
                    DataRow[] dRows = ds.Tables[ds.BirthHx_MaternalDelivery_MaternalHistory.TableName].Select("1=1", ds.BirthHx_MaternalDelivery_MaternalHistory.MaternalHistoryIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BirthHx_MaternalDelivery_MaternalHistory.DescriptionColumn.ColumnName].ToString(), dr[ds.BirthHx_MaternalDelivery_MaternalHistory.MaternalHistoryIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        /* End 05/01/2016 By K.R Lookup for Birth History */

        /*This Lookup is used For new born tab in Birth History,
         Author: Muhammad Azhar Shahzad
         Date: January 05, 2016*/
        public HashSet<NameValuePair> getBirthHxNewbornPatientBloodType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBirthHistory> obHistory = new BLLClinical().birthHxNewbornPatientBloodTypeLookup();
            DSBirthHistory ds = obHistory.Data;
          //  list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BirthHx_Newborn_PatientBloodType.TableName] != null)
                {
                    //emr 431 fix by azhar shahzad on april 12, 2016
                    DataRow[] dRows = ds.Tables[ds.BirthHx_Newborn_PatientBloodType.TableName].Select("1=1", ds.BirthHx_Newborn_PatientBloodType.PatientBloodTypeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BirthHx_Newborn_PatientBloodType.DescriptionColumn.ColumnName].ToString(), dr[ds.BirthHx_Newborn_PatientBloodType.PatientBloodTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        /*This Lookup is used For Newborn Problems At Birth,
         Author: ZeeshanAK
         Date: January 05, 2016*/
        public HashSet<NameValuePair> getBirthHxNewbornProblemsAtBirth(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBirthHistory> obHistory = new BLLClinical().lookupBirthHxNewbornProblemsAtBirth();
            DSBirthHistory ds = obHistory.Data;
          //  list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BirthHx_Newborn_ProblemsAtBirth.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.BirthHx_Newborn_ProblemsAtBirth.TableName].Select("1=1", ds.BirthHx_Newborn_ProblemsAtBirth.ProblemsAtBirthIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BirthHx_Newborn_ProblemsAtBirth.DescriptionColumn.ColumnName].ToString(), dr[ds.BirthHx_Newborn_ProblemsAtBirth.ProblemsAtBirthIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }
        #endregion



        private string FormateSoapText(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            string soapText = "<p>";

            string searchPattern = "underwent|assessment|from|Result|Onset|Duration|Severity|Pattern|Aggravated|Location|Comments";

            Regex rgxStartingDivReplace = new Regex(@"(<div)+[\s\S]+?>");
            input = rgxStartingDivReplace.Replace(input, "");

            var diseasesArray = Regex.Split(input, @"<[/]\w+>");

            int lastDiseaseArrayIndex = diseasesArray.Length - 1;
            int index = 0;

            foreach (string disease in diseasesArray)
            {
                if (index < lastDiseaseArrayIndex && Regex.IsMatch(disease, searchPattern, RegexOptions.IgnoreCase))
                {
                    soapText += disease + " ";
                }
                else if (index < lastDiseaseArrayIndex)
                {
                    soapText += disease.Replace(':', ',') + " ";
                }
                else
                {
                    soapText = soapText.Trim();
                    if (soapText.EndsWith(","))
                    {
                        soapText = soapText.Substring(0, soapText.Length - 1) + ".";
                    }
                    soapText += "<br>" + disease.Trim() + "</p>";
                }
                index++;
            }

            return soapText;
        }
        private string FormateSoapTextForHospitalizationHx(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            string soapText = "<p>";

            string searchPattern = "underwent|assessment|from|Result|Onset|Duration|Severity|Pattern|Aggravated|Location|Comments";

            Regex rgxStartingDivReplace = new Regex(@"(<div)+[\s\S]+?>");
            input = rgxStartingDivReplace.Replace(input, "");

            var diseasesArray = Regex.Split(input, @"<[/]\w+>");

            int lastDiseaseArrayIndex = diseasesArray.Length - 1;
            int index = 0;

            foreach (string disease in diseasesArray)
            {
                if (index < lastDiseaseArrayIndex && Regex.IsMatch(disease, searchPattern, RegexOptions.IgnoreCase))
                {
                    if (index == 0)
                    {
                        

                        if (Regex.IsMatch(disease, "<strong>", RegexOptions.IgnoreCase))
                        {
                            soapText += disease + "</strong>" + " ";
                        }
                        else
                        {
                            soapText += disease + " ";
                        }
                    }
                    else
                    {
                        if (Regex.IsMatch(disease, "<strong>", RegexOptions.IgnoreCase)&& Regex.IsMatch(disease, "admitted", RegexOptions.IgnoreCase))
                        {
                            soapText +="<br>"+ disease + "</strong>" + " ";
                        }
                       else if (Regex.IsMatch(disease, "<strong>", RegexOptions.IgnoreCase) )
                        {
                            soapText +=  disease + "</strong>" + " ";
                        }
                        else if (Regex.IsMatch(disease, "admitted", RegexOptions.IgnoreCase))
                        {
                            soapText += "<br>"+disease  + " ";
                        }
                        else
                        {
                            soapText += disease + " ";
                        }

                       
                        

                    }

                   

                }
                else if (index < lastDiseaseArrayIndex)
                {
                    if (index == 0)
                    {
                        if (Regex.IsMatch(disease, "<strong>", RegexOptions.IgnoreCase))
                        {
                            soapText += disease.Replace(':', ',') + "</strong>" + " ";
                        }
                        else
                        {

                            soapText += disease.Replace(':', ',') + " ";
                        }
                    }
                    else
                    {
                        if (Regex.IsMatch(disease, "<strong>", RegexOptions.IgnoreCase) && Regex.IsMatch(disease, "admitted", RegexOptions.IgnoreCase))
                        {
                            soapText += "<br>" + disease.Replace(':', ',') + "</strong>" + " ";
                        }
                        else if (Regex.IsMatch(disease, "<strong>", RegexOptions.IgnoreCase))

                        {
                            soapText += disease.Replace(':', ',') + "</strong>" + " ";
                        }
                        else if (Regex.IsMatch(disease, "admitted", RegexOptions.IgnoreCase))
                        {
                            soapText += "<br>" + disease.Replace(':', ',') + " ";
                        }
                        else
                        {

                            soapText += disease.Replace(':', ',') + " ";
                        }

                    }
                    

                   
                    
                }
                else
                {
                    soapText = soapText.Trim();
                    if (soapText.EndsWith(","))
                    {
                        soapText = soapText.Substring(0, soapText.Length - 1) + ".";
                    }
                    soapText += "<br>" + disease.Trim() + "</p>";
                }
                index++;
            }

            return soapText;
        }

        private string FormateSoapTextForBirthHx(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            string soapText = "<p>";

            string searchPattern = "underwent|assessment|from|Result|Onset|Duration|Severity|Pattern|Aggravated|Location|Comments|Circumference|Birth|Apgar at 5 Minutes|Weight|infection|Fetal|Gestation|Number|Labor|Delivery|Maternal";

            Regex rgxStartingDivReplace = new Regex(@"(<div)+[\s\S]+?>");
            input = rgxStartingDivReplace.Replace(input, "");

            var diseasesArray = Regex.Split(input, @"<[/]\w+>");

            int lastDiseaseArrayIndex = diseasesArray.Length - 1;
            int index = 0;

           // string BirthHxTypeSearchPattern = "General|Maternal & Delivery|Newborn Information";

            foreach (string disease in diseasesArray)
            {
                if (index < lastDiseaseArrayIndex && Regex.IsMatch(disease, searchPattern, RegexOptions.IgnoreCase))
                {
                    if (Regex.IsMatch(disease, "<strong>", RegexOptions.IgnoreCase))
                    {
                        soapText += "<br>" + disease + "</strong>" + " ";
                    }
                    else
                    {
                        soapText += disease + " ";
                    }

                }
                else if (index < lastDiseaseArrayIndex)
                {
                    if (index == 0)
                    {
                        if (Regex.IsMatch(disease, "<strong>", RegexOptions.IgnoreCase))
                        {
                            soapText += disease + "</strong>" + " ";
                        }

                    }
                    else
                    {
                        if (Regex.IsMatch(disease, "<strong>", RegexOptions.IgnoreCase))
                        {
                            soapText += "<br>" + disease + "</strong>" + " ";
                        }
                        else
                        {

                            soapText += disease.Replace(':', ',') + " ";
                        }
                    }
                }
                else
                {
                    soapText = soapText.Trim();
                    if (soapText.EndsWith(","))
                    {
                        soapText = soapText.Substring(0, soapText.Length - 1) + ".";
                    }
                    soapText += "<br>" + disease.Trim() + "</p>";
                }
                index++;
            }

            return soapText;
        }

        private string LoadProviderLookUp(string shortName)
        {
            try
            {
                DSProfileLookup dsProfileLookup = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupProvider("1", false, shortName);
                if (obj.Data != null)
                {
                    dsProfileLookup = obj.Data;
                    if (dsProfileLookup.Tables[dsProfileLookup.Provider.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ProviderCount = dsProfileLookup.Tables[dsProfileLookup.Provider.TableName].Rows.Count,
                            ProviderLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsProfileLookup.Tables[dsProfileLookup.Provider.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ProviderCount = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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
    }
}