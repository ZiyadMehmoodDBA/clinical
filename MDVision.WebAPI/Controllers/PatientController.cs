using Microsoft.Owin.Security;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MDVision.WebAPI;
using MDVision.WebAPI.Helpers;
using MDVision.Model.Patient;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using MDVision.WebAPI.Filters;
using MDVision.Common.Utilities;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using MDVision.Common.Shared;
using MDVision.IEHR.Controls.Patient.Demographics;

namespace MDVision.PatientPortalAPI.Controllers
{
    [RoutePrefix("api/Patient")]
    public class PatientsController : ApiController
    {
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public PatientsController()
        {

            _repo = new AuthRepository();
        }

        #region PatientRegistration

        [Route("Lookup")]
        [HttpGet]
        public string LookupPatientNative(string Searchstring, long EntityId, long UserId, string IsActive, int PageNumber = 1)
        {

            return new PatientHelper().LookupPatientNative(Searchstring, EntityId, UserId, IsActive, PageNumber);
        }

        [Route("LookupPatientVisitType")]
        [HttpGet]
        public IHttpActionResult LookupPatientVisitType()
        {

            return Json(new PatientHelper().LookupPatientVisitType());
        }

        [SetSessionProperties]
        [Route("Demographics")]
        [HttpGet]
        public object GetPatientDemographics(int PatientId, string FormName, int EntityId, string UserName)
        {
            FormName = "Demographics";

            return new PatientHelper().GetPatientDemographics(PatientId, FormName);
        }

        [Route("LookupInsurancePlan")]
        [HttpGet]
        public string LookupInsurancePlan(string ShortName, long EntityId)
        {
            return new PatientHelper().LookupInsurancePlan(ShortName, EntityId);
        }

        [Route("DemographicsSave")]
        [HttpPost]
        public string SaveDemographics(JObject data)
        {
            try
            {
                return new PatientHelper().UpdatePatientDemographicsNative(data);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
        }
        [Route("InsuranceSave")]
        [HttpPost]
        public string SaveInsurance(JObject data)
        {
            try
            {
                return new PatientHelper().UpdatePatientInsuranceNative(data);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
        }

        [SetSessionProperties]
        [Route("RecentPatient")]
        [HttpPost]
        public string RecentPatient(int PatientId, string UserName, string UserId, string EntityId)
        {

            return new PatientHelper().RecentPatient(PatientId, UserName, EntityId);
        }

        [SetSessionProperties]
        [Route("SavePatientConsent")]
        [HttpPost]
        public string SavePatientConsent(JObject data)
        {
            PatientHelper patientHelper = new PatientHelper();
            //var responce = patientHelper.UploadPDF(data);
            var conosentResponce = new PatientHelper().SavePatientConsent(data);
            // string finalResponce = responce.Insert((responce.Length - 1),","+ conosentResponce);
            return conosentResponce;
        }

        [SetSessionProperties]
        [Route("LoadPatientConsent")]
        [HttpPost]
        public string LoadPatientConsent(long PatientId, string UserName, string EntityId)
        {
            PatientHelper patientHelper = new PatientHelper();
            string responce = new PatientHelper().LoadPatientConsent(PatientId);
            return responce;
        }

        // [SetSessionProperties]
        [Route("LoadPatientMultiFilter")]
        [HttpPost]
        public IHttpActionResult LoadPatientMultiFilter(JObject data)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            LoadMuiltPatientAddress model = ser.Deserialize<LoadMuiltPatientAddress>(MDVUtility.ToStr(data["data"]));
            Object responce = new PatientHelper().SearchPatienMultiFilter(model);
            return Json(responce);
        }

        [Route("LoadPatientAdressMultiFilter")]
        [HttpPost]
        public IHttpActionResult LoadPatientAdressMultiFilter(JObject data)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            LoadMuiltPatientAddress model = ser.Deserialize<LoadMuiltPatientAddress>(MDVUtility.ToStr(data["data"]));

            return Json(new PatientHelper().SearchPatienAdressMultiFilter(model.NetWorkIP=="" ? null: model.NetWorkIP, model.Ext == "" ? null :model.Ext, model.Zip == "" ? null :model.Zip, model.FirstName == "" ? null :model.FirstName, model.LastName == "" ? null :model.LastName, model.DOB == "" ? null :model.DOB, model.Gender == "" ? null :model.Gender, model.AccountNo == "" ? null :model.AccountNo, model.EntityId == "" ? null :model.EntityId, model.MobileNumber == "" ? null : model.MobileNumber));
           
        }
        [Route("LoadPatientContact")]
        [HttpPost]
        public IHttpActionResult LoadPatientContact(JObject data)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            LoadMuiltPatientAddress model = ser.Deserialize<LoadMuiltPatientAddress>(MDVUtility.ToStr(data));

            return Json(new PatientHelper().LoadPatientContactWebApi(model.NetWorkIP == "" ? null : model.NetWorkIP, model.PatientId));

        }
        [AllowAnonymous]
        [Route("LoadPatientRelations")]
        [HttpGet]
        public IHttpActionResult LoadPatientRelations()
        {
            //JavaScriptSerializer ser = new JavaScriptSerializer();
            //LoadMuiltPatientAddress model = ser.Deserialize<LoadMuiltPatientAddress>(MDVUtility.ToStr(data));

            return Json(new PatientHelper().LoadPatientRelations());

        }
        [AllowAnonymous]
        [Route("CheckNetwrok")]
        [HttpGet]
        public IHttpActionResult CheckNetwrok(string NetworkIP)
        {
            //JavaScriptSerializer ser = new JavaScriptSerializer();
            //LoadMuiltPatientAddress model = ser.Deserialize<LoadMuiltPatientAddress>(MDVUtility.ToStr(data));

            return Json(new PatientHelper().CheckNetwrok(NetworkIP));

        }
        [Route("SavePatientSignature")]
        [HttpPost]
        public IHttpActionResult SavePatientSignature(JObject data)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            LoadMuiltPatientAddress model = ser.Deserialize<LoadMuiltPatientAddress>(MDVUtility.ToStr(data));

            return Json(new PatientHelper().SavePatientSignature(model));

        }
        [Route("TwoSetVerfication")]
        [HttpPost]
        public IHttpActionResult TwoSetVerfication(JObject data)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            LoadMuiltPatientAddress model = ser.Deserialize<LoadMuiltPatientAddress>(MDVUtility.ToStr(data));

            return Json(new PatientHelper().TwoSetVerfication(model));

        }
        [Route("LoadPatientByInsurance")]
        [HttpGet]
        public string LoadPatientByInsurance(string SubscriberId,string expiryDate)
        {
            string responce = new PatientHelper().LoadPatientByInsurance(SubscriberId, expiryDate);
            return responce;
        }


        [SetSessionProperties]
        [Route("GetPatientDemographicslockUps")]
        [HttpGet]
        public string GetPatientDemographicslockUps()
        {
            return new PatientHelper().GetPatientDemographicslockUps();
        }
        #endregion


        #region PhysicianApp

        [HttpGet]
        [SetSessionFromHeader]
        [Route("MostViewedLookup")]
        public string LookupMostViewedPatientNative()
        {

            return new PatientHelper().LookupMostViewedPatientNative();
        }
      
        [HttpGet]
        [SetSessionFromHeader]
        [Route("PatientLookup")]
        public string LookupPatientNative(string Searchstring, string IsActive, int PageNumber = 1)
        {
            long EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
            long UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);

            return new PatientHelper().LookupPatientNative(Searchstring, EntityId, UserId, IsActive, PageNumber);
        }

        [HttpPost]
        [SetSessionFromHeader]
        [Route("PatientDemographic")]
        public string PatientDemographicFull(JObject objData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
                ser.MaxJsonLength = 2147483644;
                PatientDemographicModel modelDemographic = ser.Deserialize<PatientDemographicModel>(MDVUtility.ToStr(objData["data"]));

                /*------------------Start Demographic Update/Fill*/
                if (modelDemographic.CommandType.ToLower() == "save_demographic")
                {
                    response = Patient_Demographic.Instance().SaveDemographic(modelDemographic);
                }
                if (modelDemographic.CommandType.ToLower() == "update_patient_demographic")
                {
                    response = Patient_Demographic.Instance().UpdateDemographic(modelDemographic);
                }
                if (modelDemographic.CommandType.ToLower() == "fill_patient_demographic")
                {
                    response = Patient_Demographic.Instance().FillPatient(modelDemographic);
                }
                
            return response;
        }
        [SetSessionFromHeader]
        [Route("SavePatientEmergencyContact")]
        [HttpPost]
        public string SaveEmergencyContacts(JObject data)
        {
            try
            {
                return new PatientHelper().SaveEmergencyContact(data);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
        }
        [SetSessionFromHeader]
        [Route("UpdatePatientEmergencyContact")]
        [HttpPost]
        public string UpdateEmergencyContacts(JObject data)
        {
            try
            {
                return new PatientHelper().UpdateEmergencyContact(data);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
        }
        [SetSessionFromHeader]
        [Route("FillPatientEmergencyContact")]
        [HttpGet]
        public string FillEmergencyContact(Int64 ContactId,Int64 PatientId)
        {
            try
            {
                return new PatientHelper().FillEmergencyContact(ContactId,PatientId);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        [SetSessionFromHeader]
        [Route("FillPatientInfo")]
        [HttpGet]
        public string FillPatientInfo(Int64 PatientId)
        {
            try
            {
                return new PatientHelper().FillPatientInfo(PatientId);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }



        [SetSessionFromHeader]
        [Route("DeletePatientEmergencyContact")]
        [HttpGet]
        public string DeleteEmergencyContacts(Int64 EmergencyContactId)
        {
            try
            {
                return new PatientHelper().DeleteEmergencyContact(EmergencyContactId);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(EmergencyContactId);
            }
        }
        [SetSessionFromHeader]
        [Route("updatePatientpreferences")]
        [HttpPost]
        public string UpdatePatientPreferences(JObject data)
        {
            try
            {
                return new PatientHelper().UpdatePatientPreferences(data);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
        }
        [SetSessionFromHeader]
        [Route("GetPatientPreferenceDDL")]
        [HttpGet]
        public string GetPatientPreferenceDDL()
        {
            try
            {


                var response = new
                {
                    status = true,
                   
                    PatientPreference_JSON =new PatientHelper().GetCommunication("true")
             

                    
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
                return (JsonConvert.SerializeObject(response));
            }
           
        }

        [SetSessionFromHeader]
        [Route("updatePrimaryContact")]
        [HttpPost]
        public string UpdatePrimaryContact(JObject data)
        {
            try
            {
                return new PatientHelper().UpdatePrimaryContact(data);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
        }
        [SetSessionFromHeader]
        [Route("DeletePatientPicture")]
        [HttpPost]
        public string DeletePatientPicture(Int64 PatientId)
        {
            try
            {
                return new PatientHelper().DeletePatientPicture(PatientId);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(PatientId);
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}

