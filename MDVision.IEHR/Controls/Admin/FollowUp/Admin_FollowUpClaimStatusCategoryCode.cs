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
    public class Admin_FollowUpClaimStatusCategoryCode
    {


         private BLLAdminFollowUp BLLAdminFollowUpObj = null;
         public Admin_FollowUpClaimStatusCategoryCode()
        {
            BLLAdminFollowUpObj = new BLLAdminFollowUp();
        }
        #region Singleton
        private static Admin_FollowUpClaimStatusCategoryCode _obj = null;
        public static Admin_FollowUpClaimStatusCategoryCode Instance()
        {
            if (_obj == null)
                _obj = new Admin_FollowUpClaimStatusCategoryCode();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string LoadClaimStatusCategoryCode(string fieldsJSON, Int32 CSCatCodeId, int PageNumber, int RowsPerPage)
        {
            //string isActive = "";
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

                if (SearchedfieldsJSON == null)
                    objReason = BLLAdminFollowUpObj.LoadClaimStatusCategoryCode(CSCatCodeId, null, null, null, PageNumber, RowsPerPage);
                else
                    objReason = BLLAdminFollowUpObj.LoadClaimStatusCategoryCode(CSCatCodeId, SearchedfieldsJSON["txtCode"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);

                dsClaimStatus = objReason.Data;
                if (objReason.Data != null)
                {
                    if (dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCategoryCode.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClaimStatusCategoryCodeCount = dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCategoryCode.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsClaimStatus.ClaimStatusCategoryCode.Rows.Count > 0) ? dsClaimStatus.ClaimStatusCategoryCode.Rows[0][dsClaimStatus.ClaimStatusCategoryCode.RecordCountColumn.ColumnName] : 0,
                            ClaimStatusCategoryCodeLoad_JSON = MDVUtility.JSON_DataTable(dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCategoryCode.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClaimStatusCategoryCodeCount = 0,
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
                        ClaimStatusCategoryCodeCount = 0,
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
        private string DeleteClaimStatusCategoryCode(long CSCatCodeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CSCatCodeId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteClaimStatusCategoryCode(MDVUtility.ToStr(CSCatCodeId));
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
        private string ClaimStatusCategoryCodeActiveInactive(Int32 CSCatCodeId, Int64 IsActive)
        {
            try
            {
                DSFollowUp dsClaimStatus = new DSFollowUp();

                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadClaimStatusCategoryCode(CSCatCodeId, null, null, null);
                dsClaimStatus = obj.Data;
                if (dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCategoryCode.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCategoryCode.TableName].Rows[0];
                    dr[dsClaimStatus.ClaimStatusCategoryCode.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.UpdateClaimStatusCategoryCode(dsClaimStatus);
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
        private string SaveClaimStatusCategoryCode(string fieldsJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSFollowUp dsClaimStatus = new DSFollowUp();
                DSFollowUp.ClaimStatusCategoryCodeRow dr = dsClaimStatus.ClaimStatusCategoryCode.NewClaimStatusCategoryCodeRow();


                if (!string.IsNullOrEmpty(searchedfieldsJson["txtCode"]))
                    dr.Code = MDVUtility.ToStr(searchedfieldsJson["txtCode"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["txtDescription"]))
                    dr.Description = MDVUtility.ToStr(searchedfieldsJson["txtDescription"]);

                dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True";

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsClaimStatus.ClaimStatusCategoryCode.AddClaimStatusCategoryCodeRow(dr);
                BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.InsertClaimStatusCategoryCode(dsClaimStatus);
                dsClaimStatus = objType.Data;
                if (objType.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message,
                        CSCatCodeId = dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCategoryCode.TableName].Rows[0][dsClaimStatus.ClaimStatusCategoryCode.CSCatCodeIdColumn.ColumnName]
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
        private string FillClaimStatusCategoryCode(Int32 CSCatCodeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CSCatCodeId)))
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
                    BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadClaimStatusCategoryCode(CSCatCodeId, null, null, null);
                    if (obj.Data != null)
                    {
                        dsClaimStatus = obj.Data;
                        if (dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCategoryCode.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCategoryCode.TableName].Rows[0];

                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtCode", MDVUtility.ToStr(dr[dsClaimStatus.ClaimStatusCategoryCode.CodeColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsClaimStatus.ClaimStatusCategoryCode.DescriptionColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsClaimStatus.ClaimStatusCategoryCode.IsActiveColumn.ColumnName])},
                            { "CreatedBy", MDVUtility.ToStr(dr[dsClaimStatus.ClaimStatusCategoryCode.CreatedByColumn.ColumnName])},
                            
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ClaimStatusCategoryCodeFill_JSON = js.Serialize(keyValues)
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string UpdateClaimStatusCategoryCode(string fieldsJson, Int32 CSCatCodeId)
        {
            try
            {
                DSFollowUp dsClaimStatus = null;
                BLObject<DSFollowUp> obj = null;
                obj = BLLAdminFollowUpObj.LoadClaimStatusCategoryCode(CSCatCodeId, null, null, null);
                dsClaimStatus = obj.Data;
                if (dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCategoryCode.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    foreach (DataRow dr in dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCategoryCode.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("txtCode") && !string.IsNullOrEmpty(searchedfieldsJson["txtCode"]))
                            dr[dsClaimStatus.ClaimStatusCategoryCode.CodeColumn] = MDVUtility.ToStr(searchedfieldsJson["txtCode"]);
                        if (searchedfieldsJson.ContainsKey("txtDescription") && !string.IsNullOrEmpty(searchedfieldsJson["txtDescription"]))
                            dr[dsClaimStatus.ClaimStatusCategoryCode.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["txtDescription"]);
                        if (searchedfieldsJson.ContainsKey("chkActive"))
                            dr[dsClaimStatus.ClaimStatusCategoryCode.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True" ? true : false;

                        //dr[dsClaimStatus.ClaimStatusCategoryCode.CreatedByColumn] = MDVUtility.ToStr(dr[dsClaimStatus.ClaimStatusCategoryCode.CreatedByColumn.ColumnName]);
                        dr[dsClaimStatus.ClaimStatusCategoryCode.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr[dsClaimStatus.ClaimStatusCategoryCode.CreatedOnColumn] = DateTime.Now;
                        dr[dsClaimStatus.ClaimStatusCategoryCode.ModifiedOnColumn] = DateTime.Now;
                    }
                    BLObject<DSFollowUp> objType = null;
                    objType = BLLAdminFollowUpObj.UpdateClaimStatusCategoryCode(dsClaimStatus);

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
                case "SEARCH_CLAIMSTATUSCATCODE":
                    {
                        string fieldsJSON = context.Request["ClaimStatusCategoryCodeData"];
                        Int32 CSCatCodeId = MDVUtility.ToInt32(context.Request["CSCatCodeId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadClaimStatusCategoryCode(fieldsJSON, CSCatCodeId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_CLAIMSTATUSCATCODE":
                    {
                        Int64 CSCatCodeId = MDVUtility.ToInt64(context.Request["CSCatCodeId"]);
                        string strJSONData = DeleteClaimStatusCategoryCode(CSCatCodeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_CLAIMSTATUSCATCODE_ACTIVE_INACTIVE":
                    {
                        Int32 CSCatCodeId = MDVUtility.ToInt32(context.Request["CSCatCodeId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = ClaimStatusCategoryCodeActiveInactive(CSCatCodeId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_CLAIMSTATUSCATCODE":
                    {
                        string fieldsJson = context.Request["ClaimStatusCategoryCodeData"];
                        string strJsonData = SaveClaimStatusCategoryCode(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_CLAIMSTATUSCATCODE":
                    {
                        string fieldsJson = context.Request["ClaimStatusCategoryCodeData"];
                        Int32 CSCatCodeId = MDVUtility.ToInt32(context.Request["CSCatCodeId"]);
                        string strJsonData = UpdateClaimStatusCategoryCode(fieldsJson, CSCatCodeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "FILL_CLAIMSTATUSCATCODE":
                    {
                        string CSCatCodeId = context.Request["CSCatCodeId"];
                        string strJsonData = FillClaimStatusCategoryCode(MDVUtility.ToInt32(CSCatCodeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }
        #endregion
    }
}