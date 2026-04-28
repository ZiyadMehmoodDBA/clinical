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
using MDVision.Model.Admin.Codes;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_CPTCode_Detail
    {


        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_CPTCode_Detail()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_CPTCode_Detail _obj = null;
        public static Admin_CPTCode_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_CPTCode_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the CPT code.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveCPTCode(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CPT", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCPTCode = new DSCodes();
                    DSCodes.CPTCodeRow dr = dsCPTCode.CPTCode.NewCPTCodeRow();

                    dr.CPTCode = SearchedfieldsJSON["txtCPTCode"];
                    //dr.ShortName = SearchedfieldsJSON["txtShortName"].Trim();
                    dr.ShortName = "";
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.BasicUnits = MDVUtility.ToDouble(SearchedfieldsJSON["txtBasicUnits"]);
                    dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtFee"]);
                    dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtExpectedFee"]);
                    dr.DurationPerUnit = MDVUtility.ToDouble(SearchedfieldsJSON["txtDurationPerUnit"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstTypeOfService"]))
                        dr.TOSId = MDVUtility
                            .ToInt64(SearchedfieldsJSON["lstTypeOfService"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstSpecialtyId"]))
                        dr.SpecialtyId = MDVUtility.ToInt64(SearchedfieldsJSON["lstSpecialtyId"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstProcedureCategoryId"]))
                        dr.ProcedureCategoryId = MDVUtility.ToInt64(SearchedfieldsJSON["lstProcedureCategoryId"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstPOScategory"]))
                        dr.POScategory = MDVUtility.ToInt64(SearchedfieldsJSON["lstPOScategory"]);

                    dr.Electronic = MDVUtility.ToStr(SearchedfieldsJSON["chkElectronic"]) == "True" ? true : false;
                    dr.CLIA = SearchedfieldsJSON["txtCLIA"];
                    dr.GlobalPeriodDays = SearchedfieldsJSON["txtGlobalPeriodDays"];
                    dr.ReferralRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkReferralRequired"]) == "True" ? true : false;
                    dr.ActivelyUsed = MDVUtility.ToStr(SearchedfieldsJSON["chkActivelyUsed"]) == "True" ? true : false;
                    dr.Discontinued = MDVUtility.ToStr(SearchedfieldsJSON["chkDiscontinued"]) == "True" ? true : false;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtStartDate"]))
                        dr.StartDate = MDVUtility.ToDateTime(SearchedfieldsJSON["txtStartDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtEndDate"]))
                        dr.EndDate = MDVUtility.ToDateTime(SearchedfieldsJSON["txtEndDate"]);
                    //dr.RevenueCodeId = 1;
                    dr.ServiceDescription = SearchedfieldsJSON["txtServiceDescription"];
                    //dr.CommentsForClaim = "";
                    dr.PrintOptions = MDVUtility.ToStr(SearchedfieldsJSON["chkPrintOptions"]) == "True" ? true : false;
                    dr.IsActive = true;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                    //    dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfLexiCode"]))
                        dr.LexiCode = SearchedfieldsJSON["hfLexiCode"];

                    #region Database Insertion
                    dsCPTCode.CPTCode.AddCPTCodeRow(dr);
                    BLObject<DSCodes> obj = BLLAdminCodesObj.InsertCPTCode(ref dsCPTCode);
                    dsCPTCode = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            CPTCodeId = dsCPTCode.Tables[dsCPTCode.CPTCode.TableName].Rows[0][dsCPTCode.CPTCode.CPTCodeIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        string message = obj.Message;
                        if (message.Contains("UNIQUE KEY"))
                            message = "Cannot insert duplicate CPTCode.";

                        var response = new
                        {
                            status = false,
                            Message = message
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
        public string SaveCPTCodeFromFillVisit(string cptCode, string cptDescription)
        {
            try
            {
                //Only mondatory and default values is filled
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSCodes dsCPTCode = new DSCodes();
                DSCodes.CPTCodeRow dr = dsCPTCode.CPTCode.NewCPTCodeRow();

                dr.CPTCode = cptCode;
                dr.ShortName = "";
                dr.Description = cptDescription;
                dr.BasicUnits = 1;
                dr.ReferralRequired = false;
                dr.ActivelyUsed = false;
                dr.Discontinued = false;
                dr.PrintOptions = false;
                dr.Electronic = true;
                dr.CLIA = true;
                dr.IsActive = true;

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsCPTCode.CPTCode.AddCPTCodeRow(dr);
                BLObject<DSCodes> obj = BLLAdminCodesObj.InsertCPTCode(ref dsCPTCode);
                dsCPTCode = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        CPTCodeId = dsCPTCode.Tables[dsCPTCode.CPTCode.TableName].Rows[0][dsCPTCode.CPTCode.CPTCodeIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    string message = obj.Message;
                    if (message.Contains("UNIQUE KEY"))
                        message = "Cannot insert duplicate CPTCode.";

                    var response = new
                    {
                        status = false,
                        Message = message
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
        /// Updates the CPT code.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <returns></returns>
        private string UpdateCPTCode(string fieldsJSON, Int64 CPTCodeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CPT", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCPTCode = new DSCodes();
                    //DSCodes.CPTCodeRow dr = dsCPTCode.CPTCode.NewCPTCodeRow();
                    BLObject<DSCodes> objLoad = BLLAdminCodesObj.LoadCPTCode(CPTCodeId, null, null, null, null, null, null, null, null, 1, 5000);
                    dsCPTCode = objLoad.Data;
                    foreach (DSCodes.CPTCodeRow dr in dsCPTCode.Tables[dsCPTCode.CPTCode.TableName].Rows)
                    {
                        //dr.CPTCodeId = CPTCodeId;
                        dr.CPTCode = SearchedfieldsJSON["txtCPTCode"];
                        //dr.ShortName = SearchedfieldsJSON["txtShortName"].Trim();
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.BasicUnits = MDVUtility.ToDouble(SearchedfieldsJSON["txtBasicUnits"]);
                        dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtFee"]);
                        dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtExpectedFee"]);
                        dr.DurationPerUnit = MDVUtility.ToDouble(SearchedfieldsJSON["txtDurationPerUnit"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstTypeOfService"]))
                            dr.TOSId = MDVUtility.ToInt64(SearchedfieldsJSON["lstTypeOfService"]);
                        else
                            dr.TOSId = MDVUtility.ToInt64(0);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstSpecialtyId"]))
                            dr.SpecialtyId = MDVUtility.ToInt64(SearchedfieldsJSON["lstSpecialtyId"]);
                        else
                            dr.SpecialtyId = MDVUtility.ToInt64(0);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstProcedureCategoryId"]))
                            dr.ProcedureCategoryId = MDVUtility.ToInt64(SearchedfieldsJSON["lstProcedureCategoryId"]);
                        else
                            dr.ProcedureCategoryId = MDVUtility.ToInt64(0);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstPOScategory"]))
                            dr.POScategory = MDVUtility.ToInt64(SearchedfieldsJSON["lstPOScategory"]);
                        else
                            dr.POScategory = MDVUtility.ToInt64(0);
                        dr.Electronic = MDVUtility.ToStr(SearchedfieldsJSON["chkElectronic"]) == "True" ? true : false;
                        dr.CLIA = SearchedfieldsJSON["txtCLIA"];
                        dr.GlobalPeriodDays = SearchedfieldsJSON["txtGlobalPeriodDays"];
                        dr.ReferralRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkReferralRequired"]) == "True" ? true : false;
                        dr.ActivelyUsed = MDVUtility.ToStr(SearchedfieldsJSON["chkActivelyUsed"]) == "True" ? true : false;
                        dr.Discontinued = MDVUtility.ToStr(SearchedfieldsJSON["chkDiscontinued"]) == "True" ? true : false;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtStartDate"]))
                            dr.StartDate = MDVUtility.ToDateTime(SearchedfieldsJSON["txtStartDate"]);
                        else
                            dr[dsCPTCode.CPTCode.StartDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtEndDate"]))
                            dr.EndDate = MDVUtility.ToDateTime(SearchedfieldsJSON["txtEndDate"]);
                        else
                            dr[dsCPTCode.CPTCode.EndDateColumn] = DBNull.Value;

                        //dr.RevenueCodeId = 1;
                        dr.ServiceDescription = SearchedfieldsJSON["txtServiceDescription"];
                        //dr.CommentsForClaim = "";
                        dr.PrintOptions = MDVUtility.ToStr(SearchedfieldsJSON["chkPrintOptions"]) == "True" ? true : false;
                        dr.IsActive = true;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        //dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfLexiCode"]))
                            dr.LexiCode = SearchedfieldsJSON["hfLexiCode"];
                    }

                    #region Database Updation
                    //dsCPTCode.CPTCode.AddCPTCodeRow(dr);
                    //dsCPTCode.CPTCode.AcceptChanges();

                    if (dsCPTCode.Tables[dsCPTCode.CPTCode.TableName].Rows.Count > 0)
                    {
                        //dsCPTCode.CPTCode.Rows[0].SetModified();
                        //DALSpecialty.Instance.UpdateSpecialty(ref dsCodes);
                        BLObject<DSCodes> obj = BLLAdminCodesObj.UpdateCPTCode(ref dsCPTCode);
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
                            string message = obj.Message;
                            if (message.Contains("UNIQUE KEY"))
                                message = "Cannot insert duplicate CPTCode.";

                            var response = new
                            {
                                status = false,
                                Message = message
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
        /// Fills the CPT code.
        /// </summary>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <returns></returns>
        private string FillCPTCode(Int64 CPTCodeId, Int64 pageNo, Int64 rpp)
        {
            string Format = "{0:0.00}";
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CPT", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(CPTCodeId)))
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
                        DSCodes dsCPTCode = null;
                        BLObject<DSCodes> obj = BLLAdminCodesObj.LoadCPTCode(CPTCodeId, null, null, null, null, null, null, null, null, pageNo, rpp);
                        if (obj.Data != null)
                        {
                            dsCPTCode = obj.Data;
                            if (dsCPTCode.Tables[dsCPTCode.CPTCode.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsCPTCode.Tables[dsCPTCode.CPTCode.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                         { "txtCPTCode", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.CPTCodeColumn.ColumnName])},
                         //{ "txtShortName", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.ShortNameColumn.ColumnName])},
                         { "txtDescription", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.DescriptionColumn.ColumnName])},
                         { "txtBasicUnits", String.Format(Format,dr[dsCPTCode.CPTCode.BasicUnitsColumn.ColumnName])},
                         { "txtFee", String.Format(Format,dr[dsCPTCode.CPTCode.FeeColumn.ColumnName])},
                         { "txtExpectedFee", String.Format(Format,dr[dsCPTCode.CPTCode.ExpectedFeeColumn.ColumnName])},
                         { "txtDurationPerUnit", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.DurationPerUnitColumn.ColumnName])},
                         { "lstTypeOfService", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.TOSIdColumn.ColumnName])},
                         { "lstSpecialtyId", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.SpecialtyIdColumn.ColumnName])},
                         { "lstProcedureCategoryId", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.ProcedureCategoryIdColumn.ColumnName])},
                         { "lstPOScategory", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.POScategoryColumn.ColumnName])},
                         { "chkElectronic", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.ElectronicColumn.ColumnName])},
                         { "txtCLIA", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.CLIAColumn.ColumnName])},
                         { "txtGlobalPeriodDays", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.GlobalPeriodDaysColumn.ColumnName])},
                         { "chkReferralRequired", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.ReferralRequiredColumn.ColumnName])},
                         { "chkActivelyUsed", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.ActivelyUsedColumn.ColumnName])},
                         { "chkDiscontinued", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.DiscontinuedColumn.ColumnName])},
                         //{ "txtStartDate", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.StartDateColumn.ColumnName])},
                         //{ "txtEndDate", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.EndDateColumn.ColumnName])},
                         { "txtStartDate", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.StartDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsCPTCode.CPTCode.StartDateColumn.ColumnName]).ToShortDateString():""},
                         { "txtEndDate", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.EndDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsCPTCode.CPTCode.EndDateColumn.ColumnName]).ToShortDateString():""},
                         { "txtNDC", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.NDCColumn.ColumnName])},
                         { "lstNDCMeasurementId", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.NDCMeasurementIdColumn.ColumnName])},
                         { "txtNDCUnitPrice", String.Format(Format,dr[dsCPTCode.CPTCode.NDCUnitPriceColumn.ColumnName])},
                         { "txtServiceDescription", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.ServiceDescriptionColumn.ColumnName])},
                         { "chkPrintOptions", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.PrintOptionsColumn.ColumnName])},
                         //{ "ddlEntity", MDVUtility.ToStr(dr[dsCPTCode.CPTCode.EntityIdColumn.ColumnName])},
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    CPTCodeFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the CPT code.
        /// </summary>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <returns></returns>
        private string DeleteCPTCode(Int64 CPTCodeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CPT", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(CPTCodeId)))
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
                        BLObject<string> obj = BLLAdminCodesObj.DeleteCPTCode(MDVUtility.ToStr(CPTCodeId));
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
        /// Updates the CPT code is active.
        /// </summary>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateCPTCodeIsActive(Int64 CPTCodeId, Int64 IsActive, Int64 pageNo, Int64 rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CPT", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSCodes dsCode = null;
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadCPTCode(CPTCodeId, null, null, null, null, null, null, null, null, pageNo, rpp);
                    dsCode = obj.Data;
                    if (dsCode.Tables[dsCode.CPTCode.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCode.Tables[dsCode.CPTCode.TableName].Rows[0];
                        dr[dsCode.CPTCode.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSCodes> objUser = BLLAdminCodesObj.UpdateCPTCode(ref dsCode);
                        string successMsg;
                        if (objUser.Data != null)
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
                                Message = objUser.Message
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

        #region CPT Plan Info
        /// <summary>
        /// Saves the CPT plan information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <returns></returns>
        private string SaveCPTPlanInfo(string fieldsJSON, Int64 CPTCodeId, string RowId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CPT", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    DSCodes.CPTPlanRow dr = dsCodes.CPTPlan.NewCPTPlanRow();


                    long InPlanId = 0;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPlan" + RowId]))
                    {
                        InPlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPlan" + RowId]);
                    }

                    if (InPlanId > 0)
                    {
                        dr.CPTCodeId = CPTCodeId;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCode" + RowId]))
                            dr.CPTPlanCode = MDVUtility.ToStr(SearchedfieldsJSON["txtCode" + RowId]);

                        dr.InsurancePlanId = InPlanId;

                        #region Database Insertion
                        dsCodes.CPTPlan.AddCPTPlanRow(dr);
                        BLObject<DSCodes> obj = BLLAdminCodesObj.InsertCPTPlanInfo(dsCodes);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                CPTPlanInfoId = dsCodes.Tables[dsCodes.CPTPlan.TableName].Rows[0][dsCodes.CPTPlan.CPTPlanIdColumn.ColumnName]
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
                            Message = "Invalid Plan.",
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
                    return JsonConvert.SerializeObject(response);
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
        private string SaveNDCInfo(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CPT", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    CPTNdcModel model = ser.Deserialize<CPTNdcModel>(fieldsJSON);
                    model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                    model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                    string NDCId = BLLAdminCodesObj.SaveNDCInfo(model);
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        NDCId = NDCId
                    };
                    return (JsonConvert.SerializeObject(response));
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

        private string UpdateNDCInfo(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CPT", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    CPTNdcModel model = ser.Deserialize<CPTNdcModel>(fieldsJSON);

                    model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                    string NDCId = BLLAdminCodesObj.UpdateNDCInfo(model);
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message
                    };
                    return (JsonConvert.SerializeObject(response));
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
        /// Updates the CPT plan information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <param name="CPTPlanId">The CPT plan identifier.</param>
        /// <returns></returns>
        private string UpdateCPTPlanInfo(string fieldsJSON, Int64 CPTPlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CPT", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    BLObject<DSCodes> objCode;
                    DSCodes dsCode = new DSCodes();
                    objCode = BLLAdminCodesObj.LoadCPTPlanInfo(0, CPTPlanId);
                    if (objCode.Data != null)
                    {
                        foreach (DSCodes.CPTPlanRow dr in objCode.Data.Tables[dsCode.CPTPlan.TableName].Rows)
                        {
                            long InPlanId = 0;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPlan" + CPTPlanId]))
                            {
                                InPlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPlan" + CPTPlanId]);
                            }
                            if (InPlanId > 0)
                            {
                                dr.InsurancePlanId = InPlanId;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCode" + CPTPlanId]))
                                dr.CPTPlanCode = MDVUtility.ToStr(SearchedfieldsJSON["txtCode" + CPTPlanId]);
                        }

                        dsCode = objCode.Data;

                        #region Database Update

                        BLObject<DSCodes> obj = BLLAdminCodesObj.UpdateCPTPlanInfo(dsCode);

                        if (obj.Data != null)
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
                            Message = ""
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

        /// <summary>
        /// Loads the CPT plan information.
        /// </summary>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <returns></returns>
        private string LoadCPTPlanInfo(Int64 CPTCodeId)
        {
            try
            {
                DSCodes dsCodes = null;
                BLObject<DSCodes> obj;
                if (CPTCodeId > 0)
                {
                    obj = BLLAdminCodesObj.LoadCPTPlanInfo(CPTCodeId, 0);
                    List<CPTNdcModel> NDCList = null;
                    BLObject<List<CPTNdcModel>> obj1;

                    obj1 = BLLAdminCodesObj.LoadCptNdc(CPTCodeId);


                    if (obj1.Data != null || obj.Data != null)
                    {
                        dsCodes = obj.Data;
                        NDCList = obj1.Data;
                        var response = new
                        {
                            status = true,
                            CPTNdc_JSON = NDCList,
                            CPTNdcCount = NDCList.Count,
                            CPTPlanCount = dsCodes.Tables[dsCodes.CPTPlan.TableName].Rows.Count,
                            CPTPlan_JSON = MDVUtility.JSON_DataTable(dsCodes.Tables[dsCodes.CPTPlan.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            CPTNdcCount = 0,
                            CPTPlanCount = 0
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.No_Record_Message
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
        private string LoadCPTNDCetail(Int64 CPTNdcId, string NdcCode, string CPTCode)
        {
            try
            {
                //if (CPTNdcId > 0)
                //{

                List<CPTNdcModel> NDCList = null;
                BLObject<List<CPTNdcModel>> obj;

                obj = BLLAdminCodesObj.LoadCptNdc(0, CPTCode, CPTNdcId, NdcCode);


                if (obj.Data != null)
                {
                    NDCList = obj.Data;
                    var response = new
                    {
                        status = true,
                        NDCData = NDCList,
                        NDCCount = NDCList.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        NDCCount = 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                /*}
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.No_Record_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }*/
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
        /// Fills the CPT plan information.
        /// </summary>
        /// <param name="CPTPlanId">The CPT plan identifier.</param>
        /// <returns></returns>
        private string FillCPTPlanInfo(Int64 CPTPlanId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CPTPlanId)))
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
                    DSCodes dsCodes = null;
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadCPTPlanInfo(0, CPTPlanId);
                    if (obj.Data != null)
                    {
                        dsCodes = obj.Data;
                        if (dsCodes.Tables[dsCodes.CPTPlan.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsCodes.Tables[dsCodes.CPTPlan.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                             {
                            { "ddlInsurancePlan", MDVUtility.ToStr(dr[dsCodes.CPTPlan.InsurancePlanIdColumn.ColumnName])},
                            { "txtPlanCPTCode", MDVUtility.ToStr(dr[dsCodes.CPTPlan.CPTPlanCodeColumn.ColumnName])}
                            };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                CPTCodePlanFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the CPT plan information.
        /// </summary>
        /// <param name="ProviderLicenseId">The provider license identifier.</param>
        /// <returns></returns>
        private string DeleteCPTPlanInfo(Int64 CPTPlanId)
        {

            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CPTPlanId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLAdminCodesObj.DeleteCPTPlanInfo(MDVUtility.ToStr(CPTPlanId));
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
        private string DeleteCPTNdcInfo(Int64 CPTNdcId)
        {

            try
            {
                string privilegesMessage = JsonConvert.SerializeObject(AppPrivileges.GetFormSecurity("CPT", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(CPTNdcId)))
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        BLObject<string> obj = BLLAdminCodesObj.DeleteCPTNdcInfo(MDVUtility.ToStr(CPTNdcId));
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
                    return JsonConvert.SerializeObject(response);
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
        /// Handle the CPT Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                #region CPT Code
                case "SAVE_CPT_CODE":
                    {
                        string fieldsJSON = context.Request["CPTCodeData"];
                        string strJSONData = SaveCPTCode(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_CPT_CODE":
                    {
                        string strCPTCodeId = context.Request["CPTCodeID"];
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJSONData = FillCPTCode(MDVUtility.ToInt64(strCPTCodeId), pageNo, rpp);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_CPT_CODE":
                    {
                        string strCPTCodeId = context.Request["CPTCodeID"];
                        string strJSONData = DeleteCPTCode(MDVUtility.ToInt64(strCPTCodeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_CPT_CODE":
                    {
                        string fieldsJSON = context.Request["CPTCodeData"];
                        Int64 CPTCodeID = MDVUtility.ToInt64(context.Request["CPTCodeID"]);
                        string strJSONData = UpdateCPTCode(fieldsJSON, CPTCodeID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_CPT_CODE_ACTIVE_INACTIVE":
                    {
                        Int64 CPTCodeID = MDVUtility.ToInt64(context.Request["CPTCodeID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJSONData = UpdateCPTCodeIsActive(CPTCodeID, IsActive, pageNo, rpp);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                #endregion
                #region CPT Plan
                case "SAVE_CPT_CODE_PLAN_INFO":
                    {
                        Int64 CPTCodeID = MDVUtility.ToInt64(context.Request["CPTCodeId"]);
                        string RowId = context.Request["RowId"];
                        string fieldsJSON = context.Request["CPTCodePlanData"];
                        string strJSONData = SaveCPTPlanInfo(fieldsJSON, CPTCodeID, RowId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_CPT_CODE_NDC_INFO":
                    {
                        string fieldsJSON = context.Request["CPTNDCData"];
                        string strJSONData = SaveNDCInfo(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_CPT_CODE_NDC_INFO":
                    {
                        string fieldsJSON = context.Request["CPTNDCData"];
                        string strJSONData = UpdateNDCInfo(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_CPT_CODE_PLAN_INFO":
                    {
                        string fieldsJSON = context.Request["CPTCodePlanData"];
                        Int64 CPTPlanId = MDVUtility.ToInt64(context.Request["CPTCodePlanID"]);
                        string strJSONData = UpdateCPTPlanInfo(fieldsJSON, CPTPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_CPT_CODE_PLAN_INFO":
                    {
                        Int64 CPTCodeID = MDVUtility.ToInt64(context.Request["CPTCodeId"]);
                        string strJSONData = LoadCPTPlanInfo(CPTCodeID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_CPT_CODE_PLAN_INFO":
                    {
                        Int64 CPTCodePlanID = MDVUtility.ToInt64(context.Request["CPTCodePlanID"]);
                        string strJSONData = FillCPTPlanInfo(CPTCodePlanID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_CPT_CODE_PLAN_INFO":
                    {
                        Int64 CPTPlanId = MDVUtility.ToInt64(context.Request["CPTCodePlanID"]);
                        string strJSONData = DeleteCPTPlanInfo(CPTPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_CPT_NDC_INFO":
                    {
                        Int64 CPTNdcId = MDVUtility.ToInt64(context.Request["CPTNdcId"]);
                        string strJSONData = DeleteCPTNdcInfo(CPTNdcId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_CPT_CODE_NDC":
                    {
                        string CPTCode = MDVUtility.ToStr(context.Request["CPTCode"]);
                        Int64 CPTNdcId = MDVUtility.ToInt64(context.Request["CPTNdcId"]);
                        string NDCCode = MDVUtility.ToStr(context.Request["NDCCode"]);
                        string strJSONData = LoadCPTNDCetail(CPTNdcId, NDCCode, CPTCode);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                    #endregion
            }
        }
        #endregion
    }
}