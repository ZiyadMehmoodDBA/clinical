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
    public class Admin_TypeOfService_Detail
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_TypeOfService_Detail()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_TypeOfService_Detail _obj = null;
        public static Admin_TypeOfService_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_TypeOfService_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the type of service.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveTypeOfService(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Type Of Service", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    DSCodes.TypeOfServiceRow dr = dsCodes.TypeOfService.NewTypeOfServiceRow();

                    dr.TypeOfServiceCode = SearchedfieldsJSON["txtCode"];
                    dr.Name = SearchedfieldsJSON["txtName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsCodes.TypeOfService.AddTypeOfServiceRow(dr);
                    BLObject<DSCodes> obj = BLLAdminCodesObj.InsertTypeOfService(ref dsCodes);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            TypeOfServiceId = dsCodes.Tables[dsCodes.TypeOfService.TableName].Rows[0][dsCodes.TypeOfService.TOSIdColumn.ColumnName]
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
        /// Updates the type of service.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="TypeOfServiceId">The type of service identifier.</param>
        /// <returns></returns>
        private string UpdateTypeOfService(string fieldsJSON, int TypeOfServiceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Type Of Service", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    //DSCodes.TypeOfServiceRow dr = dsCodes.TypeOfService.NewTypeOfServiceRow();
                    BLObject<DSCodes> objLoad = BLLAdminCodesObj.LoadTypeOfService(TypeOfServiceId, null, null, null, null);
                    dsCodes = objLoad.Data;
                    foreach (DSCodes.TypeOfServiceRow dr in dsCodes.Tables[dsCodes.TypeOfService.TableName].Rows)
                    {
                        //dr.TOSId = TypeOfServiceId;
                        dr.TypeOfServiceCode = SearchedfieldsJSON["txtCode"];
                        dr.Name = SearchedfieldsJSON["txtName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsCodes.TypeOfService.AddTypeOfServiceRow(dr);
                    //dsCodes.TypeOfService.AcceptChanges();

                    if (dsCodes.Tables[dsCodes.TypeOfService.TableName].Rows.Count > 0)
                    {
                        //dsCodes.TypeOfService.Rows[0].SetModified();
                        BLObject<DSCodes> obj = BLLAdminCodesObj.UpdateTypeOfService(ref dsCodes);
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
        /// Fills the type of service.
        /// </summary>
        /// <param name="TypeOfServiceId">The type of service identifier.</param>
        /// <returns></returns>
        private string FillTypeOfService(Int64 TypeOfServiceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Type Of Service", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(TypeOfServiceId)))
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
                        BLObject<DSCodes> obj = BLLAdminCodesObj.LoadTypeOfService(TypeOfServiceId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsCodes = obj.Data;
                            if (dsCodes.Tables[dsCodes.TypeOfService.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsCodes.Tables[dsCodes.TypeOfService.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtCode", MDVUtility.ToStr(dr[dsCodes.TypeOfService.TypeOfServiceCodeColumn.ColumnName])},
                            { "txtName", MDVUtility.ToStr(dr[dsCodes.TypeOfService.NameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsCodes.TypeOfService.DescriptionColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsCodes.TypeOfService.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    TypeOfServiceFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the type of service.
        /// </summary>
        /// <param name="TypeOfServiceId">The type of service identifier.</param>
        /// <returns></returns>
        private string DeleteTypeOfService(Int64 TypeOfServiceId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Type Of Service", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(TypeOfServiceId)))
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
                        BLObject<string> obj = BLLAdminCodesObj.DeleteTypeOfService(MDVUtility.ToStr(TypeOfServiceId));
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
        /// Updates the type of service is active.
        /// </summary>
        /// <param name="TypeOfServiceId">The type of service identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateTypeOfServiceIsActive(Int64 TypeOfServiceId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Type Of Service", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSCodes dsCodes = null;
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadTypeOfService(TypeOfServiceId, null, null, null, null);
                    dsCodes = obj.Data;
                    if (dsCodes.Tables[dsCodes.TypeOfService.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCodes.Tables[dsCodes.TypeOfService.TableName].Rows[0];
                        dr[dsCodes.TypeOfService.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSCodes> objTypeOfService = BLLAdminCodesObj.UpdateTypeOfService(ref dsCodes);
                        string successMsg;
                        if (objTypeOfService.Data != null)
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
                                Message = objTypeOfService.Message
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

        #region Type of Service Plan Info
        /// <summary>
        /// Saves the tos plan information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="TypeOfServiceId">The type of service identifier.</param>
        /// <returns></returns>
        private string SaveTOSPlanInfo(string fieldsJSON, string TypeOfServiceId, string RowId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Type Of Service", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    DSCodes.PlanOfTOSRow dr = dsCodes.PlanOfTOS.NewPlanOfTOSRow();

                    long InPlanId = 0;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPlan" + RowId]))
                    {
                        InPlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPlan" + RowId]);
                    }

                    if (InPlanId > 0)
                    {
                        dr.InsurancePlanId = InPlanId;
                        if (!string.IsNullOrEmpty(TypeOfServiceId))
                            dr.TypeOfServiceId = MDVUtility.ToInt64(TypeOfServiceId);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtTOS" + RowId]))
                            dr.Name = MDVUtility.ToStr(SearchedfieldsJSON["txtTOS" + RowId]);


                        dr.IsActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        #region Database Insertion
                        dsCodes.PlanOfTOS.AddPlanOfTOSRow(dr);
                        BLObject<DSCodes> obj = BLLAdminCodesObj.InsertTOSPlanInfo(ref dsCodes);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                TOSPlanId = dsCodes.Tables[dsCodes.PlanOfTOS.TableName].Rows[0][dsCodes.PlanOfTOS.PlanOfTOSIdColumn.ColumnName]
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
        /// Updates the tos plan information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="TypeOfServiceId">The type of service identifier.</param>
        /// <param name="TOSPlanId">The tos plan identifier.</param>
        /// <returns></returns>
        private string UpdateTOSPlanInfo(string fieldsJSON, int TOSPlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Type Of Service", "Edit")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    BLObject<DSCodes> objCode;
                    DSCodes dsCode = new DSCodes();
                    objCode = BLLAdminCodesObj.LoadTOSPlanInfo(0, TOSPlanId);
                    if (objCode.Data != null)
                    {
                        foreach (DSCodes.PlanOfTOSRow dr in objCode.Data.Tables[dsCode.PlanOfTOS.TableName].Rows)
                        {
                            long InPlanId = 0;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPlan" + TOSPlanId]))
                            {
                                InPlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPlan" + TOSPlanId]);
                            }
                            if (InPlanId > 0)
                            {
                                dr.InsurancePlanId = InPlanId;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtTOS" + TOSPlanId]))
                                dr.Name = MDVUtility.ToStr(SearchedfieldsJSON["txtTOS" + TOSPlanId]);
                        }

                        dsCode = objCode.Data;

                        #region Database Updation

                        if (dsCode.Tables[dsCode.PlanOfTOS.TableName].Rows.Count > 0)
                        {
                            BLObject<DSCodes> obj = BLLAdminCodesObj.UpdateTOSPlanInfo(ref dsCode);
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
        /// Loads the tos plan information.
        /// </summary>
        /// <param name="TypeOfServiceId">The type of service identifier.</param>
        /// <returns></returns>
        private string LoadTOSPlanInfo(Int64 TypeOfServiceId)
        {
            try
            {
                DSCodes dsCodes = null;
                BLObject<DSCodes> obj;
                if (TypeOfServiceId > 0)
                {
                    obj = BLLAdminCodesObj.LoadTOSPlanInfo(TypeOfServiceId, 0);

                    dsCodes = obj.Data;
                    var response = new
                    {
                        status = true,
                        TOSPlanCount = dsCodes.Tables[dsCodes.PlanOfTOS.TableName].Rows.Count,
                        TOSPlan_JSON = MDVUtility.JSON_DataTable(dsCodes.Tables[dsCodes.PlanOfTOS.TableName]),
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
        /// Deletes the tos plan information.
        /// </summary>
        /// <param name="TOSPlanId">The tos plan identifier.</param>
        /// <returns></returns>
        private string DeleteTOSPlanInfo(Int64 TOSPlanId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Type Of Service", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(TOSPlanId)))
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
                        BLObject<string> obj = BLLAdminCodesObj.DeleteTOSPlanInfo(MDVUtility.ToStr(TOSPlanId));
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
        /// Fills the tos plan information.
        /// </summary>
        /// <param name="TOSPlanId">The tos plan identifier.</param>
        /// <returns></returns>
        private string FillTOSPlanInfo(Int64 TOSPlanId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(TOSPlanId)))
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
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadTOSPlanInfo(0, TOSPlanId);
                    if (obj.Data != null)
                    {
                        dsCodes = obj.Data;
                        if (dsCodes.Tables[dsCodes.PlanOfTOS.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsCodes.Tables[dsCodes.PlanOfTOS.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "ddlInsurancePlan", MDVUtility.ToStr(dr[dsCodes.PlanOfTOS.InsurancePlanIdColumn.ColumnName])},
                            { "txtTOS", MDVUtility.ToStr(dr[dsCodes.PlanOfTOS.NameColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                TOSPlanFill_JSON = js.Serialize(keyValues)
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
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Type Of Service Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_TYPE_OF_SERVICE":
                    {
                        string fieldsJSON = context.Request["TypeOfServiceData"];
                        string strJSONData = SaveTypeOfService(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_TYPE_OF_SERVICE":
                    {
                        string strProviderId = context.Request["TypeOfServiceID"];
                        string strJSONData = FillTypeOfService(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_TYPE_OF_SERVICE":
                    {
                        string strProviderId = context.Request["TypeOfServiceID"];
                        string strJSONData = DeleteTypeOfService(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_TYPE_OF_SERVICE":
                    {
                        string fieldsJSON = context.Request["TypeOfServiceData"];
                        int ProviderID = MDVUtility.ToInt(context.Request["TypeOfServiceID"]);
                        string strJSONData = UpdateTypeOfService(fieldsJSON, ProviderID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_TYPE_OF_SERVICE_ACTIVE_INACTIVE":
                    {
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["TypeOfServiceID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateTypeOfServiceIsActive(ProviderID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_TOS_PLAN_INFO":
                    {

                        string fieldsJSON = context.Request["TOSPlanData"];
                        string RowId = context.Request["RowId"];
                        string TypeOfServiceID = context.Request["TypeOfServiceID"];
                        string strJSONData = SaveTOSPlanInfo(fieldsJSON, TypeOfServiceID, RowId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_TOS_PLAN_INFO":
                    {
                        string fieldsJSON = context.Request["TOSPlanData"];
                        int TOSPlanId = MDVUtility.ToInt(context.Request["TOSPlanId"]);
                        string strJSONData = UpdateTOSPlanInfo(fieldsJSON, TOSPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_TOS_PLAN_INFO":
                    {
                        Int64 TypeOfServiceID = MDVUtility.ToInt64(context.Request["TypeOfServiceID"]);
                        string strJSONData = LoadTOSPlanInfo(TypeOfServiceID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_TOS_PLAN_INFO":
                    {
                        Int64 TOSPlanId = MDVUtility.ToInt64(context.Request["TOSPlanId"]);
                        string strJSONData = FillTOSPlanInfo(TOSPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_TOS_PLAN_INFO":
                    {
                        Int64 TOSPlanId = MDVUtility.ToInt64(context.Request["TOSPlanId"]);
                        string strJSONData = DeleteTOSPlanInfo(TOSPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}