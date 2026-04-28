using MDVision.Business.BCommon;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.IEHR.EMR.Model.Medical;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Model.Clinical.Immunization;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.Business.BLL;
using Newtonsoft.Json;
using MDVision.Model.Clinical.Immunization;
using MDVision.Model.Clinical.Medical.Vitals;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.Clinical.Medical.ProblemLists;

namespace MDVision.IEHR.EMR.Services
{
    public class MedicalController : ApiController
    {
        public static HttpContext context1;
        [HttpGet]
        public void LogOut()
        {
            //ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString() + new Random().ToString().Replace(".", ""), "alert('ahmed');", true);

            //_Default page=new _Default();
            //page.unloadModal();
            Dictionary<String, String> students = new Dictionary<String, String>()
            {
                { "value", "12"}
            };

            JavaScriptSerializer src = new JavaScriptSerializer();
            string json = src.Serialize(students);
            HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>UnloadActionPan('Clinical_ProblemLists', 'DRFirst');</SCRIPT>");

        }
        [HttpPost]
        public string Vitals(JObject AllData)
        {
            string response = null;
            List<object> lstBloodPressureModel = null;
            List<object> lstPulseModel = null;
            List<object> lstTemperatureModel = null;
            List<object> lstRespirationModel = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            VitalModel model = ser.Deserialize<VitalModel>(MDVUtility.ToStr(AllData["data"]));
            Dictionary<string, dynamic> dictCurrentVitalsJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            if (dictCurrentVitalsJSON.ContainsKey("BloodPressureIds"))
            {
                string BloodPressureIds = dictCurrentVitalsJSON["BloodPressureIds"];	//"Template,-1,-2"	dynamic {string}                
                lstBloodPressureModel = GetListOfObject("BloodPressureModel", BloodPressureIds, dictCurrentVitalsJSON);
            }
            if (dictCurrentVitalsJSON.ContainsKey("PulseIds"))
            {
                string PulseIds = dictCurrentVitalsJSON["PulseIds"];	//"Template,-1,-2"	dynamic {string}
                lstPulseModel = GetListOfObject("PulseModel", PulseIds, dictCurrentVitalsJSON);
            }
            if (dictCurrentVitalsJSON.ContainsKey("TempratureIds"))
            {
                string TempratureIds = dictCurrentVitalsJSON["TempratureIds"];	//"Template,-1,-2"	dynamic {string}
                lstTemperatureModel = GetListOfObject("TemperatureModel", TempratureIds, dictCurrentVitalsJSON);
            }
            if (dictCurrentVitalsJSON.ContainsKey("RespirationIds"))
            {
                string RespirationIds = dictCurrentVitalsJSON["RespirationIds"];	//"Template,-1,-2"	dynamic {string}
                lstRespirationModel = GetListOfObject("RespirationModel", RespirationIds, dictCurrentVitalsJSON);
            }

            VitalsHelper helperVitals = new VitalsHelper();


            if (model.commandType.ToLower() == "search_vitals")
            {
                response = null;
                response = helperVitals.LoadVitals(model, MDVUtility.ToInt32(model.VitalSignsId), MDVUtility.ToInt32(model.PageNo), MDVUtility.ToInt32(model.rpp));
            }
            else if (model.commandType.ToLower() == "save_vitals")
            {
                response = null;
                response = helperVitals.SaveVitals(model, lstBloodPressureModel, lstPulseModel, lstTemperatureModel, lstRespirationModel);
            }
            else if (model.commandType.ToLower() == "save_vitals_from_grid")
            {
                response = null;
                response = helperVitals.SaveVitals(model, lstBloodPressureModel, lstPulseModel, lstTemperatureModel, lstRespirationModel, true);
            }
            else if (model.commandType.ToLower() == "delete_vitals")
            {
                response = null;
                response = helperVitals.DeleteVitals(model, MDVUtility.ToInt64(model.VitalSignsId));
            }
            else if (model.commandType.ToLower() == "fill_vitals")
            {
                response = null;
                response = helperVitals.FillVitals(model, MDVUtility.ToInt64(model.VitalSignsId));
            }
            else if (model.commandType.ToLower() == "update_vitals")
            {
                response = null;
                response = helperVitals.UpdateVitals(model, MDVUtility.ToInt64(model.VitalSignsId), lstBloodPressureModel, lstPulseModel, lstTemperatureModel, lstRespirationModel);
            }
            else if (model.commandType.ToLower() == "update_vitals_from_grid")
            {
                response = null;
                response = helperVitals.UpdateVitals(model, MDVUtility.ToInt64(model.VitalSignsId), lstBloodPressureModel, lstPulseModel, lstTemperatureModel, lstRespirationModel, true);
            }
            else if (model.commandType.ToLower() == "getlatest_vitalby_patientid")
            {
                response = null;
                response = helperVitals.getLatestVitalByPatientId(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.UserId), MDVUtility.ToInt64(model.EntityId));
            }
            else if (model.commandType.ToLower() == "get_vitalsigns_forsoap")
            {
                response = null;
                response = helperVitals.getVitalSignsForSoap(model.VitalSignsId);
            }
            else if (model.commandType.ToLower() == "update_vitals_activeinactive")
            {
                response = null;
                response = helperVitals.updateVitalsActiveInActive(model);
            }
            else if (model.commandType.ToLower() == "copy_vitalsigns")
            {
                response = null;
                response = helperVitals.copyVitalSigns(MDVUtility.ToInt32(model.VitalSignsId), MDVUtility.ToInt32(model.NotesId));
            }
            return response;

        }

        public string Notes(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            VitalModel model = ser.Deserialize<VitalModel>(MDVUtility.ToStr(AllData["data"]));

            VitalsHelper helperVitals = new VitalsHelper();


            if (model.commandType.ToLower() == "search_notes")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperVitals.searchVisitsNotes(model);
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
            else if(model.commandType.ToLower() == "search_notes_count")
            {
                response = helperVitals.NotesCount();
            }

            return response;

        }

        private List<object> GetListOfObject(string objectType, string selectedIds, Dictionary<string, dynamic> dictCurrentJSON)
        {

            Type CurrentModel = null;
            List<object> lstObjects = new List<object>();
            if (objectType == "BloodPressureModel")
            {
                CurrentModel = typeof(BloodPressureModel);
            }
            else if (objectType == "PulseModel")
            {
                CurrentModel = typeof(PulseModel);
            }
            else if (objectType == "RespirationModel")
            {
                CurrentModel = typeof(RespirationModel);//
            }
            else if (objectType == "TemperatureModel")
            {
                CurrentModel = typeof(TemperatureModel);
            }
            PropertyInfo[] ArrCurrentModelPropertyInfo = CurrentModel.GetProperties();
            foreach (string item in selectedIds.Split(','))
            {
                if (item != "" && item.ToLower() != "template")
                {
                    object currentObject = null;
                    if (objectType == "BloodPressureModel")
                    {
                        currentObject = new BloodPressureModel();
                    }
                    else if (objectType == "PulseModel")
                    {
                        currentObject = new PulseModel();
                    }
                    else if (objectType == "RespirationModel")
                    {
                        currentObject = new RespirationModel();
                    }
                    else if (objectType == "TemperatureModel")
                    {
                        currentObject = new TemperatureModel();
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

        public string downloadProblemList(JObject AllData)
        {
            string response = null;

            ProblemListHelper helperProblemList = new ProblemListHelper();
            response = null;
            return response;
        }
        //Start//Muhammad Ahmad Imran//01/14/2015//Rcopia Controller
        public string Rcopia(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            RcopiaModel model = ser.Deserialize<RcopiaModel>(MDVUtility.ToStr(AllData["data"]));

            RcopiaModel modelRcopia = new RcopiaModel();

            RcopiaHelper helperRcopia = new RcopiaHelper();

            //string PatientID, string StartScreen, string rcopiaportal_system_name, string rcopiapractice_user_name, string rcopiapatient_system_name, string secret_key
            if (model.commandType.ToLower() == "get_ssourl")
            {
                List<RcopiaModel> ListRcopia = helperRcopia.GetRcopiaInfo();
                string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;
                DSRcopia dsRcopia = new DSRcopia();
                BLObject<DSRcopia> obj = new BLLRcopia().SelectGetUrls();
                dsRcopia = obj.Data;
                if (obj.Data != null)
                {
                    response = null;
                    dynamic GetRcopiaUserNameResponse = JObject.Parse(helperRcopia.GetRcopiaUserName());

                    if (GetRcopiaUserNameResponse.status == "True")
                    {
                        if (GetRcopiaUserNameResponse.UserName != "-1")
                        {
                            if (dsRcopia.Rcopia_GetUrl != null && dsRcopia.Rcopia_GetUrl.Rows.Count > 0)
                            {

                                string url = MDVUtility.GetSsoUrl(model.PatientId, model.StartupScreen, RcopiaVendorUsername, RcopiaPracticeUserName, RcopiaPortalSystemName, RcopiaScretkey, GetRcopiaUserNameResponse.UserName.ToString(), MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.WebBrowserURLColumn.ColumnName]), RcopiaScretkey);
                                var response1 = new
                                {
                                    status = true,
                                    DrFirstUrl = url,
                                };
                                response = (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                            }
                            else
                            {
                                var response1 = new
                                {
                                    status = false,
                                    Message = "Error In Get Rcopia WebBrowzer Url",
                                };
                                response = (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                            }

                        }
                        else
                        {
                            var response1 = new
                            {
                                status = false,
                                Message = "Rcopia UserName Not Found",
                            };
                            response = (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                        }
                    }
                    else
                    {
                        var response1 = new
                        {
                            status = false,
                            Message = GetRcopiaUserNameResponse.Message,
                        };
                        response = (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                    }
                }
                else
                {
                    var response1 = new
                    {
                        status = false,
                        Message = "Error In Get Rcopia WebBrowzer Url",
                    };
                    response = (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                }

            }
            //else if (model.commandType.ToLower() == "downloadallclinicals")
            //{
            //    response = null;
            //    response = helperRcopia.DownloadAllClinicals(model);
            //}
            else if (model.commandType.ToLower() == "downloadallclinicals")
            {
                response = null;
                response = helperRcopia.DownloadAllClinicalDataFromDrFirst(model);
            }
            else if (model.commandType.ToLower() == "downloadclinicalsforlimpmode")
            {
                List<RcopiaModel> ListRcopia = helperRcopia.GetRcopiaInfo();
                model.RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                model.RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                model.RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                model.RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                model.RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;

                response = null;
                response = helperRcopia.DownloadClinicalsForLIMPMode(model);
            }
            else if (model.commandType.ToLower() == "check_patient_register_on_drfirst")
            {
                response = null;
                response = helperRcopia.CheckPatientIsRegisteredOnDrFirs(model);
            }

            else if (model.commandType.ToLower() == "check_user_have_rcopia_rights")
            {
                response = null;
                response = helperRcopia.CheckUserHaveRcopiaRights(model);
            }

            return response;

        }
        //End//Muhammad Ahmad Imran//01/14/2015//Rcopia Controller
        public string ProblemList(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ProblemListModel model = ser.Deserialize<ProblemListModel>(MDVUtility.ToStr(AllData["data"]));

            ProblemListHelper helperProblemList = new ProblemListHelper();
            string privilegasMessage = "";

            if (model.commandType.ToLower() == "search_problemlist")
            {
                privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Problems List", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helperProblemList.LoadProblemListOp(model);
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
            if (model.commandType.ToLower() == "search_previous_problemlists")
            {
                PreRequestModel RequestModel = new PreRequestModel();
                RequestModel = new PreRequests().ApplicationServerContent();

                Dictionary<string, string> ResponseList = new Dictionary<string, string>();
                ResponseList.Add(MDVisionConstants.RequestModel, JsonConvert.SerializeObject(RequestModel));

                if (RequestModel.IsLogIn)
                {
                    response = new ProblemListHelper().LoadPreviousProblemLists(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
                }
                return JsonConvert.SerializeObject(ResponseList);
            }
            else if (model.commandType.ToLower() == "save_problemlist")
            {
                privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Problems List", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helperProblemList.SaveProblemListOp(model);
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
            else if (model.commandType.ToLower() == "save_problemlist_unique")
            {
                response = null;
                response = helperProblemList.SaveProblemListOpUnique(model);
            }
            else if (model.commandType.ToLower() == "save_problem_details")
            {
                response = null;
                response = helperProblemList.SaveProblemDetails(model);
            }
            else if (model.commandType.ToLower() == "calljs")
            {
                response = null;
                //ScriptManager.RegisterStartupScript(this, this., Guid.NewGuid().ToString() + new Random().ToString().Replace(".", ""), "alert('ahmed');", true);
            }
            else if (model.commandType.ToLower() == "update_problemlist")
            {
                response = null;
                response = helperProblemList.UpdateProblemListOp(model);
            }
            else if (model.commandType.ToLower() == "updateproblemindrfirstforgrid")
            {
                response = null;
                response = helperProblemList.UpdateProblemInDrFirstForGrid();
            }
            else if (model.commandType.ToLower() == "update_problemlistcomments")
            {
                response = null;
                response = helperProblemList.UpdateProblemListComments(model);
            }
            else if (model.commandType.ToLower() == "delete_problemlist")
            {
                response = null;
                response = helperProblemList.DeleteProblemListOp(model);
            }
            else if (model.commandType.ToLower() == "deleteproblemfromdrfirst")
            {
                response = null;
                response = helperProblemList.DeleteProblemFromDrFirst();
            }
            else if (model.commandType.ToLower() == "inactive_problemlist")
            {
                response = null;
                response = helperProblemList.ActiveInActiveProblemListOp(model);
            }
            else if (model.commandType.ToLower() == "inactive_problemlistfromdrfirst")
            {
                response = null;
                response = helperProblemList.ActiveInActiveProblemListONDrFirst();
            }

            else if (model.commandType.ToLower() == "getlatest_problemlistby_patientid")
            {
                response = null;
                response = helperProblemList.getLatestProblemListsByPatientId(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.UserId), MDVUtility.ToInt64(model.EntityId));
            }
            else if (model.commandType.ToLower() == "loadlookups")
            {
                response = null;
                response = helperProblemList.LoadLookups();
            }
            else if (model.commandType.ToLower() == "load_cancer_codes")
            {
                response = null;
                response = helperProblemList.LoadCancerCodes();
            }
            else if (model.commandType.ToLower() == "load_details")
            {
                response = null;
                response = helperProblemList.LoadCancerDiseaseDetails(model);
            }
            else if (model.commandType.ToLower() == "getlatest_chronicproblemlistby_patientid")
            {
                response = null;
                response = helperProblemList.getLatestChronicProblemListsByPatientId(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.UserId), MDVUtility.ToInt64(model.EntityId));
            }

            else if (model.commandType.ToLower() == "get_problemlists_forsoap")
            {
                response = null;
                response = helperProblemList.getProblemListsForSoap(model.ProblemListId);
            }
            else if (model.commandType.ToLower() == "get_previousproblemlists_forsoap")
            {
                response = null;
                response = helperProblemList.getpreviousProblemListsForSoap(model.ProblemListId);
            }
            else if (model.commandType.ToLower() == "detach_problemlists_from_notes")
            {
                response = helperProblemList.detach_ProblemList_From_Notes(model.ProblemListId, MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "attach_problemlists_with_notes")
            {
                response = helperProblemList.attach_ProblemList_With_Notes(model.ProblemListId, MDVUtility.ToInt64(model.NoteId));
            }
            //Start || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list
            else if (model.commandType.ToLower() == "get_all_problemlists")
            {
                response = helperProblemList.getAllProblemLists(model);
            }
            //else if (model.commandType.ToLower() == "save_problemlist_on_drfirst")
            //{
            //    response = helperProblemList.SaveProblemOnDrFirst(model);
            //}

            //End   || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list

            else if (model.commandType.ToLower() == "fill_problems_dropdown_forreports")
            {
                response = helperProblemList.getAllProblemsforReports();
            }

            else if (model.commandType.ToLower() == "fill_chronicity_dropdown_forreports")
            {
                response = helperProblemList.GetProblemChronicityLookup();
            }

            else if (model.commandType.ToLower() == "fill_severity_dropdown_forreports")
            {
                response = helperProblemList.GetProblemSeverityLookup();
            }
            //Start 27-10-2016 Humaira Yousaf to log view action for problem lists
            else if (model.commandType.ToLower() == "logviewproblemlist")
            {
                response = null;
                response = helperProblemList.LogProblemListView(model);
            }
            else if (model.commandType.ToLower() == "update_problem_order")
            {
                response = null;
                response = helperProblemList.UpdateProblemsOrder(model);
            }
            else if (model.commandType.ToLower() == "attach_problem_with_notes_load_soap")
            {
                response = helperProblemList.attachProblemWithNotesAndLoadSOAP(model.ProblemListId, MDVUtility.ToInt64(model.NoteId));
            }
            //End 27-10-2016 Humaira Yousaf to log view action for problem lists
            return response;
        }
        //By : Khaleel Ur Rehman.
        public string Procedure(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ProceduresModel model = ser.Deserialize<ProceduresModel>(MDVUtility.ToStr(AllData["data"]));
            List<ProceduresDetailModel> modelList = ser.Deserialize<List<ProceduresDetailModel>>(MDVUtility.ToStr(AllData["data"]));
            ProceduresDetailModel detailModel = ser.Deserialize<ProceduresDetailModel>(MDVUtility.ToStr(AllData["data"]));

            ProceduresHelper helper = new ProceduresHelper();
            string privilegasMessage = "";
            if (model.commandType.ToLower() == "search_procedures")
            {
                privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    //response = helper.loadProcedures_Obsolete(model);
                    response = helper.loadProcedures(model);
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
            else if (model.commandType.ToLower() == "save_procedures")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.saveProcedure(model);
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
            else if (model.commandType.ToLower() == "save_proceduresForVaccine")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.saveProcedureForVaccine(model);
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
            else if (model.commandType.ToLower() == "update_procedure")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.updateProcedures(model.procedureDetailModel);
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
            else if (model.commandType.ToLower() == "delete_procedure")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.deleteProcedure(detailModel);
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
            /*else if (model.commandType.ToLower() == "delete_procedure")
            {
                response = null;
                response = helper.deleteProcedure(model);
            }*/
            /*else if (model.commandType.ToLower() == "calljs")
            {
                response = null;
                //ScriptManager.RegisterStartupScript(this, this., Guid.NewGuid().ToString() + new Random().ToString().Replace(".", ""), "alert('ahmed');", true);
            }
            else if (model.commandType.ToLower() == "update_problemlist")
            {
                response = null;
                response = helper.UpdateProblemList(model);
            }
            else if (model.commandType.ToLower() == "update_problemlistcomments")
            {
                response = null;
                response = helper.UpdateProblemListComments(model);
            }
            else if (model.commandType.ToLower() == "delete_problemlist")
            {
                response = null;
                response = helperProblemList.DeleteProblemList(model);
            }*/
            else if (model.commandType.ToLower() == "inactive_procedures")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.ActiveInActiveProcedures(model);
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
            else if (model.commandType.ToLower() == "getlatest_proceduresby_patientid")
            {
                response = null;
                response = helper.getLatestProceduresByPatientId(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.UserId), MDVUtility.ToInt64(model.EntityId), MDVUtility.ToInt64(model.ProviderId));
            }
            else if (model.commandType.ToLower() == "get_procedures_forsoap")
            {
                response = null;
                response = helper.getProceduresForSoap(model.ProcedureId, model.PatientId, model.ProviderId);
            }
            else if (model.commandType.ToLower() == "detach_procedures_from_notes")
            {
                response = helper.detach_Procedure_From_Notes(model.ProcedureId, model.NotesId, model.ForVBP);
            }
            else if (model.commandType.ToLower() == "attach_procedures_with_notes")
            {
                response = helper.attach_Procedure_With_Notes(model.ProcedureId, model.NotesId);
            }
            else if (model.commandType.ToLower() == "is_phq_procedure")
            {
                response = helper.isPHQProcedure(model.ProcedureId, model.PatientId, model.ProviderId, model.NotesId);
            }
            else if (model.commandType.ToLower() == "calculate_vbp_socre")
            {
                response = helper.CalculateVBPSocreForSoapText(model.NotesId, model.PHQTextNeeded);
            }

            // Trace.WriteLine(model.commandType.ToLower() + " controller done at:" + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond);
            return response;
        }
        //Start//M Ahmad Imran//01/12/2015//Cheif Complaint Controller
        public string Complaint(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ComplaintModel model = ser.Deserialize<ComplaintModel>(MDVUtility.ToStr(AllData["data"]));

            MedicationsHelper helperComplaint = new MedicationsHelper();

            string privilegasMessage = "";
            if (model.commandType.ToLower() == "save_complaint")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Complaints", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helperComplaint.SaveComplaint(model);
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
            if (model.commandType.ToLower() == "update_complaint_from_notes")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Complaints", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helperComplaint.UpdateComplaintFromNotes(model);
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

            else if (model.commandType.ToLower() == "load_complaint")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Complaints", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helperComplaint.LoadComplaint(model);
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
            else if (model.commandType.ToLower() == "get_complaints_forsoap")
            {
                response = helperComplaint.getComplaintForSoap(MDVUtility.ToLong(model.ComplaintId), MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "attach_complaints_with_notes")
            {
                response = helperComplaint.attach_Complaint_With_Notes(MDVUtility.ToLong(model.ComplaintId), MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "detach_complaints_from_notes")
            {
                response = helperComplaint.detach_Complaint_From_Notes(MDVUtility.ToLong(model.ComplaintId), MDVUtility.ToLong(model.NotesId));
            }



            else if (model.commandType.ToLower() == "delete_complaint")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Complaints", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helperComplaint.DeleteComplaint(model);
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
            else if (model.commandType.ToLower() == "reset_complaint")
            {
                response = null;
                response = helperComplaint.ResetComplaint(model);
            }
            else if (model.commandType.ToLower() == "load_searchallcomplaints")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Complaints", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helperComplaint.load_SearchAllComplaints(model);
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
            if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }
        //Start//Ahmad Raza//01/12/2015//Allergy Controller
        public string Allergy(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            AllergyModel model = ser.Deserialize<AllergyModel>(MDVUtility.ToStr(AllData["data"]));

            AllergyHelper helperAllergy = new AllergyHelper();



            if (model.commandType.ToLower() == "search_allergy")
            {
                //Start 26-10-2016 Humaira Yousaf for logging of view action
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                bool isViewed = arrJSON.ContainsKey("isViewed") == true ? Convert.ToBoolean((arrJSON["isViewed"])) : false;

                //  response = helperAllergy.loadAllergy_Obsolete(model, isViewed);
                response = helperAllergy.loadAllergy(model, isViewed);
                //End 26-10-2016 Humaira Yousaf for logging of view action
            }
            if (model.commandType.ToLower() == "search_allergy_for_cds")
            {

                response = helperAllergy.loadAllergyForCDS(string.Empty);
            }

            //else if (model.commandType.ToLower() == "save_allergy")
            //{

            //    response = helperAllergy.saveAllergy(model);
            //}
            else if (model.commandType.ToLower() == "update_allergy")
            {

                response = helperAllergy.updateAllergy(model);
            }
            else if (model.commandType.ToLower() == "update_allergycomments")
            {

                response = helperAllergy.updateAllergyComments(model);
            }
            else if (model.commandType.ToLower() == "delete_allergy")
            {

                response = helperAllergy.deleteAllergy(model);
            }
            else if (model.commandType.ToLower() == "inactive_allergy")
            {

                response = helperAllergy.activeInActiveAllergy(model);
            }
            else if (model.commandType.ToLower() == "getlatest_allergiesby_patientid")
            {
                response = null;
                response = helperAllergy.getLatestAllergyByPatientId(MDVUtility.ToInt64(model.PatientId), model.AllergyId, MDVUtility.ToInt64(model.UserId), MDVUtility.ToInt64(model.EntityId));
            }
            else if (model.commandType.ToLower() == "get_allergies_forsoap")
            {
                response = helperAllergy.getAllergiesForSoap(model.AllergyId);
            }
            else if (model.commandType.ToLower() == "detach_allergies_from_notes")
            {
                response = helperAllergy.detach_Allergy_From_Notes(model.AllergyId, MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "attach_allergies_with_notes")
            {
                response = helperAllergy.attach_Allergy_With_Notes(model.AllergyId, MDVUtility.ToInt64(model.NoteId));
            }
            //Start || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list
            else if (model.commandType.ToLower() == "lookup_allergies")
            {
                response = helperAllergy.getAllAllergies(model);
            }
            //End   || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list
            else if (model.commandType.ToLower() == "fill_allergies_dropdown_forreports")
            {
                response = helperAllergy.getAllAllergiesforReports();
            }
            else if (model.commandType.ToLower() == "fill_reactions_dropdown_forreports")
            {
                response = helperAllergy.getAllReactionsforReports();
            }
            else if (model.commandType.ToLower() == "loadallergiesreviewedby")
            {
                response = helperAllergy.loadAllergiesReviewedBy_Obsolete(model);
            }

            return response;
        }
        //End//Ahmad Raza//01/12/2015//Allergy Controller
        /// <summary>
        /// Added by Zeeshan Ahmed for Prescription on 13 Jan, 2016
        /// </summary>
        /// <param name="AllData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Prescriptions(JObject AllData)
        {
            string response = string.Empty;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            PrescriptionsModel model = ser.Deserialize<PrescriptionsModel>(MDVUtility.ToStr(AllData["data"]));
            string privilegasMessage = string.Empty;
            if (model.commandType.ToLower() == "search_prescriptions")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Medications", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    //Start 07-11-2016 Humaira Yousaf for db audit
                    Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                    bool isViewed = arrJSON.ContainsKey("isViewed") == true ? Convert.ToBoolean((arrJSON["isViewed"])) : false;

                    response = MedicationsHelper.Instance().loadPrescriptions(model, isViewed);
                    //End 07-11-2016 Humaira Yousaf for db audit
                }

            }
            if (model.commandType.ToLower() == "search_prescriptions_for_print")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Prescription", "PRINT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                    response = MedicationsHelper.Instance().LoadPrescriptionsForPrint(model);
            }

            else if (model.commandType.ToLower() == "get_prescription_forsoap")
            {
                response = MedicationsHelper.Instance().loadPrescriptionsForSoap(model.PrescriptionIDForSoap, MDVUtility.ToLong(model.NoteId));
            }
            else if (model.commandType.ToLower() == "getlatest_prescriptionsby_patientid")
            {
                response = MedicationsHelper.Instance().getLatestPrescriptionByPatientId(model.PatientID, model.NotesId);
            }
            else if (model.commandType.ToLower() == "detach_prescriptions_from_notes")
            {
                response = MedicationsHelper.Instance().detachPrescriptionsFromNotes(model.PrescriptionIDForSoap, model.NotesId);
            }
            else if (model.commandType.ToLower() == "attach_prescriptions_with_notes")
            {
                response = MedicationsHelper.Instance().attachPrescriptionsWithNotes(model.PrescriptionIDForSoap, model.NotesId);
            }

            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }
        // End of Zeeshan's code for Prescription

        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function to load Medications.
        /// Date : 14 january 2016
        /// </summary>
        /// <param name="AllData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Medications(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            MedicationModel model = ser.Deserialize<MedicationModel>(MDVUtility.ToStr(AllData["data"]));
            string privilegasMessage = string.Empty;

            if (model.commandType.ToLower() == "search_medications")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Medications", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    //Start 26-10-2016 Humaira Yousaf for logging of view action
                    Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                    bool isViewed = arrJSON.ContainsKey("isViewed") == true ? Convert.ToBoolean((arrJSON["isViewed"])) : false;

                    // response = MedicationsHelper.Instance().loadMedications_Obsolete(model, isViewed);
                    response = MedicationsHelper.Instance().loadMedications(model, isViewed);
                    //End 26-10-2016 Humaira Yousaf for logging of view action
                }

            }
            else if (model.commandType.ToLower() == "loadmedicationsreviewedby")
            {
                response = MedicationsHelper.Instance().loadMedicationsReviewd(model);
            }
            else if (model.commandType.ToLower() == "update_routeid_bymedicationid")
            {
                response = MedicationsHelper.Instance().UpdateRouteIdByMedicationId(model);
            }
            else if(model.commandType.ToLower() == "update_negationreasonid_bymedicationid")
            {
                response = MedicationsHelper.Instance().UpdateNegationReasonIdByMedicationId(model);
            }
            if (model.commandType.ToLower() == "search_all_medications")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Medications", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                    bool isViewed = arrJSON.ContainsKey("isViewed") == true ? Convert.ToBoolean((arrJSON["isViewed"])) : false;
                    response = MedicationsHelper.Instance().loadAllMedications(model, isViewed);
                }

            }
            if (model.commandType.ToLower() == "search_medications_for_cds")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Medications", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = MedicationsHelper.Instance().loadMedicationsForCDS(string.Empty);
                }

            }

            else if (model.commandType.ToLower() == "get_medications_forsoap")
            {
                response = MedicationsHelper.Instance().getMedicationsForSoap(model.MedicationIDs, MDVUtility.ToLong(model.PatientID));
            }
            else if (model.commandType.ToLower() == "get_medications_forreconciledview")
            {
                response = MedicationsHelper.Instance().getMedicationsForNoteReconciledView(MDVUtility.ToLong(model.PatientID), MDVUtility.ToLong(model.NoteId));
            }
            else if (model.commandType.ToLower() == "attach_medications_with_notes")
            {
                response = MedicationsHelper.Instance().attachMedicationsWithNotes(model.MedicationIDs, MDVUtility.ToLong(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_medications_from_notes")
            {
                response = MedicationsHelper.Instance().detachMedicationsFromNotes(model.MedicationIDs, MDVUtility.ToLong(model.NoteId));
            }

            else if (model.commandType.ToLower() == "getlatest_medicationsby_patientid")
            {
                response = MedicationsHelper.Instance().getLatestMedicationsByPatientId(MDVUtility.ToInt64(model.PatientID), MDVUtility.ToInt64(model.UserId), MDVUtility.ToInt64(model.EntityId));
            }
            //Start || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list
            else if (model.commandType.ToLower() == "lookup_medications")
            {
                response = MedicationsHelper.Instance().getAllMedications(model);
            }
            else if (model.commandType.ToLower() == "lookup_medications_report")
            {
                response = MedicationsHelper.Instance().LookupMedicationsReprot();
            }
            //End   || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list
            else if (model.commandType.ToLower() == "get_report_header_footer")
            {
                response = MedicationsHelper.Instance().getReportHeaderFooter(model);
            }

            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }

        public string PatientEducation(JObject allData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            PatientEducationModel model = ser.Deserialize<PatientEducationModel>(MDVUtility.ToStr(allData["data"]));

            PatientEducationHelper helperPatientEducation = new PatientEducationHelper();

            if (model.commandType.ToLower() == "load_patienteducation")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Patient Education", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    //response = helperPatientEducation.loadClinical_PatientEducation_Obsolete(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), model.DocType, MDVUtility.ToInt64(model.NoteId));
                    response = helperPatientEducation.loadClinical_PatientEducation(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), model.DocType, MDVUtility.ToInt64(model.NoteId));
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
            else if (model.commandType.ToLower() == "insert_patienteducation")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Patient Education", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperPatientEducation.InsertClinical_PatientEducation(model);
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
            else if (model.commandType.ToLower() == "delete_patienteducation")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Patient Education", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperPatientEducation.DeleteClinical_PatientEducation(MDVUtility.ToInt64(model.PatientEducationId), MDVUtility.ToInt32(model.DocumentId));
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
            else if (model.commandType.ToLower() == "lookup_patienteducation")
            {
                response = helperPatientEducation.LookupClinical_PatientEducation(model.DocumentName);
            }
            //Start Humaira Yousaf 28-07-2016 for soap text
            else if (model.commandType.ToLower() == "getlatest_patienteducationby_patientid")
            {
                response = null;
                response = helperPatientEducation.getlatestPatientEducationByPatientId(MDVUtility.ToInt64(model.PatientId));
            }
            else if (model.commandType.ToLower() == "get_pateducation_forsoap")
            {
                response = null;
                response = helperPatientEducation.getPatientEducationSOAP(MDVUtility.ToInt64(model.PatientId), model.PatientEducationId, MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_patienteducation_from_notes")
            {
                response = helperPatientEducation.detachPatientEducationFromNotes(model.PatientEducationId, MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "attach_patienteducation_with_notes")
            {
                response = helperPatientEducation.attachPatientEducationWithNotes(model.PatientEducationId, MDVUtility.ToInt64(model.NoteId));
            }
            //End Humaira Yousaf 28-07-2016 for soap text
            return response;

        }


        public string IMMUNIZATIONTHERAPEUTICINJECTION(JObject allData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            TherapeuticInjectionModel model = ser.Deserialize<TherapeuticInjectionModel>(MDVUtility.ToStr(allData["data"]));
            string privilegesMessage = string.Empty;
            ImmunizationHelper helperImmunization = new ImmunizationHelper();

            if (model.commandType.ToLower() == "save_therapeutic_injection")
            {
                response = null;
                response = helperImmunization.Save_Therapeutic_Injection(model);
            }
            else if (model.commandType.ToLower() == "search_immunization_therapeutic_injection")
            {
                response = null;
                response = helperImmunization.SearchImmunizationTherapeuticInjection(model);
            }
            else if (model.commandType.ToLower() == "update_therapeutic_injection")
            {
                response = null;
                response = helperImmunization.Update_Therapeutic_Injection(model);
            }
            else if (model.commandType.ToLower() == "get_procedureid_against_therapeutic_injectionid")
            {
                response = null;
                response = helperImmunization.GetProcedureIdAgainstTherapeuticInjectionId(model);
            }
            else if (model.commandType.ToLower() == "attach_thera_injection_with_notes")
            {
                response = helperImmunization.attachTheraInjectionwithnotes(model.ImmTherInjectionId, MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "detach_thera_injection_with_notes")
            {
                privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperImmunization.detachTheraInjectionwithnotes(model.ImmTherInjectionId, MDVUtility.ToLong(model.NotesId));
                }
            }
            else if (model.commandType.ToLower() == "get_cpt_codeandadministeredcode")
            {
                response = helperImmunization.GetCptCodeAndAdministeredCode(MDVUtility.ToInt16(model.TherapeuticInjectionId));
            }


            if (!string.IsNullOrEmpty(privilegesMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegesMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
                return response;

        }

        //Start//Talha Tanweer//22/03/2016//Immunization Controller

        public string Immunization(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ImmunizationModel model = ser.Deserialize<ImmunizationModel>(MDVUtility.ToStr(AllData["data"]));
            ImmunizationQueryModel Querymodel = ser.Deserialize<ImmunizationQueryModel>(MDVUtility.ToStr(AllData["data"]));
            ImmunizationQueryResponseModel QueryResponsemodel = ser.Deserialize<ImmunizationQueryResponseModel>(MDVUtility.ToStr(AllData["data"]));
            ImmunizationHelper helperImmunization = new ImmunizationHelper();
            string privilegasMessage = "";

            if (model.commandType.ToLower() == "search_immunization")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.loadParentChildImmunization(model);
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
            else if (model.commandType.ToLower() == "get_visdate_and_visurl")
            {
                response = helperImmunization.GetVISDateAndVisURL(model);
            }
            else if (model.commandType.ToLower() == "get_visdate")
            {
                response = helperImmunization.GetVISDate(model);
            }

            else if (model.commandType.ToLower() == "insert_administervaccine")
            {
                model.IsVaccineInsert = true;
                response = helperImmunization.InsertAministerVaccine(model);
            }
            else if (model.commandType.ToLower() == "update_administer_vaccine")
            {
                model.IsVaccineInsert = true;
                response = helperImmunization.AdministerVaccineUpdate(model);
            }
            //else if (model.commandType.ToLower() == "save_administervaccine")
            //{
            //    response = helperImmunization.SaveAministerVaccine(model);
            //}

            //else if (model.commandType.ToLower() == "save_vacinehxdose")
            //{
            //    response = helperImmunization.SaveAministerVaccine(model);
            //}
            //else if (model.commandType.ToLower() == "save_vacinerefusalrecord")
            //{
            //    response = helperImmunization.SaveAministerVaccine(model);
            //}
            else if (model.commandType.ToLower() == "load_vaccine")
            {
                response = helperImmunization.LoadVaccine(model);
            }

            else if (model.commandType.ToLower() == "get_userme")
            {
                response = helperImmunization.GetUserME_IdandName(model);
            }

            else if (model.commandType.ToLower() == "get_visurl")
            {
                response = helperImmunization.GetVIS_URL(model);
            }

            else if (model.commandType.ToLower() == "search_vacinehxdose")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.loadAdministerVaccine(model);
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

            //else if (model.commandType.ToLower() == "update_vacinehxdose")
            //{
            //    response = helperImmunization.updateAdministerVaccine(model);
            //}
            //else if (model.commandType.ToLower() == "update_vacinerefusalrecord")
            //{
            //    response = helperImmunization.updateAdministerVaccine(model);
            //}
            else if (model.commandType.ToLower() == "detach_vaccine_from_notes")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.detach_Vaccine_From_Notes(model.VaccineHxIds, MDVUtility.ToLong(model.NotesId));
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
            else if (model.commandType.ToLower() == "attach_vaccine_with_notes")
            {
                response = helperImmunization.attach_Vaccine_With_Notes(model.VaccineHxIds, MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "getlatest_immunizationby_patientid")
            {
                response = helperImmunization.getLatestVaccineByPatientId(MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.UserId));
            }

            else if (model.commandType.ToLower() == "generate_hl7_immunization")
            {
                ImmunizationHL7Helper helperImmunizationHl7 = new ImmunizationHL7Helper(model.VaccineHxIds);
                response = helperImmunizationHl7.Generate_HL7Immunization_Message();

            }
            else if (model.commandType.ToLower() == "inactive_vaccinehx")
            {
                response = null;
                response = helperImmunization.ActiveInActiveVaccine(model);
            }
            else if (model.commandType.ToLower() == "load_searchschedlerdata")
            {
                response = null;
                response = helperImmunization.load_SearchSchedlerData(model);
            }
            else if (model.commandType.ToLower() == "getpreviewschedulerdata")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helperImmunization.GetPreviewSchedulerData(model);
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
            else if (model.commandType.ToLower() == "preview_immunization")
            {
                response = null;
                response = helperImmunization.Preview_Immunization(model);
            }
            else if (model.commandType.ToLower() == "get_immunizationalertcount")
            {
                response = null;
                response = helperImmunization.Get_ImmunizationAlertCount(model);
            }
            else if (model.commandType.ToLower() == "search_immunization_alerts")
            {
                response = null;
                response = helperImmunization.Search_Immunization_Alerts(model);


            }
            else if (model.commandType.ToLower() == "insertorupdatepatientimmunizationalert")
            {
                response = null;
                response = helperImmunization.InsertOrUpdatePatientImmunizationAlert(MDVUtility.ToLong(model.PatientId), model.IsVaccineInsert);


            }
            else if (model.commandType.ToLower() == "getimmunizationalertforprint")
            {
                response = null;
                response = helperImmunization.GetImmunizationAlertForPrint(model);
            }
            else if (model.commandType.ToLower() == "fill_immunization_dropdown_forreports")
            {
                response = null;
                response = helperImmunization.getAllImmunizationLookupforReports();
            }
            else if (model.commandType.ToLower() == "fill_vaccine_dropdown_forreports")
            {

                response = null;
                response = helperImmunization.GetVaccineDropdownForReports(model);
            }
            else if (model.commandType.ToLower() == "load_immunization_report")
            {

                response = null;
                response = helperImmunization.GetVaccineDropdownForReports(model);
            }
            else if (model.commandType.ToLower() == "get_provider_id")
            {
                response = null;
                response = helperImmunization.GetProviderId(model);
            }
            else if (model.commandType.ToLower() == "check_patient_insurance_is_medicare")
            {
                response = null;
                response = helperImmunization.CheckPatientInsuranceIsMedicare(model);
            }
            else if (model.commandType.ToLower() == "is_last_administered_does")
            {
                response = null;
                response = helperImmunization.IsLastAdministeredDoes(model);
            }
            else if (model.commandType.ToLower() == "is_administration_period_over")
            {
                response = null;
                response = helperImmunization.IsAdministrationPeriodOver(model);
            }
            else if (model.commandType.ToLower() == "get_cpt_of_vaccine")
            {
                response = null;
                response = helperImmunization.GetCptOfVaccine(model);
            }

            else if (model.commandType.ToLower() == "get_vaccineschedulerid")
            {
                response = null;
                response = helperImmunization.GetVaccineSchedulerId(model);
            }
            else if (model.commandType.ToLower() == "get_vaccinehxids")
            {
                response = null;
                response = helperImmunization.GetVaccineHxIds(model);
            }
            else if (model.commandType.ToLower() == "get_procedureids_against_vaccandimm")
            {
                response = null;
                response = helperImmunization.GetProcedureIdsAgainstVaccAndImm(model);
            }
            else if (model.commandType.ToLower() == "get_lot_manufanucture")
            {
                response = null;
                response = helperImmunization.GetLotManufanucture(model);
            }
            else if (model.commandType.ToLower() == "why_lot_is_not_available")
            {
                response = null;
                response = helperImmunization.WhyLotIsNotAvailable(model);
            }
            else if (model.commandType.ToLower() == "send_query")
            {
                response = null;
                response = helperImmunization.SendQuery(Querymodel);
            }
            else if (model.commandType.ToLower() == "search_immunization_query")
            {
                response = null;
                response = helperImmunization.LoadImmQuery(Querymodel);
            }
            else if (model.commandType.ToLower() == "search_immunization_query_response")
            {
                response = null;
                response = helperImmunization.LoadImmQueryResponse(QueryResponsemodel);
            }
            
            else if (model.commandType.ToLower() == "save_immresponse_file")
            {
                response = null;
                response = helperImmunization.SaveImmResponseFile(QueryResponsemodel);
            }
            else if(model.commandType.ToLower()== "search_queryresponsepatient")
            {
                response = null;
                response = helperImmunization.LoadImmQueryResponsePatient(QueryResponsemodel);
            }
            else if (model.commandType.ToLower() == "search_queryresponsehx")
            {
                response = null;
                response = helperImmunization.SearchQueryResponseHX(QueryResponsemodel);
            }
            else if (model.commandType.ToLower() == "search_queryresponseforecast")
            {
                response = null;
                response = helperImmunization.SearchQueryResponseForecast(QueryResponsemodel);
            }
            else if (model.commandType.ToLower() == "delete_immunizationqueryresponse")
            {
                response = null;
                response = helperImmunization.DeleteImmunizationQueryResponse(QueryResponsemodel);
            }
            else if (model.commandType.ToLower() == "delete_immunizationquery")
            {
                response = null;
                response = helperImmunization.DeleteImmunizationQuery(Querymodel);
            }
            else if (model.commandType.ToLower() == "addtohxtab")
            {
                response = null;
                response = helperImmunization.AddToHxTab(QueryResponsemodel);
            }
            else if (model.commandType.ToLower() == "save_immacknow_file")
            {
                response = null;
                response = helperImmunization.SaveImmAcknowledgementFile(QueryResponsemodel);
            }
            else if (model.commandType.ToLower() == "search_vaccine_hx_id")
            {
                response = null;
                response = helperImmunization.LoadVaccineHxById(Querymodel);
            }
            else if (model.commandType.ToLower() == "get_vaccshedid_against_agelim_sche_categ")
            {
                response = helperImmunization.GetVaccineScheduleId(model);
            }
            return response;
        }
        public string ImmunizationRegistery(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ImmunizationRegistery model = null;

            if (!string.IsNullOrEmpty(MDVUtility.ToStr(AllData["data"])))
            {
                try
                {
                    model = ser.Deserialize<ImmunizationRegistery>(MDVUtility.ToStr(AllData["data"]));
                }

                catch (Exception ex)
                {
                    model = new ImmunizationRegistery();
                }
            }
            ImmunizationHelper helperImmunization = new ImmunizationHelper();
            string privilegasMessage = "";

            if (model.commandType.ToLower() == "search_immunization_registery")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Registery", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.Search_Immunization_Registery(model);
                }
            }
            if (model.commandType.ToLower() == "save_immunization_registery")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Registery", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperImmunization.SaveImmunizationRegistery(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            if (model.commandType.ToLower() == "update_immunization_registery")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Registery", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperImmunization.UpdateImmunizationRegistery(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            if (model.commandType.ToLower() == "delete_immunization_registery")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Registery", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperImmunization.DeleteImmunizationRegistery(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }

            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }

        //Start//Talha Tanweer//22/03/2016//Immunization Controller

        public string LotNumber(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            LotNumberModel model = ser.Deserialize<LotNumberModel>(MDVUtility.ToStr(AllData["data"]));

            ImmunizationHelper helperImmunization = new ImmunizationHelper();


            if (model.commandType.ToLower() == "get_all_vaccines")
            {

                return response = helperImmunization.GetAllVaccines(model);
            }
            else if (model.commandType.ToLower() == "get_all_manufacturers")
            {

                return response = helperImmunization.GetAllManufacturers(model);
            }


            return response;
        }


        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function to handle Immunization Category Operations.
        public string ImmunizationCategory(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            CategoryModel model = ser.Deserialize<CategoryModel>(MDVUtility.ToStr(AllData["data"]));

            ImmunizationHelper helperImmunization = new ImmunizationHelper();


            if (model.commandType.ToLower() == "get_all_categories")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Category", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.LoadImmunizationCategory(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "activeinactive_category")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Category", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.ActiveInActiveCategory(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "delete_category")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Category", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.DeleteCategory(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "save_immunizationcategory")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Category", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.SaveImmunizationCategory(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "update_immunizationcategory")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Category", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.UpdateImmunizationCategory(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            return response;
        }



        //End//Talha Tanweer//22/03/2016//Immunization Controller
        public string Manufacturer(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Manufacturer model = ser.Deserialize<Manufacturer>(MDVUtility.ToStr(AllData["data"]));

            ImmunizationHelper helperImmunization = new ImmunizationHelper();
            string privilegasMessage = "";

            if (model.commandType.ToLower() == "get_manufacturer_array")
            {

                response = helperImmunization.GetManufacturerArray(model);
            }
            else if (model.commandType.ToLower() == "load_manufacturer")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Manufacturer", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.LoadManufacturer(model);
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
            else if (model.commandType.ToLower() == "save_manufacturer")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Manufacturer", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.SaveManufacturer(model);
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
            else if (model.commandType.ToLower() == "load_manufacturer_detail")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Manufacturer", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.LoadManufacturerDetail(model);
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
            else if (model.commandType.ToLower() == "update_manufacturer")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Manufacturer", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.UpdateManufacturer(model);
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

            else if (model.commandType.ToLower() == "delete_manufacturer")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Manufacturer", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.DeleteManufacturer(model);
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
            else if (model.commandType.ToLower() == "activeorinactivemanufacturer")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Manufacturer", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.ActiveOrInactiveManufacturer(model);
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
                var responseObj = new
                {
                    status = false,
                    Message = "Not Find Command",
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            return response;
        }
        public string AddVaccineAndTherapeutic(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            VaccineAndTerapuetic model = ser.Deserialize<VaccineAndTerapuetic>(MDVUtility.ToStr(AllData["data"]));

            ImmunizationHelper helperImmunization = new ImmunizationHelper();
            string privilegasMessage = "";

            if (model.commandType.ToLower() == "get_immandthera_array")
            {

                return response = helperImmunization.GetImmAndTheraArray(model);
            }
            else if (model.commandType.ToLower() == "load_vaccine_and_therapeutic")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Add Imm/Inj", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.loadVaccineAndTherapeutic(model);
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
            else if (model.commandType.ToLower() == "save_vaccine")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Add Imm/Inj", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.SaveVaccine(model);
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
            else if (model.commandType.ToLower() == "save_therapeutic")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Add Imm/Inj", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.SaveTherapeutic(model);
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

            else if (model.commandType.ToLower() == "load_vaccine_detail")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Add Imm/Inj", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.LoadVaccineDetail(model);
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
            else if (model.commandType.ToLower() == "getvaccineinformationforautopopu")
            {
                return response = helperImmunization.GetVaccineInformationForAutoPopu(model);
            }

            else if (model.commandType.ToLower() == "load_therapeutic_detail")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Add Imm/Inj", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.LoadTherapeuticDetail(model);
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

            else if (model.commandType.ToLower() == "update_vaccine")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Add Imm/Inj", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.UpdateVaccine(model);
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
            else if (model.commandType.ToLower() == "activeorinactiveimmorthera")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Add Imm/Inj", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.ActiveOrInactiveImmOrThera(model);
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

            else if (model.commandType.ToLower() == "update_therapeutic")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Add Imm/Inj", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    return response = helperImmunization.UpdateTherapeutic(model);
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

            else if (model.commandType.ToLower() == "delete_immunization_or_therapeutic")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Add Imm/Inj", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    if (model.Type == "Immunization")
                    {
                        response = helperImmunization.DeleteImmunization(model);
                    }
                    else
                    {
                        response = helperImmunization.DeleteTherapeutic(model);
                    }
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
            else if (model.commandType.ToLower() == "delete_vaccinevis")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Add Imm/Inj", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.DeleteVISInformation(model);
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

            return response;
        }

        /// Author:Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function to handle Immunization VaccineCrosswalk Operations.
        public string ImmunizationVaccineCrosswalk(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            VaccineCrosswalk model = ser.Deserialize<VaccineCrosswalk>(MDVUtility.ToStr(AllData["data"]));

            ImmunizationHelper helperImmunization = new ImmunizationHelper();


            if (model.commandType.ToLower() == "get_all_vaccinecrosswalks")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Vaccine Crosswalk", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.LoadImmunizationVaccineCrosswalk(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "activeinactive_vaccinecrosswalk")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Vaccine Crosswalk", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.ActiveInActiveVaccineCrosswalk(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "delete_vaccinecrosswalk")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Vaccine Crosswalk", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.DeleteVaccineCrosswalk(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "save_immunizationvaccinecrosswalk")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Vaccine Crosswalk", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.SaveImmunizationVaccineCrosswalk(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "update_immunizationvaccinecrosswalk")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Vaccine Crosswalk", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.UpdateImmunizationVaccineCrosswalk(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            return response;
        }

        /// Author:Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function to handle Vaccine
        public string Vaccines(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Vaccine model = ser.Deserialize<Vaccine>(MDVUtility.ToStr(AllData["data"]));

            ImmunizationHelper helperImmunization = new ImmunizationHelper();


            if (model.commandType.ToLower() == "get_all_vaccines")
            {

                return response = helperImmunization.LoadVaccineList(model);
            }
            return response;
        }
        public string ImmunizationScheduleSetup(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ImmunizationModel model = ser.Deserialize<ImmunizationModel>(MDVUtility.ToStr(AllData["data"]));

            ImmunizationHelper helperImmunization = new ImmunizationHelper();


            if (model.commandType.ToLower() == "get_all_schedulesetup")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Schedule Setup", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.LoadImmunizationScheduleSetup(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "activeinactive_schedulesetup")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Schedule Setup", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.ActiveInActiveScheduleSetup(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "delete_schedulesetup")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Schedule Setup", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.DeleteScheduleSetup(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "save_immunizationschedulesetup")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Schedule Setup", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.SaveImmunizationScheduleSetup(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "update_immunizationschedulesetup")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Schedule Setup", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    return response = helperImmunization.UpdateImmunizationScheduleSetup(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "setpriority")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Schedule Setup", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperImmunization.SetPriority(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            return response;
        }

        public string ImmunizationLotNumber(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            LotNumberModel model = ser.Deserialize<LotNumberModel>(MDVUtility.ToStr(AllData["data"]));

            ImmunizationHelper helperImmunization = new ImmunizationHelper();
            string privilegasMessage = "";

            if (model.commandType.ToLower() == "get_all_lotnumber")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Lot Management", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.LoadImmunizationLotNumber(model);
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
            else if (model.commandType.ToLower() == "activeinactive_lotnumber")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Lot Management", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.ActiveInActiveLotNumber(model);
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
            else if (model.commandType.ToLower() == "delete_lotnumber")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Lot Management", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.DeleteLotNumber(model);
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
            else if (model.commandType.ToLower() == "save_immunizationlotnumber")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Lot Management", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.SaveImmunizationLotNumber(model);
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
            else if (model.commandType.ToLower() == "update_immunizationlotnumber")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Lot Management", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperImmunization.UpdateImmunizationLotNumber(model);
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
            if (model.commandType.ToLower() == "get_lotnumber_by_id")
            {
                if (string.IsNullOrEmpty(model.Checkprivilegas))
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Immunization_Lot Management", "VIEW")).ToString();
                    if (string.IsNullOrEmpty(privilegasMessage))
                    {
                        response = helperImmunization.LoadImmunizationLotNumberByVaccineLotNoId(model);
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
                    response = helperImmunization.LoadImmunizationLotNumberByVaccineLotNoId(model);
                }

            }
            return response;
        }

        public string response { get; set; }
        [HttpPost]
        public string GetCDSDrugs(JObject objData)
        {
            string DrugSearch = HttpContext.Current.Request.Params["data"];
            
            response = MedicationsHelper.Instance().loadMedicationsForCDS(DrugSearch);
            return response;

        }

        [HttpPost]
        public string GetCDSAllergies(JObject objData)
        {
            string AllergenSearch = HttpContext.Current.Request.Params["data"];
            AllergyHelper helperAllergy = new AllergyHelper();
            response =  helperAllergy.loadAllergyForCDS(AllergenSearch);
            return response;

        }
        
        
    }


}
