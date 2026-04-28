using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_VisitTypeDurationGroup
    {
        private BLLSchedule bLLVisitTypeObj = null;

        public Admin_VisitTypeDurationGroup()
        {
            bLLVisitTypeObj = new BLLSchedule();
        }

        #region Singleton
        private static Admin_VisitTypeDurationGroup _obj = null;
        public static Admin_VisitTypeDurationGroup Instance()
        {
            if (_obj == null)
                _obj = new Admin_VisitTypeDurationGroup();
            return _obj;
        }
        #endregion

        private string LoadVisitTypeDuration(string fieldsJSON, Int64 VisitTypeDurationGroupID, int PageNumber, int RowsPerPage, string ParentCtrl = "")
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("VisitTypeDurationGroup", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    var JSONFields = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj;

                    if (JSONFields == null)
                        obj = bLLVisitTypeObj.LoadVisitTypeDurationGroup(VisitTypeDurationGroupID, null, null, null);
                    else
                        obj = bLLVisitTypeObj.LoadVisitTypeDurationGroup(VisitTypeDurationGroupID, JSONFields["txtName"], JSONFields["checkIsActice"], JSONFields["ddlEntity"], PageNumber, RowsPerPage);

                    dsSchedule = obj.Data;

                    if (obj.Data != null)
                    {
                        if (dsSchedule.Tables[dsSchedule.VisitTypeDurationGroup.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                VisitTypeDurationGroupCount = dsSchedule.Tables[dsSchedule.VisitTypeDurationGroup.TableName].Rows.Count,
                                iTotalDisplayRecords = (dsSchedule.VisitTypeDurationGroup.Rows.Count > 0) ? dsSchedule.VisitTypeDurationGroup.Rows[0][dsSchedule.VisitTypeDurationGroup.RecordCountColumn.ColumnName] : 0,
                                VisitTypeDurationGroupLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.VisitTypeDurationGroup.TableName])
                            };

                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                VisitTypeDurationGroupCount = 0,
                                VisitTypeDurationGroupLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.VisitTypeDurationGroup.TableName])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VisitTypeDurationGroupCount = 0,
                            Message = obj.Message
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

        private string DeleteVisitTypeDurationGroup(Int64 VisitTypeDurationGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("VisitTypeDurationGroup", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(VisitTypeDurationGroupId)))
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
                        BLObject<string> obj = bLLVisitTypeObj.DeleteVisitTypeDurationGroup(MDVUtility.ToStr(VisitTypeDurationGroupId));
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

        private string UpdateVisitTypeDurationGroupActiveInActive(Int64 VisitDurationGroupId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("VisitTypeDurationGroup", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(VisitDurationGroupId)))
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
                        BLObject<string> obj = bLLVisitTypeObj.UpdateVisitTypeDurationGroupActiveInActive(VisitDurationGroupId, IsActive);
                        if (obj.Data == "")
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

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string commandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (commandAction)
            {
                case "SEARCH_VISIT_TYPE_DURATION_GROUP":
                    {
                        string fieldsJSON = context.Request["VisitTypeData"];
                        Int64 VisitTypeDurationGroupID = MDVUtility.ToInt64(context.Request["VisitTypeDurationGroupID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string ParentCtrl = MDVUtility.ToStr(context.Request["ParentCtrl"]);

                        string strJSONData = LoadVisitTypeDuration(fieldsJSON, VisitTypeDurationGroupID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage), ParentCtrl);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_VISIT_TYPE_DURATION_GROUP":
                    {
                        string strVisitDurationGroupId = context.Request["VisitTypeDurationGroupID"];
                        string strJSONData = DeleteVisitTypeDurationGroup(MDVUtility.ToInt64(strVisitDurationGroupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_VISIT_TYPE_GROUP_ACTIVE_INACTIVE":
                    {
                        string strVisitDurationGroupId = context.Request["VisitTypeDurationGroupID"];
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateVisitTypeDurationGroupActiveInActive(MDVUtility.ToInt64(strVisitDurationGroupId), IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion

    }
}