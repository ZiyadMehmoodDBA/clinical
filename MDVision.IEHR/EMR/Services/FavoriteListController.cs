using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.FavoriteList;
using MDVision.IEHR.EMR.Model.FavoriteList;
using MDVision.Model.Clinical.Favorites;
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
    public class FavoriteListController : ApiController
    {


        [HttpPost]
        public string Immunization(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            FavoriteListImmunizationModel model = ser.Deserialize<FavoriteListImmunizationModel>(MDVUtility.ToStr(AllData["data"]));
            Dictionary<string, dynamic> dictCurrentROSJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            FavoriteListHelper Helper = new FavoriteListHelper();


            if (model.commandType.ToLower() == "get_cvxandadministeredcode")
            {
                response = Helper.GetCVXAndAdministeredCode(model);
            }
            else if (model.commandType.ToLower() == "save_favvaccine")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_Vaccine", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.SaveFavVaccine(model);
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
            else if (model.commandType.ToLower() == "load_fav_immunization")
            {
                response = Helper.LoadFavImmunization(model);
            }
            else if (model.commandType.ToLower() == "load_fav_immunization_detail")
            {
                response = Helper.LoadFavImmunizationDetail(model);
            }
            else if (model.commandType.ToLower() == "load_fav_immunization_vaccine_detail")
            {
                response = Helper.LoadFavImmunizationVaccineDetail(model);
            }
            else if (model.commandType.ToLower() == "delete_favorite_immunization")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_Vaccine", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.DeleteFavoriteImmunization(model);
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
            else if (model.commandType.ToLower() == "update_favvaccine")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_Vaccine", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.UpdateFavVaccine(model);
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



        [HttpPost]
        public string FavoriteList(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            FavoriteListModel model = ser.Deserialize<FavoriteListModel>(MDVUtility.ToStr(AllData["data"]));
            Dictionary<string, dynamic> dictCurrentROSJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            FavoriteListHelper Helper = new FavoriteListHelper();


            if (model.commandType.ToLower() == "load_favoritelist")
            {
                response = Helper.loadClinical_FavoriteList(model);
            }
            else if (model.commandType.ToLower() == "load_favoritelist_icd")
            {
                response = Helper.loadClinical_FavoriteListICD(model);
            }

            //Start//23-03-2016///Ahmad Raza//calling helper method to load FavoriteListCPT
            else if (model.commandType.ToLower() == "load_favoritelist_cpt")
            {
                response = Helper.loadClinicalFavoriteListCPT(model);
            }
            else if (model.commandType.ToLower() == "load_favoritelist_customform")
            {
                response = Helper.loadClinicalFavoriteListCustomForm(model);
            }
            //End//23-03-2016///Ahmad Raza//calling helper method to load FavoriteListCPT
            else if (model.commandType.ToLower() == "save_favcomplaint")
            {
                response = Helper.SaveFavComplaints(model);
            }
            else if (model.commandType.ToLower() == "save_favcustomforms")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_CustomForms", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.insertUpdateFavouriteListCustomForms(model);
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
            else if (model.commandType.ToLower() == "save_favmedication")
            {
                response = Helper.insertUpdateFavouriteListMedication(model);
            }
            else if (model.commandType.ToLower() == "load_favoritelist_medication")
            {
                response = Helper.loadClinicalFavoriteListMedication(model);
            }

            //Start//23-03-2016///Ahmad Raza//calling helper method to save FavoriteListProcedureOrder
            else if (model.commandType.ToLower() == "save_favprocedureorder")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_ProcedureOrder", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.insertUpdateFavouriteListCPT(model);
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
            else if (model.commandType.ToLower() == "save_favfamilyhx")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_FamilyHistory", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.insertUpdateFavouriteListICD(model);
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
            else if (model.commandType.ToLower() == "save_favmedicalhx")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_MedicalHistory", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.insertUpdateFavouriteListICD(model);
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
            else if (model.commandType.ToLower() == "save_favhospitalizationhx")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_HospitalizationHistory", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.insertUpdateFavouriteListICD(model);
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
            else if (model.commandType.ToLower() == "save_favsurgicalhx")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_SurgicalHistory", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.insertUpdateFavouriteListCPT(model);
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
            //End//23-03-2016///Ahmad Raza//calling helper method to save FavoriteListProcedureOrder  
            //Start//23-03-2016///Abid Ali//calling helper method to save FavoriteListConsultationOrder
            else if (model.commandType.ToLower() == "save_favconsultationorder")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_ConsultationOrder", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.insertUpdateFavouriteListCPT(model);
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
            //End//23-03-2016///Ahmad Raza//calling helper method to save FavoriteListConsultationOrder
            else if (model.commandType.ToLower() == "update_favcomplaint")
            {
                response = Helper.updateFavComplaints(model);
            }
            else if (model.commandType.ToLower() == "fill_favcomplaint")
            {
                response = Helper.fillClinical_FavoriteListICD(model);
            }
            else if (model.commandType.ToLower() == "delete_favorite_complaint")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_Complaints", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.DeleteFavComplaints(model);
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
            //Start//24-03-2016///Ahmad Raza//calling helper method to delete FavoriteListProcedureOrder
            else if (model.commandType.ToLower() == "delete_favoritelist_procedure")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_ProcedureOrder", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.deleteFavoriteListProcedure(model);
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
            else if (model.commandType.ToLower() == "delete_favoritelist_customforms")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Favorites_CustomForms", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = Helper.deleteFavoriteListCustomForms(model);
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
            else if (model.commandType.ToLower() == "delete_favoritelist_icd")
            {
                response = Helper.deleteFavouriteListICD(MDVUtility.ToStr(model.FavoriteListICDId));
            }
            else if (model.commandType.ToLower() == "delete_favoritelist_cpt")
            {
                response = Helper.deleteFavouriteListCPT(MDVUtility.ToStr(model.FavoriteListCPTId), MDVUtility.ToStr(model.FavoriteListId));
            }
            else if (model.commandType.ToLower() == "insert_update_favlist_value")
            {
                response = Helper.InsertUpdateFavlistValue(model.FavoriteListName, model.FavListVal);
            }
            else if (model.commandType.ToLower() == "getfavlistvalue")
            {
                response = Helper.GetFavListValue(model.FavoriteListName);
            }

            //End//24-03-2016///Ahmad Raza//calling helper method to delete FavoriteListProcedureOrder
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
        [HttpPost]
        public string FavoriteListMedicationDetail(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            FavoriteListMedicationModel model = ser.Deserialize<FavoriteListMedicationModel>(MDVUtility.ToStr(AllData["data"]));
            Dictionary<string, dynamic> dictCurrentROSJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            FavoriteListHelper Helper = new FavoriteListHelper();

            if (model.commandType.ToLower() == "savefavmedicationdetail")
            {
                response = Helper.SaveFavMedicationDetail(model);
            }
            else if (model.commandType.ToLower() == "updatefavmedicationdetail")
            {
                response = Helper.UpdateFavMedicationDetail(model);
            }
            else if (model.commandType.ToLower() == "search_medication")
            {
                response = Helper.LoadMedicationDetail(model);
            }
            else if (model.commandType.ToLower() == "delete_medicationdetail")
            {
                response = Helper.DeleteMedicationDetail(model);
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