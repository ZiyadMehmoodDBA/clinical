using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using MDVision.Business.BCommon;
using MDVision.Datasets;
using System.Data;
using Newtonsoft.Json;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_Practice_Detail
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_Practice_Detail()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_Practice_Detail _obj = null;
        public static Admin_Practice_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_Practice_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the practice.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SavePractice(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Practice", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsProfile = new DSProfile();
                    DSProfile.PracticeRow dr = dsProfile.Practice.NewPracticeRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFeeGroup"]))
                        dr.FeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFeeGroup"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBasicFeeGroup"]))
                        dr.BasicFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBasicFeeGroup"]);
                    dr.Address2City = SearchedfieldsJSON["txtPayCity"];
                    dr.Address2 = SearchedfieldsJSON["txtPayAddress2"];
                    dr.Address1 = SearchedfieldsJSON["txtPayAddress1"];
                    dr.IsPayToAddress = MDVUtility.ToStr(SearchedfieldsJSON["chkIsPayToAddress"]) == "True" ? true : false;

                    dr.WebSite = SearchedfieldsJSON["txtWebSite"];
                    dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                    dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                    dr.ZIPCodeExt = SearchedfieldsJSON["txtExt"];
                    dr.State = SearchedfieldsJSON["txtState"];
                    dr.City = SearchedfieldsJSON["txtCity"];
                    dr.Address = SearchedfieldsJSON["txtAddress"];
                    dr.Fax = SearchedfieldsJSON["txtFax"];
                    dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                    dr.PhoneExt = "";
                    dr.TaxonomyCode = SearchedfieldsJSON["txtTaxonomyCode"];
                    dr.NPI = SearchedfieldsJSON["txtNPI"];
                    dr.EIN = SearchedfieldsJSON["txtEIN"];
                    dr.Notes = SearchedfieldsJSON["txtComments"];
                    dr.Address2ZIPCodeExt = SearchedfieldsJSON["txtPayExt"];
                    dr.Address2ZIPCode = SearchedfieldsJSON["txtPayZip"];
                    dr.Address2State = SearchedfieldsJSON["txtPayState"];
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpStartDate"])))
                        dr.StartDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpStartDate"]);
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.Scan = MDVUtility.ToStr(SearchedfieldsJSON["chkScan"]) == "True" ? true : false;
                    dr.OCR = MDVUtility.ToStr(SearchedfieldsJSON["chkOCR"]) == "True" ? true : false;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Old Code
                    //foreach (var fieldJson in SearchedfieldsJSON)
                    //{
                    //    string fieldKey = fieldJson.Key;
                    //    switch (fieldKey)
                    //    {
                    //        case "txtShortName":
                    //            dr.ShortName = fieldJson.Value;
                    //            break;
                    //        case "txtDescription":
                    //            dr.Description = fieldJson.Value;
                    //            break;
                    //        case "ddlFeeGroup":
                    //            if (!string.IsNullOrEmpty(fieldJson.Value))
                    //                dr.FeeGroupId = MDVUtility.ToInt64(fieldJson.Value);
                    //            //else
                    //            //    dr.FeeGroupId = DBNull.Value;
                    //            break;
                    //        case "txtPayCity":
                    //            dr.Address2City = fieldJson.Value;
                    //            break;
                    //        case "txtPayAddress2":
                    //            dr.Address2 = fieldJson.Value;
                    //            break;
                    //        case "txtPayAddress1":
                    //            dr.PayToAddress1 = fieldJson.Value;
                    //            break;
                    //        case "txtWebSite":
                    //            dr.Website = fieldJson.Value;
                    //            break;
                    //        case "txtEmail":
                    //            dr.EmailAddress = fieldJson.Value;
                    //            break;
                    //        case "txtZip":
                    //            dr.ZIPCode = fieldJson.Value;
                    //            break;
                    //        case "txtExt":
                    //            dr.ZIPCodeExt = fieldJson.Value;
                    //            break;
                    //        case "txtState":
                    //            dr.State = fieldJson.Value;
                    //            break;
                    //        case "txtCity":
                    //            dr.City = fieldJson.Value;
                    //            break;
                    //        case "txtAddress":
                    //            dr.Address = fieldJson.Value;
                    //            break;
                    //        case "txtFax":
                    //            dr.Fax = fieldJson.Value;
                    //            break;
                    //        case "txtTelephone":
                    //            dr.PhoneNo = fieldJson.Value;
                    //            break;
                    //        case "txtTaxonomyCode":
                    //            dr.TaxonomyCode = fieldJson.Value;
                    //            break;
                    //        case "txtNPI":
                    //            dr.NPI = fieldJson.Value;
                    //            break;
                    //        case "txtEIN":
                    //            dr.EIN = fieldJson.Value;
                    //            break;
                    //        case "ddlEntity":
                    //            if (!string.IsNullOrEmpty(fieldJson.Value))
                    //                dr.EntityId = MDVUtility.ToInt64(fieldJson.Value);
                    //            break;
                    //        case "txtComments":
                    //            dr.Notes = fieldJson.Value;
                    //            break;
                    //        case "txtPayExt":
                    //            dr.Address2ZIPCodeExt = fieldJson.Value;
                    //            break;
                    //        case "txtPayZip":
                    //            dr.Address2ZIPCode = fieldJson.Value;
                    //            break;
                    //        case "txtPayState":
                    //            dr.Address2State = fieldJson.Value;
                    //            break;
                    //        case "ddlBasicFeeGroup":
                    //            if (!string.IsNullOrEmpty(fieldJson.Value))
                    //                dr.BasicFeeGroupId = MDVUtility.ToInt64(fieldJson.Value);
                    //            break;
                    //        case "dtpStartDate":
                    //            if (!string.IsNullOrEmpty(fieldJson.Value))
                    //                dr.StartDate = MDVUtility.ToDateTime(fieldJson.Value);
                    //            break;
                    //    }
                    //}
                    //dr.CreatedBy = "";
                    //dr.CreatedOn = DateTime.Now;
                    //dr.IsActive = true;
                    //dr.ModifiedBy = "";
                    //dr.ModifiedOn = DateTime.Now;
                    //dr.PhoneExt = ""; 
                    #endregion

                    #region Database Insertion
                    dsProfile.Practice.AddPracticeRow(dr);
                    BLObject<DSProfile> objPractice = BLLAdminProfileObj.InsertPractice(ref dsProfile);

                    if (objPractice.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message_Contact_Admin,
                            PracticeId = dsProfile.Tables[dsProfile.Practice.TableName].Rows[0][dsProfile.Practice.PracticeIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objPractice.Message
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
        /// Updates the practice.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdatePractice(string fieldsJSON, Int64 PracticeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Practice", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsProfile = new DSProfile();
                    //DSProfile.PracticeRow dr = dsProfile.Practice.NewPracticeRow();
                    BLObject<DSProfile> objLoad = BLLAdminProfileObj.LoadPractice(PracticeId, null, null, null, null, null);
                    dsProfile = objLoad.Data;
                    foreach (DSProfile.PracticeRow dr in dsProfile.Tables[dsProfile.Practice.TableName].Rows)
                    {
                        //dr.PracticeId = PracticeId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFeeGroup"]))
                            dr.FeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFeeGroup"]);
                        else
                            dr.FeeGroupId = MDVUtility.ToInt64(0);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBasicFeeGroup"]))
                            dr.BasicFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBasicFeeGroup"]);
                        else
                            dr.BasicFeeGroupId = MDVUtility.ToInt64(0);
                        dr.Address2City = SearchedfieldsJSON["txtPayCity"];
                        dr.Address2 = SearchedfieldsJSON["txtPayAddress2"];
                        dr.Address1 = SearchedfieldsJSON["txtPayAddress1"];
                        dr.IsPayToAddress = MDVUtility.ToStr(SearchedfieldsJSON["chkIsPayToAddress"]) == "True" ? true : false;

                        dr.WebSite = SearchedfieldsJSON["txtWebSite"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                        dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                        dr.ZIPCodeExt = SearchedfieldsJSON["txtExt"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.Address = SearchedfieldsJSON["txtAddress"];
                        dr.Fax = SearchedfieldsJSON["txtFax"];
                        dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                        dr.PhoneExt = "";
                        dr.TaxonomyCode = SearchedfieldsJSON["txtTaxonomyCode"];
                        dr.NPI = SearchedfieldsJSON["txtNPI"];
                        dr.EIN = SearchedfieldsJSON["txtEIN"];
                        dr.Notes = SearchedfieldsJSON["txtComments"];
                        dr.Address2ZIPCodeExt = SearchedfieldsJSON["txtPayExt"];
                        dr.Address2ZIPCode = SearchedfieldsJSON["txtPayZip"];
                        dr.Address2State = SearchedfieldsJSON["txtPayState"];
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpStartDate"])))
                            dr.StartDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpStartDate"]);
                        else
                            dr[dsProfile.Practice.StartDateColumn] = DBNull.Value;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        dr.Scan = MDVUtility.ToStr(SearchedfieldsJSON["chkScan"]) == "True" ? true : false;
                        dr.OCR = MDVUtility.ToStr(SearchedfieldsJSON["chkOCR"]) == "True" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsProfile.Practice.AddPracticeRow(dr);
                    //dsProfile.Practice.AcceptChanges();

                    if (dsProfile.Tables[dsProfile.Practice.TableName].Rows.Count > 0)
                    {
                        //dsProfile.Practice.Rows[0].SetModified();
                        BLObject<DSProfile> objPractice;
                        objPractice = BLLAdminProfileObj.UpdatePractice(ref dsProfile);
                        if (objPractice.Data != null)
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
                                Message = objPractice.Message
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
        /// Get practice by practice Id.
        /// </summary>
        /// <param name="PracticeID">The practice identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillPractice(Int64 PracticeID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Practice", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PracticeID)))
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
                        //  System.Diagnostics.Debug.WriteLine("Start time " + DateTime.Now);
                        BLObject<DSProfile> objPractice = BLLAdminProfileObj.LoadPractice(PracticeID, null, null, null, null, null);
                        //  System.Diagnostics.Debug.WriteLine("End time " + DateTime.Now);
                        if (objPractice.Data != null)
                        {
                            dsProfile = objPractice.Data;
                            if (dsProfile.Tables[dsProfile.Practice.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsProfile.Tables[dsProfile.Practice.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsProfile.Practice.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsProfile.Practice.DescriptionColumn.ColumnName])},
                            { "ddlFeeGroup", MDVUtility.ToStr(dr[dsProfile.Practice.FeeGroupIdColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsProfile.Practice.EntityIdColumn.ColumnName])},
                            { "ddlBasicFeeGroup", MDVUtility.ToStr(dr[dsProfile.Practice.BasicFeeGroupIdColumn.ColumnName])},
                            { "txtPayCity", MDVUtility.ToStr(dr[dsProfile.Practice.Address2CityColumn.ColumnName])},
                            { "txtPayAddress2", MDVUtility.ToStr(dr[dsProfile.Practice.Address2Column.ColumnName])},
                            { "txtPayAddress1", MDVUtility.ToStr(dr[dsProfile.Practice.Address1Column.ColumnName])},
                            { "chkIsPayToAddress", MDVUtility.ToStr(dr[dsProfile.Practice.IsPayToAddressColumn.ColumnName])},
                            { "txtWebSite", MDVUtility.ToStr(dr[dsProfile.Practice.WebSiteColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsProfile.Practice.EmailAddressColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsProfile.Practice.ZIPCodeColumn.ColumnName])},
                            { "txtExt", MDVUtility.ToStr(dr[dsProfile.Practice.ZIPCodeExtColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsProfile.Practice.StateColumn.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsProfile.Practice.CityColumn.ColumnName])},
                            { "txtAddress", MDVUtility.ToStr(dr[dsProfile.Practice.AddressColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(dr[dsProfile.Practice.FaxColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsProfile.Practice.PhoneNoColumn.ColumnName])},
                            { "txtTaxonomyCode", MDVUtility.ToStr(dr[dsProfile.Practice.TaxonomyCodeColumn.ColumnName])},
                            { "txtNPI", MDVUtility.ToStr(dr[dsProfile.Practice.NPIColumn.ColumnName])},
                            { "txtEIN", MDVUtility.ToStr(dr[dsProfile.Practice.EINColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsProfile.Practice.NotesColumn.ColumnName])},
                            { "txtPayExt", MDVUtility.ToStr(dr[dsProfile.Practice.Address2ZIPCodeExtColumn.ColumnName])},
                            { "txtPayZip", MDVUtility.ToStr(dr[dsProfile.Practice.Address2ZIPCodeColumn.ColumnName])},
                            { "txtPayState", MDVUtility.ToStr(dr[dsProfile.Practice.Address2StateColumn.ColumnName])},
                            //{ "dtpStartDate", MDVUtility.ToStr(dr[dsProfile.Practice.StartDateColumn.ColumnName])},
                            { "dtpStartDate", MDVUtility.ToStr(dr[dsProfile.Practice.StartDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsProfile.Practice.StartDateColumn.ColumnName]).ToShortDateString():""},
                            { "chkActive", MDVUtility.ToStr(dr[dsProfile.Practice.IsActiveColumn.ColumnName])},
                            { "chkScan", MDVUtility.ToStr(dr[dsProfile.Practice.ScanColumn.ColumnName])},
                            { "chkOCR", MDVUtility.ToStr(dr[dsProfile.Practice.OCRColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    PracticeFill_JSON = js.Serialize(keyValues)
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
                                Message = objPractice.Message,
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

        private string demographicPractice(Int64 PracticeID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PracticeID)))
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
                    //  System.Diagnostics.Debug.WriteLine("Start time " + DateTime.Now);
                    BLObject<DSProfile> objPractice = BLLAdminProfileObj.LoadDemographicPractice(PracticeID, null, 1, 15);
                    //  System.Diagnostics.Debug.WriteLine("End time " + DateTime.Now);
                    if (objPractice.Data != null)
                    {
                        dsProfile = objPractice.Data;
                        if (dsProfile.Tables[dsProfile.DemographicPractice.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsProfile.Tables[dsProfile.DemographicPractice.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "chkScan", MDVUtility.ToStr(dr[dsProfile.DemographicPractice.ScanColumn.ColumnName])},
                            { "chkOCR", MDVUtility.ToStr(dr[dsProfile.DemographicPractice.OCRColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PracticeFill_JSON = js.Serialize(keyValues)
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
                            Message = objPractice.Message,
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
        /// Fills the basic fee group.
        /// </summary>
        /// <param name="EntityID">The entity identifier.</param>
        /// <returns>System.String.</returns>
        private string FillBasicFeeGroup(string EntityID)
        {
            try
            {
                DSFeeSchedule dsBasicFeeGroup = null;
                BLObject<DSFeeSchedule> obj;
                obj = BLLFeeScheduleObj.LoadBasicFeeGroup(0, null, null, EntityID, null);

                dsBasicFeeGroup = obj.Data;
                var response = new
                {
                    status = true,
                    BasicFeeGroupCount = dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows.Count,
                    BasicFeeGroupLoad_JSON = MDVUtility.JSON_DataTable(dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Fills the fee group.
        /// </summary>
        /// <param name="EntityID">The entity identifier.</param>
        /// <returns>System.String.</returns>
        private string FillFeeGroup(string EntityID)
        {
            try
            {
                DSFeeSchedule dsFeeGroup = null;
                BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadFeeGroup(0, null, null, EntityID, null);
                dsFeeGroup = obj.Data;
                var response = new
                {
                    status = true,
                    FeeGroupCount = dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName].Rows.Count,
                    FeeGroupLoad_JSON = MDVUtility.JSON_DataTable(dsFeeGroup.Tables[dsFeeGroup.FeeGroup.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Deletes practice by practice Id.
        /// </summary>
        /// <param name="PracticeID">The practice identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeletePractice(Int64 PracticeID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Practice", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PracticeID)))
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
                        BLObject<string> objPractice = BLLAdminProfileObj.DeletePractice(MDVUtility.ToStr(PracticeID));

                        if (objPractice.Data == "")
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
                                Message = objPractice.Data
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
        /// Updates the practice is active.
        /// </summary>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdatePracticeIsActive(Int64 PracticeId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Practice", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSProfile dsProfile = null;
                    BLObject<DSProfile> obj = BLLAdminProfileObj.LoadPractice(PracticeId, null, null, null, null, null);
                    dsProfile = obj.Data;
                    if (dsProfile.Tables[dsProfile.Practice.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsProfile.Tables[dsProfile.Practice.TableName].Rows[0];
                        dr[dsProfile.Practice.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSProfile> objPractice = BLLAdminProfileObj.UpdatePractice(ref dsProfile);
                        string successMsg;
                        if (objPractice.Data != null)
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
                                Message = objPractice.Message
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
        /// Handle the Practice Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_PRACTICE":
                    {
                        string fieldsJSON = context.Request["PracticeData"];
                        string strJSONData = SavePractice(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PRACTICE":
                    {
                        string strPracticeId = context.Request["PracticeID"];
                        string strJSONData = FillPractice(MDVUtility.ToInt64(strPracticeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DEMOGRAPHIC_PRACTICE":
                    {
                        string strPracticeId = context.Request["PracticeID"];
                        string strJSONData = demographicPractice(MDVUtility.ToInt64(strPracticeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_BASIC_FEE_GROUP":
                    {
                        string strEntityId = context.Request["EntityID"];
                        string strJSONData = FillBasicFeeGroup(strEntityId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_FEE_GROUP":
                    {
                        string strEntityId = context.Request["EntityID"];
                        string strJSONData = FillFeeGroup(strEntityId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PRACTICE":
                    {
                        string strPracticeId = context.Request["PracticeID"];
                        string strJSONData = DeletePractice(MDVUtility.ToInt64(strPracticeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PRACTICE":
                    {
                        string fieldsJSON = context.Request["PracticeData"];
                        Int64 PracticeID = MDVUtility.ToInt64(context.Request["PracticeID"]);
                        string strJSONData = UpdatePractice(fieldsJSON, PracticeID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PRACTICE_ACTIVE_INACTIVE":
                    {
                        Int64 PracticeID = MDVUtility.ToInt64(context.Request["PracticeID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdatePracticeIsActive(PracticeID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}