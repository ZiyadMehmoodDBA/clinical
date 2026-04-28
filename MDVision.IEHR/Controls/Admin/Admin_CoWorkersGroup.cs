using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using MDVision.Business.BLL;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using System.Data;
using MDVision.IEHR.Common;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_CoWorkersGroup
    {
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        public Admin_CoWorkersGroup()
        {

            BLLAdminSecurityObj = new BLLAdminSecurity();
        }
        #region Singleton
        private static Admin_CoWorkersGroup _obj = null;
        public static Admin_CoWorkersGroup Instance()
        {
            if (_obj == null)
                _obj = new Admin_CoWorkersGroup();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// LoadCoWorkersGroup
        /// </summary>
        /// <param name="CoWorkerGroupData"></param>
        /// <param name="CoWorkerGroupId"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        private string LoadCoWorkersGroup(string CoWorkerGroupData, long CoWorkerGroupId, Int32 PageNumber, Int32 RowsPerPage)
        {
            string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Co-workers Group", "SEARCH")).ToString();
            if (string.IsNullOrEmpty(privilegesMessage))
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var data = ser.Deserialize<dynamic>(CoWorkerGroupData);
                DSUsers ds = null;
                var Name = "";
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(data["txtUserName"])))
                {
                    Name = MDVUtility.ToStr(data["txtUserName"]);
                }
                string IsActive = MDVUtility.ToStr(data["chkIsActice"]);
                BLObject<DSUsers> obj = BLLAdminSecurityObj.LoadCoWorkerGroup(CoWorkerGroupId, Name, IsActive, PageNumber, RowsPerPage);
                if (obj.Data != null)
                {
                    ds = obj.Data;
                    if (ds.Tables[ds.CoWorkersGroup.TableName].Rows.Count > 0)
                    {
                        var rows = ds.Tables[ds.CoWorkersGroup.TableName];
                        var dataRows = MDVUtility.JSON_DataTable(rows);
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            CoWorkersGroup_JSON = dataRows,
                            iTotalDisplayRecords = ds.Tables[ds.CoWorkersGroup.TableName].Rows[0]["RecordCount"],
                            CoWorkersCount = ds.Tables[ds.CoWorkersGroup.TableName].Rows[0]["RecordCount"]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            iTotalDisplayRecords = 0,
                            CoWorkersCount = 0
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        /// <summary>
        /// ActiveInActiveTemplate
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        /// 
        private string DeleteCOWorkerGroup(Int64 CoWorkerGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Co-workers Group", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(CoWorkerGroupId)))
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
                        BLObject<string> obj = BLLAdminSecurityObj.DeleteCoWorkerGroup(MDVUtility.ToStr(CoWorkerGroupId));
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
        private string ActiveInActiveTemplate(string CoWorkerGroupId, bool isActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Co-workers Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    BLObject<string> obj = BLLAdminSecurityObj.ActiveInActiveCoWorkerGroup(CoWorkerGroupId, isActive);
                    if (obj.Data == "")
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
                            Message = obj.Data
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

        private string FillCoWorkersGroup(Int64 CoWorkerGroupID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Co-workers Group", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(CoWorkerGroupID)))
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
                        DSUsers ds = null;
                        BLObject<DSUsers> obj = BLLAdminSecurityObj.LoadCoWorkerGroup(CoWorkerGroupID, null, null);
                        if (obj.Data != null)
                        {
                            ds = obj.Data;
                            if (ds.Tables[ds.CoWorkersGroup.TableName].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[ds.CoWorkersGroup.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "CoWorkersGroupId", MDVUtility.ToStr(dr[ds.CoWorkersGroup.CoWorkersGroupIdColumn.ColumnName])},
                            { "Name", MDVUtility.ToStr(dr[ds.CoWorkersGroup.NameColumn.ColumnName])},
                            { "UserID", MDVUtility.ToStr(dr[ds.CoWorkersGroup.UserIDColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[ds.CoWorkersGroup.IsActiveColumn.ColumnName])},
                            { "usernames", MDVUtility.ToStr(dr[ds.CoWorkersGroup.UserNameColumn.ColumnName])},
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    CoWorkerGroup_JSON = js.Serialize(keyValues)
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

                case "SEARCH_COWORKERSGROUP":
                    {
                        long CoWorkersGroupID =MDVUtility.ToInt64( context.Request["CoWorkersGroupID"]);
                        int PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        int RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        string fieldsJSON = context.Request["Data"];
                        string strJSONData = LoadCoWorkersGroup(fieldsJSON,CoWorkersGroupID,PageNumber,RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_COWORKERSGROUP":
                    {
                        long CoWorkerGroupID = MDVUtility.ToInt64(context.Request["CoWorkerGroupIdID"]);
                        string strJSONData = DeleteCOWorkerGroup(MDVUtility.ToInt64(CoWorkerGroupID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_COWORKERSGROUP_ACTIVE_INACTIVE":
                    {
                        String CoWorkerGroupID = MDVUtility.ToStr(context.Request["CoWorkerGroupId"]);
                        bool isAtive= context.Request["IsActive"] == "1";
                        string strJSONData = ActiveInActiveTemplate(CoWorkerGroupID, isAtive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_COWORKERSGROUP":
                    {
                        String CoWorkerGroupID = MDVUtility.ToStr(context.Request["CoWorkersGroupID"]);
                        string strJSONData = FillCoWorkersGroup(MDVUtility.ToInt64(CoWorkerGroupID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}