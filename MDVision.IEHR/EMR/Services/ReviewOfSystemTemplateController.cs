/* Author: ZeeshanAK
 * OverView: Created to handel Review Of System
 * Date : January 26, 2016
 */

using MDVision.Business.BCommon;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewOfSystem;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.IEHR.EMR.Model.ReviewofSystems;
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
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems;
using MDVision.Common.Utilities;

namespace MDVision.IEHR.EMR.Services
{
    public class ReviewOfSystemTemplateController : ApiController
    {

        /// Author: ZeeshanAK
        /// This functions is created for Review Of System
        /// Date: January 26, 2016
        [HttpPost]
        public string ReviewOfSystemTemplate(JObject AllData)
        {
            string response = null;
            ClinicalReviewOfSystemTemplateHelper rosTemplateHelper = new ClinicalReviewOfSystemTemplateHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            ROSTemplateWrapperModel model = ser.Deserialize<ROSTemplateWrapperModel>(MDVUtility.ToStr(AllData["data"]));
            if (model.commandType.ToLower() == "load_ros_template_systems")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = rosTemplateHelper.loadROSTemplateSystems(MDVUtility.ToInt64(model.ROSTemplateId));
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
            else if (model.commandType.ToLower() == "load_ros_systems_characteristics")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = rosTemplateHelper.loadROSTemplateSystemsCharc(model.ROSTemplateId, model.ROSSystemId);
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
            else if (model.commandType.ToLower() == "search_ros_systems_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = rosTemplateHelper.loadROSTemplates(model.IsActive, MDVUtility.ToInt64(model.ROSTemplateId), MDVUtility.ToInt64(model.PageNumber), MDVUtility.ToInt64(model.RowsPerPage), MDVUtility.ToInt64(model.EntityId));
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
            else if (model.commandType.ToLower() == "fill_ros_systems_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = rosTemplateHelper.fillROSTemplates(MDVUtility.ToInt64(model.ROSTemplateId));
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
            else if (model.commandType.ToLower() == "update_clinical_rostemplate_active_inactive")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Int64 ROSTemplateId = MDVUtility.ToInt64(model.ROSTemplateId);
                    Int64 IsActive = MDVUtility.ToInt64(model.IsActive);
                    Int64 EntityId = MDVUtility.ToInt64(model.EntityId);
                    response = rosTemplateHelper.updateClinical_ROSTemplateIsActive(ROSTemplateId, IsActive, EntityId);
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
            else if (model.commandType.ToLower() == "delete_clinical_rostemplate")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Int64 ROSTemplateId = MDVUtility.ToInt64(model.ROSTemplateId);
                    response = rosTemplateHelper.deleteClinical_ROSTemplate(ROSTemplateId);
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
                Int64 ROSTemplateId = MDVUtility.ToInt64(model.ROSTemplateId);
                string privilegasMessage = string.Empty;
                if (ROSTemplateId>0)
                {
                     privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System", "EDIT")).ToString();
                }
                else
                {
                     privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System", "ADD")).ToString();
                }
                
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    
                    response = rosTemplateHelper.saveClinical_ROSTemplate(model);
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