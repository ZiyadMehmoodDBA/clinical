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
    public class Admin_RevenueCode_Detail
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_RevenueCode_Detail()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_RevenueCode_Detail _obj = null;
        public static Admin_RevenueCode_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_RevenueCode_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the Revenue Code.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveRevenueCode(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Revenue Code", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsRevenueCode = new DSCodes();
                    DSCodes.RevenueCodeRow dr = dsRevenueCode.RevenueCode.NewRevenueCodeRow();

                    dr.RevenueCode = SearchedfieldsJSON["txtRevenueCode"];
                    //dr.Name = "";
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                    #region Database Insertion
                    dsRevenueCode.RevenueCode.AddRevenueCodeRow(dr);
                    BLObject<DSCodes> obj = BLLAdminCodesObj.InsertRevenueCode(ref dsRevenueCode);
                    dsRevenueCode = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            RevenueCodeId = dsRevenueCode.Tables[dsRevenueCode.RevenueCode.TableName].Rows[0][dsRevenueCode.RevenueCode.RevenueCodeIdColumn.ColumnName]
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
        /// Updates the Revenue Code.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProcedureCategoryId">The procedure category identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateRevenueCode(string fieldsJSON, Int64 RevenueCodeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Revenue Code", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsRevenueCode = new DSCodes();
                    //DSCodes.RevenueCodeRow dr = dsRevenueCode.RevenueCode.NewRevenueCodeRow();
                    BLObject<DSCodes> objLoad = BLLAdminCodesObj.LoadRevenueCode(RevenueCodeId, null, null, null, null);
                    dsRevenueCode = objLoad.Data;
                    foreach (DSCodes.RevenueCodeRow dr in dsRevenueCode.Tables[dsRevenueCode.RevenueCode.TableName].Rows)
                    {
                        //dr.RevenueCodeId = RevenueCodeId;
                        dr.RevenueCode = SearchedfieldsJSON["txtRevenueCode"];
                        //dr.Name = "";
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    }

                    #region Database Updation
                    //dsRevenueCode.RevenueCode.AddRevenueCodeRow(dr);
                    //dsRevenueCode.RevenueCode.AcceptChanges();

                    if (dsRevenueCode.Tables[dsRevenueCode.RevenueCode.TableName].Rows.Count > 0)
                    {
                        //dsRevenueCode.RevenueCode.Rows[0].SetModified();
                        BLObject<DSCodes> obj = BLLAdminCodesObj.UpdateRevenueCode(ref dsRevenueCode);
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
        /// Fills the Revenue Code.
        /// </summary>
        /// <param name="ProcedureCategoryId">The procedure category identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillRevenueCode(Int64 RevenueCodeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Revenue Code", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(RevenueCodeId)))
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
                        DSCodes dsRevenueCode = null;
                        BLObject<DSCodes> obj = BLLAdminCodesObj.LoadRevenueCode(RevenueCodeId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsRevenueCode = obj.Data;
                            if (dsRevenueCode.Tables[dsRevenueCode.RevenueCode.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsRevenueCode.Tables[dsRevenueCode.RevenueCode.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "txtRevenueCode", MDVUtility.ToStr(dr[dsRevenueCode.RevenueCode.RevenueCodeColumn.ColumnName])},
                          { "txtDescription", MDVUtility.ToStr(dr[dsRevenueCode.RevenueCode.DescriptionColumn.ColumnName])},
                          { "ChkIsActive", MDVUtility.ToStr(dr[dsRevenueCode.RevenueCode.IsActiveColumn.ColumnName])},
                          { "ddlEntity", MDVUtility.ToStr(dr[dsRevenueCode.RevenueCode.EntityIdColumn.ColumnName])},
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    RevenueCode_JSON = js.Serialize(keyValues)
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
        /// Deletes the Revenue Code.
        /// </summary>
        /// <param name="ProcedureCategoryId">The procedure category identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteRevenueCode(Int64 RevenueCodeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Revenue Code", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(RevenueCodeId)))
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
                        BLObject<string> obj = BLLAdminCodesObj.DeleteRevenueCode(MDVUtility.ToStr(RevenueCodeId));
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
        /// Updates the revenue code is active.
        /// </summary>
        /// <param name="RevenueCodeId">The revenue code identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateRevenueCodeIsActive(Int64 RevenueCodeId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Revenue Code", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSCodes dsCode = null;
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadRevenueCode(RevenueCodeId, null, null, null, null);
                    dsCode = obj.Data;
                    if (dsCode.Tables[dsCode.RevenueCode.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCode.Tables[dsCode.RevenueCode.TableName].Rows[0];
                        dr[dsCode.RevenueCode.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSCodes> objUser = BLLAdminCodesObj.UpdateRevenueCode(ref dsCode);
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

        #region Revenue Code Plan Info
        /// <summary>
        /// Saves the revenue code plan information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="RevenueCodeId">The revenue code identifier.</param>
        /// <returns></returns>
        private string SaveRevenueCodePlanInfo(string fieldsJSON, string RevenueCodeID, string RowId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Revenue Code", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    DSCodes.RevenuePlanRow dr = dsCodes.RevenuePlan.NewRevenuePlanRow();

                    long InPlanId = 0;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPlan" + RowId]))
                    {
                        InPlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPlan" + RowId]);
                    }

                    if (InPlanId > 0)
                    {
                        if (!string.IsNullOrEmpty(RevenueCodeID))
                            dr.RevenueCodeId = MDVUtility.ToInt64(RevenueCodeID);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCode" + RowId]))
                            dr.Name = MDVUtility.ToStr(SearchedfieldsJSON["txtCode" + RowId]);

                        dr.InsurancePlanId = InPlanId;

                        #region Database Insertion
                        dsCodes.RevenuePlan.AddRevenuePlanRow(dr);
                        BLObject<DSCodes> obj = BLLAdminCodesObj.InsertRevenueCodePlanInfo(dsCodes);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                RevenueCodePlanId = dsCodes.Tables[dsCodes.RevenuePlan.TableName].Rows[0][dsCodes.RevenuePlan.RevenuePlanIdColumn.ColumnName]
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
        /// Updates the revenue code plan information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="RevenueCodeId">The revenue code identifier.</param>
        /// <param name="RevenueCodePlanId">The revenue code plan identifier.</param>
        /// <returns></returns>
        private string UpdateRevenueCodePlanInfo(string fieldsJSON, Int64 RevenueCodePlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Revenue Code", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    BLObject<DSCodes> objCode;
                    DSCodes dsCode = new DSCodes();
                    objCode = BLLAdminCodesObj.LoadRevenueCodePlanInfo(0, RevenueCodePlanId);
                    if (objCode.Data != null)
                    {
                        foreach (DSCodes.RevenuePlanRow dr in objCode.Data.Tables[dsCode.RevenuePlan.TableName].Rows)
                        {
                            long InPlanId = 0;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPlan" + RevenueCodePlanId]))
                            {
                                InPlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPlan" + RevenueCodePlanId]);
                            }
                            if (InPlanId > 0)
                            {
                                dr.InsurancePlanId = InPlanId;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCode" + RevenueCodePlanId]))
                                dr.Name = MDVUtility.ToStr(SearchedfieldsJSON["txtCode" + RevenueCodePlanId]);
                        }

                        dsCode = objCode.Data;

                        #region Database Update

                        BLObject<DSCodes> obj = BLLAdminCodesObj.UpdateRevenueCodePlanInfo(dsCode);

                        if (dsCode.Tables[dsCode.RevenuePlan.TableName].Rows.Count > 0)
                        {
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
        /// Loads the revenue code plan information.
        /// </summary>
        /// <param name="RevenueCodeId">The revenue code identifier.</param>
        /// <returns></returns>
        private string LoadRevenueCodePlanInfo(Int64 RevenueCodeId)
        {
            try
            {
                DSCodes dsCodes = null;
                BLObject<DSCodes> obj;
                if (RevenueCodeId > 0)
                {
                    obj = BLLAdminCodesObj.LoadRevenueCodePlanInfo(RevenueCodeId, 0);

                    dsCodes = obj.Data;
                    var response = new
                    {
                        status = true,
                        RevenueCodePlanCount = dsCodes.Tables[dsCodes.RevenuePlan.TableName].Rows.Count,
                        RevenueCodePlan_JSON = MDVUtility.JSON_DataTable(dsCodes.Tables[dsCodes.RevenuePlan.TableName]),
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
        /// Fills the revenue code plan information.
        /// </summary>
        /// <param name="RevenueCodePlanId">The revenue code plan identifier.</param>
        /// <returns></returns>
        private string FillRevenueCodePlanInfo(Int64 RevenueCodePlanId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(RevenueCodePlanId)))
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
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadRevenueCodePlanInfo(0, RevenueCodePlanId);
                    if (obj.Data != null)
                    {
                        dsCodes = obj.Data;
                        if (dsCodes.Tables[dsCodes.RevenuePlan.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsCodes.Tables[dsCodes.RevenuePlan.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "ddlInsurancePlan", MDVUtility.ToStr(dr[dsCodes.RevenuePlan.InsurancePlanIdColumn.ColumnName])},
                            { "txtCode", MDVUtility.ToStr(dr[dsCodes.RevenuePlan.NameColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                RevenueCodePlanFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the revenue code plan information.
        /// </summary>
        /// <param name="RevenueCodePlanId">The revenue code plan identifier.</param>
        /// <returns></returns>
        private string DeleteRevenueCodePlanInfo(Int64 RevenueCodePlanId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Revenue Code", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(RevenueCodePlanId)))
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
                        BLObject<string> obj = BLLAdminCodesObj.DeleteRevenueCodePlanInfo(MDVUtility.ToStr(RevenueCodePlanId));
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
        /// Handle the Revenue Code Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_REVENUE_CODE":
                    {
                        string fieldsJSON = context.Request["RevenueCodeData"];
                        string strJSONData = SaveRevenueCode(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_REVENUE_CODE":
                    {
                        string strRevenueCodeId = context.Request["RevenueCodeID"];
                        string strJSONData = FillRevenueCode(MDVUtility.ToInt64(strRevenueCodeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_REVENUE_CODE":
                    {
                        string strRevenueCodeId = context.Request["RevenueCodeID"];
                        string strJSONData = DeleteRevenueCode(MDVUtility.ToInt64(strRevenueCodeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_REVENUE_CODE":
                    {
                        string fieldsJSON = context.Request["RevenueCodeData"];
                        Int64 RevenueCodeID = MDVUtility.ToInt64(context.Request["RevenueCodeID"]);
                        string strJSONData = UpdateRevenueCode(fieldsJSON, RevenueCodeID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_REVENUE_CODE_ACTIVE_INACTIVE":
                    {
                        Int64 RevenueCodeID = MDVUtility.ToInt64(context.Request["RevenueCodeID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateRevenueCodeIsActive(RevenueCodeID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_REVENUE_CODE_INFO":
                    {
                        string RevenueCodeID = (context.Request["RevenueCodeID"]);
                        string RowId = context.Request["RowId"];
                        string fieldsJSON = context.Request["RevenueCodePlanData"];
                        string strJSONData = SaveRevenueCodePlanInfo(fieldsJSON, RevenueCodeID, RowId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_REVENUE_CODE_INFO":
                    {
                        string fieldsJSON = context.Request["RevenueCodePlanData"];
                        Int64 RevenueCodePlanId = MDVUtility.ToInt64(context.Request["RevenueCodePlanID"]);
                        string strJSONData = UpdateRevenueCodePlanInfo(fieldsJSON, RevenueCodePlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_REVENUE_CODE_INFO":
                    {
                        Int64 RevenueCodeID = MDVUtility.ToInt64(context.Request["RevenueCodeID"]);
                        string strJSONData = LoadRevenueCodePlanInfo(RevenueCodeID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_REVENUE_CODE_INFO":
                    {
                        Int64 RevenueCodePlanId = MDVUtility.ToInt64(context.Request["RevenueCodePlanId"]);
                        string strJSONData = FillRevenueCodePlanInfo(RevenueCodePlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_REVENUE_CODE_INFO":
                    {
                        Int64 RevenueCodePlanId = MDVUtility.ToInt64(context.Request["RevenueCodePlanId"]);
                        string strJSONData = DeleteRevenueCodePlanInfo(RevenueCodePlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}