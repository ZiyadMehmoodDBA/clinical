using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;

using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_BillingProvider_Detail
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_BillingProvider_Detail()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }

        #region Singleton
        private static Admin_BillingProvider_Detail _obj = null;
        public static Admin_BillingProvider_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_BillingProvider_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the BillingProviderSettings.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveBillingProvider(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Billing Provider", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSBillingProviderSettings dsBillingProvider = new DSBillingProviderSettings();
                DSBillingProviderSettings.BillingProviderRow dr = dsBillingProvider.BillingProvider.NewBillingProviderRow();


                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtShortName"]))
                    dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                    dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlProviderType"]))
                    dr.ProviderType = MDVUtility.ToStr(SearchedfieldsJSON["ddlProviderType"]);

                dr.ISEIN = MDVUtility.ToStr(SearchedfieldsJSON["chkBillToEIN"]) == "True" ? true : false;

                dr.EINSuffix = SearchedfieldsJSON["txtEINSuffix"];
                dr.EIN = SearchedfieldsJSON["txtEIN"];
                dr.LastName = SearchedfieldsJSON["txtLastName"];
                dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                dr.MI = SearchedfieldsJSON["txtMI"];
                dr.NPI = SearchedfieldsJSON["txtNPI"];
                dr.TaxonomyCode = SearchedfieldsJSON["txtTaxonomyCode"];
                dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                dr.City = SearchedfieldsJSON["txtCity"];
                dr.State = SearchedfieldsJSON["lstState"];
                dr.ZIPCode = SearchedfieldsJSON["txtZipCode"];
                dr.ZIPCodeExt = SearchedfieldsJSON["txtZipCodeExt"];
                dr.PayToAddress1 = SearchedfieldsJSON["txtPayToAddress1"];
                dr.PayToAddress2 = SearchedfieldsJSON["txtPayToAddress2"];
                dr.PayToCity = SearchedfieldsJSON["txtPayToCity"];
                dr.PayToState = SearchedfieldsJSON["txtPayToState"];
                dr.PayToZIPCode = SearchedfieldsJSON["txtPayToZipCode"];
                dr.PayToZIPCodeExt = SearchedfieldsJSON["txtPayToZipCodeExt"];
                dr.ZIPCodeExt = SearchedfieldsJSON["txtZipCodeExt"];
                dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                dr.IsPayToAddress = MDVUtility.ToStr(SearchedfieldsJSON["chkIsPayToAddress"]) == "True" ? true : false;

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsBillingProvider.BillingProvider.AddBillingProviderRow(dr);
                BLObject<DSBillingProviderSettings> obj = BLLAdminProfileObj.InsertBillingProvider(ref dsBillingProvider);
                dsBillingProvider = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        BillingProviderId = dsBillingProvider.Tables[dsBillingProvider.BillingProvider.TableName].Rows[0][dsBillingProvider.BillingProvider.BillingProviderIdColumn.ColumnName]
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
        /// Updates the BillingProviderSettings.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderId">The specialty identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateBillingProvider(string fieldsJSON, Int64 BillingProviderId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Billing Provider", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSBillingProviderSettings dsBillingProvider = new DSBillingProviderSettings();
                BLObject<DSBillingProviderSettings> objLoad = BLLAdminProfileObj.LoadBillingProvider(BillingProviderId, null, null, null, null, null);
                if (objLoad.Data != null)
                {
                    dsBillingProvider = objLoad.Data;
                    DSBillingProviderSettings.BillingProviderRow dr = (DSBillingProviderSettings.BillingProviderRow)dsBillingProvider.Tables[dsBillingProvider.BillingProvider.TableName].Rows[0];

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtShortName"]))
                        dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlProviderType"]))
                        dr.ProviderType = MDVUtility.ToStr(SearchedfieldsJSON["ddlProviderType"]);

                    dr.ISEIN = MDVUtility.ToStr(SearchedfieldsJSON["chkBillToEIN"]) == "True" ? true : false;
                    dr.EINSuffix = SearchedfieldsJSON["txtEINSuffix"];
                    dr.EIN = SearchedfieldsJSON["txtEIN"];
                    dr.LastName = SearchedfieldsJSON["txtLastName"];
                    dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                    dr.MI = SearchedfieldsJSON["txtMI"];
                    dr.NPI = SearchedfieldsJSON["txtNPI"];
                    dr.TaxonomyCode = SearchedfieldsJSON["txtTaxonomyCode"];
                    dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                    dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                    dr.City = SearchedfieldsJSON["txtCity"];
                    dr.State = SearchedfieldsJSON["lstState"];
                    dr.ZIPCode = SearchedfieldsJSON["txtZipCode"];
                    dr.ZIPCodeExt = SearchedfieldsJSON["txtZipCodeExt"];
                    dr.PayToAddress1 = SearchedfieldsJSON["txtPayToAddress1"];
                    dr.PayToAddress2 = SearchedfieldsJSON["txtPayToAddress2"];
                    dr.PayToCity = SearchedfieldsJSON["txtPayToCity"];
                    dr.PayToState = SearchedfieldsJSON["txtPayToState"];
                    dr.PayToZIPCode = SearchedfieldsJSON["txtPayToZipCode"];
                    dr.PayToZIPCodeExt = SearchedfieldsJSON["txtPayToZipCodeExt"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.IsPayToAddress = MDVUtility.ToStr(SearchedfieldsJSON["chkIsPayToAddress"]) == "True" ? true : false;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Updation

                    if (dsBillingProvider.Tables[dsBillingProvider.BillingProvider.TableName].Rows.Count > 0)
                    {

                        BLObject<DSBillingProviderSettings> obj = BLLAdminProfileObj.UpdateBillingProvider(ref dsBillingProvider);
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
        /// Fills the Billing Provider.
        /// </summary>
        /// <param name="ProviderId">The Billing Provider identifier.</param>
        /// <returns></returns>
        private string FillBillingProvider(Int64 BillingProviderId, string NPINumber)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(BillingProviderId)))
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
                    DSBillingProviderSettings dsBillingProvider = null;
                    //dsProfile = DALSpecialty.Instance.LoadSpecialty(SpecialtyId, null, null);
                    BLObject<DSBillingProviderSettings> obj = BLLAdminProfileObj.LoadBillingProvider(BillingProviderId, null, null, null, null, NPINumber);
                    if (obj.Data != null)
                    {
                        dsBillingProvider = obj.Data;
                        if (dsBillingProvider.Tables[dsBillingProvider.BillingProvider.TableName].Rows.Count > 0)
                        {
                            if (dsBillingProvider.Tables[dsBillingProvider.BillingProvider.TableName].Rows.Count < 2)
                            {
                                DataRow dr = dsBillingProvider.Tables[dsBillingProvider.BillingProvider.TableName].Rows[0];
                                bool IsBillToEIN = false;
                                bool IsBillToSSN = false;

                                if (MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.ISEINColumn.ColumnName]).ToLower() == "true")
                                    IsBillToEIN = true;
                                else
                                    IsBillToSSN = true;

                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.ShortNameColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.EntityIdColumn.ColumnName])},
                            { "ddlProviderType", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.ProviderTypeColumn.ColumnName])},
                            { "chkBillToEIN", MDVUtility.ToStr(IsBillToEIN) },
                            { "chkBillToProviderSSN", MDVUtility.ToStr(IsBillToSSN)},
                            { "txtEIN", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.EINColumn.ColumnName])},
                            { "txtEINSuffix", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.EINSuffixColumn.ColumnName])},
                            { "txtLastName", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.LastNameColumn.ColumnName])},
                            { "txtFirstName", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.FirstNameColumn.ColumnName])},
                            { "txtMI", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.MIColumn.ColumnName])},
                            { "txtNPI", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.NPIColumn.ColumnName])},
                            { "txtTaxonomyCode", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.TaxonomyCodeColumn.ColumnName])},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.Address1Column.ColumnName])},
                            { "txtAddress2", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.Address2Column.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.CityColumn.ColumnName])},
                            { "lstState", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.StateColumn.ColumnName])},
                            { "txtZipCode", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.ZIPCodeColumn.ColumnName])},
                            { "txtZipCodeExt", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.ZIPCodeExtColumn.ColumnName])},
                            { "txtPayToAddress1", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.PayToAddress1Column.ColumnName])},
                            { "txtPayToAddress2", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.PayToAddress2Column.ColumnName])},
                            { "txtPayToCity", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.PayToCityColumn.ColumnName])},
                            { "txtPayToState", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.PayToStateColumn.ColumnName])},
                            { "txtPayToZipCode", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.PayToZIPCodeColumn.ColumnName])},
                            { "txtPayToZipCodeExt", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.PayToZIPCodeExtColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.IsActiveColumn.ColumnName])},
                            { "chkIsPayToAddress", MDVUtility.ToStr(dr[dsBillingProvider.BillingProvider.IsPayToAddressColumn.ColumnName])},

                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    BillingProviderSettingsFill_JSON = js.Serialize(keyValues)
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = "Multiple records found"
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }

                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = AppPrivileges.No_Record_Message
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
        /// Deletes the Billing Provider against provider Id.
        /// </summary>
        /// <param name="ProviderId">The specialty identifier.</param>
        /// <returns></returns>
        private string DeleteBillingProvider(Int64 BillingProviderId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(BillingProviderId)))
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
                    BLObject<string> obj = BLLAdminProfileObj.DeleteBillingProvider(MDVUtility.ToStr(BillingProviderId));
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
        /// Updates the facility is active.
        /// </summary>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateBillingProviderIsActive(Int64 BillingProviderId, Int64 IsActive)
        {
            try
            {
                DSBillingProviderSettings dsBillingProvider = null;
                BLObject<DSBillingProviderSettings> obj = BLLAdminProfileObj.LoadBillingProvider(BillingProviderId, null, null, null, null, null);
                dsBillingProvider = obj.Data;
                if (dsBillingProvider.Tables[dsBillingProvider.BillingProvider.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsBillingProvider.Tables[dsBillingProvider.BillingProvider.TableName].Rows[0];
                    dr[dsBillingProvider.BillingProvider.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSBillingProviderSettings> objProvider = BLLAdminProfileObj.UpdateBillingProvider(ref dsBillingProvider);
                    string successMsg;
                    if (objProvider.Data != null)
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
                            Message = objProvider.Message
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
        /// Handle the Billing Provider Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_BILLING_PROVIDER":
                    {
                        string fieldsJSON = context.Request["BillingProviderData"];
                        string strJSONData = SaveBillingProvider(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_BILLING_PROVIDER":
                    {
                        string BillingProviderID = context.Request["BillingProviderID"];
                        string NPINumber = context.Request["NPINumber"];
                        if (NPINumber == "undefined") NPINumber = null;
                        string strJSONData = FillBillingProvider(MDVUtility.ToInt64(BillingProviderID), NPINumber);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_BILLING_PROVIDER":
                    {
                        string strBillingProviderId = context.Request["BillingProviderID"];
                        string strJSONData = DeleteBillingProvider(MDVUtility.ToInt64(strBillingProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BILLING_PROVIDER":
                    {
                        string fieldsJSON = context.Request["BillingProviderData"];
                        Int64 BillingProviderID = MDVUtility.ToInt64(context.Request["BillingProviderID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateBillingProvider(fieldsJSON, BillingProviderID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BILLING_PROVIDER_ACTIVE_INACTIVE":
                    {
                        Int64 BillingProviderID = MDVUtility.ToInt64(context.Request["BillingProviderID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateBillingProviderIsActive(BillingProviderID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion

    }
}