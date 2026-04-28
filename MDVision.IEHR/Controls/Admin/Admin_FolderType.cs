using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.IEHR.Common;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_FolderType
    {
        private BLLDocument BLLDocumentObj = null;
        public Admin_FolderType()
        {
        BLLDocumentObj=new BLLDocument();
        }

        #region Singleton
        private static Admin_FolderType _obj = null;
        public static Admin_FolderType Instance()
        {
            if (_obj == null)
                _obj = new Admin_FolderType();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Searches the type of the folder.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="FolderTypeID">The folder type identifier.</param>
        /// <returns></returns>
        private string SearchFolderType(string fieldsJSON, int DocTypeId, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("FolderType", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSDocument dsdocument = null;
                    BLObject<DSDocument> objDocumentType = null;
                    if (SearchedfieldsJSON == null)
                        objDocumentType = BLLDocumentObj.LoadDocumentType(DocTypeId, null, null, null, null);
                    else
                        objDocumentType = BLLDocumentObj.LoadDocumentType(DocTypeId, SearchedfieldsJSON["txtName"], SearchedfieldsJSON["txtDiscription"], SearchedfieldsJSON["chkIsActice"], SearchedfieldsJSON["ddlEntity"], PageNumber, RowsPerPage);

                    dsdocument = objDocumentType.Data;
                    if (objDocumentType.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            DocumentTypeCount = dsdocument.Tables[dsdocument.DocumentType.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsdocument.DocumentType.Rows.Count > 0) ? dsdocument.DocumentType.Rows[0][dsdocument.DocumentType.RecordCountColumn.ColumnName] : 0,
                            FolderTypeLoad_JSON = MDVUtility.JSON_DataTable(dsdocument.Tables[dsdocument.DocumentType.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentTypeCount = 0,
                            Message = objDocumentType.Message
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
        /// Saves the type of the folder.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="FolderTypeId">The folder type identifier.</param>
        /// <returns></returns>
        private string SaveFolderType(string fieldsJSON, int DocTypeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("FolderType", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    DSDocument dsDocument = new DSDocument();
                    DSDocument.DocumentTypeRow dr = dsDocument.DocumentType.NewDocumentTypeRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtName"]))
                        dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["txtName"]);
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCPTCode"]))
                    //    dr.CPTCodeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfcptcode"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtDescription"]))
                        dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    #region Database Insertion
                    dsDocument.DocumentType.AddDocumentTypeRow(dr);
                    BLObject<DSDocument> obj = BLLDocumentObj.InsertDocumentType(dsDocument);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            FolderTypeId = dsDocument.Tables[dsDocument.DocumentType.TableName].Rows[0][dsDocument.DocumentType.DoctypeIdColumn.ColumnName]
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
        /// Deletes the type of the folder.
        /// </summary>
        /// <param name="FolderTypeID">The folder type identifier.</param>
        /// <returns></returns>
        private string DeleteFolderType(string DocTypeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("FolderType", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(DocTypeId))
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
                        BLObject<string> obj = BLLDocumentObj.DeleteDocumentType(DocTypeId);
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
        /// Updates the folder type is active.
        /// </summary>
        /// <param name="FolderTypeID">The folder type identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateFolderTypeIsActive(int DocTypeId, long IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("FolderType", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSDocument dsDocument = null;
                    BLObject<DSDocument> obj = BLLDocumentObj.LoadDocumentType(DocTypeId, null, null, null, null);
                    dsDocument = obj.Data;
                    if (dsDocument.Tables[dsDocument.DocumentType.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsDocument.Tables[dsDocument.DocumentType.TableName].Rows[0];
                        dr[dsDocument.DocumentType.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSDocument> objFolderType = BLLDocumentObj.UpdateDocumentType(dsDocument);
                        string successMsg;
                        if (objFolderType.Data != null)
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
                                Message = objFolderType.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        string message = obj.Message;
                        if (message.Contains("UNIQUE KEY"))
                            message = "Cannot insert duplicate Folder Type.";
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
        /// Updates the type of the folder.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="FolderTypeID">The folder type identifier.</param>
        /// <returns></returns>
        private string UpdateFolderType(string fieldsJSON, int DocTypeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("FolderType", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSDocument dsDocument = new DSDocument();
                    //DSDocument.DocumentTypeRow dr = dsDocument.DocumentType.NewDocumentTypeRow();
                    BLObject<DSDocument> objDocumentTypeLoad = BLLDocumentObj.LoadDocumentType(DocTypeId, null, null, null, null);
                    dsDocument = objDocumentTypeLoad.Data;
                    foreach (DSDocument.DocumentTypeRow dr in dsDocument.Tables[dsDocument.DocumentType.TableName].Rows)
                    {
                        //dr.DoctypeId = DocTypeId;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtName"]))
                            dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["txtName"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtDescription"]))
                            dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsDocument.DocumentType.AddDocumentTypeRow(dr);
                    //dsDocument.DocumentType.AcceptChanges();

                    if (dsDocument.Tables[dsDocument.DocumentType.TableName].Rows.Count > 0)
                    {
                        //dsDocument.DocumentType.Rows[0].SetModified();
                        BLObject<DSDocument> objDocuumentType = BLLDocumentObj.UpdateDocumentType(dsDocument);
                        dsDocument = objDocuumentType.Data;
                        if (objDocuumentType.Data != null)
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
                                Message = objDocuumentType.Message
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
        /// Fills the type of the folder.
        /// </summary>
        /// <param name="FolderTypeID">The folder type identifier.</param>
        /// <returns></returns>
        private string FillFolderType(int DocTypeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("FolderType", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(DocTypeId)))
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
                        DSDocument dsDocument = null;
                        BLObject<DSDocument> objDocumentType = BLLDocumentObj.LoadDocumentType(DocTypeId, null, null, null, null);
                        if (objDocumentType.Data != null)
                        {
                            dsDocument = objDocumentType.Data;
                            if (dsDocument.Tables[dsDocument.DocumentType.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsDocument.Tables[dsDocument.DocumentType.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtName", MDVUtility.ToStr(dr[dsDocument.DocumentType.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsDocument.DocumentType.DescriptionColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsDocument.DocumentType.EntityIdColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsDocument.DocumentType.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    FolderTypeDetail_JSON = js.Serialize(keyValues)
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
                                Message = objDocumentType.Message
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
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_FOLDER_TYPE":
                    {
                        string fieldsJSON = context.Request["FolderTypeData"];
                        int DocTypeId = MDVUtility.ToInt(context.Request["DocTypeId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = SearchFolderType(fieldsJSON, DocTypeId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_FOLDER_TYPE":
                    {
                        string fieldsJSON = context.Request["FolderTypeData"];
                        int DocTypeId = MDVUtility.ToInt(context.Request["DocTypeId"]);
                        string strJSONData = SaveFolderType(fieldsJSON, DocTypeId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_FOLDER_TYPE":
                    {

                        string DocTypeId = MDVUtility.ToStr(context.Request["DocTypeId"]);
                        string strJSONData = DeleteFolderType(DocTypeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_FOLDER_TYPE_ACTIVE_INACTIVE":
                    {

                        int DocTypeId = MDVUtility.ToInt(context.Request["DocTypeId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateFolderTypeIsActive(DocTypeId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_FOLDER_TYPE":
                    {
                        string fieldsJSON = context.Request["FolderTypeData"];
                        int DocTypeId = MDVUtility.ToInt(context.Request["DocTypeId"]);
                        string strJSONData = UpdateFolderType(fieldsJSON, DocTypeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_FOLDER_TYPE":
                    {
                        int DocTypeId = MDVUtility.ToInt(context.Request["DocTypeId"]);
                        string strJSONData = FillFolderType(DocTypeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}