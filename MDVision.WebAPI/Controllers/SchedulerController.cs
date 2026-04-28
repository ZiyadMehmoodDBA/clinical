using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MDVision.WebAPI.Helpers;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using MDVision.Business.BLL;
using MDVision.IEHR.EMR.Helpers.Clinical.Summary;
using System.Data;
using System.Web.Http.Filters;
using MDVision.WebAPI.Filters;

namespace MDVision.WebAPI.Controllers
{
    [RoutePrefix("api/Schedulr")]
    public class SchedulrController : ApiController
    {
        [Route("GetAppointments")]
        [HttpGet]
        public string GetAppointments(Int64 ProviderId, string SlotDate, string StatusId,Int64 Resourceid=0, Int64 Facilityid=0, int PageNumber = 1, int RowsPerPage=15)
        {

            return new SchedulerHelper().SearchAppointment(ProviderId, SlotDate, StatusId, Resourceid, Facilityid, PageNumber, RowsPerPage);

        }

        [Route("GetNearestEmptySlot")]
        [HttpPost]
        public string GetNearestEmptySlot( string providerId, string facilityId)
        {

            return new SchedulerHelper().GetNearestEmptySlot(   providerId,  facilityId);

        }


        [Route("SaveAppointmentSlot")]
        [HttpPost]
        public string SaveAppointment(JObject data)
        {

            return new SchedulerHelper().SaveAppointment(data);

        }





        // [SetSessionFromHeader]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("PostCCDAScheduler")]
        [AllowAnonymous]
        public HttpResponseMessage GenerateCCDAScheduler(CCDARequestModel model)
        {
            ClinicalSummaryHelper bln = new ClinicalSummaryHelper();
            try
            {
                if (ModelState.IsValid)
                {
                    Int64 PatientId =  Convert.ToInt32(model.PatientKey); //333959;
                    Int64 ProviderId = Convert.ToInt32(model.ProviderId);
                    Int64 NoteId =  Convert.ToInt32(model.NoteID);  //157371;
                    
                    IEHR.EMR.Model.Clinical.ClinicalSummaryModel mdlSumm = new IEHR.EMR.Model.Clinical.ClinicalSummaryModel();

                    mdlSumm.IsConfidential = "false";
                    mdlSumm.PatientId = model.PatientKey;  //"333959";
                    mdlSumm.ProviderId = model.ProviderId;   //"0";
                    mdlSumm.NoteId = model.NoteID;  //"0";//"157371";
                    mdlSumm.commandType = "xml";

                    mdlSumm.Components = new List<IEHR.EMR.Model.Clinical.Component>();
                    CCDAGenrator.DocumentTemplateType documentType = CCDAGenrator.DocumentTemplateType.CCDAServiceComponent;

                    int ComCount = 0;

                    if (model.documentType == "ReferralNote")
                    {
                        documentType = CCDAGenrator.DocumentTemplateType.ReferralNoteVDT;

                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "DemographicDataElement" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "SocialHistory" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "CareTeamMembers" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "MentalStatus" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Vitals" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "ProviderDataElement" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Assessment" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Cognitives" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Procedures" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "HealthConcerns" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "AllergiesandAdverseReactionsData" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "PlanOfTreatment" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Immunization" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Problems" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Refferral" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Medications" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "PlanOfCare" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "MedicalEquipment" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "LabResults" });

                    }
                    else if (model.documentType == "ContinuityofCare")
                    {
                        documentType = CCDAGenrator.DocumentTemplateType.CCDVDT;

                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "DemographicDataElement" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "SocialHistory" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "CareTeamMembers" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "MentalStatus" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Vitals" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "ProviderDataElement" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Assessment" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Cognitives" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Procedures" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "HealthConcerns" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "AllergiesandAdverseReactionsData" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "PlanOfTreatment" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Immunization" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Problems" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Refferral" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Medications" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "PlanOfCare" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "MedicalEquipment" });
                        mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "LabResults" });

                    }

                    else
                    {
                         documentType = CCDAGenrator.DocumentTemplateType.CCDAServiceComponent;

                        if (model.IncludedComponents.DemographicDataElement == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "DemographicDataElement" });
                        if (model.IncludedComponents.ProviderDataElement == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "ProviderDataElement" });
                        if (model.IncludedComponents.SocialHx == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "SocialHx" });
                        if (model.IncludedComponents.ProblemLists == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "ProblemLists" });
                        if (model.IncludedComponents.Medications == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Medications" });
                        if (model.IncludedComponents.Allergies == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Allergies" });
                        if (model.IncludedComponents.Immunization == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Immunization" });
                        if (model.IncludedComponents.Assessment == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Assessment" });
                        if (model.IncludedComponents.PlanofTreatment == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "PlanofTreatment" });
                       // if (model.IncludedComponents.PlanOfCare == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "PlanOfCare" });
                        if (model.IncludedComponents.LabResult == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "LabResults RadiologyResults" });
                        if (model.IncludedComponents.Vitals == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Vitals" });
                        if (model.IncludedComponents.MedicalEquipment == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "MedicalEquipment" });
                        if (model.IncludedComponents.Procedures == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Procedures" });
                        if (model.IncludedComponents.CareTeamMembers == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "CareTeamMembers" });
                        if (model.IncludedComponents.VisitReason == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "VisitReason" });
                        if (model.IncludedComponents.HealthConcerns == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "HealthConcerns" });

                        if (model.IncludedComponents.Refferral == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "Refferral" });
                        if (model.IncludedComponents.FunctionalStatus == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "FunctionalStatus" });
                        if (model.IncludedComponents.EncounterDiagnostic == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "EncounterDiagnostic" });

                        if (model.IncludedComponents.PayersSection == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "PayersSection" });
                        if (model.IncludedComponents.MentalStatus == true)
                            mdlSumm.Components.Add(new IEHR.EMR.Model.Clinical.Component { componentId = ComCount--, componentName = "MentalStatus" });
                    }

                    string patients = bln.loadClinicalSummaryXMLData(NoteId, ProviderId, PatientId, mdlSumm, "xml", documentType, 0,"",null,false);
                    if (model.ResponseType.ToLower() == "json")
                    {
                        return utility.GetJsonResult(patients);
                    }
                    else
                        return utility.GetXMLResult(patients);
                }
                else
                {
                    return utility.GetJsonResultBadRequest();
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    message = e.Message
                };
                return utility.GetJsonResultException(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }



    }

    public class CCDARequestModel
    {
        [Required]
        public string PatientKey { get; set; }
        public string NoteID { get; set; }
        public string ProviderId { get; set; }
        public IncludeComponentsModel IncludedComponents { get; set; }
        [Required]
        public string ResponseType { get; set; }
        public string IsConfidential { get; set; }
        public string documentType { get; set; }
    }

    public class IncludeComponentsModel
    {
        public bool DemographicDataElement { get; set; }

        public bool ProviderDataElement { get; set; }

        public bool SocialHx { get; set; }
        public bool Assessment { get; set; }
        public bool PlanofTreatment { get; set; }
        public bool PlanOfCare { get; set; }
        public bool CareTeamMembers { get; set; }

        public bool VisitReason { get; set; }
        public bool HealthConcerns { get; set; }

        public bool Vitals { get; set; }
        
        public bool ProblemLists { get; set; }
        public bool Medications { get; set; }
        public bool Allergies { get; set; }
        public bool Immunization { get; set; }
        public bool LabResult { get; set; }
        public bool Procedures { get; set; }
        public bool MedicalEquipment { get; set; }
        public bool Refferral { get; set; }
        public bool FunctionalStatus { get; set; }
        public bool EncounterDiagnostic { get; set; }
        public bool PayersSection { get; set; }
        public bool MentalStatus { get; set; }
        
    }

    public class utility
    {
        public const string dateTimeFormat = "ddd, dd MMM yyyy HH':'mm':'ss 'EDT' zzz";
        public static HttpResponseMessage GetJsonResult(string dataList)
        {
            HttpRequestMessage Request = new HttpRequestMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(dataList, System.Text.Encoding.UTF8, "application/json");
            return response;
        }
        public static HttpResponseMessage GetJsonResultNoContent()
        {
            HttpRequestMessage Request = new HttpRequestMessage();
            var response = Request.CreateResponse(HttpStatusCode.NoContent);
            response.Content = new StringContent("", System.Text.Encoding.UTF8, "application/json");
            return response;
        }
        public static HttpResponseMessage GetJsonResultBadRequest()
        {
            HttpRequestMessage Request = new HttpRequestMessage();
            var response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response.Content = new StringContent("", System.Text.Encoding.UTF8, "application/json");
            return response;
        }
        public static HttpResponseMessage GetJsonResultException(string exceptionData)
        {
            HttpRequestMessage Request = new HttpRequestMessage();
            var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            response.Content = new StringContent(exceptionData, System.Text.Encoding.UTF8, "application/json");
            return response;
        }
        public static HttpResponseMessage GetJsonResultCreated(string dataList)
        {
            HttpRequestMessage Request = new HttpRequestMessage();
            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Content = new StringContent(dataList, System.Text.Encoding.UTF8, "application/json");
            return response;
        }
        public static HttpResponseMessage GetJsonResultNotImplemented()
        {
            HttpRequestMessage Request = new HttpRequestMessage();
            var response = Request.CreateResponse(HttpStatusCode.NotImplemented);
            response.Content = new StringContent("[]", System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        public static HttpResponseMessage GetXMLResult(string dataList)
        {
            HttpRequestMessage Request = new HttpRequestMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(dataList, System.Text.Encoding.UTF8, "application/xml");
            return response;
        }
    }

   
}
