using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Admin;
using MDVision.IEHR.Controls.Patient;
using MDVision.IEHR.Controls.Patient.Demographics;
using MDVision.Model.Lookups;
using MDVision.Model.MU;
using MDVision.Model.Native.Patient;
using MDVision.Model.Patient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Patient
{
    public class PatientController : ApiController
    {
        public JavaScriptSerializer serializer { get; set; }
        public string response { get; set; }
        public PatientController()
        {
            serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483644;
            var res = new
            {
                status = false,
                Message = Common.AppPrivileges.No_Record_Message
            };
            response = (Newtonsoft.Json.JsonConvert.SerializeObject(res));
        }


        [HttpPost]
        public string Patient(JObject objData)
        {
            PatientModel model = serializer.Deserialize<PatientModel>(MDVUtility.ToStr(objData["data"]));
            if (model.CommandType.ToLower() == "search")
            {
                response = Patient_Search.Instance().SearchPatient(model);
            }
            return response;
        }

        [HttpPost]
        public string PatientDemographic(JObject objData)
        {
            PatientDemographicModel modelDemographic = serializer.Deserialize<PatientDemographicModel>(MDVUtility.ToStr(objData["data"]));

            /*------------------Start Demographic Update/Fill*/
            if (modelDemographic.CommandType.ToLower() == "save_demographic")
            {
                response = Patient_Demographic.Instance().SaveDemographic(modelDemographic);
            }
            else if (modelDemographic.CommandType.ToLower() == "update_patient_demographic")
            {
                response = Patient_Demographic.Instance().UpdateDemographic(modelDemographic);
            }
            else if (modelDemographic.CommandType.ToLower() == "fill_patient_demographic")
            {
                response = Patient_Demographic.Instance().FillPatient(modelDemographic);

            }
            else if (modelDemographic.CommandType.ToLower() == "fill_patient_details_for_custom_forms")
            {
                response = Patient_Demographic.Instance().FillPatientDetailForCustomForms(modelDemographic);
            }
            else if (modelDemographic.CommandType.ToLower() == "calculate_age")
            {
                response = Patient_Demographic.Instance().GetAge(MDVUtility.ToDateTime(modelDemographic.BirthDate));
            }

            else if (modelDemographic.CommandType.ToLower() == "fill_patient_referral")
            {
                response = Patient_Demographic.Instance().loadReferralData(modelDemographic);

            }
            else if (modelDemographic.CommandType.ToLower() == "update_patient_demographic_patpic")
            {
                string PatientDocumentImage = modelDemographic.PatientDocumentImage;
                response = Patient_Demographic.Instance().UpdateDemographicPic(modelDemographic, PatientDocumentImage);
            }
            /*------------------End Demographic Update/Fill*/

            return response;

        }

        [HttpPost]
        public string PatientDemographicQuick(JObject objData)
        {

            PatientDemographicQuickModel modelDemographicQuick = serializer.Deserialize<PatientDemographicQuickModel>(MDVUtility.ToStr(objData["data"]));

            //------------------Start Quick Demographic Save
            if (modelDemographicQuick.CommandType.ToLower() == "save_demographic_quick")
            {
                response = Patient_Demographic_Quick.Instance().SaveDemographicQuick(modelDemographicQuick);
            }
            return response;
            //------------------End Quick Demographic Save
        }

        [HttpPost]
        public string RecentPatient(JObject objData)
        {
            PatientModel model = serializer.Deserialize<PatientModel>(MDVUtility.ToStr(objData["data"]));
            if (model.CommandType.ToLower() == "insert_recent_patient")
            {
                response = Patient_Search.Instance().SaveRecentPatient(model);
            }
            return response;
        }
        [HttpPost]
        public string PatientDemographicNative(JObject objData)
        {
            PatientDemographicModelNative modelDemographicNative = serializer.Deserialize<PatientDemographicModelNative>(MDVUtility.ToStr(objData["data"]));

            /*------------------Start Demographic Update/Fill*/
            if (modelDemographicNative.CommandType.ToLower() == "fill_patient_demographic_native")
            {
                response = Patient_DemographicNative.Instance().FillPatientNative(modelDemographicNative);
            }
            if (modelDemographicNative.CommandType.ToLower() == "update_patient_demographicnative")
            {
                response = Patient_DemographicNative.Instance().UpdatePatientDemographicsNative(modelDemographicNative);
            }
            return response;

            /*------------------End Demographic Update/Fill*/



        }
        [HttpPost]
        public string LanguagesAndCountries(JObject objData)
        {
            LanguageAndCountriesModel modelLC = serializer.Deserialize<LanguageAndCountriesModel>(MDVUtility.ToStr(objData["data"]));
            if (modelLC.CommandType.ToLower() == "get_prefered_languages_by_name")
            {
                response = Patient_Demographic.Instance().GetPreferedLanguages(modelLC);
            }
            else if (modelLC.CommandType.ToLower() == "get_countries_by_name")
            {
                response = Patient_Demographic.Instance().GetCountries(modelLC);
            }
            else if (modelLC.CommandType.ToLower() == "get_cities_by_name")
            {
                response = Patient_Demographic.Instance().GetCities(modelLC);
            }
            return response;

        }
        [HttpPost]
        public string GetPatientEmployer(JObject objData)
        {
            string Name = HttpContext.Current.Request.Params["data"];
            MDVisionLookups ob = new MDVisionLookups();
            response = ob.GetEmployerByName(Name);
            return response;

        }
        [HttpPost]
        public string GetPatientLawyer(JObject objData)
        {
            string Name = HttpContext.Current.Request.Params["data"];
            MDVisionLookups ob = new MDVisionLookups();
            response = ob.GetLawyerByName(Name);
            return response;

        }
        [HttpPost]
        public string GetInsurancePlan(JObject objData)
        {
            string ShortName = HttpContext.Current.Request.Params["data"];
            response = new MDVisionLookups().GetInsurancePlan("1", ShortName);
            return response;

        }
        [HttpPost]
        public string GetPatientRaces(JObject objData)
        {
            string Name = HttpContext.Current.Request.Params["data"];
            MDVisionLookups ob = new MDVisionLookups();
            response = ob.GetRaceByDescription(Name, false);
            return response;
        }

        public string SendBillingInquiryEmail(JObject objData)
        {
            BillingInquiryEmailModel modelDemographic = serializer.Deserialize<BillingInquiryEmailModel>(MDVUtility.ToStr(objData["data"]));
            /*------------------Start Demographic Update/Fill*/
            if (modelDemographic.CommandType.ToLower() == "send_billing_inquiry_email")
            {
                response = Patient_Demographic.Instance().SendBillingInquiryEmail(modelDemographic);
            }
            return response;
        }

        [HttpPost]
        public string MUAlert(JObject objData)
        {
            MUModel model = serializer.Deserialize<MUModel>(MDVUtility.ToStr(objData["data"]));
            if (model.CommandType.ToLower() == "save_mu_alert")
            {
                response = SaveMUAlert(model);
            }
            else if (model.CommandType.ToLower() == "update_mu_alert")
            {
                response = UpdateMUAlert(model);
            }
            else if (model.CommandType.ToLower() == "load_mu_alert")
            {
                response = LoadMUAlert(model.PatientId, model.ProfileName, model.IsShowAlert, model.Type);
            }
            return response;
        }

        public string SaveMUAlert(MUModel model)
        {
            try
            {
                BLLMUAlerts BLLMUAlertsObj = new BLLMUAlerts();
                foreach (var item in model.MUAlerts)
                {
                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedBy = item.CreatedBy;
                    item.UserId = MDVSession.Current.AppUserId;
                }
                BLObject<string> obj_ = BLLMUAlertsObj.InsertMUAlerts(model.MUAlerts);
                if (string.IsNullOrEmpty(obj_.Data))
                {
                    var response = new
                    {
                        status = true,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVCustomException.HumanReadableMessage(obj_.Message),
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
        public string UpdateMUAlert(MUModel model)
        {
            try
            {
                BLLMUAlerts BLLMUAlertsObj = new BLLMUAlerts();
                foreach (var item in model.MUAlerts)
                {
                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedBy = item.CreatedBy;
                    item.UserId = MDVSession.Current.AppUserId;
                }
                BLObject<List<MUAlertsModel>> obj_ = BLLMUAlertsObj.UpdateMUAlerts(model.MUAlerts,model.IsFromNote);
                if (obj_.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        MUAlerts_JSON = new JavaScriptSerializer().Serialize(obj_.Data),
                        MissingDataAlertCount= obj_.Data[0].MissingDataAlertCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVCustomException.HumanReadableMessage(obj_.Message),
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
        public string LoadMUAlert(long PatientId, string ProfileName, bool IsShowAlert, string Type)
        {
            try
            {
                if (PatientId <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.No_Record_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                BLLMUAlerts BLLMUAlertsObj = new BLLMUAlerts();
                BLObject<List<MUAlertsModel>> obj_ = BLLMUAlertsObj.LoadMUAlerts(PatientId, ProfileName, IsShowAlert, Type);
                if (obj_.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        MUAlerts_JSON = new JavaScriptSerializer().Serialize(obj_.Data)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVCustomException.HumanReadableMessage(obj_.Message),
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
    }
}