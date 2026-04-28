using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Batch.PatientList;
using MDVision.IEHR.EMR.Helpers.Patient;
using MDVision.IEHR.EMR.Model.Batch.PatientList;
using MDVision.IEHR.EMR.Model.Patient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class PatientListController : ApiController
    {
        private BatchPatientListHelper batchPtHelper = null;
        private PatientReferralHelper helperPatientReferral = null;
        public PatientListController()
        {

        }


        [HttpPost]
        public string BatchPatientList(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            BatchPatientListModelSearch model = ser.Deserialize<BatchPatientListModelSearch>(MDVUtility.ToStr(AllData["data"]));
            batchPtHelper = new BatchPatientListHelper();

            if (model.commandType.ToLower() == "search_patient_list")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Batch_PatientList", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = batchPtHelper.loadBatchPatientList(model);
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
            else if (model.commandType.ToLower() == "print_patient_list")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Batch_PatientList", "PRINT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    string HTMLResponse = batchPtHelper.printBatchPatientList(model);
                    var responseHTML = new
                    {
                        status = true,
                        HTMLResponse = HTMLResponse
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseHTML));
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
            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }

        [HttpPost]
        // Author:  Humaira Yousaf
        // Created Date: 17-03-2016
        //OverView: Entry point for Procedure Orders
        public string IncomingOrOutgoingRefferal(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            PatientReferralModel model = JsonConvert.DeserializeObject<PatientReferralModel>(MDVUtility.ToStr(AllData["data"]));

            //Dictionary<string, dynamic> dictCurrentReferralProcedureJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            //PatientReferralProblemListModel modelReferralProcedure = ser.Deserialize<PatientReferralProblemListModel>(MDVUtility.ToStr(AllData["data"]));
            //List<object> lstReferralProcedureModel = null;
            //if (dictCurrentReferralProcedureJSON.ContainsKey("ProcedureIds"))
            //{
            //    string ReferralProcedureIds = dictCurrentReferralProcedureJSON["ProcedureIds"];
            //    lstReferralProcedureModel = GetListOfObject("ReferralProcedureModel", ReferralProcedureIds, dictCurrentReferralProcedureJSON);
            //}

            helperPatientReferral = new PatientReferralHelper();

            if (model.commandType.ToLower() == "save_referral")
            {
                response = helperPatientReferral.InsertUpdatePatientReferral(model);

            }
            else if (model.commandType.ToLower() == "search_referral")
            {
                response = helperPatientReferral.loadReferralData(model);
            }

            else if (model.commandType.ToLower() == "search_facility_by_shortname")
            {
                response = helperPatientReferral.SearchFacilityByName(model.ShortName);
            }
            else if (model.commandType.ToLower() == "search_provider_by_shortname")
            {
                response = helperPatientReferral.SearchProviderByName(model.ShortName);
            }
            else if (model.commandType.ToLower() == "search_specialty_by_shortname")
            {
                response = helperPatientReferral.SearchSpecialtyByName(model.ShortName);
            }
            else if (model.commandType.ToLower() == "search_refprovider_by_shortname")
            {
                response = helperPatientReferral.SearchRefProviderByName(model.ShortName);
            }

            else if (model.commandType.ToLower() == "inactive_referral")
            {
                response = null;
                response = helperPatientReferral.ActiveInActiveReferral(model);
            }
            else if (model.commandType.ToLower() == "delete_referral_procedure")
            {
                response = helperPatientReferral.deleteReferralProcedure(model.ReferralProcedureId);
            }
            else if (model.commandType.ToLower() == "search_problemlist")
            {
                response = helperPatientReferral.LoadProblemLists(model);
            }
            else if (model.commandType.ToLower() == "delete_referral")
            {
                response = helperPatientReferral.deleteReferral(model.ReferralId);
            }
            else if (model.commandType.ToLower() == "detach_referral_from_notes")
            {
                response = helperPatientReferral.detach_Referral_From_Notes(model.ReferralId, MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "attach_referral_with_notes")
            {
                response = helperPatientReferral.attach_Referral_With_Notes(model.ReferralId, MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "getlatest_referralby_patientid")
            {
                response = null;
                response = helperPatientReferral.getLatestReferralByPatientId(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.ProviderId));
            }
            else if (model.commandType.ToLower() == "get_referral_forsoap")
            {
                response = null;
                response = helperPatientReferral.getReferralForSoap(model.ReferralId, MDVUtility.ToInt32(model.PatientId), MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId));
            }
            else if (model.commandType.ToLower() == "preview_referral")
            {
                response = helperPatientReferral.previewReferral(model);
            }
            //Start 15-08-2016 Humaira Yousaf to get referral providers
            else if (model.commandType.ToLower() == "getreferringfromprovider")
            {
                response = helperPatientReferral.loadReferralProvider(model);
            }
            //End 15-08-2016 Humaira Yousaf to get referral providers
            else if (model.commandType.ToLower() == "load_attachments")
            {
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                long patientId = arrJSON.ContainsKey("PatientId") == true ? MDVUtility.ToInt64(arrJSON["PatientId"]) : 0;
                long transitionId = arrJSON.ContainsKey("TransitionId") == true ? MDVUtility.ToInt64(arrJSON["TransitionId"]) : 0;
                long OrderSetReferralId = arrJSON.ContainsKey("OrderSetReferralId") == true ? MDVUtility.ToInt64(arrJSON["OrderSetReferralId"]) : 0;
                string refModuleName = arrJSON.ContainsKey("RefModuleName") == true ? MDVUtility.ToStr(arrJSON["RefModuleName"]) : "";

                response = helperPatientReferral.loadIncomingReferraltAttachment(patientId, transitionId, refModuleName, OrderSetReferralId);
            }
            else if(model.commandType.ToLower() == "get_status_reasons"){
                response = helperPatientReferral.GetStatusReasons(MDVUtility.ToInt64(model.Status));
            }
            return response;
        }
        private List<object> GetListOfObject(string objectType, string selectedIds, Dictionary<string, dynamic> dictCurrentJSON)
        {
            Type CurrentModel = null;
            List<object> lstObjects = new List<object>();
            if (objectType == "ReferralProcedureModel")
            {
                CurrentModel = typeof(PatientReferralProcedureModel);
            }
            PropertyInfo[] ArrCurrentModelPropertyInfo = CurrentModel.GetProperties();
            foreach (string item in selectedIds.Split(','))
            {
                if (item != "" && item.ToLower() != "template")
                {
                    object currentObject = null;
                    if (objectType == "ReferralProcedureModel")
                    {
                        currentObject = new PatientReferralProcedureModel();
                    }
                    if (currentObject != null)
                    {
                        foreach (PropertyInfo CurrentProperty in ArrCurrentModelPropertyInfo)
                        {
                            try
                            {
                                if (item.Equals("0"))
                                {
                                    currentObject.GetType().GetProperty(CurrentProperty.Name).SetValue(currentObject, dictCurrentJSON[CurrentProperty.Name]);
                                }
                                else
                                {
                                    currentObject.GetType().GetProperty(CurrentProperty.Name).SetValue(currentObject, dictCurrentJSON[CurrentProperty.Name + item]);
                                }

                            }
                            catch (Exception ex)
                            {

                                //throw;
                            }

                        }
                        lstObjects.Add(currentObject);
                    }

                }
            }
            return lstObjects;

        }
    }
}