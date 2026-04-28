using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using System.Web.Script.Serialization;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Admin.FollowUp
{
    public class Admin_FollowUpRemittanceCode
    {
        private BLLAdminFollowUp BLLAdminFollowUpObj = null;
        public Admin_FollowUpRemittanceCode()
        {
            BLLAdminFollowUpObj = new BLLAdminFollowUp();
        }
        #region Singleton
        private static Admin_FollowUpRemittanceCode _obj = null;
        public static Admin_FollowUpRemittanceCode Instance()
        {
            if (_obj == null)
                _obj = new Admin_FollowUpRemittanceCode();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string LoadRemittanceCode(string fieldsJSON, Int32 RemittanceId, int PageNumber, int RowsPerPage)
        {
            //string isRejection = "";
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Remittance Code", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFollowUp dsRemittance = null;
                    BLObject<DSFollowUp> objReason;

                    //if (SearchedfieldsJSON["chkRejection"] == true)
                    //    isRejection = "1";
                    //if (SearchedfieldsJSON["chkRejection"] == false)
                    //    isRejection = "0";

                    if (SearchedfieldsJSON == null)
                        objReason = BLLAdminFollowUpObj.LoadRemittanceCode(RemittanceId, null, null, null, PageNumber, RowsPerPage);
                    else
                        objReason = BLLAdminFollowUpObj.LoadRemittanceCode(RemittanceId, SearchedfieldsJSON["txtCode"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlRejection"], PageNumber, RowsPerPage, SearchedfieldsJSON["chkIsActice"]);

                    dsRemittance = objReason.Data;
                    if (objReason.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            RemittanceCodeCount = dsRemittance.Tables[dsRemittance.RemittanceCode.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsRemittance.RemittanceCode.Rows.Count > 0) ? dsRemittance.RemittanceCode.Rows[0][dsRemittance.RemittanceCode.RecordCountColumn.ColumnName] : 0,
                            RemittanceCodeLoad_JSON = MDVUtility.JSON_DataTable(dsRemittance.Tables[dsRemittance.RemittanceCode.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            RemittanceCodeCount = 0,
                            Message = objReason.Message
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
        private string DeleteRemittanceCode(long RemittanceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Remittance Code", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(RemittanceId)))
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
                        BLObject<string> obj = BLLAdminFollowUpObj.DeleteRemittanceCode(MDVUtility.ToStr(RemittanceId));
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
        private string RemittanceCodeActiveInactive(Int32 RemittanceId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Remittance Code", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSFollowUp dsRemittance = new DSFollowUp();

                    BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadRemittanceCode(RemittanceId, null, null, null);
                    dsRemittance = obj.Data;
                    if (dsRemittance.Tables[dsRemittance.RemittanceCode.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsRemittance.Tables[dsRemittance.RemittanceCode.TableName].Rows[0];
                        dr[dsRemittance.RemittanceCode.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.UpdateRemittanceCode(dsRemittance);
                        string successMsg;
                        if (objType.Data != null)
                        {
                            if (IsActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                Message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objType.Message
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
        private string SaveRemittanceCode(string fieldsJson)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Remittance Code", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                    DSFollowUp dsRemittance = new DSFollowUp();
                    DSFollowUp.RemittanceCodeRow dr = dsRemittance.RemittanceCode.NewRemittanceCodeRow();


                    if (!string.IsNullOrEmpty(searchedfieldsJson["txtCode"]))
                        dr.Code = MDVUtility.ToStr(searchedfieldsJson["txtCode"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["txtDescription"]))
                        dr.Description = MDVUtility.ToStr(searchedfieldsJson["txtDescription"]);

                    dr.Rejection = MDVUtility.ToStr(searchedfieldsJson["chkRejection"]) == "True";

                    dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsRemittance.RemittanceCode.AddRemittanceCodeRow(dr);
                    BLObject<DSFollowUp> objType = BLLAdminFollowUpObj.InsertRemittanceCode(dsRemittance);
                    dsRemittance = objType.Data;
                    if (objType.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Save_Message,
                            RemittanceId = dsRemittance.Tables[dsRemittance.RemittanceCode.TableName].Rows[0][dsRemittance.RemittanceCode.RemittanceIdColumn.ColumnName]
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objType.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        private string FillRemittanceCode(Int32 RemittanceId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(RemittanceId)))
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
                    DSFollowUp dsRemittance = null;
                    BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadRemittanceCode(RemittanceId, null, null, null);
                    if (obj.Data != null)
                    {
                        dsRemittance = obj.Data;
                        if (dsRemittance.Tables[dsRemittance.RemittanceCode.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsRemittance.Tables[dsRemittance.RemittanceCode.TableName].Rows[0];

                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtCode", MDVUtility.ToStr(dr[dsRemittance.RemittanceCode.CodeColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsRemittance.RemittanceCode.DescriptionColumn.ColumnName])},
                            { "chkRejection", MDVUtility.ToStr(dr[dsRemittance.RemittanceCode.RejectionColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsRemittance.RemittanceCode.IsActiveColumn.ColumnName])},

                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                RemittanceCodeFill_JSON = js.Serialize(keyValues)
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UpdateRemittanceCode(string fieldsJson, Int32 RemittanceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Remittance Code", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSFollowUp dsRemittance = null;
                    BLObject<DSFollowUp> obj = null;
                    obj = BLLAdminFollowUpObj.LoadRemittanceCode(RemittanceId, null, null, null);
                    dsRemittance = obj.Data;
                    if (dsRemittance.Tables[dsRemittance.RemittanceCode.TableName].Rows.Count > 0)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                        foreach (DataRow dr in dsRemittance.Tables[dsRemittance.RemittanceCode.TableName].Rows)
                        {
                            if (searchedfieldsJson.ContainsKey("txtCode") && !string.IsNullOrEmpty(searchedfieldsJson["txtCode"]))
                                dr[dsRemittance.RemittanceCode.CodeColumn] = MDVUtility.ToStr(searchedfieldsJson["txtCode"]);
                            if (searchedfieldsJson.ContainsKey("txtDescription") && !string.IsNullOrEmpty(searchedfieldsJson["txtDescription"]))
                                dr[dsRemittance.RemittanceCode.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["txtDescription"]);
                            if (searchedfieldsJson.ContainsKey("chkRejection"))
                                dr[dsRemittance.RemittanceCode.RejectionColumn] = MDVUtility.ToStr(searchedfieldsJson["chkRejection"]) == "True" ? true : false;

                            dr[dsRemittance.RemittanceCode.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True" ? true : false;
                            //dr[dsRemittance.RemittanceCode.CreatedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr[dsRemittance.RemittanceCode.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            //dr[dsRemittance.RemittanceCode.CreatedOnColumn] = DateTime.Now;
                            dr[dsRemittance.RemittanceCode.ModifiedOnColumn] = DateTime.Now;
                        }
                        BLObject<DSFollowUp> objType = null;
                        objType = BLLAdminFollowUpObj.UpdateRemittanceCode(dsRemittance);

                        if (objType.Data != null)
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
                                Message = objType.Message
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
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_REMITTANCECODE":
                    {
                        string fieldsJSON = context.Request["RemittanceData"];
                        Int32 RemittanceId = MDVUtility.ToInt32(context.Request["RemittanceId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadRemittanceCode(fieldsJSON, RemittanceId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_REMITTANCECODE":
                    {
                        Int64 RemittanceId = MDVUtility.ToInt64(context.Request["RemittanceId"]);
                        string strJSONData = DeleteRemittanceCode(RemittanceId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_REMITTANCECODE_ACTIVE_INACTIVE":
                    {
                        Int32 RemittanceId = MDVUtility.ToInt32(context.Request["RemittanceId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = RemittanceCodeActiveInactive(RemittanceId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_REMITTANCECODE":
                    {
                        string fieldsJson = context.Request["RemittanceCodeData"];
                        string strJsonData = SaveRemittanceCode(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_REMITTANCECODE":
                    {
                        string fieldsJson = context.Request["RemittanceCodeData"];
                        Int32 RemittanceId = MDVUtility.ToInt32(context.Request["RemittanceId"]);
                        string strJsonData = UpdateRemittanceCode(fieldsJson, RemittanceId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "FILL_REMITTANCECODE":
                    {
                        string RemittanceId = context.Request["RemittanceId"];
                        string strJsonData = FillRemittanceCode(MDVUtility.ToInt32(RemittanceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }
        #endregion
    }
}