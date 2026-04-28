using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.Orders;
using MDVision.Model.Clinical.OrdersAndResults.ConsultationOrder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class ConsultationOrderController : ApiController
    {
       [HttpPost]
        // Author:  Humaira Yousaf
        // Created Date: 17-03-2016
        //OverView: Entry point for Procedure Orders
        public string ConsultationOrder(JObject AllData)
        {
            string response = null;
            string privilegasMessage = string.Empty;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            ConsultationOrderModel model = JsonConvert.DeserializeObject<ConsultationOrderModel>(MDVUtility.ToStr(AllData["data"]));

            Dictionary<string, dynamic> dictCurrentConsultationTestJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            ConsultationOrderTestModel modelConsultationOrderTest = ser.Deserialize<ConsultationOrderTestModel>(MDVUtility.ToStr(AllData["data"]));
            List<object> lstConsultationTestModel = null;
            if (dictCurrentConsultationTestJSON.ContainsKey("ConsultationTestIds"))
            {
                string ConsultationTestIds = dictCurrentConsultationTestJSON["ConsultationTestIds"];
                lstConsultationTestModel = OrderHelpers.GetListOfObject("ConsultationTestModel", ConsultationTestIds, dictCurrentConsultationTestJSON);
            }

            ConsultationOrderHelper helperConsultationOrder = new ConsultationOrderHelper();

            if (model.commandType.ToLower() == "save_RadiologyOrder")
            {
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                string ruleTypeIds = arrJSON.ContainsKey("RuleTypes") == true ? MDVUtility.ToStr(arrJSON["RuleTypes"]) : "";

                response = "";//helperRadiologyOrder.insertUpdateRadiologyOrder(model, ruleTypeIds, vitalsModel);

            }
            else if (model.commandType.ToLower() == "getorderingprovider")
            {
                response = helperConsultationOrder.loadOrderingProvider(model);
            }
            //Start 18-03-2016 Humaira Yousaf to save consultation Orders

            else if (model.commandType.ToLower() == "save_consultationorder")
            {
                //Start 22-03-2016 Humaira Yousaf for status
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                model.Status = arrJSON.ContainsKey("Status") == true ? MDVUtility.ToStr(arrJSON["Status"]) : "";
                //End 22-03-2016 Humaira Yousaf for status
                response = helperConsultationOrder.insertUpdateConsultationOrder(model, lstConsultationTestModel);
            }
            //End 18-03-2016 Humaira Yousaf to save consultation Orders

            //Start 17-03-2016 Humaira Yousaf to load consultation Orders
            else if (model.commandType.ToLower() == "search_consultationorders")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Consultation", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperConsultationOrder.loadConsultationOrder(model);
                }
            }
            //End 17-03-2016 Humaira Yousaf to load consultation Orders

            //Start 18-03-2016 Humaira Yousaf to fill consultation Orders
            else if (model.commandType.ToLower() == "fill_consultationorder")
            {
                response = helperConsultationOrder.fillConsultationOrder(model);
            }
            //End 18-03-2016 Humaira Yousaf to fill consultation Orders

            //Start 04-03-2016 Humaira Yousaf to load RadiologyOrder
            //else if (model.commandType.ToLower() == "update_ConsultationOrder")
            //{
            //    response = helperConsultationOrder.updateConsultationOrder(model);
            //}
            //End 04-03-2016 Humaira Yousaf to load RadiologyOrder
            //Start 08-03-2016//Ahmad Raza// calling helper method to load RadiologyOrder Alert
            else if (model.commandType.ToLower() == "show_RadiologyOrder_alert")
            {
                response = "";// helperRadiologyOrder.loadRadiologyOrderAlert(model);
            }
            //End 08-03-2016//Ahmad Raza// calling helper method to load RadiologyOrder Alert
            else if (model.commandType.ToLower() == "getlatest_consultationorderby_patientid")
            {
                response = helperConsultationOrder.loadConsultationOrder(model);
            }
            else if (model.commandType.ToLower() == "attach_consultationorder_with_notes")
            {
                response = helperConsultationOrder.attachConsultationOrderWithNotes(model.ConsultationOrderId, MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_consultationorder_from_notes")
            {
                response = helperConsultationOrder.detachConsultationOrderFromNotes(model.ConsultationOrderId, MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "get_orders_forsoap")
            {
                response = helperConsultationOrder.getOrdersForSoap(model.ConsultationOrderIDs, MDVUtility.ToLong(model.PatientId), MDVUtility.ToInt64(model.NoteId));
            }

            else if (model.commandType.ToLower() == "attach_orders_with_notes")
            {
                response = helperConsultationOrder.attachOrdersWithNotes(model.ConsultationOrderIDs, MDVUtility.ToLong(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_orders_from_notes")
            {
                response = helperConsultationOrder.detachConsultationOrderFromNotes(model.ConsultationOrderIDs, MDVUtility.ToInt64(model.NoteId));
            }
            //Start 22-03-2016//Ahmad Raza// calling helper method to delete the consultation Order
            else if (model.commandType.ToLower() == "delete_consultationorder")
            {
                response = helperConsultationOrder.deleteConsultationOrder(model.ConsultationOrderId);
            }
            //End 22-03-2016//Ahmad Raza// calling helper method to delete the consultation Order
            else if (modelConsultationOrderTest.commandType.ToLower() == "delete_consultationorder_test")
            {
                response = helperConsultationOrder.deleteConsultationOrderTest(modelConsultationOrderTest.ConsultationOrderTestId);
            }
            //Start 23-03-2016 Humaira Yousaf to view PDF
            else if (model.commandType.ToLower() == "preview_consultationorder")
            {
                response = helperConsultationOrder.previewConsultationOrder(model);
            }
            //End 23-03-2016 Humaira Yousaf to view PDF

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