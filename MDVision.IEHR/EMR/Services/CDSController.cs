/* Author:  Muhammad Arshad
 * Created Date: 04/02/2016
 * OverView: Created to handel Physical Exam
 */

using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.CDS;
using MDVision.IEHR.EMR.Helpers.Clinical.PhysicalExam;
using MDVision.IEHR.EMR.Model.CDS;
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
    public class CDSController : ApiController
    {
        [HttpPost]

        // Author:  Muhammad Arshad
        // Created Date: 24/02/2016
        //OverView: Entry point for CDS

        /// <summary>
        /// Module Name: CDS
        /// Author: Humaira Yousaf
        /// Created Date: 02-03-2016
        /// Description: Handles different CDS methods
        /// </summary>
        /// <param name="AllData" type="JObject">contains CDSdata</param>
        public string CDS(JObject AllData)
        {
            string response = null;


            JavaScriptSerializer ser = new JavaScriptSerializer();

            CDSProblemListModel problemModel = null;

            CDSQuestionnaire questionnaireModel = null;

            CDSAllergyModel allergyModel = null;
            CDSMedicationModel medicationModel = null;
            CDSLabResultModel labResultModel = null;
            CDSVitalsModel vitalsModel = null;
            CDSInsuranceModel insuranceModel = null;
            CDSModel model = JsonConvert.DeserializeObject<CDSModel>(MDVUtility.ToStr(AllData["data"]));

            if (model.commandType.ToLower() == "delete_cds_problemlist")
            {
                problemModel = JsonConvert.DeserializeObject<CDSProblemListModel>(MDVUtility.ToStr(AllData["data"]));
            }
            if (model.commandType.ToLower() == "delete_cds_questionnaire")
            {
                questionnaireModel = JsonConvert.DeserializeObject<CDSQuestionnaire>(MDVUtility.ToStr(AllData["data"]));
            }
            if (model.commandType.ToLower() == "delete_cds_allergy")
            {
                allergyModel = JsonConvert.DeserializeObject<CDSAllergyModel>(MDVUtility.ToStr(AllData["data"]));
            }
            if (model.commandType.ToLower() == "delete_cds_medication")
            {
                medicationModel = JsonConvert.DeserializeObject<CDSMedicationModel>(MDVUtility.ToStr(AllData["data"]));
            }
            if (model.commandType.ToLower() == "delete_cds_labresult")
            {
                labResultModel = JsonConvert.DeserializeObject<CDSLabResultModel>(MDVUtility.ToStr(AllData["data"]));
            }
            if (model.commandType.ToLower() == "delete_cds_insurance")
            {
                insuranceModel = JsonConvert.DeserializeObject<CDSInsuranceModel>(MDVUtility.ToStr(AllData["data"]));
            }
            //if (model.commandType.ToLower() == "delete_cds_vitals")
            //{
            //    vitalsModel = JsonConvert.DeserializeObject<CDSVitalsModel>(MDVUtility.ToStr(AllData["data"]));
            //}
            //Start 08-03-2016 Humaira Yousaf for CDSVitals
            vitalsModel = ser.Deserialize<CDSVitalsModel>(MDVUtility.ToStr(AllData["data"]));
            //End 08-03-2016 Humaira Yousaf for CDSVitals

            CDSHelper helperCDS = new CDSHelper();

            if (model.commandType.ToLower() == "save_cds")
            {
                //Start 08-03-2016 Humaira Yousaf for ruleTypes
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                string ruleTypeIds = arrJSON.ContainsKey("RuleTypes") == true ? MDVUtility.ToStr(arrJSON["RuleTypes"]) : "";
                string userRoleIds = arrJSON.ContainsKey("UserRoles") == true ? MDVUtility.ToStr(arrJSON["UserRoles"]) : "";
                string triggerLocationIds = arrJSON.ContainsKey("TriggerLocations") == true ? MDVUtility.ToStr(arrJSON["TriggerLocations"]) : "";
                string genderIds = arrJSON.ContainsKey("Genders") == true ? MDVUtility.ToStr(arrJSON["Genders"]) : "";
                string ethnicityIds = arrJSON.ContainsKey("Ethnicities") == true ? MDVUtility.ToStr(arrJSON["Ethnicities"]) : "";
                string raceIds = arrJSON.ContainsKey("Races") == true ? MDVUtility.ToStr(arrJSON["Races"]) : "";
                string languageIds = arrJSON.ContainsKey("Languages") == true ? MDVUtility.ToStr(arrJSON["Languages"]) : "";

                response = helperCDS.insertUpdateCDS(model, ruleTypeIds, userRoleIds, triggerLocationIds, genderIds, ethnicityIds, raceIds, languageIds, vitalsModel);
                //End 08-03-2016 Humaira Yousaf for ruleTypes
            }
            //Start 15-03-2016//Ahmad Raza// calling helper method to update CDS Alert's Status
            else if (model.commandType.ToLower() == "update_cds_status")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ClinicalDecisionSupport_Clinical Decision Support", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperCDS.updateCDStatus(model);
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
            //End 15-03-2016//Ahmad Raza// calling helper method to update CDS Alert's Status

            //Start 03-03-2016 Humaira Yousaf to load CDS
            else if (model.commandType.ToLower() == "search_cds")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ClinicalDecisionSupport_Clinical Decision Support", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperCDS.searchCDS(model, model.CDSIDs, model.isPopup);
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
            else if (model.commandType.ToLower() == "search_cdsforalert")
            {
                response = helperCDS.loadCDS(model, model.CDSId, model.isPopup);
            }
            else if (model.commandType.ToLower() == "get_cds_alert_detail_against_patient")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ClinicalDecisionSupport_Clinical Decision Support", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperCDS.loadCDSWithPatientStatus(MDVUtility.ToInt64(model.CDSId), MDVUtility.ToInt64(model.PatientId), model.NoteId, MDVUtility.ToInt64(model.CDSPatientStatusId));
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
            //End 03-03-2016 Humaira Yousaf to load CDS

            //Start 04-03-2016 Humaira Yousaf to load CDS
            else if (model.commandType.ToLower() == "fill_cds")
            {
                response = helperCDS.fillCDS(model);
            }
            //End 04-03-2016 Humaira Yousaf to load CDS

            //Start 16-03-2016//Ahmad Raza// calling helper method to delete CDS ProblemList
            else if (model.commandType.ToLower() == "delete_cds_problemlist")
            {
                response = helperCDS.deleteCDSProblemList(MDVUtility.ToStr(problemModel.CDSProblemId));
            }
            else if (model.commandType.ToLower() == "delete_cds_questionnaire")
            {
                response = helperCDS.deleteCDSQuestionnaire(MDVUtility.ToStr(questionnaireModel.CDSQuestionnaireId));
            }
            //End 16-03-2016//Ahmad Raza// calling helper method to delete CDS ProblemList

            //Start 16-03-2016//Ahmad Raza// calling helper method to delete CDS Allergy
            else if (model.commandType.ToLower() == "delete_cds_allergy")
            {
                response = helperCDS.deleteCDSAllergy(MDVUtility.ToStr(allergyModel.CDSAllergyId), MDVUtility.ToStr(allergyModel.CDSId));
            }
            //End 16-03-2016//Ahmad Raza// calling helper method to delete CDS Allergy

            //Start 16-03-2016//Ahmad Raza// calling helper method to delete CDS Medication
            else if (model.commandType.ToLower() == "delete_cds_medication")
            {
                response = helperCDS.deleteCDSMedication(MDVUtility.ToStr(medicationModel.CDSMedicationId));
            }
            else if (model.commandType.ToLower() == "delete_cds_labresult")
            {
                response = helperCDS.deleteCDSLabResult(MDVUtility.ToStr(labResultModel.CDSId), MDVUtility.ToStr(labResultModel.TestId));
            }
            else if (model.commandType.ToLower() == "delete_cds_insurance")
            {
                response = helperCDS.deleteCDSInsurance(MDVUtility.ToStr(insuranceModel.CDSInsuranceId));
            }
            //End 16-03-2016//Ahmad Raza// calling helper method to delete CDS Medication

            //Start 08-03-2016//Ahmad Raza// calling helper method to load CDS Alert
            else if (model.commandType.ToLower() == "show_cds_alert")
            {
                //Start Farooq Ahmad 06/08/2016 Check Privilegas
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ClinicalDecisionSupport_Clinical Decision Support", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperCDS.loadCDSAlert(model);
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
                //End Farooq Ahmad 06/08/2016 Check Privilegas

            }
            //End 08-03-2016//Ahmad Raza// calling helper method to load CDS Alert
            else if (model.commandType.ToLower() == "active_inactive_cds")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ClinicalDecisionSupport_Clinical Decision Support", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperCDS.activeInActiveCDS(model);
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
            else if (model.commandType.ToLower() == "delete_cds")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ClinicalDecisionSupport_Clinical Decision Support", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperCDS.deleteCDS(model);
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
            else if (model.commandType.ToLower() == "search_cds_against_patient")
            {
              //  response = helperCDS.loadCDSAgainstPatient_Obsolete(model);
                response = helperCDS.loadCDSAgainstPatient(model);
            }
            else if (model.commandType.ToLower() == "update_cds_status_selected_alert")
            {
                response = helperCDS.updateCDStatusForSelectedAlert(model);
            }
            else if (model.commandType.ToLower() == "delete_cds_vital")
            {
                response = helperCDS.deleteCDSVital(MDVUtility.ToStr(vitalsModel.CDSVitalsId));
            }
            else if (model.commandType.ToLower() == "load_cds_ordersets")
            {
                response = null;
                response = helperCDS.loadCDSOrderSet(model);
            }
            else if (model.commandType.ToLower() == "get_cds_orderset_and_priviliges")
            {
                response = null;
                response = helperCDS.GetCDSOrderSetAndPriviliges(model);
            }
            else if (model.commandType.ToLower() == "load_cds_alerts")
            {
                response = helperCDS.loadCDSForAlerts(model, model.CDSIDs, model.isPopup);
            }
            return response;
        }
    }
}