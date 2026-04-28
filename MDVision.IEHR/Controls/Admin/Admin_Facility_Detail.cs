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

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_Facility_Detail
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_Facility_Detail()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_Facility_Detail _obj = null;
        public static Admin_Facility_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_Facility_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the facility.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string</returns>
        private string SaveFacility(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Facility", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsProfile = new DSProfile();
                    DSProfile.FacilityRow dr = dsProfile.Facility.NewFacilityRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"].Trim();
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFeeGroup"]))
                    {
                        dr.FeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFeeGroup"]);
                        dr.FeeGroupName = MDVUtility.ToStr(SearchedfieldsJSON["ddlFeeGroup_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPractice"]))
                    {
                        dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPractice"]);
                        dr.PracticeName = MDVUtility.ToStr(SearchedfieldsJSON["ddlPractice_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBasicFeeGroup"]))
                    {
                        dr.BasicFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBasicFeeGroup"]);
                        dr.BasicFeeGroupName = MDVUtility.ToStr(SearchedfieldsJSON["ddlBasicFeeGroup_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPos"]))
                    {
                        dr.POSId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPos"]);
                        dr.POSCode = MDVUtility.ToStr(SearchedfieldsJSON["ddlPos_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlRevenueCode"]))
                    {
                        dr.RevenueCodeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlRevenueCode"]);
                        dr.RevenueCode = MDVUtility.ToStr(SearchedfieldsJSON["ddlRevenueCode_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFacilityType"]))
                    {
                        dr.FacilityTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFacilityType"]);
                        dr.FacilityTypeName = MDVUtility.ToStr(SearchedfieldsJSON["ddlFacilityType_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlLocation"]))
                    {
                        dr.LocationId = MDVUtility.ToStr(SearchedfieldsJSON["ddlLocation"]);
                        dr.LocationName = MDVUtility.ToStr(SearchedfieldsJSON["ddlLocation_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCLIA"]))
                    {
                        dr.CLIA = MDVUtility.ToStr(SearchedfieldsJSON["txtCLIA"]);
                    }
                    dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                    dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                    dr.ZIPCodeExt = SearchedfieldsJSON["txtExt"];
                    dr.State = SearchedfieldsJSON["txtState"];
                    dr.City = SearchedfieldsJSON["txtCity"];
                    dr.Address = SearchedfieldsJSON["txtAddress"];
                    dr.Fax = SearchedfieldsJSON["txtFax"];
                    dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                    dr.PhoneExt = "";
                    dr.Notes = SearchedfieldsJSON["txtComments"];
                    dr.TaxonomyCode = SearchedfieldsJSON["txtTaxonomyCode"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlStatementGroup"]))
                    {
                        dr.StmtGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlStatementGroup"]);
                        dr.StmtGroupName = MDVUtility.ToStr(SearchedfieldsJSON["ddlStatementGroup_text"]);
                    }
                    dr.BillToPractice = MDVUtility.ToStr(SearchedfieldsJSON["chkBillToPractice"]) == "True" ? true : false;
                    dr.MammographyCertificateNo = SearchedfieldsJSON["txtMammographyCert"];
                    dr.NPI = SearchedfieldsJSON["txtNPI"];
                    dr.WebSiteURL = SearchedfieldsJSON["txtWebSite"];
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpStartDate"])))
                        dr.StartDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpStartDate"]);
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.Color = SearchedfieldsJSON["txtColor"];
                    dr.OrgId = SearchedfieldsJSON["txtOrgId"];

                    #region Database Insertion
                    dsProfile.Facility.AddFacilityRow(dr);
                    BLObject<DSProfile> objFacility = BLLAdminProfileObj.InsertFacility(ref dsProfile);
                    dsProfile = objFacility.Data;
                    if (objFacility.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message_Contact_Admin,
                            FacilityId = dsProfile.Tables[dsProfile.Facility.TableName].Rows[0][dsProfile.Facility.FacilityIdColumn.ColumnName]
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
        /// Updates the facility.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <returns>Json string</returns>
        private string UpdateFacility(string fieldsJSON, Int64 FacilityId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Facility", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsProfile = new DSProfile();
                    //DSProfile.FacilityRow dr = dsProfile.Facility.NewFacilityRow();
                    BLObject<DSProfile> objLoad = BLLAdminProfileObj.LoadFacility(FacilityId, null, null, null, null, null);

                    if (objLoad.Data != null)
                    {
                        dsProfile = objLoad.Data;

                        DSProfile.FacilityRow dr = (DSProfile.FacilityRow)dsProfile.Tables[dsProfile.Facility.TableName].Rows[0];

                        //dr.FacilityId = FacilityId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"].Trim();
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFeeGroup"]))
                        {
                            dr.FeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFeeGroup"]);
                            dr.FeeGroupName = MDVUtility.ToStr(SearchedfieldsJSON["ddlFeeGroup_text"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPractice"]))
                        {
                            dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPractice"]);
                            dr.PracticeName = MDVUtility.ToStr(SearchedfieldsJSON["ddlPractice_text"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBasicFeeGroup"]))
                        {
                            dr.BasicFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBasicFeeGroup"]);
                            dr.BasicFeeGroupName = MDVUtility.ToStr(SearchedfieldsJSON["ddlBasicFeeGroup_text"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPos"]))
                        {
                            dr.POSId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPos"]);
                            dr.POSCode = MDVUtility.ToStr(SearchedfieldsJSON["ddlPos_text"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlRevenueCode"]))
                        {
                            dr.RevenueCodeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlRevenueCode"]);
                            dr.RevenueCode = MDVUtility.ToStr(SearchedfieldsJSON["ddlRevenueCode_text"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFacilityType"]))
                        {
                            dr.FacilityTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFacilityType"]);
                            dr.FacilityTypeName = MDVUtility.ToStr(SearchedfieldsJSON["ddlFacilityType_text"]);
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlLocation"]))
                        {
                            dr.LocationId = MDVUtility.ToStr(SearchedfieldsJSON["ddlLocation"]);
                            dr.LocationName = MDVUtility.ToStr(SearchedfieldsJSON["ddlLocation_text"]);
                        }

                        dr.CLIA = MDVUtility.ToStr(SearchedfieldsJSON["txtCLIA"]);

                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                        dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                        dr.ZIPCodeExt = SearchedfieldsJSON["txtExt"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.Address = SearchedfieldsJSON["txtAddress"];
                        dr.Fax = SearchedfieldsJSON["txtFax"];
                        dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                        dr.PhoneExt = "";
                        dr.Notes = SearchedfieldsJSON["txtComments"];
                        dr.TaxonomyCode = SearchedfieldsJSON["txtTaxonomyCode"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlStatementGroup"]))
                            dr.StmtGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlStatementGroup"]);
                        else
                            dr.StmtGroupId = 0;

                        dr.BillToPractice = MDVUtility.ToStr(SearchedfieldsJSON["chkBillToPractice"]) == "True" ? true : false;
                        dr.MammographyCertificateNo = SearchedfieldsJSON["txtMammographyCert"];
                        dr.NPI = SearchedfieldsJSON["txtNPI"];
                        dr.WebSiteURL = SearchedfieldsJSON["txtWebSite"];
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpStartDate"])))
                            dr.StartDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpStartDate"]);
                        else
                            dr[dsProfile.Facility.StartDateColumn] = DBNull.Value;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.Color = SearchedfieldsJSON["txtColor"];
                        dr.OrgId = SearchedfieldsJSON["txtOrgId"];

                        #region Database Updation
                        //dsProfile.Facility.AddFacilityRow(dr);
                        //dsProfile.Facility.AcceptChanges();

                        if (dsProfile.Tables[dsProfile.Facility.TableName].Rows.Count > 0)
                        {
                            //dsProfile.Facility.Rows[0].SetModified();
                            BLObject<DSProfile> objFacility = BLLAdminProfileObj.UpdateFacility(ref dsProfile);
                            dsProfile = objFacility.Data;
                            if (objFacility.Data != null)
                            {
                                var response = new
                                {
                                    status = true,
                                    Message = Common.AppPrivileges.Update_Message
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
                                Message = Common.AppPrivileges.No_Record_Message
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
                            Message = Common.AppPrivileges.No_Record_Message
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
        /// Get the facility against facility ID.
        /// </summary>
        /// <param name="PracticeID">The facility identifier.</param>
        /// <returns>Json string</returns>
        private string FillFacility(Int64 FacilityId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Facility", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(FacilityId)))
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
                        BLObject<DSProfile> objFacility = BLLAdminProfileObj.LoadFacility(FacilityId, null, null, null, null, null);
                        if (objFacility.Data != null)
                        {
                            dsProfile = objFacility.Data;
                            if (dsProfile.Tables[dsProfile.Facility.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsProfile.Tables[dsProfile.Facility.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsProfile.Facility.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsProfile.Facility.DescriptionColumn.ColumnName])},
                            { "ddlFeeGroup", MDVUtility.ToStr(dr[dsProfile.Facility.FeeGroupIdColumn.ColumnName])},
                            { "ddlPractice", MDVUtility.ToStr(dr[dsProfile.Facility.PracticeIdColumn.ColumnName])},
                            { "ddlBasicFeeGroup", MDVUtility.ToStr(dr[dsProfile.Facility.BasicFeeGroupIdColumn.ColumnName])},
                            { "ddlPos", MDVUtility.ToStr(dr[dsProfile.Facility.POSIdColumn.ColumnName])},
                            { "ddlRevenueCode", MDVUtility.ToStr(dr[dsProfile.Facility.RevenueCodeIdColumn.ColumnName])},
                            { "ddlFacilityType", MDVUtility.ToStr(dr[dsProfile.Facility.FacilityTypeIdColumn.ColumnName])},
                            { "ddlLocation", MDVUtility.ToStr(dr[dsProfile.Facility.LocationIdColumn.ColumnName])},
                            { "txtWebSite", MDVUtility.ToStr(dr[dsProfile.Facility.WebSiteURLColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsProfile.Facility.EmailAddressColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsProfile.Facility.ZIPCodeColumn.ColumnName])},
                            { "txtExt", MDVUtility.ToStr(dr[dsProfile.Facility.ZIPCodeExtColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsProfile.Facility.StateColumn.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsProfile.Facility.CityColumn.ColumnName])},
                            { "txtAddress", MDVUtility.ToStr(dr[dsProfile.Facility.AddressColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(dr[dsProfile.Facility.FaxColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsProfile.Facility.PhoneNoColumn.ColumnName])},
                            { "txtTaxonomyCode", MDVUtility.ToStr(dr[dsProfile.Facility.TaxonomyCodeColumn.ColumnName])},
                            { "txtNPI", MDVUtility.ToStr(dr[dsProfile.Facility.NPIColumn.ColumnName])},
                            { "txtMammographyCert", MDVUtility.ToStr(dr[dsProfile.Facility.MammographyCertificateNoColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsProfile.Facility.NotesColumn.ColumnName])},
                            { "chkBillToPractice", MDVUtility.ToStr(dr[dsProfile.Facility.BillToPracticeColumn.ColumnName])},
                            //{ "dtpStartDate", MDVUtility.ToStr(dr[dsProfile.Facility.StartDateColumn.ColumnName])},
                            { "dtpStartDate", MDVUtility.ToStr(dr[dsProfile.Facility.StartDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsProfile.Facility.StartDateColumn.ColumnName]).ToShortDateString():""},
                            { "ddlStatementGroup", MDVUtility.ToStr(dr[dsProfile.Facility.StmtGroupIdColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsProfile.Facility.IsActiveColumn.ColumnName])},
                            { "txtColor", MDVUtility.ToStr(dr[dsProfile.Facility.ColorColumn.ColumnName])},
                            { "txtOrgId", MDVUtility.ToStr(dr[dsProfile.Facility.OrgIdColumn.ColumnName])},
                            { "txtCLIA", MDVUtility.ToStr(dr[dsProfile.Facility.CLIAColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    FacilityFill_JSON = js.Serialize(keyValues)
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
                                Message = objFacility.Message
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
        /// Deletes the facility against facility Id.
        /// </summary>
        /// <param name="PracticeID">The facility identifier.</param>
        /// <returns>Json string containing Success message or Exception message</returns>
        private string DeleteFacility(Int64 FacilityId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Facility", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(FacilityId)))
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
                        BLObject<string> objFacility = BLLAdminProfileObj.DeleteFacility(MDVUtility.ToStr(FacilityId));

                        if (objFacility.Data == "")
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
                                Message = objFacility.Data
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
        private string UpdateFacilityIsActive(Int64 FacilityId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Facility", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSProfile dsProfile = null;
                    BLObject<DSProfile> obj = BLLAdminProfileObj.LoadFacility(FacilityId, null, null, null, null, null);
                    dsProfile = obj.Data;
                    if (dsProfile.Tables[dsProfile.Facility.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsProfile.Tables[dsProfile.Facility.TableName].Rows[0];
                        dr[dsProfile.Facility.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSProfile> objFacility = BLLAdminProfileObj.UpdateFacility(ref dsProfile);
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
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Facility Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_FACILITY":
                    {
                        string fieldsJSON = context.Request["FacilityData"];
                        string strJSONData = SaveFacility(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_FACILITY":
                    {
                        string strFacilityId = context.Request["FacilityID"];
                        string strJSONData = FillFacility(MDVUtility.ToInt64(strFacilityId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_FACILITY":
                    {
                        string strFacilityId = context.Request["FacilityID"];
                        string strJSONData = DeleteFacility(MDVUtility.ToInt64(strFacilityId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_FACILITY":
                    {
                        string fieldsJSON = context.Request["FacilityData"];
                        Int64 FacilityID = MDVUtility.ToInt64(context.Request["FacilityID"]);
                        string strJSONData = UpdateFacility(fieldsJSON, FacilityID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_FACILITY_ACTIVE_INACTIVE":
                    {
                        Int64 FacilityID = MDVUtility.ToInt64(context.Request["FacilityID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateFacilityIsActive(FacilityID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}