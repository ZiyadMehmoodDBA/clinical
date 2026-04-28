using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.Lab;
using MDVision.IEHR.EMR.Model.Lab;
using Newtonsoft.Json;
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
    public class ClinicalLabController : ApiController
    {
        [HttpPost]
        public string ClinicalLab(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ClinicalLabModel model = ser.Deserialize<ClinicalLabModel>(MDVUtility.ToStr(AllData["data"]));
            Dictionary<string, dynamic> dictCurrentROSJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            ClinicalLabHelper helper = new ClinicalLabHelper();

            if (model.commandType.ToLower() == "load_clinicallab")
            {
                string privilegesMessage = string.Empty;
                if (model.moduleName == "Admin")
                    privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Laboratory", "SEARCH")).ToString();

                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helper.loadClinicalLab(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            else if (model.commandType.ToLower() == "save_clinicallab")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Laboratory", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helper.insertUpdateClinicalLab(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            else if (model.commandType.ToLower() == "delete_clinicallab")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Laboratory", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helper.deleteClincialLab(model.LabId.ToString());
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            else if (model.commandType.ToLower() == "active_inactive_clinicallab")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Laboratory", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helper.activeInactiveLab(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(response));
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

        public string ClinicalLabTest(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ClinicalLabTestModel model = ser.Deserialize<ClinicalLabTestModel>(MDVUtility.ToStr(AllData["data"]));
            Dictionary<string, dynamic> dictCurrentROSJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            ClinicalLabHelper helper = new ClinicalLabHelper();

            if (model.commandType.ToLower() == "save_clinical_lab_test")
            {
                response = helper.insertClinicalLabTest(model);
            }
            else if (model.commandType.ToLower() == "load_clinical_lab_test_attributes")
            {
                response = helper.loadClinicalLabTestAttribuites(model);
            }
            else if (model.commandType.ToLower() == "load_clinical_lab_test")
            {
                response = helper.loadClinicalLabTest(model);
            }
            else if (model.commandType.ToLower() == "save_clinical_lab_test_attribute")
            {
                response = helper.insertClinicalLabTestAttribute(model);
            }
            else if (model.commandType.ToLower() == "edit_clinical_lab_test")
            {
                response = helper.editClinicalLabTestAttribute(model);
            }
            else if (model.commandType.ToLower() == "updateuomrange")
            {
                response = helper.updateUOMRange(model);
            }
            else if (model.commandType.ToLower() == "delete_clinical_lab_test_attribute")
            {
                response = helper.deleteLabTestAttribute(model);
            }
            else if (model.commandType.ToLower() == "delete_clinical_lab_test")
            {
                response = helper.deleteLabTest(model);
            }
            else if (model.commandType.ToLower() == "load_clinical_lab_test_and_attributes")
            {
                response = helper.loadClinicalLabTestAndAttributes(model);
            }
            else if (model.commandType.ToLower() == "update_active_inactive")
            {
                response = helper.saveTestActiveInActive(model);
            }
            else if (model.commandType.ToLower() == "load_labtestattribute_result")
            {
                response = helper.LoadLabTestAttributeResult(model);
            }
            else if (model.commandType.ToLower() == "insert_labtestattribute_result")
            {
                response = helper.SaveLabTestAttributeResult(model);
            }
            else if (model.commandType.ToLower() == "delete_labtestattribute_result")
            {
                response = helper.DeleteLabTestAttributeResult(model);
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