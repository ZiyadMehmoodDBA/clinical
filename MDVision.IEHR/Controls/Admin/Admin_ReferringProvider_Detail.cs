using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.IEHR.Model.Admin;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_ReferringProvider_Detail
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_ReferringProvider_Detail()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_ReferringProvider_Detail _obj = null;
        public static Admin_ReferringProvider_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_ReferringProvider_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the ReferringProvider.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveReferringProvider(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Referring Provider", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsReferringProvider = new DSProfile();
                    DSProfile.ReferringProviderRow dr = dsReferringProvider.ReferringProvider.NewReferringProviderRow();

                    dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                    dr.LastName = SearchedfieldsJSON["txtLastName"];
                    dr.NPI = SearchedfieldsJSON["txtNPI"];
                    dr.Address = SearchedfieldsJSON["txtAddress"];
                    dr.City = SearchedfieldsJSON["txtCity"];
                    dr.State = SearchedfieldsJSON["txtState"];
                    dr.ZipCode = SearchedfieldsJSON["txtZipCode"];
                    dr.ZipCodeExt = SearchedfieldsJSON["txtZipCodeExt"];
                    dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                    dr.EmailAddress = SearchedfieldsJSON["txtEmail"];

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSpecialty"]))
                        dr.SpecialtyId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlSpecialty"]);

                    dr.MI = SearchedfieldsJSON["txtMI"];
                    dr.TaxonomyCode = SearchedfieldsJSON["txtTaxonomyCode"];
                    dr.ProfileType = SearchedfieldsJSON["ddlProfileType"];
                    dr.Statelicence = SearchedfieldsJSON["txtStateLicenceNumber"];
                    dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                    dr.TelephoneExt = SearchedfieldsJSON["txtTelephoneExt"];
                    dr.Fax = SearchedfieldsJSON["txtFax"];
                    dr.Cell = SearchedfieldsJSON["txtCell"];

                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    dr.IsSovereign = SearchedfieldsJSON["chkIsSovereign"];

                    #region Database Insertion
                    dsReferringProvider.ReferringProvider.AddReferringProviderRow(dr);
                    BLObject<DSProfile> obj = BLLAdminProfileObj.InsertReferringProvider(ref dsReferringProvider);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ReferringProviderId = dsReferringProvider.Tables[dsReferringProvider.ReferringProvider.TableName].Rows[0][dsReferringProvider.ReferringProvider.ReferringProviderIdColumn.ColumnName]
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
        /// Updates the ReferringProvider.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ResourceId">The resource identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateReferringProvider(string fieldsJSON, Int64 ReferringProviderId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Referring Provider", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    string EntityId = SearchedfieldsJSON["ddlEntity"];
                    DSProfile dsReferringProvider = new DSProfile();
                    //DSProfile.ReferringProviderRow dr = dsReferringProvider.ReferringProvider.NewReferringProviderRow();
                    BLObject<DSProfile> objLoad = BLLAdminProfileObj.LoadReferringProvider(ReferringProviderId, null, null, null, null, EntityId);

                    if (objLoad.Data != null)
                    {
                        dsReferringProvider = objLoad.Data;
                        DSProfile.ReferringProviderRow dr = (DSProfile.ReferringProviderRow)dsReferringProvider.Tables[dsReferringProvider.ReferringProvider.TableName].Rows[0];

                        //dr.ReferringProviderId = ReferringProviderId;
                        dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                        dr.LastName = SearchedfieldsJSON["txtLastName"];
                        dr.NPI = SearchedfieldsJSON["txtNPI"];
                        dr.Address = SearchedfieldsJSON["txtAddress"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.ZipCode = SearchedfieldsJSON["txtZipCode"];
                        dr.ZipCodeExt = SearchedfieldsJSON["txtZipCodeExt"];
                        dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSpecialty"]))
                            dr.SpecialtyId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlSpecialty"]);
                        else
                            dr.SpecialtyId = 0;

                        dr.MI = SearchedfieldsJSON["txtMI"];
                        dr.TaxonomyCode = SearchedfieldsJSON["txtTaxonomyCode"];
                        dr.ProfileType = SearchedfieldsJSON["ddlProfileType"];
                        dr.Statelicence = SearchedfieldsJSON["txtStateLicenceNumber"];
                        dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                        dr.TelephoneExt = SearchedfieldsJSON["txtTelephoneExt"];
                        dr.Fax = SearchedfieldsJSON["txtFax"];
                        dr.Cell = SearchedfieldsJSON["txtCell"];
                        dr.IsSovereign = SearchedfieldsJSON["chkIsSovereign"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);


                        #region Database Updation
                        //dsReferringProvider.ReferringProvider.AddReferringProviderRow(dr);
                        //dsReferringProvider.ReferringProvider.AcceptChanges();

                        if (dsReferringProvider.Tables[dsReferringProvider.ReferringProvider.TableName].Rows.Count > 0)
                        {
                            //dsReferringProvider.ReferringProvider.Rows[0].SetModified();
                            BLObject<DSProfile> obj = BLLAdminProfileObj.UpdateReferringProvider(ref dsReferringProvider);
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
                            Message = objLoad.Message,
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

        /// <summary>
        /// Fills the ReferringProvider.
        /// </summary>
        /// <param name="ResourceID">The resource identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillReferringProvider(Int64 ReferringProviderID, string EntityId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Referring Provider", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ReferringProviderID)))
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
                        DSProfile dsReferringProvider = null;
                        BLObject<DSProfile> obj = BLLAdminProfileObj.LoadReferringProvider(ReferringProviderID, null, null, null, null, EntityId);
                        if (obj.Data != null)
                        {
                            dsReferringProvider = obj.Data;
                            if (dsReferringProvider.Tables[dsReferringProvider.ReferringProvider.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsReferringProvider.Tables[dsReferringProvider.ReferringProvider.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtFirstName", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.FirstNameColumn.ColumnName])},
                            { "txtLastName", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.LastNameColumn.ColumnName])},
                            { "txtNPI", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.NPIColumn.ColumnName])},
                            { "txtAddress", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.AddressColumn.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.CityColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.StateColumn.ColumnName])},
                            { "txtZipCode", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.ZipCodeColumn.ColumnName])},
                            { "txtZipCodeExt", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.ZipCodeExtColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.PhoneNoColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.EmailAddressColumn.ColumnName])},
                            { "ddlSpecialty", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.SpecialtyIdColumn.ColumnName])},
                            { "txtMI", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.MIColumn.ColumnName])},
                            { "txtTaxonomyCode", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.TaxonomyCodeColumn.ColumnName])},
                            { "ddlProfileType", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.ProfileTypeColumn.ColumnName])},
                            { "txtStateLicenceNumber", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.StatelicenceColumn.ColumnName])},
                            { "txtAddress2", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.Address2Column.ColumnName])},
                            { "txtTelephoneExt", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.TelephoneExtColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.FaxColumn.ColumnName])},
                            { "txtCell", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.CellColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.IsActiveColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.EntityIdColumn.ColumnName])},
                            { "chkIsSovereign", MDVUtility.ToStr(dr[dsReferringProvider.ReferringProvider.IsSovereignColumn.ColumnName])},
                        };


                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    ReferringProviderFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the ReferringProvider.
        /// </summary>
        /// <param name="ResourceID">The ReferringProvider identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteReferringProvider(Int64 ReferringProviderID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Referring Provider", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ReferringProviderID)))
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
                        BLObject<string> obj = BLLAdminProfileObj.DeleteReferringProvider(MDVUtility.ToStr(ReferringProviderID));
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
        /// Updates the ReferringProvider is active.
        /// </summary>
        /// <param name="ResourceId">The resource identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateReferringProviderIsActive(Int64 ReferringProviderId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Referring Provider", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSProfile dsReferringProvider = null;
                    BLObject<DSProfile> obj = BLLAdminProfileObj.LoadReferringProvider(ReferringProviderId, null, null, null, null, null);
                    dsReferringProvider = obj.Data;
                    if (dsReferringProvider.Tables[dsReferringProvider.ReferringProvider.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsReferringProvider.Tables[dsReferringProvider.ReferringProvider.TableName].Rows[0];
                        dr[dsReferringProvider.ReferringProvider.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSProfile> objReferringProvider = BLLAdminProfileObj.UpdateReferringProvider(ref dsReferringProvider);
                        string successMsg;
                        if (objReferringProvider.Data != null)
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
                                Message = objReferringProvider.Message
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

        public string SearchReferringProvider(string name)
        {
            try
            {
                DSProfileLookup dsProfileLookup = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupReferringProvider("1", name);

                dsProfileLookup = obj.Data;
                if (obj.Data != null)
                {
                    if (dsProfileLookup.Tables[dsProfileLookup.ReferringProvider.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ReferringProviderCount = dsProfileLookup.Tables[dsProfileLookup.ReferringProvider.TableName].Rows.Count,
                            ReferringProviderLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsProfileLookup.Tables[dsProfileLookup.ReferringProvider.TableName])),
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
        public string SearchReferringProvider(ReferringProviderModel model)
        {
            try
            {
                DSProfileLookup dsProfileLookup = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupReferringProviderAutoComplete("1", model.RefProName);

                dsProfileLookup = obj.Data;
                if (obj.Data != null)
                {
                    if (dsProfileLookup.Tables[dsProfileLookup.ReferringProvider.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ReferringProviderCount = dsProfileLookup.Tables[dsProfileLookup.ReferringProvider.TableName].Rows.Count,
                            ReferringProviderLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsProfileLookup.Tables[dsProfileLookup.ReferringProvider.TableName])),
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
        /// <summary>
        /// Handle the Resource Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_REFERRING_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ReferringProviderData"];
                        string strJSONData = SaveReferringProvider(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_REFERRING_PROVIDER":
                    {
                        string strReferringProviderId = context.Request["ReferringProviderID"];
                        string strEntityId = context.Request["EntityId"];
                        string strJSONData = FillReferringProvider(MDVUtility.ToInt64(strReferringProviderId), strEntityId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_REFERRING_PROVIDER":
                    {
                        string strReferringProviderId = context.Request["ReferringProviderID"];
                        string strJSONData = DeleteReferringProvider(MDVUtility.ToInt64(strReferringProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_REFERRING_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ReferringProviderData"];
                        Int64 ReferringProviderID = MDVUtility.ToInt64(context.Request["ReferringProviderID"]);
                        string strJSONData = UpdateReferringProvider(fieldsJSON, ReferringProviderID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_REFERRING_PROVIDER_ACTIVE_INACTIVE":
                    {
                        Int64 ReferringProviderID = MDVUtility.ToInt64(context.Request["ReferringProviderID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateReferringProviderIsActive(ReferringProviderID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "LOAD_REFERRING_PROVIDER_AUTOCOMPLETE":
                    {
                        string RefProName = context.Request["RefProName"];
                        string strJSONData = SearchReferringProvider(RefProName);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}
