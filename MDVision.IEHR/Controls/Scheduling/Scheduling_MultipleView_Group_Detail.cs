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


namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_MultipleView_Group_Detail
    {
         private BLLSchedule BLLScheduleObj = null;
         public Scheduling_MultipleView_Group_Detail()
        {
            BLLScheduleObj = new BLLSchedule();
        }

        #region Singleton
        private static Scheduling_MultipleView_Group_Detail _obj = null;
        public static Scheduling_MultipleView_Group_Detail Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_MultipleView_Group_Detail();
            return _obj;
        }
        #endregion

        #region "Private Functions"
        private string SaveScheduleGroup(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSScheduleGroup dsViewGroup = new DSScheduleGroup();
                DSScheduleGroup.MultipleScheduleGroupsRow dr = dsViewGroup.MultipleScheduleGroups.NewMultipleScheduleGroupsRow();

                dr.ShortName = SearchedfieldsJSON["txtShortName"];
                dr.Description = SearchedfieldsJSON["txtDescription"];
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                    dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                //dr.ProviderId = SearchedfieldsJSON["hfProviderIds"];
                //dr.ResourceId = SearchedfieldsJSON["hfResourceIds"];
                dr.ShortName = SearchedfieldsJSON["txtShortName"];
                dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsViewGroup.MultipleScheduleGroups.AddMultipleScheduleGroupsRow(dr);
                BLObject<DSScheduleGroup> obj =BLLScheduleObj.InsertScheduleGroups(dsViewGroup);
                dsViewGroup = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        MSGroupId = dsViewGroup.Tables[dsViewGroup.MultipleScheduleGroups.TableName].Rows[0][dsViewGroup.MultipleScheduleGroups.MSGroupIdColumn.ColumnName]
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
        private string FillScheduleGroup(Int32 MSGroupId)
        {
            try
            {
                DSScheduleGroup dsSchedule = null;
                DSProfile dsProfile = new DSProfile();
              //  BLObject<DSScheduleGroup> obj =BLLScheduleObj.LoadScheduleGroups(MSGroupId, null, null, null, null); 
                BLObject<DSScheduleGroup> obj =BLLScheduleObj.LoadScheduleGroupsByMSGroupId(MSGroupId);

                if (obj.Data != null)
                {
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        string priGroup = string.Empty;
                        if (dsSchedule.Tables[dsSchedule.MultipleScheduleGroups.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsSchedule.Tables[dsSchedule.MultipleScheduleGroups.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsSchedule.MultipleScheduleGroups.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsSchedule.MultipleScheduleGroups.DescriptionColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsSchedule.MultipleScheduleGroups.IsActiveColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsSchedule.MultipleScheduleGroups.EntityIdColumn.ColumnName])},
                            //{ "hfProviderIds", MDVUtility.ToStr(dr[dsSchedule.MultipleScheduleGroups.ProviderIdColumn.ColumnName])},
                            //{ "hfResourceIds", MDVUtility.ToStr(dr[dsSchedule.MultipleScheduleGroups.ResourceIdColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            priGroup = js.Serialize(keyValues);
                        }
                        var response = new
                        {
                            status = true,
                            Provider_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.ScheduleProvider.TableName]),
                            Resource_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.ScheduleResource.TableName]),
                            ScheduleGroupFill_JSON = priGroup
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
                        Message = obj.Message
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


        private string UpdateScheduleGroup(string fieldsJSON, Int32 MSGroupId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var UpdatefieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSScheduleGroup dsSchedule = new DSScheduleGroup();
                BLObject<DSScheduleGroup> objLoad =BLLScheduleObj.LoadScheduleGroups(MSGroupId, null, null, null, null);
                dsSchedule = objLoad.Data;

                //DSScheduleGroup.MultipleScheduleGroupsRow dr = dsSchedule.MultipleScheduleGroups.NewMultipleScheduleGroupsRow();
                foreach (DSScheduleGroup.MultipleScheduleGroupsRow dr in dsSchedule.Tables[dsSchedule.MultipleScheduleGroups.TableName].Rows)
                {
                    //dr.MSGroupId = MSGroupId;
                    dr.ShortName = UpdatefieldsJSON["txtShortName"];
                    dr.Description = UpdatefieldsJSON["txtDescription"];
                    if (!string.IsNullOrEmpty(UpdatefieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(UpdatefieldsJSON["ddlEntity"]);
                    dr.IsActive = MDVUtility.ToStr(UpdatefieldsJSON["chkActive"]) == "True" ? true : false;
                    //dr.ProviderId = UpdatefieldsJSON["hfProviderIds"];
                    //dr.ResourceId = UpdatefieldsJSON["hfResourceIds"];
                    //dr.CreatedBy = "";
                    //dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                //dr.MSGroupId = MSGroupId;
                //dr.ShortName = UpdatefieldsJSON["txtShortName"];
                //dr.Description = UpdatefieldsJSON["txtDescription"];
                //if (!string.IsNullOrEmpty(UpdatefieldsJSON["ddlEntity"]))
                //    dr.EntityId = MDVUtility.ToInt64(UpdatefieldsJSON["ddlEntity"]);
                //dr.IsActive = MDVUtility.ToStr(UpdatefieldsJSON["chkActive"]) == "True" ? true : false;
                //dr.ProviderId = UpdatefieldsJSON["hfProviderIds"];
                //dr.ResourceId = UpdatefieldsJSON["hfResourceIds"];
                //dr.CreatedBy = "";
                //dr.CreatedOn = DateTime.Now;
                //dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //dr.ModifiedOn = DateTime.Now;

                #region Database Updation
                //dsSchedule.MultipleScheduleGroups.AddMultipleScheduleGroupsRow(dr);
                //dsSchedule.MultipleScheduleGroups.AcceptChanges();

                if (dsSchedule.Tables[dsSchedule.MultipleScheduleGroups.TableName].Rows.Count > 0)
                {
                    //dsSchedule.MultipleScheduleGroups.Rows[0].SetModified();
                    BLObject<DSScheduleGroup> obj =BLLScheduleObj.UpdateScheduleGroups(dsSchedule);
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
        private string DeleteScheduleGroup(Int32 MSGroupId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(MSGroupId)))
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
                    BLObject<string> obj =BLLScheduleObj.DeleteScheduleGroups(MDVUtility.ToStr(MSGroupId));
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string UpdateScheduleGroupIsActive(Int32 MSGroupId, Int64 IsActive)
        {
            try
            {
                DSScheduleGroup dsSchedule = null;
                BLObject<DSScheduleGroup> obj =BLLScheduleObj.LoadScheduleGroups(MSGroupId, null, null, null,null);
                dsSchedule = obj.Data;
                if (dsSchedule.Tables[dsSchedule.MultipleScheduleGroups.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsSchedule.Tables[dsSchedule.MultipleScheduleGroups.TableName].Rows[0];
                    dr[dsSchedule.MultipleScheduleGroups.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSScheduleGroup> objMultipleSchedule =BLLScheduleObj.UpdateScheduleGroups(dsSchedule);
                    string successMsg;
                    if (objMultipleSchedule.Data != null)
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
                            Message = objMultipleSchedule.Message
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

        private string SaveSchGroupProvider(Int32 MSGroupId, Int64 ProvidersId, Int64 FacilityId, Int64 ProScheduleId)
        {
            try
            {
                DSScheduleGroup dsSchGroup = new DSScheduleGroup();
                DSScheduleGroup.GroupProvidersRow dr = dsSchGroup.GroupProviders.NewGroupProvidersRow();

                dr.MSGroupId = MSGroupId;
                dr.ProviderId= ProvidersId;
                dr.FacilityId = FacilityId;
                dr.ProScheduleId = ProScheduleId;
               
                dr.IsActive = true;
                
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsSchGroup.GroupProviders.AddGroupProvidersRow(dr);
                BLObject<DSScheduleGroup> obj =BLLScheduleObj.InsertSchGroupProvider(ref dsSchGroup);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        GrpProvidersId = dsSchGroup.Tables[dsSchGroup.GroupProviders.TableName].Rows[0][dsSchGroup.GroupProviders.GrpProvidersIdColumn.ColumnName]
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string DeleteSchGroupProvider(string GrpProvidersId)
        {
            try
            {
                if (string.IsNullOrEmpty(GrpProvidersId))
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
                    BLObject<string> obj =BLLScheduleObj.DeleteGroupProvider(GrpProvidersId);

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

        private string SaveSchGroupResource(Int32 MSGroupId, Int64 ResourcesId, Int64 FacilityId, Int64 ResScheduleId)
        {
            try
            {
                DSScheduleGroup dsSchGroup = new DSScheduleGroup();
                DSScheduleGroup.GroupResourcesRow dr = dsSchGroup.GroupResources.NewGroupResourcesRow();

                dr.MSGroupId = MSGroupId;
                dr.ResourceId = ResourcesId;
                dr.FacilityId = FacilityId;
                dr.ResScheduleId = ResScheduleId;


                dr.IsActive = true;

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsSchGroup.GroupResources.AddGroupResourcesRow(dr);
                BLObject<DSScheduleGroup> obj =BLLScheduleObj.InsertSchGroupResource(ref dsSchGroup);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        GrpResourcesId = dsSchGroup.Tables[dsSchGroup.GroupResources.TableName].Rows[0][dsSchGroup.GroupResources.GrpResourcesIdColumn.ColumnName]
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string DeleteSchGroupResource(string GrpResourcesId)
        {
            try
            {
                if (string.IsNullOrEmpty(GrpResourcesId))
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
                    BLObject<string> obj =BLLScheduleObj.DeleteGroupResource(GrpResourcesId);

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

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                #region Security Role Action Commands

                case "SAVE_SCHEDULE_GROUP":
                    {
                        string fieldsJSON = context.Request["MultipleViewGroupData"];
                        string strJSONData = SaveScheduleGroup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_SCHEDULE_GROUP":
                    {
                        string strMSGroupId = context.Request["MSGroupId"];
                        string strJSONData = FillScheduleGroup(MDVUtility.ToInt32(strMSGroupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_SCHEDULE_GROUP":
                    {
                        string strMSGroupId = context.Request["MSGroupId"];
                        string strJSONData = DeleteScheduleGroup(MDVUtility.ToInt32(strMSGroupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SCHEDULE_GROUP":
                    {
                        string fieldsJSON = context.Request["MultipleViewGroupData"];
                        Int32 MSGroupId = MDVUtility.ToInt32(context.Request["MSGroupId"]);
                        string strJSONData = UpdateScheduleGroup(fieldsJSON, MSGroupId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SCHEDULE_GROUP_ACTIVE_INACTIVE":
                    {
                        Int32 MSGroupId = MDVUtility.ToInt32(context.Request["MSGroupId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateScheduleGroupIsActive(MSGroupId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_SCH_GROUP_PROVIDER":
                    {
                        string GrpProvidersId = context.Request["GrpProvidersId"];
                        string strJSONData = DeleteSchGroupProvider(GrpProvidersId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_SCH_GROUP_RESOURCE":
                    {
                        string GrpResourcesId = context.Request["GrpResourcesId"];
                        string strJSONData = DeleteSchGroupResource(GrpResourcesId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_SCH_GROUP_PROVIDER":
                    {
                        string MSGroupId = context.Request["MSGroupId"];
                        string ProviderId = context.Request["ProviderID"];
                        string FacilityId = context.Request["FacilityId"];
                        string ProScheduleId = context.Request["ProScheduleId"];
                        string strJSONData = SaveSchGroupProvider(MDVUtility.ToInt32(MSGroupId), MDVUtility.ToInt64(ProviderId), MDVUtility.ToInt64(FacilityId), MDVUtility.ToInt64(ProScheduleId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_SCH_GROUP_RESOURCE":
                    {
                        string MSGroupId = context.Request["MSGroupId"];
                        string ResourceId = context.Request["ResourceId"];
                        string FacilityId = context.Request["FacilityId"];
                        string ResScheduleId = context.Request["ResScheduleId"];
                        string strJSONData = SaveSchGroupResource(MDVUtility.ToInt32(MSGroupId), MDVUtility.ToInt64(ResourceId), MDVUtility.ToInt64(FacilityId), MDVUtility.ToInt64(ResScheduleId));

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