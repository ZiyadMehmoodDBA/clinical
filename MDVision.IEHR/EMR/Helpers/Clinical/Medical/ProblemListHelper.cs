using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.EMR.Model.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using MDVision.IEHR.Controls.Patient.Demographics;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using System.Threading;
using MDVision.IEHR.Common;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;
using MDVision.Model.Lookups;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.Medical.ProblemLists;
using MDVision.IEHR.EMR.Helpers.Clinical.Orders;
using MDVision.IEHR.EMR.Helpers.Clinical.FavoriteList;
using MDVision.Model.Clinical.Orderset;
using System.Net;
using System.Text;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;
using MDVision.Model.Clinical.Medical;
using System.Reflection;


//namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
namespace MDVision.IEHR.Controls.Clinical
{
    public class ProblemListHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private BLLRcopia BLLRcopiaObj = null;

        public ProblemListHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLRcopiaObj = new BLLRcopia();
        }
        private static ProblemListHelper _instance = null;
        private static bool isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
        public static ProblemListHelper Instance()
        {
            if (_instance == null)
            {
                _instance = new ProblemListHelper();
                //   isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
            }
            return _instance;
        }
        public string LoadProblemList(ProblemListModel model)
        {
            try
            {
                DSProblemLists dsProblemList = null;
                BLObject<DSProblemLists> obj;

                obj = BLLClinicalObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", "0");

                dsProblemList = obj.Data;
                if (obj.Data != null)
                {
                    int ProblemListTotalCount = 0;
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count == 0)
                    {
                        if (model.IsActive.Equals("1"))
                        {
                            obj = BLLClinicalObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), "0", "0");
                        }
                        else
                        {
                            obj = BLLClinicalObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), "0", "1");
                        }

                        if (obj.Data != null)
                        {
                            DSProblemLists dsProblemListInActive = obj.Data;
                            ProblemListTotalCount = dsProblemListInActive.Tables[dsProblemListInActive.ProblemList.TableName].Rows.Count;
                        }
                    }
                    else
                    {
                        ProblemListTotalCount = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count;
                    }
                    var response = new
                    {
                        status = true,
                        ProblemListTotalCount = ProblemListTotalCount,
                        ProblemListCount = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count,
                        ProblemListLoad_JSON = MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemList.TableName]),
                        ProblemListHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemHistory.TableName]),
                        iTotalDisplayRecords = (dsProblemList.ProblemList.Rows.Count > 0) ? dsProblemList.ProblemList.Rows[0][dsProblemList.ProblemList.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ProblemListCount = 0,
                        Message = obj.Message
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


        public string LoadLookups()
        {
            try
            {
                DSProblemLists dsProblemList = null;
                BLObject<DSProblemLists> obj;

                obj = BLLClinicalObj.LookupDiagnosisConfirmation();

                dsProblemList = obj.Data;
                if (obj.Data != null)
                {

                    var response = new
                    {
                        status = true,
                        LookupsCount = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count,
                        LookupsJSON = dsProblemList
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = false,
                        LookupsCount = 0,
                        Message = obj.Message
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

        public string LoadCancerCodes()
        {
            try
            {
                DSProblemLists dsProblemList = null;
                BLObject<DSProblemLists> obj;

                obj = BLLClinicalObj.LoadAllCancerCodes();

                dsProblemList = obj.Data;
                if (obj.Data != null)
                {

                    var response = new
                    {
                        status = true,
                        CodesCount = dsProblemList.Tables[dsProblemList.CancerCodes.TableName].Rows.Count,
                        CodesJSON = dsProblemList.CancerCodes
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = false,
                        LookupsCount = 0,
                        Message = obj.Message
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

        public string LoadCancerDiseaseDetails(ProblemListModel model)
        {
            try
            {
                

                 ProblemDetailXML ProblemDetail;

                ProblemDetail = BLLClinicalObj.LoadCancerDiseaseDetails(MDVUtility.ToInt64(model.ProblemListId));
                // Where(a => a.Name.Trim().ToLower().Contains(Name.Trim().ToLower())).ToList();

                if (ProblemDetail != null)
                {
                    var DiseaseHxkeyValues = new Dictionary<string, string> { { "", "" } };
                    DiseaseHxkeyValues = new Dictionary<string, string>
                        {
                            { "DiagnosisDate",  ProblemDetail.CancerDiagnosisDate},
                            { "Diagnosis",  ProblemDetail.DiseaseDiscription},
                            { "Laterality",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="Laterality").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "DiagnosisConfirmation",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="NAACCR Diagnostic Confirmation").Select(a=>a.TNMSystemCodeId).SingleOrDefault()  },
                            { "PrimarySiteId",ProblemDetail.PrimarySiteId},
                            { "HistologicTypeId", ProblemDetail.HistologicTypeId},
                            { "PrimarySite",ProblemDetail.PrimarySite},
                            { "HistologicType",ProblemDetail.HistologicType },
                            { "Behavior",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="NAACCR Behavior Code").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "Grade",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="NAACCR Grade").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "NKOClinical",ProblemDetail.NKOClinical },
                            { "ClinicalDiagnosisDate",ProblemDetail.CancerClinicalDiagnosisDate },
                            { "ClinicalStageGroup",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Clinical Stage Group").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "ClinicalStageDescriptor",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Clinical Stage Descriptor").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "PrimaryClinicalTumor",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Clinical Tumor").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "RLNC",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Clinical Node").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "DistanceMestastatases",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Clinical Metastasis").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "StagerClinicalCancer",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Clinical Stager Cancer").Select(a=>a.TNMSystemCodeId).SingleOrDefault()},
                            { "NKOPathologic", ProblemDetail.NKOPathologic },
                            { "EffectiveDate",ProblemDetail.CancerEffectiveDate },
                            { "PathologicStageGroup", ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Pathologic stage Group").Select(a=>a.TNMSystemCodeId).SingleOrDefault()},
                            { "PathologicStageDescriptor",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Pathologic Stage Descriptor").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "PrimaryTumorPathologic",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Pathologic Tumor").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "RLNP", ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Pathologic Node").Select(a=>a.TNMSystemCodeId).SingleOrDefault()},
                            { "DistanceMestastatasesPathologic",ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Pathologic Metastasis").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "StagerPathologicCancer", ProblemDetail.ProblemDetails.Where(a=>a.ValueSetName=="TNM Pathologic stager Cancer").Select(a=>a.TNMSystemCodeId).SingleOrDefault() },
                            { "IsActive",ProblemDetail.CancerIsActive }
                        };

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        Fill_JSON = js.Serialize(DiseaseHxkeyValues),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }

                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        Fill_JSON = "[]",
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
        public static string createProblemDetailXML(ProblemDetailXML Model)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProblemDetailXML));
                StringWriter textWriter = new StringWriter();

               
                xmlSerializer.Serialize(textWriter, Model);
                return textWriter.ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public string ReturnProblemDetailXML(ProblemListModel model)
        {
            List<ProblemDetailModel> lstProblemDetail = new List<ProblemDetailModel>();
            ProblemDetailXML objprob = new ProblemDetailXML();
            Type typModelCls = model.GetType(); //trans is the object name
            foreach (PropertyInfo prop in typModelCls.GetProperties())
            {
                if (prop.Name == "DiagnosisConfirmation" || prop.Name == "Laterality" || prop.Name == "Behavior" || prop.Name == "Grade" || prop.Name == "ClinicalStageGroup" || prop.Name == "ClinicalStageDescriptor" || prop.Name == "PrimaryClinicalTumor" || prop.Name == "RLNC"
                    || prop.Name == "DistanceMestastatases" || prop.Name == "StagerClinicalCancer" || prop.Name == "PathologicStageGroup" || prop.Name == "PathologicStageDescriptor" || prop.Name == "PrimaryTumorPathologic"
                    || prop.Name == "RLNP" || prop.Name == "DistanceMestastatasesPathologic" || prop.Name == "StagerPathologicCancer")
                {
                 string value=  (string)prop.GetValue(model, null);
                    ProblemDetailModel objDetail = new ProblemDetailModel();

                    objDetail.ProblemListId = model.ProblemListId;
                    objDetail.ProblemDetailId = model.ProblemDetailId ;
                    objDetail.TNMSystemCodeId = value;
                   
                    if(!string.IsNullOrEmpty( value))
                    lstProblemDetail.Add(objDetail);

                }

                if (prop.Name == "DiagnosisDate")
                {
                    string value = (string)prop.GetValue(model, null);
                    objprob.CancerDiagnosisDate = value;

                }
                if (prop.Name == "ClinicalDiagnosisDate")
                {
                    string value = (string)prop.GetValue(model, null);
                    if (value != "")
                        objprob.CancerClinicalDiagnosisDate = value;

                }
                if (prop.Name == "EffectiveDate")
                {
                    string value = (string)prop.GetValue(model, null);
                    if (value != "")
                        objprob.CancerEffectiveDate = value;

                }

                if (prop.Name == "IsActive")
                {
                    string value = (string)prop.GetValue(model, null);
                    objprob.CancerIsActive = value;

                }
                if (prop.Name == "PrimarySiteId")
                {
                    string value = (string)prop.GetValue(model, null);
                    objprob.PrimarySiteId = value;

                }
                if (prop.Name == "HistologicTypeId")
                {
                    string value = (string)prop.GetValue(model, null);
                    objprob.HistologicTypeId = value;

                }
                if (prop.Name == "NKOClinical")
                {
                    string value = (string)prop.GetValue(model, null);
                    objprob.NKOClinical = value;

                }
                if (prop.Name == "NKOPathologic")
                {
                    string value = (string)prop.GetValue(model, null);
                    objprob.NKOPathologic = value;

                }
            }
            objprob.ProblemDetailId = model.ProblemDetailId;
            objprob.ProblemListId = model.ProblemListId;
            objprob.ProblemDetails = lstProblemDetail;
            return createProblemDetailXML(objprob);

        }
        
        public string SaveProblemDetails(ProblemListModel model)
        {
            try
            {
                DSProblemLists dsProblemList = new DSProblemLists();

                string ProblemDetailXML = ReturnProblemDetailXML(model);
                
                #region Database Insertion
         string result = BLLClinicalObj.InsertProblemDetails(ProblemDetailXML);
               

                if (string.IsNullOrEmpty(result))
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = result
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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

        public string LoadProblemListOp_Obsolete(ProblemListModel model)
        {
            try
            {

                DSProblemLists dsProblemList = null;
                BLObject<DSProblemLists> obj;

                obj = BLLClinicalObj.LoadProblemListsOp_Obsolete(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", "0");

                dsProblemList = obj.Data;
                if (obj.Data != null)
                {
                    int ProblemListTotalCount = 0;
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count == 0)
                    {
                        BLObject<string> obj1 = BLLClinicalObj.CheckProblemListExists(MDVUtility.ToLong(model.PatientId));
                        if (obj1.Data == "1")
                        {
                            ProblemListTotalCount = 1;
                        }
                        else
                        {
                            ProblemListTotalCount = 0;
                        }
                    }
                    else
                    {
                        ProblemListTotalCount = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count;
                    }
                    var response = new
                    {
                        status = true,
                        ProblemListTotalCount = ProblemListTotalCount,
                        ProblemListCount = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count,
                        ProblemListLoad_JSON = MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemList.TableName]),
                        ProblemListHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemHistory.TableName]),
                        iTotalDisplayRecords = (dsProblemList.ProblemList.Rows.Count > 0) ? dsProblemList.ProblemList.Rows[0][dsProblemList.ProblemList.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ProblemListCount = 0,
                        Message = obj.Message
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
        public string LoadProblemListOp(ProblemListModel model)
        {
            try
            {
                Tuple<List<ProblemList>, List<ProblemHistory>> tupleResponse = BLLClinicalObj.LoadProblemListsOp(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", "0");

                if (tupleResponse != null)
                {
                    int ProblemListTotalCount = 0;
                    if (tupleResponse.Item1.Count == 0)
                    {
                        BLObject<string> obj1 = BLLClinicalObj.CheckProblemListExists(MDVUtility.ToLong(model.PatientId));
                        if (obj1.Data == "1")
                        {
                            ProblemListTotalCount = 1;
                        }
                        else
                        {
                            ProblemListTotalCount = 0;
                        }
                    }
                    else
                    {
                        ProblemListTotalCount = tupleResponse.Item1.Count;
                    }
                    var response = new
                    {
                        status = true,
                        ProblemListTotalCount = ProblemListTotalCount,
                        ProblemListCount = tupleResponse.Item1.Count,
                        ProblemListLoad_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(tupleResponse.Item1),
                        ProblemListHistoryLoad_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(tupleResponse.Item2),
                        iTotalDisplayRecords = (tupleResponse.Item1.Count > 0) ? MDVUtility.ToInt(tupleResponse.Item1[0].RecordCount) : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ProblemListCount = 0,
                        Message = "No record found"
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

        public string LoadPreviousProblemLists(ProblemListModel model)
        {
            try
            {
                List<MDVision.Model.Clinical.Notes.ProblemsListModel> PreviousProblemsList = null;
                BLObject<List<MDVision.Model.Clinical.Notes.ProblemsListModel>> obj;
                obj = BLLClinicalObj.LoadPreviousProblemLists(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), MDVUtility.ToStr(model.ProviderIDs));

                PreviousProblemsList = obj.Data;
                if (obj.Data != null)
                {
                    int ProblemListTotalCount = 0;
                    if (PreviousProblemsList.Count == 0)
                    {
                        BLObject<string> obj1 = BLLClinicalObj.CheckProblemListExists(MDVUtility.ToLong(model.PatientId));
                        if (obj1.Data == "1")
                        {
                            //ProblemListTotalCount = 1;
                        }
                        else
                        {
                            ProblemListTotalCount = 0;
                        }
                    }
                    else
                    {
                        ProblemListTotalCount = PreviousProblemsList.Count;
                    }
                    var response = new
                    {
                        status = true,
                        ProblemListCount = PreviousProblemsList.Count,
                        ProblemListLoad_JSON = PreviousProblemsList,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ProblemListCount = 0,
                        Message = obj.Message
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
        //public string SaveProblemList(ProblemListModel model)
        //{
        //    try
        //    {
        //        DSProblemLists dsProblemList = new DSProblemLists();
        //        DSProblemLists.ProblemListRow dr = dsProblemList.ProblemList.NewProblemListRow();

        //        dr.PatientId = MDVUtility.ToInt64(model.PatientId);
        //        dr.ProblemName = string.IsNullOrEmpty(MDVUtility.ToStr(model.ProblemName)) == true ? "No Known Problems" : MDVUtility.ToStr(model.ProblemName);
        //        dr.Description = MDVUtility.ToStr(model.Description);
        //        if (!string.IsNullOrEmpty(model.ChronicityLevel))
        //            dr.ChronicityLevel = MDVUtility.ToStr(model.ChronicityLevel);
        //        else
        //            dr[dsProblemList.ProblemList.ChronicityLevelColumn] = DBNull.Value;
        //        if (!string.IsNullOrEmpty(model.Severity))
        //            dr.Severity = MDVUtility.ToStr(model.Severity);
        //        else
        //            dr[dsProblemList.ProblemList.SeverityColumn] = DBNull.Value;
        //        //dr.StartDate = Utility.ToDateTime(model.StartDate);

        //        if (!string.IsNullOrEmpty(model.StartDate))
        //            dr.StartDate = MDVUtility.ToDateTime(model.StartDate);
        //        else
        //            dr[dsProblemList.ProblemList.StartDateColumn] = DBNull.Value;

        //        //dr.EndDate = MDVUtility.ToDateTime(model.EndDate);

        //        if (!string.IsNullOrEmpty(model.EndDate))
        //            dr.EndDate = MDVUtility.ToDateTime(model.EndDate);
        //        else
        //            dr[dsProblemList.ProblemList.EndDateColumn] = DBNull.Value;

        //        dr.Comments = MDVUtility.ToStr(model.Comments);
        //        dr.IsActive = true;

        //        if (!string.IsNullOrEmpty(model.ProblemOrder))
        //            dr.ProblemOrder = MDVUtility.ToStr(model.ProblemOrder);
        //        else
        //            dr[dsProblemList.ProblemList.ProblemOrderColumn] = DBNull.Value;

        //        if (!string.IsNullOrEmpty(model.ICD9))
        //            dr.ICD9 = model.ICD9;
        //        else
        //            dr[dsProblemList.ProblemList.ICD9Column] = DBNull.Value;
        //        if (!string.IsNullOrEmpty(model.ICD10))
        //            dr.ICD10 = model.ICD10;
        //        else
        //            dr[dsProblemList.ProblemList.ICD10Column] = DBNull.Value;
        //        if (!string.IsNullOrEmpty(model.ICD9_Description))
        //            dr.ICD9_Description = model.ICD9_Description;
        //        else
        //            dr[dsProblemList.ProblemList.ICD9_DescriptionColumn] = DBNull.Value;
        //        if (!string.IsNullOrEmpty(model.ICD10_Description))
        //            dr.ICD10_Description = model.ICD10_Description;
        //        else
        //            dr[dsProblemList.ProblemList.ICD10_DescriptionColumn] = DBNull.Value;
        //        if (!string.IsNullOrEmpty(model.SNOMEDID))
        //            dr.SNOMEDID = model.SNOMEDID;
        //        else
        //            dr[dsProblemList.ProblemList.SNOMEDIDColumn] = DBNull.Value;
        //        if (!string.IsNullOrEmpty(model.SNOMED_DESCRIPTION))
        //            dr.SNOMED_DESCRIPTION = model.SNOMED_DESCRIPTION;
        //        else
        //            dr[dsProblemList.ProblemList.SNOMED_DESCRIPTIONColumn] = DBNull.Value;

        //        if (!string.IsNullOrEmpty(model.NoteId))
        //        {
        //            dr.NoteId = model.NoteId;
        //        }


        //        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //        dr.CreatedOn = DateTime.Now;
        //        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //        dr.ModifiedOn = DateTime.Now;

        //        #region Database Insertion
        //        dsProblemList.ProblemList.AddProblemListRow(dr);
        //        BLObject<DSProblemLists> obj = BLLClinicalObj.InsertProblemLists(dsProblemList);
        //        dsProblemList = obj.Data;

        //        if (obj.Data != null)
        //        {
        //            var response = new
        //            {
        //                status = true,
        //                message = Common.AppPrivileges.Save_Message,
        //                ProblemListId = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.ProblemListIdColumn.ColumnName]
        //            };

        //            if (isDrFirstRequired == true)
        //            {
        //                //temp
        //                #region Add Problem In DrFirst
        //                string PatientID = MDVUtility.ToStr(dr.PatientId);
        //                string problemID = MDVUtility.ToStr(dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.ProblemListIdColumn.ColumnName]);

        //                dynamic ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("Problem", dr, problemID));

        //                // SaveProblemInDrFirst(dr, problemID);
        //                if (ResponseOfDrFirst.Rcopia != "")
        //                {
        //                    DSProblemLists DatasetProblem = new DSProblemLists();
        //                    DSProblemLists.ProblemListRow ProblemRow = DatasetProblem.ProblemList.NewProblemListRow();
        //                    ProblemRow.ProblemListId = MDVUtility.ToInt32(problemID);
        //                    ProblemRow.PatientId = dr.PatientId;
        //                    ProblemRow.Comments = MDVUtility.ToStr(model.Comments);
        //                    ProblemRow.IsActive = true;
        //                    ProblemRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                    ProblemRow.CreatedOn = DateTime.Now;
        //                    ProblemRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                    ProblemRow.ModifiedOn = DateTime.Now;
        //                    ProblemRow.RcopiaID = ResponseOfDrFirst.Rcopia;
        //                    DatasetProblem.ProblemList.AddProblemListRow(ProblemRow);
        //                    BLObject<DSProblemLists> obj1 = BLLClinicalObj.InsertProblemRcopialID(DatasetProblem);
        //                    if (obj1.Data != null)
        //                    {
        //                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //                    }
        //                    else
        //                    {
        //                        var responseRcopiaerror = new
        //                        {
        //                            status = false,
        //                            Message = obj.Message
        //                        };
        //                        return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
        //                    }
        //                }
        //                else
        //                {
        //                    var responseRcopiaerror = new
        //                    {
        //                        status = false,
        //                        Message = "Problem in Add Problem on DrFirst"
        //                    };
        //                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
        //                }


        //                #endregion
        //            }
        //            else
        //            {
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }


        //        }
        //        else
        //        {
        //            var message = "";
        //            var ProblemListId = "0";
        //            if (obj.Message.IndexOf(",/") > -1)
        //            {
        //                var messagearray = MDVCustomException.HumanReadableMessage(obj.Message).Split(new[] { ",/" }, StringSplitOptions.None);
        //                if (messagearray.Length == 2)
        //                {

        //                    message = messagearray[0];
        //                    ProblemListId = messagearray[1];
        //                }
        //                else
        //                {
        //                    message = MDVCustomException.HumanReadableMessage(obj.Message);
        //                }
        //            }
        //            else
        //            {
        //                message = MDVCustomException.HumanReadableMessage(obj.Message);
        //            }
        //            var response = new
        //            {
        //                status = false,
        //                Message = message,
        //                ProblemListId = ProblemListId
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
                   
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}



        public string SaveProblemListOp(ProblemListModel model)
        {
            try
            {
                DSProblemLists dsProblemList = new DSProblemLists();
                DSProblemLists.ProblemListRow dr = dsProblemList.ProblemList.NewProblemListRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                dr.ProblemName = string.IsNullOrEmpty(MDVUtility.ToStr(model.ProblemName)) == true ? "No Known Problems" : MDVUtility.ToStr(model.ProblemName);
                dr.Description = MDVUtility.ToStr(model.Description);
                dr.IsActiveGrid = model.IsActiveGrid;
                if (!string.IsNullOrEmpty(model.ChronicityLevel))
                    dr.ChronicityLevel = MDVUtility.ToStr(model.ChronicityLevel);
                else
                    dr[dsProblemList.ProblemList.ChronicityLevelColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.Severity))
                    dr.Severity = MDVUtility.ToStr(model.Severity);
                else
                    dr[dsProblemList.ProblemList.SeverityColumn] = DBNull.Value;
                //dr.StartDate = Utility.ToDateTime(model.StartDate);

                if (!string.IsNullOrEmpty(model.StartDate))
                    dr.StartDate = MDVUtility.ToDateTime(model.StartDate);
                else
                    dr[dsProblemList.ProblemList.StartDateColumn] = DBNull.Value;

                //dr.EndDate = MDVUtility.ToDateTime(model.EndDate);

                if (!string.IsNullOrEmpty(model.EndDate))
                    dr.EndDate = MDVUtility.ToDateTime(model.EndDate);
                else
                    dr[dsProblemList.ProblemList.EndDateColumn] = DBNull.Value;

                dr.Comments = MDVUtility.ToStr(model.Comments);
                dr.IsActive = true;


                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.ProblemName)))
                {
                    if (!string.IsNullOrEmpty(model.ICD9))
                        dr.ICD9 = model.ICD9;
                    else
                        dr[dsProblemList.ProblemList.ICD9Column] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.ICD10))
                        dr.ICD10 = model.ICD10;
                    else
                        dr[dsProblemList.ProblemList.ICD10Column] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.ICD9_Description))
                        dr.ICD9_Description = model.ICD9_Description;
                    else
                        dr[dsProblemList.ProblemList.ICD9_DescriptionColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.ICD10_Description))
                        dr.ICD10_Description = model.ICD10_Description;
                    else
                        dr[dsProblemList.ProblemList.ICD10_DescriptionColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.SNOMEDID))
                        dr.SNOMEDID = model.SNOMEDID;
                    else
                        dr[dsProblemList.ProblemList.SNOMEDIDColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.SNOMED_DESCRIPTION))
                        dr.SNOMED_DESCRIPTION = model.SNOMED_DESCRIPTION;
                    else
                        dr[dsProblemList.ProblemList.SNOMED_DESCRIPTIONColumn] = DBNull.Value;
                }
                else
                {
                    dr[dsProblemList.ProblemList.ICD9Column] = DBNull.Value;
                    dr[dsProblemList.ProblemList.ICD10Column] = DBNull.Value;
                    dr[dsProblemList.ProblemList.ICD9_DescriptionColumn] = DBNull.Value;
                    dr[dsProblemList.ProblemList.ICD10_DescriptionColumn] = "No Known Problems";
                    dr[dsProblemList.ProblemList.SNOMEDIDColumn] = DBNull.Value;
                    dr[dsProblemList.ProblemList.SNOMED_DESCRIPTIONColumn] = DBNull.Value;
                }



                if (!string.IsNullOrEmpty(model.NoteId))
                {
                    dr.NoteId = model.NoteId;
                }
                else
                {
                    dr[dsProblemList.ProblemList.NoteIdColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.IsChiefComplaint))
                {
                    dr.IsChiefComplaint = MDVUtility.ToInt64(model.IsChiefComplaint);
                }
                else
                {
                    dr[dsProblemList.ProblemList.IsChiefComplaintColumn] = DBNull.Value;
                }

                if (MDVUtility.ToInt64(model.CustomFormId) > 0)
                    dr.CustomFormId = MDVUtility.ToInt64(model.CustomFormId);
                else
                    dr[dsProblemList.ProblemList.CustomFormIdColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.CheckProblemExists))
                {
                    dr.CheckProblemExists = model.CheckProblemExists == "1" ? true : false;
                }
                else
                {
                    dr.CheckProblemExists = false;
                }
                if (!string.IsNullOrEmpty(model.ComplaintId))
                {
                    dr.ComplaintId = model.ComplaintId;
                }
                else
                {
                    dr[dsProblemList.ProblemList.ComplaintIdColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.ComplaintDetailId))
                {
                    dr.ComplaintDetailId = model.ComplaintDetailId;
                }
                else
                {
                    dr[dsProblemList.ProblemList.ComplaintDetailIdColumn] = DBNull.Value;
                }
                dr.IsNonDiabetic = model.IsNonDiabetic;

                dr.IsDiabeticScreening = model.IsDiabeticScreening;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsProblemList.ProblemList.AddProblemListRow(dr);
                BLObject<DSProblemLists> obj = BLLClinicalObj.InsertProblemLists(dsProblemList);
                dsProblemList = obj.Data;

                if (!string.IsNullOrEmpty(model.UpdateFavValues) && model.UpdateFavValues == "1")
                {
                    LabOrderHelper.insertUpdateFavListSetting(model.FavListNames);
                    FavoriteListHelper Helper = new FavoriteListHelper();
                    Helper.InsertUpdateFavlistValue(model.FavListName, model.FavListVal);
                }


                if (obj.Data == null)
                {
                    var message = "";
                    var ProblemListId = "0";
                    if (obj.Message.IndexOf(",/") > -1)
                    {
                        var messagearray = MDVCustomException.HumanReadableMessage(obj.Message).Split(new[] { ",/" }, StringSplitOptions.None);
                        if (messagearray.Length == 2)
                        {
                            message = messagearray[0];
                            ProblemListId = messagearray[1];
                        }
                        else
                        {
                            message = MDVCustomException.HumanReadableMessage(obj.Message);
                        }
                    }
                    else
                    {
                        message = MDVCustomException.HumanReadableMessage(obj.Message);
                    }
                    var response = new
                    {
                        status = false,
                        Message = message,
                        ProblemListId = ProblemListId
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var ProblemListId = MDVUtility.ToStr(dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.ProblemListIdColumn.ColumnName]);



                    DSProblemLists dsProb = new DSProblemLists();
                    //DataRow row = dsProb.Tables["ProblemList"].NewRow();
                    DSProblemLists.ProblemListRow row = dsProblemList.ProblemList.NewProblemListRow();
                    row["PatientId"] = MDVUtility.ToInt64(dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.PatientIdColumn.ColumnName]);
                    row["ProblemListId"] = MDVUtility.ToInt64(dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.ProblemListIdColumn.ColumnName]);
                    row["StartDate"] = MDVUtility.ToDateTime(dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.StartDateColumn.ColumnName]);
                    row["ICD10"] = MDVUtility.ToStr(dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.ICD10Column.ColumnName]);
                    row["ICD10_Description"] = MDVUtility.ToStr(dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.ICD10_DescriptionColumn.ColumnName]);
                    row["Description"] = MDVUtility.ToStr(dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.DescriptionColumn.ColumnName]);
                    string AppUserName = MDVSession.Current.AppUserName;
                    SharedVariable sharedVariable = SharedVariable.GetSharedVariable();
                    HttpContext hCtrl = HttpContext.Current;
                    if (obj.Data != null)
                    {
                        //AppConfig.ProblemListRow = dr;
                        //MDVSession.Current.ProblemList4DrFirst = dr;

                        Thread thread = new Thread(new ThreadStart(delegate()
                        {
                            try
                            {
                                ProblemListModel ProModel = new ProblemListModel();
                                ProModel.ProblemListId = ProblemListId;
                                ProblemListHelper probHelp = new ProblemListHelper();
                                probHelp.SaveProblemOnDrFirst(ProModel, row, AppUserName, sharedVariable, hCtrl);
                            }
                            catch (Exception ex)
                            {
                                MDVLogger.SendExcepToDB(ex, "AddProblemList On DrFirst", null);
                            }
                        }));
                        thread.IsBackground = true;
                        thread.Start();



                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ProblemListId = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.ProblemListIdColumn.ColumnName]
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }

                }

                #endregion
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
        public string SaveProblemListOpUnique(ProblemListModel model)
        {
            try
            {
                DSProblemLists dsProblemList = new DSProblemLists();
                DSProblemLists.ProblemListRow dr = dsProblemList.ProblemList.NewProblemListRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                dr.ProblemName = string.IsNullOrEmpty(MDVUtility.ToStr(model.ProblemName)) == true ? "No Known Problems" : MDVUtility.ToStr(model.ProblemName);
                dr.Description = MDVUtility.ToStr(model.Description);
                if (!string.IsNullOrEmpty(model.ChronicityLevel))
                    dr.ChronicityLevel = MDVUtility.ToStr(model.ChronicityLevel);
                else
                    dr[dsProblemList.ProblemList.ChronicityLevelColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.Severity))
                    dr.Severity = MDVUtility.ToStr(model.Severity);
                else
                    dr[dsProblemList.ProblemList.SeverityColumn] = DBNull.Value;
                //dr.StartDate = Utility.ToDateTime(model.StartDate);

                if (!string.IsNullOrEmpty(model.StartDate))
                    dr.StartDate = MDVUtility.ToDateTime(model.StartDate);
                else
                    dr[dsProblemList.ProblemList.StartDateColumn] = DBNull.Value;

                //dr.EndDate = MDVUtility.ToDateTime(model.EndDate);

                if (!string.IsNullOrEmpty(model.EndDate))
                    dr.EndDate = MDVUtility.ToDateTime(model.EndDate);
                else
                    dr[dsProblemList.ProblemList.EndDateColumn] = DBNull.Value;

                dr.Comments = MDVUtility.ToStr(model.Comments);
                dr.IsActive = true;


                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.ProblemName)))
                {
                    if (!string.IsNullOrEmpty(model.ICD9))
                        dr.ICD9 = model.ICD9;
                    else
                        dr[dsProblemList.ProblemList.ICD9Column] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.ICD10))
                        dr.ICD10 = model.ICD10;
                    else
                        dr[dsProblemList.ProblemList.ICD10Column] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.ICD9_Description))
                        dr.ICD9_Description = model.ICD9_Description;
                    else
                        dr[dsProblemList.ProblemList.ICD9_DescriptionColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.ICD10_Description))
                        dr.ICD10_Description = model.ICD10_Description;
                    else
                        dr[dsProblemList.ProblemList.ICD10_DescriptionColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.SNOMEDID))
                        dr.SNOMEDID = model.SNOMEDID;
                    else
                        dr[dsProblemList.ProblemList.SNOMEDIDColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.SNOMED_DESCRIPTION))
                        dr.SNOMED_DESCRIPTION = model.SNOMED_DESCRIPTION;
                    else
                        dr[dsProblemList.ProblemList.SNOMED_DESCRIPTIONColumn] = DBNull.Value;
                }
                else
                {
                    dr[dsProblemList.ProblemList.ICD9Column] = DBNull.Value;
                    dr[dsProblemList.ProblemList.ICD10Column] = DBNull.Value;
                    dr[dsProblemList.ProblemList.ICD9_DescriptionColumn] = DBNull.Value;
                    dr[dsProblemList.ProblemList.ICD10_DescriptionColumn] = DBNull.Value;
                    dr[dsProblemList.ProblemList.SNOMEDIDColumn] = DBNull.Value;
                    dr[dsProblemList.ProblemList.SNOMED_DESCRIPTIONColumn] = DBNull.Value;
                }



                if (!string.IsNullOrEmpty(model.NoteId))
                {
                    dr.NoteId = model.NoteId;
                }
                else
                {
                    dr[dsProblemList.ProblemList.NoteIdColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.IsChiefComplaint))
                {
                    dr.IsChiefComplaint = MDVUtility.ToInt64(model.IsChiefComplaint);
                }
                else
                {
                    dr[dsProblemList.ProblemList.IsChiefComplaintColumn] = DBNull.Value;
                }


                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.IsNonDiabetic = model.IsNonDiabetic;

                dr.IsDiabeticScreening = model.IsDiabeticScreening;
                #region Database Insertion
                dsProblemList.ProblemList.AddProblemListRow(dr);
                BLObject<DSProblemLists> obj = BLLClinicalObj.InsertProblemListsUnique(dsProblemList);
                dsProblemList = obj.Data;

                if (obj.Data != null)
                {
                    if (MDVUtility.ToInt64(dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.ProblemListIdColumn.ColumnName]) == 0)
                    {
                        var response = new
                        {
                            status = false,
                            Message = "This Problem already exists in Patient's Active Problems !",
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }

                    else if (MDVUtility.ToInt64(dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.ProblemListIdColumn.ColumnName]) == -1)
                    {
                        var response = new
                        {
                            status = false,
                            Message = "This Problem already exists in Patient's Inactive Problems !",
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        MDVSession.Current.ProblemList4DrFirst = dr;
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ProblemListId = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows[0][dsProblemList.ProblemList.ProblemListIdColumn.ColumnName]
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    //AppConfig.ProblemListRow = dr;
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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

        public void SaveProblemOnDrFirst(ProblemListModel model, DSProblemLists.ProblemListRow dr, string AppUserName, SharedVariable sharedVariable = null, HttpContext hCtrl = null)
        {
            try
            {
                DSProblemLists dsProblemList = new DSProblemLists();
                //DSProblemLists.ProblemListRow dr = dsProblemList.ProblemList.NewProblemListRow();

                //dr = (MDVSession.Current.ProblemList4DrFirst != null) ? MDVSession.Current.ProblemList4DrFirst as DSProblemLists.ProblemListRow : dr;
                //MDVSession.Current.ProblemList4DrFirst = null;

                if (isDrFirstRequired == true)
                {
                    //temp
                    #region Add Problem In DrFirst
                    string PatientID = MDVUtility.ToStr(dr.PatientId);
                    string problemID = MDVUtility.ToStr(model.ProblemListId);

                    dynamic ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("Problem", dr, problemID, sharedVariable, hCtrl));

                    // SaveProblemInDrFirst(dr, problemID);
                    if (ResponseOfDrFirst.Rcopia != "")
                    {
                        DSProblemLists DatasetProblem = new DSProblemLists();
                        DSProblemLists.ProblemListRow ProblemRow = DatasetProblem.ProblemList.NewProblemListRow();
                        ProblemRow.ProblemListId = MDVUtility.ToInt32(problemID);
                        ProblemRow.PatientId = dr.PatientId;
                        ProblemRow.Comments = MDVUtility.ToStr(model.Comments);
                        ProblemRow.IsActive = true;
                        ProblemRow.CreatedBy = MDVUtility.DecryptFrom64(AppUserName);
                        ProblemRow.CreatedOn = DateTime.Now;
                        ProblemRow.ModifiedBy = MDVUtility.DecryptFrom64(AppUserName);
                        ProblemRow.ModifiedOn = DateTime.Now;
                        ProblemRow.RcopiaID = ResponseOfDrFirst.Rcopia;
                        DatasetProblem.ProblemList.AddProblemListRow(ProblemRow);
                        BLObject<DSProblemLists> obj1 = BLLClinicalObj.InsertProblemRcopialID(DatasetProblem, sharedVariable);
                        if (obj1.Data != null)
                        {
                            var responseRcopiaerror = new
                            {
                                status = true,
                            };
                            //  return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));

                        }
                        else
                        {
                            var responseRcopiaerror = new
                            {
                                status = false,
                                Message = obj1.Message
                            };
                            //    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                        }
                    }
                    else
                    {
                        var responseRcopiaerror = new
                        {
                            status = false,
                            Message = "Problem in Add Problem on DrFirst"
                        };
                        //   return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                    }


                    #endregion
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = true,
                    };
                    //    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                }




            }
            catch (Exception ex)
            {
                throw ex;
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                //     return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        //private string SaveProblemInDrFirst(DSProblemLists.ProblemListRow ProblemData, string ProblemID)
        //{
        //    HttpClient client = new HttpClient();
        //    //client.BaseAddress = new Uri("http://localhost:8080/");

        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/xml"));

        //    var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>vendorsh1100</VendorName>       <VendorPassword>b2xddjpn</VendorPassword>       </Caller>    <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

        //    var url = "https://engine301.drfirst.com/servlet/rcopia.servlet.EngineServlet/getURL?xml=" + inputdata;

        //    HttpResponseMessage response = client.GetAsync(url).Result;

        //    if (response != null)
        //    {
        //        var getdata = response.Content.ReadAsStringAsync().Result;
        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(getdata);
        //        //string jsonText = JsonConvert.SerializeXmlNode(doc);
        //        XmlNodeList nodeListuploadurl = doc.GetElementsByTagName("EngineUploadURL");
        //        XmlNodeList nodelistDownloadurl = doc.GetElementsByTagName("EngineDownloadURL");

        //        string UploadUrl = string.Empty;
        //        string downloadUrl = string.Empty;
        //        foreach (XmlNode node in nodeListuploadurl)
        //        {
        //            UploadUrl = node.InnerText;
        //        }
        //        foreach (XmlNode node in nodelistDownloadurl)
        //        {
        //            downloadUrl = node.InnerText;
        //        }

        //        //string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername,DSPatient.PatientsRow patient
        //        var uploadProblem = MDVUtility.GetXmlForAddProblemList("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", ProblemData, ProblemID);
        //        var Uploadurl = UploadUrl + "?xml=" + uploadProblem;
        //        HttpResponseMessage ResponseUploadProblem = client.GetAsync(Uploadurl).Result;
        //        var GetProblemData = ResponseUploadProblem.Content.ReadAsStringAsync().Result;

        //        XmlDocument Xmldoc = new XmlDocument();
        //        Xmldoc.LoadXml(GetProblemData);

        //        string status = "";
        //        XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");


        //        foreach (XmlNode node in statusNode)
        //        {
        //            status = node.InnerText;
        //        }
        //        if (status == "ok")
        //        {
        //            string RcopiaID = "";
        //            XmlNodeList RcopiaNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/ProblemList/Problem/RcopiaID");

        //            foreach (XmlNode node in RcopiaNode)
        //            {
        //                RcopiaID = node.InnerText;
        //            }
        //            return RcopiaID;
        //        }
        //        else if (status == "error")
        //        {
        //            return "error";
        //        }
        //    }
        //    return "";

        //}

        public string UpdateProblemList(ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {

                    DSProblemLists dsProblemList = new DSProblemLists();
                    BLObject<DSProblemLists> obj = BLLClinicalObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId));
                    dsProblemList = obj.Data;
                    foreach (DSProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.PatientId))
                            dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                        if (!string.IsNullOrEmpty(model.ProblemName))
                            dr.ProblemName = MDVUtility.ToStr(model.ProblemName);
                        if (!string.IsNullOrEmpty(model.Description))
                            dr.Description = MDVUtility.ToStr(model.Description);

                        //if (!string.IsNullOrEmpty(model.ChronicityLevel))
                        if (!string.IsNullOrEmpty(model.ChronicityLevel))
                            dr.ChronicityLevel = MDVUtility.ToStr(model.ChronicityLevel);
                        else
                            dr[dsProblemList.ProblemList.ChronicityLevelColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.Severity))
                            dr.Severity = MDVUtility.ToStr(model.Severity);
                        else
                            dr[dsProblemList.ProblemList.SeverityColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.StartDate))
                            dr.StartDate = MDVUtility.ToDateTime(model.StartDate);
                        else
                            dr[dsProblemList.ProblemList.StartDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.EndDate))
                            dr.EndDate = MDVUtility.ToDateTime(model.EndDate);
                        else
                            dr[dsProblemList.ProblemList.EndDateColumn] = DBNull.Value;


                        if (!string.IsNullOrEmpty(model.ICD9))
                            dr.ICD9 = model.ICD9;

                        if (!string.IsNullOrEmpty(model.ICD10))
                            dr.ICD10 = model.ICD10;

                        if (!string.IsNullOrEmpty(model.ICD9_Description))
                            dr.ICD9_Description = model.ICD9_Description;

                        if (!string.IsNullOrEmpty(model.ICD10_Description))
                            dr.ICD10_Description = model.ICD10_Description;

                        if (!string.IsNullOrEmpty(model.SNOMEDID))
                            dr.SNOMEDID = model.SNOMEDID;

                        if (!string.IsNullOrEmpty(model.SNOMED_DESCRIPTION))
                            dr.SNOMED_DESCRIPTION = model.SNOMED_DESCRIPTION;



                        if (!string.IsNullOrEmpty(model.Comments))
                            dr.Comments = MDVUtility.ToStr(model.Comments);
                        else
                            dr.Comments = "";
                        // If Problem List Is attached to multiple notes, this would be string, which will give exception to string to int64 failed, so assigning to null
                        dr[dsProblemList.ProblemList.NoteIdColumn] = DBNull.Value;
                        dr.IsActive = MDVUtility.ToStr(model.IsActive) == "1" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        //end newly added
                    }
                    Patient_Demographic objUpdateProblem = new Patient_Demographic();


                    #region Database Updation
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                    {
                        BLObject<DSProblemLists> objUpdate = BLLClinicalObj.UpdateProblemLists(dsProblemList);
                        if (objUpdate.Data != null)
                        {
                            if (isDrFirstRequired == true)
                            {
                                #region Update Problem In DrFirst
                                foreach (DSProblemLists.ProblemListRow drfirst in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
                                {
                                    dynamic ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("UpdateProblem", drfirst, ""));
                                    if (ResponseOfDrFirst.Rcopia != "")
                                    {

                                    }
                                    else
                                    {
                                        var response1 = new
                                        {
                                            status = false,
                                            message = "Problem in Update Problem on DrFirst"
                                        };
                                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                                    }

                                }

                                //UpdateProblemInDrFirst(drfirst);//

                                #endregion
                            }


                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Problem not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        public string UpdateProblemListOp(ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {

                    DSProblemLists dsProblemList = new DSProblemLists();
                    BLObject<DSProblemLists> obj = BLLClinicalObj.LoadProblemListsForFillData(MDVUtility.ToInt64(model.ProblemListId));
                    dsProblemList = obj.Data;
                    DSProblemLists.ProblemListRow dr1 = null;
                    foreach (DSProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.PatientId))
                            dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                        if (!string.IsNullOrEmpty(model.ProblemName))
                            dr.ProblemName = MDVUtility.ToStr(model.ProblemName);
                        if (!string.IsNullOrEmpty(model.Description))
                            dr.Description = MDVUtility.ToStr(model.Description);

                        //if (!string.IsNullOrEmpty(model.ChronicityLevel))
                        if (!string.IsNullOrEmpty(model.ChronicityLevel))
                            dr.ChronicityLevel = MDVUtility.ToStr(model.ChronicityLevel);
                        else
                            dr[dsProblemList.ProblemList.ChronicityLevelColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.Severity))
                            dr.Severity = MDVUtility.ToStr(model.Severity);
                        else
                            dr[dsProblemList.ProblemList.SeverityColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.StartDate))
                            dr.StartDate = MDVUtility.ToDateTime(model.StartDate);
                        else
                            dr[dsProblemList.ProblemList.StartDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.EndDate))
                            dr.EndDate = MDVUtility.ToDateTime(model.EndDate);
                        else
                            dr[dsProblemList.ProblemList.EndDateColumn] = DBNull.Value;


                        if (!string.IsNullOrEmpty(model.ICD9))
                            dr.ICD9 = model.ICD9;

                        if (!string.IsNullOrEmpty(model.ICD10))
                            dr.ICD10 = model.ICD10;

                        if (!string.IsNullOrEmpty(model.ICD9_Description))
                            dr.ICD9_Description = model.ICD9_Description;

                        if (!string.IsNullOrEmpty(model.ICD10_Description))
                            dr.ICD10_Description = model.ICD10_Description;

                        if (!string.IsNullOrEmpty(model.SNOMEDID))
                            dr.SNOMEDID = model.SNOMEDID;

                        if (!string.IsNullOrEmpty(model.SNOMED_DESCRIPTION))
                            dr.SNOMED_DESCRIPTION = model.SNOMED_DESCRIPTION;



                        if (!string.IsNullOrEmpty(model.Comments))
                            dr.Comments = MDVUtility.ToStr(model.Comments);
                        else
                            dr.Comments = "";

                        if (MDVUtility.ToInt64(model.CustomFormId) > 0)
                            dr.CustomFormId = MDVUtility.ToInt64(model.CustomFormId);
                        else
                            dr[dsProblemList.ProblemList.CustomFormIdColumn] = DBNull.Value;

                        // If Problem List Is attached to multiple notes, this would be string, which will give exception to string to int64 failed, so assigning to null
                        dr[dsProblemList.ProblemList.NoteIdColumn] = DBNull.Value;
                        dr.IsActive = MDVUtility.ToStr(model.IsActive) == "1" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(model.NoteId))
                        {
                            dr.NoteId = model.NoteId;
                        }
                        else
                        {
                            dr[dsProblemList.ProblemList.NoteIdColumn] = DBNull.Value;
                        }
                        dr1 = dr;
                        //end newly added
                    }
                    Patient_Demographic objUpdateProblem = new Patient_Demographic();


                    #region Database Updation
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                    {
                        BLObject<DSProblemLists> objUpdate = BLLClinicalObj.UpdateProblemListsOp(dsProblemList);
                        if (objUpdate.Data != null)
                        {
                            //HttpContext.Current.Session["ProblemList4GridDrFirst"] = dr1;
                            MDVSession.Current.ProblemList4GridDrFirst = dr1;
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Problem not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        public string UpdateProblemInDrFirstForGrid()
        {
            if (isDrFirstRequired == true)
            {
                DSProblemLists dsProblemList = new DSProblemLists();
                DSProblemLists.ProblemListRow dr = dsProblemList.ProblemList.NewProblemListRow();
                dr = (MDVSession.Current.ProblemList4GridDrFirst != null) ? MDVSession.Current.ProblemList4GridDrFirst as DSProblemLists.ProblemListRow : dr;
                MDVSession.Current.ProblemList4GridDrFirst = null;

                dynamic ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("UpdateProblem", dr, ""));
                if (ResponseOfDrFirst.Rcopia != "")
                {
                    var response = new
                    {
                        status = true,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response1 = new
                    {
                        status = false,
                        message = "Problem in Update Problem on DrFirst"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                }
                //UpdateProblemInDrFirst(drfirst);//
            }
            else
            {
                var response = new
                {
                    status = true,

                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string UpdateProblemInDrFirst(DSProblemLists.ProblemListRow ProblemData)
        {

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:8080/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));

            var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>vendorsh1100</VendorName>       <VendorPassword>b2xddjpn</VendorPassword>       </Caller>    <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

            var url = "https://engine301.drfirst.com/servlet/rcopia.servlet.EngineServlet/getURL?xml=" + inputdata;

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response != null)
            {
                var getdata = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(getdata);
                //string jsonText = JsonConvert.SerializeXmlNode(doc);
                XmlNodeList nodeListuploadurl = doc.GetElementsByTagName("EngineUploadURL");
                XmlNodeList nodelistDownloadurl = doc.GetElementsByTagName("EngineDownloadURL");

                string UploadUrl = string.Empty;
                string downloadUrl = string.Empty;
                foreach (XmlNode node in nodeListuploadurl)
                {
                    UploadUrl = node.InnerText;
                }
                foreach (XmlNode node in nodelistDownloadurl)
                {
                    downloadUrl = node.InnerText;
                }
                //string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername,DSPatient.PatientsRow patient
                var updatePatientXml = MDVUtility.GetXmlForUpdateProblem("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", ProblemData);
                var Uploadurl = UploadUrl + "?xml=" + updatePatientXml;
                HttpResponseMessage ResponseUpdatePatient = client.GetAsync(Uploadurl).Result;
                var GetPatientUpdateData = ResponseUpdatePatient.Content.ReadAsStringAsync().Result;

                XmlDocument Xmldoc = new XmlDocument();
                Xmldoc.LoadXml(GetPatientUpdateData);

                string status = "";
                XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");


                foreach (XmlNode node in statusNode)
                {
                    status = node.InnerText;
                }
                if (status == "ok")
                {

                }
                else if (status == "error")
                {
                    return "error";
                }
            }
            return "";

        }


        public string UpdateProblemListComments(ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {

                    DSProblemLists dsProblemList = new DSProblemLists();
                    BLObject<DSProblemLists> obj = BLLClinicalObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), "1");
                    dsProblemList = obj.Data;
                    foreach (DSProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
                    {

                        if (!string.IsNullOrEmpty(model.Comments))
                            dr.Comments = MDVUtility.ToStr(model.Comments);
                        dr.IsActive = MDVUtility.ToBool(model.IsActive);
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        // If Problem List Is attached to multiple notes, this would be string, which will give exception to string to int64 failed, so assigning to null
                        dr[dsProblemList.ProblemList.NoteIdColumn] = DBNull.Value;
                        //end newly added
                    }
                    #region Database Updation
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                    {
                        BLObject<DSProblemLists> objUpdate = BLLClinicalObj.UpdateProblemLists(dsProblemList);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Problem not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        //public string DeleteProblemList(ProblemListModel model)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ProblemListId)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {

        //            BLObject<string> obj = BLLClinicalObj.DeleteProblemLists(MDVUtility.ToStr(model.ProblemListId));
        //            if (obj.Data == "")
        //            {
        //                if (isDrFirstRequired == true)
        //                {
        //                    #region "deleteRcopiaProblem"
        //                    DeleteDrFirstProblem(model);
        //                    #endregion
        //                }
        //                var response = new
        //                {
        //                    status = true,
        //                    Message = Common.AppPrivileges.Delete_Message
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = obj.Data
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        public string DeleteProblemListOp(ProblemListModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ProblemListId)))
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

                    BLObject<string> obj = BLLClinicalObj.DeleteProblemLists(MDVUtility.ToStr(model.ProblemListId));
                    if (obj.Data == "")
                    {
                        //HttpContext.Current.Session["DeleteProblemList4DrFirst"] = model;
                        MDVSession.Current.DeleteProblemList4DrFirst = model;
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
        public string DeleteProblemFromDrFirst()
        {
            try
            {

                ProblemListModel ProblemListModel = new ProblemListModel();

                ProblemListModel = (MDVSession.Current.DeleteProblemList4DrFirst != null) ? MDVSession.Current.DeleteProblemList4DrFirst as ProblemListModel : ProblemListModel;
                MDVSession.Current.DeleteProblemList4DrFirst = null;

                if (isDrFirstRequired == true)
                {
                    #region "deleteRcopiaProblem"
                    var ReturnData = BLLRcopiaObj.DeleteDrFirstProblem(ProblemListModel);
                    if (ReturnData != "Ok")
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Problem In Deleting ProblemList From DrFirst",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = true,
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

        //private string DeleteDrFirstProblem(ProblemListModel ProblemModel)
        //{
        //    try
        //    {
        //        HttpClient client = new HttpClient();
        //        //client.BaseAddress = new Uri("http://localhost:8080/");

        //        client.DefaultRequestHeaders.Accept.Add(
        //            new MediaTypeWithQualityHeaderValue("application/xml"));
        //        DSRcopia dsRcopia1 = new DSRcopia();
        //        BLObject<DSRcopia> obj1 = new BLLRcopia().SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode);
        //        dsRcopia1 = obj1.Data;
        //        if (obj1.Data != null)
        //        {
        //            if (dsRcopia1.SoftwareCustomersInfo.Rows.Count > 0)
        //            {
        //                string RcopiaANSbackup = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaANSbackupColumn.ColumnName]);
        //                string RcopiaScretkey = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaScretkeyColumn.ColumnName]);
        //                string RcopiaVendorUsername = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaVendorUsernameColumn.ColumnName]);
        //                string RcopiaVendorPassword = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaVendorPasswordColumn.ColumnName]);
        //                string RcopiaPortalSystemName = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaPortalSystemNameColumn.ColumnName]);
        //                string RcopiaPracticeUserName = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaPracticeUserNameColumn.ColumnName]);

        //                var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

        //                DSRcopia dsRcopia = new DSRcopia();
        //                BLObject<DSRcopia> obj = new BLLRcopia().SelectGetUrls();
        //                dsRcopia = obj.Data;
        //                if (obj.Data != null)
        //                {
        //                    if (dsRcopia.Rcopia_GetUrl.Rows.Count > 0)
        //                    {

        //                        var url = string.Concat(MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineUploadURLColumn.ColumnName]), "/getURL?xml=", inputdata);

        //                        HttpResponseMessage response = client.GetAsync(url).Result;

        //                        if (response != null)
        //                        {
        //                            var getdata = response.Content.ReadAsStringAsync().Result;
        //                            XmlDocument doc = new XmlDocument();
        //                            doc.LoadXml(getdata);
        //                            //string jsonText = JsonConvert.SerializeXmlNode(doc);
        //                            XmlNodeList nodeListuploadurl = doc.GetElementsByTagName("EngineUploadURL");
        //                            XmlNodeList nodelistDownloadurl = doc.GetElementsByTagName("EngineDownloadURL");

        //                            string UploadUrl = string.Empty;
        //                            string downloadUrl = string.Empty;
        //                            foreach (XmlNode node in nodeListuploadurl)
        //                            {
        //                                UploadUrl = node.InnerText;
        //                            }
        //                            foreach (XmlNode node in nodelistDownloadurl)
        //                            {
        //                                downloadUrl = node.InnerText;
        //                            }




        //                            //string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername,DSPatient.PatientsRow patient
        //                            var DeleteProblemXml = MDVUtility.GetXmlForDeleteProblem(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, ProblemModel.PatientId, ProblemModel.ProblemListId, ProblemModel.Description, ProblemModel.StartDate);
        //                            var Uploadurl = UploadUrl + "?xml=" + DeleteProblemXml;
        //                            HttpResponseMessage ResponseDeleteProblem = client.GetAsync(Uploadurl).Result;
        //                            var GetProblemdDeleteData = ResponseDeleteProblem.Content.ReadAsStringAsync().Result;

        //                            XmlDocument Xmldoc = new XmlDocument();
        //                            Xmldoc.LoadXml(GetProblemdDeleteData);

        //                            string status = "";
        //                            XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");


        //                            foreach (XmlNode node in statusNode)
        //                            {
        //                                status = node.InnerText;
        //                            }
        //                            if (status == "ok")
        //                            {
        //                                return "Ok";
        //                            }
        //                            else if (status == "error")
        //                            {
        //                                return "error";
        //                            }
        //                        }
        //                        return "";
        //                    }
        //                    else
        //                    {
        //                        throw new Exception("Error In Delete Problem On DrFirst");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception("Error In Delete Problem On DrFirst");
        //                }

        //            }
        //            else
        //            {
        //                throw new Exception("Error In Delete Problem On DrFirst");
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Error In Delete Problem On DrFirst");
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }

        //}

        public string ActiveInActiveProblemList(ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {

                    DSProblemLists dsProblemList = new DSProblemLists();
                    BLObject<DSProblemLists> obj = BLLClinicalObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId));
                    dsProblemList = obj.Data;
                    foreach (DSProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.EndDate))
                            dr.EndDate = MDVUtility.ToDateTime(model.EndDate);
                        else
                        {
                            dr[dsProblemList.ProblemList.EndDateColumn] = DBNull.Value;

                        }

                        dr.InActiveChkBoxValue = MDVUtility.ToStr(model.InActiveChkBoxValue);
                        dr.InActiveReason = MDVUtility.ToStr(model.InActiveReason);

                        dr.IsActive = MDVUtility.ToStr(model.IsActiveRecord) == "1" ? true : false;
                        // If Problem List Is attached to multiple notes, this would be string, which will give exception to string to int64 failed, so assigning to null
                        dr[dsProblemList.ProblemList.NoteIdColumn] = DBNull.Value;
                        //dr.IsActive = false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        //end newly added
                    }
                    if (isDrFirstRequired == true)
                    {
                        #region "ActiveInActiveProblemList in Drfirst"
                        foreach (DSProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
                        {
                            UpdateProblemStatusInDrFirst(dr);
                        }
                        #endregion
                    }
                    #region Database Updation
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                    {
                        BLObject<DSProblemLists> objUpdate = BLLClinicalObj.UpdateProblemLists(dsProblemList);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Problem not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        public string ActiveInActiveProblemListOp(ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {

                    DSProblemLists dsProblemList = new DSProblemLists();
                    BLObject<DSProblemLists> obj = BLLClinicalObj.LoadProblemListsForInActive(MDVUtility.ToInt64(model.ProblemListId));
                    dsProblemList = obj.Data;
                    DSProblemLists.ProblemListRow dr1 = null;
                    foreach (DSProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.EndDate))
                            dr.EndDate = MDVUtility.ToDateTime(model.EndDate);
                        else
                        {
                            dr[dsProblemList.ProblemList.EndDateColumn] = DBNull.Value;

                        }

                        dr.InActiveChkBoxValue = MDVUtility.ToStr(model.InActiveChkBoxValue);
                        dr.InActiveReason = MDVUtility.ToStr(model.InActiveReason);

                        dr.IsActive = MDVUtility.ToStr(model.IsActiveRecord) == "1" ? true : false;
                        // If Problem List Is attached to multiple notes, this would be string, which will give exception to string to int64 failed, so assigning to null
                        dr[dsProblemList.ProblemList.NoteIdColumn] = DBNull.Value;
                        //dr.IsActive = false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr1 = dr;


                        //end newly added
                    }

                    #region Database Updation
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                    {
                        BLObject<DSProblemLists> objUpdate = BLLClinicalObj.UpdateProblemListsForInActive(dsProblemList);
                        if (objUpdate.Data != null)
                        {
                            //HttpContext.Current.Session["ProblemList4INActiveDrFirst"] = dr1;
                            MDVSession.Current.ProblemList4INActiveDrFirst = dr1;
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Problem not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        public string ActiveInActiveProblemListONDrFirst()
        {
            try
            {
                if (isDrFirstRequired == true)
                {
                    DSProblemLists dsProblemList = new DSProblemLists();
                    DSProblemLists.ProblemListRow dr = dsProblemList.ProblemList.NewProblemListRow();
                    dr = (MDVSession.Current.ProblemList4INActiveDrFirst != null) ? MDVSession.Current.ProblemList4INActiveDrFirst as DSProblemLists.ProblemListRow : dr;
                    MDVSession.Current.ProblemList4INActiveDrFirst = null;
                    var RetuenData = UpdateProblemStatusInDrFirst(dr);
                    if (RetuenData == "error")
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Problem In Updating ProblemList ON DrFirst"
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
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
        private string UpdateProblemStatusInDrFirst(DSProblemLists.ProblemListRow ProblemData)
        {
            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:8080/");

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/xml"));

                DSRcopia dsRcopia1 = new DSRcopia();
                BLObject<DSRcopia> obj1 = new BLLRcopia().SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode);
                dsRcopia1 = obj1.Data;
                if (obj1.Data != null)
                {
                    if (dsRcopia1.SoftwareCustomersInfo.Rows.Count > 0)
                    {
                        string RcopiaANSbackup = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaANSbackupColumn.ColumnName]);
                        string RcopiaScretkey = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaScretkeyColumn.ColumnName]);
                        string RcopiaVendorUsername = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaVendorUsernameColumn.ColumnName]);
                        string RcopiaVendorPassword = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaVendorPasswordColumn.ColumnName]);
                        string RcopiaPortalSystemName = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaPortalSystemNameColumn.ColumnName]);
                        string RcopiaPracticeUserName = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaPracticeUserNameColumn.ColumnName]);


                        var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

                        DSRcopia dsRcopia = new DSRcopia();
                        BLObject<DSRcopia> obj = new BLLRcopia().SelectGetUrls();
                        dsRcopia = obj.Data;
                        if (obj.Data != null)
                        {
                            if (dsRcopia.Rcopia_GetUrl.Rows.Count > 0)
                            {

                                var url = string.Concat(MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineUploadURLColumn.ColumnName]), "/getURL?xml=", inputdata);
                                HttpResponseMessage response = client.GetAsync(url).Result;
                                if (response != null)
                                {
                                    var getdata = response.Content.ReadAsStringAsync().Result;
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(getdata);
                                    //string jsonText = JsonConvert.SerializeXmlNode(doc);
                                    XmlNodeList nodeListuploadurl = doc.GetElementsByTagName("EngineUploadURL");
                                    XmlNodeList nodelistDownloadurl = doc.GetElementsByTagName("EngineDownloadURL");

                                    string UploadUrl = string.Empty;
                                    string downloadUrl = string.Empty;
                                    foreach (XmlNode node in nodeListuploadurl)
                                    {
                                        UploadUrl = node.InnerText;
                                    }
                                    foreach (XmlNode node in nodelistDownloadurl)
                                    {
                                        downloadUrl = node.InnerText;
                                    }



                                    //string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername,DSPatient.PatientsRow patient
                                    var updateProblemXml = MDVUtility.GetXmlForUpdateProblem(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, ProblemData);
                                    var Uploadurl = UploadUrl + "?xml=" + updateProblemXml;
                                    HttpResponseMessage ResponseUpdateProblem = client.GetAsync(Uploadurl).Result;
                                    var GetProblemUpdateData = ResponseUpdateProblem.Content.ReadAsStringAsync().Result;

                                    XmlDocument Xmldoc = new XmlDocument();
                                    Xmldoc.LoadXml(GetProblemUpdateData);

                                    string status = "";
                                    XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");


                                    foreach (XmlNode node in statusNode)
                                    {
                                        status = node.InnerText;
                                    }
                                    if (status == "ok")
                                    {

                                    }
                                    else if (status == "error")
                                    {
                                        return "error";
                                    }
                                }
                                return "";
                            }
                            else
                            {
                                throw new Exception("Error In Update Problem On DrFirst");
                            }
                        }
                        else
                        {
                            throw new Exception("Error In Update Problem On DrFirst");
                        }
                    }
                    else
                    {
                        throw new Exception("Error In Update Problem On DrFirst");
                    }
                }
                else
                {
                    throw new Exception("Error In Update Problem On DrFirst");
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

        #region "ProblemLists with Notes"

        /// <summary>
        /// this function will get latest allergy for notes attachment
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string getLatestProblemListsByPatientId(Int64 PatientId, Int64 userId, Int64 entityId)
        {
            try
            {

                DSProblemLists dsProblemListSoap = null;
                BLObject<DSProblemLists> obj;

                obj = BLLClinicalObj.getLatestProblemListByPatientId(PatientId, userId, entityId);

                dsProblemListSoap = obj.Data;
                var response = new
                {
                    status = true,
                    ProblemListSoapCount = dsProblemListSoap.Tables[dsProblemListSoap.ProblemListSoap.TableName].Rows.Count,
                    ProblemListSoap_JSON = MDVUtility.JSON_DataTable(dsProblemListSoap.Tables[dsProblemListSoap.ProblemListSoap.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public string getLatestChronicProblemListsByPatientId(Int64 PatientId, Int64 userId, Int64 entityId)
        {
            try
            {

                DSProblemLists dsProblemListSoap = null;
                BLObject<DSProblemLists> obj;

                obj = BLLClinicalObj.getLatestChronicProblemListByPatientId(PatientId, userId, entityId);

                dsProblemListSoap = obj.Data;
                var response = new
                {
                    status = true,
                    ProblemListSoapCount = dsProblemListSoap.Tables[dsProblemListSoap.ProblemListSoap.TableName].Rows.Count,
                    ProblemListSoap_JSON = MDVUtility.JSON_DataTable(dsProblemListSoap.Tables[dsProblemListSoap.ProblemListSoap.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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


        /// Author: ZeeshanAK
        /// Purpose:  to load Problemss for Batch Patient List
        /// Date : April 06, 2016
        public string getAllProblemLists(ProblemListModel model)
        {
            try
            {

                DSProblemLists dsProblemList = null;
                BLObject<DSProblemLists> obj;

                obj = BLLClinicalObj.LookupProblemListForPt(MDVUtility.ToInt32(model.PatientId), MDVUtility.ToInt32(model.ProblemListId), MDVUtility.ToStr(model.ProblemName));

                dsProblemList = obj.Data;
                var response = new
                {
                    status = true,
                    ProblemCount = dsProblemList.Tables[dsProblemList.ProblemListForPt.TableName].Rows.Count,
                    ProblemLoad_JSON = MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemListForPt.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        /// <summary>
        /// this function will retrive allergy information for Notes attachment
        /// </summary>
        /// <param name="ProblemListId"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        internal string getProblemListsForSoap(string ProblemListId)
        {
            try
            {

                DSProblemLists dsProblemListSoap = null;
                BLObject<DSProblemLists> obj = BLLClinicalObj.loadProblemListsForSoap(ProblemListId);
                dsProblemListSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsProblemListSoap.Tables[dsProblemListSoap.ProblemListSoap.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ProblemListSoapCount = dsProblemListSoap.Tables[dsProblemListSoap.ProblemListSoap.TableName].Rows.Count,
                            ProblemListSoap_JSON = MDVUtility.JSON_DataTable(dsProblemListSoap.Tables[dsProblemListSoap.ProblemListSoap.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ProblemListSoapCount = 0,
                            ProblemListSoap_JSON = MDVUtility.JSON_DataTable(dsProblemListSoap.Tables[dsProblemListSoap.ProblemListSoap.TableName]),
                            Message = Common.AppPrivileges.No_Record_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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

        internal string getpreviousProblemListsForSoap(string ProblemListId)
        {
            try
            {

                List<MDVision.Model.Clinical.Notes.ProblemsListModel> PreviousProblemsListForSoap = null;
                BLObject<List<MDVision.Model.Clinical.Notes.ProblemsListModel>> obj;

                obj = BLLClinicalObj.loadPreviousProblemListsForSoap(ProblemListId);
                PreviousProblemsListForSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (PreviousProblemsListForSoap.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ProblemListSoapCount = PreviousProblemsListForSoap.Count,
                            ProblemListSoap_JSON = PreviousProblemsListForSoap,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ProblemListSoapCount = 0,
                            ProblemListSoap_JSON = PreviousProblemsListForSoap,
                            Message = Common.AppPrivileges.No_Record_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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

        /// <summary>
        /// This Function will detach Vital sign from notes
        /// </summary>
        /// <param name="VitalSignId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string detach_ProblemList_From_Notes(string ProblemListId, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProblemListId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachProblemListFromNotes(ProblemListId, NotesId);
                    if (obj.Data == "")
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

        /// <summary>
        /// This Function will attach vital sign to notes
        /// </summary>
        /// <param name="VitalSignId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string attach_ProblemList_With_Notes(string ProblemListId, long NotesId)
        {
            try
            {
                DSProblemLists dsProblemList = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProblemListId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<DSProblemLists> obj = BLLClinicalObj.attachProblemListWithNotes(ProblemListId, NotesId);
                    if (obj.Data != null)
                    {
                        dsProblemList = obj.Data;
                        var response = new
                        {
                            status = true,
                            ProblemListTotalCount = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count,
                            ProblemListCount = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count,
                            ProblemListLoad_JSON = MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemList.TableName]),
                            ProblemListHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemHistory.TableName]),
                            Message = Common.AppPrivileges.Update_Message
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
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

        #endregion

        //public string getRcopiaResponseUrl(string ModuleName, DataRow RowtData, string ID, SharedVariable sharedVariable = null, HttpContext hCtrl = null, List<OS_MedicationModel> MedicationModel = null, string ProviderUsername = "", long PatientId = 0)
        //{
        //    // DSPatient.PatientsRow PatientData
        //    int count = 0;
        //    int ANS1count = 0;
        //    int ANS2count = 0;

        //    dynamic Errorresponse = "";

        //    for (int i = 0; i < 3; i++)
        //    {

        //        count++;
        //        Errorresponse = JObject.Parse(UploadURLS(ModuleName, RowtData, ID, count, sharedVariable, hCtrl, MedicationModel, ProviderUsername, PatientId));
        //        if (Errorresponse.Rcopia != "")
        //        {

        //            break;
        //        }
        //        else if (Errorresponse.Error == "error")
        //        {
        //            MDVLogger.RcopiaLogMessage("Error: Send_Patient", "", "", "", MDVUtility.ToStr(Errorresponse.Error), count);
        //            break;
        //        }
        //        else
        //        {
        //            MDVLogger.RcopiaLogMessage("Error: Send_Patient", "", "", "", MDVUtility.ToStr(Errorresponse.exception), count);
        //        }
        //        int milliseconds = 30000;
        //        Thread.Sleep(milliseconds);
        //    }
        //    if (count > 2)
        //    {
        //        for (int j = 0; j < 3; j++)
        //        {
        //            ANS1count++;
        //            Errorresponse = JObject.Parse(ANS1response(ModuleName, RowtData, ID, ANS1count, sharedVariable, hCtrl, MedicationModel, ProviderUsername, PatientId));
        //            if (Errorresponse.Rcopia != "")
        //            {
        //                break;
        //            }
        //            if (Errorresponse.exception == "DrFirst ANS cannot be called within 10 minutes,Please retry later")
        //            {
        //                MDVLogger.RcopiaLogMessage("Error", "", "", "Using ANS1", MDVUtility.ToStr(Errorresponse.exception), ANS1count);
        //                break;
        //            }
        //            else if (Errorresponse.Error == "error")
        //            {
        //                MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS1", MDVUtility.ToStr(Errorresponse.Error), ANS1count);

        //                break;
        //            }
        //            else
        //            {
        //                MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS1", MDVUtility.ToStr(Errorresponse.exception), ANS1count);
        //            }
        //            int milliseconds = 30000;
        //            Thread.Sleep(milliseconds);
        //        }
        //    }
        //    if (ANS1count > 2)
        //    {

        //        Errorresponse = JObject.Parse(ANS2response(ModuleName, RowtData, ID, sharedVariable, hCtrl, MedicationModel, ProviderUsername, PatientId));
        //        if (Errorresponse.Rcopia != "")
        //        {

        //        }
        //        else
        //        {
        //            MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS2", MDVUtility.ToStr(Errorresponse.exception));
        //        }

        //    }
        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(Errorresponse));


        //}

        //public string UploadURLS(string ModuleName, DataRow RowData, string ID, int count, SharedVariable sharedVariable = null, HttpContext httpCtrl = null, List<OS_MedicationModel> MedicationModel = null, string ProviderUsername = "", long PatientId = 0)
        //{
        //    string ErrorUpDownloadUrls = string.Empty;
        //    try
        //    {
        //        if (httpCtrl == null)
        //        {
        //            httpCtrl = HttpContext.Current;
        //        }
        //        DSRcopia dsRcopia = new DSRcopia();
        //        RcopiaModel model = new RcopiaModel();
        //        RcopiaHelper ObjRcopiaHelper = new RcopiaHelper();
        //        List<RcopiaModel> ListRcopia = ObjRcopiaHelper.GetRcopiaInfo(sharedVariable);
        //        string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
        //        string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
        //        string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
        //        string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
        //        string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
        //        string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;
        //        HttpClient client = new HttpClient();
        //        //client.BaseAddress = new Uri("http://localhost:8080/");
        //        string error = string.Empty;
        //        client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/xml"));
        //        BLObject<DSRcopia> obj = BLLRcopiaObj.SelectGetUrls(sharedVariable);
        //        dsRcopia = obj.Data;
        //        model.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);
        //        model.EngineUploadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineUploadURLColumn.ColumnName]);
        //        string UploadUrl = model.EngineUploadURL;
        //        string DownloadURL = model.EngineDownloadURL;
        //        var upload = string.Empty;
        //        var xml = string.Empty;
        //        var DownloadUrl = string.Empty;
        //        if (ModuleName == "Patient")
        //        {
        //            upload = MDVUtility.GetXmlForAddPatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
        //            xml = httpCtrl.Server.UrlEncode(upload);
        //        }
        //        else if (ModuleName == "Problem")
        //        {
        //            upload = MDVUtility.GetXmlForAddProblemList(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
        //            xml = httpCtrl.Server.UrlEncode(upload);
        //        }
        //        else if (ModuleName == "UpdatePatient")
        //        {
        //            upload = MDVUtility.GetXmlForUpdatePatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
        //            xml = httpCtrl.Server.UrlEncode(upload);
        //        }
        //        else if (ModuleName == "UpdateProblem")
        //        {
        //            upload = MDVUtility.GetXmlForUpdateProblem(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
        //            xml = upload;
        //        }
        //        else if (ModuleName == "Medication")
        //        {
        //            upload = MDVUtility.GetXmlForUpdateMedication(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
        //            xml = httpCtrl.Server.UrlEncode(upload);
        //        }
        //        else if (ModuleName == "MedicationDelete")
        //        {
        //            upload = MDVUtility.GetXmlForUpdateMedicationDelete(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
        //            xml = httpCtrl.Server.UrlEncode(upload);
        //        }

        //        MDVLogger.RcopiaLogMessage("Request: Send_" + ModuleName, ID, "", UploadUrl + "?xml=" + xml, "", count);

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UploadUrl);
        //        request.Method = "POST";
        //        request.ContentType = "application/xml";
        //        request.Accept = "application/xml";
        //        xml = "xml=" + xml;
        //        byte[] bytes = Encoding.UTF8.GetBytes(xml);
        //        request.ContentLength = bytes.Length;
        //        using (Stream putStream = request.GetRequestStream())
        //        {
        //            putStream.Write(bytes, 0, bytes.Length);

        //        }

        //        var GetPatientDataANS1 = string.Empty;
        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            using (Stream stream = response.GetResponseStream())
        //            {
        //                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        //                GetPatientDataANS1 = reader.ReadToEnd();
        //            }
        //        }

        //        //HttpResponseMessage ResponseUploadPatientANS1 = client.GetAsync(Uploadurl).Result;
        //        //var GetPatientDataANS1 = ResponseUploadPatientANS1.Content.ReadAsStringAsync().Result;
        //        if (GetPatientDataANS1 != string.Empty)
        //        {
        //            XmlDocument Xmldoc = new XmlDocument();
        //            Xmldoc.LoadXml(GetPatientDataANS1);


        //            string status = "";
        //            string Problemstatus = "";
        //            XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");
        //            XmlNodeList statusNodeerror = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status/Error");


        //            foreach (XmlNode node in statusNode)
        //            {
        //                status = node.InnerText;
        //            }
        //            foreach (XmlNode noderror in statusNodeerror)
        //            {
        //                error = noderror.InnerText;
        //            }
        //            XmlNodeList statusNodeInProblem = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/ProblemList/Problem/Status");
        //            foreach (XmlNode node in statusNodeInProblem)
        //            {
        //                Problemstatus = node.InnerText;
        //            }
        //            if (status == "ok" && Problemstatus != "error")
        //            {
        //                string RcopiaID = "";
        //                XmlNodeList RcopialNode = Xmldoc.GetElementsByTagName("RcopiaID");
        //                foreach (XmlNode node in RcopialNode)
        //                {
        //                    RcopiaID = node.InnerText;
        //                }
        //                MDVLogger.RcopiaLogMessage("Response: Send_" + ModuleName, ID, status, GetPatientDataANS1, "", count);
        //                var response = new
        //                {
        //                    Rcopia = RcopiaID,
        //                    Error = "",
        //                    exception = ""
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

        //            }
        //            else if (status == "error" || Problemstatus == "error")
        //            {
        //                var response = new
        //                {
        //                    Rcopia = "",
        //                    Error = "error",
        //                    exception = ""
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorUpDownloadUrls = ex.Message;
        //    }
        //    var response1 = new
        //    {
        //        Rcopia = "",
        //        Error = "",
        //        exception = ErrorUpDownloadUrls
        //    };
        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
        //}
        //public string ANS1response(string ModuleName, DataRow RowData, string ID, int count, SharedVariable sharedVariable = null, HttpContext httpCtrl = null, List<OS_MedicationModel> MedicationModel = null, string ProviderUsername = "", long PatientId = 0)
        //{
        //    DateTime Modified;
        //    string exception = "DrFirst ANS cannot be called within 10 minutes,Please retry later";
        //    try
        //    {
        //        if (httpCtrl == null)
        //        {
        //            httpCtrl = HttpContext.Current;
        //        }
        //        DSRcopia dsRcopia = new DSRcopia();
        //        RcopiaModel model = new RcopiaModel();
        //        RcopiaHelper ObjRcopiaHelper = new RcopiaHelper();
        //        List<RcopiaModel> ListRcopia = ObjRcopiaHelper.GetRcopiaInfo(sharedVariable);
        //        string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
        //        string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
        //        string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
        //        string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
        //        string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
        //        string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;

        //        BLObject<DSRcopia> objGetUrl = BLLRcopiaObj.SelectGetUrls(sharedVariable);
        //        dsRcopia = objGetUrl.Data;
        //        Modified = MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.ModifiedOnColumn.ColumnName]);
        //        int minutes = Convert.ToInt32(DateTime.Now.Subtract(Modified).TotalMinutes);
        //        if (minutes > 10)
        //        {

        //            BLObject<DSRcopia> obj = BLLRcopiaObj.SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
        //            HttpClient client = new HttpClient();
        //            dsRcopia = obj.Data;
        //            model.RcopiaANS = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaANSColumn.ColumnName]);
        //            string RcopiaANS = model.RcopiaANS;


        //            string error = string.Empty;
        //            client.DefaultRequestHeaders.Accept.Add(
        //                   new MediaTypeWithQualityHeaderValue("application/xml"));
        //            var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

        //            var ANS1url = RcopiaANS + "?xml=" + inputdata;
        //            HttpResponseMessage ResponseUploadPatient = client.GetAsync(ANS1url).Result;

        //            if (ResponseUploadPatient != null)
        //            {
        //                var GetdataANS1 = ResponseUploadPatient.Content.ReadAsStringAsync().Result;
        //                XmlDocument DocANS1 = new XmlDocument();
        //                DocANS1.LoadXml(GetdataANS1);
        //                XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
        //                XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
        //                XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
        //                string UploadUrlANS1 = string.Empty;
        //                string downloadUrlANS1 = string.Empty;
        //                string WebBrowserURLANS1 = string.Empty;
        //                foreach (XmlNode node in nodelistWebBrowserURLANS1)
        //                {
        //                    WebBrowserURLANS1 = node.InnerText;
        //                }
        //                foreach (XmlNode node in nodeListuploadurlANS1)
        //                {
        //                    UploadUrlANS1 = node.InnerText;
        //                }
        //                foreach (XmlNode node in nodelistDownloadurlANS1)
        //                {
        //                    downloadUrlANS1 = node.InnerText;
        //                }

        //                var uploadPatientANS1 = string.Empty;
        //                var UploadANS1URL = string.Empty;
        //                var xml = string.Empty;
        //                if (ModuleName == "Patient")
        //                {
        //                    uploadPatientANS1 = MDVUtility.GetXmlForAddPatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
        //                    xml = httpCtrl.Server.UrlEncode(uploadPatientANS1);
        //                }
        //                else if (ModuleName == "Problem")
        //                {
        //                    uploadPatientANS1 = MDVUtility.GetXmlForAddProblemList(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
        //                    xml = httpCtrl.Server.UrlEncode(uploadPatientANS1);
        //                }
        //                else if (ModuleName == "UpdatePatient")
        //                {
        //                    uploadPatientANS1 = MDVUtility.GetXmlForUpdatePatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
        //                    xml = uploadPatientANS1;
        //                }
        //                else if (ModuleName == "UpdateProblem")
        //                {
        //                    uploadPatientANS1 = MDVUtility.GetXmlForUpdateProblem(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
        //                    xml = uploadPatientANS1;
        //                }
        //                else if (ModuleName == "Medication")
        //                {
        //                    uploadPatientANS1 = MDVUtility.GetXmlForUpdateMedication(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
        //                    xml = httpCtrl.Server.UrlEncode(uploadPatientANS1);
        //                }
        //                else if (ModuleName == "MedicationDelete")
        //                {
        //                    uploadPatientANS1 = MDVUtility.GetXmlForUpdateMedicationDelete(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
        //                    xml = httpCtrl.Server.UrlEncode(uploadPatientANS1);
        //                }
        //                //UploadUrlANS1
        //                //uploadPatientANS1 = MDVUtility.GetXmlForAddPatient("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", RowData,ID);
        //                //UploadANS1 = UploadUrlANS1 + "/getURL?xml=" + httpCtrl.Server.UrlEncode(uploadPatientANS1);
        //                MDVLogger.RcopiaLogMessage("Request: Send_Patient", ID, "", UploadUrlANS1 + "?xml=" + xml, "", count);

        //                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UploadUrlANS1);
        //                request.Method = "POST";
        //                request.ContentType = "application/xml";
        //                request.Accept = "application/xml";
        //                xml = "xml=" + xml;
        //                byte[] bytes = Encoding.UTF8.GetBytes(xml);
        //                request.ContentLength = bytes.Length;
        //                using (Stream putStream = request.GetRequestStream())
        //                {
        //                    putStream.Write(bytes, 0, bytes.Length);

        //                }

        //                var GetPatientDataANS1 = string.Empty;
        //                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //                {
        //                    using (Stream stream = response.GetResponseStream())
        //                    {
        //                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        //                        GetPatientDataANS1 = reader.ReadToEnd();
        //                    }
        //                }

        //                //HttpResponseMessage ResponseUploadPatientANS1 = client.GetAsync(UploadANS1URL).Result;
        //                //var GetPatientDataANS1 = ResponseUploadPatientANS1.Content.ReadAsStringAsync().Result;
        //                if (GetPatientDataANS1 != string.Empty)
        //                {
        //                    model.URLID = 1;
        //                    model.EngineDownloadURL = downloadUrlANS1;
        //                    model.EngineUploadURL = UploadUrlANS1;
        //                    model.WebBrowserURL = WebBrowserURLANS1;
        //                    model.CreatedOn = DateTime.Now;
        //                    model.ModifiedOn = DateTime.Now;
        //                    model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                    model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                    dsRcopia = new DSRcopia();
        //                    DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
        //                    dr.UrlID = model.URLID;
        //                    dr.EngineDownloadURL = model.EngineDownloadURL;
        //                    dr.EngineUploadURL = model.EngineUploadURL;
        //                    dr.WebBrowserURL = model.WebBrowserURL;
        //                    dr.IsActive = true;
        //                    dr.CreatedOn = model.CreatedOn;
        //                    dr.ModifiedOn = model.ModifiedOn;
        //                    dr.CreatedBy = model.CreatedBy;
        //                    dr.ModifiedBy = model.ModifiedBy;
        //                    dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);

        //                    BLObject<DSRcopia> objMofieddate = BLLRcopiaObj.UpDateGetUrl(dsRcopia, sharedVariable);
        //                    dsRcopia = objMofieddate.Data;


        //                    XmlDocument Xmldoc = new XmlDocument();
        //                    Xmldoc.LoadXml(GetPatientDataANS1);
        //                    exception = string.Empty;

        //                    string status = "";
        //                    XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");
        //                    XmlNodeList statusNodeerror = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status/Error");

        //                    foreach (XmlNode node in statusNode)
        //                    {
        //                        status = node.InnerText;
        //                    }
        //                    foreach (XmlNode noderror in statusNodeerror)
        //                    {
        //                        error = noderror.InnerText;
        //                    }
        //                    string Problemstatus = "";
        //                    XmlNodeList statusNodeInProblem = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/ProblemList/Problem/Status");
        //                    foreach (XmlNode node in statusNodeInProblem)
        //                    {
        //                        Problemstatus = node.InnerText;
        //                    }
        //                    if (status == "ok" && Problemstatus != "error")
        //                    {
        //                        string RcopiaID = "";
        //                        XmlNodeList RcopialNode = Xmldoc.GetElementsByTagName("RcopiaID");
        //                        foreach (XmlNode node in RcopialNode)
        //                        {
        //                            RcopiaID = node.InnerText;
        //                        }
        //                        MDVLogger.RcopiaLogMessage("Response: Send_Patient", ID, status, GetPatientDataANS1, "", count);
        //                        var response = new
        //                        {
        //                            Rcopia = RcopiaID,
        //                            Error = "",
        //                            exception = ""
        //                        };
        //                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //                    }
        //                    else if (status == "error" || Problemstatus == "error")
        //                    {

        //                        var response = new
        //                        {
        //                            Rcopia = "",
        //                            Error = "error",
        //                            exception = ""
        //                        };
        //                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

        //                    }


        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex.Message;
        //    }
        //    var response1 = new
        //    {
        //        Rcopia = "",
        //        Error = "",
        //        exception = exception
        //    };
        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
        //}
        //public string ANS2response(string ModuleName, DataRow RowData, string ID, SharedVariable sharedVariable = null, HttpContext httpCtrl = null, List<OS_MedicationModel> MedicationModel = null, string ProviderUsername = "", long PatientId = 0)
        //{
        //    string exception = string.Empty;
        //    try
        //    {
        //        if (httpCtrl == null)
        //        {
        //            httpCtrl = HttpContext.Current;
        //        }
        //        DSRcopia dsRcopia = new DSRcopia();
        //        BLObject<DSRcopia> obj = BLLRcopiaObj.SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
        //        dsRcopia = obj.Data;
        //        HttpClient client = new HttpClient();
        //        RcopiaModel model = new RcopiaModel();
        //        RcopiaHelper ObjRcopiaHelper = new RcopiaHelper();
        //        List<RcopiaModel> ListRcopia = ObjRcopiaHelper.GetRcopiaInfo(sharedVariable);
        //        string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
        //        string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
        //        string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
        //        string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
        //        string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
        //        string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;
        //        //string RcopiaANS = RcopiaANSbackup;

        //        string error = string.Empty;
        //        client.DefaultRequestHeaders.Accept.Add(
        //               new MediaTypeWithQualityHeaderValue("application/xml"));
        //        var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

        //        var ANS1url = RcopiaANSbackup + "?xml=" + inputdata;
        //        MDVLogger.RcopiaLogMessage("Request: GET URL from ANS2", ID, "", ANS1url);
        //        HttpResponseMessage ResponseUploadPatient = client.GetAsync(ANS1url).Result;
        //        MDVLogger.RcopiaLogMessage("Response: GET URL  from ANS2", ID, "", ResponseUploadPatient.ToString());
        //        if (ResponseUploadPatient != null)
        //        {
        //            var GetdataANS1 = ResponseUploadPatient.Content.ReadAsStringAsync().Result;
        //            XmlDocument DocANS1 = new XmlDocument();
        //            DocANS1.LoadXml(GetdataANS1);
        //            XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
        //            XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
        //            XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
        //            string UploadUrlANS2 = string.Empty;
        //            string downloadUrlANS2 = string.Empty;
        //            string WebBrowserURLANS2 = string.Empty;
        //            foreach (XmlNode node in nodelistWebBrowserURLANS1)
        //            {
        //                WebBrowserURLANS2 = node.InnerText;
        //            }
        //            foreach (XmlNode node in nodeListuploadurlANS1)
        //            {
        //                UploadUrlANS2 = node.InnerText;
        //            }
        //            foreach (XmlNode node in nodelistDownloadurlANS1)
        //            {
        //                downloadUrlANS2 = node.InnerText;
        //            }


        //            var uploadPatientANS2 = string.Empty;
        //            var UploadANS1URL = string.Empty;
        //            var xml = string.Empty;
        //            if (ModuleName == "Patient")
        //            {
        //                uploadPatientANS2 = MDVUtility.GetXmlForAddPatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
        //                xml = httpCtrl.Server.UrlEncode(uploadPatientANS2);
        //            }
        //            else if (ModuleName == "Problem")
        //            {
        //                uploadPatientANS2 = MDVUtility.GetXmlForAddProblemList(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData, ID);
        //                xml = httpCtrl.Server.UrlEncode(uploadPatientANS2);
        //            }
        //            else if (ModuleName == "UpdatePatient")
        //            {
        //                uploadPatientANS2 = MDVUtility.GetXmlForUpdatePatient(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
        //                xml = uploadPatientANS2;
        //            }
        //            else if (ModuleName == "UpdateProblem")
        //            {
        //                uploadPatientANS2 = MDVUtility.GetXmlForUpdateProblem(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, RowData);
        //                xml = uploadPatientANS2;
        //            }
        //            else if (ModuleName == "Medication")
        //            {
        //                uploadPatientANS2 = MDVUtility.GetXmlForUpdateMedication(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
        //                xml = httpCtrl.Server.UrlEncode(uploadPatientANS2);
        //            }
        //            else if (ModuleName == "MedicationDelete")
        //            {
        //                uploadPatientANS2 = MDVUtility.GetXmlForUpdateMedicationDelete(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, MedicationModel, ProviderUsername, PatientId);
        //                xml = httpCtrl.Server.UrlEncode(uploadPatientANS2);
        //            }
        //            //
        //            //var uploadPatientANS1 = MDVUtility.GetXmlForAddPatient("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", RowData,ID);
        //            //var UploadANS2 = UploadUrlANS2 + "/getURL?xml=" + HttpContext.Current.Server.UrlEncode(uploadPatientANS1);
        //            MDVLogger.RcopiaLogMessage("Request:" + ModuleName, ID, "", UploadUrlANS2 + "?xml=" + xml);


        //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UploadUrlANS2);
        //            request.Method = "POST";
        //            request.ContentType = "application/xml";
        //            request.Accept = "application/xml";
        //            xml = "xml=" + xml;
        //            byte[] bytes = Encoding.UTF8.GetBytes(xml);
        //            request.ContentLength = bytes.Length;
        //            using (Stream putStream = request.GetRequestStream())
        //            {
        //                putStream.Write(bytes, 0, bytes.Length);

        //            }

        //            var GetPatientDataANS1 = string.Empty;
        //            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //            {
        //                using (Stream stream = response.GetResponseStream())
        //                {
        //                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        //                    GetPatientDataANS1 = reader.ReadToEnd();
        //                }
        //            }
        //            if (GetPatientDataANS1 != string.Empty)
        //            {
        //                model.URLID = 1;
        //                model.EngineDownloadURL = downloadUrlANS2;
        //                model.EngineUploadURL = UploadUrlANS2;
        //                model.WebBrowserURL = WebBrowserURLANS2;
        //                model.CreatedOn = DateTime.Now;
        //                model.ModifiedOn = DateTime.Now;
        //                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                dsRcopia = new DSRcopia();
        //                DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
        //                dr.UrlID = model.URLID;
        //                dr.EngineDownloadURL = model.EngineDownloadURL;
        //                dr.EngineUploadURL = model.EngineUploadURL;
        //                dr.WebBrowserURL = model.WebBrowserURL;
        //                dr.IsActive = true;
        //                dr.CreatedOn = model.CreatedOn;
        //                dr.ModifiedOn = model.ModifiedOn;
        //                dr.CreatedBy = model.CreatedBy;
        //                dr.ModifiedBy = model.ModifiedBy;
        //                dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);

        //                BLObject<DSRcopia> objMofieddate = BLLRcopiaObj.UpDateGetUrl(dsRcopia, sharedVariable);

        //                XmlDocument Xmldoc = new XmlDocument();
        //                Xmldoc.LoadXml(GetPatientDataANS1);


        //                string status = "";
        //                XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");
        //                XmlNodeList statusNodeerror = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status/Error");

        //                foreach (XmlNode node in statusNode)
        //                {
        //                    status = node.InnerText;
        //                }
        //                foreach (XmlNode noderror in statusNodeerror)
        //                {
        //                    error = noderror.InnerText;
        //                }
        //                string Problemstatus = "";
        //                XmlNodeList statusNodeInProblem = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/ProblemList/Problem/Status");
        //                foreach (XmlNode node in statusNodeInProblem)
        //                {
        //                    Problemstatus = node.InnerText;
        //                }
        //                if (status == "ok" && Problemstatus != "error")
        //                {
        //                    string RcopiaID = "";
        //                    XmlNodeList RcopialNode = Xmldoc.GetElementsByTagName("RcopiaID");
        //                    foreach (XmlNode node in RcopialNode)
        //                    {
        //                        RcopiaID = node.InnerText;
        //                    }
        //                    MDVLogger.RcopiaLogMessage("Response:" + ModuleName, ID, status, GetPatientDataANS1, "");
        //                    var response = new
        //                    {
        //                        Rcopia = RcopiaID,
        //                        Error = "",
        //                        exception = ""
        //                    };
        //                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //                }
        //                else if (status == "error" || Problemstatus == "error")
        //                {

        //                    var response = new
        //                    {
        //                        Rcopia = "",
        //                        Error = "error",
        //                        exception = ""
        //                    };
        //                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

        //                }


        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex.Message;
        //    }
        //    var response1 = new
        //    {
        //        Rcopia = "",
        //        Error = "",
        //        exception = exception
        //    };
        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
        //}




        // Author: Talha Tanweer
        // Purpose:  to load Problems for Clinical Reports
        // Date : August 30, 2016
        internal string getAllProblemsforReports()
        {
            try
            {
                List<ProblemLookUpModel> modelList = null;
                BLObject<List<ProblemLookUpModel>> obj;

                obj = BLLClinicalObj.getAllProblemsforReports();

                modelList = obj.Data;
                var ChronicityLevelList = modelList.GroupBy(x => x.ChronicityLevel).Select(y => y.First()).Distinct();
                var SeverityList = modelList.GroupBy(x => x.Severity).Select(y => y.First()).Distinct();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var response = new
                {
                    status = true,
                    ChronicityLevelList = js.Serialize(ChronicityLevelList),
                    SeverityList = js.Serialize(SeverityList),
                    ProblemsCount = modelList.Count
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        internal string GetProblemChronicityLookup()
        {
            try
            {

                List<ChronicityLookupModel> modelList = null;
                BLObject<List<ChronicityLookupModel>> obj;

                obj = BLLClinicalObj.getProblemChronicityLookup();

                modelList = obj.Data;
                var chronicityList = modelList.GroupBy(x => x.ShortName).Select(y => y.First()).Distinct();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var response = new
                {
                    status = true,
                    ChronicityList = js.Serialize(chronicityList),
                    ChronicityCount = modelList.Count
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        internal string GetProblemSeverityLookup()
        {
            try
            {

                List<SeverityLookupModel> modelList = null;
                BLObject<List<SeverityLookupModel>> obj;

                obj = BLLClinicalObj.getProblemSeverityLookup();

                modelList = obj.Data;
                var severityList = modelList.GroupBy(x => x.ShortName).Select(y => y.First()).Distinct();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var response = new
                {
                    status = true,
                    SeverityList = js.Serialize(severityList),
                    severityCount = modelList.Count
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public string LogProblemListView(ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {
                    BLObject<DSProblemLists> obj = BLLClinicalObj.LoadProblemListsForFillData(MDVUtility.ToInt64(model.ProblemListId), "1");

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Problem not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        public string UpdateProblemsOrder(ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.FromElementId) > 0 || MDVUtility.ToInt64(model.ToElementId) > 0)
                {
                    BLObject<DSProblemLists> objPatientProblemFrom;
                    BLObject<DSProblemLists> objPatientProblemTo;

                    objPatientProblemFrom = BLLClinicalObj.LoadProblemListsForFillData(MDVUtility.ToInt64(model.FromElementId));
                    objPatientProblemTo = BLLClinicalObj.LoadProblemListsForFillData(MDVUtility.ToInt64(model.ToElementId));
                    if (objPatientProblemFrom.Data != null && objPatientProblemTo.Data != null)
                    {
                        DSProblemLists mergedDataSet = new DSProblemLists();
                        mergedDataSet.Merge(objPatientProblemFrom.Data);
                        mergedDataSet.Merge(objPatientProblemTo.Data);

                        int from_sortId = MDVUtility.ToInt(mergedDataSet.Tables[mergedDataSet.ProblemList.TableName].Rows[0][mergedDataSet.ProblemList.ProblemOrderColumn.ColumnName]);
                        int to_sortId = MDVUtility.ToInt(mergedDataSet.Tables[mergedDataSet.ProblemList.TableName].Rows[1][mergedDataSet.ProblemList.ProblemOrderColumn.ColumnName]);

                        mergedDataSet.Tables[mergedDataSet.ProblemList.TableName].Rows[0][mergedDataSet.ProblemList.ProblemOrderColumn.ColumnName] = to_sortId;
                        mergedDataSet.Tables[mergedDataSet.ProblemList.TableName].Rows[0][mergedDataSet.ProblemList.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        mergedDataSet.Tables[mergedDataSet.ProblemList.TableName].Rows[0][mergedDataSet.ProblemList.ModifiedOnColumn.ColumnName] = DateTime.Now;

                        mergedDataSet.Tables[mergedDataSet.ProblemList.TableName].Rows[1][mergedDataSet.ProblemList.ProblemOrderColumn.ColumnName] = from_sortId;
                        mergedDataSet.Tables[mergedDataSet.ProblemList.TableName].Rows[1][mergedDataSet.ProblemList.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        mergedDataSet.Tables[mergedDataSet.ProblemList.TableName].Rows[1][mergedDataSet.ProblemList.ModifiedOnColumn.ColumnName] = DateTime.Now;

                        //if (isDrFirstRequired == true)
                        //{
                        //    #region "ActiveInActiveProblemList in Drfirst"
                        //    foreach (DSProblemLists.ProblemListRow dr in mergedDataSet.Tables[dsProblemList.ProblemList.TableName].Rows)
                        //    {
                        //        UpdateProblemStatusInDrFirst(dr);
                        //    }
                        //    #endregion
                        //}

                        #region Database Updation
                        if (mergedDataSet.Tables[mergedDataSet.ProblemList.TableName].Rows.Count > 0)
                        {
                            BLObject<DSProblemLists> objUpdate = BLLClinicalObj.UpdateProblemListsOrder(mergedDataSet);
                            if (objUpdate.Data != null)
                            {
                                var response = new
                                {
                                    status = true,
                                    message = Common.AppPrivileges.Update_Message
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    message = objUpdate.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = ""
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        #endregion}
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = "Problem not found."
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Problem not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        public string attachProblemWithNotesAndLoadSOAP(string ProblemListId, long NotesId)
        {
            try
            {
                DSProblemLists dsProblemList = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProblemListId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
                {
                    ClinicalNotesHelper.Instance().SaveNoteSessionData(NotesId, "ProblemList", MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message));
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<DSProblemLists> obj = BLLClinicalObj.attachProblemWithNotesAndLoadSOAP(ProblemListId, NotesId);

                    if (obj.Data != null)
                    {
                        dsProblemList = obj.Data;
                        var response = new
                        {
                            status = true,
                            ProblemListSoapCount = obj.Data != null && dsProblemList.Tables[dsProblemList.ProblemListSoap.TableName].Rows.Count > 0 ? dsProblemList.Tables[dsProblemList.ProblemListSoap.TableName].Rows.Count : 0,
                            ProblemListSoap_JSON = obj.Data != null && dsProblemList.Tables[dsProblemList.ProblemListSoap.TableName].Rows.Count > 0 ? MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemListSoap.TableName]) : "[]",
                            Message = Common.AppPrivileges.Update_Message
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        ClinicalNotesHelper.Instance().SaveNoteSessionData(NotesId, "ProblemList", obj.Message);
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
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

    }
}