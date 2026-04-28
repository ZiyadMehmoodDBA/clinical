using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_ParticipentProviderDetail
    {

        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_ParticipentProviderDetail()
        {
            this.BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_ParticipentProviderDetail _obj = null;
        public static Admin_ParticipentProviderDetail Instance()
        {
            if (_obj == null)
                _obj = new Admin_ParticipentProviderDetail();
            return _obj;
        }
        #endregion
        #region Private Functions
        /// <summary>
        /// Saves the Participant Provider.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveParticipantProvider(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSProfile dsProfile = new DSProfile();
                DSProfile.ProviderParticipantRow dr = dsProfile.ProviderParticipant.NewProviderParticipantRow();

                dr.ProviderId = Convert.ToInt32(SearchedfieldsJSON["hfProvider"]);
                dr.Assignment = SearchedfieldsJSON["txtAssignment"];
                dr.AssingnmentId = SearchedfieldsJSON["txtAssingmentId"];
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlParticipentStatus"]))
                    dr.ProviderParticipantStatusId = Convert.ToInt32(SearchedfieldsJSON["ddlParticipentStatus"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignmentTypeRTK"]))
                    dr.AssignmentTypeRTK = SearchedfieldsJSON["ddlAssignmentTypeRTK"];
                dr.AssingnmentAdditionalId = SearchedfieldsJSON["txtAssingnmentAdditionalId"];
                dr.StartDate = string.IsNullOrEmpty(SearchedfieldsJSON["dpAssingStartDate"]) ? null : Convert.ToDateTime(SearchedfieldsJSON["dpAssingStartDate"]);
                dr.Comments = SearchedfieldsJSON["txtComments"];
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                #region Database Insertion
                dsProfile.ProviderParticipant.AddProviderParticipantRow(dr);
                BLObject<DSProfile> obj = BLLAdminProfileObj.InsertProviderParticipant(ref dsProfile);
                dsProfile = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        ProviderParticipantId = dsProfile.Tables[dsProfile.ProviderParticipant.TableName].Rows[0][dsProfile.ProviderParticipant.ProviderParticipantIdColumn.ColumnName]
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
        private string UpdateParticipantProvider(string fieldsJSON, Int64 ParticipantProviderId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSProfile dsProfile = new DSProfile();
                //DSProfile.SpecialtyRow dr = dsProfile.Specialty.NewSpecialtyRow();
                BLObject<DSProfile> objLoad = BLLAdminProfileObj.LoadProviderParticipant(ParticipantProviderId, null, null, null);
                dsProfile = objLoad.Data;
                foreach (DSProfile.ProviderParticipantRow dr in dsProfile.Tables[dsProfile.ProviderParticipant.TableName].Rows)
                {
                    //dr.SpecialtyId = SpecialtyId;
                     dr.ProviderName = SearchedfieldsJSON["txtProvider"];
                    dr.ProviderId =Convert.ToInt32( SearchedfieldsJSON["hfProvider"]);
                    dr.Assignment = SearchedfieldsJSON["txtAssignment"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlParticipentStatus"]))
                        dr.ProviderParticipantStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlParticipentStatus"]);
                    dr.AssingnmentId = SearchedfieldsJSON["txtAssingmentId"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignmentTypeRTK"]))
                        dr.AssignmentTypeRTK = SearchedfieldsJSON["ddlAssignmentTypeRTK"];
                    dr.AssingnmentId = SearchedfieldsJSON["txtAssingnmentAdditionalId"];
                    dr.StartDate = string.IsNullOrEmpty( SearchedfieldsJSON["dpAssingStartDate"])?null:Convert.ToDateTime(SearchedfieldsJSON["dpAssingStartDate"]);
                    dr.Comments = SearchedfieldsJSON["txtComments"];
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }

                #region Database Updation
                //dsProfile.Specialty.AddSpecialtyRow(dr);
                //dsProfile.Specialty.AcceptChanges();

                if (dsProfile.Tables[dsProfile.ProviderParticipant.TableName].Rows.Count > 0)
                {
                    BLObject<DSProfile> obj = BLLAdminProfileObj.UpdateParticipantProvider(ref dsProfile);
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Fills the Participant.
        /// </summary>
        /// <param name="ProviderId">The Participant identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillParticipantProvider(Int64 ParticipantProviderId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ParticipantProviderId)))
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
                    BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProviderParticipant(ParticipantProviderId, null, null, null);
                    if (obj.Data != null)
                    {
                        dsProfile = obj.Data;
                        if (dsProfile.Tables[dsProfile.ProviderParticipant.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsProfile.Tables[dsProfile.ProviderParticipant.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtProvider", MDVUtility.ToStr(dr[dsProfile.ProviderParticipant.ProviderNameColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsProfile.ProviderParticipant.ProviderIdColumn.ColumnName])},
                            { "txtAssignment", MDVUtility.ToStr(dr[dsProfile.ProviderParticipant.AssignmentColumn.ColumnName])},
                            { "ddlParticipentStatus", MDVUtility.ToStr(dr[dsProfile.ProviderParticipant.ProviderParticipantStatusIdColumn.ColumnName])},
                            { "txtAssingmentId", MDVUtility.ToStr(dr[dsProfile.ProviderParticipant.AssingnmentIdColumn.ColumnName])},
                            { "ddlAssignmentTypeRTK", MDVUtility.ToStr(dr[dsProfile.ProviderParticipant.AssignmentTypeRTKColumn.ColumnName])},
                            { "txtAssingnmentAdditionalId", MDVUtility.ToStr(dr[dsProfile.ProviderParticipant.AssingnmentAdditionalIdColumn.ColumnName])},
                            { "dpAssingStartDate", MDVUtility.ToStr(dr[dsProfile.ProviderParticipant.StartDateColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsProfile.ProviderParticipant.CommentsColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ParticipantFill_JSON = js.Serialize(keyValues)
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
        private string DeleteParticipantProvider(Int64 ParticipantProviderId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ParticipantProviderId)))
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
                    BLObject<string> obj = BLLAdminProfileObj.DeleteParticipantProvider(MDVUtility.ToStr(ParticipantProviderId));
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

        /// <summary>
        /// Updates the specialty is active.
        /// </summary>
        /// <param name="SpecialtyId">The specialty identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateParticipantProviderIsActive(Int64 ParticipantProviderId, Int64 IsActive)
        {
            try
            {
                DSProfile dsProfile = null;
                BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProviderParticipant(ParticipantProviderId, null, null, null);
                dsProfile = obj.Data;
                if (dsProfile.Tables[dsProfile.ProviderParticipant.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsProfile.Tables[dsProfile.ProviderParticipant.TableName].Rows[0];
                    dr[dsProfile.ProviderParticipant.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSProfile> objSpecialty = BLLAdminProfileObj.UpdateParticipantProvider(ref dsProfile);
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
                case "SAVE_PARTICIPANT_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ParticipantProviderData"];
                        string strJSONData = SaveParticipantProvider(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PARTICIPANT_PROVIDER":
                    {
                        string strParticipantProviderId = context.Request["ParticipantProviderId"];
                        string strJSONData = FillParticipantProvider(MDVUtility.ToInt64(strParticipantProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PARTICIPANT_PROVIDER":
                    {
                        string strParticipantProviderId = context.Request["ParticipantProviderId"];
                        string strJSONData = DeleteParticipantProvider(MDVUtility.ToInt64(strParticipantProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PARTICIPANT_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ParticipantProviderData"];
                        Int64 ParticipantProviderId = MDVUtility.ToInt64(context.Request["ParticipantProviderId"]);
                        string strJSONData = UpdateParticipantProvider(fieldsJSON, ParticipantProviderId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PARTICIPANT_PROVIDER_ACTIVE_INACTIVE":
                    {
                        Int64 SpecialtyID = MDVUtility.ToInt64(context.Request["ParticipantProviderId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateParticipantProviderIsActive(SpecialtyID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion

    }
}