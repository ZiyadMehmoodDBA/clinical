/* Author: ZeeshanAK
 * OverView: Created to handel Review Of System
 * Date : January 26, 2016
 */

using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewOfSystem;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.IEHR.EMR.Model.ReviewofSystems;
using MDVision.Model.Clinical.ROS;
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
    public class ReviewOfSystemController : ApiController
    {

        /// Author: ZeeshanAK
        /// This functions is created for Review Of System
        /// Date: January 26, 2016
        [HttpPost]
        public string ReviewOfSystems(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ROSSystemsModel model = ser.Deserialize<ROSSystemsModel>(MDVUtility.ToStr(AllData["data"]));
            ROSSystemPatientCharacteristicsModel modelSystemPatientCharacteristics = ser.Deserialize<ROSSystemPatientCharacteristicsModel>(MDVUtility.ToStr(AllData["data"]));//kr
            ROSSystemsWrapperModel modelWrapper = ser.Deserialize<ROSSystemsWrapperModel>(MDVUtility.ToStr(AllData["data"]));
            Dictionary<string, dynamic> dictCurrentROSJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            var rosHelper = ClinicalReviewOfSystemHelper.Instance;


            if (model.commandType.ToLower() == "load_ros_systems")
            {
                response = rosHelper.loadROSSystems(model.ROSSystemInfoID, 0, model.NotesId, model.ROSTemplateId, model.ROSDataTemplateId);
            }
            else if (model.commandType.ToLower() == "load_and_save_ros_systems")
            {
                response = rosHelper.saveROSSystemPatientInfoFromNotes(model.NotesId, model.ROSDataTemplateId);
            }
            else if (model.commandType.ToLower() == "load_ros_systems_characteristics")
            {
                response = rosHelper.loadSystemCharacteristics(model.ROSSystemId, model.ROSSystemPatientID, model.ROSDataTemplateId);
            }
            else if (model.commandType.ToLower() == "save_reviewofsystems")
            {
                if (modelWrapper.isSaveAS == true)
                {
                    ReviewOfSystemDataTemplateHelper rosTemplateHelper = new ReviewOfSystemDataTemplateHelper();
                    ROSDataTemplateWrapperModel modelDT = ser.Deserialize<ROSDataTemplateWrapperModel>(MDVUtility.ToStr(AllData["data"]));
                    modelDT.SystemsWrapperModel = ser.Deserialize<ROSSystemsWrapperModel>(modelDT.SystemsWrapperString);
                    response = rosTemplateHelper.saveAsROSDataTemplate(modelDT);
                }
                else
                {
                    response = rosHelper.saveROSSystemPatientInfo(modelWrapper);
                }

            }
            else if (model.commandType.ToLower() == "load_ros_systems_patientinfo")
            {
                response = rosHelper.loadROSSystemInfo(model.ROSSystemInfoID, model.NotesId, model.ROSDataTemplateId);
            }
            else if (model.commandType.ToLower() == "load_characteristic_detail")
            {
                response = rosHelper.loadROSCharacteristicsDetails(model.PatientId, modelSystemPatientCharacteristics.ROSSystemPatientCharacteristicsID, 0, model.ROSDataTemplateId);//kr temp zero value
            }
            else if (model.commandType.ToLower() == "disassociate_systems_againstnoteid")
            {
                response = rosHelper.disAssociateSystemsAgainstNoteId(model.NotesId, model.ROSSystemInfoID);
            }
            else if (model.commandType.ToLower() == "getlatest_reviewofsystemsby_patientid")
            {
                response = rosHelper.loadROSSystemInfo(model.PatientId, model.ROSSystemInfoID, model.ROSDataTemplateId);
            }
            else if (model.commandType.ToLower() == "ros_system_patient_reset")
            {
                response = rosHelper.rOSSystemPatientReset(model.ROSSystemPatientID, true);//Temp hard code true value as asked by azharSialSb.
            }
            else if (model.commandType.ToLower() == "delete_characteristics_details")
            {
                response = rosHelper.deleteCharacteristicsDetails(modelSystemPatientCharacteristics.ROSSystemPatientCharacteristicsID, modelSystemPatientCharacteristics.RemoveSystemCharcDetails);
            }
            else if (model.commandType.ToLower() == "load_ros_template_systems")
            {
                response = rosHelper.loadROSTemplateSystems(model.ROSSystemInfoID, 0, model.NotesId, model.ROSTemplateId);
            }
            else if (model.commandType.ToLower() == "toggle_rossystempatientcharacteristics")
            {
                dynamic modelDT = ser.Deserialize<dynamic>(MDVUtility.ToStr(AllData["data"]));
                response = rosHelper.toggleROSSystemPatientCharacteristics(modelDT["ROSSystemPatientCharacteristicsID"], modelDT["IsPositive"]);
            }
            //else if (model.commandType.ToLower() == "update_reviewofsystems_user_info")
            //{
            //    response = rosHelper.updateROSSystemUserInfo(model.systemOrder);
            //}
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


      
        public string getSpecficKeyValue(Dictionary<string, dynamic> Data_Array, string getValue)
        {
            if (Data_Array.ContainsValue(getValue))
            {
                foreach (String key in Data_Array.Keys)
                {
                    if (Data_Array[key].Equals(getValue))
                        return key;
                }
            }
            return null;

        }
        public bool ContainsLoop(List<string> list, string value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == value)
                {
                    return true;
                }
            }
            return false;
        }
        public bool? ContainsSystemCharcLoopBool(Dictionary<string, dynamic> dictCurrentROSJSON, string value)
        {
            foreach (var item in dictCurrentROSJSON)
            {
                if (item.Key.Equals(value))
                {
                    return item.Value;
                }
            }

            return null;
        }

        public string ContainsSystemCharcLoopString(Dictionary<string, dynamic> dictCurrentROSJSON, string value)
        {
            foreach (var item in dictCurrentROSJSON)
            {
                if (item.Key.Equals(value))
                {
                    return item.Value;
                }
            }

            return string.Empty;
        }
    }
}


