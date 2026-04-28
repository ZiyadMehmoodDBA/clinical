using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Configuration;
using MDVision.WebAPI;
using MDVision.WebAPI.Helpers;
using MDVision.WebAPI.Filters;
using MDVision.WebAPI.Entities;

namespace MDVision.WebAPI.Controllers
{
    [RoutePrefix("api/Clinical")]
    [SetSessionProperties]
    public class ClinicalController : ApiController
    {
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public ClinicalController()
        {

            _repo = new AuthRepository();
        }

      //  [SetSessionFromHeader]
        [Route("SocialHistory")]
        [HttpGet]
        public string SocialHistory(int PatientId, string UserName, string EntityId, string Gender,Int64 UserId)
        {

            return new ClinicalHelper().GetSocialHistory(PatientId, Gender);
        }

        [Route("FamilyHistory")]
        [HttpGet]
        public string FamilyHistory(int PatientId, string UserName, string EntityId)
        {

            return new ClinicalHelper().GetFamilyHistory(PatientId);
        }

        [Route("Diagnosis")]
        [HttpGet]
        public string GetDiagnosis(string Name, string UserName, string UserId, string EntityId, string isMDVision)
        {

            return new ClinicalHelper().GetDiagnosis(Name, EntityId, isMDVision);
        }
        [Route("GetProcedures")]
        [HttpGet]
        public string GetProcedures(string Name, string UserName, string UserId, string EntityId, string isMDVision)
        {

            return new ClinicalHelper().GetProcedures(Name, EntityId, isMDVision);
        }
       // [SetSessionFromHeader]
        [Route("GetProvider")]
        [HttpGet]
        public string GetProvider(string ProviderName, string UserName, string UserId,string IsActive,string EntityId)
        {
            return new ClinicalHelper().GetProviderEntityBased(ProviderName, IsActive,EntityId);

        }

        [Route("GetProviderWithSpecility")]
        [HttpGet]
        public IHttpActionResult GetProviderWithSpecility(string ProviderName, string UserName, string UserId, string IsActive, string EntityId)
        {
            return Json(new ClinicalHelper().GetProviderWithSpeciality(ProviderName, IsActive, EntityId));

        }

        [Route("MedicalHistory")]
        [HttpGet]
        public string MedicalHistory(int PatientId, string UserName, string EntityId)
        {

            return new ClinicalHelper().GetMedicalHistory(PatientId);
        }
     //   [SetSessionFromHeader]
        [Route("SurgicalHistory")]
        [HttpGet]
        public string SurgicalHistory(int PatientId, string UserName, string EntityId,Int64 diseaseId)
        {

            return new ClinicalHelper().GetSurgicalHistory(PatientId,diseaseId);
        }
   
        [Route("HospitalizationHistory")]
        [HttpGet]
        public string HospitalizationHistory(int PatientId, string UserName, string EntityId, Int64 diseaseId)
        {

            return new ClinicalHelper().GetHospitalizationHistory(PatientId, diseaseId);
        }
        [Route("BirthHistory")]
        [HttpGet]
        public string BirthHistory(Int64 PatientId, string UserName, string EntityId, Int64 birthHxId)
        {

            return new ClinicalHelper().GetBirthHistory(PatientId, birthHxId);
        }
        [Route("SaveSocialHistory")]
        [HttpPost]
        public string SaveSocialHistory(JObject data)
        {

            return new ClinicalHelper().SaveSocialHistory(data);
        }
        [Route("UpdateSocialHistory")]
        [HttpPost]
        public string UpdateSocialHistory(JObject data)
        {

            return new ClinicalHelper().UpdateSocialHistory(data);
        }

        [Route("SaveFamilyHistory")]
        [HttpPost]
        public string SaveFamilyHistory(JObject data)
        {

            return new ClinicalHelper().SaveFamilyHistory(data);
        }
        [Route("UpdateFamilyHistory")]
        [HttpPost]
        public string UpdateFamilyHistory(JObject data)
        {

            return new ClinicalHelper().UpdateFamilyHistory(data);
        }
        [Route("SaveMedicalHistory")]
        [HttpPost]
        public string SaveMedicalHistory(JObject data)
        {

            return new ClinicalHelper().SaveMedicalHistory(data);
        }
        [Route("UpdateMedicalHistory")]
        [HttpPost]
        public string UpdateMedicalHistory(JObject data)
        {

            return new ClinicalHelper().UpdateMedicalHistory(data);
        }
      //  [SetSessionFromHeader]
        [Route("SaveSurgicalHistory")]
        [HttpPost]
        public string SaveSurgicalHistory(JObject data)
        {

            return new ClinicalHelper().SaveSurigicalHistory(data);
        }
      //  [SetSessionFromHeader]
        [Route("UpdateSurgicalHistory")]
        [HttpPost]
        public string UpdateSurgicalHistory(JObject data)
        {

            return new ClinicalHelper().UpdateSurgicalHistory(data);
        }
       
        [Route("SaveHospitalizationHistory")]
        [HttpPost]
        public string SaveHospitalizationHistory(JObject data)
        {

            return new ClinicalHelper().SaveHospitalizationHistory(data);
        }
        
        [Route("UpdateHospitalizationHistory")]
        [HttpPost]
        public string UpdateHospitalizationHistory(JObject data)
        {

            return new ClinicalHelper().UpdateHospitalizationHistory(data);
        }
        [Route("SaveBirthHistory")]
        [HttpPost]
        public string SaveBirthHistory(JObject data)
        {

            return new ClinicalHelper().SaveBirthHistory(data);
        }

        [Route("UpdateBirthHistory")]
        [HttpPost]
        public string UpdateBirthHistory(JObject data)
        {

            return new ClinicalHelper().UpdateBirthHistory(data);
        }
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
