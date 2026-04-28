/* Author:  Muhammad Arshad
 * Created Date: 16/03/2016
 * OverView: Created to handel Lab Order
 */

using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.IEHR.EMR.Helpers.Clinical.Orders;
using MDVision.Common.Utilities;


namespace MDVision.IEHR.EMR.Services
{
    public class LabOrderController : ApiController
    {
        [HttpPost]

        // Author:  Muhammad Arshad
        // Created Date: 16/03/2016
        //OverView: Entry point for Lab Order       
        public string LabOrder(JObject AllData)
        {
            string response = null;
            string privilegasMessage = string.Empty;

        JavaScriptSerializer ser = new JavaScriptSerializer();
            LabOrderModel model = JsonConvert.DeserializeObject<LabOrderModel>(MDVUtility.ToStr(AllData["data"]));

            Dictionary<string, dynamic> dictCurrentLabTestJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            LabOrderTestModel modelLabOrderTest = ser.Deserialize<LabOrderTestModel>(MDVUtility.ToStr(AllData["data"]));
            List<object> lstLabTestModel = null;
            if (dictCurrentLabTestJSON.ContainsKey("LabTestIds"))
            {
                string LabTestIds = dictCurrentLabTestJSON["LabTestIds"];
                lstLabTestModel = OrderHelpers.GetListOfObject("LabTestModel", LabTestIds, dictCurrentLabTestJSON);
            }
            LabOrderQuestionAnswerModel quesAnswerModel = JsonConvert.DeserializeObject<LabOrderQuestionAnswerModel>(MDVUtility.ToStr(AllData["data"]));
            LabOrderHelper helperLabOrder = new LabOrderHelper();
            List<LabOrderTestModel> lstLabOrderTest = new List<LabOrderTestModel>();
            if (lstLabTestModel != null)
            {
                lstLabOrderTest = lstLabTestModel.OfType<LabOrderTestModel>().ToList();
            }
            if (model.commandType.ToLower() == "save_laborder")
            {

                //Start 08-03-2016 Humaira Yousaf for ruleTypes rrr
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                //Start 22-03-2016 Humaira Yousaf for status                        
                model.Status = arrJSON.ContainsKey("Status") == true ? MDVUtility.ToStr(arrJSON["Status"]) : "";
                //End 22-03-2016 Humaira Yousaf for status            
              //  response = helperLabOrder.SaveLabOrder_Obsolete(model, lstLabOrderTest);
                response = helperLabOrder.SaveLabOrder(model, lstLabOrderTest);
                //End 08-03-2016 Humaira Yousaf for ruleTypes
            }
            //Start 15-03-2016//Ahmad Raza// calling helper method to update LabOrder Alert's Status
            else if (model.commandType.ToLower() == "update_laborder")
            {
                response = helperLabOrder.insertUpdateLabOrder_Obsolete(model, lstLabOrderTest);
            }
            //End 15-03-2016//Ahmad Raza// calling helper method to update LabOrder Alert's Status

            //Start 17-03-2016 Humaira Yousaf to load Lab Orders
            else if (model.commandType.ToLower() == "search_laborders")
            {
                response = helperLabOrder.loadLabOrder(model);
            }

                 //Start 04-03-2016 Humaira Yousaf to load LabOrder
            else if (model.commandType.ToLower() == "fill_laborder")
            {
                response = helperLabOrder.fillLabOrder(model);
            }
            else if (model.commandType.ToLower() == "fill_favgrouplaborder")
            {
                response = helperLabOrder.fillFavGroupLabOrder(model);
            }
            else if (model.commandType.ToLower() == "save_abntest")
            {
                string json = JsonConvert.SerializeObject(AllData);
                var id = AllData.SelectToken("LabOrderTestId");
               
                response = helperLabOrder.saveLabTestABN(json);
            }

            //End 04-03-2016 Humaira Yousaf to load RadiologyOrder  

            else if (model.commandType.ToLower() == "getorderingprovider")
            {
                response = helperLabOrder.loadOrderingProvider(model);
            }


            //End 04-03-2016 Humaira Yousaf to load LabOrder  
            //Start 08-03-2016//Ahmad Raza// calling helper method to load LabOrder Alert
            else if (model.commandType.ToLower() == "show_labOrder_alert")
            {
                response = "";// helperLabOrder.loadLabOrderAlert(model);
            }
            else if (model.commandType.ToLower() == "getlatest_laborderby_patientid")
            {
                response = helperLabOrder.loadLabOrder(model);
            }
            else if (model.commandType.ToLower() == "attach_laborder_with_notes")
            {
                response = helperLabOrder.attachLabOrderWithNotes(MDVUtility.ToStr(model.LabOrderId), MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_laborder_from_notes")
            {
                response = helperLabOrder.detachLabOrderFromNotes(MDVUtility.ToStr(model.LabOrderId), MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_orders_from_notes")
            {
                response = helperLabOrder.detachLabOrderFromNotes(model.LabOrderIDs, MDVUtility.ToInt64(model.NoteId));
            }
            //End 08-03-2016//Ahmad Raza// calling helper method to load LabOrder Alert
            else if (model.commandType.ToLower() == "load_patient_insurance")
            {
                response = "";//helperLabOrder.loadLabOrderAlert(model);
            }

            else if (model.commandType.ToLower() == "search_guarantor")
            {
                response = helperLabOrder.loadPatientGuarantor(model);
            }
            else if (model.commandType.ToLower() == "save_lab_order_test")
            {
                response = helperLabOrder.insertUpdateLabOrderTest_Obsolete(MDVUtility.ToInt64(26), lstLabOrderTest, model.CPTCodeQuestionsAnswers);
            }
            //Start 22-03-2016//Ahmad Raza// calling helper method to delete LabOrder
            else if (model.commandType.ToLower() == "delete_laborder")
            {
                response = helperLabOrder.deleteLabOrder(MDVUtility.ToStr(model.LabOrderId));
            }
            //End 22-03-2016//Ahmad Raza// calling helper method to delete LabOrder
            else if (modelLabOrderTest.commandType.ToLower() == "delete_laborder_test")
            {
                response = helperLabOrder.deleteLabOrderTest(modelLabOrderTest.LabOrderTestId, model.PatientId);
            }

            //Start 23-03-2016 Humaira Yousaf to view PDF
            else if (model.commandType.ToLower() == "preview_laborder")
            {
                response = helperLabOrder.previewLabOrder(model);
            }
            //End 23-03-2016 Humaira Yousaf to view PDF
            // By Ahsan Nasir
            else if (model.commandType.ToLower() == "preview_laborderabn")
            {
                string json = JsonConvert.SerializeObject(AllData);

                response = helperLabOrder.previewLabOrderABN(model,json);
            }
            else if (model.commandType.ToLower() == "get_orders_forsoap")
            {
                response = helperLabOrder.getOrdersForSoap(model.LabOrderIDs, MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.NoteId), MDVUtility.ToInt64(model.ProviderId));
            }

            else if (model.commandType.ToLower() == "attach_orders_with_notes")
            {
                response = helperLabOrder.attachOrdersWithNotes(model.LabOrderIDs, MDVUtility.ToLong(model.NoteId));
            }

            else if (model.commandType.ToLower() == "insert_update_favlist_toggle_status")
            {
                response = LabOrderHelper.insertUpdateFavListSetting(model.FavListNames);
            }
            else if (model.commandType.ToLower() == "insert_update_freetext_status")
            {
                response = LabOrderHelper.insertUpdateFreeTextSetting(model.FreeTextNames);
            }
            else if (model.commandType.ToLower() == "get_question_answer_by_cptcode")
            {               
                response = LabOrderHelper.getQuestionsAnswersByCPTCode(quesAnswerModel);
            }
            else if (model.commandType.ToLower() == "laborder_change_patient")
            {
                response = helperLabOrder.labOrderChangePatient(MDVUtility.ToLong(model.LabOrderId), MDVUtility.ToLong(model.PatientId));
            }
            else if (model.commandType.ToLower() == "search_laborders_dashboard")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Lab Order", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = LabOrderHelper.loadLabOrderDashBorad(model);
                }
            }
            else if (model.commandType.ToLower() == "lookup_labtest_report")
            {
                response = LabOrderHelper.LookupLabTestReport(model);
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