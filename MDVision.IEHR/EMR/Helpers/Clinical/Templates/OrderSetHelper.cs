
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using System.Text;
using MDVision.IEHR.EMR.Model.ReviewofSystems;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Model;
using MDVision.Model.Lookups;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.Patient;
using MDVision.Model.Clinical.FollowUp;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.IEHR.Controls.Clinical;
using Newtonsoft.Json.Linq;
using MDVision.Model.Clinical.Templates.OrderSets;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.Model.Clinical.Orderset;
using System.IO;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using MDVision.Model.Clinical.Medical.ProblemLists;

namespace MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems
{
    public class OrderSetHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private BLLOrderSet BLLOrderSetObj = null;
        public OrderSetHelper()
        {
            BLLOrderSetObj = new BLLOrderSet();
            BLLClinicalObj = new BLLClinical();
        }
        private static OrderSetHelper _instance = null;
        public static OrderSetHelper Instance()
        {
            if (_instance == null)
                _instance = new OrderSetHelper();
            return _instance;
        }

        //Author: Azeem Raza Tayyab
        //Purpose: Insert Order Set
        //Date : 10-Jan- 2017
        public string insertOrderSet(OrderSetModel model)
        {
            OrderSetResponse responseModel = new OrderSetResponse();
            try
            {
                foreach (OrderSetProblemModel m in model.AssociatedProblemData)
                {
                    m.IsActive = "1";
                    m.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    m.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                    m.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    m.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                    m.OrderSetProblemQuery = m.ProblemOperator != "" ? m.ProblemOperator + " SNOMEDID='" + m.SnomedCode + "'" : " SNOMEDID='" + m.SnomedCode + "'";
                }
                //string ProblemXMLString = string.Empty;
                //using (TextWriter writer = new StringWriter())
                //{
                //    GetOrderSetProblemListModelXMLTable(model.AssociatedProblemData).WriteXml(writer);
                //    ProblemXMLString = writer.ToString();
                //}


                model.OrderSetProblemXML = MDVUtility.GetXmlOfObject(typeof(List<OrderSetProblemModel>), model.AssociatedProblemData);

                BLObject<string> obj = BLLOrderSetObj.insertOrderSet(model);


                if (obj.Data != null && obj.Data != "-1")
                {
                    responseModel.OrderSetId = obj.Data.ToString();
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Save_Message;
                }
                else
                {
                    responseModel.status = false;
                    responseModel.OrderSetId = model.OrderSetId;
                    responseModel.Message = obj.Message == "" ? "Order Set already exist" : obj.Message;
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.OrderSetId = model.OrderSetId;

            }
            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    OrderSetId = responseModel.OrderSetId,
                    Message = Common.AppPrivileges.Save_Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = responseModel.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        DataTable GetOrderSetProblemListModelXMLTable(List<OrderSetProblemModel> ProblemList)
        {

            DataTable OrderSetProblemListTable = new DataTable() { TableName = "OrderSetProblemList" };


            OrderSetProblemListTable.Columns.Add("OrderSetProblemId", typeof(Int64));
            OrderSetProblemListTable.Columns.Add("OrderSetId", typeof(Int64));
            OrderSetProblemListTable.Columns.Add("Problem", typeof(String));
            OrderSetProblemListTable.Columns.Add("Comments", typeof(String));
            OrderSetProblemListTable.Columns.Add("ModifiedOn", typeof(DateTime));
            OrderSetProblemListTable.Columns.Add("IsActive", typeof(byte));
            OrderSetProblemListTable.Columns.Add("CreatedBy", typeof(String));
            OrderSetProblemListTable.Columns.Add("CreatedOn", typeof(DateTime));
            OrderSetProblemListTable.Columns.Add("ModifiedBy", typeof(String));
            OrderSetProblemListTable.Columns.Add("OrderSetProblemQuery", typeof(String));
            OrderSetProblemListTable.Columns.Add("ProblemOperator", typeof(String));
            OrderSetProblemListTable.Columns.Add("SnomedCode", typeof(String));
            OrderSetProblemListTable.Columns.Add("ICD9", typeof(String));
            OrderSetProblemListTable.Columns.Add("ICD10", typeof(String));
            OrderSetProblemListTable.Columns.Add("SnomedId", typeof(String));
            OrderSetProblemListTable.Columns.Add("SnomedDescription", typeof(String));
            OrderSetProblemListTable.Columns.Add("Icd9Description", typeof(String));
            OrderSetProblemListTable.Columns.Add("Icd10Description", typeof(String));
            for (int i = 0; i < ProblemList.Count; i++)
            {
                OrderSetProblemListTable.Rows.Add
                    (
                         ProblemList[i].OrderSetProblemId,
                         ProblemList[i].OrderSetId,
                         ProblemList[i].Problem,
                         ProblemList[i].Comments,
                         ProblemList[i].ModifiedOn,
                         ProblemList[i].IsActive,
                         ProblemList[i].CreatedBy,
                         ProblemList[i].CreatedOn,
                         ProblemList[i].ModifiedBy,
                         ProblemList[i].OrderSetProblemQuery,
                         ProblemList[i].ProblemOperator,
                         ProblemList[i].SnomedCode,
                         ProblemList[i].ICD9,
                         ProblemList[i].ICD10,
                         ProblemList[i].SnomedId,
                         ProblemList[i].SnomedDescription,
                         ProblemList[i].Icd9Description,
                         ProblemList[i].Icd10Description
                    );
            }

            return OrderSetProblemListTable;
        }
        internal string detach_OrderSet_From_Note(OrderSetModel model)
        {
            try
            {
                DSOrderSet ds = null;
                BLObject<DSOrderSet> obj = BLLOrderSetObj.detachOrderSetFromNotes(MDVUtility.ToInt64(model.PatientId), model.OrderSetId, MDVUtility.ToInt64(model.NotesId));
                ds = obj.Data;
                if (ds != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    bool MedicationDeleted = false;
                    string DelteMedicationIds = "";
                    foreach (DSOrderSet.DeletedListRow dr in ds.DeletedList)
                    {
                        if (MDVUtility.ToStr(dr.Component) == "NoteMedication")
                        {
                            if (MDVUtility.ToStr(dr.DeletedIds) != "")
                            {
                                MedicationDeleted = true;
                                DelteMedicationIds = MDVUtility.ToStr(dr.DeletedIds);
                                break;
                            }
                        }
                    }
                    if (MedicationDeleted && DelteMedicationIds != "")
                    {
                        RcopiaHelper rcopiaHelper = new RcopiaHelper();
                        dynamic ResponseOfUploadMedicationOnDrFirst = JObject.Parse(rcopiaHelper.UploadMedicationOnDrFirst(MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), MDVUtility.ToInt64(model.PatientId), model.OrderSetId, MDVUtility.ToInt64(model.NotesId), "", DelteMedicationIds));
                        if (ResponseOfUploadMedicationOnDrFirst.status == "true")
                        {
                            if (ResponseOfUploadMedicationOnDrFirst.SavedMedicationIds != "")
                            {
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = "Problem In Medication",
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }

                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Problem In Medication",
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    var response1 = new
                    {
                        status = true,
                        DeletedIDsFill_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.DeletedList.TableName]),
                        //  ProcedureListFill_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.ProcedureSoap.TableName])
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        DeletedIDsFill_JSON = "[]",
                        //  ProcedureListFill_JSON = "[]"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        //Author: Azeem Raza Tayyab
        //Purpose: Load Order Set
        //Date : 10-Jan- 2017
        public string loadOrderSet(OrderSetModel model)
        {
            try
            {
                List<OrderSetModel> listOrderSet = new List<OrderSetModel>();
                BLObject<List<OrderSetModel>> obj = BLLOrderSetObj.loadOrderSet(model.OrderSetName, Convert.ToInt16(model.IsActive), model.ProviderIds, model.SpecialtyIds, model.PageNumber, model.RowsPerPage);

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listOrderSet = obj.Data,
                        OrderSetCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listOrderSet = "[]",
                        OrderSetCount = 0,
                        iTotalDisplayRecords = 0
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        //Author: Azeem Raza Tayyab
        //Purpose: Fill Order Set
        //Date : 10-Jan- 2017
        public string fillOrderSet(OrderSetModel model)
        {
            try
            {
                List<OrderSetModel> listOrderSet = new List<OrderSetModel>();
                BLObject<List<OrderSetModel>> obj = BLLOrderSetObj.fillOrderSet(model.OrderSetId, MDVUtility.ToStr(model.NotesId), MDVUtility.ToStr(model.CDSId));
                BLObject<List<OrderSetProblemModel>> obj1 = BLLOrderSetObj.LoadOrderSetProblem(model.OrderSetId);
                if (obj.Data != null && obj1.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listOrderSet = obj.Data,
                        OrderSetCount = obj.Data.Count,
                        listProblem=obj1.Data,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        listOrderSet = "[]",
                        OrderSetCount = 0,
                        listProblem ="[]",
                        iTotalDisplayRecords = 0
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string LoadPatientAndOrdProblems(OrderSetModel model)
        {
            try
            {
                BLObject<List<OrderSetProblemModel>> obj = BLLOrderSetObj.LoadPatientAndOrdProblems(model.OrderSetId, model.PatientId);
                if (obj.Data != null)
                {

                    var response = new
                    {
                        status = true,
                        listProblem = obj.Data,
                        ProblemCount = obj.Data.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        listProblem ="[]",
                        ProblemCount = 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        
        //Author: Azeem Raza Tayyab
        //Purpose: Delete Order Set
        //Date : 10-Jan- 2017
        public string deleteOrderSet(string orderSetId)
        {
            try
            {
                if (string.IsNullOrEmpty(orderSetId))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLOrderSetObj.deleteOrderSet(orderSetId);
                    if (obj.Data != null && obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string SaveProblemOnDrFirst(ProblemListModel model, DataRow dr)
        {
            try
            {
                DSProblemLists dsProblemList = new DSProblemLists();

                string PatientID = (model.PatientId);
                string problemID = MDVUtility.ToStr(model.ProblemListId);
                BLLRcopia BLLRcopiaObj = new BLLRcopia();
                dynamic ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("Problem", dr, problemID));

                if (ResponseOfDrFirst.Rcopia != "")
                {
                    DSProblemLists DatasetProblem = new DSProblemLists();
                    DSProblemLists.ProblemListRow ProblemRow = DatasetProblem.ProblemList.NewProblemListRow();
                    ProblemRow.ProblemListId = MDVUtility.ToInt32(problemID);
                    ProblemRow.PatientId = MDVUtility.ToInt64(PatientID);
                    ProblemRow.Comments = MDVUtility.ToStr(model.Comments);
                    ProblemRow.IsActive = true;
                    ProblemRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    ProblemRow.CreatedOn = DateTime.Now;
                    ProblemRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    ProblemRow.ModifiedOn = DateTime.Now;
                    ProblemRow.RcopiaID = ResponseOfDrFirst.Rcopia;
                    DatasetProblem.ProblemList.AddProblemListRow(ProblemRow);
                    BLObject<DSProblemLists> obj1 = BLLClinicalObj.InsertProblemRcopialID(DatasetProblem);
                    if (obj1.Data != null)
                    {
                        var responseRcopiaerror = new
                        {
                            status = true,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));

                    }
                    else
                    {
                        var responseRcopiaerror = new
                        {
                            status = false,
                            Message = obj1.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                    }
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = false,
                        Message = "Problem in Add Problem on DrFirst"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        internal string attachNoteOrderSetsWithNote(long notesId, long patientId, string orderSetIds, string OrderSetComponents)
        {
            try
            {
                List<long> orderSet_list = orderSetIds.Split(',').ToList().Select(s => long.Parse(s)).ToList();
                Dictionary<long, bool> Status_ = new Dictionary<long, bool>();
                List<string> response_ = new List<string>();
                var ExistsMedicationsDrugName = "";
                foreach (var ordersetId in orderSet_list)
                {
                    Status_.Add(ordersetId, false);
                    BLObject<DSOrderSet> obj = BLLOrderSetObj.attachOrderSetWithNote(notesId, patientId, ordersetId.ToString(), OrderSetComponents);
                    if (obj.Data != null)
                    {
                        DSOrderSet ds = obj.Data;
                        if (ds.Tables[ds.OS_ProblemListSoap.TableName].Rows.Count > 0)
                        {
                            foreach (DSOrderSet.OS_ProblemListSoapRow datarow in ds.Tables[ds.OS_ProblemListSoap.TableName].Rows)
                            {
                                ProblemListModel model = new ProblemListModel();
                                model.ProblemListId = datarow.ProblemListId.ToString();
                                model.PatientId = patientId.ToString();
                                if (!(datarow.IsCommentsNull()))
                                {
                                    model.Comments = datarow.Comments.ToString();
                                }
                                SaveProblemOnDrFirst(model, datarow);
                            }
                        }

                        #region Medication Upload
                        var MedicationIDs = "";
                        BLObject<List<OS_MedicationModel>> obj1 = null;

                        obj1 = new BLLOrderSet().LoadMedication(MDVUtility.ToStr(ordersetId), "", 1, 1000);
                        List<OS_MedicationModel> model1 = new List<OS_MedicationModel>();
                        if (obj1.Data != null)
                        {
                            model1 = obj1.Data;
                            if (model1.Count > 0)
                            {
                                MedicationIDs = string.Join(",", model1.Select(p => p.OS_MedicationId));
                            }
                        }



                        if (!string.IsNullOrEmpty(MedicationIDs))
                        {
                            List<OS_MedicationModel> MedicationList = null;
                            BLObject<List<OS_MedicationModel>> ExistsOrNotExistsMedicationobj;
                            ExistsOrNotExistsMedicationobj = BLLOrderSetObj.ExistsOrNotExistsMedication(MedicationIDs, patientId);
                            MedicationList = ExistsOrNotExistsMedicationobj.Data;

                            if (obj.Data != null)
                            {
                                if (MedicationList.Count > 0)
                                {
                                    var NotExistsMedicationIds = "";
                                    var ExistsMedicationIds = "";

                                    ExistsMedicationIds = string.Join(",", MedicationList.Where(p => p.alreadyExists == true).Select(p => p.OS_MedicationId));
                                    NotExistsMedicationIds = string.Join(",", MedicationList.Where(p => p.alreadyExists == false).Select(p => p.OS_MedicationId));
                                    var existsMedicsDrugName = string.Join(", ", MedicationList.Where(p => p.alreadyExists == true).Select(p => p.BrandName));
                                    if (existsMedicsDrugName != "")
                                    {
                                        ExistsMedicationsDrugName = ExistsMedicationsDrugName + "," + existsMedicsDrugName;
                                    }
                                    if (!string.IsNullOrEmpty(NotExistsMedicationIds))
                                    {
                                        RcopiaHelper rcopiaHelper = new RcopiaHelper();
                                        dynamic ResponseOfUploadMedicationOnDrFirst = JObject.Parse(rcopiaHelper.UploadMedicationOnDrFirst(MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), patientId, MDVUtility.ToStr(ordersetId), notesId, NotExistsMedicationIds));
                                        if (ResponseOfUploadMedicationOnDrFirst.status == "true")
                                        {
                                            if (ResponseOfUploadMedicationOnDrFirst.SavedMedicationIds != "")
                                            {
                                                DSClinicalMedication dsMedicationSoap = null;
                                                BLObject<DSClinicalMedication> objMed = BLLClinicalObj.loadMedicationsForSoap(MDVUtility.ToStr(ResponseOfUploadMedicationOnDrFirst.SavedMedicationIds), patientId);
                                                dsMedicationSoap = objMed.Data;
                                                if (objMed.Data != null)
                                                {
                                                    if (dsMedicationSoap.Tables[dsMedicationSoap.Medication.TableName].Rows.Count > 0)
                                                    {
                                                        ds.Merge(dsMedicationSoap);
                                                    }
                                                }
                                                else
                                                {
                                                    var response = new
                                                    {
                                                        status = false,
                                                        Message = "Problem In Medication",
                                                    };
                                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var response = new
                                            {
                                                status = false,
                                                Message = "Problem In Medication",
                                            };
                                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                        }
                                    }
                                }
                                else
                                {
                                    var response = new
                                    {
                                        status = false,
                                        Message = "Problem In Medication",
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = "Problem In Medication",
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }

                        }



                        #endregion


                        var response1 = new
                        {
                            status = true,
                            ProcedureSoapCount = ds.Tables[ds.ProcedureSoap.TableName].Rows.Count,
                            ProcedureSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.ProcedureSoap.TableName]),
                            ProblemListSoapCount = ds.Tables[ds.OS_ProblemListSoap.TableName].Rows.Count,
                            ProblemListSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.OS_ProblemListSoap.TableName]),
                            MedicationSoapCount = ds.Tables[ds.LabOrder.TableName].Rows.Count,
                            MedicationSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.LabOrder.TableName]),
                            radiologySoapCount = ds.Tables[ds.OS_RadiologyOrder.TableName].Rows.Count,
                            radiologySoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.OS_RadiologyOrder.TableName]),
                            PatientEducationSoapCount = ds.Tables[ds.PatientDocumentSoap.TableName].Rows.Count,
                            PatientEducationSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.PatientDocumentSoap.TableName]),
                            VaccineCount = ds.Tables[ds.OS_VaccineHx.TableName].Rows.Count,
                            Vaccines = MDVUtility.JSON_DataTable(ds.Tables[ds.OS_VaccineHx.TableName]),
                            ReferralListCount = ds.Tables[ds.Referrals.TableName].Rows.Count,
                            ReferralData_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.Referrals.TableName]),
                            TheraeuticInjectionCount = ds.Tables[ds.OS_TherapeuticInjection.TableName].Rows.Count,
                            FromOrderSet = true,
                            Injections = MDVUtility.JSON_DataTable(ds.Tables[ds.OS_TherapeuticInjection.TableName]),
                            FollowUp = MDVUtility.JSON_DataTable(ds.Tables[ds.FollowUp.TableName]),
                            Appointment = MDVUtility.JSON_DataTable(ds.Tables[ds.Appointment.TableName]),
                            MedicationsSoapCount = ds.Tables[ds.Medication.TableName].Rows.Count,
                            MedicationsSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.Medication.TableName]),
                            ProcedureOrderSoapCount = ds.Tables[ds.ProcedureOrder.TableName].Rows.Count,
                            ProcedureOrderSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.ProcedureOrder.TableName]),
                            ExistsMedicationsDrugName = ExistsMedicationsDrugName,
                            Message = Common.AppPrivileges.Save_Message,
                            OrdersetId = ordersetId
                        };

                        response_.Add(Newtonsoft.Json.JsonConvert.SerializeObject(response1));

                        Status_[ordersetId] = true;

                    }
                    else
                        Status_[ordersetId] = false;
                }


                List<OrderSetModel> listOrderSet = new List<OrderSetModel>();
                BLObject<List<OrderSetModel>> objOrderSet = BLLOrderSetObj.loadOrderSetName(orderSetIds);

                if (objOrderSet.Data != null && objOrderSet.Data.Count != 0)
                {
                    if (Status_.Count(p => p.Value == true) > 0)
                    {
                        string message_ = "";
                        if (Status_.Count(p => p.Value == true) == orderSet_list.Count)
                            message_ = "Order Set successfully attached with note.";
                        else
                            message_ = Status_.Count(p => p.Value == true) + " Order Set out of (" + orderSet_list.Count + ") successfully attached with note.";

                        var response = new
                        {
                            status = true,
                            Message = message_,
                            OrderSet_JSON = response_,
                            listOrderSet = objOrderSet.Data,
                            ExistsMedicationsDrugName = ExistsMedicationsDrugName
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Orderset with the same name already attached.",
                            OrderSet_JSON = response_
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "No Orderset Found.",
                        OrderSet_JSON = response_
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string attachOrderSetWithNote(long notesId, long patientId, string orderSetId, string OrderSetComponents, string ProblemIDs, string ProcedureIDs, string LabOrderIDs, string RadiologyOrderIDs, string FollowUpIDs, string PatientEducationIDs, string ReferralsIDs, string ImmunizationIDs, string TherapeuticIDs, string MedicationIDs, string ProcedureOrderIDs, long ProviderId, bool AddInValidAgeRecordsInHxTab = false,string PatientProblemIds="",string OrderSetAssociatedProblemIds="")
        {
            try
            {
                BLObject<DSOrderSet> obj = BLLOrderSetObj.attachOrderSetWithNote(notesId, patientId, orderSetId, OrderSetComponents, ProblemIDs, ProcedureIDs, LabOrderIDs, RadiologyOrderIDs, FollowUpIDs, PatientEducationIDs, ReferralsIDs, ImmunizationIDs, TherapeuticIDs, ProcedureOrderIDs, ProviderId, AddInValidAgeRecordsInHxTab,PatientProblemIds,OrderSetAssociatedProblemIds);
                if (obj.Data != null)
                {
                    DSOrderSet ds = obj.Data;
                    if (ds.Tables[ds.OS_ProblemListSoap.TableName].Rows.Count > 0)
                    {
                        foreach (DSOrderSet.OS_ProblemListSoapRow datarow in ds.Tables[ds.OS_ProblemListSoap.TableName].Rows)
                        {
                            ProblemListModel model = new ProblemListModel();
                            model.ProblemListId = datarow.ProblemListId.ToString();
                            model.PatientId = patientId.ToString();
                            if (!(datarow.IsCommentsNull()))
                            {
                                model.Comments = datarow.Comments.ToString();
                            }
                            SaveProblemOnDrFirst(model, datarow);
                        }
                    }
                    #region Medication Upload

                    if (OrderSetComponents == "")
                    {
                        BLObject<List<OS_MedicationModel>> obj1 = null;

                        obj1 = new BLLOrderSet().LoadMedication(MDVUtility.ToStr(orderSetId), "", 1, 1000);
                        List<OS_MedicationModel> model1 = new List<OS_MedicationModel>();
                        if (obj1.Data != null)
                        {
                            model1 = obj1.Data;
                            if (model1.Count > 0)
                            {
                                MedicationIDs = string.Join(",", model1.Select(p => p.OS_MedicationId));
                            }
                        }
                    }


                    var ExistsMedicationsDrugName = "";
                    if (!string.IsNullOrEmpty(MedicationIDs))
                    {
                        List<OS_MedicationModel> MedicationList = null;
                        BLObject<List<OS_MedicationModel>> ExistsOrNotExistsMedicationobj;
                        ExistsOrNotExistsMedicationobj = BLLOrderSetObj.ExistsOrNotExistsMedication(MedicationIDs, patientId);
                        MedicationList = ExistsOrNotExistsMedicationobj.Data;

                        if (obj.Data != null)
                        {
                            if (MedicationList.Count > 0)
                            {
                                var NotExistsMedicationIds = "";
                                var ExistsMedicationIds = "";

                                ExistsMedicationIds = string.Join(",", MedicationList.Where(p => p.alreadyExists == true).Select(p => p.OS_MedicationId));
                                NotExistsMedicationIds = string.Join(",", MedicationList.Where(p => p.alreadyExists == false).Select(p => p.OS_MedicationId));
                                ExistsMedicationsDrugName = string.Join(", ", MedicationList.Where(p => p.alreadyExists == true).Select(p => p.BrandName));
                                if (!string.IsNullOrEmpty(NotExistsMedicationIds))
                                {
                                    RcopiaHelper rcopiaHelper = new RcopiaHelper();
                                    dynamic ResponseOfUploadMedicationOnDrFirst = JObject.Parse(rcopiaHelper.UploadMedicationOnDrFirst(MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), patientId, orderSetId, notesId, NotExistsMedicationIds));
                                    if (ResponseOfUploadMedicationOnDrFirst.status == "true")
                                    {
                                        if (ResponseOfUploadMedicationOnDrFirst.SavedMedicationIds != "")
                                        {
                                            DSClinicalMedication dsMedicationSoap = null;
                                            BLObject<DSClinicalMedication> objMed = BLLClinicalObj.loadMedicationsForSoap(MDVUtility.ToStr(ResponseOfUploadMedicationOnDrFirst.SavedMedicationIds), patientId);
                                            dsMedicationSoap = objMed.Data;
                                            if (objMed.Data != null)
                                            {
                                                if (dsMedicationSoap.Tables[dsMedicationSoap.Medication.TableName].Rows.Count > 0)
                                                {
                                                    ds.Merge(dsMedicationSoap);
                                                }
                                            }
                                            else
                                            {
                                                var response = new
                                                {
                                                    status = false,
                                                    Message = "Problem In Medication",
                                                };
                                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var response = new
                                        {
                                            status = false,
                                            Message = "Problem In Medication",
                                        };
                                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                    }
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = "Problem In Medication",
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Problem In Medication",
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }



                    #endregion


                    var procedures = ds.Tables[ds.ProcedureSoap.TableName];
                    var proceduresCount = ds.Tables[ds.ProcedureSoap.TableName].Rows.Count;
                    //if (ds.Tables[ds.ProcedureSoap.TableName].Rows.Count < 1)
                    //{
                    //    procedures = ds.Tables[ds.OS_VaccineHx.TableName];
                    //    proceduresCount = ds.Tables[ds.OS_VaccineHx.TableName].Rows.Count;
                    //}
                    List<OrderSetModel> listOrderSet = new List<OrderSetModel>();
                    BLObject<List<OrderSetModel>> objOrderSet = BLLOrderSetObj.loadOrderSetName(orderSetId);

                    var response1 = new
                    {
                        status = true,
                        ProcedureSoapCount = proceduresCount,
                        ProcedureSoap_JSON = MDVUtility.JSON_DataTable(procedures),
                        ProblemListSoapCount = ds.Tables[ds.OS_ProblemListSoap.TableName].Rows.Count,
                        ProblemListSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.OS_ProblemListSoap.TableName]),
                        MedicationSoapCount = ds.Tables[ds.LabOrder.TableName].Rows.Count,
                        MedicationSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.LabOrder.TableName]),
                        LabOrderTest_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.LabOrderTest.TableName]),
                        LabOrderProblem_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.LabOrderProblem.TableName]),
                        radiologySoapCount = ds.Tables[ds.OS_RadiologyOrder.TableName].Rows.Count,
                        radiologySoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.OS_RadiologyOrder.TableName]),
                        radiologyOrderTest_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.RadiologyOrderTest.TableName]),
                        radiologyOrderProblem_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.RadiologyOrderProblem.TableName]),
                        PatientEducationSoapCount = ds.Tables[ds.PatientDocumentSoap.TableName].Rows.Count,
                        PatientEducationSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.PatientDocumentSoap.TableName]),
                        VaccineCount = ds.Tables[ds.OS_VaccineHx.TableName].Rows.Count,
                        Vaccines = MDVUtility.JSON_DataTable(ds.Tables[ds.OS_VaccineHx.TableName]),
                        ReferralListCount = ds.Tables[ds.Referrals.TableName].Rows.Count,
                        ReferralData_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.Referrals.TableName]),
                        TheraeuticInjectionCount = ds.Tables[ds.OS_TherapeuticInjection.TableName].Rows.Count,
                        FromOrderSet = true,
                        Injections = MDVUtility.JSON_DataTable(ds.Tables[ds.OS_TherapeuticInjection.TableName]),
                        FollowUp = MDVUtility.JSON_DataTable(ds.Tables[ds.FollowUp.TableName]),
                        Appointment = MDVUtility.JSON_DataTable(ds.Tables[ds.Appointment.TableName]),
                        MedicationsSoapCount = ds.Tables[ds.Medication.TableName].Rows.Count,
                        MedicationsSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.Medication.TableName]),
                        ProcedureOrderSoapCount = ds.Tables[ds.ProcedureOrder.TableName].Rows.Count,
                        ProcedureOrderSoap_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.ProcedureOrder.TableName]),
                        ExistsMedicationsDrugName = ExistsMedicationsDrugName,
                        Message = Common.AppPrivileges.Save_Message,
                        listOrderSet = objOrderSet.Data
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message == "-1" ? "Orderset with the same name Already Exists" : obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        //Author: Azeem Raza Tayyab
        //Purpose: Update Order Set
        //Date : 10-Jan- 2017
        public string updateOrderSet(OrderSetModel model)
        {
            try
            {
                foreach (OrderSetProblemModel m in model.AssociatedProblemData)
                {
                    m.IsActive = "1";
                    m.OrderSetId = model.OrderSetId;
                    m.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    m.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                    m.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    m.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                    m.OrderSetProblemQuery = m.ProblemOperator != "" ? m.ProblemOperator + " SNOMEDID='" + m.SnomedCode + "'" : " SNOMEDID='" + m.SnomedCode + "'";
                }
                //string ProblemXMLString = string.Empty;
                //using (TextWriter writer = new StringWriter())
                //{
                //    GetOrderSetProblemListModelXMLTable(model.AssociatedProblemData).WriteXml(writer);
                //    ProblemXMLString = writer.ToString();
                //}
                //model.OrderSetProblemXML = ProblemXMLString;

                model.OrderSetProblemXML = MDVUtility.GetXmlOfObject(typeof(List<OrderSetProblemModel>), model.AssociatedProblemData);

                
                BLObject<string> obj = BLLOrderSetObj.updateOrderSet(model);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message == "-1" ? "Order Set with the same name Already Exists" : obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        //Author: Azeem Raza Tayyab
        //Purpose: Update Order Set Active Inactive
        //Date : 10-Jan- 2017
        public string activeInactiveOrderSet(string orderSetId, string isActive)
        {
            try
            {
                BLObject<string> obj = BLLOrderSetObj.updateOrderSetStatus(orderSetId, isActive);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Inactive_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Data
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string saveAsOrderSet(OrderSetModel model)
        {
            try
            {
                BLObject<string> obj = BLLOrderSetObj.saveAsOrderSet(model.OrderSetId, model.OrderSetName, model.DefaultFollowUpId);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else if (obj.Data != null && obj.Data == "-1")
                {
                    var response = new
                    {
                        status = false,
                        Message = "Order Set with the same name Already Exists"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message == "-1" ? "Order Set with the same name Already Exists" : obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        #region " Note Order Set "

        public string insertNoteOrderSet(OS_NoteModel model)
        {
            OS_NoteResponse responseModel = new OS_NoteResponse();
            try
            {
                BLObject<string> obj = BLLOrderSetObj.insertNoteOrderSet(model);
                if (obj.Data != null && obj.Data != "-1")
                {
                    responseModel.NoteOSId = obj.Data.ToString();
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Save_Message;
                }
                else
                {
                    responseModel.status = false;
                    responseModel.NoteOSId = model.OrderSetId;
                    responseModel.Message = obj.Message == "" ? "Order Set already added to notes" : obj.Message;
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.NoteOSId = model.OrderSetId;

            }
            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    OrderSetId = responseModel.NoteOSId,
                    Message = Common.AppPrivileges.Save_Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = responseModel.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string loadNoteOrderSet(OS_NoteModel model)
        {
            try
            {
                List<OS_NoteModel> listOrderSet = new List<OS_NoteModel>();
                BLObject<List<OS_NoteModel>> obj = BLLOrderSetObj.loadNoteOrderSet(MDVUtility.ToLong(model.NoteOSId));

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listNoteOrderSet = obj.Data,
                        NoteOrderSetCount = obj.Data.Count
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listNoteOrderSet = "[]",
                        NoteOrderSetCount = 0
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string updateNoteOrderSet(OS_NoteModel model)
        {
            try
            {
                BLObject<string> obj = BLLOrderSetObj.updateNoteOrderSet(model);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message == "-1" ? "Order Set Already added to note." : obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string deleteNoteOrderSet(OS_NoteModel model)
        {
            try
            {
                BLObject<DSOrderSet> ds = BLLOrderSetObj.deleteNoteOrderSet(model.NoteOSId, MDVUtility.ToLong(model.NoteId), model.OrderSetId);
                if (ds.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        OrderSetIDS_JSON = MDVUtility.JSON_DataTable(ds.Data.Tables[ds.Data.OrderSetDelete.TableName]),
                        Message = Common.AppPrivileges.Delete_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        OrderSetIDS_JSON = "[]",
                        Message = ds.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion
        public string deleteOrderSetProblemList(string OrderSetProblemListId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(OrderSetProblemListId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLOrderSetObj.deleteOrderSetProblemList(MDVUtility.ToStr(OrderSetProblemListId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
    }
}