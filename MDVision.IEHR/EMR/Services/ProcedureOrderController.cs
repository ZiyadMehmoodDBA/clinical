using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.Orders;
using MDVision.IEHR.EMR.Model.OrdersAndResults.ProcedureOrder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class ProcedureOrderController : ApiController
    {
        [HttpPost]
        // Author:  Humaira Yousaf
        // Created Date: 17-03-2016
        //OverView: Entry point for Procedure Orders
        public string ProcedureOrder(JObject AllData)
        {
            string response = null;
            string privilegasMessage = string.Empty;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            ProcedureOrderModel model = JsonConvert.DeserializeObject<ProcedureOrderModel>(MDVUtility.ToStr(AllData["data"]));

            Dictionary<string, dynamic> dictCurrentProcedureTestJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            ProcedureOrderTestModel modelProcedureOrderTest = ser.Deserialize<ProcedureOrderTestModel>(MDVUtility.ToStr(AllData["data"]));
            List<object> lstProcedureTestModel = null;
            if (dictCurrentProcedureTestJSON.ContainsKey("ProcedureTestIds"))
            {
                string ProcedureTestIds = dictCurrentProcedureTestJSON["ProcedureTestIds"];
                lstProcedureTestModel = OrderHelpers.GetListOfObject("ProcedureTestModel", ProcedureTestIds, dictCurrentProcedureTestJSON);
            }

            ProcedureOrderHelper helperProcedureOrder = new ProcedureOrderHelper();

            if (model.commandType.ToUpper() == "SAVE_PROCEDURE_ORDER")
            {
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                string ruleTypeIds = arrJSON.ContainsKey("RuleTypes") == true ? MDVUtility.ToStr(arrJSON["RuleTypes"]) : "";
                //Start 22-03-2016 Humaira Yousaf for status
                model.Status= arrJSON.ContainsKey("Status") == true ? MDVUtility.ToStr(arrJSON["Status"]) : "";
                //End 22-03-2016 Humaira Yousaf for status
                response = helperProcedureOrder.saveProcedureOrder(model, lstProcedureTestModel);
                //response = helperProcedureOrder.ProcedureOrderInsertUpdate(model, lstProcedureTestModel);

            }
            else if (model.commandType.ToLower() == "getorderingprovider")
            {
                response = helperProcedureOrder.loadOrderingProvider(model);
            }
            //Start 15-03-2016//Ahmad Raza// calling helper method to update RadiologyOrder Alert's Status
            else if (model.commandType.ToLower() == "update_RadiologyOrder_status")
            {
                response = "";// helperRadiologyOrder.updateRadiologyOrdertatus(model);
            }
            //End 15-03-2016//Ahmad Raza// calling helper method to update RadiologyOrder Alert's Status

            //Start 17-03-2016 Humaira Yousaf to load Radiology Orders
            else if (model.commandType.ToLower() == "search_procedureorders")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Procedure", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperProcedureOrder.loadProcedureOrder(model);
                }
            }
            //End 17-03-2016 Humaira Yousaf to load Radiology Orders

            //Start 04-03-2016 Humaira Yousaf to load RadiologyOrder
            else if (model.commandType.ToLower() == "fill_procedureorder")
            {
                response = helperProcedureOrder.loadProcedureOrder(model);
            }
            //End 04-03-2016 Humaira Yousaf to load RadiologyOrder

            else if (model.commandType.ToLower() == "delete_procedureorder")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Procedure", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperProcedureOrder.deleteProcedureOrder(model.ProcedureOrderId);
                }
            }
            //Start 08-03-2016//Ahmad Raza// calling helper method to load RadiologyOrder Alert
            else if (model.commandType.ToLower() == "show_radiologyorder_alert")
            {
                response = "";// helperRadiologyOrder.loadRadiologyOrderAlert(model);
            }
            else if (model.commandType.ToLower() == "getlatest_procedureorderby_patientid")
            {
                response = helperProcedureOrder.loadProcedureOrder(model);
            }
            else if (model.commandType.ToLower() == "get_orders_forsoap")
            {
                response = helperProcedureOrder.getOrdersForSoap(model.ProcedureOrderIDs, MDVUtility.ToLong(model.PatientId), Convert.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "attach_procedureorder_with_notes")
            {
                response = helperProcedureOrder.attachProcedureOrderWithNotes(model.ProcedureOrderId, Convert.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "attach_orders_with_notes")
            {
                response = helperProcedureOrder.attachProcedureOrderWithNotes(model.ProcedureOrderIDs, Convert.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_procedureorder_from_notes")
            {
                response = helperProcedureOrder.detachProcedureOrderFromNotes(model.ProcedureOrderId, Convert.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "detach_orders_from_notes")
            {
                response = helperProcedureOrder.detachProcedureOrderFromNotes(model.ProcedureOrderIDs, Convert.ToInt64(model.NoteId));
            }

            //End 08-03-2016//Ahmad Raza// calling helper method to load RadiologyOrder Alert

            //Start 22-03-2016 Humaira Yousaf to view PDF
            else if (model.commandType.ToLower() == "preview_procedureorder")
            {
                response = helperProcedureOrder.previewProcedureOrder(model);
            }
            //End 22-03-2016 Humaira Yousaf to view PDF

            else if (modelProcedureOrderTest.commandType.ToLower() == "delete_procedureorder_test")
            {
                response = helperProcedureOrder.deleteProcedureOrderTest(modelProcedureOrderTest.ProcedureOrderTestId, modelProcedureOrderTest.PatientId);
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