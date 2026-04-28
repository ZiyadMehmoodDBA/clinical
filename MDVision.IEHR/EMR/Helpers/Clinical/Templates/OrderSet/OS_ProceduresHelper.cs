using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Utilities;
using MDVision.Model.Clinical.Templates.OrderSets;
using System;
using System.Collections.Generic;
using System.Linq;
using MDVision.Datasets;
using System.Web;
using MDVision.Common.Shared;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet
{
 
    public class OS_ProceduresHelper
    {
        private BLLOrderSet BLLOrderSetObj = null;
        public OS_ProceduresHelper()
        {
            BLLOrderSetObj = new BLLOrderSet();
        }
        private static OS_ProceduresHelper _instance = null;
        public static OS_ProceduresHelper Instance()
        {
            if (_instance == null)
                _instance = new OS_ProceduresHelper();
            return _instance;
        }
        public string loadProcedures(OS_ProceduresModel model)
        {
            try
            {
                DSOS_Procedures DSOS_Procedures = null;
                BLObject<DSOS_Procedures> obj = null;
                //Start 27-10-2016 Humaira Yousaf to log view action for problem lists
                if (MDVUtility.ToInt64(model.ProcedureId) > 0)
                {
                    obj = BLLOrderSetObj.loadProcedures(MDVUtility.ToInt32(model.ProcedureId), MDVUtility.ToInt64(model.OrderSetId),  "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(0), MDVUtility.ToInt32(0), "1", "0");

                }
                else if (model.procedureDetailModel != null && model.procedureDetailModel.Count > 0)
                {
                    obj = BLLOrderSetObj.loadProcedures(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].OrderSetId),  "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.procedureDetailModel[0].PageNumber), MDVUtility.ToInt32(model.procedureDetailModel[0].RowsPerPage), "", "0");
                }
                else
                {
                    obj = BLLOrderSetObj.loadProcedures(MDVUtility.ToInt32(model.ProcedureId), MDVUtility.ToInt64(model.OrderSetId),  "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(0), MDVUtility.ToInt32(0), "", "0");
                }
                //End 27-10-2016 Humaira Yousaf to log view action for problem lists
                DSOS_Procedures = obj.Data;
                if (obj.Data != null)
                {
                    int ProcedureCount = 0;
                    //if (model.procedureDetailModel != null && model.procedureDetailModel.Count > 0 && model.procedureDetailModel[0].IsActive.Equals("1")) 
                    if (DSOS_Procedures.Tables[DSOS_Procedures.Procedures.TableName].Rows.Count == 0)
                    {
                        //if (model.procedureDetailModel[0].IsActive.Equals("1"))
                        if (model.procedureDetailModel != null && model.procedureDetailModel.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(model.procedureDetailModel[0].IsActive) && model.procedureDetailModel[0].IsActive.Equals("1"))
                            {
                                obj = BLLOrderSetObj.loadProcedures(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].OrderSetId), "0", "0");
                            }
                        }
                        else
                        {
                            obj = BLLOrderSetObj.loadProcedures(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].OrderSetId), "0", "1");
                        }

                        if (obj.Data != null)
                        {
                            DSOS_Procedures DSOS_ProceduresInActive = obj.Data;
                            ProcedureCount = DSOS_ProceduresInActive.Tables[DSOS_ProceduresInActive.Procedures.TableName].Rows.Count;
                        }
                    }
                    else
                    {
                        ProcedureCount = DSOS_Procedures.Tables[DSOS_Procedures.Procedures.TableName].Rows.Count;
                    }
                    var response = new
                    {
                        status = true,
                        ProcedureTotalCount = ProcedureCount,
                        ProcedureCount = DSOS_Procedures.Tables[DSOS_Procedures.Procedures.TableName].Rows.Count,
                        ProcedureLoad_JSON = MDVUtility.JSON_DataTable(DSOS_Procedures.Tables[DSOS_Procedures.Procedures.TableName]),
                        ProcedureHistoryLoad_JSON = MDVUtility.JSON_DataTable(DSOS_Procedures.Tables[DSOS_Procedures.ProcedureHistory.TableName]),
                        iTotalDisplayRecords = (DSOS_Procedures.Procedures.Rows.Count > 0) ? DSOS_Procedures.Procedures.Rows[0][DSOS_Procedures.Procedures.RecordCountColumn.ColumnName] : 0,
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



        //---------------------------------------------
        public string saveProcedure(OS_ProceduresModel model)
        {
            try
            {
                DSOS_Procedures DSOS_Procedures = new DSOS_Procedures();

                for (int i = 0; i < model.procedureDetailModel.Count; i++)
                {

                    DSOS_Procedures.ProceduresRow procModel = DSOS_Procedures.Procedures.NewProceduresRow();
                    procModel.ProcedureId = -i;
                    procModel.Modifier = model.procedureDetailModel[i].Modifier;
                    procModel.Unit = model.procedureDetailModel[i].Unit;

                    if (!string.IsNullOrEmpty(model.procedureDetailModel[i].ProblemListId))
                    {
                        procModel.ProblemListId = MDVUtility.ToLong(model.procedureDetailModel[i].ProblemListId);
                    }
                    else
                    {
                        procModel[DSOS_Procedures.Procedures.ProblemListIdColumn] = DBNull.Value;
                    }



                    if (!string.IsNullOrEmpty(model.procedureDetailModel[i].SNOMEDID))
                        procModel.SNOMEDID = model.procedureDetailModel[i].SNOMEDID;
                    else
                        procModel[DSOS_Procedures.Procedures.SNOMEDIDColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(model.procedureDetailModel[i].SNOMED_DESCRIPTION))
                        procModel.SNOMED_DESCRIPTION = model.procedureDetailModel[i].SNOMED_DESCRIPTION;
                    else
                        procModel[DSOS_Procedures.Procedures.SNOMED_DESCRIPTIONColumn] = DBNull.Value;



                    if (!string.IsNullOrEmpty(model.procedureDetailModel[i].StartDate))
                    {
                        procModel.StartDate = MDVUtility.ToDateTime(model.procedureDetailModel[i].StartDate);
                    }
                    else
                    {
                        procModel[DSOS_Procedures.Procedures.StartDateColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(model.procedureDetailModel[i].EndDate))
                    {
                        procModel.EndDate = MDVUtility.ToDateTime(model.procedureDetailModel[i].EndDate);
                    }
                    else
                    {
                        procModel[DSOS_Procedures.Procedures.EndDateColumn] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(model.procedureDetailModel[i].Comments))
                    {
                        procModel.Comments = MDVUtility.ToStr(model.procedureDetailModel[i].Comments);
                    }
                    else
                    {
                        procModel[DSOS_Procedures.Procedures.CommentsColumn] = DBNull.Value;
                    }


                    procModel.OrderSetId = MDVUtility.ToLong(model.procedureDetailModel[i].OrderSetId);
               

                    if (MDVUtility.ToStr(model.procedureDetailModel[i].VaccineHxId) != "")
                    {
                        procModel.VaccineHxId = MDVUtility.ToLong(model.procedureDetailModel[i].VaccineHxId);
                    }
                    else
                    {
                        procModel[DSOS_Procedures.Procedures.VaccineHxIdColumn] = DBNull.Value;
                    }
                    if (MDVUtility.ToStr(model.procedureDetailModel[i].ImmTherInjectionId) != "")
                    {
                        procModel.ImmTherInjectionId = MDVUtility.ToLong(model.procedureDetailModel[i].ImmTherInjectionId);
                    }
                    else
                    {
                        procModel[DSOS_Procedures.Procedures.ImmTherInjectionIdColumn] = DBNull.Value;
                    }

                    if (MDVUtility.ToStr(model.procedureDetailModel[i].CPTId) != "")
                    {
                        procModel.CPTId = MDVUtility.ToLong(model.procedureDetailModel[i].CPTId);
                    }
                    else
                    {
                        procModel[DSOS_Procedures.Procedures.CPTIdColumn] = DBNull.Value;
                    }

                    procModel.IsActive = true;
                    procModel.ProblemListId = MDVUtility.ToLong(model.procedureDetailModel[i].ProblemListId);
                    procModel.CPTCode = MDVUtility.ToStr(model.procedureDetailModel[i].CPTCode);
                    procModel.CPT_DESCRIPTION = MDVUtility.ToStr(model.procedureDetailModel[i].CPT_DESCRIPTION);
                    procModel.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    procModel.CreatedOn = DateTime.Now;
                    procModel.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    procModel.ModifiedOn = DateTime.Now;

                    DSOS_Procedures.Procedures.AddProceduresRow(procModel);
                }
                BLObject<DSOS_Procedures> obj = BLLOrderSetObj.insertProcedure(DSOS_Procedures);
                if (obj.Data != null)
                {
                    DSOS_Procedures = obj.Data;
                    var response = new
                    {
                        // ProcedureId = DSOS_Procedures.Tables[DSOS_Procedures.Procedure.TableName].Rows[0][DSOS_Procedures.Procedure.ProcedureIdColumn.ColumnName],
                        ProcedureCount = DSOS_Procedures.Procedures.Rows.Count,
                        ProcedureLoad_JSON = MDVUtility.JSON_DataTable(DSOS_Procedures.Tables[DSOS_Procedures.Procedures.TableName]),
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

        public string saveProcedureForVaccine(OS_ProceduresModel model)
        {
            try
            {
                DSOS_Procedures DSOS_Procedures = new DSOS_Procedures();

                for (int i = 0; i < model.procedureDetailModel.Count; i++)
                {

                    DSOS_Procedures.ProceduresRow procModel = DSOS_Procedures.Procedures.NewProceduresRow();

                    procModel.ProcedureId = -i;
                    procModel[DSOS_Procedures.Procedures.ModifierColumn] = "";
                    procModel[DSOS_Procedures.Procedures.UnitColumn] = 1;
                    procModel[DSOS_Procedures.Procedures.ProblemListIdColumn] = DBNull.Value;
                    procModel[DSOS_Procedures.Procedures.SNOMEDIDColumn] = DBNull.Value;
                    procModel[DSOS_Procedures.Procedures.SNOMED_DESCRIPTIONColumn] = DBNull.Value;
                    procModel[DSOS_Procedures.Procedures.StartDateColumn] = DBNull.Value;
                    procModel[DSOS_Procedures.Procedures.EndDateColumn] = DBNull.Value;
                    procModel[DSOS_Procedures.Procedures.CommentsColumn] = DBNull.Value;

                    procModel.OrderSetId = MDVUtility.ToLong(model.procedureDetailModel[i].OrderSetId);
                  
                    procModel.IsActive = true;
                    procModel[DSOS_Procedures.Procedures.ModifierColumn] = DBNull.Value;
                    procModel.CPTCode = MDVUtility.ToStr(model.procedureDetailModel[i].CPTCode);
                    procModel.CPT_DESCRIPTION = MDVUtility.ToStr(model.procedureDetailModel[i].CPT_DESCRIPTION);
                    procModel.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    procModel.CreatedOn = DateTime.Now;
                    procModel.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    procModel.ModifiedOn = DateTime.Now;

                    DSOS_Procedures.Procedures.AddProceduresRow(procModel);
                }

                BLObject<DSOS_Procedures> obj = BLLOrderSetObj.insertProcedure(DSOS_Procedures);
                if (obj.Data != null)
                {
                    DSOS_Procedures = obj.Data;
                    var response = new
                    {
                        // ProcedureId = DSOS_Procedures.Tables[DSOS_Procedures.Procedure.TableName].Rows[0][DSOS_Procedures.Procedure.ProcedureIdColumn.ColumnName],
                        ProcedureCount = DSOS_Procedures.Procedures.Rows.Count,
                        ProcedureLoad_JSON = MDVUtility.JSON_DataTable(DSOS_Procedures.Tables[DSOS_Procedures.Procedures.TableName]),
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
        public string updateProcedures(List<OS_ProceduresDetailModel> model)
        {
            try
            {
                if (MDVUtility.ToInt32(model[0].ProcedureId) > 0)
                {
                    DSOS_Procedures DSOS_Procedures = null;
                    BLObject<DSOS_Procedures> obj;
                    obj = BLLOrderSetObj.loadProcedures(MDVUtility.ToInt32(model[0].ProcedureId), MDVUtility.ToInt64(model[0].OrderSetId),  "1", MDVUtility.ToStr(model[0].IsActive), MDVUtility.ToInt32(model[0].PageNumber), MDVUtility.ToInt32(model[0].RowsPerPage));
                    DSOS_Procedures = obj.Data;
                    //-----------------------------------
                    foreach (DSOS_Procedures.ProceduresRow dr in DSOS_Procedures.Tables[DSOS_Procedures.Procedures.TableName].Rows)
                    {
                        //if (!string.IsNullOrEmpty(model[0].StartDate))
                        //dr.StartDate = MDVUtility.ToDateTime(model[0].StartDate);
                        //if (!string.IsNullOrEmpty(model[0].EndDate))
                        //dr.EndDate = MDVUtility.ToDateTime(model[0].EndDate);



                        if (!string.IsNullOrEmpty(model[0].StartDate))
                        {
                            dr.StartDate = MDVUtility.ToDateTime(model[0].StartDate);
                        }
                        else
                        {
                            dr[DSOS_Procedures.Procedures.StartDateColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model[0].EndDate))
                        {
                            dr.EndDate = MDVUtility.ToDateTime(model[0].EndDate);
                        }
                        else
                        {
                            dr[DSOS_Procedures.Procedures.EndDateColumn] = DBNull.Value;
                        }


                        dr.Comments = MDVUtility.ToStr(model[0].Comments);
                        if (!string.IsNullOrEmpty(model[0].OrderSetId))
                            dr.OrderSetId = MDVUtility.ToInt64(model[0].OrderSetId);
                        if (model[0].IsActive != null)
                        {
                            if (model[0].IsActive == "1") model[0].IsActive = "true"; else model[0].IsActive = "false";
                        }


                        if (!string.IsNullOrEmpty(model[0].IsActive))
                            dr.IsActive = Boolean.Parse(model[0].IsActive);

                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(model[0].InActiveChkBoxValue))
                            dr.InActiveChkBoxValue = MDVUtility.ToStr(model[0].InActiveChkBoxValue);
                        if (!string.IsNullOrEmpty(model[0].InActiveReason))
                            dr.InActiveReason = MDVUtility.ToStr(model[0].InActiveReason);
                        if (!string.IsNullOrEmpty(model[0].ProblemListId))
                            dr.ProblemListId = MDVUtility.ToInt64(model[0].ProblemListId);
                        if (!string.IsNullOrEmpty(model[0].CPTCode))
                            dr.CPTCode = MDVUtility.ToStr(model[0].CPTCode);
                        if (!string.IsNullOrEmpty(model[0].CPT_DESCRIPTION))
                            dr.CPT_DESCRIPTION = MDVUtility.ToStr(model[0].CPT_DESCRIPTION);



                        var ICD10description = "";
                        var Code9 = "";
                        var Code10 = "";
                        if (model[0].ProblemListId_text != null && model[0].ProblemListId_text != "")
                        {
                            var SplitedProblemListText = model[0].ProblemListId_text.ToString().Split('-');
                            if (SplitedProblemListText.Count() >= 1)
                            {
                                Code9 = SplitedProblemListText[0];
                            }
                            if (SplitedProblemListText.Count() > 1)
                            {
                                Code10 = SplitedProblemListText[1];
                            }
                            for (int j = 2; j < SplitedProblemListText.Count(); j++)
                            {
                                ICD10description += SplitedProblemListText[j];
                            }
                        }

                        dr.Modifier = model[0].Modifier;
                        dr.Unit = model[0].Unit;
                        dr.ICD9 = Code9;
                        dr.ICD10 = Code10;
                        dr.ICD10_DESCRIPTION = ICD10description;
                        //     procModel.ProblemListId = MDVUtility.ToLong(model.procedureDetailModel[i].ProblemListId);


                    }
                    //-----------------------------------
                    #region Database Updation
                    if (obj.Data != null)
                    {
                        BLObject<DSOS_Procedures> objUpdate = BLLOrderSetObj.updateProcedure(DSOS_Procedures);
                        if (objUpdate.Data != null)
                        {
                            DSOS_Procedures = objUpdate.Data;
                            var response = new
                            {
                                ProcedureId = DSOS_Procedures.Tables[DSOS_Procedures.Procedures.TableName].Rows[0][DSOS_Procedures.Procedures.ProcedureIdColumn.ColumnName],
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
                                Message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Procedures not found."
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
                        Message = "Procedures not found."
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
        public string ActiveInActiveProcedures(OS_ProceduresModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProcedureId) > 0)
                {

                    DSOS_Procedures DSOS_Procedures = new DSOS_Procedures();
                    BLObject<DSOS_Procedures> obj = BLLOrderSetObj.loadProcedures(MDVUtility.ToInt32(model.ProcedureId), MDVUtility.ToInt64(model.OrderSetId),  "1", null, 1, 1);
                    DSOS_Procedures = obj.Data;
                    foreach (DSOS_Procedures.ProceduresRow dr in DSOS_Procedures.Tables[DSOS_Procedures.Procedures.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.IsActive))
                            dr.IsActive = Boolean.Parse(model.IsActive);
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }
                    //#endregion
                    #region Database Updation
                    if (DSOS_Procedures.Tables[DSOS_Procedures.Procedures.TableName].Rows.Count > 0)
                    {
                        BLObject<DSOS_Procedures> objUpdate = BLLOrderSetObj.updateProcedure(DSOS_Procedures);
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
        public string deleteProcedure(OS_ProceduresDetailModel model)
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

                    BLObject<string> obj = BLLOrderSetObj.deleteProcedure(MDVUtility.ToStr(model.ProcedureId), MDVUtility.ToStr(model.VaccineHxId));
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

            
    }
}