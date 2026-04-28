/* Author:  Muhammad Arshad
 * Created Date: 12/10/2015
 * OverView: Created to handel FaceSheet
 */

using MDVision.Model.Common;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.FaceSheet;
using MDVision.IEHR.EMR.Model.FaceSheet;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class FaceSheetController : ApiController
    {
        [HttpPost]
        // Author:  Muhammad Arshad
        // Created Date: 12/10/2015
        //OverView: Entry point for FaceSheet
        public string FaceSheet(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            FaceSheetModel modelFaceSheet = ser.Deserialize<FaceSheetModel>(MDVUtility.ToStr(AllData["data"]));

            FaceSheetHelper helperFaceSheet = new FaceSheetHelper();

            if (modelFaceSheet.commandType.ToLower() == "save_facesheet")
            {
                response = null;
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("FaceSheet_Face Sheet", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperFaceSheet.saveFaceSheet(modelFaceSheet);
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

            else if (modelFaceSheet.commandType.ToLower() == "update_componentordersorting")
            {
                response = helperFaceSheet.UpdateComponentOrderSorting(modelFaceSheet);
                response = "";
            }
            else if (modelFaceSheet.commandType.ToLower() == "search_facesheet_appointments")
            {
                response = helperFaceSheet.loadFaceSheetAppointments(modelFaceSheet);
            }
            else if (modelFaceSheet.commandType.ToLower() == "preview_facesheet")
            {
                response = helperFaceSheet.previewFaceSheet(modelFaceSheet);
            }
            else if (modelFaceSheet.commandType.ToLower() == "getclinicalsummaryprintcomponents")
            {
                response = helperFaceSheet.LoadClinicalSummaryPrintComponents(modelFaceSheet.PatientId, modelFaceSheet.PrintComponents);
            }
            return response;
        }

        private Dictionary<string, string> Task1(SharedModel shared_model, FaceSheetModel modelFaceSheet)
        {
            Dictionary<string, string> Response = new Dictionary<string, string>();

            FaceSheetHelper helperFaceSheet = new FaceSheetHelper();
            Response.Add("FaceSheet", helperFaceSheet.LoadFaceSheet(shared_model, modelFaceSheet));
            Response.Add("ProblemList", helperFaceSheet.loadFaceSheetProblemList(shared_model, modelFaceSheet));
            Response.Add("Allergies", helperFaceSheet.loadFaceSheetAllergies(shared_model, modelFaceSheet));
            Response.Add("Vitals", helperFaceSheet.loadFaceSheetVitals(shared_model, modelFaceSheet));
            return Response;
        }

        private Dictionary<string, string> Task2(SharedModel shared_model, FaceSheetModel modelFaceSheet)
        {
            Dictionary<string, string> Response = new Dictionary<string, string>();

            FaceSheetHelper helperFaceSheet = new FaceSheetHelper();
            Response.Add("Notes", helperFaceSheet.loadFaceSheetNotes(shared_model, modelFaceSheet));
            Response.Add("Appointments", helperFaceSheet.loadFaceSheetAppointmentsNew(shared_model, modelFaceSheet));
            Response.Add("History", helperFaceSheet.loadFaceSheetHistory(shared_model, modelFaceSheet));
            Response.Add("LabResult", helperFaceSheet.loadFaceSheetLabResult(shared_model, modelFaceSheet));
            return Response;
        }
        private Dictionary<string, string> Task3(SharedModel shared_model, FaceSheetModel modelFaceSheet)
        {
            Dictionary<string, string> Response = new Dictionary<string, string>();

            FaceSheetHelper helperFaceSheet = new FaceSheetHelper();
            Response.Add("ProcedureOrder", helperFaceSheet.loadFaceSheetProcedureOrder(shared_model, modelFaceSheet));
            Response.Add("RadiologyOrder", helperFaceSheet.loadFaceSheetRadiologyOrder(shared_model, modelFaceSheet));
            Response.Add("Medications", helperFaceSheet.loadFaceSheetMedications(shared_model, modelFaceSheet));
            Response.Add("Referrals", helperFaceSheet.loadFaceSheetReferrals(shared_model, modelFaceSheet));
            return Response;
        }
        private Dictionary<string, string> Task4(SharedModel shared_model, FaceSheetModel modelFaceSheet)
        {
            Dictionary<string, string> Response = new Dictionary<string, string>();

            FaceSheetHelper helperFaceSheet = new FaceSheetHelper();
            Response.Add("Immunization", helperFaceSheet.loadFaceSheetImmunization(shared_model, modelFaceSheet));
            Response.Add("Complaints", helperFaceSheet.loadFaceSheetComplaints(shared_model, modelFaceSheet));
            Response.Add("PatientDocument", helperFaceSheet.loadFaceSheetPatientDocument(shared_model, modelFaceSheet));
            Response.Add("ImplantableDevices", helperFaceSheet.loadFaceSheetImplantableDevices(shared_model, modelFaceSheet));
            Response.Add("LabOrder", helperFaceSheet.loadFaceSheetLabOrder(shared_model, modelFaceSheet));
            return Response;
        }
        [HttpPost]
        public string LoadFaceSheetAll(JObject AllData)
        {
            // string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            FaceSheetModel modelFaceSheet = ser.Deserialize<FaceSheetModel>(MDVUtility.ToStr(AllData["data"]));
            string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("FaceSheet_Face Sheet", "SEARCH")).ToString();
            if (string.IsNullOrEmpty(privilegasMessage))
            {
                if (!string.IsNullOrEmpty(modelFaceSheet.PatientId))
                {
                    List<Task<Dictionary<string, string>>> tasks = new List<Task<Dictionary<string, string>>>();
                    SharedModel shared_model = new SharedModel();
                    shared_model.p_ClientId = MDVSession.Current.ClientId;
                    shared_model.p_EntityId = MDVSession.Current.EntityId;
                    shared_model.p_UserName = MDVSession.Current.AppUserName;
                    shared_model.p_AppUserId = MDVSession.Current.AppUserId;
                    shared_model.p_WebEntityURL = MDVSession.Current.WebEntityURL;
                    shared_model.p_AppPassWord = MDVSession.Current.AppPassWord;
                    FaceSheetHelper helperFaceSheet = new FaceSheetHelper();

                    Task<Dictionary<string, string>> task1 = new Task<Dictionary<string, string>>(() => Task1(shared_model, modelFaceSheet));
                    Task<Dictionary<string, string>> task2 = new Task<Dictionary<string, string>>(() => Task2(shared_model, modelFaceSheet));
                    Task<Dictionary<string, string>> task3 = new Task<Dictionary<string, string>>(() => Task3(shared_model, modelFaceSheet));
                    Task<Dictionary<string, string>> task4 = new Task<Dictionary<string, string>>(() => Task4(shared_model, modelFaceSheet));
                    tasks.Add(task1); tasks.Add(task2);
                    tasks.Add(task3); tasks.Add(task4);
                    foreach (var item in tasks)
                    {
                        item.Start();
                    }
                    Task.WaitAll(tasks.ToArray());

                    var response = new
                    {
                        status = true,
                        FaceSheetLoad_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "FaceSheet").Value != null).Result.FirstOrDefault(q => q.Key == "FaceSheet").Value,
                        ProblemList_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "ProblemList").Value != null).Result.FirstOrDefault(q => q.Key == "ProblemList").Value,
                        Allergy_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "Allergies").Value != null).Result.FirstOrDefault(q => q.Key == "Allergies").Value,
                        VitalSignsSoap_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "Vitals").Value != null).Result.FirstOrDefault(q => q.Key == "Vitals").Value,

                        Notes_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "Notes").Value != null).Result.FirstOrDefault(q => q.Key == "Notes").Value,
                        PatientAppointment_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "Appointments").Value != null).Result.FirstOrDefault(q => q.Key == "Appointments").Value,
                        History_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "History").Value != null).Result.FirstOrDefault(q => q.Key == "History").Value,
                        LabResults_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "LabResult").Value != null).Result.FirstOrDefault(q => q.Key == "LabResult").Value,
                       
                        ProcedureOrders_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "ProcedureOrder").Value != null).Result.FirstOrDefault(q => q.Key == "ProcedureOrder").Value,
                        RadiologyOrders_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "RadiologyOrder").Value != null).Result.FirstOrDefault(q => q.Key == "RadiologyOrder").Value,
                        Medications_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "Medications").Value != null).Result.FirstOrDefault(q => q.Key == "Medications").Value,
                        PatientReferrals_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "Referrals").Value != null).Result.FirstOrDefault(q => q.Key == "Referrals").Value,

                        Immunization_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "Immunization").Value != null).Result.FirstOrDefault(q => q.Key == "Immunization").Value,
                        Complaints_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "Complaints").Value != null).Result.FirstOrDefault(q => q.Key == "Complaints").Value,
                        PatientDocument_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "PatientDocument").Value != null).Result.FirstOrDefault(q => q.Key == "PatientDocument").Value,
                        Implantable_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "ImplantableDevices").Value != null).Result.FirstOrDefault(q => q.Key == "ImplantableDevices").Value,
                        LabOrders_JSON = tasks.FirstOrDefault(p => p.Result.FirstOrDefault(q => q.Key == "LabOrder").Value != null).Result.FirstOrDefault(q => q.Key == "LabOrder").Value,
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,

                        Message = "Please select Patient"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

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

    }
}