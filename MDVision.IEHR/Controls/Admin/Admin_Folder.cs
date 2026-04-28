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
    public class Admin_Folder
    {
         private BLLDocument BLLDocumentObj = null;
         public Admin_Folder()
        {
        BLLDocumentObj=new BLLDocument();
        }
        #region Singleton
        private static Admin_Folder _obj = null;
        public static Admin_Folder Instance()
        {
            if (_obj == null)
                _obj = new Admin_Folder();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string SearchDocument(string fieldsJSON, int DocumentId, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Folder", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    DSDocument dsDocument = null;
                    BLObject<DSDocument> objDocument = null;

                    if (SearchedfieldsJSON == null)
                        objDocument = BLLDocumentObj.LoadDocument(DocumentId, null, 0, null, null, null);
                    //objDocument = Admin_Folder.BusinessObj.LoadDocument(FolderId, null, 0, null);
                    else
                        objDocument = BLLDocumentObj.LoadDocument(DocumentId, SearchedfieldsJSON["txtName"], MDVUtility.ToInt32(SearchedfieldsJSON["ddlFolderType"]), SearchedfieldsJSON["chkIsActice"], null, SearchedfieldsJSON["txtDiscription"], PageNumber, RowsPerPage);
                    dsDocument = objDocument.Data;
                    if (objDocument.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = dsDocument.Tables[dsDocument.Documents.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsDocument.Documents.Rows.Count > 0) ? dsDocument.Documents.Rows[0][dsDocument.Documents.RecordCountColumn.ColumnName] : 0,
                            DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsDocument.Tables[dsDocument.Documents.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = objDocument.Message
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
        /// Saves the SaveUser.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        /// 
        private string SaveDocument(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Folder", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    DSDocument dsDocument = new DSDocument();
                    DSDocument.DocumentsRow dr = dsDocument.Documents.NewDocumentsRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtName"]))
                        dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["txtName"]);
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCPTCode"]))
                    //    dr.CPTCodeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfcptcode"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtDescription"]))
                        dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlType"]))
                        dr.DocTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlType"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtBarcodeValue"]))
                        dr.BarCodeValue = MDVUtility.ToStr(SearchedfieldsJSON["txtBarcodeValue"]);
                    //dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                    //    dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    #region Database Insertion
                    dsDocument.Documents.AddDocumentsRow(dr);
                    BLObject<DSDocument> obj = BLLDocumentObj.InsertDocument(dsDocument);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            DocumentId = dsDocument.Tables[dsDocument.Documents.TableName].Rows[0][dsDocument.Documents.DocIdColumn.ColumnName]
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
        /// Updates the User.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <returns></returns>
        private string UpdateDocument(string fieldsJSON, Int32 DocumentId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Folder", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSDocument dsDocument = new DSDocument();
                    //DSDocument.DocumentsRow dr = dsDocument.Documents.NewDocumentsRow();
                    BLObject<DSDocument> objDocumentLoad = BLLDocumentObj.LoadDocument(DocumentId, null, 0, null, null, null);
                    dsDocument = objDocumentLoad.Data;

                    foreach (DSDocument.DocumentsRow dr in dsDocument.Tables[dsDocument.Documents.TableName].Rows)
                    {
                        //dr.DocId = DocumentId;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtName"]))
                            dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["txtName"]);
                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCPTCode"]))
                        //    dr.CPTCodeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfcptcode"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtDescription"]))
                            dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlType"]))
                            dr.DocTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlType"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtBarcodeValue"]))
                            dr.BarCodeValue = MDVUtility.ToStr(SearchedfieldsJSON["txtBarcodeValue"]);
                        //dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        //    dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }


                    #region Database Updation
                    //dsDocument.Documents.AddDocumentsRow(dr);
                    //dsDocument.Documents.AcceptChanges();

                    if (dsDocument.Tables[dsDocument.Documents.TableName].Rows.Count > 0)
                    {
                        //dsDocument.Documents.Rows[0].SetModified();
                        BLObject<DSDocument> obj = BLLDocumentObj.UpdateDocument(dsDocument);
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Deletes Users by User Id.
        /// </summary>
        /// <param name="PracticeID">The practice identifier.</param>
        /// <returns></returns>
        private string DeleteDocument(Int64 DocumentID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Folder", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(DocumentID)))
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
                        BLObject<string> obj = BLLDocumentObj.DeleteDocument(MDVUtility.ToStr(DocumentID));
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
        /// Updates the user is active.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateDocumentIsActive(Int32 DocumentId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Folder", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSDocument dsDocument = null;
                    BLObject<DSDocument> obj = BLLDocumentObj.LoadDocument(DocumentId, "", 0, null, "", "");
                    dsDocument = obj.Data;
                    if (dsDocument.Tables[dsDocument.Documents.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsDocument.Tables[dsDocument.Documents.TableName].Rows[0];
                        dr[dsDocument.Documents.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSDocument> objUser = BLLDocumentObj.UpdateDocument(dsDocument);
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

        /// <summary>
        /// Get the Document against Document ID.
        /// </summary>
        /// <param name="PracticeID">The Document identifier.</param>
        /// <returns>Json string</returns>
        private string FillDocument(Int32 DocumentId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Folder", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(DocumentId)))
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
                        BLObject<DSDocument> objDocument = BLLDocumentObj.LoadDocument(DocumentId, null, 0, null, null, null);
                        if (objDocument.Data != null)
                        {
                            dsDocument = objDocument.Data;
                            if (dsDocument.Tables[dsDocument.Documents.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsDocument.Tables[dsDocument.Documents.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtName", MDVUtility.ToStr(dr[dsDocument.Documents.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsDocument.Documents.DescriptionColumn.ColumnName])},
                            { "ddlType", MDVUtility.ToStr(dr[dsDocument.Documents.DocTypeIdColumn.ColumnName])},
                            { "txtBarcodeValue", MDVUtility.ToStr(dr[dsDocument.Documents.BarCodeValueColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsDocument.Documents.IsActiveColumn.ColumnName])},
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    DocumentFill_JSON = js.Serialize(keyValues)
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
                                Message = objDocument.Message
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
                case "SEARCH_FOLDER":
                    {
                        string fieldsJSON = context.Request["FolderData"];
                        int DocumentId = MDVUtility.ToInt(context.Request["FolderId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = SearchDocument(fieldsJSON, DocumentId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_FOLDER":
                    {
                        string fieldsJSON = context.Request["FolderData"];
                        string strJSONData = SaveDocument(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_FOLDER":
                    {
                        string fieldsJSON = context.Request["FolderData"];
                        int DocumentId = MDVUtility.ToInt(context.Request["FolderId"]);
                        string strJSONData = UpdateDocument(fieldsJSON, DocumentId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_FOLDER_ACTIVE_INACTIVE":
                    {

                        int DocumentId = MDVUtility.ToInt(context.Request["FolderId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateDocumentIsActive(DocumentId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_FOLDER":
                    {

                        int DocumentId = MDVUtility.ToInt(context.Request["FolderId"]);
                        string strJSONData = DeleteDocument(DocumentId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_FOLDER":
                    {
                        int DocumentId = MDVUtility.ToInt(context.Request["FolderId"]);
                        string strJSONData = FillDocument(DocumentId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}