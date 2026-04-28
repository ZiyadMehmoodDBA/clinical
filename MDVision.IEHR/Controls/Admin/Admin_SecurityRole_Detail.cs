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
    public class Admin_SecurityRole_Detail
    {
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        public Admin_SecurityRole_Detail()
        {
            BLLAdminSecurityObj = new BLLAdminSecurity();
        }
        #region Singleton
        private static Admin_SecurityRole_Detail _obj = null;
        public static Admin_SecurityRole_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_SecurityRole_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        #region Security Roles
        /// <summary>
        /// Saves the security role.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveSecurityRole(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Roles", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSRoles dsRoles = new DSRoles();
                    DSRoles.RolesRow dr = dsRoles.Roles.NewRolesRow();

                    dr.RoleName = SearchedfieldsJSON["txtShortName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    //Role is Emergency or regular
                    dr.RoleType = MDVUtility.ToStr(SearchedfieldsJSON["roleType"]) == "0" ? false : true;
                    dr.RoleTypeName = MDVUtility.ToStr(SearchedfieldsJSON["roleType_text"]) == "0" ? "Regular" : "Emergency Access";
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.IsAdmin = MDVUtility.ToStr(SearchedfieldsJSON["chkIsAdmin"]) == "True" ? true : false;
                    dr.IsDeleted = false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsRoles.Roles.AddRolesRow(dr);
                    BLObject<DSRoles> obj = BLLAdminSecurityObj.InsertRole(ref dsRoles);

                    if (obj.Data != null)
                    {
                        dsRoles = obj.Data;
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            SecurityRoleId = dsRoles.Roles.Rows[0][dsRoles.Roles.RoleIdColumn.ColumnName]
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
        /// Updates the security role.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="SecurityRoleId">The security role identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateSecurityRole(string fieldsJSON, Int64 SecurityRoleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Roles", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSRoles dsRoles = new DSRoles();
                    //DSRoles.RolesRow dr = dsRoles.Roles.NewRolesRow();
                    BLObject<DSRoles> objLoad = BLLAdminSecurityObj.LoadRoles(SecurityRoleId, null, null, null);
                    dsRoles = objLoad.Data;

                    foreach (DSRoles.RolesRow dr in dsRoles.Tables[dsRoles.Roles.TableName].Rows)
                    {
                        //dr.RoleId = SecurityRoleId;
                        dr.RoleName = SearchedfieldsJSON["txtShortName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        //Role is Emergency or regular
                        dr.RoleType = MDVUtility.ToStr(SearchedfieldsJSON["roleType"]) == "0" ? false : true;
                        dr.RoleTypeName = MDVUtility.ToStr(SearchedfieldsJSON["roleType_text"]) == "0" ? "Regular" : "Emergency Access";
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        dr.IsAdmin = MDVUtility.ToStr(SearchedfieldsJSON["chkIsAdmin"]) == "True" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Insertion
                    //dsRoles.Roles.AddRolesRow(dr);
                    //dsRoles.Roles.AcceptChanges();

                    if (dsRoles.Roles.Rows.Count > 0)
                    {
                        //dsRoles.Roles.Rows[0].SetModified();
                        BLObject<DSRoles> obj = BLLAdminSecurityObj.UpdateRole(ref dsRoles);

                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                SecurityRoleId = dsRoles.Roles.Rows[0][dsRoles.Roles.RoleIdColumn.ColumnName]
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
        /// Deletes the security role.
        /// </summary>
        /// <param name="SecurityRoleId">The security role identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteSecurityRole(Int64 SecurityRoleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Roles", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(SecurityRoleId)))
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
                        BLObject<string> obj = BLLAdminSecurityObj.DeleteRole(MDVUtility.ToStr(SecurityRoleId));
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
        /// Updates the security role is active.
        /// </summary>
        /// <param name="SecurityRoleId">The security role identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateSecurityRoleIsActive(Int64 SecurityRoleId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Roles", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSRoles dsRoles = null;
                    BLObject<DSRoles> obj = BLLAdminSecurityObj.LoadRoles(SecurityRoleId, null, null, null);
                    dsRoles = obj.Data;
                    if (dsRoles.Roles.Rows.Count > 0)
                    {
                        DSRoles.RolesRow dr = (DSRoles.RolesRow)dsRoles.Roles.Rows[0];
                        dr.IsActive = Convert.ToBoolean(IsActive);

                        BLObject<DSRoles> objSecurityRole = BLLAdminSecurityObj.UpdateRole(ref dsRoles);
                        string successMsg;
                        if (objSecurityRole.Data != null)
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
                                Message = objSecurityRole.Message
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

        #region Modules, Forms & Privileges
        /// <summary>
        /// Gets the module form role privileges by role identifier.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        /// <param name="ModuleId">The module identifier.</param>
        /// <param name="ModuleFormId">The module form identifier.</param>
        /// <returns>Json string containing many Datatables or Exception message</returns>
        private string GetModuleFormRolePrivilegesByRoleID(Int64 RoleId, Int64 ModuleId, Int64 ModuleFormId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Roles", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSRoles dsRoles = null;
                    BLObject<DSRoles> obj = BLLAdminSecurityObj.LoadModuleFormRoles(RoleId, ModuleId, ModuleFormId);
                    dsRoles = obj.Data;
                    if (obj.Data != null)
                    {
                        string Roles = string.Empty;
                        DSModuleForm dsModuleForm = new DSModuleForm();
                        if (dsRoles.Roles.Rows.Count > 0)
                        {

                            DSRoles.RolesRow dr = (DSRoles.RolesRow)dsRoles.Roles.Rows[0];
                            String RoleType = dr.RoleType ? "Emergency Access" : "Regular";
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr.RoleName )},
                            { "txtDescription", MDVUtility.ToStr(dr.Description )},
                            { "chkActive", MDVUtility.ToStr(dr.IsActive)},
                             { "chkIsAdmin", MDVUtility.ToStr(dr.IsAdmin)},
                            { "roleType", RoleType},

                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            Roles = js.Serialize(keyValues);


                        }
                        //if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) != AppPrivileges.DefaultUser)
                        //{
                        //    if (dsRoles.Tables[dsModuleForm.Modules.TableName].Rows.Count > 0)
                        //    {
                        //        DataRow[] rows = dsRoles.Tables[dsModuleForm.Modules.TableName].Select(dsModuleForm.Modules.NameColumn.ColumnName + "='Admin'");
                        //        foreach (var item in rows)
                        //        {
                        //            item.Delete();
                        //        }
                        //        dsRoles.AcceptChanges();
                        //    }
                        //}
                        var response = new
                        {
                            status = true,
                            Modules_JSON = MDVUtility.JSON_DataTable(dsRoles.Tables[dsModuleForm.Modules.TableName]),
                            Forms_JSON = MDVUtility.JSON_DataTable(dsRoles.Tables[dsModuleForm.ModuleForms.TableName]),
                            Privileges_JSON = MDVUtility.JSON_DataTable(dsRoles.Tables[dsModuleForm.Privileges.TableName]),
                            Roles_JSON = Roles
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Deletes the modules and references.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        /// <param name="ModuleId">The module identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteModulesAndReferences(Int64 RoleId, Int64 ModuleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Roles", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ModuleId)))
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        BLObject<string> obj = BLLAdminSecurityObj.DeleteModulesFormsPrivileges(RoleId, ModuleId);

                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Saves the module forms.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        /// <param name="ModuleFormId">The module form identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveModuleFormRoles(Int64 RoleId, Int64 ModuleFormId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Roles", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSRoles dsRoles = new DSRoles();
                    DSRoles.ModuleFormRolesRow dr = dsRoles.ModuleFormRoles.NewModuleFormRolesRow();

                    dr.ModuleFormId = ModuleFormId;
                    dr.RoleId = RoleId;

                    #region Database Insertion
                    dsRoles.ModuleFormRoles.AddModuleFormRolesRow(dr);
                    BLObject<DSRoles> obj = BLLAdminSecurityObj.InsertModuleFormRole(ref dsRoles);
                    if (dsRoles.ModuleFormRoles.Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ModuleFormRoleId = dsRoles.ModuleFormRoles.Rows[0][dsRoles.ModuleFormRoles.MFRIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.Save_Error_Message
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
        /// Deletes the module form roles.
        /// </summary>
        /// <param name="ModuleFormRoleId">The module form role identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteModuleFormRoles(Int64 ModuleFormRoleId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ModuleFormRoleId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLAdminSecurityObj.DeleteModuleFormRole(MDVUtility.ToStr(ModuleFormRoleId));

                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Saves the module form privileges.
        /// </summary>
        /// <param name="PrivilegeId">The privilege identifier.</param>
        /// <param name="ModuleFormRoleId">The module form role identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveModuleFormPrivileges(Int64 PrivilegeId, Int64 ModuleFormRoleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Roles", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSRoles dsRoles = new DSRoles();
                    DSRoles.ModuleFormRolePrivilegesRow dr = dsRoles.ModuleFormRolePrivileges.NewModuleFormRolePrivilegesRow();

                    dr.ModuleFormRolePrivilegesId = 0;
                    dr.ModuleFormRoleId = ModuleFormRoleId;
                    dr.PrivilegeSelectionid = PrivilegeId;
                    dr.IsPrivileged = true;
                    dr.IsActive = true;
                    dr.IsDeleted = false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsRoles.ModuleFormRolePrivileges.AddModuleFormRolePrivilegesRow(dr);
                    BLObject<DSRoles> obj = BLLAdminSecurityObj.InsertModuleFormRolePrivileges(ref dsRoles);

                    if (dsRoles.ModuleFormRolePrivileges.Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ModuleFormRolePrivilegeId = dsRoles.ModuleFormRolePrivileges.Rows[0][dsRoles.ModuleFormRolePrivileges.ModuleFormRolePrivilegesIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.Save_Error_Message
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

        private string SaveModuleFormRoleAllPrivileges(string Action, string JSONData, Int64 ModuleFormRoleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Roles", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(JSONData);

                    if (Action == "SAVE")
                    {
                        DSRoles dsRoles = new DSRoles();
                        foreach (var PrivilegeId in data)
                        {
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(PrivilegeId)))
                            {
                                DSRoles.ModuleFormRolePrivilegesRow dr = dsRoles.ModuleFormRolePrivileges.NewModuleFormRolePrivilegesRow();
                                dr.ModuleFormRoleId = ModuleFormRoleId;
                                dr.PrivilegeSelectionid = MDVUtility.ToLong(PrivilegeId);
                                dr.IsPrivileged = true;
                                dr.IsActive = true;
                                dr.IsDeleted = false;
                                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                dr.CreatedOn = DateTime.Now;
                                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                dr.ModifiedOn = DateTime.Now;
                                dsRoles.ModuleFormRolePrivileges.AddModuleFormRolePrivilegesRow(dr);
                            }
                        }

                        #region Database Insertion

                        BLObject<DSRoles> obj = BLLAdminSecurityObj.InsertModuleFormRolePrivileges(ref dsRoles);

                        if (dsRoles.ModuleFormRolePrivileges.Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                JsonModuleFormRolePrivilege = MDVUtility.JSON_DataTable(dsRoles.Tables[dsRoles.ModuleFormRolePrivileges.TableName])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.Save_Error_Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        #endregion

                    }
                    else if (Action == "DELETE")
                    {
                        foreach (var PrivilegeId in data)
                        {
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(PrivilegeId)))
                            {
                                BLObject<string> obj = BLLAdminSecurityObj.DeleteModuleFormRolePrivilege(MDVUtility.ToStr(PrivilegeId));
                            }
                        }
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
                            Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
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
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }

        /// <summary>
        /// Deletes the module form role privilege.
        /// </summary>
        /// <param name="ModuleFormRolePrivilegeId">The module form role privilege identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteModuleFormRolePrivilege(Int64 ModuleFormRolePrivilegeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ModuleFormRolePrivilegeId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLAdminSecurityObj.DeleteModuleFormRolePrivilege(MDVUtility.ToStr(ModuleFormRolePrivilegeId));
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message
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
        #endregion
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Security Role Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                #region Security Role Action Commands
                case "SAVE_SECURITY_ROLE":
                    {
                        string strJSONData = SaveSecurityRole(MDVUtility.ToStr(context.Request["SecurityRoleData"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_SECURITY_ROLE":
                    {
                        string strJSONData = GetModuleFormRolePrivilegesByRoleID(MDVUtility.ToInt64(context.Request["SecurityRoleID"]), MDVUtility.ToInt64(context.Request["ModuleID"]), MDVUtility.ToInt64(context.Request["ModuleFormID"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_SECURITY_ROLE":
                    {
                        string strJSONData = DeleteSecurityRole(MDVUtility.ToInt64(context.Request["SecurityRoleID"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SECURITY_ROLE":
                    {
                        string strJSONData = UpdateSecurityRole(MDVUtility.ToStr(context.Request["SecurityRoleData"]), MDVUtility.ToInt64(context.Request["SecurityRoleID"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SECURITY_ROLE_ACTIVE_INACTIVE":
                    {
                        string strJSONData = UpdateSecurityRoleIsActive(MDVUtility.ToInt64(context.Request["SecurityRoleID"]), MDVUtility.ToInt64(context.Request["IsActive"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                #endregion
                #region Modules, Forms, Privileges Action Commands
                case "SAVE_SECURITY_ROLE_MODULE_FORM":
                    {
                        string strJSONData = SaveModuleFormRoles(MDVUtility.ToInt64(context.Request["RoleID"]), MDVUtility.ToInt64(context.Request["ModuleFormID"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_SECURITY_ROLE_MODULE_FORM_PRIVILEGE":
                    {
                        string strJSONData = SaveModuleFormPrivileges(MDVUtility.ToInt64(context.Request["ModuleFormPrivilegeID"]), MDVUtility.ToInt64(context.Request["ModuleFormRoleID"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_SECURITY_ROLE_MODULE_FORM_ALL_PRIVILEGE":
                    {
                        string strJSONData = SaveModuleFormRoleAllPrivileges(context.Request["Action"], context.Request["Data"], MDVUtility.ToInt64(context.Request["ModuleFormRoleID"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_SECURITY_ROLE_MODULE_FORM":
                    {
                        string strJSONData = DeleteModuleFormRoles(MDVUtility.ToInt64(context.Request["ModuleFormRoleID"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_SECURITY_ROLE_MODULE_FORM_PRIVILEGE":
                    {
                        string strJSONData = DeleteModuleFormRolePrivilege(MDVUtility.ToInt64(context.Request["ModuleFormRolePrivilegeID"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_ROLE_MODULE":
                    {
                        string strJSONData = DeleteModulesAndReferences(MDVUtility.ToInt64(context.Request["RoleID"]), MDVUtility.ToInt64(context.Request["ModuleId"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                #endregion
            }
        }
        #endregion


    }
}