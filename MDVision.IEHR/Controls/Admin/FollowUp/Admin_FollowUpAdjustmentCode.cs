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
    public class Admin_FollowUpAdjustmentCode
    {
         private BLLAdminFollowUp BLLAdminFollowUpObj = null;
         public Admin_FollowUpAdjustmentCode()
        {
            BLLAdminFollowUpObj = new BLLAdminFollowUp();
        }
        #region Singleton
        private static Admin_FollowUpAdjustmentCode _obj = null;
        public static Admin_FollowUpAdjustmentCode Instance()
        {
            if (_obj == null)
                _obj = new Admin_FollowUpAdjustmentCode();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string LoadAdjustmentCode(string fieldsJSON, Int32 AdjustmentId, int PageNumber, int RowsPerPage)
        {
           // string isActive = "";
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSFollowUp dsAdjustment = null;
                BLObject<DSFollowUp> objReason;

                //if (SearchedfieldsJSON["chkActive"] == true)
                //    isActive = "1";
                //if (SearchedfieldsJSON["chkActive"] == false)
                //    isActive = "0";

                if (SearchedfieldsJSON == null)
                    objReason = BLLAdminFollowUpObj.LoadAdjustmentCode(AdjustmentId, null, null, null, PageNumber, RowsPerPage);
                else
                    objReason = BLLAdminFollowUpObj.LoadAdjustmentCode(AdjustmentId, SearchedfieldsJSON["txtCode"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);

                dsAdjustment = objReason.Data;
                if (objReason.Data != null)
                {
                    if (dsAdjustment.Tables[dsAdjustment.AdjustmentReasonCode.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            AdjustmentCodeCount = dsAdjustment.Tables[dsAdjustment.AdjustmentReasonCode.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsAdjustment.AdjustmentReasonCode.Rows.Count > 0) ? dsAdjustment.AdjustmentReasonCode.Rows[0][dsAdjustment.AdjustmentReasonCode.RecordCountColumn.ColumnName] : 0,
                            AdjustmentCodeLoad_JSON = MDVUtility.JSON_DataTable(dsAdjustment.Tables[dsAdjustment.AdjustmentReasonCode.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AdjustmentCodeCount = 0,
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
                        AdjustmentCodeCount = 0,
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
        private string DeleteAdjustmentCode(long AdjustmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AdjustmentId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteAdjustmentCode(MDVUtility.ToStr(AdjustmentId));
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
        private string AdjustmentCodeActiveInactive(Int32 AdjustmentId, Int64 IsActive)
        {
            try
            {
                DSFollowUp dsAdjustment = new DSFollowUp();

                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadAdjustmentCode(AdjustmentId, null, null, null);
                dsAdjustment = obj.Data;
                if (dsAdjustment.Tables[dsAdjustment.AdjustmentReasonCode.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsAdjustment.Tables[dsAdjustment.AdjustmentReasonCode.TableName].Rows[0];
                    dr[dsAdjustment.AdjustmentReasonCode.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.UpdateAdjustmentCode(dsAdjustment);
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
        private string SaveAdjustmentCode(string fieldsJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSFollowUp dsAdjustment = new DSFollowUp();
                DSFollowUp.AdjustmentReasonCodeRow dr = dsAdjustment.AdjustmentReasonCode.NewAdjustmentReasonCodeRow();


                if (!string.IsNullOrEmpty(searchedfieldsJson["txtCode"]))
                    dr.Code = MDVUtility.ToStr(searchedfieldsJson["txtCode"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["txtDescription"]))
                    dr.Description = MDVUtility.ToStr(searchedfieldsJson["txtDescription"]);

                dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True";

                //dr.IsActive =  true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsAdjustment.AdjustmentReasonCode.AddAdjustmentReasonCodeRow(dr);
                BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.InsertAdjustmentCode(dsAdjustment);
                dsAdjustment = objType.Data;
                if (objType.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message,
                        AdjustmentId = dsAdjustment.Tables[dsAdjustment.AdjustmentReasonCode.TableName].Rows[0][dsAdjustment.AdjustmentReasonCode.AdjustmentIdColumn.ColumnName]
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
        private string FillAdjustmentCode(Int32 AdjustmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AdjustmentId)))
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
                    DSFollowUp dsAdjustment = null;
                    BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadAdjustmentCode(AdjustmentId, null, null, null);
                    if (obj.Data != null)
                    {
                        dsAdjustment = obj.Data;
                        if (dsAdjustment.Tables[dsAdjustment.AdjustmentReasonCode.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsAdjustment.Tables[dsAdjustment.AdjustmentReasonCode.TableName].Rows[0];

                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtCode", MDVUtility.ToStr(dr[dsAdjustment.AdjustmentReasonCode.CodeColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsAdjustment.AdjustmentReasonCode.DescriptionColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsAdjustment.AdjustmentReasonCode.IsActiveColumn.ColumnName])},
                            { "CreatedBy", MDVUtility.ToStr(dr[dsAdjustment.AdjustmentReasonCode.CreatedByColumn.ColumnName])},
                            
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                AdjustmentCodeFill_JSON = js.Serialize(keyValues)
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

        private string UpdateAdjustmentCode(string fieldsJson, Int32 AdjustmentId)
        {
            try
            {
                DSFollowUp dsAdjustment = null;
                BLObject<DSFollowUp> obj = null;
                obj = BLLAdminFollowUpObj.LoadAdjustmentCode(AdjustmentId, null, null, null);
                dsAdjustment = obj.Data;
                if (dsAdjustment.Tables[dsAdjustment.AdjustmentReasonCode.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    foreach (DataRow dr in dsAdjustment.Tables[dsAdjustment.AdjustmentReasonCode.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("txtCode") && !string.IsNullOrEmpty(searchedfieldsJson["txtCode"]))
                            dr[dsAdjustment.AdjustmentReasonCode.CodeColumn] = MDVUtility.ToStr(searchedfieldsJson["txtCode"]);
                        if (searchedfieldsJson.ContainsKey("txtDescription") && !string.IsNullOrEmpty(searchedfieldsJson["txtDescription"]))
                            dr[dsAdjustment.AdjustmentReasonCode.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["txtDescription"]);
                        if (searchedfieldsJson.ContainsKey("chkActive"))
                            dr[dsAdjustment.AdjustmentReasonCode.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True" ? true : false;

                        // dr[dsAdjustment.AdjustmentCode.IsActiveColumn] = MDVUtility.ToStr(dr[dsAdjustment.AdjustmentCode.IsActiveColumn.ColumnName]); ;
                        //dr[dsAdjustment.AdjustmentCode.CreatedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsAdjustment.AdjustmentReasonCode.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr[dsAdjustment.AdjustmentCode.CreatedOnColumn] = DateTime.Now;
                        dr[dsAdjustment.AdjustmentReasonCode.ModifiedOnColumn] = DateTime.Now;
                    }
                    BLObject<DSFollowUp> objType = null;
                    objType = BLLAdminFollowUpObj.UpdateAdjustmentCode(dsAdjustment);

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
                case "SEARCH_ADJUSTMENTCODE":
                    {
                        string fieldsJSON = context.Request["AdjustmentData"];
                        Int32 AdjustmentId = MDVUtility.ToInt32(context.Request["AdjustmentId"]);
                        int pageNo = MDVUtility.ToInt(context.Request["pageNo"]);
                        int recordPerPage = MDVUtility.ToInt(context.Request["recordPerPage"]);
                        string strJSONData = LoadAdjustmentCode(fieldsJSON, AdjustmentId, pageNo, recordPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_ADJUSTMENTCODE":
                    {
                        Int64 AdjustmentId = MDVUtility.ToInt64(context.Request["AdjustmentId"]);
                        string strJSONData = DeleteAdjustmentCode(AdjustmentId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_ADJUSTMENTCODE_ACTIVE_INACTIVE":
                    {
                        Int32 AdjustmentId = MDVUtility.ToInt32(context.Request["AdjustmentId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = AdjustmentCodeActiveInactive(AdjustmentId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_ADJUSTMENTCODE":
                    {
                        string fieldsJson = context.Request["AdjustmentCodeData"];
                        string strJsonData = SaveAdjustmentCode(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_ADJUSTMENTCODE":
                    {
                        string fieldsJson = context.Request["AdjustmentCodeData"];
                        Int32 AdjustmentId = MDVUtility.ToInt32(context.Request["AdjustmentId"]);
                        string strJsonData = UpdateAdjustmentCode(fieldsJson, AdjustmentId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "FILL_ADJUSTMENTCODE":
                    {
                        string AdjustmentId = context.Request["AdjustmentId"];
                        string strJsonData = FillAdjustmentCode(MDVUtility.ToInt32(AdjustmentId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }
        #endregion
    }
}