/* Author:  Muhammad Arshad
 * Created Date: 15/04/2016
 * OverView: Created to handel Radiology Result
 */

using MDVision.IEHR.EMR.Model.OrdersAndResults.RadiologyResult;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.IEHR.EMR.Helpers.Results;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.EMR.Services
{
    public class RadiologyResultController : ApiController
    {
        [HttpPost]

        // Author: Abid Ali
        // Created Date: 25/04/2016
        //OverView: Entry point for Radiology Result       
        public string RadiologyResult(JObject AllData)
        {
            string response = null;
            string privilegasMessage = string.Empty;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            RadiologyOrderResultModel model = JsonConvert.DeserializeObject<RadiologyOrderResultModel>(MDVUtility.ToStr(AllData["data"]));


            RadiologyResultHelper helperRadiologyResult = new RadiologyResultHelper();
            LabResultHelper helperLabResult = new LabResultHelper();

            if (model.commandType.ToLower() == "save_radiologyresult")
            {            
                response = helperRadiologyResult.insertUpdateRadiologyResult(model);              
            }
          
            else if (model.commandType.ToLower() == "update_radiologyresult")
            {
                response = helperRadiologyResult.insertUpdateRadiologyResult(model);
            }
         
            else if (model.commandType.ToLower() == "search_radiologyresults")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Diagnostic Imaging Result", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperRadiologyResult.loadRadiologyResult(model);
                }
            }
                
            else if (model.commandType.ToLower() == "fill_radiologyresult")
            {
                response = helperRadiologyResult.fillRadiologyResult(model);
            }       
           
            else if (model.commandType.ToUpper() == "LOAD_RADIOLOGYRESULTLOINC")
            {
                response = helperLabResult.OrderResultLOINCLookup(model.LOINCCode, model.LOINCDescription);
            }
           
            else if (model.commandType.ToLower() == "getorderingprovider")
            {
                response = helperRadiologyResult.loadOrderingProvider(model);
            }          
         
            else if (model.commandType.ToLower() == "show_RadiologyResult_alert")
            {
                response = "";// helperRadiologyResult.loadRadiologyResultAlert(model);
            }
            else if (model.commandType.ToLower() == "getlatest_radiologyresultby_patientid")
            {
                //response = helperRadiologyResult.loadRadiologyResult(model);
                response = helperRadiologyResult.loadResultsForSoap(model);
            }
            else if (model.commandType.ToLower() == "attach_results_with_notes")
            {
                response = helperRadiologyResult.attachRadiologyResultWithNotes(MDVUtility.ToStr(model.RadiologyResultIDs), MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_radiologyresult_from_notes")
            {
                response = helperRadiologyResult.detachRadiologyResultFromNotes(MDVUtility.ToStr(model.RadiologyResultIDs), MDVUtility.ToInt64(model.NoteId));
            }
            
            else if (model.commandType.ToLower() == "load_patient_insurance")
            {
                response = "";//helperRadiologyResult.loadRadiologyResultAlert(model);
            }
            else if (model.commandType.ToLower() == "search_radiologyresults")
            {
                response = helperRadiologyResult.fillRadiologyResults(model);
            }

            else if (model.commandType.ToLower() == "save_Radiology_order_test")
            {
                response = helperRadiologyResult.insertUpdateRadiologyResult(model);
            }
         
            else if (model.commandType.ToLower() == "delete_radiologyresult")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Diagnostic Imaging Result", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperRadiologyResult.deleteRadiologyResult(MDVUtility.ToStr(model.RadiologyResultId));
                }
            }
            //Start 02-05-2016 Humaira Yousaf to show radiology results
            else if (model.commandType.ToLower() == "preview_radiologyresult")
            {
                response = helperRadiologyResult.previewRadiologyResult(model);
            }
                
            //End 02-05-2016 Humaira Yousaf to show radiology results
            else if (model.commandType.ToLower() == "delete_radiologyresultdetail")
            {
                response = helperRadiologyResult.deleteRadiologyResultTest(model.RadiologyOrderResultDetailId);
                
            }
            else if (model.commandType.ToLower() == "get_results_forsoap")
            {
                response = helperRadiologyResult.getResultsForSoap(model.RadiologyResultIDs, MDVUtility.ToLong(model.PatientId), MDVUtility.ToInt64(model.ProviderId));
            }
            //Start 02-05-2016 Humaira Yousaf to load attachments
            else if (model.commandType.ToLower() == "load_attachments")
            {
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                long patientId = arrJSON.ContainsKey("PatientId") == true ? MDVUtility.ToInt64(arrJSON["PatientId"]) : 0;
                long transitionId = arrJSON.ContainsKey("TransitionId") == true ? MDVUtility.ToInt64(arrJSON["TransitionId"]) : 0;
                string refModuleName = arrJSON.ContainsKey("RefModuleName") == true ? MDVUtility.ToStr(arrJSON["RefModuleName"]) : "";

                response = helperLabResult.loadOrderResultAttachment(patientId, transitionId, refModuleName);
            }
            //End 02-05-2016 Humaira Yousaf to load attachments
          //Start 09-05-2016 Humaira Yousaf to view pdf
		    else if (model.commandType.ToLower() == "viewpdfradiologyresult")
            {
                response = helperRadiologyResult.viewPdfRadiologyResult(model);
            }
			 //End 09-05-2016 Humaira Yousaf to view pdf

            else if (model.commandType.ToLower() == "delete_radiologytest")
            {
                response = helperRadiologyResult.deleteRadiologyTest(model.RadiologyOrderTestId);
            }

            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }

            else
            {
                return response;
            }
        }

    }
}