using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using System.Globalization;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_BlockHours_Detail
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_BlockHours_Detail()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_BlockHours_Detail _obj = null;
        public static Admin_BlockHours_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_BlockHours_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Saves the block hours.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        private string SaveBlockHours(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Block Hours", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    DSScheduleSetup.SchBlockHoursRow dr = dsSchedule.SchBlockHours.NewSchBlockHoursRow();

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                    {
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfResource"]))
                    {
                        dr.ResourceId = MDVUtility.ToInt64(SearchedfieldsJSON["hfResource"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                    {
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                    }
                    else
                    {
                        dr.FacilityId = 0;
                    }
                    //dr.BlockReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["hfBlockReason"]);
                    dr.Description = SearchedfieldsJSON["txtBlockHours"];
                    dr.FromDate = MDVUtility.ToDateTime(SearchedfieldsJSON["blockHoursFromDate"]);
                    dr.ToDate = MDVUtility.ToDateTime(SearchedfieldsJSON["blockHoursToDate"]);
                    dr.FromTime = MDVUtility.ToStr(SearchedfieldsJSON["blockHoursFromTime"]);
                    dr.ToTime = MDVUtility.ToStr(SearchedfieldsJSON["blockHoursToTime"]);
                    //  dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.OverLappingAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkOverLappingAllowed"]) == "True" ? true : false;
                    dr.Color = MDVUtility.ToStr(SearchedfieldsJSON["txtColor"]);
                    #region Database Insertion
                    dsSchedule.SchBlockHours.AddSchBlockHoursRow(dr);
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.InsertBlockHours(dsSchedule);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            BlockHoursId = dsSchedule.Tables[dsSchedule.SchBlockHours.TableName].Rows[0][dsSchedule.SchBlockHours.BlockHoursIdColumn.ColumnName]
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
        /// Updates the block hours.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="BlockHoursId">The block hours identifier.</param>
        /// <returns>System.String.</returns>
        private string UpdateBlockHours(string fieldsJSON, Int64 BlockHoursId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Block Hours", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    //DSScheduleSetup.SchBlockHoursRow dr = dsSchedule.SchBlockHours.NewSchBlockHoursRow();
                    BLObject<DSScheduleSetup> objLoad = BLLScheduleObj.LoadBlockHours(BlockHoursId, null, null, null, null, null, null, null);
                    dsSchedule = objLoad.Data;
                    foreach (DSScheduleSetup.SchBlockHoursRow dr in dsSchedule.Tables[dsSchedule.SchBlockHours.TableName].Rows)
                    {
                        //dr.BlockHoursId = BlockHoursId;
                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                        //{
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                        //}
                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfResource"]))
                        //{
                        dr.ResourceId = MDVUtility.ToInt64(SearchedfieldsJSON["hfResource"]);
                        //}
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                        {
                            dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                        }
                        else
                        {
                            dr.FacilityId = 0;
                        }
                        //dr.BlockReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["hfBlockReason"]);
                        dr.Description = SearchedfieldsJSON["txtBlockHours"];
                        dr.FromDate = MDVUtility.ToDateTime(SearchedfieldsJSON["blockHoursFromDate"]);
                        dr.ToDate = MDVUtility.ToDateTime(SearchedfieldsJSON["blockHoursToDate"]);
                        dr.FromTime = MDVUtility.ToStr(SearchedfieldsJSON["blockHoursFromTime"]);
                        dr.ToTime = MDVUtility.ToStr(SearchedfieldsJSON["blockHoursToTime"]);
                        //  dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.OverLappingAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkOverLappingAllowed"]) == "True" ? true : false;
                        dr.Color = MDVUtility.ToStr(SearchedfieldsJSON["txtColor"]);
                    }



                    #region Database Updation
                    //dsSchedule.SchBlockHours.AddSchBlockHoursRow(dr);
                    //dsSchedule.SchBlockHours.AcceptChanges();

                    if (dsSchedule.Tables[dsSchedule.SchBlockHours.TableName].Rows.Count > 0)
                    {
                        //dsSchedule.SchBlockHours.Rows[0].SetModified();
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.UpdateBlockHours(dsSchedule);
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
        /// Fills the block hours.
        /// </summary>
        /// <param name="BlockHoursId">The block hours identifier.</param>
        /// <returns>System.String.</returns>
        private string FillBlockHours(Int64 BlockHoursId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Block Hours", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(BlockHoursId)))
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
                        DSScheduleSetup dsSchedule = null;
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadBlockHours(BlockHoursId, null, null, null, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsSchedule = obj.Data;
                            if (dsSchedule.Tables[dsSchedule.SchBlockHours.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsSchedule.Tables[dsSchedule.SchBlockHours.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "hfProvider", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.SchBlockHours.ProviderIdColumn.ColumnName]))},
                            //{ "hfBlockReason", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.SchBlockHours.BlockReasonIdColumn.ColumnName]))},
                            { "hfResource", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.SchBlockHours.ResourceIdColumn.ColumnName]))},
                            { "hfFacility", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.SchBlockHours.FacilityIdColumn.ColumnName]))},
                            { "txtFacility", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.FacilityNameColumn.ColumnName]) + " - " + (MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.FacilityEntityColumn.ColumnName]))},
                            { "txtProvider", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.ProviderNameColumn.ColumnName]) + " - " + (MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.ProviderEntityColumn.ColumnName]))},
                            { "txtResource", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.ResourceNameColumn.ColumnName]) + " - " + (MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.ResourceEntityColumn.ColumnName]))},
                            { "txtFacility1", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.FacilityNameColumn.ColumnName])},
                            { "txtProvider1", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.ProviderNameColumn.ColumnName])},
                            { "txtResource1", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.ResourceNameColumn.ColumnName])},
                            { "lstBlockReasonType", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.SchReasonColumn.ColumnName])},
                            { "txtBlockHours", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.DescriptionColumn.ColumnName])},
                            { "blockHoursFromDate", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsSchedule.SchBlockHours.FromDateColumn.ColumnName]).ToShortDateString())},
                            { "blockHoursToDate", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsSchedule.SchBlockHours.ToDateColumn.ColumnName]).ToShortDateString())},
                            { "blockHoursFromTime", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.FromTimeColumn.ColumnName])},
                            { "blockHoursToTime", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.ToTimeColumn.ColumnName])},
                            { "txtColor", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.ColorColumn.ColumnName])},
                            { "chkOverLappingAllowed", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.OverLappingAllowedColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsSchedule.SchBlockHours.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    BlockHoursFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the block hours.
        /// </summary>
        /// <param name="BlockHoursId">The block hours identifier.</param>
        /// <returns>System.String.</returns>
        private string DeleteBlockHours(Int64 BlockHoursId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Block Hours", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(BlockHoursId)))
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
                        BLObject<string> obj = BLLScheduleObj.DeleteBlockHours(MDVUtility.ToStr(BlockHoursId));
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
        /// Updates the block hours is active.
        /// </summary>
        /// <param name="BlockHoursId">The block hours identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>System.String.</returns>
        private string UpdateBlockHoursIsActive(Int64 BlockHoursId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Block Hours", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadBlockHours(BlockHoursId, null, null, null, null, null, null, null);
                    dsSchedule = obj.Data;
                    if (dsSchedule.Tables[dsSchedule.SchBlockHours.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsSchedule.Tables[dsSchedule.SchBlockHours.TableName].Rows[0];
                        dr[dsSchedule.SchBlockHours.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSScheduleSetup> objBlockHours = BLLScheduleObj.UpdateBlockHours(dsSchedule);
                        string successMsg;
                        if (objBlockHours.Data != null)
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
                                Message = objBlockHours.Message
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
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string ResonsAutoComplete(string name)
        {
            try
            {
                DSScheduleLookups dsReason = null;
                BLObject<DSScheduleLookups> obj;
                obj = BLLScheduleObj.LookupReasonsAutoComplete("1", name);

                dsReason = obj.Data;
                if (obj.Data != null)
                {
                    if (dsReason.Tables[dsReason.ScheduleReasons.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ResonsCount = dsReason.Tables[dsReason.ScheduleReasons.TableName].Rows.Count,
                            ResonsLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsReason.Tables[dsReason.ScheduleReasons.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ReferringProviderCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
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
                case "SAVE_BLOCK_HOURS":
                    {
                        string fieldsJSON = context.Request["BlockHoursData"];
                        string strJSONData = SaveBlockHours(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_BLOCK_HOURS":
                    {
                        string strBlockHoursId = context.Request["BlockHoursID"];
                        string strJSONData = FillBlockHours(MDVUtility.ToInt64(strBlockHoursId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_BLOCK_HOURS":
                    {
                        string strBlockHoursId = context.Request["BlockHoursID"];
                        string strJSONData = DeleteBlockHours(MDVUtility.ToInt64(strBlockHoursId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BLOCK_HOURS":
                    {
                        string fieldsJSON = context.Request["BlockHoursData"];
                        Int64 BlockHoursID = MDVUtility.ToInt64(context.Request["BlockHoursID"]);
                        string strJSONData = UpdateBlockHours(fieldsJSON, BlockHoursID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BLOCK_HOURS_ACTIVE_INACTIVE":
                    {
                        Int64 BlockHoursID = MDVUtility.ToInt64(context.Request["BlockHoursID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateBlockHoursIsActive(BlockHoursID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_BLOCK_HOURS_AUTOCOMPLETE":
                    {
                        string BlockHours = context.Request["BlockHours"];
                        string strJSONData = ResonsAutoComplete(BlockHours);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}