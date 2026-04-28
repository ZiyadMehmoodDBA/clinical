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
    public class Admin_DrugCodeCost_Detail
    {
        private BLLAdminCodes BLLAdminDrugCodeCostObj = null;
        public Admin_DrugCodeCost_Detail()
        {
            BLLAdminDrugCodeCostObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_DrugCodeCost_Detail _obj = null;
        public static Admin_DrugCodeCost_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_DrugCodeCost_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the specialty.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveDrugCodeCost(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("DrugCodeCost", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    DSCodes.CPTCodeCostRow dr = dsCodes.CPTCodeCost.NewCPTCodeCostRow();
                    dr.CPTCodeCostID = -1;
                    dr.CPTCode = SearchedfieldsJSON["txtCPTCode"];
                    dr.Cost = SearchedfieldsJSON["txtCost"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsCodes.CPTCodeCost.AddCPTCodeCostRow(dr);
                    BLObject<DSCodes> obj = BLLAdminDrugCodeCostObj.InsertDrugCodeCost(ref dsCodes);
                    dsCodes = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            CPTCodeCostID = dsCodes.Tables[dsCodes.CPTCodeCost.TableName].Rows[0][dsCodes.CPTCodeCost.CPTCodeCostIDColumn.ColumnName]

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
        /// Updates the specialty.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="SpecialtyId">The specialty identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateDrugCodeCost(string fieldsJSON, Int64 DrugCodeCostId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("DrugCodeCost", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCode = new DSCodes();
                    //DSProfile.SpecialtyRow dr = dsProfile.Specialty.NewSpecialtyRow();
                    BLObject<DSCodes> objLoad = BLLAdminDrugCodeCostObj.LoadDrugCodeCost(DrugCodeCostId, null, null, null);
                    dsCode = objLoad.Data;
                    foreach (DSCodes.CPTCodeCostRow dr in dsCode.Tables[dsCode.CPTCodeCost.TableName].Rows)
                    {
                        //dr.SpecialtyId = SpecialtyId;
                        dr.CPTCode = SearchedfieldsJSON["txtCPTCode"];
                        dr.Cost = SearchedfieldsJSON["txtCost"];

                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsProfile.Specialty.AddSpecialtyRow(dr);
                    //dsProfile.Specialty.AcceptChanges();

                    if (dsCode.Tables[dsCode.CPTCodeCost.TableName].Rows.Count > 0)
                    {
                        //dsProfile.Specialty.Rows[0].SetModified();
                        BLObject<DSCodes> obj = BLLAdminDrugCodeCostObj.UpdateDrugCodeCost(ref dsCode);
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
        /// Fills the specialty.
        /// </summary>
        /// <param name="ProviderId">The specialty identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillDrugCodeCost(Int64 CPTCodeCostID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("DrugCodeCost", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(CPTCodeCostID)))
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
                        DSCodes dsCode = null;
                        //dsProfile = DALSpecialty.Instance.LoadSpecialty(SpecialtyId, null, null);
                        BLObject<DSCodes> obj = BLLAdminDrugCodeCostObj.LoadDrugCodeCost(CPTCodeCostID, null, null, null);
                        if (obj.Data != null)
                        {
                            dsCode = obj.Data;
                            if (dsCode.Tables[dsCode.CPTCodeCost.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsCode.Tables[dsCode.CPTCodeCost.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtCPTCode", MDVUtility.ToStr(dr[dsCode.CPTCodeCost.CPTCodeColumn.ColumnName])},
                            { "txtCost", MDVUtility.ToStr(dr[dsCode.CPTCodeCost.CostColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsCode.CPTCodeCost.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    DrugCodeCostFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the specialty against provider Id.
        /// </summary>
        /// <param name="ProviderId">The specialty identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleterDrugCodeCost(Int64 strDrugCodeCostId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("DrugCodeCost", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(strDrugCodeCostId)))
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
                        BLObject<string> obj = BLLAdminDrugCodeCostObj.DeleteDrugCodeCost(MDVUtility.ToStr(strDrugCodeCostId));
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
        /// Updates the specialty is active.
        /// </summary>
        /// <param name="SpecialtyId">The specialty identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateDrugCodeCostIsActive(Int64 DrugCodeCostId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("DrugCodeCost", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSCodes dsCode = null;
                    BLObject<DSCodes> obj = BLLAdminDrugCodeCostObj.LoadDrugCodeCost(DrugCodeCostId, null, null, null);
                    dsCode = obj.Data;
                    if (dsCode.Tables[dsCode.CPTCodeCost.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCode.Tables[dsCode.CPTCodeCost.TableName].Rows[0];
                        dr[dsCode.CPTCodeCost.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSCodes> objDrugCodeCost = BLLAdminDrugCodeCostObj.UpdateDrugCodeCost(ref dsCode);
                        string successMsg;
                        if (objDrugCodeCost.Data != null)
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
                                Message = objDrugCodeCost.Message
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
        /// Handle the Specialty Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_DRUGCODECOST":
                    {
                        string fieldsJSON = context.Request["DrugCodeCostData"];
                        string strJSONData = SaveDrugCodeCost(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_DRUGCODECOST":
                    {
                        string strCPTCodeCostID = context.Request["CPTCodeCostID"];
                        string strJSONData = FillDrugCodeCost(MDVUtility.ToInt64(strCPTCodeCostID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_DRUGCODECOST":
                    {
                        string strDrugCodeCostId = context.Request["DrugCodeCostID"];
                        string strJSONData = DeleterDrugCodeCost(MDVUtility.ToInt64(strDrugCodeCostId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_DRUGCODECOST":
                    {
                        string fieldsJSON = context.Request["DrugCodeCostData"];
                        Int64 DrugCodeCostDataID = MDVUtility.ToInt64(context.Request["DrugCodeCostDataID"]);
                        string strJSONData = UpdateDrugCodeCost(fieldsJSON, DrugCodeCostDataID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_DRUGCODECOST_ACTIVE_INACTIVE":
                    {
                        Int64 DrugCodeCostID = MDVUtility.ToInt64(context.Request["DrugCodeCostID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateDrugCodeCostIsActive(DrugCodeCostID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}