using System.Data;
using Newtonsoft.Json;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Web;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_CoWorkersGroupDetail
    {
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        public Admin_CoWorkersGroupDetail()
        {

            BLLAdminSecurityObj = new BLLAdminSecurity();
        }
        #region Singleton
        private static Admin_CoWorkersGroupDetail _obj = null;
        public static Admin_CoWorkersGroupDetail Instance()
        {
            if (_obj == null)
                _obj = new Admin_CoWorkersGroupDetail();
            return _obj;
        }
        #endregion



        #region CRUD

        private string SaveCoWorkersGroup(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Co-workers Group", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCoWorkersGroup dsGroup = new DSCoWorkersGroup();
                    DSCoWorkersGroup.GroupRow dr = dsGroup.Group.NewGroupRow();
                    if (MDVUtility.ToInt64(SearchedfieldsJSON["hfCoWorkersGroupId"]) > -1)
                    {
                        dr.CoWorkersGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCoWorkersGroupId"]);
                    }
                    else
                    {
                        dr.CoWorkersGroupId = -1;
                    }

                    dr.Name = SearchedfieldsJSON["txtName"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["UserIDs"]))
                        dr.UserIds = SearchedfieldsJSON["UserIDs"];
                    else
                        dr.UserIds = null;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    #region Database Insertion
                    dsGroup.Group.AddGroupRow(dr);
                    BLObject<DSCoWorkersGroup> obj = BLLAdminSecurityObj.InsertCoWorkersGroup(ref dsGroup);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            CoWorkersGroupId = dsGroup.Tables[dsGroup.Group.TableName].Rows[0][dsGroup.Group.CoWorkersGroupIdColumn.ColumnName]
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
        private string UpdateCoWorkersGroup(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Co-workers Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCoWorkersGroup dsGroup = new DSCoWorkersGroup();
                    //DSCoWorkersGroup.GroupRow dr = dsGroup.Group.NewGroupRow();
                    BLObject<DSCoWorkersGroup> objLoad = BLLAdminSecurityObj.FillCoWorkerGroup(MDVUtility.ToInt64(SearchedfieldsJSON["hfCoWorkersGroupId"]), null, null);
                    dsGroup = objLoad.Data;
                    foreach (DSCoWorkersGroup.GroupRow dr in dsGroup.Tables[dsGroup.Group.TableName].Rows)
                    {
                        if (MDVUtility.ToInt64(SearchedfieldsJSON["hfCoWorkersGroupId"]) > -1)
                        {
                            dr.CoWorkersGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCoWorkersGroupId"]);
                        }
                        else
                        {
                            dr.CoWorkersGroupId = -1;
                        }

                        dr.Name = SearchedfieldsJSON["txtName"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["UserIDs"]))
                            dr.UserIds = SearchedfieldsJSON["UserIDs"];
                        else
                            dr.UserIds = null;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }
                    #region Database Insertion
                    //dsGroup.Group.AddGroupRow(dr);
                    BLObject<DSCoWorkersGroup> obj = BLLAdminSecurityObj.UpdateCoWorkersGroup(ref dsGroup);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
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
        /// Updates the User.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <returns></returns>
        //private string UpdateCoWorkersGroup(string fieldsJSON, Int64 UserId, Int64 IsActive)
        //{
        //    try
        //    {
        //        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

        //        DSUsers dsUser = new DSUsers();
        //        DSUsers.UsersRow dr = dsUser.Users.NewUsersRow();

        //        dr.UserId = UserId;
        //        dr.UserName = SearchedfieldsJSON["txtUserName"];
        //        if (SearchedfieldsJSON["txtUserPassword"] == "")
        //        {
        //            dr.UserPassword = SearchedfieldsJSON["txtUserPasswordHF"];
        //        }
        //        else
        //        {
        //            dr.UserPassword = SearchedfieldsJSON["txtUserPassword"];
        //        }

        //        dr.RcUserName = SearchedfieldsJSON["RcUserName"];
        //        dr.RcPassword = SearchedfieldsJSON["RcPassword"];
        //        dr.RcSigPassword = SearchedfieldsJSON["RcSigPassword"];


        //        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstEntityId"]))
        //            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["lstEntityId"]);
        //        dr.FirstName = SearchedfieldsJSON["txtFirstName"];
        //        dr.LastName = SearchedfieldsJSON["txtLastName"];
        //        dr.EmailAddress = SearchedfieldsJSON["txtEmailAddress"];
        //        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstProviderId"]))
        //            dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["lstProviderId"]);
        //        dr.PhoneNo = SearchedfieldsJSON["txtPhoneNo"];
        //        dr.PhoneExt = "";
        //        dr.EmailAddress = SearchedfieldsJSON["txtEmailAddress"];

        //        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstUserRoles"]))
        //            dr.UserRoleId = MDVUtility.ToInt64(SearchedfieldsJSON["lstUserRoles"]); ;
        //        //Start || 14 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 
        //        if (SearchedfieldsJSON.ContainsKey("lstEmergencyUserRoles"))
        //        {
        //            if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstEmergencyUserRoles"]))
        //            {
        //                dr.EmergencyRoleId = MDVUtility.ToInt64(SearchedfieldsJSON["lstEmergencyUserRoles"]);
        //            }
        //        }
        //        //End   || 14 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 

        //        //Start || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail
        //        dr.DirectAddress = SearchedfieldsJSON["txtDirectAddress"];
        //        //End   || 14 May, 2016 || ZeeshanAK || Change made for adding Direct Address to User detail
        //        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
        //        dr.IsAdmin = MDVUtility.ToStr(SearchedfieldsJSON["chkIsAdmin"]) == "True" ? true : false;
        //        dr.IsEMR = MDVUtility.ToStr(SearchedfieldsJSON["chkIsEMR"]) == "True" ? true : false;
        //        dr.IsLocked = MDVUtility.ToStr(SearchedfieldsJSON["chkIsLocked"]) == "True" ? true : false;
        //        dr.RCopialUser = MDVUtility.ToStr(SearchedfieldsJSON["RCopialUser"]) == "True" ? true : false;
        //        dr.AutoLogOff = MDVUtility.ToInt32(SearchedfieldsJSON["txtAutoLogOff"]);//kr

        //        //dr.CreatedBy = "";
        //        //dr.CreatedOn = DateTime.Now;
        //        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //        dr.ModifiedOn = DateTime.Now;


        //        #region Database Updation
        //        dsUser.Users.AddUsersRow(dr);
        //        dsUser.Users.AcceptChanges();

        //        if (dsUser.Tables[dsUser.Users.TableName].Rows.Count > 0)
        //        {
        //            dsUser.Users.Rows[0].SetModified();
        //            BLObject<DSUsers> obj = BLLAdminSecurityObj.UpdateUser(ref dsUser);
        //            if (obj.Data != null)
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    message = Common.AppPrivileges.Update_Message
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = obj.Message
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = ""
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message)
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        ///// <summary>
        ///// Deletes Users by User Id.
        ///// </summary>
        ///// <param name="PracticeID">The practice identifier.</param>
        ///// <returns></returns>
        //private string DeletCoWorkersGroup(Int64 UserID)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(MDVUtility.ToStr(UserID)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {
        //            BLObject<string> obj = BLLAdminSecurityObj.DeleteUser(MDVUtility.ToStr(UserID));
        //            if (obj.Data == "")
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    Message = Common.AppPrivileges.Delete_Message
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = obj.Data
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }

        //}

        ///// <summary>
        ///// Updates the user is active.
        ///// </summary>
        ///// <param name="UserId">The user identifier.</param>
        ///// <param name="IsActive">The is active.</param>
        ///// <returns></returns>
        //private string UpdateCoWorkersGroupIsActive(Int64 UserId, Int64 IsActive)
        //{
        //    try
        //    {
        //        DSUsers dsUser = null;
        //        BLObject<DSUsers> obj = BLLAdminSecurityObj.LoadUser(UserId, null, null, null, null, null, null);
        //        dsUser = obj.Data;
        //        if (dsUser.Tables[dsUser.Users.TableName].Rows.Count > 0)
        //        {
        //            DataRow dr = dsUser.Tables[dsUser.Users.TableName].Rows[0];
        //            dr[dsUser.Users.IsActiveColumn.ColumnName] = IsActive;

        //            BLObject<DSUsers> objUser = BLLAdminSecurityObj.UpdateUser(ref dsUser);
        //            string successMsg;
        //            if (objUser.Data != null)
        //            {
        //                if (IsActive == 0)
        //                    successMsg = Common.AppPrivileges.Inactive_Message;
        //                else
        //                    successMsg = Common.AppPrivileges.Active_Message;
        //                var response = new
        //                {
        //                    status = true,
        //                    message = successMsg
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = objUser.Message
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = obj.Message
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}




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

                case "SAVE_SAVECOWORKERSGROUP":
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        string fieldsJSON = context.Request["Data"];
                        //Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(context.Request["Data"]) as Dictionary<string, dynamic>;
                        //string userRoleIds = arrJSON.ContainsKey("UserRoles") == true ? MDVUtility.ToStr(arrJSON["UserRoles"]) : "";
                        string strJSONData = SaveCoWorkersGroup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_COWORKERSGROUP":
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        string fieldsJSON = context.Request["Data"];
                        string strJSONData = UpdateCoWorkersGroup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                    //case "FILL_USER":
                    //    {
                    //        string strUserId = context.Request["UserID"];
                    //        string strModuleId = context.Request["ModuleID"];
                    //        string strModuleFormId = context.Request["ModuleFormID"];
                    //        string strJSONData = GetModuleFormUserPrivilegesByUserID(MDVUtility.ToInt64(strUserId), MDVUtility.ToInt64(strModuleId), MDVUtility.ToInt64(strModuleFormId));
                    //        context.Response.ContentType = "text/plain";
                    //        context.Response.Write(strJSONData);
                    //    }
                    //    break;
                    //case "DELETE_USER":
                    //    {
                    //        string strUserId = context.Request["UserID"];
                    //        string strIsAdmin = context.Request["UserID"];
                    //        string strJSONData = DeleteUser(MDVUtility.ToInt64(strUserId));

                    //        context.Response.ContentType = "text/plain";
                    //        context.Response.Write(strJSONData);
                    //    }
                    //    break;
                    //case "UPDATE_USER":
                    //    {
                    //        string fieldsJSON = context.Request["UserData"];
                    //        Int64 UserID = MDVUtility.ToInt64(context.Request["UserID"]);
                    //        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);

                    //        string strJSONData = UpdateUser(fieldsJSON, UserID, IsActive);

                    //        context.Response.ContentType = "text/plain";
                    //        context.Response.Write(strJSONData);
                    //    }
                    //    break;

            }
        }
        #endregion







    }
}