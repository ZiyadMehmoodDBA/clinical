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
using MDVision.IEHR.Model.Admin;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_InsurancePlan_Detail
    {
        BLLAdminInsurance BLLAdminInsuranceObj = null;
        public Admin_InsurancePlan_Detail()
        {
            BLLAdminInsuranceObj = new BLLAdminInsurance();
        }
        #region Singleton
        private static Admin_InsurancePlan_Detail _obj = null;
        public static Admin_InsurancePlan_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_InsurancePlan_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the insurance plan.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveInsurancePlan(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = new DSInsurance();
                    DSInsurance.InsurancePlanRow dr = dsInsurance.InsurancePlan.NewInsurancePlanRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.Description = SearchedfieldsJSON["txtDiscription"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlInsurance"]))
                        dr.InsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlInsurance"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanCategory"]))
                        dr.PlanId = MDVUtility.ToInt16(SearchedfieldsJSON["ddlPlanCategory"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanFeeLink"]))
                        dr.PlanFeeLinkId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlanFeeLink"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanType"]))
                        dr.PlanTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlanType"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimFlag"]))
                        dr.ClaimFlagId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaimFlag"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimType"]))
                        dr.ClaimTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaimType"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlScrubbingTemplate"]))
                        dr.ClaimScrubbingTemplate = MDVUtility.ToInt64(SearchedfieldsJSON["ddlScrubbingTemplate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfEDIInsurance"]))
                        dr.EDISubmitInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["hfEDIInsurance"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEDIClaimStatusInsurance"]))
                        dr.EDIClaimStatusInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEDIClaimStatusInsurance"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBox24IJShaded"]))
                        dr.Box24IJShaded = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBox24IJShaded"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBox24BShaded"]))
                        dr.Box24BShaded = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBox24BShaded"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfEDIEligibilityInsurance"]))
                        dr.EDIEligibilityInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["hfEDIEligibilityInsurance"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ICDType"]))
                        dr.IsICD10 = Convert.ToBoolean(MDVUtility.ToInt32(SearchedfieldsJSON["ICDType"]));

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtSplitInsurancePlan"])))
                        dr.SplittedInsurancePlanId = Convert.ToInt64(MDVUtility.ToInt32(SearchedfieldsJSON["hfSplitInsurancePlanId"]));
                    else
                        dr[dsInsurance.InsurancePlan.SplittedInsurancePlanIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkSplit"])))
                        dr.Splitted = Convert.ToBoolean(SearchedfieldsJSON["chkSplit"]);

                    //dr.CoPayment = SearchedfieldsJSON["txtCopayment"];
                    //adnan maqbool, PMS-5212
                    dr.OutstandingDays = string.IsNullOrEmpty(((String)(SearchedfieldsJSON["txtOutstandingDays"])).Replace("_", "")) ? null : ((String)(SearchedfieldsJSON["txtOutstandingDays"])).Replace("_", "");
                    dr.Modifier = SearchedfieldsJSON["txtModifier"];
                    dr.ClaimPayerId = ((String)(SearchedfieldsJSON["txtClaimPayorId"])).Replace("_", "");
                    dr.CapitatedPlan = MDVUtility.ToStr(SearchedfieldsJSON["chkCapitatedPlan"]) == "True" ? true : false;
                    dr.ElectronicSubmit = MDVUtility.ToStr(SearchedfieldsJSON["chkElectronicSubmit"]) == "True" ? true : false;
                    dr.CapAutoWriteOff = MDVUtility.ToStr(SearchedfieldsJSON["chkCapitatedAutoWriteoff"]) == "True" ? true : false;
                    dr.ManagedCare = MDVUtility.ToStr(SearchedfieldsJSON["chkManagedCare"]) == "True" ? true : false;
                    dr.IsReportNPI = MDVUtility.ToStr(SearchedfieldsJSON["chkIsReportNPI"]) == "True" ? true : false;
                    dr.IsAnesthesiaByMinutes = MDVUtility.ToStr(SearchedfieldsJSON["chkIsAnesthesiaByMinutes"]) == "True" ? true : false;
                    dr.IsReferralRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkRefRequired"]) == "True" ? true : false;

                    dr.SearchPattern = SearchedfieldsJSON["txtSearchPattern"];
                    if (!(string.IsNullOrEmpty(dr.SearchPattern)))
                    {
                        dr.RegularExpression = BLLAdminInsuranceObj.RegexCreator(dr.SearchPattern);
                    }
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsInsurance.InsurancePlan.AddInsurancePlanRow(dr);
                    BLObject<DSInsurance> obj = BLLAdminInsuranceObj.InsertInsurancePlan(ref dsInsurance);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            InsurancePlanId = dsInsurance.Tables[dsInsurance.InsurancePlan.TableName].Rows[0][dsInsurance.InsurancePlan.InsurancePlanIdColumn.ColumnName]
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
        /// Updates the insurance plan.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateInsurancePlan(string fieldsJSON, Int64 InsurancePlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = new DSInsurance();
                    //DSInsurance.InsurancePlanRow dr = dsInsurance.InsurancePlan.NewInsurancePlanRow();
                    BLObject<DSInsurance> objLoad = BLLAdminInsuranceObj.LoadInsurancePlan(InsurancePlanId, null, null, null, null, null, null, null);
                    dsInsurance = objLoad.Data;
                    foreach (DSInsurance.InsurancePlanRow dr in dsInsurance.Tables[dsInsurance.InsurancePlan.TableName].Rows)
                    {
                        //dr.InsurancePlanId = InsurancePlanId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"];
                        dr.Description = SearchedfieldsJSON["txtDiscription"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlInsurance"]))
                            dr.InsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlInsurance"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanCategory"]))
                            dr.PlanId = MDVUtility.ToInt16(SearchedfieldsJSON["ddlPlanCategory"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanFeeLink"]))
                            dr.PlanFeeLinkId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlanFeeLink"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanType"]))
                            dr.PlanTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlanType"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimFlag"]))
                            dr.ClaimFlagId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaimFlag"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimType"]))
                            dr.ClaimTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaimType"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlScrubbingTemplate"]))
                            dr.ClaimScrubbingTemplate = MDVUtility.ToInt64(SearchedfieldsJSON["ddlScrubbingTemplate"]);
                        else
                            dr.ClaimScrubbingTemplate = MDVUtility.ToInt64(0);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfEDIInsurance"]))
                            dr.EDISubmitInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["hfEDIInsurance"]);
                        else
                            dr.EDISubmitInsuranceId = MDVUtility.ToInt64(0);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEDIClaimStatusInsurance"]))
                            dr.EDIClaimStatusInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEDIClaimStatusInsurance"]);
                        else
                            dr.EDIClaimStatusInsuranceId = MDVUtility.ToInt64(0);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBox24IJShaded"]))
                            dr.Box24IJShaded = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBox24IJShaded"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBox24BShaded"]))
                            dr.Box24BShaded = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBox24BShaded"]);


                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfEDIEligibilityInsurance"]))
                            dr.EDIEligibilityInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["hfEDIEligibilityInsurance"]);
                        else
                            dr.EDIEligibilityInsuranceId = MDVUtility.ToInt64(0);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ICDType"]))
                            dr.IsICD10 = Convert.ToBoolean(MDVUtility.ToInt32(SearchedfieldsJSON["ICDType"]));

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtSplitInsurancePlan"])))
                            dr.SplittedInsurancePlanId = Convert.ToInt64(MDVUtility.ToInt32(SearchedfieldsJSON["hfSplitInsurancePlanId"]));
                        else
                            dr[dsInsurance.InsurancePlan.SplittedInsurancePlanIdColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkSplit"])))
                            dr.Splitted = Convert.ToBoolean(SearchedfieldsJSON["chkSplit"]);

                        //dr.CoPayment = SearchedfieldsJSON["txtCopayment"];
                        //adnan maqbool, PMS-5212
                        dr.OutstandingDays = string.IsNullOrEmpty(((String)(SearchedfieldsJSON["txtOutstandingDays"])).Replace("_", "")) ? null : ((String)(SearchedfieldsJSON["txtOutstandingDays"])).Replace("_", "");
                        dr.Modifier = SearchedfieldsJSON["txtModifier"];
                        dr.ClaimPayerId = ((String)(SearchedfieldsJSON["txtClaimPayorId"])).Replace("_", "");
                        dr.CapitatedPlan = MDVUtility.ToStr(SearchedfieldsJSON["chkCapitatedPlan"]) == "True" ? true : false;
                        dr.ElectronicSubmit = MDVUtility.ToStr(SearchedfieldsJSON["chkElectronicSubmit"]) == "True" ? true : false;
                        dr.CapAutoWriteOff = MDVUtility.ToStr(SearchedfieldsJSON["chkCapitatedAutoWriteoff"]) == "True" ? true : false;
                        dr.ManagedCare = MDVUtility.ToStr(SearchedfieldsJSON["chkManagedCare"]) == "True" ? true : false;
                        dr.IsReportNPI = MDVUtility.ToStr(SearchedfieldsJSON["chkIsReportNPI"]) == "True" ? true : false;
                        dr.IsAnesthesiaByMinutes = MDVUtility.ToStr(SearchedfieldsJSON["chkIsAnesthesiaByMinutes"]) == "True" ? true : false;
                        dr.IsReferralRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkRefRequired"]) == "True" ? true : false;
                        dr.SearchPattern = SearchedfieldsJSON["txtSearchPattern"];
                        if (!(string.IsNullOrEmpty(dr.SearchPattern)))
                        {
                            dr.RegularExpression = BLLAdminInsuranceObj.RegexCreator(dr.SearchPattern);
                        }
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsInsurance.InsurancePlan.AddInsurancePlanRow(dr);
                    //dsInsurance.InsurancePlan.AcceptChanges();

                    if (dsInsurance.Tables[dsInsurance.InsurancePlan.TableName].Rows.Count > 0)
                    {
                        //dsInsurance.InsurancePlan.Rows[0].SetModified();
                        BLObject<DSInsurance> obj = BLLAdminInsuranceObj.UpdateInsurancePlan(ref dsInsurance);
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
        /// Fills the insurance plan.
        /// </summary>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillInsurancePlan(Int64 InsurancePlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(InsurancePlanId)))
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
                        DSInsurance dsInsurance = null;
                        BLObject<DSInsurance> obj = BLLAdminInsuranceObj.LoadInsurancePlan(InsurancePlanId, null, null, null, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsInsurance = obj.Data;
                            if (dsInsurance.Tables[dsInsurance.InsurancePlan.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsInsurance.Tables[dsInsurance.InsurancePlan.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.ShortNameColumn.ColumnName])},
                            { "txtDiscription", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.DescriptionColumn.ColumnName])},
                            { "ddlInsurance", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.InsuranceIdColumn.ColumnName])},
                            { "txtddlInsurance", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.InsuranceNameColumn.ColumnName])},
                            { "ddlPlanCategory", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.PlanIdColumn.ColumnName])},
                            { "ddlPlanFeeLink", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.PlanFeeLinkIdColumn.ColumnName])},
                            { "ddlPlanType", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.PlanTypeIdColumn.ColumnName])},
                            { "ddlClaimFlag", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.ClaimFlagIdColumn.ColumnName])},
                            { "ddlClaimType", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.ClaimTypeIdColumn.ColumnName])},
                            { "txtCopayment", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.CoPaymentColumn.ColumnName])},
                            { "txtOutstandingDays", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.OutstandingDaysColumn.ColumnName])},
                            { "txtModifier", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.ModifierColumn.ColumnName])},
                            { "ddlScrubbingTemplate", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.ClaimScrubbingTemplateColumn.ColumnName])},
                            { "chkCapitatedPlan", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.CapitatedPlanColumn.ColumnName])},
                            { "hfEDIInsurance", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.EDISubmitInsuranceIdColumn.ColumnName])},
                            { "ddlEDISubmitInsurance", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.SubmitInsuranceNameColumn.ColumnName])},
                            { "ddlEDIClaimStatusInsurance", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.EDIClaimStatusInsuranceIdColumn.ColumnName])},
                            { "ddlBox24BShaded", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.Box24BShadedColumn.ColumnName])},
                            { "ddlBox24IJShaded", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.Box24IJShadedColumn.ColumnName])},
                            { "chkElectronicSubmit", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.ElectronicSubmitColumn.ColumnName])},
                            { "chkCapitatedAutoWriteoff", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.CapAutoWriteOffColumn.ColumnName])},
                            { "hfEDIEligibilityInsurance", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.EDIEligibilityInsuranceIdColumn.ColumnName])},
                             { "ddlEDIEligibilityInsurance", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.EligibilityInsuranceNameColumn.ColumnName])},
                            { "txtClaimPayorId", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.ClaimPayerIdColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.IsActiveColumn.ColumnName])},
                            { "chkManagedCare", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.ManagedCareColumn.ColumnName])},
                            { "chkIsReportNPI", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.IsReportNPIColumn.ColumnName])},
                            { "chkIsAnesthesiaByMinutes", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.IsAnesthesiaByMinutesColumn.ColumnName])},
                            { "txtSearchPattern", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.SearchPatternColumn.ColumnName])},
                            { "ICDType",string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.IsICD10Column.ColumnName]))? "0" : (Convert.ToBoolean(MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.IsICD10Column.ColumnName]))) ? "1" : "0" },
                            { "txtSplitInsurancePlan",MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.SplittedInsurancePlanNameColumn.ColumnName])},
                            { "hfSplitInsurancePlanId", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.SplittedInsurancePlanIdColumn.ColumnName])},
                            { "chkSplit",string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.SplittedColumn.ColumnName])) ? "0" :(Convert.ToBoolean(MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.SplittedColumn.ColumnName]))) ? "1": "0" },
                            { "chkRefRequired", MDVUtility.ToStr(dr[dsInsurance.InsurancePlan.IsReferralRequiredColumn.ColumnName])},

                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    InsurancePlanFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the insurance plan.
        /// </summary>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteInsurancePlan(Int64 InsurancePlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(InsurancePlanId)))
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
                        BLObject<string> obj = BLLAdminInsuranceObj.DeleteInsurancePlan(MDVUtility.ToStr(InsurancePlanId));
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
        /// Updates the insurance plan is active.
        /// </summary>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateInsurancePlanIsActive(Int64 InsurancePlanId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSInsurance dsInsurance = null;
                    BLObject<DSInsurance> obj = BLLAdminInsuranceObj.LoadInsurancePlan(InsurancePlanId, null, null, null, null, null, null, null);
                    dsInsurance = obj.Data;
                    if (dsInsurance.Tables[dsInsurance.InsurancePlan.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsInsurance.Tables[dsInsurance.InsurancePlan.TableName].Rows[0];
                        dr[dsInsurance.InsurancePlan.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSInsurance> objInsurancePlan = BLLAdminInsuranceObj.UpdateInsurancePlan(ref dsInsurance);
                        string successMsg;
                        if (objInsurancePlan.Data != null)
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
                                Message = objInsurancePlan.Message
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

        #region Insurance Plan Address Info
        /// <summary>
        /// Saves the insurance plan address information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <returns></returns>
        private string SaveInsurancePlanAddressInfo(string fieldsJSON, Int64 InsurancePlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = new DSInsurance();
                    DSInsurance.InsurancePlanAddressRow dr = dsInsurance.InsurancePlanAddress.NewInsurancePlanAddressRow();

                    dr.Address = SearchedfieldsJSON["txtAddInfoAddress"];
                    dr.City = SearchedfieldsJSON["txtAddInfoCity"];
                    dr.State = SearchedfieldsJSON["txtAddInfoState"];
                    dr.ZipCode = SearchedfieldsJSON["txtAddInfoZip"];
                    dr.ZipCodeExt = SearchedfieldsJSON["txtAddInfoZipExt"];
                    dr.PhoneNo = SearchedfieldsJSON["txtAddInfoTelephone"];
                    dr.PhoneExt = SearchedfieldsJSON["txtAddInfoPhoneExt"];
                    dr.Fax = SearchedfieldsJSON["txtAddInfoFax"];
                    dr.UserName = SearchedfieldsJSON["txtAddInfoUserId"];
                    dr.Password = SearchedfieldsJSON["txtAddInfoPassword"];
                    dr.EmailAddress = SearchedfieldsJSON["txtAddInfoEmail"];
                    dr.WebPortal = SearchedfieldsJSON["txtAddInfoWebPortal"];
                    dr.IsActive = true;
                    dr.CreateBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreateOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.InsurancePlanId = InsurancePlanId;

                    #region Database Insertion
                    dsInsurance.InsurancePlanAddress.AddInsurancePlanAddressRow(dr);
                    BLObject<DSInsurance> obj = BLLAdminInsuranceObj.InsertInsurancePlanAddressInfo(dsInsurance);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            InsurancePlanAddressId = dsInsurance.Tables[dsInsurance.InsurancePlanAddress.TableName].Rows[0][dsInsurance.InsurancePlanAddress.InsurancePlanAddressIdColumn.ColumnName]
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
        /// Updates the insurance plan address information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <param name="InsurPlanAddressId">The insur plan address identifier.</param>
        /// <returns></returns>
        private string UpdateInsurancePlanAddressInfo(string fieldsJSON, Int64 InsurancePlanId, Int64 InsurPlanAddressId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = new DSInsurance();
                    //DSInsurance.InsurancePlanAddressRow dr = dsInsurance.InsurancePlanAddress.NewInsurancePlanAddressRow();

                    BLObject<DSInsurance> objLoad = BLLAdminInsuranceObj.LoadInsurancePlanAddressInfo(0, InsurPlanAddressId);
                    dsInsurance = objLoad.Data;

                    foreach (DSInsurance.InsurancePlanAddressRow dr in dsInsurance.Tables[dsInsurance.InsurancePlanAddress.TableName].Rows)
                    {
                        //dr.InsurancePlanAddressId = InsurPlanAddressId;
                        dr.Address = SearchedfieldsJSON["txtAddInfoAddress"];
                        dr.City = SearchedfieldsJSON["txtAddInfoCity"];
                        dr.State = SearchedfieldsJSON["txtAddInfoState"];
                        dr.ZipCode = SearchedfieldsJSON["txtAddInfoZip"];
                        dr.ZipCodeExt = SearchedfieldsJSON["txtAddInfoZipExt"];
                        dr.PhoneNo = SearchedfieldsJSON["txtAddInfoTelephone"];
                        dr.PhoneExt = SearchedfieldsJSON["txtAddInfoPhoneExt"];
                        dr.Fax = SearchedfieldsJSON["txtAddInfoFax"];
                        dr.UserName = SearchedfieldsJSON["txtAddInfoUserId"];
                        dr.Password = SearchedfieldsJSON["txtAddInfoPassword"];
                        dr.EmailAddress = SearchedfieldsJSON["txtAddInfoEmail"];
                        dr.WebPortal = SearchedfieldsJSON["txtAddInfoWebPortal"];
                        dr.IsActive = true;
                        //dr.CreateBy = "";
                        //dr.CreateOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.InsurancePlanId = InsurancePlanId;
                    }
                    #region Database Updation
                    //dsInsurance.InsurancePlanAddress.AddInsurancePlanAddressRow(dr);
                    //dsInsurance.InsurancePlanAddress.AcceptChanges();

                    if (dsInsurance.Tables[dsInsurance.InsurancePlanAddress.TableName].Rows.Count > 0)
                    {
                        //dsInsurance.InsurancePlanAddress.Rows[0].SetModified();
                        BLObject<DSInsurance> obj = BLLAdminInsuranceObj.UpdateInsurancePlanAddressInfo(dsInsurance);
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
        /// Loads the insurance plan address information.
        /// </summary>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadInsurancePlanAddressInfo(Int64 InsurancePlanId)
        {
            try
            {
                DSInsurance dsInsurance = null;
                BLObject<DSInsurance> obj;
                if (InsurancePlanId > 0)
                {
                    obj = BLLAdminInsuranceObj.LoadInsurancePlanAddressInfo(InsurancePlanId, 0);

                    dsInsurance = obj.Data;
                    var response = new
                    {
                        status = true,
                        InsurancePlanAddressCount = dsInsurance.Tables[dsInsurance.InsurancePlanAddress.TableName].Rows.Count,
                        InsurancePlanAddress_JSON = MDVUtility.JSON_DataTable(dsInsurance.Tables[dsInsurance.InsurancePlanAddress.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        private string LoadInsurancePlanAddress(string fieldsJSON, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    ser.MaxJsonLength = Int32.MaxValue;
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = null;
                    BLObject<DSInsurance> obj;
                    //List<InsurancePlanAddressModel> PlanAddress = new List<InsurancePlanAddressModel>();

                    //if (InsurancePlanId > 0)
                    //{
                    obj = BLLAdminInsuranceObj.LoadInsurancePlanAddressSearch(SearchedfieldsJSON["txtInsurancePlan"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["txtAddress"], SearchedfieldsJSON["txtCity"], SearchedfieldsJSON["txtState"], SearchedfieldsJSON["txtZip"], SearchedfieldsJSON["txtTelephone"], PageNumber, RowsPerPage);

                    dsInsurance = obj.Data;
                    var response = new
                    {
                        //status = true,
                        //InsurancePlanAddressCount = PlanAddress.Count,
                        //InsurancePlanAddress_JSON = PlanAddress,
                        //iTotalDisplayRecords = PlanAddress.Count,
                        status = true,
                        InsurancePlanAddressCount = dsInsurance.Tables[dsInsurance.InsurancePlanAddress.TableName].Rows.Count,
                        InsurancePlanAddress_JSON = MDVUtility.JSON_DataTable(dsInsurance.Tables[dsInsurance.InsurancePlanAddress.TableName]),
                        iTotalDisplayRecords = (dsInsurance.InsurancePlanAddress.Rows.Count > 0) ? dsInsurance.InsurancePlanAddress.Rows[0][dsInsurance.InsurancePlanAddress.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
            //else
            //{
            //    var response = new
            //    {
            //        status = true,
            //        Message = Common.AppPrivileges.No_Record_Message
            //    };
            //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //}
            //}
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
        /// Fills the insurance plan address plan information.
        /// </summary>
        /// <param name="InsurPlanAddressId">The insur plan address identifier.</param>
        /// <returns></returns>
        private string FillInsurancePlanAddressPlanInfo(Int64 InsurPlanAddressId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(InsurPlanAddressId)))
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
                        DSInsurance dsInsurance = null;
                        BLObject<DSInsurance> obj = BLLAdminInsuranceObj.LoadInsurancePlanAddressInfo(0, InsurPlanAddressId);
                        if (obj.Data != null)
                        {
                            dsInsurance = obj.Data;
                            if (dsInsurance.Tables[dsInsurance.InsurancePlanAddress.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsInsurance.Tables[dsInsurance.InsurancePlanAddress.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtAddInfoAddress", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.AddressColumn.ColumnName])},
                            { "txtAddInfoCity", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.CityColumn.ColumnName])},
                            { "txtAddInfoState", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.StateColumn.ColumnName])},
                            { "txtAddInfoZip", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.ZipCodeColumn.ColumnName])},
                            { "txtAddInfoZipExt", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.ZipCodeExtColumn.ColumnName])},
                            { "txtAddInfoTelephone", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.PhoneNoColumn.ColumnName])},
                            { "txtAddInfoPhoneExt", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.PhoneExtColumn.ColumnName])},
                            { "txtAddInfoFax", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.FaxColumn.ColumnName])},
                            { "txtAddInfoUserId", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.UserNameColumn.ColumnName])},
                            { "txtAddInfoPassword", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.PasswordColumn.ColumnName])},
                            { "txtAddInfoWebPortal", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.WebPortalColumn.ColumnName])},
                            { "txtAddInfoEmail", MDVUtility.ToStr(dr[dsInsurance.InsurancePlanAddress.EmailAddressColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    InsurancePlanAddressFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the insurance plan address information.
        /// </summary>
        /// <param name="InsurPlanAddressId">The insur plan address identifier.</param>
        /// <returns></returns>
        private string DeleteInsurancePlanAddressInfo(Int64 InsurPlanAddressId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(InsurPlanAddressId)))
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
                        BLObject<string> obj = BLLAdminInsuranceObj.DeleteInsurancePlanAddressInfo(MDVUtility.ToStr(InsurPlanAddressId));
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
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Insurance Plan Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                #region Insurance Plan
                case "SAVE_INSURANCE_PLAN":
                    {
                        string fieldsJSON = context.Request["InsurancePlanData"];
                        string strJSONData = SaveInsurancePlan(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_INSURANCE_PLAN":
                    {
                        string strInsurancePlanId = context.Request["InsurancePlanID"];
                        string strJSONData = FillInsurancePlan(MDVUtility.ToInt64(strInsurancePlanId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_INSURANCE_PLAN":
                    {
                        string strInsurancePlanId = context.Request["InsurancePlanID"];
                        string strJSONData = DeleteInsurancePlan(MDVUtility.ToInt64(strInsurancePlanId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_INSURANCE_PLAN":
                    {
                        string fieldsJSON = context.Request["InsurancePlanData"];
                        Int64 InsurancePlanID = MDVUtility.ToInt64(context.Request["InsurancePlanID"]);
                        string strJSONData = UpdateInsurancePlan(fieldsJSON, InsurancePlanID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_INSURANCE_PLAN_ACTIVE_INACTIVE":
                    {
                        Int64 InsurancePlanID = MDVUtility.ToInt64(context.Request["InsurancePlanID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateInsurancePlanIsActive(InsurancePlanID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                #endregion
                #region Insurance Plan Address
                case "SAVE_ADDRESS_INFO":
                    {
                        string fieldsJSON = context.Request["InsurancePlanAddressData"];
                        Int64 InsurancePlanID = MDVUtility.ToInt64(context.Request["InsurancePlanId"]);
                        string strJSONData = SaveInsurancePlanAddressInfo(fieldsJSON, InsurancePlanID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_ADDRESS_INFO":
                    {
                        string fieldsJSON = context.Request["InsurancePlanAddressData"];
                        Int64 InsurancePlanID = MDVUtility.ToInt64(context.Request["InsurancePlanId"]);
                        Int64 InsurPlanAddressID = MDVUtility.ToInt64(context.Request["InsurancePlanAddressID"]);
                        string strJSONData = UpdateInsurancePlanAddressInfo(fieldsJSON, InsurancePlanID, InsurPlanAddressID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_ADDRESS_INFO":
                    {
                        Int64 InsurancePlanID = MDVUtility.ToInt64(context.Request["InsurancePlanId"]);
                        string strJSONData = LoadInsurancePlanAddressInfo(InsurancePlanID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PLAN_ADDRESS":
                    {
                        string fieldsJSON = context.Request["InsurancePlanAddressData"];
                        int PageNumber = MDVUtility.ToInt(context.Request["PageNumber"]);
                        int RowsPerPage = MDVUtility.ToInt(context.Request["RowsPerPage"]);
                        string strJSONData = LoadInsurancePlanAddress(fieldsJSON, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_ADDRESS_INFO":
                    {
                        Int64 InsurancePlanAddressID = MDVUtility.ToInt64(context.Request["InsurancePlanAddressID"]);
                        string strJSONData = FillInsurancePlanAddressPlanInfo(InsurancePlanAddressID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_ADDRESS_INFO":
                    {
                        Int64 InsurPlanAddressID = MDVUtility.ToInt64(context.Request["InsurancePlanAddressID"]);
                        string strJSONData = DeleteInsurancePlanAddressInfo(InsurPlanAddressID);

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