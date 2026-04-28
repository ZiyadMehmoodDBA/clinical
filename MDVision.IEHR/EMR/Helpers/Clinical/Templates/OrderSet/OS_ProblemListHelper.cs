using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Model.Clinical.Templates.OrderSets;
using MDVision.Model.Lookups;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet
{
    public class OS_ProblemListHelper
    {
        //private BLLClinical BLLOrderSetObj = null;
        //private BLLRcopia BLLRcopiaObj = null;
        private BLLOrderSet BLLOrderSetObj = null;
        public OS_ProblemListHelper()
        {
            BLLOrderSetObj = new BLLOrderSet();
           // BLLRcopiaObj = new BLLRcopia();
        }
        private static OS_ProblemListHelper _instance = null;
        //private static bool isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
        public static OS_ProblemListHelper Instance()
        {
            if (_instance == null)
            {
                _instance = new OS_ProblemListHelper();
                //   isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
            }
            return _instance;
        }
        public string LoadProblemList(OS_ProblemListModel model)
        {
            try
            {
                DSOS_ProblemLists dsProblemList = null;
                BLObject<DSOS_ProblemLists> obj;

                obj = BLLOrderSetObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.OrderSetId),  "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", "0");

                dsProblemList = obj.Data;
                if (obj.Data != null)
                {
                    int ProblemListTotalCount = 0;
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count == 0)
                    {
                        if (model.IsActive.Equals("1"))
                        {
                            obj = BLLOrderSetObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.OrderSetId), "0", "0");
                        }
                        else
                        {
                            obj = BLLOrderSetObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.OrderSetId),  "0", "1");
                        }

                        if (obj.Data != null)
                        {
                            DSOS_ProblemLists dsProblemListInActive = obj.Data;
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

        public string LoadProblemListOp(OS_ProblemListModel model)
        {
            try
            {

                DSOS_ProblemLists dsProblemList = null;
                BLObject<DSOS_ProblemLists> obj;

                obj = BLLOrderSetObj.LoadProblemListsOp(MDVUtility.ToInt64(model.OrderSetId),  "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", "0");

                dsProblemList = obj.Data;
                if (obj.Data != null)
                {
                    int ProblemListTotalCount = 0;
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count == 0)
                    {
                        BLObject<string> obj1 = BLLOrderSetObj.CheckProblemListExists(MDVUtility.ToLong(model.OrderSetId));
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
        public string SaveProblemList(OS_ProblemListModel model)
        {
            try
            {
                DSOS_ProblemLists dsProblemList = new DSOS_ProblemLists();
                DSOS_ProblemLists.ProblemListRow dr = dsProblemList.ProblemList.NewProblemListRow();

                dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
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

            


                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsProblemList.ProblemList.AddProblemListRow(dr);
                BLObject<DSOS_ProblemLists> obj = BLLOrderSetObj.InsertProblemLists(dsProblemList);
                dsProblemList = obj.Data;

                if (obj.Data != null)
                {
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

        public string SaveProblemListOp(OS_ProblemListModel model)
        {
            try
            {
                DSOS_ProblemLists dsProblemList = new DSOS_ProblemLists();
                DSOS_ProblemLists.ProblemListRow dr = dsProblemList.ProblemList.NewProblemListRow();

                dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
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

                #region Database Insertion
                dsProblemList.ProblemList.AddProblemListRow(dr);
                BLObject<DSOS_ProblemLists> obj = BLLOrderSetObj.InsertProblemLists(dsProblemList);
                dsProblemList = obj.Data;

                if (obj.Data != null)
                {
                    //AppConfig.ProblemListRow = dr;
                   
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
        
        public string UpdateProblemList(OS_ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {

                    DSOS_ProblemLists dsProblemList = new DSOS_ProblemLists();
                    BLObject<DSOS_ProblemLists> obj = BLLOrderSetObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.OrderSetId));
                    dsProblemList = obj.Data;
                    foreach (DSOS_ProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.OrderSetId))
                            dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
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
                       
                        dr.IsActive = MDVUtility.ToStr(model.IsActive) == "1" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        //end newly added
                    }


                    #region Database Updation
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                    {
                        BLObject<DSOS_ProblemLists> objUpdate = BLLOrderSetObj.UpdateProblemLists(dsProblemList);
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

        public string UpdateProblemListOp(OS_ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {

                    DSOS_ProblemLists dsProblemList = new DSOS_ProblemLists();
                    BLObject<DSOS_ProblemLists> obj = BLLOrderSetObj.LoadProblemListsForFillData(MDVUtility.ToInt64(model.ProblemListId));
                    dsProblemList = obj.Data;
                    DSOS_ProblemLists.ProblemListRow dr1 = null;
                    foreach (DSOS_ProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.OrderSetId))
                            dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
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
                      
                        dr.IsActive =  true;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                      
                        //end newly added
                    }


                    #region Database Updation
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                    {
                        BLObject<DSOS_ProblemLists> objUpdate = BLLOrderSetObj.UpdateProblemListsOp(dsProblemList);
                        if (objUpdate.Data != null)
                        {
                            //HttpContext.Current.Session["ProblemList4GridDrFirst"] = dr1;
                          
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

    

        public string UpdateProblemListComments(OS_ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {

                    DSOS_ProblemLists dsProblemList = new DSOS_ProblemLists();
                    BLObject<DSOS_ProblemLists> obj = BLLOrderSetObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.OrderSetId), "1");
                    dsProblemList = obj.Data;
                    foreach (DSOS_ProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
                    {

                        if (!string.IsNullOrEmpty(model.Comments))
                            dr.Comments = MDVUtility.ToStr(model.Comments);
                        dr.IsActive = true;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    
                    }
                    #region Database Updation
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                    {
                        BLObject<DSOS_ProblemLists> objUpdate = BLLOrderSetObj.UpdateProblemLists(dsProblemList);
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
        public string DeleteProblemList(OS_ProblemListModel model)
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

                    BLObject<string> obj = BLLOrderSetObj.DeleteProblemLists(MDVUtility.ToStr(model.ProblemListId));
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

        public string DeleteProblemListOp(OS_ProblemListModel model)
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

                    BLObject<string> obj = BLLOrderSetObj.DeleteProblemLists(MDVUtility.ToStr(model.ProblemListId));
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
        
        public string ActiveInActiveProblemList(OS_ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {

                    DSOS_ProblemLists dsProblemList = new DSOS_ProblemLists();
                    BLObject<DSOS_ProblemLists> obj = BLLOrderSetObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.OrderSetId));
                    dsProblemList = obj.Data;
                    foreach (DSOS_ProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
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
                        
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        //end newly added
                    }
                   
                    #region Database Updation
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                    {
                        BLObject<DSOS_ProblemLists> objUpdate = BLLOrderSetObj.UpdateProblemLists(dsProblemList);
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

        public string ActiveInActiveProblemListOp(OS_ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {

                    DSOS_ProblemLists dsProblemList = new DSOS_ProblemLists();
                    BLObject<DSOS_ProblemLists> obj = BLLOrderSetObj.LoadProblemListsForInActive(MDVUtility.ToInt64(model.ProblemListId));
                    dsProblemList = obj.Data;
                  
                    foreach (DSOS_ProblemLists.ProblemListRow dr in dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows)
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
                       
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                     


                        //end newly added
                    }

                    #region Database Updation
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                    {
                        BLObject<DSOS_ProblemLists> objUpdate = BLLOrderSetObj.UpdateProblemListsForInActive(dsProblemList);
                        if (objUpdate.Data != null)
                        {
                            //HttpContext.Current.Session["ProblemList4INActiveDrFirst"] = dr1;
                         
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
      



        
        public string LogProblemListView(OS_ProblemListModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ProblemListId) > 0)
                {
                    BLObject<DSOS_ProblemLists> obj = BLLOrderSetObj.LoadProblemListsForFillData(MDVUtility.ToInt64(model.ProblemListId), "1");

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
    }
}