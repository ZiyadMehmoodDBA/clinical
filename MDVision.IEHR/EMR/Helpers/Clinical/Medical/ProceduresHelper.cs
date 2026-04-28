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
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using System.Threading;
using MDVision.IEHR.Common;
using Newtonsoft.Json.Linq;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.Procedures;
using Newtonsoft.Json;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;
using MDVision.Model.Clinical.Medical;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
{
    public class ProceduresHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public ProceduresHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static ProceduresHelper _instance = null;
        public static ProceduresHelper Instance()
        {
            if (_instance == null)
                _instance = new ProceduresHelper();
            return _instance;
        }
        public string loadProcedures_Obsolete(ProceduresModel model)
        {
            try
            {
                DSProcedures dsProcedures = null;
                BLObject<DSProcedures> obj = null;
                //Start 27-10-2016 Humaira Yousaf to log view action for problem lists
                if (MDVUtility.ToInt64(model.ProcedureId) > 0)
                {
                    obj = BLLClinicalObj.loadProcedures_Obsolete(MDVUtility.ToInt32(model.ProcedureId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NotesId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(0), MDVUtility.ToInt32(0), "1", "0");

                }
                else if (model.procedureDetailModel != null && model.procedureDetailModel.Count > 0)
                {
                    obj = BLLClinicalObj.loadProcedures_Obsolete(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].PatientId), MDVUtility.ToInt64(model.procedureDetailModel[0].NotesId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.procedureDetailModel[0].PageNumber), MDVUtility.ToInt32(model.procedureDetailModel[0].RowsPerPage), "", "0", model.ShowEMCodes);
                }
                else
                {
                    obj = BLLClinicalObj.loadProcedures_Obsolete(MDVUtility.ToInt32(model.ProcedureId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NotesId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(0), MDVUtility.ToInt32(0), "", "0");
                }
                //End 27-10-2016 Humaira Yousaf to log view action for problem lists
                dsProcedures = obj.Data;
                if (obj.Data != null)
                {
                    int ProcedureCount = 0;
                    //if (model.procedureDetailModel != null && model.procedureDetailModel.Count > 0 && model.procedureDetailModel[0].IsActive.Equals("1"))
                    if (dsProcedures.Tables[dsProcedures.Procedures.TableName].Rows.Count == 0)
                    {
                        //if (model.procedureDetailModel[0].IsActive.Equals("1"))
                        if (model.procedureDetailModel != null && model.procedureDetailModel.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(model.procedureDetailModel[0].IsActive) && model.procedureDetailModel[0].IsActive.Equals("1"))
                            {
                                obj = BLLClinicalObj.loadProcedures_Obsolete(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].PatientId), MDVUtility.ToInt64(model.procedureDetailModel[0].NotesId), "0", "0", 1, 1000, "", "", model.ShowEMCodes);
                            }
                        }
                        else
                        {
                            obj = BLLClinicalObj.loadProcedures_Obsolete(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].PatientId), MDVUtility.ToInt64(model.procedureDetailModel[0].NotesId), "0", "1");
                        }

                        if (obj.Data != null)
                        {
                            DSProcedures dsProceduresInActive = obj.Data;
                            ProcedureCount = dsProceduresInActive.Tables[dsProceduresInActive.Procedures.TableName].Rows.Count;
                        }
                    }
                    else
                    {
                        ProcedureCount = dsProcedures.Tables[dsProcedures.Procedures.TableName].Rows.Count;
                    }
                    var response = new
                    {
                        status = true,
                        ProcedureTotalCount = ProcedureCount,
                        ProcedureCount = dsProcedures.Tables[dsProcedures.Procedures.TableName].Rows.Count,
                        ProcedureLoad_JSON = MDVUtility.JSON_DataTable(dsProcedures.Tables[dsProcedures.Procedures.TableName]),
                        ProcedureHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsProcedures.Tables[dsProcedures.ProcedureHistory.TableName]),
                        iTotalDisplayRecords = (dsProcedures.Procedures.Rows.Count > 0) ? dsProcedures.Procedures.Rows[0][dsProcedures.Procedures.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ProcedureCount = 0,
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

        public string loadProcedures(ProceduresModel model)
        {
            try
            {
                //  DSProcedures dsProcedures = null;
                //  BLObject<DSProcedures> obj = null;

                Tuple<List<ProcedureModel>, List<ProcedureHistoryModel>> tupleProcedure = null;
                BLObject<Tuple<List<ProcedureModel>, List<ProcedureHistoryModel>>> obj;
                List<ProcedureModel> clinicalProcedureModelList;
                List<ProcedureHistoryModel> clinicalProcedureHistoryModelList;

                if (MDVUtility.ToInt64(model.ProcedureId) > 0)
                {
                    obj = BLLClinicalObj.loadProcedures(MDVUtility.ToInt32(model.ProcedureId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NotesId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(0), MDVUtility.ToInt32(0), "1", "0");

                }
                else if (model.procedureDetailModel != null && model.procedureDetailModel.Count > 0)
                {
                    obj = BLLClinicalObj.loadProcedures(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].PatientId), MDVUtility.ToInt64(model.procedureDetailModel[0].NotesId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.procedureDetailModel[0].PageNumber), MDVUtility.ToInt32(model.procedureDetailModel[0].RowsPerPage), "", "0", model.ShowEMCodes);
                }
                else
                {
                    obj = BLLClinicalObj.loadProcedures(MDVUtility.ToInt32(model.ProcedureId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NotesId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(0), MDVUtility.ToInt32(0), "", "0");
                }

                if (obj.Data != null)
                {
                    tupleProcedure = obj.Data;
                    clinicalProcedureModelList = tupleProcedure.Item1;
                    clinicalProcedureHistoryModelList = tupleProcedure.Item2;

                    int ProcedureCount = 0;
                    if (clinicalProcedureModelList.Count == 0)
                    {
                        if (model.procedureDetailModel != null && model.procedureDetailModel.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(model.procedureDetailModel[0].IsActive) && model.procedureDetailModel[0].IsActive.Equals("1"))
                            {
                                obj = BLLClinicalObj.loadProcedures(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].PatientId), MDVUtility.ToInt64(model.procedureDetailModel[0].NotesId), "0", "0", 1, 1000, "", "", model.ShowEMCodes);
                            }
                        }
                        else
                        {
                            obj = BLLClinicalObj.loadProcedures(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].PatientId), MDVUtility.ToInt64(model.procedureDetailModel[0].NotesId), "0", "1");
                        }

                        if (obj.Data != null)
                        {

                            ProcedureCount = clinicalProcedureModelList.Count;
                        }
                    }
                    else
                    {
                        ProcedureCount = clinicalProcedureModelList.Count;
                    }
                    var response = new
                    {
                        status = true,
                        ProcedureTotalCount = ProcedureCount,
                        ProcedureCount = clinicalProcedureModelList.Count,
                        ProcedureLoad_JSON = JsonConvert.SerializeObject(clinicalProcedureModelList),
                        ProcedureHistoryLoad_JSON = JsonConvert.SerializeObject(clinicalProcedureHistoryModelList),
                        iTotalDisplayRecords = (clinicalProcedureModelList.Count > 0) ? MDVUtility.ToInt(clinicalProcedureModelList[0].RecordCount) : 0,
                    };
                    return (JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ProcedureCount = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
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


        //---------------------------------------------
        public string saveProcedure(ProceduresModel model)
        {
            try
            {
                DSProcedures dsprocedures = new DSProcedures();
                BLObject<DSProcedures> obj = BLLClinicalObj.saveProcedure(model);
                if (obj.Data != null)
                {
                    dsprocedures = obj.Data;
                    var response = new
                    {
                        // ProcedureId = dsprocedures.Tables[dsprocedures.Procedure.TableName].Rows[0][dsprocedures.Procedure.ProcedureIdColumn.ColumnName],
                        ProcedureCount = dsprocedures.Procedures.Rows.Count,
                        ProcedureLoad_JSON = MDVUtility.JSON_DataTable(dsprocedures.Tables[dsprocedures.Procedures.TableName]),
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
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

        public string saveProcedureForVaccine(ProceduresModel model)
        {
            try
            {
                DSProcedures dsprocedures = new DSProcedures();

                for (int i = 0; i < model.procedureDetailModel.Count; i++)
                {

                    DSProcedures.ProceduresRow procModel = dsprocedures.Procedures.NewProceduresRow();

                    procModel.ProcedureId = -i;
                    procModel[dsprocedures.Procedures.ModifierColumn] = "";
                    procModel[dsprocedures.Procedures.UnitColumn] = 1;
                    procModel[dsprocedures.Procedures.ProblemListIdColumn] = DBNull.Value;
                    procModel[dsprocedures.Procedures.SNOMEDIDColumn] = DBNull.Value;
                    procModel[dsprocedures.Procedures.SNOMED_DESCRIPTIONColumn] = DBNull.Value;
                    procModel[dsprocedures.Procedures.StartDateColumn] = DBNull.Value;
                    procModel[dsprocedures.Procedures.EndDateColumn] = DBNull.Value;
                    procModel[dsprocedures.Procedures.CommentsColumn] = DBNull.Value;

                    procModel.PatientId = MDVUtility.ToLong(model.procedureDetailModel[i].PatientId);
                    if (MDVUtility.ToLong(model.procedureDetailModel[i].NotesId) != -1)
                    {
                        procModel.NoteId = MDVUtility.ToLong(model.procedureDetailModel[i].NotesId);
                    }
                    else
                    {
                        procModel[dsprocedures.Procedures.NoteIdColumn] = DBNull.Value;
                    }
                    procModel.IsActive = true;
                    procModel[dsprocedures.Procedures.ModifierColumn] = DBNull.Value;
                    procModel.CPTCode = MDVUtility.ToStr(model.procedureDetailModel[i].CPTCode);
                    procModel.CPT_DESCRIPTION = MDVUtility.ToStr(model.procedureDetailModel[i].CPT_DESCRIPTION);
                    procModel.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    procModel.CreatedOn = DateTime.Now;
                    procModel.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    procModel.ModifiedOn = DateTime.Now;

                    dsprocedures.Procedures.AddProceduresRow(procModel);
                }

                BLObject<DSProcedures> obj = BLLClinicalObj.insertProcedure(dsprocedures);
                if (obj.Data != null)
                {
                    dsprocedures = obj.Data;
                    var response = new
                    {
                        // ProcedureId = dsprocedures.Tables[dsprocedures.Procedure.TableName].Rows[0][dsprocedures.Procedure.ProcedureIdColumn.ColumnName],
                        ProcedureCount = dsprocedures.Procedures.Rows.Count,
                        ProcedureLoad_JSON = MDVUtility.JSON_DataTable(dsprocedures.Tables[dsprocedures.Procedures.TableName]),
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
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
        //--------------------------------------
        public string updateProcedures(List<ProceduresDetailModel> model)
        {
            try
            {
                DSProcedures dsProcedures = new DSProcedures();
                BLObject<DSProcedures> obj = BLLClinicalObj.updateProcedures(model);
                if (obj.Data != null)
                {
                    dsProcedures = obj.Data;
                    var response = new
                    {
                        ProcedureId = dsProcedures.Tables[dsProcedures.Procedures.TableName].Rows[0][dsProcedures.Procedures.ProcedureIdColumn.ColumnName],
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
                        Message = obj.Message
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
        //-------------------------------------------------------
        public string ActiveInActiveProcedures(ProceduresModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProcedureId) > 0)
                {

                    DSProcedures dsProcedures = new DSProcedures();
                    BLObject<DSProcedures> obj = BLLClinicalObj.loadProcedures_Obsolete(MDVUtility.ToInt32(model.ProcedureId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NotesId), "1", null, 1, 1);
                    dsProcedures = obj.Data;
                    foreach (DSProcedures.ProceduresRow dr in dsProcedures.Tables[dsProcedures.Procedures.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.IsActive))
                            dr.IsActive = Boolean.Parse(model.IsActive);
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }
                    //#endregion
                    #region Database Updation
                    if (dsProcedures.Tables[dsProcedures.Procedures.TableName].Rows.Count > 0)
                    {
                        BLObject<DSProcedures> objUpdate = BLLClinicalObj.updateProcedure(dsProcedures);
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
                            message = "Procedure not found."
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
                        message = "Procedure not found."
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
        public string deleteProcedure(ProceduresDetailModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ProcedureId)))
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

                    BLObject<string> obj = BLLClinicalObj.deleteProcedure(MDVUtility.ToStr(model.ProcedureId), MDVUtility.ToStr(model.VaccineHxId));
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
        //----------------------------------------------


        #region "Procedures with Notes"

        /// <summary>
        /// this function will get latest allergy for notes attachment
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string getLatestProceduresByPatientId(Int64 PatientId, Int64 UserId, Int64 EntityId, long ProviderId)
        {
            try
            {

                DSProcedures dsProcedureSoap = null;
                BLObject<DSProcedures> obj;

                obj = BLLClinicalObj.getLatestProcedureByPatientId(PatientId, UserId, EntityId, ProviderId);

                dsProcedureSoap = obj.Data;
                var response = new
                {
                    status = true,
                    ProcedureSoapCount = dsProcedureSoap.Tables[dsProcedureSoap.ProcedureSoap.TableName].Rows.Count,
                    ProcedureSoap_JSON = MDVUtility.JSON_DataTable(dsProcedureSoap.Tables[dsProcedureSoap.ProcedureSoap.TableName]),
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
        /// <param name="ProcedureId"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        internal string getProceduresForSoap(string ProcedureId, long PatientId, string ProviderId = null)
        {
            try
            {

                DSProcedures dsProcedureSoap = null;
                BLObject<DSProcedures> obj = BLLClinicalObj.loadProceduresForSoap(ProcedureId, PatientId, ProviderId);
                dsProcedureSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsProcedureSoap.Tables[dsProcedureSoap.ProcedureSoap.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ProcedureSoapCount = dsProcedureSoap.Tables[dsProcedureSoap.ProcedureSoap.TableName].Rows.Count,
                            ProcedureSoap_JSON = MDVUtility.JSON_DataTable(dsProcedureSoap.Tables[dsProcedureSoap.ProcedureSoap.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ProcedureSoapCount = 0,
                            ProcedureSoap_JSON = MDVUtility.JSON_DataTable(dsProcedureSoap.Tables[dsProcedureSoap.ProcedureSoap.TableName]),
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
        internal string detach_Procedure_From_Notes(string ProcedureId, long NotesId, bool ForVBP = false)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProcedureId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachProcedureFromNotes(ProcedureId, NotesId, ForVBP);
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
        internal string attach_Procedure_With_Notes(string ProcedureId, long NotesId)
        {
            try
            {
                DSProcedures dsProcedure = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProcedureId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<DSProcedures> obj = BLLClinicalObj.attachProcedureWithNotes(ProcedureId, NotesId);
                    if (obj.Data != null)
                    {
                        dsProcedure = obj.Data;
                        var response = new
                        {
                            status = true,
                            ProcedureTotalCount = dsProcedure.Tables[dsProcedure.Procedures.TableName].Rows.Count,
                            ProcedureCount = dsProcedure.Tables[dsProcedure.Procedures.TableName].Rows.Count,
                            ProcedureLoad_JSON = MDVUtility.JSON_DataTable(dsProcedure.Tables[dsProcedure.Procedures.TableName]),
                            ProcedureHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsProcedure.Tables[dsProcedure.ProcedureHistory.TableName]),
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

        internal string isPHQProcedure(string ProcedureId, long PatientID, string providerid, long notesID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProcedureId)) && string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)) && string.IsNullOrEmpty(MDVUtility.ToStr(providerid)) && string.IsNullOrEmpty(MDVUtility.ToStr(notesID)))
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
                    string obj = BLLClinicalObj.isPHQProcedure(ProcedureId, PatientID, MDVUtility.ToInt64(providerid), notesID);
                    if (obj != "")
                    {
                        var response = new
                        {
                            status = true,
                            isPHQProcedure = obj,
                            Message = Common.AppPrivileges.Update_Message
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "",
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
        internal string CalculateVBPSocreForSoapText(long NotesId, bool PHQTextNeeded)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var tupleobj = BLLClinicalObj.CalculateVBPSocreForSoapText(NotesId, PHQTextNeeded);
                    if (tupleobj != null)
                    {
                        if (tupleobj.Item1 != "" && tupleobj.Item2 != "")
                        {
                            var response = new
                            {
                                status = true,
                                PHQSoapText = tupleobj.Item1,
                                ProceudereID = tupleobj.Item2
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "",
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "",
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
    }
}