using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_BasicFeeGroup_Detail
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_BasicFeeGroup_Detail()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_BasicFeeGroup_Detail _obj = null;
        public static Admin_BasicFeeGroup_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_BasicFeeGroup_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the Fee Group.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveBasicFeeGroup(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Group", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsBasicFeeGroup = new DSFeeSchedule();
                    DSFeeSchedule.BasicFeeGroupRow dr = dsBasicFeeGroup.BasicFeeGroup.NewBasicFeeGroupRow();

                    dr.ShortName = SearchedfieldsJSON["txtName"].Trim();
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtEndDate"])))
                        dr.EndDate = MDVUtility.ToDateTime(SearchedfieldsJSON["txtEndDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlNextBasicFeeGroup"]))
                        dr.NextBasicFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlNextBasicFeeGroup"]);

                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                    #region Database Insertion
                    dsBasicFeeGroup.BasicFeeGroup.AddBasicFeeGroupRow(dr);
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.InsertBasicFeeGroup(ref dsBasicFeeGroup);
                    dsBasicFeeGroup = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            BasicFeeGroupId = dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows[0][dsBasicFeeGroup.BasicFeeGroup.BasicFeeGroupIdColumn.ColumnName]
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
        /// Updates the BasicFeeGroup Link.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProcedureCategoryId">The BasicFeeGroup identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateBasicFeeGroup(string fieldsJSON, Int64 BasicFeeGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsBasicFeeGroup = new DSFeeSchedule();
                    //DSFeeSchedule.BasicFeeGroupRow dr = dsBasicFeeGroup.BasicFeeGroup.NewBasicFeeGroupRow();
                    BLObject<DSFeeSchedule> objLoad = BLLFeeScheduleObj.LoadBasicFeeGroup(BasicFeeGroupId, null, null, null, null);
                    dsBasicFeeGroup = objLoad.Data;
                    foreach (DSFeeSchedule.BasicFeeGroupRow dr in dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows)
                    {
                        //dr.BasicFeeGroupId = BasicFeeGroupId;
                        dr.ShortName = SearchedfieldsJSON["txtName"].Trim();
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtEndDate"])))
                            dr.EndDate = MDVUtility.ToDateTime(SearchedfieldsJSON["txtEndDate"]);
                        else
                            dr[dsBasicFeeGroup.BasicFeeGroup.EndDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlNextBasicFeeGroup"]))
                            dr.NextBasicFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlNextBasicFeeGroup"]);
                        else
                            dr.NextBasicFeeGroupId = MDVUtility.ToInt64(0);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    }

                    #region Database Updation
                    //dsBasicFeeGroup.BasicFeeGroup.AddBasicFeeGroupRow(dr);
                    //dsBasicFeeGroup.BasicFeeGroup.AcceptChanges();

                    if (dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows.Count > 0)
                    {
                        //dsBasicFeeGroup.BasicFeeGroup.Rows[0].SetModified();
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.UpdateBasicFeeGroup(ref dsBasicFeeGroup);
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
        /// Fills the BasicFeeGroup.
        /// </summary>
        /// <param name="BasicFeeGroup">The Plan Fee Link identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillBasicFeeGroup(Int64 BasicFeeGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Group", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(BasicFeeGroupId)))
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
                        DSFeeSchedule dsBasicFeeGroup = null;
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadBasicFeeGroup(BasicFeeGroupId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsBasicFeeGroup = obj.Data;
                            if (dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "txtName", MDVUtility.ToStr(dr[dsBasicFeeGroup.BasicFeeGroup.ShortNameColumn.ColumnName])},
                          { "txtDescription", MDVUtility.ToStr(dr[dsBasicFeeGroup.BasicFeeGroup.DescriptionColumn.ColumnName])},
                          //{ "txtEndDate", MDVUtility.ToStr(dr[dsBasicFeeGroup.BasicFeeGroup.EndDateColumn.ColumnName])},
                          { "txtEndDate", MDVUtility.ToStr(dr[dsBasicFeeGroup.BasicFeeGroup.EndDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsBasicFeeGroup.BasicFeeGroup.EndDateColumn.ColumnName]).ToShortDateString():""},
                          { "ddlNextBasicFeeGroup", MDVUtility.ToStr(dr[dsBasicFeeGroup.BasicFeeGroup.NextBasicFeeGroupIdColumn.ColumnName])},
                          { "ChkIsActive", MDVUtility.ToStr(dr[dsBasicFeeGroup.BasicFeeGroup.IsActiveColumn.ColumnName])},
                          { "ddlEntity", MDVUtility.ToStr(dr[dsBasicFeeGroup.BasicFeeGroup.EntityIdColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    BasicFeeGroupFill_JSON = js.Serialize(keyValues)
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {

                                var response = new
                                {
                                    status = false,
                                    Message = AppPrivileges.No_Record_Message
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
        /// Fills the next basic fee group.
        /// </summary>
        /// <param name="EntityID">The entity identifier.</param>
        /// <returns>System.String.</returns>
        private string FillNextBasicFeeGroup(Int64 EntityID)
        {
            try
            {
                DSFeeSchedule dsBasicFeeGroup = null;
                BLObject<DSFeeSchedule> obj;
                obj = BLLFeeScheduleObj.LoadBasicFeeGroup(0, null, null, EntityID.ToString(), null);

                dsBasicFeeGroup = obj.Data;
                var response = new
                {
                    status = true,
                    BasicFeeGroupCount = dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows.Count,
                    NextBasicFeeGroupLoad_JSON = MDVUtility.JSON_DataTable(dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName]),
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
        /// Deletes the BasicFeeGroup.
        /// </summary>
        /// <param name="ProcedureCategoryId">The Plan Fee Link identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteBasicFeeGroup(Int64 BasicFeeGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Group", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(BasicFeeGroupId)))
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
                        BLObject<string> obj = BLLFeeScheduleObj.DeleteBasicFeeGroup(MDVUtility.ToStr(BasicFeeGroupId));
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

        private string UpdateBasicFeeGroupIsActive(Int64 BasicFeeGroupId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSFeeSchedule dsBasicFeeGroup = null;
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadBasicFeeGroup(BasicFeeGroupId, null, null, null, null);
                    dsBasicFeeGroup = obj.Data;
                    if (dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows[0];
                        dr[dsBasicFeeGroup.BasicFeeGroup.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSFeeSchedule> objUser = BLLFeeScheduleObj.UpdateBasicFeeGroup(ref dsBasicFeeGroup);
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
                case "SAVE_BASIC_FEE_GROUP":
                    {
                        string fieldsJSON = context.Request["BasicFeeGroupData"];
                        string strJSONData = SaveBasicFeeGroup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_BASIC_FEE_GROUP":
                    {
                        string strBasicFeeGroupId = context.Request["BasicFeeGroupID"];
                        string strJSONData = FillBasicFeeGroup(MDVUtility.ToInt64(strBasicFeeGroupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_NEXT_BASIC_FEE_GROUP":
                    {
                        string strEntityId = context.Request["EntityID"];
                        string strJSONData = FillNextBasicFeeGroup(MDVUtility.ToInt64(strEntityId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_BASIC_FEE_GROUP":
                    {
                        string strBasicFeeGroupId = context.Request["BasicFeeGroupID"];
                        string strJSONData = DeleteBasicFeeGroup(MDVUtility.ToInt64(strBasicFeeGroupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BASIC_FEE_GROUP":
                    {
                        string fieldsJSON = context.Request["BasicFeeGroupData"];
                        Int64 BasicFeeGroupID = MDVUtility.ToInt64(context.Request["BasicFeeGroupID"]);
                        string strJSONData = UpdateBasicFeeGroup(fieldsJSON, BasicFeeGroupID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BASIC_FEE_GROUP_ACTIVE_INACTIVE":
                    {
                        Int64 BasicFeeGroupID = MDVUtility.ToInt64(context.Request["BasicFeeGroupID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateBasicFeeGroupIsActive(BasicFeeGroupID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}