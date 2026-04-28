
using MDVision.Business.BCommon;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewOfSystem;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.IEHR.EMR.Model.ReviewofSystems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems;
using MDVision.Common.Utilities;
using MDVision.Model;
using MDVision.Model.Clinical.Templates.OrderSets;
using MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.Patient;
using MDVision.IEHR.EMR.Model.Clinical.Templates.OrderSets;
using MDVision.Model.Clinical.FollowUp;
using MDVision.IEHR.EMR.Model.Clinical.Immunization;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.Model.Clinical.Orderset;
using System.Xml;
using System.Text;
using HtmlAgilityPack;
using System.Drawing;
using System.IO;
using MDVision.Datasets;
using MDVision.Business.BLL;
using System.Text.RegularExpressions;
using MDVision.IEHR.EMR.Model.OrdersAndResults.ProcedureOrder;

namespace MDVision.IEHR.EMR.Services
{
    public class OrderSetController : ApiController
    {

        [HttpPost]
        public string OrderSet(JObject AllData)
        {
            string response = null;
            OrderSetHelper OrderSetsHelper = new OrderSetHelper();
            OrderSetProblemModel problemModel = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            OrderSetModel model = ser.Deserialize<OrderSetModel>(MDVUtility.ToStr(AllData["data"]));
            if (model.commandType.ToLower() == "delete_orderset_problemlist")
            {
                problemModel = JsonConvert.DeserializeObject<OrderSetProblemModel>(MDVUtility.ToStr(AllData["data"]));
            }
            if (model.commandType.ToLower() == "load_order_set")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OrderSetsHelper.loadOrderSet(model);
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
            else if (model.commandType.ToLower() == "fill_order_set")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OrderSetsHelper.fillOrderSet(model);
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
            else if (model.commandType.ToLower() == "load_patient_and_ord_problems")
            {
                response = OrderSetsHelper.LoadPatientAndOrdProblems(model);
            }
            else if (model.commandType.ToLower() == "update_order_set")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OrderSetsHelper.updateOrderSet(model);
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
            else if (model.commandType.ToLower() == "search_ros_systems_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets ", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    //response = OrderSetsHelper.loadROSTemplates(model.IsActive, MDVUtility.ToInt64(model.OrderSetsId), MDVUtility.ToInt64(model.PageNumber), MDVUtility.ToInt64(model.RowsPerPage), MDVUtility.ToInt64(model.EntityId));
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
            else if (model.commandType.ToLower() == "update_order_set_active_inactive")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OrderSetsHelper.activeInactiveOrderSet(model.OrderSetId, model.IsActive);
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
            else if (model.commandType.ToLower() == "delete_order_set")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets ", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OrderSetsHelper.deleteOrderSet(model.OrderSetId);
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
            else if (model.commandType.ToLower() == "delete_orderset_problemlist")
            {
                response = OrderSetsHelper.deleteOrderSetProblemList(MDVUtility.ToStr(problemModel.OrderSetProblemId));
            }

            else if (model.commandType.ToLower() == "detach_orderset_from_notes")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Problems List", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = OrderSetsHelper.detach_OrderSet_From_Note(model);
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
            else if (model.commandType.ToLower() == "save_order_set")
            {
                Int64 OrderSetsId = MDVUtility.ToInt64(model.OrderSetId);
                string privilegasMessage = string.Empty;
                if (OrderSetsId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets ", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets ", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {

                    response = OrderSetsHelper.insertOrderSet(model);
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
            else if (model.commandType.ToLower() == "attach_order_set_to_note")
            {
                Int64 OrderSetsId = MDVUtility.ToInt64(model.OrderSetId);
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OrderSetsHelper.attachOrderSetWithNote(MDVUtility.ToLong(model.NotesId), MDVUtility.ToLong(model.PatientId), model.OrderSetId, model.OrderSetComponents, model.ProblemListIDs, model.ProceduresIDs, model.LabOrderIDs, model.RadiologyOrderIDs, model.FollowUpIDs, model.PatientEducationIDs, model.ReferralsIDs, model.ImmunizationIDs, model.TherapeuticIDs, model.MedicationIDs, model.ProcedureOrderIDs, MDVUtility.ToInt64(model.ProviderId), model.AddInValidAgeRecordsInHxTab,model.PatientProblemIds,model.OrderSetAssociatedProblemIds);
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
            else if (model.commandType.ToLower() == "attach_note_order_sets_to_note")
            {
                Int64 OrderSetsId = MDVUtility.ToInt64(model.OrderSetId);
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OrderSetsHelper.attachNoteOrderSetsWithNote(MDVUtility.ToLong(model.NotesId), MDVUtility.ToLong(model.PatientId), model.OrderSetIds, model.OrderSetComponents);
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
            else if (model.commandType.ToLower() == "saveas_order_set")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets ", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OrderSetsHelper.saveAsOrderSet(model);
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
            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }

        [HttpPost]
        public string OrderSetPatientEducation(JObject AllData)
        {
            string response = null;
            OS_PatientEducation OS_EducationHelper = new OS_PatientEducation();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            OrderSetPatientEducationModel model = ser.Deserialize<OrderSetPatientEducationModel>(MDVUtility.ToStr(AllData["data"]));
            if (model.commandType.ToLower() == "load_ordersetpatienteducation")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_EducationHelper.loadOrderSetPatientEducation(model);
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
            else if (model.commandType.ToLower() == "delete_ordersetpatienteducation")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_EducationHelper.deleteOrderSetPatientEducation(model.OrderSetPatEducationId, model.DocId);
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
            else if (model.commandType.ToLower() == "insert_ordersetpatienteducation")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_EducationHelper.insertOrderSetPatientEducation(model);
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
            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }
        [HttpPost]
        public string ProblemList(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            OS_ProblemListModel model = ser.Deserialize<OS_ProblemListModel>(MDVUtility.ToStr(AllData["data"]));

            OS_ProblemListHelper helperProblemList = new OS_ProblemListHelper();


            if (model.commandType.ToLower() == "search_problemlist")
            {
                response = null;
                response = helperProblemList.LoadProblemListOp(model);
            }
            else if (model.commandType.ToLower() == "save_problemlist")
            {
                response = null;
                response = helperProblemList.SaveProblemListOp(model);
            }
            else if (model.commandType.ToLower() == "calljs")
            {
                response = null;
                //ScriptManager.RegisterStartupScript(this, this., Guid.NewGuid().ToString() + new Random().ToString().Replace(".", ""), "alert('ahmed');", true);
            }
            else if (model.commandType.ToLower() == "update_problemlist")
            {
                response = null;
                response = helperProblemList.UpdateProblemListOp(model);
            }
            else if (model.commandType.ToLower() == "updateproblemindrfirstforgrid")
            {
                response = null;
                // response = helperProblemList.UpdateProblemInDrFirstForGrid();
            }
            else if (model.commandType.ToLower() == "update_problemlistcomments")
            {
                response = null;
                response = helperProblemList.UpdateProblemListComments(model);
            }
            else if (model.commandType.ToLower() == "delete_problemlist")
            {
                response = null;
                response = helperProblemList.DeleteProblemListOp(model);
            }

            else if (model.commandType.ToLower() == "inactive_problemlist")
            {
                response = null;
                response = helperProblemList.ActiveInActiveProblemListOp(model);
            }




            //Start || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list
            else if (model.commandType.ToLower() == "get_all_problemlists")
            {
                // response = helperProblemList.getAllProblemLists(model);
            }

            //End   || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list




            //Start 27-10-2016 Humaira Yousaf to log view action for problem lists
            else if (model.commandType.ToLower() == "logviewproblemlist")
            {
                response = null;
                // response = helperProblemList.LogProblemListView(model);
            }
            //End 27-10-2016 Humaira Yousaf to log view action for problem lists
            return response;
        }

        [HttpPost]
        public string Immunization(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            OS_ImmunizationModel model = ser.Deserialize<OS_ImmunizationModel>(MDVUtility.ToStr(AllData["data"]));

            OS_ImmunizationHelper helper_OS_Immunization = new OS_ImmunizationHelper();


            if (model.commandType.ToLower() == "search_problemlist")
            {
                response = null;
                //response = helperProblemList.LoadProblemListOp(model);
            }
            else if (model.commandType.ToLower() == "save_administervaccine" || model.commandType.ToLower() == "save_vacinehxdose" || model.commandType.ToLower() == "save_vacinerefusalrecord")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helper_OS_Immunization.SaveAministerVaccine(model);
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
            else if (model.commandType.ToLower() == "search_immunization")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helper_OS_Immunization.loadParentChildImmunization(model);
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
            else if (model.commandType.ToLower() == "search_vacinehxforedit")
            {
                response = helper_OS_Immunization.SearchVacinehxForEdit(model);
            }
            else if (model.commandType.ToLower() == "get_agelim_sche_categ_against_vaccshedid")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helper_OS_Immunization.GetAgeLimScheCategAgainstVaccShedId(model);
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
            else if (model.commandType.ToLower() == "update_vacinehxdose" || model.commandType.ToLower() == "update_vacinerefusalrecord")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helper_OS_Immunization.updateAdministerVaccine(model);
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
            else if (model.commandType.ToLower() == "delete_os_vaccinehx")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helper_OS_Immunization.DeleteOsVaccinehx(model);
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
            else if (model.commandType.ToLower() == "is_vaccinehx_in_valid_age")
            {
                response = helper_OS_Immunization.IsVaccineHxInValidAge(model);
            }
            else if (model.commandType.ToLower() == "is_vaccinehx_lot_issue")
            {
                response = helper_OS_Immunization.IsVaccineHxLotIssue(model);
            }
            
            
            return response;
        }

        [HttpPost]
        public string IMMUNIZATIONTHERAPEUTICINJECTION(JObject allData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            TherapeuticInjectionModel model = ser.Deserialize<TherapeuticInjectionModel>(MDVUtility.ToStr(allData["data"]));

            OS_ImmunizationHelper helperImmunization = new OS_ImmunizationHelper();

            if (model.commandType.ToLower() == "save_therapeutic_injection")
            {
                response = null;
                response = helperImmunization.Save_Therapeutic_Injection(model);
            }
            else if (model.commandType.ToLower() == "search_therapeutic")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = null;
                    response = helperImmunization.SearchImmunizationTherapeuticInjection(model);
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
            else if (model.commandType.ToLower() == "update_therapeutic_injection")
            {
                response = null;
                response = helperImmunization.Update_Therapeutic_Injection(model);
            }
            else if (model.commandType.ToLower() == "delete_os_therapeutic_hx")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = null;
                    response = helperImmunization.DeleteOsTherapeutichx(model);
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
            return response;
        }
        
        [HttpPost]
        public string Procedure(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            OS_ProceduresModel model = ser.Deserialize<OS_ProceduresModel>(MDVUtility.ToStr(AllData["data"]));
            List<OS_ProceduresDetailModel> modelList = ser.Deserialize<List<OS_ProceduresDetailModel>>(MDVUtility.ToStr(AllData["data"]));
            OS_ProceduresDetailModel detailModel = ser.Deserialize<OS_ProceduresDetailModel>(MDVUtility.ToStr(AllData["data"]));

            OS_ProceduresHelper helper = new OS_ProceduresHelper();
            string privilegasMessage = "";
            if (model.commandType.ToLower() == "search_procedures")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.loadProcedures(model);
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
            else if (model.commandType.ToLower() == "save_procedures")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.saveProcedure(model);
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
            else if (model.commandType.ToLower() == "save_proceduresForVaccine")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Immunization", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.saveProcedureForVaccine(model);
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
            else if (model.commandType.ToLower() == "update_procedure")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.updateProcedures(model.procedureDetailModel);
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
            else if (model.commandType.ToLower() == "delete_procedure")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.deleteProcedure(detailModel);
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

            else if (model.commandType.ToLower() == "inactive_procedures")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.ActiveInActiveProcedures(model);
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

            return response;
        }

        [HttpPost]
        public string OrderSetPatientReferralsOutgoingDetail(JObject AllData)
        {
            string response = null;
            OS_Referrals OS_ReferralHelper = new OS_Referrals();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            OrderSetPatientReferralModel model = ser.Deserialize<OrderSetPatientReferralModel>(MDVUtility.ToStr(AllData["data"]));
            if (model.commandType.ToLower() == "save_ordersetreferral")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_ReferralHelper.insertOrderSetPatientReferralsOutgoingDetail(model);
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
            else if (model.commandType.ToLower() == "fill_ordersetreferral")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_ReferralHelper.loadOrderSetPatientReferralsOutgoingDetail(model);
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
            else if (model.commandType.ToLower() == "update_ordersetreferral")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_ReferralHelper.updateOrderSetPatientReferralsOutgoingDetail(model);
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
            else if (model.commandType.ToLower() == "delete_ordersetreferral")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_ReferralHelper.deleteOrderSetPatientReferralsOutgoingDetail(model.OrderSetReferralId);
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
            else if (model.commandType.ToLower() == "delete_referral_procedure")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_ReferralHelper.deleteOrderSetReferralsProcedure(model.OrderSetReferralProcedureId);
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
            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }

        [HttpPost]
        public string OrderSetFollowUp(JObject AllData)
        {
            string response = null;
            OS_FollowUp OS_FollowUpHelper = new OS_FollowUp();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            OrdertSetFollowUpModel model = ser.Deserialize<OrdertSetFollowUpModel>(MDVUtility.ToStr(AllData["data"]));
            if (model.commandType.ToLower() == "save_ordersetfollowup")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_FollowUpHelper.insertOrderSetFollowUp(model);
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
            else if (model.commandType.ToLower() == "delete_ordersetfollowup")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_FollowUpHelper.deleteOrderSetFollowUp(model.FollowUpId);
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
            else if (model.commandType.ToLower() == "fill_ordersetfollowup")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_FollowUpHelper.loadOrderSetFollowUp(model);
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
            else if (model.commandType.ToLower() == "update_ordersetfollowup")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Order Sets", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = OS_FollowUpHelper.updateOrderSetFollowUp(model);
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
            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }
        [HttpPost]
        public string LabOrder(JObject AllData)
        {
            string response = null;


            JavaScriptSerializer ser = new JavaScriptSerializer();
            OS_LabOrderModel model = JsonConvert.DeserializeObject<OS_LabOrderModel>(MDVUtility.ToStr(AllData["data"]));

            Dictionary<string, dynamic> dictCurrentLabTestJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            OS_LabOrderTestModel modelLabOrderTest = ser.Deserialize<OS_LabOrderTestModel>(MDVUtility.ToStr(AllData["data"]));
            List<object> lstLabTestModel = null;
            if (dictCurrentLabTestJSON.ContainsKey("LabTestIds"))
            {
                string LabTestIds = dictCurrentLabTestJSON["LabTestIds"];
                lstLabTestModel = OrderHelpers.GetListOfObject("LabTestModel", LabTestIds, dictCurrentLabTestJSON);
            }
            OS_LabOrderQuestionAnswerModel quesAnswerModel = JsonConvert.DeserializeObject<OS_LabOrderQuestionAnswerModel>(MDVUtility.ToStr(AllData["data"]));
            OS_LabOrderHelper helperLabOrder = new OS_LabOrderHelper();

            if (model.commandType.ToLower() == "save_laborder")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Lab Order", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    //Start 08-03-2016 Humaira Yousaf for ruleTypes
                    Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                    //Start 22-03-2016 Humaira Yousaf for status
                    model.Status = arrJSON.ContainsKey("Status") == true ? MDVUtility.ToStr(arrJSON["Status"]) : "";
                    //End 22-03-2016 Humaira Yousaf for status
                    response = helperLabOrder.insertUpdateLabOrder(model, lstLabTestModel);
                    //End 08-03-2016 Humaira Yousaf for ruleTypes
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
            //Start 15-03-2016//Ahmad Raza// calling helper method to update LabOrder Alert's Status
            else if (model.commandType.ToLower() == "update_laborder")
            {
                response = helperLabOrder.insertUpdateLabOrder(model, lstLabTestModel);
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
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Lab Order", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperLabOrder.fillLabOrder(model);
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
                response = helperLabOrder.insertUpdateLabOrderTest(MDVUtility.ToInt64(26), lstLabTestModel, model.CPTCodeQuestionsAnswers);
            }
            //Start 22-03-2016//Ahmad Raza// calling helper method to delete LabOrder
            else if (model.commandType.ToLower() == "delete_laborder")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Lab Order", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperLabOrder.deleteLabOrder(MDVUtility.ToStr(model.LabOrderId));
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
            //End 22-03-2016//Ahmad Raza// calling helper method to delete LabOrder
            else if (modelLabOrderTest.commandType.ToLower() == "delete_laborder_test")
            {
                response = helperLabOrder.deleteLabOrderTest(modelLabOrderTest.LabOrderTestId);
            }

            //Start 23-03-2016 Humaira Yousaf to view PDF
            else if (model.commandType.ToLower() == "preview_laborder")
            {
                response = helperLabOrder.previewLabOrder(model);
            }
            else if (model.commandType.ToLower() == "search_laborders_dashboard")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Lab Order", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperLabOrder.loadLabOrderDashBorad(model);
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
            else if (model.commandType.ToLower() == "get_question_answer_by_cptcode")
            {
                response = helperLabOrder.getQuestionsAnswersByCPTCode(quesAnswerModel);
            }
            //End 23-03-2016 Humaira Yousaf to view PDF
            return response;
        }
        [HttpPost]
        public string RadiologyOrder(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            OS_RadiologyOrderModel model = JsonConvert.DeserializeObject<OS_RadiologyOrderModel>(MDVUtility.ToStr(AllData["data"]));

            Dictionary<string, dynamic> dictCurrentRadiologyTestJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            OS_RadiologyOrderTestModel modelRadiologyOrderTest = ser.Deserialize<OS_RadiologyOrderTestModel>(MDVUtility.ToStr(AllData["data"]));
            List<object> lstRadiologyTestModel = null;
            if (dictCurrentRadiologyTestJSON.ContainsKey("RadiologyTestIds"))
            {
                string RadiologyTestIds = dictCurrentRadiologyTestJSON["RadiologyTestIds"];
                lstRadiologyTestModel = OrderHelpers.GetListOfObject("RadiologyTestModel", RadiologyTestIds, dictCurrentRadiologyTestJSON);
            }

            OS_RadiologyOrderHelper helperRadiologyOrder = new OS_RadiologyOrderHelper();

            if (model.commandType.ToLower() == "save_radiologyorder")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Diagnostic Imaging Order", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    //Start 08-03-2016 Humaira Yousaf for ruleTypes
                    Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                //Start 22-03-2016 Humaira Yousaf for status
                model.Status = arrJSON.ContainsKey("Status") == true ? MDVUtility.ToStr(arrJSON["Status"]) : "";
                //End 22-03-2016 Humaira Yousaf for status
                response = helperRadiologyOrder.insertUpdateRadiologyOrder(model, lstRadiologyTestModel);
                    //End 08-03-2016 Humaira Yousaf for ruleTypes
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
            //Start 15-03-2016//Ahmad Raza// calling helper method to update RadiologyOrder Alert's Status
            else if (model.commandType.ToLower() == "update_radiologyorder")
            {
                response = helperRadiologyOrder.insertUpdateRadiologyOrder(model, lstRadiologyTestModel);
            }
            //End 15-03-2016//Ahmad Raza// calling helper method to update RadiologyOrder Alert's Status

            //Start 17-03-2016 Humaira Yousaf to load Radiology Orders
            else if (model.commandType.ToLower() == "search_radiologyorders")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Diagnostic Imaging Order", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperRadiologyOrder.loadRadiologyOrder(model);
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
            else if (model.commandType.ToLower() == "getorderingprovider")
            {
                response = helperRadiologyOrder.loadOrderingProvider(model);
            }
            //End 17-03-2016 Humaira Yousaf to load Radiology Orders

            //Start 04-03-2016 Humaira Yousaf to load RadiologyOrder
            else if (model.commandType.ToLower() == "fill_radiologyorder")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Diagnostic Imaging Order", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperRadiologyOrder.fillRadiologyOrder(model);
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

            //Start 04-03-2016 Abid Ali to load RadiologyOrder Provider
            else if (model.commandType.ToLower() == "getorderingprovider")
            {
                response = helperRadiologyOrder.loadOrderingProvider(model);
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
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results_Diagnostic Imaging Order", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperRadiologyOrder.deleteRadiologyOrder(MDVUtility.ToStr(model.RadiologyOrderId));
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
            else if (model.commandType.ToLower() == "lookup_radiologylabtest_report")
            {
                response = helperRadiologyOrder.LookupRadiologyTestReport(model);
            }

            return response;
        }

        [HttpPost]
        public string NoteOrderSet(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            OS_NoteModel model = JsonConvert.DeserializeObject<OS_NoteModel>(MDVUtility.ToStr(AllData["data"]));

            Dictionary<string, dynamic> dictCurrentRadiologyTestJSON = ser.Deserialize<Dictionary<string, dynamic>>(MDVUtility.ToStr(AllData["data"]));
            OS_NoteModel modelNoteOrderTest = ser.Deserialize<OS_NoteModel>(MDVUtility.ToStr(AllData["data"]));

            OrderSetHelper helperNoteOrderSet = new OrderSetHelper();

            if (model.commandType.ToLower() == "save_note_order_set")
            {
                response = helperNoteOrderSet.insertNoteOrderSet(model);
            }
            else if (model.commandType.ToLower() == "update_note_order_set")
            {
                response = helperNoteOrderSet.updateNoteOrderSet(model);
            }
            else if (model.commandType.ToLower() == "delete_note_order_set")
            {
                response = helperNoteOrderSet.deleteNoteOrderSet(model);
            }
            return response;
        }

        [HttpPost]
        public string MEDICATION(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            OS_MedicationModel model = ser.Deserialize<OS_MedicationModel>(MDVUtility.ToStr(AllData["data"]));
            OS_MedicationHelper helper_MedicationHelper = new OS_MedicationHelper();
            if (model.commandType.ToLower() == "get_drug_array")
            {
                RcopiaHelper helper_RcopiaHelper = new RcopiaHelper();
                response = null;
                response = helper_RcopiaHelper.GetDrugNamesFDB(model.DrugName);
            }
            else if (model.commandType.ToLower() == "save_os_medication")
            {
                
                response = null;
                response = helper_MedicationHelper.SaveOSMedication(model);
            }
            else if (model.commandType.ToLower() == "search_medication")
            {
                response = null;
                response = helper_MedicationHelper.LoadMedication(model);
            }
            else if (model.commandType.ToLower() == "update_os_medication")
            {
                response = null;
                response = helper_MedicationHelper.UpdateOSMedication(model);
            }
            else if (model.commandType.ToLower() == "delete_os_medication")
            {
                response = null;
                response = helper_MedicationHelper.DeleteOSMedication(model);
            }
            return response;
        }
        [HttpPost]
        public string GetInfobuttonDetails(JObject allData)
        {
            var datastr = string.Empty;
            var statusstr = true;
            var message = string.Empty;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            dynamic data = ser.Deserialize<dynamic>(MDVUtility.ToStr(allData["data"]));
            var connectPlusResponse = connectMedlinePlus(Convert.ToString(data["searchStr"]), Convert.ToString(data["codeSystem"]));

            string patientId = Convert.ToString(data["PatientId"]);
            string parentCtrl = Convert.ToString(data["Caller"]);
            string providerId = Convert.ToString(data["ProviderId"]);
            string orderSetId = Convert.ToString(data["OrderSetId"]);

            if (connectPlusResponse.data != null)
            {
                XmlDocument doc = connectPlusResponse.data;
                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("feed", "http://www.w3.org/2005/Atom");
                XmlNodeList nodes = doc.SelectNodes("//feed:entry", nsmgr);
                int EntryCounter = 0;
                var html = "<!DOCTYPE html><html><head></head><body>";
                html += "<div id='AllContent' style='padding-left: 50px; padding-right: 50px; '>";
                foreach (XmlNode n in nodes)
                {
                    EntryCounter++;
                    html += "<div id='entry" + EntryCounter + "'>";
                    foreach (XmlNode subNode in n.ChildNodes)
                    {
                        if (subNode.Name.ToLower() == "title")
                        {
                            html += "<h3>" + subNode.InnerText + "</h3>"; // Title
                        }
                        else if (subNode.Name.ToLower() == "summary")
                        {
                            html += subNode.InnerText;         // HTML
                        }
                    }
                    html += "</div>";
                }

                html += "</div></body></html>";
                BLObject<byte[]> objLoad = ModifyHtml(html, patientId, parentCtrl, providerId);

                if (objLoad.Data != null)
                {
                    var bytearray = objLoad.Data;
                    var base64Str = Convert.ToBase64String(bytearray);

                    if (!objLoad.Message.Equals("No Information Found"))
                    {
                        OrderSetPatientEducationModel ptEduModel = new OrderSetPatientEducationModel
                        {
                            OrderSetId = orderSetId,
                            DocType = "1", //Info Data
                            FileType = "application/html",
                            DocumentName = objLoad.Message,
                            DocId = "0",
                            Comments = "",
                            FileStream = base64Str
                        };

                        OS_PatientEducation ptEdu = OS_PatientEducation.Instance();
                        var responseptEdu = ptEdu.InsertAdmin_OSPatientEducation(ptEduModel);
                        data = ser.Deserialize<dynamic>(responseptEdu);
                        datastr = data["FileStream"];
                    }
                    else
                    {
                        datastr = base64Str;
                    }
                }
                else
                {
                    message = objLoad.Message;
                    statusstr = false;
                }
            }
            else
            {
                message = connectPlusResponse.Message;
                statusstr = false;
            }

            var response = new
            {
                data = datastr,
                Message = message,
                status = statusstr
            };
            return (JsonConvert.SerializeObject(response));
        }
        private object connectMedlinePlus(string searchStr, string codeSection)
        {
            XmlDocument response = null;
            var error = string.Empty;
            try
            {
                //Web Service Call
                //string url = string.Format("https://apps.nlm.nih.gov/medlineplus/services/mpconnect_service.cfm?mainSearchCriteria.v.c={0}&mainSearchCriteria.v.cs={1}", searchStr, codeSection);
                //Web Application Call
                //  string url = string.Format("https://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.c={0}&mainSearchCriteria.v.cs={1}", searchStr, codeSection);
                string url = string.Format(WebConfigurationManager.AppSettings["MedlineURL"].ToString()
                    + WebConfigurationManager.AppSettings["MedlineParam1"].ToString() + "{1}" + "&"
                    + WebConfigurationManager.AppSettings["MedlineParam2"].ToString() + "{0}", searchStr, codeSection);

                XmlDocument doc = new XmlDocument();
                doc.Load(url);


                if (doc != null)
                {
                    response = doc;
                }
                else
                {
                    error = "No Information Found";
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new
            {
                data = response,
                Message = error
            };
        }
        private BLObject<byte[]> ModifyHtml(string htmlContent, string patientId, string parentCtrl, string providerId)
        {
            try
            {
                htmlContent = RemoveComments(htmlContent);
                string caller = string.Empty;
                var noinfoFound = string.Empty;
                HtmlDocument document = new HtmlDocument();
                HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Empty;
                document.LoadHtml(htmlContent);

                string patientTable = string.Empty;

                //Create Div for Header
                HtmlNode headerDiv = document.CreateElement("div");
                headerDiv.Attributes.Add("id", "mplus-wrap");

                HtmlNode contentDiv = document.CreateElement("div");
                contentDiv.Attributes.Add("id", "mplus-content");

                HtmlNode serviceDiv = document.CreateElement("div");
                serviceDiv.Attributes.Add("id", "serviceContent");

                HtmlNode resultsDiv = document.CreateElement("div");
                resultsDiv.Attributes.Add("class", "results col-xs-12");

                HtmlNode resultDiv = document.CreateElement("div");
                resultDiv.Attributes.Add("class", "result col-xs-12 pl-none pt-sm");

                HtmlNode tblHeading = document.CreateElement("table");
                tblHeading.Attributes.Add("style", "width:100%");

                HtmlNode divStyle = document.CreateElement("div");
                divStyle.Attributes.Add("id", "separatorDiv");
                divStyle.Attributes.Add("style", "float:left; width:100%; border-bottom:22px solid #005da9;margin-top:5px; margin-bottom:5px;");

                HtmlNode bodyNode = document.DocumentNode.SelectSingleNode("html/body");
                HtmlNode contentNode = bodyNode.ChildNodes.FindFirst("div");

                string footerDivBar = "<div style='float:left;border-top:3px solid #005da9;width:100%;padding-top:3px;'></div>";

                HtmlNode footerNode = document.CreateElement("div");
                footerNode.Attributes.Add("id", "ptEduFooter");
                footerNode.Attributes.Add("style", "float:left;width:100%;background:#005da9 !important; color:#fff !important;padding:5px 25px");
                DSReportHeader dsreportHeader = null;

                BLObject<DSReportHeader> obj = new BLLAdminClinical().getReportHeaderTagsValue(MDVUtility.ToInt64(patientId), MDVUtility.ToInt64(providerId), -1, "Patient Education");
                dsreportHeader = obj.Data;
                if (dsreportHeader != null && dsreportHeader.ReportHeaderTags.Rows.Count > 0)
                {
                    DSReportHeader.ReportHeaderTagsRow dr = (DSReportHeader.ReportHeaderTagsRow)dsreportHeader.Tables[dsreportHeader.ReportHeaderTags.TableName].Rows[0];

                    //if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.HeaderLogoColumn.ColumnName])) && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PracticeTextColumn.ColumnName])) && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PatientTextColumn.ColumnName]))
                    //    && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.ProviderTextColumn.ColumnName])) && !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.FooterTextColumn.ColumnName])))
                    bool IsCustomHdear = bool.Parse(dsreportHeader.ReportHeaderTags.Rows[0][9].ToString());
                    if (IsCustomHdear == true)
                    {
                        HtmlNode tblBody = document.CreateElement("tbody");
                        HtmlNode tbltr = document.CreateElement("tr");
                        HtmlNode tbltd = document.CreateElement("td");
                        tbltd.SetAttributeValue("style", "width:70%;padding-left:0px;");
                        HtmlNode logoImg = document.CreateElement("img");
                        logoImg.SetAttributeValue("src", MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.HeaderLogoColumn.ColumnName]));
                        logoImg.SetAttributeValue("style", "max-width: 350px;max-height:140px;");
                        tbltd.AppendChild(logoImg);

                        HtmlNode tbltdH = document.CreateElement("td");
                        //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                        tbltdH.SetAttributeValue("style", "width:30%;vertical-align:top;text-align:right;");
                        //END Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090

                        var arrPracticeTextColumn = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PracticeTextColumn.ColumnName]).Split(new[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x));
                        string finalstrPractice = "";
                        foreach (string PracticeColumn in arrPracticeTextColumn)
                        {
                            finalstrPractice += PracticeColumn + "<br/>";
                        }
                        //tbltdH.InnerHtml = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PracticeTextColumn.ColumnName]);
                        tbltdH.InnerHtml = finalstrPractice;
                        tbltr.AppendChild(tbltd);
                        tbltr.AppendChild(tbltdH);
                        tblBody.AppendChild(tbltr);
                        tblHeading.AppendChild(tblBody);

                        //var arrProvider =  MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.ProviderTextColumn.ColumnName]).Replace("<br/>", "~").Split('~');
                        //var arrPatient = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PatientTextColumn.ColumnName]).Replace("<br/>", "~").Split('~');
                        //string finalstrProvider = "";
                        //foreach (var item in arrProvider)
                        //{
                        //    var newstr = item.TrimStart().TrimEnd();
                        //    if (newstr != "")
                        //    {
                        //        finalstrProvider += newstr + "<br/>";
                        //    }
                        //}
                        //var finalstrPatient = "";
                        //foreach (var item in arrPatient)
                        //{
                        //    var newstr = item.TrimStart().TrimEnd();
                        //    if (newstr != "")
                        //    {
                        //        finalstrPatient += newstr + "<br/>";
                        //    }
                        //}
                        var arrProvider = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.ProviderTextColumn.ColumnName]).Split(new[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x));
                        var arrPatient = MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.PatientTextColumn.ColumnName]).Split(new[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x));
                        string finalstrPatient = "";
                        string finalstrProvider = "";
                        foreach (string ProviderColumn in arrProvider)
                        {
                            finalstrProvider += ProviderColumn + "<br/>";
                        }
                        foreach (string PatientColumn in arrPatient)
                        {
                            finalstrPatient += PatientColumn + "<br/>";
                        }
                        //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                        patientTable = "<table width='100%' style='padding-left:0px;'>" +
                                      "<tbody>" +
                                      " <tr>" +
                                   "<td width='68%' style='line-height:1.5;padding-left:0px;'>" + finalstrPatient +
                         "</td><td width='32%' style='text-align:right;'>" + finalstrProvider +
                           "<br/><button class='btn btn-default btn-sm mr-xs' type='button' id='btnPrinter' onclick='Clinical_InfoButtonView.printInfo();'><i class='fa fa-print'></i> Print</button>" +
                        "<button class='btn btn-default btn-sm mr-xs' type='button' id='btnDownload' onclick='Clinical_InfoButtonView.DownloadInfo();'><i class='fa fa-download'></i> Download</button>" +
                        "</td></tr></tbody></table>";
                        //END Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                        //footerNode.InnerHtml = "Generated by: " + MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.FooterTextColumn.ColumnName]) + "<span style='float:right;'>Page 1 of 1</span></div><div class='clearfix'></div>";
                        footerNode.InnerHtml = "Generated by: " + MDVUtility.ToStr(dr[dsreportHeader.ReportHeaderTags.FooterTextColumn.ColumnName]) + "</div><div class='clearfix'></div>";
                    }
                    else
                    {
                        HtmlNode tblBody = document.CreateElement("tbody");
                        HtmlNode tbltr = document.CreateElement("tr");
                        HtmlNode tbltd = document.CreateElement("td");
                        tbltd.SetAttributeValue("style", "width:70%;padding-left:25px;");
                        HtmlNode logoImg = document.CreateElement("img");
                        logoImg.SetAttributeValue("src", VirtualPathUtility.ToAbsolute(@"~\content\images\SHS-nav-logo.png"));
                        logoImg.SetAttributeValue("height", "65px");
                        tbltd.AppendChild(logoImg);

                        HtmlNode tbltdH = document.CreateElement("td");
                        //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                        tbltdH.SetAttributeValue("style", "width:30%; text-align:right;");
                        //End Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                        HtmlNode h3Heading = document.CreateElement("h4");
                        h3Heading.Attributes.Add("class", "text-bold");
                        h3Heading.InnerHtml = "Educational Material";
                        tbltdH.AppendChild(h3Heading);
                        tbltr.AppendChild(tbltd);
                        tbltr.AppendChild(tbltdH);
                        tblBody.AppendChild(tbltr);
                        tblHeading.AppendChild(tblBody);
                        //footerNode.InnerHtml = "Generated by: MDVISION PM EMR <span style='float:right;'>Page 1 of 1</span></div><div class='clearfix'></div>";
                        footerNode.InnerHtml = "Generated by: MDVISION PM EMR</div><div class='clearfix'></div>";
                    }
                }
                else
                {
                    HtmlNode tblBody = document.CreateElement("tbody");
                    HtmlNode tbltr = document.CreateElement("tr");
                    HtmlNode tbltd = document.CreateElement("td");
                    tbltd.SetAttributeValue("style", "width:70%;padding-left:25px;");
                    HtmlNode logoImg = document.CreateElement("img");
                    logoImg.SetAttributeValue("src", VirtualPathUtility.ToAbsolute(@"~\content\images\SHS-nav-logo.png"));
                    logoImg.SetAttributeValue("height", "65px");
                    tbltd.AppendChild(logoImg);

                    HtmlNode tbltdH = document.CreateElement("td");
                    //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                    tbltdH.SetAttributeValue("style", "width:30%; text-align:right;");
                    //End Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                    HtmlNode h3Heading = document.CreateElement("h4");
                    h3Heading.Attributes.Add("class", "text-bold");
                    h3Heading.InnerHtml = "Educational Material";
                    tbltdH.AppendChild(h3Heading);
                    tbltr.AppendChild(tbltd);
                    tbltr.AppendChild(tbltdH);
                    tblBody.AppendChild(tbltr);
                    tblHeading.AppendChild(tblBody);

                    footerNode.InnerHtml = "Generated by: MDVISION PM EMR <span style='float:right;'></span></div><div class='clearfix'></div>";

                }


                if (bodyNode != null)
                {
                    headerDiv.AppendChild(contentDiv);
                    contentDiv.AppendChild(serviceDiv);
                    serviceDiv.AppendChild(resultsDiv);
                    resultsDiv.AppendChild(resultDiv);
                    resultDiv.AppendChild(tblHeading);
                    resultDiv.AppendChild(divStyle);
                    bodyNode.InsertBefore(headerDiv, contentNode);
                }

                //Remove Scripts from html File
                document.DocumentNode.Descendants()
                                .Where(n => n.Name == "script" || n.Name == "noscript" || n.Name == "meta")
                                .ToList()
                                .ForEach(n => n.Remove());

                //Remove Navigation tags
                document.DocumentNode.Descendants()
                                .Where(d => d.Attributes.Contains("class") && (d.Attributes["class"].Value.Contains("hide-offscreen") || d.Attributes["class"].Value.Contains("return-top") || d.Attributes["class"].Value.Contains("subhead") || d.Attributes["class"].Value.Contains("mplushead") || d.Attributes["class"].Value.Contains("notes")))
                                .ToList()
                                .ForEach(n => n.Remove());

                //Add target=_blank to <a> tags so that the links would open in New Tab
                var anchorNodes = document.DocumentNode.Descendants()
                    .Where(d => d.Name.Equals("a") && (d.Attributes["target"] == null)).ToList();
                foreach (var anode in anchorNodes)
                {
                    anode.Attributes.Add("target", "_blank");
                }

                //Add styles and images
                Dictionary<int, string> cssPath = new Dictionary<int, string>();
                //cssPath.Add(1, "https://apps.nlm.nih.gov/medlineplus/services/css/connect.css");
                //cssPath.Add(2, "~/Content/Default/bootstrap.css");
                //cssPath.Add(3, "~/Content/Blue/default.css");
                //cssPath.Add(4, "~/Content/Blue/theme.css");
                //cssPath.Add(5, "~/Content/Blue/theme-custom.css");

                HtmlNode head = document.DocumentNode.SelectSingleNode("/html/head");
                var newLineNode = HtmlNode.CreateNode("\r\n");
                head.AppendChild(newLineNode);
                foreach (var css in cssPath)
                {
                    HtmlNode link = document.CreateElement("link");
                    head.AppendChild(link);
                    link.SetAttributeValue("rel", "stylesheet");
                    link.SetAttributeValue("href", css.Key > 1 ? VirtualPathUtility.ToAbsolute(css.Value) : css.Value);
                    head.AppendChild(newLineNode);
                }

                var styleNode = document.DocumentNode.Descendants().FirstOrDefault(d => d.Name.Equals("link") && (d.Attributes["rel"].Value.Equals("stylesheet")));
                if (styleNode != null)
                {
                    styleNode.Remove();
                }

                var node = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("mplus-nav-info"));

                if (node != null)
                {
                    if (node.InnerText.Trim().Equals("0 results found."))
                        noinfoFound = "No Information Found";
                }

                document.DocumentNode.Descendants()
                           .Where(n => n.Name == "div" && n.Attributes.Contains("id") && (n.Attributes["id"].Value.Contains("mplus-orgs") || n.Attributes["id"].Value.Contains("mplus-logo") || n.Attributes["id"].Value.Contains("mplus-nav") || n.Attributes["id"].Value.Contains("mplus-nav") || n.Attributes["id"].Value.Contains("mplus-footer")))
                           .ToList()
                           .ForEach(n => n.Remove());
                caller = "Illness: ";
                if(parentCtrl=="Medication")
                    caller = "Drug: ";

                HtmlNode resultNode = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("result-main"));

                //Check if Record found then get FileName else show No Information Found
                var fileName = string.Empty;
                if (String.IsNullOrWhiteSpace(noinfoFound))
                {
                    //Get Name for Problem List
                    node = document.DocumentNode.Descendants().FirstOrDefault(n => n.Id.Equals("problems_topichead"));
                    if (node != null)
                    {
                        fileName = node.InnerText.Trim();

                        HtmlNode summaryNode = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("problems_summaryTopic"));

                        HtmlNode summaryImageNode = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("problems_imagetopic"));

                        HtmlNode summaryImage = summaryImageNode.Descendants().FirstOrDefault(d => d.Name.Equals("a"));

                        string contentTable = "<table width='100%' style='border:1px solid #d8eaf6;  margin-top:10px'><thead><tr style='background:#d8eaf6;'><th style='padding-left:15px; font-weight:normal;'>" +
                                                             "<b>" + caller + "</b>" + fileName + "</th></tr></thead><tbody><tr><td style='padding:15px;'><p>" + summaryNode.InnerHtml + "</p><p>" + summaryImage.InnerHtml + "</p></td></tr></tbody></table>";


                        HtmlNode withAlsoNode = resultNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("with-also"));
                        if (withAlsoNode != null)
                            withAlsoNode.Remove();

                        HtmlNode alsoCalledNode = resultNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("alsoCalled"));
                        if (alsoCalledNode != null)
                            alsoCalledNode.Remove();

                        HtmlNode image = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("result-side"));
                        if (image != null)
                            image.Remove();

                        summaryNode.Remove();
                        HtmlNode content = HtmlNode.CreateNode(contentTable);

                        resultNode.PrependChild(content);

                    }
                    else
                    {
                        //Get Name for Medication & Lab Results
                        node =
                            document.DocumentNode.Descendants()
                                .FirstOrDefault(
                                    n => n.Name.Equals("a") && n.Attributes.Any(a => a.Value.Contains("druginfo") || a.Value.Contains("health") || a.Value.Contains("factsheets") || a.Value.Contains("ency")));
                        fileName = node != null ? node.InnerText.Trim() : Guid.NewGuid().ToString();
                        if (node != null)
                        {
                            fileName = node.InnerText.Trim();


                            HtmlNode resultsNode = document.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("results"));
                            if (resultNode != null)
                            {
                                resultNode.SetAttributeValue("class", "noBorder p-none");
                            }

                            node.ParentNode.InnerHtml = "<table width='100%' style='border:1px solid #d8eaf6;'><thead><tr style='background:#d8eaf6;'><th style='padding-left:15px; font-weight:normal;'>" +
                            "<b>" + caller + "</b>" + fileName + "</th></tr></thead><tbody><tr><td style='padding:15px;'>" + node.ParentNode.InnerHtml + "</td></tr></tbody></table>";
                        }
                    }
                }

                HtmlNode footerNodeBar = HtmlNode.CreateNode(footerDivBar);
                bodyNode.AppendChild(footerNodeBar);
                bodyNode.AppendChild(footerNode);

                foreach (HtmlNode item in document.DocumentNode.Descendants()
                             .Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("mplus-wrap")))
                {
                    item.SetAttributeValue("class", "noBoxshadow ");

                }

                foreach (HtmlNode item in document.DocumentNode.Descendants()
                             .Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("mplus-content")))
                {
                    item.SetAttributeValue("class", "noBorder p-none");

                }

                HtmlNode printer = document.DocumentNode.Descendants().FirstOrDefault(n => n.Name.Equals("button") && (n.Id.Equals("btnPrinter")));
                if (printer != null)
                {
                    printer.Attributes.Remove("background");
                }
                HtmlNode download = document.DocumentNode.Descendants().FirstOrDefault(n => n.Name.Equals("button") && (n.Id.Equals("btnDownload")));
                if (download != null)
                {
                    download.Attributes.Remove("background");
                }
                node = document.DocumentNode.Descendants().FirstOrDefault(n => n.Name.Equals("a") && (n.Id.Equals("mplus-lang-toggle") || n.Id.Equals("toggleMenu")));
                if (node != null)
                {
                    //Update Language to English
                    node.InnerHtml = "English";

                    var href = node.Attributes["href"].Value.Replace("sp", "en");
                    href = "https://apps.nlm.nih.gov" + href;
                    node.Attributes["href"].Value = href;
                }

                //Convert images to Base64 to be shown in pdf file
                foreach (var element in document.DocumentNode.SelectNodes("//img[@src]"))
                {
                    HtmlAttribute attr = element.Attributes["src"];
                    attr.Value = B64Encode(attr.Value);
                }

                document.OptionWriteEmptyNodes = true;
                htmlContent = document.DocumentNode.OuterHtml;

                byte[] array = Encoding.ASCII.GetBytes(htmlContent);

                return new BLObject<byte[]>(array, fileName != string.Empty ? fileName : noinfoFound);
            }
            catch (Exception ex)
            {
                return new BLObject<byte[]>(null, ex.Message);
            }
        }
        private string B64Encode(string path)
        {
            byte[] imageBytes = null;
            if (path.IndexOf("http", StringComparison.Ordinal) > -1)
            {
                Uri uri = new Uri(path);
                WebClient client = new WebClient();
                try
                {
                    imageBytes = client.DownloadData(uri);
                }
                catch (Exception)
                {
                    //ignore
                }
                finally
                {
                    client.Dispose();
                }
            }
            else
            {
                try
                {
                    using (Image image = Image.FromFile(HttpContext.Current.Server.MapPath(path)))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            imageBytes = m.ToArray();
                        }
                    }
                }
                catch (Exception)
                {
                    //ignore
                }

            }


            if (imageBytes != null)
            {
                string base64String = Convert.ToBase64String(imageBytes);
                return "data:image/png;base64," + base64String;
            }

            return path;
        }
        public static string RemoveComments(string input)
        {
            string tagPattern = @"<!--(.|[\r\n])*?-->";

            MatchCollection matches = Regex.Matches(input, tagPattern);
            foreach (Match match in matches)
            {
                input = input.Replace(match.Value, string.Empty);
            }

            return input;
        }

        public string ProcedureOrder(JObject AllData)
        {
            string response = null;

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

            OS_ProcedureOrderHelper helperProcedureOrder = new OS_ProcedureOrderHelper();

            if (model.commandType.ToUpper() == "SAVE_PROCEDURE_ORDER")
            {
                Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
                string ruleTypeIds = arrJSON.ContainsKey("RuleTypes") == true ? MDVUtility.ToStr(arrJSON["RuleTypes"]) : "";
                //Start 22-03-2016 Humaira Yousaf for status            
                model.Status = arrJSON.ContainsKey("Status") == true ? MDVUtility.ToStr(arrJSON["Status"]) : "";
                //End 22-03-2016 Humaira Yousaf for status            
                response = helperProcedureOrder.saveProcedureOrder(model, lstProcedureTestModel);

            }
            //else if (model.commandType.ToLower() == "getorderingprovider")
            //{
            //    response = helperProcedureOrder.loadOrderingProvider(model);
            //}
            ////Start 15-03-2016//Ahmad Raza// calling helper method to update RadiologyOrder Alert's Status
            //else if (model.commandType.ToLower() == "update_RadiologyOrder_status")
            //{
            //    response = "";// helperRadiologyOrder.updateRadiologyOrdertatus(model);
            //}
            ////End 15-03-2016//Ahmad Raza// calling helper method to update RadiologyOrder Alert's Status

            ////Start 17-03-2016 Humaira Yousaf to load Radiology Orders
            else if (model.commandType.ToLower() == "search_procedureorders")
            {
                response = helperProcedureOrder.loadProcedureOrder(model);
            }
            ////End 17-03-2016 Humaira Yousaf to load Radiology Orders

            ////Start 04-03-2016 Humaira Yousaf to load RadiologyOrder
            else if (model.commandType.ToLower() == "fill_procedureorder")
            {
                response = helperProcedureOrder.loadProcedureOrder(model);
            }
            ////End 04-03-2016 Humaira Yousaf to load RadiologyOrder  

            else if (model.commandType.ToLower() == "delete_procedureorder")
            {
                response = helperProcedureOrder.deleteProcedureOrder(model.ProcedureOrderId);
            }
            ////Start 08-03-2016//Ahmad Raza// calling helper method to load RadiologyOrder Alert
            //else if (model.commandType.ToLower() == "show_radiologyorder_alert")
            //{
            //    response = "";// helperRadiologyOrder.loadRadiologyOrderAlert(model);
            //}
            //else if (model.commandType.ToLower() == "getlatest_procedureorderby_patientid")
            //{
            //    response = helperProcedureOrder.loadProcedureOrder(model);
            //}
            //else if (model.commandType.ToLower() == "get_orders_forsoap")
            //{
            //    response = helperProcedureOrder.getOrdersForSoap(model.ProcedureOrderIDs, MDVUtility.ToLong(model.PatientId));
            //}
            //else if (model.commandType.ToLower() == "attach_procedureorder_with_notes")
            //{
            //    response = helperProcedureOrder.attachProcedureOrderWithNotes(model.ProcedureOrderId, Convert.ToInt64(model.NoteId));
            //}
            //else if (model.commandType.ToLower() == "attach_orders_with_notes")
            //{
            //    response = helperProcedureOrder.attachProcedureOrderWithNotes(model.ProcedureOrderIDs, Convert.ToInt64(model.NoteId));
            //}
            //else if (model.commandType.ToLower() == "detach_procedureorder_from_notes")
            //{
            //    response = helperProcedureOrder.detachProcedureOrderFromNotes(model.ProcedureOrderId, Convert.ToInt64(model.NoteId));
            //}
            //else if (model.commandType.ToLower() == "detach_orders_from_notes")
            //{
            //    response = helperProcedureOrder.detachProcedureOrderFromNotes(model.ProcedureOrderIDs, Convert.ToInt64(model.NoteId));
            //}

            ////End 08-03-2016//Ahmad Raza// calling helper method to load RadiologyOrder Alert

            ////Start 22-03-2016 Humaira Yousaf to view PDF          
            //else if (model.commandType.ToLower() == "preview_procedureorder")
            //{
            //    response = helperProcedureOrder.previewProcedureOrder(model);
            //}
            ////End 22-03-2016 Humaira Yousaf to view PDF

            else if (modelProcedureOrderTest.commandType.ToLower() == "delete_procedureorder_test")
            {
                response = helperProcedureOrder.deleteProcedureOrderTest(modelProcedureOrderTest.ProcedureOrderTestId);
            }

            return response;
        }
    }
}