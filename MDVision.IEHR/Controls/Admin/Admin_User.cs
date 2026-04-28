using System;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using Newtonsoft.Json;
using MDVision.IEHR.Security;
using System.Web.Script.Serialization;
using MDVision.IEHR.Common;
using iTextSharp.text;
using System.Collections.Generic;
using MDVision.Model.Security;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_User
    {
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        public Admin_User()
        {

            BLLAdminSecurityObj = new BLLAdminSecurity();
        }
        #region Singleton

        private static Admin_User _instance = null;
        public static Admin_User Instance()
        {
            if (_instance == null)
                _instance = new Admin_User();
            return _instance;
        }

        #endregion

        #region Data Members
        #endregion

        #region Private Functions
        /// <summary>
        /// Load User methods.
        /// </summary>
        /// <param name="context">The context.</param>
        private string LoadUser(string fieldsJSON, Int64 UserID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Users", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    BLObject<DSUsers> objUser;
                    DSUsers dsEntity = null;
                    if (SearchedfieldsJSON == null)

                        objUser = BLLAdminSecurityObj.LoadUser(UserID, null, null, null, null, null, null);
                    else
                        objUser = BLLAdminSecurityObj.LoadUser(UserID, SearchedfieldsJSON["txtUserName"], SearchedfieldsJSON["lstEntityId"], SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["chkIsActice"], null, PageNumber, RowsPerPage);

                    if (objUser.Data != null)
                    {
                        dsEntity = objUser.Data;

                        var response = new
                        {
                            status = true,
                            UserCount = dsEntity.Tables[dsEntity.Users.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEntity.Users.Rows.Count > 0) ? dsEntity.Users.Rows[0][dsEntity.Users.RecordCountColumn.ColumnName] : 0,
                            UserLoad_JSON = MDVUtility.JSON_DataTable(dsEntity.Tables[dsEntity.Users.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objUser.Message,
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        private string LoadLoggedInusers(string fieldsJSON)
        {
            //TODO CHECK IF ITS DEFAULT UESR
            try
            {
                if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == AppPrivileges.DefaultUser)
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    string Username = MDVUtility.ToStr(SearchedfieldsJSON["txtUserName"]).ToUpper();
                    var EntityId = MDVUtility.ToStr(SearchedfieldsJSON["lstEntityId"]);
                    string SessionStatus = MDVUtility.ToStr(SearchedfieldsJSON["ddSessionStatusId"]);
                    List<LoginHelperModel> loggedInUsersList = new List<LoginHelperModel>();

                    //switch (SessionStatus.ToLower())
                    //{
                    //    case "active":
                    //        loggedInUsersList = UserLoginHelper.LoggedInUsers.Select(LoggedInusers => LoggedInusers.Value).Where(criteria => (criteria.SessionId != MDVSession.Current.SessionID) && (criteria.AppUsername.StartsWith(Username) || string.IsNullOrEmpty(Username)) && (criteria.EntityId == EntityId || string.IsNullOrEmpty(EntityId)) && (criteria.IsLogin) && (!criteria.IsSuspended)).ToList();
                    //        break;
                    //    case "loggedout":
                    //        loggedInUsersList = UserLoginHelper.LoggedInUsers.Select(LoggedInusers => LoggedInusers.Value).Where(criteria => (criteria.SessionId != MDVSession.Current.SessionID) && (criteria.AppUsername.StartsWith(Username) || string.IsNullOrEmpty(Username)) && (criteria.EntityId == EntityId || string.IsNullOrEmpty(EntityId)) && (!criteria.IsLogin)).ToList();
                    //        break;
                    //    case "suspended":
                    //        loggedInUsersList = UserLoginHelper.LoggedInUsers.Select(LoggedInusers => LoggedInusers.Value).Where(criteria => (criteria.SessionId != MDVSession.Current.SessionID) && (criteria.AppUsername.StartsWith(Username) || string.IsNullOrEmpty(Username)) && (criteria.EntityId == EntityId || string.IsNullOrEmpty(EntityId)) && (criteria.IsSuspended)).ToList();
                    //        break;
                    //    default:
                    //        loggedInUsersList = UserLoginHelper.LoggedInUsers.Select(LoggedInusers => LoggedInusers.Value).Where(criteria => (criteria.SessionId != MDVSession.Current.SessionID) && (criteria.AppUsername.StartsWith(Username) || string.IsNullOrEmpty(Username)) && (criteria.EntityId == EntityId || string.IsNullOrEmpty(EntityId))).ToList();
                    //        break;
                    //}


                    var loggedInUsersCount = loggedInUsersList.Count;

                    //if (loggedInUsersCount > 0)
                    //{
                    var response = new
                    {
                        status = true,
                        loggedInUsersCount = loggedInUsersCount,
                        LoggedInusers_JSON = JsonConvert.SerializeObject(loggedInUsersList)
                    };
                    return (JsonConvert.SerializeObject(response));
                    //}
                    //else
                    //{
                    //    var response = new
                    //    {
                    //        status = false,
                    //        Message = "No one is Logged in Except you :)",
                    //    };
                    //    return (JsonConvert.SerializeObject(response));
                    //}
                }
                else
                {

                    var response = new
                    {
                        status = false,
                        Message = "You Are not Auhtorized to View These Records",
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
                return (JsonConvert.SerializeObject(response));
            }
        }


        #region Service Command Handler
        /// <summary>
        /// Handle the User Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "SEARCH_USER":
                    {

                        string fieldsJSON = context.Request["UserData"];
                        Int64 UserID = MDVUtility.ToInt64(context.Request["UserID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);

                        string strJSONData = LoadUser(fieldsJSON, UserID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_LOGGED_IN_USER":
                    {

                        string fieldsJSON = context.Request["UserData"];


                        string strJSONData = LoadLoggedInusers(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SUSPEND_SESSION":
                    {
                        var SessionId = context.Request["SessionId"];
                        string strJSONData = "";

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


            }
        }

        #endregion
    }
}