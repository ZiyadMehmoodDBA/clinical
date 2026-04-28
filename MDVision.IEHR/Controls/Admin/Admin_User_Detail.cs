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
    public class Admin_User_Detail
    {
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        private BLLPatient BLLPatientObj = null;
        public Admin_User_Detail()
        {

            BLLAdminSecurityObj = new BLLAdminSecurity();
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Admin_User_Detail _obj = null;
        public static Admin_User_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_User_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions

        #region User
        /// <summary>
        /// Saves the SaveUser.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        /// 
        private string SaveUser(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSUsers dsUser = new DSUsers();
                    DSUsers.UsersRow dr = dsUser.Users.NewUsersRow();


                    dr.UserName = SearchedfieldsJSON["txtUserName"];
                    dr.UserPassword = MDVUtility.EncryptToSHA256(SearchedfieldsJSON["txtUserPassword"], SearchedfieldsJSON["txtUserName"]);
                    dr.RcUserName = SearchedfieldsJSON["RcUserName"];
                    dr.RcPassword = SearchedfieldsJSON["RcPassword"];
                    dr.RcSigPassword = SearchedfieldsJSON["RcSigPassword"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstEntityId"]))
                    {
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["lstEntityId"]);
                        dr.EntityName = MDVUtility.ToStr(SearchedfieldsJSON["lstEntityId_text"]);
                    }
                    dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                    dr.LastName = SearchedfieldsJSON["txtLastName"];
                    dr.EmailAddress = SearchedfieldsJSON["txtEmailAddress"];

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstProviderId"]))
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["lstProviderId"]);
                    dr.PhoneNo = SearchedfieldsJSON["txtPhoneNo"];
                    dr.PhoneExt = "";
                    dr.EmailAddress = SearchedfieldsJSON["txtEmailAddress"];


                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstUserRoles"]))
                    {
                        dr.UserRoleId = MDVUtility.ToInt64(SearchedfieldsJSON["lstUserRoles"]);
                        dr.UserRoleName = MDVUtility.ToStr(SearchedfieldsJSON["lstUserRoles_text"]);
                    }
                    //Start || 14 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 
                    if (SearchedfieldsJSON.ContainsKey("lstEmergencyUserRoles"))
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstEmergencyUserRoles"]))
                        {
                            dr.EmergencyRoleId = MDVUtility.ToInt64(SearchedfieldsJSON["lstEmergencyUserRoles"]);
                            dr.EmergencyRoleName = MDVUtility.ToStr(SearchedfieldsJSON["lstEmergencyUserRoles_text"]);
                        }
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstCoWorkersGroups"]))
                    {
                        dr.CoWorkersGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["lstCoWorkersGroups"]);
                        dr.CoWorkerGroupName = MDVUtility.ToStr(SearchedfieldsJSON["lstCoWorkersGroups_text"]);
                    }

                    //End   || 14 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 

                    //Start || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail
                    dr.DirectAddress = SearchedfieldsJSON["txtDirectAddress"];
                    //End   || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.IsAdmin = MDVUtility.ToStr(SearchedfieldsJSON["chkIsAdmin"]) == "True" ? true : false;
                    dr.IsEMR = MDVUtility.ToStr(SearchedfieldsJSON["chkIsEMR"]) == "True" ? true : false;
                    dr.IsNoteUnSign = MDVUtility.ToStr(SearchedfieldsJSON["chkIsNoteUnSign"]) == "True" ? true : false;
                    dr.IsLocked = MDVUtility.ToStr(SearchedfieldsJSON["chkIsLocked"]) == "True" ? true : false;
                    dr.RCopialUser = MDVUtility.ToStr(SearchedfieldsJSON["RCopialUser"]) == "True" ? true : false;
                    dr.IsFullSSN = MDVUtility.ToStr(SearchedfieldsJSON["chkIsFullSSN"]) == "True" ? true : false;
                    dr.IsCollection = MDVUtility.ToStr(SearchedfieldsJSON["chkIsShowColBal"]) == "True" ? true : false;
                    // Faizan Ameen MU3-15 Start
                    dr.IsDataPrivacy = MDVUtility.ToStr(SearchedfieldsJSON["chkIsDataPrivacy"]) == "True" ? true : false;
                    dr.IsExpiryAlert = MDVUtility.ToStr(SearchedfieldsJSON["chkIsExpiryAlert"]) == "True" ? true : false;
                    // End
                    dr.IsMobileLogin = MDVUtility.ToStr(SearchedfieldsJSON["IsMobileLogin"]) == "True" ? true : false;
                    dr.IsMedText = MDVUtility.ToStr(SearchedfieldsJSON["chkIsMedText"]) == "True" ? true : false;
                    dr.MobSessionExpTime = MDVUtility.ToStr(SearchedfieldsJSON["MobSessionExpTime"]);
                    dr.AutoLogOff = MDVUtility.ToInt32(SearchedfieldsJSON["txtAutoLogOff"]);//kr
                    dr.DaysBeforeExpiry = MDVUtility.ToInt32(SearchedfieldsJSON["txtDaysBeforeExpiry"]);
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.MiddleInitial = MDVUtility.ToStr(SearchedfieldsJSON["txtMI"]);
                    dr.UserSelectedDocuments = MDVUtility.ToStr(SearchedfieldsJSON["UserSelectedDocuments"]);
                    dr.UserTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["lstUserType"]);
                    #region Database Insertion
                    dsUser.Users.AddUsersRow(dr);
                    BLObject<DSUsers> obj = BLLAdminSecurityObj.InsertUser(ref dsUser);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            UserId = dsUser.Tables[dsUser.Users.TableName].Rows[0][dsUser.Users.UserIdColumn.ColumnName]
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
        private string GetIsDataPrivacy()
        {
            try
            {

                string Result = BLLPatientObj.GetIsDataPrivacy();

               
                var response = new
                {
                    status = true,
                    IsDataPrivacy = Result,
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
        /// Updates the User.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <returns></returns>
        private string UpdateUser(string fieldsJSON, Int64 UserId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSUsers dsUser = new DSUsers();
                    BLObject<DSUsers> objUser = BLLAdminSecurityObj.LoadUser(UserId, "", "", "", "", "", "");
                    dsUser = objUser.Data;
                    if (dsUser.Tables[dsUser.Users.TableName].Rows.Count > 0)
                    {
                        DSUsers.UsersRow dr = (DSUsers.UsersRow)dsUser.Tables[dsUser.Users.TableName].Rows[0];
                        //DSUsers.UsersRow dr = dsUser.Users.NewUsersRow();

                        //dr.UserId = UserId;
                        dr.UserName = SearchedfieldsJSON["txtUserName"];
                        if (SearchedfieldsJSON["txtUserPassword"] == "")
                        {
                            dr.UserPassword = SearchedfieldsJSON["txtUserPasswordHF"];
                        }
                        else
                        {
                            dr.UserPassword = SearchedfieldsJSON["txtUserPassword"];
                        }

                        dr.RcUserName = SearchedfieldsJSON["RcUserName"];
                        dr.RcPassword = SearchedfieldsJSON["RcPassword"];
                        dr.RcSigPassword = SearchedfieldsJSON["RcSigPassword"];


                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstEntityId"]))
                        {
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["lstEntityId"]);
                            dr.EntityName = MDVUtility.ToStr(SearchedfieldsJSON["lstEntityId_text"]);
                        }
                        dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                        dr.LastName = SearchedfieldsJSON["txtLastName"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmailAddress"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstProviderId"]))
                            dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["lstProviderId"]);
                        else
                            dr.ProviderId = 0;

                        dr.PhoneNo = SearchedfieldsJSON["txtPhoneNo"];
                        dr.PhoneExt = "";
                        dr.EmailAddress = SearchedfieldsJSON["txtEmailAddress"];

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstUserRoles"]))
                        {
                            dr.UserRoleId = MDVUtility.ToInt64(SearchedfieldsJSON["lstUserRoles"]);
                            if (SearchedfieldsJSON["lstUserRoles_text"] == "PMS  EMR")
                                SearchedfieldsJSON["lstUserRoles_text"] = "PMS & EMR";
                            dr.UserRoleName = MDVUtility.ToStr(SearchedfieldsJSON["lstUserRoles_text"]);
                        }
                        //Start || 14 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 
                        if (SearchedfieldsJSON.ContainsKey("lstEmergencyUserRoles"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstEmergencyUserRoles"]))
                            {
                                dr.EmergencyRoleId = MDVUtility.ToInt64(SearchedfieldsJSON["lstEmergencyUserRoles"]);
                                dr.EmergencyRoleName = MDVUtility.ToStr(SearchedfieldsJSON["lstEmergencyUserRoles_text"]);
                            }
                            else
                            {
                                dr[dsUser.Users.EmergencyRoleIdColumn] = System.DBNull.Value;
                                dr.EmergencyRoleName = "";
                            }
                        }

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstCoWorkersGroups"]))
                        {
                            dr.CoWorkersGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["lstCoWorkersGroups"]);
                            dr.CoWorkerGroupName = MDVUtility.ToStr(SearchedfieldsJSON["lstCoWorkersGroups_text"]);
                        }
                        else
                        {
                            dr[dsUser.Users.CoWorkersGroupIdColumn] = System.DBNull.Value;
                            dr.CoWorkerGroupName = "";
                        }
                        //End   || 14 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 

                        //Start || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail
                        dr.DirectAddress = SearchedfieldsJSON["txtDirectAddress"];
                        //End   || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        dr.IsAdmin = MDVUtility.ToStr(SearchedfieldsJSON["chkIsAdmin"]) == "True" ? true : false;
                        dr.IsEMR = MDVUtility.ToStr(SearchedfieldsJSON["chkIsEMR"]) == "True" ? true : false;
                        dr.IsConfigureAlerts = MDVUtility.ToStr(SearchedfieldsJSON["chkConfigureAlerts"]) == "True" ? true : false;
                        dr.IsNoteUnSign = MDVUtility.ToStr(SearchedfieldsJSON["chkIsNoteUnSign"]) == "True" ? true : false;
                        dr.IsLocked = MDVUtility.ToStr(SearchedfieldsJSON["chkIsLocked"]) == "True" ? true : false;
                        dr.RCopialUser = MDVUtility.ToStr(SearchedfieldsJSON["RCopialUser"]) == "True" ? true : false;
                        dr.IsFullSSN = MDVUtility.ToStr(SearchedfieldsJSON["chkIsFullSSN"]) == "True" ? true : false;
                        dr.IsCollection = MDVUtility.ToStr(SearchedfieldsJSON["chkIsShowColBal"]) == "True" ? true : false;
                        // Faizan Ameen MU3-15 START
                        dr.IsDataPrivacy = MDVUtility.ToStr(SearchedfieldsJSON["chkIsDataPrivacy"]) == "True" ? true : false;
                        // END
                        dr.IsMobileLogin = MDVUtility.ToStr(SearchedfieldsJSON["IsMobileLogin"]) == "True" ? true : false;
                        dr.IsMedText = MDVUtility.ToStr(SearchedfieldsJSON["chkIsMedText"]) == "True" ? true : false;
                        dr.MobSessionExpTime = MDVUtility.ToStr(SearchedfieldsJSON["MobSessionExpTime"]);
                        dr.IsExpiryAlert = MDVUtility.ToStr(SearchedfieldsJSON["chkIsExpiryAlert"]) == "True" ? true : false;
                        dr.AutoLogOff = MDVUtility.ToInt32(SearchedfieldsJSON["txtAutoLogOff"]);//kr
                        dr.DaysBeforeExpiry = MDVUtility.ToInt32(SearchedfieldsJSON["txtDaysBeforeExpiry"]);
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.MiddleInitial = MDVUtility.ToStr(SearchedfieldsJSON["txtMI"]);
                        dr.UserSelectedDocuments = MDVUtility.ToStr(SearchedfieldsJSON["UserSelectedDocuments"]);
                        dr.UserTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["lstUserType"]);
                    }
                    #region Database Updation
                    //dsUser.Users.AddUsersRow(dr);
                    //dsUser.Users.AcceptChanges();

                    if (dsUser.Tables[dsUser.Users.TableName].Rows.Count > 0)
                    {
                        // dsUser.Users.Rows[0].SetModified();
                        BLObject<DSUsers> obj = BLLAdminSecurityObj.UpdateUser(ref dsUser);
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Deletes Users by User Id.
        /// </summary>
        /// <param name="PracticeID">The practice identifier.</param>
        /// <returns></returns>
        private string DeleteUser(Int64 UserID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(UserID)))
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
                        BLObject<string> obj = BLLAdminSecurityObj.DeleteUser(MDVUtility.ToStr(UserID));
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
        /// Updates the user is active.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateUserIsActive(Int64 UserId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSUsers dsUser = null;
                    BLObject<DSUsers> obj = BLLAdminSecurityObj.LoadUser(UserId, null, null, null, null, null, null);
                    dsUser = obj.Data;
                    if (dsUser.Tables[dsUser.Users.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsUser.Tables[dsUser.Users.TableName].Rows[0];
                        dr[dsUser.Users.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSUsers> objUser = BLLAdminSecurityObj.UpdateUser(ref dsUser);
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

        private string GetUserNameStatus(string UserName)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(UserName)))
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
                    BLObject<string> obj = BLLAdminSecurityObj.IsUserExist(MDVUtility.ToStr(UserName));
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
                            Message = "1"//obj.Data
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        #endregion

        #region Module_Form_Priviliges
        /// <summary>
        /// Saves the module_ form_ users.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="ModuleFormId">The module form identifier.</param>
        /// <returns></returns>
        private string SaveModule_Form_Users(long UserId, long ModuleFormId)
        {
            try
            {
                DSUsers dsUser = new DSUsers();
                DSUsers.ModuleFormUsersRow dr = dsUser.ModuleFormUsers.NewModuleFormUsersRow();

                dr.ModuleFormId = ModuleFormId;
                dr.UserId = UserId;
                dr.AuditUserId = MDVSession.Current.AppUserId.ToString();
                #region Database Insertion
                dsUser.ModuleFormUsers.AddModuleFormUsersRow(dr);
                BLObject<DSUsers> obj = BLLAdminSecurityObj.InsertModuleFormUsers(ref dsUser);

                if (dsUser.Tables[dsUser.ModuleFormUsers.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserId = dsUser.Tables[dsUser.ModuleFormUsers.TableName].Rows[0][dsUser.ModuleFormUsers.ModuleFormIdColumn.ColumnName]

                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        /// Saves the module forms.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="ModuleFormId">The module form identifier.</param>
        /// <returns></returns>
        private string SaveModuleForms(Int64 UserId, Int64 ModuleFormId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSUsers dsUser = new DSUsers();
                    DSUsers.ModuleFormUsersRow dr = dsUser.ModuleFormUsers.NewModuleFormUsersRow();

                    dr.ModuleFormId = ModuleFormId;
                    dr.UserId = UserId;
                    dr.AuditUserId = MDVSession.Current.AppUserId.ToString();
                    #region Database Insertion
                    dsUser.ModuleFormUsers.AddModuleFormUsersRow(dr);

                    BLObject<DSUsers> obj = BLLAdminSecurityObj.InsertModuleFormUsers(ref dsUser);
                    if (dsUser.Tables[dsUser.ModuleFormUsers.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ModuleFormUserId = dsUser.Tables[dsUser.ModuleFormUsers.TableName].Rows[0][dsUser.ModuleFormUsers.MFUIdColumn.ColumnName]
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
        /// Saves the module form privileges.
        /// </summary>
        /// <param name="PrivilegeId">The privilege identifier.</param>
        /// <param name="ModuleFormUserId">The module form user identifier.</param>
        /// <returns></returns>
        private string SaveModuleFormPrivileges(Int64 PrivilegeId, Int64 ModuleFormUserId, string PrivilegeName, string FormName, string AssignedTo)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSUsers dsUser = new DSUsers();
                    DSUsers.ModuleFormUsersPrivilegesRow dr = dsUser.ModuleFormUsersPrivileges.NewModuleFormUsersPrivilegesRow();

                    dr.ModuleFormUsersId = ModuleFormUserId;
                    dr.PrivilegeSelectionId = PrivilegeId;
                    dr.PrivilegeName = PrivilegeName;
                    dr.FormName = FormName;
                    dr.AssignedTo = AssignedTo;
                    dr.IsPrivileged = true;
                    dr.IsActive = true;
                    dr.IsDeleted = false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsUser.ModuleFormUsersPrivileges.AddModuleFormUsersPrivilegesRow(dr);
                    BLObject<DSUsers> obj = BLLAdminSecurityObj.InsertModuleFormUsersPrivileges(ref dsUser);

                    if (dsUser.Tables[dsUser.ModuleFormUsersPrivileges.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ModuleFormUserPrivilegeId = dsUser.Tables[dsUser.ModuleFormUsersPrivileges.TableName].Rows[0][dsUser.ModuleFormUsersPrivileges.ModuleFormUserPriviligesIdColumn.ColumnName]
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
        private string SaveModuleFormPrivilegesSelectAll(string PriviligeJSON, Int64 ModuleFormUserId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var PrivilageJSON = ser.Deserialize<dynamic>(PriviligeJSON);

                    DSUsers dsUser = new DSUsers();

                    foreach (dynamic Ids in PrivilageJSON)
                    {

                        string PrivilegeId = Ids["PrivilegeId"];
                        string PrivilegeName = Ids["PrivilegeName"];
                        string FormName = Ids["FormName"];
                        string AssignedTo = Ids["AssignedTo"];
                        DSUsers.ModuleFormUsersPrivilegesRow dr = dsUser.ModuleFormUsersPrivileges.NewModuleFormUsersPrivilegesRow();

                        dr.ModuleFormUsersId = ModuleFormUserId;
                        dr.PrivilegeSelectionId = MDVUtility.ToInt64(PrivilegeId);
                        dr.IsPrivileged = true;
                        dr.IsActive = true;
                        dr.IsDeleted = false;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.PrivilegeName = PrivilegeName;
                        dr.FormName = FormName;
                        dr.AssignedTo = AssignedTo;

                        dsUser.ModuleFormUsersPrivileges.AddModuleFormUsersPrivilegesRow(dr);
                    }

                    #region Database Insertion
                    //dsUser.ModuleFormUsersPrivileges.AddModuleFormUsersPrivilegesRow(item);
                    BLObject<DSUsers> obj = BLLAdminSecurityObj.InsertModuleFormUsersPrivileges(ref dsUser);

                    if (obj.Data != null)
                    //(dsUser.Tables[dsUser.ModuleFormUsersPrivileges.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            PriviligeCount = dsUser.Tables[dsUser.ModuleFormUsersPrivileges.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsUser.ModuleFormUsersPrivileges.Rows.Count > 0) ? dsUser.ModuleFormUsersPrivileges.Rows[0][dsUser.ModuleFormUsersPrivileges.ModuleFormUserPriviligesIdColumn.ColumnName] : 0,
                            Privilige_JSON = MDVUtility.JSON_DataTable(dsUser.Tables[dsUser.ModuleFormUsersPrivileges.TableName]),
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
        /// Saves the module_ form_ users_ priviliges.
        /// </summary>
        /// <param name="ModuleFormUserid">The module form userid.</param>
        /// <param name="PrivilegeSelectionid">The privilege selectionid.</param>
        /// <returns></returns>
        private string SaveModule_Form_Users_Priviliges(long ModuleFormUserid, long PrivilegeSelectionid)
        {
            try
            {
                DSUsers dsUser = new DSUsers();
                DSUsers.ModuleFormUsersPrivilegesRow dr = dsUser.ModuleFormUsersPrivileges.NewModuleFormUsersPrivilegesRow();

                dr.ModuleFormUsersId = ModuleFormUserid;
                dr.PrivilegeSelectionId = PrivilegeSelectionid;
                dr.IsPrivileged = true;
                dr.IsActive = true;
                dr.IsDeleted = false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsUser.ModuleFormUsersPrivileges.AddModuleFormUsersPrivilegesRow(dr);
                BLObject<DSUsers> obj = BLLAdminSecurityObj.InsertModuleFormUsersPrivileges(ref dsUser);
                if (dsUser.Tables[dsUser.ModuleFormUsersPrivileges.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserId = dsUser.Tables[dsUser.ModuleFormUsersPrivileges.TableName].Rows[0][dsUser.ModuleFormUsersPrivileges.ModuleFormUserPriviligesIdColumn.ColumnName]
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
        /// Gets the module form user privileges by user identifier.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="ModuleId">The module identifier.</param>
        /// <param name="ModuleFormId">The module form identifier.</param>
        /// <returns></returns>
        private string GetModuleFormUserPrivilegesByUserID(Int64 UserId, Int64 ModuleId, Int64 ModuleFormId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSUsers dsUsers = null;
                    BLObject<DSUsers> obj = BLLAdminSecurityObj.LoadModuleFormUser(UserId, ModuleId, ModuleFormId);
                    if (obj.Data != null)
                    {
                        dsUsers = obj.Data;

                        DSPrivilegeGroup dsPrivilegeGroup = new DSPrivilegeGroup();
                        DSModuleForm dsModuleForm = new DSModuleForm();
                        string Users = string.Empty;
                        if (dsUsers.Users.Rows.Count > 0)
                        {
                            DSUsers.UsersRow dr = (DSUsers.UsersRow)dsUsers.Users.Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtUserName", MDVUtility.ToStr(dr.UserName)},
                            { "txtUserPassword", MDVUtility.ToStr(dr.UserPassword)},
                            { "txtMI", MDVUtility.ToStr(dr.MiddleInitial)},
                            { "txtUserPasswordHF", MDVUtility.ToStr(dr.UserPassword)},
                            { "lstEntityId", MDVUtility.ToStr(dr.EntityId)},
                            { "lstProviderId", dr.IsProviderIdNull()==true ? "" : MDVUtility.ToStr(dr.ProviderId)},
                            { "txtFirstName", MDVUtility.ToStr(dr.FirstName)},
                            { "txtLastName", MDVUtility.ToStr(dr.LastName)},
                            { "txtEmailAddress",dr.IsEmailAddressNull()==true ? "" : MDVUtility.ToStr(dr.EmailAddress)},
                            { "txtPhoneNo",dr.IsPhoneNoNull()==true ? "" : MDVUtility.ToStr(dr.PhoneNo)},
                            { "lstUserRoles", MDVUtility.ToStr(dr.UserRoleId)},
                            { "chkIsActive", MDVUtility.ToStr(dr.IsActive)},
                            { "chkIsAdmin", MDVUtility.ToStr(dr.IsAdmin)},
                            { "chkIsEMR", MDVUtility.ToStr(dr.IsEMR)},
                            { "chkIsNoteUnSign", MDVUtility.ToStr(dr.IsNoteUnSign)},
                            { "chkIsLocked",(dr[dsUsers.Users.IsLockedColumn.ColumnName] != System.DBNull.Value? MDVUtility.ToStr(dr[dsUsers.Users.IsLockedColumn.ColumnName]) :"false")},
                            { "txtAutoLogOff", MDVUtility.ToStr(dr.AutoLogOff)},//kr
                            //Start || 14 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 
                            { "lstEmergencyUserRoles", dr.IsEmergencyRoleIdNull()?"":MDVUtility.ToStr(dr.EmergencyRoleId)},
                            //End   || 14 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 

                             //Start || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail
                          //  { "txtDirectAddress", MDVUtility.ToStr(dr.DirectAddress)},
                            { "txtDirectAddress", dr.IsDirectAddressNull()?"":MDVUtility.ToStr(dr.DirectAddress)},
                            { "RCopialUser", MDVUtility.ToStr(dr.RCopialUser)},
                            { "RcUserName", dr.IsRcUserNameNull()?"":MDVUtility.ToStr(dr.RcUserName)},
                            { "RcPassword", dr.IsRcPasswordNull()?"":MDVUtility.ToStr(dr.RcPassword)},
                            { "RcSigPassword", dr.IsRcSigPasswordNull()?"":MDVUtility.ToStr(dr.RcSigPassword)},
                            { "lstCoWorkersGroups", dr.IsCoWorkersGroupIdNull()?"":MDVUtility.ToStr(dr.CoWorkersGroupId)},
                             { "chkIsFullSSN", (dr[dsUsers.Users.IsFullSSNColumn.ColumnName] != System.DBNull.Value? MDVUtility.ToStr(dr[dsUsers.Users.IsFullSSNColumn.ColumnName]) :"false")},
                            //End   || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail
                             // Faizan Ameen MU3-15
                            { "chkIsDataPrivacy", MDVUtility.ToStr(dr.IsDataPrivacy)},
                             { "chkIsShowColBal", (dr[dsUsers.Users.IsCollectionColumn.ColumnName] != System.DBNull.Value? MDVUtility.ToStr(dr[dsUsers.Users.IsCollectionColumn.ColumnName]) :"false")},
                            { "UserSelectedDocuments", dr.UserSelectedDocuments==null?"":dr.UserSelectedDocuments},
                            // End
                            { "chkIsExpiryAlert", MDVUtility.ToStr(dr.IsExpiryAlert)},
                            { "txtDaysBeforeExpiry", dr.IsDaysBeforeExpiryNull()?"": MDVUtility.ToStr(dr.DaysBeforeExpiry)},
                            { "IsMobileLogin", MDVUtility.ToStr(dr.IsMobileLogin)},
                            { "chkIsMedText", MDVUtility.ToStr(dr.IsMedText)},
                            { "MobSessionExpTime", dr.MobSessionExpTime!=null?MDVUtility.ToStr(dr.MobSessionExpTime):""},
                            { "lstUserType", dr.IsUserTypeIdNull() == true ? "" : MDVUtility.ToStr(dr.UserTypeId) },
                            { "chkConfigureAlerts",  (dr[dsUsers.Users.IsConfigureAlertsColumn.ColumnName] != System.DBNull.Value? MDVUtility.ToStr(dr[dsUsers.Users.IsConfigureAlertsColumn.ColumnName]) :"false")},

                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            Users = js.Serialize(keyValues);



                            //if (MDVUtility.ToStr(dr.IsAdmin) != "True")

                        }

                        //if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) != AppPrivileges.DefaultUser)
                        //{
                        //    if (dsUsers.Tables[dsModuleForm.Modules.TableName].Rows.Count> 0)
                        //    {
                        //        DataRow[] rows;
                        //        //if (AppConfig.IsAdmin)
                        //        //{
                        //        //    rows = dsUsers.Tables[dsModuleForm.Modules.TableName].Select(dsModuleForm.Modules.NameColumn.ColumnName + "<>'Admin'");
                        //        //}
                        //        //else {
                        //            rows = dsUsers.Tables[dsModuleForm.Modules.TableName].Select(dsModuleForm.Modules.NameColumn.ColumnName + "='Admin'");
                        //        //}

                        //        foreach (var item in rows)
                        //        {
                        //            item.Delete();
                        //        }
                        //        dsUsers.AcceptChanges();
                        //    }
                        //}
                        var response = new
                        {
                            status = true,
                            Modules_JSON = MDVUtility.JSON_DataTable(dsUsers.Tables[dsModuleForm.Modules.TableName]),
                            Forms_JSON = MDVUtility.JSON_DataTable(dsUsers.Tables[dsModuleForm.ModuleForms.TableName]),
                            Privileges_JSON = MDVUtility.JSON_DataTable(dsUsers.Tables[dsModuleForm.Privileges.TableName]),
                            SecurityGroup_JSON = MDVUtility.JSON_DataTable(dsUsers.Tables[dsPrivilegeGroup.SecurityGroup.TableName]),
                            Users_JSON = Users
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

        private string GetNoteUnSignPrivilegesByUserID(Int64 UserId)
        {
            try
            {
                BLObject<string> obj = BLLAdminSecurityObj.CheckUserHaveNoteUnSignRights(UserId);

                if (obj.Data.ToLower() == "yes" || obj.Data.ToLower() == "no")
                {
                    var response = new
                    {
                        status = true,
                        Message = obj.Data.ToLower(),
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
        /// Deletes the module users.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="ModuleId">The module identifier.</param>
        /// <returns></returns>
        private string DeleteModuleUsers(Int64 UserId, Int64 ModuleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "EDIT")).ToString();
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
                        BLObject<string> obj = BLLAdminSecurityObj.DeleteModuleUsers(UserId, ModuleId);
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
        /// Deletes the module form users.
        /// </summary>
        /// <param name="ModuleFormUserId">The module form user identifier.</param>
        /// <returns></returns>
        private string DeleteModuleFormUsers(Int64 ModuleFormUserId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ModuleFormUserId)))
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
                    BLObject<string> obj = BLLAdminSecurityObj.DeleteModuleFormUsers(MDVUtility.ToStr(ModuleFormUserId),MDVSession.Current.AppUserId);
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
        /// Deletes the module form users privilege.
        /// </summary>
        /// <param name="ModuleFormUserPrivilegeId">The module form user privilege identifier.</param>
        /// <returns></returns>
        private string DeleteModuleFormUsersPrivilege(Int64 ModuleFormUserPrivilegeId, string PrivilegeName, string FormName, string AssignedTo)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ModuleFormUserPrivilegeId)))
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
                    BLObject<string> obj = BLLAdminSecurityObj.DeleteModuleFormUsersPrivileges(MDVUtility.ToStr(ModuleFormUserPrivilegeId), PrivilegeName, FormName, AssignedTo,MDVSession.Current.AppUserId);
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

        private string DeleteModuleFormUsersPrivilegeSelectAll(string PriviligeJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var PrivilageJSON = ser.Deserialize<dynamic>(PriviligeJSON);

                if (string.IsNullOrEmpty(MDVUtility.ToStr(PrivilageJSON)))
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
                    foreach (dynamic Ids in PrivilageJSON)
                    {

                        string ModuleFormUserPrivilegeId = Ids["ModuleFormUserPrivilegeId"];
                        string PrivilegeName = Ids["PrivilegeName"];
                        string FormName = Ids["FormName"];
                        string AssignedTo = Ids["AssignedTo"];
                        long AuditUserId = MDVSession.Current.AppUserId;
                        BLObject<string> obj = BLLAdminSecurityObj.DeleteModuleFormUsersPrivileges(MDVUtility.ToStr(ModuleFormUserPrivilegeId), PrivilegeName, FormName, AssignedTo, AuditUserId);
                    }
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
        #endregion

        #region User_Entity_Group
        /// <summary>
        /// Saves the user entity group.
        /// </summary>
        /// <param name="SecGroupId">The sec group identifier.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <returns></returns>
        private string SaveUserEntityGroup(Int64 SecGroupId, Int64 UserId, Int64 EntityId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSUsers dsUser = new DSUsers();
                    DSUsers.UsersEntityGroupRow dr = dsUser.UsersEntityGroup.NewUsersEntityGroupRow();

                    dr.SecurityGroupId = SecGroupId;
                    dr.UserId = UserId;
                    dr.Entityid = EntityId;
                    dr.AuditUserId = MDVSession.Current.AppUserId;
                    #region Database Insertion
                    dsUser.UsersEntityGroup.AddUsersEntityGroupRow(dr);
                    BLObject<DSUsers> obj = BLLAdminSecurityObj.InsertUserEntityGroup(ref dsUser);
                    if (dsUser.Tables[dsUser.UsersEntityGroup.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            UsersEntityGroupId = dsUser.Tables[dsUser.UsersEntityGroup.TableName].Rows[0][dsUser.UsersEntityGroup.UserEntityGroupIdColumn.ColumnName]
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
        /// Deletes the entity group.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <returns></returns>
        private string DeleteEntityGroup(Int64 UserID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(UserID)))
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
                    BLObject<string> obj = BLLAdminSecurityObj.DeleteUserEntityGroup(MDVUtility.ToStr(UserID));
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the User Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_MODULE_FORM_USER":
                    {
                        string fieldsJSON = context.Request["UserData"];
                        Int64 UserID = MDVUtility.ToInt64(context.Request["UserID"]);
                        Int64 ModuleFormId = MDVUtility.ToInt64(context.Request["ModuleFormId"]);
                        string strJSONData = SaveModule_Form_Users(UserID, ModuleFormId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_MODULE_FORM_USERS_PRIVILEGES":
                    {
                        Int64 PrivilegeSelectionid = MDVUtility.ToInt64(context.Request["PrivilegeSelectionid"]);
                        string strJSONData = SaveModule_Form_Users_Priviliges(0, PrivilegeSelectionid);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_USER":
                    {
                        string fieldsJSON = context.Request["UserData"];

                        string strJSONData = SaveUser(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_USER":
                    {
                        string strUserId = context.Request["UserID"];
                        string strModuleId = context.Request["ModuleID"];
                        string strModuleFormId = context.Request["ModuleFormID"];
                        string strJSONData = GetModuleFormUserPrivilegesByUserID(MDVUtility.ToInt64(strUserId), MDVUtility.ToInt64(strModuleId), MDVUtility.ToInt64(strModuleFormId));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CHECK_USER_PERMISSIONS_OF_UNSIGN_NOTE":
                    {
                        string strUserId = context.Request["UserID"];
                        
                        string strJSONData = GetNoteUnSignPrivilegesByUserID(MDVUtility.ToInt64(strUserId));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;




                case "DELETE_USER":
                    {
                        string strUserId = context.Request["UserID"];
                        string strIsAdmin = context.Request["UserID"];
                        string strJSONData = DeleteUser(MDVUtility.ToInt64(strUserId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_USER":
                    {
                        string fieldsJSON = context.Request["UserData"];
                        Int64 UserID = MDVUtility.ToInt64(context.Request["UserID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);

                        string strJSONData = UpdateUser(fieldsJSON, UserID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_USER_MODULE_FORM":
                    {
                        Int64 UserID = MDVUtility.ToInt64(context.Request["UserID"]);
                        Int64 ModuleFormID = MDVUtility.ToInt64(context.Request["ModuleFormID"]);
                        string strJSONData = SaveModuleForms(UserID, ModuleFormID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_USER_MODULE_FORM_PRIVILEGE":
                    {
                        Int64 PrivilegeID = MDVUtility.ToInt64(context.Request["ModuleFormPrivilegeID"]);
                        Int64 ModuleFormUserID = MDVUtility.ToInt64(context.Request["ModuleFormUserID"]);
                        string PrivilegeName = context.Request["PrivilegeName"];
                        string FormName = context.Request["FormName"];
                        string AssignedTo = context.Request["AssignedTo"];
                        string strJSONData = SaveModuleFormPrivileges(PrivilegeID, ModuleFormUserID, PrivilegeName, FormName, AssignedTo);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_USER_MODULE_FORM_PRIVILEGE_SELECT_ALL":
                    {
                        string PriviligeJSON = MDVUtility.ToStr(context.Request["PriviligeJSon"]);
                        Int64 ModuleFormUserID = MDVUtility.ToInt64(context.Request["ModuleFormUserID"]);
                        string strJSONData = SaveModuleFormPrivilegesSelectAll(PriviligeJSON, ModuleFormUserID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_USER_MODULE":
                    {
                        Int64 UserID = MDVUtility.ToInt64(context.Request["UserID"]);
                        Int64 ModuleId = MDVUtility.ToInt64(context.Request["ModuleId"]);
                        string strJSONData = DeleteModuleUsers(UserID, ModuleId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_USER_MODULE_FORM":
                    {
                        Int64 ModuleFormUserID = MDVUtility.ToInt64(context.Request["ModuleFormUserID"]);
                        string strJSONData = DeleteModuleFormUsers(ModuleFormUserID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_USER_MODULE_FORM_PRIVILEGE":
                    {
                        Int64 ModuleFormUserPrivilegeID = MDVUtility.ToInt64(context.Request["ModuleFormUserPrivilegeID"]);
                        string PrivilegeName = context.Request["PrivilegeName"];
                        string FormName = context.Request["FormName"];
                        string AssignedTo = context.Request["AssignedTo"];
                        string strJSONData = DeleteModuleFormUsersPrivilege(ModuleFormUserPrivilegeID, PrivilegeName, FormName, AssignedTo);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_USER_MODULE_FORM_PRIVILEGE_SELECT_ALL":
                    {
                        string PriviligeJSON = MDVUtility.ToStr(context.Request["PriviligeJSon"]);
                        string strJSONData = DeleteModuleFormUsersPrivilegeSelectAll(PriviligeJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_SECURITY_GROUP":
                    {
                        Int64 SecurityGroupId = MDVUtility.ToInt64(context.Request["SecurityGroupId"]);
                        Int64 UserId = MDVUtility.ToInt64(context.Request["UserId"]);
                        Int64 EntityId = MDVUtility.ToInt64(context.Request["EntityId"]);
                        string strJSONData = SaveUserEntityGroup(SecurityGroupId, UserId, EntityId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_SECURITY_GROUP":
                    {
                        Int64 UsersEntityGroupId = MDVUtility.ToInt64(context.Request["UsersEntityGroupId"]);
                        string strJSONData = DeleteEntityGroup(UsersEntityGroupId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_USER_ACTIVE_INACTIVE":
                    {
                        Int64 UserID = MDVUtility.ToInt64(context.Request["UserID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateUserIsActive(UserID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CHECK_USER_AVAILABILITY":
                    {
                        string UserName = MDVUtility.ToStr(context.Request["UserName"]);
                        string strJSONData = GetUserNameStatus(UserName);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CHECK_IS_DATA_PRIVACY":
                    {
                        
                        string strJSONData = GetIsDataPrivacy();

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}
