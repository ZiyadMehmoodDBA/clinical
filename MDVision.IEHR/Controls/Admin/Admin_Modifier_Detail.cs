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
    public class Admin_Modifier_Detail
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_Modifier_Detail()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_Modifier_Detail _obj = null;
        public static Admin_Modifier_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_Modifier_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the modifier.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveModifier(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Modifier", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    DSCodes.ModifierRow dr = dsCodes.Modifier.NewModifierRow();

                    dr.ModifierCode = SearchedfieldsJSON["txtModifierCode"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsCodes.Modifier.AddModifierRow(dr);
                    BLObject<DSCodes> obj = BLLAdminCodesObj.InsertModifier(ref dsCodes);
                    dsCodes = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ModifierId = dsCodes.Tables[dsCodes.Modifier.TableName].Rows[0][dsCodes.Modifier.ModifierIdColumn.ColumnName]
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
        /// Updates the modifier.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ModifierId">The modifier identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateModifier(string fieldsJSON, Int64 ModifierId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Modifier", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    //DSCodes.ModifierRow dr = dsCodes.Modifier.NewModifierRow();
                    BLObject<DSCodes> objLoad = BLLAdminCodesObj.LoadModifier(ModifierId, null, null, null);
                    dsCodes = objLoad.Data;
                    foreach (DSCodes.ModifierRow dr in dsCodes.Tables[dsCodes.Modifier.TableName].Rows)
                    {
                        //dr.ModifierId = ModifierId;
                        dr.ModifierCode = SearchedfieldsJSON["txtModifierCode"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsCodes.Modifier.AddModifierRow(dr);
                    //dsCodes.Modifier.AcceptChanges();

                    if (dsCodes.Tables[dsCodes.Modifier.TableName].Rows.Count > 0)
                    {
                        //dsCodes.Modifier.Rows[0].SetModified();
                        BLObject<DSCodes> obj = BLLAdminCodesObj.UpdateModifier(ref dsCodes);
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
        /// Fills the modifier.
        /// </summary>
        /// <param name="ModifierId">The modifier identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillModifier(Int64 ModifierId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Modifier", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ModifierId)))
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
                        DSCodes dsCodes = null;
                        BLObject<DSCodes> obj = BLLAdminCodesObj.LoadModifier(ModifierId, null, null, null);
                        if (obj.Data != null)
                        {
                            dsCodes = obj.Data;
                            if (dsCodes.Tables[dsCodes.Modifier.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsCodes.Tables[dsCodes.Modifier.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtModifierCode", MDVUtility.ToStr(dr[dsCodes.Modifier.ModifierCodeColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsCodes.Modifier.DescriptionColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsCodes.Modifier.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    ModifierFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the modifier.
        /// </summary>
        /// <param name="ModifierId">The modifier identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteModifier(Int64 ModifierId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Modifier", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ModifierId)))
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
                        BLObject<string> obj = BLLAdminCodesObj.DeleteModifier(MDVUtility.ToStr(ModifierId));
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
        /// Updates the modifier is active.
        /// </summary>
        /// <param name="ModifierId">The modifier identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateModifierIsActive(Int64 ModifierId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Modifier", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSCodes dsCodes = null;
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadModifier(ModifierId, null, null, null);
                    dsCodes = obj.Data;
                    if (dsCodes.Tables[dsCodes.Modifier.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCodes.Tables[dsCodes.Modifier.TableName].Rows[0];
                        dr[dsCodes.Modifier.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSCodes> objModifier = BLLAdminCodesObj.UpdateModifier(ref dsCodes);
                        string successMsg;
                        if (objModifier.Data != null)
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
                                Message = objModifier.Message
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
        /// Handle the Modifier Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_MODIFIER":
                    {
                        string fieldsJSON = context.Request["ModifierData"];
                        string strJSONData = SaveModifier(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_MODIFIER":
                    {
                        string strModifierId = context.Request["ModifierID"];
                        string strJSONData = FillModifier(MDVUtility.ToInt64(strModifierId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_MODIFIER":
                    {
                        string strModifierId = context.Request["ModifierID"];
                        string strJSONData = DeleteModifier(MDVUtility.ToInt64(strModifierId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_MODIFIER":
                    {
                        string fieldsJSON = context.Request["ModifierData"];
                        Int64 ModifierID = MDVUtility.ToInt64(context.Request["ModifierID"]);
                        string strJSONData = UpdateModifier(fieldsJSON, ModifierID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_MODIFIER_ACTIVE_INACTIVE":
                    {
                        Int64 ModifierID = MDVUtility.ToInt64(context.Request["ModifierID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateModifierIsActive(ModifierID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}