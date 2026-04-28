/* Author:  Muhammad Arshad
 * Created Date: 03/12/2015
 * OverView: Created to handel Histories (SocialHx etc)
 */

using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.History;
using MDVision.IEHR.EMR.Model.History;
using MDVision.IEHR.EMR.Model.Medical;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.Model.Clinical.History.HistorySummary;

namespace MDVision.IEHR.EMR.Services
{
    public class HistoryController : ApiController
    {
        [HttpPost]

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: Entry point for socialHx

        public string SocialHx(JObject AllData)
        {
            string response = null;
            //MDVision.IEHR.Common.MDVisionHandler objHandler = new Common.MDVisionHandler();
            //bool isValidSession = objHandler.IsEMRUserSessionAvalibale(HttpContext.Current);
            if (true == true)
            {
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
                List<object> lstTravelHxModel = new List<object>();
                // End 07/01/2016 Muhammad Arshad MiscHx Related

                JavaScriptSerializer ser = new JavaScriptSerializer();
                SocialHxModel model = ser.Deserialize<SocialHxModel>(MDVUtility.ToStr(AllData["data"]));
                SocialHxTobaccoModel modelTobacco = ser.Deserialize<SocialHxTobaccoModel>(MDVUtility.ToStr(AllData["data"]));
                SocialHxAlcoholModel modelAlcohol = ser.Deserialize<SocialHxAlcoholModel>(MDVUtility.ToStr(AllData["data"]));
                SocialHxDrugAbuseModel modelDrugAbuse = ser.Deserialize<SocialHxDrugAbuseModel>(MDVUtility.ToStr(AllData["data"]));
                SocialHxSexualHxModel modelSexualHx = ser.Deserialize<SocialHxSexualHxModel>(MDVUtility.ToStr(AllData["data"]));

                // Start 07/01/2016 Muhammad Arshad MiscHx Related
                SocialHxMiscHxOccupationModel modelOccupationHx = ser.Deserialize<SocialHxMiscHxOccupationModel>(MDVUtility.ToStr(AllData["data"]));
                SocialHxMiscHxSleepModel modelSleepHx = ser.Deserialize<SocialHxMiscHxSleepModel>(MDVUtility.ToStr(AllData["data"]));
                SocialHxMiscHxExercisesModel modelExercisesHx = ser.Deserialize<SocialHxMiscHxExercisesModel>(MDVUtility.ToStr(AllData["data"]));
                SocialHxMiscHxHousingModel modelHousingHx = ser.Deserialize<SocialHxMiscHxHousingModel>(MDVUtility.ToStr(AllData["data"]));
                SocialHxMiscHxCaffeineIntakeModel modelCaffeineIntakHx = ser.Deserialize<SocialHxMiscHxCaffeineIntakeModel>(MDVUtility.ToStr(AllData["data"]));
                SocialHxMiscHxTravelHxModel modelTravelHx = ser.Deserialize<SocialHxMiscHxTravelHxModel>(MDVUtility.ToStr(AllData["data"]));
                // End 07/01/2016 Muhammad Arshad MiscHx Related

                if (model.commandType.ToLower() == "save_socialhx" || model.commandType.ToLower() == "update_socialhx")
                {
                    if (model.SocialHxType.ToLower() == "tobacco")
                    {
                        lstTobaccoModel.Add(modelTobacco);
                    }
                    else if (model.SocialHxType.ToLower() == "alcohol")
                    {
                        lstAlcoholModel.Add(modelAlcohol);
                    }
                    else if (model.SocialHxType.ToLower() == "drug") 
                    {
                        lstDrugAbuseModel.Add(modelDrugAbuse);
                    }
                    else if (model.SocialHxType.ToLower() == "sexual")
                    {
                        lstSexualHxModel.Add(modelSexualHx);
                    }
                    else if (model.SocialHxType.ToLower() == "miscellaneous_occupation")
                    {
                        lstOccupationHxModel.Add(modelOccupationHx);
                    }
                    else if (model.SocialHxType.ToLower() == "miscellaneous_sleep")
                    {
                        lstSleepHxModel.Add(modelSleepHx);
                    }
                    else if (model.SocialHxType.ToLower() == "miscellaneous_exercises")
                    {
                        lstExercisesHxModel.Add(modelExercisesHx);
                    }
                    else if (model.SocialHxType.ToLower() == "miscellaneous_housing")
                    {
                        lstHousingHxModel.Add(modelHousingHx);
                    }
                    else if (model.SocialHxType.ToLower() == "miscellaneous_caffeineintake")
                    {
                        lstCaffeineIntakHxModel.Add(modelCaffeineIntakHx);
                    }
                    else if (model.SocialHxType.ToLower() == "miscellaneous_travel")
                    {
                        lstTravelHxModel.Add(modelTravelHx);
                    }
                }

                Dictionary<string, dynamic> dictCurrentVitalsJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));

                SocialHxHelper helperSocialHx = new SocialHxHelper();
                if (model.commandType.ToLower() == "fill_socialhx")
                {
                    response = null;
                    response = helperSocialHx.fillSocialHx(model, 0, model.SocialHxType);
                }
                if (model.commandType.ToLower() == "fill_socialhx_native")
                {
                    response = null;
                    response = helperSocialHx.FillSocialHxNative(model);
                }
                else if (model.commandType.ToLower() == "save_socialhx")
                {
                    response = null;
                    //Start Farooq Ahmad 22/01/2016 passing lstSleepHxModel ,lstExercisesHxModel and lstHousingHxModel in saveSocialHx function as parameter
                    response = helperSocialHx.saveSocialHx(model, "", lstTobaccoModel, lstAlcoholModel, lstDrugAbuseModel, lstSexualHxModel, lstOccupationHxModel, lstCaffeineIntakHxModel, lstSleepHxModel, lstExercisesHxModel, lstHousingHxModel,lstTravelHxModel);
                    //End Farooq Ahmad 22/01/2016 passing lstSleepHxModel ,lstExercisesHxModel and lstHousingHxModel in saveSocialHx function as parameter
                }
                if (model.commandType.ToLower() == "save_socialhx_native")
                {
                    response = null;
                    response = helperSocialHx.saveSocialHxNative(model);//, "", lstTobaccoModel, lstAlcoholModel, lstDrugAbuseModel, lstSexualHxModel, lstOccupationHxModel, lstSleepHxModel, lstExercisesHxModel, lstHousingHxModel, lstCaffeineIntakHxModel);
                }
                else if (model.commandType.ToLower() == "update_socialhx")
                {
                    response = null;
                    //Start Farooq Ahmad 22/01/2016 passing lstSleepHxModel ,lstExercisesHxModel and lstHousingHxModel in updateSocialHx function as parameter
                    response = helperSocialHx.updateSocialHx(model, MDVUtility.ToInt64(model.SocialHxId), lstTobaccoModel, lstAlcoholModel, lstDrugAbuseModel, lstSexualHxModel, lstOccupationHxModel, lstCaffeineIntakHxModel, lstSleepHxModel, lstExercisesHxModel, lstHousingHxModel, lstTravelHxModel);
                    //End Farooq Ahmad 22/01/2016 passing lstSleepHxModel ,lstExercisesHxModel and lstHousingHxModel in updateSocialHx function as parameter
                }
                //Author: Azhar Shahzad
                // Section: For Progress Note Attachement
                // Date: Dec 14,2015
                else if (model.commandType.ToLower() == "getlatest_socialhxby_patientid")
                {
                    response = helperSocialHx.fillSocialHx(model, 0, string.Empty);
                }
                else if (model.commandType.ToLower() == "attach_socialhx_from_notes")
                {
                    response = helperSocialHx.attach_SocialHx_With_Notes(model.SocialHxId, model.NotesId);
                }
                else if (model.commandType.ToLower() == "detach_socialhx_from_notes")
                {
                    response = helperSocialHx.detach_SocialHx_From_Notes(model.SocialHxId, model.NotesId);
                }
                //end azhar changed

                else if (model.commandType.ToLower() == "update_componentordersorting")
                {
                    response = helperSocialHx.updateComponentOrderSorting(model);
                }
                
            }

            return response;

        }
        public string SocialHxMiscHxTravelHx(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            SocialHxMiscHxTravelHxModel modelTravelHx = ser.Deserialize<SocialHxMiscHxTravelHxModel>(MDVUtility.ToStr(AllData["data"]));
            SocialHxHelper helperSocialHx = new SocialHxHelper();
            if (modelTravelHx.commandType.ToLower() == "fill_sochxmischxtravelhx")
            {
                response = helperSocialHx.SocialHxMiscHxTravelHxFill(modelTravelHx);
            }
            else if (modelTravelHx.commandType.ToLower() == "save_sochxmischxtravelhx" || modelTravelHx.commandType.ToLower() == "update_sochxmischxtravelhx")
            {
                response = helperSocialHx.insertUpdateTravelHx(modelTravelHx);
            }
            else if (modelTravelHx.commandType.ToLower() == "delete_sochxmischxtravelhx" )
            {
                response = helperSocialHx.DeleteSocialMiscTravelHx(modelTravelHx.TravelHxId, modelTravelHx.SocialHxId);
            }
            return response;
        }
        public string SocialHxMiscHxOccupationHx(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            SocialHxMiscHxOccupationModel modelOccupationHx = ser.Deserialize<SocialHxMiscHxOccupationModel>(MDVUtility.ToStr(AllData["data"]));
            SocialHxHelper helperSocialHx = new SocialHxHelper();
            if (modelOccupationHx.commandType.ToLower() == "fill_sochxmischxoccupationhx")
            {
                response = helperSocialHx.SocialHxMiscHxOccupationHxFill(modelOccupationHx);
            }
           else if (modelOccupationHx.commandType.ToLower() == "save_sochxmischxoccupationhx" || modelOccupationHx.commandType.ToLower() == "update_sochxmischxoccupationhx")
            {
                response = helperSocialHx.insertUpdateOccupationHx(modelOccupationHx);
            }
            else if (modelOccupationHx.commandType.ToLower() == "delete_sochxmischxoccupationhx")
            {
                response = helperSocialHx.DeleteSocialMiscOccupationHx(modelOccupationHx.OccupationHxId, modelOccupationHx.SocialHxId);
            }
            return response;
        }
        /// <summary>
        /// This Function Is created for History Summary Controller
        /// Author: Muhammad Azhar Shahzad
        /// Date: Dec 16, 2015
        /// </summary>
        /// <param name="AllData"></param>
        /// <returns></returns>
        [HttpPost]
        public string HistorySummary(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            HistorySummary model = ser.Deserialize<HistorySummary>(MDVUtility.ToStr(AllData["data"]));
            HistorySummaryHelper helperHxSummary = new HistorySummaryHelper();
            if (model.commandType.ToLower() == "gethistorysummarysoapbypatientid")
            {
                // response = helperHxSummary.getHistorySummarySoapByPatientid_Obsolete(model);
                response = helperHxSummary.getHistorySummarySoapByPatientid(model);
            }
            else if (model.commandType.ToLower() == "get_hx_log")
            {
                response = helperHxSummary.getHxLog(model);
            }
            else if (model.commandType.ToLower() == "getautopopulatesetting")
            {
                response = helperHxSummary.getAutoPopulateSetting(model);
            }
            else if (model.commandType.ToLower() == "updatenoteshxtaborder")
            {
                response = null;
                response = helperHxSummary.UpdateNotesHxtabOrder(MDVUtility.ToInt64(model.NoteId), model.HxtabOrder);
            }
            else if (model.commandType.ToLower() == "savecachehistories")
            {
                return helperHxSummary.UpdateHistoryTabFromNotes(model);
            }

            return response;
        }
     

        /// <summary>
        /// This Function Is created for Birth History Controller
        /// Author: Muhammad Azhar Shahzad
        /// Date: January 07, 2016
        /// </summary>
        /// <param name="AllData"></param>
        /// <returns></returns>
        [HttpPost]
        public string birthHistory(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            BirthHxGeneralModel modelGeneralObj = null;
            BirthHxNewbornModel modelNewbornObj = null;
            BirthHxMaternalDeliveryModel modelMaternalDeliveryObj = null;
            BirthHxModel model = ser.Deserialize<BirthHxModel>(MDVUtility.ToStr(AllData["data"]));
            if ((model.commandType.ToLower() == "save_birthhx" || model.commandType.ToLower() == "update_birthhx") && model.BirthHxUnremarkable == false)
            {
                if ((model.IsGeneralUpdate != null && model.IsGeneralUpdate.Equals("true")))// && (model.birthHxSection.Equals("general") || model.birthHxSection.Equals("general")))
                {
                    modelGeneralObj = ser.Deserialize<BirthHxGeneralModel>(MDVUtility.ToStr(AllData["data"]));
                }
                if ((model.IsDeliveryUpdate != null && model.IsDeliveryUpdate.Equals("true")))// && (model.birthHxSection.Equals("maternalDelivery") || model.birthHxSection.Equals("maternalDelivery")))
                {
                    modelMaternalDeliveryObj = ser.Deserialize<BirthHxMaternalDeliveryModel>(MDVUtility.ToStr(AllData["data"]));
                }
                if ((model.IsNewbornUpdate != null && model.IsNewbornUpdate.Equals("true")))// && (model.birthHxSection.Equals("newborn") || model.birthHxSection.Equals("newborn")))
                {
                    modelNewbornObj = ser.Deserialize<BirthHxNewbornModel>(MDVUtility.ToStr(AllData["data"]));
                }
            }


            BirthHxHelper helperBirthHx = new BirthHxHelper();

            if (model.commandType.ToLower() == "fill_birthhx")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("History_Birth Hx", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperBirthHx.fillBirthHx(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "save_birthhx")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("History_Birth Hx", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperBirthHx.saveBirthHx(model, modelGeneralObj, modelMaternalDeliveryObj, modelNewbornObj);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "update_birthhx")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("History_Birth Hx", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperBirthHx.updateBirthHx(model, MDVUtility.ToInt64(model.BirthHxId), modelGeneralObj, modelMaternalDeliveryObj, modelNewbornObj);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }

            else if (model.commandType.ToLower() == "getlatest_birthhxby_patientid")
            {
                response = helperBirthHx.fillBirthHx(model);
            }
            else if (model.commandType.ToLower() == "attach_birthhx_from_notes")
            {
                response = helperBirthHx.attach_BirthHx_With_Notes(model.BirthHxId, model.NotesId);
            }
            else if (model.commandType.ToLower() == "detach_birthhx_from_notes")
            {
                response = helperBirthHx.detach_BirthHx_From_Notes(model.BirthHxId, model.NotesId);
            }
            return response;
        }

        [HttpPost]

        // Author:  Muhammad Arshad
        // Created Date: 07/01/2016
        //OverView: Entry point for medicalHx

        public string MedicalHx(JObject AllData)
        {
            string response = null;
            //MDVision.IEHR.Common.MDVisionHandler objHandler = new Common.MDVisionHandler();
            //bool isValidSession = objHandler.IsEMRUserSessionAvalibale(HttpContext.Current);
            if (true == true)
            {
                List<object> lstDiseaseModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                MedicalHxModel model = ser.Deserialize<MedicalHxModel>(MDVUtility.ToStr(AllData["data"]));
                MedicalHxDiseaseModel modelDisease = ser.Deserialize<MedicalHxDiseaseModel>(MDVUtility.ToStr(AllData["data"]));

                if (model.commandType.ToLower() == "save_medicalhx" || model.commandType.ToLower() == "update_medicalhx" || model.commandType.ToLower() == "save_medicalhx_native")
                {
                    if (model.MedicalHxType.ToLower() == "disease")
                    {
                        lstDiseaseModel.Add(modelDisease);
                    }
                }

                Dictionary<string, dynamic> dictCurrentVitalsJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));

                MedicalHxHelper helperMedicalHx = new MedicalHxHelper();
                if (model.commandType.ToLower() == "fill_medicalhx")
                {
                    response = null;
                    response = helperMedicalHx.fillMedicalHx(model, Convert.ToInt64(model.MedicalHxId), model.MedicalHxType);
                }
                if (model.commandType.ToLower() == "fill_medicalhx_native")
                {
                    response = null;
                    response = helperMedicalHx.fillMedicalHxNative(model, Convert.ToInt64(model.PatientId),model.RequestStatus,Convert.ToInt64(model.DiseaseId));
                }
                else if (model.commandType.ToLower() == "save_medicalhx")
                {
                    response = null;
                    response = helperMedicalHx.saveMedicalHx(model, "", lstDiseaseModel);
                }
               
                
                else if (model.commandType.ToLower() == "update_medicalhx")
                {
                    response = null;
                    response = helperMedicalHx.updateMedicalHx(model, MDVUtility.ToInt64(model.MedicalHxId), lstDiseaseModel);
                }

                else if (model.commandType.ToLower() == "getlatest_medicalhxby_patientid")
                {
                    response = helperMedicalHx.fillMedicalHx(model, 0, string.Empty);
                }
                else if (model.commandType.ToLower() == "attach_medicalhx_from_notes")
                {
                    response = helperMedicalHx.attach_MedicalHx_With_Notes(model.MedicalHxId, model.NotesId);
                }
                else if (model.commandType.ToLower() == "detach_medicalhx_from_notes")
                {
                    response = helperMedicalHx.detach_MedicalHx_From_Notes(Convert.ToInt64(model.MedicalHxId), Convert.ToInt64(model.NotesId));
                }

                else if (model.commandType.ToLower() == "delete_medicalhxdisease")
                {
                    response = null;
                    response = helperMedicalHx.deleteMedicalHxDisease(MDVUtility.ToStr(model.DiseaseId), MDVUtility.ToStr(model.MedicalHxId));
                }
            }

            return response;

        }

        [HttpPost]

        // Author:  Muhammad Arshad
        // Created Date: 14/01/2016
        //OverView: Entry point for FamilyHx

        public string FamilyHx(JObject AllData)
        {
            string response = null;
            //MDVision.IEHR.Common.MDVisionHandler objHandler = new Common.MDVisionHandler();
            //bool isValidSession = objHandler.IsEMRUserSessionAvalibale(HttpContext.Current);
            if (true == true)
            {
                List<object> lstDiseaseModel = new List<object>();
                List<object> lstMembersModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                FamilyHxModel model = ser.Deserialize<FamilyHxModel>(MDVUtility.ToStr(AllData["data"]));
                FamilyHxDiseaseModel modelDisease = ser.Deserialize<FamilyHxDiseaseModel>(MDVUtility.ToStr(AllData["data"]));
                FamilyHxFamilyMemberModel modelMembers = ser.Deserialize<FamilyHxFamilyMemberModel>(MDVUtility.ToStr(AllData["data"]));

                if (model.commandType.ToLower() == "save_familyhx" || model.commandType.ToLower() == "update_familyhx")
                {
                    //if (model.MedicalHxType.ToLower() == "disease")
                    //{
                    //    lstDiseaseModel.Add(modelDisease);
                    //}
                    lstDiseaseModel.Add(modelDisease);
                    lstMembersModel.Add(modelMembers);
                }

                Dictionary<string, dynamic> dictCurrentVitalsJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));

                FamilyHxHelper helperFamilyHx = new FamilyHxHelper();
                if (model.commandType.ToLower() == "fill_familyhx")
                {
                    response = null;
                    response = helperFamilyHx.fillFamilyHx(model, MDVUtility.ToInt64(model.FamilyHxId));
                }
                else
                    if (model.commandType.ToLower() == "save_familyhx")
                    {
                        response = null;
                        response = helperFamilyHx.saveFamilyHx(model, lstDiseaseModel, lstMembersModel, MDVUtility.ToInt64(model.PatientId));
                    }
                    else if (model.commandType.ToLower() == "update_familyhx")
                    {
                        response = null;
                        response = helperFamilyHx.updateFamilyHx(model, MDVUtility.ToInt64(model.FamilyHxId), lstDiseaseModel, lstMembersModel, MDVUtility.ToInt64(model.PatientId));
                    }
                    else if (model.commandType.ToLower() == "fill_members")
                    {
                        response = null;
                        response = helperFamilyHx.fillFamilyMember(modelMembers);
                    }
                    else if (model.commandType.ToLower() == "fill_members_detail")
                    {
                        response = null;
                        string familyHxId = dictCurrentVitalsJSON.ContainsKey("FamilyHxId") == true ? MDVUtility.ToStr(dictCurrentVitalsJSON["FamilyHxId"]) : "";
                        response = helperFamilyHx.fillFamilyMemberDetail(modelMembers, familyHxId);
                    }
                    //Start//27/01/2016//Ahmad Raza//method calling for attach/detach,delete and get latest familyhx 
                    else if (model.commandType.ToLower() == "getlatest_familyhxby_patientid")
                    {
                        response = helperFamilyHx.fillFamilyHx(model, 0);
                    }
                    else if (model.commandType.ToLower() == "attach_familyhx_from_notes")
                    {
                        response = helperFamilyHx.attach_FamilyHx_With_Notes(model.FamilyHxId, model.NotesId);
                    }
                    else if (model.commandType.ToLower() == "detach_familyhx_from_notes")
                    {
                        response = helperFamilyHx.detach_FamilyHx_From_Notes(Convert.ToInt64(model.FamilyHxId), Convert.ToInt64(model.NotesId));
                    }
                    else if (model.commandType.ToLower() == "delete_familyhxdisease")
                    {
                        response = null;
						//Start 03-11-2016 Humaira Yousaf for familyMemberId
                        response = helperFamilyHx.deleteFamilyHxDisease(MDVUtility.ToStr(model.DiseaseId), MDVUtility.ToStr(model.FamilyHxId), model.FamilyMemberId, model.PatientId);
						//End 03-11-2016 Humaira Yousaf for familyMemberId
                    }
                    else if (model.commandType.ToLower() == "delete_familymemberdetail")
                    {
                        response = null;
                        response = helperFamilyHx.deleteFamilyHxMemberDetail(MDVUtility.ToStr(modelMembers.MemberDetailId), MDVUtility.ToStr(model.FamilyHxId), MDVUtility.ToStr(model.DiseaseId), MDVUtility.ToInt64(model.PatientId));
                    }
                //End//27/01/2016//Ahmad Raza//method calling for attach/detach,delete and get latest familyhx 
                   //Start 04-11-2016 Humaira Yousaf to fill member diseases
				    else if (model.commandType.ToLower() == "fill_members_disease")
                    {
                        response = null;
                        response = helperFamilyHx.fillFamilyMemberDiseases(modelDisease);
                    }
					//Start 04-11-2016 Humaira Yousaf for familyMemberId
                
            }

            return response;

        }

        [HttpPost]

        // Author:  Muhammad Arshad
        // Created Date: 14/01/2016
        //OverView: Entry point for SurgicalHx

        public string SurgicalHx(JObject AllData)
        {
            string response = null;
            //MDVision.IEHR.Common.MDVisionHandler objHandler = new Common.MDVisionHandler();
            //bool isValidSession = objHandler.IsEMRUserSessionAvalibale(HttpContext.Current);
            if (true == true)
            {
                List<object> lstDiseaseModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                SurgicalHxModel model = ser.Deserialize<SurgicalHxModel>(MDVUtility.ToStr(AllData["data"]));
                SurgicalHxDiseaseModel modelSurgical = ser.Deserialize<SurgicalHxDiseaseModel>(MDVUtility.ToStr(AllData["data"]));

                if (model.commandType.ToLower() == "save_surgicalhx" || model.commandType.ToLower() == "update_surgicalhx")
                {
                    if (model.SurgicalHxType.ToLower() == "disease")
                    {
                        lstDiseaseModel.Add(modelSurgical);
                    }
                }

                Dictionary<string, dynamic> dictCurrentVitalsJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
                SurgicalHxHelper helperSurgicalHx = new SurgicalHxHelper();
                if (model.commandType.ToLower() == "fill_surgicalhx")
                {
                    response = null;
                    response = helperSurgicalHx.fillSurgicalHx(model, MDVUtility.ToInt64(model.SurgicalHxId), MDVUtility.ToInt64(model.DiseaseId));
                }
                else if (model.commandType.ToLower() == "save_surgicalhx")
                {
                    response = null;
                    response = helperSurgicalHx.saveSurgicalHx(model, "", lstDiseaseModel);
                }
                else if (model.commandType.ToLower() == "save_surgicalhxnative")
                {
                    response = null;
                    response = helperSurgicalHx.saveSurgicalHxNative(model, "", modelSurgical);
                }
                else if (model.commandType.ToLower() == "update_surgicalhx")
                {
                    response = null;
                    response = helperSurgicalHx.updateSurgicalHx(model, MDVUtility.ToInt64(model.SurgicalHxId), lstDiseaseModel);
                }

                else if (model.commandType.ToLower() == "getlatest_surgicalhxby_patientid")
                {
                    response = helperSurgicalHx.fillSurgicalHx(model, 0, MDVUtility.ToInt64(model.DiseaseId));
                }
                else if (model.commandType.ToLower() == "attach_surgicalhx_from_notes")
                {
                    response = helperSurgicalHx.attach_SurgicalHx_With_Notes(model.SurgicalHxId, model.NotesId);
                }
                else if (model.commandType.ToLower() == "detach_surgicalhx_from_notes")
                {
                    response = helperSurgicalHx.detach_SurgicalHx_From_Notes(Convert.ToInt64(model.SurgicalHxId), model.NotesId);
                }
                else if (model.commandType.ToLower() == "delete_surgicalhxdisease")
                {
                    response = null;
                    response = helperSurgicalHx.deleteSurgicalHxDisease(MDVUtility.ToStr(model.DiseaseId), MDVUtility.ToStr(model.SurgicalHxId), model.PatientId);
                }

            }

            return response;

        }

        [HttpPost]

        // Author:  Muhammad Arshad
        // Created Date: 14/01/2016
        //OverView: Entry point for HospitalizationHx

        public string HospitalizationHx(JObject AllData)
        {
            string response = null;
            //MDVision.IEHR.Common.MDVisionHandler objHandler = new Common.MDVisionHandler();
            //bool isValidSession = objHandler.IsEMRUserSessionAvalibale(HttpContext.Current);
            if (true == true)
            {
                List<object> lstDiseaseModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                HospitalizationHxModel model = ser.Deserialize<HospitalizationHxModel>(MDVUtility.ToStr(AllData["data"]));
                //start Farooq Ahmad 22/01/2016 Deserialize data as HospitalizationHxDiseaseModel
                HospitalizationHxDiseaseModel modelDisease = ser.Deserialize<HospitalizationHxDiseaseModel>(MDVUtility.ToStr(AllData["data"]));
                //end Farooq Ahmad 22/01/2016 Deserialize data as HospitalizationHxDiseaseModel
                if (model.commandType.ToLower() == "save_hospitalizationhx" || model.commandType.ToLower() == "update_hospitalizationhx")
                {
                    if (model.HospitalizationHxType.ToLower() == "disease")
                    {
                        lstDiseaseModel.Add(modelDisease);
                    }
                }

                Dictionary<string, dynamic> dictCurrentVitalsJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));

                HospitalizationHxHelper helperHospitalizationHx = new HospitalizationHxHelper();
                if (model.commandType.ToLower() == "fill_hospitalizationhx")
                {
                    response = null;
                    response = helperHospitalizationHx.fillHospitalizationHx(model, MDVUtility.ToInt64(model.HospitalizationHxId), model.HospitalizationHxType);
                }
                else if (model.commandType.ToLower() == "save_hospitalizationhx")
                {
                    response = null;
                    response = helperHospitalizationHx.saveHospitalizationHx(model, "", lstDiseaseModel);
                }
                
                else if (model.commandType.ToLower() == "update_hospitalizationhx")
                {
                    response = null;
                    response = helperHospitalizationHx.updateHospitalizationHx(model, MDVUtility.ToInt64(model.HospitalizationHxId), lstDiseaseModel);
                }
                //Start//22/01/2016//Ahmad Raza//Implimented methods for HospitalizationHx's association with Note
                else if (model.commandType.ToLower() == "getlatest_hospitalizationhxby_patientid")
                {
                    response = null;
                    response = helperHospitalizationHx.fillHospitalizationHx(model, 0, string.Empty);
                }
                else if (model.commandType.ToLower() == "attach_hospitalizationhx_from_notes")
                {
                    //Start 01/03/2016 Farooq Ahmad rename the function name 
                    response = helperHospitalizationHx.attachHospitalizationHxWithNotes(model.HospitalizationHxId, model.NotesId);
                    //End 01/03/2016 Farooq Ahmad rename the function name 
                }
                else if (model.commandType.ToLower() == "detach_hospitalizationhx_from_notes")
                {
                    //Start 01/03/2016 Farooq Ahmad rename the function name 
                    response = helperHospitalizationHx.detachHospitalizationHxFromNotes(Convert.ToInt64(model.HospitalizationHxId), Convert.ToInt64(model.NotesId));
                    //End 01/03/2016 Farooq Ahmad rename the function name 
                }
                //End//22/01/2016//Ahmad Raza//Implimented methods for HospitalizationHx's association with Note
                else if (model.commandType.ToLower() == "delete_hospitalizationhxdisease")
                {
                    response = null;
                    response = helperHospitalizationHx.deleteHospitalizationHxDisease(model.DiseaseId.ToString(), model.HospitalizationHxId, model.PatientId);
                }
            }

            return response;

        }
        public string SocPsyandBehaviorHx(JObject AllData)
        {
            string response = null;
            //MDVision.IEHR.Common.MDVisionHandler objHandler = new Common.MDVisionHandler();
            //bool isValidSession = objHandler.IsEMRUserSessionAvalibale(HttpContext.Current);
            if (true == true)
            {
                List<object> lstDiseaseModel = new List<object>();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                SocPsyandBehaviorHxModel model = ser.Deserialize<SocPsyandBehaviorHxModel>(MDVUtility.ToStr(AllData["data"]));

                string privilegasMessage = "";

                SocPsyandBehaviorHxHelper helperSocPsyandBehaviorHx = new SocPsyandBehaviorHxHelper();
                if (model.commandType.ToLower() == "getquestions")
                {
                    response = null;
                    response = helperSocPsyandBehaviorHx.GetQuestions(model);
                }
                else if (model.commandType.ToLower() == "gettotalandphqscore")
                {
                    response = null;
                    response = helperSocPsyandBehaviorHx.GetTotalAndPHQScore(model);
                }
                else if (model.commandType.ToLower() == "socpsyandbehaviorhxsave")
                {
                    privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("History_Social, Psychological and Behavior Hx", "ADD")).ToString();
                    if (string.IsNullOrEmpty(privilegasMessage))
                    {
                        response = null;
                        response = helperSocPsyandBehaviorHx.SocPsyandBehaviorHxSave(model);
                    }
                    else
                    {
                        var responseObj = new
                        {
                            status = false,
                            Message = privilegasMessage
                        };
                        return (JsonConvert.SerializeObject(responseObj));
                    }
                }
                else if (model.commandType.ToLower() == "socpsyandbehaviorhxupdate")
                {
                    privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("History_Social, Psychological and Behavior Hx", "EDIT")).ToString();
                    if (string.IsNullOrEmpty(privilegasMessage))
                    {
                        response = null;
                        response = helperSocPsyandBehaviorHx.SocPsyandBehaviorHxSave(model);
                    }
                    else
                    {
                        var responseObj = new
                        {
                            status = false,
                            Message = privilegasMessage
                        };
                        return (JsonConvert.SerializeObject(responseObj));
                    }
                }
                else if (model.commandType.ToLower() == "fillsocpsyandbehaviorhx")
                {
                    privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("History_Social, Psychological and Behavior Hx", "EDIT")).ToString();
                    if (string.IsNullOrEmpty(privilegasMessage))
                    {
                        response = null;
                        response = helperSocPsyandBehaviorHx.LoadSocPsyandBehaviorHx(model);
                    }
                    else
                    {
                        var responseObj = new
                        {
                            status = false,
                            Message = privilegasMessage
                        };
                        return (JsonConvert.SerializeObject(responseObj));
                    }

                }
                else if (model.commandType.ToLower() == "searchsocpsyandbehaviorhx")
                {
                    privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("History_Social, Psychological and Behavior Hx", "SEARCH")).ToString();
                    if (string.IsNullOrEmpty(privilegasMessage))
                    {
                        response = null;
                        response = helperSocPsyandBehaviorHx.SearchSocPsyandBehaviorHx(model);
                    }
                    else
                    {
                        var responseObj = new
                        {
                            status = false,
                            Message = privilegasMessage
                        };
                        return (JsonConvert.SerializeObject(responseObj));
                    }
                }
                else if (model.commandType.ToLower() == "detach_socpsyandbehaviorhx_from_notes")
                {
                    response = helperSocPsyandBehaviorHx.detachSocPsyandBehaviorHxFromNotes(Convert.ToInt64(model.SocialandBehaviorHxId), Convert.ToInt64(model.NotesId));
                }
            }

            return response;

        }

    }
}
