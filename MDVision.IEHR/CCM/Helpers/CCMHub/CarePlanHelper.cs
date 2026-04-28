using MDVision.Business.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Model.CCM.PatientHub;
using MDVision.Business.BCommon;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using Newtonsoft.Json;
using MDVision.Model.Clinical.Medical.CarePlan;
using Newtonsoft.Json.Linq;
using MDVision.IEHR.Common;
using MDVision.Model.CCM.CCMHub;
using System.Xml.Serialization;
using System.IO;
using MDVision.Model.Clinical.Medication;

namespace MDVision.IEHR.CCM.Helpers.PatientHub
{
    public class CarePlanHelper
    {
        private BLLCCM BLLCCMObj = null;
        private BLLClinical BLLClinicalObj = null;
        private BLLCCM BLLCCCMObj = null;
        public CarePlanHelper()
        {
            BLLCCMObj = new BLLCCM();
            BLLClinicalObj = new BLLClinical();
            BLLCCCMObj = new BLLCCM();
        }
        private static CarePlanHelper _instance = null;

      //  public object BLLCCCMObj { get; private set; }

        public static CarePlanHelper Instance()
        {
            if (_instance == null)
                _instance = new CarePlanHelper();
            return _instance;
        }

        internal string loadCarePlanList(CarePlanSearchModel model)
        {
            try
            {
                BLObject<List<CarePlanFillModel>> obj = BLLCCMObj.loadCarePlanList(model);
                List<CarePlanFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        CPCount = modelList.Count,
                        iTotalDisplayRecords = modelList[0].RecordCount,
                        CPList_JSON = js.Serialize(modelList),
                    };
                    return (JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        CPCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        internal string deleteCarePlanList(long carePlanId)
        {
            try
            {
                if (carePlanId <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLCCMObj.deleteCarePlan(carePlanId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        internal string fillCarePlanList(long carePlanId)
        {
            try
            {
                List<CarePlanModel> modelList = BLLCCMObj.fillCarePlanList(carePlanId);
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        CPCount = modelList.Count,
                        CPList_JSON = js.Serialize(modelList),
                    };
                    return (JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        CPCount = 0,
                        Message = Common.AppPrivileges.No_Record_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        internal string saveCarePlanList(CarePlanModel model)
        {
            try
            {

                model.CarePlanId = BLLCCMObj.saveCarePlanList(model);

                var response = new
                {
                    status = true,
                    CarePlanId= model.CarePlanId,
                    Message = Common.AppPrivileges.Save_Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        internal string updateCarePlanList(CarePlanModel model)
        {
            try
            {

                model.CarePlanId = BLLCCMObj.updateCarePlanList(model);

                var response = new
                {
                    status = true,
                    CarePlanId = model.CarePlanId,
                    Message = Common.AppPrivileges.Update_Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        internal string updateStatusCarePlanList(long carePlanId, string IsActive)
        {
            try
            {
                if (carePlanId <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = IsActive.Equals("1") ? MDVUtility.ToStr(Common.AppPrivileges.Inactive_Error_Message) : MDVUtility.ToStr(Common.AppPrivileges.Active_Error_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLCCMObj.updateStatusCarePlanList(carePlanId, IsActive);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = IsActive.Equals("1") ? MDVUtility.ToStr(Common.AppPrivileges.Active_Message) : MDVUtility.ToStr(Common.AppPrivileges.Inactive_Message)
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #region Clinical Care Plan

        public string FillCarePlan(CarePlanClinicalModel model)
        {
            try
            {
                List<CarePlanClinicalModel> carePlan = BLLClinicalObj.FillCarePlan(model.CarePlanId, model.PatientId);

                if (carePlan != null && carePlan.Count > 0)
                {
                    string carePlanId = carePlan[0].CarePlanId;                 

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<CarePlanGoalsModel> goals = null;
                    List<CarePlanHealthConcernsModel> concerns = null;
                    List<CarePlanInterventionsModel> interventions = null;
                    List<CarePlanOutcomesModel> outcomes = null;
                    List<ProviderCareTeam> careTeamList = null;

                    if (model.CarePlanType == "Goals")
                    {
                        goals = BLLClinicalObj.FillCarePlanGoal(carePlanId, "",model.NotesId,"", model.ProviderId);                        
                    }
                    else if (model.CarePlanType == "Concern")
                    {
                        concerns = BLLClinicalObj.FillCarePlanHealthConcern(carePlanId, "", model.NotesId, "");
                    }
                    else if (model.CarePlanType == "Intervention")
                    {
                       interventions = BLLClinicalObj.FillCarePlanInterventions(carePlanId, "", model.NotesId, "");
                    }
                    else if (model.CarePlanType == "Outcome")
                    {
                        outcomes = BLLClinicalObj.FillCarePlanOutcomes(carePlanId, "", model.NotesId, "");
                    }
                    else if (model.CarePlanType == "CareTeam")
                    {
                        long providerId = carePlan[0].ProviderId;
                        long careTeamId = carePlan[0].CareTeamId;
                        if (providerId > 0 && careTeamId > 0)
                        {
                            BLObject<List<ProviderCareTeam>> obj = BLLCCCMObj.LoadCCMPatientHUBCareTeam(providerId, 0, careTeamId);
                            careTeamList = obj.Data;
                        }                        
                    }
                    var response = new
                    {
                        status = true,
                        CarePlanCount = carePlan.Count,
                        CarePlan_JSON = js.Serialize(carePlan),
                        Goals_JSON = js.Serialize(goals),
                        GoalsCount = goals != null ? goals.Count : 0,
                        Concern_JSON = js.Serialize(concerns),
                        ConcernCount = concerns != null ? concerns.Count : 0,
                        Intervention_JSON = js.Serialize(interventions),
                        InterventionCount = interventions != null ? interventions.Count : 0,
                        Outcomes_JSON = js.Serialize(outcomes),
                        OutcomesCount = outcomes != null ? outcomes.Count : 0,
                        CareTeamCount = careTeamList != null ? careTeamList.Count : 0,
                        CareTeam_JSON = js.Serialize(careTeamList),
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.No_Record_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string SaveCarePlan(CarePlanClinicalModel model, CarePlanGoalsModel goalsModel, CarePlanHealthConcernsModel concernModel, CarePlanInterventionsModel interventionModel, CarePlanOutcomesModel outcomesModel, long ProviderId)
        {
            try
            {
                dynamic response = null;
                if (model.CarePlanType != "CareTeam")
                {
                    List<CarePlanClinicalModel> carePlan = BLLClinicalObj.FillCarePlan(model.CarePlanId, model.PatientId);
                    if (carePlan != null && carePlan.Count > 0)
                    {
                        model.ProviderId = carePlan[0].ProviderId;
                        model.CareTeamId = carePlan[0].CareTeamId;
                    }
                }
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();

                model.CarePlanId = BLLClinicalObj.InsertUpdateCarePlan(model);

                if (MDVUtility.ToLong(model.CarePlanId) > 0)
                {
                    if (model.CarePlanType == "Goals" && goalsModel != null)
                    {
                        goalsModel.CarePlanId = model.CarePlanId;
                        dynamic carePlanGoal;
                        if (MDVUtility.ToLong(goalsModel.GoalId) > 0)
                        {
                            carePlanGoal = JObject.Parse(UpdateCarePlanGoal(goalsModel));
                        }
                        else
                        {
                            carePlanGoal = JObject.Parse(SaveCarePlanGoal(goalsModel));

                        }
                    }
                    else if (model.CarePlanType == "Concern" && concernModel != null)
                    {
                        concernModel.CarePlanId = model.CarePlanId;
                        InsertUpdateHealthConcern(concernModel);
                    }

                    else if (model.CarePlanType == "Intervention" && interventionModel != null)
                    {
                        interventionModel.CarePlanId = model.CarePlanId;
                        dynamic carePlanIntervention;
                        if (MDVUtility.ToLong(interventionModel.InterventionId) > 0)
                        {
                            carePlanIntervention = JObject.Parse(UpdateCarePlanIntervention(interventionModel));
                        }
                        else
                        {
                            carePlanIntervention = JObject.Parse(SaveCarePlanIntervention(interventionModel));
                        }
                    }
                    else if (model.CarePlanType == "Outcome" && outcomesModel != null)
                    {
                        outcomesModel.CarePlanId = model.CarePlanId;
                        dynamic carePlanOutcome;
                        if (MDVUtility.ToLong(outcomesModel.OutcomesId) > 0)
                        {
                            carePlanOutcome = JObject.Parse(UpdateCarePlanOutcome(outcomesModel));
                        }
                        else
                        {
                            carePlanOutcome = JObject.Parse(SaveCarePlanOutcome(outcomesModel));
                        }
                    }
                    if (model.CarePlanType == "Goals")
                    {
                        dynamic carePlanGoals = JObject.Parse(LoadCarePlanGoals(model.CarePlanId, "0",model.NotesId, "", MDVUtility.ToStr(ProviderId)));
                        if (carePlanGoals.status == true)
                        {
                            response = new
                            {
                                status = true,
                                CarePlanId = model.CarePlanId,
                                GoalId = carePlanGoals.GoalId,
                                GoalsCount = carePlanGoals.GoalsCount,
                                Goals_JSON = carePlanGoals.Goals_JSON,
                                Message = AppPrivileges.Save_Message,
                            };
                        }
                    }
                    if (model.CarePlanType == "Concern")
                    {
                        dynamic carePlanConcern = JObject.Parse(LoadHealthConcern(model.CarePlanId, "0", model.NotesId,""));
                        if (carePlanConcern.status == true)
                        {
                            response = new
                            {
                                status = true,
                                CarePlanId = model.CarePlanId,
                                ConcernCount = carePlanConcern.ConcernCount,
                                Concern_JSON = carePlanConcern.Concern_JSON,
                                Message = AppPrivileges.Save_Message,
                            };
                        }
                    }
                    if (model.CarePlanType == "Intervention")
                    {
                        dynamic carePlanIntervention = JObject.Parse(LoadCarePlanInterventions(model.CarePlanId, "0",model.NotesId, ""));
                        if (carePlanIntervention.status == true)
                        {
                            response = new
                            {
                                status = true,
                                CarePlanId = model.CarePlanId,
                                InterventionCount = carePlanIntervention.InterventionCount,
                                Intervention_JSON = carePlanIntervention.Intervention_JSON,
                                Message = AppPrivileges.Save_Message,
                            };
                        }
                    }
                    if (model.CarePlanType == "Outcome")
                    {
                        dynamic carePlanOutcomes = JObject.Parse(LoadCarePlanOutcomes(model.CarePlanId, "0", model.NotesId,""));
                        if (carePlanOutcomes.status == true)
                        {
                            response = new
                            {
                                status = true,
                                CarePlanId = model.CarePlanId,
                                OutcomesCount = carePlanOutcomes.OutcomesCount,
                                Outcomes_JSON = carePlanOutcomes.Outcomes_JSON,
                                Message = AppPrivileges.Save_Message,
                            };
                        }
                    }
                    if (model.CarePlanType == "CareTeam")
                    {
                        response = new
                        {
                            status = true,
                            CarePlanId = model.CarePlanId,                            
                            Message = AppPrivileges.Save_Message,
                        };
                    }
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Save_Error_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string LoadCarePlanGoals(string CarePlanId, string GoalId, string NotesId, string action, string ProviderId)
        {
            try
            {
                List<CarePlanGoalsModel> goals = BLLClinicalObj.FillCarePlanGoal(CarePlanId, GoalId, NotesId, action, MDVUtility.ToInt64(ProviderId));
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                if (goals != null && goals.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        GoalsCount = goals.Count,
                        Goals_JSON = js.Serialize(goals),
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        MedicationCount = 0,
                        Message = "Record not found."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SaveCarePlanGoal(CarePlanGoalsModel goalsModel)
        {
            try
            {                
                dynamic response = null;
               
                goalsModel.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                goalsModel.CreatedOn = DateTime.Now.ToString();
                goalsModel.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                goalsModel.ModifiedOn = DateTime.Now.ToString();
                List<CarePlanGoalsModel> goalsList = BLLClinicalObj.CarePlanGoalInsert(goalsModel); ;

                if (goalsList != null && goalsList.Count > 0)
                {
                    goalsModel.GoalId = goalsList[0].GoalId;

                    response = new
                    {
                        status = true,
                        GoalId = goalsModel.GoalId,
                        Message = AppPrivileges.Save_Message,
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    response = new
                    {
                        status = false,
                        Message = AppPrivileges.Save_Error_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }               
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string UpdateCarePlanGoal(CarePlanGoalsModel goalsModel)
        {
            try
            {               
                dynamic response = null;
                
                goalsModel.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                goalsModel.CreatedOn = DateTime.Now.ToString();
                goalsModel.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                goalsModel.ModifiedOn = DateTime.Now.ToString();
                string goalId = BLLClinicalObj.CarePlanGoalUpdate(goalsModel); ;

                if (!string.IsNullOrEmpty(goalId))
                {
                    goalsModel.GoalId = goalId;

                    response = new
                    {
                        status = true,
                        GoalId = goalId,
                        Message = AppPrivileges.Save_Message,

                    };
                   
                    return (JsonConvert.SerializeObject(response));                 
                }
                else
                {
                    response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Save_Error_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string DeleteCarePlanGoal(string CarePlanId, string GoalId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(GoalId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.DeleteCarePlanGoal(CarePlanId, GoalId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }

        }

        public string InsertUpdateHealthConcern(CarePlanHealthConcernsModel model)
        {
            try
            {
                dynamic response = null;
                List<CarePlanHealthConcernsModel> concernList = null;

                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();

                concernList = BLLClinicalObj.InsertUpdateHealthConcern(model);

                if (concernList != null && concernList.Count > 0)
                {
                    model.HealthConcernsId = concernList[0].HealthConcernsId;

                    response = new
                    {
                        status = true,
                        HealthConcernId = model.HealthConcernsId,
                        Message = AppPrivileges.Save_Message,
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    response = new
                    {
                        status = false,
                        Message = AppPrivileges.Save_Error_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string LoadHealthConcern(string CarePlanId, string HealthConcernId, string NotesId, string action)
        {
            try
            {
                List<CarePlanHealthConcernsModel> healthConcern = BLLClinicalObj.FillCarePlanHealthConcern(CarePlanId, HealthConcernId, NotesId, action);
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                if (healthConcern != null && healthConcern.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        ConcernCount = healthConcern.Count,                    
                        Concern_JSON = js.Serialize(healthConcern)
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,                      
                        Message = "Record not found."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string DeleteCarePlanHealthConcern(string concernId, string carePlanId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(concernId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.DeleteCarePlanHealthConcern(concernId, carePlanId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }

        }

        public string SaveCarePlanIntervention(CarePlanInterventionsModel model)
        {
            try
            {
                dynamic response = null;

                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                CarePlanInterventionsModel interventionObj = BLLClinicalObj.CarePlanInterventionInsert(model); ;

                if (interventionObj != null)
                {
                    model.InterventionId = interventionObj.InterventionId;

                    response = new
                    {
                        status = true,
                        InterventionId = model.InterventionId,
                        Message = AppPrivileges.Save_Message,
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    response = new
                    {
                        status = false,
                        Message = AppPrivileges.Save_Error_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return JsonConvert.SerializeObject(response);
            }
        }
        public string UpdateCarePlanIntervention(CarePlanInterventionsModel model)
        {
            try
            {
                dynamic response = null;

                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                string interventionId = BLLClinicalObj.CarePlanInterventionUpdate(model); ;

                if (!string.IsNullOrEmpty(interventionId))
                {
                    model.InterventionId = interventionId;

                    response = new
                    {
                        status = true,
                        InterventionId = model.InterventionId,
                        Message = AppPrivileges.Save_Message,

                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Save_Error_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string LoadCarePlanInterventions(string CarePlanId, string InterventionId, string NotesId, string action)
        {
            try
            {
                List<CarePlanInterventionsModel> interventions = BLLClinicalObj.FillCarePlanInterventions(CarePlanId, InterventionId, NotesId, action);
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                if (interventions != null && interventions.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        InterventionCount = interventions.Count,
                        Intervention_JSON = js.Serialize(interventions)
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = "Record not found."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string DeleteCarePlanIntervention(string InterventionId, string CarePlanId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(InterventionId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.DeleteCarePlanIntervention(InterventionId, CarePlanId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }

        }
        public string SaveCarePlanOutcome(CarePlanOutcomesModel model)
        {
            try
            {
                dynamic response = null;

                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                CarePlanOutcomesModel outcomeObj = BLLClinicalObj.CarePlanOutcomeInsert(model); ;

                if (outcomeObj != null)
                {
                    model.OutcomesId = outcomeObj.OutcomesId;

                    response = new
                    {
                        status = true,
                        OutcomesId = model.OutcomesId,
                        Message = AppPrivileges.Save_Message,
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    response = new
                    {
                        status = false,
                        Message = AppPrivileges.Save_Error_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return JsonConvert.SerializeObject(response);
            }
        }
        public string UpdateCarePlanOutcome(CarePlanOutcomesModel model)
        {
            try
            {
                dynamic response = null;

                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                string outcomeId = BLLClinicalObj.CarePlanOutcomeUpdate(model);

                if (!string.IsNullOrEmpty(outcomeId))
                {
                    model.OutcomesId = outcomeId;

                    response = new
                    {
                        status = true,
                        InterventionId = model.OutcomesId,
                        Message = AppPrivileges.Save_Message,

                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Save_Error_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string LoadCarePlanOutcomes(string CarePlanId, string OutcomesId, string NotesId, string action)
        {
            try
            {
                List<CarePlanOutcomesModel> outcomes = BLLClinicalObj.FillCarePlanOutcomes(CarePlanId, OutcomesId, NotesId, action);
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                if (outcomes != null && outcomes.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        OutcomesCount = outcomes.Count,
                        Outcomes_JSON = js.Serialize(outcomes)
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = "Record not found."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string DeleteCarePlanOutcome(string OutcomeId, string CarePlanId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(OutcomeId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.DeleteCarePlanOutcome(OutcomeId, CarePlanId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }

        }
        public string AddCarePlanToNote(List<CarePlanNoteModel> ItemsList, long NotesId, long PatientId)
        {
            try
            {              
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CarePlanNoteModel>));
                StringWriter textWriter = new StringWriter();
                xmlSerializer.Serialize(textWriter, ItemsList);
                string xml = textWriter.ToString();
                if (ItemsList.Count > 0)
                {
                    string res = BLLClinicalObj.AddCarePlanToNote(xml, NotesId);

                    if (res=="")
                    {
                        List<ClinicalMedicationsModel> medications = new BLLClinical().GetMedicationsLookup(PatientId);
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                        var response = new
                        {
                            status = true,
                            Medications_JSON = js.Serialize(medications),
                            Message = AppPrivileges.Save_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = AppPrivileges.Save_Error_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false, 
                        Message = "No data to add to note."
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string DetachCarePlanFromNote(List<CarePlanNoteModel> ItemsList, long notesId)
        {
            try
            {
                if(ItemsList== null || ItemsList.Count <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CarePlanNoteModel>));
                    StringWriter textWriter = new StringWriter();
                    xmlSerializer.Serialize(textWriter, ItemsList);
                    string xml = textWriter.ToString();

                    string res = BLLClinicalObj.DetachCarePlanFromoNote(xml, notesId);
                    if (res == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = AppPrivileges.Delete_Error_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string GetCarePlanByPatient(string CarePlanId, string PatientId, long ProviderId)
        {
            try
            {
                CarePlanClinicalModel model = BLLClinicalObj.GetCarePlanByPatientId(CarePlanId,PatientId, ProviderId);
                List<ClinicalMedicationsModel> medications = new BLLClinical().GetMedicationsLookup(MDVUtility.ToLong(PatientId));
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                if (model != null)
                {
                    var response = new
                    {
                        status = true,
                        CarePlanId = model.CarePlanId,
                        Comments = model.Comments,
                        GoalsCount = model.GoalsModelList.Count,
                        GoalsList = js.Serialize(model.GoalsModelList),
                        ConcernCount = model.ConcernsModelList.Count,
                        ConcernsList = js.Serialize(model.ConcernsModelList),
                        InterventionsCount = model.InterventionsModelList.Count,
                        InterventionsList = js.Serialize(model.InterventionsModelList),
                        OutcomesCount = model.OutcomesModelList.Count,
                        OutcomesList = js.Serialize(model.OutcomesModelList),
                        Medications_JSON = js.Serialize(medications)
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Record not found."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion



    }
}