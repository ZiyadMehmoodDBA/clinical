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
    public class Admin_POSFeeSchedule_Detail
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_POSFeeSchedule_Detail()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_POSFeeSchedule_Detail _obj = null;
        public static Admin_POSFeeSchedule_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_POSFeeSchedule_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the position fee schedule.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SavePOSFeeSchedule(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT POS", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeSchedule = new DSFeeSchedule();
                    DSFeeSchedule.FeeGroupPOSScheduleRow dr = dsFeeSchedule.FeeGroupPOSSchedule.NewFeeGroupPOSScheduleRow();

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFeeGroup"]))
                        dr.FeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFeeGroup"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanFeeLink"]))
                        dr.PlanFeeLinkId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlanFeeLink"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCPTCode"]))
                        dr.CPTCode = Convert.ToString(SearchedfieldsJSON["hfCPTCode"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPOSCode"]))
                        dr.POSId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPOSCode"]);
                    dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtFee"]);
                    dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtExpectedFee"]);
                    dr.AuthorizationRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkAuthorizationRequired"]) == "True" ? true : false;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsFeeSchedule.FeeGroupPOSSchedule.AddFeeGroupPOSScheduleRow(dr);
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.InsertFeeGroupPOSSchedule(dsFeeSchedule);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            POSFeeScheduleId = dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupPOSSchedule.TableName].Rows[0][dsFeeSchedule.FeeGroupPOSSchedule.FeeGroupPOSIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        string message = obj.Message;
                        if (message.Contains("UNIQUE KEY"))
                            message = "Cannot insert duplicate Fee Group Plan CPT POS.";

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

        /// <summary>
        /// Updates the position fee schedule.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="POSFeeScheduleId">The position fee schedule identifier.</param>
        /// <returns></returns>
        private string UpdatePOSFeeSchedule(string fieldsJSON, int POSFeeScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT POS", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeSchedule = new DSFeeSchedule();
                    //DSFeeSchedule.FeeGroupPOSScheduleRow dr = dsFeeSchedule.FeeGroupPOSSchedule.NewFeeGroupPOSScheduleRow();
                    BLObject<DSFeeSchedule> objLoad = BLLFeeScheduleObj.LoadFeeGroupPOSSchedule(POSFeeScheduleId, null, null, null, null, null);
                    dsFeeSchedule = objLoad.Data;
                    foreach (DSFeeSchedule.FeeGroupPOSScheduleRow dr in dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupPOSSchedule.TableName].Rows)
                    {
                        //dr.FeeGroupPOSId = POSFeeScheduleId;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFeeGroup"]))
                            dr.FeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFeeGroup"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanFeeLink"]))
                            dr.PlanFeeLinkId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlanFeeLink"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCPTCode"]))
                            dr.CPTCode = Convert.ToString(SearchedfieldsJSON["hfCPTCode"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPOSCode"]))
                            dr.POSId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPOSCode"]);
                        dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtFee"]);
                        dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtExpectedFee"]);
                        dr.AuthorizationRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkAuthorizationRequired"]) == "True" ? true : false;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsFeeSchedule.FeeGroupPOSSchedule.AddFeeGroupPOSScheduleRow(dr);
                    //dsFeeSchedule.FeeGroupPOSSchedule.AcceptChanges();

                    if (dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupPOSSchedule.TableName].Rows.Count > 0)
                    {
                        //dsFeeSchedule.FeeGroupPOSSchedule.Rows[0].SetModified();
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.UpdateFeeGroupPOSSchedule(dsFeeSchedule);
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
                                message = "Cannot insert duplicate Fee Group Plan CPT POS.";

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
        /// Fills the position fee schedule.
        /// </summary>
        /// <param name="POSFeeScheduleId">The position fee schedule identifier.</param>
        /// <returns></returns>
        private string FillPOSFeeSchedule(Int64 POSFeeScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT POS", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(POSFeeScheduleId)))
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
                        DSFeeSchedule dsFeeSchedule = null;
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadFeeGroupPOSSchedule(POSFeeScheduleId, null, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsFeeSchedule = obj.Data;
                            if (dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupPOSSchedule.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupPOSSchedule.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "ddlFeeGroup", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupPOSSchedule.FeeGroupIdColumn.ColumnName])},
                            { "ddlPlanFeeLink", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupPOSSchedule.PlanFeeLinkIdColumn.ColumnName])},
                            { "hfCPTCode", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupPOSSchedule.CPTCodeColumn.ColumnName])},
                            { "txtCPTCode", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupPOSSchedule.CPTCodeColumn.ColumnName])},
                            { "ddlPOSCode", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupPOSSchedule.POSIdColumn.ColumnName])},
                            { "txtFee",string.Format("{0:N2}",dr[dsFeeSchedule.FeeGroupPOSSchedule.FeeColumn.ColumnName])},
                            { "txtExpectedFee",string.Format("{0:N2}",dr[dsFeeSchedule.FeeGroupPOSSchedule.ExpectedFeeColumn.ColumnName])},
                            { "chkAuthorizationRequired", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupPOSSchedule.AuthorizationRequiredColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupPOSSchedule.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    POSFeeScheduleFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the position fee schedule.
        /// </summary>
        /// <param name="POSFeeScheduleId">The position fee schedule identifier.</param>
        /// <returns></returns>
        private string DeletePOSFeeSchedule(Int64 POSFeeScheduleId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT POS", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(POSFeeScheduleId)))
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
                        BLObject<string> obj = BLLFeeScheduleObj.DeleteFeeGroupPOSSchedule(MDVUtility.ToStr(POSFeeScheduleId));
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
        /// Updates the position fee schedule is active.
        /// </summary>
        /// <param name="POSFeeScheduleId">The position fee schedule identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdatePOSFeeScheduleIsActive(Int64 POSFeeScheduleId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT POS", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSFeeSchedule dsFeeSchedule = null;
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadFeeGroupPOSSchedule(POSFeeScheduleId, null, null, null, null, null);
                    dsFeeSchedule = obj.Data;
                    if (dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupPOSSchedule.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupPOSSchedule.TableName].Rows[0];
                        dr[dsFeeSchedule.FeeGroupPOSSchedule.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSFeeSchedule> objPOSFeeSchedule = BLLFeeScheduleObj.UpdateFeeGroupPOSSchedule(dsFeeSchedule);
                        string successMsg;
                        if (objPOSFeeSchedule.Data != null)
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
                                Message = objPOSFeeSchedule.Message
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


        private string SavePOSPlanInfo(string fieldsJSON, int FeeGroupPOSId, string RowId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT POS", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dspos = new DSFeeSchedule();
                    DSFeeSchedule.PlanPOSRow dr = dspos.PlanPOS.NewPlanPOSRow();

                    long ModifierId = 0;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfModifier" + RowId]))
                    {
                        ModifierId = MDVUtility.ToInt64(SearchedfieldsJSON["hfModifier" + RowId]);
                    }

                    if (ModifierId > 0)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifierFee" + RowId]))
                            dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtModifierFee" + RowId]);
                        dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtModifierExpectedFee" + RowId]);
                        dr.FeeGroupPOSId = FeeGroupPOSId;
                        dr.ModifierId = ModifierId;
                        dr.IsRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkRequired" + RowId]) == "True" ? true : false;
                        dr.IsActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        #region Database Insertion
                        dspos.PlanPOS.AddPlanPOSRow(dr);
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.InsertPOSPlanInfo(dspos);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                POSPlanId = dspos.Tables[dspos.PlanPOS.TableName].Rows[0][dspos.PlanPOS.FeeGroupPOSModifierFeeSchIdColumn.ColumnName]
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
                            Message = "Invalid Modifier.",
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

        private string LoadPOSPlanInfo(long POSFeeId)
        {
            try
            {
                DSFeeSchedule dspos = null;
                BLObject<DSFeeSchedule> obj;
                if (POSFeeId > 0)
                {
                    obj =BLLFeeScheduleObj.LoadPOSPlanInfo(POSFeeId, 0);

                    dspos = obj.Data;
                    var response = new
                    {
                        status = true,
                        POSPlanCount = dspos.Tables[dspos.PlanPOS.TableName].Rows.Count,
                        POSPlan_JSON = MDVUtility.JSON_DataTable(dspos.Tables[dspos.PlanPOS.TableName]),
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UpdatePOSPlanInfo(string fieldsJSON, int POSPlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT POS", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    BLObject<DSFeeSchedule> objPos;
                    DSFeeSchedule dsPos = new DSFeeSchedule();
                    objPos = BLLFeeScheduleObj.LoadPOSPlanInfo(0, POSPlanId);

                    if (objPos.Data != null)
                    {
                        foreach (DSFeeSchedule.PlanPOSRow dr in objPos.Data.Tables[dsPos.PlanPOS.TableName].Rows)
                        {
                            long ModifierId = 0;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfModifier" + POSPlanId]))
                            {
                                ModifierId = MDVUtility.ToInt64(SearchedfieldsJSON["hfModifier" + POSPlanId]);
                            }
                            if (ModifierId > 0)
                            {
                                dr.ModifierId = ModifierId;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifierFee" + POSPlanId]))
                                dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtModifierFee" + POSPlanId]);
                            dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtModifierExpectedFee" + POSPlanId]);
                            dr.IsRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkRequired" + POSPlanId]) == "True" ? true : false;

                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }

                        dsPos = objPos.Data;

                        #region Database Updation

                        if (dsPos.Tables[dsPos.PlanPOS.TableName].Rows.Count > 0)
                        {
                            BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.UpdatePOSPlanInfo(dsPos);
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

        private string FillPOSPlanInfo(long POSPlanId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(POSPlanId)))
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
                    DSFeeSchedule dspos = null;
                    BLObject<DSFeeSchedule> obj =BLLFeeScheduleObj.LoadPOSPlanInfo(0, POSPlanId);
                    if (obj.Data != null)
                    {
                        dspos = obj.Data;
                        if (dspos.Tables[dspos.PlanPOS.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dspos.Tables[dspos.PlanPOS.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "ddlModifier", MDVUtility.ToStr(dr[dspos.PlanPOS.ModifierIdColumn.ColumnName])},
                            { "txtModifierFee", string.Format("{0:N2}",dr[dspos.PlanPOS.FeeColumn.ColumnName])},
                            { "txtModifierExpectedFee", string.Format("{0:N2}",dr[dspos.PlanPOS.ExpectedFeeColumn.ColumnName])},
                            { "chkRequired", MDVUtility.ToStr(dr[dspos.PlanPOS.IsRequiredColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                POSPlanFill_JSON = js.Serialize(keyValues)
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string DeletePOSPlanInfo(long POSPlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT POS", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(POSPlanId)))
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
                        BLObject<string> obj = BLLFeeScheduleObj.DeletePOSPlanInfo(MDVUtility.ToStr(POSPlanId));
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
        /// Handle the POS Fee Schedule Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_POS_FEE_SCHEDULE":
                    {
                        string fieldsJSON = context.Request["POSFeeScheduleData"];
                        string strJSONData = SavePOSFeeSchedule(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_POS_FEE_SCHEDULE":
                    {
                        string strProviderId = context.Request["POSFeeScheduleID"];
                        string strJSONData = FillPOSFeeSchedule(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_POS_FEE_SCHEDULE":
                    {
                        string strProviderId = context.Request["POSFeeScheduleID"];
                        string strJSONData = DeletePOSFeeSchedule(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_POS_FEE_SCHEDULE":
                    {
                        string fieldsJSON = context.Request["POSFeeScheduleData"];
                        int ProviderID = MDVUtility.ToInt(context.Request["POSFeeScheduleID"]);
                        string strJSONData = UpdatePOSFeeSchedule(fieldsJSON, ProviderID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_POS_FEE_SCHEDULE_ACTIVE_INACTIVE":
                    {
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["POSFeeScheduleID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdatePOSFeeScheduleIsActive(ProviderID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_POS_PLAN_INFO":
                    {
                        string fieldsJSON = context.Request["POSFeeSchedulePlanData"];
                        int POSFeeId = MDVUtility.ToInt(context.Request["POSFeeId"]);
                        string RowId = context.Request["RowId"];
                        string strJSONData = SavePOSPlanInfo(fieldsJSON, POSFeeId, RowId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_POS_PLAN_INFO":
                    {
                        Int64 POSFeeId = MDVUtility.ToInt64(context.Request["POSFeeId"]);
                        string strJSONData = LoadPOSPlanInfo(POSFeeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_POS_PLAN_INFO":
                    {
                        string fieldsJSON = context.Request["POSPlanData"];
                        int POSPlanId = MDVUtility.ToInt(context.Request["POSPlanID"]);
                        string strJSONData = UpdatePOSPlanInfo(fieldsJSON, POSPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_POS_PLAN_INFO":
                    {
                        Int64 POSPlanId = MDVUtility.ToInt64(context.Request["POSPlanId"]);
                        string strJSONData = FillPOSPlanInfo(POSPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_POS_PLAN_INFO":
                    {
                        Int64 POSPlanId = MDVUtility.ToInt64(context.Request["POSPlanId"]);
                        string strJSONData = DeletePOSPlanInfo(POSPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }
        }








        #endregion
    }
}