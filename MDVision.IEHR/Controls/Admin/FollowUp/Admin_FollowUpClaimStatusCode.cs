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
    public class Admin_FollowUpClaimStatusCode
    {
         private BLLAdminFollowUp BLLAdminFollowUpObj = null;
         public Admin_FollowUpClaimStatusCode()
        {
            BLLAdminFollowUpObj = new BLLAdminFollowUp();
        }

        #region Singleton
        private static Admin_FollowUpClaimStatusCode _obj = null;
        public static Admin_FollowUpClaimStatusCode Instance()
        {
            if (_obj == null)
                _obj = new Admin_FollowUpClaimStatusCode();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string LoadClaimStatusCode(string fieldsJSON, Int32 CSCodeId, int PageNumber, int RowsPerPage)
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
                    objReason = BLLAdminFollowUpObj.LoadClaimStatusCode(CSCodeId, null, null, null, PageNumber, RowsPerPage);
                else
                    objReason = BLLAdminFollowUpObj.LoadClaimStatusCode(CSCodeId, SearchedfieldsJSON["txtCode"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);

                dsClaimStatus = objReason.Data;
                if (objReason.Data != null)
                {
                    if (dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCode.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClaimStatusCodeCount = dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCode.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsClaimStatus.ClaimStatusCode.Rows.Count > 0) ? dsClaimStatus.ClaimStatusCode.Rows[0][dsClaimStatus.ClaimStatusCode.RecordCountColumn.ColumnName] : 0,
                            ClaimStatusCodeLoad_JSON = MDVUtility.JSON_DataTable(dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCode.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClaimStatusCodeCount = 0,
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
                        ClaimStatusCodeCount = 0,
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
        private string DeleteClaimStatusCode(long CSCodeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CSCodeId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteClaimStatusCode(MDVUtility.ToStr(CSCodeId));
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
        private string ClaimStatusCodeActiveInactive(Int32 CSCodeId, Int64 IsActive)
        {
            try
            {
                DSFollowUp dsClaimStatus = new DSFollowUp();

                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadClaimStatusCode(CSCodeId, null, null, null);
                dsClaimStatus = obj.Data;
                if (dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCode.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCode.TableName].Rows[0];
                    dr[dsClaimStatus.ClaimStatusCode.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.UpdateClaimStatusCode(dsClaimStatus);
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
        private string SaveClaimStatusCode(string fieldsJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSFollowUp dsClaimStatus = new DSFollowUp();
                DSFollowUp.ClaimStatusCodeRow dr = dsClaimStatus.ClaimStatusCode.NewClaimStatusCodeRow();


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
                dsClaimStatus.ClaimStatusCode.AddClaimStatusCodeRow(dr);
                BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.InsertClaimStatusCode(dsClaimStatus);
                dsClaimStatus = objType.Data;
                if (objType.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message,
                        CSCodeId = dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCode.TableName].Rows[0][dsClaimStatus.ClaimStatusCode.CSCodeIdColumn.ColumnName]
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
        private string FillClaimStatusCode(Int32 CSCodeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CSCodeId)))
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
                    BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadClaimStatusCode(CSCodeId, null, null, null);
                    if (obj.Data != null)
                    {
                        dsClaimStatus = obj.Data;
                        if (dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCode.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCode.TableName].Rows[0];

                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtCode", MDVUtility.ToStr(dr[dsClaimStatus.ClaimStatusCode.CodeColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsClaimStatus.ClaimStatusCode.DescriptionColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsClaimStatus.ClaimStatusCode.IsActiveColumn.ColumnName])},
                            { "CreatedBy", MDVUtility.ToStr(dr[dsClaimStatus.ClaimStatusCode.CreatedByColumn.ColumnName])},
                            
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ClaimStatusCodeFill_JSON = js.Serialize(keyValues)
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
        private string UpdateClaimStatusCode(string fieldsJson, Int32 CSCodeId)
        {
            try
            {
                DSFollowUp dsClaimStatus = null;
                BLObject<DSFollowUp> obj = null;
                obj = BLLAdminFollowUpObj.LoadClaimStatusCode(CSCodeId, null, null, null);
                dsClaimStatus = obj.Data;
                if (dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCode.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    foreach (DataRow dr in dsClaimStatus.Tables[dsClaimStatus.ClaimStatusCode.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("txtCode") && !string.IsNullOrEmpty(searchedfieldsJson["txtCode"]))
                            dr[dsClaimStatus.ClaimStatusCode.CodeColumn] = MDVUtility.ToStr(searchedfieldsJson["txtCode"]);
                        if (searchedfieldsJson.ContainsKey("txtDescription") && !string.IsNullOrEmpty(searchedfieldsJson["txtDescription"]))
                            dr[dsClaimStatus.ClaimStatusCode.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["txtDescription"]);
                        if (searchedfieldsJson.ContainsKey("chkActive"))
                            dr[dsClaimStatus.ClaimStatusCode.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True" ? true : false;

                        //dr[dsClaimStatus.ClaimStatusCode.CreatedByColumn] = MDVUtility.ToStr(dr[dsClaimStatus.ClaimStatusCode.CreatedByColumn.ColumnName]);
                        dr[dsClaimStatus.ClaimStatusCode.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr[dsClaimStatus.ClaimStatusCode.CreatedOnColumn] = DateTime.Now;
                        dr[dsClaimStatus.ClaimStatusCode.ModifiedOnColumn] = DateTime.Now;
                    }
                    BLObject<DSFollowUp> objType = null;
                    objType = BLLAdminFollowUpObj.UpdateClaimStatusCode(dsClaimStatus);

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
                case "SEARCH_CLAIMSTATUSCODE":
                    {
                        string fieldsJSON = context.Request["RemittanceData"];
                        Int32 CSCodeId = MDVUtility.ToInt32(context.Request["CSCodeId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadClaimStatusCode(fieldsJSON, CSCodeId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_CLAIMSTATUSCODE":
                    {
                        Int64 CSCodeId = MDVUtility.ToInt64(context.Request["CSCodeId"]);
                        string strJSONData = DeleteClaimStatusCode(CSCodeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_CLAIMSTATUSCODE_ACTIVE_INACTIVE":
                    {
                        Int32 CSCodeId = MDVUtility.ToInt32(context.Request["CSCodeId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = ClaimStatusCodeActiveInactive(CSCodeId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_CLAIMSTATUSCODE":
                    {
                        string fieldsJson = context.Request["ClaimStatusCodeData"];
                        string strJsonData = SaveClaimStatusCode(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_CLAIMSTATUSCODE":
                    {
                        string fieldsJson = context.Request["ClaimStatusCodeData"];
                        Int32 CSCodeId = MDVUtility.ToInt32(context.Request["CSCodeId"]);
                        string strJsonData = UpdateClaimStatusCode(fieldsJson, CSCodeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "FILL_CLAIMSTATUSCODE":
                    {
                        string CSCodeId = context.Request["CSCodeId"];
                        string strJsonData = FillClaimStatusCode(MDVUtility.ToInt32(CSCodeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }
        #endregion
    }
}