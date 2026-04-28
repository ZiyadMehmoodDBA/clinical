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
    public class Admin_PlaceOfService_Detail
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_PlaceOfService_Detail()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_PlaceOfService_Detail _obj = null;
        public static Admin_PlaceOfService_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_PlaceOfService_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the place of service.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SavePlaceOfService(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Place Of Service", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    DSCodes.PlaceOfServiceRow dr = dsCodes.PlaceOfService.NewPlaceOfServiceRow();

                    dr.POSCode = SearchedfieldsJSON["txtCode"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsCodes.PlaceOfService.AddPlaceOfServiceRow(dr);
                    BLObject<DSCodes> obj = BLLAdminCodesObj.InsertPlaceOfService(dsCodes);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            PlaceOfServiceId = dsCodes.Tables[dsCodes.PlaceOfService.TableName].Rows[0][dsCodes.PlaceOfService.POSIdColumn.ColumnName]
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
        /// Updates the place of service.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PlaceOfServiceId">The place of service identifier.</param>
        /// <returns></returns>
        private string UpdatePlaceOfService(string fieldsJSON, int PlaceOfServiceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Place Of Service", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    //DSCodes.PlaceOfServiceRow dr = dsCodes.PlaceOfService.NewPlaceOfServiceRow();
                    BLObject<DSCodes> objLoad = BLLAdminCodesObj.LoadPlaceOfService(PlaceOfServiceId, null, null, null);
                    dsCodes = objLoad.Data;
                    foreach (DSCodes.PlaceOfServiceRow dr in dsCodes.Tables[dsCodes.PlaceOfService.TableName].Rows)
                    {
                        //dr.POSId = PlaceOfServiceId;
                        dr.POSCode = SearchedfieldsJSON["txtCode"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false; ;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsCodes.PlaceOfService.AddPlaceOfServiceRow(dr);
                    //dsCodes.PlaceOfService.AcceptChanges();

                    if (dsCodes.Tables[dsCodes.PlaceOfService.TableName].Rows.Count > 0)
                    {
                        //dsCodes.PlaceOfService.Rows[0].SetModified();
                        BLObject<DSCodes> obj = BLLAdminCodesObj.UpdatePlaceOfService(dsCodes);
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
        /// Fills the place of service.
        /// </summary>
        /// <param name="PlaceOfServiceId">The place of service identifier.</param>
        /// <returns></returns>
        private string FillPlaceOfService(Int64 PlaceOfServiceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Place Of Service", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PlaceOfServiceId)))
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
                        BLObject<DSCodes> obj = BLLAdminCodesObj.LoadPlaceOfService(PlaceOfServiceId, null, null, null);
                        if (obj.Data != null)
                        {
                            dsCodes = obj.Data;
                            if (dsCodes.Tables[dsCodes.PlaceOfService.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsCodes.Tables[dsCodes.PlaceOfService.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtCode", MDVUtility.ToStr(dr[dsCodes.PlaceOfService.POSCodeColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsCodes.PlaceOfService.DescriptionColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsCodes.PlaceOfService.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    PlaceOfServiceFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the place of service.
        /// </summary>
        /// <param name="PlaceOfServiceId">The place of service identifier.</param>
        /// <returns></returns>
        private string DeletePlaceOfService(Int64 PlaceOfServiceId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Place Of Service", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PlaceOfServiceId)))
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
                        BLObject<string> obj = BLLAdminCodesObj.DeletePlaceOfService(MDVUtility.ToStr(PlaceOfServiceId));
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
        /// Updates the place of service is active.
        /// </summary>
        /// <param name="PlaceOfServiceId">The place of service identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdatePlaceOfServiceIsActive(Int64 PlaceOfServiceId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Place Of Service", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSCodes dsCodes = null;
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadPlaceOfService(PlaceOfServiceId, null, null, null);
                    dsCodes = obj.Data;
                    if (dsCodes.Tables[dsCodes.PlaceOfService.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCodes.Tables[dsCodes.PlaceOfService.TableName].Rows[0];
                        dr[dsCodes.PlaceOfService.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSCodes> objPlaceOfService = BLLAdminCodesObj.UpdatePlaceOfService(dsCodes);
                        string successMsg;
                        if (objPlaceOfService.Data != null)
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
                                Message = objPlaceOfService.Message
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

        #region Place of Service Plan Info
        /// <summary>
        /// Saves the pos plan information.
        /// </summary>
        /// <param name="PlanId">The plan identifier.</param>
        /// <param name="POS">The pos.</param>
        /// <param name="PlaceOfServiceId">The place of service identifier.</param>
        /// <returns></returns>
        private string SavePOSPlanInfo(string fieldsJSON, string PlaceOfServiceID, string RowId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Place Of Service", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSCodes dsCodes = new DSCodes();
                    DSCodes.PlanOfPOSRow dr = dsCodes.PlanOfPOS.NewPlanOfPOSRow();

                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    long InPlanId = 0;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPlan" + RowId]))
                    {
                        InPlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPlan" + RowId]);
                    }

                    if (InPlanId > 0)
                    {
                        if (!string.IsNullOrEmpty(PlaceOfServiceID))
                            dr.PlaceOfServiceId = MDVUtility.ToInt64(PlaceOfServiceID);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPOS" + RowId]))
                            dr.Name = MDVUtility.ToStr(SearchedfieldsJSON["txtPOS" + RowId]);

                        dr.InsurancePlanId = InPlanId;
                        dr.IsActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        #region Database Insertion
                        dsCodes.PlanOfPOS.AddPlanOfPOSRow(dr);
                        BLObject<DSCodes> obj = BLLAdminCodesObj.InsertPOSPlanInfo(dsCodes);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                POSPlanId = dsCodes.Tables[dsCodes.PlanOfPOS.TableName].Rows[0][dsCodes.PlanOfPOS.PlanOfPOSIdColumn.ColumnName]
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
        /// Updates the pos plan information.
        /// </summary>
        /// <param name="PlanId">The plan identifier.</param>
        /// <param name="POS">The pos.</param>
        /// <param name="PlaceOfServiceId">The place of service identifier.</param>
        /// <param name="POSPlanId">The pos plan identifier.</param>
        /// <returns></returns>
        private string UpdatePOSPlanInfo(string fieldsJSON, Int64 POSPlanId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Place Of Service", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    BLObject<DSCodes> objCode;
                    DSCodes dsCode = new DSCodes();
                    objCode = BLLAdminCodesObj.LoadPOSPlanInfo(0, POSPlanId);
                    if (objCode.Data != null)
                    {
                        foreach (DSCodes.PlanOfPOSRow dr in objCode.Data.Tables[dsCode.PlanOfPOS.TableName].Rows)
                        {
                            long InPlanId = 0;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPlan" + POSPlanId]))
                            {
                                InPlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPlan" + POSPlanId]);
                            }
                            if (InPlanId > 0)
                            {
                                dr.InsurancePlanId = InPlanId;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPOS" + POSPlanId]))
                                dr.Name = MDVUtility.ToStr(SearchedfieldsJSON["txtPOS" + POSPlanId]);
                        }

                        dsCode = objCode.Data;

                        #region Database Update

                        BLObject<DSCodes> obj = BLLAdminCodesObj.UpdatePOSPlanInfo(dsCode);

                        if (dsCode.Tables[dsCode.PlanOfPOS.TableName].Rows.Count > 0)
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
        /// Loads the pos plan information.
        /// </summary>
        /// <param name="PlaceOfServiceId">The place of service identifier.</param>
        /// <returns></returns>
        private string LoadPOSPlanInfo(Int64 PlaceOfServiceId)
        {
            try
            {
                DSCodes dsCodes = null;
                BLObject<DSCodes> obj;
                if (PlaceOfServiceId > 0)
                {
                    obj = BLLAdminCodesObj.LoadPOSPlanInfo(PlaceOfServiceId, 0);

                    dsCodes = obj.Data;
                    var response = new
                    {
                        status = true,
                        POSPlanCount = dsCodes.Tables[dsCodes.PlanOfPOS.TableName].Rows.Count,
                        POSPlan_JSON = MDVUtility.JSON_DataTable(dsCodes.Tables[dsCodes.PlanOfPOS.TableName]),
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
        /// Fills the position plan information.
        /// </summary>
        /// <param name="POSPlanId">The position plan identifier.</param>
        /// <returns></returns>
        private string FillPOSPlanInfo(Int64 POSPlanId)
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
                    DSCodes dsCodes = null;
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadPOSPlanInfo(0, POSPlanId);
                    if (obj.Data != null)
                    {
                        dsCodes = obj.Data;
                        if (dsCodes.Tables[dsCodes.PlanOfPOS.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsCodes.Tables[dsCodes.PlanOfPOS.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "ddlInsurancePlan", MDVUtility.ToStr(dr[dsCodes.PlanOfPOS.InsurancePlanIdColumn.ColumnName])},
                            { "txtPOS", MDVUtility.ToStr(dr[dsCodes.PlanOfPOS.NameColumn.ColumnName])}
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
        /// Deletes the tos plan information.
        /// </summary>
        /// <param name="POSPlanId">The tos plan identifier.</param>
        /// <returns></returns>
        private string DeletePOSPlanInfo(Int64 POSPlanId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Place Of Service", "DELETE")).ToString();
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
                        BLObject<string> obj = BLLAdminCodesObj.DeletePOSPlanInfo(MDVUtility.ToStr(POSPlanId));
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
        /// Handle the Place Of Service Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_PLACE_OF_SERVICE":
                    {
                        string fieldsJSON = context.Request["PlaceOfServiceData"];
                        string strJSONData = SavePlaceOfService(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PLACE_OF_SERVICE":
                    {
                        string strProviderId = context.Request["PlaceOfServiceID"];
                        string strJSONData = FillPlaceOfService(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PLACE_OF_SERVICE":
                    {
                        string strProviderId = context.Request["PlaceOfServiceID"];
                        string strJSONData = DeletePlaceOfService(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PLACE_OF_SERVICE":
                    {
                        string fieldsJSON = context.Request["PlaceOfServiceData"];
                        int ProviderID = MDVUtility.ToInt(context.Request["PlaceOfServiceID"]);
                        string strJSONData = UpdatePlaceOfService(fieldsJSON, ProviderID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PLACE_OF_SERVICE_ACTIVE_INACTIVE":
                    {
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["PlaceOfServiceID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdatePlaceOfServiceIsActive(ProviderID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_POS_PLAN_INFO":
                    {
                        string PlaceOfServiceID = (context.Request["PlaceOfServiceID"]);
                        string RowId = context.Request["RowId"];
                        string fieldsJSON = context.Request["POSPlanData"];
                        string strJSONData = SavePOSPlanInfo(fieldsJSON, PlaceOfServiceID, RowId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_POS_PLAN_INFO":
                    {
                        string fieldsJSON = context.Request["POSPlanData"];
                        Int64 POSPlanId = MDVUtility.ToInt64(context.Request["POSPlanID"]);
                        string strJSONData = UpdatePOSPlanInfo(fieldsJSON, POSPlanId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_POS_PLAN_INFO":
                    {
                        Int64 PlaceOfServiceID = MDVUtility.ToInt64(context.Request["PlaceOfServiceID"]);
                        string strJSONData = LoadPOSPlanInfo(PlaceOfServiceID);

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