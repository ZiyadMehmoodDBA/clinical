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
    public class Admin_Specialty_Detail
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_Specialty_Detail()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_Specialty_Detail _obj = null;
        public static Admin_Specialty_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_Specialty_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the specialty.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveSpecialty(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Specialty", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsProfile = new DSProfile();
                    DSProfile.SpecialtyRow dr = dsProfile.Specialty.NewSpecialtyRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsProfile.Specialty.AddSpecialtyRow(dr);
                    BLObject<DSProfile> obj = BLLAdminProfileObj.InsertSpecialty(ref dsProfile);
                    dsProfile = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            SpecialtyId = dsProfile.Tables[dsProfile.Specialty.TableName].Rows[0][dsProfile.Specialty.SpecialtyIdColumn.ColumnName]
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
        /// Updates the specialty.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="SpecialtyId">The specialty identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateSpecialty(string fieldsJSON, Int64 SpecialtyId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Specialty", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsProfile = new DSProfile();
                    //DSProfile.SpecialtyRow dr = dsProfile.Specialty.NewSpecialtyRow();
                    BLObject<DSProfile> objLoad = BLLAdminProfileObj.LoadSpecialty(SpecialtyId, null, null, null, null);
                    dsProfile = objLoad.Data;
                    foreach (DSProfile.SpecialtyRow dr in dsProfile.Tables[dsProfile.Specialty.TableName].Rows)
                    {
                        //dr.SpecialtyId = SpecialtyId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsProfile.Specialty.AddSpecialtyRow(dr);
                    //dsProfile.Specialty.AcceptChanges();

                    if (dsProfile.Tables[dsProfile.Specialty.TableName].Rows.Count > 0)
                    {
                        //dsProfile.Specialty.Rows[0].SetModified();
                        BLObject<DSProfile> obj = BLLAdminProfileObj.UpdateSpecialty(ref dsProfile);
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
        /// Fills the specialty.
        /// </summary>
        /// <param name="ProviderId">The specialty identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillSpecialty(Int64 SpecialtyId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Specialty", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(SpecialtyId)))
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
                        DSProfile dsProfile = null;
                        //dsProfile = DALSpecialty.Instance.LoadSpecialty(SpecialtyId, null, null);
                        BLObject<DSProfile> obj = BLLAdminProfileObj.LoadSpecialty(SpecialtyId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsProfile = obj.Data;
                            if (dsProfile.Tables[dsProfile.Specialty.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsProfile.Tables[dsProfile.Specialty.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsProfile.Specialty.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsProfile.Specialty.DescriptionColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsProfile.Specialty.EntityIdColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsProfile.Specialty.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    SpecialtyFill_JSON = js.Serialize(keyValues)
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

        /// <summary>
        /// Deletes the specialty against provider Id.
        /// </summary>
        /// <param name="ProviderId">The specialty identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteSpecialty(Int64 SpecialtyId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Specialty", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(SpecialtyId)))
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
                        BLObject<string> obj = BLLAdminProfileObj.DeleteSpecialty(MDVUtility.ToStr(SpecialtyId));
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
        /// Updates the specialty is active.
        /// </summary>
        /// <param name="SpecialtyId">The specialty identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateSpecialtyIsActive(Int64 SpecialtyId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Specialty", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSProfile dsProfile = null;
                    BLObject<DSProfile> obj = BLLAdminProfileObj.LoadSpecialty(SpecialtyId, null, null, null, null);
                    dsProfile = obj.Data;
                    if (dsProfile.Tables[dsProfile.Specialty.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsProfile.Tables[dsProfile.Specialty.TableName].Rows[0];
                        dr[dsProfile.Specialty.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSProfile> objSpecialty = BLLAdminProfileObj.UpdateSpecialty(ref dsProfile);
                        string successMsg;
                        if (objSpecialty.Data != null)
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
                                Message = objSpecialty.Message
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
        /// Handle the Specialty Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_SPECIALTY":
                    {
                        string fieldsJSON = context.Request["SpecialtyData"];
                        string strJSONData = SaveSpecialty(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_SPECIALTY":
                    {
                        string strSpecialtyId = context.Request["SpecialtyID"];
                        string strJSONData = FillSpecialty(MDVUtility.ToInt64(strSpecialtyId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_SPECIALTY":
                    {
                        string strSpecialtyId = context.Request["SpecialtyID"];
                        string strJSONData = DeleteSpecialty(MDVUtility.ToInt64(strSpecialtyId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SPECIALTY":
                    {
                        string fieldsJSON = context.Request["SpecialtyData"];
                        Int64 SpecialtyID = MDVUtility.ToInt64(context.Request["SpecialtyID"]);
                        string strJSONData = UpdateSpecialty(fieldsJSON, SpecialtyID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SPECIALTY_ACTIVE_INACTIVE":
                    {
                        Int64 SpecialtyID = MDVUtility.ToInt64(context.Request["SpecialtyID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateSpecialtyIsActive(SpecialtyID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}