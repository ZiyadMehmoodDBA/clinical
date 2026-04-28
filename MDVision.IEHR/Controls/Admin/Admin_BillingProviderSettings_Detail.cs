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
    public class Admin_BillingProviderSettings_Detail
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_BillingProviderSettings_Detail()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_BillingProviderSettings_Detail _obj = null;
        public static Admin_BillingProviderSettings_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_BillingProviderSettings_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the BillingProviderSettings.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveBillingProviderSettings(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Additional Billing Provider", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSBillingProviderSettings dsBillingProviderSettings = new DSBillingProviderSettings();
                    DSBillingProviderSettings.BillingProviderSettingsRow dr = dsBillingProviderSettings.BillingProviderSettings.NewBillingProviderSettingsRow();


                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstInsurance"]))
                        dr.InsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["lstInsurance"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstFacility"]))
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["lstFacility"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstProvider"]))
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["lstProvider"]);
                    dr.BillToProviderSSN = MDVUtility.ToStr(SearchedfieldsJSON["chkBillToProviderSSN"]) == "True" ? true : false;
                    dr.AcceptAssignment = MDVUtility.ToStr(SearchedfieldsJSON["chkAcceptAssignment"]) == "True" ? true : false;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lst2310B"]))
                        dr.Loop2310B = MDVUtility.ToInt16(SearchedfieldsJSON["lst2310B"]);
                    dr.BillToEIN = MDVUtility.ToStr(SearchedfieldsJSON["chkBillToEIN"]) == "True" ? true : false;
                    dr.EINSuffix = SearchedfieldsJSON["txtEINSuffix"];
                    dr.EIN = SearchedfieldsJSON["txtEIN"];
                    dr.EINName = SearchedfieldsJSON["txtEINName"];
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
                    dsBillingProviderSettings.BillingProviderSettings.AddBillingProviderSettingsRow(dr);
                    BLObject<DSBillingProviderSettings> obj = BLLAdminProfileObj.InsertBillingProviderSettings(ref dsBillingProviderSettings);
                    dsBillingProviderSettings = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            BillingProviderSettingsId = dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProviderSettings.TableName].Rows[0][dsBillingProviderSettings.BillingProviderSettings.BillingProviderIdColumn.ColumnName]
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
        private string UpdateBillingProviderSettings(string fieldsJSON, Int64 BillingProviderSettingsId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Additional Billing Provider", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSBillingProviderSettings dsBillingProviderSettings = new DSBillingProviderSettings();
                    //DSBillingProviderSettings.BillingProviderSettingsRow dr = dsBillingProviderSettings.BillingProviderSettings.NewBillingProviderSettingsRow();
                    BLObject<DSBillingProviderSettings> objLoad = BLLAdminProfileObj.LoadBillingProviderSettings(BillingProviderSettingsId, null, null, null, null);
                    if (objLoad.Data != null)
                    {
                        dsBillingProviderSettings = objLoad.Data;
                        DSBillingProviderSettings.BillingProviderSettingsRow dr = (DSBillingProviderSettings.BillingProviderSettingsRow)dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProviderSettings.TableName].Rows[0];
                        //dr.BillingProviderId = BillingProviderSettingsId;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstInsurance"]))
                            dr.InsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["lstInsurance"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstFacility"]))
                            dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["lstFacility"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstProvider"]))
                            dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["lstProvider"]);
                        dr.BillToProviderSSN = MDVUtility.ToStr(SearchedfieldsJSON["chkBillToProviderSSN"]) == "True" ? true : false;
                        dr.AcceptAssignment = MDVUtility.ToStr(SearchedfieldsJSON["chkAcceptAssignment"]) == "True" ? true : false;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lst2310B"]))
                            dr.Loop2310B = MDVUtility.ToInt16(SearchedfieldsJSON["lst2310B"]);
                        else
                            dr.Loop2310B = MDVUtility.ToInt16(0);
                        dr.BillToEIN = MDVUtility.ToStr(SearchedfieldsJSON["chkBillToEIN"]) == "True" ? true : false;
                        dr.EINSuffix = SearchedfieldsJSON["txtEINSuffix"];
                        dr.EIN = SearchedfieldsJSON["txtEIN"];
                        dr.EINName = SearchedfieldsJSON["txtEINName"];
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

                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        #region Database Updation
                        //dsBillingProviderSettings.BillingProviderSettings.AddBillingProviderSettingsRow(dr);
                        //dsBillingProviderSettings.BillingProviderSettings.AcceptChanges();

                        if (dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProviderSettings.TableName].Rows.Count > 0)
                        {
                            //dsBillingProviderSettings.BillingProviderSettings.Rows[0].SetModified();
                            BLObject<DSBillingProviderSettings> obj = BLLAdminProfileObj.UpdateBillingProviderSettings(ref dsBillingProviderSettings);
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
        private string FillBillingProviderSettings(Int64 BillingProviderSettingsId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Additional Billing Provider", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(BillingProviderSettingsId)))
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
                        DSBillingProviderSettings dsBillingProviderSettings = null;
                        //dsProfile = DALSpecialty.Instance.LoadSpecialty(SpecialtyId, null, null);
                        BLObject<DSBillingProviderSettings> obj = BLLAdminProfileObj.LoadBillingProviderSettings(BillingProviderSettingsId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsBillingProviderSettings = obj.Data;
                            if (dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProviderSettings.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProviderSettings.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "lstProvider", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.ProviderIdColumn.ColumnName])},
                            { "txtlstProvider", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.ProviderNameColumn.ColumnName])},
                            { "lstFacility", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.FacilityIdColumn.ColumnName])},
                            { "txtlstFacility", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.FacilityNameColumn.ColumnName])},
                            { "lstInsurance", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.InsuranceIdColumn.ColumnName])},
                            { "txtlstInsurance", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.InsuranceNameColumn.ColumnName])},
                            { "chkBillToProviderSSN", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.BillToProviderSSNColumn.ColumnName])},
                            { "chkAcceptAssignment", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.AcceptAssignmentColumn.ColumnName])},
                            { "lst2310B", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.Loop2310BColumn.ColumnName])},
                            { "chkBillToEIN", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.BillToEINColumn.ColumnName])},
                            { "txtEINSuffix", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.EINSuffixColumn.ColumnName])},
                            { "txtEIN", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.EINColumn.ColumnName])},
                            { "txtEINName", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.EINNameColumn.ColumnName])},
                            { "txtNPI", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.NPIColumn.ColumnName])},
                            { "txtTaxonomyCode", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.TaxonomyCodeColumn.ColumnName])},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.Address1Column.ColumnName])},
                            { "txtAddress2", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.Address2Column.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.CityColumn.ColumnName])},
                            { "lstState", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.StateColumn.ColumnName])},
                            { "txtZipCode", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.ZIPCodeColumn.ColumnName])},
                            { "txtZipCodeExt", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.ZIPCodeExtColumn.ColumnName])},
                            { "txtPayToAddress1", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.PayToAddress1Column.ColumnName])},
                            { "txtPayToAddress2", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.PayToAddress2Column.ColumnName])},
                            { "txtPayToCity", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.PayToCityColumn.ColumnName])},
                            { "txtPayToState", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.PayToStateColumn.ColumnName])},
                            { "txtPayToZipCode", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.PayToZIPCodeColumn.ColumnName])},
                            { "txtPayToZipCodeExt", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.PayToZIPCodeExtColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.IsActiveColumn.ColumnName])},
                            { "chkIsPayToAddress", MDVUtility.ToStr(dr[dsBillingProviderSettings.BillingProviderSettings.IsPayToAddressColumn.ColumnName])},

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
        /// Deletes the Billing Provider against provider Id.
        /// </summary>
        /// <param name="ProviderId">The specialty identifier.</param>
        /// <returns></returns>
        private string DeleteBillingProviderSettings(Int64 BillingProviderSettingsId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Additional Billing Provider", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(BillingProviderSettingsId)))
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
                        BLObject<string> obj = BLLAdminProfileObj.DeleteBillingProviderSettings(MDVUtility.ToStr(BillingProviderSettingsId));
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
        /// Updates the facility is active.
        /// </summary>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateBillingProviderSettingsIsActive(Int64 BillingProviderSettingsId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Additional Billing Provider", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSBillingProviderSettings dsBillingProviderSettings = null;
                    BLObject<DSBillingProviderSettings> obj = BLLAdminProfileObj.LoadBillingProviderSettings(BillingProviderSettingsId, null, null, null, null);
                    dsBillingProviderSettings = obj.Data;
                    if (dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProviderSettings.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsBillingProviderSettings.Tables[dsBillingProviderSettings.BillingProviderSettings.TableName].Rows[0];
                        dr[dsBillingProviderSettings.BillingProviderSettings.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSBillingProviderSettings> objFacility = BLLAdminProfileObj.UpdateBillingProviderSettings(ref dsBillingProviderSettings);
                        string successMsg;
                        if (objFacility.Data != null)
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
                                Message = objFacility.Message
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

        private string CheckProviderSSN(long ProviderId)
        {
            try
            {
                DSProfile dsProvider = null;
                BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProvider(ProviderId, null, null, null, null, null, null, null);
                if (obj.Data != null)
                {
                    dsProvider = obj.Data;
                    if (dsProvider.Tables[dsProvider.Provider.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsProvider.Tables[dsProvider.Provider.TableName].Rows[0];
                        long ssn = MDVUtility.ToLong(dr[dsProvider.Provider.SSNColumn.ColumnName]);
                        if (ssn != 0)
                        {
                            var response = new
                            {
                                status = true,
                                Message = "",
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Selected Provider not have SSN.",
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "",
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
        /// Handle the Billing Provider Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_BILLING_PROVIDER_SETTINGS":
                    {
                        string fieldsJSON = context.Request["BillingProviderSettingsData"];
                        string strJSONData = SaveBillingProviderSettings(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_BILLING_PROVIDER_SETTINGS":
                    {
                        string strBillingProviderSettingsId = context.Request["BillingProviderSettingsID"];
                        string strJSONData = FillBillingProviderSettings(MDVUtility.ToInt64(strBillingProviderSettingsId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_BILLING_PROVIDER_SETTINGS":
                    {
                        string strBillingProviderSettingsId = context.Request["BillingProviderSettingsID"];
                        string strJSONData = DeleteBillingProviderSettings(MDVUtility.ToInt64(strBillingProviderSettingsId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BILLING_PROVIDER_SETTINGS":
                    {
                        string fieldsJSON = context.Request["BillingProviderSettingsData"];
                        Int64 BillingProviderSettingsID = MDVUtility.ToInt64(context.Request["BillingProviderSettingsID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateBillingProviderSettings(fieldsJSON, BillingProviderSettingsID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BILLING_PROVIDER_SETTINGS_ACTIVE_INACTIVE":
                    {
                        Int64 BillingProviderSettingsID = MDVUtility.ToInt64(context.Request["BillingProviderSettingsID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateBillingProviderSettingsIsActive(BillingProviderSettingsID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CHECK_PROVIDER_SSN":
                    {
                        Int64 ProviderId = MDVUtility.ToInt64(context.Request["ProviderId"]);
                        string strJSONData = CheckProviderSSN(ProviderId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }


        #endregion
    }
}