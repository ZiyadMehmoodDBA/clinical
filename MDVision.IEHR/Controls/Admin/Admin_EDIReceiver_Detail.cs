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
    public class Admin_EDIReceiver_Detail
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDIReceiver_Detail()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDIReceiver_Detail _obj = null;
        public static Admin_EDIReceiver_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDIReceiver_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the edi receiver setup.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveEDIReceiverSetup(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Receiver", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    DSEDI.EDIReceiverSetupRow dr = dsEDI.EDIReceiverSetup.NewEDIReceiverSetupRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"].Trim();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EnitityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBillingToProvType"]))
                        dr.RS2000APRV01 = MDVUtility.ToStr(SearchedfieldsJSON["ddlBillingToProvType"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearingHouse"]))
                        dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearingHouse"]);
                    dr.SubmitterID = SearchedfieldsJSON["txtSubmitterID"];
                    dr.BatchType = SearchedfieldsJSON["txtBatchType"];
                    dr.ReceiverNSFName = SearchedfieldsJSON["txtReceiverNSFName"];
                    dr.VersioncodeNational = SearchedfieldsJSON["txtVersionCodeNational"];
                    dr.SoftwareIssuerId = SearchedfieldsJSON["txtSoftwareIssuerID"];
                    dr.OrgSubId = SearchedfieldsJSON["txtOrgSubID"];
                    dr.VendorSoftVersion = SearchedfieldsJSON["txtVendorSoftVersion"];
                    dr.ValidationXML = SearchedfieldsJSON["txtValidationXML"];
                    dr.BatchbyBillingNPI = MDVUtility.ToStr(SearchedfieldsJSON["chkBatchBillingNPI"]) == "True" ? true : false;
                    dr.ANSI5010 = MDVUtility.ToStr(SearchedfieldsJSON["chkANSI5010"]) == "True" ? true : false;
                    dr.RS2010AAREFG5 = MDVUtility.ToStr(SearchedfieldsJSON["chkSendSiteID"]) == "True" ? true : false;
                    dr.ReceiverCode = SearchedfieldsJSON["txtReceiverCode"];
                    dr.TestProductionIndicator = SearchedfieldsJSON["txtTestProductionIndicator"];
                    dr.ReceiverId = SearchedfieldsJSON["txtReceiverID"];
                    dr.ReceiverTypecode = SearchedfieldsJSON["txtReceiverTypeCode"];
                    dr.VersionCodeLocal = SearchedfieldsJSON["txtVersionCodeLocal"];
                    dr.BHT02Transaction = SearchedfieldsJSON["ddlTranSetPurposeCode"];
                    dr.VendorAppCategory = SearchedfieldsJSON["txtVendorAppCategory"];
                    dr.VendorSoftUpdate = SearchedfieldsJSON["txtVendorSoftUpdate"];
                    dr.Path = SearchedfieldsJSON["txtPath"];
                    dr.IsPrimary = MDVUtility.ToStr(SearchedfieldsJSON["chkIsPrimary"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsEDI.EDIReceiverSetup.AddEDIReceiverSetupRow(dr);
                    BLObject<DSEDI> objEDIReceiverSetup = BLLAdminEDIObj.InsertReceiverSetup(dsEDI);
                    dsEDI = objEDIReceiverSetup.Data;
                    if (objEDIReceiverSetup.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            EDIReceiverSetupId = dsEDI.Tables[dsEDI.EDIReceiverSetup.TableName].Rows[0][dsEDI.EDIReceiverSetup.EDIReceiverSetupIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objEDIReceiverSetup.Message
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
        /// Updates the edi receiver setup.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EDIReceiverSetupId">The edi receiver setup identifier.</param>
        /// <returns></returns>
        private string UpdateEDIReceiverSetup(string fieldsJSON, Int64 EDIReceiverSetupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Receiver", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    //  DSEDI.EDIReceiverSetupRow dr = dsEDI.EDIReceiverSetup.NewEDIReceiverSetupRow();
                    BLObject<DSEDI> objLoad = BLLAdminEDIObj.LoadReceiverSetup(EDIReceiverSetupId, null, null, null, null, null);
                    dsEDI = objLoad.Data;
                    foreach (DSEDI.EDIReceiverSetupRow dr in dsEDI.Tables[dsEDI.EDIReceiverSetup.TableName].Rows)
                    {
                        //  dr.EDIReceiverSetupId = EDIReceiverSetupId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"].Trim();
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EnitityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBillingToProvType_text"]))
                            dr.RS2000APRV01 = MDVUtility.ToStr(SearchedfieldsJSON["ddlBillingToProvType_text"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearingHouse"]))
                            dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearingHouse"]);
                        dr.SubmitterID = SearchedfieldsJSON["txtSubmitterID"];
                        dr.BatchType = SearchedfieldsJSON["txtBatchType"];
                        dr.ReceiverNSFName = SearchedfieldsJSON["txtReceiverNSFName"];
                        dr.VersioncodeNational = SearchedfieldsJSON["txtVersionCodeNational"];
                        dr.SoftwareIssuerId = SearchedfieldsJSON["txtSoftwareIssuerID"];
                        dr.OrgSubId = SearchedfieldsJSON["txtOrgSubID"];
                        dr.VendorSoftVersion = SearchedfieldsJSON["txtVendorSoftVersion"];
                        dr.ValidationXML = SearchedfieldsJSON["txtValidationXML"];
                        dr.BatchbyBillingNPI = MDVUtility.ToStr(SearchedfieldsJSON["chkBatchBillingNPI"]) == "True" ? true : false;
                        dr.ANSI5010 = MDVUtility.ToStr(SearchedfieldsJSON["chkANSI5010"]) == "True" ? true : false;
                        dr.RS2010AAREFG5 = MDVUtility.ToStr(SearchedfieldsJSON["chkSendSiteID"]) == "True" ? true : false;
                        dr.ReceiverCode = SearchedfieldsJSON["txtReceiverCode"];
                        dr.TestProductionIndicator = SearchedfieldsJSON["txtTestProductionIndicator"];
                        dr.ReceiverId = SearchedfieldsJSON["txtReceiverID"];
                        dr.ReceiverTypecode = SearchedfieldsJSON["txtReceiverTypeCode"];
                        dr.VersionCodeLocal = SearchedfieldsJSON["txtVersionCodeLocal"];
                        dr.BHT02Transaction = SearchedfieldsJSON["ddlTranSetPurposeCode"];
                        dr.VendorAppCategory = SearchedfieldsJSON["txtVendorAppCategory"];
                        dr.VendorSoftUpdate = SearchedfieldsJSON["txtVendorSoftUpdate"];
                        dr.Path = SearchedfieldsJSON["txtPath"];
                        dr.IsPrimary = MDVUtility.ToStr(SearchedfieldsJSON["chkIsPrimary"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        #region Database Updation
                        // dsEDI.EDIReceiverSetup.AddEDIReceiverSetupRow(dr);
                        // dsEDI.EDIReceiverSetup.AcceptChanges();
                    }

                    if (dsEDI.Tables[dsEDI.EDIReceiverSetup.TableName].Rows.Count > 0)
                    {
                        // dsEDI.EDIReceiverSetup.Rows[0].SetModified();
                        BLObject<DSEDI> objEDIReceiverSetup = BLLAdminEDIObj.UpdateReceiverSetup(dsEDI);
                        dsEDI = objEDIReceiverSetup.Data;
                        if (objEDIReceiverSetup.Data != null)
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
                                Message = objEDIReceiverSetup.Message
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
        /// Saves the edi receiver setup X12.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EDIReceiverID">The edi receiver identifier.</param>
        /// <returns></returns>
        private string SaveEDIReceiverSetupX12(string fieldsJSON, string EDIReceiverID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Receiver", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    DSEDI.EDIReceiverX12SetupRow dr = dsEDI.EDIReceiverX12Setup.NewEDIReceiverX12SetupRow();

                    dr.EDIReceiverSetupId = MDVUtility.ToInt64(EDIReceiverID);
                    dr.ISA01 = SearchedfieldsJSON["ddlAuthInfoQual"];
                    dr.ISA02 = MDVUtility.ConvertHashIntoSpace(SearchedfieldsJSON["txtAuthInfo"]);
                    dr.ISA03 = SearchedfieldsJSON["ddlSecurityInfoQual"];
                    dr.Securityinfo = MDVUtility.ConvertHashIntoSpace(SearchedfieldsJSON["txtSecurityInfo"]);
                    dr.ISA05 = SearchedfieldsJSON["ddlInterSenderIDQual"];
                    dr.InterchangeSenderId = SearchedfieldsJSON["txtInterSenderID"];
                    dr.ISA07 = SearchedfieldsJSON["ddlInterReceiverIDQual"];
                    dr.ISA08 = SearchedfieldsJSON["txtInterReceiverID"];
                    dr.ISA11 = SearchedfieldsJSON["txtRepetitionSeparator"];
                    dr.ISA12 = SearchedfieldsJSON["txtInterCtrlVerNum"];
                    dr.ISA13 = SearchedfieldsJSON["txtInterchangeControlNumber"];
                    dr.ISA14 = SearchedfieldsJSON["ddlAcknowledgementRequested"];
                    dr.ISA15 = SearchedfieldsJSON["ddlUsageIndicator"];
                    dr.ISA16 = SearchedfieldsJSON["txtCompElemSeparator"];
                    dr.GS02 = SearchedfieldsJSON["txtAppSenderCode"];
                    dr.GS03 = SearchedfieldsJSON["txtAppReceiverCode"];
                    dr.GS06 = SearchedfieldsJSON["txtGroupControlNum"];
                    dr.GS08 = SearchedfieldsJSON["txtVersionReleaseID"];
                    dr.BHT02 = SearchedfieldsJSON["ddlTransactionSetupPurposeCode"];
                    dr.BHT03 = SearchedfieldsJSON["txtReferenceID"];
                    dr.BHT06 = SearchedfieldsJSON["ddlTranTypeCode"];
                    dr.ST02Transaction = SearchedfieldsJSON["txtTranSetupCtrlNum"];
                    dr.ST03 = SearchedfieldsJSON["txtImplementationConventionRef"];
                    dr.TNSHDLRClass = SearchedfieldsJSON["txtTNSHDLRClass"];
                    dr.ReceiverName = SearchedfieldsJSON["txtReceiverName"];
                    dr.FileCounter = SearchedfieldsJSON["txtFileCounter"];
                    dr.RX1000BNM109 = SearchedfieldsJSON["txtRecPrimaryIDNum"];
                    dr.ToDay = SearchedfieldsJSON["txtToDay"];
                    dr.ONEISA = MDVUtility.ToStr(SearchedfieldsJSON["chkISAOneFile"]) == "True" ? true : false;
                    dr.SegmentTerminator = SearchedfieldsJSON["txtSegmentTerminator"];
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.IsActive = true;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsEDI.EDIReceiverX12Setup.AddEDIReceiverX12SetupRow(dr);
                    BLObject<DSEDI> objEDIReceiverSetup = BLLAdminEDIObj.InsertReceiverSetupX12(dsEDI);
                    dsEDI = objEDIReceiverSetup.Data;
                    if (objEDIReceiverSetup.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            EDIReceiverX12SetupId = dsEDI.Tables[dsEDI.EDIReceiverX12Setup.TableName].Rows[0][dsEDI.EDIReceiverX12Setup.EDIReceiverX12SetupIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objEDIReceiverSetup.Message
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
        /// Updates the edi receiver setup X12.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EDIReceiverID">The edi receiver identifier.</param>
        /// <param name="EDIReceiverX12Id">The edi receiver X12 identifier.</param>
        /// <returns></returns>
        private string UpdateEDIReceiverSetupX12(string fieldsJSON, string EDIReceiverID, Int64 EDIReceiverX12Id)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Receiver", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    // DSEDI.EDIReceiverX12SetupRow dr = dsEDI.EDIReceiverX12Setup.NewEDIReceiverX12SetupRow();
                    BLObject<DSEDI> objLoad = BLLAdminEDIObj.LoadReceiverSetupX12(MDVUtility.ToInt64(EDIReceiverID));
                    dsEDI = objLoad.Data;
                    foreach (DSEDI.EDIReceiverX12SetupRow dr in dsEDI.Tables[dsEDI.EDIReceiverX12Setup.TableName].Rows)
                    {
                        //dr.EDIReceiverX12SetupId = EDIReceiverX12Id;
                        dr.EDIReceiverSetupId = MDVUtility.ToInt64(EDIReceiverID);
                        dr.ISA01 = SearchedfieldsJSON["ddlAuthInfoQual"];
                        dr.ISA02 = MDVUtility.ConvertHashIntoSpace(SearchedfieldsJSON["txtAuthInfo"]);
                        dr.ISA03 = SearchedfieldsJSON["ddlSecurityInfoQual"];
                        dr.Securityinfo = MDVUtility.ConvertHashIntoSpace(SearchedfieldsJSON["txtSecurityInfo"]);
                        dr.ISA05 = SearchedfieldsJSON["ddlInterSenderIDQual"];
                        dr.InterchangeSenderId = SearchedfieldsJSON["txtInterSenderID"];
                        dr.ISA07 = SearchedfieldsJSON["ddlInterReceiverIDQual"];
                        dr.ISA08 = SearchedfieldsJSON["txtInterReceiverID"];
                        dr.ISA11 = SearchedfieldsJSON["txtRepetitionSeparator"];
                        dr.ISA12 = SearchedfieldsJSON["txtInterCtrlVerNum"];
                        dr.ISA13 = SearchedfieldsJSON["txtInterchangeControlNumber"];
                        dr.ISA14 = SearchedfieldsJSON["ddlAcknowledgementRequested"];
                        dr.ISA15 = SearchedfieldsJSON["ddlUsageIndicator"];
                        dr.ISA16 = SearchedfieldsJSON["txtCompElemSeparator"];
                        dr.GS02 = SearchedfieldsJSON["txtAppSenderCode"];
                        dr.GS03 = SearchedfieldsJSON["txtAppReceiverCode"];
                        dr.GS06 = SearchedfieldsJSON["txtGroupControlNum"];
                        dr.GS08 = SearchedfieldsJSON["txtVersionReleaseID"];
                        dr.BHT02 = SearchedfieldsJSON["ddlTransactionSetupPurposeCode"];
                        dr.BHT03 = SearchedfieldsJSON["txtReferenceID"];
                        dr.BHT06 = SearchedfieldsJSON["ddlTranTypeCode"];
                        dr.ST02Transaction = SearchedfieldsJSON["txtTranSetupCtrlNum"];
                        dr.ST03 = SearchedfieldsJSON["txtImplementationConventionRef"];
                        dr.TNSHDLRClass = SearchedfieldsJSON["txtTNSHDLRClass"];
                        dr.ReceiverName = SearchedfieldsJSON["txtReceiverName"];
                        dr.FileCounter = SearchedfieldsJSON["txtFileCounter"];
                        dr.RX1000BNM109 = SearchedfieldsJSON["txtRecPrimaryIDNum"];
                        dr.ToDay = SearchedfieldsJSON["txtToDay"];
                        dr.ONEISA = MDVUtility.ToStr(SearchedfieldsJSON["chkISAOneFile"]) == "True" ? true : false;
                        dr.SegmentTerminator = SearchedfieldsJSON["txtSegmentTerminator"];
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.IsActive = true;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        #region Database Updation
                        // dsEDI.EDIReceiverX12Setup.AddEDIReceiverX12SetupRow(dr);
                        // dsEDI.EDIReceiverX12Setup.AcceptChanges();
                    }
                    if (dsEDI.Tables[dsEDI.EDIReceiverX12Setup.TableName].Rows.Count > 0)
                    {
                        //dsEDI.EDIReceiverX12Setup.Rows[0].SetModified();
                        dsEDI.Tables[dsEDI.EDIReceiverX12Setup.TableName].Columns.Remove("ShortName");
                        BLObject<DSEDI> objEDIReceiverSetup = BLLAdminEDIObj.UpdateReceiverSetupX12(dsEDI);
                        dsEDI = objEDIReceiverSetup.Data;
                        if (objEDIReceiverSetup.Data != null)
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
                                Message = objEDIReceiverSetup.Message
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
        /// Fills the edi receiver setup.
        /// </summary>
        /// <param name="EDIReceiverSetupId">The edi receiver setup identifier.</param>
        /// <returns></returns>
        private string FillEDIReceiverSetup(Int64 EDIReceiverSetupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Receiver", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIReceiverSetupId)))
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
                        BLObject<DSEDI> objEDIReceiverSetup = BLLAdminEDIObj.LoadReceiverSetup(EDIReceiverSetupId, null, null, null, null, null);
                        if (objEDIReceiverSetup.Data != null)
                        {
                            dsEDI = objEDIReceiverSetup.Data;
                            if (dsEDI.Tables[dsEDI.EDIReceiverSetup.TableName].Rows.Count > 0)
                            {
                                DataRow drReceiver = dsEDI.Tables[dsEDI.EDIReceiverSetup.TableName].Rows[0];
                                var keyValuesReceiver = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.ShortNameColumn.ColumnName])},
                            { "txtSubmitterID", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.SubmitterIDColumn.ColumnName])},
                            { "txtBatchType", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.BatchTypeColumn.ColumnName])},
                            { "txtReceiverNSFName", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.ReceiverNSFNameColumn.ColumnName])},
                            { "txtVersionCodeNational", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.VersioncodeNationalColumn.ColumnName])},
                            { "txtSoftwareIssuerID", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.SoftwareIssuerIdColumn.ColumnName])},
                            { "txtOrgSubID", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.OrgSubIdColumn.ColumnName])},
                            { "txtVendorSoftVersion", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.VendorSoftVersionColumn.ColumnName])},
                            { "txtValidationXML", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.ValidationXMLColumn.ColumnName])},
                            { "ddlBillingToProvType", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.RS2000APRV01Column.ColumnName])},
                            { "ddlClearingHouse", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.ClearingHouseIdColumn.ColumnName])},
                            { "chkBatchBillingNPI", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.BatchbyBillingNPIColumn.ColumnName])},
                            { "chkANSI5010", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.ANSI5010Column.ColumnName])},
                            { "chkSendSiteID", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.RS2010AAREFG5Column.ColumnName])},
                            { "txtReceiverCode", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.ReceiverCodeColumn.ColumnName])},
                            { "txtTestProductionIndicator", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.TestProductionIndicatorColumn.ColumnName])},
                            { "txtReceiverID", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.ReceiverIdColumn.ColumnName])},
                            { "txtReceiverTypeCode", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.ReceiverTypecodeColumn.ColumnName])},
                            { "txtVersionCodeLocal", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.VersionCodeLocalColumn.ColumnName])},
                            { "ddlTranSetPurposeCode", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.BHT02TransactionColumn.ColumnName])},
                            { "txtVendorAppCategory", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.VendorAppCategoryColumn.ColumnName])},
                            { "txtVendorSoftUpdate", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.VendorSoftUpdateColumn.ColumnName])},
                            { "txtPath", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.PathColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.EnitityIdColumn.ColumnName])},
                            { "chkIsPrimary", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.IsPrimaryColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(drReceiver[dsEDI.EDIReceiverSetup.IsActiveColumn.ColumnName])}

                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                string ReceiverSerialized = js.Serialize(keyValuesReceiver);

                                string ReceiverX12Serialized = string.Empty;
                                string EDIReceiverX12SetupId = string.Empty;
                                if (dsEDI.Tables[dsEDI.EDIReceiverX12Setup.TableName].Rows.Count > 0)
                                {
                                    DataRow drReceiverX12 = dsEDI.Tables[dsEDI.EDIReceiverX12Setup.TableName].Rows[0];
                                    var keyValuesReceiverX12 = new Dictionary<string, string>
                            {
                                { "ddlAuthInfoQual", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA01Column.ColumnName])},
                                { "txtAuthInfo", MDVUtility.ConvertSpaceIntoHash(MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA02Column.ColumnName]))},
                                { "ddlSecurityInfoQual", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA03Column.ColumnName])},
                                { "txtSecurityInfo", MDVUtility.ConvertSpaceIntoHash(MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.SecurityinfoColumn.ColumnName]))},
                                { "ddlInterSenderIDQual", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA05Column.ColumnName])},
                                { "txtInterSenderID", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.InterchangeSenderIdColumn.ColumnName])},
                                { "ddlInterReceiverIDQual", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA07Column.ColumnName])},
                                { "txtInterReceiverID", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA08Column.ColumnName])},
                                { "txtRepetitionSeparator", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA11Column.ColumnName])},
                                { "txtInterCtrlVerNum", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA12Column.ColumnName])},
                                { "txtInterchangeControlNumber", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA13Column.ColumnName])},
                                { "ddlAcknowledgementRequested", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA14Column.ColumnName])},
                                { "ddlUsageIndicator", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA15Column.ColumnName])},
                                { "txtCompElemSeparator", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ISA16Column.ColumnName])},
                                { "txtAppSenderCode", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.GS02Column.ColumnName])},
                                { "txtAppReceiverCode", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.GS03Column.ColumnName])},
                                { "txtGroupControlNum", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.GS06Column.ColumnName])},
                                { "txtVersionReleaseID", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.GS08Column.ColumnName])},
                                { "ddlTransactionSetupPurposeCode", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.BHT02Column.ColumnName])},
                                { "txtReferenceID", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.BHT03Column.ColumnName])},
                                { "ddlTranTypeCode", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.BHT06Column.ColumnName])},
                                { "txtTranSetupCtrlNum", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ST02TransactionColumn.ColumnName])},
                                { "txtImplementationConventionRef", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ST03Column.ColumnName])},
                                { "txtTNSHDLRClass", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.TNSHDLRClassColumn.ColumnName])},
                                { "txtReceiverName", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ReceiverNameColumn.ColumnName])},
                                { "txtFileCounter", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.FileCounterColumn.ColumnName])},
                                { "txtRecPrimaryIDNum", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.RX1000BNM109Column.ColumnName])},
                                { "txtToDay", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ToDayColumn.ColumnName])},
                                { "chkISAOneFile", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.ONEISAColumn.ColumnName])},
                                { "txtSegmentTerminator", MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.SegmentTerminatorColumn.ColumnName])}
                            };
                                    EDIReceiverX12SetupId = MDVUtility.ToStr(drReceiverX12[dsEDI.EDIReceiverX12Setup.EDIReceiverX12SetupIdColumn.ColumnName]);
                                    ReceiverX12Serialized = js.Serialize(keyValuesReceiverX12);

                                }
                                var response = new
                                {
                                    status = true,
                                    EDIReceiverSetupFill_JSON = ReceiverSerialized,
                                    EDIReceiverX12SetupFill_JSON = ReceiverX12Serialized,
                                    EDIReceiverX12SetupId = EDIReceiverX12SetupId
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
                                Message = objEDIReceiverSetup.Message
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
        /// Deletes the edi receiver setup.
        /// </summary>
        /// <param name="EDIReceiverSetupId">The edi receiver setup identifier.</param>
        /// <returns></returns>
        private string DeleteEDIReceiverSetup(Int64 EDIReceiverSetupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Receiver", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIReceiverSetupId)))
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
                        BLObject<string> objEDIReceiverSetup = BLLAdminEDIObj.DeleteReceiverSetup(MDVUtility.ToStr(EDIReceiverSetupId));

                        if (objEDIReceiverSetup.Data == "")
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
                                Message = objEDIReceiverSetup.Data
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
        /// Updates the edi receiver setup is active.
        /// </summary>
        /// <param name="EDIReceiverSetupId">The edi receiver setup identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateEDIReceiverSetupIsActive(Int64 EDIReceiverSetupId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Receiver", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSEDI dsEDI = null;
                    BLObject<DSEDI> obj = BLLAdminEDIObj.LoadReceiverSetup(EDIReceiverSetupId, null, null, null, null, null);
                    dsEDI = obj.Data;
                    if (dsEDI.Tables[dsEDI.EDIReceiverSetup.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsEDI.Tables[dsEDI.EDIReceiverSetup.TableName].Rows[0];
                        dr[dsEDI.EDIReceiverSetup.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSEDI> objEDIReceiverSetup = BLLAdminEDIObj.UpdateReceiverSetup(dsEDI);
                        string successMsg;
                        if (objEDIReceiverSetup.Data != null)
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
                                Message = objEDIReceiverSetup.Message
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
        /// Handle the EDI Receiver Setup Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_RECEIVER_SETUP":
                    {
                        string fieldsJSON = context.Request["EDIReceiverData"];
                        string strJSONData = SaveEDIReceiverSetup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_RECEIVER_SETUP_X12":
                    {
                        string fieldsJSON = context.Request["EDIReceiverData"];
                        string EDIReceiverID = context.Request["EDIReceiverID"];
                        string strJSONData = SaveEDIReceiverSetupX12(fieldsJSON, EDIReceiverID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_RECEIVER_SETUP":
                    {
                        string EDIReceiverID = context.Request["EDIReceiverID"];
                        string strJSONData = FillEDIReceiverSetup(MDVUtility.ToInt64(EDIReceiverID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_RECEIVER_SETUP":
                    {
                        string ReceiverSetupID = context.Request["ReceiverSetupID"];
                        string strJSONData = DeleteEDIReceiverSetup(MDVUtility.ToInt64(ReceiverSetupID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_RECEIVER_SETUP":
                    {
                        string fieldsJSON = context.Request["EDIReceiverData"];
                        Int64 EDIReceiverSetupID = MDVUtility.ToInt64(context.Request["EDIReceiverID"]);
                        string strJSONData = UpdateEDIReceiverSetup(fieldsJSON, EDIReceiverSetupID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_RECEIVER_SETUP_X12":
                    {
                        string fieldsJSON = context.Request["EDIReceiverData"];
                        string EDIReceiverID = MDVUtility.ToStr(context.Request["EDIReceiverID"]);
                        Int64 EDIReceiverX12ID = MDVUtility.ToInt64(context.Request["EDIReceiverX12ID"]);
                        string strJSONData = UpdateEDIReceiverSetupX12(fieldsJSON, EDIReceiverID, EDIReceiverX12ID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_RECEIVER_SETUP_ACTIVE_INACTIVE":
                    {
                        Int64 EDIReceiverSetupID = MDVUtility.ToInt64(context.Request["EDIReceiverID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateEDIReceiverSetupIsActive(EDIReceiverSetupID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}