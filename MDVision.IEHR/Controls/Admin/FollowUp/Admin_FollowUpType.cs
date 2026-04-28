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
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Admin.FollowUp
{
    public class Admin_FollowUpType
    {
        private BLLAdminFollowUp BLLAdminFollowUpObj = null;
        public Admin_FollowUpType()
        {
            BLLAdminFollowUpObj = new BLLAdminFollowUp();
        }

        #region Singleton
        private static Admin_FollowUpType _obj = null;
        public static Admin_FollowUpType Instance()
        {
            if (_obj == null)
                _obj = new Admin_FollowUpType();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Loads The Type
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        private string LoadType(string fieldsJSON, long TypeId, int PageNumber, int RowsPerPage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSFollowUp dsType = null;
                BLObject<DSFollowUp> objType;

                if (SearchedfieldsJSON == null)
                    objType = BLLAdminFollowUpObj.LoadFollowUpARTypes(TypeId, null, null, null);
                else
                    objType = BLLAdminFollowUpObj.LoadFollowUpARTypes(
                        TypeId,
                        SearchedfieldsJSON["txtShortName"],
                        SearchedfieldsJSON["txtDescription"],
                        SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage
                        );

                dsType = objType.Data;
                if (objType.Data != null)
                {
                    if (dsType.Tables[dsType.ARType.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            TypeCount = dsType.Tables[dsType.ARType.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsType.ARType.Rows.Count > 0) ? dsType.ARType.Rows[0][dsType.ARType.RecordCountColumn.ColumnName] : 0,
                            TypeLoad_JSON = MDVUtility.JSON_DataTable(dsType.Tables[dsType.ARType.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            TypeCount = 0,
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
                        TypeCount = 0,
                        Message = objType.Message
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
        /// Delete the Type
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        private string DeleteType(long TypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(TypeId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteFollowUpARType(MDVUtility.ToStr(TypeId));
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

        /// <summary>
        /// Set Active Inactive Status
        /// </summary>
        /// <param name="TypeId"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        private string TypeUpdateActiveInactive(Int64 TypeId, Int64 IsActive)
        {
            try
            {
                DSFollowUp dsType = new DSFollowUp();

                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadFollowUpARTypes(TypeId, null, null, null);
                dsType = obj.Data;
                if (dsType.Tables[dsType.ARType.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsType.Tables[dsType.ARType.TableName].Rows[0];
                    dr[dsType.ARType.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.UpdateFollowUpARType(dsType);
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

        /// <summary>
        /// Save the ARType
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <returns></returns>
        private string SaveType(string fieldsJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSFollowUp dsType = new DSFollowUp();
                DSFollowUp.ARTypeRow dr = dsType.ARType.NewARTypeRow();

                if (!string.IsNullOrEmpty(searchedfieldsJson["hfTypeId"]))
                    dr.ARTypeId = MDVUtility.ToInt64(searchedfieldsJson["hfTypeId"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["ShortName"]))
                    dr.ShortName = MDVUtility.ToStr(searchedfieldsJson["ShortName"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["Description"]))
                    dr.Description = MDVUtility.ToStr(searchedfieldsJson["Description"]);

                dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["Active"]) == "True";


                #region Database Insertion
                dsType.ARType.AddARTypeRow(dr);
                BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.InsertFollowUpARType(dsType);
                dsType = objType.Data;
                if (objType.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message,
                        TypeId = dsType.Tables[dsType.ARType.TableName].Rows[0][dsType.ARType.ARTypeIdColumn.ColumnName]
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

        /// <summary>
        /// Fill the ARType
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        private string FillType(Int64 TypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(TypeId)))
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
                    DSFollowUp dsType = null;
                    BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadFollowUpARTypes(TypeId, null, null, null);
                    if (obj.Data != null)
                    {
                        dsType = obj.Data;
                        if (dsType.Tables[dsType.ARType.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsType.Tables[dsType.ARType.TableName].Rows[0];

                            var keyValues = new Dictionary<string, string>
                        {
                            { "ShortName", MDVUtility.ToStr(dr[dsType.ARType.ShortNameColumn.ColumnName])},
                            { "Description", MDVUtility.ToStr(dr[dsType.ARType.DescriptionColumn.ColumnName])},
                            { "Active", MDVUtility.ToStr(dr[dsType.ARType.IsActiveColumn.ColumnName])},
                            
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                TypeLoad_JSON = js.Serialize(keyValues)
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

        /// <summary>
        /// Update thr ARType
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        private string UpdateType(string fieldsJson, Int64 TypeId)
        {
            try
            {
                DSFollowUp dsType = null;
                BLObject<DSFollowUp> obj = null;
                obj = BLLAdminFollowUpObj.LoadFollowUpARTypes(TypeId, null, null, null);
                dsType = obj.Data;
                if (dsType.Tables[dsType.ARType.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    foreach (DataRow dr in dsType.Tables[dsType.ARType.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("ShortName") && !string.IsNullOrEmpty(searchedfieldsJson["ShortName"]))
                            dr[dsType.ARType.ShortNameColumn] = MDVUtility.ToStr(searchedfieldsJson["ShortName"]);
                        if (searchedfieldsJson.ContainsKey("Description") && !string.IsNullOrEmpty(searchedfieldsJson["Description"]))
                            dr[dsType.ARType.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["Description"]);
                        if (searchedfieldsJson.ContainsKey("Active"))
                            dr[dsType.ARType.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["Active"]) == "True" ? true : false;
                    }
                    BLObject<DSFollowUp> objType = null;
                    objType = BLLAdminFollowUpObj.UpdateFollowUpARType(dsType);

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

        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_TYPE":
                    {
                        string fieldsJSON = context.Request["TypeData"];
                        Int64 TypeId = MDVUtility.ToInt64(context.Request["TypeId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadType(fieldsJSON, TypeId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_TYPE":
                    {
                        Int64 TypeId = MDVUtility.ToInt64(context.Request["TypeId"]);
                        string strJSONData = DeleteType(TypeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_TYPE_ACTIVE_INACTIVE":
                    {
                        Int64 TypeId = MDVUtility.ToInt64(context.Request["TypeId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = TypeUpdateActiveInactive(TypeId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_TYPE":
                    {
                        string fieldsJson = context.Request["TypeData"];
                        string strJsonData = SaveType(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_TYPE":
                    {
                        string fieldsJson = context.Request["TypeData"];
                        Int64 TypeId = MDVUtility.ToInt64(context.Request["TypeId"]);
                        string strJsonData = UpdateType(fieldsJson, TypeId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "FILL_TYPE":
                    {
                        string TypeId = context.Request["TypeId"];
                        string strJsonData = FillType(MDVUtility.ToInt64(TypeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }
        #endregion
    }
}