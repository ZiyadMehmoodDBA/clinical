/* Author:  Muhammad Arshad
 * Created Date: 15/04/2016
 * OverView: Created to handel Lab Result
 */

using MDVision.IEHR.EMR.Model.OrdersAndResults.LabResult;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.IEHR.EMR.Helpers.Results;
using MDVision.Common.Utilities;
using MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.EMR.Services
{
    public class LabResultController : ApiController
    {
        [HttpPost]

        // Author:  Muhammad Arshad
        // Created Date: 16/03/2016
        //OverView: Entry point for Lab Result       
        public string LabResult(JObject AllData)
        {
            string response = null;
            string privilegasMessage = string.Empty;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            LabOrderResultModel model = JsonConvert.DeserializeObject<LabOrderResultModel>(MDVUtility.ToStr(AllData["data"]));
            LabOrderTestModel testModel = JsonConvert.DeserializeObject<LabOrderTestModel>(MDVUtility.ToStr(AllData["data"]));
            if (!string.IsNullOrEmpty(model.LabResultIDs)) model.LabResultIDs = model.LabResultIDs.Replace("chkMarkAsReviewed", "").Replace(",chkMarkAsReviewed", ""); // Temp Patch
            Dictionary<string, dynamic> dictCurrentLabTestJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            //LabResultTestModel modelLabResultTest = ser.Deserialize<LabResultTestModel>(MDVUtility.ToStr(AllData["data"]));
            List<object> lstLabTestModel = null;
            if (dictCurrentLabTestJSON.ContainsKey("LabTestIds"))
            {
                string LabTestIds = dictCurrentLabTestJSON["LabTestIds"];
                lstLabTestModel = GetListOfObject("LabTestModel", LabTestIds, dictCurrentLabTestJSON);
            }

            LabResultHelper helperLabResult = new LabResultHelper();

            if (model.commandType.ToLower() == "save_labresult")
            {                                                        
                //End 22-03-2016 Humaira Yousaf for status            
                response = helperLabResult.insertUpdateLabResult(model);
                //End 08-03-2016 Humaira Yousaf for ruleTypes,
            }
            //Start 15-03-2016//Ahmad Raza// calling helper method to update LabResult Alert's Status
            else if (model.commandType.ToLower() == "update_labresult")
            {
                response = helperLabResult.insertUpdateLabResult(model);
            }
            //End 15-03-2016//Ahmad Raza// calling helper method to update LabResult Alert's Status
            else if (model.commandType.ToLower() == "delete_labresultdetail")
            {
                response = helperLabResult.deleteLabResultTest(model.LabOrderResultDetailId);
            }
            //Start 17-03-2016 Humaira Yousaf to load Lab Results
            else if (model.commandType.ToLower() == "delete_labtest")
            {
                response = helperLabResult.deleteLabTest(testModel.LabOrderTestId);
            }
           
            else if (model.commandType.ToLower() == "search_labresults")
            {
                response = helperLabResult.fillLabResults(model);
            }

                 //Start 04-03-2016 Humaira Yousaf to load LabResult
            else if (model.commandType.ToLower() == "fill_labresult")
            {
                response = helperLabResult.fillLabResult(model);
            }

            //End 04-03-2016 Humaira Yousaf to load RadiologyOrder 

            //Start 18-04-2016 Muhammad Arshad LOINC Lookup
            else if (model.commandType.ToUpper() == "LOAD_LABRESULTLOINC")
            {
                response = helperLabResult.OrderResultLOINCLookup(model.LOINCCode, model.LOINCDescription,model.LabId);
            }
            else if (model.commandType.ToUpper() == "LOAD_LABRESULTORGANISMS")
            {
                response = helperLabResult.OrderResultOrganismLookup(model.LOINCDescription);
            }
            //End 18-04-2016 Muhammad Arshad LOINC Lookup
            else if (model.commandType.ToLower() == "getorderingprovider")
            {
                response = helperLabResult.loadOrderingProvider(model);
            }
            else if (model.commandType.ToLower() == "get_results_forsoap")
            {
                response = helperLabResult.getResultsForSoap(model.LabResultIDs, MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.NoteId), MDVUtility.ToInt64(model.ProviderId));
            }
            else if (model.commandType.ToLower() == "fetch_labresult_trends")
            {
                response = helperLabResult.fetch_LabResultTrends(Convert.ToInt64(model.LabResultId), model.FilterSearch, model.DateFrom, model.DateTo);
            }
            else if (model.commandType.ToLower() == "get_labtemps")
            {
                response = helperLabResult.get_LabTemps();
            }
            else if (model.commandType.ToLower() == "attach_labordertest_with_notes")
            {
                response = helperLabResult.attachLabOrderTestWithNotes(Convert.ToInt64(testModel.LabOrderTestId),Convert.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_labordertest_with_notes")
            {
                response = helperLabResult.detachLabOrderTestWithNotes(Convert.ToInt64(testModel.LabOrderTestId), Convert.ToInt64(model.NoteId));
            }
            //End 04-03-2016 Humaira Yousaf to load LabResult  
            //Start 08-03-2016//Ahmad Raza// calling helper method to load LabResult Alert
            else if (model.commandType.ToLower() == "show_LabResult_alert")
            {
                response = "";// helperLabResult.loadLabResultAlert(model);
            }
            else if (model.commandType.ToLower() == "getlatest_labresultby_patientid")
            {
                response = helperLabResult.loadLabResult(model);
            }
            else if (model.commandType.ToLower() == "attach_results_with_notes")
            {
                response = helperLabResult.attachLabResultWithNotes(MDVUtility.ToStr(model.LabResultIDs), MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_labresult_from_notes")
            {
                response = helperLabResult.detachLabResultFromNotes(MDVUtility.ToStr(model.LabResultIDs), MDVUtility.ToInt64(model.NoteId));
            }
            //End 08-03-2016//Ahmad Raza// calling helper method to load LabResult Alert
            else if (model.commandType.ToLower() == "load_patient_insurance")
            {
                response = "";//helperLabResult.loadLabResultAlert(model);
            }

            else if (model.commandType.ToLower() == "save_lab_order_test")
            {
                response = helperLabResult.insertUpdateLabResult(model);
            }
            //Start 22-03-2016//Ahmad Raza// calling helper method to delete LabResult
            else if (model.commandType.ToLower() == "delete_labresult")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Lab Result", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperLabResult.deleteLabResult(MDVUtility.ToStr(model.LabResultId));
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
            //End 22-03-2016//Ahmad Raza// calling helper method to delete LabResult
            //else if (modelLabResultTest.commandType.ToLower() == "delete_LabResult_test")
            //{
            //    response = helperLabResult.deleteLabResultTest(modelLabResultTest.LabResultTestId);
            //}

            //Start 25-04-2016 Humaira Yousaf to view PDF
            else if (model.commandType.ToLower() == "preview_labresult")
            {
                response = helperLabResult.previewLabResult(model,"Lab Result", true);
            }
            else if (model.commandType.ToLower() == "preview_labresultexternalpdf")
            {
                response = helperLabResult.previewLabResultExternalPDFId(model);
            }
            else if (model.commandType.ToLower() == "acknowledge_labresult")
            {
                response = helperLabResult.AcknowledgeLabResult(model.LabResultId);
            }
            //End 25-04-2016 Humaira Yousaf to view PDF
           //Start 27-04-2016 Humaira Yousaf to load attachments
		    else if (model.commandType.ToLower() == "load_attachments")
            {
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                long patientId = arrJSON.ContainsKey("PatientId") == true ? MDVUtility.ToInt64(arrJSON["PatientId"]) : 0;
                long tranisitionId = arrJSON.ContainsKey("TranisitionId") == true ? MDVUtility.ToInt64(arrJSON["TranisitionId"]) : 0;
                string refModuleName = arrJSON.ContainsKey("RefModuleName") == true ? MDVUtility.ToStr(arrJSON["RefModuleName"]) : "";

                response = helperLabResult.loadOrderResultAttachment(patientId, tranisitionId, refModuleName);
            }
            //End 27-04-2016 Humaira Yousaf to load attachments
            else if (model.commandType.ToLower()=="labresult_importhl7")
            {
                 response = helperLabResult.parseLabResultHL7Message();
            }
           //Start 06-05-2016 Humaira Yousaf to view pdf
            else if (model.commandType.ToLower()=="viewpdflabresult")
            {
                response = helperLabResult.viewPdfLabResult(model);
            }
            //End 06-05-2016 Humaira Yousaf to view pdf

            else if (model.commandType.ToLower() == "search_labresults_dashboard")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Lab Result", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperLabResult.loadLabResultDashboard(model);
                }
            }
            else if (model.commandType.ToLower() == "get_assigned_labresults_count")
            {
                response = helperLabResult.GetAssignedLabResultsCount(model.AssigneeId);
            }
            else if (model.commandType.ToLower() == "search_labunsolicitedresults_dashboard")
            {
                response = helperLabResult.loadLabResultUnsolicitedDashboard(model);
            }
            else if (model.commandType.ToLower() == "update_labresultcomments")
            {
                response = helperLabResult.UpdateLabResultComments(model);
            }
            else if (model.commandType.ToLower() == "labresults_count_dashboard")
            {
                long entityId = MDVUtility.ToLong(MDVision.Common.Shared.MDVSession.Current.EntityId);
                string userId = MDVUtility.ToStr(MDVision.Common.Shared.MDVSession.Current.AppUserId);
                var obj = new Controls.DashBoard.DashBoardDB();
                var result = obj.LoadDashboardOrdersResultsCount(entityId, userId);
                var count = new
                {
                    LABRESULT_COUUNT = result
                };
                return (JsonConvert.SerializeObject(count));
            }
            else if (model.commandType.ToLower() == "tcm_count_dashboard")
            {
                long entityId = MDVUtility.ToLong(MDVision.Common.Shared.MDVSession.Current.EntityId);
                string userId = MDVUtility.ToStr(MDVision.Common.Shared.MDVSession.Current.AppUserId);
                var obj = new Controls.DashBoard.DashBoardDB();
                var result = obj.LoadDashBoardTCMsCount(entityId, userId);
                var count = new
                {
                    TCMS_COUUNT = result
                };
                return (JsonConvert.SerializeObject(count));
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

        private List<object> GetListOfObject(string objectType, string selectedIds, Dictionary<string, dynamic> dictCurrentJSON)
        {

            Type CurrentModel = null;
            List<object> lstObjects = new List<object>();
            //if (objectType == "LabTestModel")
            //{
            //    CurrentModel = typeof(LabResultTestModel);
            //}
            PropertyInfo[] ArrCurrentModelPropertyInfo = CurrentModel.GetProperties();
            foreach (string item in selectedIds.Split(','))
            {
                if (item != "" && item.ToLower() != "template")
                {
                    object currentObject = null;
                    //if (objectType == "LabTestModel")
                    //{
                    //    currentObject = new LabResultTestModel();
                    //}
                    if (currentObject != null)
                    {
                        foreach (PropertyInfo CurrentProperty in ArrCurrentModelPropertyInfo)
                        {
                            if (item.Equals("0") || dictCurrentJSON.ContainsKey(CurrentProperty.Name))
                            {
                                currentObject.GetType().GetProperty(CurrentProperty.Name).SetValue(currentObject, dictCurrentJSON[CurrentProperty.Name]);
                            }
                            else if (dictCurrentJSON.ContainsKey(CurrentProperty.Name + item))
                            {
                                currentObject.GetType().GetProperty(CurrentProperty.Name).SetValue(currentObject, dictCurrentJSON[CurrentProperty.Name + item]);
                            }

                        }
                        lstObjects.Add(currentObject);
                    }

                }
            }
            return lstObjects;
        }
    }
}