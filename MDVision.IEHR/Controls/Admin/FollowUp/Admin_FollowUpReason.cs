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
    public class Admin_FollowUpReason
    {
        private BLLAdminFollowUp BLLAdminFollowUpObj = null;
        public Admin_FollowUpReason()
        {
            BLLAdminFollowUpObj = new BLLAdminFollowUp();
        }
        #region Singleton
        private static Admin_FollowUpReason _obj = null;
        public static Admin_FollowUpReason Instance()
        {
            if (_obj == null)
                _obj = new Admin_FollowUpReason();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Loads The Reason
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        private string LoadReason(string fieldsJSON, Int64 reasonId, int PageNumber, int RowsPerPage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSFollowUp dsReason = null;
                BLObject<DSFollowUp> objReason;
                var ddlARType = "";
                if (SearchedfieldsJSON.ContainsKey("ddlARType"))
                    ddlARType = SearchedfieldsJSON["ddlARType"];
                else
                    ddlARType = null;

                if (SearchedfieldsJSON == null)
                    objReason = BLLAdminFollowUpObj.LoadFollowUpReasons(reasonId, null, null, null, null);
                else
                    objReason = BLLAdminFollowUpObj.LoadFollowUpReasons(
                        reasonId,
                        SearchedfieldsJSON["txtShortName"],
                        SearchedfieldsJSON["txtDiscription"],
                        ddlARType,
                        SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage
                        );

                dsReason = objReason.Data;
                if (objReason.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ReasonCount = dsReason.Tables[dsReason.FollowupReasons.TableName].Rows.Count,
                        iTotalDisplayRecords = (dsReason.FollowupReasons.Rows.Count > 0) ? dsReason.FollowupReasons.Rows[0][dsReason.FollowupReasons.RecordCountColumn.ColumnName] : 0,
                        ReasonLoad_JSON = MDVUtility.JSON_DataTable(dsReason.Tables[dsReason.FollowupReasons.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ReasonCount = 0,
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

        /// <summary>
        /// Delete the Reason
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        private string DeleteReason(long reasonId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(reasonId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteFollowUpReason(MDVUtility.ToStr(reasonId));
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


        private string ReasonUpdateActiveInactive(Int64 reasonId, Int64 IsActive)
        {
            try
            {
                DSFollowUp dsReason = new DSFollowUp();

                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadFollowUpReasons(reasonId, null, null, null, null);
                dsReason = obj.Data;
                if (dsReason.Tables[dsReason.FollowupReasons.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsReason.Tables[dsReason.FollowupReasons.TableName].Rows[0];
                    dr[dsReason.FollowupReasons.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSFollowUp> objReason = BLLAdminFollowUpObj.UpdateFollowUpReason(dsReason);
                    string successMsg;
                    if (objReason.Data != null)
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
                            Message = objReason.Message
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

        private string SaveReason(string fieldsJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSFollowUp dsReason = new DSFollowUp();
                DSFollowUp.FollowupReasonsRow dr = dsReason.FollowupReasons.NewFollowupReasonsRow();

                if (!string.IsNullOrEmpty(searchedfieldsJson["hfReasonId"]))
                    dr.ReasonId = MDVUtility.ToInt64(searchedfieldsJson["hfReasonId"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["ShortName"]))
                    dr.ShortName = MDVUtility.ToStr(searchedfieldsJson["ShortName"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["Description"]))
                    dr.Description = MDVUtility.ToStr(searchedfieldsJson["Description"]);

                dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["Active"]) == "True";

                if (!string.IsNullOrEmpty(searchedfieldsJson["lstARTypeId"]))
                    dr.ARTypeId = MDVUtility.ToInt64(searchedfieldsJson["lstARTypeId"]);
                else
                    dr[dsReason.FollowupReasons.ARTypeIdColumn] = DBNull.Value;

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;


                #region Database Insertion
                dsReason.FollowupReasons.AddFollowupReasonsRow(dr);
                BLObject<DSFollowUp> objReason = BLLAdminFollowUpObj.InsertFollowupReason(dsReason);
                dsReason = objReason.Data;
                if (objReason.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message,
                        ReasonId = dsReason.Tables[dsReason.FollowupReasons.TableName].Rows[0][dsReason.FollowupReasons.ReasonIdColumn.ColumnName]
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objReason.Message
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

        private string FillReason(Int64 reasonId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(reasonId)))
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
                    DSFollowUp dsReason = null;
                    BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadFollowUpReasons(reasonId, null, null, null, null);
                    if (obj.Data != null)
                    {
                        dsReason = obj.Data;
                        if (dsReason.Tables[dsReason.FollowupReasons.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsReason.Tables[dsReason.FollowupReasons.TableName].Rows[0];

                            var keyValues = new Dictionary<string, string>
                        {
                            { "ShortName", MDVUtility.ToStr(dr[dsReason.FollowupReasons.ShortNameColumn.ColumnName])},
                            { "Description", MDVUtility.ToStr(dr[dsReason.FollowupReasons.DescriptionColumn.ColumnName])},
                            { "lstARTypeId", MDVUtility.ToStr(dr[dsReason.FollowupReasons.ARTypeIdColumn.ColumnName])},
                            { "Active", MDVUtility.ToStr(dr[dsReason.FollowupReasons.IsActiveColumn.ColumnName])},
                            
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ReasonLoad_JSON = js.Serialize(keyValues)
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

        private string UpdateReason(string fieldsJson, Int64 reasonId)
        {
            try
            {
                DSFollowUp dsReason = null;
                BLObject<DSFollowUp> obj = null;
                obj = BLLAdminFollowUpObj.LoadFollowUpReasons(reasonId, null, null, null, null);
                dsReason = obj.Data;
                if (dsReason.Tables[dsReason.FollowupReasons.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    foreach (DataRow dr in dsReason.Tables[dsReason.FollowupReasons.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("ShortName") && !string.IsNullOrEmpty(searchedfieldsJson["ShortName"]))
                            dr[dsReason.FollowupReasons.ShortNameColumn] = MDVUtility.ToStr(searchedfieldsJson["ShortName"]);
                        if (searchedfieldsJson.ContainsKey("Description") && !string.IsNullOrEmpty(searchedfieldsJson["Description"]))
                            dr[dsReason.FollowupReasons.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["Description"]);
                        if (searchedfieldsJson.ContainsKey("lstARTypeId") && !string.IsNullOrEmpty(searchedfieldsJson["lstARTypeId"]))
                            dr[dsReason.FollowupReasons.ARTypeIdColumn] = MDVUtility.ToStr(searchedfieldsJson["lstARTypeId"]);
                        if (searchedfieldsJson.ContainsKey("Active"))
                            dr[dsReason.FollowupReasons.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["Active"]) == "True" ? true : false;

                        dr[dsReason.FollowupReasons.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsReason.FollowupReasons.ModifiedOnColumn] = DateTime.Now;
                    }
                    BLObject<DSFollowUp> objReason = null;
                    objReason = BLLAdminFollowUpObj.UpdateFollowUpReason(dsReason);

                    if (objReason.Data != null)
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
                            Message = objReason.Message
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

        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_REASON":
                    {
                        string fieldsJSON = context.Request["reasonData"];
                        Int64 reasonId = MDVUtility.ToInt64(context.Request["reasonId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadReason(fieldsJSON, reasonId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_REASON":
                    {
                        Int64 reasonId = MDVUtility.ToInt64(context.Request["reasonId"]);
                        string strJSONData = DeleteReason(reasonId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_REASON_ACTIVE_INACTIVE":
                    {
                        Int64 reasonId = MDVUtility.ToInt64(context.Request["reasonId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = ReasonUpdateActiveInactive(reasonId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_REASON":
                    {
                        string fieldsJson = context.Request["reasonData"];
                        string strJsonData = SaveReason(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_REASON":
                    {
                        string fieldsJson = context.Request["reasonData"];
                        Int64 reasonId = MDVUtility.ToInt64(context.Request["reasonId"]);
                        string strJsonData = UpdateReason(fieldsJson, reasonId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "FILL_REASON":
                    {
                        string reasonId = context.Request["reasonId"];
                        string strJsonData = FillReason(MDVUtility.ToInt64(reasonId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }
        #endregion

    }
}