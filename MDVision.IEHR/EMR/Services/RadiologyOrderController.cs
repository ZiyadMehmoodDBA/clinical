using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Model.OrdersAndResults.RadiologyOrder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.IEHR.EMR.Helpers.Clinical.Orders;
using MDVision.Common.Utilities;

namespace MDVision.IEHR.EMR.Services
{

    /* Author:  Muhammad Arshad
    * Created Date: 16/03/2016
    * OverView: Created to handle Radiology Order
    */
    public class RadiologyOrderController : ApiController
    {

        // Author:  Muhammad Arshad
        // Created Date: 16/03/2016
        //OverView: Entry point for Radiology Order
        [HttpPost]
        public string RadiologyOrder(JObject AllData)
        {
            string response = null;
            string privilegasMessage = string.Empty;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            RadiologyOrderModel model = JsonConvert.DeserializeObject<RadiologyOrderModel>(MDVUtility.ToStr(AllData["data"]));

            Dictionary<string, dynamic> dictCurrentRadiologyTestJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            RadiologyOrderTestModel modelRadiologyOrderTest = ser.Deserialize<RadiologyOrderTestModel>(MDVUtility.ToStr(AllData["data"]));
            List<object> lstRadiologyTestModel = null;
            if (dictCurrentRadiologyTestJSON.ContainsKey("RadiologyTestIds"))
            {
                string RadiologyTestIds = dictCurrentRadiologyTestJSON["RadiologyTestIds"];
                lstRadiologyTestModel = OrderHelpers.GetListOfObject("RadiologyTestModel", RadiologyTestIds, dictCurrentRadiologyTestJSON);
            }

            RadiologyOrderHelper helperRadiologyOrder = new RadiologyOrderHelper();

            if (model.commandType.ToLower() == "save_radiologyorder")
            {
                //Start 08-03-2016 Humaira Yousaf for ruleTypes
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                //Start 22-03-2016 Humaira Yousaf for status
                model.Status = arrJSON.ContainsKey("Status") == true ? MDVUtility.ToStr(arrJSON["Status"]) : "";
                //End 22-03-2016 Humaira Yousaf for status
                response = helperRadiologyOrder.insertUpdateRadiologyOrder(model, lstRadiologyTestModel);
                //End 08-03-2016 Humaira Yousaf for ruleTypes
            }
            //Start 15-03-2016//Ahmad Raza// calling helper method to update RadiologyOrder Alert's Status
            else if (model.commandType.ToLower() == "update_radiologyorder")
            {
                response = helperRadiologyOrder.insertUpdateRadiologyOrder(model, lstRadiologyTestModel);
            }
            //End 15-03-2016//Ahmad Raza// calling helper method to update RadiologyOrder Alert's Status

            //Start 17-03-2016 Humaira Yousaf to load Radiology Orders
            else if (model.commandType.ToLower() == "search_radiologyorders")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Diagnostic Imaging Order", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperRadiologyOrder.loadRadiologyOrder(model);
                }
            }
            else if (model.commandType.ToLower() == "getorderingprovider")
            {
                response = helperRadiologyOrder.loadOrderingProvider(model);
            }
            //End 17-03-2016 Humaira Yousaf to load Radiology Orders

            //Start 04-03-2016 Humaira Yousaf to load RadiologyOrder
            else if (model.commandType.ToLower() == "fill_radiologyorder")
            {
                response = helperRadiologyOrder.fillRadiologyOrder(model);
            }

            //Start 04-03-2016 Abid Ali to load RadiologyOrder Provider
            else if (model.commandType.ToLower() == "getorderingprovider")
            {
                response = helperRadiologyOrder.loadOrderingProvider(model);
            }
            else if (model.commandType.ToLower() == "get_users")
            {
                response = helperRadiologyOrder.LoadUsers(MDVUtility.ToStr(model.UserName));
            }
            //End 04-03-2016 Humaira Yousaf to  RadiologyOrder Provider

            //Start 08-03-2016//Ahmad Raza// calling helper method to load RadiologyOrder Alert
            else if (model.commandType.ToLower() == "show_RadiologyOrder_alert")
            {
                response = "";// helperRadiologyOrder.loadRadiologyOrderAlert(model);
            }
            else if (model.commandType.ToLower() == "getlatest_radiologyorderby_patientid")
            {
                response = helperRadiologyOrder.loadRadiologyOrder(model);
            }
            else if (model.commandType.ToLower() == "attach_radiologyorder_with_notes")
            {
                response = helperRadiologyOrder.attachRadiologyOrderWithNotes(MDVUtility.ToStr(model.RadiologyOrderId), MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_radiologyorder_from_notes")
            {
                response = helperRadiologyOrder.detachRadiologyOrderFromNotes(MDVUtility.ToStr(model.RadiologyOrderId), MDVUtility.ToInt64(model.NoteId));
            }
            //End 08-03-2016//Ahmad Raza// calling helper method to load RadiologyOrder Alert
            else if (model.commandType.ToLower() == "load_patient_insurance")
            {
                response = "";//helperRadiologyOrder.loadRadiologyOrderAlert(model);
            }

            else if (model.commandType.ToLower() == "search_guarantor")
            {
                response = helperRadiologyOrder.loadPatientGuarantor(model);
            }
            else if (model.commandType.ToLower() == "save_radiology_order_test")
            {
                response = helperRadiologyOrder.insertUpdateRadiologyOrderTest(MDVUtility.ToInt64(26), lstRadiologyTestModel);
            }
            //Start 22-03-2016//Ahmad Raza// calling helper method to delete radiologyOrder
            else if (model.commandType.ToLower() == "delete_radiologyorder")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Diagnostic Imaging Order", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperRadiologyOrder.deleteRadiologyOrder(MDVUtility.ToStr(model.RadiologyOrderId));
                }
            }
            //End 22-03-2016//Ahmad Raza// calling helper method to delete radiologyOrder
            else if (modelRadiologyOrderTest.commandType.ToLower() == "delete_radiologyorder_test")
            {
                response = helperRadiologyOrder.deleteRadiologyOrderTest(modelRadiologyOrderTest.RadiologyOrderTestId);
            }

            //Start 23-03-2016 Humaira Yousaf to view PDF
            else if (model.commandType.ToLower() == "preview_radiologyorder")
            {
                response = helperRadiologyOrder.previewRadiologyOrder(model);
            }
            else if (model.commandType.ToLower() == "saveandattachprocedurereport")
            {
                response = helperRadiologyOrder.SaveAndAttachProcedureReport(model);
            }
            //End 23-03-2016 Humaira Yousaf to view PDF

            else if (model.commandType.ToLower() == "get_orders_forsoap")
            {
                response = helperRadiologyOrder.getOrdersForSoap(model.RadiologyOrderIDs, MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.NoteId), MDVUtility.ToInt64(model.ProviderId));
            }
            else if (model.commandType.ToLower() == "attach_orders_with_notes")
            {
                response = helperRadiologyOrder.attachRadiologyOrderWithNotes(model.RadiologyOrderIDs, MDVUtility.ToLong(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_orders_from_notes")
            {
                response = helperRadiologyOrder.detachRadiologyOrderFromNotes(model.RadiologyOrderIDs, MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "lookup_radiologylabtest_report")
            {
                response = RadiologyOrderHelper.LookupRadiologyTestReport(model);
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