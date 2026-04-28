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
    public class Admin_BasicFeeSchedule_Detail
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_BasicFeeSchedule_Detail()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_BasicFeeSchedule_Detail _obj = null;
        public static Admin_BasicFeeSchedule_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_BasicFeeSchedule_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the basic fee schedule.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveBasicFeeSchedule(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Schedule", "ADDUPDATE_BASIC_FEE_SCHEDULE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeSchedule = new DSFeeSchedule();
                    DSFeeSchedule.BasicFeeScheduleRow dr = dsFeeSchedule.BasicFeeSchedule.NewBasicFeeScheduleRow();

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBasicFeeGroup"]))
                        dr.BasicFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBasicFeeGroup"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCPTCode"]))
                        dr.CPTCode = Convert.ToString(SearchedfieldsJSON["hfCPTCode"]);
                    dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtFee"]);
                    dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtExpectedFee"]);
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsFeeSchedule.BasicFeeSchedule.AddBasicFeeScheduleRow(dr);
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.InsertBasicFeeSchedule(dsFeeSchedule);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            BasicFeeScheduleId = dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchedule.TableName].Rows[0][dsFeeSchedule.BasicFeeSchedule.BasicFeeSchIdColumn.ColumnName]
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
        /// Updates the basic fee schedule.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <returns></returns>
        private string UpdateBasicFeeSchedule(string fieldsJSON, int BasicFeeScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Schedule", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeSchedule = new DSFeeSchedule();
                    //DSFeeSchedule.BasicFeeScheduleRow dr = dsFeeSchedule.BasicFeeSchedule.NewBasicFeeScheduleRow();
                    BLObject<DSFeeSchedule> objLoad = BLLFeeScheduleObj.LoadBasicFeeSchedule(BasicFeeScheduleId, null, null, null);
                    dsFeeSchedule = objLoad.Data;
                    foreach (DSFeeSchedule.BasicFeeScheduleRow dr in dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchedule.TableName].Rows)
                    {
                        //dr.BasicFeeSchId = BasicFeeScheduleId;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBasicFeeGroup"]))
                            dr.BasicFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBasicFeeGroup"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCPTCode"]))
                            dr.CPTCode = Convert.ToString(SearchedfieldsJSON["hfCPTCode"]);
                        dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtFee"]);
                        dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtExpectedFee"]);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsFeeSchedule.BasicFeeSchedule.AddBasicFeeScheduleRow(dr);
                    //dsFeeSchedule.BasicFeeSchedule.AcceptChanges();

                    if (dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchedule.TableName].Rows.Count > 0)
                    {
                        //dsFeeSchedule.BasicFeeSchedule.Rows[0].SetModified();
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.UpdateBasicFeeSchedule(dsFeeSchedule);
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
        /// Fills the basic fee schedule.
        /// </summary>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <returns></returns>
        private string FillBasicFeeSchedule(Int64 BasicFeeScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Schedule", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(BasicFeeScheduleId)))
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
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadBasicFeeSchedule(BasicFeeScheduleId, null, null, null);
                        if (obj.Data != null)
                        {
                            dsFeeSchedule = obj.Data;
                            if (dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchedule.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchedule.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "ddlBasicFeeGroup", MDVUtility.ToStr(dr[dsFeeSchedule.BasicFeeSchedule.BasicFeeGroupIdColumn.ColumnName])},
                            { "txtddlBasicFeeGroup", MDVUtility.ToStr(dr[dsFeeSchedule.BasicFeeSchedule.BasicFeeGroupNameColumn.ColumnName])},
                            { "hfCPTCode", MDVUtility.ToStr(dr[dsFeeSchedule.BasicFeeSchedule.CPTCodeColumn.ColumnName])},
                            { "txtCPTCode", MDVUtility.ToStr(dr[dsFeeSchedule.BasicFeeSchedule.CPTCodeColumn.ColumnName])},
                            { "txtFee", string.Format("{0:N2}", dr[dsFeeSchedule.BasicFeeSchedule.FeeColumn.ColumnName])},
                            { "txtExpectedFee",string.Format("{0:N2}",dr[dsFeeSchedule.BasicFeeSchedule.ExpectedFeeColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsFeeSchedule.BasicFeeSchedule.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    BasicFeeScheduleFill_JSON = js.Serialize(keyValues)
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {

                                var response = new
                                {
                                    status = false,
                                    Message = AppPrivileges.No_Record_Message
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
        /// Deletes the basic fee schedule.
        /// </summary>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <returns></returns>
        private string DeleteBasicFeeSchedule(Int64 BasicFeeScheduleId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Schedule", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(BasicFeeScheduleId)))
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
                        BLObject<string> obj = BLLFeeScheduleObj.DeleteBasicFeeSchedule(MDVUtility.ToStr(BasicFeeScheduleId));
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
        /// Updates the basic fee schedule is active.
        /// </summary>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateBasicFeeScheduleIsActive(Int64 BasicFeeScheduleId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Schedule", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSFeeSchedule dsFeeSchedule = null;
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadBasicFeeSchedule(BasicFeeScheduleId, null, null, null);
                    dsFeeSchedule = obj.Data;
                    if (dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchedule.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchedule.TableName].Rows[0];
                        dr[dsFeeSchedule.BasicFeeSchedule.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSFeeSchedule> objBasicFeeSchedule = BLLFeeScheduleObj.UpdateBasicFeeSchedule(dsFeeSchedule);
                        string successMsg;
                        if (objBasicFeeSchedule.Data != null)
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
                                Message = objBasicFeeSchedule.Message
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

        #region Basic Fee Schedule Plan Info
        /// <summary>
        /// Saves the BFS plan information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <returns></returns>
        private string SaveBFSPlanInfo(string fieldsJSON, int BasicFeeScheduleId, string RowId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Schedule", "ADDDELETE_BFS_PLAN_INFO")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFeeSchedule = new DSFeeSchedule();
                    DSFeeSchedule.BasicFeeSchModifierFeeScheduleRow dr = dsFeeSchedule.BasicFeeSchModifierFeeSchedule.NewBasicFeeSchModifierFeeScheduleRow();

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
                        dr.BasicFeeSchId = BasicFeeScheduleId;
                        dr.ModifierId = ModifierId;
                        dr.IsRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkRequired" + RowId]) == "True" ? true : false;
                        dr.IsActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        #region Database Insertion
                        dsFeeSchedule.BasicFeeSchModifierFeeSchedule.AddBasicFeeSchModifierFeeScheduleRow(dr);
                        BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.InsertBFSPlanInfo(dsFeeSchedule);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                BFSPlanId = dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchModifierFeeSchedule.TableName].Rows[0][dsFeeSchedule.BasicFeeSchModifierFeeSchedule.BasicFeeSchModifierFeeSchIdColumn.ColumnName]
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
        private string UpdateBFSPlanInfo(string fieldsJSON, int BFSPlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Schedule", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);


                    BLObject<DSFeeSchedule> objCode;
                    DSFeeSchedule dsFeeSchedule = new DSFeeSchedule();
                    objCode = BLLFeeScheduleObj.LoadBFSPlanInfo(0, BFSPlanId);
                    if (objCode.Data != null)
                    {
                        foreach (DSFeeSchedule.BasicFeeSchModifierFeeScheduleRow dr in objCode.Data.Tables[dsFeeSchedule.BasicFeeSchModifierFeeSchedule.TableName].Rows)
                        {
                            long ModifierId = 0;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfModifier" + BFSPlanId]))
                            {
                                ModifierId = MDVUtility.ToInt64(SearchedfieldsJSON["hfModifier" + BFSPlanId]);
                            }
                            if (ModifierId > 0)
                            {
                                dr.ModifierId = ModifierId;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifierFee" + BFSPlanId]))
                                dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtModifierFee" + BFSPlanId]);
                            dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtModifierExpectedFee" + BFSPlanId]);
                            dr.IsRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkRequired" + BFSPlanId]) == "True" ? true : false;

                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }

                        dsFeeSchedule = objCode.Data;

                        #region Database Updation

                        if (dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchModifierFeeSchedule.TableName].Rows.Count > 0)
                        {
                            BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.UpdateBFSPlanInfo(dsFeeSchedule);
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
        private string LoadBFSPlanInfo(Int64 BasicFeeScheduleId)
        {
            try
            {
                DSFeeSchedule dsFeeSchedule = null;
                BLObject<DSFeeSchedule> obj;
                if (BasicFeeScheduleId > 0)
                {
                    obj = BLLFeeScheduleObj.LoadBFSPlanInfo(BasicFeeScheduleId, 0);

                    dsFeeSchedule = obj.Data;
                    var response = new
                    {
                        status = true,
                        BFSPlanCount = dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchModifierFeeSchedule.TableName].Rows.Count,
                        BFSPlan_JSON = MDVUtility.JSON_DataTable(dsFeeSchedule.Tables[dsFeeSchedule.BasicFeeSchModifierFeeSchedule.TableName]),
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
        private string FillBFSPlanInfo(Int64 BFSPlanId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(BFSPlanId)))
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
                    DSFeeSchedule dsInsurance = null;
                    BLObject<DSFeeSchedule> obj = BLLFeeScheduleObj.LoadBFSPlanInfo(0, BFSPlanId);
                    if (obj.Data != null)
                    {
                        dsInsurance = obj.Data;
                        if (dsInsurance.Tables[dsInsurance.BasicFeeSchModifierFeeSchedule.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsInsurance.Tables[dsInsurance.BasicFeeSchModifierFeeSchedule.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "ddlModifier", MDVUtility.ToStr(dr[dsInsurance.BasicFeeSchModifierFeeSchedule.ModifierIdColumn.ColumnName])},
                            { "txtModifierFee", string.Format("{0:N2}",dr[dsInsurance.BasicFeeSchModifierFeeSchedule.FeeColumn.ColumnName])},
                            { "chkRequired", MDVUtility.ToStr(dr[dsInsurance.BasicFeeSchModifierFeeSchedule.IsRequiredColumn.ColumnName])},
                            { "txtModifierExpectedFee",string.Format("{0:N2}",dr[dsInsurance.BasicFeeSchModifierFeeSchedule.ExpectedFeeColumn.ColumnName])},
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                BasicFeeSchedulePlanFill_JSON = js.Serialize(keyValues)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Deletes the BFS plan information.
        /// </summary>
        /// <param name="BFSPlanId">The BFS plan identifier.</param>
        /// <returns></returns>
        private string DeleteBFSPlanInfo(Int64 BFSPlanId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Basic Fee Schedule", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(BFSPlanId)))
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
                        BLObject<string> obj = BLLFeeScheduleObj.DeleteBFSPlanInfo(MDVUtility.ToStr(BFSPlanId));
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
        /// Handle the Basic Fee Schedule Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_BASIC_FEE_SCHEDULE":
                    {
                        string fieldsJSON = context.Request["BasicFeeScheduleData"];
                        string strJSONData = SaveBasicFeeSchedule(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_BASIC_FEE_SCHEDULE":
                    {
                        string strProviderId = context.Request["BasicFeeScheduleID"];
                        string strJSONData = FillBasicFeeSchedule(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_BASIC_FEE_SCHEDULE":
                    {
                        string strProviderId = context.Request["BasicFeeScheduleID"];
                        string strJSONData = DeleteBasicFeeSchedule(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BASIC_FEE_SCHEDULE":
                    {
                        string fieldsJSON = context.Request["BasicFeeScheduleData"];
                        int ProviderID = MDVUtility.ToInt(context.Request["BasicFeeScheduleID"]);
                        string strJSONData = UpdateBasicFeeSchedule(fieldsJSON, ProviderID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BASIC_FEE_SCHEDULE_ACTIVE_INACTIVE":
                    {
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["BasicFeeScheduleID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateBasicFeeScheduleIsActive(ProviderID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_BFS_PLAN_INFO":
                    {
                        int BasicFeeScheduleID = MDVUtility.ToInt(context.Request["BasicFeeScheduleId"]);
                        string fieldsJSON = context.Request["BasicFeeSchedulePlanData"];
                        string RowId = context.Request["RowId"];
                        string strJSONData = SaveBFSPlanInfo(fieldsJSON, BasicFeeScheduleID, RowId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_BFS_PLAN_INFO":
                    {
                        string fieldsJSON = context.Request["BasicFeeSchedulePlanData"];
                        int BFSPlanId = MDVUtility.ToInt(context.Request["BasicFeeSchedulePlanID"]);
                        string strJSONData = UpdateBFSPlanInfo(fieldsJSON, BFSPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_BFS_PLAN_INFO":
                    {
                        Int64 BasicFeeScheduleID = MDVUtility.ToInt64(context.Request["BasicFeeScheduleId"]);
                        string strJSONData = LoadBFSPlanInfo(BasicFeeScheduleID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_BFS_PLAN_INFO":
                    {
                        Int64 InsurancePlanAddressID = MDVUtility.ToInt64(context.Request["BasicFeeSchedulePlanId"]);
                        string strJSONData = FillBFSPlanInfo(InsurancePlanAddressID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_BFS_PLAN_INFO":
                    {
                        Int64 BFSPlanId = MDVUtility.ToInt64(context.Request["BasicFeeSchedulePlanId"]);
                        string strJSONData = DeleteBFSPlanInfo(BFSPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}