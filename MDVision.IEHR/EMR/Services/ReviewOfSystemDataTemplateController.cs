using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using MDVision.IEHR.EMR.Model.ReviewofSystems;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class ReviewOfSystemDataTemplateController : ApiController
    {
        [HttpPost]
        public string ReviewOfSystemDataTemplate(JObject AllData)
        {
            string response = null;
            ReviewOfSystemDataTemplateHelper rosTemplateHelper = new ReviewOfSystemDataTemplateHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            ROSDataTemplateWrapperModel model = ser.Deserialize<ROSDataTemplateWrapperModel>(MDVUtility.ToStr(AllData["data"]));
            if (model.commandType.ToLower() == "search_ros_data_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Review of System", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = rosTemplateHelper.loadROSDataTemplates(model);
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
            else if (model.commandType.ToLower() == "update_clinical_rosdatatemplate_active_inactive")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Review of System", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Int64 ROSTemplateId = MDVUtility.ToInt64(model.ROSTemplateId);
                    Int64 ROSDataTemplateId = MDVUtility.ToInt64(model.ROSDataTemplateId);
                    Int64 IsActive = MDVUtility.ToInt64(model.IsActive);
                    Int64 EntityId = MDVUtility.ToInt64(model.EntityId);
                    response = rosTemplateHelper.updateClinical_ROSDataTemplateIsActive(ROSDataTemplateId, ROSTemplateId, IsActive, EntityId);
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
            else if (model.commandType.ToLower() == "delete_clinical_ros_data_rostemplate")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Review of System", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Int64 ROSDataTemplateId = MDVUtility.ToInt64(model.ROSDataTemplateId);
                    response = rosTemplateHelper.deleteClinical_ROSDataTemplate(ROSDataTemplateId);
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
            else if (model.commandType.ToLower() == "save_rostemplate")
            {
                Int64 ROSDataTempInfoId = MDVUtility.ToInt64(model.ROSDataTempInfoId);
                model.SystemsWrapperModel = ser.Deserialize<ROSSystemsWrapperModel>(MDVUtility.ToStr(model.SystemsWrapperString));
                string privilegasMessage = string.Empty;
                if (ROSDataTempInfoId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Review of System", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Review of System", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    if (model.isSaveAS == true)
                    {
                        response = rosTemplateHelper.saveAsROSDataTemplateInfo(model);
                    }
                    else
                    {
                        response = rosTemplateHelper.saveROSDataTemplateInfo(model);
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
            else if (model.commandType.ToLower() == "ros_data_system_reset")
            {
                string privilegasMessage = string.Empty;
                if (model.ROSDataTempInfoId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Review of System", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Review of System", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = rosTemplateHelper.rOSDataSystemReset(model.ROSDataSystemID, true);//Temp hard code true value as asked by azharSialSb.
                }
            }
            else if (model.commandType.ToLower() == "delete_ros_data_system_charc_details")
            {
                string privilegasMessage = string.Empty;
                if (model.ROSDataTempInfoId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Review of System", "EDIT")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = rosTemplateHelper.deleteClinical_ROSDataTemplate_CharcDetail(model.ROSDataSystemCharcID, model.RemoveSystemCharcDetails);//Temp hard code true value as asked by azharSialSb.
                }
            }
            else if (model.commandType.ToLower() == "load_ros_datatemplate_forprovider")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Review of System", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = rosTemplateHelper.loadROSForProvider(MDVUtility.ToInt64(model.ProviderId));
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
    }
}