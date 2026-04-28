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
    public class Admin_PlanFeeLink_Detail
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_PlanFeeLink_Detail()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_PlanFeeLink_Detail _obj = null;
        public static Admin_PlanFeeLink_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_PlanFeeLink_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the Plan Fee Link.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SavePlanFeeLink(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Fee Link", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsPlanFeeLink = new DSFeeSchedule();
                    DSFeeSchedule.PlanFeeLinkRow dr = dsPlanFeeLink.PlanFeeLink.NewPlanFeeLinkRow();

                    dr.Name = SearchedfieldsJSON["txtName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                    #region Database Insertion
                    dsPlanFeeLink.PlanFeeLink.AddPlanFeeLinkRow(dr);
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.InsertPlanFeeLink(ref dsPlanFeeLink);
                    dsPlanFeeLink = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            PlanFeeLinkId = dsPlanFeeLink.Tables[dsPlanFeeLink.PlanFeeLink.TableName].Rows[0][dsPlanFeeLink.PlanFeeLink.PlanFeeLinkIdColumn.ColumnName]
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
        /// Updates the Plan Fee Link.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProcedureCategoryId">The PlanFeeLink identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdatePlanFeeLink(string fieldsJSON, Int64 PlanFeeLinkId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Fee Link", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsPlanFeeLink = new DSFeeSchedule();
                    //DSFeeSchedule.PlanFeeLinkRow dr = dsPlanFeeLink.PlanFeeLink.NewPlanFeeLinkRow();
                    BLObject<DSFeeSchedule> objLoad = BLLFeeScheduleObj.LoadPlanFeeLink(PlanFeeLinkId, null, null, null, null);
                    dsPlanFeeLink = objLoad.Data;
                    foreach (DSFeeSchedule.PlanFeeLinkRow dr in dsPlanFeeLink.Tables[dsPlanFeeLink.PlanFeeLink.TableName].Rows)
                    {
                        //dr.PlanFeeLinkId = PlanFeeLinkId;
                        dr.Name = SearchedfieldsJSON["txtName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    }

                    #region Database Updation
                    //dsPlanFeeLink.PlanFeeLink.AddPlanFeeLinkRow(dr);
                    //dsPlanFeeLink.PlanFeeLink.AcceptChanges();

                    if (dsPlanFeeLink.Tables[dsPlanFeeLink.PlanFeeLink.TableName].Rows.Count > 0)
                    {
                        //dsPlanFeeLink.PlanFeeLink.Rows[0].SetModified();
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.UpdatePlanFeeLink(ref dsPlanFeeLink);
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
        /// Fills the Plan Fee Link.
        /// </summary>
        /// <param name="ProcedureCategoryId">The Plan Fee Link identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillPlanFeeLink(Int64 PlanFeeLinkId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Fee Link", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PlanFeeLinkId)))
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
                        DSFeeSchedule dsPlanFeeLink = null;
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadPlanFeeLink(PlanFeeLinkId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsPlanFeeLink = obj.Data;
                            if (dsPlanFeeLink.Tables[dsPlanFeeLink.PlanFeeLink.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsPlanFeeLink.Tables[dsPlanFeeLink.PlanFeeLink.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "txtName", MDVUtility.ToStr(dr[dsPlanFeeLink.PlanFeeLink.NameColumn.ColumnName])},
                          { "txtDescription", MDVUtility.ToStr(dr[dsPlanFeeLink.PlanFeeLink.DescriptionColumn.ColumnName])},
                          { "ChkIsActive", MDVUtility.ToStr(dr[dsPlanFeeLink.PlanFeeLink.IsActiveColumn.ColumnName])},
                          { "ddlEntity", MDVUtility.ToStr(dr[dsPlanFeeLink.PlanFeeLink.EntityIdColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    PlanFeeLinkFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the Plan Fee Link.
        /// </summary>
        /// <param name="ProcedureCategoryId">The Plan Fee Link identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeletePlanFeeLink(Int64 PlanFeeLinkId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Fee Link", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PlanFeeLinkId)))
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
                        BLObject<string> obj = BLLFeeScheduleObj.DeletePlanFeeLink(MDVUtility.ToStr(PlanFeeLinkId));
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

        /// <summary>
        /// Updates the plan fee link is active.
        /// </summary>
        /// <param name="PlanFeeLinkId">The plan fee link identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdatePlanFeeLinkIsActive(Int64 PlanFeeLinkId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Fee Link", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSFeeSchedule dsPlanFeeLink = null;
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadPlanFeeLink(PlanFeeLinkId, null, null, null, null);
                    dsPlanFeeLink = obj.Data;
                    if (dsPlanFeeLink.Tables[dsPlanFeeLink.PlanFeeLink.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPlanFeeLink.Tables[dsPlanFeeLink.PlanFeeLink.TableName].Rows[0];
                        dr[dsPlanFeeLink.PlanFeeLink.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSFeeSchedule> objUser = BLLFeeScheduleObj.UpdatePlanFeeLink(ref dsPlanFeeLink);
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
                case "SAVE_PLAN_FEE_LINK":
                    {
                        string fieldsJSON = context.Request["PlanFeeLinkData"];
                        string strJSONData = SavePlanFeeLink(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PLAN_FEE_LINK":
                    {
                        string strPlanFeeLinkId = context.Request["PlanFeeLinkID"];
                        string strJSONData = FillPlanFeeLink(MDVUtility.ToInt64(strPlanFeeLinkId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PLAN_FEE_LINK":
                    {
                        string strPlanFeeLinkId = context.Request["PlanFeeLinkID"];
                        string strJSONData = DeletePlanFeeLink(MDVUtility.ToInt64(strPlanFeeLinkId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PLAN_FEE_LINK":
                    {
                        string fieldsJSON = context.Request["PlanFeeLinkData"];
                        Int64 PlanFeeLinkID = MDVUtility.ToInt64(context.Request["PlanFeeLinkID"]);
                        string strJSONData = UpdatePlanFeeLink(fieldsJSON, PlanFeeLinkID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PLAN_FEE_LINK_ACTIVE_INACTIVE":
                    {
                        Int64 PlanFeeLinkID = MDVUtility.ToInt64(context.Request["PlanFeeLinkID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdatePlanFeeLinkIsActive(PlanFeeLinkID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}