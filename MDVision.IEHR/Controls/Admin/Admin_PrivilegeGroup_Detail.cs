using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.IEHR.Common;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_PrivilegeGroup_Detail
    {
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        public Admin_PrivilegeGroup_Detail()
        {
            BLLAdminSecurityObj = new BLLAdminSecurity();
        }
        #region Singleton
        private static Admin_PrivilegeGroup_Detail _obj = null;
        public static Admin_PrivilegeGroup_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_PrivilegeGroup_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        #region Privilege Group
        /// <summary>
        /// Saves the privilege group.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SavePrivilegeGroup(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSPrivilegeGroup dsPrivGroup = new DSPrivilegeGroup();
                    DSPrivilegeGroup.SecurityGroupRow dr = dsPrivGroup.SecurityGroup.NewSecurityGroupRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.Name = "";
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.IsDeleted = false;
                    dr.IsAdmin = MDVUtility.ToStr(SearchedfieldsJSON["chkAdmin"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsPrivGroup.SecurityGroup.AddSecurityGroupRow(dr);
                    BLObject<DSPrivilegeGroup> obj = BLLAdminSecurityObj.InsertPrivilegeGroup(ref dsPrivGroup);

                    if (obj.Data != null)
                    {
                        dsPrivGroup = obj.Data;
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            PrivilegeGroupId = dsPrivGroup.Tables[dsPrivGroup.SecurityGroup.TableName].Rows[0][dsPrivGroup.SecurityGroup.SecGroupIdColumn.ColumnName]
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
        /// Fills the privilege group.
        /// </summary>
        /// <param name="PrivilegeGroupId">The privilege group identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillPrivilegeGroup(Int64 PrivilegeGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSPrivilegeGroup dsPrivGroup = null;
                    DSProfile dsProfile = new DSProfile();
                    BLObject<DSPrivilegeGroup> obj = BLLAdminSecurityObj.LoadPrivilegeGroupByPrivilegeGroupId(PrivilegeGroupId, 0);

                    dsPrivGroup = obj.Data;
                    if (obj.Data != null)
                    {
                        string priGroup = string.Empty;
                        if (dsPrivGroup.Tables[dsPrivGroup.SecurityGroup.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPrivGroup.Tables[dsPrivGroup.SecurityGroup.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsPrivGroup.SecurityGroup.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsPrivGroup.SecurityGroup.DescriptionColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsPrivGroup.SecurityGroup.IsActiveColumn.ColumnName])},
                            { "chkAdmin", MDVUtility.ToStr(dr[dsPrivGroup.SecurityGroup.IsAdminColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            priGroup = js.Serialize(keyValues);
                        }
                        var response = new
                        {
                            status = true,
                            Facility_JSON = MDVUtility.JSON_DataTable(dsPrivGroup.Tables[dsProfile.Facility.TableName]),
                            Provider_JSON = MDVUtility.JSON_DataTable(dsPrivGroup.Tables[dsProfile.Provider.TableName]),
                            Resources_JSON = MDVUtility.JSON_DataTable(dsPrivGroup.Tables[dsProfile.Resources.TableName]),
                            PrivilegeGroup_JSON = priGroup
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
                    //return "";
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
        /// Updates the privilege group.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PrivilegeGroupId">The privilege group identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdatePrivilegeGroup(string fieldsJSON, Int64 PrivilegeGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSPrivilegeGroup dsPrivGroup = new DSPrivilegeGroup();
                    //DSPrivilegeGroup.SecurityGroupRow dr = dsPrivGroup.SecurityGroup.NewSecurityGroupRow();
                    BLObject<DSPrivilegeGroup> objLoad = BLLAdminSecurityObj.LoadPrivilegeGroup(PrivilegeGroupId, null, null, null);
                    dsPrivGroup = objLoad.Data;

                    foreach (DSPrivilegeGroup.SecurityGroupRow dr in dsPrivGroup.Tables[dsPrivGroup.SecurityGroup.TableName].Rows)
                    {
                        //dr.SecGroupId = PrivilegeGroupId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"];
                        dr.Name = "";
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        dr.IsDeleted = false;
                        dr.IsAdmin = MDVUtility.ToStr(SearchedfieldsJSON["chkAdmin"]) == "True" ? true : false;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Insertion
                    //dsPrivGroup.SecurityGroup.AddSecurityGroupRow(dr);
                    //dsPrivGroup.SecurityGroup.AcceptChanges();

                    if (dsPrivGroup.Tables[dsPrivGroup.SecurityGroup.TableName].Rows.Count > 0)
                    {
                        //dsPrivGroup.SecurityGroup.Rows[0].SetModified();
                        BLObject<DSPrivilegeGroup> obj = BLLAdminSecurityObj.UpdatePrivilegeGroup(ref dsPrivGroup);

                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                PrivilegeGroupId = dsPrivGroup.Tables[dsPrivGroup.SecurityGroup.TableName].Rows[0][dsPrivGroup.SecurityGroup.SecGroupIdColumn.ColumnName]
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
        /// Deletes the privilege group.
        /// </summary>
        /// <param name="PrivilegeGroupId">The privilege group identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeletePrivilegeGroup(Int64 PrivilegeGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PrivilegeGroupId)))
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
                        BLObject<string> obj = BLLAdminSecurityObj.DeletePrivilegeGroup(MDVUtility.ToStr(PrivilegeGroupId));
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
        /// Updates the privilege group is active.
        /// </summary>
        /// <param name="PrivilegeGroupId">The privilege group identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdatePrivilegeGroupIsActive(Int64 PrivilegeGroupId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSPrivilegeGroup dsPrivGroup = null;
                    BLObject<DSPrivilegeGroup> obj = BLLAdminSecurityObj.LoadPrivilegeGroup(PrivilegeGroupId, null, null, null);
                    dsPrivGroup = obj.Data;
                    if (dsPrivGroup.Tables[dsPrivGroup.SecurityGroup.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPrivGroup.Tables[dsPrivGroup.SecurityGroup.TableName].Rows[0];
                        dr[dsPrivGroup.SecurityGroup.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPrivilegeGroup> objPrivGroup = BLLAdminSecurityObj.UpdatePrivilegeGroup(ref dsPrivGroup);
                        string successMsg;
                        if (objPrivGroup.Data != null)
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
                                Message = objPrivGroup.Message
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

        #region Privilege Group Deatil (Facility, Practice, Entity, Provider, Resources)
        /// <summary>
        /// Saves the privilege group provider.
        /// </summary>
        /// <param name="PrivilegeGroupId">The privilege group identifier.</param>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SavePrivilegeGroupProvider(Int64 PrivilegeGroupId, Int64 ProviderId)
        {
            try
            {
                DSPrivilegeGroup dsPrivGroup = new DSPrivilegeGroup();
                DSPrivilegeGroup.SecurityGroupProviderRow dr = dsPrivGroup.SecurityGroupProvider.NewSecurityGroupProviderRow();

                dr.SecGroupId = PrivilegeGroupId;
                dr.ProviderId = ProviderId;
                dr.Name = "";
                dr.ShortName = "";
                dr.Description = "";
                dr.IsActive = true;
                dr.IsDeleted = false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsPrivGroup.SecurityGroupProvider.AddSecurityGroupProviderRow(dr);
                BLObject<DSPrivilegeGroup> obj =BLLAdminSecurityObj.SavePrivilegeGroupProvider(ref dsPrivGroup);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        SecurityGroupProviderId = dsPrivGroup.Tables[dsPrivGroup.SecurityGroupProvider.TableName].Rows[0][dsPrivGroup.SecurityGroupProvider.SecGroupProviderIdColumn.ColumnName]
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

        /// <summary>
        /// Deletes the privilege group provider.
        /// </summary>
        /// <param name="PrivGroupProviderId">The priv group provider identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeletePrivilegeGroupProvider(string PrivGroupProviderId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(PrivGroupProviderId))
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
                        BLObject<string> obj = BLLAdminSecurityObj.DeletePrivilegeGroupProvider(PrivGroupProviderId);
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

        private string SavePrivilegeGroupProviderCheckAll(Int64 PrivilegeGroupId, string ProviderJson)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var ProviderJSON = ser.Deserialize<dynamic>(ProviderJson);

                    DSPrivilegeGroup dsPrivGroup = new DSPrivilegeGroup();

                    foreach (string item in ProviderJSON)
                    {
                        DSPrivilegeGroup.SecurityGroupProviderRow dr = dsPrivGroup.SecurityGroupProvider.NewSecurityGroupProviderRow();

                        dr.SecGroupId = PrivilegeGroupId;
                        dr.ProviderId = MDVUtility.ToInt64(item);
                        dr.Name = "";
                        dr.ShortName = "";
                        dr.Description = "";
                        dr.IsActive = true;
                        dr.IsDeleted = false;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        dsPrivGroup.SecurityGroupProvider.AddSecurityGroupProviderRow(dr);
                    }

                    #region Database Insertion
                    BLObject<DSPrivilegeGroup> obj = BLLAdminSecurityObj.SavePrivilegeGroupProvider(ref dsPrivGroup);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            Provider_JSON = MDVUtility.JSON_DataTable(dsPrivGroup.Tables[dsPrivGroup.SecurityGroupProvider.TableName]),
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

        private string DeletePrivilegeGroupProviderCheckAll(string ProviderJson)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var ProviderJSON = ser.Deserialize<dynamic>(ProviderJson);

                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProviderJSON)))
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
                    foreach (string item in ProviderJSON)
                    {
                        BLObject<string> obj =BLLAdminSecurityObj.DeletePrivilegeGroupProvider(item);
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Saves the privilege group resource.
        /// </summary>
        /// <param name="PrivilegeGroupId">The privilege group identifier.</param>
        /// <param name="ResourceId">The resource identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SavePrivilegeGroupResource(Int64 PrivilegeGroupId, Int64 ResourceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSPrivilegeGroup dsPrivGroup = new DSPrivilegeGroup();
                    DSPrivilegeGroup.SecurityGroupResourceRow dr = dsPrivGroup.SecurityGroupResource.NewSecurityGroupResourceRow();

                    dr.SecGroupId = PrivilegeGroupId;
                    dr.ResourceId = ResourceId;
                    dr.Name = "";
                    dr.ShortName = "";
                    dr.Description = "";
                    dr.IsActive = true;
                    dr.IsDeleted = false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsPrivGroup.SecurityGroupResource.AddSecurityGroupResourceRow(dr);
                    BLObject<DSPrivilegeGroup> obj = BLLAdminSecurityObj.SavePrivilegeGroupResource(ref dsPrivGroup);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            SecurityGroupResourceId = dsPrivGroup.Tables[dsPrivGroup.SecurityGroupResource.TableName].Rows[0][dsPrivGroup.SecurityGroupResource.SecGroupResourceIdColumn.ColumnName]
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
        private string SavePrivilegeGroupResourceCheckAll(Int64 PrivilegeGroupId, string ResourceJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var ResourceJSon = ser.Deserialize<dynamic>(ResourceJSON);

                    DSPrivilegeGroup dsPrivGroup = new DSPrivilegeGroup();

                    foreach (string item in ResourceJSon)
                    {
                        DSPrivilegeGroup.SecurityGroupResourceRow dr = dsPrivGroup.SecurityGroupResource.NewSecurityGroupResourceRow();

                        dr.SecGroupId = PrivilegeGroupId;
                        dr.ResourceId = MDVUtility.ToInt64(item);
                        dr.Name = "";
                        dr.ShortName = "";
                        dr.Description = "";
                        dr.IsActive = true;
                        dr.IsDeleted = false;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        dsPrivGroup.SecurityGroupResource.AddSecurityGroupResourceRow(dr);
                    }
                    #region Database Insertion

                    BLObject<DSPrivilegeGroup> obj = BLLAdminSecurityObj.SavePrivilegeGroupResource(ref dsPrivGroup);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            Resource_JSON = MDVUtility.JSON_DataTable(dsPrivGroup.Tables[dsPrivGroup.SecurityGroupResource.TableName])
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
        /// Deletes the privilege group resource.
        /// </summary>
        /// <param name="PrivGroupResourceId">The priv group resource identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeletePrivilegeGroupResource(string PrivGroupResourceId)
        {
            try
            {
                if (string.IsNullOrEmpty(PrivGroupResourceId))
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
                    BLObject<string> obj =BLLAdminSecurityObj.DeletePrivilegeGroupResource(PrivGroupResourceId);

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
        private string DeletePrivilegeGroupResourceCheckAll(string ResourceJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var ResourceJSon = ser.Deserialize<dynamic>(ResourceJSON);

                if (string.IsNullOrEmpty(MDVUtility.ToStr(ResourceJSON)))
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
                    foreach (string item in ResourceJSon)
                    {
                        BLObject<string> obj =BLLAdminSecurityObj.DeletePrivilegeGroupResource(item);
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Saves the privilege group facility.
        /// </summary>
        /// <param name="PrivilegeGroupId">The privilege group identifier.</param>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SavePrivilegeGroupFacility(Int64 PrivilegeGroupId, Int64 FacilityId, Int64 EntityId, Int64 PracticeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSPrivilegeGroup dsPrivGroup = new DSPrivilegeGroup();
                    DSPrivilegeGroup.SecurityGroupFacilityRow dr = dsPrivGroup.SecurityGroupFacility.NewSecurityGroupFacilityRow();

                    dr.SecGroupId = PrivilegeGroupId;
                    dr.FacilityId = FacilityId;
                    dr.EntityId = EntityId;
                    dr.PracticeId = PracticeId;
                    dr.Name = "";
                    dr.ShortName = "";
                    dr.Description = "";
                    dr.IsActive = true;
                    dr.IsDeleted = false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsPrivGroup.SecurityGroupFacility.AddSecurityGroupFacilityRow(dr);
                    BLObject<DSPrivilegeGroup> obj = BLLAdminSecurityObj.SavePrivilegeGroupFacility(ref dsPrivGroup);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            SecurityGroupFacilityId = dsPrivGroup.Tables[dsPrivGroup.SecurityGroupFacility.TableName].Rows[0][dsPrivGroup.SecurityGroupFacility.SecGroupFacilityIdColumn.ColumnName]
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
        /// Deletes the privilege group facility.
        /// </summary>
        /// <param name="PrivGroupFacilityId">The priv group facility identifier.</param>
        /// <param name="PrivGroupId">The priv group identifier.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeletePrivilegeGroupFacility(string PrivGroupFacilityId, string PrivGroupId, string EntityId, string PracticeId)
        {
            try
            {
                if (string.IsNullOrEmpty(PrivGroupFacilityId))
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
                    BLObject<string> obj =BLLAdminSecurityObj.DeletePrivilegeGroupFacility(PrivGroupFacilityId, PrivGroupId, EntityId, PracticeId);
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

        private string SavePrivilegeGroupFacilityCheckAll(Int64 PrivilegeGroupId, string FacilityJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Security Entity Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    //string FacilityId = null;
                    //string PracticeId = null;
                    //string EntityId = null;
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var FacilityJson = ser.Deserialize<dynamic>(FacilityJSON);


                    Dictionary<string, object> Facility = new Dictionary<string, object>();
                    DSPrivilegeGroup dsPrivGroup = new DSPrivilegeGroup();

                    foreach (dynamic Ids in FacilityJson)
                    {

                        string facilityId = Ids["FacilityId"];
                        string PracticeId = Ids["PracticeId"];
                        string EntityId = Ids["EntityId"];

                        DSPrivilegeGroup.SecurityGroupFacilityRow dr = dsPrivGroup.SecurityGroupFacility.NewSecurityGroupFacilityRow();

                        dr.SecGroupId = PrivilegeGroupId;
                        dr.FacilityId = MDVUtility.ToInt64(facilityId);
                        dr.EntityId = MDVUtility.ToInt64(EntityId);
                        dr.PracticeId = MDVUtility.ToInt64(PracticeId);
                        dr.Name = "";
                        dr.ShortName = "";
                        dr.Description = "";
                        dr.IsActive = true;
                        dr.IsDeleted = false;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        dsPrivGroup.SecurityGroupFacility.AddSecurityGroupFacilityRow(dr);

                    }

                    #region Database Insertion
                    //dsPrivGroup.SecurityGroupFacility.AddSecurityGroupFacilityRow(dr);
                    BLObject<DSPrivilegeGroup> obj = BLLAdminSecurityObj.SavePrivilegeGroupFacility(ref dsPrivGroup);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            Facility_JSON = MDVUtility.JSON_DataTable(dsPrivGroup.Tables[dsPrivGroup.SecurityGroupFacility.TableName]),
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
        private string DeletePrivilegeGroupFacilityCheckAll(string PrivGroupId, string FacilityJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var FacilityJSon = ser.Deserialize<dynamic>(FacilityJSON);

                if (string.IsNullOrEmpty(MDVUtility.ToStr(FacilityJSon)))
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
                    foreach (dynamic Ids in FacilityJSon)
                    {

                        string PrivGroupFacilityId = Ids["FacilityId"];
                        string PracticeId = Ids["PracticeId"];
                        string EntityId = Ids["EntityId"];


                        BLObject<string> obj =BLLAdminSecurityObj.DeletePrivilegeGroupFacility(PrivGroupFacilityId, PrivGroupId, EntityId, PracticeId);
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Privilege Group Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                #region Security Role Action Commands
                case "SAVE_PRIVILEGE_GROUP":
                    {
                        string fieldsJSON = context.Request["PrivilegeGroupData"];
                        string strJSONData = SavePrivilegeGroup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PRIVILEGE_GROUP":
                    {
                        string PrivilegeGroupId = context.Request["PrivilegeGroupID"];
                        string strJSONData = FillPrivilegeGroup(MDVUtility.ToInt64(PrivilegeGroupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PRIVILEGE_GROUP":
                    {
                        string PrivilegeGroupId = context.Request["PrivilegeGroupID"];
                        string strJSONData = DeletePrivilegeGroup(MDVUtility.ToInt64(PrivilegeGroupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PRIVILEGE_GROUP":
                    {
                        string fieldsJSON = context.Request["PrivilegeGroupData"];
                        Int64 PrivilegeGroupID = MDVUtility.ToInt64(context.Request["PrivilegeGroupID"]);
                        string strJSONData = UpdatePrivilegeGroup(fieldsJSON, PrivilegeGroupID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PRIVILEGE_GROUP_PROVIDER":
                    {
                        string PrivilegeGroupId = context.Request["PrivilegeGroupID"];
                        string ProviderId = context.Request["ProviderID"];
                        string strJSONData = SavePrivilegeGroupProvider(MDVUtility.ToInt64(PrivilegeGroupId), MDVUtility.ToInt64(ProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PRIVILEGE_GROUP_PROVIDER_CHECK_ALL":
                    {
                        string PrivilegeGroupId = context.Request["PrivilegeGroupID"];
                        string ProviderJson = context.Request["ProviderJSon"];
                        string strJSONData = SavePrivilegeGroupProviderCheckAll(MDVUtility.ToInt64(PrivilegeGroupId), ProviderJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PRIVILEGE_GROUP_PROVIDER":
                    {
                        string PrivGroupProviderId = context.Request["PrivilegeGroupProviderID"];
                        string strJSONData = DeletePrivilegeGroupProvider(PrivGroupProviderId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PRIVILEGE_GROUP_PROVIDER_CHECK_ALL":
                    {
                        string PrivGroupProviderJson = context.Request["PrivilegeGroupProviderJSon"];
                        string strJSONData = DeletePrivilegeGroupProviderCheckAll(PrivGroupProviderJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PRIVILEGE_GROUP_RESOURCE":
                    {
                        string PrivilegeGroupId = context.Request["PrivilegeGroupID"];
                        string ResourceId = context.Request["ResourceID"];
                        string strJSONData = SavePrivilegeGroupResource(MDVUtility.ToInt64(PrivilegeGroupId), MDVUtility.ToInt64(ResourceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PRIVILEGE_GROUP_RESOURCE_CHECK_ALL":
                    {
                        string PrivilegeGroupId = context.Request["PrivilegeGroupID"];
                        string ResourceJSON = context.Request["ResourceJSON"];
                        string strJSONData = SavePrivilegeGroupResourceCheckAll(MDVUtility.ToInt64(PrivilegeGroupId), ResourceJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PRIVILEGE_GROUP_RESOURCE":
                    {
                        string PrivGroupResourceID = context.Request["PrivilegeGroupResourceID"];
                        string strJSONData = DeletePrivilegeGroupResource(PrivGroupResourceID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PRIVILEGE_GROUP_RESOURCE_CHECK_ALL":
                    {
                        string ResourceJSON = context.Request["ResourceJSON"];
                        string strJSONData = DeletePrivilegeGroupResourceCheckAll(ResourceJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PRIVILEGE_GROUP_FACILITY":
                    {
                        string PrivilegeGroupId = context.Request["PrivilegeGroupID"];
                        string FacilityId = context.Request["FacilityID"];
                        string EntityId = context.Request["EntityID"];
                        string PracticeId = context.Request["PracticeID"];
                        string strJSONData = SavePrivilegeGroupFacility(MDVUtility.ToInt64(PrivilegeGroupId), MDVUtility.ToInt64(FacilityId), MDVUtility.ToInt64(EntityId), MDVUtility.ToInt64(PracticeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PRIVILEGE_GROUP_FACILITY_CHECK_ALL":
                    {
                        string PrivilegeGroupId = context.Request["PrivilegeGroupID"];
                        string FacilityJSON = context.Request["FacilityJSon"];
                        string strJSONData = SavePrivilegeGroupFacilityCheckAll(MDVUtility.ToInt64(PrivilegeGroupId), FacilityJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PRIVILEGE_GROUP_FACILITY":
                    {
                        string PrivilegeGroupId = context.Request["PrivilegeGroupID"];
                        string PrivilegeGroupFacilityId = context.Request["PrivilegeGroupFacilityID"];
                        string EntityId = context.Request["EntityID"];
                        string PracticeId = context.Request["PracticeID"];
                        string strJSONData = DeletePrivilegeGroupFacility(PrivilegeGroupFacilityId, PrivilegeGroupId, EntityId, PracticeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PRIVILEGE_GROUP_FACILITY_CHECK_ALL":
                    {
                        string PrivilegeGroupId = context.Request["PrivilegeGroupID"];
                        string FacilityJSON = context.Request["FacilityJSon"];
                        string strJSONData = DeletePrivilegeGroupFacilityCheckAll(PrivilegeGroupId, FacilityJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PRIVILEGE_GROUP_ACTIVE_INACTIVE":
                    {
                        Int64 PrivilegeGroupID = MDVUtility.ToInt64(context.Request["PrivilegeGroupID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdatePrivilegeGroupIsActive(PrivilegeGroupID, IsActive);

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