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
    public class Admin_ProcedureFeeSchedule_Detail
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_ProcedureFeeSchedule_Detail()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_ProcedureFeeSchedule_Detail _obj = null;
        public static Admin_ProcedureFeeSchedule_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_ProcedureFeeSchedule_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the procedure fee schedule.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveProcedureFeeSchedule(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeSchedule = new DSFeeSchedule();
                    DSFeeSchedule.FeeGroupProceduralScheduleRow dr = dsFeeSchedule.FeeGroupProceduralSchedule.NewFeeGroupProceduralScheduleRow();

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFeeGroup"]))
                        dr.FeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFeeGroup"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanFeeLink"]))
                        dr.PlanFeeLinkId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlanFeeLink"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCPTCode"]))
                        dr.CPTCode = Convert.ToString(SearchedfieldsJSON["hfCPTCode"]);
                    dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtFee"]);
                    dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtExpectedFee"]);
                    dr.AuthorizationRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkAuthorizationRequired"]) == "True" ? true : false;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsFeeSchedule.FeeGroupProceduralSchedule.AddFeeGroupProceduralScheduleRow(dr);
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.InsertFeeGroupProceduralSchedule(dsFeeSchedule);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ProcedureFeeScheduleId = dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProceduralSchedule.TableName].Rows[0][dsFeeSchedule.FeeGroupProceduralSchedule.FeeGroupProcIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        string message = obj.Message;
                        if (message.Contains("UNIQUE KEY"))
                            message = "Cannot insert duplicate Fee Group Plan CPT.";

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
        /// Updates the procedure fee schedule.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProcedureFeeScheduleId">The procedure fee schedule identifier.</param>
        /// <returns></returns>
        private string UpdateProcedureFeeSchedule(string fieldsJSON, int ProcedureFeeScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeSchedule = new DSFeeSchedule();
                    //DSFeeSchedule.FeeGroupProceduralScheduleRow dr = dsFeeSchedule.FeeGroupProceduralSchedule.NewFeeGroupProceduralScheduleRow();
                    BLObject<DSFeeSchedule> objLoad = BLLFeeScheduleObj.LoadFeeGroupProceduralSchedule(ProcedureFeeScheduleId, null, null, null, null);
                    dsFeeSchedule = objLoad.Data;
                    foreach (DSFeeSchedule.FeeGroupProceduralScheduleRow dr in dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProceduralSchedule.TableName].Rows)
                    {
                        //dr.FeeGroupProcId = ProcedureFeeScheduleId;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFeeGroup"]))
                            dr.FeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFeeGroup"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanFeeLink"]))
                            dr.PlanFeeLinkId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlanFeeLink"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCPTCode"]))
                            dr.CPTCode = Convert.ToString(SearchedfieldsJSON["hfCPTCode"]);
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
                    //dsFeeSchedule.FeeGroupProceduralSchedule.AddFeeGroupProceduralScheduleRow(dr);
                    //dsFeeSchedule.FeeGroupProceduralSchedule.AcceptChanges();

                    if (dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProceduralSchedule.TableName].Rows.Count > 0)
                    {
                        //dsFeeSchedule.FeeGroupProceduralSchedule.Rows[0].SetModified();
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.UpdateFeeGroupProceduralSchedule(dsFeeSchedule);
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
                                message = "Cannot insert duplicate Fee Group Plan CPT.";

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
        /// Fills the procedure fee schedule.
        /// </summary>
        /// <param name="ProcedureFeeScheduleId">The procedure fee schedule identifier.</param>
        /// <returns></returns>
        private string FillProcedureFeeSchedule(Int64 ProcedureFeeScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ProcedureFeeScheduleId)))
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
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadFeeGroupProceduralSchedule(ProcedureFeeScheduleId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsFeeSchedule = obj.Data;
                            if (dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProceduralSchedule.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProceduralSchedule.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "ddlFeeGroup", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupProceduralSchedule.FeeGroupIdColumn.ColumnName])},
                            { "ddlPlanFeeLink", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupProceduralSchedule.PlanFeeLinkIdColumn.ColumnName])},
                            { "hfCPTCode", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupProceduralSchedule.CPTCodeColumn.ColumnName])},
                            { "txtCPTCode", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupProceduralSchedule.CPTCodeColumn.ColumnName])},
                            { "txtFee", string.Format("{0:N2}",dr[dsFeeSchedule.FeeGroupProceduralSchedule.FeeColumn.ColumnName])},
                            { "txtExpectedFee", string.Format("{0:N2}",dr[dsFeeSchedule.FeeGroupProceduralSchedule.ExpectedFeeColumn.ColumnName])},
                            { "chkAuthorizationRequired", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupProceduralSchedule.AuthorizationRequiredColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupProceduralSchedule.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    ProcedureFeeScheduleFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the procedure fee schedule.
        /// </summary>
        /// <param name="ProcedureFeeScheduleId">The procedure fee schedule identifier.</param>
        /// <returns></returns>
        private string DeleteProcedureFeeSchedule(Int64 ProcedureFeeScheduleId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ProcedureFeeScheduleId)))
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
                        BLObject<string> obj = BLLFeeScheduleObj.DeleteFeeGroupProceduralSchedule(MDVUtility.ToStr(ProcedureFeeScheduleId));
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
        /// Updates the procedure fee schedule is active.
        /// </summary>
        /// <param name="ProcedureFeeScheduleId">The procedure fee schedule identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateProcedureFeeScheduleIsActive(Int64 ProcedureFeeScheduleId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSFeeSchedule dsFeeSchedule = null;
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadFeeGroupProceduralSchedule(ProcedureFeeScheduleId, null, null, null, null);
                    dsFeeSchedule = obj.Data;
                    if (dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProceduralSchedule.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProceduralSchedule.TableName].Rows[0];
                        dr[dsFeeSchedule.FeeGroupProceduralSchedule.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSFeeSchedule> objProcedureFeeSchedule = BLLFeeScheduleObj.UpdateFeeGroupProceduralSchedule(dsFeeSchedule);
                        string successMsg;
                        if (objProcedureFeeSchedule.Data != null)
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
                                Message = objProcedureFeeSchedule.Message
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

        #region Fee Group Schedule Plan CPT Info
        /// <summary>
        /// Saves the BFS plan information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <returns></returns>
        private string SavePGPSPlanInfo(string fieldsJSON, int ProcedureFeeScheduleId, string RowId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeSchedule = new DSFeeSchedule();
                    DSFeeSchedule.FeeGroupProcModifierFeeScheduleRow dr = dsFeeSchedule.FeeGroupProcModifierFeeSchedule.NewFeeGroupProcModifierFeeScheduleRow();

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
                        dr.FeeGroupProcId = ProcedureFeeScheduleId;
                        dr.ModifierId = ModifierId;
                        dr.IsRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkRequired" + RowId]) == "True" ? true : false;
                        dr.IsActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        #region Database Insertion
                        dsFeeSchedule.FeeGroupProcModifierFeeSchedule.AddFeeGroupProcModifierFeeScheduleRow(dr);
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.InsertFeeGroupProceduralModifierSchedule(dsFeeSchedule);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                PGSPlanId = dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.TableName].Rows[0][dsFeeSchedule.FeeGroupProcModifierFeeSchedule.FeeGroupProcModifierFeeSchIdColumn.ColumnName]
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

        /// <summary>
        /// Updates the BFS plan information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <param name="BFSPlanId">The BFS plan identifier.</param>
        /// <returns></returns>
        private string UpdatePGPSPlanInfo(string fieldsJSON, int procedureFeeSchedulePlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeSchedule = new DSFeeSchedule();
                    BLObject<DSFeeSchedule> objPGP;

                    objPGP = BLLFeeScheduleObj.LoadFeeGroupProceduralModifierSchedule(0, procedureFeeSchedulePlanId);
                    if (objPGP.Data != null)
                    {
                        foreach (DSFeeSchedule.FeeGroupProcModifierFeeScheduleRow dr in objPGP.Data.Tables[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.TableName].Rows)
                        {
                            long ModifierId = 0;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfModifier" + procedureFeeSchedulePlanId]))
                            {
                                ModifierId = MDVUtility.ToInt64(SearchedfieldsJSON["hfModifier" + procedureFeeSchedulePlanId]);
                            }
                            if (ModifierId > 0)
                            {
                                dr.ModifierId = ModifierId;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifierFee" + procedureFeeSchedulePlanId]))
                                dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtModifierFee" + procedureFeeSchedulePlanId]);
                            dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtModifierExpectedFee" + procedureFeeSchedulePlanId]);
                            dr.IsRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkRequired" + procedureFeeSchedulePlanId]) == "True" ? true : false;

                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }


                        dsFeeSchedule = objPGP.Data;

                        #region Database Updation

                        if (dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.TableName].Rows.Count > 0)
                        {
                            BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.UpdateFeeGroupProceduralModifierSchedule(dsFeeSchedule);
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

        /// <summary>
        /// Loads the BFS plan information.
        /// </summary>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <returns></returns>
        private string LoadPGSPlanInfo(Int64 feeGroupProcId, Int64 procedureFeeSchedulePlanId)
        {
            try
            {
                DSFeeSchedule dsFeeSchedule = null;
                BLObject<DSFeeSchedule> obj;
                if (feeGroupProcId > 0)
                {
                    obj = BLLFeeScheduleObj.LoadFeeGroupProceduralModifierSchedule(feeGroupProcId, 0);

                    dsFeeSchedule = obj.Data;
                    var response = new
                    {
                        status = true,
                        PGSPlanCount = dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.TableName].Rows.Count,
                        PGSPlan_JSON = MDVUtility.JSON_DataTable(dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.TableName]),
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

        /// <summary>
        /// Fills the BFS plan information.
        /// </summary>
        /// <param name="BFSPlanId">The BFS plan identifier.</param>
        /// <returns></returns>
        private string FillPGSPlanInfo(Int64 FeeGroupProcModifierFeeSchId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(FeeGroupProcModifierFeeSchId)))
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
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadFeeGroupProceduralModifierSchedule(0, FeeGroupProcModifierFeeSchId);
                    if (obj.Data != null)
                    {
                        dsFeeSchedule = obj.Data;
                        if (dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsFeeSchedule.Tables[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "ddlModifier", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.ModifierIdColumn.ColumnName])},
                            { "txtModifierFee", string.Format("{0:N2}",dr[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.FeeColumn.ColumnName])},
                            { "txtModifierExpectedFee", string.Format("{0:N2}",dr[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.ExpectedFeeColumn.ColumnName])},
                            { "chkRequired", MDVUtility.ToStr(dr[dsFeeSchedule.FeeGroupProcModifierFeeSchedule.IsRequiredColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ProcedureFeeSchedulePlanFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the BFS plan information.
        /// </summary>
        /// <param name="BFSPlanId">The BFS plan identifier.</param>
        /// <returns></returns>
        private string DeletePGSPlanInfo(Int64 ProcedureFeeScheduleId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ProcedureFeeScheduleId)))
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
                        BLObject<string> obj = BLLFeeScheduleObj.DeleteFeeGroupProceduralModifierSchedule(MDVUtility.ToStr(ProcedureFeeScheduleId));
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
        /// Handle the Procedure Fee Schedule Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_PROCEDURE_FEE_SCHEDULE":
                    {
                        string fieldsJSON = context.Request["ProcedureFeeScheduleData"];
                        string strJSONData = SaveProcedureFeeSchedule(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PROCEDURE_FEE_SCHEDULE":
                    {
                        string strProviderId = context.Request["ProcedureFeeScheduleID"];
                        string strJSONData = FillProcedureFeeSchedule(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PROCEDURE_FEE_SCHEDULE":
                    {
                        string strProviderId = context.Request["ProcedureFeeScheduleID"];
                        string strJSONData = DeleteProcedureFeeSchedule(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PROCEDURE_FEE_SCHEDULE":
                    {
                        string fieldsJSON = context.Request["ProcedureFeeScheduleData"];
                        int ProviderID = MDVUtility.ToInt(context.Request["ProcedureFeeScheduleID"]);
                        string strJSONData = UpdateProcedureFeeSchedule(fieldsJSON, ProviderID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PROCEDURE_FEE_SCHEDULE_ACTIVE_INACTIVE":
                    {
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["ProcedureFeeScheduleID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateProcedureFeeScheduleIsActive(ProviderID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PROCEDURE_FEE_SCHEDULE_PLAN_CPT":
                    {
                        int ProcedureFeeScheduleId = MDVUtility.ToInt(context.Request["ProcedureFeeScheduleId"]);
                        string fieldsJSON = context.Request["ProcedureFeeSchedulePlanData"];
                        string RowId = context.Request["RowId"];
                        string strJSONData = SavePGPSPlanInfo(fieldsJSON, ProcedureFeeScheduleId, RowId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PROCEDURE_FEE_SCHEDULE_PLAN_CPT":
                    {
                        int procedureFeeSchedulePlanId = MDVUtility.ToInt(context.Request["procedureFeeSchedulePlanId"]);
                        string fieldsJSON = context.Request["ProcedureFeeSchedulePlanData"];
                        string strJSONData = UpdatePGPSPlanInfo(fieldsJSON, procedureFeeSchedulePlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PROCEDURE_FEE_SCHEDULE_PLAN_CPT":
                    {
                        Int64 feeGroupProcId = MDVUtility.ToInt64(context.Request["feeGroupProcId"]);
                        string strJSONData = LoadPGSPlanInfo(feeGroupProcId, 0);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PROCEDURE_FEE_SCHEDULE_PLAN_CPT":
                    {
                        Int64 ProcedureFeeScheduleId = MDVUtility.ToInt64(context.Request["procedureFeeSchedulePlanId"]);
                        string strJSONData = FillPGSPlanInfo(ProcedureFeeScheduleId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PROCEDURE_FEE_SCHEDULE_PLAN_CPT":
                    {
                        Int64 ProcedureFeeScheduleId = MDVUtility.ToInt64(context.Request["procedureFeeSchedulePlanId"]);
                        string strJSONData = DeletePGSPlanInfo(ProcedureFeeScheduleId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}