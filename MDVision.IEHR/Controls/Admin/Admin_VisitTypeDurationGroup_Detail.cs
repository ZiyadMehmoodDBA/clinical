using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.Model.Schedule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
//using MDVision.Model.Schedule.VisitTypeDuartionsModel;


namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_VisitTypeDurationGroup_Detail
    {
        private BLLSchedule bLLVisitTypeObj = null;

        public Admin_VisitTypeDurationGroup_Detail()
        {
            bLLVisitTypeObj = new BLLSchedule();
        }

        #region Singleton
        private static Admin_VisitTypeDurationGroup_Detail _obj = null;
        public static Admin_VisitTypeDurationGroup_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_VisitTypeDurationGroup_Detail();
            return _obj;
        }
        #endregion

        private string SaveVisitTypeGroupData(string fieldJson)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("VisitTypeDurationGroup", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    var JSONFields = ser.Deserialize<dynamic>(fieldJson);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    DSScheduleSetup.VisitTypeDurationGroupRow dr = dsSchedule.VisitTypeDurationGroup.NewVisitTypeDurationGroupRow();

                    dr.Id = MDVUtility.ToInt64(JSONFields["VisitGroupId"]);
                    dr.Name = JSONFields["VisitDurationGroupName"];
                    dr.IsActive = MDVUtility.ToStr(JSONFields["IsActive"]) == "True" ? true : false;
                    if (!string.IsNullOrEmpty(JSONFields["Entity"]))
                        dr.EntityId = MDVUtility.ToInt64(JSONFields["Entity"]);
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsSchedule.VisitTypeDurationGroup.AddVisitTypeDurationGroupRow(dr);

                    BLObject<DSScheduleSetup> obj = bLLVisitTypeObj.InsertVisitTypeDurationGroup(dsSchedule);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        long visitTypeDurationGroupId = MDVUtility.ToInt64(dsSchedule.Tables[dsSchedule.VisitTypeDurationGroup.TableName].Rows[0][dsSchedule.VisitTypeDurationGroup.IdColumn.ColumnName]);
                        if (visitTypeDurationGroupId != 0)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Save_Message,
                                VisitTypeDurationGroupId = dsSchedule.Tables[dsSchedule.VisitTypeDurationGroup.TableName].Rows[0][dsSchedule.VisitTypeDurationGroup.IdColumn.ColumnName]
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = dsSchedule.Tables[dsSchedule.VisitTypeDurationGroup.TableName].Rows[0][dsSchedule.VisitTypeDurationGroup.ErrorMessageColumn.ColumnName]
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = obj.Message
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

        private string SaveVisitTypeDurationData(string fieldJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var JSONFields = ser.Deserialize<dynamic>(fieldJson);
                var JSONFields2 = ser.Deserialize<MDVision.Model.Schedule.VisitTypeDurationsModel>(fieldJson);
                Int64 VisitTypeId, VisitDurationId, VisitGroupId;
                float Duration;
                bool IsActive;
                string CreatedBy, ModifiedBy, message = "",color;
                BLObject<string> retVal;
                DateTime CreatedOn, ModifiedOn;

                DSScheduleSetup dsSchedule = new DSScheduleSetup();


                DSScheduleSetup.VisitTypeDurationGroupRow dr = dsSchedule.VisitTypeDurationGroup.NewVisitTypeDurationGroupRow();

                foreach (var item in JSONFields2.VisitTypeDurations)
                {
                    Duration = MDVUtility.Tofloat(item.Duration);
                    color = MDVUtility.ToStr(item.Color);
                    VisitDurationId = MDVUtility.ToInt64(item.VisitTypeId);
                    VisitTypeId = MDVUtility.ToInt64(JSONFields["VisitDurationId"]);
                    VisitGroupId = MDVUtility.ToInt64(JSONFields["VisitGroupId"]);
                    IsActive = MDVUtility.ToStr(JSONFields["IsActive"]) == "True" ? true : false;
                    CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    CreatedOn = DateTime.Now;
                    ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    ModifiedOn = DateTime.Now;
                    retVal = bLLVisitTypeObj.InsertVisitTypeDurations(VisitTypeId, VisitGroupId, VisitDurationId, Duration, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn,color);
                }
                //if (retVal.Data != "")
                //{
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,

                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                //}
                //else
                //{
                //    var response = new
                //    {
                //        status = false,
                //        message = retVal.Message
                //    };
                //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                //}
                //}
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

        private string FillVisitTypeDurationGroupData(Int64 VisitTypeDurationGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("VisitTypeDurationGroup", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(VisitTypeDurationGroupId)))
                    {
                        DSScheduleSetup ds = null;

                        BLObject<DSScheduleSetup> obj = bLLVisitTypeObj.LoadVisitTypeDurationGroupForm(VisitTypeDurationGroupId, null, null, null);

                        if (obj.Data != null)
                        {
                            ds = obj.Data;
                            if (ds.Tables[ds.VisitTypeDurationGroup.TableName].Rows.Count > 0)
                            {

                                //JavaScriptSerializer ser = new JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    VisitTypeId = VisitTypeDurationGroupId,
                                    VisitDurationGroup_JSON = ds.VisitTypeDurationGroup
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
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
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


        private string UpdateVisitTypeDurationGroup(string fieldsJSON, Int64 VisitTypeGroupId, bool IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("VisitTypeDurationGroup", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    var JSONFields = ser.Deserialize<dynamic>(fieldsJSON);
                    var JSONFields2 = ser.Deserialize<VisitTypeDurationsModel>(fieldsJSON);

                    Int64 VisitTypeId, VisitDurationGroupId, VisitGroupId;
                    float Duration;
                    string CreatedBy, ModifiedBy, GroupName,Color, EntityId, ErrorMessage = "";
                    BLObject<string> retVal;
                    DateTime CreatedOn, ModifiedOn;
                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    bool IsOk = true;
                    foreach (var item in JSONFields2.VisitTypeDurations)
                    {

                        GroupName = MDVUtility.ToStr(JSONFields["VisitDurationGroupName"]);
                        EntityId = MDVUtility.ToStr(JSONFields["Entity"]);
                        Duration = MDVUtility.Tofloat(item.Duration);
                        VisitTypeId = MDVUtility.ToInt64(item.VisitTypeId);
                        VisitDurationGroupId = MDVUtility.ToInt64(item.VisitDurationGroupId);
                        VisitGroupId = MDVUtility.ToInt64(JSONFields["VisitGroupId"]);
                        IsActive = MDVUtility.ToStr(JSONFields["IsActive"]) == "True" ? true : false;
                        CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        CreatedOn = DateTime.Now;
                        ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        ModifiedOn = DateTime.Now;
                        Color = MDVUtility.ToStr(item.Color);
                        retVal = bLLVisitTypeObj.UpdateVisitTypeDurationGroup(GroupName, VisitTypeId, VisitGroupId, VisitDurationGroupId, Duration, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, EntityId, Color);
                        if (retVal.Data == null)
                        {
                            IsOk = false;
                            ErrorMessage = retVal.Message;
                            break;
                        }
                    }
                    if (!IsOk)
                    {
                        var response = new
                        {
                            status = false,
                            message = ErrorMessage
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
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
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
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
                case "SAVE_VISIT_TYPE_DURATION_GROUP":
                    {
                        string fieldsJSON = context.Request["VisitTypeDurationGroupData"];
                        string strJSONData = SaveVisitTypeGroupData(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_VISIT_TYPE_DURATIONS":
                    {
                        string fieldsJSON = context.Request["VisitTypeDurationsData"];
                        string strJSONData = SaveVisitTypeDurationData(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_VISIT_TYPE_DURATION_GROUP":
                    {
                        string strVisitTypeDurationGroupId = context.Request["VisitTypeDurationGroupId"];
                        string strJSONData = FillVisitTypeDurationGroupData(MDVUtility.ToInt64(strVisitTypeDurationGroupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_VISIT_TYPE_DURATION_GROUP":
                    {
                        string strVisitTypeDurationGroupId = context.Request["VisitTypeDurationGroupId"];
                        string strIsActive = context.Request["IsActive"];
                        string fieldJSON = context.Request["VisitTypeDurationGroupData"];
                        string strJSONData = UpdateVisitTypeDurationGroup(fieldJSON, MDVUtility.ToInt64(strVisitTypeDurationGroupId), MDVUtility.ToBool(strIsActive));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}