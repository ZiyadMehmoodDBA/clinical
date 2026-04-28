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
    public class Admin_ICD_Detail
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_ICD_Detail()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_ICD_Detail _obj = null;
        public static Admin_ICD_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_ICD_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the icd.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes Message or Exception message</returns>
        private string SaveICD(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ICD", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    DSCodes.ICDRow dr = dsCodes.ICD.NewICDRow();

                    dr.ShortName = ""; //SearchedfieldsJSON["txtShortName"];
                    dr.Description = SearchedfieldsJSON["txtICD9Description"];
                    dr.ICD9 = SearchedfieldsJSON["txtICD9Code"];

                    dr.ICD10Description = SearchedfieldsJSON["txtICD10Description"];
                    dr.ICD10 = SearchedfieldsJSON["txtICD10Code"];

                    dr.SNOMEDDescription = SearchedfieldsJSON["txtSnomedDescription"];
                    dr.SNOMEDId = SearchedfieldsJSON["txtSnomedCode"];

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfLexiCode"]))
                        dr.LexiCode = SearchedfieldsJSON["hfLexiCode"];

                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                    //dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                    dr.ActivelyUsed = MDVUtility.ToStr(SearchedfieldsJSON["chkActivelyUsed"]) == "True" ? true : false;
                    dr.Valid = MDVUtility.ToStr(SearchedfieldsJSON["chkValid"]) == "True" ? true : false;

                    dr.IsICD10 = true;//SearchedfieldsJSON["ddlICDType"] == "1" ? true : false;

                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsCodes.ICD.AddICDRow(dr);
                    BLObject<DSCodes> obj = BLLAdminCodesObj.InsertICD(ref dsCodes);
                    dsCodes = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ICDId = dsCodes.Tables[dsCodes.ICD.TableName].Rows[0][dsCodes.ICD.ICDIdColumn.ColumnName]
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
                    return JsonConvert.SerializeObject(response);
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
        /// Updates the icd.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ICDId">The icd identifier.</param>
        /// <returns>Json string containing Succes Message or Exception message</returns>
        private string UpdateICD(string fieldsJSON, Int64 ICDId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ICD", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = new DSCodes();
                    //DSCodes.ICDRow dr = dsCodes.ICD.NewICDRow();
                    BLObject<DSCodes> objLoad = BLLAdminCodesObj.LoadICD(ICDId, null, null, null, null, null, null, null, null, 1, 50000);
                    dsCodes = objLoad.Data;
                    foreach (DSCodes.ICDRow dr in dsCodes.Tables[dsCodes.ICD.TableName].Rows)
                    {
                        //dr.ICDId = ICDId;

                        dr.ShortName = "";
                        dr.Description = SearchedfieldsJSON["txtICD9Description"];
                        dr.ICD9 = SearchedfieldsJSON["txtICD9Code"];

                        dr.ICD10Description = SearchedfieldsJSON["txtICD10Description"];
                        dr.ICD10 = SearchedfieldsJSON["txtICD10Code"];

                        dr.SNOMEDDescription = SearchedfieldsJSON["txtSnomedDescription"];
                        dr.SNOMEDId = SearchedfieldsJSON["txtSnomedCode"];

                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfLexiCode"]))
                        //    dr.LexiCode = SearchedfieldsJSON["hfLexiCode"];

                        dr.ActivelyUsed = MDVUtility.ToStr(SearchedfieldsJSON["chkActivelyUsed"]) == "True" ? true : false;
                        dr.Valid = MDVUtility.ToStr(SearchedfieldsJSON["chkValid"]) == "True" ? true : false;
                        dr.IsActive = true;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsCodes.ICD.AddICDRow(dr);
                    //dsCodes.ICD.AcceptChanges();

                    if (dsCodes.Tables[dsCodes.ICD.TableName].Rows.Count > 0)
                    {
                        //dsCodes.ICD.Rows[0].SetModified();
                        BLObject<DSCodes> obj = BLLAdminCodesObj.UpdateICD(ref dsCodes);
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
                    return JsonConvert.SerializeObject(response);
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
        /// Fills the icd.
        /// </summary>
        /// <param name="ICDId">The icd identifier.</param>
        /// <returns>Json string containing Key Value Pair or Exception message</returns>
        private string FillICD(Int64 ICDId, Int64 pageNo, Int64 rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ICD", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ICDId)))
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
                        BLObject<DSCodes> obj = BLLAdminCodesObj.LoadICD(ICDId, null, null, null, null, null, null, null, null, pageNo, rpp);
                        if (obj.Data != null)
                        {
                            dsCodes = obj.Data;
                            if (dsCodes.Tables[dsCodes.ICD.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsCodes.Tables[dsCodes.ICD.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                           // { "txtShortName", MDVUtility.ToStr(dr[dsCodes.ICD.ShortNameColumn.ColumnName])},
                            { "txtICD9Description", MDVUtility.ToStr(dr[dsCodes.ICD.DescriptionColumn.ColumnName])},
                            { "txtICD10Description", MDVUtility.ToStr(dr[dsCodes.ICD.ICD10DescriptionColumn.ColumnName])},
                            { "txtSnomedDescription", MDVUtility.ToStr(dr[dsCodes.ICD.SNOMEDDescriptionColumn.ColumnName])},
                            { "chkActivelyUsed", MDVUtility.ToStr(dr[dsCodes.ICD.ActivelyUsedColumn.ColumnName])},
                            { "chkValid", MDVUtility.ToStr(dr[dsCodes.ICD.ValidColumn.ColumnName])},
                            { "txtICD9Code", MDVUtility.ToStr(dr[dsCodes.ICD.ICD9Column.ColumnName])},
                            { "txtICD10Code", MDVUtility.ToStr(dr[dsCodes.ICD.ICD10Column.ColumnName])},
                            { "txtSnomedCode", MDVUtility.ToStr(dr[dsCodes.ICD.SNOMEDIdColumn.ColumnName])},
                            //{ "ddlICDType" , MDVUtility.ToStr(dr[dsCodes.ICD.IsICD10Column.ColumnName]) == "True" ? "1" : "0" },
                            //{ "ddlEntity", MDVUtility.ToStr(dr[dsCodes.ICD.EntityIdColumn.ColumnName])},
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    ICDFill_JSON = js.Serialize(keyValues)
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
                    return JsonConvert.SerializeObject(response);
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
        /// Deletes the icd.
        /// </summary>
        /// <param name="ICDId">The icd identifier.</param>
        /// <returns>Json string containing Succes Message or Exception message</returns>
        private string DeleteICD(Int64 ICDId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ICD", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ICDId)))
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
                        BLObject<string> obj = BLLAdminCodesObj.DeleteICD(MDVUtility.ToStr(ICDId));
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
                    return JsonConvert.SerializeObject(response);
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
        /// Updates the icd is active.
        /// </summary>
        /// <param name="ICDId">The icd identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes Message or Exception message</returns>
        private string UpdateICDIsActive(Int64 ICDId, Int64 IsActive, Int64 pageNo, Int64 rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ICD", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSCodes dsCodes = null;
                    BLObject<DSCodes> obj = BLLAdminCodesObj.LoadICD(ICDId, null, null, null, null, null, null, null, null, pageNo, rpp);
                    dsCodes = obj.Data;
                    if (dsCodes.Tables[dsCodes.ICD.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCodes.Tables[dsCodes.ICD.TableName].Rows[0];
                        dr[dsCodes.ICD.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSCodes> objICD = BLLAdminCodesObj.UpdateICD(ref dsCodes);
                        string successMsg;
                        if (objICD.Data != null)
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
                                Message = objICD.Message
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
                    return JsonConvert.SerializeObject(response);
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
        /// Handle the ICD Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_ICD":
                    {
                        string fieldsJSON = context.Request["ICDData"];
                        string strJSONData = SaveICD(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_ICD":
                    {
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strICDId = context.Request["ICDID"];
                        string strJSONData = FillICD(MDVUtility.ToInt64(strICDId), pageNo, rpp);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_ICD":
                    {
                        string strICDId = context.Request["ICDID"];
                        string strJSONData = DeleteICD(MDVUtility.ToInt64(strICDId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_ICD":
                    {
                        string fieldsJSON = context.Request["ICDData"];
                        Int64 ICDID = MDVUtility.ToInt64(context.Request["ICDID"]);
                        string strJSONData = UpdateICD(fieldsJSON, ICDID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_ICD_ACTIVE_INACTIVE":
                    {
                        Int64 ICDID = MDVUtility.ToInt64(context.Request["ICDID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJSONData = UpdateICDIsActive(ICDID, IsActive, pageNo, rpp);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}