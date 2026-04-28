using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using System.Web.Script.Serialization;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Admin.FollowUp
{
    public class Admin_FollowUpCodesMapping
    {
        private BLLAdminFollowUp BLLAdminFollowUpObj = null;
        public Admin_FollowUpCodesMapping()
        {
            BLLAdminFollowUpObj = new BLLAdminFollowUp();
        }
        #region Singleton
        private static Admin_FollowUpCodesMapping _obj = null;
        public static Admin_FollowUpCodesMapping Instance()
        {
            if (_obj == null)
                _obj = new Admin_FollowUpCodesMapping();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string LoadCodesMapping(string fieldsJSON, Int64 CodesMappingId, int PageNumber, int RowsPerPage)
        {
            // string isActive = "";
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSFollowUp dsClaimStatus = null;
                BLObject<DSFollowUp> objReason;

                //if (SearchedfieldsJSON["chkActive"] == true)
                //    isActive = "1";
                //if (SearchedfieldsJSON["chkActive"] == false)
                //    isActive = "0";

                int ddlCSCode = MDVUtility.ToInt32(SearchedfieldsJSON["ddlCSCode"]);
                int ddlCSCategoryCode = MDVUtility.ToInt32(SearchedfieldsJSON["ddlCSCategoryCode"]);
                int ddlAction = MDVUtility.ToInt32(SearchedfieldsJSON["ddlAction"]);
                int ddlReason = MDVUtility.ToInt32(SearchedfieldsJSON["ddlReason"]);

                if (SearchedfieldsJSON == null)
                    objReason = BLLAdminFollowUpObj.LoadCodesMapping(CodesMappingId, 0, 0, 0, 0, null, PageNumber, RowsPerPage);
                else
                    objReason = BLLAdminFollowUpObj.LoadCodesMapping(CodesMappingId, ddlCSCode, ddlCSCategoryCode, ddlAction, ddlReason, SearchedfieldsJSON["ddlActive"]);

                dsClaimStatus = objReason.Data;
                if (objReason.Data != null)
                {
                    if (dsClaimStatus.Tables[dsClaimStatus.CodesMapping.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CodesMappingCount = dsClaimStatus.Tables[dsClaimStatus.CodesMapping.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsClaimStatus.CodesMapping.Rows.Count > 0) ? dsClaimStatus.CodesMapping.Rows[0][dsClaimStatus.CodesMapping.RecordCountColumn.ColumnName] : 0,
                            CodesMappingLoad_JSON = MDVUtility.JSON_DataTable(dsClaimStatus.Tables[dsClaimStatus.CodesMapping.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CodesMappingCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        CodesMappingCount = 0,
                        Message = objReason.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string DeleteCodesMapping(long CodesMappingId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CodesMappingId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteCodesMapping(MDVUtility.ToStr(CodesMappingId));
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string CodesMappingActiveInactive(Int32 CodesMappingId, Int64 IsActive)
        {
            try
            {
                DSFollowUp dsClaimStatus = new DSFollowUp();

                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadCodesMapping(CodesMappingId, 0, 0, 0, 0, null);
                dsClaimStatus = obj.Data;
                if (dsClaimStatus.Tables[dsClaimStatus.CodesMapping.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsClaimStatus.Tables[dsClaimStatus.CodesMapping.TableName].Rows[0];
                    dr[dsClaimStatus.CodesMapping.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.UpdateCodesMapping(dsClaimStatus);
                    string successMsg;
                    if (objType.Data != null)
                    {
                        if (IsActive == 0)
                            successMsg = Common.AppPrivileges.Inactive_Message;
                        else
                            successMsg = Common.AppPrivileges.Active_Message;
                        var response = new
                        {
                            status = true,
                            Message = successMsg
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objType.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string SaveCodesMapping(string fieldsJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSFollowUp dsClaimStatus = new DSFollowUp();
                DSFollowUp.CodesMappingRow dr = dsClaimStatus.CodesMapping.NewCodesMappingRow();

                if (!string.IsNullOrEmpty(searchedfieldsJson["ddlCSCode"]))
                    dr.ClaimStatusCodeId = MDVUtility.ToInt32(searchedfieldsJson["ddlCSCode"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["ddlCSCategoryCode"]))
                    dr.ClaimStatusCategoryCodeId = MDVUtility.ToInt32(searchedfieldsJson["ddlCSCategoryCode"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["ddlAction"]))
                    dr.ActionId = MDVUtility.ToInt64(searchedfieldsJson["ddlAction"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["ddlReason"]))
                    dr.ReasonId = MDVUtility.ToInt64(searchedfieldsJson["ddlReason"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["txtDescription"]))
                    dr.Description = MDVUtility.ToStr(searchedfieldsJson["txtDescription"]);

                dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True";

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsClaimStatus.CodesMapping.AddCodesMappingRow(dr);
                BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.InsertCodesMapping(dsClaimStatus);
                dsClaimStatus = objType.Data;
                if (objType.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message,
                        CodesMappingId = dsClaimStatus.Tables[dsClaimStatus.CodesMapping.TableName].Rows[0][dsClaimStatus.CodesMapping.CodesMappingIdColumn.ColumnName]
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objType.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        private string FillCodesMapping(Int32 CodesMappingId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CodesMappingId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSFollowUp dsClaimStatus = null;
                    BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadCodesMapping(CodesMappingId, 0, 0, 0, 0, null);
                    if (obj.Data != null)
                    {
                        dsClaimStatus = obj.Data;
                        if (dsClaimStatus.Tables[dsClaimStatus.CodesMapping.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsClaimStatus.Tables[dsClaimStatus.CodesMapping.TableName].Rows[0];

                            var keyValues = new Dictionary<string, string>
                        {
                            { "ddlCSCode", MDVUtility.ToStr(dr[dsClaimStatus.CodesMapping.ClaimStatusCodeIdColumn.ColumnName])},
                            { "ddlCSCategoryCode", MDVUtility.ToStr(dr[dsClaimStatus.CodesMapping.ClaimStatusCategoryCodeIdColumn.ColumnName])},
                            { "ddlAction", MDVUtility.ToStr(dr[dsClaimStatus.CodesMapping.ActionIdColumn.ColumnName])},
                            { "ddlReason", MDVUtility.ToStr(dr[dsClaimStatus.CodesMapping.ReasonIdColumn.ColumnName])},

                            { "txtDescription", MDVUtility.ToStr(dr[dsClaimStatus.CodesMapping.DescriptionColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsClaimStatus.CodesMapping.IsActiveColumn.ColumnName])},
                            
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                CodesMappingFill_JSON = js.Serialize(keyValues)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string UpdateCodesMapping(string fieldsJson, Int32 CodesMappingId)
        {
            try
            {
                DSFollowUp dsClaimStatus = null;
                BLObject<DSFollowUp> obj = null;
                obj = BLLAdminFollowUpObj.LoadCodesMapping(CodesMappingId, 0, 0, 0, 0, null);
                dsClaimStatus = obj.Data;
                if (dsClaimStatus.Tables[dsClaimStatus.CodesMapping.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    foreach (DataRow dr in dsClaimStatus.Tables[dsClaimStatus.CodesMapping.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("ddlCSCode") && !string.IsNullOrEmpty(searchedfieldsJson["ddlCSCode"]))
                            dr[dsClaimStatus.CodesMapping.ClaimStatusCodeIdColumn] = MDVUtility.ToInt32(searchedfieldsJson["ddlCSCode"]);

                        if (searchedfieldsJson.ContainsKey("ddlCSCategoryCode") && !string.IsNullOrEmpty(searchedfieldsJson["ddlCSCategoryCode"]))
                            dr[dsClaimStatus.CodesMapping.ClaimStatusCategoryCodeIdColumn] = MDVUtility.ToInt32(searchedfieldsJson["ddlCSCategoryCode"]);

                        if (searchedfieldsJson.ContainsKey("ddlAction") && !string.IsNullOrEmpty(searchedfieldsJson["ddlAction"]))
                            dr[dsClaimStatus.CodesMapping.ActionIdColumn] = MDVUtility.ToInt32(searchedfieldsJson["ddlAction"]);

                        if (searchedfieldsJson.ContainsKey("ddlReason") && !string.IsNullOrEmpty(searchedfieldsJson["ddlReason"]))
                            dr[dsClaimStatus.CodesMapping.ReasonIdColumn] = MDVUtility.ToInt32(searchedfieldsJson["ddlReason"]);

                        if (searchedfieldsJson.ContainsKey("txtDescription") && !string.IsNullOrEmpty(searchedfieldsJson["txtDescription"]))
                            dr[dsClaimStatus.CodesMapping.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["txtDescription"]);
                        if (searchedfieldsJson.ContainsKey("chkActive"))
                            dr[dsClaimStatus.CodesMapping.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True" ? true : false;

                        //dr[dsClaimStatus.CodesMapping.CreatedByColumn] = MDVUtility.ToStr(dr[dsClaimStatus.CodesMapping.CreatedByColumn.ColumnName]);
                        dr[dsClaimStatus.CodesMapping.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr[dsClaimStatus.CodesMapping.CreatedOnColumn] = DateTime.Now;
                        dr[dsClaimStatus.CodesMapping.ModifiedOnColumn] = DateTime.Now;
                    }
                    BLObject<DSFollowUp> objType = null;
                    objType = BLLAdminFollowUpObj.UpdateCodesMapping(dsClaimStatus);

                    if (objType.Data != null)
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
                            Message = objType.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_CODESMAPPING":
                    {
                        string fieldsJSON = context.Request["CodesMappingData"];
                        Int64 CodesMappingId = MDVUtility.ToInt64(context.Request["CodesMappingId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadCodesMapping(fieldsJSON, CodesMappingId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_CODESMAPPING":
                    {
                        Int64 CodesMappingId = MDVUtility.ToInt64(context.Request["CodesMappingId"]);
                        string strJSONData = DeleteCodesMapping(CodesMappingId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_CODESMAPPING_ACTIVE_INACTIVE":
                    {
                        Int32 CodesMappingId = MDVUtility.ToInt32(context.Request["CodesMappingId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = CodesMappingActiveInactive(CodesMappingId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_CODESMAPPING":
                    {
                        string fieldsJson = context.Request["CodesMappingData"];
                        string strJsonData = SaveCodesMapping(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_CODESMAPPING":
                    {
                        string fieldsJson = context.Request["CodesMappingData"];
                        Int32 CodesMappingId = MDVUtility.ToInt32(context.Request["CodesMappingId"]);
                        string strJsonData = UpdateCodesMapping(fieldsJson, CodesMappingId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "FILL_CODESMAPPING":
                    {
                        string CodesMappingId = context.Request["CodesMappingId"];
                        string strJsonData = FillCodesMapping(MDVUtility.ToInt32(CodesMappingId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }
        #endregion
    }
}