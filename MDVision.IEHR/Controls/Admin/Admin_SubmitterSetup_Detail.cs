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
    public class Admin_SubmitterSetup_Detail
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_SubmitterSetup_Detail()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_SubmitterSetup_Detail _obj = null;
        public static Admin_SubmitterSetup_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_SubmitterSetup_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the submitter setup.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveSubmitterSetup(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Submitter Setup", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    DSEDI.SubmitterSetupRow dr = dsEDI.SubmitterSetup.NewSubmitterSetupRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    dr.SS1000ANM103 = SearchedfieldsJSON["txtOrganizationLastName"];
                    dr.AA07 = SearchedfieldsJSON["txtSubmitterAddress1"];
                    dr.AA08 = SearchedfieldsJSON["txtSubmitterAddress2"];
                    dr.AA09 = SearchedfieldsJSON["txtCity"];
                    dr.AA010 = SearchedfieldsJSON["txtState"];
                    dr.AA011 = SearchedfieldsJSON["txtZipcode"];
                    dr.AA012 = SearchedfieldsJSON["txtRegion"];
                    dr.SS1000APer02 = SearchedfieldsJSON["txtContactName"];
                    dr.SS1000APer04 = MDVUtility.GetSimpleNumber(SearchedfieldsJSON["txtContactTelephone"]);
                    dr.SS1000ANM101 = SearchedfieldsJSON["txtEntityIdentifier"];
                    dr.SS1000ANM102 = SearchedfieldsJSON["txtEntityTypeQualifier"];
                    dr.SS1000ANM104 = SearchedfieldsJSON["txtSubmitterFirstName"];
                    dr.SS1000ANM105 = SearchedfieldsJSON["txtSubmitterMiddleName"];
                    dr.SS1000ANM109 = SearchedfieldsJSON["txtIdentificationCodeQualifier"];
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.isActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsEDI.SubmitterSetup.AddSubmitterSetupRow(dr);
                    BLObject<DSEDI> objSubmitterSetup = BLLAdminEDIObj.InsertSubmitterSetup(dsEDI);
                    dsEDI = objSubmitterSetup.Data;
                    if (objSubmitterSetup.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            SubmitterSetupId = dsEDI.Tables[dsEDI.SubmitterSetup.TableName].Rows[0][dsEDI.SubmitterSetup.SubmitterSetupIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objSubmitterSetup.Message
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
        /// Updates the submitter setup.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="SubmitterSetupId">The submitter setup identifier.</param>
        /// <returns></returns>
        private string UpdateSubmitterSetup(string fieldsJSON, Int64 SubmitterSetupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Submitter Setup", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    //DSEDI.SubmitterSetupRow dr = dsEDI.SubmitterSetup.NewSubmitterSetupRow();
                    BLObject<DSEDI> objLoad = BLLAdminEDIObj.LoadSubmitterSetup(SubmitterSetupId, null, null, null, null);
                    dsEDI = objLoad.Data;
                    foreach (DSEDI.SubmitterSetupRow dr in dsEDI.Tables[dsEDI.SubmitterSetup.TableName].Rows)
                    {
                        //dr.SubmitterSetupId = SubmitterSetupId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        dr.SS1000ANM103 = SearchedfieldsJSON["txtOrganizationLastName"];
                        dr.AA07 = SearchedfieldsJSON["txtSubmitterAddress1"];
                        dr.AA08 = SearchedfieldsJSON["txtSubmitterAddress2"];
                        dr.AA09 = SearchedfieldsJSON["txtCity"];
                        dr.AA010 = SearchedfieldsJSON["txtState"];
                        dr.AA011 = SearchedfieldsJSON["txtZipcode"];
                        dr.AA012 = SearchedfieldsJSON["txtRegion"];
                        dr.SS1000APer02 = SearchedfieldsJSON["txtContactName"];
                        dr.SS1000APer04 = MDVUtility.GetSimpleNumber(SearchedfieldsJSON["txtContactTelephone"]);
                        dr.SS1000ANM101 = SearchedfieldsJSON["txtEntityIdentifier"];
                        dr.SS1000ANM102 = SearchedfieldsJSON["txtEntityTypeQualifier"];
                        dr.SS1000ANM104 = SearchedfieldsJSON["txtSubmitterFirstName"];
                        dr.SS1000ANM105 = SearchedfieldsJSON["txtSubmitterMiddleName"];
                        dr.SS1000ANM109 = SearchedfieldsJSON["txtIdentificationCodeQualifier"];
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.isActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsEDI.SubmitterSetup.AddSubmitterSetupRow(dr);
                    //dsEDI.SubmitterSetup.AcceptChanges();

                    if (dsEDI.Tables[dsEDI.SubmitterSetup.TableName].Rows.Count > 0)
                    {
                        //dsEDI.SubmitterSetup.Rows[0].SetModified();
                        BLObject<DSEDI> objSubmitterSetup = BLLAdminEDIObj.UpdateSubmitterSetup(dsEDI);
                        dsEDI = objSubmitterSetup.Data;
                        if (objSubmitterSetup.Data != null)
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
                                Message = objSubmitterSetup.Message
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
        /// Fills the submitter setup.
        /// </summary>
        /// <param name="SubmitterSetupId">The submitter setup identifier.</param>
        /// <returns></returns>
        private string FillSubmitterSetup(Int64 SubmitterSetupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Submitter Setup", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(SubmitterSetupId)))
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
                        DSEDI dsEDI = null;
                        BLObject<DSEDI> objSubmitterSetup = BLLAdminEDIObj.LoadSubmitterSetup(SubmitterSetupId, null, null, null, null);
                        if (objSubmitterSetup.Data != null)
                        {
                            dsEDI = objSubmitterSetup.Data;
                            if (dsEDI.Tables[dsEDI.SubmitterSetup.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsEDI.Tables[dsEDI.SubmitterSetup.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.ShortNameColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.EntityIdColumn.ColumnName])},
                            { "txtOrganizationLastName", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.SS1000ANM103Column.ColumnName])},
                            { "txtSubmitterAddress1", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.AA07Column.ColumnName])},
                            { "txtSubmitterAddress2", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.AA08Column.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.AA09Column.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.AA010Column.ColumnName])},
                            { "txtZipcode", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.AA011Column.ColumnName])},
                            { "txtRegion", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.AA012Column.ColumnName])},
                            { "txtContactName", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.SS1000APer02Column.ColumnName])},
                            { "txtContactTelephone", MDVUtility.GetFormatedNumber(MDVUtility.ToLong(dr[dsEDI.SubmitterSetup.SS1000APer04Column.ColumnName]),MDVUtility.NumbersType.TelephoneNumber)},
                            { "txtEntityIdentifier", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.SS1000ANM101Column.ColumnName])},
                            { "txtEntityTypeQualifier", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.SS1000ANM102Column.ColumnName])},
                            { "txtSubmitterFirstName", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.SS1000ANM104Column.ColumnName])},
                            { "txtSubmitterMiddleName", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.SS1000ANM105Column.ColumnName])},
                            { "txtIdentificationCodeQualifier", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.SS1000ANM109Column.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsEDI.SubmitterSetup.isActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    SubmitterSetupFill_JSON = js.Serialize(keyValues)
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
                                Message = objSubmitterSetup.Message
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
        /// Deletes the submitter setup.
        /// </summary>
        /// <param name="SubmitterSetupId">The submitter setup identifier.</param>
        /// <returns></returns>
        private string DeleteSubmitterSetup(Int64 SubmitterSetupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Submitter Setup", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(SubmitterSetupId)))
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
                        BLObject<string> objSubmitterSetup = BLLAdminEDIObj.DeleteSubmitterSetup(MDVUtility.ToStr(SubmitterSetupId));

                        if (objSubmitterSetup.Data == "")
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
                                Message = objSubmitterSetup.Data
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
        /// Updates the submitter setup is active.
        /// </summary>
        /// <param name="SubmitterSetupId">The submitter setup identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateSubmitterSetupIsActive(Int64 SubmitterSetupId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Submitter Setup", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSEDI dsEDI = null;
                    BLObject<DSEDI> obj = BLLAdminEDIObj.LoadSubmitterSetup(SubmitterSetupId, null, null, null, null);
                    dsEDI = obj.Data;
                    if (dsEDI.Tables[dsEDI.SubmitterSetup.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsEDI.Tables[dsEDI.SubmitterSetup.TableName].Rows[0];
                        dr[dsEDI.SubmitterSetup.isActiveColumn.ColumnName] = IsActive;

                        BLObject<DSEDI> objSubmitterSetup = BLLAdminEDIObj.UpdateSubmitterSetup(dsEDI);
                        string successMsg;
                        if (objSubmitterSetup.Data != null)
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
                                Message = objSubmitterSetup.Message
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
        /// Handle the Submitter Setup Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_SUBMITTER_SETUP":
                    {
                        string fieldsJSON = context.Request["SubmitterSetupData"];
                        string strJSONData = SaveSubmitterSetup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_SUBMITTER_SETUP":
                    {
                        string strSubmitterSetupId = context.Request["SubmitterSetupID"];
                        string strJSONData = FillSubmitterSetup(MDVUtility.ToInt64(strSubmitterSetupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_SUBMITTER_SETUP":
                    {
                        string strSubmitterSetupId = context.Request["SubmitterSetupID"];
                        string strJSONData = DeleteSubmitterSetup(MDVUtility.ToInt64(strSubmitterSetupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SUBMITTER_SETUP":
                    {
                        string fieldsJSON = context.Request["SubmitterSetupData"];
                        Int64 SubmitterSetupID = MDVUtility.ToInt64(context.Request["SubmitterSetupID"]);
                        string strJSONData = UpdateSubmitterSetup(fieldsJSON, SubmitterSetupID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SUBMITTER_SETUP_ACTIVE_INACTIVE":
                    {
                        Int64 SubmitterSetupID = MDVUtility.ToInt64(context.Request["SubmitterSetupID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateSubmitterSetupIsActive(SubmitterSetupID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}