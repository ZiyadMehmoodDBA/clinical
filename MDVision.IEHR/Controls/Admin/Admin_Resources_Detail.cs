

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
    public class Admin_Resources_Detail
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_Resources_Detail()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_Resources_Detail _obj = null;
        public static Admin_Resources_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_Resources_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the resources.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveResources(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resources", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsEntity = new DSProfile();
                    DSProfile.ResourcesRow dr = dsEntity.Resources.NewResourcesRow();

                    dr.Name = "";
                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFacility"]))
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFacility"]);
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.Duration = ((String)(SearchedfieldsJSON["txtDuration"])).Replace("_", "");
                    dr.ProviderRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkProviderRequired"]) == "True" ? true : false;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.ResourceProviderId = string.IsNullOrEmpty(SearchedfieldsJSON["hfResourceProvider"]) ? 0 : MDVUtility.ToInt64(SearchedfieldsJSON["hfResourceProvider"]);
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsEntity.Resources.AddResourcesRow(dr);
                    BLObject<DSProfile> obj = BLLAdminProfileObj.InsertResources(ref dsEntity);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message_Contact_Admin,
                            ResourceId = dsEntity.Tables[dsEntity.Resources.TableName].Rows[0][dsEntity.Resources.ResourceIdColumn.ColumnName]
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
        /// Updates the resources.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ResourceId">The resource identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateResources(string fieldsJSON, Int64 ResourceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resources", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {

                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsEntity = new DSProfile();
                    //DSProfile.ResourcesRow dr = dsEntity.Resources.NewResourcesRow();
                    BLObject<DSProfile> objLoad = BLLAdminProfileObj.LoadResources(ResourceId, null, null);
                    dsEntity = objLoad.Data;
                    foreach (DSProfile.ResourcesRow dr in dsEntity.Tables[dsEntity.Resources.TableName].Rows)
                    {
                        //dr.ResourceId = ResourceId;
                        dr.Name = "";
                        dr.ShortName = SearchedfieldsJSON["txtShortName"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFacility"]))
                            dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFacility"]);
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.Duration = ((String)(SearchedfieldsJSON["txtDuration"])).Replace("_", "");
                        dr.ProviderRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkProviderRequired"]) == "True" ? true : false;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        dr.ResourceProviderId = string.IsNullOrEmpty(SearchedfieldsJSON["hfResourceProvider"]) ? 0 : MDVUtility.ToInt64(SearchedfieldsJSON["hfResourceProvider"]);
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsEntity.Resources.AddResourcesRow(dr);
                    //dsEntity.Resources.AcceptChanges();

                    if (dsEntity.Tables[dsEntity.Resources.TableName].Rows.Count > 0)
                    {
                        //dsEntity.Resources.Rows[0].SetModified();
                        BLObject<DSProfile> obj = BLLAdminProfileObj.UpdateResources(ref dsEntity);
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
        /// Fills the resources.
        /// </summary>
        /// <param name="ResourceID">The resource identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillResources(Int64 ResourceID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resources", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ResourceID)))
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
                        DSProfile dsEntity = null;
                        BLObject<DSProfile> obj = BLLAdminProfileObj.LoadResources(ResourceID, null, null);
                        if (obj.Data != null)
                        {
                            dsEntity = obj.Data;
                            if (dsEntity.Tables[dsEntity.Resources.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsEntity.Tables[dsEntity.Resources.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtName", MDVUtility.ToStr(dr[dsEntity.Resources.NameColumn.ColumnName])},
                            { "txtShortName", MDVUtility.ToStr(dr[dsEntity.Resources.ShortNameColumn.ColumnName])},
                            { "ddlFacility", MDVUtility.ToStr(dr[dsEntity.Resources.FacilityIdColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsEntity.Resources.DescriptionColumn.ColumnName])},
                            { "txtDuration", MDVUtility.ToStr(dr[dsEntity.Resources.DurationColumn.ColumnName])},
                            { "chkProviderRequired", MDVUtility.ToStr(dr[dsEntity.Resources.ProviderRequiredColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsEntity.Resources.IsActiveColumn.ColumnName])},
                            {"txtResourceProvider",MDVUtility.ToStr(dr[dsEntity.Resources.RProviderNameColumn.ColumnName])},
                            { "hfResourceProvider",MDVUtility.ToStr(dr[dsEntity.Resources.ResourceProviderIdColumn.ColumnName])},
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    ResourcesFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the resources.
        /// </summary>
        /// <param name="ResourceID">The resource identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteResources(Int64 ResourceID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resources", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ResourceID)))
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
                        BLObject<string> obj = BLLAdminProfileObj.DeleteResources(MDVUtility.ToStr(ResourceID));
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
        /// Updates the resource is active.
        /// </summary>
        /// <param name="ResourceId">The resource identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateResourceIsActive(Int64 ResourceId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resources", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSProfile dsProfile = null;
                    BLObject<DSProfile> obj = BLLAdminProfileObj.LoadResources(ResourceId, null, null);
                    dsProfile = obj.Data;
                    if (dsProfile.Tables[dsProfile.Resources.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsProfile.Tables[dsProfile.Resources.TableName].Rows[0];
                        dr[dsProfile.Resources.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSProfile> objResource = BLLAdminProfileObj.UpdateResources(ref dsProfile);
                        string successMsg;
                        if (objResource.Data != null)
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
                                Message = objResource.Message
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
        /// Handle the Resource Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_RESOURCES":
                    {
                        string fieldsJSON = context.Request["ResourcesData"];
                        string strJSONData = SaveResources(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_RESOURCES":
                    {
                        string strResourceId = context.Request["ResourceID"];
                        string strJSONData = FillResources(MDVUtility.ToInt64(strResourceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_RESOURCES":
                    {
                        string strResourceId = context.Request["ResourceID"];
                        string strJSONData = DeleteResources(MDVUtility.ToInt64(strResourceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_RESOURCES":
                    {
                        string fieldsJSON = context.Request["ResourcesData"];
                        Int64 ResourceID = MDVUtility.ToInt64(context.Request["ResourceID"]);
                        string strJSONData = UpdateResources(fieldsJSON, ResourceID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_RESOURCE_ACTIVE_INACTIVE":
                    {
                        Int64 ResourceID = MDVUtility.ToInt64(context.Request["ResourceID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateResourceIsActive(ResourceID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}