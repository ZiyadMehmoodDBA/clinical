using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_FeeGroup_Detail
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_FeeGroup_Detail()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_FeeGroup_Detail _obj = null;
        public static Admin_FeeGroup_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_FeeGroup_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the Fee Group.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveFeeGroup(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeGroup = new DSFeeSchedule();
                    DSFeeSchedule.FeeGroupRow dr = dsFeeGroup.FeeGroup.NewFeeGroupRow();

                    dr.ShortName = SearchedfieldsJSON["txtName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtEndDate"])))
                        dr.EndDate = MDVUtility.ToDateTime(SearchedfieldsJSON["txtEndDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlNextFeeGroup"]))
                        dr.NextFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlNextFeeGroup"]);

                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                    #region Database Insertion
                    dsFeeGroup.FeeGroup.AddFeeGroupRow(dr);
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.InsertFeeGroup(ref dsFeeGroup);
                    dsFeeGroup = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            FeeGroupId = dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName].Rows[0][dsFeeGroup.FeeGroup.FeeGroupIdColumn.ColumnName]
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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

        /// <summary>
        /// Updates the FeeGroup Link.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProcedureCategoryId">The FeeGroup identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateFeeGroup(string fieldsJSON, Int64 FeeGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeGroup = new DSFeeSchedule();
                    //DSFeeSchedule.FeeGroupRow dr = dsFeeGroup.FeeGroup.NewFeeGroupRow();
                    BLObject<DSFeeSchedule> objLoad = BLLFeeScheduleObj.LoadFeeGroup(FeeGroupId, null, null, null, null);
                    dsFeeGroup = objLoad.Data;
                    foreach (DSFeeSchedule.FeeGroupRow dr in dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName].Rows)
                    {
                        //dr.FeeGroupId = FeeGroupId;
                        dr.ShortName = SearchedfieldsJSON["txtName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtEndDate"])))
                            dr.EndDate = MDVUtility.ToDateTime(SearchedfieldsJSON["txtEndDate"]);
                        else
                            dr[dsFeeGroup.FeeGroup.EndDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlNextFeeGroup"]))
                            dr.NextFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlNextFeeGroup"]);
                        else
                            dr.NextFeeGroupId = MDVUtility.ToInt64(0);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    }

                    #region Database Updation
                    //dsFeeGroup.FeeGroup.AddFeeGroupRow(dr);
                    //dsFeeGroup.FeeGroup.AcceptChanges();

                    if (dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName].Rows.Count > 0)
                    {
                        //dsFeeGroup.FeeGroup.Rows[0].SetModified();
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.UpdateFeeGroup(ref dsFeeGroup);
                        if (obj.Data != null)
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
                                Message = obj.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ""
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
                        Message = privilegesMessage
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

        /// <summary>
        /// Fills the FeeGroup.
        /// </summary>
        /// <param name="ProcedureCategoryId">The Plan Fee Link identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillFeeGroup(Int64 FeeGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(FeeGroupId)))
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
                        DSFeeSchedule dsFeeGroup = null;
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadFeeGroup(FeeGroupId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsFeeGroup = obj.Data;
                            if (dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "txtName", MDVUtility.ToStr(dr[dsFeeGroup.FeeGroup.ShortNameColumn.ColumnName])},
                          { "txtDescription", MDVUtility.ToStr(dr[dsFeeGroup.FeeGroup.DescriptionColumn.ColumnName])},
                          //{ "txtEndDate", MDVUtility.ToStr(dr[dsFeeGroup.FeeGroup.EndDateColumn.ColumnName])},
                          { "txtEndDate", MDVUtility.ToStr(dr[dsFeeGroup.FeeGroup.EndDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsFeeGroup.FeeGroup.EndDateColumn.ColumnName]).ToShortDateString():""},
                          { "ddlNextFeeGroup", MDVUtility.ToStr(dr[dsFeeGroup.FeeGroup.NextFeeGroupIdColumn.ColumnName])},
                          { "ChkIsActive", MDVUtility.ToStr(dr[dsFeeGroup.FeeGroup.IsActiveColumn.ColumnName])},
                          { "ddlEntity", MDVUtility.ToStr(dr[dsFeeGroup.FeeGroup.EntityIdColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    FeeGroupFill_JSON = js.Serialize(keyValues)
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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


        /// <summary>
        /// Fills the next fee group.
        /// </summary>
        /// <param name="EntityID">The entity identifier.</param>
        /// <returns>System.String.</returns>
        private string FillNextFeeGroup(string EntityID)
        {
            try
            {
                DSFeeSchedule dsFeeGroup = null;
                BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadFeeGroup(0, null, null, EntityID, null);
                dsFeeGroup = obj.Data;
                var response = new
                {
                    status = true,
                    NextFeeGroupCount = dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName].Rows.Count,
                    NextFeeGroupLoad_JSON = MDVUtility.JSON_DataTable(dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName]),
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
        /// Deletes the FeeGroup.
        /// </summary>
        /// <param name="ProcedureCategoryId">The Plan Fee Link identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteFeeGroup(Int64 FeeGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(FeeGroupId)))
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
                        BLObject<string> obj = BLLFeeScheduleObj.DeleteFeeGroup(MDVUtility.ToStr(FeeGroupId));
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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

        private string UpdateFeeGroupIsActive(Int64 FeeGroupId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSFeeSchedule dsFeeGroup = null;
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadFeeGroup(FeeGroupId, null, null, null, null);
                    dsFeeGroup = obj.Data;
                    if (dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName].Rows[0];
                        dr[dsFeeGroup.FeeGroup.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSFeeSchedule> objUser = BLLFeeScheduleObj.UpdateFeeGroup(ref dsFeeGroup);
                        string successMsg;
                        if (objUser.Data != null)
                        {
                            if (IsActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objUser.Message
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Plan Fee Link Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_FEE_GROUP":
                    {
                        string fieldsJSON = context.Request["FeeGroupData"];
                        string strJSONData = SaveFeeGroup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_FEE_GROUP":
                    {
                        string strFeeGroupId = context.Request["FeeGroupID"];
                        string strJSONData = FillFeeGroup(MDVUtility.ToInt64(strFeeGroupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_NEXT_FEE_GROUP":
                    {
                        string strEntityId = context.Request["EntityID"];
                        string strJSONData = FillNextFeeGroup(strEntityId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_FEE_GROUP":
                    {
                        string strFeeGroupId = context.Request["FeeGroupID"];
                        string strJSONData = DeleteFeeGroup(MDVUtility.ToInt64(strFeeGroupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_FEE_GROUP":
                    {
                        string fieldsJSON = context.Request["FeeGroupData"];
                        Int64 FeeGroupID = MDVUtility.ToInt64(context.Request["FeeGroupID"]);
                        string strJSONData = UpdateFeeGroup(fieldsJSON, FeeGroupID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_FEE_GROUP_ACTIVE_INACTIVE":
                    {
                        Int64 FeeGroupID = MDVUtility.ToInt64(context.Request["FeeGroupID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateFeeGroupIsActive(FeeGroupID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}