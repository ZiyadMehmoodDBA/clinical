/* Author:  Abid Ali
 * Created Date: 13/06/2016
 * OverView: Created to handel Physical Exam Data Template 
 */

using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.Data_Templates;
using MDVision.IEHR.EMR.Helpers.Clinical.PhysicalExam;
using MDVision.IEHR.EMR.Helpers.Clinical.Templates;
using MDVision.IEHR.EMR.Model.DataTemplates;
using MDVision.IEHR.EMR.Model.PhysicalExam;
using MDVision.IEHR.EMR.Model.Templates;
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
    public class PhysicalExamDataTemplateController : ApiController
    {

        [HttpPost]

        // Author:  Abid Ali
        // Created Date: 15/June/2016
        //OverView: Entry point for Physical Exam Data Template

        public string PhysicalExamDataTemplate(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            PhysicalExamDataTemplateModel model = ser.Deserialize<PhysicalExamDataTemplateModel>(MDVUtility.ToStr(AllData["data"]));

            PhysicalExamDataTemplateHelper helperPhysExamDateTemplate = new PhysicalExamDataTemplateHelper();
            if (model.commandType == "add_update_PhysExam_DataTemplate")
            {
                response = null;

                string privilegasMessage = "";// Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Physical Exam", "DELETE")).ToString();
                if (MDVUtility.ToInt64(model.DataTemplateId) < 1)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Physical Exam", "ADD")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Physical Exam", "Edit")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperPhysExamDateTemplate.insertUpdatePhysExamDataTemplate(model);
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
            else if (model.commandType.ToUpper() == "ACTIVE_INACTIVE_PHYSEXAM_DATATEMPLATE")
            {
                string privilegasMessage = privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Physical Exam", "Edit")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperPhysExamDateTemplate.activeInactivePhysExamDataTemplate(model);
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
            else if (model.commandType.ToUpper() == "DELETE_PHYSEXAM_DATATEMPLATE")
            {
                response = null;
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Data Template_Physical Exam", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperPhysExamDateTemplate.deletePhysExamDataTemplate(MDVUtility.ToInt64(model.DataTemplateId));
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
            else if (model.commandType == "Search_PhysExam_DataTemplate")
            {
                response = null;
                response = helperPhysExamDateTemplate.searchPhysExamDataTemplate(model);
            }
            else if (model.commandType == "Fill_PhysExam_DataTemplate")
            {
                response = null;
                response = helperPhysExamDateTemplate.fillPhysExamDataTemplate(model);
            }

            return response;
        }
    }
}