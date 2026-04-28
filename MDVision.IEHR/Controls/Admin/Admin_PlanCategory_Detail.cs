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
    public class Admin_PlanCategory_Detail
    {
        BLLAdminInsurance BLLAdminInsuranceObj = null;
        public Admin_PlanCategory_Detail()
        {
            BLLAdminInsuranceObj = new BLLAdminInsurance();
        }
        #region Singleton
        private static Admin_PlanCategory_Detail _obj = null;
        public static Admin_PlanCategory_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_PlanCategory_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the plan category.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SavePlanCategory(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Category", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = new DSInsurance();
                    DSInsurance.PlanCategoryRow dr = dsInsurance.PlanCategory.NewPlanCategoryRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                    #region Database Insertion
                    dsInsurance.PlanCategory.AddPlanCategoryRow(dr);
                    BLObject<DSInsurance> obj = BLLAdminInsuranceObj.InsertPlanCategory(ref dsInsurance);
                    dsInsurance = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            PlanCategoryId = dsInsurance.Tables[dsInsurance.PlanCategory.TableName].Rows[0][dsInsurance.PlanCategory.PlanIdColumn.ColumnName]
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
        /// Updates the plan category.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PlanCategoryId">The plan category identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdatePlanCategory(string fieldsJSON, Int64 PlanCategoryId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Category", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = new DSInsurance();
                    //DSInsurance.PlanCategoryRow dr = dsInsurance.PlanCategory.NewPlanCategoryRow();
                    BLObject<DSInsurance> objLoad = BLLAdminInsuranceObj.LoadPlanCategory(PlanCategoryId, null, null, null, null);
                    dsInsurance = objLoad.Data;
                    foreach (DSInsurance.PlanCategoryRow dr in dsInsurance.Tables[dsInsurance.PlanCategory.TableName].Rows)
                    {
                        //dr.PlanId = PlanCategoryId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    }

                    #region Database Updation
                    //dsInsurance.PlanCategory.AddPlanCategoryRow(dr);
                    //dsInsurance.PlanCategory.AcceptChanges();

                    if (dsInsurance.Tables[dsInsurance.PlanCategory.TableName].Rows.Count > 0)
                    {
                        //dsInsurance.PlanCategory.Rows[0].SetModified();
                        BLObject<DSInsurance> obj = BLLAdminInsuranceObj.UpdatePlanCategory(ref dsInsurance);
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
        /// Fills the plan category.
        /// </summary>
        /// <param name="PlanCategoryId">The plan category identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillPlanCategory(Int64 PlanCategoryId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Category", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PlanCategoryId)))
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
                        DSInsurance dsInsurance = null;
                        BLObject<DSInsurance> obj = BLLAdminInsuranceObj.LoadPlanCategory(PlanCategoryId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsInsurance = obj.Data;
                            if (dsInsurance.Tables[dsInsurance.PlanCategory.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsInsurance.Tables[dsInsurance.PlanCategory.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsInsurance.PlanCategory.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsInsurance.PlanCategory.DescriptionColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsInsurance.PlanCategory.IsActiveColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsInsurance.Insurance.EntityIdColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    PlanCategoryFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the plan category.
        /// </summary>
        /// <param name="PlanCategoryId">The plan category identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeletePlanCategory(Int64 PlanCategoryId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Category", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PlanCategoryId)))
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
                        BLObject<string> obj = BLLAdminInsuranceObj.DeletePlanCategory(MDVUtility.ToStr(PlanCategoryId));
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
        /// Updates the plan category is active.
        /// </summary>
        /// <param name="PlanCategoryId">The plan category identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdatePlanCategoryIsActive(Int64 PlanCategoryId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Category", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSInsurance dsInsurance = null;
                    BLObject<DSInsurance> obj = BLLAdminInsuranceObj.LoadPlanCategory(PlanCategoryId, null, null, null, null);
                    dsInsurance = obj.Data;
                    if (dsInsurance.Tables[dsInsurance.PlanCategory.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsInsurance.Tables[dsInsurance.PlanCategory.TableName].Rows[0];
                        dr[dsInsurance.PlanCategory.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSInsurance> objPlanCategory = BLLAdminInsuranceObj.UpdatePlanCategory(ref dsInsurance);
                        string successMsg;
                        if (objPlanCategory.Data != null)
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
                                Message = objPlanCategory.Message
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
        /// Handle the PlanCategory Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_PLAN_CATEGORY":
                    {
                        string fieldsJSON = context.Request["PlanCategoryData"];
                        string strJSONData = SavePlanCategory(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PLAN_CATEGORY":
                    {
                        string strPlanCategoryId = context.Request["PlanCategoryID"];
                        string strJSONData = FillPlanCategory(MDVUtility.ToInt64(strPlanCategoryId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PLAN_CATEGORY":
                    {
                        string strPlanCategoryId = context.Request["PlanCategoryID"];
                        string strJSONData = DeletePlanCategory(MDVUtility.ToInt64(strPlanCategoryId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PLAN_CATEGORY":
                    {
                        string fieldsJSON = context.Request["PlanCategoryData"];
                        Int64 PlanCategoryID = MDVUtility.ToInt64(context.Request["PlanCategoryID"]);
                        string strJSONData = UpdatePlanCategory(fieldsJSON, PlanCategoryID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PLAN_CATEGORY_ACTIVE_INACTIVE":
                    {
                        Int64 PlanCategoryID = MDVUtility.ToInt64(context.Request["PlanCategoryID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdatePlanCategoryIsActive(PlanCategoryID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}