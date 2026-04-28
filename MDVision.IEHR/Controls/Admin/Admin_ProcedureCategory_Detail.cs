
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
    public class Admin_ProcedureCategory_Detail
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_ProcedureCategory_Detail()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_ProcedureCategory_Detail _obj = null;
        public static Admin_ProcedureCategory_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_ProcedureCategory_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the procedure category.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveProcedureCategory(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Procedure Category", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsProcedureCategory = new DSCodes();
                    DSCodes.ProcedureCategoryRow dr = dsProcedureCategory.ProcedureCategory.NewProcedureCategoryRow();

                    dr.Name = SearchedfieldsJSON["txtName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsProcedureCategory.ProcedureCategory.AddProcedureCategoryRow(dr);
                    BLObject<DSCodes> obj = BLLAdminCodesObj.InsertProcedureCategory(ref dsProcedureCategory);
                    dsProcedureCategory = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ProcedureCategoryId = dsProcedureCategory.Tables[dsProcedureCategory.ProcedureCategory.TableName].Rows[0][dsProcedureCategory.ProcedureCategory.ProcCategoryIdColumn.ColumnName]
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
        /// Updates the procedure category.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProcedureCategoryId">The procedure category identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateProcedureCategory(string fieldsJSON, Int64 ProcedureCategoryId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Procedure Category", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsProcedureCategory = new DSCodes();
                    //DSCodes.ProcedureCategoryRow dr = dsProcedureCategory.ProcedureCategory.NewProcedureCategoryRow();
                    BLObject<DSCodes> objLoad = BLLAdminCodesObj.LoadProcedureCategory(ProcedureCategoryId, null, null, null);
                    dsProcedureCategory = objLoad.Data;
                    foreach (DSCodes.ProcedureCategoryRow dr in dsProcedureCategory.Tables[dsProcedureCategory.ProcedureCategory.TableName].Rows)
                    {
                        //dr.ProcCategoryId = ProcedureCategoryId;
                        dr.Name = SearchedfieldsJSON["txtName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsProcedureCategory.ProcedureCategory.AddProcedureCategoryRow(dr);
                    //dsProcedureCategory.ProcedureCategory.AcceptChanges();

                    if (dsProcedureCategory.Tables[dsProcedureCategory.ProcedureCategory.TableName].Rows.Count > 0)
                    {
                        //dsProcedureCategory.ProcedureCategory.Rows[0].SetModified();
                        BLObject<DSCodes> obj = BLLAdminCodesObj.UpdateProcedureCategory(ref dsProcedureCategory);
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
        /// Fills the procedure category.
        /// </summary>
        /// <param name="ProcedureCategoryId">The procedure category identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillProcedureCategory(Int64 ProcedureCategoryId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Procedure Category", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ProcedureCategoryId)))
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
                        DSCodes dsProcedureCategory = null;
                        BLObject<DSCodes> obj = BLLAdminCodesObj.LoadProcedureCategory(ProcedureCategoryId, null, null, null);
                        if (obj.Data != null)
                        {
                            dsProcedureCategory = obj.Data;
                            if (dsProcedureCategory.Tables[dsProcedureCategory.ProcedureCategory.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsProcedureCategory.Tables[dsProcedureCategory.ProcedureCategory.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "txtName", MDVUtility.ToStr(dr[dsProcedureCategory.ProcedureCategory.NameColumn.ColumnName])},
                          { "txtDescription", MDVUtility.ToStr(dr[dsProcedureCategory.ProcedureCategory.DescriptionColumn.ColumnName])},
                          { "ChkIsActive", MDVUtility.ToStr(dr[dsProcedureCategory.ProcedureCategory.IsActiveColumn.ColumnName])},
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    ProcedureCategoryFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the procedure category.
        /// </summary>
        /// <param name="ProcedureCategoryId">The procedure category identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteProcedureCategory(Int64 ProcedureCategoryId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Procedure Category", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ProcedureCategoryId)))
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
                        BLObject<string> obj = BLLAdminCodesObj.DeleteProcedureCategory(MDVUtility.ToStr(ProcedureCategoryId));
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
        /// Updates the procedure category is active.
        /// </summary>
        /// <param name="ProcCategoryId">The proc category identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateProcedureCategoryIsActive(Int64 ProcCategoryId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Procedure Category", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSCodes dsCode = null;
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadProcedureCategory(ProcCategoryId, null, null, null);
                    dsCode = obj.Data;
                    if (dsCode.Tables[dsCode.ProcedureCategory.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCode.Tables[dsCode.ProcedureCategory.TableName].Rows[0];
                        dr[dsCode.ProcedureCategory.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSCodes> objUser = BLLAdminCodesObj.UpdateProcedureCategory(ref dsCode);
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

        #region Service Command Handler
        /// <summary>
        /// Handle the Procedure Category Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_PROCEDURE_CATEGORY":
                    {
                        string fieldsJSON = context.Request["ProcedureCategoryData"];
                        string strJSONData = SaveProcedureCategory(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PROCEDURE_CATEGORY":
                    {
                        string strProcedureCategoryId = context.Request["ProcedureCategoryID"];
                        string strJSONData = FillProcedureCategory(MDVUtility.ToInt64(strProcedureCategoryId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PROCEDURE_CATEGORY":
                    {
                        string strProcedureCategoryId = context.Request["ProcedureCategoryID"];
                        string strJSONData = DeleteProcedureCategory(MDVUtility.ToInt64(strProcedureCategoryId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PROCEDURE_CATEGORY":
                    {
                        string fieldsJSON = context.Request["ProcedureCategoryData"];
                        Int64 ProcedureCategoryID = MDVUtility.ToInt64(context.Request["ProcedureCategoryID"]);
                        string strJSONData = UpdateProcedureCategory(fieldsJSON, ProcedureCategoryID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PROCEDURE_CATEGORY_ACTIVE_INACTIVE":
                    {
                        Int64 ProcedureCategoryID = MDVUtility.ToInt64(context.Request["ProcedureCategoryID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateProcedureCategoryIsActive(ProcedureCategoryID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}