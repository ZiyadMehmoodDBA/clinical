using MDVision.Common.Utilities;
using MDVision.IEHR.CCM.Helpers.PatientHub;
using MDVision.IEHR.Common;
using MDVision.Model.Clinical.Medical.CarePlan;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class CarePlanController : ApiController
    {
        [HttpPost]
        public string CarePlan(JObject AllData)
        {
            string response = null;


            JavaScriptSerializer ser = new JavaScriptSerializer();
            CarePlanClinicalModel carePlanmodel = ser.Deserialize<CarePlanClinicalModel>(MDVUtility.ToStr(AllData["data"]));
            CarePlanHelper carePlanHelper = new CarePlanHelper();
            CarePlanGoalsModel carePlanGoalsmodel = null;
            CarePlanHealthConcernsModel healthConcernModel = null;
            CarePlanInterventionsModel interventionModel = null;
            CarePlanOutcomesModel outcomesModel = null;
            if (carePlanmodel.commandType.ToLower() == "save_careplan" || carePlanmodel.commandType.ToLower() == "update_careplan")
            {
                string privilegasMessage = string.Empty;
                if (carePlanmodel.commandType.ToLower() == "save_careplan")
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Care Plan", "ADD")).ToString();
                }
                else if (carePlanmodel.commandType.ToLower() == "update_careplan")
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Care Plan", "EDIT")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    if (carePlanmodel.CarePlanType == "Goals")
                    {
                        carePlanGoalsmodel = ser.Deserialize<CarePlanGoalsModel>(MDVUtility.ToStr(AllData["data"]));                       
                    }
                    else if (carePlanmodel.CarePlanType == "Concern")
                    {
                         healthConcernModel = ser.Deserialize<CarePlanHealthConcernsModel>(MDVUtility.ToStr(AllData["data"]));                        
                    }
                    else if (carePlanmodel.CarePlanType == "Intervention")
                    {
                        interventionModel = ser.Deserialize<CarePlanInterventionsModel>(MDVUtility.ToStr(AllData["data"]));
                    }
                    else if (carePlanmodel.CarePlanType == "Outcome")
                    {
                        outcomesModel = ser.Deserialize<CarePlanOutcomesModel>(MDVUtility.ToStr(AllData["data"]));
                    }
                    else if (carePlanmodel.CarePlanType == "Outcome")
                    {
                        carePlanmodel = ser.Deserialize<CarePlanClinicalModel>(MDVUtility.ToStr(AllData["data"]));
                    }
                    response = carePlanHelper.SaveCarePlan(carePlanmodel, carePlanGoalsmodel, healthConcernModel, interventionModel, outcomesModel, carePlanmodel.ProviderId);

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
            else if (carePlanmodel.commandType.ToLower() == "fill_careplan")
            {
                response = carePlanHelper.FillCarePlan(carePlanmodel);
            }
            else if (carePlanmodel.commandType.ToLower() == "fill_careplangoal")
            {
                carePlanGoalsmodel = ser.Deserialize<CarePlanGoalsModel>(MDVUtility.ToStr(AllData["data"]));
                response = carePlanHelper.LoadCarePlanGoals(carePlanGoalsmodel.CarePlanId, carePlanGoalsmodel.GoalId, carePlanGoalsmodel.NotesId, "View", carePlanGoalsmodel.ProviderId);
            }
            else if (carePlanmodel.commandType.ToLower() == "fill_careplanconcern")
            {
                healthConcernModel = ser.Deserialize<CarePlanHealthConcernsModel>(MDVUtility.ToStr(AllData["data"]));
                response = carePlanHelper.LoadHealthConcern(healthConcernModel.CarePlanId, healthConcernModel.HealthConcernsId, healthConcernModel.NotesId, "View");
            }
            else if (carePlanmodel.commandType.ToLower() == "fill_careplanintervention")
            {
                interventionModel = ser.Deserialize<CarePlanInterventionsModel>(MDVUtility.ToStr(AllData["data"]));
                response = carePlanHelper.LoadCarePlanInterventions(interventionModel.CarePlanId, interventionModel.InterventionId, interventionModel.NotesId, "View");
            }
            else if (carePlanmodel.commandType.ToLower() == "fill_careplanoutcomes")
            {
                outcomesModel = ser.Deserialize<CarePlanOutcomesModel>(MDVUtility.ToStr(AllData["data"]));
                response = carePlanHelper.LoadCarePlanOutcomes(outcomesModel.CarePlanId, outcomesModel.OutcomesId, outcomesModel.NotesId, "View");
            }
            else if (carePlanmodel.commandType.ToLower() == "delete_careplangoal")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Care Plan", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    carePlanGoalsmodel = ser.Deserialize<CarePlanGoalsModel>(MDVUtility.ToStr(AllData["data"]));
                    response = carePlanHelper.DeleteCarePlanGoal(carePlanGoalsmodel.CarePlanId, carePlanGoalsmodel.GoalId);
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
            else if (carePlanmodel.commandType.ToLower() == "delete_careplanconcern")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Care Plan", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    healthConcernModel = ser.Deserialize<CarePlanHealthConcernsModel>(MDVUtility.ToStr(AllData["data"]));
                    response = carePlanHelper.DeleteCarePlanHealthConcern(healthConcernModel.HealthConcernsId, healthConcernModel.CarePlanId);
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
            else if (carePlanmodel.commandType.ToLower() == "delete_careplanintervention")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Care Plan", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    interventionModel = ser.Deserialize<CarePlanInterventionsModel>(MDVUtility.ToStr(AllData["data"]));
                    response = carePlanHelper.DeleteCarePlanIntervention(interventionModel.InterventionId,interventionModel.CarePlanId);
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
            else if (carePlanmodel.commandType.ToLower() == "delete_careplanoutcomes")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Care Plan", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    outcomesModel = ser.Deserialize<CarePlanOutcomesModel>(MDVUtility.ToStr(AllData["data"]));
                    response = carePlanHelper.DeleteCarePlanOutcome(outcomesModel.OutcomesId, outcomesModel.CarePlanId);
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
            else if (carePlanmodel.commandType.ToLower() == "addcareplantonote")
            {                
                carePlanmodel = ser.Deserialize<CarePlanClinicalModel>(MDVUtility.ToStr(AllData["data"]));
                response = carePlanHelper.AddCarePlanToNote(carePlanmodel.ItemsForNoteList, MDVUtility.ToLong(carePlanmodel.NotesId), MDVUtility.ToLong(carePlanmodel.PatientId));
            }
            else if (carePlanmodel.commandType.ToLower() == "detach_careplan_from_notes")
            {
                carePlanmodel = ser.Deserialize<CarePlanClinicalModel>(MDVUtility.ToStr(AllData["data"]));
                response = carePlanHelper.DetachCarePlanFromNote(carePlanmodel.DetachItemsNoteList, MDVUtility.ToLong(carePlanmodel.NotesId));
            }
            else if (carePlanmodel.commandType.ToLower() == "get_careplan_by_patientid")
            {
                carePlanmodel = ser.Deserialize<CarePlanClinicalModel>(MDVUtility.ToStr(AllData["data"]));
                response = carePlanHelper.GetCarePlanByPatient(carePlanmodel.CarePlanId, MDVUtility.ToStr(carePlanmodel.PatientId), MDVUtility.ToInt64(carePlanmodel.ProviderId));
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