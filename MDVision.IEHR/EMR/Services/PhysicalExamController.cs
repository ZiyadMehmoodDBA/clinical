/* Author:  Muhammad Arshad
 * Created Date: 04/02/2016
 * OverView: Created to handel Physical Exam
 */

using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.PhysicalExam;
using MDVision.IEHR.EMR.Model.PhysicalExam;
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

namespace MDVision.IEHR.EMR.Services
{
    public class PhysicalExamController : ApiController
    {
        [HttpPost]

        // Author:  Muhammad Arshad
        // Created Date: 04/02/2016
        //OverView: Entry point for Physical Exam

        public string PhysicalExam(JObject AllData)
        {
          
            Exception exp = null;

            string response = null;
            List<object> lstCharacteristicModel = null;
            List<object> lstSubCharacteristicModel = null;

            List<object> lstCharacteristicDetailModel = null;
            List<object> lstSubCharacteristicDetailModel = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            PhysicalExamSystemModel model = ser.Deserialize<PhysicalExamSystemModel>(MDVUtility.ToStr(AllData["data"]));
            Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
            string characteristicIds = arrJSON.ContainsKey("characteristicIds") == true ? MDVUtility.ToStr(arrJSON["characteristicIds"]) : "";
            string subcharacteristicIds = arrJSON.ContainsKey("subcharacteristicIds") == true ? MDVUtility.ToStr(arrJSON["subcharacteristicIds"]) : "";
            string characteristicdata = arrJSON.ContainsKey("characteristicdata") == true ? MDVUtility.ToStr(arrJSON["characteristicdata"]) : "";
            Dictionary<string, dynamic> charcteristicsJSON = ser.DeserializeObject(characteristicdata) as Dictionary<string, dynamic>;
            string subcharacteristicdata = arrJSON.ContainsKey("subcharacteristicdata") == true ? MDVUtility.ToStr(arrJSON["subcharacteristicdata"]) : "";
            Dictionary<string, dynamic> subcharcteristicsJSON = ser.DeserializeObject(subcharacteristicdata) as Dictionary<string, dynamic>;

            MDVLogger.BLLErrorLog("BLLClinical::LoadPhysicalExam -  after parsing", exp);



            if (characteristicIds != "")
            {
                lstCharacteristicModel = GetListOfObject("Characteristic", characteristicIds, charcteristicsJSON);
                lstCharacteristicDetailModel = GetListOfObject("CharacteristicDetail", characteristicIds, charcteristicsJSON);
            }

            if (subcharacteristicIds != "")
            {
                lstSubCharacteristicModel = GetListOfObject("SubCharacteristic", subcharacteristicIds, subcharcteristicsJSON);
                lstSubCharacteristicDetailModel = GetListOfObject("SubCharacteristicDetail", subcharacteristicIds, subcharcteristicsJSON);
            }
            //Start Farooq Ahmad 08/02/2016 following code will Deserialize the data into PatientPhysicalExamModel and PatientPhysicalExamSystemModel
            PatientPhysicalExamModel patientPhysicalExamModel = new PatientPhysicalExamModel();
            try
            {
                patientPhysicalExamModel = ser.Deserialize<PatientPhysicalExamModel>(MDVUtility.ToStr(AllData["data"]));
            }
            catch (Exception ex)
            {

            }
            PatientPhysicalExamSystemModel patientPhysicalExamSystemModel = ser.Deserialize<PatientPhysicalExamSystemModel>(MDVUtility.ToStr(AllData["data"]));
            //End Farooq Ahmad 08/02/2016 following code will Deserialize the data into PatientPhysicalExamModel and PatientPhysicalExamSystemModel

            //Start Farooq Ahmad 08/02/2016 following code will instantiate object of PhysicalExamHelper
            PhysicalExamHelper helperPhysicalExam = new PhysicalExamHelper();
            //End Farooq Ahmad 08/02/2016 following code will instantiate object of PhysicalExamHelper

            //Start Farooq Ahmad 08/02/2016 following code will update section order sorting
            if (model.commandType.ToLower() == "update_sectionordersorting")
            {
                helperPhysicalExam.savePhysicalExamUserSystem(model);
            }
            //End Farooq Ahmad 08/02/2016 following code will update section order sorting
            if (model.commandType.ToLower() == "fill_patientphysicalexam")
            {
                response = null;
                response = helperPhysicalExam.fillPatientPhysicalExam(patientPhysicalExamModel, Convert.ToInt64(patientPhysicalExamModel.PatientPhysicalExamId));


            }
            //Start//18-02-2016//Ahmad Raza//Commands added for save/update of Normal system's inner detail
            else if (model.commandType.ToLower() == "save_system_normal_detail")
            {
                response = null;

                response = helperPhysicalExam.savePatientPhysicalExam(patientPhysicalExamModel, lstCharacteristicModel = null, lstSubCharacteristicModel = null, lstCharacteristicDetailModel = null, lstSubCharacteristicDetailModel = null, patientPhysicalExamSystemModel);
            }
            else if (model.commandType.ToLower() == "update_system_normal_detail")
            {
                response = null;
                //response = helperPhysicalExam.updatePatientPhysicalExam(patientPhysicalExamModel);
                response = helperPhysicalExam.updatePatientPhysicalExam(patientPhysicalExamModel, MDVUtility.ToInt64(patientPhysicalExamModel.PatientPhysicalExamId), lstCharacteristicModel = null, lstSubCharacteristicModel = null, lstCharacteristicDetailModel = null, lstSubCharacteristicDetailModel = null, patientPhysicalExamSystemModel);
            }
            else if (model.commandType.ToLower() == "get_physicalexam_for_soap")
            {
                response = null;
                response = helperPhysicalExam.getPhysicalExamForSoap(patientPhysicalExamModel.PatientPhysicalExamId, MDVUtility.ToInt64(patientPhysicalExamModel.PatientId));
            }
            //End//18-02-2016//Ahmad Raza//Commands added for save/update of Normal system's inner detail
            else if (model.commandType.ToLower() == "save_patientphysicalexam")
            {
                response = null;
                response = helperPhysicalExam.savePatientPhysicalExam(patientPhysicalExamModel);
                //response = helperPhysicalExam.savePatientPhysicalExam(patientPhysicalExamModel, lstCharacteristicModel, lstSubCharacteristicModel, lstCharacteristicDetailModel, lstSubCharacteristicDetailModel,null, model.TemplateId);
            }
            else if (model.commandType.ToLower() == "update_patientphysicalexam")
            {
                response = null;
                //response = helperPhysicalExam.updatePatientPhysicalExam(patientPhysicalExamModel, MDVUtility.ToInt64(patientPhysicalExamModel.PatientPhysicalExamId), lstCharacteristicModel, lstSubCharacteristicModel, lstCharacteristicDetailModel, lstSubCharacteristicDetailModel, patientPhysicalExamSystemModel, model.TemplateId);
                response = helperPhysicalExam.updatePatientPhysicalExam(patientPhysicalExamModel);//, MDVUtility.ToInt64(patientPhysicalExamModel.PatientPhysicalExamId), lstCharacteristicModel, lstSubCharacteristicModel, lstCharacteristicDetailModel, lstSubCharacteristicDetailModel, patientPhysicalExamSystemModel, model.TemplateId);

            }
            //Start//12-02-2016//Ahmad Raza//calling helper methods to get latest physical exam,attach and detach physical exam from notes
            else if (model.commandType.ToLower() == "getlatest_physicalexamby_patientid")
            {
                response = null;
                response = helperPhysicalExam.fillPatientPhysicalExam(patientPhysicalExamModel, MDVUtility.ToInt64(patientPhysicalExamModel.PatientPhysicalExamId));

            }
            else if (model.commandType.ToLower() == "attach_physicalexam_with_notes")
            {
                response = helperPhysicalExam.attachPhysicalExamWithNotes(patientPhysicalExamModel, patientPhysicalExamModel.NotesId, model.TemplateId);
            }
            else if (model.commandType.ToLower() == "detach_physicalexam_from_notes")
            {
                response = helperPhysicalExam.detachPhysicalExamFromNotes(MDVUtility.ToInt64(patientPhysicalExamModel.PatientPhysicalExamId), patientPhysicalExamModel.NotesId);
            }
            //End//12-02-2016//Ahmad Raza//calling helper methods to get latest physical exam,attach and detach physical exam from notes

            //Start Farooq Ahmad 15/02/2016 calling method of helper to save and update patient physical exam 
            else if (model.commandType.ToLower() == "save_detailforcharacteristic")
            {
                response = null;
                object obj = ser.Deserialize<PatientPhysicalExamCharacteristicModel>(MDVUtility.ToStr(AllData["data"]));
                lstCharacteristicModel = new List<object>();
                lstCharacteristicModel.Add(obj);

                obj = ser.Deserialize<PatientPhysicalExamSubCharacteristicModel>(MDVUtility.ToStr(AllData["data"]));
                lstSubCharacteristicModel = new List<object>();
                if (((MDVision.IEHR.EMR.Model.PhysicalExam.PatientPhysicalExamSubCharacteristicModel)(obj)).SubCharacteristicId != null)
                    lstSubCharacteristicModel.Add(obj);

                response = helperPhysicalExam.savePatientPhysicalExam(patientPhysicalExamModel, lstCharacteristicModel, lstSubCharacteristicModel, lstCharacteristicDetailModel, lstSubCharacteristicDetailModel);
            }
            else if (model.commandType.ToLower() == "update_detailforcharacteristic")
            {
                response = null;
                object obj = ser.Deserialize<PatientPhysicalExamCharacteristicModel>(MDVUtility.ToStr(AllData["data"]));
                lstCharacteristicModel = new List<object>();
                lstCharacteristicModel.Add(obj);

                obj = ser.Deserialize<PatientPhysicalExamSubCharacteristicModel>(MDVUtility.ToStr(AllData["data"]));
                lstSubCharacteristicModel = new List<object>();
                if (((MDVision.IEHR.EMR.Model.PhysicalExam.PatientPhysicalExamSubCharacteristicModel)(obj)).SubCharacteristicId != null)
                    lstSubCharacteristicModel.Add(obj);
                response = helperPhysicalExam.updatePatientPhysicalExam(patientPhysicalExamModel, MDVUtility.ToInt64(patientPhysicalExamModel.PatientPhysicalExamId), lstCharacteristicModel, lstSubCharacteristicModel, lstCharacteristicDetailModel, lstSubCharacteristicDetailModel, patientPhysicalExamSystemModel);
            }
            //End Farooq Ahmad 15/02/2016 calling method of helper to save and update patient physical exam 

                // Start 18-02-2016 Humaira Yousaf deleted physical exam
            else if (model.commandType.ToLower() == "delete_patientphysicalexam")
            {
                Dictionary<string, dynamic> jsonData = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                long patientId = arrJSON.ContainsKey("patientId") == true ? MDVUtility.ToInt64(arrJSON["patientId"]) : 0;
                long patientPhysicaExamlId = arrJSON.ContainsKey("patientPhysicaExamlId") == true ? MDVUtility.ToInt64(arrJSON["patientPhysicaExamlId"]) : 0;
                long systemId = arrJSON.ContainsKey("systemId") == true ? MDVUtility.ToInt64(arrJSON["systemId"]) : 0;

                response = null;
                response = helperPhysicalExam.deletePatientPhysicalExam(patientId, patientPhysicaExamlId, systemId);
            }
            // End 18-02-2016 Humaira Yousaf deleted physical exam



             //Start//18-02-2016//Ahmad Raza//delete characteristics detail
            else if (model.commandType.ToLower() == "delete_characteristics")
            {
                Dictionary<string, dynamic> jsonData = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                long patientId = arrJSON.ContainsKey("patientId") == true ? MDVUtility.ToInt64(arrJSON["patientId"]) : 0;
                long patientPhysicaExamlId = arrJSON.ContainsKey("patientPhysicaExamlId") == true ? MDVUtility.ToInt64(arrJSON["patientPhysicaExamlId"]) : 0;
                long sectionId = arrJSON.ContainsKey("sectionId") == true ? MDVUtility.ToInt64(arrJSON["sectionId"]) : 0;
                long characteristicId = arrJSON.ContainsKey("characteristicId") == true ? MDVUtility.ToInt64(arrJSON["characteristicId"]) : 0;

                response = null;
                response = helperPhysicalExam.deletePatientPhysicalExamSystemSectionCharacteristic(patientPhysicaExamlId, sectionId, characteristicId);
            }
            //End//18-02-2016//Ahmad Raza//delete characteristics detail

                //Start//19-02-2016//Ahmad Raza//delete subcharacteristics detail
            else if (model.commandType.ToLower() == "delete_subcharacteristics")
            {
                Dictionary<string, dynamic> jsonData = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                long patientId = arrJSON.ContainsKey("patientId") == true ? MDVUtility.ToInt64(arrJSON["patientId"]) : 0;
                long patientPhysicaExamlId = arrJSON.ContainsKey("patientPhysicaExamlId") == true ? MDVUtility.ToInt64(arrJSON["patientPhysicaExamlId"]) : 0;
                long characteristicId = arrJSON.ContainsKey("characteristicId") == true ? MDVUtility.ToInt64(arrJSON["characteristicId"]) : 0;
                long subCharacteristicId = arrJSON.ContainsKey("subCharacteristicId") == true ? MDVUtility.ToInt64(arrJSON["subCharacteristicId"]) : 0;

                response = null;
                response = helperPhysicalExam.deletePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(patientPhysicaExamlId, characteristicId, subCharacteristicId);
            }
            //End//19-02-2016//Ahmad Raza//delete subcharacteristics detail


             // Start 19-02-2016 Humaira Yousaf deleted physical exam system section
            else if (model.commandType.ToLower() == "delete_patientphysicalexamsystemsection")
            {
                Dictionary<string, dynamic> jsonData = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                long systemId = arrJSON.ContainsKey("systemId") == true ? MDVUtility.ToInt64(arrJSON["systemId"]) : 0;
                long patientPhysicaExamlId = arrJSON.ContainsKey("patientPhysicaExamlId") == true ? MDVUtility.ToInt64(arrJSON["patientPhysicaExamlId"]) : 0;
                long sectionId = arrJSON.ContainsKey("sectionId") == true ? MDVUtility.ToInt64(arrJSON["sectionId"]) : 0;

                response = null;
                response = helperPhysicalExam.deletePatientPhysicalExamSystemSection(patientPhysicaExamlId, systemId, sectionId);
            }


            // End 19-02-2016 Humaira Yousaf deleted physical exam system section

            //Begin 26-02-2016 Added By Humaira Yousaf Bug# 377
            else if (model.commandType.ToLower() == "save_detailforexam")
            {
                response = null;
                object obj = ser.Deserialize<PatientPhysicalExamCharacteristicModel>(MDVUtility.ToStr(AllData["data"]));
                lstCharacteristicModel = new List<object>();
                lstCharacteristicModel.Add(obj);

                obj = ser.Deserialize<PatientPhysicalExamSubCharacteristicModel>(MDVUtility.ToStr(AllData["data"]));
                lstSubCharacteristicModel = new List<object>();
                if (((MDVision.IEHR.EMR.Model.PhysicalExam.PatientPhysicalExamSubCharacteristicModel)(obj)).SubCharacteristicId != null)
                    lstSubCharacteristicModel.Add(obj);

                response = helperPhysicalExam.savePatientPhysicalExam(patientPhysicalExamModel, lstCharacteristicModel, lstSubCharacteristicModel, lstCharacteristicDetailModel, lstSubCharacteristicDetailModel);
            }

            else if (model.commandType.ToLower() == "update_detailforexam")
            {
                response = null;
                object obj = ser.Deserialize<PatientPhysicalExamCharacteristicModel>(MDVUtility.ToStr(AllData["data"]));
                lstCharacteristicModel = new List<object>();
                lstCharacteristicModel.Add(obj);

                obj = ser.Deserialize<PatientPhysicalExamSubCharacteristicModel>(MDVUtility.ToStr(AllData["data"]));
                lstSubCharacteristicModel = new List<object>();
                if (((MDVision.IEHR.EMR.Model.PhysicalExam.PatientPhysicalExamSubCharacteristicModel)(obj)).SubCharacteristicId != null)
                    lstSubCharacteristicModel.Add(obj);
                response = helperPhysicalExam.updatePatientPhysicalExam(patientPhysicalExamModel, MDVUtility.ToInt64(patientPhysicalExamModel.PatientPhysicalExamId), lstCharacteristicModel, lstSubCharacteristicModel, lstCharacteristicDetailModel, lstSubCharacteristicDetailModel, patientPhysicalExamSystemModel);
            }
            //End 26-02-2016 Added By Humaira Yousaf Bug# 377
            else if (model.commandType.ToLower() == "toggle_physicalexamcharacteristics")
            {
                dynamic modelDT = ser.Deserialize<dynamic>(MDVUtility.ToStr(AllData["data"]));
                response = helperPhysicalExam.toggle_PhysicalExamCharacteristics(modelDT["PrimaryKeyId"], modelDT["IsPositive"], modelDT["type"]);
            }
            return response;
        }

        // Author:  Muhammad Arshad
        // Created Date: 12/02/2016
        //OverView: Get List of Objects from JSON
        private List<object> GetListOfObject(string objectType, string selectedIds, Dictionary<string, dynamic> dictCurrentJSON)
        {

            Type CurrentModel = null;
            List<object> lstObjects = new List<object>();
            if (objectType == "Characteristic")
            {
                CurrentModel = typeof(PatientPhysicalExamCharacteristicModel);
            }
            else if (objectType == "SubCharacteristic")
            {
                CurrentModel = typeof(PatientPhysicalExamSubCharacteristicModel);
            }
            else if (objectType == "CharacteristicDetail")
            {
                CurrentModel = typeof(PatientPhysicalExamCharacteristicDetailModel);//
            }
            else if (objectType == "SubCharacteristicDetail")
            {
                CurrentModel = typeof(PatientPhysicalExamSubCharacteristicDetailModel);
            }
            PropertyInfo[] ArrCurrentModelPropertyInfo = CurrentModel.GetProperties();
            foreach (string item in selectedIds.Split(','))
            {
                if (item != "" && item.ToLower() != "template")
                {
                    object currentObject = null;
                    if (objectType == "Characteristic")
                    {
                        currentObject = new PatientPhysicalExamCharacteristicModel();
                    }
                    else if (objectType == "SubCharacteristic")
                    {
                        currentObject = new PatientPhysicalExamSubCharacteristicModel();
                    }
                    else if (objectType == "CharacteristicDetail")
                    {
                        currentObject = new PatientPhysicalExamCharacteristicDetailModel();
                    }
                    else if (objectType == "SubCharacteristicDetail")
                    {
                        currentObject = new PatientPhysicalExamSubCharacteristicDetailModel();
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
                                    //Start//16-02-2016//Ahmad Raza//Logic to get text value of drop downs
                                    if (CurrentProperty.Name.IndexOf("_text") < 0)
                                        currentObject.GetType().GetProperty(CurrentProperty.Name).SetValue(currentObject, dictCurrentJSON[CurrentProperty.Name + item]);
                                    else
                                        currentObject.GetType().GetProperty(CurrentProperty.Name).SetValue(currentObject, dictCurrentJSON[CurrentProperty.Name.Replace("_text", "") + item + "_text"]);
                                    //End//16-02-2016//Ahmad Raza//Logic to get text value of drop downs
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

        //Author: Farooq Ahmad
        // Created Date: 22/02/2016
        //This will handle the request PhysicalExamUserSystem
        public string PhysicalExamUserSystem(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            PhysicalExamSystemModel model = ser.Deserialize<PhysicalExamSystemModel>(MDVUtility.ToStr(AllData["data"]));
            long physicalExamId = model.PatientPhysicalExamId;
            PhysicalExamHelper helperPhysicalExam = new PhysicalExamHelper();
            if (model.commandType == "PhysicalExam_UserSystemLoad")
            {
                response = null;
                response = helperPhysicalExam.fillPhysicalExamUserSystem(physicalExamId, model.TemplateId);
            }
            if (model.commandType.ToLower() == "update_sectionordersorting")
            {
                response = null;
                response = helperPhysicalExam.savePhysicalExamUserSystem(model);
            }
            return response;
        }


    }
}
