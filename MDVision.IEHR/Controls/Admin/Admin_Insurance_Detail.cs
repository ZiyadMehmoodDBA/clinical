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
    public class Admin_Insurance_Detail
    {
        BLLAdminInsurance BLLAdminInsuranceObj = null;
        public Admin_Insurance_Detail()
        {
            BLLAdminInsuranceObj = new BLLAdminInsurance();
        }
        #region Singleton
        private static Admin_Insurance_Detail _obj = null;
        public static Admin_Insurance_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_Insurance_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveInsurance(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = new DSInsurance();
                    DSInsurance.InsuranceRow dr = dsInsurance.Insurance.NewInsuranceRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                    dr.WebSiteURL = SearchedfieldsJSON["txtWebsite"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                    #region Database Insertion
                    dsInsurance.Insurance.AddInsuranceRow(dr);
                    BLObject<DSInsurance> obj = BLLAdminInsuranceObj.InsertInsurance(ref dsInsurance);
                    dsInsurance = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            InsuranceId = dsInsurance.Tables[dsInsurance.Insurance.TableName].Rows[0][dsInsurance.Insurance.InsuranceIdColumn.ColumnName]
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
        /// Updates the insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="InsuranceId">The insurance identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateInsurance(string fieldsJSON, Int64 InsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = new DSInsurance();
                    //DSInsurance.InsuranceRow dr = dsInsurance.Insurance.NewInsuranceRow();
                    BLObject<DSInsurance> objLoad = BLLAdminInsuranceObj.LoadInsurance(InsuranceId, null, null, null, null);
                    dsInsurance = objLoad.Data;
                    foreach (DSInsurance.InsuranceRow dr in dsInsurance.Tables[dsInsurance.Insurance.TableName].Rows)
                    {
                        //dr.InsuranceId = InsuranceId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                        dr.WebSiteURL = SearchedfieldsJSON["txtWebsite"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    }

                    #region Database Updation
                    //dsInsurance.Insurance.AddInsuranceRow(dr);
                    //dsInsurance.Insurance.AcceptChanges();

                    if (dsInsurance.Tables[dsInsurance.Insurance.TableName].Rows.Count > 0)
                    {
                        //dsInsurance.Insurance.Rows[0].SetModified();
                        BLObject<DSInsurance> obj = BLLAdminInsuranceObj.UpdateInsurance(ref dsInsurance);
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
        /// Fills the insurance.
        /// </summary>
        /// <param name="InsuranceId">The insurance identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillInsurance(Int64 InsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(InsuranceId)))
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
                        BLObject<DSInsurance> obj = BLLAdminInsuranceObj.LoadInsurance(InsuranceId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsInsurance = obj.Data;
                            if (dsInsurance.Tables[dsInsurance.Insurance.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsInsurance.Tables[dsInsurance.Insurance.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsInsurance.Insurance.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsInsurance.Insurance.DescriptionColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsInsurance.Insurance.EmailAddressColumn.ColumnName])},
                            { "txtWebsite", MDVUtility.ToStr(dr[dsInsurance.Insurance.WebSiteURLColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsInsurance.Insurance.EntityIdColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsInsurance.Insurance.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    InsuranceFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the insurance.
        /// </summary>
        /// <param name="InsuranceId">The insurance identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteInsurance(Int64 InsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(InsuranceId)))
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
                        BLObject<string> obj = BLLAdminInsuranceObj.DeleteInsurance(MDVUtility.ToStr(InsuranceId));
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
        /// Updates the insurance is active.
        /// </summary>
        /// <param name="InsuranceId">The insurance identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateInsuranceIsActive(Int64 InsuranceId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSInsurance dsInsurance = null;
                    BLObject<DSInsurance> obj = BLLAdminInsuranceObj.LoadInsurance(InsuranceId, null, null, null, null);
                    dsInsurance = obj.Data;
                    if (dsInsurance.Tables[dsInsurance.Insurance.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsInsurance.Tables[dsInsurance.Insurance.TableName].Rows[0];
                        dr[dsInsurance.Insurance.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSInsurance> objInsurance = BLLAdminInsuranceObj.UpdateInsurance(ref dsInsurance);
                        string successMsg;
                        if (objInsurance.Data != null)
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
                                Message = objInsurance.Message
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
        /// Handle the Insurance Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_INSURANCE":
                    {
                        string fieldsJSON = context.Request["InsuranceData"];
                        string strJSONData = SaveInsurance(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_INSURANCE":
                    {
                        string strInsuranceId = context.Request["InsuranceID"];
                        string strJSONData = FillInsurance(MDVUtility.ToInt64(strInsuranceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_INSURANCE":
                    {
                        string strInsuranceId = context.Request["InsuranceID"];
                        string strJSONData = DeleteInsurance(MDVUtility.ToInt64(strInsuranceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_INSURANCE":
                    {
                        string fieldsJSON = context.Request["InsuranceData"];
                        Int64 InsuranceID = MDVUtility.ToInt64(context.Request["InsuranceID"]);
                        string strJSONData = UpdateInsurance(fieldsJSON, InsuranceID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_INSURANCE_ACTIVE_INACTIVE":
                    {
                        Int64 InsuranceID = MDVUtility.ToInt64(context.Request["InsuranceID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateInsuranceIsActive(InsuranceID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}